namespace Humanizer;

internal class TokenMapWordsToNumberConverter(TokenMapWordsToNumberRules rules) : GenderlessWordsToNumberConverter
{
    readonly TokenMapWordsToNumberRules rules = rules;
    readonly FrozenDictionary<string, int>? exactOrdinalMap = rules.ExactOrdinalMap;
    readonly FrozenDictionary<string, long>? ordinalScaleMap = rules.OrdinalScaleMap;
    readonly FrozenDictionary<string, long>? gluedOrdinalScaleSuffixes = rules.GluedOrdinalScaleSuffixes;

    public override int Convert(string words)
    {
        if (!TryConvert(words, out var parsedValue, out var unrecognizedWord))
        {
            throw new ArgumentException($"Unrecognized number word: {unrecognizedWord}");
        }

        return parsedValue;
    }

    public override bool TryConvert(string words, out int parsedValue) =>
        TryConvert(words, out parsedValue, out _);

    public override bool TryConvert(string words, out int parsedValue, out string? unrecognizedWord)
    {
        if (string.IsNullOrWhiteSpace(words))
        {
            throw new ArgumentException("Input words cannot be empty.");
        }

        if (rules.AllowInvariantIntegerInput &&
            int.TryParse(words.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out parsedValue))
        {
            unrecognizedWord = null;
            return true;
        }

        var normalized = TokenMapWordsToNumberNormalizer.Normalize(words, rules.NormalizationProfile);
        var negative = false;

        foreach (var negativePrefix in rules.NegativePrefixes)
        {
            if (!normalized.StartsWith(negativePrefix, StringComparison.Ordinal))
            {
                continue;
            }

            negative = true;
            normalized = normalized[negativePrefix.Length..].Trim();
            break;
        }

        foreach (var ordinalPrefix in rules.OrdinalPrefixes)
        {
            if (!normalized.StartsWith(ordinalPrefix, StringComparison.Ordinal))
            {
                continue;
            }

            normalized = normalized[ordinalPrefix.Length..].Trim();
            break;
        }

        if (!negative)
        {
            foreach (var negativeSuffix in rules.NegativeSuffixes)
            {
                if (!normalized.EndsWith(negativeSuffix, StringComparison.Ordinal))
                {
                    continue;
                }

                negative = true;
                normalized = normalized[..^negativeSuffix.Length].Trim();
                break;
            }
        }

        if (TryParseOrdinal(normalized, out parsedValue) ||
            TryParseCardinal(normalized, out parsedValue, out unrecognizedWord))
        {
            if (negative)
            {
                parsedValue = -parsedValue;
            }

            unrecognizedWord = null;
            return true;
        }

        parsedValue = default;
        return false;
    }

    bool TryParseOrdinal(string words, out int value)
    {
        if (TryParseOrdinalAbbreviation(words, out value))
        {
            return true;
        }

        return ordinalScaleMap is null
            ? TryParseExactOrdinal(words, out value)
            : TryParseExtendedOrdinal(words, ordinalScaleMap, out value);
    }

    bool TryParseExactOrdinal(string words, out int value)
    {
        if (exactOrdinalMap?.TryGetValue(words, out value) == true)
        {
            return true;
        }

        return TryParseGluedOrdinalScale(words, out value);
    }

    bool TryParseExtendedOrdinal(string words, FrozenDictionary<string, long> ordinalScaleMap, out int value)
    {
        if (exactOrdinalMap?.TryGetValue(words, out value) == true)
        {
            return true;
        }

        if (ordinalScaleMap.TryGetValue(words, out var scaleValue) &&
            scaleValue is <= int.MaxValue and >= int.MinValue)
        {
            value = (int)scaleValue;
            return true;
        }

        return TryParseGluedOrdinalScale(words, out value);
    }

    bool TryParseOrdinalAbbreviation(string words, out int value)
    {
        if (rules.OrdinalAbbreviationSuffixes.Length == 0)
        {
            value = default;
            return false;
        }

        var span = words.AsSpan();
        var digitLength = 0;

        while (digitLength < span.Length && span[digitLength] is >= '0' and <= '9')
        {
            digitLength++;
        }

        if (digitLength == 0 || digitLength == span.Length)
        {
            value = default;
            return false;
        }

        var suffix = span[digitLength..];
        foreach (var candidate in rules.OrdinalAbbreviationSuffixes)
        {
            if (suffix.Equals(candidate, StringComparison.Ordinal) &&
                int.TryParse(span[..digitLength], NumberStyles.None, CultureInfo.InvariantCulture, out value))
            {
                return true;
            }
        }

        value = default;
        return false;
    }

    bool TryParseCardinal(string words, out int value, out string? unrecognizedWord)
    {
        if (rules.CardinalMap.TryGetValue(words, out var directValue))
        {
            if (directValue is > int.MaxValue or < int.MinValue)
            {
                value = default;
                unrecognizedWord = words;
                return false;
            }

            value = (int)directValue;
            unrecognizedWord = null;
            return true;
        }

        long total = 0;
        long current = 0;
        unrecognizedWord = null;
        var tokenizer = WordsToNumberTokenizer.Enumerate(words).GetEnumerator();
        string? pendingToken = null;

        while (TryReadNextToken(ref tokenizer, ref pendingToken, out var token))
        {
            if (TryParseTerminalOrdinal(token, ref tokenizer, ref pendingToken, total, current, out value, out unrecognizedWord))
            {
                return true;
            }

            if (TryGetCompositeScaleValue(token, ref tokenizer, ref pendingToken, out var compositeScaleValue))
            {
                total += (current == 0 ? 1 : current) * compositeScaleValue;
                current = 0;
                continue;
            }

            if (!TryGetTokenValue(token, out var tokenValue))
            {
                value = default;
                unrecognizedWord = token;
                return false;
            }

            if (TryApplyLookaheadCompound(tokenValue, ref tokenizer, ref pendingToken, out var compoundValue))
            {
                current += compoundValue;
                continue;
            }

            if (tokenValue >= rules.ScaleThreshold)
            {
                total += (current == 0 ? 1 : current) * tokenValue;
                current = 0;
                continue;
            }

            if (ShouldMultiplyToken(token, tokenValue))
            {
                current = (current == 0 ? 1 : current) * tokenValue;
                continue;
            }

            current += tokenValue;
        }

        var parsedLong = total + current;
        if (parsedLong is > int.MaxValue or < int.MinValue)
        {
            value = default;
            unrecognizedWord = words;
            return false;
        }

        value = (int)parsedLong;
        return true;
    }

    bool TryParseTerminalOrdinal(
        string token,
        ref WordsToNumberTokenizer.Enumerator tokenizer,
        ref string? pendingToken,
        long total,
        long current,
        out int value,
        out string? unrecognizedWord)
    {
        if (!rules.AllowTerminalOrdinalToken)
        {
            value = default;
            unrecognizedWord = null;
            return false;
        }

        return ordinalScaleMap is null
            ? TryParseTerminalExactOrdinal(token, exactOrdinalMap, ref tokenizer, ref pendingToken, total, current, out value, out unrecognizedWord)
            : TryParseTerminalExtendedOrdinal(token, exactOrdinalMap, ordinalScaleMap, ref tokenizer, ref pendingToken, total, current, out value, out unrecognizedWord);
    }

    bool TryParseTerminalExactOrdinal(
        string token,
        FrozenDictionary<string, int>? exactOrdinalMap,
        ref WordsToNumberTokenizer.Enumerator tokenizer,
        ref string? pendingToken,
        long total,
        long current,
        out int value,
        out string? unrecognizedWord)
    {
        if (exactOrdinalMap?.TryGetValue(token, out var exactOrdinalValue) != true)
        {
            value = default;
            unrecognizedWord = null;
            return false;
        }

        if (TryReadNextToken(ref tokenizer, ref pendingToken, out var trailingToken))
        {
            pendingToken = trailingToken;
            value = default;
            unrecognizedWord = null;
            return false;
        }

        var parsedLong = total + current + exactOrdinalValue;
        if (parsedLong is > int.MaxValue or < int.MinValue)
        {
            value = default;
            unrecognizedWord = token;
            return false;
        }

        value = (int)parsedLong;
        unrecognizedWord = null;
        return true;
    }

    bool TryParseTerminalExtendedOrdinal(
        string token,
        FrozenDictionary<string, int>? exactOrdinalMap,
        FrozenDictionary<string, long> ordinalScaleMap,
        ref WordsToNumberTokenizer.Enumerator tokenizer,
        ref string? pendingToken,
        long total,
        long current,
        out int value,
        out string? unrecognizedWord)
    {
        var ordinalValue = default(int);
        var ordinalScaleValue = default(long);
        var isExactOrdinal = exactOrdinalMap?.TryGetValue(token, out ordinalValue) == true;
        var isOrdinalScale = ordinalScaleMap.TryGetValue(token, out ordinalScaleValue);
        if (!isExactOrdinal && !isOrdinalScale)
        {
            value = default;
            unrecognizedWord = null;
            return false;
        }

        if (TryReadNextToken(ref tokenizer, ref pendingToken, out var trailingToken))
        {
            pendingToken = trailingToken;
            value = default;
            unrecognizedWord = null;
            return false;
        }

        var parsedLong = isOrdinalScale
            ? total + ((current == 0 ? 1 : current) * ordinalScaleValue)
            : total + current + ordinalValue;

        if (parsedLong is > int.MaxValue or < int.MinValue)
        {
            value = default;
            unrecognizedWord = token;
            return false;
        }

        value = (int)parsedLong;
        unrecognizedWord = null;
        return true;
    }

    bool TryParseGluedOrdinalScale(string words, out int value)
    {
        if (gluedOrdinalScaleSuffixes is null || gluedOrdinalScaleSuffixes.Count == 0)
        {
            value = default;
            return false;
        }

        foreach (var suffix in gluedOrdinalScaleSuffixes)
        {
            if (!words.EndsWith(suffix.Key, StringComparison.Ordinal) || words.Length == suffix.Key.Length)
            {
                continue;
            }

            if (!TryParseCardinal(words[..^suffix.Key.Length], out value, out _))
            {
                continue;
            }

            var scaledValue = (long)value * suffix.Value;
            if (scaledValue is > int.MaxValue or < int.MinValue)
            {
                value = default;
                return false;
            }

            value = (int)scaledValue;
            return true;
        }

        value = default;
        return false;
    }

    bool ShouldIgnore(string token)
    {
        foreach (var ignoredToken in rules.IgnoredTokens)
        {
            if (token == ignoredToken)
            {
                return true;
            }
        }

        return false;
    }

    bool ShouldMultiplyToken(string token, long tokenValue)
    {
        if (rules.UseHundredMultiplier && tokenValue == 100)
        {
            return true;
        }

        foreach (var multiplierToken in rules.MultiplierTokens)
        {
            if (token == multiplierToken)
            {
                return true;
            }
        }

        return false;
    }

    bool TryGetCompositeScaleValue(
        string token,
        ref WordsToNumberTokenizer.Enumerator tokenizer,
        ref string? pendingToken,
        out long tokenValue)
    {
        if (rules.CompositeScaleMap is null ||
            rules.CompositeScaleMap.Count == 0 ||
            !TryReadNextToken(ref tokenizer, ref pendingToken, out var nextToken))
        {
            tokenValue = default;
            return false;
        }

        if (rules.CompositeScaleMap.TryGetValue(token + " " + nextToken, out tokenValue))
        {
            return true;
        }

        pendingToken = nextToken;
        tokenValue = default;
        return false;
    }

    bool TryApplyLookaheadCompound(long tokenValue, ref WordsToNumberTokenizer.Enumerator tokenizer, ref string? pendingToken, out long compoundValue)
    {
        if (!TryGetUnitTokenValue(tokenValue, out var unitValue) ||
            !TryReadNextToken(ref tokenizer, ref pendingToken, out var nextToken))
        {
            compoundValue = default;
            return false;
        }

        if (IsMatch(rules.TeenSuffixTokens, nextToken))
        {
            compoundValue = rules.TeenBaseValue + unitValue;
            return true;
        }

        if (unitValue >= rules.HundredSuffixMinValue &&
            unitValue <= rules.HundredSuffixMaxValue &&
            IsMatch(rules.HundredSuffixTokens, nextToken))
        {
            compoundValue = unitValue * rules.HundredSuffixValue;
            return true;
        }

        pendingToken = nextToken;
        compoundValue = default;
        return false;
    }

    bool TryReadNextToken(ref WordsToNumberTokenizer.Enumerator tokenizer, ref string? pendingToken, out string token)
    {
        while (WordsToNumberTokenizer.TryReadNext(ref tokenizer, ref pendingToken, out var rawToken))
        {
            token = NormalizeToken(rawToken);
            if (token.Length == 0 || ShouldIgnore(token))
            {
                continue;
            }

            return true;
        }

        token = string.Empty;
        return false;
    }

    string NormalizeToken(string token)
    {
        foreach (var prefix in rules.LeadingTokenPrefixesToTrim)
        {
            if (token.StartsWith(prefix, StringComparison.Ordinal))
            {
                return token[prefix.Length..];
            }
        }

        return token;
    }

    bool TryGetUnitTokenValue(long tokenValue, out long unitValue)
    {
        if (tokenValue >= rules.UnitTokenMinValue && tokenValue <= rules.UnitTokenMaxValue)
        {
            unitValue = tokenValue;
            return true;
        }

        unitValue = default;
        return false;
    }

    static bool IsMatch(string[] candidates, string token)
    {
        foreach (var candidate in candidates)
        {
            if (token == candidate)
            {
                return true;
            }
        }

        return false;
    }

    bool TryGetTokenValue(string token, out long tokenValue)
    {
        if (rules.CardinalMap.TryGetValue(token, out tokenValue))
        {
            return true;
        }

        foreach (var suffix in rules.TokenSuffixesToStrip)
        {
            if (!token.EndsWith(suffix, StringComparison.Ordinal) || token.Length == suffix.Length)
            {
                continue;
            }

            if (rules.CardinalMap.TryGetValue(token[..^suffix.Length], out tokenValue))
            {
                return true;
            }
        }

        tokenValue = default;
        return false;
    }
}

internal sealed class TokenMapWordsToNumberRules
{
    public required FrozenDictionary<string, long> CardinalMap { get; init; }
    public FrozenDictionary<string, int>? ExactOrdinalMap { get; init; }
    public FrozenDictionary<string, long>? OrdinalScaleMap { get; init; }
    public FrozenDictionary<string, long>? GluedOrdinalScaleSuffixes { get; init; }
    public FrozenDictionary<string, long>? CompositeScaleMap { get; init; }
    public TokenMapNormalizationProfile NormalizationProfile { get; init; }
    public string[] NegativePrefixes { get; init; } = [];
    public string[] NegativeSuffixes { get; init; } = [];
    public string[] OrdinalPrefixes { get; init; } = [];
    public string[] IgnoredTokens { get; init; } = [];
    public string[] LeadingTokenPrefixesToTrim { get; init; } = [];
    public string[] MultiplierTokens { get; init; } = [];
    public string[] TokenSuffixesToStrip { get; init; } = [];
    public string[] OrdinalAbbreviationSuffixes { get; init; } = [];
    public string[] TeenSuffixTokens { get; init; } = [];
    public string[] HundredSuffixTokens { get; init; } = [];
    public bool AllowTerminalOrdinalToken { get; init; }
    public bool UseHundredMultiplier { get; init; }
    public bool AllowInvariantIntegerInput { get; init; }
    public long TeenBaseValue { get; init; } = 10;
    public long HundredSuffixValue { get; init; } = 100;
    public long UnitTokenMinValue { get; init; } = 1;
    public long UnitTokenMaxValue { get; init; } = 9;
    public long HundredSuffixMinValue { get; init; } = long.MaxValue;
    public long HundredSuffixMaxValue { get; init; } = long.MinValue;
    public long ScaleThreshold { get; init; } = 1000;
}

internal enum TokenMapNormalizationProfile
{
    CollapseWhitespace,
    LowercaseRemovePeriods,
    LowercaseReplacePeriodsWithSpaces,
    LowercaseRemovePeriodsAndDiacritics,
    PunctuationToSpacesRemoveDiacritics,
    Persian
}

static class TokenMapWordsToNumberNormalizer
{
    public static string Normalize(string words, TokenMapNormalizationProfile profile)
    {
        return profile switch
        {
            TokenMapNormalizationProfile.CollapseWhitespace => NormalizeCollapseWhitespace(words),
            TokenMapNormalizationProfile.LowercaseRemovePeriods => NormalizeLowercaseRemovePeriods(words),
            TokenMapNormalizationProfile.LowercaseReplacePeriodsWithSpaces => NormalizeLowercaseReplacePeriodsWithSpaces(words),
            TokenMapNormalizationProfile.LowercaseRemovePeriodsAndDiacritics => NormalizeWithBuilder(RemoveDiacritics(words), profile),
            TokenMapNormalizationProfile.PunctuationToSpacesRemoveDiacritics => NormalizeWithBuilder(RemoveDiacritics(words), profile),
            TokenMapNormalizationProfile.Persian => NormalizeWithBuilder(NormalizePersian(words), profile),
            _ => throw new ArgumentOutOfRangeException(nameof(profile), profile, null)
        };
    }

    static string NormalizeCollapseWhitespace(string words)
    {
        var source = words.AsSpan().Trim();
        if (source.IsEmpty)
        {
            return string.Empty;
        }

        var previousWasSpace = false;
        foreach (var current in source)
        {
            if (char.IsWhiteSpace(current))
            {
                if (previousWasSpace)
                {
                    return NormalizeWithBuilder(words, TokenMapNormalizationProfile.CollapseWhitespace);
                }

                previousWasSpace = true;
                continue;
            }

            previousWasSpace = false;
        }

        return source.Length == words.Length ? words : source.ToString();
    }

    static string NormalizeLowercaseRemovePeriods(string words)
    {
        var source = words.AsSpan().Trim();
        if (source.IsEmpty)
        {
            return string.Empty;
        }

        var needsNormalization = false;
        var previousWasSpace = false;

        foreach (var current in source)
        {
            if (current is ',' or '.')
            {
                needsNormalization = true;
                break;
            }

            if (current == '-')
            {
                needsNormalization = true;
                break;
            }

            if (char.IsWhiteSpace(current))
            {
                if (current != ' ' || previousWasSpace)
                {
                    needsNormalization = true;
                    break;
                }

                previousWasSpace = true;
                continue;
            }

            if (char.IsUpper(current))
            {
                needsNormalization = true;
                break;
            }

            previousWasSpace = false;
        }

        if (!needsNormalization)
        {
            return source.Length == words.Length ? words : source.ToString();
        }

        return NormalizeWithBuilder(words, TokenMapNormalizationProfile.LowercaseRemovePeriods);
    }

    static string NormalizeLowercaseReplacePeriodsWithSpaces(string words)
    {
        var source = words.AsSpan().Trim();
        if (source.IsEmpty)
        {
            return string.Empty;
        }

        var needsNormalization = false;
        var previousWasSpace = false;

        foreach (var current in source)
        {
            if (current == ',')
            {
                needsNormalization = true;
                break;
            }

            if (current is '.' or '-')
            {
                needsNormalization = true;
                break;
            }

            if (char.IsWhiteSpace(current))
            {
                if (current != ' ' || previousWasSpace)
                {
                    needsNormalization = true;
                    break;
                }

                previousWasSpace = true;
                continue;
            }

            if (char.IsUpper(current))
            {
                needsNormalization = true;
                break;
            }

            previousWasSpace = false;
        }

        if (!needsNormalization)
        {
            return source.Length == words.Length ? words : source.ToString();
        }

        return NormalizeWithBuilder(words, TokenMapNormalizationProfile.LowercaseReplacePeriodsWithSpaces);
    }

    static string NormalizeWithBuilder(string words, TokenMapNormalizationProfile profile)
    {
        var source = words.AsSpan().Trim();
        if (source.IsEmpty)
        {
            return string.Empty;
        }

        var builder = new StringBuilder(source.Length);
        var previousWasSpace = false;

        foreach (var sourceChar in source)
        {
            var current = sourceChar;
            switch (profile)
            {
                case TokenMapNormalizationProfile.CollapseWhitespace:
                    break;
                case TokenMapNormalizationProfile.LowercaseRemovePeriods:
                    if (current == ',' || current == '.')
                    {
                        continue;
                    }

                    if (current == '-')
                    {
                        current = ' ';
                    }

                    current = char.ToLowerInvariant(current);
                    break;
                case TokenMapNormalizationProfile.LowercaseReplacePeriodsWithSpaces:
                    if (current == ',')
                    {
                        continue;
                    }

                    if (current is '.' or '-')
                    {
                        current = ' ';
                    }

                    current = char.ToLowerInvariant(current);
                    break;
                case TokenMapNormalizationProfile.LowercaseRemovePeriodsAndDiacritics:
                    if (current == ',' || current == '.')
                    {
                        continue;
                    }

                    if (current == '-')
                    {
                        current = ' ';
                    }

                    current = char.ToLowerInvariant(current);
                    break;
                case TokenMapNormalizationProfile.PunctuationToSpacesRemoveDiacritics:
                    switch (current)
                    {
                        case ',':
                        case '.':
                        case ':':
                        case ';':
                        case '"':
                        case '\'':
                        case '״':
                        case '׳':
                        case '-':
                        case '־':
                        case '/':
                        case '\\':
                        case '(':
                        case ')':
                        case '[':
                        case ']':
                        case '{':
                        case '}':
                            current = ' ';
                            break;
                    }
                    break;
                case TokenMapNormalizationProfile.Persian:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(profile), profile, null);
            }

            if (char.IsWhiteSpace(current))
            {
                if (previousWasSpace || builder.Length == 0)
                {
                    continue;
                }

                builder.Append(' ');
                previousWasSpace = true;
                continue;
            }

            builder.Append(current);
            previousWasSpace = false;
        }

        if (previousWasSpace)
        {
            builder.Length--;
        }

        return builder.ToString();
    }

    static string NormalizePersian(string words) =>
        words.Replace(",", string.Empty)
             .Replace(".", string.Empty)
             .Replace("،", string.Empty)
             .Replace("\u200c", " ")
             .Replace('ي', 'ی')
             .Replace('ك', 'ک');

    internal static string RemoveDiacritics(string text)
    {
        var normalized = text.Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder(text.Length);

        foreach (var character in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(character) != UnicodeCategory.NonSpacingMark)
            {
                builder.Append(character);
            }
        }

        return builder.ToString().Normalize(NormalizationForm.FormC);
    }
}

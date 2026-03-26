namespace Humanizer;

internal class TokenMapWordsToNumberConverter(TokenMapWordsToNumberRules rules) : GenderlessWordsToNumberConverter
{
    readonly TokenMapWordsToNumberRules rules = rules;

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

        return rules.OrdinalMap?.TryGetValue(words, out value) == true;
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

        foreach (var tokenSpan in WordsToNumberTokenizer.Enumerate(words))
        {
            var token = tokenSpan.ToString();

            if (ShouldIgnore(token))
            {
                continue;
            }

            if (!TryGetTokenValue(token, out var tokenValue))
            {
                value = default;
                unrecognizedWord = token;
                return false;
            }

            if (tokenValue >= rules.ScaleThreshold)
            {
                total += (current == 0 ? 1 : current) * tokenValue;
                current = 0;
                continue;
            }

            if (rules.UseHundredMultiplier && tokenValue == 100)
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
    public FrozenDictionary<string, int>? OrdinalMap { get; init; }
    public TokenMapNormalizationProfile NormalizationProfile { get; init; }
    public string[] NegativePrefixes { get; init; } = [];
    public string[] IgnoredTokens { get; init; } = [];
    public string[] TokenSuffixesToStrip { get; init; } = [];
    public string[] OrdinalAbbreviationSuffixes { get; init; } = [];
    public bool UseHundredMultiplier { get; init; }
    public bool AllowInvariantIntegerInput { get; init; }
    public long ScaleThreshold { get; init; } = 1000;
}

internal enum TokenMapNormalizationProfile
{
    CollapseWhitespace,
    LowercaseRemovePeriods,
    LowercaseReplacePeriodsWithSpaces
}

static class TokenMapWordsToNumberNormalizer
{
    public static string Normalize(string words, TokenMapNormalizationProfile profile)
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
}

namespace Humanizer;

internal class InvertedTensWordsToNumberConverter(InvertedTensWordsToNumberProfile profile) : GenderlessWordsToNumberConverter
{
    readonly InvertedTensWordsToNumberProfile profile = profile;

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

        var normalized = Normalize(words);
        var negative = false;

        foreach (var negativePrefix in profile.NegativePrefixes)
        {
            if (!normalized.StartsWith(negativePrefix, StringComparison.Ordinal))
            {
                continue;
            }

            negative = true;
            normalized = normalized[negativePrefix.Length..].Trim();
            break;
        }

        if (profile.AllowInvariantIntegerInput &&
            int.TryParse(normalized, NumberStyles.Integer, CultureInfo.InvariantCulture, out parsedValue))
        {
            unrecognizedWord = null;
            if (negative)
            {
                parsedValue = -parsedValue;
            }

            return true;
        }

        if (profile.OrdinalMap.TryGetValue(normalized, out parsedValue) ||
            TryParsePhrase(normalized, out parsedValue, out unrecognizedWord))
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

    bool TryParsePhrase(string words, out int value, out string? unrecognizedWord)
    {
        if (!words.Contains(' '))
        {
            return TryParseCompact(words, out value, out unrecognizedWord);
        }

        var total = 0;
        var current = 0;
        unrecognizedWord = null;

        foreach (var tokenSpan in WordsToNumberTokenizer.Enumerate(words))
        {
            var token = tokenSpan.ToString();

            if (ShouldIgnore(token))
            {
                continue;
            }

            if (!TryParseCompact(token, out var tokenValue, out unrecognizedWord))
            {
                value = default;
                return false;
            }

            if (tokenValue >= 1000)
            {
                total += (current == 0 ? 1 : current) * tokenValue;
                current = 0;
            }
            else if (tokenValue == 100)
            {
                current = (current == 0 ? 1 : current) * tokenValue;
            }
            else
            {
                current += tokenValue;
            }
        }

        value = total + current;
        return true;
    }

    bool TryParseCompact(string word, out int value, out string? unrecognizedWord)
    {
        if (profile.CardinalMap.TryGetValue(word, out value) ||
            profile.OrdinalMap.TryGetValue(word, out value))
        {
            unrecognizedWord = null;
            return true;
        }

        if (TryParseOrdinalStem(word, out value, out unrecognizedWord))
        {
            return true;
        }

        foreach (var scale in profile.ScaleTokens)
        {
            var index = word.IndexOf(scale, StringComparison.Ordinal);
            if (index < 0)
            {
                continue;
            }

            var left = word[..index];
            var right = StripLeadingIgnoredTokens(word[(index + scale.Length)..].Trim());
            var factor = 1;

            if (!string.IsNullOrEmpty(left) &&
                !TryParseCompact(left, out factor, out _) ||
                !TryParseOptional(right, out var remainder, out _))
            {
                continue;
            }

            value = factor * profile.CardinalMap[scale] + remainder;
            unrecognizedWord = null;
            return true;
        }

        if (TryParseCompoundTens(word, out value))
        {
            unrecognizedWord = null;
            return true;
        }

        if (word.Contains(' '))
        {
            var collapsed = word.Replace(" ", string.Empty);
            if (!ReferenceEquals(word, collapsed) &&
                TryParseCompact(collapsed, out value, out unrecognizedWord))
            {
                return true;
            }
        }

        value = default;
        unrecognizedWord = word;
        return false;
    }

    bool TryParseOrdinalStem(string word, out int value, out string? unrecognizedWord)
    {
        foreach (var suffix in profile.OrdinalSuffixes)
        {
            if (!word.EndsWith(suffix, StringComparison.Ordinal) || word.Length == suffix.Length)
            {
                continue;
            }

            return TryParseCompact(word[..^suffix.Length], out value, out unrecognizedWord);
        }

        value = default;
        unrecognizedWord = null;
        return false;
    }

    bool TryParseOptional(string word, out int value, out string? unrecognizedWord)
    {
        if (string.IsNullOrEmpty(word))
        {
            value = 0;
            unrecognizedWord = null;
            return true;
        }

        return TryParseCompact(word, out value, out unrecognizedWord);
    }

    bool TryParseCompoundTens(string word, out int value)
    {
        foreach (var tensToken in profile.TensTokens)
        {
            if (!word.EndsWith(tensToken.Word, StringComparison.Ordinal))
            {
                continue;
            }

            var prefix = word[..^tensToken.Word.Length];
            if (!prefix.EndsWith(profile.TensLinker, StringComparison.Ordinal))
            {
                continue;
            }

            var unitPart = ApplyUnitPartReplacements(prefix[..^profile.TensLinker.Length]);
            if (profile.UnitMap.TryGetValue(unitPart, out var unitValue) && unitValue is >= 1 and <= 9)
            {
                value = tensToken.Value + unitValue;
                return true;
            }
        }

        value = default;
        return false;
    }

    string StripLeadingIgnoredTokens(string words)
    {
        if (string.IsNullOrEmpty(words))
        {
            return words;
        }

        var remaining = words;
        var stripped = true;

        while (stripped)
        {
            stripped = false;

            foreach (var ignoredToken in profile.IgnoredTokens)
            {
                var ignoredTokenWithSpace = ignoredToken + " ";
                if (remaining.StartsWith(ignoredTokenWithSpace, StringComparison.Ordinal))
                {
                    remaining = remaining[ignoredTokenWithSpace.Length..].TrimStart();
                    stripped = true;
                    break;
                }

                if (remaining.StartsWith(ignoredToken, StringComparison.Ordinal))
                {
                    remaining = remaining[ignoredToken.Length..].TrimStart();
                    stripped = true;
                    break;
                }
            }
        }

        return remaining;
    }

    string ApplyUnitPartReplacements(string word)
    {
        var normalized = word;

        foreach (var replacement in profile.UnitPartReplacements)
        {
            normalized = normalized.Replace(replacement.OldValue, replacement.NewValue);
        }

        return normalized;
    }

    bool ShouldIgnore(string token)
    {
        foreach (var ignoredToken in profile.IgnoredTokens)
        {
            if (token == ignoredToken)
            {
                return true;
            }
        }

        return false;
    }

    static string Normalize(string words) =>
        TokenMapWordsToNumberNormalizer.Normalize(words, TokenMapNormalizationProfile.LowercaseRemovePeriods);

    internal static FrozenDictionary<string, int> BuildOrdinalMap(INumberToWordsConverter converter)
    {
        var ordinals = new Dictionary<string, int>(StringComparer.Ordinal);

        for (var number = 1; number <= 200; number++)
        {
            ordinals[Normalize(converter.ConvertToOrdinal(number))] = number;
        }

        return ordinals.ToFrozenDictionary(StringComparer.Ordinal);
    }
}

internal sealed class InvertedTensWordsToNumberProfile(
    FrozenDictionary<string, int> cardinalMap,
    FrozenDictionary<string, int> unitMap,
    InvertedTensToken[] tensTokens,
    string tensLinker,
    string[] scaleTokens,
    FrozenDictionary<string, int> ordinalMap,
    string[] negativePrefixes,
    string[] ignoredTokens,
    string[] ordinalSuffixes,
    StringReplacement[] unitPartReplacements,
    bool allowInvariantIntegerInput = false)
{
    public FrozenDictionary<string, int> CardinalMap { get; } = cardinalMap;
    public FrozenDictionary<string, int> UnitMap { get; } = unitMap;
    public InvertedTensToken[] TensTokens { get; } = tensTokens;
    public string TensLinker { get; } = tensLinker;
    public string[] ScaleTokens { get; } = scaleTokens;
    public FrozenDictionary<string, int> OrdinalMap { get; } = ordinalMap;
    public string[] NegativePrefixes { get; } = negativePrefixes;
    public string[] IgnoredTokens { get; } = ignoredTokens;
    public string[] OrdinalSuffixes { get; } = ordinalSuffixes;
    public StringReplacement[] UnitPartReplacements { get; } = unitPartReplacements;
    public bool AllowInvariantIntegerInput { get; } = allowInvariantIntegerInput;
}

internal readonly record struct InvertedTensToken(string Word, int Value);

internal readonly record struct StringReplacement(string OldValue, string NewValue);

namespace Humanizer;

internal class LinkingAffixWordsToNumberConverter(LinkingAffixWordsToNumberProfile profile) : GenderlessWordsToNumberConverter
{
    readonly LinkingAffixWordsToNumberProfile profile = profile;

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

        var normalized = TokenMapWordsToNumberNormalizer.Normalize(
            words.Replace("'", string.Empty).Replace("’", string.Empty),
            TokenMapNormalizationProfile.LowercaseReplacePeriodsWithSpaces);
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

        if (TryParseCardinal(normalized, out parsedValue))
        {
            if (negative)
            {
                parsedValue = -parsedValue;
            }

            unrecognizedWord = null;
            return true;
        }

        unrecognizedWord = WordsToNumberTokenizer.GetLastTokenOrSelf(normalized);
        parsedValue = default;
        return false;
    }

    bool TryParseCardinal(string words, out int value)
    {
        if (profile.CardinalMap.TryGetValue(words, out value))
        {
            return true;
        }

        var total = 0;
        var current = 0;

        foreach (var tokenSpan in WordsToNumberTokenizer.Enumerate(words))
        {
            var token = tokenSpan.ToString();
            if (ShouldIgnore(token))
            {
                continue;
            }

            if (token.StartsWith(profile.TeenPrefix, StringComparison.Ordinal) &&
                token.Length > profile.TeenPrefix.Length &&
                TryParseCardinal(token[profile.TeenPrefix.Length..], out var teenUnit))
            {
                current += profile.TeenBaseValue + teenUnit;
                continue;
            }

            if (TryParseLinkedToken(token, out var linkedValue))
            {
                current += linkedValue;
                continue;
            }

            if (!profile.CardinalMap.TryGetValue(token, out var tokenValue))
            {
                value = default;
                return false;
            }

            if (tokenValue >= profile.ScaleThreshold)
            {
                total += (current == 0 ? 1 : current) * tokenValue;
                current = 0;
            }
            else if (tokenValue == profile.HundredValue)
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

    bool TryParseLinkedToken(string token, out int value)
    {
        foreach (var suffix in profile.LinkedSuffixes)
        {
            if (!token.EndsWith(suffix, StringComparison.Ordinal) || token.Length == suffix.Length)
            {
                continue;
            }

            if (profile.CardinalMap.TryGetValue(token[..^suffix.Length], out value))
            {
                return true;
            }
        }

        value = default;
        return false;
    }
}

sealed class LinkingAffixWordsToNumberProfile(
    FrozenDictionary<string, int> cardinalMap,
    string teenPrefix,
    int teenBaseValue,
    string[] linkedSuffixes,
    string[] ignoredTokens,
    string[] negativePrefixes,
    int hundredValue = 100,
    int scaleThreshold = 1000)
{
    public FrozenDictionary<string, int> CardinalMap { get; } = cardinalMap;
    public string TeenPrefix { get; } = teenPrefix;
    public int TeenBaseValue { get; } = teenBaseValue;
    public string[] LinkedSuffixes { get; } = linkedSuffixes;
    public string[] IgnoredTokens { get; } = ignoredTokens;
    public string[] NegativePrefixes { get; } = negativePrefixes;
    public int HundredValue { get; } = hundredValue;
    public int ScaleThreshold { get; } = scaleThreshold;
}

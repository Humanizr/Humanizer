namespace Humanizer;

internal class VigesimalCompoundWordsToNumberConverter(VigesimalCompoundWordsToNumberProfile profile) : GenderlessWordsToNumberConverter
{
    readonly VigesimalCompoundWordsToNumberProfile profile = profile;

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

        var normalized = TokenMapWordsToNumberNormalizer.Normalize(words, TokenMapNormalizationProfile.LowercaseReplacePeriodsWithSpaces);
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

        if (profile.OrdinalMap.TryGetValue(normalized, out parsedValue) ||
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

    bool TryParseCardinal(string words, out int value, out string? unrecognizedWord)
    {
        var total = 0;
        var current = 0;
        unrecognizedWord = null;
        var tokenizer = WordsToNumberTokenizer.Enumerate(words).GetEnumerator();
        string? pendingToken = null;

        while (WordsToNumberTokenizer.TryReadNext(ref tokenizer, ref pendingToken, out var token))
        {
            if (ShouldIgnore(token))
            {
                continue;
            }

            if (token == profile.VigesimalLeadingToken &&
                WordsToNumberTokenizer.TryReadNext(ref tokenizer, ref pendingToken, out var vigesimalFollower))
            {
                if (IsVigesimalFollower(vigesimalFollower))
                {
                    current += profile.VigesimalValue;
                    continue;
                }

                pendingToken = vigesimalFollower;
            }

            if (token == profile.TeenLeaderToken &&
                profile.TeenLeaderBases.Contains(current) &&
                WordsToNumberTokenizer.TryReadNext(ref tokenizer, ref pendingToken, out var teenToken))
            {
                if (profile.CardinalMap.TryGetValue(teenToken, out var teenPart) && teenPart is >= 1 and <= 9)
                {
                    current += profile.TeenBaseValue + teenPart;
                    continue;
                }

                pendingToken = teenToken;
            }

            if (!profile.CardinalMap.TryGetValue(token, out var numeric))
            {
                value = default;
                unrecognizedWord = token;
                return false;
            }

            if (numeric == profile.HundredValue)
            {
                current = (current == 0 ? 1 : current) * numeric;
            }
            else if (numeric >= profile.ScaleThreshold)
            {
                total += (current == 0 ? 1 : current) * numeric;
                current = 0;
            }
            else
            {
                current += numeric;
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

    bool IsVigesimalFollower(string token)
    {
        foreach (var follower in profile.VigesimalFollowerTokens)
        {
            if (token == follower)
            {
                return true;
            }
        }

        return false;
    }
}

sealed class VigesimalCompoundWordsToNumberProfile(
    FrozenDictionary<string, int> cardinalMap,
    FrozenDictionary<string, int> ordinalMap,
    string[] negativePrefixes,
    string[] ignoredTokens,
    string vigesimalLeadingToken,
    string[] vigesimalFollowerTokens,
    int vigesimalValue,
    string teenLeaderToken,
    FrozenSet<int> teenLeaderBases,
    int teenBaseValue = 10,
    int hundredValue = 100,
    int scaleThreshold = 1000)
{
    public FrozenDictionary<string, int> CardinalMap { get; } = cardinalMap;
    public FrozenDictionary<string, int> OrdinalMap { get; } = ordinalMap;
    public string[] NegativePrefixes { get; } = negativePrefixes;
    public string[] IgnoredTokens { get; } = ignoredTokens;
    public string VigesimalLeadingToken { get; } = vigesimalLeadingToken;
    public string[] VigesimalFollowerTokens { get; } = vigesimalFollowerTokens;
    public int VigesimalValue { get; } = vigesimalValue;
    public string TeenLeaderToken { get; } = teenLeaderToken;
    public FrozenSet<int> TeenLeaderBases { get; } = teenLeaderBases;
    public int TeenBaseValue { get; } = teenBaseValue;
    public int HundredValue { get; } = hundredValue;
    public int ScaleThreshold { get; } = scaleThreshold;
}

namespace Humanizer;

internal class ScandinavianCompoundWordsToNumberConverter(ScandinavianCompoundWordsToNumberProfile profile) : GenderlessWordsToNumberConverter
{
    readonly ScandinavianCompoundWordsToNumberProfile profile = profile;

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

        if (profile.OrdinalMap.TryGetValue(normalized, out parsedValue) ||
            TryParseCardinal(normalized, out parsedValue))
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

        if (words.Contains(' '))
        {
            var total = 0;
            var current = 0;

            foreach (var tokenSpan in WordsToNumberTokenizer.Enumerate(words))
            {
                var token = tokenSpan.ToString();

                if (token == profile.IgnoredToken)
                {
                    continue;
                }

                if (!TryParseCardinal(token, out var tokenValue))
                {
                    value = default;
                    return false;
                }

                if (tokenValue >= 1000)
                {
                    total += (current == 0 ? 1 : current) * tokenValue;
                    current = 0;
                }
                else if (profile.SequenceMultiplierThreshold.HasValue &&
                         tokenValue >= profile.SequenceMultiplierThreshold.Value)
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

        foreach (var scale in profile.LargeScales)
        {
            var index = words.IndexOf(scale, StringComparison.Ordinal);
            if (index < 0)
            {
                continue;
            }

            var left = words[..index].Trim();
            var right = words[(index + scale.Length)..].Trim();
            var factor = 1;

            if ((string.IsNullOrEmpty(left) || TryParseCardinal(left, out factor)) &&
                TryParseOptional(right, out var remainder))
            {
                value = factor * profile.CardinalMap[scale] + remainder;
                return true;
            }
        }

        foreach (var tens in profile.Tens)
        {
            if (!words.StartsWith(tens, StringComparison.Ordinal))
            {
                continue;
            }

            var remainder = words[tens.Length..];
            if (string.IsNullOrEmpty(remainder))
            {
                value = profile.CardinalMap[tens];
                return true;
            }

            if (TryParseCardinal(remainder, out var unit) && unit is >= 1 and <= 9)
            {
                value = profile.CardinalMap[tens] + unit;
                return true;
            }
        }

        value = default;
        return false;
    }

    bool TryParseOptional(string words, out int value)
    {
        if (string.IsNullOrEmpty(words))
        {
            value = 0;
            return true;
        }

        if (!string.IsNullOrEmpty(profile.IgnoredToken))
        {
            var ignoredTokenWithSpace = profile.IgnoredToken + " ";
            if (words.StartsWith(ignoredTokenWithSpace, StringComparison.Ordinal))
            {
                words = words[ignoredTokenWithSpace.Length..];
            }
            else if (words.StartsWith(profile.IgnoredToken, StringComparison.Ordinal))
            {
                words = words[profile.IgnoredToken.Length..];
            }
        }

        return TryParseCardinal(words, out value);
    }

    static string Normalize(string words) =>
        Regex.Replace(words.Replace(",", string.Empty)
                .Replace(".", string.Empty)
                .Replace("-", " ")
                .ToLowerInvariant()
                .Trim(),
            @"\s+",
            " ");

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

sealed class ScandinavianCompoundWordsToNumberProfile(
    FrozenDictionary<string, int> cardinalMap,
    string[] tens,
    string[] largeScales,
    string ignoredToken,
    FrozenDictionary<string, int> ordinalMap,
    string[] negativePrefixes,
    int? sequenceMultiplierThreshold = null)
{
    public FrozenDictionary<string, int> CardinalMap { get; } = cardinalMap;
    public string[] Tens { get; } = tens;
    public string[] LargeScales { get; } = largeScales;
    public string IgnoredToken { get; } = ignoredToken;
    public FrozenDictionary<string, int> OrdinalMap { get; } = ordinalMap;
    public string[] NegativePrefixes { get; } = negativePrefixes;
    public int? SequenceMultiplierThreshold { get; } = sequenceMultiplierThreshold;
}

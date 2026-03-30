namespace Humanizer;

internal class GreedyCompoundWordsToNumberConverter(GreedyCompoundWordsToNumberProfile profile) : GenderlessWordsToNumberConverter
{
    readonly GreedyCompoundWordsToNumberProfile profile = profile;
    readonly string[] cardinalTokenOrder = profile.CardinalMap.Keys
        .OrderByDescending(static key => key.Length)
        .ToArray();

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

        var trimmed = words.Trim();
        var normalized = Normalize(trimmed);
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

        if (TryParseOrdinalAbbreviation(trimmed, out parsedValue) ||
            profile.OrdinalMap.TryGetValue(normalized, out parsedValue) ||
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

    string Normalize(string words)
        => Normalize(
            words,
            profile.CharactersToRemove,
            profile.CharactersToReplaceWithSpace,
            profile.TextReplacements,
            profile.Lowercase,
            profile.RemoveDiacritics);

    internal static FrozenDictionary<string, int> BuildOrdinalMap(
        INumberToWordsConverter converter,
        string charactersToRemove,
        string charactersToReplaceWithSpace,
        StringReplacement[] textReplacements,
        bool lowercase = false,
        bool removeDiacritics = false)
    {
        var ordinals = new Dictionary<string, int>(StringComparer.Ordinal);

        for (var number = 1; number <= 200; number++)
        {
            foreach (var gender in new[] { GrammaticalGender.Masculine, GrammaticalGender.Feminine })
            {
                var ordinal = Normalize(
                    converter.ConvertToOrdinal(number, gender),
                    charactersToRemove,
                    charactersToReplaceWithSpace,
                    textReplacements,
                    lowercase,
                    removeDiacritics);

                if (!ordinals.ContainsKey(ordinal))
                {
                    ordinals[ordinal] = number;
                }
            }
        }

        return ordinals.ToFrozenDictionary(StringComparer.Ordinal);
    }

    static string Normalize(
        string words,
        string charactersToRemove,
        string charactersToReplaceWithSpace,
        StringReplacement[] textReplacements,
        bool lowercase,
        bool removeDiacritics)
    {
        var normalized = removeDiacritics
            ? TokenMapWordsToNumberNormalizer.RemoveDiacritics(words)
            : words;

        foreach (var replacement in textReplacements)
        {
            normalized = normalized.Replace(replacement.OldValue, replacement.NewValue);
        }

        var source = normalized.AsSpan().Trim();
        var builder = new StringBuilder(source.Length);
        var previousWasSpace = false;

        foreach (var sourceChar in source)
        {
            var current = lowercase
                ? char.ToLowerInvariant(sourceChar)
                : sourceChar;

            if (charactersToRemove.Contains(current))
            {
                continue;
            }

            if (charactersToReplaceWithSpace.Contains(current))
            {
                current = ' ';
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

    bool TryParseCardinal(string words, out int value, out string? unrecognizedWord)
    {
        if (profile.CardinalMap.TryGetValue(words, out value))
        {
            unrecognizedWord = null;
            return true;
        }

        value = default;
        unrecognizedWord = null;
        var total = 0;
        var current = 0;
        var position = 0;

        if (string.IsNullOrEmpty(words))
        {
            unrecognizedWord = string.Empty;
            return false;
        }

        while (position < words.Length)
        {
            if (char.IsWhiteSpace(words[position]))
            {
                position++;
                continue;
            }

            if (!TryReadToken(words, ref position, out var token))
            {
                var start = position;
                while (position < words.Length && !char.IsWhiteSpace(words[position]))
                {
                    position++;
                }

                unrecognizedWord = words[start..position];
                return false;
            }

            if (ShouldIgnore(token))
            {
                continue;
            }

            if (!profile.CardinalMap.TryGetValue(token, out var numeric))
            {
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

    bool TryParseOrdinalAbbreviation(string words, out int value)
    {
        if (profile.OrdinalAbbreviationSuffixes.Length == 0)
        {
            value = default;
            return false;
        }

        var span = words.AsSpan().Trim();
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
        foreach (var candidate in profile.OrdinalAbbreviationSuffixes)
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

    bool TryReadToken(string words, ref int position, out string token)
    {
        foreach (var candidate in cardinalTokenOrder)
        {
            if (position + candidate.Length > words.Length)
            {
                continue;
            }

            if (words.AsSpan(position, candidate.Length).SequenceEqual(candidate.AsSpan()))
            {
                token = candidate;
                position += candidate.Length;
                return true;
            }
        }

        token = string.Empty;
        return false;
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

}

sealed class GreedyCompoundWordsToNumberProfile(
    FrozenDictionary<string, int> cardinalMap,
    FrozenDictionary<string, int> ordinalMap,
    string[] negativePrefixes,
    string[] ignoredTokens,
    string[] ordinalAbbreviationSuffixes,
    string charactersToRemove,
    string charactersToReplaceWithSpace,
    StringReplacement[] textReplacements,
    bool lowercase = false,
    bool removeDiacritics = false,
    int hundredValue = 100,
    int scaleThreshold = 1000)
{
    public FrozenDictionary<string, int> CardinalMap { get; } = cardinalMap;
    public FrozenDictionary<string, int> OrdinalMap { get; } = ordinalMap;
    public string[] NegativePrefixes { get; } = negativePrefixes;
    public string[] IgnoredTokens { get; } = ignoredTokens;
    public string[] OrdinalAbbreviationSuffixes { get; } = ordinalAbbreviationSuffixes;
    public string CharactersToRemove { get; } = charactersToRemove;
    public string CharactersToReplaceWithSpace { get; } = charactersToReplaceWithSpace;
    public StringReplacement[] TextReplacements { get; } = textReplacements;
    public bool Lowercase { get; } = lowercase;
    public bool RemoveDiacritics { get; } = removeDiacritics;
    public int HundredValue { get; } = hundredValue;
    public int ScaleThreshold { get; } = scaleThreshold;
}

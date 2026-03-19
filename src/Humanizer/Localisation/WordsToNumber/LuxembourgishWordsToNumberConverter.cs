namespace Humanizer;

internal class LuxembourgishWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly FrozenDictionary<string, int> CardinalMap = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["null"] = 0,
        ["een"] = 1,
        ["eng"] = 1,
        ["zwee"] = 2,
        ["zwou"] = 2,
        ["dräi"] = 3,
        ["drai"] = 3,
        ["véier"] = 4,
        ["veier"] = 4,
        ["fënnef"] = 5,
        ["fennef"] = 5,
        ["sechs"] = 6,
        ["siwen"] = 7,
        ["aacht"] = 8,
        ["néng"] = 9,
        ["neng"] = 9,
        ["zéng"] = 10,
        ["zeng"] = 10,
        ["eelef"] = 11,
        ["zwielef"] = 12,
        ["dräizéng"] = 13,
        ["draizeng"] = 13,
        ["véierzéng"] = 14,
        ["veierzeng"] = 14,
        ["fofzéng"] = 15,
        ["fofzeng"] = 15,
        ["sechzéng"] = 16,
        ["sechzeng"] = 16,
        ["siwwenzéng"] = 17,
        ["siwwenzeng"] = 17,
        ["uechtzéng"] = 18,
        ["uechtzeng"] = 18,
        ["nonzéng"] = 19,
        ["nonzeng"] = 19,
        ["zwanzeg"] = 20,
        ["drësseg"] = 30,
        ["dresseg"] = 30,
        ["véierzeg"] = 40,
        ["veierzeg"] = 40,
        ["fofzeg"] = 50,
        ["sechzeg"] = 60,
        ["siwwenzeg"] = 70,
        ["achtzeg"] = 80,
        ["nonzeg"] = 90,
        ["Millioun"] = 1_000_000,
        ["Milliounen"] = 1_000_000,
        ["Milliard"] = 1_000_000_000,
        ["Milliarden"] = 1_000_000_000
    }.ToFrozenDictionary(StringComparer.Ordinal);

    static readonly FrozenDictionary<string, int> UnitPrefixes = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["een"] = 1,
        ["eng"] = 1,
        ["zwee"] = 2,
        ["zwou"] = 2,
        ["dräi"] = 3,
        ["drai"] = 3,
        ["véier"] = 4,
        ["veier"] = 4,
        ["fënnef"] = 5,
        ["fennef"] = 5,
        ["sechs"] = 6,
        ["siwen"] = 7,
        ["aacht"] = 8,
        ["néng"] = 9,
        ["neng"] = 9
    }.ToFrozenDictionary(StringComparer.Ordinal);

    static readonly (string Word, int Value)[] TensWords =
    [
        ("nonzeg", 90),
        ("achtzeg", 80),
        ("siwwenzeg", 70),
        ("sechzeg", 60),
        ("fofzeg", 50),
        ("véierzeg", 40),
        ("veierzeg", 40),
        ("drësseg", 30),
        ("dresseg", 30),
        ("zwanzeg", 20)
    ];

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

        var normalized = Regex.Replace(words.Trim(), @"\s+", " ");
        if (TryParsePhrase(normalized, out parsedValue, out unrecognizedWord))
        {
            return true;
        }

        if (normalized.StartsWith("minus ", StringComparison.Ordinal) &&
            TryParsePhrase(normalized["minus ".Length..], out parsedValue, out unrecognizedWord))
        {
            parsedValue = -parsedValue;
            unrecognizedWord = null;
            return true;
        }

        parsedValue = default;
        return false;
    }

    static bool TryParsePhrase(string words, out int value, out string? unrecognizedWord)
    {
        var tokens = words.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var total = 0;
        var current = 0;
        unrecognizedWord = null;

        foreach (var rawToken in tokens)
        {
            if (CardinalMap.TryGetValue(rawToken, out var scale) && scale >= 1_000_000)
            {
                total += (current == 0 ? 1 : current) * scale;
                current = 0;
                continue;
            }

            if (!TryParseCompact(rawToken, out var numeric))
            {
                value = default;
                unrecognizedWord = rawToken;
                return false;
            }

            current += numeric;
        }

        value = total + current;
        return true;
    }

    static bool TryParseCompact(string word, out int value)
    {
        if (CardinalMap.TryGetValue(word, out value))
        {
            return value < 1_000_000;
        }

        var dausendIndex = word.IndexOf("dausend", StringComparison.Ordinal);
        if (dausendIndex >= 0)
        {
            var left = word[..dausendIndex];
            var right = word[(dausendIndex + "dausend".Length)..];
            var factor = 1;
            if (!string.IsNullOrEmpty(left) && !TryParseCompact(left, out factor))
            {
                value = default;
                return false;
            }

            if (!TryParseOptional(right, out var remainder))
            {
                value = default;
                return false;
            }

            value = factor * 1_000 + remainder;
            return true;
        }

        var honnertIndex = word.IndexOf("honnert", StringComparison.Ordinal);
        if (honnertIndex >= 0)
        {
            var left = word[..honnertIndex];
            var right = word[(honnertIndex + "honnert".Length)..];
            var factor = 1;
            if (!string.IsNullOrEmpty(left) && !TryParseCompact(left, out factor))
            {
                value = default;
                return false;
            }

            if (!TryParseOptional(right, out var remainder))
            {
                value = default;
                return false;
            }

            value = factor * 100 + remainder;
            return true;
        }

        foreach (var (wordPart, tensValue) in TensWords)
        {
            if (!word.EndsWith(wordPart, StringComparison.Ordinal))
            {
                continue;
            }

            var prefix = word[..^wordPart.Length];
            if (!prefix.EndsWith("an", StringComparison.Ordinal))
            {
                continue;
            }

            prefix = prefix[..^2];
            if (UnitPrefixes.TryGetValue(prefix, out var unit))
            {
                value = unit + tensValue;
                return true;
            }
        }

        value = default;
        return false;
    }

    static bool TryParseOptional(string word, out int value)
    {
        if (string.IsNullOrEmpty(word))
        {
            value = 0;
            return true;
        }

        return TryParseCompact(word, out value);
    }
}

namespace Humanizer;

internal class DanishWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly FrozenDictionary<string, int> CardinalMap = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["nul"] = 0,
        ["en"] = 1,
        ["et"] = 1,
        ["to"] = 2,
        ["tre"] = 3,
        ["fire"] = 4,
        ["fem"] = 5,
        ["seks"] = 6,
        ["syv"] = 7,
        ["otte"] = 8,
        ["ni"] = 9,
        ["ti"] = 10,
        ["elleve"] = 11,
        ["tolv"] = 12,
        ["tretten"] = 13,
        ["fjorten"] = 14,
        ["femten"] = 15,
        ["seksten"] = 16,
        ["sytten"] = 17,
        ["atten"] = 18,
        ["nitten"] = 19,
        ["tyve"] = 20,
        ["tredive"] = 30,
        ["fyrre"] = 40,
        ["halvtreds"] = 50,
        ["tres"] = 60,
        ["halvfjerds"] = 70,
        ["firs"] = 80,
        ["halvfems"] = 90,
        ["hundrede"] = 100,
        ["tusind"] = 1000,
        ["million"] = 1_000_000,
        ["millioner"] = 1_000_000,
        ["milliard"] = 1_000_000_000,
        ["milliarder"] = 1_000_000_000
    }.ToFrozenDictionary(StringComparer.Ordinal);

    static readonly FrozenDictionary<string, int> TensMap = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["tyve"] = 20,
        ["tredive"] = 30,
        ["fyrre"] = 40,
        ["halvtreds"] = 50,
        ["tres"] = 60,
        ["halvfjerds"] = 70,
        ["firs"] = 80,
        ["halvfems"] = 90
    }.ToFrozenDictionary(StringComparer.Ordinal);

    static readonly string[] ScaleTokens =
    [
        "milliarder",
        "milliard",
        "millioner",
        "million",
        "tusind",
        "hundrede"
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

        var normalized = Normalize(words);
        var negative = false;

        if (normalized.StartsWith("minus ", StringComparison.Ordinal))
        {
            negative = true;
            normalized = normalized["minus ".Length..].Trim();
        }

        if (int.TryParse(normalized, NumberStyles.Integer, CultureInfo.InvariantCulture, out parsedValue) ||
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

    static string Normalize(string words) =>
        Regex.Replace(words.Replace(",", string.Empty)
                .Replace(".", string.Empty)
                .Replace("-", " ")
                .ToLowerInvariant()
                .Trim(),
            @"\s+",
            " ");

    static bool TryParseCardinal(string words, out int value, out string? unrecognizedWord)
    {
        if (CardinalMap.TryGetValue(words, out value))
        {
            unrecognizedWord = null;
            return true;
        }

        if (words.Contains(' '))
        {
            return TryParseSequence(words, out value, out unrecognizedWord);
        }

        return TryParseCompositeToken(words, out value, out unrecognizedWord);
    }

    static bool TryParseSequence(string words, out int value, out string? unrecognizedWord)
    {
        var total = 0;
        var current = 0;
        unrecognizedWord = null;

        foreach (var token in words.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            if (token == "og")
            {
                continue;
            }

            if (!TryParseCardinal(token, out var tokenValue, out unrecognizedWord))
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

    static bool TryParseCompositeToken(string word, out int value, out string? unrecognizedWord)
    {
        if (TryParseCompoundTens(word, out value))
        {
            unrecognizedWord = null;
            return true;
        }

        foreach (var scale in ScaleTokens)
        {
            var index = word.IndexOf(scale, StringComparison.Ordinal);
            if (index < 0)
            {
                continue;
            }

            var left = word[..index].Trim();
            var right = StripLeadingAnd(word[(index + scale.Length)..].Trim());
            var factor = 1;

            if (!string.IsNullOrEmpty(left) &&
                !TryParseCardinal(left, out factor, out _))
            {
                continue;
            }

            if (TryParseOptional(right, out var remainder, out unrecognizedWord))
            {
                value = factor * CardinalMap[scale] + remainder;
                unrecognizedWord = null;
                return true;
            }
        }

        value = default;
        unrecognizedWord = word;
        return false;
    }

    static string StripLeadingAnd(string words)
    {
        if (string.IsNullOrEmpty(words))
        {
            return words;
        }

        if (words.StartsWith("og ", StringComparison.Ordinal))
        {
            return words["og ".Length..].TrimStart();
        }

        return words.StartsWith("og", StringComparison.Ordinal)
            ? words["og".Length..].Trim()
            : words;
    }

    static bool TryParseOptional(string words, out int value, out string? unrecognizedWord)
    {
        if (string.IsNullOrEmpty(words))
        {
            value = 0;
            unrecognizedWord = null;
            return true;
        }

        return TryParseCardinal(words, out value, out unrecognizedWord);
    }

    static bool TryParseCompoundTens(string word, out int value)
    {
        foreach (var (tensWord, tensValue) in TensMap)
        {
            if (!word.EndsWith(tensWord, StringComparison.Ordinal))
            {
                continue;
            }

            var prefix = word[..^tensWord.Length];
            if (!prefix.EndsWith("og", StringComparison.Ordinal))
            {
                continue;
            }

            var unitPart = prefix[..^"og".Length];
            if (TryParseCardinal(unitPart, out var unitValue, out _) && unitValue is >= 1 and <= 9)
            {
                value = tensValue + unitValue;
                return true;
            }
        }

        value = default;
        return false;
    }
}

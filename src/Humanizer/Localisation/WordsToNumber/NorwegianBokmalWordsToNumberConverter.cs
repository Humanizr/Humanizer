namespace Humanizer;

internal class NorwegianBokmalWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly FrozenDictionary<string, int> CardinalMap = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["null"] = 0,
        ["en"] = 1,
        ["ei"] = 1,
        ["et"] = 1,
        ["to"] = 2,
        ["tre"] = 3,
        ["fire"] = 4,
        ["fem"] = 5,
        ["seks"] = 6,
        ["sju"] = 7,
        ["åtte"] = 8,
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
        ["tjue"] = 20,
        ["tretti"] = 30,
        ["førti"] = 40,
        ["femti"] = 50,
        ["seksti"] = 60,
        ["sytti"] = 70,
        ["åtti"] = 80,
        ["nitti"] = 90,
        ["hundre"] = 100,
        ["tusen"] = 1000,
        ["million"] = 1_000_000,
        ["millioner"] = 1_000_000,
        ["milliard"] = 1_000_000_000,
        ["milliarder"] = 1_000_000_000
    }.ToFrozenDictionary(StringComparer.Ordinal);

    static readonly string[] Tens = ["tjue", "tretti", "førti", "femti", "seksti", "sytti", "åtti", "nitti"];
    static readonly string[] LargeScales = ["milliard", "millioner", "million", "tusen", "hundre"];

    static readonly FrozenDictionary<string, int> OrdinalMap = BuildOrdinalMap();

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

        if (OrdinalMap.TryGetValue(normalized, out parsedValue) ||
            TryParseCardinal(normalized, out parsedValue))
        {
            if (negative)
            {
                parsedValue = -parsedValue;
            }

            unrecognizedWord = null;
            return true;
        }

        unrecognizedWord = normalized.Split(' ', StringSplitOptions.RemoveEmptyEntries).LastOrDefault() ?? normalized;
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

    static bool TryParseCardinal(string words, out int value)
    {
        if (CardinalMap.TryGetValue(words, out value))
        {
            return true;
        }

        if (words.Contains(' '))
        {
            var total = 0;
            var current = 0;

            foreach (var token in words.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                if (token == "og")
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
                else
                {
                    current += tokenValue;
                }
            }

            value = total + current;
            return true;
        }

        foreach (var scale in LargeScales)
        {
            var index = words.IndexOf(scale, StringComparison.Ordinal);
            if (index >= 0)
            {
                var left = words[..index].Trim();
                var right = words[(index + scale.Length)..].Trim();
                var factor = 1;

                if ((string.IsNullOrEmpty(left) || TryParseCardinal(left, out factor)) &&
                    TryParseOptional(right, out var remainder))
                {
                    value = factor * CardinalMap[scale] + remainder;
                    return true;
                }
            }
        }

        foreach (var tens in Tens)
        {
            if (words.StartsWith(tens, StringComparison.Ordinal))
            {
                var remainder = words[tens.Length..];
                if (string.IsNullOrEmpty(remainder))
                {
                    value = CardinalMap[tens];
                    return true;
                }

                if (TryParseCardinal(remainder, out var unit) && unit is >= 1 and <= 9)
                {
                    value = CardinalMap[tens] + unit;
                    return true;
                }
            }
        }

        value = default;
        return false;
    }

    static bool TryParseOptional(string words, out int value)
    {
        if (string.IsNullOrEmpty(words))
        {
            value = 0;
            return true;
        }

        if (words.StartsWith("og ", StringComparison.Ordinal))
        {
            words = words["og ".Length..];
        }
        else if (words.StartsWith("og", StringComparison.Ordinal))
        {
            words = words["og".Length..];
        }

        return TryParseCardinal(words, out value);
    }

    static FrozenDictionary<string, int> BuildOrdinalMap()
    {
        var converter = new NorwegianBokmalNumberToWordsConverter();
        var ordinals = new Dictionary<string, int>(StringComparer.Ordinal);

        for (var number = 1; number <= 200; number++)
        {
            ordinals[Normalize(converter.ConvertToOrdinal(number))] = number;
        }

        return ordinals.ToFrozenDictionary(StringComparer.Ordinal);
    }
}

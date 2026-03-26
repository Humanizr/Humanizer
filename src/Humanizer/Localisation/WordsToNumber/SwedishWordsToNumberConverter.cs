namespace Humanizer;

internal class SwedishWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly FrozenDictionary<string, int> CardinalMap = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["noll"] = 0,
        ["ett"] = 1,
        ["en"] = 1,
        ["två"] = 2,
        ["tre"] = 3,
        ["fyra"] = 4,
        ["fem"] = 5,
        ["sex"] = 6,
        ["sju"] = 7,
        ["åtta"] = 8,
        ["nio"] = 9,
        ["tio"] = 10,
        ["elva"] = 11,
        ["tolv"] = 12,
        ["tretton"] = 13,
        ["fjorton"] = 14,
        ["femton"] = 15,
        ["sexton"] = 16,
        ["sjutton"] = 17,
        ["arton"] = 18,
        ["nitton"] = 19,
        ["tjugo"] = 20,
        ["trettio"] = 30,
        ["fyrtio"] = 40,
        ["femtio"] = 50,
        ["sextio"] = 60,
        ["sjuttio"] = 70,
        ["åttio"] = 80,
        ["nittio"] = 90,
        ["hundra"] = 100,
        ["tusen"] = 1000,
        ["miljon"] = 1_000_000,
        ["miljoner"] = 1_000_000,
        ["miljard"] = 1_000_000_000,
        ["miljarder"] = 1_000_000_000
    }.ToFrozenDictionary(StringComparer.Ordinal);

    static readonly string[] Tens = ["tjugo", "trettio", "fyrtio", "femtio", "sextio", "sjuttio", "åttio", "nittio"];
    static readonly string[] LargeScales = ["miljard", "miljoner", "miljon", "tusen", "hundra"];

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

        unrecognizedWord = WordsToNumberTokenizer.GetLastTokenOrSelf(normalized);
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

            foreach (var tokenSpan in WordsToNumberTokenizer.Enumerate(words))
            {
                var token = tokenSpan.ToString();

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

        return TryParseCardinal(words, out value);
    }

    static FrozenDictionary<string, int> BuildOrdinalMap()
    {
        var converter = new SwedishNumberToWordsConverter();
        var ordinals = new Dictionary<string, int>(StringComparer.Ordinal);

        for (var number = 1; number <= 200; number++)
        {
            ordinals[Normalize(converter.ConvertToOrdinal(number))] = number;
        }

        return ordinals.ToFrozenDictionary(StringComparer.Ordinal);
    }
}

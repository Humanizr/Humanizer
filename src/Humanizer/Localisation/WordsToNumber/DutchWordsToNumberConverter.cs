namespace Humanizer;

internal class DutchWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly FrozenDictionary<string, int> CardinalMap = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["nul"] = 0,
        ["een"] = 1,
        ["twee"] = 2,
        ["drie"] = 3,
        ["vier"] = 4,
        ["vijf"] = 5,
        ["zes"] = 6,
        ["zeven"] = 7,
        ["acht"] = 8,
        ["negen"] = 9,
        ["tien"] = 10,
        ["elf"] = 11,
        ["twaalf"] = 12,
        ["dertien"] = 13,
        ["veertien"] = 14,
        ["vijftien"] = 15,
        ["zestien"] = 16,
        ["zeventien"] = 17,
        ["achttien"] = 18,
        ["negentien"] = 19,
        ["twintig"] = 20,
        ["dertig"] = 30,
        ["veertig"] = 40,
        ["vijftig"] = 50,
        ["zestig"] = 60,
        ["zeventig"] = 70,
        ["tachtig"] = 80,
        ["negentig"] = 90,
        ["honderd"] = 100,
        ["duizend"] = 1000,
        ["miljoen"] = 1_000_000,
        ["miljard"] = 1_000_000_000
    }.ToFrozenDictionary(StringComparer.Ordinal);

    static readonly string[] Tens = ["twintig", "dertig", "veertig", "vijftig", "zestig", "zeventig", "tachtig", "negentig"];
    static readonly string[] LargeScales = ["miljard", "miljoen", "duizend", "honderd"];

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

        if (normalized.StartsWith("min ", StringComparison.Ordinal))
        {
            negative = true;
            normalized = normalized["min ".Length..].Trim();
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

        if (TryParseCompoundTens(words, out value))
        {
            return true;
        }

        if (words.Contains(' '))
        {
            var collapsed = words.Replace(" ", string.Empty);
            if (TryParseCardinal(collapsed, out value))
            {
                return true;
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

    static bool TryParseCompoundTens(string words, out int value)
    {
        foreach (var tens in Tens)
        {
            if (!words.EndsWith(tens, StringComparison.Ordinal))
            {
                continue;
            }

            var prefix = words[..^tens.Length];
            if (!prefix.EndsWith("en", StringComparison.Ordinal))
            {
                continue;
            }

            prefix = prefix[..^2];
            prefix = prefix.Replace("ë", "e");

            if (TryParseCardinal(prefix, out var unit) && unit is >= 1 and <= 9)
            {
                value = unit + CardinalMap[tens];
                return true;
            }
        }

        value = default;
        return false;
    }

    static FrozenDictionary<string, int> BuildOrdinalMap()
    {
        var converter = new DutchNumberToWordsConverter();
        var ordinals = new Dictionary<string, int>(StringComparer.Ordinal);

        for (var number = 1; number <= 200; number++)
        {
            ordinals[Normalize(converter.ConvertToOrdinal(number))] = number;
        }

        return ordinals.ToFrozenDictionary(StringComparer.Ordinal);
    }
}

namespace Humanizer;

internal class IcelandicWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly FrozenDictionary<string, int> CardinalMap = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["núll"] = 0,
        ["einn"] = 1,
        ["ein"] = 1,
        ["eitt"] = 1,
        ["tveir"] = 2,
        ["tvær"] = 2,
        ["tvö"] = 2,
        ["þrír"] = 3,
        ["þrjár"] = 3,
        ["þrjú"] = 3,
        ["fjórir"] = 4,
        ["fjórar"] = 4,
        ["fjögur"] = 4,
        ["fimm"] = 5,
        ["sex"] = 6,
        ["sjö"] = 7,
        ["átta"] = 8,
        ["níu"] = 9,
        ["tíu"] = 10,
        ["ellefu"] = 11,
        ["tólf"] = 12,
        ["þrettán"] = 13,
        ["fjórtán"] = 14,
        ["fimmtán"] = 15,
        ["sextán"] = 16,
        ["sautján"] = 17,
        ["átján"] = 18,
        ["nítján"] = 19,
        ["tuttugu"] = 20,
        ["þrjátíu"] = 30,
        ["fjörutíu"] = 40,
        ["fimmtíu"] = 50,
        ["sextíu"] = 60,
        ["sjötíu"] = 70,
        ["áttatíu"] = 80,
        ["níutíu"] = 90,
        ["hundrað"] = 100,
        ["hundruð"] = 100,
        ["þúsund"] = 1000,
        ["milljón"] = 1_000_000,
        ["milljónir"] = 1_000_000,
        ["milljarður"] = 1_000_000_000,
        ["milljarðar"] = 1_000_000_000
    }.ToFrozenDictionary(StringComparer.Ordinal);

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

        if (normalized.StartsWith("mínus ", StringComparison.Ordinal))
        {
            negative = true;
            normalized = normalized["mínus ".Length..].Trim();
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

        var tokens = words.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (tokens.Length > 1)
        {
            var total = 0;
            var current = 0;

            foreach (var token in tokens)
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

        foreach (var scale in new[] { "trilljón", "billjarður", "billjón", "milljarður", "milljón", "þúsund", "hundrað" })
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

        foreach (var tens in new[] { "tuttugu", "þrjátíu", "fjörutíu", "fimmtíu", "sextíu", "sjötíu", "áttatíu", "níutíu" })
        {
            if (words.StartsWith(tens, StringComparison.Ordinal))
            {
                var remainder = words[tens.Length..].Trim();
                if (string.IsNullOrEmpty(remainder))
                {
                    value = CardinalMap[tens];
                    return true;
                }

                if (remainder.StartsWith("og ", StringComparison.Ordinal))
                {
                    remainder = remainder["og ".Length..];
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

        return TryParseCardinal(words, out value);
    }

    static FrozenDictionary<string, int> BuildOrdinalMap()
    {
        var converter = new IcelandicNumberToWordsConverter();
        var ordinals = new Dictionary<string, int>(StringComparer.Ordinal);

        for (var number = 1; number <= 200; number++)
        {
            ordinals[Normalize(converter.ConvertToOrdinal(number, GrammaticalGender.Masculine))] = number;
            ordinals[Normalize(converter.ConvertToOrdinal(number, GrammaticalGender.Feminine))] = number;
            ordinals[Normalize(converter.ConvertToOrdinal(number, GrammaticalGender.Neuter))] = number;
        }

        return ordinals.ToFrozenDictionary(StringComparer.Ordinal);
    }
}

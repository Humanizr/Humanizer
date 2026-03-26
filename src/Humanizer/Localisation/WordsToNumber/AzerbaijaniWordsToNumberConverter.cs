namespace Humanizer;

internal class AzerbaijaniWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly FrozenDictionary<string, int> CardinalMap = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["sıfır"] = 0,
        ["bir"] = 1,
        ["iki"] = 2,
        ["üç"] = 3,
        ["dörd"] = 4,
        ["beş"] = 5,
        ["altı"] = 6,
        ["yeddi"] = 7,
        ["səkkiz"] = 8,
        ["doqquz"] = 9,
        ["on"] = 10,
        ["iyirmi"] = 20,
        ["otuz"] = 30,
        ["qırx"] = 40,
        ["əlli"] = 50,
        ["altmış"] = 60,
        ["yetmiş"] = 70,
        ["səksən"] = 80,
        ["doxsan"] = 90,
        ["yüz"] = 100,
        ["min"] = 1000,
        ["milyon"] = 1_000_000
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

        var normalized = Regex.Replace(words.Trim(), @"\s+", " ");
        var negative = false;

        if (normalized.StartsWith("mənfi ", StringComparison.Ordinal))
        {
            negative = true;
            normalized = normalized["mənfi ".Length..];
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

    static bool TryParseCardinal(string words, out int value)
    {
        if (CardinalMap.TryGetValue(words, out value))
        {
            return true;
        }

        var total = 0;
        var current = 0;
        foreach (var tokenSpan in WordsToNumberTokenizer.Enumerate(words))
        {
            var token = tokenSpan.ToString();

            if (!CardinalMap.TryGetValue(token, out var tokenValue))
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

    static FrozenDictionary<string, int> BuildOrdinalMap()
    {
        var converter = new AzerbaijaniNumberToWordsConverter();
        var ordinals = new Dictionary<string, int>(StringComparer.Ordinal);

        for (var number = 1; number <= 200; number++)
        {
            ordinals[converter.ConvertToOrdinal(number)] = number;
        }

        return ordinals.ToFrozenDictionary(StringComparer.Ordinal);
    }
}

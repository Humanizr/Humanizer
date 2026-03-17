namespace Humanizer;

internal class UzbekLatnWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly FrozenDictionary<string, int> CardinalMap = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["nol"] = 0,
        ["bir"] = 1,
        ["ikkinchi"] = 2,
        ["ikki"] = 2,
        ["uch"] = 3,
        ["to`rt"] = 4,
        ["besh"] = 5,
        ["olti"] = 6,
        ["yetti"] = 7,
        ["sakkiz"] = 8,
        ["to`qqiz"] = 9,
        ["o`n"] = 10,
        ["yigirma"] = 20,
        ["o`ttiz"] = 30,
        ["qirq"] = 40,
        ["ellik"] = 50,
        ["oltmish"] = 60,
        ["yetmish"] = 70,
        ["sakson"] = 80,
        ["to`qson"] = 90,
        ["yuz"] = 100,
        ["ming"] = 1000,
        ["million"] = 1_000_000,
        ["milliard"] = 1_000_000_000
    }.ToFrozenDictionary(StringComparer.Ordinal);

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

        if (normalized.StartsWith("minus ", StringComparison.Ordinal))
        {
            negative = true;
            normalized = normalized["minus ".Length..];
        }

        if (TryParseCardinal(normalized, out parsedValue))
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

    static bool TryParseCardinal(string words, out int value)
    {
        if (CardinalMap.TryGetValue(words, out value))
        {
            return true;
        }

        var total = 0;
        var current = 0;

        foreach (var token in words.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            var normalizedToken = token;
            if (!CardinalMap.TryGetValue(normalizedToken, out var tokenValue))
            {
                if (normalizedToken.EndsWith("inchi", StringComparison.Ordinal))
                {
                    normalizedToken = normalizedToken[..^"inchi".Length];
                }
                else if (normalizedToken.EndsWith("nchi", StringComparison.Ordinal))
                {
                    normalizedToken = normalizedToken[..^"nchi".Length];
                }

                if (!CardinalMap.TryGetValue(normalizedToken, out tokenValue))
                {
                    value = default;
                    return false;
                }
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
}

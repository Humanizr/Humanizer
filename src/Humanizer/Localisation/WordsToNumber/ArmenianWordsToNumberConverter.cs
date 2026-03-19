namespace Humanizer;

internal class ArmenianWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly FrozenDictionary<string, int> CardinalMap = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["զրո"] = 0,
        ["մեկ"] = 1,
        ["երկու"] = 2,
        ["երեք"] = 3,
        ["չորս"] = 4,
        ["հինգ"] = 5,
        ["վեց"] = 6,
        ["յոթ"] = 7,
        ["ութ"] = 8,
        ["ինը"] = 9,
        ["տաս"] = 10,
        ["տասնմեկ"] = 11,
        ["տասներկու"] = 12,
        ["տասներեք"] = 13,
        ["տասնչորս"] = 14,
        ["տասնհինգ"] = 15,
        ["տասնվեց"] = 16,
        ["տասնյոթ"] = 17,
        ["տասնութ"] = 18,
        ["տասնինը"] = 19,
        ["քսան"] = 20,
        ["երեսուն"] = 30,
        ["քառասուն"] = 40,
        ["հիսուն"] = 50,
        ["վաթսուն"] = 60,
        ["յոթանասուն"] = 70,
        ["ութսուն"] = 80,
        ["իննսուն"] = 90,
        ["հարյուր"] = 100,
        ["հազար"] = 1_000,
        ["միլիոն"] = 1_000_000,
        ["միլիարդ"] = 1_000_000_000
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
        if (TryParseCardinal(normalized, out var longValue, out unrecognizedWord))
        {
            if (longValue > int.MaxValue || longValue < int.MinValue)
            {
                parsedValue = default;
                unrecognizedWord = normalized;
                return false;
            }

            parsedValue = (int)longValue;
            return true;
        }

        if (normalized.StartsWith("մինուս ", StringComparison.Ordinal) &&
            TryParseCardinal(normalized["մինուս ".Length..], out longValue, out unrecognizedWord))
        {
            if (-longValue > int.MaxValue || -longValue < int.MinValue)
            {
                parsedValue = default;
                unrecognizedWord = normalized;
                return false;
            }

            parsedValue = (int)-longValue;
            unrecognizedWord = null;
            return true;
        }

        parsedValue = default;
        return false;
    }

    static bool TryParseCardinal(string words, out long value, out string? unrecognizedWord)
    {
        var total = 0L;
        var current = 0L;
        unrecognizedWord = null;

        foreach (var token in words.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            if (!CardinalMap.TryGetValue(token, out var numeric))
            {
                value = default;
                unrecognizedWord = token;
                return false;
            }

            if (token == "հարյուր")
            {
                current = (current == 0 ? 1 : current) * 100;
                continue;
            }

            if (numeric >= 1000)
            {
                total += (current == 0 ? 1 : current) * numeric;
                current = 0;
                continue;
            }

            current += numeric;
        }

        value = total + current;
        return true;
    }
}

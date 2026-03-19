namespace Humanizer;

internal class LithuanianWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly FrozenDictionary<string, int> CardinalMap = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["nulis"] = 0,
        ["vienas"] = 1,
        ["viena"] = 1,
        ["du"] = 2,
        ["dvi"] = 2,
        ["trys"] = 3,
        ["keturi"] = 4,
        ["keturios"] = 4,
        ["penki"] = 5,
        ["penkios"] = 5,
        ["šeši"] = 6,
        ["sesi"] = 6,
        ["šešios"] = 6,
        ["sesios"] = 6,
        ["septyni"] = 7,
        ["septynios"] = 7,
        ["aštuoni"] = 8,
        ["astuoni"] = 8,
        ["aštuonios"] = 8,
        ["astuonios"] = 8,
        ["devyni"] = 9,
        ["devynios"] = 9,
        ["dešimt"] = 10,
        ["desimt"] = 10,
        ["vienuolika"] = 11,
        ["dvylika"] = 12,
        ["trylika"] = 13,
        ["keturiolika"] = 14,
        ["penkiolika"] = 15,
        ["šešiolika"] = 16,
        ["sesiolika"] = 16,
        ["septyniolika"] = 17,
        ["aštuoniolika"] = 18,
        ["astuoniolika"] = 18,
        ["devyniolika"] = 19,
        ["dvidešimt"] = 20,
        ["dvidesimt"] = 20,
        ["trisdešimt"] = 30,
        ["trisdesimt"] = 30,
        ["keturiasdešimt"] = 40,
        ["keturiasdesimt"] = 40,
        ["penkiasdešimt"] = 50,
        ["penkiasdesimt"] = 50,
        ["šešiasdešimt"] = 60,
        ["sesiasdesimt"] = 60,
        ["septyniasdešimt"] = 70,
        ["septyniasdesimt"] = 70,
        ["aštuoniasdešimt"] = 80,
        ["astuoniasdesimt"] = 80,
        ["devyniasdešimt"] = 90,
        ["devyniasdesimt"] = 90,
        ["šimtas"] = 100,
        ["simtas"] = 100,
        ["šimtai"] = 100,
        ["simtai"] = 100,
        ["tūkstantis"] = 1_000,
        ["tukstantis"] = 1_000,
        ["tūkstančiai"] = 1_000,
        ["tukstanciai"] = 1_000,
        ["tūkstančių"] = 1_000,
        ["tukstanciu"] = 1_000,
        ["milijonas"] = 1_000_000,
        ["milijonai"] = 1_000_000,
        ["milijonų"] = 1_000_000,
        ["milijonu"] = 1_000_000
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

        var normalized = Normalize(words);

        if (TryParseCardinal(normalized, out parsedValue, out unrecognizedWord))
        {
            return true;
        }

        if (normalized.StartsWith("minus ", StringComparison.Ordinal) &&
            TryParseCardinal(normalized["minus ".Length..], out parsedValue, out unrecognizedWord))
        {
            parsedValue = -parsedValue;
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
        var total = 0;
        var current = 0;
        unrecognizedWord = null;

        foreach (var token in words.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            if (!CardinalMap.TryGetValue(token, out var numeric))
            {
                value = default;
                unrecognizedWord = token;
                return false;
            }

            if (token is "šimtas" or "simtas" or "šimtai" or "simtai")
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

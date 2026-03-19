namespace Humanizer;

internal class LatvianWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly FrozenDictionary<string, int> CardinalMap = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["viens"] = 1,
        ["divi"] = 2,
        ["trīs"] = 3,
        ["tris"] = 3,
        ["četri"] = 4,
        ["cetri"] = 4,
        ["pieci"] = 5,
        ["seši"] = 6,
        ["sesi"] = 6,
        ["septiņi"] = 7,
        ["septini"] = 7,
        ["astoņi"] = 8,
        ["astoni"] = 8,
        ["deviņi"] = 9,
        ["devini"] = 9,
        ["desmit"] = 10,
        ["vienpadsmit"] = 11,
        ["divpadsmit"] = 12,
        ["trīspadsmit"] = 13,
        ["trispadsmit"] = 13,
        ["četrpadsmit"] = 14,
        ["cetrpadsmit"] = 14,
        ["piecpadsmit"] = 15,
        ["sešpadsmit"] = 16,
        ["sespadsmit"] = 16,
        ["septiņpadsmit"] = 17,
        ["septinpadsmit"] = 17,
        ["astoņpadsmit"] = 18,
        ["astonpadsmit"] = 18,
        ["deviņpadsmit"] = 19,
        ["devinpadsmit"] = 19,
        ["divdesmit"] = 20,
        ["trīsdesmit"] = 30,
        ["trisdesmit"] = 30,
        ["četrdesmit"] = 40,
        ["cetrdesmit"] = 40,
        ["piecdesmit"] = 50,
        ["sešdesmit"] = 60,
        ["sesdesmit"] = 60,
        ["septiņdesmit"] = 70,
        ["septindesmit"] = 70,
        ["astoņdesmit"] = 80,
        ["astondesmit"] = 80,
        ["deviņdesmit"] = 90,
        ["devindesmit"] = 90,
        ["simts"] = 100,
        ["simtu"] = 100,
        ["simti"] = 100,
        ["tūkstotis"] = 1_000,
        ["tukstotis"] = 1_000,
        ["tūkstoš"] = 1_000,
        ["tukstos"] = 1_000,
        ["tūkstoši"] = 1_000,
        ["tukstosi"] = 1_000,
        ["miljons"] = 1_000_000,
        ["miljoni"] = 1_000_000
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

            if (token is "simts" or "simtu" or "simti")
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

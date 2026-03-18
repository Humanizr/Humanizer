namespace Humanizer;

internal class CzechWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly FrozenDictionary<string, int> CardinalMap = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["nula"] = 0,
        ["jeden"] = 1,
        ["jedna"] = 1,
        ["jedno"] = 1,
        ["dva"] = 2,
        ["dvě"] = 2,
        ["dve"] = 2,
        ["tři"] = 3,
        ["tri"] = 3,
        ["čtyři"] = 4,
        ["ctyri"] = 4,
        ["pět"] = 5,
        ["pet"] = 5,
        ["šest"] = 6,
        ["sest"] = 6,
        ["sedm"] = 7,
        ["osm"] = 8,
        ["devět"] = 9,
        ["devet"] = 9,
        ["deset"] = 10,
        ["jedenáct"] = 11,
        ["jedenact"] = 11,
        ["dvanáct"] = 12,
        ["dvanact"] = 12,
        ["třináct"] = 13,
        ["trinact"] = 13,
        ["čtrnáct"] = 14,
        ["ctrnact"] = 14,
        ["patnáct"] = 15,
        ["patnact"] = 15,
        ["šestnáct"] = 16,
        ["sestnact"] = 16,
        ["sedmnáct"] = 17,
        ["sedmnact"] = 17,
        ["osmnáct"] = 18,
        ["osmnact"] = 18,
        ["devatenáct"] = 19,
        ["devatenact"] = 19,
        ["dvacet"] = 20,
        ["třicet"] = 30,
        ["tricet"] = 30,
        ["čtyřicet"] = 40,
        ["ctyricet"] = 40,
        ["padesát"] = 50,
        ["padesat"] = 50,
        ["šedesát"] = 60,
        ["sedmdesát"] = 70,
        ["sedmdesat"] = 70,
        ["osmdesát"] = 80,
        ["osmdesat"] = 80,
        ["devadesát"] = 90,
        ["devadesat"] = 90,
        ["sto"] = 100,
        ["sta"] = 100,
        ["set"] = 100,
        ["tisíc"] = 1_000,
        ["tisic"] = 1_000,
        ["tisíce"] = 1_000,
        ["tisice"] = 1_000,
        ["milion"] = 1_000_000,
        ["miliony"] = 1_000_000,
        ["milionů"] = 1_000_000,
        ["milionu"] = 1_000_000,
        ["miliarda"] = 1_000_000_000,
        ["miliardy"] = 1_000_000_000
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

        if ((normalized.StartsWith("mínus ", StringComparison.Ordinal) || normalized.StartsWith("minus ", StringComparison.Ordinal)) &&
            TryParseCardinal(normalized[(normalized[1] == 'í' ? "mínus " : "minus ").Length..], out parsedValue, out unrecognizedWord))
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

            if (token is "sta" or "set")
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

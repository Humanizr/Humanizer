namespace Humanizer;

internal class SerbianLatinWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly FrozenDictionary<string, int> CardinalMap = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["nula"] = 0,
        ["jedan"] = 1,
        ["jedna"] = 1,
        ["jedno"] = 1,
        ["dva"] = 2,
        ["dve"] = 2,
        ["tri"] = 3,
        ["četiri"] = 4,
        ["cetiri"] = 4,
        ["pet"] = 5,
        ["šest"] = 6,
        ["sest"] = 6,
        ["sedam"] = 7,
        ["osam"] = 8,
        ["devet"] = 9,
        ["deset"] = 10,
        ["jedanaest"] = 11,
        ["dvanaest"] = 12,
        ["trinaest"] = 13,
        ["četrnaest"] = 14,
        ["cetrnaest"] = 14,
        ["petnaest"] = 15,
        ["šesnaest"] = 16,
        ["sesnaest"] = 16,
        ["sedamnaest"] = 17,
        ["osamnaest"] = 18,
        ["devetnaest"] = 19,
        ["dvadeset"] = 20,
        ["trideset"] = 30,
        ["četrdeset"] = 40,
        ["cetrdeset"] = 40,
        ["pedeset"] = 50,
        ["šestdeset"] = 60,
        ["sestdeset"] = 60,
        ["sedamdeset"] = 70,
        ["osamdeset"] = 80,
        ["devedeset"] = 90,
        ["sto"] = 100,
        ["dvesto"] = 200,
        ["tristo"] = 300,
        ["četiristo"] = 400,
        ["cetiristo"] = 400,
        ["petsto"] = 500,
        ["šeststo"] = 600,
        ["seststo"] = 600,
        ["sedamsto"] = 700,
        ["osamsto"] = 800,
        ["devetsto"] = 900,
        ["hiljadu"] = 1_000,
        ["hiljada"] = 1_000,
        ["milion"] = 1_000_000,
        ["miliona"] = 1_000_000,
        ["milijarda"] = 1_000_000_000,
        ["milijarde"] = 1_000_000_000
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

        foreach (var tokenSpan in WordsToNumberTokenizer.Enumerate(words))
        {
            var token = tokenSpan.ToString();

            if (!CardinalMap.TryGetValue(token, out var numeric))
            {
                value = default;
                unrecognizedWord = token;
                return false;
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

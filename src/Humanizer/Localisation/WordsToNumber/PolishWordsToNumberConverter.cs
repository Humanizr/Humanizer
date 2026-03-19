namespace Humanizer;

internal class PolishWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly FrozenDictionary<string, int> CardinalMap = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["zero"] = 0,
        ["jeden"] = 1,
        ["jedna"] = 1,
        ["jedno"] = 1,
        ["dwa"] = 2,
        ["dwie"] = 2,
        ["trzy"] = 3,
        ["cztery"] = 4,
        ["pięć"] = 5,
        ["piec"] = 5,
        ["sześć"] = 6,
        ["szesc"] = 6,
        ["siedem"] = 7,
        ["osiem"] = 8,
        ["dziewięć"] = 9,
        ["dziewiec"] = 9,
        ["dziesięć"] = 10,
        ["dziesiec"] = 10,
        ["jedenaście"] = 11,
        ["jedenascie"] = 11,
        ["dwanaście"] = 12,
        ["dwanascie"] = 12,
        ["trzynaście"] = 13,
        ["trzynascie"] = 13,
        ["czternaście"] = 14,
        ["czternascie"] = 14,
        ["piętnaście"] = 15,
        ["pietnascie"] = 15,
        ["szesnaście"] = 16,
        ["szesnascie"] = 16,
        ["siedemnaście"] = 17,
        ["siedemnascie"] = 17,
        ["osiemnaście"] = 18,
        ["osiemnascie"] = 18,
        ["dziewiętnaście"] = 19,
        ["dziewietnascie"] = 19,
        ["dwadzieścia"] = 20,
        ["dwadziescia"] = 20,
        ["trzydzieści"] = 30,
        ["trzydziesci"] = 30,
        ["czterdzieści"] = 40,
        ["czterdziesci"] = 40,
        ["pięćdziesiąt"] = 50,
        ["piecdziesiat"] = 50,
        ["sześćdziesiąt"] = 60,
        ["szescdziesiat"] = 60,
        ["siedemdziesiąt"] = 70,
        ["siedemdziesiat"] = 70,
        ["osiemdziesiąt"] = 80,
        ["osiemdziesiat"] = 80,
        ["dziewięćdziesiąt"] = 90,
        ["dziewiecdziesiat"] = 90,
        ["sto"] = 100,
        ["dwieście"] = 200,
        ["dwiescie"] = 200,
        ["trzysta"] = 300,
        ["czterysta"] = 400,
        ["pięćset"] = 500,
        ["piecset"] = 500,
        ["sześćset"] = 600,
        ["szescset"] = 600,
        ["siedemset"] = 700,
        ["osiemset"] = 800,
        ["dziewięćset"] = 900,
        ["dziewiecset"] = 900,
        ["tysiąc"] = 1_000,
        ["tysiac"] = 1_000,
        ["tysiące"] = 1_000,
        ["tysiace"] = 1_000,
        ["tysięcy"] = 1_000,
        ["tysiecy"] = 1_000,
        ["milion"] = 1_000_000,
        ["miliony"] = 1_000_000,
        ["milionów"] = 1_000_000,
        ["milionow"] = 1_000_000,
        ["miliard"] = 1_000_000_000,
        ["miliardy"] = 1_000_000_000,
        ["miliardów"] = 1_000_000_000,
        ["miliardow"] = 1_000_000_000
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

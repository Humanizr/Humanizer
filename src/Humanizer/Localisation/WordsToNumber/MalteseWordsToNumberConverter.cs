namespace Humanizer;

internal class MalteseWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly FrozenDictionary<string, int> CardinalMap = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["żero"] = 0,
        ["zero"] = 0,
        ["wieħed"] = 1,
        ["wiehed"] = 1,
        ["waħda"] = 1,
        ["wahda"] = 1,
        ["tnejn"] = 2,
        ["tlieta"] = 3,
        ["tlett"] = 3,
        ["erbgħa"] = 4,
        ["erbgha"] = 4,
        ["ħames"] = 5,
        ["hames"] = 5,
        ["sitt"] = 6,
        ["sebgħa"] = 7,
        ["sebgha"] = 7,
        ["tmienja"] = 8,
        ["tminn"] = 8,
        ["disgħa"] = 9,
        ["disgha"] = 9,
        ["għaxra"] = 10,
        ["ghaxra"] = 10,
        ["ħdax"] = 11,
        ["hdax"] = 11,
        ["tnax"] = 12,
        ["tlettax"] = 13,
        ["erbatax"] = 14,
        ["ħmistax"] = 15,
        ["hmistax"] = 15,
        ["sittax"] = 16,
        ["sbatax"] = 17,
        ["tmintax"] = 18,
        ["dsatax"] = 19,
        ["għoxrin"] = 20,
        ["ghoxrin"] = 20,
        ["tletin"] = 30,
        ["erbgħin"] = 40,
        ["erbghin"] = 40,
        ["ħamsin"] = 50,
        ["hamsin"] = 50,
        ["sittin"] = 60,
        ["sebgħin"] = 70,
        ["sebghin"] = 70,
        ["disgħin"] = 90,
        ["disghin"] = 90,
        ["mija"] = 100,
        ["mitejn"] = 200,
        ["elf"] = 1_000,
        ["elfejn"] = 2_000,
        ["elef"] = 1_000,
        ["miljun"] = 1_000_000,
        ["miljuni"] = 1_000_000,
        ["biljun"] = 1_000_000_000,
        ["biljuni"] = 1_000_000_000
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

        if (normalized.EndsWith(" inqas minn żero", StringComparison.Ordinal) ||
            normalized.EndsWith(" inqas minn zero", StringComparison.Ordinal))
        {
            var positivePart = normalized[..normalized.LastIndexOf(" inqas minn ", StringComparison.Ordinal)];
            if (TryParseCardinal(positivePart, out parsedValue, out unrecognizedWord))
            {
                parsedValue = -parsedValue;
                unrecognizedWord = null;
                return true;
            }
        }

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
                .Trim()
                .ToLowerInvariant(),
            @"\s+",
            " ");

    static bool TryParseCardinal(string words, out int value, out string? unrecognizedWord)
    {
        var total = 0;
        var current = 0;
        unrecognizedWord = null;

        foreach (var token in words.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            if (token == "u")
            {
                continue;
            }

            if (!CardinalMap.TryGetValue(token, out var numeric))
            {
                value = default;
                unrecognizedWord = token;
                return false;
            }

            if (token == "mija")
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

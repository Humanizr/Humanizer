namespace Humanizer;

internal class AfrikaansWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly FrozenDictionary<string, int> CardinalMap = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["nul"] = 0,
        ["een"] = 1,
        ["twee"] = 2,
        ["drie"] = 3,
        ["vier"] = 4,
        ["vyf"] = 5,
        ["ses"] = 6,
        ["sewe"] = 7,
        ["agt"] = 8,
        ["nege"] = 9,
        ["tien"] = 10,
        ["elf"] = 11,
        ["twaalf"] = 12,
        ["dertien"] = 13,
        ["veertien"] = 14,
        ["vyftien"] = 15,
        ["sestien"] = 16,
        ["sewentien"] = 17,
        ["agtien"] = 18,
        ["negentien"] = 19,
        ["twintig"] = 20,
        ["dertig"] = 30,
        ["veertig"] = 40,
        ["vyftig"] = 50,
        ["sestig"] = 60,
        ["sewentig"] = 70,
        ["tagtig"] = 80,
        ["negentig"] = 90,
        ["honderd"] = 100,
        ["duisend"] = 1_000,
        ["miljoen"] = 1_000_000,
        ["miljard"] = 1_000_000_000
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
            if (token == "en")
            {
                continue;
            }

            if (!CardinalMap.TryGetValue(token, out var numeric))
            {
                value = default;
                unrecognizedWord = token;
                return false;
            }

            if (token == "honderd")
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

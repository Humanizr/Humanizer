namespace Humanizer;

internal class SlovakWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly FrozenDictionary<string, int> CardinalMap = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["nula"] = 0,
        ["jeden"] = 1,
        ["jedna"] = 1,
        ["jedno"] = 1,
        ["dva"] = 2,
        ["dve"] = 2,
        ["tri"] = 3,
        ["štyri"] = 4,
        ["styri"] = 4,
        ["päť"] = 5,
        ["pat"] = 5,
        ["šesť"] = 6,
        ["sest"] = 6,
        ["sedem"] = 7,
        ["osem"] = 8,
        ["deväť"] = 9,
        ["devat"] = 9,
        ["desať"] = 10,
        ["desat"] = 10,
        ["jedenásť"] = 11,
        ["jedenast"] = 11,
        ["dvanásť"] = 12,
        ["dvanast"] = 12,
        ["trinásť"] = 13,
        ["trinast"] = 13,
        ["štrnásť"] = 14,
        ["strnast"] = 14,
        ["pätnásť"] = 15,
        ["patnast"] = 15,
        ["šestnásť"] = 16,
        ["sestnast"] = 16,
        ["sedemnásť"] = 17,
        ["sedemnast"] = 17,
        ["osemnásť"] = 18,
        ["osemnast"] = 18,
        ["devätnásť"] = 19,
        ["devatnast"] = 19,
        ["dvadsať"] = 20,
        ["dvadsat"] = 20,
        ["tridsať"] = 30,
        ["tridsat"] = 30,
        ["štyridsať"] = 40,
        ["styridsat"] = 40,
        ["päťdesiat"] = 50,
        ["patdesiat"] = 50,
        ["šesťdesiat"] = 60,
        ["sestdesiat"] = 60,
        ["sedemdesiat"] = 70,
        ["osemdesiat"] = 80,
        ["deväťdesiat"] = 90,
        ["devatdesiat"] = 90,
        ["sto"] = 100,
        ["dvesto"] = 200,
        ["tristo"] = 300,
        ["štyristo"] = 400,
        ["styristo"] = 400,
        ["päťsto"] = 500,
        ["patsto"] = 500,
        ["šesťsto"] = 600,
        ["seststo"] = 600,
        ["sedemsto"] = 700,
        ["osemsto"] = 800,
        ["deväťsto"] = 900,
        ["devatsto"] = 900,
        ["tisíc"] = 1_000,
        ["tisic"] = 1_000,
        ["tisíce"] = 1_000,
        ["tisice"] = 1_000,
        ["milión"] = 1_000_000,
        ["milion"] = 1_000_000,
        ["milióny"] = 1_000_000,
        ["miliony"] = 1_000_000,
        ["miliónov"] = 1_000_000,
        ["milionov"] = 1_000_000,
        ["miliarda"] = 1_000_000_000,
        ["miliardy"] = 1_000_000_000,
        ["miliárd"] = 1_000_000_000,
        ["miliard"] = 1_000_000_000
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

        if (normalized.StartsWith("mínus ", StringComparison.Ordinal) &&
            TryParseCardinal(normalized["mínus ".Length..], out parsedValue, out unrecognizedWord))
        {
            parsedValue = -parsedValue;
            unrecognizedWord = null;
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

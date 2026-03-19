namespace Humanizer;

internal class ArabicWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly FrozenDictionary<string, int> CardinalMap = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["صفر"] = 0,
        ["واحد"] = 1,
        ["واحدة"] = 1,
        ["أحد"] = 1,
        ["احد"] = 1,
        ["اثنان"] = 2,
        ["اثنتان"] = 2,
        ["اثنين"] = 2,
        ["اثنتين"] = 2,
        ["ثلاثة"] = 3,
        ["ثلاث"] = 3,
        ["أربعة"] = 4,
        ["اربعة"] = 4,
        ["أربع"] = 4,
        ["اربع"] = 4,
        ["خمسة"] = 5,
        ["خمس"] = 5,
        ["ستة"] = 6,
        ["ست"] = 6,
        ["سبعة"] = 7,
        ["سبع"] = 7,
        ["ثمانية"] = 8,
        ["ثمان"] = 8,
        ["تسعة"] = 9,
        ["تسع"] = 9,
        ["عشرة"] = 10,
        ["عشر"] = 10,
        ["عشرون"] = 20,
        ["ثلاثون"] = 30,
        ["أربعون"] = 40,
        ["اربعون"] = 40,
        ["خمسون"] = 50,
        ["ستون"] = 60,
        ["سبعون"] = 70,
        ["ثمانون"] = 80,
        ["تسعون"] = 90,
        ["مئة"] = 100,
        ["مائة"] = 100,
        ["ألف"] = 1_000,
        ["الف"] = 1_000,
        ["آلاف"] = 1_000,
        ["الاف"] = 1_000,
        ["ألفاً"] = 1_000,
        ["ألفا"] = 1_000,
        ["مليون"] = 1_000_000,
        ["مليوناً"] = 1_000_000,
        ["ملايين"] = 1_000_000,
        ["مليار"] = 1_000_000_000,
        ["ملياراً"] = 1_000_000_000,
        ["مليارات"] = 1_000_000_000
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

        if (normalized.StartsWith("سالب ", StringComparison.Ordinal) &&
            TryParseCardinal(normalized["سالب ".Length..], out parsedValue, out unrecognizedWord))
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
                .Replace("،", string.Empty)
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
            if (token == "و")
            {
                continue;
            }

            if (!CardinalMap.TryGetValue(token, out var numeric))
            {
                value = default;
                unrecognizedWord = token;
                return false;
            }

            if (token is "مئة" or "مائة")
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

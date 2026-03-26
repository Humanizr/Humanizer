namespace Humanizer;

internal class BengaliWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly FrozenDictionary<string, int> CardinalMap = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["শূন্য"] = 0,
        ["এক"] = 1,
        ["দুই"] = 2,
        ["তিন"] = 3,
        ["চার"] = 4,
        ["পাঁচ"] = 5,
        ["ছয়"] = 6,
        ["সাত"] = 7,
        ["আট"] = 8,
        ["নয়"] = 9,
        ["দশ"] = 10,
        ["এগারো"] = 11,
        ["বারো"] = 12,
        ["বিশ"] = 20,
        ["একুশ"] = 21,
        ["বাইশ"] = 22,
        ["তেইশ"] = 23,
        ["নব্বই"] = 90,
        ["একশ"] = 100,
        ["দুইশ"] = 200,
        ["তিনশ"] = 300,
        ["চারশ"] = 400,
        ["পাঁচশ"] = 500,
        ["ছয়শ"] = 600,
        ["সাতশ"] = 700,
        ["আটশ"] = 800,
        ["এক হাজার"] = 1_000,
        ["হাজার"] = 1_000,
        ["লক্ষ"] = 100_000,
        ["কোটি"] = 10_000_000,
        ["চৌঁতিরিশ"] = 34,
        ["পঁয়তাল্লিশ"] = 45,
        ["ছাপ্পান্ন"] = 56,
        ["সাতষট্টি"] = 67,
        ["আটাত্তর"] = 78,
        ["উননব্বই"] = 99
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
        if (TryParseCardinal(normalized, out parsedValue, out unrecognizedWord))
        {
            return true;
        }

        if (normalized.StartsWith("ঋণাত্মক ", StringComparison.Ordinal) &&
            TryParseCardinal(normalized["ঋণাত্মক ".Length..], out parsedValue, out unrecognizedWord))
        {
            parsedValue = -parsedValue;
            unrecognizedWord = null;
            return true;
        }

        parsedValue = default;
        return false;
    }

    static bool TryParseCardinal(string words, out int value, out string? unrecognizedWord)
    {
        if (CardinalMap.TryGetValue(words, out value))
        {
            unrecognizedWord = null;
            return true;
        }

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

            if (token is "হাজার" or "লক্ষ" or "কোটি")
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

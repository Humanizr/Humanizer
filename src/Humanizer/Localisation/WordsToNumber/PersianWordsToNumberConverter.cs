namespace Humanizer;

internal class PersianWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly FrozenDictionary<string, long> CardinalMap = new Dictionary<string, long>(StringComparer.Ordinal)
    {
        ["صفر"] = 0,
        ["یک"] = 1,
        ["دو"] = 2,
        ["سه"] = 3,
        ["چهار"] = 4,
        ["پنج"] = 5,
        ["شش"] = 6,
        ["هفت"] = 7,
        ["هشت"] = 8,
        ["نه"] = 9,
        ["ده"] = 10,
        ["یازده"] = 11,
        ["دوازده"] = 12,
        ["سیزده"] = 13,
        ["چهارده"] = 14,
        ["پانزده"] = 15,
        ["شانزده"] = 16,
        ["هفده"] = 17,
        ["هجده"] = 18,
        ["نوزده"] = 19,
        ["بیست"] = 20,
        ["سی"] = 30,
        ["چهل"] = 40,
        ["پنجاه"] = 50,
        ["شصت"] = 60,
        ["هفتاد"] = 70,
        ["هشتاد"] = 80,
        ["نود"] = 90,
        ["صد"] = 100,
        ["دویست"] = 200,
        ["سیصد"] = 300,
        ["چهارصد"] = 400,
        ["پانصد"] = 500,
        ["ششصد"] = 600,
        ["هفتصد"] = 700,
        ["هشتصد"] = 800,
        ["نهصد"] = 900,
        ["هزار"] = 1_000,
        ["میلیون"] = 1_000_000,
        ["میلیارد"] = 1_000_000_000,
        ["بیلیون"] = 1_000_000_000_000,
        ["بیلیارد"] = 1_000_000_000_000_000,
        ["تریلیون"] = 1_000_000_000_000_000_000
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
        var negative = false;

        if (normalized.StartsWith("منفی ", StringComparison.Ordinal))
        {
            negative = true;
            normalized = normalized["منفی ".Length..];
        }

        if (!TryParseCardinal(normalized, out var parsedLong, out unrecognizedWord))
        {
            parsedValue = default;
            return false;
        }

        if (negative)
        {
            parsedLong = -parsedLong;
        }

        if (parsedLong is < int.MinValue or > int.MaxValue)
        {
            parsedValue = default;
            unrecognizedWord ??= normalized;
            return false;
        }

        parsedValue = (int)parsedLong;
        unrecognizedWord = null;
        return true;
    }

    static bool TryParseCardinal(string words, out long value, out string? unrecognizedWord)
    {
        var total = 0L;
        var current = 0L;
        unrecognizedWord = null;

        foreach (var tokenSpan in WordsToNumberTokenizer.Enumerate(words))
        {
            var token = tokenSpan.ToString();

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

            if (numeric >= 1000)
            {
                total += (current == 0 ? 1 : current) * numeric;
                current = 0;
            }
            else
            {
                current += numeric;
            }
        }

        value = total + current;
        return true;
    }

    static string Normalize(string words) =>
        Regex.Replace(words.Replace(",", string.Empty)
                .Replace(".", string.Empty)
                .Replace("،", string.Empty)
                .Replace("\u200c", " ")
                .Replace('ي', 'ی')
                .Replace('ك', 'ک')
                .Trim(),
            @"\s+",
            " ");
}

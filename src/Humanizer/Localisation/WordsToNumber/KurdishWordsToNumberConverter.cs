namespace Humanizer;

internal class KurdishWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly FrozenDictionary<string, int> CardinalMap = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["یەک"] = 1,
        ["دوو"] = 2,
        ["سێ"] = 3,
        ["چوار"] = 4,
        ["پێنج"] = 5,
        ["شەش"] = 6,
        ["حەوت"] = 7,
        ["هەشت"] = 8,
        ["نۆ"] = 9,
        ["دە"] = 10,
        ["یازدە"] = 11,
        ["دوازدە"] = 12,
        ["بیست"] = 20,
        ["سی"] = 30,
        ["چل"] = 40,
        ["پەنجا"] = 50,
        ["شەست"] = 60,
        ["حەفتا"] = 70,
        ["هەشتا"] = 80,
        ["نەوەد"] = 90,
        ["سەد"] = 100,
        ["هەزار"] = 1_000,
        ["میلیۆن"] = 1_000_000,
        ["میلیارد"] = 1_000_000_000
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

        if (normalized.StartsWith("نێگەتیڤ ", StringComparison.Ordinal) &&
            TryParseCardinal(normalized["نێگەتیڤ ".Length..], out parsedValue, out unrecognizedWord))
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

            if (token == "سەد")
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

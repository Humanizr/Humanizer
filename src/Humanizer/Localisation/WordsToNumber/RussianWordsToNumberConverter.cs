namespace Humanizer;

internal class RussianWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly FrozenDictionary<string, int> CardinalMap = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["ноль"] = 0,
        ["один"] = 1,
        ["одна"] = 1,
        ["одно"] = 1,
        ["два"] = 2,
        ["две"] = 2,
        ["три"] = 3,
        ["четыре"] = 4,
        ["пять"] = 5,
        ["шесть"] = 6,
        ["семь"] = 7,
        ["восемь"] = 8,
        ["девять"] = 9,
        ["десять"] = 10,
        ["одиннадцать"] = 11,
        ["двенадцать"] = 12,
        ["тринадцать"] = 13,
        ["четырнадцать"] = 14,
        ["пятнадцать"] = 15,
        ["шестнадцать"] = 16,
        ["семнадцать"] = 17,
        ["восемнадцать"] = 18,
        ["девятнадцать"] = 19,
        ["двадцать"] = 20,
        ["тридцать"] = 30,
        ["сорок"] = 40,
        ["пятьдесят"] = 50,
        ["шестьдесят"] = 60,
        ["семьдесят"] = 70,
        ["восемьдесят"] = 80,
        ["девяносто"] = 90,
        ["сто"] = 100,
        ["двести"] = 200,
        ["триста"] = 300,
        ["четыреста"] = 400,
        ["пятьсот"] = 500,
        ["шестьсот"] = 600,
        ["семьсот"] = 700,
        ["восемьсот"] = 800,
        ["девятьсот"] = 900,
        ["тысяча"] = 1_000,
        ["тысячи"] = 1_000,
        ["тысяч"] = 1_000,
        ["миллион"] = 1_000_000,
        ["миллиона"] = 1_000_000,
        ["миллионов"] = 1_000_000,
        ["миллиард"] = 1_000_000_000,
        ["миллиарда"] = 1_000_000_000,
        ["миллиардов"] = 1_000_000_000
    }.ToFrozenDictionary(StringComparer.Ordinal);

    static readonly FrozenDictionary<string, int> OrdinalMap = BuildOrdinalMap();

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

        if (normalized.StartsWith("минус ", StringComparison.Ordinal))
        {
            negative = true;
            normalized = normalized["минус ".Length..].Trim();
        }

        if (OrdinalMap.TryGetValue(normalized, out parsedValue) ||
            TryParseCardinal(normalized, out parsedValue, out unrecognizedWord))
        {
            if (negative)
            {
                parsedValue = -parsedValue;
            }

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

    static FrozenDictionary<string, int> BuildOrdinalMap()
    {
        var converter = new RussianNumberToWordsConverter();
        var ordinals = new Dictionary<string, int>(StringComparer.Ordinal);

        for (var number = 1; number <= 200; number++)
        {
            ordinals[Normalize(converter.ConvertToOrdinal(number))] = number;
            ordinals[Normalize(converter.ConvertToOrdinal(number, GrammaticalGender.Feminine))] = number;
            ordinals[Normalize(converter.ConvertToOrdinal(number, GrammaticalGender.Neuter))] = number;
        }

        return ordinals.ToFrozenDictionary(StringComparer.Ordinal);
    }
}

namespace Humanizer;

internal class UkrainianWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly FrozenDictionary<string, int> CardinalMap = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["нуль"] = 0,
        ["один"] = 1,
        ["одна"] = 1,
        ["одне"] = 1,
        ["два"] = 2,
        ["дві"] = 2,
        ["три"] = 3,
        ["чотири"] = 4,
        ["п'ять"] = 5,
        ["пять"] = 5,
        ["шість"] = 6,
        ["сім"] = 7,
        ["вісім"] = 8,
        ["дев'ять"] = 9,
        ["девять"] = 9,
        ["десять"] = 10,
        ["одинадцять"] = 11,
        ["дванадцять"] = 12,
        ["тринадцять"] = 13,
        ["чотирнадцять"] = 14,
        ["п'ятнадцять"] = 15,
        ["пятнадцять"] = 15,
        ["шістнадцять"] = 16,
        ["сімнадцять"] = 17,
        ["вісімнадцять"] = 18,
        ["дев'ятнадцять"] = 19,
        ["девятнадцять"] = 19,
        ["двадцять"] = 20,
        ["тридцять"] = 30,
        ["сорок"] = 40,
        ["п'ятдесят"] = 50,
        ["пятдесят"] = 50,
        ["шістдесят"] = 60,
        ["сімдесят"] = 70,
        ["вісімдесят"] = 80,
        ["дев'яносто"] = 90,
        ["девяносто"] = 90,
        ["сто"] = 100,
        ["двісті"] = 200,
        ["триста"] = 300,
        ["чотириста"] = 400,
        ["п'ятсот"] = 500,
        ["пятсот"] = 500,
        ["шістсот"] = 600,
        ["сімсот"] = 700,
        ["вісімсот"] = 800,
        ["дев'ятсот"] = 900,
        ["девятсот"] = 900,
        ["тисяча"] = 1_000,
        ["тисячі"] = 1_000,
        ["тисяч"] = 1_000,
        ["мільйон"] = 1_000_000,
        ["мільйона"] = 1_000_000,
        ["мільйонів"] = 1_000_000,
        ["мільярд"] = 1_000_000_000,
        ["мільярда"] = 1_000_000_000,
        ["мільярдів"] = 1_000_000_000
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

        if (OrdinalMap.TryGetValue(normalized, out parsedValue) ||
            TryParseCardinal(normalized, out parsedValue, out unrecognizedWord))
        {
            unrecognizedWord = null;
            return true;
        }

        if ((normalized.StartsWith("мінус ", StringComparison.Ordinal) || normalized.StartsWith("минус ", StringComparison.Ordinal)) &&
            TryParseCardinal(normalized[6..], out parsedValue, out unrecognizedWord))
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

    static FrozenDictionary<string, int> BuildOrdinalMap()
    {
        var converter = new UkrainianNumberToWordsConverter();
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

namespace Humanizer;

internal class SerbianCyrillicWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly FrozenDictionary<string, int> CardinalMap = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["нула"] = 0,
        ["један"] = 1,
        ["једна"] = 1,
        ["једно"] = 1,
        ["два"] = 2,
        ["две"] = 2,
        ["три"] = 3,
        ["четири"] = 4,
        ["пет"] = 5,
        ["шест"] = 6,
        ["седам"] = 7,
        ["осам"] = 8,
        ["девет"] = 9,
        ["десет"] = 10,
        ["једанаест"] = 11,
        ["дванаест"] = 12,
        ["тринаест"] = 13,
        ["четрнаест"] = 14,
        ["петнаест"] = 15,
        ["шеснаест"] = 16,
        ["седамнаест"] = 17,
        ["осамнаест"] = 18,
        ["деветнаест"] = 19,
        ["двадесет"] = 20,
        ["тридесет"] = 30,
        ["четрдесет"] = 40,
        ["педесет"] = 50,
        ["шестдесет"] = 60,
        ["седамдесет"] = 70,
        ["осамдесет"] = 80,
        ["деветдесет"] = 90,
        ["сто"] = 100,
        ["двесто"] = 200,
        ["тристо"] = 300,
        ["четиристо"] = 400,
        ["петсто"] = 500,
        ["шестсто"] = 600,
        ["седамсто"] = 700,
        ["осамсто"] = 800,
        ["деветсто"] = 900,
        ["хиљаду"] = 1_000,
        ["хиљада"] = 1_000,
        ["милион"] = 1_000_000,
        ["милиона"] = 1_000_000,
        ["милијарда"] = 1_000_000_000,
        ["милијарде"] = 1_000_000_000
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

        if (normalized.StartsWith("минус ", StringComparison.Ordinal) &&
            TryParseCardinal(normalized["минус ".Length..], out parsedValue, out unrecognizedWord))
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

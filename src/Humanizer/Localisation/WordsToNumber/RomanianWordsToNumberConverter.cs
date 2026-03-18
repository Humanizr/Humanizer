namespace Humanizer;

internal class RomanianWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly FrozenDictionary<string, int> CardinalMap = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["zero"] = 0,
        ["un"] = 1,
        ["unu"] = 1,
        ["o"] = 1,
        ["una"] = 1,
        ["doi"] = 2,
        ["doua"] = 2,
        ["trei"] = 3,
        ["patru"] = 4,
        ["cinci"] = 5,
        ["sase"] = 6,
        ["sapte"] = 7,
        ["opt"] = 8,
        ["noua"] = 9,
        ["zece"] = 10,
        ["unsprezece"] = 11,
        ["doisprezece"] = 12,
        ["douasprezece"] = 12,
        ["treisprezece"] = 13,
        ["paisprezece"] = 14,
        ["cincisprezece"] = 15,
        ["saisprezece"] = 16,
        ["saptesprezece"] = 17,
        ["optsprezece"] = 18,
        ["nouasprezece"] = 19,
        ["douazeci"] = 20,
        ["treizeci"] = 30,
        ["patruzeci"] = 40,
        ["cincizeci"] = 50,
        ["saizeci"] = 60,
        ["saptezeci"] = 70,
        ["optzeci"] = 80,
        ["nouazeci"] = 90,
        ["suta"] = 100,
        ["sute"] = 100,
        ["mie"] = 1000,
        ["mii"] = 1000,
        ["milion"] = 1_000_000,
        ["milioane"] = 1_000_000,
        ["miliard"] = 1_000_000_000,
        ["miliarde"] = 1_000_000_000
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

        if (normalized.StartsWith("minus ", StringComparison.Ordinal))
        {
            negative = true;
            normalized = normalized["minus ".Length..].Trim();
        }

        if (int.TryParse(normalized, NumberStyles.Integer, CultureInfo.InvariantCulture, out parsedValue) ||
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

        foreach (var token in words.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            if (token is "si" or "de")
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
            else if (numeric == 100)
            {
                current = (current == 0 ? 1 : current) * numeric;
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
        Regex.Replace(RemoveDiacritics(words)
                .Replace(",", string.Empty)
                .Replace(".", string.Empty)
                .Replace("-", " ")
                .ToLowerInvariant()
                .Trim(),
            @"\s+",
            " ");

    static string RemoveDiacritics(string text)
    {
        var normalized = text.Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder(text.Length);

        foreach (var ch in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
            {
                builder.Append(ch);
            }
        }

        return builder.ToString().Normalize(NormalizationForm.FormC);
    }
}

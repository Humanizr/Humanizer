namespace Humanizer;

internal class HungarianWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly FrozenDictionary<string, int> CardinalMap = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["nulla"] = 0,
        ["egy"] = 1,
        ["ket"] = 2,
        ["ketto"] = 2,
        ["harom"] = 3,
        ["negy"] = 4,
        ["ot"] = 5,
        ["hat"] = 6,
        ["het"] = 7,
        ["nyolc"] = 8,
        ["kilenc"] = 9,
        ["tiz"] = 10,
        ["husz"] = 20,
        ["harminc"] = 30,
        ["negyven"] = 40,
        ["otven"] = 50,
        ["hatvan"] = 60,
        ["hetven"] = 70,
        ["nyolcvan"] = 80,
        ["kilencven"] = 90,
        ["szaz"] = 100,
        ["ezer"] = 1000,
        ["millio"] = 1_000_000,
        ["milliard"] = 1_000_000_000
    }.ToFrozenDictionary(StringComparer.Ordinal);

    static readonly FrozenDictionary<string, int> TensMap = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["harminc"] = 30,
        ["negyven"] = 40,
        ["otven"] = 50,
        ["hatvan"] = 60,
        ["hetven"] = 70,
        ["nyolcvan"] = 80,
        ["kilencven"] = 90
    }.ToFrozenDictionary(StringComparer.Ordinal);

    static readonly (string Token, int Value)[] Scales =
    [
        ("milliard", 1_000_000_000),
        ("millio", 1_000_000),
        ("ezer", 1000),
        ("szaz", 100)
    ];

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

        if (normalized.StartsWith("minusz ", StringComparison.Ordinal))
        {
            negative = true;
            normalized = normalized["minusz ".Length..];
        }
        else if (normalized.StartsWith("minus ", StringComparison.Ordinal))
        {
            negative = true;
            normalized = normalized["minus ".Length..];
        }

        normalized = CollapseCompoundSeparators(normalized);

        if (int.TryParse(normalized, NumberStyles.Integer, CultureInfo.InvariantCulture, out parsedValue) ||
            TryParseCardinal(normalized, out parsedValue))
        {
            if (negative)
            {
                parsedValue = -parsedValue;
            }

            unrecognizedWord = null;
            return true;
        }

        unrecognizedWord = normalized;
        parsedValue = default;
        return false;
    }

    static bool TryParseCardinal(string word, out int value)
    {
        if (string.IsNullOrEmpty(word))
        {
            value = default;
            return false;
        }

        if (CardinalMap.TryGetValue(word, out value))
        {
            return true;
        }

        foreach (var (token, scaleValue) in Scales)
        {
            var index = word.IndexOf(token, StringComparison.Ordinal);
            if (index < 0)
            {
                continue;
            }

            var left = word[..index];
            var right = word[(index + token.Length)..];
            var factor = 1;

            if (!string.IsNullOrEmpty(left) && !TryParseCardinal(left, out factor))
            {
                continue;
            }

            if (!TryParseOptional(right, out var remainder))
            {
                continue;
            }

            value = checked(factor * scaleValue + remainder);
            return true;
        }

        if (word.StartsWith("tizen", StringComparison.Ordinal) &&
            TryParseCardinal(word["tizen".Length..], out var teenUnit) &&
            teenUnit is >= 1 and <= 9)
        {
            value = 10 + teenUnit;
            return true;
        }

        if (word.StartsWith("huszon", StringComparison.Ordinal) &&
            TryParseCardinal(word["huszon".Length..], out var twentyUnit) &&
            twentyUnit is >= 1 and <= 9)
        {
            value = 20 + twentyUnit;
            return true;
        }

        foreach (var (tensToken, tensValue) in TensMap)
        {
            if (!word.StartsWith(tensToken, StringComparison.Ordinal))
            {
                continue;
            }

            var remainder = word[tensToken.Length..];
            if (string.IsNullOrEmpty(remainder))
            {
                value = tensValue;
                return true;
            }

            if (TryParseCardinal(remainder, out var unitValue) && unitValue is >= 1 and <= 9)
            {
                value = tensValue + unitValue;
                return true;
            }
        }

        value = default;
        return false;
    }

    static bool TryParseOptional(string word, out int value)
    {
        if (string.IsNullOrEmpty(word))
        {
            value = 0;
            return true;
        }

        return TryParseCardinal(word, out value);
    }

    static string Normalize(string words) =>
        Regex.Replace(RemoveDiacritics(words)
                .Replace(",", string.Empty)
                .Replace(".", string.Empty)
                .Replace("’", string.Empty)
                .Replace("'", string.Empty)
                .ToLowerInvariant()
                .Trim(),
            @"\s+",
            " ");

    static string CollapseCompoundSeparators(string words) =>
        words.Replace("-", string.Empty)
            .Replace(" ", string.Empty);

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

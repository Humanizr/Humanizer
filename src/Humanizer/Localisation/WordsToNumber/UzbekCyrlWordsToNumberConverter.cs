namespace Humanizer;

internal class UzbekCyrlWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly FrozenDictionary<string, int> CardinalMap = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["нол"] = 0,
        ["бир"] = 1,
        ["иккинчи"] = 2,
        ["икки"] = 2,
        ["уч"] = 3,
        ["тўрт"] = 4,
        ["беш"] = 5,
        ["олти"] = 6,
        ["етти"] = 7,
        ["саккиз"] = 8,
        ["тўққиз"] = 9,
        ["ўн"] = 10,
        ["йигирма"] = 20,
        ["ўттиз"] = 30,
        ["қирқ"] = 40,
        ["эллик"] = 50,
        ["олтмиш"] = 60,
        ["етмиш"] = 70,
        ["саксон"] = 80,
        ["тўқсон"] = 90,
        ["юз"] = 100,
        ["минг"] = 1000,
        ["миллион"] = 1_000_000,
        ["миллиард"] = 1_000_000_000
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
        var negative = false;

        if (normalized.StartsWith("минус ", StringComparison.Ordinal))
        {
            negative = true;
            normalized = normalized["минус ".Length..];
        }

        if (TryParseCardinal(normalized, out parsedValue))
        {
            if (negative)
            {
                parsedValue = -parsedValue;
            }

            unrecognizedWord = null;
            return true;
        }

        unrecognizedWord = WordsToNumberTokenizer.GetLastTokenOrSelf(normalized);
        parsedValue = default;
        return false;
    }

    static bool TryParseCardinal(string words, out int value)
    {
        if (CardinalMap.TryGetValue(words, out value))
        {
            return true;
        }

        var total = 0;
        var current = 0;

        foreach (var tokenSpan in WordsToNumberTokenizer.Enumerate(words))
        {
            var token = tokenSpan.ToString();
            var normalizedToken = token;
            if (!CardinalMap.TryGetValue(normalizedToken, out var tokenValue))
            {
                if (normalizedToken.EndsWith("инчи", StringComparison.Ordinal))
                {
                    normalizedToken = normalizedToken[..^"инчи".Length];
                }
                else if (normalizedToken.EndsWith("нчи", StringComparison.Ordinal))
                {
                    normalizedToken = normalizedToken[..^"нчи".Length];
                }

                if (!CardinalMap.TryGetValue(normalizedToken, out tokenValue))
                {
                    value = default;
                    return false;
                }
            }

            if (tokenValue >= 1000)
            {
                total += (current == 0 ? 1 : current) * tokenValue;
                current = 0;
            }
            else if (tokenValue == 100)
            {
                current = (current == 0 ? 1 : current) * tokenValue;
            }
            else
            {
                current += tokenValue;
            }
        }

        value = total + current;
        return true;
    }
}

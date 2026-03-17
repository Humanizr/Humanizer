namespace Humanizer;

internal class VietnameseWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly FrozenDictionary<string, int> Cardinals = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["không"] = 0,
        ["một"] = 1,
        ["mốt"] = 1,
        ["hai"] = 2,
        ["ba"] = 3,
        ["bốn"] = 4,
        ["tư"] = 4,
        ["năm"] = 5,
        ["lăm"] = 5,
        ["sáu"] = 6,
        ["bảy"] = 7,
        ["tám"] = 8,
        ["chín"] = 9,
        ["mười"] = 10,
        ["mươi"] = 10,
        ["trăm"] = 100,
        ["nghìn"] = 1000,
        ["triệu"] = 1_000_000,
        ["tỉ"] = 1_000_000_000
    }.ToFrozenDictionary(StringComparer.Ordinal);

    static readonly FrozenDictionary<string, int> Ordinals = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["nhất"] = 1,
        ["nhì"] = 2,
        ["tư"] = 4
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

        if (normalized.StartsWith("trừ ", StringComparison.Ordinal))
        {
            negative = true;
            normalized = normalized["trừ ".Length..];
        }

        if (normalized.StartsWith("thứ ", StringComparison.Ordinal))
        {
            normalized = normalized["thứ ".Length..];
        }

        if (Ordinals.TryGetValue(normalized, out parsedValue) ||
            TryParseCardinal(normalized, out parsedValue))
        {
            if (negative)
            {
                parsedValue = -parsedValue;
            }

            unrecognizedWord = null;
            return true;
        }

        unrecognizedWord = normalized.Split(' ', StringSplitOptions.RemoveEmptyEntries).LastOrDefault() ?? normalized;
        parsedValue = default;
        return false;
    }

    static bool TryParseCardinal(string words, out int value)
    {
        if (Cardinals.TryGetValue(words, out value))
        {
            return true;
        }

        var total = 0;
        var current = 0;

        foreach (var token in words.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            if (token == "linh")
            {
                continue;
            }

            if (!Cardinals.TryGetValue(token, out var tokenValue))
            {
                value = default;
                return false;
            }

            if (tokenValue >= 1000)
            {
                total += (current == 0 ? 1 : current) * tokenValue;
                current = 0;
            }
            else if (tokenValue == 100 || tokenValue == 10 && token == "mươi")
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

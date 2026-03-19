namespace Humanizer;

internal class ThaiWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly (string Token, int Value)[] DigitTokens =
    [
        ("หนึ่ง", 1),
        ("เอ็ด", 1),
        ("ยี่", 2),
        ("สอง", 2),
        ("สาม", 3),
        ("สี่", 4),
        ("ห้า", 5),
        ("หก", 6),
        ("เจ็ด", 7),
        ("แปด", 8),
        ("เก้า", 9)
    ];

    static readonly (string Token, int Value)[] ScaleTokens =
    [
        ("แสน", 100000),
        ("หมื่น", 10000),
        ("พัน", 1000),
        ("ร้อย", 100),
        ("สิบ", 10),
        ("ล้าน", 1000000)
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

        var normalized = words.Trim();
        if (normalized.StartsWith("ลบ", StringComparison.Ordinal) &&
            TryParseCardinal(normalized["ลบ".Length..], out parsedValue, out unrecognizedWord))
        {
            parsedValue = -parsedValue;
            unrecognizedWord = null;
            return true;
        }

        return TryParseCardinal(normalized, out parsedValue, out unrecognizedWord);
    }

    static bool TryParseCardinal(string words, out int value, out string? unrecognizedWord)
    {
        var tokens = new List<string>();
        var remaining = words;

        while (remaining.Length > 0)
        {
            var matched = false;

            foreach (var (token, _) in ScaleTokens)
            {
                if (remaining.StartsWith(token, StringComparison.Ordinal))
                {
                    tokens.Add(token);
                    remaining = remaining[token.Length..];
                    matched = true;
                    break;
                }
            }

            if (matched)
            {
                continue;
            }

            foreach (var (token, _) in DigitTokens)
            {
                if (remaining.StartsWith(token, StringComparison.Ordinal))
                {
                    tokens.Add(token);
                    remaining = remaining[token.Length..];
                    matched = true;
                    break;
                }
            }

            if (!matched)
            {
                value = default;
                unrecognizedWord = remaining;
                return false;
            }
        }

        var total = 0;
        var group = 0;
        var pendingDigit = 0;

        foreach (var token in tokens)
        {
            var scale = ScaleTokens.FirstOrDefault(x => x.Token == token);
            if (!string.IsNullOrEmpty(scale.Token))
            {
                if (scale.Value == 1000000)
                {
                    group += pendingDigit;
                    total += (group == 0 ? 1 : group) * scale.Value;
                    group = 0;
                    pendingDigit = 0;
                }
                else
                {
                    group += (pendingDigit == 0 ? 1 : pendingDigit) * scale.Value;
                    pendingDigit = 0;
                }

                continue;
            }

            pendingDigit = DigitTokens.First(x => x.Token == token).Value;
        }

        value = total + group + pendingDigit;
        unrecognizedWord = null;
        return true;
    }
}

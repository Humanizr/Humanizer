namespace Humanizer;

internal class KoreanWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly FrozenDictionary<char, int> Digits = new Dictionary<char, int>
    {
        ['영'] = 0, ['일'] = 1, ['이'] = 2, ['삼'] = 3, ['사'] = 4, ['오'] = 5, ['육'] = 6, ['칠'] = 7, ['팔'] = 8, ['구'] = 9
    }.ToFrozenDictionary();

    static readonly FrozenDictionary<char, int> SmallUnits = new Dictionary<char, int>
    {
        ['십'] = 10, ['백'] = 100, ['천'] = 1000
    }.ToFrozenDictionary();

    static readonly FrozenDictionary<char, int> LargeUnits = new Dictionary<char, int>
    {
        ['만'] = 10_000, ['억'] = 100_000_000
    }.ToFrozenDictionary();

    static readonly FrozenDictionary<string, int> Ordinals = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["첫번째"] = 1,
        ["두번째"] = 2,
        ["세번째"] = 3
    }.ToFrozenDictionary();

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

        var normalized = words.Replace(" ", string.Empty).Trim();

        if (normalized.StartsWith("마이너스", StringComparison.Ordinal))
        {
            normalized = normalized["마이너스".Length..];
            if (TryParseKorean(normalized, out parsedValue))
            {
                parsedValue = -parsedValue;
                unrecognizedWord = null;
                return true;
            }
        }
        else
        {
            if (Ordinals.TryGetValue(normalized, out parsedValue))
            {
                unrecognizedWord = null;
                return true;
            }

            if (normalized.EndsWith("번째", StringComparison.Ordinal))
            {
                normalized = normalized[..^"번째".Length];
            }

            if (TryParseKorean(normalized, out parsedValue))
            {
                unrecognizedWord = null;
                return true;
            }
        }

        unrecognizedWord = normalized.FirstOrDefault().ToString();
        parsedValue = default;
        return false;
    }

    static bool TryParseKorean(string text, out int value)
    {
        var total = 0;
        var section = 0;
        var number = 0;

        foreach (var ch in text)
        {
            if (Digits.TryGetValue(ch, out var digit))
            {
                number = digit;
                continue;
            }

            if (SmallUnits.TryGetValue(ch, out var smallUnit))
            {
                if (number == 0)
                {
                    number = 1;
                }

                section += number * smallUnit;
                number = 0;
                continue;
            }

            if (LargeUnits.TryGetValue(ch, out var largeUnit))
            {
                section += number;
                if (section == 0)
                {
                    section = 1;
                }

                total += section * largeUnit;
                section = 0;
                number = 0;
                continue;
            }

            value = default;
            return false;
        }

        value = total + section + number;
        return true;
    }
}

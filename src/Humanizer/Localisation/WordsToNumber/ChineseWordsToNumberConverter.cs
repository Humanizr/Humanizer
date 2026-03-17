namespace Humanizer;

internal class ChineseWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly FrozenDictionary<char, int> Digits = new Dictionary<char, int>
    {
        ['零'] = 0,
        ['一'] = 1,
        ['二'] = 2,
        ['三'] = 3,
        ['四'] = 4,
        ['五'] = 5,
        ['六'] = 6,
        ['七'] = 7,
        ['八'] = 8,
        ['九'] = 9
    }.ToFrozenDictionary();

    static readonly FrozenDictionary<char, int> SmallUnits = new Dictionary<char, int>
    {
        ['十'] = 10,
        ['百'] = 100,
        ['千'] = 1000
    }.ToFrozenDictionary();

    static readonly FrozenDictionary<char, int> LargeUnits = new Dictionary<char, int>
    {
        ['万'] = 10_000,
        ['亿'] = 100_000_000
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
        var negative = false;

        if (normalized.StartsWith('负'))
        {
            negative = true;
            normalized = normalized[1..];
        }

        if (normalized.StartsWith('第'))
        {
            normalized = normalized[1..];
        }

        if (TryParseChinese(normalized, out parsedValue))
        {
            if (negative)
            {
                parsedValue = -parsedValue;
            }

            unrecognizedWord = null;
            return true;
        }

        unrecognizedWord = normalized.FirstOrDefault().ToString();
        parsedValue = default;
        return false;
    }

    static bool TryParseChinese(string text, out int value)
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

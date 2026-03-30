using System.Collections.Generic;

namespace Humanizer;

class MalayFamilyNumberToWordsConverter(MalayNumberToWordsProfile profile) : GenderlessNumberToWordsConverter
{
    readonly MalayNumberToWordsProfile profile = profile;

    public override string Convert(long number)
    {
        if (number == 0)
        {
            return profile.ZeroWord;
        }

        if (number < 0)
        {
            return $"{profile.MinusWord} {Convert(-number)}";
        }

        return ConvertPositive(number);
    }

    string ConvertPositive(long number)
    {
        var parts = new List<string>();

        foreach (var scale in profile.Scales)
        {
            if (number < scale.Value)
            {
                continue;
            }

            var part = number / scale.Value;
            number %= scale.Value;

            if (part == 0)
            {
                continue;
            }

            if (scale.Value == 1_000 && part == 1)
            {
                parts.Add(profile.ThousandOneWord);
            }
            else
            {
                parts.Add($"{ConvertUnderThousand(part)} {scale.Name}");
            }
        }

        if (number > 0)
        {
            parts.Add(ConvertUnderThousand(number));
        }

        return string.Join(" ", parts);
    }

    protected virtual string GetHundredsWord(long count) =>
        count == 1 ? profile.HundredWord : $"{profile.Units[count]} {profile.HundredUnitWord}";

    string ConvertUnderThousand(long number)
    {
        if (number < 100)
        {
            return ConvertUnderHundred(number);
        }

        var hundreds = number / 100;
        var remainder = number % 100;
        var parts = new List<string> { GetHundredsWord(hundreds) };

        if (remainder > 0)
        {
            parts.Add(ConvertUnderHundred(remainder));
        }

        return string.Join(" ", parts);
    }

    string ConvertUnderHundred(long number)
    {
        if (number < profile.Units.Length)
        {
            return profile.Units[number];
        }

        var tens = (int)(number / 10);
        var remainder = number % 10;
        var tensWord = profile.Tens[tens];

        return remainder == 0
            ? tensWord
            : $"{tensWord} {profile.Units[remainder]}";
    }

    public override string ConvertToOrdinal(int number) => Convert(number);

    internal readonly struct Scale
    {
        public long Value { get; }
        public string Name { get; }

        public Scale(long value, string name)
        {
            Value = value;
            Name = name;
        }
    }
}

sealed class MalayNumberToWordsProfile(
    string zeroWord,
    string minusWord,
    string thousandOneWord,
    string hundredWord,
    string hundredUnitWord,
    string[] units,
    string[] tens,
    MalayFamilyNumberToWordsConverter.Scale[] scales)
{
    public string ZeroWord { get; } = zeroWord;
    public string MinusWord { get; } = minusWord;
    public string ThousandOneWord { get; } = thousandOneWord;
    public string HundredWord { get; } = hundredWord;
    public string HundredUnitWord { get; } = hundredUnitWord;
    public string[] Units { get; } = units;
    public string[] Tens { get; } = tens;
    public MalayFamilyNumberToWordsConverter.Scale[] Scales { get; } = scales;
}

using System.Collections.Generic;

namespace Humanizer;

abstract class MalayNumberToWordsConverterBase : GenderlessNumberToWordsConverter
{
    protected abstract string ZeroWord { get; }
    protected abstract string MinusWord { get; }
    protected abstract string ThousandOneWord { get; }
    protected abstract string[] Units { get; }
    protected abstract string[] Tens { get; }
    protected abstract Scale[] Scales { get; }

    public override string Convert(long number)
    {
        if (number == 0)
        {
            return ZeroWord;
        }

        if (number < 0)
        {
            return $"{MinusWord} {Convert(-number)}";
        }

        return ConvertPositive(number);
    }

    string ConvertPositive(long number)
    {
        var parts = new List<string>();

        foreach (var scale in Scales)
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
                parts.Add(ThousandOneWord);
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
        count == 1 ? "seratus" : $"{Units[count]} ratus";

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
        if (number < Units.Length)
        {
            return Units[number];
        }

        var tens = (int)(number / 10);
        var remainder = number % 10;
        var tensWord = Tens[tens];

        return remainder == 0
            ? tensWord
            : $"{tensWord} {Units[remainder]}";
    }

    public override string ConvertToOrdinal(int number) => Convert(number);

    protected readonly struct Scale
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

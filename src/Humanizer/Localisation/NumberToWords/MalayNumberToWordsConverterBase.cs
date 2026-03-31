using System.Collections.Generic;

namespace Humanizer;

// Shared Malay-family engine. Scale-level one-word overrides are generated so the converter does not hardcode thousand-specific behavior.
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

            // Some Malay-family locales contract "one + scale" into a dedicated word such as "seribu".
            if (part == 1 && scale.OneWord is not null)
            {
                parts.Add(scale.OneWord);
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

    // Hundreds remain shared; only the singular one-word form and scale words vary by generated data.
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
        public string? OneWord { get; }

        public Scale(long value, string name, string? oneWord = null)
        {
            Value = value;
            Name = name;
            OneWord = oneWord;
        }
    }
}

// Holds the generated Malay-family lexicon and scale metadata.
sealed class MalayNumberToWordsProfile(
    string zeroWord,
    string minusWord,
    string hundredWord,
    string hundredUnitWord,
    string[] units,
    string[] tens,
    MalayFamilyNumberToWordsConverter.Scale[] scales)
{
    public string ZeroWord { get; } = zeroWord;
    public string MinusWord { get; } = minusWord;
    public string HundredWord { get; } = hundredWord;
    public string HundredUnitWord { get; } = hundredUnitWord;
    public string[] Units { get; } = units;
    public string[] Tens { get; } = tens;
    public MalayFamilyNumberToWordsConverter.Scale[] Scales { get; } = scales;
}

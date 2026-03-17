using System.Collections.Generic;

namespace Humanizer;

class DanishNumberToWordsConverter : GenderlessNumberToWordsConverter
{
    static readonly string[] UnitsMap =
    [
        "nul",
        "en",
        "to",
        "tre",
        "fire",
        "fem",
        "seks",
        "syv",
        "otte",
        "ni",
        "ti",
        "elleve",
        "tolv",
        "tretten",
        "fjorten",
        "femten",
        "seksten",
        "sytten",
        "atten",
        "nitten"
    ];

    static readonly string[] TensMap =
    [
        string.Empty,
        "ti",
        "tyve",
        "tredive",
        "fyrre",
        "halvtreds",
        "tres",
        "halvfjerds",
        "firs",
        "halvfems"
    ];

    static readonly string[] HundredsPrefixes =
    [
        string.Empty,
        "et",
        "to",
        "tre",
        "fire",
        "fem",
        "seks",
        "syv",
        "otte",
        "ni"
    ];

    public override string Convert(long number)
    {
        if (number == 0)
        {
            return UnitsMap[0];
        }

        if (number < 0)
        {
            return $"minus {Convert(-number)}";
        }

        var parts = new List<string>();
        var remainder = number;

        AppendLarge(parts, ref remainder, 1_000_000_000, "en milliard", "milliarder");
        AppendLarge(parts, ref remainder, 1_000_000, "en million", "millioner");
        AppendThousands(parts, ref remainder);

        if (remainder > 0)
        {
            var tail = remainder < 100 && parts.Count > 0
                ? $"og {ConvertLessThanHundred((int)remainder)}"
                : ConvertLessThanThousand((int)remainder);

            parts.Add(tail);
        }

        return string.Join(" ", parts);
    }

    public override string ConvertToOrdinal(int number) =>
        number.ToString();

    void AppendLarge(List<string> parts, ref long remainder, long divisor, string singular, string plural)
    {
        var count = remainder / divisor;
        if (count == 0)
        {
            return;
        }

        if (count == 1)
        {
            parts.Add(singular);
        }
        else
        {
            parts.Add($"{Convert(count)} {plural}");
        }

        remainder %= divisor;
    }

    void AppendThousands(List<string> parts, ref long remainder)
    {
        var thousands = remainder / 1000;
        if (thousands == 0)
        {
            return;
        }

        if (thousands == 1)
        {
            parts.Add("et tusind");
        }
        else
        {
            parts.Add($"{Convert(thousands)} tusind");
        }

        remainder %= 1000;
    }

    static string ConvertLessThanThousand(int number)
    {
        if (number >= 100)
        {
            var prefix = HundredsPrefixes[number / 100];
            var words = $"{prefix} hundrede";
            var remainder = number % 100;
            if (remainder > 0)
            {
                words = $"{words} og {ConvertLessThanHundred(remainder)}";
            }

            return words;
        }

        return ConvertLessThanHundred(number);
    }

    static string ConvertLessThanHundred(int number)
    {
        if (number < 20)
        {
            return UnitsMap[number];
        }

        var tens = number / 10;
        var unit = number % 10;
        var tensWord = TensMap[tens];

        return unit == 0
            ? tensWord
            : $"{UnitsMap[unit]}og{tensWord}";
    }
}

using System.Collections.Generic;
using System.Globalization;

namespace Humanizer;

class SlovakNumberToWordsConverter(CultureInfo culture) :
    GenderedNumberToWordsConverter
{
    readonly CultureInfo _culture = culture;

    static readonly string[] BillionsMap = ["miliarda", "miliardy", "miliárd"];
    static readonly string[] MillionsMap = ["milión", "milióny", "miliónov"];
    static readonly string[] ThousandsMap = ["tisíc", "tisíce", "tisíc"];
    static readonly string[] HundredsMap = ["nula", "sto", "dvesto", "tristo", "štyristo", "päťsto", "šesťsto", "sedemsto", "osemsto", "deväťsto"];
    static readonly string[] TensMap = ["nula", "desať", "dvadsať", "tridsať", "štyridsať", "päťdesiat", "šesťdesiat", "sedemdesiat", "osemdesiat", "deväťdesiat"];
    static readonly string[] UnitsMap = ["nula", "jeden", "dva", "tri", "štyri", "päť", "šesť", "sedem", "osem", "deväť", "desať", "jedenásť", "dvanásť", "trinásť", "štrnásť", "pätnásť", "šestnásť", "sedemnásť", "osemnásť", "devätnásť"];
    static readonly string[] UnitsMasculineOverrideMap = ["jeden", "dva"];
    static readonly string[] UnitsFeminineOverrideMap = ["jedna", "dve"];
    static readonly string[] UnitsNeuterOverride = ["jedno", "dva"];
    static readonly string[] UnitsIntraOverride = ["jedna", "dva"];

    public override string Convert(long number, GrammaticalGender gender, bool addAnd = true)
    {
        if (number == 0)
        {
            return UnitsMap[0];
        }

        var parts = new List<string>();

        if (number < 0)
        {
            parts.Add("mínus");
            number = -number;
        }

        CollectThousandAndAbove(parts, ref number, 1_000_000_000, GrammaticalGender.Feminine, BillionsMap);
        CollectThousandAndAbove(parts, ref number, 1_000_000, GrammaticalGender.Masculine, MillionsMap);
        CollectThousandAndAbove(parts, ref number, 1_000, GrammaticalGender.Masculine, ThousandsMap);

        CollectLessThanThousand(parts, number, gender);

        return string.Join(" ", parts);
    }

    public override string ConvertToOrdinal(int number, GrammaticalGender gender) =>
        number.ToString(_culture);

    static string UnitByGender(long number, GrammaticalGender? gender)
    {
        if (number != 1 && number != 2)
        {
            return UnitsMap[number];
        }

        return gender switch
        {
            GrammaticalGender.Masculine => UnitsMasculineOverrideMap[number - 1],
            GrammaticalGender.Feminine => UnitsFeminineOverrideMap[number - 1],
            GrammaticalGender.Neuter => UnitsNeuterOverride[number - 1],
            null => UnitsIntraOverride[number - 1],
            _ => throw new ArgumentOutOfRangeException(nameof(gender)),
        };
    }

    static void CollectLessThanThousand(List<string> parts, long number, GrammaticalGender? gender)
    {
        if (number >= 100)
        {
            parts.Add(HundredsMap[number / 100]);
            number %= 100;
        }

        if (number >= 20)
        {
            parts.Add(TensMap[number / 10]);
            number %= 10;
        }

        if (number > 0)
        {
            parts.Add(UnitByGender(number, gender));
        }
    }

    static void CollectThousandAndAbove(List<string> parts, ref long number, long divisor, GrammaticalGender gender, string[] map)
    {
        var n = number / divisor;

        if (n <= 0)
        {
            return;
        }

        CollectLessThanThousand(parts, n, n < 19 ? gender : null);

        var units = n % 1000;

        if (units == 1)
        {
            parts.Add(map[0]);
        }
        else if (units is > 1 and < 5)
        {
            parts.Add(map[1]);
        }
        else
        {
            parts.Add(map[2]);
        }

        number %= divisor;
    }
}

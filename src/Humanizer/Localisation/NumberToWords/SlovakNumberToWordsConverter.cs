namespace Humanizer;

class SlovakNumberToWordsConverter(CultureInfo culture) :
    GenderedNumberToWordsConverter
{
    static readonly string[] BillionsMap = ["miliarda", "miliardy", "miliárd"];
    static readonly string[] MillionsMap = ["milión", "milióny", "miliónov"];
    static readonly string[] ThousandsMap = ["tisíc", "tisíce", "tisíc"];
    static readonly string[] HundredsMap = ["nula", "sto", "dvesto", "tristo", "štyristo", "päťsto", "šesťsto", "sedemsto", "osemsto", "deväťsto"];
    static readonly string[] TensMap = ["nula", "desať", "dvadsať", "tridsať", "štyridsať", "päťdesiat", "šesťdesiat", "sedemdesiat", "osemdesiat", "deväťdesiat"];
    static readonly string[] UnitsMap = ["nula", "jeden", "dva", "tri", "štyri", "päť", "šesť", "sedem", "osem", "deväť", "desať", "jedenásť", "dvanásť", "trinásť", "štrnásť", "pätnásť", "šestnásť", "sedemnásť", "osemnásť", "devätnásť"];

    public override string Convert(long number, GrammaticalGender gender, bool addAnd = true)
    {
        if (number == 0)
        {
            return UnitByGender(number, gender);
        }

        var parts = new List<string>();
        if (number < 0)
        {
            parts.Add("mínus");
            number = -number;
        }

        CollectThousandsAndAbove(parts, ref number, 1_000_000_000, GrammaticalGender.Feminine, BillionsMap);
        CollectThousandsAndAbove(parts, ref number, 1_000_000, GrammaticalGender.Masculine, MillionsMap);
        CollectThousandsAndAbove(parts, ref number, 1_000, GrammaticalGender.Masculine, ThousandsMap);

        CollectLessThanThousand(parts, number, gender);

        return string.Join(" ", parts);
    }

    public override string ConvertToOrdinal(int number, GrammaticalGender gender) =>
        number.ToString(culture);

    static string UnitByGender(long number, GrammaticalGender? gender)
    {
        if (number != 1 && number != 2)
        {
            return UnitsMap[number];
        }

        return gender switch
        {
            GrammaticalGender.Feminine => number == 1 ? "jedna" : "dve",
            GrammaticalGender.Neuter => number == 1 ? "jedno" : "dve",
            _ => number == 1 ? "jeden" : "dva"
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

    static void CollectThousandsAndAbove(List<string> parts, ref long number, long divisor, GrammaticalGender gender, string[] map)
    {
        var n = number / divisor;
        if (n <= 0)
        {
            return;
        }

        if (!(divisor == 1000 && n == 1))
        {
            CollectLessThanThousand(parts, n, n < 20 ? gender : null);
        }

        var tens = (n / 10) % 10;
        var units = n % 10;

        if (units == 1 && tens != 1)
        {
            parts.Add(map[0]);
        }
        else if (units is > 1 and < 5 && tens != 1)
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

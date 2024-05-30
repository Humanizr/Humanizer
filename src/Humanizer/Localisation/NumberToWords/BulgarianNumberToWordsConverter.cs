namespace Humanizer;

class BulgarianNumberToWordsConverter() :
    GenderedNumberToWordsConverter(GrammaticalGender.Neuter)
{
    static readonly string[] UnitsMap =
    [
        "нула", "едно", "две", "три", "четири", "пет", "шест", "седем", "осем", "девет", "десет", "единадесет",
        "дванадесет", "тринадесет", "четиринадесет", "петнадесет", "шестнадесет", "седемнадесет", "осемнадесет",
        "деветнадесет"
    ];

    static readonly string[] TensMap =
    [
        "нула", "десет", "двадесет", "тридесет", "четиридесет", "петдесет", "шестдесет", "седемдесет",
        "осемдесет", "деветдесет"
    ];

    static readonly string[] HundredsMap =
    [
        "нула", "сто", "двеста", "триста", "четиристотин", "петстотин", "шестстотин", "седемстотин",
        "осемстотин", "деветстотин"
    ];

    static readonly string[] HundredsOrdinalMap =
    [
        string.Empty, "стот", "двестот", "тристот", "четиристот", "петстот", "шестстот", "седемстот", "осемстот",
        "деветстот"
    ];

    static readonly string[] UnitsOrdinal =
    [
        string.Empty, "първ", "втор", "трет", "четвърт", "пет", "шест", "седм", "осм", "девeт", "десeт",
        "единадесет", "дванадесет", "тринадесет", "четиринадесет", "петнадесет", "шестнадесет", "седемнадесет",
        "осемнадесет", "деветнадесет"
    ];

    public override string Convert(long input, GrammaticalGender gender, bool addAnd = true) =>
        InnerConvert(input, gender, false);

    public override string ConvertToOrdinal(int input, GrammaticalGender gender) =>
        InnerConvert(input, gender, true);

    static string InnerConvert(long input, GrammaticalGender gender, bool isOrdinal)
    {
        if (input == 0)
        {
            return isOrdinal ? OrdinalZero(gender) : "нула";
        }

        var parts = new List<string>();
        if (input < 0)
        {
            parts.Add("минус");
            input = -input;
        }

        CollectParts(parts, ref input, isOrdinal, 1_000_000_000_000_000_000, GrammaticalGender.Masculine, "квинтилион", "квадрилиона", ToOrdinalOverAHundred("квинтилион", gender));
        CollectParts(parts, ref input, isOrdinal, 1_000_000_000_000_000, GrammaticalGender.Masculine, "квадрилион", "квадрилиона", ToOrdinalOverAHundred("квадрилион", gender));
        CollectParts(parts, ref input, isOrdinal, 1_000_000_000_000, GrammaticalGender.Masculine, "трилион", "трилиона", ToOrdinalOverAHundred("трилион", gender));
        CollectParts(parts, ref input, isOrdinal, 1_000_000_000, GrammaticalGender.Masculine, "милиард", "милиарда", ToOrdinalOverAHundred("милиард", gender));
        CollectParts(parts, ref input, isOrdinal, 1_000_000, GrammaticalGender.Masculine, "милион", "милиона", ToOrdinalOverAHundred("милион", gender));
        CollectParts(parts, ref input, isOrdinal, 1_000, GrammaticalGender.Feminine, "хиляда", "хиляди", ToOrdinalOverAHundred("хиляд", gender));
        CollectPartsUnderOneThousand(parts, ref input, isOrdinal, gender);

        return string.Join(" ", parts);
    }

    static void CollectParts(List<string> parts, ref long number, bool isOrdinal, long divisor, GrammaticalGender gender, string singular, string plural, string ordinal)
    {
        if (number < divisor)
        {
            return;
        }

        var result = number / divisor;

        if (parts.Count > 0)
        {
            parts.Add("и");
        }

        CollectPartsUnderOneThousand(parts, ref result, false, gender);

        number %= divisor;
        if (number == 0 && isOrdinal)
        {
            parts.Add(ordinal);
        }
        else
        {
            parts.Add(result == 1 ? singular : plural);
        }
    }

    static void CollectPartsUnderOneThousand(List<string> parts, ref long number, bool isOrdinal, GrammaticalGender gender)
    {
        if (number == 0)
        {
            return;
        }

        if (number >= 100)
        {
            var hundreds = number / 100;
            number %= 100;
            if (number == 0 && isOrdinal)
            {
                parts.Add(ToOrdinalOverAHundred(HundredsOrdinalMap[hundreds], gender));
            }
            else
            {
                parts.Add(HundredsMap[hundreds]);
            }
        }

        if (number >= 20)
        {
            var tens = number / 10;
            number %= 10;
            if (number == 0 && isOrdinal)
            {
                parts.Add(ToOrdinalUnitsAndTens(TensMap[tens], gender));
            }
            else
            {
                parts.Add(TensMap[tens]);
            }
        }

        if (number > 0)
        {
            if (isOrdinal)
            {
                parts.Add(ToOrdinalUnitsAndTens(UnitsOrdinal[number], gender));
            }
            else
            {
                parts.Add(GetUnit(number, gender));
            }
        }

        if (parts.Count > 1)
        {
            parts.Insert(parts.Count - 1, "и");
        }
    }

    static string GetUnit(long number, GrammaticalGender gender) =>
        (number, gender) switch
        {
            (1, GrammaticalGender.Masculine) => "един",
            (1, GrammaticalGender.Feminine) => "една",
            (2, GrammaticalGender.Masculine) => "два",
            _ => UnitsMap[number],
        };

    static string OrdinalZero(GrammaticalGender gender) =>
        gender switch
        {
            GrammaticalGender.Masculine => "нулев",
            GrammaticalGender.Feminine => "нулева",
            GrammaticalGender.Neuter => "нулево",
            _ => throw new ArgumentOutOfRangeException(nameof(gender), gender, null)
        };

    static string ToOrdinalOverAHundred(string word, GrammaticalGender gender) =>
        gender switch
        {
            GrammaticalGender.Masculine => $"{word}ен",
            GrammaticalGender.Feminine => $"{word}на",
            GrammaticalGender.Neuter => $"{word}но",
            _ => throw new ArgumentOutOfRangeException(nameof(gender))
        };

    static string ToOrdinalUnitsAndTens(string word, GrammaticalGender gender) =>
        gender switch
        {
            GrammaticalGender.Masculine => $"{word}и",
            GrammaticalGender.Feminine => $"{word}а",
            GrammaticalGender.Neuter => $"{word}о",
            _ => throw new ArgumentOutOfRangeException(nameof(gender))
        };
}
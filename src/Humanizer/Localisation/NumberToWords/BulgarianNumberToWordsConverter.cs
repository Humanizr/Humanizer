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

    public override string Convert(long input, GrammaticalGender gender, bool addAnd = true)
    {
        if (input == 0)
        {
            return "нула";
        }

        var parts = new List<string>();
        if (input < 0)
        {
            parts.Add("минус");
            input = -input;
        }

        CollectParts(parts, ref input, 1_000_000_000_000_000_000, GrammaticalGender.Masculine, "квинтилион", "квадрилиона");
        CollectParts(parts, ref input, 1_000_000_000_000_000, GrammaticalGender.Masculine, "квадрилион", "квадрилиона");
        CollectParts(parts, ref input, 1_000_000_000_000, GrammaticalGender.Masculine, "трилион", "трилиона");
        CollectParts(parts, ref input, 1_000_000_000, GrammaticalGender.Masculine, "милиард", "милиарда");
        CollectParts(parts, ref input, 1_000_000, GrammaticalGender.Masculine, "милион", "милиона");
        CollectParts(parts, ref input, 1_000, GrammaticalGender.Feminine, "хиляда", "хиляди");
        CollectPartsUnderOneThousand(parts, ref input, gender);

        return string.Join(" ", parts);
    }

    public override string ConvertToOrdinal(int input, GrammaticalGender gender)
    {
        if (input == 0)
        {
            return gender switch
            {
                GrammaticalGender.Masculine => "нулев",
                GrammaticalGender.Feminine => "нулева",
                GrammaticalGender.Neuter => "нулево",
                _ => throw new ArgumentOutOfRangeException(nameof(gender), gender, null)
            };
        }

        var parts = new List<string>();

        long number = input;
        if (number < 0)
        {
            parts.Add("минус");
            number = -number;
        }

        var ending = GetEnding2(gender);
        CollectOrdinalParts(parts, ref number, 1_000_000_000, GrammaticalGender.Masculine, "милиард", "милиарда", "милиард" + ending);
        CollectOrdinalParts(parts, ref number, 1_000_000, GrammaticalGender.Masculine, "милион", "милиона", "милион" + ending);
        CollectOrdinalParts(parts, ref number, 1_000, GrammaticalGender.Feminine, "хиляда", "хиляди", "хиляд" + ending);
        CollectOrdinalPartsUnderOneThousand(parts, number, gender);

        return string.Join(" ", parts);
    }

    static string GetEnding2(GrammaticalGender gender)
    {
        var ending = gender switch
        {
            GrammaticalGender.Masculine => "ен",
            GrammaticalGender.Feminine => "на",
            GrammaticalGender.Neuter => "но",
            _ => throw new ArgumentOutOfRangeException(nameof(gender))
        };
        return ending;
    }

    static void CollectPartsUnderOneThousand(IList<string> parts, ref long number, GrammaticalGender gender)
    {
        if (number == 0)
        {
            return;
        }

        if (number >= 100)
        {
            var hundreds = number / 100;
            parts.Add(HundredsMap[hundreds]);
            number %= 100;
        }

        if (number >= 20)
        {
            var tens = number / 10;
            parts.Add(TensMap[tens]);
            number %= 10;
        }

        if (number > 0)
        {
            parts.Add(GetUnit(number, gender));
        }

        if (parts.Count > 1)
        {
            parts.Insert(parts.Count - 1, "и");
        }
    }

    static void CollectParts(IList<string> parts, ref long number, long divisor, GrammaticalGender gender, string singular, string plural)
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

        CollectPartsUnderOneThousand(parts, ref result, gender);

        number %= divisor;
        parts.Add(result == 1 ? singular : plural);
    }

    static void CollectOrdinalPartsUnderOneThousand(IList<string> parts, long number, GrammaticalGender gender)
    {
        if (number == 0)
        {
            return;
        }

        if (number >= 100)
        {
            var hundreds = number / 100;
            number %= 100;
            if (number == 0)
            {
                parts.Add(HundredsOrdinalMap[hundreds] + GetEnding2(gender));
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
            if (number == 0)
            {
                parts.Add(TensMap[tens] + GetEnding(gender));
            }
            else
            {
                parts.Add(TensMap[tens]);
            }
        }

        if (number > 0)
        {
            parts.Add(UnitsOrdinal[number] + GetEnding(gender));
        }

        if (parts.Count > 1)
        {
            parts.Insert(parts.Count - 1, "и");
        }
    }


    static void CollectOrdinalParts(IList<string> parts, ref long number, long divisor, GrammaticalGender gender, string singular, string plural, string ordinal)
    {
        if (number < divisor)
        {
            return;
        }

        var result = number / divisor;
        number %= divisor;

        CollectPartsUnderOneThousand(parts, ref result, gender);

        if (number == 0)
        {
            parts.Add(ordinal);
        }
        else
        {
            parts.Add(result == 1 ? singular : plural);
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

    static string SelectForm(long number, params string[] forms) =>
        number switch
        {
            1 => forms[0],
            _ => forms[1]
        };

    static string GetEnding(GrammaticalGender gender) =>
        gender switch
        {
            GrammaticalGender.Masculine => "и",
            GrammaticalGender.Feminine => "а",
            GrammaticalGender.Neuter => "о",
            _ => throw new ArgumentOutOfRangeException(nameof(gender))
        };
}
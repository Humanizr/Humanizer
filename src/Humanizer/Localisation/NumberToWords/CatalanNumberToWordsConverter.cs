namespace Humanizer;

class CatalanNumberToWordsConverter : GenderedNumberToWordsConverter
{
    // Cardinal units
    private static readonly string[] UnitsMasculine =
    [
        "", "un", "dos", "tres", "quatre", "cinc", "sis", "set", "vuit", "nou"
    ];
    private static readonly string[] UnitsFeminine =
    [
        "", "una", "dues", "tres", "quatre", "cinc", "sis", "set", "vuit", "nou"
    ];

    // Special cases for teens (10-19)
    private static readonly string[] Teens =
    [
        "deu", "onze", "dotze", "tretze", "catorze", "quinze", "setze", "disset", "divuit", "dinou"
    ];

    // Decenas: exact tens and for composition
    private static readonly string[] Tens =
    [
        "", "deu", "vint", "trenta", "quaranta", "cinquanta", "seixanta", "setanta", "vuitanta", "noranta"
    ];

    // Centenas (masculino y femenino)
    private static readonly string[] HundredsMasculine =
    [
        "", "cent", "dos-cents", "tres-cents", "quatre-cents", "cinc-cents", "sis-cents", "set-cents", "vuit-cents", "nou-cents"
    ];
    private static readonly string[] HundredsFeminine =
    [
        "", "cent", "dues-centes", "tres-centes", "quatre-centes", "cinc-centes", "sis-centes", "set-centes", "vuit-centes", "nou-centes"
    ];

    static readonly string[] TupleMap =
    [
       "zero vegades", "una vegada", "doble", "triple", "qüàdruple", "quíntuple", "sèxtuple", "sèptuple", "òctuple",
       "nònuple", "dècuple", "undècuple", "duodècuple", "tercidecuple"
    ];

    #region Convert
    public override string Convert(long number, GrammaticalGender gender, bool addAnd = true)
    {
        if (number == 0)
        {
            return "zero";
        }

        if (number < 0)
        {
            return "menys " + Convert(-number, gender);
        }

        if (number < 10)
        {
            return GetUnit((int)number, gender);
        }

        if (number < 20)
        {
            return Teens[number - 10];
        }

        if (number < 100)
        {
            return GetTens((int)number, gender);
        }

        if (number < 1000)
        {
            return GetHundreds((int)number, gender);
        }

        if (number < 1000000)
        {
            return GetThousands((int)number, gender);
        }

        if (number < 1000000000)
        {
            return GetMillions((int)number, gender);
        }

        throw new NotImplementedException("Nombres més grans de mil milions no estan implementats.");
    }

    private static string GetUnit(int number, GrammaticalGender gender)
        => gender == GrammaticalGender.Feminine ? UnitsFeminine[number] : UnitsMasculine[number];

    private static string GetTens(int number, GrammaticalGender gender)
    {
        var tens = number / 10;
        var units = number % 10;
        if (number < 20)
        {
            return Teens[number - 10];
        }

        if (units == 0)
        {
            return Tens[tens];
        }
        // "vint-i-un", "trenta-dos"
        var conjunction = tens == 2 ? "-i-" : "-";

        var num = (units == 1) && gender == GrammaticalGender.Masculine
            ? "u"
            : GetUnit(units, gender);

        return $"{Tens[tens]}{conjunction}{num}";
    }

    private static string GetHundreds(int number, GrammaticalGender gender)
    {
        var hundreds = number / 100;
        var rest = number % 100;
        var hundredPart = gender == GrammaticalGender.Feminine ? HundredsFeminine[hundreds] : HundredsMasculine[hundreds];

        if (rest == 0)
        {
            return hundredPart;
        }

        var num = rest == 1 && gender == GrammaticalGender.Masculine
            ? "u"
            : (rest < 10 ? GetUnit(rest, gender) : GetTens(rest, gender));

        return $"{hundredPart} {num}";
    }

    private string GetThousands(int number, GrammaticalGender gender)
    {
        var thousands = number / 1000;
        var rest = number % 1000;
        string thousandPart;
        if (thousands == 1)
        {
            thousandPart = "mil";
        }
        else
        {
            thousandPart = $"{Convert(thousands, gender)} mil";
        }

        if (rest == 0)
        {
            return thousandPart;
        }

        var num = rest == 1 && gender == GrammaticalGender.Masculine
            ? "u"
            : Convert(rest, gender);

        return $"{thousandPart} {num}";
    }

    private string GetMillions(int number, GrammaticalGender gender)
    {
        var millions = number / 1000000;
        var rest = number % 1000000;
        string millionPart;
        if (millions == 1)
        {
            millionPart = "un milió";
        }
        else
        {
            millionPart = $"{Convert(millions, GrammaticalGender.Masculine)} milions";
        }

        if (rest == 0)
        {
            return millionPart;
        }

        var num = rest == 1 && gender == GrammaticalGender.Masculine
            ? "u"
            : Convert(rest, gender);

        return $"{millionPart} {num}";
    }

    #endregion

    #region ConvertToOrdinal
    public override string ConvertToOrdinal(int number, GrammaticalGender gender)
    {
        if (number < 0)
        {
            return "menys " + ConvertToOrdinal(-number, gender);
        }

        if (number == 0)
        {
            return "zero";
        }

        // Ordinales simples
        string[] masc = ["", "primer", "segon", "tercer", "quart", "cinquè", "sisè", "setè", "vuitè", "novè", "desè", "onzè", "dotzè", "tretzè", "catorzè", "quinzè"];
        string[] fem = ["", "primera", "segona", "tercera", "quarta", "cinquena", "sisena", "setena", "vuitena", "novena", "desena", "onzena", "dotzena", "tretzena", "catorzena", "quinzena"];

        if (number < masc.Length)
        {
            return gender == GrammaticalGender.Feminine ? fem[number] : masc[number];
        }

        if (number < 100)
        {
            return GetOrdinalTens(number, gender);
        }

        if (number < 1000)
        {
            return GetOrdinalHundreds(number, gender);
        }

        if (number < 1000000)
        {
            return GetOrdinalThousands(number, gender);
        }

        if (number < 1000000000)
        {
            return GetOrdinalMillions(number, gender);
        }

        throw new NotImplementedException("Ordinal més gran de cent milions no implementat.");
    }

    // Helpers

    private static string GetOrdinalTens(int number, GrammaticalGender gender)
    {
        var dec = number / 10;
        var rem = number % 10;
        string[] tens = ["", "", "vint", "trenta", "quaranta", "cinquanta", "seixanta", "setanta", "vuitanta", "noranta"];

        var ordSuf = gender == GrammaticalGender.Feminine ? "ena" : "è";

        if (rem == 0)
        {
            var tensDec = tens[dec];
            if (dec == 3 || dec == 6 || dec == 7 || dec == 8 || dec == 9)
            {
                tensDec = tensDec[..^1]; //
            }

            return tensDec + ordSuf;
        }

        var unitRoots = new[] { "", "un", "dos", "tres", "quatr", "cinqu", "sis", "set", "vuit", "nov" };
        var num = $"{unitRoots[rem]}{ordSuf}";
        if (rem == 1)
        {
            num = gender == GrammaticalGender.Feminine ? "una" : "un";
        }

        var conj = dec == 2 ? "-i-" : "-";
        return $"{tens[dec]}{conj}{num}";
    }

    private string GetOrdinalHundreds(int number, GrammaticalGender gender)
    {
        var centenas = number / 100;
        var rest = number % 100;

        var hundred = gender == GrammaticalGender.Feminine
            ? new[] { "", "cent", "dues-centes", "tres-centes", "quatre-centes", "cinc-centes", "sis-centes", "set-centes", "vuit-centes", "nou-centes" }[centenas]
            : new[] { "", "cent", "dos-cents", "tres-cents", "quatre-cents", "cinc-cents", "sis-cents", "set-cents", "vuit-cents", "nou-cents" }[centenas];

        if (rest == 0 && centenas == 1)
        {
            return $"{hundred}{(gender == GrammaticalGender.Feminine ? "ena" : "è")}";
        }

        if (rest == 0)
        {
            return hundred;
        }

        string? num;
        if (centenas == 1 && rest % 10 != 0)
        {
            num = ConvertToOrdinal(rest, gender);
        }
        else
        {
            num = Convert(rest, gender);
            if (gender == GrammaticalGender.Masculine && rest != 1 && rest % 10 == 1)
            {
                num += "n";
            }
        }

        return $"{hundred} {num}";
    }

    private string GetOrdinalThousands(int number, GrammaticalGender gender)
    {
        var mils = number / 1000;
        var rest = number % 1000;
        var milStr = mils == 1 ? "mil" : $"{Convert(mils, gender)} mil";
        if (rest == 0)
        {
            return milStr;
        }

        if (rest == 100)
        {
            return $"{milStr} cent";
        }

        var num = Convert(rest, gender);
        if (gender == GrammaticalGender.Masculine && rest != 1 && rest % 10 == 1)
        {
            num += "n";
        }

        return $"{milStr} {num}";
    }

    private string GetOrdinalMillions(int number, GrammaticalGender gender)
    {
        var mills = number / 1000000;
        var rest = number % 1000000;
        var millsStr = mills == 1 ? "un milió" : $"{Convert(mills, GrammaticalGender.Masculine)} milions";
        if (rest == 0)
        {
            return millsStr;
        }

        var num = Convert(rest, gender);
        if (gender == GrammaticalGender.Masculine && rest != 1 && rest % 10 == 1)
        {
            num += "n";
        }

        return $"{millsStr} {num}";
    }

    public override string ConvertToOrdinal(int number, GrammaticalGender gender, WordForm wordForm)
    {
        // ordinal (Ej: 1r, 1a, 2n, 2a, 11è, 11a, etc.)
        if (wordForm == WordForm.Abbreviation)
        {
            if (number == 1)
            {
                return gender == GrammaticalGender.Feminine ? "1a" : "1r";
            }

            if (number == 2)
            {
                return gender == GrammaticalGender.Feminine ? "2a" : "2n";
            }

            if (number == 3)
            {
                return gender == GrammaticalGender.Feminine ? "3a" : "3r";
            }

            if (number == 22)
            {
                return gender == GrammaticalGender.Feminine ? "22a" : "22n";
            }

            if (number == 31)
            {
                return gender == GrammaticalGender.Feminine ? "31a" : "31r";
            }

            if (number == 11 || number == 100 || number == 999)
            {
                return number + (gender == GrammaticalGender.Feminine ? "a" : "è");
            }

            if (number == 101)
            {
                return gender == GrammaticalGender.Feminine ? "101a" : "101r";
            }

            if (number == 999)
            {
                return gender == GrammaticalGender.Feminine ? "999a" : "999è";
            }
            // Comportamiento genérico
            if (gender == GrammaticalGender.Feminine)
            {
                return number + "a";
            }

            return number + (
                (number % 10 == 1 || number % 10 == 3) ? "r" :
                (number % 10 == 2 || number % 10 == 7) ? "n" :
                "è"
            );
        }
        return ConvertToOrdinal(number, gender);
    }

    #endregion

    public override string ConvertToTuple(int number)
    {
        number = Math.Abs(number);

        if (number < TupleMap.Length)
        {
            return TupleMap[number];
        }

        return Convert(number) + " vegades";
    }

}

namespace Humanizer;

class SpanishNumberToWordsConverter : GenderedNumberToWordsConverter
{
    static readonly string[] HundredsRootMap =
    [
        "cero", "ciento", "doscient", "trescient", "cuatrocient", "quinient", "seiscient", "setecient",
        "ochocient", "novecient"
    ];

    static readonly string[] HundredthsRootMap =
    [
        "", "centésim", "ducentésim", "tricentésim", "cuadringentésim", "quingentésim", "sexcentésim",
        "septingentésim", "octingentésim", "noningentésim"
    ];

    static readonly string[] OrdinalsRootMap =
    [
        "", "primer", "segund", "tercer", "cuart", "quint", "sext",
        "séptim", "octav", "noven"
    ];

    static readonly string[] TensMap =
    [
        "cero", "diez", "veinte", "treinta", "cuarenta", "cincuenta", "sesenta", "setenta", "ochenta", "noventa"
    ];

    static readonly string[] TenthsRootMap =
    [
        "", "décim", "vigésim", "trigésim", "cuadragésim", "quincuagésim", "sexagésim", "septuagésim",
        "octogésim", "nonagésim"
    ];

    static readonly string[] ThousandthsRootMap =
    [
        "", "milésim", "dosmilésim", "tresmilésim", "cuatromilésim", "cincomilésim", "seismilésim",
        "sietemilésim", "ochomilésim", "nuevemilésim"
    ];

    static readonly string[] TupleMap =
    [
        "cero veces", "una vez", "doble", "triple", "cuádruple", "quíntuple", "séxtuple", "séptuple", "óctuple",
        "nonuplo", "décuplo", "undécuplo", "duodécuplo", "terciodécuplo"
    ];

    static readonly string[] UnitsMap =
    [
        "cero", "uno", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve", "diez", "once", "doce",
        "trece", "catorce", "quince", "dieciséis", "diecisiete", "dieciocho", "diecinueve", "veinte", "veintiuno",
        "veintidós", "veintitrés", "veinticuatro", "veinticinco", "veintiséis", "veintisiete", "veintiocho", "veintinueve"
    ];

    public override string Convert(long input, GrammaticalGender gender, bool addAnd = true) =>
        Convert(input, WordForm.Normal, gender, addAnd);

    public override string Convert(long number, WordForm wordForm, GrammaticalGender gender, bool addAnd = true)
    {
        List<string> wordBuilder = [];

        if (number == 0)
        {
            return "cero";
        }

        if (number == long.MinValue)
        {
            return
                "menos nueve trillones doscientos veintitrés mil trescientos setenta y dos billones treinta y seis mil " +
                "ochocientos cincuenta y cuatro millones setecientos setenta y cinco mil ochocientos ocho";
        }

        if (number < 0)
        {
            return $"menos {Convert(-number)}";
        }

        wordBuilder.Add(ConvertGreaterThanMillion(number, out var remainder));
        wordBuilder.Add(ConvertThousands(remainder, out remainder, gender));
        wordBuilder.Add(ConvertHundreds(remainder, out remainder, gender));
        wordBuilder.Add(ConvertUnits(remainder, gender, wordForm));

        return BuildWord(wordBuilder);
    }

    public override string ConvertToOrdinal(int number, GrammaticalGender gender) =>
        ConvertToOrdinal(number, gender, WordForm.Normal);

    public override string ConvertToOrdinal(int number, GrammaticalGender gender, WordForm wordForm)
    {
        List<string> wordBuilder = [];

        if (number is 0 or int.MinValue)
        {
            return "cero";
        }

        if (number < 0)
        {
            return ConvertToOrdinal(Math.Abs(number), gender);
        }

        if (IsRoundBillion(number))
        {
            return ConvertRoundBillionths(number, gender);
        }

        if (IsRoundMillion(number))
        {
            return ConvertToOrdinal(number / 1000, gender)
                .Replace("milésim", "millonésim");
        }

        wordBuilder.Add(ConvertTensAndHunderdsOfThousandths(number, out var remainder, gender));
        wordBuilder.Add(ConvertThousandths(remainder, out remainder, gender));
        wordBuilder.Add(ConvertHundredths(remainder, out remainder, gender));
        wordBuilder.Add(ConvertTenths(remainder, out remainder, gender));
        wordBuilder.Add(ConvertOrdinalUnits(remainder, gender, wordForm));

        return BuildWord(wordBuilder);
    }

    public override string ConvertToTuple(int number)
    {
        number = Math.Abs(number);

        if (number < TupleMap.Length)
            return TupleMap[number];

        return Convert(number) + " veces";
    }

    static string BuildWord(List<string> wordParts)
    {
        wordParts.RemoveAll(string.IsNullOrEmpty);
        return string.Join(" ", wordParts);
    }

    static string ConvertHundreds(in long inputNumber, out long remainder, GrammaticalGender gender)
    {
        var wordPart = string.Empty;
        remainder = inputNumber;

        if (inputNumber / 100 > 0)
        {
            wordPart = inputNumber == 100 ? "cien" : GetGenderedHundredsMap(gender)[(int) (inputNumber / 100)];

            remainder = inputNumber % 100;
        }

        return wordPart;
    }

    static string ConvertHundredths(in int number, out int remainder, GrammaticalGender gender) =>
        ConvertMappedOrdinalNumber(number, 100, HundredthsRootMap, out remainder, gender);

    static string ConvertMappedOrdinalNumber(
        in int number,
        in int divisor,
        IReadOnlyList<string> map,
        out int remainder,
        GrammaticalGender gender)
    {
        var wordPart = string.Empty;
        remainder = number;

        if (number / divisor > 0)
        {
            var genderedEnding = gender == GrammaticalGender.Feminine ? "a" : "o";
            wordPart = map[number / divisor] + genderedEnding;
            remainder = number % divisor;
        }

        return wordPart;
    }

    static string ConvertOrdinalUnits(in int number, GrammaticalGender gender, WordForm wordForm)
    {
        if (number is <= 0 or >= 10)
        {
            return string.Empty;
        }

        switch (gender)
        {
            case GrammaticalGender.Masculine:
            case GrammaticalGender.Neuter:
                if (HasOrdinalAbbreviation(number, wordForm))
                {
                    return OrdinalsRootMap[number];
                }

                return OrdinalsRootMap[number] + 'o';
            case GrammaticalGender.Feminine:
                return OrdinalsRootMap[number] + "a";
            default:
                throw new ArgumentOutOfRangeException(nameof(gender), gender, null);
        }
    }

    static string ConvertTenths(in int number, out int remainder, GrammaticalGender gender) =>
        ConvertMappedOrdinalNumber(number, 10, TenthsRootMap, out remainder, gender);

    static string ConvertThousandths(in int number, out int remainder, GrammaticalGender gender) =>
        ConvertMappedOrdinalNumber(number, 1000, ThousandthsRootMap, out remainder, gender);

    static string ConvertUnits(long inputNumber, GrammaticalGender gender, WordForm wordForm = WordForm.Normal)
    {
        if (inputNumber <= 0)
        {
            return string.Empty;
        }

        UnitsMap[1] = GetGenderedOne(gender, wordForm);
        UnitsMap[21] = GetGenderedTwentyOne(gender, wordForm);

        if (inputNumber < 30)
        {
            return UnitsMap[inputNumber];
        }

        var wordPart = TensMap[inputNumber / 10];
        if (inputNumber % 10 <= 0)
        {
            return wordPart;
        }

        return wordPart + $" y {UnitsMap[inputNumber % 10]}";
    }

    static List<string> GetGenderedHundredsMap(GrammaticalGender gender)
    {
        var genderedEnding = gender == GrammaticalGender.Feminine ? "as" : "os";
        var map = new List<string>();
        map.AddRange(HundredsRootMap.Take(2));

        for (var i = 2; i < HundredsRootMap.Length; i++)
        {
            map.Add(HundredsRootMap[i] + genderedEnding);
        }

        return map;
    }

    static string GetGenderedOne(GrammaticalGender gender, WordForm wordForm = WordForm.Normal)
    {
        switch (gender)
        {
            case GrammaticalGender.Masculine:
            case GrammaticalGender.Neuter:
                return wordForm == WordForm.Abbreviation ? "un" : "uno";
            case GrammaticalGender.Feminine:
                return "una";
            default:
                throw new ArgumentOutOfRangeException(nameof(gender), gender, null);
        }
    }

    static string GetGenderedTwentyOne(GrammaticalGender gender, WordForm wordForm = WordForm.Normal)
    {
        switch (gender)
        {
            case GrammaticalGender.Masculine:
            case GrammaticalGender.Neuter:
                return wordForm == WordForm.Abbreviation ? "veintiún" : "veintiuno";
            case GrammaticalGender.Feminine:
                return "veintiuna";
            default:
                throw new ArgumentOutOfRangeException(nameof(gender), gender, null);
        }
    }

    static bool HasOrdinalAbbreviation(int number, WordForm wordForm) =>
        number is 1 or 3 && wordForm == WordForm.Abbreviation;

    static bool IsRoundBillion(int number) =>
        number >= 1000_000_000 && number % 1_000_000 == 0;

    static bool IsRoundMillion(int number) =>
        number >= 1000000 && number % 1000000 == 0;

    static string PluralizeGreaterThanMillion(string singularWord) =>
        singularWord.TrimEnd('ó', 'n') + "ones";

    static readonly KeyValuePair<string, long>[] NumbersAndWordsDict =
    [
        new("trillón", 1_000_000_000_000_000_000),
        new("billón", 1_000_000_000_000),
        new("millón", 1_000_000),
    ];

    string ConvertGreaterThanMillion(in long inputNumber, out long remainder)
    {
        List<string> wordBuilder = [];

        remainder = inputNumber;
        foreach (var numberAndWord in NumbersAndWordsDict)
        {
            if (remainder / numberAndWord.Value > 0)
            {
                if (remainder / numberAndWord.Value == 1)
                {
                    wordBuilder.Add($"un {numberAndWord.Key}");
                }
                else
                {
                    wordBuilder.Add(remainder / numberAndWord.Value % 10 == 1 ? $"{Convert(remainder / numberAndWord.Value, WordForm.Abbreviation, GrammaticalGender.Masculine)} {PluralizeGreaterThanMillion(numberAndWord.Key)}" : $"{Convert(remainder / numberAndWord.Value)} {PluralizeGreaterThanMillion(numberAndWord.Key)}");
                }

                remainder %= numberAndWord.Value;
            }
        }

        return BuildWord(wordBuilder);
    }

    string ConvertRoundBillionths(int number, GrammaticalGender gender)
    {
        var cardinalPart = Convert(number / 1_000_000, WordForm.Abbreviation, gender);
        var sep = number == 1_000_000_000 ? "" : " ";
        var ordinalPart = ConvertToOrdinal(1_000_000, gender);
        return cardinalPart + sep + ordinalPart;
    }

    string ConvertTensAndHunderdsOfThousandths(in int number, out int remainder, GrammaticalGender gender)
    {
        var wordPart = string.Empty;
        remainder = number;

        if (number / 10000 > 0)
        {
            wordPart = Convert(number / 1000 * 1000, gender);

            if (number < 30000 || IsRoundNumber(number))
            {
                if (number == 21000)
                {
                    wordPart = wordPart
                        .Replace("a", "")
                        .Replace("ú", "u");
                }

                wordPart = wordPart.Remove(wordPart.LastIndexOf(' '), 1);
            }

            wordPart += "ésim" + (gender == GrammaticalGender.Masculine ? "o" : "a");

            remainder = number % 1000;
        }

        return wordPart;

        static bool IsRoundNumber(int number) =>
            (number % 10000 == 0 && number < 100000)
            || (number % 100000 == 0 && number < 1000000)
            || (number % 1000000 == 0 && number < 10000000)
            || (number % 10000000 == 0 && number < 100000000)
            || (number % 100000000 == 0 && number < 1000000000)
            || (number % 1000000000 == 0 && number < int.MaxValue);
    }

    string ConvertThousands(in long inputNumber, out long remainder, GrammaticalGender gender)
    {
        var wordPart = string.Empty;
        remainder = inputNumber;

        if (inputNumber / 1000 > 0)
        {
            if (inputNumber / 1000 == 1)
            {
                wordPart = "mil";
            }
            else
            {
                wordPart = gender == GrammaticalGender.Feminine ? $"{Convert(inputNumber / 1000, GrammaticalGender.Feminine)} mil" : $"{Convert(inputNumber / 1000, WordForm.Abbreviation, gender)} mil";
            }

            remainder = inputNumber % 1000;
        }

        return wordPart;
    }
}
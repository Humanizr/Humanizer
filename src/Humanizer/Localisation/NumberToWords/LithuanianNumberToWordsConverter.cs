namespace Humanizer;

class LithuanianNumberToWordsConverter : GenderedNumberToWordsConverter
{
    static readonly string[] UnitsMap = ["nulis", "vienas", "du", "trys", "keturi", "penki", "šeši", "septyni", "aštuoni", "devyni", "dešimt", "vienuolika", "dvylika", "trylika", "keturiolika", "penkiolika", "šešiolika", "septyniolika", "aštuoniolika", "devyniolika"];
    static readonly string[] TensMap = [string.Empty, "dešimt", "dvidešimt", "trisdešimt", "keturiasdešimt", "penkiasdešimt", "šešiasdešimt", "septyniasdešimt", "aštuoniasdešimt", "devyniasdešimt"];
    static readonly string[] HundredsMap = [string.Empty, "šimtas", "du šimtai", "trys šimtai", "keturi šimtai", "penki šimtai", "šeši šimtai", "septyni šimtai", "aštuoni šimtai", "devyni šimtai"];

    static readonly string[] OrdinalUnitsMap = [string.Empty, "pirm", "antr", "treči", "ketvirt", "penkt", "šešt", "septint", "aštunt", "devint", "dešimt", "vienuolikt", "dvylikt", "trylikt", "keturiolikt", "penkiolikt", "šešiolikt", "septyniolikt", "aštuoniolikt", "devyniolikt", "dvidešimt"];
    static readonly string[] OrdinalTensMap = [string.Empty, "dešimt", "dvidešimt", "trisdešimt", "keturiasdešimt", "penkiasdešimt", "šešiasdešimt", "septyniasdešimt", "aštuoniasdešimt", "devyniasdešimt"];
    static readonly string[] OrdinalHundredsMap = [string.Empty, "šimt", "du šimt", "trys šimt", "keturi šimt", "penki šimt", "šeši šimt", "septyni šimt", "aštuoni šimt", "devyni šimt"];

    public override string Convert(long input, GrammaticalGender gender, bool addAnd = true)
    {
        if (gender == GrammaticalGender.Neuter)
        {
            throw new NotSupportedException();
        }

        var parts = new List<string>();
        var number = input;

        HandleNegative(parts, ref number);
        CollectParts(parts, ref number, 1000000000000000000, GrammaticalGender.Masculine, "kvintilijonas", "kvintilijonai", "kvintilijonų");
        CollectParts(parts, ref number, 1000000000000000, GrammaticalGender.Masculine, "kvadrilijonas", "kvadrilijonai", "kvadrilijonų");
        CollectParts(parts, ref number, 1000000000000, GrammaticalGender.Masculine, "trilijonas", "trilijonai", "trilijonų");
        CollectParts(parts, ref number, 1000000000, GrammaticalGender.Masculine, "milijardas", "milijardai", "milijardų");
        CollectParts(parts, ref number, 1000000, GrammaticalGender.Masculine, "milijonas", "milijonai", "milijonų");
        CollectParts(parts, ref number, 1000, GrammaticalGender.Masculine, "tūkstantis", "tūkstančiai", "tūkstančių");
        CollectPartsUnderOneThousand(parts, number, gender);

        return string.Join(" ", parts);
    }

    public override string ConvertToOrdinal(int input, GrammaticalGender gender)
    {
        if (gender == GrammaticalGender.Neuter)
        {
            throw new NotSupportedException();
        }

        var parts = new List<string>();
        var number = (long)input;

        HandleNegative(parts, ref number);
        CollectOrdinalParts(parts, ref number, 1000000000, GrammaticalGender.Masculine, "milijard" + GetOrdinalEndingForGender(gender), "milijardas", "milijardai", "milijardų");
        CollectOrdinalParts(parts, ref number, 1000000, GrammaticalGender.Masculine, "milijon" + GetOrdinalEndingForGender(gender), "milijonas", "milijonai", "milijonų");
        CollectOrdinalParts(parts, ref number, 1000, GrammaticalGender.Masculine, "tūkstant" + GetOrdinalEndingForGender(gender), "tūkstantis", "tūkstančiai", "tūkstančių");
        CollectOrdinalPartsUnderOneThousand(parts, number, gender, true);

        return string.Join(" ", parts);
    }

    static void HandleNegative(List<string> parts, ref long number)
    {
        if (number < 0)
        {
            parts.Add("minus");
            number = -number;
        }
    }

    static void CollectParts(List<string> parts, ref long number, long divisor,
        GrammaticalGender gender, params string[] forms)
    {
        var result = number / divisor;
        if (result == 0)
        {
            return;
        }

        number %= divisor;

        if (result > 1)
        {
            CollectPartsUnderOneThousand(parts, result, gender);
        }

        parts.Add(ChooseForm(result, forms));
    }

    static void CollectOrdinalParts(List<string> parts, ref long number, long divisor,
        GrammaticalGender gender, string ordinalForm, params string[] forms)
    {
        var result = number / divisor;
        if (result == 0)
        {
            return;
        }

        number %= divisor;

        if (result > 1)
        {
            CollectOrdinalPartsUnderOneThousand(parts, result, gender);
        }

        parts.Add(ChooseCardinalOrOrdinalForm(result, ordinalForm, forms, useOrdinalForm: number == 0));
    }

    static void CollectPartsUnderOneThousand(List<string> parts, long number, GrammaticalGender gender)
    {
        if (number >= 100)
        {
            var hundreds = number / 100;
            number %= 100;
            parts.Add(HundredsMap[hundreds]);
        }

        if (number >= 20)
        {
            var tens = number / 10;
            parts.Add(TensMap[tens]);
            number %= 10;
        }

        if (number > 0 || parts.Count == 0)
        {
            parts.Add(GetCardinalNumberForGender(UnitsMap[number], gender));
        }
    }

    static void CollectOrdinalPartsUnderOneThousand(List<string> parts, long number,
        GrammaticalGender gender, bool lastNumber = false)
    {
        if (number >= 100)
        {
            var hundreds = number / 100;
            number %= 100;

            parts.Add(!lastNumber || number > 0
                ? HundredsMap[hundreds]
                : OrdinalHundredsMap[hundreds] + GetOrdinalEndingForGender(gender));
        }

        if (number >= 20)
        {
            var tens = number / 10;
            number %= 10;

            parts.Add(!lastNumber || number > 0
                ? TensMap[tens]
                : OrdinalTensMap[tens] + GetOrdinalEndingForGender(gender));
        }

        if (number > 0)
        {
            parts.Add(!lastNumber
                ? UnitsMap[number]
                : OrdinalUnitsMap[number] + GetOrdinalEndingForGender(gender));
        }
        else if (number == 0 && parts.Count == 0)
        {
            parts.Add(gender == GrammaticalGender.Masculine ? "nulinis" : "nulinė");
        }
    }

    static string ChooseForm(long number, string[] forms) =>
        forms[GetFormIndex(number)];

    static string ChooseCardinalOrOrdinalForm(long number, string ordinalForm, string[] cardinalForms,
        bool useOrdinalForm = false)
    {
        if (useOrdinalForm)
        {
            return ordinalForm;
        }

        return ChooseForm(number, cardinalForms);
    }

    static int GetFormIndex(long number)
    {
        var form = LithuanianNumberFormDetector.Detect(number);
        return form switch
        {
            LithuanianNumberForm.Singular => 0,
            LithuanianNumberForm.Plural => 1,
            LithuanianNumberForm.GenitivePlural => 2,
            _ => throw new ArgumentOutOfRangeException(nameof(number))
        };
    }

    static string GetCardinalNumberForGender(string number, GrammaticalGender gender)
    {
        if (gender == GrammaticalGender.Masculine)
        {
            return number;
        }

        if (gender == GrammaticalGender.Feminine)
        {
            if (number == "du")
            {
                return "dvi";
            }

            if (number.EndsWith("as"))
            {
                return number[..^1];
            }

            if (number.EndsWith('i'))
            {
                return number + "os";
            }

            return number;
        }

        throw new ArgumentOutOfRangeException(nameof(gender));
    }

    static string GetOrdinalEndingForGender(GrammaticalGender gender) =>
        gender switch
        {
            GrammaticalGender.Masculine => "as",
            GrammaticalGender.Feminine => "a",
            _ => throw new ArgumentOutOfRangeException(nameof(gender))
        };
}
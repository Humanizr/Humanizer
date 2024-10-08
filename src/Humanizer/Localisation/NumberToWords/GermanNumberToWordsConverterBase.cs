namespace Humanizer;

abstract class GermanNumberToWordsConverterBase : GenderedNumberToWordsConverter
{
    static readonly string[] UnitsMap = ["null", "ein", "zwei", "drei", "vier", "fünf", "sechs", "sieben", "acht", "neun", "zehn", "elf", "zwölf", "dreizehn", "vierzehn", "fünfzehn", "sechzehn", "siebzehn", "achtzehn", "neunzehn"];
    static readonly string[] TensMap = ["null", "zehn", "zwanzig", "dreißig", "vierzig", "fünfzig", "sechzig", "siebzig", "achtzig", "neunzig"];
    static readonly string[] UnitsOrdinal = [string.Empty, "ers", "zwei", "drit", "vier", "fünf", "sechs", "sieb", "ach", "neun", "zehn", "elf", "zwölf", "dreizehn", "vierzehn", "fünfzehn", "sechzehn", "siebzehn", "achtzehn", "neunzehn"];
    static readonly string[] HundredOrdinalSingular = ["einhundert"];
    static readonly string[] HundredOrdinalPlural = ["{0}hundert"];
    static readonly string[] ThousandOrdinalSingular = ["eintausend"];
    static readonly string[] ThousandOrdinalPlural = ["{0}tausend"];
    static readonly string[] MillionOrdinalSingular = ["einmillion", "einemillion"];
    static readonly string[] MillionOrdinalPlural = ["{0}million", "{0}millionen"];
    static readonly string[] BillionOrdinalSingular = ["einmilliard", "einemilliarde"];
    static readonly string[] BillionOrdinalPlural = ["{0}milliard", "{0}milliarden"];

    public override string Convert(long number, GrammaticalGender gender, bool addAnd = true)
    {
        if (number == 0)
        {
            return UnitsMap[number];
        }

        var parts = new List<string>();
        if (number < 0)
        {
            parts.Add("minus ");
            number = -number;
        }

        CollectParts(parts, ref number, 1000000000000000000, true, "{0} Trillionen", "eine Trillion");
        CollectParts(parts, ref number, 1000000000000000, true, "{0} Billiarden", "eine Billiarde");
        CollectParts(parts, ref number, 1000000000000, true, "{0} Billionen", "eine Billion");
        CollectParts(parts, ref number, 1000000000, true, "{0} Milliarden", "eine Milliarde");
        CollectParts(parts, ref number, 1000000, true, "{0} Millionen", "eine Million");
        CollectParts(parts, ref number, 1000, false, "{0}tausend", "eintausend");
        CollectParts(parts, ref number, 100, false, "{0}hundert", "einhundert");

        if (number > 0)
        {
            if (number < 20)
            {
                if (number == 1 && gender == GrammaticalGender.Feminine)
                {
                    parts.Add("eine");
                }
                else if (number == 1)
                {
                    parts.Add($"{UnitsMap[number]}s");
                }
                else
                {
                    parts.Add(UnitsMap[number]);
                }
            }
            else
            {
                var units = number % 10;
                if (units > 0)
                {
                    parts.Add($"{UnitsMap[units]}und");
                }

                parts.Add(GetTens(number / 10));
            }
        }

        return string.Concat(parts);
    }

    public override string ConvertToOrdinal(int number, GrammaticalGender gender)
    {
        if (number == 0)
        {
            return UnitsMap[number] + GetEndingForGender(gender);
        }

        var parts = new List<string>();
        if (number < 0)
        {
            parts.Add("minus ");
            number = -number;
        }

        CollectOrdinalParts(parts, ref number, 1000000000, true, BillionOrdinalPlural, BillionOrdinalSingular);
        CollectOrdinalParts(parts, ref number, 1000000, true, MillionOrdinalPlural, MillionOrdinalSingular);
        CollectOrdinalParts(parts, ref number, 1000, false, ThousandOrdinalPlural, ThousandOrdinalSingular);
        CollectOrdinalParts(parts, ref number, 100, false, HundredOrdinalPlural, HundredOrdinalSingular);

        if (number > 0)
        {
            parts.Add(number < 20 ? UnitsOrdinal[number] : Convert(number));
        }

        if (number is 0 or >= 20)
        {
            parts.Add("s");
        }

        parts.Add(GetEndingForGender(gender));

        return string.Concat(parts);
    }

    void CollectParts(List<string> parts, ref long number, long divisor, bool addSpaceBeforeNextPart, string pluralFormat, string singular)
    {
        if (number / divisor > 0)
        {
            parts.Add(Part(pluralFormat, singular, number / divisor));
            number %= divisor;
            if (addSpaceBeforeNextPart && number > 0)
            {
                parts.Add(" ");
            }
        }
    }

    void CollectOrdinalParts(List<string> parts, ref int number, int divisor, bool evaluateNoRest, string[] pluralFormats, string[] singulars)
    {
        if (number / divisor > 0)
        {
            var noRest = evaluateNoRest ? NoRestIndex(number % divisor) : 0;
            parts.Add(Part(pluralFormats[noRest], singulars[noRest], number / divisor));
            number %= divisor;
        }
    }

    string Part(string pluralFormat, string singular, long number)
    {
        if (number == 1)
        {
            return singular;
        }

        return string.Format(pluralFormat, Convert(number));
    }

    static int NoRestIndex(int number) =>
        number == 0 ? 0 : 1;

    static string GetEndingForGender(GrammaticalGender gender) =>
        gender switch
        {
            GrammaticalGender.Masculine => "ter",
            GrammaticalGender.Feminine => "te",
            GrammaticalGender.Neuter => "tes",
            _ => throw new ArgumentOutOfRangeException(nameof(gender))
        };

    protected virtual string GetTens(long tens) =>
        TensMap[tens];
}
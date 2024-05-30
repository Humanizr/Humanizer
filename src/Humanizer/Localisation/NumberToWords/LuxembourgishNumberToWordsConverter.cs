namespace Humanizer;

class LuxembourgishNumberToWordsConverter : GenderedNumberToWordsConverter
{
    static readonly string[] UnitsMap = ["null", "een", "zwee", "dräi", "véier", "fënnef", "sechs", "siwen", "aacht", "néng", "zéng", "eelef", "zwielef", "dräizéng", "véierzéng", "fofzéng", "siechzéng", "siwwenzéng", "uechtzéng", "nonzéng"];
    static readonly string[] TensMap = ["null", "zéng", "zwanzeg", "drësseg", "véierzeg", "fofzeg", "sechzeg", "siwwenzeg", "achtzeg", "nonzeg"];
    static readonly string[] UnitsOrdinal = [string.Empty, "éisch", "zwee", "drët", "véier", "fënnef", "sechs", "siwen", "aach", "néng", "zéng", "eelef", "zwielef", "dräizéng", "véierzéng", "fofzéng", "siechzéng", "siwwenzéng", "uechtzéng", "nonzéng"];
    static readonly string[] HundredOrdinalSingular = ["eenhonnert"];
    static readonly string[] HundredOrdinalPlural = ["{0}honnert"];
    static readonly string[] ThousandOrdinalSingular = ["eendausend"];
    static readonly string[] ThousandOrdinalPlural = ["{0}dausend"];
    static readonly string[] MillionOrdinalSingular = ["eemillioun", "engmillioun"];
    static readonly string[] MillionOrdinalPlural = ["{0}millioun", "{0}milliounen"];
    static readonly string[] BillionOrdinalSingular = ["eemilliard", "engmilliard"];
    static readonly string[] BillionOrdinalPlural = ["{0}milliard", "{0}milliarden"];

    public override string Convert(long number, GrammaticalGender gender, bool addAnd = true)
        => Convert(number, WordForm.Normal, gender, addAnd);

    public override string Convert(long number, WordForm wordForm, GrammaticalGender gender, bool addAnd = true)
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

        CollectParts(parts, ref number, 1000000000000000000, true, "{0} Trilliounen", "eng Trillioun");
        CollectParts(parts, ref number, 1000000000000000, true, "{0} Billiarden", "eng Billiard");
        CollectParts(parts, ref number, 1000000000000, true, "{0} Billiounen", "eng Billioun");
        CollectParts(parts, ref number, 1000000000, true, "{0} Milliarden", "eng Milliard");
        CollectParts(parts, ref number, 1000000, true, "{0} Milliounen", "eng Millioun");
        CollectParts(parts, ref number, 1000, false, "{0}dausend", "eendausend");
        CollectParts(parts, ref number, 100, false, "{0}honnert", "eenhonnert");

        if (number <= 0)
        {
            return string.Concat(parts);
        }

        if (number < 20)
        {
            switch (number)
            {
                case 1 when gender == GrammaticalGender.Feminine:
                    parts.Add("eng");
                    break;
                case 2 when gender == GrammaticalGender.Feminine:
                    parts.Add("zwou");
                    break;
                case 1 or 7:
                    parts.Add(wordForm is WordForm.Eifeler
                        ? LuxembourgishFormatter.ApplyEifelerRule(UnitsMap[number])
                        : UnitsMap[number]);
                    break;
                default:
                    parts.Add(UnitsMap[number]);
                    break;
            }
        }
        else
        {
            var units = number % 10;
            var tens = TensMap[number / 10];

            if (units > 0)
            {
                var andPart = LuxembourgishFormatter.CheckForAndApplyEifelerRule("an", tens);
                parts.Add($"{UnitsMap[units]}{andPart}");
            }

            parts.Add(tens);
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

    private void CollectParts(List<string> parts, ref long number, long divisor, bool addSpaceBeforeNextPart, string pluralFormat, string singular)
    {
        if (number / divisor <= 0)
        {
            return;
        }

        parts.Add(Part(pluralFormat, singular, number / divisor, divisor));
        number %= divisor;
        if (addSpaceBeforeNextPart && number > 0)
        {
            parts.Add(" ");
        }
    }

    private void CollectOrdinalParts(List<string> parts, ref int number, int divisor, bool evaluateNoRest, string[] pluralFormats, string[] singulars)
    {
        if (number / divisor <= 0)
        {
            return;
        }

        var noRest = evaluateNoRest ? NoRestIndex(number % divisor) : 0;
        parts.Add(Part(pluralFormats[noRest], singulars[noRest], number / divisor, divisor));
        number %= divisor;
    }

    private string Part(string pluralFormat, string singular, long number, long divisor) =>
        number switch
        {
            1 => singular,
            2 when divisor >= 1000000 => string.Format(pluralFormat, Convert(number, GrammaticalGender.Feminine)),
            7 => GetPartWithEifelerRule(pluralFormat, number, GrammaticalGender.Masculine),
            _ => string.Format(pluralFormat, Convert(number))
        };

    private static int NoRestIndex(int number) =>
        number == 0 ? 0 : 1;

    private static string GetEndingForGender(GrammaticalGender gender) =>
        gender switch
        {
            GrammaticalGender.Masculine => "ten",
            GrammaticalGender.Feminine => "t",
            GrammaticalGender.Neuter => "t",
            _ => throw new ArgumentOutOfRangeException(nameof(gender))
        };

    private string GetPartWithEifelerRule(string pluralFormat, long number, GrammaticalGender gender)
    {
        var nextWord = pluralFormat
            .AsSpan(3, pluralFormat.Length - 3)
            .TrimStart();
        var wordForm = LuxembourgishFormatter.DoesEifelerRuleApply(nextWord)
            ? WordForm.Eifeler
            : WordForm.Normal;
        return string.Format(pluralFormat, Convert(number, wordForm, gender));
    }
}

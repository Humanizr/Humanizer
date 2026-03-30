namespace Humanizer;

class FrenchFamilyNumberToWordsConverter(FrenchNumberToWordsProfile profile) : GenderedNumberToWordsConverter
{
    static readonly string[] UnitsMap = ["zéro", "un", "deux", "trois", "quatre", "cinq", "six", "sept", "huit", "neuf", "dix", "onze", "douze", "treize", "quatorze", "quinze", "seize", "dix-sept", "dix-huit", "dix-neuf"];
    static readonly string[] TensMap = ["zéro", "dix", "vingt", "trente", "quarante", "cinquante", "soixante", "septante", "octante", "nonante"];
    readonly FrenchNumberToWordsProfile profile = profile;

    public override string Convert(long number, GrammaticalGender gender, bool addAnd = true)
    {
        if (number == 0)
        {
            return UnitsMap[0];
        }

        var parts = new List<string>();

        if (number < 0)
        {
            parts.Add("moins");
            number = -number;
        }

        CollectParts(parts, ref number, 1000000000000000000, "trillion");
        CollectParts(parts, ref number, 1000000000000000, "billiard");
        CollectParts(parts, ref number, 1000000000000, "billion");
        CollectParts(parts, ref number, 1000000000, "milliard");
        CollectParts(parts, ref number, 1000000, "million");
        CollectThousands(parts, ref number, 1000, "mille");

        CollectPartsUnderAThousand(parts, number, gender, true);

        return string.Join(" ", parts);
    }

    public override string ConvertToOrdinal(int number, GrammaticalGender gender)
    {
        if (number == 1)
        {
            return gender == GrammaticalGender.Feminine ? "première" : "premier";
        }

        var convertedNumber = Convert(number);

        if (convertedNumber.EndsWith('s') && !convertedNumber.EndsWith("trois"))
        {
            convertedNumber = convertedNumber.TrimEnd('s');
        }
        else if (convertedNumber.EndsWith("cinq"))
        {
            convertedNumber += "u";
        }
        else if (convertedNumber.EndsWith("neuf"))
        {
            convertedNumber = convertedNumber.TrimEnd('f') + "v";
        }

        if (convertedNumber.StartsWith("un "))
        {
            convertedNumber = convertedNumber[3..];
        }

        if (number == 0)
        {
            convertedNumber += "t";
        }

        convertedNumber = convertedNumber.TrimEnd('e');
        convertedNumber += "ième";
        return convertedNumber;
    }

    protected static string GetUnits(long number, GrammaticalGender gender)
    {
        if (number == 1 && gender == GrammaticalGender.Feminine)
        {
            return "une";
        }

        return UnitsMap[number];
    }

    static void CollectHundreds(List<string> parts, ref long number, long d, string form, bool pluralize)
    {
        if (number < d)
        {
            return;
        }

        var result = number / d;
        if (result == 1)
        {
            parts.Add(form);
        }
        else
        {
            parts.Add(GetUnits(result, GrammaticalGender.Masculine));
            if (number % d == 0 && pluralize)
            {
                parts.Add(form + "s");
            }
            else
            {
                parts.Add(form);
            }
        }

        number %= d;
    }

    void CollectParts(List<string> parts, ref long number, long d, string form)
    {
        if (number < d)
        {
            return;
        }

        var result = number / d;

        CollectPartsUnderAThousand(parts, result, GrammaticalGender.Masculine, true);

        if (result == 1)
        {
            parts.Add(form);
        }
        else
        {
            parts.Add(form + "s");
        }

        number %= d;
    }

    void CollectPartsUnderAThousand(List<string> parts, long number, GrammaticalGender gender, bool pluralize)
    {
        CollectHundreds(parts, ref number, 100, "cent", pluralize);

        if (number > 0)
        {
            CollectPartsUnderAHundred(parts, ref number, gender, pluralize);
        }
    }

    void CollectThousands(List<string> parts, ref long number, int d, string form)
    {
        if (number < d)
        {
            return;
        }

        var result = number / d;
        if (result > 1)
        {
            CollectPartsUnderAThousand(parts, result, GrammaticalGender.Masculine, false);
        }

        parts.Add(form);

        number %= d;
    }

    void CollectPartsUnderAHundred(List<string> parts, ref long number, GrammaticalGender gender, bool pluralize)
    {
        if (number < 20)
        {
            parts.Add(GetUnits(number, gender));
        }
        else if (profile.Style == FrenchNumberingStyle.Metropolitan && number == 71)
        {
            parts.Add("soixante et onze");
        }
        else if (UsesQuatreVingt(number))
        {
            if (number == 80)
            {
                parts.Add(pluralize ? "quatre-vingts" : "quatre-vingt");
            }
            else if (profile.Style == FrenchNumberingStyle.Belgian && number == 81)
            {
                parts.Add(gender == GrammaticalGender.Feminine ? "quatre-vingt-une" : "quatre-vingt-un");
            }
            else if (profile.Style == FrenchNumberingStyle.Metropolitan && number >= 70)
            {
                var @base = number < 80 ? 60 : 80;
                var units = number - @base;
                var tens = @base / 10;
                parts.Add($"{GetTens(tens)}-{GetUnits(units, gender)}");
            }
            else
            {
                AppendStandardTens(parts, number, gender);
            }
        }
        else
        {
            AppendStandardTens(parts, number, gender);
        }
    }

    void AppendStandardTens(List<string> parts, long number, GrammaticalGender gender)
    {
        var units = number % 10;
        var tens = GetTens(number / 10);
        if (units == 0)
        {
            parts.Add(tens);
        }
        else if (units == 1)
        {
            parts.Add(tens);
            parts.Add("et");
            parts.Add(GetUnits(1, gender));
        }
        else
        {
            parts.Add($"{tens}-{GetUnits(units, gender)}");
        }
    }

    bool UsesQuatreVingt(long number) =>
        profile.Style switch
        {
            FrenchNumberingStyle.Metropolitan => number >= 70,
            FrenchNumberingStyle.Belgian => number is >= 80 and < 90,
            _ => false
        };

    string GetTens(long tens) =>
        tens == 8 && UsesQuatreVingtTens()
            ? "quatre-vingt"
            : TensMap[tens];

    bool UsesQuatreVingtTens() =>
        profile.Style == FrenchNumberingStyle.Metropolitan || profile.Style == FrenchNumberingStyle.Belgian;
}

enum FrenchNumberingStyle
{
    Swiss,
    Belgian,
    Metropolitan
}

sealed class FrenchNumberToWordsProfile(FrenchNumberingStyle style)
{
    public FrenchNumberingStyle Style { get; } = style;
}

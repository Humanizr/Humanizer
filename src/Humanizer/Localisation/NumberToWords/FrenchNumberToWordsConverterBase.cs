namespace Humanizer;

// Shared French-family engine. Locale differences in the 70/80/90 decades are generated decade strategies, not locale-name branches.
class FrenchFamilyNumberToWordsConverter(FrenchNumberToWordsProfile profile) : GenderedNumberToWordsConverter
{
    static readonly string[] UnitsMap = ["zéro", "un", "deux", "trois", "quatre", "cinq", "six", "sept", "huit", "neuf", "dix", "onze", "douze", "treize", "quatorze", "quinze", "seize", "dix-sept", "dix-huit", "dix-neuf"];
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
            parts.Add(profile.MinusWord);
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
        else if (profile.SeventyStrategy == FrenchSeventyStrategy.SixtyPlusTeens && number is >= 70 and < 80)
        {
            parts.Add(number == 71 && profile.SpecialSeventyOneWord.Length != 0
                ? profile.SpecialSeventyOneWord
                : $"{profile.TensMap[6]}-{GetUnits(number - 60, gender)}");
        }
        else if (profile.NinetyStrategy == FrenchNinetyStrategy.EightyPlusTeens && number is >= 90 and < 100)
        {
            parts.Add($"{profile.TensMap[8]}-{GetUnits(number - 80, gender)}");
        }
        else
        {
            AppendStandardTens(parts, number, gender, pluralize);
        }
    }

    // Exact 80-pluralization and "et un" join behavior now come from generated decade metadata.
    void AppendStandardTens(List<string> parts, long number, GrammaticalGender gender, bool pluralize)
    {
        var units = number % 10;
        var tensIndex = (int)(number / 10);
        var tens = profile.TensMap[tensIndex];
        if (units == 0)
        {
            parts.Add(number == 80 && pluralize && profile.PluralizeExactEighty
                ? tens + "s"
                : tens);
        }
        else if (units == 1 && profile.TensUsingEtWhenUnitIsOne.Contains(tensIndex))
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

}

// 70 can stay regular ("septante") or reuse 60-plus-teens ("soixante-dix") depending on locale profile data.
enum FrenchSeventyStrategy
{
    Regular,
    SixtyPlusTeens
}

// 90 can stay regular ("nonante") or reuse 80-plus-teens ("quatre-vingt-dix") depending on locale profile data.
enum FrenchNinetyStrategy
{
    Regular,
    EightyPlusTeens
}

// Carries the generated decade strategies for the French family so new locale variants are mostly profile edits.
sealed class FrenchNumberToWordsProfile(
    string minusWord,
    FrenchSeventyStrategy seventyStrategy,
    FrenchNinetyStrategy ninetyStrategy,
    string specialSeventyOneWord,
    bool pluralizeExactEighty,
    FrozenSet<int> tensUsingEtWhenUnitIsOne,
    string[] tensMap)
{
    public string MinusWord { get; } = minusWord;
    public FrenchSeventyStrategy SeventyStrategy { get; } = seventyStrategy;
    public FrenchNinetyStrategy NinetyStrategy { get; } = ninetyStrategy;
    public string SpecialSeventyOneWord { get; } = specialSeventyOneWord;
    public bool PluralizeExactEighty { get; } = pluralizeExactEighty;
    public FrozenSet<int> TensUsingEtWhenUnitIsOne { get; } = tensUsingEtWhenUnitIsOne;
    public string[] TensMap { get; } = tensMap;
}

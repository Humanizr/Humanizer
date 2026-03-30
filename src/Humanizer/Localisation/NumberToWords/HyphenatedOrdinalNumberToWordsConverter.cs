namespace Humanizer;

class HyphenatedOrdinalNumberToWordsConverter(HyphenatedOrdinalNumberToWordsConverter.Profile profile) : GenderedNumberToWordsConverter
{
    public override string Convert(long number, GrammaticalGender gender, bool addAnd = true)
    {
        if (number == 0)
        {
            return profile.ZeroWord;
        }

        if (number < 0)
        {
            return profile.NegativeWord + " " + Convert(-number, gender);
        }

        if (number < 10)
        {
            return GetUnit((int)number, gender);
        }

        if (number < 20)
        {
            return profile.Teens[number - 10];
        }

        if (number < 100)
        {
            return GetTens((int)number, gender);
        }

        if (number < 1000)
        {
            return GetHundreds((int)number, gender);
        }

        if (number < 1_000_000)
        {
            return GetThousands((int)number, gender);
        }

        if (number < 1_000_000_000)
        {
            return GetMillions((int)number, gender);
        }

        throw new NotImplementedException();
    }

    public override string ConvertToOrdinal(int number, GrammaticalGender gender)
    {
        if (number < 0)
        {
            return profile.NegativeWord + " " + ConvertToOrdinal(-number, gender);
        }

        if (number == 0)
        {
            return profile.ZeroWord;
        }

        var exactOrdinals = gender == GrammaticalGender.Feminine
            ? profile.OrdinalFeminine
            : profile.OrdinalMasculine;

        if (number < exactOrdinals.Length && !string.IsNullOrEmpty(exactOrdinals[number]))
        {
            return exactOrdinals[number];
        }

        if (number < 100)
        {
            return GetOrdinalTens(number, gender);
        }

        if (number < 1000)
        {
            return GetOrdinalHundreds(number, gender);
        }

        if (number < 1_000_000)
        {
            return GetOrdinalThousands(number, gender);
        }

        if (number < 1_000_000_000)
        {
            return GetOrdinalMillions(number, gender);
        }

        throw new NotImplementedException();
    }

    public override string ConvertToOrdinal(int number, GrammaticalGender gender, WordForm wordForm)
    {
        if (wordForm != WordForm.Abbreviation)
        {
            return ConvertToOrdinal(number, gender);
        }

        if (profile.OrdinalAbbreviations.TryGetValue(GetOrdinalAbbreviationKey(number, gender), out var abbreviation))
        {
            return abbreviation;
        }

        return number + (gender == GrammaticalGender.Feminine
            ? profile.DefaultOrdinalAbbreviationFeminine
            : GetMasculineOrdinalAbbreviationSuffix(number));
    }

    public override string ConvertToTuple(int number)
    {
        number = Math.Abs(number);
        return number < profile.TupleMap.Length
            ? profile.TupleMap[number]
            : Convert(number) + " " + profile.TupleFallbackWord;
    }

    string GetUnit(int number, GrammaticalGender gender) =>
        gender == GrammaticalGender.Feminine ? profile.UnitsFeminine[number] : profile.UnitsMasculine[number];

    string GetTens(int number, GrammaticalGender gender)
    {
        var tens = number / 10;
        var units = number % 10;
        if (units == 0)
        {
            return profile.Tens[tens];
        }

        return profile.Tens[tens] + GetTensJoiner(tens) + GetCompoundUnit(units, gender);
    }

    string GetHundreds(int number, GrammaticalGender gender)
    {
        var hundreds = number / 100;
        var remainder = number % 100;
        var hundredPart = gender == GrammaticalGender.Feminine ? profile.HundredsFeminine[hundreds] : profile.HundredsMasculine[hundreds];
        if (remainder == 0)
        {
            return hundredPart;
        }

        return hundredPart + " " + (remainder < 10 ? GetCompoundUnit(remainder, gender) : GetTens(remainder, gender));
    }

    string GetThousands(int number, GrammaticalGender gender)
    {
        var thousands = number / 1000;
        var remainder = number % 1000;
        var thousandPart = thousands == 1
            ? profile.ThousandWord
            : Convert(thousands, gender) + " " + profile.ThousandWord;
        if (remainder == 0)
        {
            return thousandPart;
        }

        return thousandPart + " " + (remainder == 1 && gender == GrammaticalGender.Masculine ? profile.MasculineCompoundOne : Convert(remainder, gender));
    }

    string GetMillions(int number, GrammaticalGender gender)
    {
        var millions = number / 1_000_000;
        var remainder = number % 1_000_000;
        var millionPart = millions == 1
            ? profile.MillionSingularPrefix + " " + profile.MillionSingular
            : Convert(millions, GrammaticalGender.Masculine) + " " + profile.MillionPlural;
        if (remainder == 0)
        {
            return millionPart;
        }

        return millionPart + " " + (remainder == 1 && gender == GrammaticalGender.Masculine ? profile.MasculineCompoundOne : Convert(remainder, gender));
    }

    string GetOrdinalTens(int number, GrammaticalGender gender)
    {
        var tens = number / 10;
        var remainder = number % 10;
        var suffix = gender == GrammaticalGender.Feminine ? profile.FeminineTensOrdinalSuffix : profile.MasculineTensOrdinalSuffix;
        if (remainder == 0)
        {
            var tensStem = profile.OrdinalTensStems[tens];
            return tensStem + suffix;
        }

        var unitOrdinal = GetOrdinalUnitComponent(remainder, gender);
        return profile.Tens[tens] + GetTensJoiner(tens) + unitOrdinal;
    }

    string GetOrdinalHundreds(int number, GrammaticalGender gender)
    {
        var hundreds = number / 100;
        var remainder = number % 100;
        var hundredPart = gender == GrammaticalGender.Feminine ? profile.HundredsFeminine[hundreds] : profile.HundredsMasculine[hundreds];
        if (remainder == 0 && hundreds == 1)
        {
            return hundredPart + (gender == GrammaticalGender.Feminine ? profile.FeminineTensOrdinalSuffix : profile.MasculineTensOrdinalSuffix);
        }

        if (remainder == 0)
        {
            return hundredPart;
        }

        string rest;
        if (hundreds == 1 && remainder % 10 != 0)
        {
            rest = ConvertToOrdinal(remainder, gender);
        }
        else
        {
            rest = Convert(remainder, gender);
            if (gender == GrammaticalGender.Masculine && remainder != 1 && remainder % 10 == 1)
            {
                rest += profile.MasculineOrdinalAppender;
            }
        }

        return hundredPart + " " + rest;
    }

    string GetOrdinalThousands(int number, GrammaticalGender gender)
    {
        var thousands = number / 1000;
        var remainder = number % 1000;
        var thousandPart = thousands == 1 ? profile.ThousandWord : Convert(thousands, gender) + " " + profile.ThousandWord;
        if (remainder == 0)
        {
            return thousandPart;
        }

        if (remainder == 100)
        {
            return thousandPart + " " + profile.HundredsMasculine[1];
        }

        var rest = Convert(remainder, gender);
        if (gender == GrammaticalGender.Masculine && remainder != 1 && remainder % 10 == 1)
        {
            rest += profile.MasculineOrdinalAppender;
        }

        return thousandPart + " " + rest;
    }

    string GetOrdinalMillions(int number, GrammaticalGender gender)
    {
        var millions = number / 1_000_000;
        var remainder = number % 1_000_000;
        var millionPart = millions == 1
            ? profile.MillionSingularPrefix + " " + profile.MillionSingular
            : Convert(millions, GrammaticalGender.Masculine) + " " + profile.MillionPlural;
        if (remainder == 0)
        {
            return millionPart;
        }

        var rest = Convert(remainder, gender);
        if (gender == GrammaticalGender.Masculine && remainder != 1 && remainder % 10 == 1)
        {
            rest += profile.MasculineOrdinalAppender;
        }

        return millionPart + " " + rest;
    }

    string GetCompoundUnit(int number, GrammaticalGender gender) =>
        number == 1 && gender == GrammaticalGender.Masculine
            ? profile.MasculineCompoundOne
            : GetUnit(number, gender);

    string GetOrdinalUnitComponent(int number, GrammaticalGender gender) =>
        number == 1
            ? (gender == GrammaticalGender.Feminine ? profile.FeminineOrdinalOne : profile.MasculineOrdinalOne)
            : profile.OrdinalUnitComponents[number] + (gender == GrammaticalGender.Feminine
                ? profile.FeminineTensOrdinalSuffix
                : profile.MasculineTensOrdinalSuffix);

    string GetTensJoiner(int tens) =>
        tens == profile.SpecialJoinerTensValue ? profile.SpecialTensJoiner : profile.DefaultTensJoiner;

    static string GetOrdinalAbbreviationKey(int number, GrammaticalGender gender) =>
        number.ToString(CultureInfo.InvariantCulture) + "|" + gender;

    string GetMasculineOrdinalAbbreviationSuffix(int number) =>
        (number % 10) switch
        {
            1 or 3 => "r",
            2 or 7 => "n",
            _ => profile.DefaultOrdinalAbbreviationMasculine
        };

    public sealed record Profile(
        string ZeroWord,
        string NegativeWord,
        string ThousandWord,
        string MillionSingularPrefix,
        string MillionSingular,
        string MillionPlural,
        string MasculineCompoundOne,
        string FeminineCompoundOne,
        string MasculineOrdinalOne,
        string FeminineOrdinalOne,
        string DefaultTensJoiner,
        string SpecialTensJoiner,
        int SpecialJoinerTensValue,
        string MasculineTensOrdinalSuffix,
        string FeminineTensOrdinalSuffix,
        string MasculineOrdinalAppender,
        string DefaultOrdinalAbbreviationMasculine,
        string DefaultOrdinalAbbreviationFeminine,
        string TupleFallbackWord,
        string[] UnitsMasculine,
        string[] UnitsFeminine,
        string[] Teens,
        string[] Tens,
        string[] HundredsMasculine,
        string[] HundredsFeminine,
        string[] OrdinalMasculine,
        string[] OrdinalFeminine,
        string[] OrdinalTensStems,
        string[] OrdinalUnitComponents,
        string[] TupleMap,
        FrozenDictionary<string, string> OrdinalAbbreviations);
}

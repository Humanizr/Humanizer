namespace Humanizer;

class LongScaleStemOrdinalNumberToWordsConverter(LongScaleStemOrdinalNumberToWordsConverter.Profile profile) : GenderedNumberToWordsConverter
{
    public override string Convert(long number, GrammaticalGender gender, bool addAnd = true) =>
        Convert(number, WordForm.Normal, gender, addAnd);

    public override string Convert(long number, WordForm wordForm, GrammaticalGender gender, bool addAnd = true)
    {
        if (number == 0)
        {
            return profile.ZeroWord;
        }

        if (number == long.MinValue)
        {
            return profile.MinLongValueWord;
        }

        if (number < 0)
        {
            return profile.NegativeWord + " " + Convert(-number);
        }

        List<string> wordBuilder =
        [
            ConvertGreaterThanHighestScale(number, out var remainder),
            ConvertThousands(remainder, out remainder, gender),
            ConvertHundreds(remainder, out remainder, gender),
            ConvertUnits(remainder, gender, wordForm)
        ];

        return BuildWord(wordBuilder);
    }

    public override string ConvertToOrdinal(int number, GrammaticalGender gender) =>
        ConvertToOrdinal(number, gender, WordForm.Normal);

    public override string ConvertToOrdinal(int number, GrammaticalGender gender, WordForm wordForm)
    {
        if (number is 0 or int.MinValue)
        {
            return profile.ZeroWord;
        }

        if (number < 0)
        {
            return ConvertToOrdinal(Math.Abs(number), gender);
        }

        if (IsRoundHigherScale(number))
        {
            return ConvertRoundHigherScaleOrdinal(number, gender);
        }

        if (IsRoundHighestScale(number))
        {
            return ConvertToOrdinal(number / 1000, gender)
                .Replace(profile.HighestScaleOrdinalSource, profile.HighestScaleOrdinalTarget);
        }

        List<string> wordBuilder =
        [
            ConvertHigherThousandsOrdinal(number, out var remainder, gender),
            ConvertMappedOrdinalNumber(remainder, 1000, profile.ThousandthsRoots, out remainder, gender),
            ConvertMappedOrdinalNumber(remainder, 100, profile.HundredthsRoots, out remainder, gender),
            ConvertMappedOrdinalNumber(remainder, 10, profile.TenthsRoots, out remainder, gender),
            ConvertOrdinalUnits(remainder, gender, wordForm)
        ];

        return BuildWord(wordBuilder);
    }

    public override string ConvertToTuple(int number)
    {
        number = Math.Abs(number);
        return number < profile.TupleMap.Length
            ? profile.TupleMap[number]
            : Convert(number) + profile.TupleFallbackSuffix;
    }

    string ConvertGreaterThanHighestScale(long inputNumber, out long remainder)
    {
        List<string> wordBuilder = [];
        remainder = inputNumber;

        foreach (var scale in profile.LargeScales)
        {
            if (remainder / scale.Value <= 0)
            {
                continue;
            }

            if (remainder / scale.Value == 1)
            {
                wordBuilder.Add(scale.SingularPrefix + " " + scale.Singular);
            }
            else
            {
                var count = remainder / scale.Value;
                var countWords = count % 10 == 1
                    ? Convert(count, WordForm.Abbreviation, GrammaticalGender.Masculine)
                    : Convert(count);
                wordBuilder.Add(countWords + " " + scale.Plural);
            }

            remainder %= scale.Value;
        }

        return BuildWord(wordBuilder);
    }

    string ConvertThousands(long inputNumber, out long remainder, GrammaticalGender gender)
    {
        remainder = inputNumber;
        if (inputNumber / 1000 <= 0)
        {
            return string.Empty;
        }

        if (inputNumber / 1000 == 1)
        {
            remainder = inputNumber % 1000;
            return profile.ThousandWord;
        }

        var count = inputNumber / 1000;
        remainder = inputNumber % 1000;
        var countWords = gender == GrammaticalGender.Feminine
            ? Convert(count, GrammaticalGender.Feminine)
            : Convert(count, WordForm.Abbreviation, gender);
        return countWords + " " + profile.ThousandWord;
    }

    string ConvertHigherThousandsOrdinal(int number, out int remainder, GrammaticalGender gender)
    {
        remainder = number;
        if (number / 10000 <= 0)
        {
            return string.Empty;
        }

        var wordPart = Convert(number / 1000 * 1000, gender);

        if (number < 30000 || IsRoundNumber(number))
        {
            if (number == 21000)
            {
                wordPart = wordPart.Replace("a", string.Empty)
                    .Replace("ú", "u");
            }

            wordPart = wordPart.Remove(wordPart.LastIndexOf(' '), 1);
        }

        remainder = number % 1000;
        return wordPart + profile.ThousandOrdinalStem + GetGenderedOrdinalEnding(gender);
    }

    string ConvertHundreds(long inputNumber, out long remainder, GrammaticalGender gender)
    {
        remainder = inputNumber;
        if (inputNumber / 100 <= 0)
        {
            return string.Empty;
        }

        remainder = inputNumber % 100;
        return inputNumber == 100
            ? profile.ExactHundredWord
            : GetHundredsMap(gender)[inputNumber / 100];
    }

    string ConvertUnits(long inputNumber, GrammaticalGender gender, WordForm wordForm)
    {
        if (inputNumber <= 0)
        {
            return string.Empty;
        }

        var unitsMap = GetUnitsMap(gender, wordForm);
        if (inputNumber < 30)
        {
            return unitsMap[inputNumber];
        }

        var wordPart = profile.TensMap[inputNumber / 10];
        return inputNumber % 10 <= 0
            ? wordPart
            : wordPart + " " + profile.TensJoiner + " " + unitsMap[inputNumber % 10];
    }

    string ConvertOrdinalUnits(int number, GrammaticalGender gender, WordForm wordForm)
    {
        if (number is <= 0 or >= 10)
        {
            return string.Empty;
        }

        var root = profile.OrdinalUnitRoots[number];
        return gender switch
        {
            GrammaticalGender.Feminine => root + profile.FeminineOrdinalEnding,
            GrammaticalGender.Masculine or GrammaticalGender.Neuter when HasOrdinalAbbreviation(number, wordForm) => root,
            _ => root + profile.MasculineOrdinalEnding
        };
    }

    string ConvertMappedOrdinalNumber(int number, int divisor, string[] map, out int remainder, GrammaticalGender gender)
    {
        remainder = number;
        if (number / divisor <= 0)
        {
            return string.Empty;
        }

        remainder = number % divisor;
        return map[number / divisor] + GetGenderedOrdinalEnding(gender);
    }

    string[] GetHundredsMap(GrammaticalGender gender) =>
        gender == GrammaticalGender.Feminine ? profile.HundredsFeminine : profile.HundredsMasculine;

    string[] GetUnitsMap(GrammaticalGender gender, WordForm wordForm) =>
        gender switch
        {
            GrammaticalGender.Feminine => profile.UnitsFeminine,
            GrammaticalGender.Masculine or GrammaticalGender.Neuter when wordForm == WordForm.Abbreviation => profile.UnitsMasculineAbbreviation,
            _ => profile.UnitsMasculine
        };

    string ConvertRoundHigherScaleOrdinal(int number, GrammaticalGender gender)
    {
        var cardinalPart = Convert(number / profile.HighestScaleValue, WordForm.Abbreviation, gender);
        var separator = number == profile.RoundHigherScaleCompactValue ? string.Empty : " ";
        var ordinalPart = ConvertToOrdinal((int)profile.HighestScaleValue, gender);
        return cardinalPart + separator + ordinalPart;
    }

    string GetGenderedOrdinalEnding(GrammaticalGender gender) =>
        gender == GrammaticalGender.Feminine ? profile.FeminineOrdinalEnding : profile.MasculineOrdinalEnding;

    static bool HasOrdinalAbbreviation(int number, WordForm wordForm) =>
        number is 1 or 3 && wordForm == WordForm.Abbreviation;

    bool IsRoundHigherScale(int number) =>
        number >= profile.RoundHigherScaleCompactValue && number % profile.HighestScaleValue == 0;

    bool IsRoundHighestScale(int number) =>
        number >= profile.HighestScaleValue && number % profile.HighestScaleValue == 0;

    static string BuildWord(List<string> wordParts)
    {
        wordParts.RemoveAll(string.IsNullOrEmpty);
        return string.Join(" ", wordParts);
    }

    static bool IsRoundNumber(int number) =>
        (number % 10000 == 0 && number < 100000)
        || (number % 100000 == 0 && number < 1000000)
        || (number % 1000000 == 0 && number < 10000000)
        || (number % 10000000 == 0 && number < 100000000)
        || (number % 100000000 == 0 && number < 1000000000)
        || (number % 1000000000 == 0 && number < int.MaxValue);

    public sealed record Profile(
        string ZeroWord,
        string NegativeWord,
        string ExactHundredWord,
        string ThousandWord,
        string TensJoiner,
        string ThousandOrdinalStem,
        string MasculineOrdinalEnding,
        string FeminineOrdinalEnding,
        string MinLongValueWord,
        long HighestScaleValue,
        int RoundHigherScaleCompactValue,
        string HighestScaleOrdinalSource,
        string HighestScaleOrdinalTarget,
        string TupleFallbackSuffix,
        string[] HundredsMasculine,
        string[] HundredsFeminine,
        string[] HundredthsRoots,
        string[] OrdinalUnitRoots,
        string[] TensMap,
        string[] TenthsRoots,
        string[] ThousandthsRoots,
        string[] TupleMap,
        string[] UnitsMasculine,
        string[] UnitsMasculineAbbreviation,
        string[] UnitsFeminine,
        LargeScale[] LargeScales);

    public sealed record LargeScale(long Value, string SingularPrefix, string Singular, string Plural);
}

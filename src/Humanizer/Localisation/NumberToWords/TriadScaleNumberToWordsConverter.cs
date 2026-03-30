namespace Humanizer;

class TriadScaleNumberToWordsConverter(TriadScaleNumberToWordsConverter.Profile profile) : GenderedNumberToWordsConverter
{
    public override string Convert(long input, GrammaticalGender gender, bool addAnd = true)
    {
        if (input is > int.MaxValue or < int.MinValue)
        {
            throw new NotImplementedException();
        }

        var number = (int)input;
        if (number < 0)
        {
            return profile.MinusWord + " " + Convert(Math.Abs(number), gender);
        }

        if (number == 0)
        {
            return profile.ZeroWord;
        }

        if (gender == GrammaticalGender.Feminine && number == 1)
        {
            return profile.FeminineOneWord;
        }

        Span<int> parts = stackalloc int[profile.Scales.Length + 1];
        var count = SplitEveryThreeDigits(number, parts);
        var words = string.Empty;

        for (var index = 0; index < count; index++)
        {
            var part = parts[index];
            if (part == 0)
            {
                continue;
            }

            words = (index == 0 ? ConvertTriad(part, true) : ConvertScalePart(index, part)) + words;
        }

        return words.TrimEnd();
    }

    public override string ConvertToOrdinal(int number, GrammaticalGender gender)
    {
        if (number == 0)
        {
            return profile.ZeroWord;
        }

        if (number <= 9)
        {
            return profile.OrdinalUnderTen[number] + GetOrdinalGenderSuffix(gender);
        }

        var words = Convert(number, gender);
        if (number % 100 == 10)
        {
            return words[..^profile.TenWord.Length] + profile.TenOrdinalStem + GetOrdinalGenderSuffix(gender);
        }

        words = words[..^1];
        words = ApplyOrdinalVowelRestoration(words, number % 10);
        words = ApplyExactScaleOrdinalTransforms(words, number);

        return words + profile.CommonOrdinalStem + GetOrdinalGenderSuffix(gender);
    }

    static int SplitEveryThreeDigits(int number, Span<int> parts)
    {
        var count = 0;
        var remaining = number;

        while (remaining > 0)
        {
            parts[count++] = remaining % 1000;
            remaining /= 1000;
        }

        return count;
    }

    string ConvertScalePart(int scaleIndex, int number)
    {
        var scale = profile.Scales[scaleIndex - 1];
        if (number == 1)
        {
            return scale.Singular;
        }

        var countText = ConvertTriad(number, scale.CountUsesFinalAccent);
        var joined = string.IsNullOrEmpty(scale.CountToScaleJoiner)
            ? countText + scale.Plural
            : countText + scale.CountToScaleJoiner + scale.Plural;

        return scale.AppendTrailingSpace ? joined + " " : joined;
    }

    string GetOrdinalGenderSuffix(GrammaticalGender gender) =>
        gender == GrammaticalGender.Feminine ? profile.FeminineOrdinalSuffix : profile.MasculineOrdinalSuffix;

    string ApplyOrdinalVowelRestoration(string words, int lastUnit) =>
        lastUnit switch
        {
            3 => words + profile.OrdinalUnit3RestoredVowel,
            6 => words + profile.OrdinalUnit6RestoredVowel,
            _ => words
        };

    string ApplyExactScaleOrdinalTransforms(string words, int number)
    {
        for (var index = profile.Scales.Length - 1; index >= 0; index--)
        {
            var scale = profile.Scales[index];
            if (number < scale.Value || number % scale.Value != 0)
            {
                continue;
            }

            if (!string.IsNullOrEmpty(scale.OrdinalCompactionMatch))
            {
                words = words.Replace(scale.OrdinalCompactionMatch, scale.OrdinalCompactionReplacement);
            }

            if (scale.RemoveLeadingOneOnExactOrdinal && number == scale.Value)
            {
                words = words.Replace(profile.LeadingOneWord, string.Empty);
            }

            if (!string.IsNullOrEmpty(scale.ExactOrdinalSuffix) && number > scale.Value)
            {
                words += scale.ExactOrdinalSuffix;
            }

            break;
        }

        return words;
    }

    string ConvertTriad(int number, bool thisIsLastSet)
    {
        if (number == 0)
        {
            return string.Empty;
        }

        var tensAndUnits = number % 100;
        var hundreds = number / 100;
        var units = tensAndUnits % 10;
        var tens = tensAndUnits / 10;

        var words = string.Empty;
        words += profile.HundredsMap[hundreds];
        words += profile.TensMap[tens];

        if (tensAndUnits <= 9)
        {
            words += profile.UnitsMap[tensAndUnits];
        }
        else if (tensAndUnits <= 19)
        {
            words += profile.TeensMap[tensAndUnits - 10];
        }
        else
        {
            if (units is 1 or 8)
            {
                words = words[..^1];
            }

            var unitWord = thisIsLastSet && units == 3
                ? profile.UnitsFinalAccent[units]
                : profile.UnitsMap[units];

            words += unitWord;
        }

        return words;
    }

    public sealed record Profile(
        string ZeroWord,
        string MinusWord,
        string FeminineOneWord,
        string LeadingOneWord,
        string TenWord,
        string TenOrdinalStem,
        string CommonOrdinalStem,
        string MasculineOrdinalSuffix,
        string FeminineOrdinalSuffix,
        string OrdinalUnit3RestoredVowel,
        string OrdinalUnit6RestoredVowel,
        string[] UnitsMap,
        string[] UnitsFinalAccent,
        string[] TensMap,
        string[] TeensMap,
        string[] HundredsMap,
        string[] OrdinalUnderTen,
        TriadScale[] Scales);

    public sealed record TriadScale(
        int Value,
        string Singular,
        string Plural,
        string CountToScaleJoiner,
        bool CountUsesFinalAccent,
        bool AppendTrailingSpace,
        string OrdinalCompactionMatch,
        string OrdinalCompactionReplacement,
        bool RemoveLeadingOneOnExactOrdinal,
        string ExactOrdinalSuffix);
}

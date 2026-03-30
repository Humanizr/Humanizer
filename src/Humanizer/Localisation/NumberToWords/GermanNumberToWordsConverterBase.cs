namespace Humanizer;

class GermanFamilyNumberToWordsConverter(GermanNumberToWordsProfile profile) : GenderedNumberToWordsConverter
{
    readonly GermanNumberToWordsProfile profile = profile;

    public override string Convert(long number, GrammaticalGender gender, bool addAnd = true) =>
        Convert(number, WordForm.Normal, gender, addAnd);

    public override string Convert(long number, WordForm wordForm, GrammaticalGender gender, bool addAnd = true)
    {
        if (number == 0)
        {
            return profile.ZeroWord;
        }

        var parts = new List<string>();
        if (number < 0)
        {
            parts.Add(profile.MinusWord);
            number = -number;
        }

        foreach (var scale in profile.Scales)
        {
            CollectParts(parts, ref number, scale);
        }

        if (number > 0)
        {
            parts.Add(ConvertUnderOneHundred((int)number, wordForm, gender));
        }

        return string.Concat(parts);
    }

    public override string ConvertToOrdinal(int number, GrammaticalGender gender)
    {
        if (number == 0)
        {
            return profile.ZeroWord + GetEndingForGender(gender);
        }

        var parts = new List<string>();
        long remaining = number;
        if (remaining < 0)
        {
            parts.Add(profile.MinusWord);
            remaining = -remaining;
        }

        foreach (var scale in profile.Scales)
        {
            CollectOrdinalParts(parts, ref remaining, scale);
        }

        if (remaining > 0)
        {
            var residual = (int)remaining;
            parts.Add(residual < 20 ? profile.UnitsOrdinal[residual] : Convert(residual));
        }

        if (remaining is 0 or >= 20)
        {
            parts.Add(profile.OrdinalStemSuffix);
        }

        parts.Add(GetEndingForGender(gender));

        return string.Concat(parts);
    }

    void CollectParts(List<string> parts, ref long number, GermanFamilyScale scale)
    {
        var count = number / scale.Value;
        if (count == 0)
        {
            return;
        }

        parts.Add(BuildCardinalScalePart(count, scale));
        number %= scale.Value;
        if (scale.AddSpaceBeforeNextPart && number > 0)
        {
            parts.Add(" ");
        }
    }

    void CollectOrdinalParts(List<string> parts, ref long number, GermanFamilyScale scale)
    {
        var count = number / scale.Value;
        if (count == 0)
        {
            return;
        }

        var hasRemainder = number % scale.Value != 0;
        parts.Add(BuildOrdinalScalePart(count, scale, hasRemainder));
        number %= scale.Value;
    }

    string BuildCardinalScalePart(long count, GermanFamilyScale scale)
    {
        if (count == 1)
        {
            return scale.SingularCardinal;
        }

        return string.Format(
            scale.PluralCardinalFormat,
            ConvertCount(count, scale));
    }

    string BuildOrdinalScalePart(long count, GermanFamilyScale scale, bool hasRemainder)
    {
        if (count == 1)
        {
            return scale.OrdinalSingular[hasRemainder ? 1 : 0];
        }

        return string.Format(
            scale.OrdinalPlural[hasRemainder ? 1 : 0],
            ConvertCount(count, scale));
    }

    string ConvertCount(long count, GermanFamilyScale scale)
    {
        var wordForm = profile.TensJoinerTransform == GermanicTensJoinerTransform.Eifeler &&
                       scale.CountWordFormNextWord is { Length: > 0 } nextWord &&
                       EifelerRule.DoesApply(nextWord)
            ? WordForm.Eifeler
            : WordForm.Normal;

        return Convert(count, wordForm, scale.CountGender);
    }

    string ConvertUnderOneHundred(int number, WordForm wordForm, GrammaticalGender gender)
    {
        if (number < 20)
        {
            return GetUnit(number, gender, wordForm);
        }

        var units = number % 10;
        var tensWord = profile.TensMap[number / 10];
        if (units == 0)
        {
            return tensWord;
        }

        return GetCompoundUnit(units) + GetTensJoiner(tensWord) + tensWord;
    }

    string GetUnit(int number, GrammaticalGender gender, WordForm wordForm)
    {
        if (number == 1)
        {
            return gender switch
            {
                GrammaticalGender.Feminine => profile.FeminineOne,
                GrammaticalGender.Neuter => profile.NeuterOne,
                _ => profile.MasculineOne
            };
        }

        if (number == 2 &&
            gender == GrammaticalGender.Feminine &&
            profile.FeminineTwo is not null)
        {
            return profile.FeminineTwo;
        }

        var unit = profile.UnitsMap[number];
        return profile.SupportsEifelerRule &&
               wordForm == WordForm.Eifeler &&
               number is 1 or 7
            ? EifelerRule.Apply(unit)
            : unit;
    }

    string GetCompoundUnit(int number) => profile.CompoundUnitsMap[number];

    string GetTensJoiner(string nextWord) =>
        profile.TensJoinerTransform switch
        {
            GermanicTensJoinerTransform.None => profile.TensJoiner,
            GermanicTensJoinerTransform.Eifeler => EifelerRule.ApplyIfNeeded(profile.TensJoiner, nextWord),
            _ => throw new InvalidOperationException("Unknown Germanic tens joiner transform.")
        };

    string GetEndingForGender(GrammaticalGender gender) =>
        gender switch
        {
            GrammaticalGender.Masculine => profile.MasculineOrdinalEnding,
            GrammaticalGender.Feminine => profile.FeminineOrdinalEnding,
            GrammaticalGender.Neuter => profile.NeuterOrdinalEnding,
            _ => throw new ArgumentOutOfRangeException(nameof(gender))
        };
}

sealed class GermanNumberToWordsProfile(
    string zeroWord,
    string minusWord,
    string masculineOne,
    string feminineOne,
    string neuterOne,
    string? feminineTwo,
    string tensJoiner,
    GermanicTensJoinerTransform tensJoinerTransform,
    string ordinalStemSuffix,
    string masculineOrdinalEnding,
    string feminineOrdinalEnding,
    string neuterOrdinalEnding,
    bool supportsEifelerRule,
    string[] unitsMap,
    string[] compoundUnitsMap,
    string[] tensMap,
    string[] unitsOrdinal,
    GermanFamilyScale[] scales)
{
    public string ZeroWord { get; } = zeroWord;
    public string MinusWord { get; } = minusWord;
    public string MasculineOne { get; } = masculineOne;
    public string FeminineOne { get; } = feminineOne;
    public string NeuterOne { get; } = neuterOne;
    public string? FeminineTwo { get; } = feminineTwo;
    public string TensJoiner { get; } = tensJoiner;
    public GermanicTensJoinerTransform TensJoinerTransform { get; } = tensJoinerTransform;
    public string OrdinalStemSuffix { get; } = ordinalStemSuffix;
    public string MasculineOrdinalEnding { get; } = masculineOrdinalEnding;
    public string FeminineOrdinalEnding { get; } = feminineOrdinalEnding;
    public string NeuterOrdinalEnding { get; } = neuterOrdinalEnding;
    public bool SupportsEifelerRule { get; } = supportsEifelerRule;
    public string[] UnitsMap { get; } = unitsMap;
    public string[] CompoundUnitsMap { get; } = compoundUnitsMap;
    public string[] TensMap { get; } = tensMap;
    public string[] UnitsOrdinal { get; } = unitsOrdinal;
    public GermanFamilyScale[] Scales { get; } = scales;
}

readonly record struct GermanFamilyScale(
    long Value,
    bool AddSpaceBeforeNextPart,
    GrammaticalGender CountGender,
    string SingularCardinal,
    string PluralCardinalFormat,
    string[] OrdinalSingular,
    string[] OrdinalPlural,
    string? CountWordFormNextWord = null);

enum GermanicTensJoinerTransform
{
    None,
    Eifeler
}

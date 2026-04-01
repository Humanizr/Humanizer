namespace Humanizer;

/// <summary>
/// Shared renderer for locales that place the unit before the tens stem in compound numbers and
/// whose scale phrases may also change the joiner or inflected count form.
///
/// The algorithm is:
/// - decompose by descending scale rows
/// - render each scale count using the configured count gender and optional next-word transform
/// - render the terminal under-hundred remainder with unit-leading tens composition
/// - append ordinal stem and gender ending only once the terminal segment is known
///
/// The expected result is a compound phrase such as the Luxembourgish family forms that place the
/// unit before the tens and may apply Eifeler-style joiner adjustments from generated metadata.
/// </summary>
class UnitLeadingCompoundNumberToWordsConverter(UnitLeadingCompoundNumberToWordsProfile profile) : GenderedNumberToWordsConverter
{
    /// <summary>
    /// Immutable generated profile that owns the unit-leading compound lexicon and scale metadata.
    /// </summary>
    readonly UnitLeadingCompoundNumberToWordsProfile profile = profile;

    public override string Convert(long number, GrammaticalGender gender, bool addAnd = true) =>
        Convert(number, WordForm.Normal, gender, addAnd);

    // Cardinal rendering walks the descending scale rows first so the terminal under-hundred
    // segment can use the correct joiner and count morphology once all higher-order parts are known.
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

    // Ordinals in this family mutate the terminal segment only once the higher-order scale walk is
    // complete. The ordinal stem suffix and gender ending are therefore appended at the end.
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

    void CollectParts(List<string> parts, ref long number, UnitLeadingCompoundScale scale)
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

    void CollectOrdinalParts(List<string> parts, ref long number, UnitLeadingCompoundScale scale)
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

    // Cardinal scale rows format either a dedicated singular form or a generated plural format fed
    // by the recursively rendered count phrase.
    string BuildCardinalScalePart(long count, UnitLeadingCompoundScale scale)
    {
        if (count == 1)
        {
            return scale.SingularCardinal;
        }

        return string.Format(
            scale.PluralCardinalFormat,
            ConvertCount(count, scale));
    }

    // Ordinal scale rows distinguish exact terminal ordinals from continued phrases with a remainder.
    string BuildOrdinalScalePart(long count, UnitLeadingCompoundScale scale, bool hasRemainder)
    {
        if (count == 1)
        {
            return scale.OrdinalSingular[hasRemainder ? 1 : 0];
        }

        return string.Format(
            scale.OrdinalPlural[hasRemainder ? 1 : 0],
            ConvertCount(count, scale));
    }

    // Some locales transform the count form or the tens joiner depending on the next word. That
    // rule stays generated and localized through the profile rather than by branching on locale name.
    string ConvertCount(long count, UnitLeadingCompoundScale scale)
    {
        var wordForm = profile.TensJoinerTransform == CompoundTensJoinerTransform.Eifeler &&
                       scale.CountWordFormNextWord is { Length: > 0 } nextWord &&
                       EifelerRule.DoesApply(nextWord)
            ? WordForm.Eifeler
            : WordForm.Normal;

        return Convert(count, wordForm, scale.CountGender);
    }

    // Under one hundred is the heart of the family: emit units directly for small numbers, or put
    // the unit form in front of the tens stem with the configured joiner.
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

    // The tens joiner itself may need a transform such as the Eifeler rule depending on the next word.
    string GetTensJoiner(string nextWord) =>
        profile.TensJoinerTransform switch
        {
            CompoundTensJoinerTransform.None => profile.TensJoiner,
            CompoundTensJoinerTransform.Eifeler => EifelerRule.ApplyIfNeeded(profile.TensJoiner, nextWord),
            _ => throw new InvalidOperationException("Unknown unit-leading-compound tens joiner transform.")
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

/// <summary>
/// Immutable generated profile for <see cref="UnitLeadingCompoundNumberToWordsConverter"/>.
/// </summary>
sealed class UnitLeadingCompoundNumberToWordsProfile(
    string zeroWord,
    string minusWord,
    string masculineOne,
    string feminineOne,
    string neuterOne,
    string? feminineTwo,
    string tensJoiner,
    CompoundTensJoinerTransform tensJoinerTransform,
    string ordinalStemSuffix,
    string masculineOrdinalEnding,
    string feminineOrdinalEnding,
    string neuterOrdinalEnding,
    bool supportsEifelerRule,
    string[] unitsMap,
    string[] compoundUnitsMap,
    string[] tensMap,
    string[] unitsOrdinal,
    UnitLeadingCompoundScale[] scales)
{
    /// <summary>Gets the cardinal zero word.</summary>
    public string ZeroWord { get; } = zeroWord;
    /// <summary>Gets the word used to prefix negative values.</summary>
    public string MinusWord { get; } = minusWord;
    /// <summary>Gets the masculine word for one.</summary>
    public string MasculineOne { get; } = masculineOne;
    /// <summary>Gets the feminine word for one.</summary>
    public string FeminineOne { get; } = feminineOne;
    /// <summary>Gets the neuter word for one.</summary>
    public string NeuterOne { get; } = neuterOne;
    /// <summary>Gets the optional feminine word for two.</summary>
    public string? FeminineTwo { get; } = feminineTwo;
    /// <summary>Gets the base joiner inserted between the unit and tens stem.</summary>
    public string TensJoiner { get; } = tensJoiner;
    /// <summary>Gets the transform applied to the tens joiner, if any.</summary>
    public CompoundTensJoinerTransform TensJoinerTransform { get; } = tensJoinerTransform;
    /// <summary>Gets the ordinal stem suffix appended before the gender ending.</summary>
    public string OrdinalStemSuffix { get; } = ordinalStemSuffix;
    /// <summary>Gets the masculine ordinal ending.</summary>
    public string MasculineOrdinalEnding { get; } = masculineOrdinalEnding;
    /// <summary>Gets the feminine ordinal ending.</summary>
    public string FeminineOrdinalEnding { get; } = feminineOrdinalEnding;
    /// <summary>Gets the neuter ordinal ending.</summary>
    public string NeuterOrdinalEnding { get; } = neuterOrdinalEnding;
    /// <summary>Gets a value indicating whether the Eifeler rule may apply in this locale.</summary>
    public bool SupportsEifelerRule { get; } = supportsEifelerRule;
    /// <summary>Gets the base units lexicon.</summary>
    public string[] UnitsMap { get; } = unitsMap;
    /// <summary>Gets the unit forms used inside compound tens.</summary>
    public string[] CompoundUnitsMap { get; } = compoundUnitsMap;
    /// <summary>Gets the tens lexicon.</summary>
    public string[] TensMap { get; } = tensMap;
    /// <summary>Gets the ordinal unit lexicon.</summary>
    public string[] UnitsOrdinal { get; } = unitsOrdinal;
    /// <summary>Gets the descending scale rows used during decomposition.</summary>
    public UnitLeadingCompoundScale[] Scales { get; } = scales;
}

/// <summary>
/// One descending scale row for <see cref="UnitLeadingCompoundNumberToWordsConverter"/>.
/// </summary>
readonly record struct UnitLeadingCompoundScale(
    long Value,
    bool AddSpaceBeforeNextPart,
    GrammaticalGender CountGender,
    string SingularCardinal,
    string PluralCardinalFormat,
    string[] OrdinalSingular,
    string[] OrdinalPlural,
    string? CountWordFormNextWord = null);

/// <summary>
/// Describes the optional transform applied to the unit-leading tens joiner.
/// </summary>
enum CompoundTensJoinerTransform
{
    None,
    Eifeler
}

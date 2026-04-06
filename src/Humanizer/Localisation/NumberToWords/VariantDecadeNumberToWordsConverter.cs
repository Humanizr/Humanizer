namespace Humanizer;

/// <summary>
/// Shared renderer for French-family locales whose main divergence is how the 70, 80, and 90
/// decades are composed.
///
/// The algorithm is intentionally simple above one hundred:
/// - walk trillions down through thousands with stable long-scale logic
/// - render the terminal under-hundred segment through the decade strategy rules
///
/// The generated profile decides whether the locale uses regular decades or reuses 60/80 plus
/// teens, whether exact 80 pluralizes, and which tens require the "et un" join pattern.
/// </summary>
class VariantDecadeNumberToWordsConverter(VariantDecadeNumberToWordsProfile profile) : GenderedNumberToWordsConverter
{
    static readonly string[] UnitsMap = ["zéro", "un", "deux", "trois", "quatre", "cinq", "six", "sept", "huit", "neuf", "dix", "onze", "douze", "treize", "quatorze", "quinze", "seize", "dix-sept", "dix-huit", "dix-neuf"];
    /// <summary>
    /// Immutable generated profile that owns the decade strategy choices for the locale.
    /// </summary>
    readonly VariantDecadeNumberToWordsProfile profile = profile;

    /// <summary>
    /// Converts the number using the locale's variant-decade cardinal rules.
    /// </summary>
    /// <inheritdoc />
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

        // Large-scale traversal remains explicit because the lexical family is stable and the
        // generated profile only needs to drive the decade behavior rather than the long-scale map.
        CollectParts(parts, ref number, 1000000000000000000, "trillion");
        CollectParts(parts, ref number, 1000000000000000, "billiard");
        CollectParts(parts, ref number, 1000000000000, "billion");
        CollectParts(parts, ref number, 1000000000, "milliard");
        CollectParts(parts, ref number, 1000000, "million");
        CollectThousands(parts, ref number, 1000, "mille");

        CollectPartsUnderAThousand(parts, number, gender, true);

        return string.Join(" ", parts);
    }

    // Ordinals in this family are derived from the rendered cardinal phrase through a small set of
    // stem adjustments plus the terminal "ième" suffix.
    /// <summary>
    /// Converts the number using the locale's variant-decade ordinal rules.
    /// </summary>
    /// <inheritdoc />
    public override string ConvertToOrdinal(int number, GrammaticalGender gender)
    {
        if (number == 1)
        {
            return gender == GrammaticalGender.Feminine ? "première" : "premier";
        }

        // The ordinal is derived from the cardinal surface first because the family only changes a
        // handful of terminal stems; applying the rewrite rules in this order preserves those family
        // exceptions without reimplementing the full cardinal grammar.
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

    /// <summary>
    /// Gets the unit word for the supplied value, including the feminine one-form when required.
    /// </summary>
    /// <param name="number">The unit value to render.</param>
    /// <param name="gender">The grammatical gender requested for the terminal unit.</param>
    /// <returns>The localized unit word for <paramref name="number"/>.</returns>
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

    // Higher scales stay fully structural in this family. The only special-case long-scale rule is
    // whether the singular form omits the leading "un", which is handled in the explicit callers.
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

    // Under one hundred is where the family divergence lives, so the strategy enums only apply
    // inside this method and everything above it stays generic.
    // Under one hundred is the whole point of this engine: it is where septante/soixante-dix and
    // nonante/quatre-vingt-dix style variants actually diverge.
    void CollectPartsUnderAHundred(List<string> parts, ref long number, GrammaticalGender gender, bool pluralize)
    {
        if (number < 20)
        {
            parts.Add(GetUnits(number, gender));
        }
        else if (profile.SeventyStrategy == VariantDecadeSeventyStrategy.SixtyPlusTeens && number is >= 70 and < 80)
        {
            // 70 is not a generic decade here; the profile chooses whether the locale spells it as
            // 60 + teens, and 71 may still need a dedicated lexical entry.
            parts.Add(number == 71 && profile.SpecialSeventyOneEnabled
                ? profile.SpecialSeventyOneWord
                : $"{profile.TensMap[6]}-{GetUnits(number - 60, gender)}");
        }
        else if (profile.NinetyStrategy == VariantDecadeNinetyStrategy.EightyPlusTeens && number is >= 90 and < 100)
        {
            parts.Add($"{profile.TensMap[8]}-{GetUnits(number - 80, gender)}");
        }
        else
        {
            AppendStandardTens(parts, number, gender, pluralize);
        }
    }

    // Exact 80-pluralization and "et un" join behavior now come from generated decade metadata
    // instead of locale-specific converter classes.
    void AppendStandardTens(List<string> parts, long number, GrammaticalGender gender, bool pluralize)
    {
        var units = number % 10;
        var tensIndex = (int)(number / 10);
        var tens = profile.TensMap[tensIndex];
        if (units == 0)
        {
            // Exact 80 only pluralizes in locales whose generated profile says so; the `pluralize`
            // flag prevents higher-scale contexts from accidentally forcing an 80 plural suffix.
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

/// <summary>
/// Describes whether 70 stays regular or reuses 60 plus the teen forms.
/// </summary>
enum VariantDecadeSeventyStrategy
{
    /// <summary>
    /// Uses the locale's regular tens word for the seventies.
    /// </summary>
    Regular,
    /// <summary>
    /// Reuses sixty plus the teen forms for the seventies.
    /// </summary>
    SixtyPlusTeens
}

/// <summary>
/// Describes whether 90 stays regular or reuses 80 plus the teen forms.
/// </summary>
enum VariantDecadeNinetyStrategy
{
    /// <summary>
    /// Uses the locale's regular tens word for the nineties.
    /// </summary>
    Regular,
    /// <summary>
    /// Reuses eighty plus the teen forms for the nineties.
    /// </summary>
    EightyPlusTeens
}

/// <summary>
/// Immutable generated profile for <see cref="VariantDecadeNumberToWordsConverter"/>.
/// </summary>
/// <param name="minusWord">The word used to prefix negative values.</param>
/// <param name="seventyStrategy">The strategy used for the seventies.</param>
/// <param name="ninetyStrategy">The strategy used for the nineties.</param>
/// <param name="specialSeventyOneWord">The optional dedicated spelling for seventy-one.</param>
/// <param name="pluralizeExactEighty">A value indicating whether exact eighty takes a plural suffix.</param>
/// <param name="tensUsingEtWhenUnitIsOne">The tens indices that use an "et un" join pattern.</param>
/// <param name="tensMap">The tens lexicon keyed by decade value.</param>
sealed class VariantDecadeNumberToWordsProfile(
    string minusWord,
    VariantDecadeSeventyStrategy seventyStrategy,
    VariantDecadeNinetyStrategy ninetyStrategy,
    bool specialSeventyOneEnabled,
    string? specialSeventyOneWord,
    bool pluralizeExactEighty,
    FrozenSet<int> tensUsingEtWhenUnitIsOne,
    string[] tensMap)
{
    /// <summary>Gets the word used to prefix negative values.</summary>
    public string MinusWord { get; } = minusWord;
    /// <summary>Gets the strategy used for the seventies.</summary>
    public VariantDecadeSeventyStrategy SeventyStrategy { get; } = seventyStrategy;
    /// <summary>Gets the strategy used for the nineties.</summary>
    public VariantDecadeNinetyStrategy NinetyStrategy { get; } = ninetyStrategy;
    /// <summary>Gets a value indicating whether the locale uses a dedicated spelling for 71.</summary>
    public bool SpecialSeventyOneEnabled { get; } = specialSeventyOneEnabled;
    /// <summary>Gets the optional dedicated spelling for 71 when the locale needs one.</summary>
    public string SpecialSeventyOneWord { get; } = specialSeventyOneWord ?? throw new InvalidOperationException("A dedicated 71 spelling is required when specialSeventyOneEnabled is true.");
    /// <summary>Gets a value indicating whether exact 80 takes a plural suffix.</summary>
    public bool PluralizeExactEighty { get; } = pluralizeExactEighty;
    /// <summary>Gets the set of tens indices that use an "et un" style joiner when the unit is one.</summary>
    public FrozenSet<int> TensUsingEtWhenUnitIsOne { get; } = tensUsingEtWhenUnitIsOne;
    /// <summary>Gets the tens lexicon.</summary>
    public string[] TensMap { get; } = tensMap;
}
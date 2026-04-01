namespace Humanizer;

/// <summary>
/// Renders West Slavic numbers with gendered unit overrides and plural scale forms for thousands,
/// millions, and billions.
/// </summary>
class WestSlavicGenderedNumberToWordsConverter(WestSlavicNumberToWordsProfile profile, CultureInfo culture) :
    GenderedNumberToWordsConverter
{
    readonly WestSlavicNumberToWordsProfile profile = profile;

    /// <inheritdoc/>
    public override string Convert(long number, GrammaticalGender gender, bool addAnd = true)
    {
        if (number == 0)
        {
            return profile.UnitsMap[0];
        }

        if (number == long.MinValue)
        {
            throw new NotImplementedException();
        }

        var parts = new List<string>();
        if (number < 0)
        {
            parts.Add(profile.MinusWord);
            number = -number;
        }

        CollectScale(parts, ref number, 1_000_000_000, GrammaticalGender.Feminine, profile.Billions);
        CollectScale(parts, ref number, 1_000_000, GrammaticalGender.Masculine, profile.Millions);
        CollectScale(parts, ref number, 1_000, GrammaticalGender.Masculine, profile.Thousands);
        CollectLessThanThousand(parts, number, gender);

        return string.Join(" ", parts);
    }

    /// <inheritdoc/>
    public override string ConvertToOrdinal(int number, GrammaticalGender gender) =>
        number.ToString(culture);

    /// <summary>
    /// Appends one scale row and consumes the part of the number it covers.
    /// </summary>
    void CollectScale(List<string> parts, ref long number, long divisor, GrammaticalGender gender, WestSlavicScaleForms forms)
    {
        var scaleNumber = number / divisor;
        if (scaleNumber <= 0)
        {
            return;
        }

        if (forms.OmitLeadingOne && scaleNumber == 1)
        {
            parts.Add(forms.Singular);
            number %= divisor;
            return;
        }

        CollectLessThanThousand(parts, scaleNumber, scaleNumber < 19 ? gender : null);

        var units = scaleNumber % 1000;
        parts.Add(units switch
        {
            1 => forms.Singular,
            > 1 and < 5 => forms.Paucal,
            _ => forms.Plural
        });

        number %= divisor;
    }

    /// <summary>
    /// Appends the fragment below one thousand.
    /// </summary>
    void CollectLessThanThousand(List<string> parts, long number, GrammaticalGender? gender)
    {
        if (number >= 100)
        {
            parts.Add(profile.HundredsMap[number / 100]);
            number %= 100;
        }

        if (number >= 20)
        {
            parts.Add(profile.TensMap[number / 10]);
            number %= 10;
        }

        if (number > 0)
        {
            parts.Add(UnitByGender(number, gender));
        }
    }

    /// <summary>
    /// Returns the unit form for the requested gender.
    /// </summary>
    string UnitByGender(long number, GrammaticalGender? gender)
    {
        if (number != 1 && number != 2)
        {
            return profile.UnitsMap[number];
        }

        return gender switch
        {
            GrammaticalGender.Masculine => profile.UnitsMasculineOverrides[number - 1],
            GrammaticalGender.Feminine => profile.UnitsFeminineOverrides[number - 1],
            GrammaticalGender.Neuter => profile.UnitsNeuterOverrides[number - 1],
            null => profile.UnitsInvariantOverrides[number - 1],
            _ => throw new ArgumentOutOfRangeException(nameof(gender))
        };
    }
}

/// <summary>
/// Immutable generated profile for <see cref="WestSlavicGenderedNumberToWordsConverter"/>.
/// </summary>
/// <param name="minusWord">The word used to prefix negative values.</param>
/// <param name="unitsMap">The base unit lexicon.</param>
/// <param name="tensMap">The tens lexicon.</param>
/// <param name="hundredsMap">The hundreds lexicon.</param>
/// <param name="unitsMasculineOverrides">The masculine overrides for units 1 and 2.</param>
/// <param name="unitsFeminineOverrides">The feminine overrides for units 1 and 2.</param>
/// <param name="unitsNeuterOverrides">The neuter overrides for units 1 and 2.</param>
/// <param name="unitsInvariantOverrides">The fallback overrides when no gender-specific form is used.</param>
/// <param name="thousands">The plural forms for the thousands scale.</param>
/// <param name="millions">The plural forms for the millions scale.</param>
/// <param name="billions">The plural forms for the billions scale.</param>
internal sealed class WestSlavicNumberToWordsProfile(
    string minusWord,
    string[] unitsMap,
    string[] tensMap,
    string[] hundredsMap,
    string[] unitsMasculineOverrides,
    string[] unitsFeminineOverrides,
    string[] unitsNeuterOverrides,
    string[] unitsInvariantOverrides,
    WestSlavicScaleForms thousands,
    WestSlavicScaleForms millions,
    WestSlavicScaleForms billions)
{
    /// <summary>Gets the word used to prefix negative values.</summary>
    public string MinusWord { get; } = minusWord;
    /// <summary>Gets the base unit lexicon.</summary>
    public string[] UnitsMap { get; } = unitsMap;
    /// <summary>Gets the tens lexicon.</summary>
    public string[] TensMap { get; } = tensMap;
    /// <summary>Gets the hundreds lexicon.</summary>
    public string[] HundredsMap { get; } = hundredsMap;
    /// <summary>Gets the masculine overrides for units 1 and 2.</summary>
    public string[] UnitsMasculineOverrides { get; } = unitsMasculineOverrides;
    /// <summary>Gets the feminine overrides for units 1 and 2.</summary>
    public string[] UnitsFeminineOverrides { get; } = unitsFeminineOverrides;
    /// <summary>Gets the neuter overrides for units 1 and 2.</summary>
    public string[] UnitsNeuterOverrides { get; } = unitsNeuterOverrides;
    /// <summary>Gets the fallback overrides used when no explicit gender-specific form is selected.</summary>
    public string[] UnitsInvariantOverrides { get; } = unitsInvariantOverrides;
    /// <summary>Gets the plural forms for the thousands scale.</summary>
    public WestSlavicScaleForms Thousands { get; } = thousands;
    /// <summary>Gets the plural forms for the millions scale.</summary>
    public WestSlavicScaleForms Millions { get; } = millions;
    /// <summary>Gets the plural forms for the billions scale.</summary>
    public WestSlavicScaleForms Billions { get; } = billions;
}

/// <summary>
/// One scale's singular, paucal, and plural forms.
/// </summary>
/// <param name="Singular">The singular form.</param>
/// <param name="Paucal">The paucal form.</param>
/// <param name="Plural">The plural form.</param>
/// <param name="OmitLeadingOne">Whether the scale omits an explicit leading one.</param>
internal readonly record struct WestSlavicScaleForms(string Singular, string Paucal, string Plural, bool OmitLeadingOne = false);

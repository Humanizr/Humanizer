namespace Humanizer;

/// <summary>
/// Shared renderer for dual-form scale languages where singular, dual, and plural words depend on
/// the count and on whether a prefixed form is required for lower digits.
///
/// The generated profile provides the scale lexicon and prefix maps while the runtime keeps the
/// recursive decomposition and gender handling consistent.
/// </summary>
class DualFormScaleNumberToWordsConverter(DualFormScaleNumberToWordsProfile profile) : GenderedNumberToWordsConverter
{
    readonly DualFormScaleNumberToWordsProfile profile = profile;

    /// <summary>
    /// Converts the given value using the locale's dual-form cardinal rules.
    /// </summary>
    /// <param name="input">The number to convert.</param>
    /// <param name="gender">The grammatical gender to use when rendering lower groups.</param>
    /// <param name="addAnd">Reserved for compatibility with other converters; this implementation derives conjunction placement from the generated profile.</param>
    /// <returns>The localized cardinal words for <paramref name="input"/>.</returns>
    public override string Convert(long input, GrammaticalGender gender, bool addAnd = true)
    {
        var negativeNumber = false;

        if (input < 0)
        {
            // Keep the sign separate so the recursive positive path can stay focused on scale
            // morphology and gender selection.
            negativeNumber = true;
            input *= -1;
        }

        var text = ConvertPositive(input, gender);
        return text + (negativeNumber ? $" {profile.MinusSuffix}" : string.Empty);
    }

    /// <summary>
    /// Converts the given value into a locale-specific ordinal phrase.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <param name="gender">The grammatical gender to use when rendering the ordinal.</param>
    /// <returns>The localized ordinal words for <paramref name="number"/>.</returns>
    public override string ConvertToOrdinal(int number, GrammaticalGender gender)
    {
        if (number <= 20)
        {
            // The first twenty ordinals are fully irregular and are therefore stored as exact
            // words.
            return profile.ExactOrdinals[number];
        }

        var ordinal = Convert(number, gender);

        // Abbreviated ordinals are built from the cardinal spelling and then prefixed according to
        // the locale's orthographic conventions.
        if (ordinal.StartsWith('d'))
        {
            return $"id-{Convert(number, gender)}";
        }
        if (ordinal.StartsWith('s'))
        {
            return $"is-{Convert(number, gender)}";
        }
        if (ordinal.StartsWith('t'))
        {
            return $"it-{Convert(number, gender)}";
        }
        if (ordinal.StartsWith('e'))
        {
            return $"l-{Convert(number, gender)}";
        }
        return $"il-{Convert(number, gender)}";
    }

    /// <summary>
    /// Renders a tens or unit fragment, optionally using the prefix map used by the locale.
    /// </summary>
    string GetTens(long value, bool usePrefixMap, bool usePrefixMapForLowerDigits, GrammaticalGender gender)
    {
        if (value == 1 && gender == GrammaticalGender.Feminine)
        {
            // Feminine "one" is a dedicated lexical item in this family.
            return profile.FeminineOneWord;
        }

        if (value < 11 && usePrefixMap && usePrefixMapForLowerDigits)
        {
            // Some lower-digit combinations use the prefix map instead of the ordinary unit table.
            return profile.PrefixMap[value];
        }

        if (value < 11 && usePrefixMap && !usePrefixMapForLowerDigits)
        {
            // Other scale rows prefer the hundreds map for the same low-digit range.
            return profile.HundredsMap[value];
        }

        if (value is > 10 and < 20 && usePrefixMap)
        {
            // Teen forms can also come from the prefix map when the scale row needs a contracted
            // stem.
            return profile.PrefixMap[value];
        }

        if (value < 20)
        {
            return profile.UnitsMap[value];
        }

        var single = value % 10;
        var numberOfTens = value / 10;
        if (single == 0)
        {
            return profile.TensMap[numberOfTens];
        }

        // Compound tens are rendered as "unit + conjunction + tens" in the dual-form family.
        return $"{profile.UnitsMap[single]} {profile.Conjunction} {profile.TensMap[numberOfTens]}";
    }

    string GetHundreds(long value, bool usePrefixMap, bool usePrefixMapForLowerValueDigits, GrammaticalGender gender)
    {
        if (value < 100)
        {
            return GetTens(value, usePrefixMap, usePrefixMapForLowerValueDigits, gender);
        }

        var tens = value % 100;
        var numberOfHundreds = value / 100;

        string hundredsText;
        if (numberOfHundreds == 1)
        {
            hundredsText = profile.HundredWord;
        }
        else if (numberOfHundreds == 2)
        {
            // Two hundred has its own form in the generated profile.
            hundredsText = profile.DualHundredsWord;
        }
        else
        {
            // Larger hundreds are built from the digit table plus the shared hundred word.
            hundredsText = profile.HundredsMap[numberOfHundreds] + $" {profile.HundredWord}";
        }

        if (tens == 0)
        {
            return hundredsText;
        }

        return $"{hundredsText} {profile.Conjunction} {GetTens(tens, usePrefixMap, usePrefixMapForLowerValueDigits, gender)}";
    }

    string ConvertPositive(long value, GrammaticalGender gender)
    {
        if (value < 1000)
        {
            return GetHundreds(value, false, false, gender);
        }

        foreach (var scale in profile.Scales)
        {
            if (value < scale.Value)
            {
                continue;
            }

            var count = value / scale.Value;
            var remainder = value % scale.Value;
            var text = GetScaleText(count, count % 100, scale.Forms, gender);
            if (remainder == 0)
            {
                return text;
            }

            return $"{text} {profile.Conjunction} {ConvertPositive(remainder, gender)}";
        }

        return GetHundreds(value, false, false, gender);
    }

    string GetScaleText(long count, long lastTwoDigits, DualFormScale scale, GrammaticalGender gender)
    {
        if (count == 1)
        {
            // Singular, dual, and plural forms are all generated data; one is the simplest case.
            return scale.Singular;
        }

        if (count == 2)
        {
            // The dual form is explicit and should not be forced through the plural fallback.
            return scale.Dual;
        }

        if (lastTwoDigits > 10)
        {
            // Large counts keep the singular scale noun after the fully rendered count phrase.
            return $"{GetHundreds(count, true, scale.UsePrefixMapForLowerDigits, gender)} {scale.Singular}";
        }

        if (count == 100)
        {
            // Exact one hundred uses a dedicated prefix form in some locales.
            return $"{profile.HundredPrefixWord} {scale.Singular}";
        }

        if (count == 101)
        {
            // One hundred and one is the lone case that keeps the conjunction before the singular.
            return $"{profile.HundredWord} {profile.Conjunction} {scale.Singular}";
        }

        // Everything else falls back to the plural scale noun after the rendered count.
        return $"{GetHundreds(count, true, scale.UsePrefixMapForLowerDigits, gender)} {scale.Plural}";
    }
}

/// <summary>
/// Immutable generated profile for <see cref="DualFormScaleNumberToWordsConverter"/>.
/// </summary>
sealed class DualFormScaleNumberToWordsProfile(
    string conjunction,
    string minusSuffix,
    string feminineOneWord,
    string hundredWord,
    string dualHundredsWord,
    string hundredPrefixWord,
    FrozenDictionary<int, string> exactOrdinals,
    string[] unitsMap,
    string[] tensMap,
    string[] hundredsMap,
    string[] prefixMap,
    DualFormScale thousandScale,
    DualFormScale millionScale,
    DualFormScale billionScale,
    DualFormScale trillionScale,
    DualFormScale quadrillionScale,
    DualFormScale quintillionScale)
{
    /// <summary>
    /// Gets the conjunction inserted between rendered parts.
    /// </summary>
    public string Conjunction { get; } = conjunction;
    /// <summary>
    /// Gets the suffix appended to negative values.
    /// </summary>
    public string MinusSuffix { get; } = minusSuffix;
    /// <summary>
    /// Gets the feminine word for one.
    /// </summary>
    public string FeminineOneWord { get; } = feminineOneWord;
    /// <summary>
    /// Gets the standalone hundred word.
    /// </summary>
    public string HundredWord { get; } = hundredWord;
    /// <summary>
    /// Gets the special dual-form hundred word.
    /// </summary>
    public string DualHundredsWord { get; } = dualHundredsWord;
    /// <summary>
    /// Gets the prefix used when exactly one hundred precedes a scale.
    /// </summary>
    public string HundredPrefixWord { get; } = hundredPrefixWord;
    /// <summary>
    /// Gets the exact ordinal words for values up to twenty.
    /// </summary>
    public FrozenDictionary<int, string> ExactOrdinals { get; } = exactOrdinals;
    /// <summary>
    /// Gets the unit lexicon.
    /// </summary>
    public string[] UnitsMap { get; } = unitsMap;
    /// <summary>
    /// Gets the tens lexicon.
    /// </summary>
    public string[] TensMap { get; } = tensMap;
    /// <summary>
    /// Gets the hundreds lexicon.
    /// </summary>
    public string[] HundredsMap { get; } = hundredsMap;
    /// <summary>
    /// Gets the prefix lexicon used by some low-digit combinations.
    /// </summary>
    public string[] PrefixMap { get; } = prefixMap;
    /// <summary>
    /// Gets the thousand scale configuration.
    /// </summary>
    public DualFormScale ThousandScale { get; } = thousandScale;
    /// <summary>
    /// Gets the million scale configuration.
    /// </summary>
    public DualFormScale MillionScale { get; } = millionScale;
    /// <summary>
    /// Gets the billion scale configuration.
    /// </summary>
    public DualFormScale BillionScale { get; } = billionScale;
    /// <summary>
    /// Gets the trillion scale configuration.
    /// </summary>
    public DualFormScale TrillionScale { get; } = trillionScale;
    /// <summary>
    /// Gets the quadrillion scale configuration.
    /// </summary>
    public DualFormScale QuadrillionScale { get; } = quadrillionScale;
    /// <summary>
    /// Gets the quintillion scale configuration.
    /// </summary>
    public DualFormScale QuintillionScale { get; } = quintillionScale;
    /// <summary>
    /// Gets the scale ladder used for recursive high-range decomposition.
    /// </summary>
    public (long Value, DualFormScale Forms)[] Scales { get; } =
    [
        (1000000000000000000, quintillionScale),
        (1000000000000000, quadrillionScale),
        (1000000000000, trillionScale),
        (1000000000, billionScale),
        (1000000, millionScale),
        (1000, thousandScale)
    ];
}

/// <summary>
/// One dual-form scale row for <see cref="DualFormScaleNumberToWordsConverter"/>.
/// </summary>
readonly record struct DualFormScale(
    string Singular,
    string Dual,
    string Plural,
    bool UsePrefixMapForLowerDigits);
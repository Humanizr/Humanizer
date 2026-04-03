namespace Humanizer;

/// <summary>
/// Shared renderer for languages that mark scale words with construct-state forms and apply
/// gendered rules to the lower decimal groups.
///
/// The generated profile provides the scale forms, teen contractions, and special thousands cases
/// so the runtime can stay focused on the numeric decomposition order.
/// </summary>
class ConstructStateScaleNumberToWordsConverter(ConstructStateScaleNumberToWordsProfile profile, CultureInfo culture) :
    GenderedNumberToWordsConverter(profile.DefaultGender)
{
    readonly ConstructStateScaleNumberToWordsProfile profile = profile;
    readonly CultureInfo culture = culture;

    /// <summary>
    /// Converts the given value using the locale's construct-state cardinal rules.
    /// </summary>
    /// <param name="input">The number to convert.</param>
    /// <param name="gender">The grammatical gender to use for the under-one-hundred portion.</param>
    /// <param name="addAnd">Reserved for compatibility with other converters; this implementation derives conjunction placement from the generated profile.</param>
    /// <returns>The localized cardinal words for <paramref name="input"/>.</returns>
    public override string Convert(long input, GrammaticalGender gender, bool addAnd = true)
    {
        if (input == 0)
        {
            return profile.ZeroWord;
        }

        if (input < 0)
        {
            // The sign is carried separately so the positive path can remain focused on construct
            // state and gender rules.
            return profile.MinusWord + " " + Convert(-input, gender);
        }

        var number = input;
        var parts = new List<string>(6);

        foreach (var scale in profile.Scales)
        {
            if (number < scale.Value)
            {
                continue;
            }

            // Large scale rows stay fully data-driven; count gender is part of the generated scale
            // row because different locales need different agreement for the same magnitude.
            var count = number / scale.Value;
            parts.Add(BuildLargeScalePart(count, scale));
            number %= scale.Value;
        }

        if (number >= 1000)
        {
            // Thousands have irregular spellings in this family, so check the generated special
            // cases before applying the generic feminine-unit fallback.
            var thousands = number / 1000;
            parts.Add(BuildThousandsPart(thousands));
            number %= 1000;
        }

        if (number >= 100)
        {
            var hundreds = (int)(number / 100);
            parts.Add(BuildHundredsPart(hundreds));
            number %= 100;
        }

        if (number > 0)
        {
            parts.Add(BuildUnderOneHundredPart((int)number, gender, parts.Count > 0));
        }

        return string.Join(" ", parts);
    }

    /// <summary>
    /// Converts the given value to the locale's ordinal representation.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <param name="gender">The grammatical gender to use.</param>
    /// <returns>The culture-specific numeric ordinal form.</returns>
    public override string ConvertToOrdinal(int number, GrammaticalGender gender) =>
        number.ToString(culture);

    string BuildLargeScalePart(long count, ConstructStateScale scale) =>
        count switch
        {
            1 => scale.Singular,
            2 => scale.DualPrefix + " " + scale.Singular,
            _ => Convert(count, scale.CountGender) + " " + scale.Singular
        };

    string BuildThousandsPart(long thousands)
    {
        if (thousands <= int.MaxValue && profile.ThousandsSpecialCases.TryGetValue((int)thousands, out var special))
        {
            return special;
        }

        if (thousands <= 10)
        {
            // Small thousand counts use the feminine unit table plus the plural suffix; the
            // singular suffix is reserved for the recursive fallback above ten.
            return profile.UnitsFeminine[thousands] + profile.ThousandsPluralSuffix;
        }

        return Convert(thousands) + profile.ThousandsSingularSuffix;
    }

    string BuildHundredsPart(int hundreds) =>
        hundreds switch
        {
            1 => profile.OneHundredWord,
            2 => profile.TwoHundredsWord,
            _ => profile.UnitsFeminine[hundreds] + profile.HundredsPluralSuffix
        };

    string BuildUnderOneHundredPart(int number, GrammaticalGender gender, bool appendAnd)
    {
        if (number <= 10)
        {
            // Direct unit lookup stays the cheapest path and preserves the locale's explicit
            // conjunction prefix when a higher group already exists.
            var unit = GetUnit(number, gender);
            return appendAnd ? profile.AndPrefix + unit : unit;
        }

        if (number < 20)
        {
            // Teens are not stored as raw digit + teen word because the unit form can normalize
            // before the suffix is appended.
            var unit = GetUnit(number % 10, gender).Replace(profile.TeenNormalizationOld, profile.TeenNormalizationNew);
            var teenWord = unit + " " + (gender == GrammaticalGender.Masculine ? profile.TeenMasculineWord : profile.TeenFeminineWord);
            return appendAnd ? profile.AndPrefix + teenWord : teenWord;
        }

        // Twenty and above use a stable tens table; only a non-zero unit needs the conjunction.
        var tensWord = profile.TensMap[number / 10 - 2];
        if (number % 10 == 0)
        {
            return tensWord;
        }

        return tensWord + " " + profile.AndPrefix + GetUnit(number % 10, gender);
    }

    string GetUnit(int number, GrammaticalGender gender) =>
        gender == GrammaticalGender.Masculine
            ? profile.UnitsMasculine[number]
            : profile.UnitsFeminine[number];
}

/// <summary>
/// Immutable generated profile for <see cref="ConstructStateScaleNumberToWordsConverter"/>.
/// </summary>
sealed class ConstructStateScaleNumberToWordsProfile(
    GrammaticalGender defaultGender,
    string zeroWord,
    string minusWord,
    string andPrefix,
    string teenMasculineWord,
    string teenFeminineWord,
    string teenNormalizationOld,
    string teenNormalizationNew,
    string[] unitsFeminine,
    string[] unitsMasculine,
    string[] tensMap,
    string oneHundredWord,
    string twoHundredsWord,
    string hundredsPluralSuffix,
    string thousandsPluralSuffix,
    string thousandsSingularSuffix,
    FrozenDictionary<int, string> thousandsSpecialCases,
    ConstructStateScale[] scales)
{
    /// <summary>
    /// Gets the default grammatical gender for the locale.
    /// </summary>
    public GrammaticalGender DefaultGender { get; } = defaultGender;
    /// <summary>
    /// Gets the cardinal zero word.
    /// </summary>
    public string ZeroWord { get; } = zeroWord;
    /// <summary>
    /// Gets the word used to prefix negative values.
    /// </summary>
    public string MinusWord { get; } = minusWord;
    /// <summary>
    /// Gets the prefix inserted before the under-one-hundred conjunction.
    /// </summary>
    public string AndPrefix { get; } = andPrefix;
    /// <summary>
    /// Gets the masculine word used for teen compounds.
    /// </summary>
    public string TeenMasculineWord { get; } = teenMasculineWord;
    /// <summary>
    /// Gets the feminine word used for teen compounds.
    /// </summary>
    public string TeenFeminineWord { get; } = teenFeminineWord;
    /// <summary>
    /// Gets the source text replaced during teen normalization.
    /// </summary>
    public string TeenNormalizationOld { get; } = teenNormalizationOld;
    /// <summary>
    /// Gets the replacement text used during teen normalization.
    /// </summary>
    public string TeenNormalizationNew { get; } = teenNormalizationNew;
    /// <summary>
    /// Gets the feminine units lexicon.
    /// </summary>
    public string[] UnitsFeminine { get; } = unitsFeminine;
    /// <summary>
    /// Gets the masculine units lexicon.
    /// </summary>
    public string[] UnitsMasculine { get; } = unitsMasculine;
    /// <summary>
    /// Gets the tens lexicon.
    /// </summary>
    public string[] TensMap { get; } = tensMap;
    /// <summary>
    /// Gets the dedicated one-hundred word.
    /// </summary>
    public string OneHundredWord { get; } = oneHundredWord;
    /// <summary>
    /// Gets the dedicated two-hundred word.
    /// </summary>
    public string TwoHundredsWord { get; } = twoHundredsWord;
    /// <summary>
    /// Gets the plural suffix used for hundreds above two.
    /// </summary>
    public string HundredsPluralSuffix { get; } = hundredsPluralSuffix;
    /// <summary>
    /// Gets the plural suffix used for thousands.
    /// </summary>
    public string ThousandsPluralSuffix { get; } = thousandsPluralSuffix;
    /// <summary>
    /// Gets the singular suffix used for thousands.
    /// </summary>
    public string ThousandsSingularSuffix { get; } = thousandsSingularSuffix;
    /// <summary>
    /// Gets exact thousands exceptions keyed by count.
    /// </summary>
    public FrozenDictionary<int, string> ThousandsSpecialCases { get; } = thousandsSpecialCases;
    /// <summary>
    /// Gets the descending scale rows used during decomposition.
    /// </summary>
    public ConstructStateScale[] Scales { get; } = scales;
}

/// <summary>
/// One descending scale row for <see cref="ConstructStateScaleNumberToWordsConverter"/>.
/// </summary>
readonly record struct ConstructStateScale(
    long Value,
    string Singular,
    string DualPrefix,
    GrammaticalGender CountGender);

namespace Humanizer;

/// <summary>
/// Ordinalizer that applies template patterns that vary by grammatical gender and word form.
/// </summary>
class WordFormTemplateOrdinalizer(CultureInfo culture, WordFormTemplateOrdinalizer.Options options) : DefaultOrdinalizer
{
    readonly CultureInfo culture = culture;
    readonly Options options = options;

    /// <summary>
    /// Ordinalizes the number using the default masculine form.
    /// </summary>
    public override string Convert(int number, string numberString) =>
        Convert(number, numberString, GrammaticalGender.Masculine, WordForm.Normal);

    /// <summary>
    /// Ordinalizes the number using the default word form for the requested gender.
    /// </summary>
    public override string Convert(int number, string numberString, GrammaticalGender gender) =>
        Convert(number, numberString, gender, WordForm.Normal);

    /// <summary>
    /// Ordinalizes the number using the template data for the requested gender and word form.
    /// </summary>
    public override string Convert(int number, string numberString, GrammaticalGender gender, WordForm wordForm)
    {
        if (options.ZeroAsPlainNumber && number == 0)
        {
            return "0";
        }

        if (options.MinValueAsPlainNumber && number == int.MinValue)
        {
            return "0";
        }

        if (number < 0)
        {
            return options.NegativeMode switch
            {
                NegativeNumberMode.None => Format(number, numberString, gender, wordForm),
                NegativeNumberMode.AbsoluteInvariant => Convert(-number, (-number).ToString(CultureInfo.InvariantCulture), gender, wordForm),
                NegativeNumberMode.AbsoluteCulture => Convert(-number, (-number).ToString(culture), gender, wordForm),
                _ => throw new InvalidOperationException("Unknown negative number mode.")
            };
        }

        return Format(number, numberString, gender, wordForm);
    }

    string Format(int number, string numberString, GrammaticalGender gender, WordForm wordForm)
    {
        var pattern = GetPattern(gender, wordForm);
        if (pattern.ExactReplacements.TryGetValue(number, out var exactReplacement))
        {
            return exactReplacement;
        }

        if (pattern.ExactSuffixes.TryGetValue(number, out var exactSuffix))
        {
            return pattern.Prefix + numberString + exactSuffix;
        }

        var lastDigit = Math.Abs(number % 10);
        if (pattern.LastDigitSuffixes.TryGetValue(lastDigit, out var lastDigitSuffix))
        {
            return pattern.Prefix + numberString + lastDigitSuffix;
        }

        return pattern.Prefix + numberString + pattern.DefaultSuffix;
    }

    Pattern GetPattern(GrammaticalGender gender, WordForm wordForm)
    {
        var set = gender switch
        {
            GrammaticalGender.Feminine => options.Feminine,
            GrammaticalGender.Neuter => options.Neuter,
            _ => options.Masculine
        };

        return wordForm == WordForm.Abbreviation
            ? set.Abbreviation
            : set.Normal;
    }

    /// <summary>
    /// Describes the prefix and suffix rules for one grammatical gender and word form.
    /// </summary>
    /// <param name="Prefix">The text to prepend before the ordinal number.</param>
    /// <param name="DefaultSuffix">The fallback suffix when no special rule matches.</param>
    /// <param name="ExactReplacements">Exact-number replacements that bypass the input text.</param>
    /// <param name="ExactSuffixes">Exact-number suffixes that append after the input text.</param>
    /// <param name="LastDigitSuffixes">Suffixes keyed by the absolute last digit.</param>
    public readonly record struct Pattern(
        string Prefix,
        string DefaultSuffix,
        FrozenDictionary<int, string> ExactReplacements,
        FrozenDictionary<int, string> ExactSuffixes,
        FrozenDictionary<int, string> LastDigitSuffixes)
    {
        public string Prefix { get; } = Prefix;
        public string DefaultSuffix { get; } = DefaultSuffix;
        public FrozenDictionary<int, string> ExactReplacements { get; } = ExactReplacements;
        public FrozenDictionary<int, string> ExactSuffixes { get; } = ExactSuffixes;
        public FrozenDictionary<int, string> LastDigitSuffixes { get; } = LastDigitSuffixes;
    }

    /// <summary>
    /// Pairs the normal and abbreviated patterns for a single grammatical gender.
    /// </summary>
    /// <param name="Normal">The full-word pattern.</param>
    /// <param name="Abbreviation">The abbreviated pattern.</param>
    public readonly record struct PatternSet(Pattern Normal, Pattern Abbreviation)
    {
        public Pattern Normal { get; } = Normal;
        public Pattern Abbreviation { get; } = Abbreviation;
    }

    /// <summary>
    /// Describes the template patterns for the three grammatical genders.
    /// </summary>
    /// <param name="Masculine">The masculine pattern set.</param>
    /// <param name="Feminine">The feminine pattern set.</param>
    /// <param name="Neuter">The neuter pattern set.</param>
    /// <param name="ZeroAsPlainNumber">Whether zero should be returned as plain <c>0</c>.</param>
    /// <param name="MinValueAsPlainNumber">Whether <see cref="int.MinValue"/> should be returned as plain <c>0</c>.</param>
    /// <param name="NegativeMode">How negative numbers should be normalized before formatting.</param>
    public readonly record struct Options(
        PatternSet Masculine,
        PatternSet Feminine,
        PatternSet Neuter,
        bool ZeroAsPlainNumber = false,
        bool MinValueAsPlainNumber = false,
        NegativeNumberMode NegativeMode = NegativeNumberMode.None)
    {
        public PatternSet Masculine { get; } = Masculine;
        public PatternSet Feminine { get; } = Feminine;
        public PatternSet Neuter { get; } = Neuter;
        public bool ZeroAsPlainNumber { get; } = ZeroAsPlainNumber;
        public bool MinValueAsPlainNumber { get; } = MinValueAsPlainNumber;
        public NegativeNumberMode NegativeMode { get; } = NegativeMode;
    }

    /// <summary>
    /// Controls how negative values are normalized before ordinal formatting.
    /// </summary>
    public enum NegativeNumberMode
    {
        /// <summary>
        /// Leave the negative value unchanged.
        /// </summary>
        None,

        /// <summary>
        /// Reformat the absolute value using invariant culture.
        /// </summary>
        AbsoluteInvariant,

        /// <summary>
        /// Reformat the absolute value using the configured culture.
        /// </summary>
        AbsoluteCulture
    }
}
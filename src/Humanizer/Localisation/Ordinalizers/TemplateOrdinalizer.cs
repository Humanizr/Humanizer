namespace Humanizer;

/// <summary>
/// Ordinalizer that applies locale-provided prefixes, suffixes, and exact replacements.
/// </summary>
class TemplateOrdinalizer(TemplateOrdinalizer.Options options) : DefaultOrdinalizer
{
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

        if (number < 0 && options.NegativeMode == NegativeNumberMode.AbsoluteInvariant)
        {
            var positiveNumber = -number;
            return Convert(positiveNumber, positiveNumber.ToString(CultureInfo.InvariantCulture), gender, wordForm);
        }

        var pattern = GetPattern(gender);
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

    Pattern GetPattern(GrammaticalGender gender) =>
        gender switch
        {
            GrammaticalGender.Feminine => options.Feminine,
            GrammaticalGender.Neuter => options.Neuter,
            _ => options.Masculine
        };

    /// <summary>
    /// Describes the prefix and suffix rules for one grammatical gender.
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
    /// Describes the template patterns for the three grammatical genders.
    /// </summary>
    /// <param name="Masculine">The masculine pattern.</param>
    /// <param name="Feminine">The feminine pattern.</param>
    /// <param name="Neuter">The neuter pattern.</param>
    /// <param name="ZeroAsPlainNumber">Whether zero should be returned as plain <c>0</c>.</param>
    /// <param name="MinValueAsPlainNumber">Whether <see cref="int.MinValue"/> should be returned as plain <c>0</c>.</param>
    /// <param name="NegativeMode">How negative numbers should be normalized before formatting.</param>
    public readonly record struct Options(
        Pattern Masculine,
        Pattern Feminine,
        Pattern Neuter,
        bool ZeroAsPlainNumber = false,
        bool MinValueAsPlainNumber = false,
        NegativeNumberMode NegativeMode = NegativeNumberMode.None)
    {
        public Pattern Masculine { get; } = Masculine;
        public Pattern Feminine { get; } = Feminine;
        public Pattern Neuter { get; } = Neuter;
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
        AbsoluteInvariant
    }
}
namespace Humanizer;

/// <summary>
/// Ordinalizer that converts a number to its cardinal word form via <see cref="INumberToWordsConverter"/>
/// and then appends a gendered suffix. Exact replacements for irregular low ordinals bypass the
/// cardinal-plus-suffix path entirely.
/// </summary>
/// <remarks>
/// This engine requires a <see cref="CultureInfo"/> at construction time (<c>useCulture: true</c>
/// in the YAML profile) so it can resolve the correct number-to-words converter for the locale.
/// Neuter gender falls back to masculine.
/// </remarks>
class NumberWordSuffixOrdinalizer(CultureInfo culture, NumberWordSuffixOrdinalizer.Options options) : DefaultOrdinalizer
{
    readonly CultureInfo culture = culture;
    readonly Options options = options;

    /// <summary>
    /// Ordinalizes using the default masculine gender.
    /// </summary>
    public override string Convert(int number, string numberString) =>
        Convert(number, numberString, GrammaticalGender.Masculine);

    /// <summary>
    /// Ordinalizes using the requested gender (neuter falls back to masculine).
    /// </summary>
    public override string Convert(int number, string numberString, GrammaticalGender gender) =>
        ConvertCore(number, gender);

    /// <summary>
    /// Ordinalizes using the requested gender and word form. Word form is ignored because
    /// this engine does not distinguish abbreviation from normal.
    /// </summary>
    public override string Convert(int number, string numberString, GrammaticalGender gender, WordForm wordForm) =>
        ConvertCore(number, gender);

    string ConvertCore(int number, GrammaticalGender gender)
    {
        var effectiveGender = ResolveEffectiveGender(gender);
        var block = ResolveGenderBlock(effectiveGender);

        // Negative numbers: check the absolute magnitude against exact replacements.
        // When found, delegate to the number-to-words converter's ConvertToOrdinal which
        // already composes the locale's negative prefix with the irregular ordinal form.
        // For non-exact negatives, the cardinal-plus-suffix path naturally includes the
        // negative prefix from the converter.
        if (number < 0)
        {
            var magnitude = number == int.MinValue ? (long)int.MaxValue + 1 : Math.Abs(number);
            if (magnitude <= int.MaxValue
                && block.ExactReplacements.TryGetValue((int)magnitude, out _))
            {
                return Configurator.GetNumberToWordsConverter(culture).ConvertToOrdinal(number, effectiveGender);
            }

            var cardinal = Configurator.GetNumberToWordsConverter(culture).Convert(number, effectiveGender);
            return cardinal + block.DefaultSuffix;
        }

        if (block.ExactReplacements.TryGetValue(number, out var exact))
        {
            return exact;
        }

        var positiveCardinal = Configurator.GetNumberToWordsConverter(culture).Convert(number, effectiveGender);
        return positiveCardinal + block.DefaultSuffix;
    }

    GrammaticalGender ResolveEffectiveGender(GrammaticalGender gender) =>
        gender switch
        {
            GrammaticalGender.Neuter => options.NeuterFallbackGender == GrammaticalGender.Feminine
                ? GrammaticalGender.Feminine
                : GrammaticalGender.Masculine,
            _ => gender
        };

    GenderBlock ResolveGenderBlock(GrammaticalGender effectiveGender) =>
        effectiveGender switch
        {
            GrammaticalGender.Feminine => options.Feminine,
            _ => options.Masculine
        };

    /// <summary>
    /// Suffix and exact-replacement data for one grammatical gender.
    /// </summary>
    /// <param name="DefaultSuffix">The suffix appended to the cardinal word form for productive ordinals.</param>
    /// <param name="ExactReplacements">Irregular ordinals that replace the entire output for specific numbers.</param>
    public readonly record struct GenderBlock(
        string DefaultSuffix,
        FrozenDictionary<int, string> ExactReplacements)
    {
        /// <summary>The suffix appended to the cardinal word form for productive ordinals.</summary>
        public string DefaultSuffix { get; } = DefaultSuffix;

        /// <summary>Irregular ordinals that replace the entire output for specific numbers.</summary>
        public FrozenDictionary<int, string> ExactReplacements { get; } = ExactReplacements;
    }

    /// <summary>
    /// Configuration for the number-word-suffix ordinalizer engine.
    /// </summary>
    /// <param name="Masculine">The masculine gender block.</param>
    /// <param name="Feminine">The feminine gender block.</param>
    /// <param name="NeuterFallbackGender">The gender that neuter resolves to.</param>
    public readonly record struct Options(
        GenderBlock Masculine,
        GenderBlock Feminine,
        GrammaticalGender NeuterFallbackGender)
    {
        /// <summary>The masculine gender block.</summary>
        public GenderBlock Masculine { get; } = Masculine;

        /// <summary>The feminine gender block.</summary>
        public GenderBlock Feminine { get; } = Feminine;

        /// <summary>The gender that neuter resolves to (typically <see cref="GrammaticalGender.Masculine"/>).</summary>
        public GrammaticalGender NeuterFallbackGender { get; } = NeuterFallbackGender;
    }
}
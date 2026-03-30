namespace Humanizer;

sealed class WordFormTemplateOrdinalizer(CultureInfo culture, WordFormTemplateOrdinalizer.Options options) : DefaultOrdinalizer
{
    readonly CultureInfo culture = culture;
    readonly Options options = options;

    public override string Convert(int number, string numberString) =>
        Convert(number, numberString, GrammaticalGender.Masculine, WordForm.Normal);

    public override string Convert(int number, string numberString, GrammaticalGender gender) =>
        Convert(number, numberString, gender, WordForm.Normal);

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

    public readonly record struct PatternSet(Pattern Normal, Pattern Abbreviation)
    {
        public Pattern Normal { get; } = Normal;
        public Pattern Abbreviation { get; } = Abbreviation;
    }

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

    public enum NegativeNumberMode
    {
        None,
        AbsoluteInvariant,
        AbsoluteCulture
    }
}

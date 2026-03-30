namespace Humanizer;

sealed class TemplateOrdinalizer(TemplateOrdinalizer.Options options) : DefaultOrdinalizer
{
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

    public enum NegativeNumberMode
    {
        None,
        AbsoluteInvariant
    }
}

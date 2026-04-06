namespace Humanizer;

/// <summary>
/// Ordinalizer that appends a suffix chosen from exact-number, range, and digit-based rules.
/// </summary>
class ModuloSuffixOrdinalizer(ModuloSuffixOrdinalizer.Options options) : DefaultOrdinalizer
{
    readonly Options options = options;

    /// <summary>
    /// Appends the configured suffix for <paramref name="number"/> to <paramref name="numberString"/>.
    /// </summary>
    public override string Convert(int number, string numberString) =>
        numberString + GetSuffix(number);

    string GetSuffix(int number)
    {
        var comparisonValue = options.UseAbsoluteValue
            ? Math.Abs((long)number)
            : number;

        // The precedence matters: range rules, exact numbers, and threshold rules can all override
        // the general last-digit fallback for the same locale.
        if (options.LastTwoDigitsRange is { } lastTwoDigitsRange)
        {
            var lastTwoDigits = Math.Abs(comparisonValue % 100);
            if (lastTwoDigits >= lastTwoDigitsRange.Start &&
                lastTwoDigits <= lastTwoDigitsRange.End)
            {
                return lastTwoDigitsRange.Suffix;
            }
        }

        if (comparisonValue <= int.MaxValue &&
            options.ExactSuffixes.TryGetValue((int)comparisonValue, out var exactSuffix))
        {
            return exactSuffix;
        }

        if (options.AbsoluteAtLeast is { } absoluteAtLeast &&
            comparisonValue >= absoluteAtLeast &&
            options.AbsoluteAtLeastSuffix is not null)
        {
            return options.AbsoluteAtLeastSuffix;
        }

        var lastDigit = (int)Math.Abs(comparisonValue % 10);
        return options.LastDigitSuffixes.TryGetValue(lastDigit, out var lastDigitSuffix)
            ? lastDigitSuffix
            : options.DefaultSuffix;
    }

    /// <summary>
    /// Represents an inclusive last-two-digit range that maps to a specific suffix.
    /// </summary>
    /// <param name="Start">The first two-digit value in the range.</param>
    /// <param name="End">The last two-digit value in the range.</param>
    /// <param name="Suffix">The suffix to use for values in the range.</param>
    public readonly record struct RangeRule(int Start, int End, string Suffix);

    /// <summary>
    /// Configures suffix selection for <see cref="ModuloSuffixOrdinalizer"/>.
    /// </summary>
    /// <param name="DefaultSuffix">The fallback suffix used when no other rule matches.</param>
    /// <param name="ExactSuffixes">Exact-number suffix overrides.</param>
    /// <param name="LastDigitSuffixes">Suffixes keyed by the absolute last digit.</param>
    /// <param name="LastTwoDigitsRange">An optional inclusive range of last-two-digit values.</param>
    /// <param name="AbsoluteAtLeast">An optional lower bound that forces a dedicated suffix.</param>
    /// <param name="AbsoluteAtLeastSuffix">The suffix to use when <paramref name="AbsoluteAtLeast"/> matches.</param>
    /// <param name="UseAbsoluteValue">Whether to evaluate rules against the absolute number.</param>
    public readonly record struct Options(
        string DefaultSuffix,
        FrozenDictionary<int, string> ExactSuffixes,
        FrozenDictionary<int, string> LastDigitSuffixes,
        RangeRule? LastTwoDigitsRange = null,
        long? AbsoluteAtLeast = null,
        string? AbsoluteAtLeastSuffix = null,
        bool UseAbsoluteValue = false)
    {
        public string DefaultSuffix { get; } = DefaultSuffix;
        public FrozenDictionary<int, string> ExactSuffixes { get; } = ExactSuffixes;
        public FrozenDictionary<int, string> LastDigitSuffixes { get; } = LastDigitSuffixes;
        public RangeRule? LastTwoDigitsRange { get; } = LastTwoDigitsRange;
        public long? AbsoluteAtLeast { get; } = AbsoluteAtLeast;
        public string? AbsoluteAtLeastSuffix { get; } = AbsoluteAtLeastSuffix;
        public bool UseAbsoluteValue { get; } = UseAbsoluteValue;
    }
}
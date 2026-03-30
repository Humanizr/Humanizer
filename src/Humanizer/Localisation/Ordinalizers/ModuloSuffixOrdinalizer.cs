namespace Humanizer;

sealed class ModuloSuffixOrdinalizer(ModuloSuffixOrdinalizer.Options options) : DefaultOrdinalizer
{
    readonly Options options = options;

    public override string Convert(int number, string numberString) =>
        numberString + GetSuffix(number);

    string GetSuffix(int number)
    {
        var comparisonValue = options.UseAbsoluteValue
            ? Math.Abs((long)number)
            : number;

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

    public readonly record struct RangeRule(int Start, int End, string Suffix);

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

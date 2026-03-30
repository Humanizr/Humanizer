#if NET6_0_OR_GREATER

namespace Humanizer;

class PatternDateOnlyToOrdinalWordsConverter(OrdinalDatePattern pattern) : DefaultDateOnlyToOrdinalWordConverter
{
    readonly OrdinalDatePattern pattern = pattern;

    public override string Convert(DateOnly date) =>
        pattern.Format(date);
}

#endif

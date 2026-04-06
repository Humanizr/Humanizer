#if NET6_0_OR_GREATER

namespace Humanizer;

/// <summary>
/// Formats <see cref="DateOnly"/> values using a prebuilt <see cref="OrdinalDatePattern"/>.
/// </summary>
class PatternDateOnlyToOrdinalWordsConverter(OrdinalDatePattern pattern) : DefaultDateOnlyToOrdinalWordConverter
{
    readonly OrdinalDatePattern pattern = pattern;

    /// <summary>
    /// Formats the given date using the configured ordinal date pattern.
    /// </summary>
    /// <returns>The formatted ordinal-date string.</returns>
    public override string Convert(DateOnly date) =>
        pattern.Format(date);
}

#endif
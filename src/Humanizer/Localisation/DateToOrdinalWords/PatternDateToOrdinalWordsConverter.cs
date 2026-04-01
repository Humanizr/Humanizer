namespace Humanizer;

/// <summary>
/// Formats <see cref="DateTime"/> values using a prebuilt <see cref="OrdinalDatePattern"/>.
/// </summary>
class PatternDateToOrdinalWordsConverter(OrdinalDatePattern pattern) : DefaultDateToOrdinalWordConverter
{
    readonly OrdinalDatePattern pattern = pattern;

    /// <summary>
    /// Formats the given date using the configured ordinal date pattern.
    /// </summary>
    /// <returns>The formatted ordinal-date string.</returns>
    public override string Convert(DateTime date) =>
        pattern.Format(date);
}

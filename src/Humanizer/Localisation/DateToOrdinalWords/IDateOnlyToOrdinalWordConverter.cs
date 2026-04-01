#if NET6_0_OR_GREATER

namespace Humanizer;

/// <summary>
/// Converts dates into the localized text used by <c>ToOrdinalWords</c>.
/// </summary>
public interface IDateOnlyToOrdinalWordConverter
{
    /// <summary>
    /// Converts the given <paramref name="date"/> to ordinal words for the current culture.
    /// </summary>
    /// <param name="date">The date to format.</param>
    /// <returns>The localized ordinal-date string.</returns>
    string Convert(DateOnly date);

    /// <summary>
    /// Converts the given <paramref name="date"/> to ordinal words using the specified grammatical case.
    /// </summary>
    /// <param name="date">The date to format.</param>
    /// <param name="grammaticalCase">The grammatical case to apply when the locale supports case-specific date forms.</param>
    /// <returns>The localized ordinal-date string.</returns>
    string Convert(DateOnly date, GrammaticalCase grammaticalCase);
}
#endif

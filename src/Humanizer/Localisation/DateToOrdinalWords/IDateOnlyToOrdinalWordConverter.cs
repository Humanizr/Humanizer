#if NET6_0_OR_GREATER

namespace Humanizer;

/// <summary>
/// The interface used to localise the ToOrdinalWords method.
/// </summary>
public interface IDateOnlyToOrdinalWordConverter
{
    /// <summary>
    /// Converts the date to Ordinal Words
    /// </summary>
    string Convert(DateOnly date);

    /// <summary>
    /// Converts the date to Ordinal Words using the provided grammatical case
    /// </summary>
    string Convert(DateOnly date, GrammaticalCase grammaticalCase);
}
#endif

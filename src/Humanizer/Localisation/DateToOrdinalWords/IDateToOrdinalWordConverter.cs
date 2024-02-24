namespace Humanizer;

/// <summary>
/// The interface used to localise the ToOrdinalWords method.
/// </summary>
public interface IDateToOrdinalWordConverter
{
    /// <summary>
    /// Converts the date to Ordinal Words
    /// </summary>
    string Convert(DateTime date);

    /// <summary>
    /// Converts the date to Ordinal Words using the provided grammatical case
    /// </summary>
    string Convert(DateTime date, GrammaticalCase grammaticalCase);
}
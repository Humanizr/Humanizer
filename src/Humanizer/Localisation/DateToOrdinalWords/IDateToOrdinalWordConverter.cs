using System;

namespace Humanizer.Localisation.DateToOrdinalWords
{
    /// <summary>
    /// The interface used to localise the ToOrdinalWords method.
    /// </summary>
    public interface IDateToOrdinalWordConverter
    {
        /// <summary>
        /// Converts the date to Ordinal Words 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        string Convert(DateTime date);

        /// <summary>
        /// Converts the date to Ordinal Words using the provided grammatical case
        /// </summary>
        /// <param name="date"></param>
        /// <param name="grammaticalCase"></param>
        /// <returns></returns>
        string Convert(DateTime date, GrammaticalCase grammaticalCase);
    }
}

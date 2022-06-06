using System;

namespace Humanizer.Localisation.DateToWords
{
    /// <summary>
    /// The interface used to localise the ToWords method.
    /// </summary>
    public interface IDateToWordConverter
    {
        /// <summary>
        /// Converts the date to Words 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        string Convert(DateTime date);

        /// <summary>
        /// Converts the date to Words using the provided grammatical case
        /// </summary>
        /// <param name="date"></param>
        /// <param name="grammaticalCase"></param>
        /// <returns></returns>
        string Convert(DateTime date, GrammaticalCase grammaticalCase);
    }
}

using System;
using System.Globalization;
using Humanizer.Configuration;

namespace Humanizer
{
    /// <summary>
    /// Humanizes DateTime into human readable sentence
    /// </summary>
    public static class DateToOrdinalWordsExtensions
    {
        /// <summary>
        /// Turns the provided date into ordinal words
        /// </summary>
        /// <param name="input">The date to be made into ordinal words</param>
        /// <returns>The date in ordinal words</returns>
        public static string ToOrdinalWords(this DateTime input)
        {
            return input.Day.Ordinalize() + input.ToString(" MMMM yyyy");
        }
        /// <summary>
        /// Turns the provided date into ordinal words
        /// </summary>
        /// <param name="input">The date to be made into ordinal words</param>
        /// <param name="gender">The grammatical gender to use for output words</param>
        /// <returns>The date in ordinal words</returns>
        public static string ToOrdinalWords(this DateTime input, GrammaticalGender gender)
        {
            return input.Day.Ordinalize(gender) + input.ToString(" MMMM yyyy");
        }
    }
}
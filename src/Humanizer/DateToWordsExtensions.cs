using System;
using Humanizer.Configuration;

namespace Humanizer
{
    /// <summary>
    /// Humanizes DateTime into human readable sentence
    /// </summary>
    public static class DateToWordsExtensions
    {
        /// <summary>
        /// Turns the provided date into words
        /// </summary>
        /// <param name="input">The date to be made into words</param>
        /// <returns>The date in words</returns>
        public static string ToWords(this DateTime input)
        {
            return Configurator.DateToWordsConverter.Convert(input);
        }
        /// <summary>
        /// Turns the provided date into words
        /// </summary>
        /// <param name="input">The date to be made into words</param>
        /// <param name="grammaticalCase">The grammatical case to use for output words</param>
        /// <returns>The date in words</returns>
        public static string ToWords(this DateTime input, GrammaticalCase grammaticalCase)
        {
            return Configurator.DateToWordsConverter.Convert(input, grammaticalCase);
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// Turns the provided date into words
        /// </summary>
        /// <param name="input">The date to be made into words</param>
        /// <returns>The date in words</returns>
        public static string ToWords(this DateOnly input)
        {
            return Configurator.DateOnlyToWordsConverter.Convert(input);
        }
        /// <summary>
        /// Turns the provided date into words
        /// </summary>
        /// <param name="input">The date to be made into words</param>
        /// <param name="grammaticalCase">The grammatical case to use for output words</param>
        /// <returns>The date in words</returns>
        public static string ToWords(this DateOnly input, GrammaticalCase grammaticalCase)
        {
            return Configurator.DateOnlyToWordsConverter.Convert(input, grammaticalCase);
        }
#endif
    }
}

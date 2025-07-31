using System.Globalization;

namespace Humanizer
{
    /// <summary>
    /// Transform humanized string to number; e.g. one => 1
    /// </summary>
    public static class WordsToNumberExtension
    {
        /// <summary>
        /// Throws an exception if any word is not recognized.
        /// </summary>
        /// <param name="words">The spelled-out number (e.g., "three hundred twenty-one").</param>
        /// <param name="culture">The culture used for parsing (e.g., en-US).</param>
        /// <returns>The integer value represented by the words.</returns>
        /// <exception cref="FormatException">Thrown if the input contains unrecognized words.</exception>
        public static int ToNumber(this string words, CultureInfo culture)
            => Configurator.GetWordsToNumberConverter(culture).Convert(words);

        /// <summary>
        /// Returns false if the input is not a valid spelled-out number.
        /// </summary>
        /// <param name="words">The spelled-out number (e.g., "forty-two").</param>
        /// <param name="parsedNumber">The parsed integer result if successful; otherwise 0.</param>
        /// <param name="culture">The culture used for parsing.</param>
        /// <returns>True if conversion was successful; otherwise false.</returns>
        public static bool TryToNumber(this string words, out int parsedNumber, CultureInfo culture)
            => Configurator.GetWordsToNumberConverter(culture).TryConvert(words, out parsedNumber);

        /// <summary>
        /// Returns false if any word is unrecognized, and provides the first invalid word.
        /// </summary>
        /// <param name="words">The spelled-out number (e.g., "one thousand one").</param>
        /// <param name="parsedNumber">The parsed integer result if successful; otherwise 0.</param>
        /// <param name="culture">The culture used for parsing.</param>
        /// <param name="unrecognizedWord">
        /// The first unrecognized word found in the input; null if all words are valid.
        /// </param>
        /// <returns>True if conversion was successful; otherwise false.</returns>
        public static bool TryToNumber(this string words, out int parsedNumber, CultureInfo culture, out string? unrecognizedWord)
            => Configurator.GetWordsToNumberConverter(culture).TryConvert(words, out parsedNumber, out unrecognizedWord);

    }
}

using System.Globalization;
using Humanizer.Configuration;

namespace Humanizer
{
    /// <summary>
    /// Transform a number into words; e.g. 1 => one
    /// </summary>
    public static class NumberToWordsExtension
    {
        /// <summary>
        /// 3501.ToWords() -> "three thousand five hundred and one"
        /// </summary>
        /// <param name="number">Number to be turned to words</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <returns></returns>
        public static string ToWords(this int number, CultureInfo culture = null)
        {
            return Configurator.GetNumberToWordsConverter(culture).Convert(number);
        }

        /// <summary>
        /// For locales that support gender-specific forms
        /// </summary>
        /// <example>
        /// Russian:
        /// <code>
        ///   1.ToWords(GrammaticalGender.Masculine) -> "один"
        ///   1.ToWords(GrammaticalGender.Feminine) -> "одна"
        /// </code>
        /// Hebrew:
        /// <code>
        ///   1.ToWords(GrammaticalGender.Masculine) -> "אחד"
        ///   1.ToWords(GrammaticalGender.Feminine) -> "אחת"
        /// </code>
        /// </example>
        /// 
        /// <param name="number">Number to be turned to words</param>
        /// <param name="gender">The grammatical gender to use for output words</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <returns></returns>
        public static string ToWords(this int number, GrammaticalGender gender, CultureInfo culture = null)
        {
            return Configurator.GetNumberToWordsConverter(culture).Convert(number, gender);
        }

        /// <summary>
        /// 1.ToOrdinalWords() -> "first"
        /// </summary>
        /// <param name="number">Number to be turned to ordinal words</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <returns></returns>
        public static string ToOrdinalWords(this int number, CultureInfo culture = null)
        {
            return Configurator.GetNumberToWordsConverter(culture).ConvertToOrdinal(number);
        }

        /// <summary>
        /// for Brazilian Portuguese locale
        /// 1.ToOrdinalWords(GrammaticalGender.Masculine) -> "primeiro"
        /// 1.ToOrdinalWords(GrammaticalGender.Feminine) -> "primeira"
        /// </summary>
        /// <param name="number">Number to be turned to words</param>
        /// <param name="gender">The grammatical gender to use for output words</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <returns></returns>
        public static string ToOrdinalWords(this int number, GrammaticalGender gender, CultureInfo culture = null)
        {
            return Configurator.GetNumberToWordsConverter(culture).ConvertToOrdinal(number, gender);
        }
    }
}

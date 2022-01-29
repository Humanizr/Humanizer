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
        /// Converts a number to ordinal words supporting locale's specific variations.
        /// </summary>
        /// <example>
        /// In Spanish:
        /// <code>
        /// 1.ToOrdinalWords(WordForm.Normal) -> "primero" // As in "He llegado el primero".
        /// 3.ToOrdinalWords(WordForm.Abbreviation) -> "tercer" // As in "Vivo en el tercer piso"
        /// </code>
        /// </example>
        /// <param name="number">Number to be turned to ordinal words</param>
        /// <param name="wordForm">Form of the word, i.e. abbreviation</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <returns>The number converted into ordinal words</returns>
        public static string ToOrdinalWords(this int number, WordForm wordForm, CultureInfo culture = null)
        {
            return Configurator.GetNumberToWordsConverter(culture).ConvertToOrdinal(number, wordForm);
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

        /// <summary>
        /// Converts a number to ordinal words supporting locale's specific variations.
        /// </summary>
        /// <example>
        /// In Spanish:
        /// <code>
        /// 3.ToOrdinalWords(GrammaticalGender.Masculine, WordForm.Normal) -> "tercero"
        /// 3.ToOrdinalWords(GrammaticalGender.Masculine, WordForm.Abbreviation) -> "tercer"
        /// 3.ToOrdinalWords(GrammaticalGender.Feminine, WordForm.Normal) -> "tercera"
        /// 3.ToOrdinalWords(GrammaticalGender.Feminine, WordForm.Abbreviation) -> "tercera"
        /// </code>
        /// </example>
        /// <param name="number">Number to be turned to ordinal words</param>
        /// <param name="gender">The grammatical gender to use for output words</param>
        /// <param name="wordForm">Form of the word, i.e. abbreviation</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <returns>The number converted into ordinal words</returns>
        public static string ToOrdinalWords(this int number, GrammaticalGender gender, WordForm wordForm, CultureInfo culture = null)
        {
            return Configurator.GetNumberToWordsConverter(culture).ConvertToOrdinal(number, gender, wordForm);
        }

        /// <summary>
        /// 1.ToTuple() -> "single"
        /// </summary>
        /// <param name="number">Number to be turned to tuple</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <returns></returns>
        public static string ToTuple(this int number, CultureInfo culture = null)
        {
            return Configurator.GetNumberToWordsConverter(culture).ConvertToTuple(number);
        }

        /// <summary>
        /// 3501.ToWords() -> "three thousand five hundred and one"
        /// </summary>
        /// <param name="number">Number to be turned to words</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <returns></returns>
        public static string ToWords(this int number, CultureInfo culture = null)
        {
            return ((long)number).ToWords(culture);
        }

        /// <summary>
        /// Converts a number to words supporting specific word variations, including grammatical gender, of some locales.
        /// </summary>
        /// <example>
        /// In Spanish, numbers ended in 1 change its form depending on their position in the sentence.
        /// <code>
        /// 21.ToWords(WordForm.Normal) -> veintiuno // as in "Mi número favorito es el veintiuno".
        /// 21.ToWords(WordForm.Abbreviation) -> veintiún // as in "En total, conté veintiún coches"
        /// </code>
        /// </example>
        /// <param name="number">Number to be turned to words</param>
        /// <param name="wordForm">Form of the word, i.e. abbreviation</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <returns>The number converted to words</returns>
        public static string ToWords(this int number, WordForm wordForm, CultureInfo culture = null)
        {
            return ((long)number).ToWords(wordForm, culture);
        }

        /// <summary>
        /// 3501.ToWords(false) -> "three thousand five hundred one"
        /// </summary>
        /// <param name="number">Number to be turned to words</param>
        /// <param name="addAnd">To add 'and' before the last number.</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <returns></returns>
        public static string ToWords(this int number, bool addAnd, CultureInfo culture = null)
        {
            return ((long)number).ToWords(culture, addAnd);
        }

        /// <summary>
        /// Converts a number to words supporting specific word variations of some locales.
        /// </summary>
        /// <example>
        /// In Spanish, numbers ended in 1 changes its form depending on their position in the sentence.
        /// <code>
        /// 21.ToWords(WordForm.Normal) -> veintiuno // as in "Mi número favorito es el veintiuno".
        /// 21.ToWords(WordForm.Abbreviation) -> veintiún // as in "En total, conté veintiún coches"
        /// </code>
        /// </example>
        /// <param name="number">Number to be turned to words</param>
        /// <param name="addAnd">To add 'and' before the last number</param>
        /// <param name="wordForm">Form of the word, i.e. abbreviation</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <returns>The number converted to words</returns>
        public static string ToWords(this int number, bool addAnd, WordForm wordForm, CultureInfo culture = null)
        {
            return ((long)number).ToWords(wordForm, culture, addAnd);
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
        /// <param name="number">Number to be turned to words</param>
        /// <param name="gender">The grammatical gender to use for output words</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <returns></returns>
        public static string ToWords(this int number, GrammaticalGender gender, CultureInfo culture = null)
        {
            return ((long)number).ToWords(gender, culture);
        }

        /// <summary>
        /// Converts a number to words supporting specific word variations, including grammatical gender, of some locales.
        /// </summary>
        /// <example>
        /// In Spanish, numbers ended in 1 change its form depending on their position in the sentence.
        /// <code>
        /// 21.ToWords(WordForm.Normal, GrammaticalGender.Masculine) -> veintiuno // as in "Mi número favorito es el veintiuno".
        /// 21.ToWords(WordForm.Abbreviation, GrammaticalGender.Masculine) -> veintiún // as in "En total, conté veintiún coches"
        /// 21.ToWords(WordForm.Normal, GrammaticalGender.Feminine) -> veintiuna // as in "veintiuna personas"
        /// </code>
        /// </example>
        /// <param name="number">Number to be turned to words</param>
        /// <param name="wordForm">Form of the word, i.e. abbreviation</param>
        /// <param name="gender">The grammatical gender to use for output words</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <returns>The number converted to words</returns>
        public static string ToWords(this int number, WordForm wordForm, GrammaticalGender gender, CultureInfo culture = null)
        {
            return ((long)number).ToWords(wordForm, gender, culture);
        }

        /// <summary>
        /// 3501.ToWords() -> "three thousand five hundred and one"
        /// </summary>
        /// <param name="number">Number to be turned to words</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <param name="addAnd">Whether "and" should be included or not.</param>
        /// <returns></returns>
        public static string ToWords(this long number, CultureInfo culture = null, bool addAnd = true)
        {
            return Configurator.GetNumberToWordsConverter(culture).Convert(number, addAnd);
        }

        /// <summary>
        /// Converts a number to words supporting specific word variations of some locales.
        /// </summary>
        /// <example>
        /// In Spanish, numbers ended in 1 changes its form depending on their position in the sentence.
        /// <code>
        /// 21.ToWords(WordForm.Normal) -> veintiuno // as in "Mi número favorito es el veintiuno".
        /// 21.ToWords(WordForm.Abbreviation) -> veintiún // as in "En total, conté veintiún coches"
        /// </code>
        /// </example>
        /// <param name="number">Number to be turned to words</param>
        /// <param name="wordForm">Form of the word, i.e. abbreviation</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <param name="addAnd">To add 'and' before the last number</param>
        /// <returns>The number converted to words</returns>
        public static string ToWords(this long number, WordForm wordForm, CultureInfo culture = null, bool addAnd = false)
        {
            return Configurator.GetNumberToWordsConverter(culture).Convert(number, addAnd, wordForm);
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
        public static string ToWords(this long number, GrammaticalGender gender, CultureInfo culture = null)
        {
            return Configurator.GetNumberToWordsConverter(culture).Convert(number, gender);
        }

        /// <summary>
        /// Converts a number to words supporting specific word variations, including grammatical gender, of some locales.
        /// </summary>
        /// <example>
        /// In Spanish, numbers ended in 1 changes its form depending on their position in the sentence.
        /// <code>
        /// 21.ToWords(WordForm.Normal, GrammaticalGender.Masculine) -> veintiuno // as in "Mi número favorito es el veintiuno".
        /// 21.ToWords(WordForm.Abbreviation, GrammaticalGender.Masculine) -> veintiún // as in "En total, conté veintiún coches"
        /// 21.ToWords(WordForm.Normal, GrammaticalGender.Feminine) -> veintiuna // as in "veintiuna personas"
        /// </code>
        /// </example>
        /// <param name="number">Number to be turned to words</param>
        /// <param name="wordForm">Form of the word, i.e. abbreviation</param>
        /// <param name="gender">The grammatical gender to use for output words</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <returns>The number converted to words</returns>
        public static string ToWords(this long number, WordForm wordForm, GrammaticalGender gender, CultureInfo culture = null)
        {
            return Configurator.GetNumberToWordsConverter(culture).Convert(number, wordForm, gender);
        }
    }
}

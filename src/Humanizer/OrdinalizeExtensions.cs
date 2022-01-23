using System.Globalization;

using Humanizer.Configuration;

namespace Humanizer
{
    /// <summary>
    /// Ordinalize extensions
    /// </summary>
    public static class OrdinalizeExtensions
    {
        /// <summary>
        /// Turns a number into an ordinal string used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th.
        /// </summary>
        /// <param name="numberString">The number, in string, to be ordinalized</param>
        /// <returns></returns>
        public static string Ordinalize(this string numberString)
        {
            return Configurator.Ordinalizer.Convert(int.Parse(numberString), numberString);
        }

        /// <summary>
        /// Turns a number into an ordinal number used to denote the position in an ordered sequence supporting specific locale's variations.
        /// </summary>
        /// <example>
        /// In Spanish:
        /// <code>
        /// "1".Ordinalize(WordForm.Abbreviation) -> 1.er // As in "Vivo en el 1.er piso"
        /// "1".Ordinalize(WordForm.Normal) -> 1.º // As in "Fui el 1º de mi promoción"
        /// </code>
        /// </example>
        /// <param name="numberString">The number, in string, to be ordinalized</param>
        /// <param name="wordForm">Form of the word, i.e. abbreviation</param>
        /// <returns>The number ordinalized</returns>
        public static string Ordinalize(this string numberString, WordForm wordForm)
        {
            return Configurator.Ordinalizer.Convert(int.Parse(numberString), numberString, wordForm);
        }

        /// <summary>
        /// Turns a number into an ordinal string used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th.
        /// </summary>
        /// <param name="numberString">The number, in string, to be ordinalized</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <returns></returns>
        public static string Ordinalize(this string numberString, CultureInfo culture)
        {
            return Configurator.Ordinalizers.ResolveForCulture(culture).Convert(int.Parse(numberString, culture), numberString);
        }

        /// <summary>
        /// Turns a number into an ordinal number used to denote the position in an ordered sequence supporting specific locale's variations.
        /// </summary>
        /// <example>
        /// In Spanish:
        /// <code>
        /// "1".Ordinalize(new CultureInfo("es-ES"),WordForm.Abbreviation) -> 1.er // As in "Vivo en el 1.er piso"
        /// "1".Ordinalize(new CultureInfo("es-ES"), WordForm.Normal) -> 1.º // As in "Fui el 1º de mi promoción"
        /// </code>
        /// </example>
        /// <param name="numberString">The number to be ordinalized</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <param name="wordForm">Form of the word, i.e. abbreviation</param>
        /// <returns>The number ordinalized</returns>
        public static string Ordinalize(this string numberString, CultureInfo culture, WordForm wordForm)
        {
            return Configurator.Ordinalizers.ResolveForCulture(culture).Convert(int.Parse(numberString, culture), numberString, wordForm);
        }

        /// <summary>
        /// Turns a number into an ordinal string used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th.
        /// Gender for Brazilian Portuguese locale
        /// "1".Ordinalize(GrammaticalGender.Masculine) -> "1º"
        /// "1".Ordinalize(GrammaticalGender.Feminine) -> "1ª"
        /// </summary>
        /// <param name="numberString">The number, in string, to be ordinalized</param>
        /// <param name="gender">The grammatical gender to use for output words</param>
        /// <returns></returns>
        public static string Ordinalize(this string numberString, GrammaticalGender gender)
        {
            return Configurator.Ordinalizer.Convert(int.Parse(numberString), numberString, gender);
        }

        /// <summary>
        /// Turns a number into an ordinal number used to denote the position in an ordered sequence supporting specific
        /// locale's variations using the grammatical gender provided
        /// </summary>
        /// <example>
        /// In Spanish:
        /// <code>
        /// "1".Ordinalize(GrammaticalGender.Masculine, WordForm.Abbreviation) -> 1.er // As in "Vivo en el 1.er piso"
        /// "1".Ordinalize(GrammaticalGender.Masculine, WordForm.Normal) -> 1.º // As in "Fui el 1º de mi promoción"
        /// "1".Ordinalize(GrammaticalGender.Feminine, WordForm.Normal) -> 1.ª // As in "Es 1ª vez que hago esto"
        /// </code>
        /// </example>
        /// <param name="numberString">The number to be ordinalized</param>
        /// <param name="gender">The grammatical gender to use for output words</param>
        /// <param name="wordForm">Form of the word, i.e. abbreviation</param>
        /// <returns>The number ordinalized</returns>
        public static string Ordinalize(this string numberString, GrammaticalGender gender, WordForm wordForm)
        {
            return Configurator.Ordinalizer.Convert(int.Parse(numberString), numberString, gender, wordForm);
        }

        /// <summary>
        /// Turns a number into an ordinal string used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th.
        /// Gender for Brazilian Portuguese locale
        /// "1".Ordinalize(GrammaticalGender.Masculine) -> "1º"
        /// "1".Ordinalize(GrammaticalGender.Feminine) -> "1ª"
        /// </summary>
        /// <param name="numberString">The number, in string, to be ordinalized</param>
        /// <param name="gender">The grammatical gender to use for output words</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <returns></returns>
        public static string Ordinalize(this string numberString, GrammaticalGender gender, CultureInfo culture)
        {
            return Configurator.Ordinalizers.ResolveForCulture(culture).Convert(int.Parse(numberString, culture), numberString, gender);
        }

        /// <summary>
        /// Turns a number into an ordinal number used to denote the position in an ordered sequence supporting specific
        /// locale's variations using the grammatical gender provided
        /// </summary>
        /// <example>
        /// In Spanish:
        /// <code>
        /// "1".Ordinalize(GrammaticalGender.Masculine, new CultureInfo("es-ES"),WordForm.Abbreviation) -> 1.er // As in "Vivo en el 1.er piso"
        /// "1".Ordinalize(GrammaticalGender.Masculine, new CultureInfo("es-ES"), WordForm.Normal) -> 1.º // As in "Fui el 1º de mi promoción"
        /// "1".Ordinalize(GrammaticalGender.Feminine, new CultureInfo("es-ES"), WordForm.Normal) -> 1.º // As in "Fui el 1º de mi promoción"
        /// </code>
        /// </example>
        /// <param name="numberString">The number to be ordinalized</param>
        /// <param name="gender">The grammatical gender to use for output words</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <param name="wordForm">Form of the word, i.e. abbreviation</param>
        /// <returns>The number ordinalized</returns>
        public static string Ordinalize(this string numberString, GrammaticalGender gender, CultureInfo culture, WordForm wordForm)
        {
            return Configurator.Ordinalizers.ResolveForCulture(culture).Convert(int.Parse(numberString, culture), numberString, gender, wordForm);
        }

        /// <summary>
        /// Turns a number into an ordinal number used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th.
        /// </summary>
        /// <param name="number">The number to be ordinalized</param>
        /// <returns></returns>
        public static string Ordinalize(this int number)
        {
            return Configurator.Ordinalizer.Convert(number, number.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Turns a number into an ordinal number used to denote the position in an ordered sequence supporting specific locale's variations.
        /// </summary>
        /// <example>
        /// In Spanish:
        /// <code>
        /// 1.Ordinalize(WordForm.Abbreviation) -> 1.er // As in "Vivo en el 1.er piso"
        /// 1.Ordinalize(WordForm.Normal) -> 1.º // As in "Fui el 1º de mi promoción"
        /// </code>
        /// </example>
        /// <param name="number">The number to be ordinalized</param>
        /// <param name="wordForm">Form of the word, i.e. abbreviation</param>
        /// <returns>The number ordinalized</returns>
        public static string Ordinalize(this int number, WordForm wordForm)
        {
            return Configurator.Ordinalizer.Convert(number, number.ToString(CultureInfo.InvariantCulture), wordForm);
        }

        /// <summary>
        /// Turns a number into an ordinal number used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th.
        /// </summary>
        /// <param name="number">The number to be ordinalized</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <returns></returns>
        public static string Ordinalize(this int number, CultureInfo culture)
        {
            return Configurator.Ordinalizers.ResolveForCulture(culture).Convert(number, number.ToString(culture));
        }

        /// <summary>
        /// Turns a number into an ordinal number used to denote the position in an ordered sequence supporting specific locale's variations.
        /// </summary>
        /// <example>
        /// In Spanish:
        /// <code>
        /// 1.Ordinalize(new CultureInfo("es-ES"),WordForm.Abbreviation) -> 1.er // As in "Vivo en el 1.er piso"
        /// 1.Ordinalize(new CultureInfo("es-ES"), WordForm.Normal) -> 1.º // As in "Fui el 1º de mi promoción"
        /// </code>
        /// </example>
        /// <param name="number">The number to be ordinalized</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <param name="wordForm">Form of the word, i.e. abbreviation</param>
        /// <returns>The number ordinalized</returns>
        public static string Ordinalize(this int number, CultureInfo culture, WordForm wordForm)
        {
            return Configurator.Ordinalizers.ResolveForCulture(culture).Convert(number, number.ToString(culture), wordForm);
        }

        /// <summary>
        /// Turns a number into an ordinal number used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th.
        /// Gender for Brazilian Portuguese locale
        /// 1.Ordinalize(GrammaticalGender.Masculine) -> "1º"
        /// 1.Ordinalize(GrammaticalGender.Feminine) -> "1ª"
        /// </summary>
        /// <param name="number">The number to be ordinalized</param>
        /// <param name="gender">The grammatical gender to use for output words</param>
        /// <returns></returns>
        public static string Ordinalize(this int number, GrammaticalGender gender)
        {
            return Configurator.Ordinalizer.Convert(number, number.ToString(CultureInfo.InvariantCulture), gender);
        }

        /// <summary>
        /// Turns a number into an ordinal number used to denote the position in an ordered sequence supporting specific
        /// locale's variations using the grammatical gender provided
        /// </summary>
        /// <example>
        /// In Spanish:
        /// <code>
        /// 1.Ordinalize(GrammaticalGender.Masculine, WordForm.Abbreviation) -> 1.er // As in "Vivo en el 1.er piso"
        /// 1.Ordinalize(GrammaticalGender.Masculine, WordForm.Normal) -> 1.º // As in "Fui el 1º de mi promoción"
        /// 1.Ordinalize(GrammaticalGender.Feminine, WordForm.Normal) -> 1.ª // As in "Es 1ª vez que hago esto"
        /// </code>
        /// </example>
        /// <param name="number">The number to be ordinalized</param>
        /// <param name="gender">The grammatical gender to use for output words</param>
        /// <param name="wordForm">Form of the word, i.e. abbreviation</param>
        /// <returns>The number ordinalized</returns>
        public static string Ordinalize(this int number, GrammaticalGender gender, WordForm wordForm)
        {
            return Configurator.Ordinalizer.Convert(number, number.ToString(CultureInfo.InvariantCulture), gender, wordForm);
        }

        /// <summary>
        /// Turns a number into an ordinal number used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th.
        /// Gender for Brazilian Portuguese locale
        /// 1.Ordinalize(GrammaticalGender.Masculine) -> "1º"
        /// 1.Ordinalize(GrammaticalGender.Feminine) -> "1ª"
        /// </summary>
        /// <param name="number">The number to be ordinalized</param>
        /// <param name="gender">The grammatical gender to use for output words</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <returns></returns>
        public static string Ordinalize(this int number, GrammaticalGender gender, CultureInfo culture)
        {
            return Configurator.Ordinalizers.ResolveForCulture(culture).Convert(number, number.ToString(culture), gender);
        }

        /// <summary>
        /// Turns a number into an ordinal number used to denote the position in an ordered sequence supporting specific
        /// locale's variations using the grammatical gender provided
        /// </summary>
        /// <example>
        /// In Spanish:
        /// <code>
        /// 1.Ordinalize(GrammaticalGender.Masculine, new CultureInfo("es-ES"),WordForm.Abbreviation) -> 1.er // As in "Vivo en el 1.er piso"
        /// 1.Ordinalize(GrammaticalGender.Masculine, new CultureInfo("es-ES"), WordForm.Normal) -> 1.º // As in "Fui el 1º de mi promoción"
        /// 1.Ordinalize(GrammaticalGender.Feminine, new CultureInfo("es-ES"), WordForm.Normal) -> 1.º // As in "Fui el 1º de mi promoción"
        /// </code>
        /// </example>
        /// <param name="number">The number to be ordinalized</param>
        /// <param name="gender">The grammatical gender to use for output words</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <param name="wordForm">Form of the word, i.e. abbreviation</param>
        /// <returns>The number ordinalized</returns>
        public static string Ordinalize(this int number, GrammaticalGender gender, CultureInfo culture, WordForm wordForm)
        {
            return Configurator.Ordinalizers.ResolveForCulture(culture).Convert(number, number.ToString(culture), gender, wordForm);
        }
    }
}

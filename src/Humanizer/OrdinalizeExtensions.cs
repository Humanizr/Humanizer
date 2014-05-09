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
        /// Turns a number into an ordinal number used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th.
        /// </summary>
        /// <param name="number">The number to be ordinalized</param>
        /// <returns></returns>
        public static string Ordinalize(this int number)
        {
            return Configurator.Ordinalizer.Convert(number, number.ToString(CultureInfo.InvariantCulture));
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
    }
}
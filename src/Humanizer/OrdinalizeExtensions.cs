using System.Globalization;

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
            return Ordinalize(int.Parse(numberString), numberString);
        }

        /// <summary>
        /// Turns a number into an ordinal number used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th.
        /// </summary>
        /// <param name="number">The number to be ordinalized</param>
        /// <returns></returns>
        public static string Ordinalize(this int number)
        {
            return Ordinalize(number, number.ToString(CultureInfo.InvariantCulture));
        }

        private static string Ordinalize(int number, string numberString)
        {
            int nMod100 = number % 100;

            if (nMod100 >= 11 && nMod100 <= 13)
            {
                return numberString + "th";
            }

            switch (number % 10)
            {
                case 1:
                    return numberString + "st";
                case 2:
                    return numberString + "nd";
                case 3:
                    return numberString + "rd";
                default:
                    return numberString + "th";
            }
        }
    }
}

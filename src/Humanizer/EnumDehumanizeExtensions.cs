using System;
using System.Linq;

namespace Humanizer
{
    /// <summary>
    /// Contains extension methods for dehumanizing Enum string values.
    /// </summary>
    public static class EnumDehumanizeExtensions
    {
        /// <summary>
        /// Dehumanizes a string into the Enum it was originally Humanized from!
        /// </summary>
        /// <typeparam name="TTargetEnum">The target enum</typeparam>
        /// <param name="input">The string to be converted</param>
        /// <exception cref="ArgumentException">If TTargetEnum is not an enum</exception>
        /// <exception cref="NoMatchFoundException">Couldn't find any enum member that matches the string</exception>
        /// <returns></returns>
        public static TTargetEnum DehumanizeTo<TTargetEnum>(this string input)
            where TTargetEnum : struct, IComparable, IFormattable
        {
            return (TTargetEnum)DehumanizeToPrivate(input, typeof(TTargetEnum), OnNoMatch.ThrowsException);
        }

        /// <summary>
        /// Dehumanizes a string into the Enum it was originally Humanized from!
        /// </summary>
        /// <param name="input">The string to be converted</param>
        /// <param name="targetEnum">The target enum</param>
        /// <param name="onNoMatch">What to do when input is not matched to the enum.</param>
        /// <returns></returns>
        /// <exception cref="NoMatchFoundException">Couldn't find any enum member that matches the string</exception>
        /// <exception cref="ArgumentException">If targetEnum is not an enum</exception>
        public static Enum DehumanizeTo(this string input, Type targetEnum, OnNoMatch onNoMatch = OnNoMatch.ThrowsException)
        {
            return (Enum)DehumanizeToPrivate(input, targetEnum, onNoMatch);
        }

        private static object DehumanizeToPrivate(string input, Type targetEnum, OnNoMatch onNoMatch)
        {
            var match = Enum.GetValues(targetEnum).Cast<Enum>().FirstOrDefault(value => string.Equals(value.Humanize(), input, StringComparison.OrdinalIgnoreCase));

            if (match == null && onNoMatch == OnNoMatch.ThrowsException)
                throw new NoMatchFoundException("Couldn't find any enum member that matches the string " + input);

            return match;
        }
    }
}
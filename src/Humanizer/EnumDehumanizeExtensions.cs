using System;
using System.Linq;

namespace Humanizer
{
    public static class EnumDehumanizeExtensions
    {
        /// <summary>
        /// Dehumanizes a string into the Enum it was originally Humanized from!
        /// </summary>
        /// <typeparam name="TTargetEnum">The target enum</typeparam>
        /// <param name="input">The string to be converted</param>
        /// <exception cref="ArgumentException">If TTargetEnum is not an enum</exception>
        /// <exception cref="CannotMapToTargetException">If the provided string cannot be mapped to the target enum</exception>
        /// <returns></returns>
        public static TTargetEnum DehumanizeTo<TTargetEnum>(this string input) 
            where TTargetEnum : struct, IComparable, IFormattable, IConvertible
        {
            var values = Enum.GetValues(typeof(TTargetEnum)).Cast<TTargetEnum>();

            foreach (var value in values)
            {
                var @enum = value as Enum;
                if (string.Equals(@enum.Humanize(), input, StringComparison.OrdinalIgnoreCase))
                    return value;
            }

            throw new CannotMapToTargetException("Couldn't find any enum member that matches the string " + input);
        }

        /// <summary>
        /// Dehumanizes a string into the Enum it was originally Humanized from!
        /// </summary>
        /// <param name="input">The string to be converted</param>
        /// <param name="targetEnum">The target enum</param>
        /// <exception cref="ArgumentException">If targetEnum is not an enum</exception>
        /// <exception cref="CannotMapToTargetException">If the provided string cannot be mapped to the target enum</exception>
        /// <returns></returns>
        public static Enum DehumanizeTo(this string input, Type targetEnum) 
        {
            var values = Enum.GetValues(targetEnum);

            foreach (var value in values)
            {
                var @enum = value as Enum;
                if (string.Equals(@enum.Humanize(), input, StringComparison.OrdinalIgnoreCase))
                    return @enum;
            }

            throw new CannotMapToTargetException("Couldn't find any enum member that matches the string " + input);
        }
    }

    /// <summary>
    /// This is thrown on String.DehumanizeTo enum when the provided string cannot be mapped to the target enum
    /// </summary>
    public class CannotMapToTargetException : Exception
    {
        public CannotMapToTargetException()
        {
        }

        public CannotMapToTargetException(string message)
            : base(message)
        {
        }

        public CannotMapToTargetException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
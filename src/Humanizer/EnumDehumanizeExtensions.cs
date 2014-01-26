using System;
using System.Collections.Generic;
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
        /// <exception cref="KeyNotFoundException">If the string doesn't match to any of the dehuminized enum values</exception>
        /// <returns></returns>
        public static TTargetEnum DehumanizeTo<TTargetEnum>(this string input) where TTargetEnum : struct, IComparable, IFormattable, IConvertible
        {
            var values = Enum.GetValues(typeof(TTargetEnum)).Cast<TTargetEnum>();

            foreach (var value in values)
            {
                var @enum = value as Enum;

                if (string.Equals(@enum.Humanize(), input, StringComparison.OrdinalIgnoreCase))
                    return value;
            }

            throw new KeyNotFoundException("Couldn't find a dehumanized enum value that matches the string : " + input);
        }
    }
}
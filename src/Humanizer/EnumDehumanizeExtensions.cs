using System;

namespace Humanizer
{
    public static class EnumDehumanizeExtensions
    {
        /// <summary>
        /// Dehumanizes a string into the Enum it was originally Humanized from!
        /// </summary>
        /// <typeparam name="TTargetEnum">The target enum</typeparam>
        /// <param name="input">The string to be converted</param>
        /// <returns></returns>
        public static Enum DehumanizeTo<TTargetEnum>(this string input) 
        {
            var values = (TTargetEnum[]) Enum.GetValues(typeof (TTargetEnum));

            foreach (var value in values)
            {
                var enumValue = value as Enum;
                if (enumValue == null)
                    return null;

                if (string.Equals(enumValue.Humanize(), input, StringComparison.OrdinalIgnoreCase))
                    return enumValue;
            }

            return null;
        }
    }
}
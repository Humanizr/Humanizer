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
        /// <param name="casing">A hint to Dehumanizer around what casing was originally used for humanization</param>
        /// <returns></returns>
        public static Enum DehumanizeTo<TTargetEnum>(this string input, LetterCasing? casing = null) 
        {
            var values = (TTargetEnum[]) Enum.GetValues(typeof (TTargetEnum));
            Func<Enum, string> humanize = e => e.Humanize();
            if (casing != null)
                humanize = e => e.Humanize(casing.Value);

            foreach (var value in values)
            {
                var enumValue = value as Enum;
                if (enumValue == null)
                    return null;

                if (humanize(enumValue) == input)
                    return enumValue;
            }

            return null;
        }
    }
}
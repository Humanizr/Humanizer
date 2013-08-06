using System;

namespace Humanizer
{
    public static class EnumDehumanizeExtensions
    {
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
using System.ComponentModel.DataAnnotations;

namespace Humanizer
{
    /// <summary>
    /// Contains extension methods for humanizing Enums
    /// </summary>
    public static class EnumHumanizeExtensions
    {
        /// <summary>
        /// Turns an enum member into a human readable string; e.g. AnonymousUser -> Anonymous user. It also honors DescriptionAttribute data annotation
        /// </summary>
        /// <param name="input">The enum member to be humanized</param>
        public static string Humanize<T>(this T input)
            where T : struct, Enum
        {
            var type = input.GetType();

            if (IsBitFieldEnum(type) && !Enum.IsDefined(type, input))
            {
                return EnumPolyfill.GetValues<T>()
                           .Where(_ =>  input.HasFlag(_) &&
                                        _.CompareTo(Convert.ChangeType(Enum.ToObject(type, 0), type)) != 0)
                           .Select(_ => _.Humanize())
                           .Humanize();
            }

            var caseName = input.ToString();
            var member = type.GetField(caseName)!;

            if (TryGetDescription(member, out var description))
            {
                return description;
            }

            return caseName.Humanize();
        }

        /// <summary>
        /// Checks whether the given enum is to be used as a bit field type.
        /// </summary>
        /// <returns>True if the given enum is a bit field enum, false otherwise.</returns>
        static bool IsBitFieldEnum(Type type) =>
            type.GetCustomAttribute(typeof(FlagsAttribute)) != null;

        static bool TryGetDescription(MemberInfo member, out string description)
        {
            var displayAttribute = member.GetCustomAttribute<DisplayAttribute>();
            if (displayAttribute != null)
            {
                description = displayAttribute.GetDescription() ?? displayAttribute.GetName();

                return true;
            }

            foreach (var attr in member.GetCustomAttributes())
            {
                var attrType = attr.GetType();
                var descriptionProperty =
                    attrType.GetRuntimeProperties()
                        .Where(p => p.PropertyType == typeof(string))
                        .FirstOrDefault(Configurator.EnumDescriptionPropertyLocator);
                if (descriptionProperty != null)
                {
                    description = descriptionProperty.GetValue(attr, null)!.ToString();
                    return true;
                }
            }

            description = null;
            return false;
        }

        /// <summary>
        /// Turns an enum member into a human readable string with the provided casing; e.g. AnonymousUser with Title casing -> Anonymous User. It also honors DescriptionAttribute data annotation
        /// </summary>
        /// <param name="input">The enum member to be humanized</param>
        /// <param name="casing">The casing to use for humanizing the enum member</param>
        public static string Humanize<T>(this T input, LetterCasing casing)
            where T : struct, Enum
        {
            var humanizedEnum = Humanize(input);

            return humanizedEnum.ApplyCase(casing);
        }
    }
}

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
        public static string Humanize(this Enum input)
        {
            var type = input.GetType();

            if (IsBitFieldEnum(type) && !Enum.IsDefined(type, input))
            {
                return Enum.GetValues(type)
                           .Cast<Enum>()
                           .Where(e => e.CompareTo(Convert.ChangeType(Enum.ToObject(type, 0), type)) != 0)
                           .Where(input.HasFlag)
                           .Select(e => e.Humanize())
                           .Humanize();
            }

            var caseName = input.ToString();
            var member = type.GetTypeInfo().GetDeclaredField(caseName);

            if (member != null)
            {
                if (TryGetDescription(member, out var s))
                {
                    return s;
                }
            }

            return caseName.Humanize();
        }


        /// <summary>
        /// Checks whether the given enum is to be used as a bit field type.
        /// </summary>
        /// <returns>True if the given enum is a bit field enum, false otherwise.</returns>
        static bool IsBitFieldEnum(Type type) =>
            type.GetCustomAttribute(typeof(FlagsAttribute)) != null;

        static bool TryGetDescription(MemberInfo memberInfo, out string description)
        {
            var displayAttribute = memberInfo.GetCustomAttribute<DisplayAttribute>();
            if (displayAttribute != null)
            {
                description = displayAttribute.GetDescription() ??
                              displayAttribute.GetName();
                return true;
            }

            foreach (var attribute in memberInfo.GetCustomAttributes())
            {
                var descriptionProperty =
                    attribute.GetType().GetRuntimeProperties()
                        .Where(p => p.PropertyType == typeof(string))
                        .FirstOrDefault(Configurator.EnumDescriptionPropertyLocator);
                if (descriptionProperty != null)
                {
                    description = descriptionProperty.GetValue(attribute, null)!.ToString();
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
        public static string Humanize(this Enum input, LetterCasing casing)
        {
            var humanizedEnum = Humanize(input);

            return humanizedEnum.ApplyCase(casing);
        }
    }
}

using System;
using System.Linq;
using System.Reflection;
using Humanizer.Configuration;

namespace Humanizer
{
    /// <summary>
    /// Contains extension methods for humanizing Enums
    /// </summary>
    public static class EnumHumanizeExtensions
    {
        private const string DisplayAttributeTypeName = "System.ComponentModel.DataAnnotations.DisplayAttribute";
        private const string DisplayAttributeGetDescriptionMethodName = "GetDescription";

        private static readonly Func<PropertyInfo, bool> StringTypedProperty = p => p.PropertyType == typeof(string);

        /// <summary>
        /// Turns an enum member into a human readable string; e.g. AnonymousUser -> Anonymous user. It also honors DescriptionAttribute data annotation
        /// </summary>
        /// <param name="input">The enum member to be humanized</param>
        /// <returns></returns>
        public static string Humanize(this Enum input)
        {
            Type type = input.GetType();
            var caseName = input.ToString();
            var memInfo = type.GetMember(caseName);

            if (memInfo.Length > 0)
            {
                var customDescription = GetCustomDescription(memInfo[0]);

                if (customDescription != null)
                    return customDescription;
            }

            return caseName.Humanize();
        }

        // I had to add this method because PCL doesn't have DescriptionAttribute & I didn't want two versions of the code & thus the reflection
        private static string GetCustomDescription(MemberInfo memberInfo)
        {
            var attrs = memberInfo.GetCustomAttributes(true);

            foreach (var attr in attrs)
            {
                var attrType = attr.GetType();
                if (attrType.FullName == DisplayAttributeTypeName)
                {
                    var method = attrType.GetMethod(DisplayAttributeGetDescriptionMethodName);
                    if (method != null)
                        return method.Invoke(attr, new object[0]).ToString();
                }
                var descriptionProperty =
                    attrType.GetProperties()
                        .Where(StringTypedProperty)
                        .FirstOrDefault(Configurator.EnumDescriptionPropertyLocator);
                if (descriptionProperty != null)
                    return descriptionProperty.GetValue(attr, null).ToString();
            }

            return null;
        }

        /// <summary>
        /// Turns an enum member into a human readable string with the provided casing; e.g. AnonymousUser with Title casing -> Anonymous User. It also honors DescriptionAttribute data annotation
        /// </summary>
        /// <param name="input">The enum member to be humanized</param>
        /// <param name="casing">The casing to use for humanizing the enum member</param>
        /// <returns></returns>
        public static string Humanize(this Enum input, LetterCasing casing)
        {
            var humanizedEnum = Humanize(input);

            return humanizedEnum.ApplyCase(casing);
        }
   }
}
using System;
using System.Linq;
using System.Reflection;
using Humanizer.Configuration;
using System.Collections.Generic;

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
            var type = input.GetType();

            if (IsBitFieldEnum(input) && !DirectlyMapsToEnumConstant(input))
            {
                var inputIntValue = Convert.ToInt32(input);

                var humanizedEnumValues = new List<string>();

                foreach (var enumValue in Enum.GetValues(type))
                {
                    var enumIntValue = (int)enumValue;

                    if ((inputIntValue & enumIntValue) != 0)
                    {
                        humanizedEnumValues.Add(EnumHumanizeExtensions.Humanize((Enum)enumValue));
                    }
                }

                return String.Join(", ", humanizedEnumValues);
            }

            var caseName = input.ToString();
            var memInfo = type.GetTypeInfo().GetDeclaredField(caseName);

            if (memInfo != null)
            {
                var customDescription = GetCustomDescription(memInfo);

                if (customDescription != null)
                    return customDescription;
            }

            return caseName.Humanize();
        }

        /// <summary>
        /// Checks whether the given enum is to be used as a bit field type.
        /// </summary>
        /// <param name="input">An enum</param>
        /// <returns>True if the given enum is a bit field enum, false otherwise.</returns>
        private static bool IsBitFieldEnum(Enum input)
        {
            var type = input.GetType();

            var hasFlagsAttribute = type.GetTypeInfo().GetCustomAttribute(typeof(FlagsAttribute)) != null;
            var underlyingTypeIsInt = Enum.GetUnderlyingType(type) == typeof(int);

            return hasFlagsAttribute && underlyingTypeIsInt;
        }

        /// <summary>
        /// Returns true if the given enum instance can map exactly to one of the enum's constants.
        /// This doesn't always happen when using BitField enums.
        /// </summary>
        /// <param name="input">An instance of an enum.</param>
        /// <returns>True, if the instance maps directly to a single enum property, false if otherwise.</returns>
        private static bool DirectlyMapsToEnumConstant(Enum input)
        {
            bool exactMatch = false;

            foreach (var raw in Enum.GetValues(input.GetType()))
            {
                if (input.Equals(raw))
                {
                    exactMatch = true;
                    break;
                }
            }

            return exactMatch;
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
                    var method = attrType.GetRuntimeMethod(DisplayAttributeGetDescriptionMethodName, new Type[0]);
                    if (method != null)
                        return method.Invoke(attr, new object[0]).ToString();
                }
                var descriptionProperty =
                    attrType.GetRuntimeProperties()
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
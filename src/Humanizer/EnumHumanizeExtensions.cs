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
        private const string DisplayAttributeGetNameMethodName = "GetName";

        private static readonly Func<PropertyInfo, bool> StringTypedProperty = p => p.PropertyType == typeof(string);

        /// <summary>
        /// Turns an enum member into a human readable string; e.g. AnonymousUser -> Anonymous user. It also honors DescriptionAttribute data annotation
        /// </summary>
        /// <param name="input">The enum member to be humanized</param>
        /// <returns></returns>
        public static string Humanize(this Enum input)
        {
            var enumType = input.GetType();
            var enumTypeInfo = enumType.GetTypeInfo();

            if (IsBitFieldEnum(enumTypeInfo) && !Enum.IsDefined(enumType, input))
            {
                return Enum.GetValues(enumType)
                           .Cast<Enum>()
                           .Where(e => e.CompareTo(Convert.ChangeType(Enum.ToObject(enumType, 0), enumType)) != 0)
                           .Where(input.HasFlag)
                           .Select(e => e.Humanize())
                           .Humanize();
            }

            var caseName = input.ToString();
            var memInfo = enumTypeInfo.GetDeclaredField(caseName);

            if (memInfo != null)
            {
                var customDescription = GetCustomDescription(memInfo);

                if (customDescription != null)
                {
                    return customDescription;
                }
            }

            return caseName.Humanize();
        }

        /// <summary>
        /// Checks whether the given enum is to be used as a bit field type.
        /// </summary>
        /// <param name="typeInfo"></param>
        /// <returns>True if the given enum is a bit field enum, false otherwise.</returns>
        private static bool IsBitFieldEnum(TypeInfo typeInfo)
        {
            return typeInfo.GetCustomAttribute(typeof(FlagsAttribute)) != null;
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
                    var methodGetDescription = attrType.GetRuntimeMethod(DisplayAttributeGetDescriptionMethodName, new Type[0]);
                    if (methodGetDescription != null)
                    {
                        var executedMethod = methodGetDescription.Invoke(attr, new object[0]);
                        if (executedMethod != null)
                        {
                            return executedMethod.ToString();
                        }
                    }
                    var methodGetName = attrType.GetRuntimeMethod(DisplayAttributeGetNameMethodName, new Type[0]);
                    if (methodGetName != null)
                    {
                        var executedMethod = methodGetName.Invoke(attr, new object[0]);
                        if (executedMethod != null)
                        {
                            return executedMethod.ToString();
                        }
                    }
                    return null;
                }

                var descriptionProperty =
                    attrType.GetRuntimeProperties()
                        .Where(StringTypedProperty)
                        .FirstOrDefault(Configurator.EnumDescriptionPropertyLocator);
                if (descriptionProperty != null)
                {
                    return descriptionProperty.GetValue(attr, null).ToString();
                }
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

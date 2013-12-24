using System;
using System.Linq;
using System.Reflection;

namespace Humanizer
{
    public static class EnumHumanizeExtensions
    {
        private static readonly Func<PropertyInfo, bool> DescriptionProperty = p => p.Name == "Description" && p.PropertyType == typeof (string);

        /// <summary>
        /// Turns an enum member into a human readable string; e.g. AnonymousUser -> Anonymous user. It also honors DescriptionAttribute data annotation
        /// </summary>
        /// <param name="input">The enum member to be humanized</param>
        /// <returns></returns>
        public static string Humanize(this Enum input)
        {
<<<<<<< HEAD
            //TODO: RWM: The DescriptionAttribute is not available in PCLs.

            //Type type = input.GetType();
            //MemberInfo[] memInfo = type.GetMember(input.ToString());

            //if (memInfo.Length > 0)
            //{
            //    object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), true);

            //    if (attrs.Length > 0)
            //    {
            //        return ((DescriptionAttribute)attrs[0]).Description;
            //    }
            //}
=======
            Type type = input.GetType();
            var memInfo = type.GetMember(input.ToString());

            if (memInfo.Length > 0)
            {
                var customDescription = GetCustomDescription(memInfo[0]);

                if (customDescription != null)
                    return customDescription;
            }

            return input.ToString().Humanize();
        }

        // I had to add this method because PCL doesn't have DescriptionAttribute & I didn't want two versions of the code & thus the reflection
        private static string GetCustomDescription(MemberInfo memberInfo)
        {
            var attrs = memberInfo.GetCustomAttributes(true);

            foreach (var attr in attrs)
            {
                var attrType = attr.GetType();
                if (attrType.FullName == "System.ComponentModel.DescriptionAttribute")
                {
                    var descriptionProperty = attrType.GetProperties().FirstOrDefault(DescriptionProperty);
                    if (descriptionProperty != null)
                    {
                        //we have a hit
                        return descriptionProperty.GetValue(attr, null).ToString();
                    }
                }
            }
>>>>>>> 0d285f39a5d6d56c869e3ea01743f81197b1415f

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
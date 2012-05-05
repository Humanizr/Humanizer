using System;
using System.ComponentModel;
using System.Reflection;

namespace Humanize.Extensions
{
    public static class EnumExtensions
    {
         public static string Humanize(this Enum input)
         {
             Type type = input.GetType();
             MemberInfo[] memInfo = type.GetMember(input.ToString());

             if (memInfo.Length > 0)
             {
                 object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                 if (attrs.Length > 0)
                 {
                     return ((DescriptionAttribute)attrs[0]).Description;
                 }
             }

             return input.ToString().Humanize();
         }
    }
}
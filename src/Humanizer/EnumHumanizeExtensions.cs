using System;
using System.ComponentModel;
using System.Reflection;

namespace Humanizer
{
    public static class EnumHumanizeExtensions
    {
         public static string Humanize(this Enum input)
         {
             Type type = input.GetType();
             MemberInfo[] memInfo = type.GetMember(input.ToString());

             if (memInfo.Length > 0)
             {
                 object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), true);

                 if (attrs.Length > 0)
                 {
                     return ((DescriptionAttribute)attrs[0]).Description;
                 }
             }

             return input.ToString().Humanize();
         }

         public static string Humanize(this Enum input, LetterCasing casing)
         {
             var humanizedEnum = Humanize(input);

             return humanizedEnum.ApplyCase(casing);
         }
   }
}
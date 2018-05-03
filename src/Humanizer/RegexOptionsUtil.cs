using System;
using System.Text.RegularExpressions;

namespace Humanizer
{
    internal static class RegexOptionsUtil
    {
        static RegexOptionsUtil()
        {
            Compiled = Enum.TryParse("Compiled", out RegexOptions compiled) ? compiled : RegexOptions.None;
        }

        public static RegexOptions Compiled { get; }
    }
}

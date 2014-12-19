using System;
using System.Text.RegularExpressions;

namespace Humanizer
{
    internal static class RegexOptionsUtil
    {
        private static readonly RegexOptions _compiled;

        static RegexOptionsUtil()
        {
            RegexOptions compiled;
            _compiled = Enum.TryParse("Compiled", out compiled) ? compiled : RegexOptions.None;
        }

        public static RegexOptions Compiled
        {
            get { return _compiled; }
        }
    }
}
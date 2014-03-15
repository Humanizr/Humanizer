using System;

namespace Humanizer
{
    /// <summary>
    /// Extension methods for String type.
    /// </summary>
    public static class StringExentions
    {
        /// <summary>
        /// Extension method to format string with passed arguments
        /// </summary>
        /// <param name="format">string format</param>
        /// <param name="args">arguments</param>
        /// <returns></returns>
        public static string FormatWith(this string format, params object[] args)
        {
            return String.Format(format, args);
        }
    }
}

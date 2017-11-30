using System;
using JetBrains.Annotations;

namespace Humanizer
{
    /// <summary>
    /// Extension methods for String type.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Extension method to format string with passed arguments. Current thread's current culture is used
        /// </summary>
        /// <param name="format">string format</param>
        /// <param name="args">arguments</param>
        /// <returns></returns>
        [NotNull]
        [PublicAPI]
        public static string FormatWith([NotNull] this string format, [NotNull] params object[] args)
        {
            return string.Format(format, args);
        }

        /// <summary>
        /// Extension method to format string with passed arguments using specified format provider (i.e. CultureInfo)
        /// </summary>
        /// <param name="format">string format</param>
        /// <param name="provider">An object that supplies culture-specific formatting information</param>
        /// <param name="args">arguments</param>
        /// <returns></returns>
        [NotNull]
        [PublicAPI]
        public static string FormatWith([NotNull] this string format, [NotNull] IFormatProvider provider, [NotNull] params object[] args)
        {
            return string.Format(provider, format, args);
        }
    }
}

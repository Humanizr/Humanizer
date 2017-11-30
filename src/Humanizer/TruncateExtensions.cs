using System;
using JetBrains.Annotations;

namespace Humanizer
{
    /// <summary>
    /// Allow strings to be truncated
    /// </summary>
    public static class TruncateExtensions
    {
        /// <summary>
        /// Truncate the string
        /// </summary>
        /// <param name="input">The string to be truncated</param>
        /// <param name="length">The length to truncate to</param>
        /// <returns>The truncated string</returns>
        [CanBeNull]
        [PublicAPI]
        public static string Truncate([NotNull] this string input, int length)
        {
            return input.Truncate(length, "…", Truncator.FixedLength);
        }

        /// <summary>
        /// Truncate the string
        /// </summary>
        /// <param name="input">The string to be truncated</param>
        /// <param name="length">The length to truncate to</param>
        /// <param name="truncator">The truncate to use</param>
        /// <param name="from">The enum value used to determine from where to truncate the string</param>
        /// <returns>The truncated string</returns>
        [CanBeNull]
        [PublicAPI]
        public static string Truncate([NotNull] this string input, int length, [NotNull] ITruncator truncator, TruncateFrom from = TruncateFrom.Right)
        {
            return input.Truncate(length, "…", truncator, from);
        }

        /// <summary>
        /// Truncate the string
        /// </summary>
        /// <param name="input">The string to be truncated</param>
        /// <param name="length">The length to truncate to</param>
        /// <param name="truncationString">The string used to truncate with</param>
        /// <param name="from">The enum value used to determine from where to truncate the string</param>
        /// <returns>The truncated string</returns>
        [CanBeNull]
        [PublicAPI]
        public static string Truncate([NotNull] this string input, int length, [NotNull] string truncationString, TruncateFrom from = TruncateFrom.Right)
        {
            return input.Truncate(length, truncationString, Truncator.FixedLength, from);
        }

        /// <summary>
        /// Truncate the string
        /// </summary>
        /// <param name="input">The string to be truncated</param>
        /// <param name="length">The length to truncate to</param>
        /// <param name="truncationString">The string used to truncate with</param>
        /// <param name="truncator">The truncator to use</param>
        /// <param name="from">The enum value used to determine from where to truncate the string</param>
        /// <returns>The truncated string</returns>
        [CanBeNull]
        [PublicAPI]
        [ContractAnnotation("truncator:null => halt; input:null => null")]
        public static string Truncate([CanBeNull] this string input, int length, [NotNull] string truncationString, [NotNull] ITruncator truncator, TruncateFrom from = TruncateFrom.Right)
        {
            if (truncator == null)
                throw new ArgumentNullException(nameof(truncator));

            if (input == null)
                return null;

            return truncator.Truncate(input, length, truncationString, from);
        }
    }

    /// <summary>
    /// Truncation location for humanizer
    /// </summary>
    public enum TruncateFrom
    {
        /// <summary>
        /// Truncate letters from the left (start) of the string
        /// </summary>
        Left,
        /// <summary>
        /// Truncate letters from the right (end) of the string
        /// </summary>
        Right
    }
}

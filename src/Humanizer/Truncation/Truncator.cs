using System;

namespace Humanizer
{
    public static class Truncator
    {
        /// <summary>
        /// Truncate the string
        /// </summary>
        /// <param name="input">The string to be truncated</param>
        /// <param name="length">The length to truncate to</param>
        /// <returns>The truncated string</returns>
        public static string Truncate(this string input, int length)
        {
            return input.Truncate(length, "…", FixedLength);
        }

        /// <summary>
        /// Truncate the string
        /// </summary>
        /// <param name="input">The string to be truncated</param>
        /// <param name="length">The length to truncate to</param>
        /// <param name="truncator">The truncate to use</param>
        /// <returns>The truncated string</returns>
        public static string Truncate(this string input, int length, ITruncator truncator)
        {
            return input.Truncate(length, "…", truncator);
        }

        /// <summary>
        /// Truncate the string
        /// </summary>
        /// <param name="input">The string to be truncated</param>
        /// <param name="length">The length to truncate to</param>
        /// <param name="truncatationString">The string used to truncate with</param>
        /// <returns>The truncated string</returns>
        public static string Truncate(this string input, int length, string truncatationString)
        {
            return input.Truncate(length, truncatationString, FixedLength);
        }

        /// <summary>
        /// Truncate the string
        /// </summary>
        /// <param name="input">The string to be truncated</param>
        /// <param name="length">The length to truncate to</param>
        /// <param name="truncatationString">The string used to truncate with</param>
        /// <param name="truncator">The truncator to use</param>
        /// <returns>The truncated string</returns>
        public static string Truncate(this string input, int length, string truncatationString, ITruncator truncator)
        {
            if (truncator == null)
                throw new ArgumentNullException("truncator");

            if (input == null) 
                return null;

            return truncator.Truncate(input, length, truncatationString);
        }

        /// <summary>
        /// Fixed length truncator
        /// </summary>
        public static ITruncator FixedLength 
        {
            get
            {
                return new FixedLengthTruncator();
            } 
        }

        /// <summary>
        /// Fixed number of characters truncator
        /// </summary>
        public static ITruncator FixedNumberOfCharacters
        {
            get
            {
                return new FixedNumberOfCharactersTruncator();
            }
        }

        /// <summary>
        /// Fixed number of words truncator
        /// </summary>
        public static ITruncator FixedNumberOfWords
        {
            get
            {
                return new FixedNumberOfWordsTruncator();
            }
        }
    }
}

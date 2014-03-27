namespace Humanizer
{
    using System;
    using System.Linq;

    /// <summary>
    /// Truncate method to allow truncate a string easily
    /// </summary>
    public static class TruncateExtensions
    {
        /// <summary>
        /// Changes the casing of the provided input
        /// </summary>
        /// <param name="input">The string to be truncated</param>
        /// <param name="length">The length to truncate to</param>
        /// <param name="mode">The truncate mode</param>
        /// <returns>The truncated string</returns>
        public static string Truncate(this string input, int length, TruncateMode mode)
        {
            return input.Truncate(length, "…", mode);
        }

        /// <summary>
        /// Changes the casing of the provided input
        /// </summary>
        /// <param name="input">The string to be truncated</param>
        /// <param name="length">The length to truncate to</param>
        /// <param name="truncatationString">The string used to truncate with</param>
        /// <param name="mode">The truncate mode</param>
        /// <returns>The truncated string</returns>
        public static string Truncate(this string input, int length, string truncatationString, TruncateMode mode)
        {
            if (input == null)
                return null;

            if (input.Length == 0)
                return input;

            switch (mode)
            {
                case TruncateMode.FixedLength:
                    if (truncatationString == null || truncatationString.Length > length) 
                        return input.Substring(0, length);
                    
                    return input.Length > length ? input.Substring(0, length - truncatationString.Length) + truncatationString : input;

                case TruncateMode.FixedNumberOfCharacters:
                    if (truncatationString == null || truncatationString.Length > length)
                        return input.Substring(0, length);

                    var alphaNumericalCharactersProcessed = 0;

                    var numberOfCharactersEqualToTruncateLength = input.ToCharArray().Count(Char.IsLetterOrDigit) == length;

                    for (var i = 0; i < input.Length - truncatationString.Length; i++)
                    {
                        if (Char.IsLetterOrDigit(input[i]))
                            ++alphaNumericalCharactersProcessed;

                        if (numberOfCharactersEqualToTruncateLength && alphaNumericalCharactersProcessed == length) 
                            return input;

                        if (!numberOfCharactersEqualToTruncateLength && alphaNumericalCharactersProcessed + truncatationString.Length == length)
                            return input.Substring(0, i + 1) + truncatationString;
                    }

                    return input;

               case TruncateMode.FixedNumberOfWords:
                    var numberOfWordsProcessed = 0;
                    var numberOfWords = input.Split((char[])null, StringSplitOptions.RemoveEmptyEntries).Count();

                    if (numberOfWords <= length) 
                        return input;

                    var lastCharactersWasWhiteSpace = true;

                    for (var i = 0; i < input.Length; i++)
                    {
                        if (Char.IsWhiteSpace(input[i]))
                        {
                            if (!lastCharactersWasWhiteSpace)
                                ++numberOfWordsProcessed;

                            lastCharactersWasWhiteSpace = true;

                            if (numberOfWordsProcessed == length) 
                                return input.Substring(0, i) + truncatationString;
                        }
                        else
                        { 
                            lastCharactersWasWhiteSpace = false;
                        }
                    }

                    return input + truncatationString;

                default:
                    throw new ArgumentOutOfRangeException("mode");
            }
        }
    }
}
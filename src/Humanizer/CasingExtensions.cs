using System;
using System.Globalization;

namespace Humanizer
{
    /// <summary>
    /// ApplyCase method to allow changing the case of a sentence easily
    /// </summary>
    public static class CasingExtensions
    {
        /// <summary>
        /// Changes the casing of the provided input
        /// </summary>
        /// <param name="input"></param>
        /// <param name="casing"></param>
        /// <returns></returns>
        public static string ApplyCase(this string input, LetterCasing casing)
        {
            switch (casing)
            {
                case LetterCasing.Title:
                    //TODO: RWM: Fix this in Portable Class Libraries
                    //return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input);
                    System.Diagnostics.Debug.WriteLine("TitleCase is not supported in Portable Class Libraries.");
                    return input;

                case LetterCasing.LowerCase:
                    return CultureInfo.CurrentCulture.TextInfo.ToLower(input);

                case LetterCasing.AllCaps:
                    return input.ToUpper();

                case LetterCasing.Sentence:
                    if (input.Length >= 1)
                        return String.Concat(input.Substring(0, 1).ToUpper(), input.Substring(1));

                    return input.ToUpper();

                default:
                    throw new ArgumentOutOfRangeException("casing");
            }
        }

        
    }
}
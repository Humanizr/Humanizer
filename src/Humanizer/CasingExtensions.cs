using System;
using System.Globalization;
using System.Text.RegularExpressions;

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
                    return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input);

                case LetterCasing.LowerCase:
                    return input.ToLower();

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

        /// <summary>
        /// Humanizes the input with Title casing
        /// </summary>
        /// <param name="input">The string to be titleized</param>
        /// <returns></returns>
        public static string Titleize(this string input)
        {
            return input.Humanize(LetterCasing.Title);
        }

        /// <summary>
        /// By default, pascalize converts strings to UpperCamelCase also removing underscores
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Pascalize(this string input)
        {
            return Regex.Replace(input, "(?:^|_)(.)", match => match.Groups[1].Value.ToUpper());
        }

        /// <summary>
        /// Same as Pascalize except that the first character is lower case
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Camelize(this string input)
        {
            string word = Pascalize(input);
            return word.Substring(0, 1).ToLower() + word.Substring(1);
        }

        /// <summary>
        /// Separates the input words with underscore
        /// </summary>
        /// <param name="input">The string to be underscored</param>
        /// <returns></returns>
        public static string Underscore(this string input)
        {
            return Regex.Replace(
                Regex.Replace(
                    Regex.Replace(input, @"([A-Z]+)([A-Z][a-z])", "$1_$2"), @"([a-z\d])([A-Z])", "$1_$2"), @"[-\s]", "_").ToLower();
        }

        /// <summary>
        /// Replaces underscores with dashes in the string
        /// </summary>
        /// <param name="underscoredWord"></param>
        /// <returns></returns>
        public static string Dasherize(this string underscoredWord)
        {
            return underscoredWord.Replace('_', '-');
        }
    }
}
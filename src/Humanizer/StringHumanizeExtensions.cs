using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Humanizer
{
    public static class StringHumanizeExtensions
    {
        static readonly Func<string, string> FromUnderscoreDashSeparatedWords = methodName => string.Join(" ", methodName.Split(new[] { '_', '-' }));

        static readonly Regex PascalCaseWordBoundaryRegex = new Regex(@"
(?# word to word, number or acronym)
(?<=[a-z])(?=[A-Z0-9])|
(?# number to word or acronym)
(?<=[0-9])(?=[A-Za-z])|
(?# acronym to number)
(?<=[A-Z])(?=[0-9])|
(?# acronym to word)
(?<=[A-Z])(?=[A-Z][a-z])
", RegexOptions.IgnorePatternWhitespace|RegexOptions.Compiled);

        static string FromPascalCase(string name)
        {
            var result = PascalCaseWordBoundaryRegex
                .Split(name)
                .Select(word =>
                    word.All(Char.IsUpper) && word.Length > 1
                        ? word
                        : word.ToLower())
                .Aggregate((res, word) => res + " " + word);

            result = Char.ToUpper(result[0]) +
                result.Substring(1, result.Length - 1);
            return result.Replace(" i ", " I "); // I is an exception
        }

        /// <summary>
        /// Humanizes the input string; e.g. Underscored_input_String_is_turned_INTO_sentence -> 'Underscored input String is turned INTO sentence'
        /// </summary>
        /// <param name="input">The string to be humanized</param>
        /// <returns></returns>
        public static string Humanize(this string input)
        {
            // if input is all capitals (e.g. an acronym) then return it without change
            if (input.All(Char.IsUpper))
                return input;

            if (input.Contains('_') || input.Contains('-'))
                return FromUnderscoreDashSeparatedWords(input);

            return FromPascalCase(input);
        }

        /// <summary>
        /// Humanized the input string based on the provided casing
        /// </summary>
        /// <param name="input">The string to be humanized</param>
        /// <param name="casing">The desired casing for the output</param>
        /// <returns></returns>
        public static string Humanize(this string input, LetterCasing casing)
        {
            var humanizedString = input.Humanize();

            return ApplyCase(humanizedString, casing);
        }

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
                        return string.Concat(input.Substring(0, 1).ToUpper(), input.Substring(1));

                    return input.ToUpper();

                default:
                    throw new ArgumentOutOfRangeException("casing");
            }
        }
    }
}

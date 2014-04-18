using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Humanizer
{
    /// <summary>
    /// Contains extension methods for humanizing string values.
    /// </summary>
    public static class StringHumanizeExtensions
    {
        static string FromUnderscoreDashSeparatedWords (string input)
        {
            return String.Join(" ", input.Split(new[] {'_', '-'}));
        }

        static string FromPascalCase(string input)
        {
            var pascalCaseWordBoundaryRegex = new Regex(@"
(?# word to word, number or acronym)
(?<=[a-z])(?=[A-Z0-9])|
(?# number to word or acronym)
(?<=[0-9])(?=[A-Za-z])|
(?# acronym to number)
(?<=[A-Z])(?=[0-9])|
(?# acronym to word)
(?<=[A-Z])(?=[A-Z][a-z])
", RegexOptions.IgnorePatternWhitespace);

            var result = pascalCaseWordBoundaryRegex
                .Split(input)
                .Select(word =>
                    word.ToCharArray().All(Char.IsUpper) && word.Length > 1
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
            if (input.ToCharArray().All(Char.IsUpper))
                return input;

            if (input.Contains("_") || input.Contains("-"))
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
            return input.Humanize().ApplyCase(casing);
        }
    }
}

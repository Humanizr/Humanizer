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
        private static readonly Regex PascalCaseWordPartsRegex;
        private static readonly Regex FreestandingSpacingCharRegex;

        static StringHumanizeExtensions()
        {
            PascalCaseWordPartsRegex = new Regex(@"[A-Z]?[a-z]+|[0-9]+|[A-Z]+(?=[A-Z][a-z]|[0-9]|\b)",
                RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture | RegexOptionsUtil.Compiled);
            FreestandingSpacingCharRegex = new Regex(@"\s[-_]|[-_]\s", RegexOptionsUtil.Compiled);
        }

        static string FromUnderscoreDashSeparatedWords (string input)
        {
            return String.Join(" ", input.Split(new[] {'_', '-'}));
        }

        static string FromPascalCase(string input)
        {
            if (input.Length == 1)
                return Char.ToUpper(input[0]).ToString();

            var result = PascalCaseWordPartsRegex
                .Matches(input).Cast<Match>()
                .Select(match => match.Value.ToCharArray().All(Char.IsUpper) &&
                    (match.Value.Length > 1 || (match.Index > 0 && input[match.Index - 1] == ' ') || match.Value == "I")
                    ? match.Value
                    : match.Value.ToLower())
                .Aggregate((res, word) => res + " " + word);

            result = Char.ToUpper(result[0]) +
                result.Substring(1, result.Length - 1);
            return result;
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

            // if input contains a dash or underscore which preceeds or follows a space (or both, i.g. free-standing)
            // remove the dash/underscore and run it through FromPascalCase
            if (FreestandingSpacingCharRegex.IsMatch(input))
                return FromPascalCase(FromUnderscoreDashSeparatedWords(input));

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

namespace Humanizer
{
    /// <summary>
    /// Contains extension methods for humanizing string values.
    /// </summary>
    public static class StringHumanizeExtensions
    {
        private static readonly Regex PascalCaseWordPartsRegex;
        private static readonly Regex FreestandingSpacingCharRegex;

        private const string OptionallyCapitalizedWord = @"\p{Lu}?\p{Ll}+";
        private const string IntegerAndOptionalLowercaseLetters = @"[0-9]+\p{Ll}*";
        private const string Acronym = @"\p{Lu}+(?=\p{Lu}|[0-9]|\b)";
        private const string SequenceOfOtherLetters = @"\p{Lo}+";
        private const string MidSentencePunctuation = "[,;]?";

        static StringHumanizeExtensions()
        {
            PascalCaseWordPartsRegex = new Regex(
                $"({OptionallyCapitalizedWord}|{IntegerAndOptionalLowercaseLetters}|{Acronym}|{SequenceOfOtherLetters}){MidSentencePunctuation}",
                RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture | RegexOptionsUtil.Compiled);
            FreestandingSpacingCharRegex = new Regex(@"\s[-_]|[-_]\s", RegexOptionsUtil.Compiled);
        }

        private static string FromUnderscoreDashSeparatedWords(string input) =>
            string.Join(" ", input.Split(['_', '-']));

        private static string FromPascalCase(string input)
        {
            var result = string.Join(" ", PascalCaseWordPartsRegex
                .Matches(input).Cast<Match>()
                .Select(match =>
                {
                    var value = match.Value;
                    return value.All(char.IsUpper) &&
                           (value.Length > 1 || (match.Index > 0 && input[match.Index - 1] == ' ') || value == "I")
                        ? value
                        : value.ToLower();
                }));

            if (result.Replace(" ", "").All(char.IsUpper) &&
                result.Contains(" "))
            {
                result = result.ToLower();
            }

            return result.Length > 0 ? char.ToUpper(result[0]) +
                result.Substring(1, result.Length - 1) : result;
        }

        /// <summary>
        /// Humanizes the input string; e.g. Underscored_input_String_is_turned_INTO_sentence -> 'Underscored input String is turned INTO sentence'
        /// </summary>
        /// <param name="input">The string to be humanized</param>
        public static string Humanize(this string input)
        {
            // if input is all capitals (e.g. an acronym) then return it without change
            if (input.All(char.IsUpper))
            {
                return input;
            }

            // if input contains a dash or underscore which precedes or follows a space (or both, e.g. free-standing)
            // remove the dash/underscore and run it through FromPascalCase
            if (FreestandingSpacingCharRegex.IsMatch(input))
            {
                return FromPascalCase(FromUnderscoreDashSeparatedWords(input));
            }

            if (input.Contains("_") || input.Contains("-"))
            {
                return FromUnderscoreDashSeparatedWords(input);
            }

            return FromPascalCase(input);
        }

        /// <summary>
        /// Humanized the input string based on the provided casing
        /// </summary>
        /// <param name="input">The string to be humanized</param>
        /// <param name="casing">The desired casing for the output</param>
        public static string Humanize(this string input, LetterCasing casing) =>
            input.Humanize().ApplyCase(casing);
    }
}

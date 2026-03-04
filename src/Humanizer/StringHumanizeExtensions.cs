using System.Runtime.InteropServices;

namespace Humanizer;

/// <summary>
/// Contains extension methods for humanizing string values.
/// </summary>
public static partial class StringHumanizeExtensions
{
    const string PascalCaseWordPartsPattern = @"(\p{Lu}?\p{Ll}+|[0-9]+\p{Ll}*|\p{Lu}+(?=\p{Lu}|[0-9]|\b)|\p{Lo}+)[,;]?";
    const string FreestandingSpacingCharPattern = @"\s[-_]|[-_]\s";

#if NET7_0_OR_GREATER
    [GeneratedRegex(PascalCaseWordPartsPattern, RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture)]
    private static partial Regex PascalCaseWordPartsRegexGenerated();
    
    private static Regex PascalCaseWordPartsRegex() => PascalCaseWordPartsRegexGenerated();

    [GeneratedRegex(FreestandingSpacingCharPattern)]
    private static partial Regex FreestandingSpacingCharRegexGenerated();
    
    private static Regex FreestandingSpacingCharRegex() => FreestandingSpacingCharRegexGenerated();
#else
    private static readonly Regex PascalCaseWordPartsRegexField = new(
        PascalCaseWordPartsPattern,
        RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture | RegexOptions.Compiled);

    private static Regex PascalCaseWordPartsRegex() => PascalCaseWordPartsRegexField;

    private static readonly Regex FreestandingSpacingCharRegexField = new(FreestandingSpacingCharPattern, RegexOptions.Compiled);

    private static Regex FreestandingSpacingCharRegex() => FreestandingSpacingCharRegexField;
#endif

    static string FromUnderscoreDashSeparatedWords(string input)
    {
#if NET7_0_OR_GREATER
        return string.Create(input.Length, input, (span, state) =>
        {
            state.AsSpan().CopyTo(span);
            span.Replace('_', ' ');
            span.Replace('-', ' ');
        });
#else
        return input.Replace('_', ' ').Replace('-', ' ');
#endif
    }

    static string FromPascalCase(string input)
    {
        var result = string.Join(" ", PascalCaseWordPartsRegex()
            .Matches(input)
            // ReSharper disable once RedundantEnumerableCastCall
            .Cast<Match>()
            .Select(match =>
            {
                var value = match.Value;
                return value.All(char.IsUpper) &&
                       (value.Length > 1 || (match.Index > 0 && input[match.Index - 1] == ' ') || value == "I")
                    ? value
                    : value.ToLower();
            }));

        if (result.All(c => c == ' ' || char.IsUpper(c)) && result.Contains(' '))
        {
            result = result.ToLower();
        }

        return result.Length > 0
            ? Concat(char.ToUpper(result[0]), result.AsSpan(1, result.Length - 1))
            : result;
    }

    /// <summary>
    /// Transforms a string into a human-readable format by intelligently handling PascalCase, camelCase,
    /// underscored_strings, and dash-separated-strings, converting them into space-separated text with
    /// appropriate capitalization.
    /// </summary>
    /// <param name="input">The string to be humanized. Must not be null.</param>
    /// <returns>
    /// A humanized version of the input string with spaces inserted between words and appropriate
    /// capitalization. Preserves all-uppercase acronyms unchanged.
    /// </returns>
    /// <remarks>
    /// The method applies several rules in order:
    /// - If the entire input is uppercase (an acronym), it returns unchanged
    /// - Handles freestanding underscores/dashes (e.g., "some _ string")
    /// - Splits on underscores and dashes
    /// - Breaks up PascalCase and camelCase text
    /// The first letter of the result is always capitalized.
    /// </remarks>
    /// <example>
    /// <code>
    /// "PascalCaseInputString".Humanize() => "Pascal case input string"
    /// "Underscored_input_String_is_turned_INTO_sentence".Humanize() => "Underscored input String is turned INTO sentence"
    /// "dash-separated-string".Humanize() => "Dash separated string"
    /// "HTML".Humanize() => "HTML"
    /// "camelCaseText".Humanize() => "Camel case text"
    /// </code>
    /// </example>
    public static string Humanize(this string input)
    {
        // if input is all capitals (e.g. an acronym) then return it without change
        if (input.All(char.IsUpper))
        {
            return input;
        }

        // if input contains a dash or underscore which precedes or follows a space (or both, e.g. freestanding)
        // remove the dash/underscore and run it through FromPascalCase
        if (FreestandingSpacingCharRegex().IsMatch(input))
        {
            return FromPascalCase(FromUnderscoreDashSeparatedWords(input));
        }

        if (input.IndexOfAny(['_', '-']) >= 0)
        {
            return FromUnderscoreDashSeparatedWords(input);
        }

        return FromPascalCase(input);
    }

    /// <summary>
    /// Transforms a string into a human-readable format and applies the specified letter casing.
    /// </summary>
    /// <param name="input">The string to be humanized. Must not be null.</param>
    /// <param name="casing">The desired letter casing to apply to the humanized result.</param>
    /// <returns>
    /// A humanized version of the input string with the specified casing applied.
    /// </returns>
    /// <remarks>
    /// This is a convenience method that combines <see cref="Humanize(string)"/> with <see cref="CasingExtensions.ApplyCase"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// "PascalCaseInputString".Humanize(LetterCasing.AllCaps) => "PASCAL CASE INPUT STRING"
    /// "PascalCaseInputString".Humanize(LetterCasing.LowerCase) => "pascal case input string"
    /// "PascalCaseInputString".Humanize(LetterCasing.Title) => "Pascal Case Input String"
    /// </code>
    /// </example>
    public static string Humanize(this string input, LetterCasing casing) =>
        input
            .Humanize()
            .ApplyCase(casing);

#if NET
    internal static string Concat(CharSpan left, CharSpan right) =>
        string.Concat(left, right);

    internal static string Concat(char left, CharSpan right) =>
        string.Concat(MemoryMarshal.CreateReadOnlySpan(ref left, 1), right);

    internal static string Concat(CharSpan left, char right) =>
        string.Concat(left, MemoryMarshal.CreateReadOnlySpan(ref right, 1));
#else
    internal static unsafe string Concat(CharSpan left, CharSpan right)
    {
        var result = new string('\0', left.Length + right.Length);
        fixed (char* pResult = result)
        {
            left.CopyTo(new(pResult, left.Length));
            right.CopyTo(new(pResult + left.Length, right.Length));
        }
        return result;
    }

    internal static unsafe string Concat(char left, CharSpan right) =>
        Concat(new CharSpan(&left, 1), right);

    internal static unsafe string Concat(CharSpan left, char right) =>
        Concat(left, new CharSpan(&right, 1));
#endif
}

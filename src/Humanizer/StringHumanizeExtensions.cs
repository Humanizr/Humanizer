using System.Runtime.InteropServices;

namespace Humanizer;

/// <summary>
/// Contains extension methods for humanizing string values.
/// </summary>
public static class StringHumanizeExtensions
{
    static readonly Regex PascalCaseWordPartsRegex = new(
        $"({OptionallyCapitalizedWord}|{IntegerAndOptionalLowercaseLetters}|{Acronym}|{SequenceOfOtherLetters}){MidSentencePunctuation}",
        RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture | RegexOptions.Compiled);

    static readonly Regex FreestandingSpacingCharRegex =
        new(@"\s[-_]|[-_]\s", RegexOptions.Compiled);

    const string OptionallyCapitalizedWord = @"\p{Lu}?\p{Ll}+";
    const string IntegerAndOptionalLowercaseLetters = @"[0-9]+\p{Ll}*";
    const string Acronym = @"\p{Lu}+(?=\p{Lu}|[0-9]|\b)";
    const string SequenceOfOtherLetters = @"\p{Lo}+";
    const string MidSentencePunctuation = "[,;]?";

    static string FromUnderscoreDashSeparatedWords(string input) =>
        string.Join(" ", input.Split(['_', '-']));

    static string FromPascalCase(string input)
    {
        var result = string.Join(" ", PascalCaseWordPartsRegex
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

        if (result
                .Replace(" ", "")
                .All(char.IsUpper) &&
            result.Contains(" "))
        {
            result = result.ToLower();
        }

        return result.Length > 0
            ? Concat(char.ToUpper(result[0]), result.AsSpan(1, result.Length - 1))
            : result;
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

        // if input contains a dash or underscore which precedes or follows a space (or both, e.g. freestanding)
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
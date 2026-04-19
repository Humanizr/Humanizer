#if NET
using System.Runtime.InteropServices;
#endif

namespace Humanizer;

/// <summary>
/// Contains extension methods for humanizing string values.
/// </summary>
public static partial class StringHumanizeExtensions
{
    const string PascalCaseWordPartsPattern = @"(\p{Lu}?\p{Ll}+|[0-9]+\p{Ll}*|\p{Lu}+(?=\p{Lu}|[0-9]|\b)|\p{Lo}+)[,;]?";

#if NET7_0_OR_GREATER
    [GeneratedRegex(PascalCaseWordPartsPattern, RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture)]
    private static partial Regex PascalCaseWordPartsRegexGenerated();

    private static Regex PascalCaseWordPartsRegex() => PascalCaseWordPartsRegexGenerated();

#else
    private static readonly Regex PascalCaseWordPartsRegexField = new(
        PascalCaseWordPartsPattern,
        RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture | RegexOptions.Compiled);

    private static Regex PascalCaseWordPartsRegex() => PascalCaseWordPartsRegexField;
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

    static string FromPascalCaseRegex(string input)
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

    static string FromPascalCase(string input) =>
        TryFromAsciiPascalCase(input, out var result)
            ? result
            : FromPascalCaseRegex(input);

    static bool TryFromAsciiPascalCase(string input, [NotNullWhen(true)] out string? result)
    {
        result = null;
        if (input.Length == 0)
        {
            result = "";
            return true;
        }

        for (var i = 0; i < input.Length; i++)
        {
            if (input[i] > '\u007F' || (!IsAsciiLetterOrDigit(input[i]) && input[i] != ' '))
            {
                return false;
            }
        }

        var textInfo = CultureInfo.CurrentCulture.TextInfo;
        var buffer = new char[Math.Max(1, input.Length * 2)];
        var pos = 0;
        var wordCount = 0;
        var containsSpace = false;
        var allUpperOrSpace = true;

        for (var i = 0; i < input.Length;)
        {
            if (input[i] == ' ')
            {
                i++;
                continue;
            }

            var start = i;
            var length = ReadAsciiWord(input, ref i);
            if (length == 0)
            {
                return false;
            }

            if (wordCount > 0)
            {
                buffer[pos++] = ' ';
                containsSpace = true;
            }

            var preserveUpper = IsAllUpper(input, start, length) &&
                (length > 1 || (start > 0 && input[start - 1] == ' ') || input.AsSpan(start, length).SequenceEqual("I"));

            for (var j = 0; j < length; j++)
            {
                var c = input[start + j];
                var value = preserveUpper ? c : textInfo.ToLower(c);
                buffer[pos++] = value;
                if (value != ' ' && !char.IsUpper(value))
                {
                    allUpperOrSpace = false;
                }
            }

            wordCount++;
        }

        if (pos == 0)
        {
            result = "";
            return true;
        }

        if (allUpperOrSpace && containsSpace)
        {
            for (var i = 0; i < pos; i++)
            {
                buffer[i] = textInfo.ToLower(buffer[i]);
            }
        }

        buffer[0] = textInfo.ToUpper(buffer[0]);
        result = new(buffer, 0, pos);
        return true;
    }

    static int ReadAsciiWord(string input, ref int index)
    {
        var start = index;
        var c = input[index];
        if (IsAsciiDigit(c))
        {
            index++;
            while (index < input.Length && IsAsciiDigit(input[index]))
            {
                index++;
            }

            while (index < input.Length && IsAsciiLower(input[index]))
            {
                index++;
            }

            return index - start;
        }

        if (IsAsciiLower(c))
        {
            index++;
            while (index < input.Length && IsAsciiLower(input[index]))
            {
                index++;
            }

            return index - start;
        }

        if (!IsAsciiUpper(c))
        {
            return 0;
        }

        var upperStart = index;
        index++;
        while (index < input.Length && IsAsciiUpper(input[index]))
        {
            index++;
        }

        if (index < input.Length && IsAsciiLower(input[index]))
        {
            if (index - upperStart > 1)
            {
                index--;
                return index - start;
            }

            index++;
            while (index < input.Length && IsAsciiLower(input[index]))
            {
                index++;
            }
        }

        return index - start;
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
        if (IsAllUpper(input, 0, input.Length))
        {
            return input;
        }

        // if input contains a dash or underscore which precedes or follows a space (or both, e.g. freestanding)
        // remove the dash/underscore and run it through FromPascalCase
        if (HasFreestandingSpacingChar(input))
        {
            return FromPascalCase(FromUnderscoreDashSeparatedWords(input));
        }

        if (input.IndexOfAny(['_', '-']) >= 0)
        {
            return FromUnderscoreDashSeparatedWords(input);
        }

        return FromPascalCase(input);
    }

    static bool HasFreestandingSpacingChar(string input)
    {
        for (var i = 0; i < input.Length; i++)
        {
            if ((input[i] == '-' || input[i] == '_') &&
                ((i > 0 && char.IsWhiteSpace(input[i - 1])) ||
                 (i + 1 < input.Length && char.IsWhiteSpace(input[i + 1]))))
            {
                return true;
            }
        }

        return false;
    }

    static bool IsAllUpper(string input, int index, int length)
    {
        var end = index + length;
        for (var i = index; i < end; i++)
        {
            if (!char.IsUpper(input[i]))
            {
                return false;
            }
        }

        return true;
    }

    static bool IsAsciiLetterOrDigit(char c) =>
        IsAsciiUpper(c) || IsAsciiLower(c) || IsAsciiDigit(c);

    static bool IsAsciiUpper(char c) =>
        c is >= 'A' and <= 'Z';

    static bool IsAsciiLower(char c) =>
        c is >= 'a' and <= 'z';

    static bool IsAsciiDigit(char c) =>
        c is >= '0' and <= '9';

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
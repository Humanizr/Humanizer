namespace Humanizer;

/// <summary>
/// ApplyCase method to allow changing the case of a sentence easily
/// </summary>
public static class CasingExtensions
{
    /// <summary>
    /// Applies the specified letter casing transformation to the input string.
    /// </summary>
    /// <param name="input">The string to transform. Must not be null.</param>
    /// <param name="casing">The desired letter casing style to apply to the input string.</param>
    /// <returns>
    /// A new string with the specified casing applied.
    /// - <see cref="LetterCasing.Title"/>: Each word is capitalized (e.g., "Some String")
    /// - <see cref="LetterCasing.LowerCase"/>: All letters are lowercase (e.g., "some string")
    /// - <see cref="LetterCasing.AllCaps"/>: All letters are uppercase (e.g., "SOME STRING")
    /// - <see cref="LetterCasing.Sentence"/>: First letter capitalized, rest lowercase (e.g., "Some string")
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when an invalid <see cref="LetterCasing"/> value is provided.</exception>
    /// <example>
    /// <code>
    /// "some string".ApplyCase(LetterCasing.Title) => "Some String"
    /// "SOME STRING".ApplyCase(LetterCasing.LowerCase) => "some string"
    /// "some string".ApplyCase(LetterCasing.AllCaps) => "SOME STRING"
    /// "some string".ApplyCase(LetterCasing.Sentence) => "Some string"
    /// </code>
    /// </example>
    public static string ApplyCase(this string input, LetterCasing casing) =>
        casing switch
        {
            LetterCasing.Title => input.Transform(To.TitleCase),
            LetterCasing.LowerCase => input.Transform(To.LowerCase),
            LetterCasing.AllCaps => input.Transform(To.UpperCase),
            LetterCasing.Sentence => input.Transform(To.SentenceCase),
            _ => throw new ArgumentOutOfRangeException(nameof(casing))
        };
}
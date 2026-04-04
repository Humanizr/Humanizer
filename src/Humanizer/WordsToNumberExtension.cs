namespace Humanizer;

/// <summary>
/// Converts localized number words back into numeric values.
/// Parsing is culture-aware, honors locale inheritance, and supports the same natural high-range
/// forms that the locale authoring data exposes through <c>number.words</c> and
/// <c>number.parse</c>.
/// </summary>
public static class WordsToNumberExtension
{
    /// <summary>
    /// Converts a spelled-out number string to its numeric representation.
    /// </summary>
    /// <param name="words">
    /// The spelled-out number. Must not be <c>null</c>.
    /// </param>
    /// <param name="culture">
    /// The culture to use for parsing.
    /// </param>
    /// <returns>The numeric value represented by <paramref name="words"/>.</returns>
    /// <exception cref="FormatException">
    /// If the input contains unrecognized words or cannot be parsed as a number.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="words"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// This method now returns <see cref="long"/> to support locale-authored high-range number
    /// parsing beyond <see cref="int.MaxValue"/>. Existing code that depended on an <see cref="int"/>
    /// result should either switch the receiving type to <see cref="long"/> or use an explicit
    /// checked conversion.
    /// Use <see cref="TryToNumber(string, out long, CultureInfo)"/> when you want a non-throwing
    /// parse path and the first unrecognized token.
    /// </remarks>
    public static long ToNumber(this string words, CultureInfo culture) =>
        Configurator.GetWordsToNumberConverter(culture).Convert(words);

    /// <summary>
    /// Attempts to convert a spelled-out number string to its numeric representation without throwing.
    /// </summary>
    /// <param name="words">
    /// The spelled-out number. Must not be <c>null</c>.
    /// </param>
    /// <param name="parsedNumber">
    /// When this method returns, contains the parsed numeric value if successful; otherwise, 0.
    /// </param>
    /// <param name="culture">
    /// The culture to use for parsing.
    /// </param>
    /// <returns><c>true</c> if the conversion was successful; otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// This is the recommended method when the input may be invalid.
    /// </remarks>
    public static bool TryToNumber(this string words, out long parsedNumber, CultureInfo culture)
        => Configurator.GetWordsToNumberConverter(culture).TryConvert(words, out parsedNumber);

    /// <summary>
    /// Attempts to convert a spelled-out number string to its numeric representation and reports the
    /// first unrecognized word if the conversion fails.
    /// </summary>
    /// <param name="words">
    /// The spelled-out number. Must not be <c>null</c>.
    /// </param>
    /// <param name="parsedNumber">
    /// When this method returns, contains the parsed numeric value if successful; otherwise, 0.
    /// </param>
    /// <param name="culture">
    /// The culture to use for parsing.
    /// </param>
    /// <param name="unrecognizedWord">
    /// When this method returns <c>false</c>, contains the first unrecognized word found in the input.
    /// When this method returns <c>true</c>, this parameter is set to <c>null</c>.
    /// </param>
    /// <returns><c>true</c> if the conversion was successful; otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// This overload is useful for debugging or user-facing validation because it identifies the
    /// first token that could not be recognized.
    /// </remarks>
    public static bool TryToNumber(this string words, out long parsedNumber, CultureInfo culture, out string? unrecognizedWord)
        => Configurator.GetWordsToNumberConverter(culture).TryConvert(words, out parsedNumber, out unrecognizedWord);

}

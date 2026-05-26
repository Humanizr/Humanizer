namespace Humanizer;

/// <summary>
/// Converts localized decimal number words back into numeric values.
/// </summary>
public static class WordsToDecimalNumberExtension
{
    /// <summary>
    /// Converts a spelled-out decimal number string to its numeric representation.
    /// </summary>
    /// <param name="words">The spelled-out decimal number.</param>
    /// <param name="culture">The culture to use for parsing.</param>
    /// <returns>The decimal value represented by <paramref name="words"/>.</returns>
    public static decimal ToDecimalNumber(this string words, CultureInfo culture) =>
        Configurator.GetWordsToDecimalNumberConverter(culture).Convert(words);

    /// <summary>
    /// Attempts to convert a spelled-out decimal number string to its numeric representation.
    /// </summary>
    /// <param name="words">The spelled-out decimal number.</param>
    /// <param name="parsedNumber">When this method returns, contains the parsed value if successful.</param>
    /// <param name="culture">The culture to use for parsing.</param>
    /// <returns><c>true</c> if the conversion was successful; otherwise, <c>false</c>.</returns>
    public static bool TryToDecimalNumber(this string words, out decimal parsedNumber, CultureInfo culture) =>
        Configurator.GetWordsToDecimalNumberConverter(culture).TryConvert(words, out parsedNumber);

    /// <summary>
    /// Attempts to convert a spelled-out decimal number string and reports the first unrecognized word.
    /// </summary>
    /// <param name="words">The spelled-out decimal number.</param>
    /// <param name="parsedNumber">When this method returns, contains the parsed value if successful.</param>
    /// <param name="culture">The culture to use for parsing.</param>
    /// <param name="unrecognizedWord">When parsing fails, the first unrecognized token.</param>
    /// <returns><c>true</c> if the conversion was successful; otherwise, <c>false</c>.</returns>
    public static bool TryToDecimalNumber(
        this string words,
        out decimal parsedNumber,
        CultureInfo culture,
        out string? unrecognizedWord) =>
        Configurator.GetWordsToDecimalNumberConverter(culture).TryConvert(words, out parsedNumber, out unrecognizedWord);
}

namespace Humanizer;

/// <summary>
/// Converts English decimal number words into <see cref="decimal"/> values.
/// </summary>
public static class WordsToDecimalNumberExtension
{
    /// <summary>
    /// Converts English decimal number words containing one <c>point</c> marker to a decimal value.
    /// </summary>
    /// <param name="words">The decimal number words to convert.</param>
    /// <param name="culture">The English culture to use for parsing.</param>
    /// <returns>The parsed decimal value, including its authored fractional scale.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="words"/> is <c>null</c>.</exception>
    /// <exception cref="FormatException">If the phrase is malformed or outside the supported <see cref="decimal"/> range.</exception>
    /// <exception cref="NotSupportedException">If <paramref name="culture"/> is not an English culture.</exception>
    /// <remarks>
    /// The integer part uses the existing English words-to-number grammar. The fractional part
    /// requires one to 28 digit words (<c>zero</c> through <c>nine</c>); conjunctions are not
    /// accepted after <c>point</c>. A leading <c>minus</c> or <c>negative</c> applies to the
    /// complete value. The integer part may be omitted, as in <c>point five</c>.
    /// A decimal marker and at least one fractional digit word are required.
    /// </remarks>
    public static decimal ToDecimalNumber(this string words, CultureInfo culture)
    {
        ArgumentNullException.ThrowIfNull(words);
        return Configurator.GetWordsToDecimalNumberConverter(culture).Convert(words);
    }

    /// <summary>
    /// Attempts to convert English decimal number words without throwing for malformed input,
    /// unsupported cultures, or values outside the supported <see cref="decimal"/> range.
    /// </summary>
    /// <param name="words">The decimal number words to convert.</param>
    /// <param name="parsedNumber">When this method returns, contains the parsed value if successful; otherwise, zero.</param>
    /// <param name="culture">The English culture to use for parsing.</param>
    /// <returns><c>true</c> if conversion succeeded; otherwise, <c>false</c>.</returns>
    public static bool TryToDecimalNumber(this string words, out decimal parsedNumber, CultureInfo culture) =>
        Configurator.GetWordsToDecimalNumberConverter(culture).TryConvert(words, out parsedNumber);

    /// <summary>
    /// Attempts to convert English decimal number words and reports the first unrecognized token.
    /// </summary>
    /// <param name="words">The decimal number words to convert.</param>
    /// <param name="parsedNumber">When this method returns, contains the parsed value if successful; otherwise, zero.</param>
    /// <param name="culture">The English culture to use for parsing.</param>
    /// <param name="unrecognizedWord">When conversion fails, the best-effort unrecognized token or phrase.</param>
    /// <returns><c>true</c> if conversion succeeded; otherwise, <c>false</c>.</returns>
    public static bool TryToDecimalNumber(
        this string words,
        out decimal parsedNumber,
        CultureInfo culture,
        out string? unrecognizedWord) =>
        Configurator.GetWordsToDecimalNumberConverter(culture).TryConvert(words, out parsedNumber, out unrecognizedWord);
}
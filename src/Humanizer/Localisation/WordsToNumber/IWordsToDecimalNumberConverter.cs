namespace Humanizer;

/// <summary>
/// Converts localized decimal number words into numeric values.
/// </summary>
public interface IWordsToDecimalNumberConverter
{
    /// <summary>
    /// Attempts to convert <paramref name="words"/> into a decimal value.
    /// </summary>
    /// <param name="words">The localized decimal number phrase to convert.</param>
    /// <param name="parsedValue">When this method returns, contains the parsed decimal value.</param>
    /// <returns><c>true</c> if the phrase was parsed successfully; otherwise, <c>false</c>.</returns>
    bool TryConvert(string words, out decimal parsedValue);

    /// <summary>
    /// Attempts to convert <paramref name="words"/> into a decimal value and reports the first
    /// unrecognized token when parsing fails.
    /// </summary>
    /// <param name="words">The localized decimal number phrase to convert.</param>
    /// <param name="parsedValue">When this method returns, contains the parsed decimal value.</param>
    /// <param name="unrecognizedNumber">When parsing fails, the first unrecognized token.</param>
    /// <returns><c>true</c> if the phrase was parsed successfully; otherwise, <c>false</c>.</returns>
    bool TryConvert(string words, out decimal parsedValue, out string? unrecognizedNumber);

    /// <summary>
    /// Converts <paramref name="words"/> into a decimal value.
    /// </summary>
    /// <param name="words">The localized decimal number phrase to convert.</param>
    /// <returns>The parsed decimal value.</returns>
    decimal Convert(string words);
}

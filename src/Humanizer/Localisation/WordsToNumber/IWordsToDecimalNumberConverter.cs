namespace Humanizer;

/// <summary>
/// Converts localized decimal number words into <see cref="decimal"/> values.
/// </summary>
public interface IWordsToDecimalNumberConverter
{
    /// <summary>
    /// Converts <paramref name="words"/> into a decimal value.
    /// </summary>
    /// <param name="words">The localized decimal number phrase to convert.</param>
    /// <returns>The parsed decimal value.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="words"/> is <c>null</c>.</exception>
    /// <exception cref="FormatException">If <paramref name="words"/> is malformed or outside the supported <see cref="decimal"/> range.</exception>
    /// <exception cref="NotSupportedException">If decimal word parsing is not supported for the converter's locale.</exception>
    decimal Convert(string words);

    /// <summary>
    /// Attempts to convert <paramref name="words"/> into a decimal value.
    /// </summary>
    /// <param name="words">The localized decimal number phrase to convert.</param>
    /// <param name="parsedValue">When this method returns, contains the parsed value if successful; otherwise, zero.</param>
    /// <returns><c>true</c> if the phrase was parsed successfully; otherwise, <c>false</c>.</returns>
    bool TryConvert(string words, out decimal parsedValue);

    /// <summary>
    /// Attempts to convert <paramref name="words"/> into a decimal value and reports the first
    /// unrecognized token when parsing fails.
    /// </summary>
    /// <param name="words">The localized decimal number phrase to convert.</param>
    /// <param name="parsedValue">When this method returns, contains the parsed value if successful; otherwise, zero.</param>
    /// <param name="unrecognizedNumber">When parsing fails, the best-effort unrecognized token or phrase.</param>
    /// <returns><c>true</c> if the phrase was parsed successfully; otherwise, <c>false</c>.</returns>
    bool TryConvert(string words, out decimal parsedValue, out string? unrecognizedNumber);
}
namespace Humanizer;

/// <summary>
/// Converts localized number words into numeric values.
/// </summary>
/// <remarks>
/// Implementations expect a meaningful localized number phrase. They may throw
/// <see cref="ArgumentException"/> for <c>null</c>, empty, or whitespace input, and fallback
/// implementations for unsupported locales may throw <see cref="NotSupportedException"/> instead
/// of returning a parse failure.
/// </remarks>
public interface IWordsToNumberConverter
{
    /// <summary>
    /// Attempts to convert <paramref name="words"/> into a numeric value.
    /// </summary>
    /// <param name="words">The localized number phrase to convert.</param>
    /// <param name="parsedValue">When this method returns, contains the parsed numeric value. The value is meaningful only when the method returns <c>true</c>.</param>
    /// <returns><c>true</c> if the phrase was parsed successfully; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentException">If <paramref name="words"/> is <c>null</c>, empty, or whitespace.</exception>
    /// <exception cref="NotSupportedException">If the current implementation does not support words-to-number conversion for its locale.</exception>
    bool TryConvert(string words, out long parsedValue);

    /// <summary>
    /// Attempts to convert <paramref name="words"/> into a numeric value and reports the first
    /// token-like fragment that could not be interpreted when parsing fails.
    /// </summary>
    /// <param name="words">The localized number phrase to convert.</param>
    /// <param name="parsedValue">When this method returns, contains the parsed numeric value. The value is meaningful only when the method returns <c>true</c>.</param>
    /// <param name="unrecognizedNumber">When parsing fails, the best-effort token or fragment that remained unrecognized.</param>
    /// <returns><c>true</c> if the value was parsed successfully; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentException">If <paramref name="words"/> is <c>null</c>, empty, or whitespace.</exception>
    /// <exception cref="NotSupportedException">If the current implementation does not support words-to-number conversion for its locale.</exception>
    bool TryConvert(string words, out long parsedValue, out string? unrecognizedNumber);

    /// <summary>
    /// Converts <paramref name="words"/> into a numeric value.
    /// </summary>
    /// <param name="words">The localized number phrase to convert.</param>
    /// <returns>The parsed numeric value.</returns>
    /// <exception cref="ArgumentException">
    /// If <paramref name="words"/> is <c>null</c>, empty, whitespace, or cannot be parsed by the current implementation.
    /// </exception>
    /// <exception cref="NotSupportedException">If the current implementation does not support words-to-number conversion for its locale.</exception>
    long Convert(string words);
}

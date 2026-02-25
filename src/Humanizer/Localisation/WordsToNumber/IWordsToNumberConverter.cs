namespace Humanizer;

/// <summary>
/// Implement this interface to support locale-specific conversion of written words to numeric values.
/// Register a custom implementation via <see cref="Configurator.WordsToNumberConverters"/>.
/// </summary>
public interface IWordsToNumberConverter
{
    /// <summary>
    /// Attempts to convert the given words to an integer value.
    /// </summary>
    /// <param name="words">The words to convert, e.g. <c>"forty-two"</c>.</param>
    /// <param name="parsedValue">When this method returns <see langword="true"/>, contains the parsed integer.</param>
    /// <returns><see langword="true"/> if conversion succeeded; otherwise <see langword="false"/>.</returns>
    bool TryConvert(string words, out int parsedValue);

    /// <summary>
    /// Attempts to convert the given words to an integer value, reporting any unrecognized portion of the input.
    /// </summary>
    /// <param name="words">The words to convert, e.g. <c>"forty-two"</c>.</param>
    /// <param name="parsedValue">When this method returns <see langword="true"/>, contains the parsed integer.</param>
    /// <param name="unrecognizedNumber">
    /// When conversion fails, contains the portion of <paramref name="words"/> that could not be recognized;
    /// otherwise <see langword="null"/>.
    /// </param>
    /// <returns><see langword="true"/> if conversion succeeded; otherwise <see langword="false"/>.</returns>
    bool TryConvert(string words, out int parsedValue, out string? unrecognizedNumber);

    /// <summary>
    /// Converts the given words to an integer value.
    /// </summary>
    /// <param name="words">The words to convert, e.g. <c>"forty-two"</c>.</param>
    /// <returns>The integer represented by <paramref name="words"/>.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="words"/> cannot be converted to a number.</exception>
    int Convert(string words);
}

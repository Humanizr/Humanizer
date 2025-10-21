namespace Humanizer;

/// <summary>
/// Transform humanized string to number; e.g. one => 1
/// </summary>
public static class WordsToNumberExtension
{
    /// <summary>
    /// Converts a spelled-out number string to its integer representation.
    /// </summary>
    /// <param name="words">
    /// The spelled-out number (e.g., "three hundred twenty-one", "forty-two").
    /// Must not be null.
    /// </param>
    /// <param name="culture">
    /// The culture to use for parsing. Different cultures may have different word representations
    /// for numbers (e.g., "twenty" in English vs. "vingt" in French).
    /// </param>
    /// <returns>
    /// The integer value represented by the words.
    /// </returns>
    /// <exception cref="FormatException">
    /// Thrown when the input contains unrecognized words or cannot be parsed as a number.
    /// </exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="words"/> is null.</exception>
    /// <remarks>
    /// This method strictly parses the input and throws an exception if any word is not recognized.
    /// For a non-throwing version, use <see cref="TryToNumber(string, out int, CultureInfo)"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// // English (en-US)
    /// "three hundred twenty-one".ToNumber(new CultureInfo("en-US")) => 321
    /// "forty-two".ToNumber(new CultureInfo("en-US")) => 42
    /// "one thousand".ToNumber(new CultureInfo("en-US")) => 1000
    /// 
    /// // Invalid input throws exception
    /// "xyz".ToNumber(new CultureInfo("en-US")) => throws FormatException
    /// </code>
    /// </example>
    public static int ToNumber(this string words, CultureInfo culture)
        => Configurator.GetWordsToNumberConverter(culture).Convert(words);

    /// <summary>
    /// Attempts to convert a spelled-out number string to its integer representation without throwing exceptions.
    /// </summary>
    /// <param name="words">
    /// The spelled-out number (e.g., "forty-two", "one hundred").
    /// Must not be null.
    /// </param>
    /// <param name="parsedNumber">
    /// When this method returns, contains the integer value represented by the words if the conversion succeeded,
    /// or 0 if the conversion failed.
    /// </param>
    /// <param name="culture">
    /// The culture to use for parsing.
    /// </param>
    /// <returns>
    /// <c>true</c> if the conversion was successful; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// This is the recommended method for parsing when you're not sure if the input is valid.
    /// It does not throw exceptions on invalid input.
    /// </remarks>
    /// <example>
    /// <code>
    /// // Successful conversion
    /// "forty-two".TryToNumber(out int result, new CultureInfo("en-US")) => returns true, result = 42
    /// 
    /// // Failed conversion
    /// "xyz".TryToNumber(out int result, new CultureInfo("en-US")) => returns false, result = 0
    /// </code>
    /// </example>
    public static bool TryToNumber(this string words, out int parsedNumber, CultureInfo culture)
        => Configurator.GetWordsToNumberConverter(culture).TryConvert(words, out parsedNumber);

    /// <summary>
    /// Attempts to convert a spelled-out number string to its integer representation and provides
    /// the first unrecognized word if the conversion fails.
    /// </summary>
    /// <param name="words">
    /// The spelled-out number (e.g., "one thousand one").
    /// Must not be null.
    /// </param>
    /// <param name="parsedNumber">
    /// When this method returns, contains the integer value represented by the words if the conversion succeeded,
    /// or 0 if the conversion failed.
    /// </param>
    /// <param name="culture">
    /// The culture to use for parsing.
    /// </param>
    /// <param name="unrecognizedWord">
    /// When this method returns <c>false</c>, contains the first unrecognized word found in the input.
    /// When this method returns <c>true</c>, this parameter is set to <c>null</c>.
    /// </param>
    /// <returns>
    /// <c>true</c> if the conversion was successful; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// This overload is useful for debugging or providing detailed error messages to users,
    /// as it identifies which specific word caused the parsing failure.
    /// </remarks>
    /// <example>
    /// <code>
    /// // Successful conversion
    /// "one thousand".TryToNumber(out int result, new CultureInfo("en-US"), out string? badWord) 
    ///   => returns true, result = 1000, badWord = null
    /// 
    /// // Failed conversion with unrecognized word
    /// "one xyz three".TryToNumber(out int result, new CultureInfo("en-US"), out string? badWord)
    ///   => returns false, result = 0, badWord = "xyz"
    /// </code>
    /// </example>
    public static bool TryToNumber(this string words, out int parsedNumber, CultureInfo culture, out string? unrecognizedWord)
        => Configurator.GetWordsToNumberConverter(culture).TryConvert(words, out parsedNumber, out unrecognizedWord);

}

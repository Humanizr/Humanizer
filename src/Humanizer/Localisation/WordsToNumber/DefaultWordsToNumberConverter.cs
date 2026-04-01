namespace Humanizer;

/// <summary>
/// Provides the fallback implementation for cultures that do not support words-to-number parsing.
/// </summary>
internal class DefaultWordsToNumberConverter(CultureInfo culture) : GenderlessWordsToNumberConverter
{
    readonly CultureInfo cultureInfo = culture;

    /// <inheritdoc />
    public override int Convert(string words)
    {
        TryConvert(words, out var parsedValue);

        return parsedValue;
    }

    /// <inheritdoc />
    public override bool TryConvert(string words, out int parsedValue) => TryConvert(words, out parsedValue, out _);

    /// <summary>
    /// Throws because this culture does not have a words-to-number implementation.
    /// </summary>
    /// <param name="words">The localized number phrase to convert.</param>
    /// <param name="parsedValue">When this method returns, contains the parsed integer value.</param>
    /// <param name="unrecognizedWord">When this method returns, contains <c>null</c>.</param>
    /// <returns>Never returns successfully.</returns>
    /// <exception cref="NotSupportedException">
    /// Thrown because the culture does not have a words-to-number implementation.
    /// </exception>
    public override bool TryConvert(string words, out int parsedValue, out string? unrecognizedWord)
    {
        throw new NotSupportedException($"Words-to-number conversion is not supported for '{cultureInfo.TwoLetterISOLanguageName}'.");
    }

}

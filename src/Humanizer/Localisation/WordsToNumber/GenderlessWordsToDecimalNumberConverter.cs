namespace Humanizer;

/// <summary>
/// Provides the common contract for words-to-decimal-number converters that do not vary by
/// grammatical gender.
/// </summary>
internal abstract class GenderlessWordsToDecimalNumberConverter : IWordsToDecimalNumberConverter
{
    /// <inheritdoc />
    public abstract decimal Convert(string words);

    /// <inheritdoc />
    public abstract bool TryConvert(string words, out decimal parsedValue);

    /// <inheritdoc />
    public abstract bool TryConvert(string words, out decimal parsedValue, out string? unrecognizedNumber);
}

namespace Humanizer;

/// <summary>
/// Provides the common contract for words-to-number converters that do not vary by grammatical
/// gender.
/// </summary>
internal abstract class GenderlessWordsToNumberConverter : IWordsToNumberConverter
{
    /// <inheritdoc />
    public abstract long Convert(string words);

    /// <inheritdoc />
    public abstract bool TryConvert(string words, out long parsedValue);

    /// <inheritdoc />
    public abstract bool TryConvert(string words, out long parsedValue, out string? unrecognizedNumber);
}

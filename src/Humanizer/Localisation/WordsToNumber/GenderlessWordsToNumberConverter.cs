namespace Humanizer;

/// <summary>
/// Provides the common contract for words-to-number converters that do not vary by grammatical
/// gender.
/// </summary>
internal abstract class GenderlessWordsToNumberConverter : IWordsToNumberConverter
{
    /// <inheritdoc />
    public abstract int Convert(string words);

    /// <inheritdoc />
    public abstract bool TryConvert(string words, out int parsedValue);

    /// <inheritdoc />
    public abstract bool TryConvert(string words, out int parsedValue, out string? unrecognizedNumber);
}

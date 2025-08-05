namespace Humanizer;

internal abstract class GenderlessWordsToNumberConverter : IWordsToNumberConverter
{
    public abstract int Convert(string words);
    public abstract bool TryConvert(string words, out int parsedValue);
    public abstract bool TryConvert(string words, out int parsedValue, out string? unrecognizedNumber);
}

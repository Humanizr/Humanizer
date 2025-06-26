namespace Humanizer;

public interface IWordsToNumberConverter
{
    bool TryConvert(string words, out int parsedValue);
    bool TryConvert(string words, out int parsedValue, out string? unrecognizedNumber);

    int Convert(string words);
}

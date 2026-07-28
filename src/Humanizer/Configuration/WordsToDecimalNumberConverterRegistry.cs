namespace Humanizer;

internal sealed class WordsToDecimalNumberConverterRegistry : LocaliserRegistry<IWordsToDecimalNumberConverter>
{
    public WordsToDecimalNumberConverterRegistry()
        : base(UnsupportedWordsToDecimalNumberConverter.Instance) =>
        Register("en", static culture => new EnglishWordsToDecimalNumberConverter(culture));
}
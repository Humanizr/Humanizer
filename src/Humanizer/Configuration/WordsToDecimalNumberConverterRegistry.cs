namespace Humanizer;

internal class WordsToDecimalNumberConverterRegistry : LocaliserRegistry<IWordsToDecimalNumberConverter>
{
    public WordsToDecimalNumberConverterRegistry()
        : base(_ => UnsupportedWordsToDecimalNumberConverter.Instance)
        => WordsToDecimalNumberConverterRegistryRegistrations.Register(this);
}

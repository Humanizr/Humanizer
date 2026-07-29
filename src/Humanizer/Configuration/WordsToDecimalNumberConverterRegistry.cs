namespace Humanizer;

internal sealed class WordsToDecimalNumberConverterRegistry : LocaliserRegistry<IWordsToDecimalNumberConverter>
{
    public WordsToDecimalNumberConverterRegistry()
        : base(UnsupportedWordsToDecimalNumberConverter.Instance) =>
        WordsToDecimalNumberConverterRegistryRegistrations.Register(this);
}
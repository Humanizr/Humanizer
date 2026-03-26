namespace Humanizer;

internal class WordsToNumberConverterRegistry : LocaliserRegistry<IWordsToNumberConverter>
{
    public WordsToNumberConverterRegistry()
        : base(culture => new DefaultWordsToNumberConverter(culture))
        => WordsToNumberConverterRegistryRegistrations.Register(this);
}

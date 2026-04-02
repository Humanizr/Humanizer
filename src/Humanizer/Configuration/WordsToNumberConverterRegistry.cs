namespace Humanizer;

internal class WordsToNumberConverterRegistry : LocaliserRegistry<IWordsToNumberConverter>
{
    public WordsToNumberConverterRegistry()
        : base(TokenMapWordsToNumberConverters.En)
        => WordsToNumberConverterRegistryRegistrations.Register(this);
}

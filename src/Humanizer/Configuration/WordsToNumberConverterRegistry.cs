namespace Humanizer;

internal class WordsToNumberConverterRegistry : LocaliserRegistry<IWordsToNumberConverter>
{
    static readonly IWordsToNumberConverter englishConverter = new EnglishWordsToNumberConverter();

    public WordsToNumberConverterRegistry()
        : base(culture => new DefaultWordsToNumberConverter(culture))
    {
        WordsToNumberConverterRegistryRegistrations.Register(this);
        Register("en", englishConverter);
        Register("en-US", englishConverter);
        Register("en-GB", englishConverter);
    }
}

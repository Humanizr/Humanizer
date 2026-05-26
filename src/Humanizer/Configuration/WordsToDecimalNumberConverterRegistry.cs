namespace Humanizer;

internal class WordsToDecimalNumberConverterRegistry : LocaliserRegistry<IWordsToDecimalNumberConverter>
{
    public WordsToDecimalNumberConverterRegistry()
        : base(_ => UnsupportedWordsToDecimalNumberConverter.Instance)
    {
        Register(
            "en",
            static _ => new TokenMapWordsToDecimalNumberConverter(
                (TokenMapWordsToNumberConverter)TokenMapWordsToNumberConverters.En));
    }
}

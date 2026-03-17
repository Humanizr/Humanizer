namespace Humanizer;

using System.Globalization;

internal class WordsToNumberConverterRegistry : LocaliserRegistry<IWordsToNumberConverter>
{
    public WordsToNumberConverterRegistry()
        : base(CreateConverter)
    {
        Register("en", _ => new EnglishWordsToNumberConverter());
        Register("ca", _ => new CatalanWordsToNumberConverter());
        Register("de", _ => new GermanWordsToNumberConverter());
        Register("de-CH", _ => new GermanWordsToNumberConverter());
        Register("de-LI", _ => new GermanWordsToNumberConverter());
    }

    private static IWordsToNumberConverter CreateConverter(CultureInfo culture) =>
        culture.TwoLetterISOLanguageName switch
        {
            "en" => new EnglishWordsToNumberConverter(),
            "ca" => new CatalanWordsToNumberConverter(),
            "de" => new GermanWordsToNumberConverter(),
            _ => new DefaultWordsToNumberConverter(culture)
        };
}

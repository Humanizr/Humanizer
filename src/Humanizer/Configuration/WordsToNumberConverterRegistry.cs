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
        Register("fr", _ => new FrenchWordsToNumberConverter());
        Register("pt", _ => new PortugueseWordsToNumberConverter());
        Register("pt-BR", _ => new PortugueseWordsToNumberConverter());
        Register("it", _ => new ItalianWordsToNumberConverter());
        Register("es", _ => new SpanishWordsToNumberConverter());
        Register("es-ES", _ => new SpanishWordsToNumberConverter());
        Register("nl", _ => new DutchWordsToNumberConverter());
        Register("sv", _ => new SwedishWordsToNumberConverter());
        Register("is", _ => new IcelandicWordsToNumberConverter());
        Register("nb", _ => new NorwegianBokmalWordsToNumberConverter());
        Register("nb-NO", _ => new NorwegianBokmalWordsToNumberConverter());
        Register("fil", _ => new FilipinoWordsToNumberConverter());
        Register("id", _ => new IndonesianWordsToNumberConverter());
        Register("ms", _ => new MalayWordsToNumberConverter());
        Register("zh-CN", _ => new ChineseWordsToNumberConverter());
        Register("zh-Hans", _ => new ChineseWordsToNumberConverter());
        Register("zh-Hant", _ => new ChineseWordsToNumberConverter());
        Register("uz-Latn-UZ", _ => new UzbekLatnWordsToNumberConverter());
        Register("uz-Cyrl-UZ", _ => new UzbekCyrlWordsToNumberConverter());
    }

    private static IWordsToNumberConverter CreateConverter(CultureInfo culture) =>
        culture.TwoLetterISOLanguageName switch
        {
            "en" => new EnglishWordsToNumberConverter(),
            "ca" => new CatalanWordsToNumberConverter(),
            "de" => new GermanWordsToNumberConverter(),
            "fr" => new FrenchWordsToNumberConverter(),
            "pt" => new PortugueseWordsToNumberConverter(),
            "it" => new ItalianWordsToNumberConverter(),
            "es" => new SpanishWordsToNumberConverter(),
            "nl" => new DutchWordsToNumberConverter(),
            "sv" => new SwedishWordsToNumberConverter(),
            "is" => new IcelandicWordsToNumberConverter(),
            "nb" => new NorwegianBokmalWordsToNumberConverter(),
            "fil" => new FilipinoWordsToNumberConverter(),
            "id" => new IndonesianWordsToNumberConverter(),
            "ms" => new MalayWordsToNumberConverter(),
            "zh" => new ChineseWordsToNumberConverter(),
            "uz" => culture.Name.Contains("Cyrl", StringComparison.Ordinal)
                ? new UzbekCyrlWordsToNumberConverter()
                : new UzbekLatnWordsToNumberConverter(),
            _ => new DefaultWordsToNumberConverter(culture)
        };
}

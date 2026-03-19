namespace Humanizer;

using System.Globalization;

internal class WordsToNumberConverterRegistry : LocaliserRegistry<IWordsToNumberConverter>
{
    public WordsToNumberConverterRegistry()
        : base(CreateConverter)
    {
        Register("en", _ => new EnglishWordsToNumberConverter());
        Register("af", _ => new AfrikaansWordsToNumberConverter());
        Register("bg", _ => new BulgarianWordsToNumberConverter());
        Register("ca", _ => new CatalanWordsToNumberConverter());
        Register("cs", _ => new CzechWordsToNumberConverter());
        Register("da", _ => new DanishWordsToNumberConverter());
        Register("de", _ => new GermanWordsToNumberConverter());
        Register("de-CH", _ => new GermanWordsToNumberConverter());
        Register("de-LI", _ => new GermanWordsToNumberConverter());
        Register("fi", _ => new FinnishWordsToNumberConverter());
        Register("fi-FI", _ => new FinnishWordsToNumberConverter());
        Register("fr", _ => new FrenchWordsToNumberConverter());
        Register("he", _ => new HebrewWordsToNumberConverter());
        Register("hr", _ => new CroatianWordsToNumberConverter());
        Register("hu", _ => new HungarianWordsToNumberConverter());
        Register("lt", _ => new LithuanianWordsToNumberConverter());
        Register("lv", _ => new LatvianWordsToNumberConverter());
        Register("mt", _ => new MalteseWordsToNumberConverter());
        Register("pl", _ => new PolishWordsToNumberConverter());
        Register("pt", _ => new PortugueseWordsToNumberConverter());
        Register("pt-BR", _ => new PortugueseWordsToNumberConverter());
        Register("it", _ => new ItalianWordsToNumberConverter());
        Register("ro", _ => new RomanianWordsToNumberConverter());
        Register("ro-RO", _ => new RomanianWordsToNumberConverter());
        Register("sl", _ => new SlovenianWordsToNumberConverter());
        Register("sr", _ => new SerbianCyrillicWordsToNumberConverter());
        Register("sr-Latn", _ => new SerbianLatinWordsToNumberConverter());
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
        Register("ru", _ => new RussianWordsToNumberConverter());
        Register("ru-RU", _ => new RussianWordsToNumberConverter());
        Register("sk", _ => new SlovakWordsToNumberConverter());
        Register("tr", _ => new TurkishWordsToNumberConverter());
        Register("uk", _ => new UkrainianWordsToNumberConverter());
        Register("uk-UA", _ => new UkrainianWordsToNumberConverter());
        Register("az", _ => new AzerbaijaniWordsToNumberConverter());
        Register("ja", _ => new JapaneseWordsToNumberConverter());
        Register("ko", _ => new KoreanWordsToNumberConverter());
        Register("vi", _ => new VietnameseWordsToNumberConverter());
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
            "af" => new AfrikaansWordsToNumberConverter(),
            "bg" => new BulgarianWordsToNumberConverter(),
            "ca" => new CatalanWordsToNumberConverter(),
            "cs" => new CzechWordsToNumberConverter(),
            "da" => new DanishWordsToNumberConverter(),
            "de" => new GermanWordsToNumberConverter(),
            "fi" => new FinnishWordsToNumberConverter(),
            "fr" => new FrenchWordsToNumberConverter(),
            "he" => new HebrewWordsToNumberConverter(),
            "hr" => new CroatianWordsToNumberConverter(),
            "hu" => new HungarianWordsToNumberConverter(),
            "lt" => new LithuanianWordsToNumberConverter(),
            "lv" => new LatvianWordsToNumberConverter(),
            "mt" => new MalteseWordsToNumberConverter(),
            "pl" => new PolishWordsToNumberConverter(),
            "pt" => new PortugueseWordsToNumberConverter(),
            "it" => new ItalianWordsToNumberConverter(),
            "ro" => new RomanianWordsToNumberConverter(),
            "sl" => new SlovenianWordsToNumberConverter(),
            "sr" => culture.Name.Contains("Latn", StringComparison.Ordinal)
                ? new SerbianLatinWordsToNumberConverter()
                : new SerbianCyrillicWordsToNumberConverter(),
            "es" => new SpanishWordsToNumberConverter(),
            "nl" => new DutchWordsToNumberConverter(),
            "sv" => new SwedishWordsToNumberConverter(),
            "is" => new IcelandicWordsToNumberConverter(),
            "nb" => new NorwegianBokmalWordsToNumberConverter(),
            "fil" => new FilipinoWordsToNumberConverter(),
            "id" => new IndonesianWordsToNumberConverter(),
            "ms" => new MalayWordsToNumberConverter(),
            "ru" => new RussianWordsToNumberConverter(),
            "sk" => new SlovakWordsToNumberConverter(),
            "tr" => new TurkishWordsToNumberConverter(),
            "uk" => new UkrainianWordsToNumberConverter(),
            "az" => new AzerbaijaniWordsToNumberConverter(),
            "ja" => new JapaneseWordsToNumberConverter(),
            "ko" => new KoreanWordsToNumberConverter(),
            "vi" => new VietnameseWordsToNumberConverter(),
            "zh" => new ChineseWordsToNumberConverter(),
            "uz" => culture.Name.Contains("Cyrl", StringComparison.Ordinal)
                ? new UzbekCyrlWordsToNumberConverter()
                : new UzbekLatnWordsToNumberConverter(),
            _ => new DefaultWordsToNumberConverter(culture)
        };
}

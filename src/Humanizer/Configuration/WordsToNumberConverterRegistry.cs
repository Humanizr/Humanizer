namespace Humanizer;

using System.Globalization;

internal class WordsToNumberConverterRegistry : LocaliserRegistry<IWordsToNumberConverter>
{
    public WordsToNumberConverterRegistry()
        : base(culture => new DefaultWordsToNumberConverter(culture))
    {
        RegisterShared(TokenMapWordsToNumberConverters.Afrikaans, "af");
        RegisterShared(TokenMapWordsToNumberConverters.Armenian, "hy");
        RegisterShared(TokenMapWordsToNumberConverters.Bulgarian, "bg");
        RegisterShared(TokenMapWordsToNumberConverters.Bengali, "bn");
        RegisterShared(TokenMapWordsToNumberConverters.Catalan, "ca");
        RegisterShared(TokenMapWordsToNumberConverters.Croatian, "hr");
        RegisterShared(TokenMapWordsToNumberConverters.Czech, "cs");
        RegisterShared(TokenMapWordsToNumberConverters.Greek, "el");
        RegisterShared(TokenMapWordsToNumberConverters.Latvian, "lv");
        RegisterShared(TokenMapWordsToNumberConverters.Lithuanian, "lt");
        RegisterShared(TokenMapWordsToNumberConverters.Polish, "pl");
        RegisterShared(TokenMapWordsToNumberConverters.Russian, "ru", "ru-RU");
        RegisterShared(TokenMapWordsToNumberConverters.SerbianCyrillic, "sr");
        RegisterShared(TokenMapWordsToNumberConverters.SerbianLatin, "sr-Latn");
        RegisterShared(TokenMapWordsToNumberConverters.Turkish, "tr");
        RegisterShared(TokenMapWordsToNumberConverters.Ukrainian, "uk", "uk-UA");
        RegisterShared(TokenMapWordsToNumberConverters.UzbekCyrl, "uz-Cyrl-UZ");
        RegisterShared(TokenMapWordsToNumberConverters.UzbekLatn, "uz-Latn-UZ");
        RegisterShared(TokenMapWordsToNumberConverters.Azerbaijani, "az");

        Register("en", _ => new EnglishWordsToNumberConverter());
        Register("ar", _ => new ArabicWordsToNumberConverter());
        Register("da", _ => new DanishWordsToNumberConverter());
        Register("de", _ => new GermanWordsToNumberConverter());
        Register("de-CH", _ => new GermanWordsToNumberConverter());
        Register("de-LI", _ => new GermanWordsToNumberConverter());
        Register("fi", _ => new FinnishWordsToNumberConverter());
        Register("fi-FI", _ => new FinnishWordsToNumberConverter());
        Register("fr", _ => new FrenchWordsToNumberConverter());
        Register("he", _ => new HebrewWordsToNumberConverter());
        Register("ku", _ => new KurdishWordsToNumberConverter());
        Register("lb", _ => new LuxembourgishWordsToNumberConverter());
        Register("hu", _ => new HungarianWordsToNumberConverter());
        Register("mt", _ => new MalteseWordsToNumberConverter());
        Register("pt", _ => new PortugueseWordsToNumberConverter());
        Register("pt-BR", _ => new PortugueseWordsToNumberConverter());
        Register("it", _ => new ItalianWordsToNumberConverter());
        Register("ro", _ => new RomanianWordsToNumberConverter());
        Register("ro-RO", _ => new RomanianWordsToNumberConverter());
        Register("sl", _ => new SlovenianWordsToNumberConverter());
        Register("es", _ => new SpanishWordsToNumberConverter());
        Register("es-ES", _ => new SpanishWordsToNumberConverter());
        Register("fa", _ => new PersianWordsToNumberConverter());
        Register("nl", _ => new DutchWordsToNumberConverter());
        Register("sv", _ => new SwedishWordsToNumberConverter());
        Register("th", _ => new ThaiWordsToNumberConverter());
        Register("th-TH", _ => new ThaiWordsToNumberConverter());
        Register("is", _ => new IcelandicWordsToNumberConverter());
        Register("nb", _ => new NorwegianBokmalWordsToNumberConverter());
        Register("nb-NO", _ => new NorwegianBokmalWordsToNumberConverter());
        Register("fil", _ => new FilipinoWordsToNumberConverter());
        Register("id", _ => new IndonesianWordsToNumberConverter());
        Register("ms", _ => new MalayWordsToNumberConverter());
        Register("sk", _ => new SlovakWordsToNumberConverter());
        Register("ja", _ => new JapaneseWordsToNumberConverter());
        Register("ko", _ => new KoreanWordsToNumberConverter());
        Register("vi", _ => new VietnameseWordsToNumberConverter());
        Register("zh-CN", _ => new ChineseWordsToNumberConverter());
        Register("zh-Hans", _ => new ChineseWordsToNumberConverter());
        Register("zh-Hant", _ => new ChineseWordsToNumberConverter());
    }

    void RegisterShared(IWordsToNumberConverter converter, params string[] cultures)
    {
        foreach (var culture in cultures)
        {
            Register(culture, _ => converter);
        }
    }
}

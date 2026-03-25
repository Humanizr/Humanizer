namespace Humanizer;

using System.Globalization;

internal class WordsToNumberConverterRegistry : LocaliserRegistry<IWordsToNumberConverter>
{
    public WordsToNumberConverterRegistry()
        : base(culture => new DefaultWordsToNumberConverter(culture))
    {
        Register("en", _ => new EnglishWordsToNumberConverter());
        Register("af", _ => new AfrikaansWordsToNumberConverter());
        Register("ar", _ => new ArabicWordsToNumberConverter());
        Register("bg", _ => new BulgarianWordsToNumberConverter());
        Register("bn", _ => new BengaliWordsToNumberConverter());
        Register("ca", _ => new CatalanWordsToNumberConverter());
        Register("cs", _ => new CzechWordsToNumberConverter());
        Register("da", _ => new DanishWordsToNumberConverter());
        Register("de", _ => new GermanWordsToNumberConverter());
        Register("de-CH", _ => new GermanWordsToNumberConverter());
        Register("de-LI", _ => new GermanWordsToNumberConverter());
        Register("el", _ => new GreekWordsToNumberConverter());
        Register("fi", _ => new FinnishWordsToNumberConverter());
        Register("fi-FI", _ => new FinnishWordsToNumberConverter());
        Register("fr", _ => new FrenchWordsToNumberConverter());
        Register("he", _ => new HebrewWordsToNumberConverter());
        Register("hr", _ => new CroatianWordsToNumberConverter());
        Register("ku", _ => new KurdishWordsToNumberConverter());
        Register("lb", _ => new LuxembourgishWordsToNumberConverter());
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
        Register("fa", _ => new PersianWordsToNumberConverter());
        Register("nl", _ => new DutchWordsToNumberConverter());
        Register("sv", _ => new SwedishWordsToNumberConverter());
        Register("th", _ => new ThaiWordsToNumberConverter());
        Register("th-TH", _ => new ThaiWordsToNumberConverter());
        Register("hy", _ => new ArmenianWordsToNumberConverter());
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
}

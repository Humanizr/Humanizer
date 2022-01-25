using Humanizer.Localisation.NumberToWords;

namespace Humanizer.Configuration
{
    internal class NumberToWordsConverterRegistry : LocaliserRegistry<INumberToWordsConverter>
    {
        public NumberToWordsConverterRegistry()
            : base((culture) => new EnglishNumberToWordsConverter())
        {
            Register("af", new AfrikaansNumberToWordsConverter());
            Register("en", new EnglishNumberToWordsConverter());
            Register("ar", new ArabicNumberToWordsConverter());
            Register("cs", (culture) => new CzechNumberToWordsConverter(culture));
            Register("fa", new FarsiNumberToWordsConverter());
            Register("es", new SpanishNumberToWordsConverter());
            Register("pl", (culture) => new PolishNumberToWordsConverter(culture));
            Register("pt", new PortugueseNumberToWordsConverter());
            Register("pt-BR", new BrazilianPortugueseNumberToWordsConverter());
            Register("ro", new RomanianNumberToWordsConverter());
            Register("ru", new RussianNumberToWordsConverter());
            Register("fi", new FinnishNumberToWordsConverter());
            Register("fr-BE", new FrenchBelgianNumberToWordsConverter());
            Register("fr-CH", new FrenchSwissNumberToWordsConverter());
            Register("fr", new FrenchNumberToWordsConverter());
            Register("nl", new DutchNumberToWordsConverter());
            Register("he", (culture) => new HebrewNumberToWordsConverter(culture));
            Register("sl", (culture) => new SlovenianNumberToWordsConverter(culture));
            Register("de", new GermanNumberToWordsConverter());
            Register("de-CH", new GermanSwissLiechtensteinNumberToWordsConverter());
            Register("de-LI", new GermanSwissLiechtensteinNumberToWordsConverter());
            Register("bn-BD", new BanglaNumberToWordsConverter());
            Register("tr", new TurkishNumberToWordConverter());
            Register("is", new IcelandicNumberToWordsConverter());
            Register("it", new ItalianNumberToWordsConverter());
            Register("mt", new MalteseNumberToWordsConvertor());
            Register("uk", new UkrainianNumberToWordsConverter());
            Register("uz-Latn-UZ", new UzbekLatnNumberToWordConverter());
            Register("uz-Cyrl-UZ", new UzbekCyrlNumberToWordConverter());
            Register("sv", new SwedishNumberToWordsConverter());
            Register("sr", (culture) => new SerbianCyrlNumberToWordsConverter(culture));
            Register("sr-Latn", (culture) => new SerbianNumberToWordsConverter(culture));
            Register("ta", new TamilNumberToWordsConverter());
            Register("hr", (culture) => new CroatianNumberToWordsConverter(culture));
            Register("nb", new NorwegianBokmalNumberToWordsConverter());
            Register("vi", new VietnameseNumberToWordsConverter());
            Register("zh-CN", new ChineseNumberToWordsConverter());
            Register("bg", new BulgarianNumberToWordsConverter());
            Register("hy", new ArmenianNumberToWordsConverter());
            Register("az", new AzerbaijaniNumberToWordsConverter());
            Register("ja", new JapaneseNumberToWordsConverter());
            Register("ku", new CentralKurdishNumberToWordsConverter());
            Register("el", new GreekNumberToWordsConverter());
            Register("th-TH", new ThaiNumberToWordsConverter());
            Register("lv", new LatvianNumberToWordsConverter());
            Register("ko-KR", new KoreanNumberToWordsConverter());
            Register("en-IN", new IndianNumberToWordsConverter());

        }
    }
}

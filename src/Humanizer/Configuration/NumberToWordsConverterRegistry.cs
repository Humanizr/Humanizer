namespace Humanizer;

class NumberToWordsConverterRegistry : LocaliserRegistry<INumberToWordsConverter>
{
    public NumberToWordsConverterRegistry()
        : base(_ => new EnglishNumberToWordsConverter())
    {
        Register("af", _ => new AfrikaansNumberToWordsConverter());
        Register("en", _ => new EnglishNumberToWordsConverter());
        Register("ar", _ => new ArabicNumberToWordsConverter());
        Register("cs", c => new CzechNumberToWordsConverter(c));
        Register("fa", _ => new FarsiNumberToWordsConverter());
        Register("es", _ => new SpanishNumberToWordsConverter());
        Register("pl", c => new PolishNumberToWordsConverter(c));
        Register("pt", _ => new PortugueseNumberToWordsConverter());
        Register("pt-BR", _ => new BrazilianPortugueseNumberToWordsConverter());
        Register("ro", _ => new RomanianNumberToWordsConverter());
        Register("ru", _ => new RussianNumberToWordsConverter());
        Register("fi", _ => new FinnishNumberToWordsConverter());
        Register("fr-BE", _ => new FrenchBelgianNumberToWordsConverter());
        Register("fr-CH", _ => new FrenchSwissNumberToWordsConverter());
        Register("fr", _ => new FrenchNumberToWordsConverter());
        Register("nl", _ => new DutchNumberToWordsConverter());
        Register("he", c => new HebrewNumberToWordsConverter(c));
        Register("sl", c => new SlovenianNumberToWordsConverter(c));
        Register("de", _ => new GermanNumberToWordsConverter());
        Register("de-CH", _ => new GermanSwissLiechtensteinNumberToWordsConverter());
        Register("de-LI", _ => new GermanSwissLiechtensteinNumberToWordsConverter());
        Register("bn", _ => new BanglaNumberToWordsConverter());
        Register("tr", _ => new TurkishNumberToWordConverter());
        Register("is", _ => new IcelandicNumberToWordsConverter());
        Register("it", _ => new ItalianNumberToWordsConverter());
        Register("mt", _ => new MalteseNumberToWordsConvertor());
        Register("uk", _ => new UkrainianNumberToWordsConverter());
        Register("uz-Latn-UZ", _ => new UzbekLatnNumberToWordConverter());
        Register("uz-Cyrl-UZ", _ => new UzbekCyrlNumberToWordConverter());
        Register("sv", _ => new SwedishNumberToWordsConverter());
        Register("sr", c => new SerbianCyrlNumberToWordsConverter(c));
        Register("sr-Latn", c => new SerbianNumberToWordsConverter(c));
        Register("ta", _ => new TamilNumberToWordsConverter());
        Register("hr", c => new CroatianNumberToWordsConverter(c));
        Register("nb", _ => new NorwegianBokmalNumberToWordsConverter());
        Register("vi", _ => new VietnameseNumberToWordsConverter());
        Register("zh-CN", _ => new ChineseNumberToWordsConverter());
        Register("zh-Hans", _ => new ChineseNumberToWordsConverter());
        Register("zh-Hant", _ => new ChineseNumberToWordsConverter());
        Register("bg", _ => new BulgarianNumberToWordsConverter());
        Register("hy", _ => new ArmenianNumberToWordsConverter());
        Register("az", _ => new AzerbaijaniNumberToWordsConverter());
        Register("ja", _ => new JapaneseNumberToWordsConverter());
        Register("ku", _ => new CentralKurdishNumberToWordsConverter());
        Register("el", _ => new GreekNumberToWordsConverter());
        Register("th", _ => new ThaiNumberToWordsConverter());
        Register("lv", _ => new LatvianNumberToWordsConverter());
        Register("ko", _ => new KoreanNumberToWordsConverter());
        Register("en-IN", _ => new IndianNumberToWordsConverter());
        Register("lt", _ => new LithuanianNumberToWordsConverter());
        Register("lb", _ => new LuxembourgishNumberToWordsConverter());
        Register("hu", _ => new HungarianNumberToWordsConverter());
        Register("ca", _ => new CatalanNumberToWordsConverter());
    }
}
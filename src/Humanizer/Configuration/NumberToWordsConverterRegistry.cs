namespace Humanizer;

class NumberToWordsConverterRegistry : LocaliserRegistry<INumberToWordsConverter>
{
    public NumberToWordsConverterRegistry()
        : base(new EnglishNumberToWordsConverter())
    {
        Register("af", new AfrikaansNumberToWordsConverter());
        Register("en", new EnglishNumberToWordsConverter());
        Register("ar", new ArabicNumberToWordsConverter());
        Register("cs", c => new CzechNumberToWordsConverter(c));
        Register("fa", new FarsiNumberToWordsConverter());
        Register("es", new SpanishNumberToWordsConverter());
        Register("pl", c => new PolishNumberToWordsConverter(c));
        Register("pt", new PortugueseNumberToWordsConverter());
        Register("pt-BR", new BrazilianPortugueseNumberToWordsConverter());
        Register("ro", new RomanianNumberToWordsConverter());
        Register("ru", new RussianNumberToWordsConverter());
        Register("fi", new FinnishNumberToWordsConverter());
        Register("fr-BE", new FrenchBelgianNumberToWordsConverter());
        Register("fr-CH", new FrenchSwissNumberToWordsConverter());
        Register("fr", new FrenchNumberToWordsConverter());
        Register("nl", new DutchNumberToWordsConverter());
        Register("he", c => new HebrewNumberToWordsConverter(c));
        Register("sl", c => new SlovenianNumberToWordsConverter(c));
        Register("de", new GermanNumberToWordsConverter());
        Register("de-CH", new GermanSwissLiechtensteinNumberToWordsConverter());
        Register("de-LI", new GermanSwissLiechtensteinNumberToWordsConverter());
        Register("bn", new BanglaNumberToWordsConverter());
        Register("tr", new TurkishNumberToWordConverter());
        Register("is", new IcelandicNumberToWordsConverter());
        Register("it", new ItalianNumberToWordsConverter());
        Register("mt", new MalteseNumberToWordsConvertor());
        Register("uk", new UkrainianNumberToWordsConverter());
        Register("uz-Latn-UZ", new UzbekLatnNumberToWordConverter());
        Register("uz-Cyrl-UZ", new UzbekCyrlNumberToWordConverter());
        Register("sv", new SwedishNumberToWordsConverter());
        Register("sr", c => new SerbianCyrlNumberToWordsConverter(c));
        Register("sr-Latn", c => new SerbianNumberToWordsConverter(c));
        Register("ta", new TamilNumberToWordsConverter());
        Register("hr", c => new CroatianNumberToWordsConverter(c));
        Register("nb", new NorwegianBokmalNumberToWordsConverter());
        Register("vi", new VietnameseNumberToWordsConverter());
        Register("zh-CN", new ChineseNumberToWordsConverter());
        Register("zh-Hans", new ChineseNumberToWordsConverter());
        Register("zh-Hant", new ChineseNumberToWordsConverter());
        Register("bg", new BulgarianNumberToWordsConverter());
        Register("hy", new ArmenianNumberToWordsConverter());
        Register("az", new AzerbaijaniNumberToWordsConverter());
        Register("ja", new JapaneseNumberToWordsConverter());
        Register("ku", new CentralKurdishNumberToWordsConverter());
        Register("el", new GreekNumberToWordsConverter());
        Register("th", new ThaiNumberToWordsConverter());
        Register("lv", new LatvianNumberToWordsConverter());
        Register("ko", new KoreanNumberToWordsConverter());
        Register("en-IN", new IndianNumberToWordsConverter());
        Register("lt", new LithuanianNumberToWordsConverter());
        Register("lb", new LuxembourgishNumberToWordsConverter());
        Register("hu", new HungarianNumberToWordsConverter());
    }
}
namespace Humanizer;

class NumberToWordsConverterRegistry : LocaliserRegistry<INumberToWordsConverter>
{
    public NumberToWordsConverterRegistry()
        : base(_ => new EnglishNumberToWordsConverter())
    {
            Register("af", new AfrikaansNumberToWordsConverter());
            Register("en", new EnglishNumberToWordsConverter());
            Register("ar", new ArabicNumberToWordsConverter());
            Register("cs", _ => new CzechNumberToWordsConverter(_));
            Register("fa", new FarsiNumberToWordsConverter());
            Register("es", new SpanishNumberToWordsConverter());
            Register("pl", _ => new PolishNumberToWordsConverter(_));
            Register("pt", new PortugueseNumberToWordsConverter());
            Register("pt-BR", new BrazilianPortugueseNumberToWordsConverter());
            Register("ro", new RomanianNumberToWordsConverter());
            Register("ru", new RussianNumberToWordsConverter());
            Register("fi", new FinnishNumberToWordsConverter());
            Register("fr-BE", new FrenchBelgianNumberToWordsConverter());
            Register("fr-CH", new FrenchSwissNumberToWordsConverter());
            Register("fr", new FrenchNumberToWordsConverter());
            Register("nl", new DutchNumberToWordsConverter());
            Register("he", _ => new HebrewNumberToWordsConverter(_));
            Register("sl", _ => new SlovenianNumberToWordsConverter(_));
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
            Register("sr", _ => new SerbianCyrlNumberToWordsConverter(_));
            Register("sr-Latn", _ => new SerbianNumberToWordsConverter(_));
            Register("ta", new TamilNumberToWordsConverter());
            Register("hr", _ => new CroatianNumberToWordsConverter(_));
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
            Register("lt", new LithuanianNumberToWordsConverter());
            Register("lb", new LuxembourgishNumberToWordsConverter());
        }
}
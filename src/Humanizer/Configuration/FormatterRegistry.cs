namespace Humanizer;

class FormatterRegistry : LocaliserRegistry<IFormatter>
{
    public FormatterRegistry()
        : base(c => new DefaultFormatter(c, InvariantResources.Instance))
    {
        Register("ar", c => new ArabicFormatter(c));
        Register("de", c => new GermanFormatter(c));
        Register("he", c => new HebrewFormatter(c));
        Register("ro", c => new RomanianFormatter(c));
        Register("ru", c => new RussianFormatter(c));
        Register("sl", c => new SlovenianFormatter(c));
        Register("hr", c => new CroatianFormatter(c));
        Register("sr", c => new SerbianFormatter(c, SerbianResources.Instance));
        Register("sr-Latn", c => new SerbianFormatter(c, SerbianLatinResources.Instance));
        Register("uk", c => new UkrainianFormatter(c));
        Register("fr", c => new FrenchFormatter(c, FrenchResources.Instance));
        Register("fr-BE", c => new FrenchFormatter(c, FrenchBelgiumResources.Instance));
        RegisterCzechSlovakPolishFormatter("cs", CzechResources.Instance);
        RegisterCzechSlovakPolishFormatter("pl", PolishResources.Instance);
        RegisterCzechSlovakPolishFormatter("sk", SlovakResources.Instance);
        Register("bg", c => new BulgarianFormatter(c));
        RegisterDefaultFormatter("ku", CentralKurdishResources.Instance);
        RegisterDefaultFormatter("pt", PortugueseResources.Instance);
        RegisterDefaultFormatter("sv", SwedishResources.Instance);
        RegisterDefaultFormatter("tr", TurkishResources.Instance);
        RegisterDefaultFormatter("vi", VietnameseResources.Instance);
        RegisterDefaultFormatter("en-US", InvariantResources.Instance);
        RegisterDefaultFormatter("af", AfrikaansResources.Instance);
        RegisterDefaultFormatter("az", AzerbaijaniResources.Instance);
        RegisterDefaultFormatter("da", DanishResources.Instance);
        RegisterDefaultFormatter("el", GreekResources.Instance);
        RegisterDefaultFormatter("es", SpanishResources.Instance);
        RegisterDefaultFormatter("fa", PersianResources.Instance);
        RegisterDefaultFormatter("fi-FI", FinnishFinlandResources.Instance);
        RegisterDefaultFormatter("fil-PH", FilipinoPhilippinesResources.Instance);
        RegisterDefaultFormatter("hu", HungarianResources.Instance);
        RegisterDefaultFormatter("hy", ArmenianResources.Instance);
        RegisterDefaultFormatter("id", IndonesianResources.Instance);
        Register("is", c => new IcelandicFormatter(c));
        RegisterDefaultFormatter("ja", JapaneseResources.Instance);
        RegisterDefaultFormatter("ko-KR", KoreanKoreaResources.Instance);
        RegisterDefaultFormatter("lv", LatvianResources.Instance);
        Register("mt", c => new MalteseFormatter(c));
        RegisterDefaultFormatter("ms-MY", MalayMalaysiaResources.Instance);
        RegisterDefaultFormatter("nb", NorwegianBokmålResources.Instance);
        RegisterDefaultFormatter("nb-NO", NorwegianBokmålNorwayResources.Instance);
        RegisterDefaultFormatter("nl", DutchResources.Instance);
        RegisterDefaultFormatter("bn-BD", BanglaBangladeshResources.Instance);
        RegisterDefaultFormatter("it", ItalianResources.Instance);
        RegisterDefaultFormatter("ta", InvariantResources.Instance);
        RegisterDefaultFormatter("uz-Latn-UZ", UzbekLatinUzbekistanResources.Instance);
        RegisterDefaultFormatter("uz-Cyrl-UZ", UzbekCyrillicUzbekistanResources.Instance);
        RegisterDefaultFormatter("zh-CN", ChineseSimplifiedPRCResources.Instance);
        RegisterDefaultFormatter("zh-Hans", ChineseSimplifiedResources.Instance);
        RegisterDefaultFormatter("zh-Hant", ChineseTraditionalResources.Instance);
        RegisterDefaultFormatter("th-TH", ThaiThailandResources.Instance);
        RegisterDefaultFormatter("en-IN", InvariantResources.Instance);
        Register("lt", c => new LithuanianFormatter(c));
        Register("lb", c => new LuxembourgishFormatter(c));
    }

    void RegisterDefaultFormatter(string localeCode, IResources resources) =>
        Register(localeCode, c => new DefaultFormatter(c, resources));

    void RegisterCzechSlovakPolishFormatter(string localeCode, IResources resources) =>
        Register(localeCode, c => new CzechSlovakPolishFormatter(c, resources));
}
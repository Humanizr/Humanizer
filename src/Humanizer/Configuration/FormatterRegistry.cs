namespace Humanizer;

class FormatterRegistry : LocaliserRegistry<IFormatter>
{
    public FormatterRegistry()
        : base(c => new DefaultFormatter(c))
    {
        Register("ar", c => new ArabicFormatter(c));
        Register("de", c => new GermanFormatter(c));
        Register("he", c => new HebrewFormatter(c));
        Register("ro", c => new RomanianFormatter(c));
        Register("ru", c => new RussianFormatter(c));
        Register("sl", c => new SlovenianFormatter(c));
        Register("hr", c => new CroatianFormatter(c));
        Register("sr", c => new SerbianFormatter(c));
        Register("uk", c => new UkrainianFormatter(c));
        Register("fr", c => new FrenchFormatter(c));
        RegisterCzechSlovakPolishFormatter("cs");
        RegisterCzechSlovakPolishFormatter("pl");
        RegisterCzechSlovakPolishFormatter("sk");
        Register("bg", c => new BulgarianFormatter(c));
        RegisterDefaultFormatter("ku");
        RegisterDefaultFormatter("pt");
        RegisterDefaultFormatter("sv");
        RegisterDefaultFormatter("tr");
        RegisterDefaultFormatter("vi");
        RegisterDefaultFormatter("en");
        RegisterDefaultFormatter("af");
        RegisterDefaultFormatter("az");
        RegisterDefaultFormatter("da");
        RegisterDefaultFormatter("el");
        RegisterDefaultFormatter("es");
        RegisterDefaultFormatter("fa");
        RegisterDefaultFormatter("fi");
        RegisterDefaultFormatter("fil");
        Register("hu", c => new HungarianFormatter(c));
        RegisterDefaultFormatter("hy");
        RegisterDefaultFormatter("id");
        Register("is", c => new IcelandicFormatter(c));
        RegisterDefaultFormatter("ja");
        Register("ko", c => new TrimPluralSuffixFormatter(c));
        Register("lv", c => new LatvianFormatter(c));
        Register("mt", c => new MalteseFormatter(c));
        Register("ms", c => new TrimPluralSuffixFormatter(c));
        RegisterDefaultFormatter("nb");
        RegisterDefaultFormatter("nl");
        Register("bn", c => new TrimPluralSuffixFormatter(c));
        RegisterDefaultFormatter("it");
        RegisterDefaultFormatter("ta");
        RegisterDefaultFormatter("uz-Latn-UZ");
        RegisterDefaultFormatter("uz-Cyrl-UZ");
        RegisterDefaultFormatter("zh-CN");
        RegisterDefaultFormatter("zh-Hans");
        RegisterDefaultFormatter("zh-Hant");
        Register("th", c => new TrimPluralSuffixFormatter(c));
        Register("lt", c => new LithuanianFormatter(c));
        Register("lb", c => new LuxembourgishFormatter(c));
        Register("ca", c => new CatalanFormatter(c));
    }

    void RegisterDefaultFormatter(string localeCode) =>
        Register(localeCode, c => new DefaultFormatter(c));

    void RegisterCzechSlovakPolishFormatter(string localeCode) =>
        Register(localeCode, c => new CzechSlovakPolishFormatter(c));
}

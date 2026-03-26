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
        Register("sr-Latn", c => new SerbianFormatter(c));
        Register("uk", c => new UkrainianFormatter(c));
        Register("fr", c => new FrenchFormatter(c));
        RegisterCzechSlovakPolishFormatter("cs");
        RegisterCzechSlovakPolishFormatter("pl");
        RegisterCzechSlovakPolishFormatter("sk");
        Register("bg", c => new BulgarianFormatter(c));
        Register("sv", c => new SwedishFormatter(c));
        RegisterTrimPluralSuffixFormatter("tr", "vi", "az", "el", "fa");
        Register("fi", c => new FinnishFormatter(c));
        Register("hu", c => new HungarianFormatter(c));
        RegisterTrimPluralSuffixFormatter("hy", "id");
        Register("is", c => new IcelandicFormatter(c));
        RegisterTrimPluralSuffixFormatter("ja", "ko");
        Register("lv", c => new LatvianFormatter(c));
        Register("mt", c => new MalteseFormatter(c));
        RegisterTrimPluralSuffixFormatter("ms", "nb", "bn");
        Register("it", c => new ItalianFormatter(c));
        RegisterTrimPluralSuffixFormatter("uz-Latn-UZ", "uz-Cyrl-UZ", "zh-CN", "zh-Hans", "zh-Hant", "th");
        Register("lt", c => new LithuanianFormatter(c));
        Register("lb", c => new LuxembourgishFormatter(c));
        Register("ca", c => new CatalanFormatter(c));
    }

    void RegisterCzechSlovakPolishFormatter(string localeCode) =>
        Register(localeCode, c => new CzechSlovakPolishFormatter(c));

    void RegisterTrimPluralSuffixFormatter(params string[] localeCodes)
    {
        foreach (var localeCode in localeCodes)
        {
            Register(localeCode, c => new TrimPluralSuffixFormatter(c));
        }
    }
}

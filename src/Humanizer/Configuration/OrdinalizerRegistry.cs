namespace Humanizer;

class OrdinalizerRegistry : LocaliserRegistry<IOrdinalizer>
{
    public OrdinalizerRegistry()
        : base(_ => new DefaultOrdinalizer())
    {
        RegisterDefaultOrdinalizer("af");
        RegisterDefaultOrdinalizer("ar");
        Register("az", _ => new AzerbaijaniOrdinalizer());
        RegisterDefaultOrdinalizer("bg");
        RegisterDefaultOrdinalizer("bn");
        Register("ca", _ => new CatalanOrdinalizer());
        RegisterDefaultOrdinalizer("cs");
        RegisterDefaultOrdinalizer("da");
        Register("de", _ => new GermanOrdinalizer());
        Register("en", _ => new EnglishOrdinalizer());
        RegisterDefaultOrdinalizer("el");
        Register("es", c => new SpanishOrdinalizer(c));
        RegisterDefaultOrdinalizer("fa");
        RegisterDefaultOrdinalizer("fi");
        RegisterDefaultOrdinalizer("fil");
        Register("fr", _ => new FrenchOrdinalizer());
        RegisterDefaultOrdinalizer("he");
        RegisterDefaultOrdinalizer("hr");
        Register("hu", _ => new HungarianOrdinalizer());
        Register("hy", _ => new ArmenianOrdinalizer());
        RegisterDefaultOrdinalizer("id");
        Register("is", _ => new IcelandicOrdinalizer());
        Register("it", _ => new ItalianOrdinalizer());
        RegisterDefaultOrdinalizer("ja");
        RegisterDefaultOrdinalizer("ko");
        RegisterDefaultOrdinalizer("ku");
        RegisterDefaultOrdinalizer("lt");
        RegisterDefaultOrdinalizer("lv");
        Register("lb", _ => new LuxembourgishOrdinalizer());
        RegisterDefaultOrdinalizer("ms");
        RegisterDefaultOrdinalizer("mt");
        RegisterDefaultOrdinalizer("nb");
        Register("nl", _ => new DutchOrdinalizer());
        RegisterDefaultOrdinalizer("pl");
        Register("pt", _ => new PortugueseOrdinalizer());
        Register("pt-BR", _ => new PortugueseOrdinalizer());
        Register("ro", _ => new RomanianOrdinalizer());
        Register("ru", _ => new RussianOrdinalizer());
        RegisterDefaultOrdinalizer("sk");
        RegisterDefaultOrdinalizer("sl");
        RegisterDefaultOrdinalizer("sr");
        RegisterDefaultOrdinalizer("sr-Latn");
        RegisterDefaultOrdinalizer("sv");
        RegisterDefaultOrdinalizer("th");
        Register("tr", _ => new TurkishOrdinalizer());
        Register("uk", _ => new UkrainianOrdinalizer());
        RegisterDefaultOrdinalizer("uz-Cyrl-UZ");
        RegisterDefaultOrdinalizer("uz-Latn-UZ");
        RegisterDefaultOrdinalizer("vi");
        RegisterDefaultOrdinalizer("zh-CN");
        RegisterDefaultOrdinalizer("zh-Hans");
        RegisterDefaultOrdinalizer("zh-Hant");
    }

    void RegisterDefaultOrdinalizer(string localeCode) =>
        Register(localeCode, _ => new DefaultOrdinalizer());
}

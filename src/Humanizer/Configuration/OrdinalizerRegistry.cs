namespace Humanizer;

class OrdinalizerRegistry : LocaliserRegistry<IOrdinalizer>
{
    public OrdinalizerRegistry()
        : base(_ => new DefaultOrdinalizer())
    {
        Register("af", _ => new AfrikaansOrdinalizer());
        RegisterDefaultOrdinalizer("ar");
        Register("az", _ => new AzerbaijaniOrdinalizer());
        RegisterNumericSuffixOrdinalizer("bg", ".");
        RegisterDefaultOrdinalizer("bn");
        Register("ca", _ => new CatalanOrdinalizer());
        RegisterNumericSuffixOrdinalizer("cs", ".");
        RegisterNumericSuffixOrdinalizer("da", ".");
        Register("de", _ => new GermanOrdinalizer());
        Register("en", _ => new EnglishOrdinalizer());
        RegisterDefaultOrdinalizer("el");
        Register("es", c => new SpanishOrdinalizer(c));
        RegisterDefaultOrdinalizer("fa");
        RegisterNumericSuffixOrdinalizer("fi", ".");
        RegisterDefaultOrdinalizer("fil");
        Register("fr", _ => new FrenchOrdinalizer());
        RegisterDefaultOrdinalizer("he");
        RegisterNumericSuffixOrdinalizer("hr", ".");
        Register("hu", _ => new HungarianOrdinalizer());
        Register("hy", _ => new ArmenianOrdinalizer());
        RegisterDefaultOrdinalizer("id");
        Register("is", _ => new IcelandicOrdinalizer());
        Register("it", _ => new ItalianOrdinalizer());
        RegisterDefaultOrdinalizer("ja");
        RegisterDefaultOrdinalizer("ko");
        RegisterDefaultOrdinalizer("ku");
        RegisterNumericSuffixOrdinalizer("lt", ".");
        RegisterNumericSuffixOrdinalizer("lv", ".");
        Register("lb", _ => new LuxembourgishOrdinalizer());
        RegisterDefaultOrdinalizer("ms");
        RegisterDefaultOrdinalizer("mt");
        RegisterNumericSuffixOrdinalizer("nb", ".");
        Register("nl", _ => new DutchOrdinalizer());
        RegisterNumericSuffixOrdinalizer("pl", ".");
        Register("pt", _ => new PortugueseOrdinalizer());
        Register("pt-BR", _ => new PortugueseOrdinalizer());
        Register("ro", _ => new RomanianOrdinalizer());
        Register("ru", _ => new RussianOrdinalizer());
        RegisterNumericSuffixOrdinalizer("sk", ".");
        RegisterNumericSuffixOrdinalizer("sl", ".");
        RegisterNumericSuffixOrdinalizer("sr", ".");
        RegisterNumericSuffixOrdinalizer("sr-Latn", ".");
        Register("sv", _ => new SwedishOrdinalizer());
        RegisterDefaultOrdinalizer("th");
        Register("tr", _ => new TurkishOrdinalizer());
        Register("uk", _ => new UkrainianOrdinalizer());
        Register("uz-Cyrl-UZ", _ => new UzbekCyrillicOrdinalizer());
        Register("uz-Latn-UZ", _ => new UzbekLatinOrdinalizer());
        RegisterDefaultOrdinalizer("vi");
        RegisterDefaultOrdinalizer("zh-CN");
        RegisterDefaultOrdinalizer("zh-Hans");
        RegisterDefaultOrdinalizer("zh-Hant");
    }

    void RegisterDefaultOrdinalizer(string localeCode) =>
        Register(localeCode, _ => new DefaultOrdinalizer());

    void RegisterNumericSuffixOrdinalizer(string localeCode, string suffix) =>
        Register(localeCode, _ => new NumericSuffixOrdinalizer(suffix));
}

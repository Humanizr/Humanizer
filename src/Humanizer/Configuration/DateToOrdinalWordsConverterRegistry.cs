namespace Humanizer;

class DateToOrdinalWordsConverterRegistry : LocaliserRegistry<IDateToOrdinalWordConverter>
{
    public DateToOrdinalWordsConverterRegistry()
        : base(_ => new DefaultDateToOrdinalWordConverter())
    {
        RegisterDefaultConverter("af");
        RegisterDefaultConverter("ar");
        RegisterDefaultConverter("az");
        RegisterDefaultConverter("bg");
        RegisterDefaultConverter("bn");
        Register("en-US", _ => new UsDateToOrdinalWordsConverter());
        Register("ca", _ => new CaDateToOrdinalWordsConverter());
        RegisterDefaultConverter("cs");
        RegisterDefaultConverter("da");
        Register("de", _ => new LongDateToOrdinalWordsConverter());
        RegisterDefaultConverter("el");
        Register("es", _ => new EsDateToOrdinalWordsConverter());
        RegisterDefaultConverter("fa");
        RegisterDefaultConverter("fi");
        RegisterDefaultConverter("fil");
        Register("fr", _ => new FrDateToOrdinalWordsConverter());
        RegisterDefaultConverter("he");
        RegisterDefaultConverter("hr");
        RegisterDefaultConverter("hu");
        RegisterDefaultConverter("hy");
        RegisterDefaultConverter("id");
        RegisterDefaultConverter("is");
        RegisterDefaultConverter("it");
        RegisterDefaultConverter("ja");
        RegisterDefaultConverter("ko");
        RegisterDefaultConverter("ku");
        Register("lb", _ => new LongDateToOrdinalWordsConverter());
        Register("lt", _ => new LtDateToOrdinalWordsConverter());
        Register("lv", _ => new LvDateToOrdinalWordsConverter());
        RegisterDefaultConverter("ms");
        RegisterDefaultConverter("mt");
        RegisterDefaultConverter("nb");
        RegisterDefaultConverter("nl");
        RegisterDefaultConverter("pl");
        RegisterDefaultConverter("pt");
        Register("pt-BR", _ => new PtBrDateToOrdinalWordsConverter());
        RegisterDefaultConverter("ro");
        RegisterDefaultConverter("ru");
        RegisterDefaultConverter("sk");
        RegisterDefaultConverter("sl");
        RegisterDefaultConverter("sr");
        RegisterDefaultConverter("sr-Latn");
        RegisterDefaultConverter("sv");
        RegisterDefaultConverter("th");
        RegisterDefaultConverter("tr");
        RegisterDefaultConverter("uk");
        RegisterDefaultConverter("uz-Cyrl-UZ");
        RegisterDefaultConverter("uz-Latn-UZ");
        RegisterDefaultConverter("vi");
        RegisterDefaultConverter("zh-CN");
        RegisterDefaultConverter("zh-Hans");
        RegisterDefaultConverter("zh-Hant");
    }

    void RegisterDefaultConverter(string localeCode) =>
        Register(localeCode, _ => new DefaultDateToOrdinalWordConverter());
}

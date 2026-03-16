#if NET6_0_OR_GREATER
namespace Humanizer;

class DateOnlyToOrdinalWordsConverterRegistry : LocaliserRegistry<IDateOnlyToOrdinalWordConverter>
{
    public DateOnlyToOrdinalWordsConverterRegistry() : base(_ => new DefaultDateOnlyToOrdinalWordConverter())
    {
        RegisterDefaultConverter("af");
        RegisterDefaultConverter("ar");
        RegisterDefaultConverter("az");
        RegisterDefaultConverter("bg");
        RegisterDefaultConverter("bn");
        Register("ca", _ => new CaDateOnlyToOrdinalWordsConverter());
        RegisterDefaultConverter("cs");
        RegisterDefaultConverter("da");
        Register("de", _ => new LongDateOnlyToOrdinalWordsConverter());
        Register("en-US", _ => new UsDateOnlyToOrdinalWordsConverter());
        RegisterDefaultConverter("el");
        Register("es", _ => new EsDateOnlyToOrdinalWordsConverter());
        RegisterDefaultConverter("fa");
        RegisterDefaultConverter("fi");
        RegisterDefaultConverter("fil");
        Register("fr", _ => new FrDateOnlyToOrdinalWordsConverter());
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
        Register("lb", _ => new LongDateOnlyToOrdinalWordsConverter());
        Register("lt", _ => new LtDateOnlyToOrdinalWordsConverter());
        Register("lv", _ => new LongDateOnlyToOrdinalWordsConverter());
        RegisterDefaultConverter("ms");
        RegisterDefaultConverter("mt");
        RegisterDefaultConverter("nb");
        RegisterDefaultConverter("nl");
        RegisterDefaultConverter("pl");
        RegisterDefaultConverter("pt");
        Register("pt-BR", _ => new PtBrDateOnlyToOrdinalWordsConverter());
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
        Register(localeCode, _ => new DefaultDateOnlyToOrdinalWordConverter());
}
#endif

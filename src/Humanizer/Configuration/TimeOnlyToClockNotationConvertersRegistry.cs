#if NET6_0_OR_GREATER

namespace Humanizer;

class TimeOnlyToClockNotationConvertersRegistry : LocaliserRegistry<ITimeOnlyToClockNotationConverter>
{
    public TimeOnlyToClockNotationConvertersRegistry() : base(_ => new DefaultTimeOnlyToClockNotationConverter())
    {
        RegisterDefaultConverter("af");
        RegisterDefaultConverter("ar");
        RegisterDefaultConverter("az");
        RegisterDefaultConverter("bg");
        RegisterDefaultConverter("bn");
        Register("ca", _ => new CaTimeOnlyToClockNotationConverter());
        RegisterDefaultConverter("cs");
        RegisterDefaultConverter("da");
        Register("de", _ => new GermanTimeOnlyToClockNotationConverter());
        RegisterDefaultConverter("el");
        Register("es", _ => new EsTimeOnlyToClockNotationConverter());
        RegisterDefaultConverter("fa");
        RegisterDefaultConverter("fi");
        RegisterDefaultConverter("fil");
        Register("fr", _ => new FrTimeOnlyToClockNotationConverter());
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
        Register("lb", _ => new LbTimeOnlyToClockNotationConverter());
        RegisterDefaultConverter("lt");
        RegisterDefaultConverter("lv");
        RegisterDefaultConverter("ms");
        RegisterDefaultConverter("mt");
        RegisterDefaultConverter("nb");
        RegisterDefaultConverter("nl");
        RegisterDefaultConverter("pl");
        Register("pt", _ => new PortugueseTimeOnlyToClockNotationConverter());
        Register("pt-BR", _ => new BrazilianPortugueseTimeOnlyToClockNotationConverter());
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
        Register(localeCode, _ => new DefaultTimeOnlyToClockNotationConverter());
}

#endif

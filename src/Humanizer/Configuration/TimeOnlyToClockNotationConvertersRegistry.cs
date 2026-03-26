#if NET6_0_OR_GREATER

namespace Humanizer;

class TimeOnlyToClockNotationConvertersRegistry : LocaliserRegistry<ITimeOnlyToClockNotationConverter>
{
    public TimeOnlyToClockNotationConvertersRegistry() : base(culture => new DefaultTimeOnlyToClockNotationConverter(culture))
    {
        RegisterDefaultConverters("af", "ar", "az", "bg", "bn");
        Register("ca", _ => new CaTimeOnlyToClockNotationConverter());
        RegisterDefaultConverters("cs", "da");
        Register("de", _ => new GermanTimeOnlyToClockNotationConverter());
        RegisterDefaultConverters("el");
        Register("es", _ => new EsTimeOnlyToClockNotationConverter());
        RegisterDefaultConverters("fa", "fi", "fil");
        Register("fr", _ => new FrTimeOnlyToClockNotationConverter());
        RegisterDefaultConverters("he", "hr", "hu", "hy", "id", "is", "it", "ja", "ko", "ku");
        Register("lb", _ => new LbTimeOnlyToClockNotationConverter());
        RegisterDefaultConverters("lt", "lv", "ms", "mt", "nb", "nl", "pl");
        Register("pt", _ => new PortugueseTimeOnlyToClockNotationConverter());
        Register("pt-BR", _ => new BrazilianPortugueseTimeOnlyToClockNotationConverter());
        RegisterDefaultConverters("ro", "ru", "sk", "sl", "sr", "sr-Latn", "sv", "th", "tr", "uk", "uz-Cyrl-UZ", "uz-Latn-UZ", "vi", "zh-CN", "zh-Hans", "zh-Hant");
    }

    void RegisterDefaultConverters(params string[] localeCodes)
    {
        foreach (var localeCode in localeCodes)
        {
            Register(localeCode, culture => new DefaultTimeOnlyToClockNotationConverter(culture));
        }
    }
}

#endif

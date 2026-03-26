namespace Humanizer;

class DateToOrdinalWordsConverterRegistry : LocaliserRegistry<IDateToOrdinalWordConverter>
{
    public DateToOrdinalWordsConverterRegistry()
        : base(_ => new DefaultDateToOrdinalWordConverter())
    {
        RegisterDefaultConverters("af", "ar", "az", "bg", "bn");
        Register("en-US", _ => new UsDateToOrdinalWordsConverter());
        Register("ca", _ => new CaDateToOrdinalWordsConverter());
        RegisterDefaultConverters("cs", "da");
        Register("de", _ => new LongDateToOrdinalWordsConverter());
        RegisterDefaultConverters("el");
        Register("es", _ => new EsDateToOrdinalWordsConverter());
        RegisterDefaultConverters("fa", "fi", "fil");
        Register("fr", _ => new FrDateToOrdinalWordsConverter());
        RegisterDefaultConverters("he", "hr", "hu", "hy", "id", "is", "it", "ja", "ko", "ku");
        Register("lb", _ => new LongDateToOrdinalWordsConverter());
        Register("lt", _ => new LtDateToOrdinalWordsConverter());
        Register("lv", _ => new LvDateToOrdinalWordsConverter());
        RegisterDefaultConverters("ms", "mt", "nb", "nl", "pl", "pt");
        Register("pt-BR", _ => new PtBrDateToOrdinalWordsConverter());
        RegisterDefaultConverters("ro", "ru", "sk", "sl", "sr", "sr-Latn", "sv", "th", "tr", "uk", "uz-Cyrl-UZ", "uz-Latn-UZ", "vi", "zh-CN", "zh-Hans", "zh-Hant");
    }

    void RegisterDefaultConverters(params string[] localeCodes)
    {
        foreach (var localeCode in localeCodes)
        {
            Register(localeCode, _ => new DefaultDateToOrdinalWordConverter());
        }
    }
}

#if NET6_0_OR_GREATER
namespace Humanizer;

class DateOnlyToOrdinalWordsConverterRegistry : LocaliserRegistry<IDateOnlyToOrdinalWordConverter>
{
    public DateOnlyToOrdinalWordsConverterRegistry() : base(_ => new DefaultDateOnlyToOrdinalWordConverter())
    {
        RegisterDefaultConverters("af", "ar", "az", "bg", "bn");
        Register("en-US", _ => new UsDateOnlyToOrdinalWordsConverter());
        Register("ca", _ => new CaDateOnlyToOrdinalWordsConverter());
        RegisterDefaultConverters("cs", "da");
        Register("de", _ => new LongDateOnlyToOrdinalWordsConverter());
        RegisterDefaultConverters("el");
        Register("es", _ => new EsDateOnlyToOrdinalWordsConverter());
        RegisterDefaultConverters("fa", "fi", "fil");
        Register("fr", _ => new FrDateOnlyToOrdinalWordsConverter());
        RegisterDefaultConverters("he", "hr", "hu", "hy", "id", "is", "it", "ja", "ko", "ku");
        Register("lb", _ => new LongDateOnlyToOrdinalWordsConverter());
        Register("lt", _ => new LtDateOnlyToOrdinalWordsConverter());
        Register("lv", _ => new LvDateOnlyToOrdinalWordsConverter());
        RegisterDefaultConverters("ms", "mt", "nb", "nl", "pl", "pt");
        Register("pt-BR", _ => new PtBrDateOnlyToOrdinalWordsConverter());
        RegisterDefaultConverters("ro", "ru", "sk", "sl", "sr", "sr-Latn", "sv", "th", "tr", "uk", "uz-Cyrl-UZ", "uz-Latn-UZ", "vi", "zh-CN", "zh-Hans", "zh-Hant");
    }

    void RegisterDefaultConverters(params string[] localeCodes)
    {
        foreach (var localeCode in localeCodes)
        {
            Register(localeCode, _ => new DefaultDateOnlyToOrdinalWordConverter());
        }
    }
}
#endif

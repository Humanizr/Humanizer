namespace Humanizer;

class CollectionFormatterRegistry : LocaliserRegistry<ICollectionFormatter>
{
    public CollectionFormatterRegistry()
        : base(_ => new DefaultCollectionFormatter("&"))
    {
        Register("af", _ => new DefaultCollectionFormatter("en"));
        Register("ar", _ => new CompactCollectionFormatter(" و", "، "));
        Register("az", _ => new DefaultCollectionFormatter("və"));
        Register("bg", _ => new DefaultCollectionFormatter("и"));
        Register("bn", _ => new DefaultCollectionFormatter("ও"));
        Register("ca", _ => new DefaultCollectionFormatter("i"));
        Register("cs", _ => new DefaultCollectionFormatter("a"));
        Register("da", _ => new DefaultCollectionFormatter("og"));
        Register("en", _ => new OxfordStyleCollectionFormatter());
        Register("de", _ => new DefaultCollectionFormatter("und"));
        Register("el", _ => new DefaultCollectionFormatter("και"));
        Register("es", _ => new DefaultCollectionFormatter("y"));
        Register("fa", _ => new DefaultCollectionFormatter("و"));
        Register("fi", _ => new DefaultCollectionFormatter("ja"));
        Register("fil", _ => new DefaultCollectionFormatter("at"));
        Register("fr", _ => new DefaultCollectionFormatter("et"));
        Register("he", _ => new CompactCollectionFormatter(" ו", ", "));
        Register("hr", _ => new DefaultCollectionFormatter("i"));
        Register("hu", _ => new DefaultCollectionFormatter("és"));
        Register("hy", _ => new DefaultCollectionFormatter("և"));
        Register("id", _ => new DefaultCollectionFormatter("dan"));
        Register("is", _ => new DefaultCollectionFormatter("og"));
        Register("it", _ => new DefaultCollectionFormatter("e"));
        Register("ja", _ => new CompactCollectionFormatter("と"));
        Register("ko", _ => new DefaultCollectionFormatter("및"));
        Register("ku", _ => new DefaultCollectionFormatter("و"));
        Register("lb", _ => new DefaultCollectionFormatter("an"));
        Register("lt", _ => new DefaultCollectionFormatter("ir"));
        Register("lv", _ => new DefaultCollectionFormatter("un"));
        Register("ms", _ => new DefaultCollectionFormatter("dan"));
        Register("mt", _ => new DefaultCollectionFormatter("u"));
        Register("nb", _ => new DefaultCollectionFormatter("og"));
        Register("nl", _ => new DefaultCollectionFormatter("en"));
        Register("pl", _ => new DefaultCollectionFormatter("i"));
        Register("pt", _ => new DefaultCollectionFormatter("e"));
        Register("pt-BR", _ => new DefaultCollectionFormatter("e"));
        Register("ro", _ => new DefaultCollectionFormatter("și"));
        Register("ru", _ => new DefaultCollectionFormatter("и"));
        Register("sk", _ => new DefaultCollectionFormatter("a"));
        Register("sl", _ => new DefaultCollectionFormatter("in"));
        Register("sr", _ => new DefaultCollectionFormatter("и"));
        Register("sr-Latn", _ => new DefaultCollectionFormatter("i"));
        Register("sv", _ => new DefaultCollectionFormatter("och"));
        Register("th", _ => new DefaultCollectionFormatter("และ"));
        Register("tr", _ => new DefaultCollectionFormatter("ve"));
        Register("uk", _ => new DefaultCollectionFormatter("і"));
        Register("uz-Cyrl-UZ", _ => new DefaultCollectionFormatter("ва"));
        Register("uz-Latn-UZ", _ => new DefaultCollectionFormatter("va"));
        Register("vi", _ => new DefaultCollectionFormatter("và"));
        Register("zh-CN", _ => new CompactCollectionFormatter("和"));
        Register("zh-Hans", _ => new CompactCollectionFormatter("和"));
        Register("zh-Hant", _ => new CompactCollectionFormatter("和"));
    }
}

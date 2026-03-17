namespace Humanizer;

class CollectionFormatterRegistry : LocaliserRegistry<ICollectionFormatter>
{
    public CollectionFormatterRegistry()
        : base(_ => new DefaultCollectionFormatter("&"))
    {
        Register("en", _ => new OxfordStyleCollectionFormatter());
        Register("af", _ => new DefaultCollectionFormatter("en"));
        Register("az", _ => new DefaultCollectionFormatter("və"));
        Register("bg", _ => new DefaultCollectionFormatter("и"));
        Register("bn", _ => new DefaultCollectionFormatter("ও"));
        Register("cs", _ => new DefaultCollectionFormatter("a"));
        Register("it", _ => new DefaultCollectionFormatter("e"));
        Register("de", _ => new DefaultCollectionFormatter("und"));
        Register("da", _ => new DefaultCollectionFormatter("og"));
        Register("nl", _ => new DefaultCollectionFormatter("en"));
        Register("pt", _ => new DefaultCollectionFormatter("e"));
        Register("el", _ => new DefaultCollectionFormatter("και"));
        Register("fi", _ => new DefaultCollectionFormatter("ja"));
        Register("fil", _ => new DefaultCollectionFormatter("at"));
        Register("fr", _ => new DefaultCollectionFormatter("et"));
        Register("hr", _ => new DefaultCollectionFormatter("i"));
        Register("hu", _ => new DefaultCollectionFormatter("és"));
        Register("hy", _ => new DefaultCollectionFormatter("և"));
        Register("id", _ => new DefaultCollectionFormatter("dan"));
        Register("ko", _ => new DefaultCollectionFormatter("및"));
        Register("ku", _ => new DefaultCollectionFormatter("û"));
        Register("lt", _ => new DefaultCollectionFormatter("ir"));
        Register("lv", _ => new DefaultCollectionFormatter("un"));
        Register("ms", _ => new DefaultCollectionFormatter("dan"));
        Register("mt", _ => new DefaultCollectionFormatter("u"));
        Register("pl", _ => new DefaultCollectionFormatter("i"));
        Register("pt-BR", _ => new DefaultCollectionFormatter("e"));
        Register("ro", _ => new DefaultCollectionFormatter("și"));
        Register("nn", _ => new DefaultCollectionFormatter("og"));
        Register("nb", _ => new DefaultCollectionFormatter("og"));
        Register("ru", _ => new DefaultCollectionFormatter("и"));
        Register("sk", _ => new DefaultCollectionFormatter("a"));
        Register("sl", _ => new DefaultCollectionFormatter("in"));
        Register("sr", _ => new DefaultCollectionFormatter("и"));
        Register("sr-Latn", _ => new DefaultCollectionFormatter("i"));
        Register("sv", _ => new DefaultCollectionFormatter("och"));
        Register("is", _ => new DefaultCollectionFormatter("og"));
        Register("es", _ => new DefaultCollectionFormatter("y"));
        Register("lb", _ => new DefaultCollectionFormatter("an"));
        Register("ca", _ => new DefaultCollectionFormatter("i"));
        Register("tr", _ => new DefaultCollectionFormatter("ve"));
        Register("uk", _ => new DefaultCollectionFormatter("і"));
        Register("uz-Latn-UZ", _ => new DefaultCollectionFormatter("va"));
        Register("vi", _ => new DefaultCollectionFormatter("và"));
    }
}

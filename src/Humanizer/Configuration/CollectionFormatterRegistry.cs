namespace Humanizer;

class CollectionFormatterRegistry : LocaliserRegistry<ICollectionFormatter>
{
    public CollectionFormatterRegistry()
        : base(_ => new DefaultCollectionFormatter("&"))
    {
        Register("en", _ => new OxfordStyleCollectionFormatter());
        Register("it", _ => new DefaultCollectionFormatter("e"));
        Register("de", _ => new DefaultCollectionFormatter("und"));
        Register("dk", _ => new DefaultCollectionFormatter("og"));
        Register("nl", _ => new DefaultCollectionFormatter("en"));
        Register("pt", _ => new DefaultCollectionFormatter("e"));
        Register("ro", _ => new DefaultCollectionFormatter("și"));
        Register("nn", _ => new DefaultCollectionFormatter("og"));
        Register("nb", _ => new DefaultCollectionFormatter("og"));
        Register("sv", _ => new DefaultCollectionFormatter("och"));
        Register("is", _ => new DefaultCollectionFormatter("og"));
        Register("es", _ => new DefaultCollectionFormatter("y"));
        Register("lb", _ => new DefaultCollectionFormatter("an"));
        Register("ca", _ => new DefaultCollectionFormatter("i"));
    }
}
using Humanizer.Localisation.CollectionFormatters;

namespace Humanizer.Configuration
{
    internal class CollectionFormatterRegistry : LocaliserRegistry<ICollectionFormatter>
    {
        public CollectionFormatterRegistry()
            : base(new DefaultCollectionFormatter("&"))
        {
            Register("en", new OxfordStyleCollectionFormatter("and"));
            Register("it", new DefaultCollectionFormatter("e"));
            Register("de", new DefaultCollectionFormatter("und"));
            Register("dk", new DefaultCollectionFormatter("og"));
            Register("nl", new DefaultCollectionFormatter("en"));
            Register("pt", new DefaultCollectionFormatter("e"));
            Register("ro", new DefaultCollectionFormatter("și"));
            Register("nn", new DefaultCollectionFormatter("og"));
            Register("nb", new DefaultCollectionFormatter("og"));
            Register("sv", new DefaultCollectionFormatter("och"));
            Register("is", new DefaultCollectionFormatter("og"));
            Register("es", new DefaultCollectionFormatter("y"));
        }
    }
}

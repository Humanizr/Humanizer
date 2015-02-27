using Humanizer.Localisation.CollectionFormatters;

namespace Humanizer.Configuration
{
    internal class CollectionFormatterRegistry : LocaliserRegistry<ICollectionFormatter>
    {
        public CollectionFormatterRegistry()
            : base(new DefaultCollectionFormatter())
        {
            Register("en", new OxfordStyleCollectionFormatter("and"));
            Register("it", new RegularStyleCollectionFormatter("e"));
            Register("de", new RegularStyleCollectionFormatter("und"));
            Register("dk", new RegularStyleCollectionFormatter("og"));
            Register("nl", new RegularStyleCollectionFormatter("en"));
            Register("pt", new RegularStyleCollectionFormatter("e"));
            Register("nn", new RegularStyleCollectionFormatter("og"));
            Register("nb", new RegularStyleCollectionFormatter("og"));
        }
    }
}
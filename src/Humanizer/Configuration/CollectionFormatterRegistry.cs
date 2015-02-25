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
        }
    }
}

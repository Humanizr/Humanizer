using Humanizer.Localisation.CollectionFormatters;

namespace Humanizer.Configuration
{
    internal class CollectionFormatterRegistry : LocaliserRegistry<ICollectionFormatter>
    {
        public CollectionFormatterRegistry()
            : base(new DefaultCollectionFormatter())
        {
            Register("en", new EnglishCollectionFormatter());
            Register("it", new ItalianCollectionFormatter());
        }
    }
}

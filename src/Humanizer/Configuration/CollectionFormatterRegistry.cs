namespace Humanizer;

class CollectionFormatterRegistry : LocaliserRegistry<ICollectionFormatter>
{
    public CollectionFormatterRegistry()
        : base(_ => new DefaultCollectionFormatter("&"))
        => CollectionFormatterRegistryRegistrations.Register(this);
}

namespace Humanizer;

class OrdinalizerRegistry : LocaliserRegistry<IOrdinalizer>
{
    public OrdinalizerRegistry()
        : base(_ => new DefaultOrdinalizer())
        => OrdinalizerRegistryRegistrations.Register(this);
}

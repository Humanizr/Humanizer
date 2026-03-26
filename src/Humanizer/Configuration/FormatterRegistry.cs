namespace Humanizer;

class FormatterRegistry : LocaliserRegistry<IFormatter>
{
    public FormatterRegistry()
        : base(c => new DefaultFormatter(c))
        => FormatterRegistryRegistrations.Register(this);
}

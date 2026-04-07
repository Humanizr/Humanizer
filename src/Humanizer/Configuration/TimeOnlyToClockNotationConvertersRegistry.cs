#if NET6_0_OR_GREATER

namespace Humanizer;

class TimeOnlyToClockNotationConvertersRegistry : LocaliserRegistry<ITimeOnlyToClockNotationConverter>
{
    public TimeOnlyToClockNotationConvertersRegistry() : base(_ => TimeOnlyToClockNotationProfileCatalog.Resolve("en")) =>
        TimeOnlyToClockNotationConvertersRegistryRegistrations.Register(this);
}

#endif
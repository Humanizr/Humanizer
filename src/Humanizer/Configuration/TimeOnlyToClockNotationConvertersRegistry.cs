#if NET6_0_OR_GREATER

namespace Humanizer;

class TimeOnlyToClockNotationConvertersRegistry : LocaliserRegistry<ITimeOnlyToClockNotationConverter>
{
    public TimeOnlyToClockNotationConvertersRegistry() : base(culture => TimeOnlyToClockNotationProfileCatalog.Resolve("en", culture)) =>
        TimeOnlyToClockNotationConvertersRegistryRegistrations.Register(this);
}

#endif
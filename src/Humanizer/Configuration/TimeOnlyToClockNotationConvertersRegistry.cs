#if NET6_0_OR_GREATER

namespace Humanizer;

class TimeOnlyToClockNotationConvertersRegistry : LocaliserRegistry<ITimeOnlyToClockNotationConverter>
{
    public TimeOnlyToClockNotationConvertersRegistry() : base(culture => new DefaultTimeOnlyToClockNotationConverter(culture)) =>
        TimeOnlyToClockNotationConvertersRegistryRegistrations.Register(this);
}

#endif

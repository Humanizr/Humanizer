#if NET6_0_OR_GREATER

namespace Humanizer;

class TimeOnlyToClockNotationConvertersRegistry : LocaliserRegistry<ITimeOnlyToClockNotationConverter>
{
    public TimeOnlyToClockNotationConvertersRegistry() : base(new DefaultTimeOnlyToClockNotationConverter())
    {
        Register("pt-BR", new BrazilianPortugueseTimeOnlyToClockNotationConverter());
        Register("fr", new FrTimeOnlyToClockNotationConverter());
        Register("es", new EsTimeOnlyToClockNotationConverter());
        Register("lb", new LbTimeOnlyToClockNotationConverter());
    }
}

#endif

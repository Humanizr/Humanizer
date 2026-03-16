#if NET6_0_OR_GREATER

namespace Humanizer;

class TimeOnlyToClockNotationConvertersRegistry : LocaliserRegistry<ITimeOnlyToClockNotationConverter>
{
    public TimeOnlyToClockNotationConvertersRegistry() : base(_ => new DefaultTimeOnlyToClockNotationConverter())
    {
        Register("pt-BR", _ => new BrazilianPortugueseTimeOnlyToClockNotationConverter());
        Register("fr", _ => new FrTimeOnlyToClockNotationConverter());
        Register("de", _ => new GermanTimeOnlyToClockNotationConverter());
        Register("es", _ => new EsTimeOnlyToClockNotationConverter());
        Register("lb", _ => new LbTimeOnlyToClockNotationConverter());
        Register("pt", _ => new PortugueseTimeOnlyToClockNotationConverter());
        Register("ca", _ => new CaTimeOnlyToClockNotationConverter());
    }
}

#endif

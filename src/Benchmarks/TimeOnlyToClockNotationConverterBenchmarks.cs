[MemoryDiagnoser(false)]
public class TimeOnlyToClockNotationConverterBenchmarks
{
    static readonly TimeOnly time = new(13, 6, 2, 88, 987);

    static readonly ITimeOnlyToClockNotationConverter esConverter = Configurator.TimeOnlyToClockNotationConverters.ResolveForCulture(new("es-ES"));
    [Benchmark]
    public void EsClockNotationConverter()
    {
        esConverter.Convert(time, ClockNotationRounding.None);
        esConverter.Convert(time, ClockNotationRounding.NearestFiveMinutes);
    }

    static readonly ITimeOnlyToClockNotationConverter brazilianConverter = Configurator.TimeOnlyToClockNotationConverters.ResolveForCulture(new("pt-BR"));
    [Benchmark]
    public void BrazilianPortugueseClockNotationConverter()
    {
        brazilianConverter.Convert(time, ClockNotationRounding.None);
        brazilianConverter.Convert(time, ClockNotationRounding.NearestFiveMinutes);
    }

    static readonly ITimeOnlyToClockNotationConverter frConverter = Configurator.TimeOnlyToClockNotationConverters.ResolveForCulture(new("fr"));
    [Benchmark]
    public void FrClockNotationConverter()
    {
        frConverter.Convert(time, ClockNotationRounding.None);
        frConverter.Convert(time, ClockNotationRounding.NearestFiveMinutes);
    }

    static readonly ITimeOnlyToClockNotationConverter lbConverter = Configurator.TimeOnlyToClockNotationConverters.ResolveForCulture(new("lb"));
    [Benchmark]
    public void LbClockNotationConverter()
    {
        lbConverter.Convert(time, ClockNotationRounding.None);
        lbConverter.Convert(time, ClockNotationRounding.NearestFiveMinutes);
    }

    static readonly ITimeOnlyToClockNotationConverter defaultConverter = Configurator.TimeOnlyToClockNotationConverters.ResolveForCulture(new("en-US"));
    [Benchmark]
    public void DefaultClockNotationConverter()
    {
        defaultConverter.Convert(time, ClockNotationRounding.None);
        defaultConverter.Convert(time, ClockNotationRounding.NearestFiveMinutes);
    }
}

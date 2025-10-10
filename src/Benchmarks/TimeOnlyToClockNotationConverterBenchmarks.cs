[MemoryDiagnoser(false)]
public class TimeOnlyToClockNotationConverterBenchmarks
{
    static readonly TimeOnly time = new(13, 6, 2, 88, 987);

    static readonly EsTimeOnlyToClockNotationConverter esConverter = new();
    [Benchmark]
    public void EsClockNotationConverter()
    {
        esConverter.Convert(time, ClockNotationRounding.None);
        esConverter.Convert(time, ClockNotationRounding.NearestFiveMinutes);
    }

    static readonly BrazilianPortugueseTimeOnlyToClockNotationConverter brazilianConverter = new();
    [Benchmark]
    public void BrazilianPortugueseClockNotationConverter()
    {
        brazilianConverter.Convert(time, ClockNotationRounding.None);
        brazilianConverter.Convert(time, ClockNotationRounding.NearestFiveMinutes);
    }

    static readonly FrTimeOnlyToClockNotationConverter frConverter = new();
    [Benchmark]
    public void FrClockNotationConverter()
    {
        frConverter.Convert(time, ClockNotationRounding.None);
        frConverter.Convert(time, ClockNotationRounding.NearestFiveMinutes);
    }

    static readonly LbTimeOnlyToClockNotationConverter lbConverter = new();
    [Benchmark]
    public void LbClockNotationConverter()
    {
        lbConverter.Convert(time, ClockNotationRounding.None);
        lbConverter.Convert(time, ClockNotationRounding.NearestFiveMinutes);
    }

    static readonly DefaultTimeOnlyToClockNotationConverter defaultConverter = new();
    [Benchmark]
    public void DefaultClockNotationConverter()
    {
        defaultConverter.Convert(time, ClockNotationRounding.None);
        defaultConverter.Convert(time, ClockNotationRounding.NearestFiveMinutes);
    }
}
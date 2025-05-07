[MemoryDiagnoser(false)]
public class TimeOnlyToClockNotationConverterBenchmarks
{
    static TimeOnly time = new(13, 6, 2, 88, 987);

    static EsTimeOnlyToClockNotationConverter esConverter = new();
    [Benchmark]
    public void EsClockNotationConverter()
    {
        esConverter.Convert(time, ClockNotationRounding.None, null);
        esConverter.Convert(time, ClockNotationRounding.NearestFiveMinutes, null);
    }

    static BrazilianPortugueseTimeOnlyToClockNotationConverter brazilianConverter = new();
    [Benchmark]
    public void BrazilianPortugueseClockNotationConverter()
    {
        brazilianConverter.Convert(time, ClockNotationRounding.None, null);
        brazilianConverter.Convert(time, ClockNotationRounding.NearestFiveMinutes, null);
    }

    static FrTimeOnlyToClockNotationConverter frConverter = new();
    [Benchmark]
    public void FrClockNotationConverter()
    {
        frConverter.Convert(time, ClockNotationRounding.None, null);
        frConverter.Convert(time, ClockNotationRounding.NearestFiveMinutes, null);
    }

    static LbTimeOnlyToClockNotationConverter lbConverter = new();
    [Benchmark]
    public void LbClockNotationConverter()
    {
        lbConverter.Convert(time, ClockNotationRounding.None, null);
        lbConverter.Convert(time, ClockNotationRounding.NearestFiveMinutes, null);
    }

    static DefaultTimeOnlyToClockNotationConverter defaultConverter = new();
    [Benchmark]
    public void DefaultClockNotationConverter()
    {
        defaultConverter.Convert(time, ClockNotationRounding.None, null);
        defaultConverter.Convert(time, ClockNotationRounding.NearestFiveMinutes, null);
    }
}
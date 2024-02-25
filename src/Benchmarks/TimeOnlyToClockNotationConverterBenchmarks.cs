[MemoryDiagnoser(false)]
public class TimeOnlyToClockNotationConverterBenchmarks
{
    static TimeOnly time = new(13, 6, 2, 88, 987);

    static EsTimeOnlyToClockNotationConverter esConverter = new();
    [Benchmark]
    public string EsClockNotationConverter() =>
        esConverter.Convert(time, ClockNotationRounding.None);

    static BrazilianPortugueseTimeOnlyToClockNotationConverter brazilianConverter = new();
    [Benchmark]
    public string BrazilianPortugueseClockNotationConverter() =>
        brazilianConverter.Convert(time, ClockNotationRounding.None);

    static FrTimeOnlyToClockNotationConverter frConverter = new();
    [Benchmark]
    public string FrClockNotationConverter() =>
        frConverter.Convert(time, ClockNotationRounding.None);

    static LbTimeOnlyToClockNotationConverter lbConverter = new();
    [Benchmark]
    public string LbClockNotationConverter() =>
        lbConverter.Convert(time, ClockNotationRounding.None);

    static DefaultTimeOnlyToClockNotationConverter defaultConverter = new();
    [Benchmark]
    public string DefaultClockNotationConverter() =>
        defaultConverter.Convert(time, ClockNotationRounding.None);
}
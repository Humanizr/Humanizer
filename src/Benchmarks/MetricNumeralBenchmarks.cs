namespace Benchmarks;

/// <summary>
/// Benchmarks for MetricNumeralExtensions - demonstrates FrozenDictionary performance
/// </summary>
[MemoryDiagnoser]
public class MetricNumeralBenchmarks
{
    [Benchmark(Description = "ToMetric small")]
    public string ToMetricSmall() =>
        123.ToMetric();

    [Benchmark(Description = "ToMetric kilo")]
    public string ToMetricKilo() =>
        1000.ToMetric();

    [Benchmark(Description = "ToMetric mega")]
    public string ToMetricMega() =>
        1000000.ToMetric();

    [Benchmark(Description = "ToMetric boundary")]
    public string ToMetricBoundary() =>
        999500.ToMetric();

    [Benchmark(Description = "ToMetric giga")]
    public string ToMetricGiga() =>
        int.MaxValue.ToMetric();

    [Benchmark(Description = "ToMetric formatted")]
    public string ToMetricFormatted() =>
        1230.ToMetric(MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseName, 2);

    [Benchmark(Description = "ToMetric milli")]
    public string ToMetricMilli() =>
        0.001.ToMetric();

    [Benchmark(Description = "FromMetric kilo")]
    public double FromMetricKilo() =>
        "1k".FromMetric();

    [Benchmark(Description = "FromMetric mega")]
    public double FromMetricMega() =>
        "5M".FromMetric();

    [Benchmark(Description = "FromMetric micro")]
    public double FromMetricMicro() =>
        "100μ".FromMetric();
}
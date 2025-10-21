using BenchmarkDotNet.Attributes;
using Humanizer;

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

using BenchmarkDotNet.Attributes;
using Humanizer;

namespace Benchmarks;

/// <summary>
/// Benchmarks for ToQuantity - demonstrates string.Create performance optimization
/// </summary>
[MemoryDiagnoser]
public class ToQuantityBenchmarks
{
    [Benchmark(Description = "ToQuantity Numeric - Plural")]
    public string ToQuantityNumericPlural() =>
        "request".ToQuantity(5);

    [Benchmark(Description = "ToQuantity Numeric - Singular")]
    public string ToQuantityNumericSingular() =>
        "request".ToQuantity(1);

    [Benchmark(Description = "ToQuantity Words")]
    public string ToQuantityWords() =>
        "process".ToQuantity(123, ShowQuantityAs.Words);

    [Benchmark(Description = "ToQuantity Double")]
    public string ToQuantityDouble() =>
        "item".ToQuantity(2.5);

    [Benchmark(Description = "ToQuantity Formatted")]
    public string ToQuantityFormatted() =>
        "request".ToQuantity(10000, "N0");
}

using BenchmarkDotNet.Attributes;
using Humanizer;
using System.Globalization;

namespace Benchmarks;

/// <summary>
/// Benchmarks for ordinal operations - demonstrates SearchValues and FrozenDictionary performance
/// </summary>
[MemoryDiagnoser]
public class OrdinalBenchmarks
{
    [Benchmark(Description = "English Ordinalize")]
    public string EnglishOrdinalize() =>
        42.Ordinalize();

    [Benchmark(Description = "Dutch Ordinalize")]
    public string DutchOrdinalize() =>
        42.Ordinalize(new CultureInfo("nl"));

    [Benchmark(Description = "Turkish Ordinalize")]
    public string TurkishOrdinalize() =>
        42.Ordinalize(new CultureInfo("tr"));

    [Benchmark(Description = "Greek Ordinalize")]
    public string GreekOrdinalize() =>
        42.Ordinalize(new CultureInfo("el"));

    [Benchmark(Description = "Finnish Ordinalize")]
    public string FinnishOrdinalize() =>
        42.Ordinalize(new CultureInfo("fi"));
}

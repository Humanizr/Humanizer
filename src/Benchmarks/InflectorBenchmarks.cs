using BenchmarkDotNet.Attributes;
using Humanizer;

namespace Benchmarks;

/// <summary>
/// Benchmarks for InflectorExtensions - demonstrates source-generated regex performance
/// </summary>
[MemoryDiagnoser]
public class InflectorBenchmarks
{
    [Benchmark(Description = "Pascalize")]
    public string Pascalize() =>
        "some_title for_an article".Pascalize();

    [Benchmark(Description = "Camelize")]
    public string Camelize() =>
        "some_title for_an article".Camelize();

    [Benchmark(Description = "Underscore")]
    public string Underscore() =>
        "SomeClassName".Underscore();

    [Benchmark(Description = "Dasherize")]
    public string Dasherize() =>
        "some_text_string".Dasherize();

    [Benchmark(Description = "Kebaberize")]
    public string Kebaberize() =>
        "PascalCaseString".Kebaberize();

    [Benchmark(Description = "Titleize")]
    public string Titleize() =>
        "some_title for_an article".Titleize();

    [Benchmark(Description = "Pluralize")]
    public string Pluralize() =>
        "person".Pluralize();

    [Benchmark(Description = "Singularize")]
    public string Singularize() =>
        "people".Singularize();
}

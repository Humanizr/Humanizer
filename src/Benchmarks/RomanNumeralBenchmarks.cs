using BenchmarkDotNet.Attributes;
using Humanizer;

namespace Benchmarks;

/// <summary>
/// Benchmarks for RomanNumeralExtensions - demonstrates source-generated regex performance
/// </summary>
[MemoryDiagnoser]
public class RomanNumeralBenchmarks
{
    [Benchmark(Description = "ToRoman small")]
    public string ToRomanSmall() =>
        42.ToRoman();

    [Benchmark(Description = "ToRoman medium")]
    public string ToRomanMedium() =>
        1987.ToRoman();

    [Benchmark(Description = "ToRoman large")]
    public string ToRomanLarge() =>
        3999.ToRoman();

    [Benchmark(Description = "FromRoman small")]
    public int FromRomanSmall() =>
        "XLII".FromRoman();

    [Benchmark(Description = "FromRoman medium")]
    public int FromRomanMedium() =>
        "MCMLXXXVII".FromRoman();

    [Benchmark(Description = "FromRoman large")]
    public int FromRomanLarge() =>
        "MMMCMXCIX".FromRoman();
}

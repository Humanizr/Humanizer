using BenchmarkDotNet.Attributes;
using Humanizer;
using System.Globalization;

namespace Benchmarks;

/// <summary>
/// Benchmarks for words-to-number conversions - demonstrates FrozenDictionary and source-generated regex
/// </summary>
[MemoryDiagnoser]
public class WordsToNumberBenchmarks
{
    static readonly CultureInfo EnglishCulture = new("en-US");

    [Benchmark(Description = "Simple word")]
    public int SimpleWord() =>
        "five".ToNumber(EnglishCulture);

    [Benchmark(Description = "Compound word")]
    public int CompoundWord() =>
        "forty-two".ToNumber(EnglishCulture);

    [Benchmark(Description = "Complex number")]
    public int ComplexNumber() =>
        "one thousand two hundred thirty-four".ToNumber(EnglishCulture);

    [Benchmark(Description = "Ordinal word")]
    public int OrdinalWord() =>
        "fifth".ToNumber(EnglishCulture);

    [Benchmark(Description = "Large number")]
    public int LargeNumber() =>
        "nine hundred ninety-nine million".ToNumber(EnglishCulture);
}

using BenchmarkDotNet.Attributes;
using Humanizer;

namespace Benchmarks;

/// <summary>
/// Benchmarks for number-to-words conversions - demonstrates FrozenDictionary performance
/// </summary>
[MemoryDiagnoser]
public class NumberToWordsBenchmarks
{
    [Benchmark(Description = "English ToWords small")]
    public string EnglishToWordsSmall() =>
        42.ToWords();

    [Benchmark(Description = "English ToWords large")]
    public string EnglishToWordsLarge() =>
        int.MaxValue.ToWords();

    [Benchmark(Description = "English ToOrdinalWords")]
    public string EnglishToOrdinalWords() =>
        42.ToOrdinalWords();

    [Benchmark(Description = "Turkish ToWords")]
    public string TurkishToWords() =>
        42.ToWords(new System.Globalization.CultureInfo("tr"));

    [Benchmark(Description = "Greek ToWords")]
    public string GreekToWords() =>
        42.ToWords(new System.Globalization.CultureInfo("el"));

    [Benchmark(Description = "Korean ToWords")]
    public string KoreanToWords() =>
        42.ToWords(new System.Globalization.CultureInfo("ko"));

    [Benchmark(Description = "Finnish ToWords")]
    public string FinnishToWords() =>
        42.ToWords(new System.Globalization.CultureInfo("fi"));
}

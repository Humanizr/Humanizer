namespace Benchmarks;

/// <summary>
/// Benchmarks for ordinal operations - demonstrates SearchValues and FrozenDictionary performance
/// </summary>
[MemoryDiagnoser]
public class OrdinalBenchmarks
{
    static readonly CultureInfo dutchCulture = new("nl");
    static readonly CultureInfo turkishCulture = new("tr");
    static readonly CultureInfo greekCulture = new("el");
    static readonly CultureInfo finnishCulture = new("fi");

    [Benchmark(Description = "English Ordinalize")]
    public string EnglishOrdinalize() =>
        42.Ordinalize();

    [Benchmark(Description = "Dutch Ordinalize")]
    public string DutchOrdinalize() =>
        42.Ordinalize(dutchCulture);

    [Benchmark(Description = "Turkish Ordinalize")]
    public string TurkishOrdinalize() =>
        42.Ordinalize(turkishCulture);

    [Benchmark(Description = "Greek Ordinalize")]
    public string GreekOrdinalize() =>
        42.Ordinalize(greekCulture);

    [Benchmark(Description = "Finnish Ordinalize")]
    public string FinnishOrdinalize() =>
        42.Ordinalize(finnishCulture);
}
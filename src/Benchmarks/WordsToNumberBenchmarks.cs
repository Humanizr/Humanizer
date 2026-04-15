namespace Benchmarks;

/// <summary>
/// Benchmarks for words-to-number conversions and generated profile resolution.
/// </summary>
[MemoryDiagnoser]
public class WordsToNumberBenchmarks
{
    static readonly CultureInfo EnglishCulture = new("en-US");
    static readonly CultureInfo PortugueseCulture = new("pt-PT");
    static readonly CultureInfo SpanishCulture = new("es-ES");

    [Benchmark(Description = "Simple word")]
    public int SimpleWord() =>
        (int)"five".ToNumber(EnglishCulture);

    [Benchmark(Description = "Compound word")]
    public int CompoundWord() =>
        (int)"forty-two".ToNumber(EnglishCulture);

    [Benchmark(Description = "Complex number")]
    public int ComplexNumber() =>
        (int)"one thousand two hundred thirty-four".ToNumber(EnglishCulture);

    [Benchmark(Description = "Steady-state small ordinal parse")]
    public int SmallOrdinalWord() =>
        (int)"vigésimo primero".ToNumber(SpanishCulture);

    [Benchmark(Description = "Steady-state scale ordinal parse")]
    public int ScaleOrdinalWord() =>
        (int)"milésimo".ToNumber(PortugueseCulture);

    [Benchmark(Description = "Steady-state glued ordinal parse")]
    public int GluedScaleOrdinalWord() =>
        (int)"dosmilésimo".ToNumber(SpanishCulture);

    [Benchmark(Description = "First-touch locale/profile resolution")]
    public string FirstTouchEnglishLocaleProfileResolution() =>
        Configurator.GetWordsToNumberConverter(EnglishCulture).GetType().FullName!;

    [Benchmark(Description = "First-touch non-English locale/profile resolution")]
    public string FirstTouchNonEnglishLocaleProfileResolution() =>
        Configurator.GetWordsToNumberConverter(SpanishCulture).GetType().FullName!;
}
using System.Globalization;
using BenchmarkDotNet.Attributes;

using Humanizer;

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
        "five".ToNumber(EnglishCulture);

    [Benchmark(Description = "Compound word")]
    public int CompoundWord() =>
        "forty-two".ToNumber(EnglishCulture);

    [Benchmark(Description = "Complex number")]
    public int ComplexNumber() =>
        "one thousand two hundred thirty-four".ToNumber(EnglishCulture);

    [Benchmark(Description = "Steady-state small ordinal parse")]
    public int SmallOrdinalWord() =>
        "vigésimo primero".ToNumber(SpanishCulture);

    [Benchmark(Description = "Steady-state scale ordinal parse")]
    public int ScaleOrdinalWord() =>
        "milésimo".ToNumber(PortugueseCulture);

    [Benchmark(Description = "Steady-state glued ordinal parse")]
    public int GluedScaleOrdinalWord() =>
        "dosmilésimo".ToNumber(SpanishCulture);

    [Benchmark(Description = "First-touch locale/profile resolution")]
    public string FirstTouchLocaleProfileResolution() =>
        Configurator.GetWordsToNumberConverter(SpanishCulture).GetType().FullName!;
}

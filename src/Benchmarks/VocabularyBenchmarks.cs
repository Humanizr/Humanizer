using BenchmarkDotNet.Attributes;
using Humanizer;

namespace Benchmarks;

/// <summary>
/// Benchmarks for Vocabulary operations - demonstrates source-generated regex performance
/// </summary>
[MemoryDiagnoser]
public class VocabularyBenchmarks
{
    readonly string[] testWords = 
    [
        "person", "man", "woman", "child", "tooth", "foot",
        "mouse", "goose", "sheep", "deer", "fish", "species",
        "series", "movie", "index", "matrix", "vertex", "ox"
    ];

    [Benchmark(Description = "Pluralize various words")]
    public void PluralizeBatch()
    {
        foreach (var word in testWords)
        {
            _ = word.Pluralize();
        }
    }

    [Benchmark(Description = "Singularize various words")]
    public void SingularizeBatch()
    {
        foreach (var word in testWords)
        {
            _ = word.Singularize();
        }
    }

    [Benchmark(Description = "Pluralize single common")]
    public string PluralizeCommon() =>
        "person".Pluralize();

    [Benchmark(Description = "Pluralize single irregular")]
    public string PluralizeIrregular() =>
        "mouse".Pluralize();

    [Benchmark(Description = "Singularize single common")]
    public string SingularizeCommon() =>
        "people".Singularize();

    [Benchmark(Description = "Singularize single irregular")]
    public string SingularizeIrregular() =>
        "mice".Singularize();
}

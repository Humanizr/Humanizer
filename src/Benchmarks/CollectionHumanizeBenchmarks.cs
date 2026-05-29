namespace Benchmarks;

/// <summary>
/// Benchmarks collection humanization paths with trimming, filtering, and formatter delegates.
/// </summary>
[MemoryDiagnoser]
public class CollectionHumanizeBenchmarks
{
    static readonly string[] strings = ["Alpha", " Beta ", "Gamma", "", "Delta"];
    static readonly int?[] nullableInts = [1, null, 3, 4];
    static readonly object?[] objects = ["Alpha", null, " Gamma ", 4];

    [Benchmark(Description = "Collection Humanize strings")]
    public string CollectionHumanizeStrings() =>
        strings.Humanize();

    [Benchmark(Description = "Collection Humanize string formatter")]
    public string CollectionHumanizeStringFormatter() =>
        nullableInts.Humanize(value => value?.ToString() ?? "");

    [Benchmark(Description = "Collection Humanize object formatter")]
    public string CollectionHumanizeObjectFormatter() =>
        objects.Humanize(value => value ?? "");
}
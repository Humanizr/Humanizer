namespace Benchmarks;

/// <summary>
/// Benchmarks common ByteSize formatting paths.
/// </summary>
[MemoryDiagnoser]
public class ByteSizeBenchmarks
{
    static readonly ByteSize size = ByteSize.FromMegabytes(10.501);
    static readonly CultureInfo frenchCulture = new("fr");

    [Benchmark(Description = "ByteSize ToString default")]
    public string ByteSizeToStringDefault() =>
        size.ToString();

    [Benchmark(Description = "ByteSize ToString culture")]
    public string ByteSizeToStringCulture() =>
        size.ToString(frenchCulture);

    [Benchmark(Description = "ByteSize ToString format")]
    public string ByteSizeToStringFormat() =>
        size.ToString("0.## MB", frenchCulture);

    [Benchmark(Description = "ByteSize ToFullWords")]
    public string ByteSizeToFullWords() =>
        size.ToFullWords(provider: frenchCulture);
}
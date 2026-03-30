[MemoryDiagnoser]
public class EnglishToWordsBenchmark
{
    readonly CultureInfo culture = new("en-US");

    [Benchmark]
    public string ToWords() =>
        int.MaxValue.ToWords(culture);

    [Benchmark]
    public string ToWordsOrdinal() =>
        int.MaxValue.ToOrdinalWords(culture);
}

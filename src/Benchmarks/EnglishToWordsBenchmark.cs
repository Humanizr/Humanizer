[MemoryDiagnoser]
public class EnglishToWordsBenchmark
{
    EnglishNumberToWordsConverter converter = new();

    [Benchmark]
    public string ToWords() =>
        converter.Convert(long.MaxValue);

    [Benchmark]
    public string ToWordsOrdinal() =>
        converter.ConvertToOrdinal(int.MaxValue);
}
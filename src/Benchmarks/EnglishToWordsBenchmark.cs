[MemoryDiagnoser]
public class EnglishToWordsBenchmark
{
    EnglishNumberToWordsConverter converter = new();

    [Benchmark]
    public string ToWords() =>
        converter.Convert(int.MaxValue);

    [Benchmark]
    public string ToWordsOrdinal() =>
        converter.ConvertToOrdinal(int.MaxValue);
}
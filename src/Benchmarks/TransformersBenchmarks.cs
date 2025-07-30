[MemoryDiagnoser(false)]
public class TransformersBenchmarks
{
    // hard-coded seed ensures the same random strings are generated each time.
    const int RAND_SEED = 17432;

    static readonly char[] _alphabet =
        Enumerable
            .Repeat((int) ' ', 12)
            .Concat(Enumerable.Range('a', 'z' - 'a'))
            .Concat(Enumerable.Range('A', 'Z' - 'A'))
            .Concat(Enumerable.Range('0', '9' - '0'))
            .Concat([
                '.',
                ',',
                '(',
                ')',
                '!',
                '$'
            ])
            .Select(x => (char) x)
            .ToArray();

    Random random = new(RAND_SEED);
    string input = null!;

    [Params(10, 100, 1000)]
    public int StringLen;

    [GlobalSetup]
    public void GlobalSetup()
    {
        var chars = new char[StringLen];
        for (var i = 0; i < StringLen; i++)
        {
            chars[i] = _alphabet[random.Next(0, _alphabet.Length)];
        }

        input = new(chars);
    }

    [Benchmark]
    public string AllTransforms() =>
        input.Transform(To.LowerCase, To.UpperCase, To.SentenceCase, To.TitleCase);

    [Benchmark]
    public string LowerCase() =>
        input.Transform(To.LowerCase);

    [Benchmark]
    public string UpperCase() =>
        input.Transform(To.UpperCase);

    [Benchmark]
    public string SentenceCase() =>
        input.Transform(To.SentenceCase);

    [Benchmark]
    public string TitleCase() =>
        input.Transform(To.TitleCase);
}
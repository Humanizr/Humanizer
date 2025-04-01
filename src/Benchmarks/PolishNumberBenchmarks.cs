using System.Globalization;

[MemoryDiagnoser(false)]
public class PolishNumberBenchmarks
{
    static CultureInfo culture = new("pl");
    [Benchmark(Description = "PolishNumber.ToWords")]
    public void ToWords()
    {
        foreach (var number in numbers)
        {
            number.ToWords(culture);
            number.ToWords(GrammaticalGender.Feminine, culture);
        }
    }

    static int[] numbers =
    [
        0,
        1,
        2,
        9,
        10,
        11,
        15,
        18,
        20,
        21,
        22,
        28,
        30,
        44,
        55,
        60,
        63,
        66,
        77,
        88,
        99,
        100,
        101,
        102,
        105,
        109,
        110,
        119,
        120,
        121,
        200,
        201,
        240,
        300,
        900,
        1000,
        1001,
        1002,
        1003,
        1009,
        1010,
        1021,
        2000,
        2001,
        3000,
        10000,
        10001,
        10121,
        100000,
        100001,
        1000000,
        1000001,
        1000002,
        2000000,
        10000000,
        100000000,
        1000000000,
        2000000000,
    ];
}
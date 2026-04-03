namespace uzCyrl;

[UseCulture("uz-Cyrl-UZ")]
public class WordsToNumberHighRangeTests
{
    [Theory]
    [InlineData(1_000_000_000_000L, "бир триллион")]
    [InlineData(1_000_000_000_001L, "бир триллион бир")]
    [InlineData(1_000_000_000_000_000L, "бир квадриллион")]
    [InlineData(1_000_000_000_000_000_000L, "бир квинтиллион")]
    public void HighRangeWordsRoundTrip(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords());
        Assert.Equal(number, expected.ToNumber(CultureInfo.CurrentCulture));
    }
}

namespace ruRU;

[UseCulture("ru-RU")]
public class WordsToNumberHighRangeTests
{
    [Theory]
    [InlineData(1_000_000_000_000_000_000L, "один квинтиллион")]
    [InlineData(1_000_000_000_000_000_001L, "один квинтиллион один")]
    public void HighRangeWordsRoundTrip(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords());
        Assert.Equal(number, expected.ToNumber(CultureInfo.CurrentCulture));
    }
}

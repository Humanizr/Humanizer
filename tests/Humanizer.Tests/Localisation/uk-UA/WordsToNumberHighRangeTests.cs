namespace ukUA;

[UseCulture("uk-UA")]
public class WordsToNumberHighRangeTests
{
    [Theory]
    [InlineData(1_000_000_000_000_000_000L, "один квінтильйон")]
    [InlineData(1_000_000_000_000_000_001L, "один квінтильйон один")]
    public void HighRangeWordsRoundTrip(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords());
        Assert.Equal(number, expected.ToNumber(CultureInfo.CurrentCulture));
    }
}

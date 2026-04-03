namespace mt;

[UseCulture("mt")]
public class WordsToNumberHighRangeTests
{
    [Theory]
    [InlineData(1_000_000_000_000L, "triljun")]
    [InlineData(1_000_000_000_001L, "triljun u wieħed")]
    [InlineData(1_000_000_000_000_000L, "kwadriljun")]
    [InlineData(1_000_000_000_000_000_000L, "kwintiljun")]
    public void HighRangeWordsRoundTrip(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords());
        Assert.Equal(number, expected.ToNumber(CultureInfo.CurrentCulture));
    }
}

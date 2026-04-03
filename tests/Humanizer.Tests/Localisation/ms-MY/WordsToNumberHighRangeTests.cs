namespace msMY;

[UseCulture("ms-MY")]
public class WordsToNumberHighRangeTests
{
    [Theory]
    [InlineData(1_000_000_000_000_000_000L, "satu kuintilion")]
    [InlineData(1_000_000_000_000_000_001L, "satu kuintilion satu")]
    public void HighRangeWordsRoundTrip(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords());
        Assert.Equal(number, expected.ToNumber(CultureInfo.CurrentCulture));
    }
}

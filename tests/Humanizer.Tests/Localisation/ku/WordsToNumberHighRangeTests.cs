namespace ku;

[UseCulture("ku")]
public class WordsToNumberHighRangeTests
{
    [Theory]
    [InlineData(1_000_000_000_000_000_000L, "یەک کوینتیلیۆن")]
    [InlineData(1_000_000_000_000_000_001L, "یەک کوینتیلیۆن و یەک")]
    public void HighRangeWordsRoundTrip(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords());
        Assert.Equal(number, expected.ToNumber(CultureInfo.CurrentCulture));
    }
}

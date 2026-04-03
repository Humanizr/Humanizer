namespace filPH;

[UseCulture("fil-PH")]
public class WordsToNumberHighRangeTests
{
    [Theory]
    [InlineData(1_000_000_000_000_000_000L, "isang kwintilyon")]
    [InlineData(1_000_000_000_000_000_001L, "isang kwintilyon isa")]
    public void HighRangeWordsRoundTrip(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords());
        Assert.Equal(number, expected.ToNumber(CultureInfo.CurrentCulture));
    }
}

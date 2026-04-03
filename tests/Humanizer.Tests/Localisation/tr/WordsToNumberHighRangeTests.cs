namespace tr;

[UseCulture("tr")]
public class WordsToNumberHighRangeTests
{
    [Theory]
    [InlineData(1_000_000_000_000_000_000L, "bir kentilyon")]
    [InlineData(1_000_000_000_000_000_001L, "bir kentilyon bir")]
    public void HighRangeWordsRoundTrip(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords());
        Assert.Equal(number, expected.ToNumber(CultureInfo.CurrentCulture));
    }
}

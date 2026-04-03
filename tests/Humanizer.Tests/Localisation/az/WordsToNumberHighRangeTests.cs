namespace az;

[UseCulture("az")]
public class WordsToNumberHighRangeTests
{
    [Theory]
    [InlineData(1_000_000_000_000_000_000L, "bir kvintilyon")]
    [InlineData(1_000_000_000_000_000_001L, "bir kvintilyon bir")]
    public void HighRangeWordsRoundTrip(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords());
        Assert.Equal(number, expected.ToNumber(CultureInfo.CurrentCulture));
    }
}

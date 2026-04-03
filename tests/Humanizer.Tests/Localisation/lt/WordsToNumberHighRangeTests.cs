namespace lt;

[UseCulture("lt-LT")]
public class WordsToNumberHighRangeTests
{
    [Theory]
    [InlineData(1_000_000_000_000_000_000L, "kvintilijonas")]
    [InlineData(1_000_000_000_000_000_001L, "kvintilijonas vienas")]
    public void HighRangeWordsRoundTrip(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords());
        Assert.Equal(number, expected.ToNumber(CultureInfo.CurrentCulture));
    }
}

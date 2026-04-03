namespace fa;

[UseCulture("fa")]
public class WordsToNumberHighRangeTests
{
    [Theory]
    [InlineData(1_000_000_000_000_000_000L, "یک تریلیون")]
    [InlineData(1_000_000_000_000_000_001L, "یک تریلیون و یک")]
    public void HighRangeWordsRoundTrip(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords());
        Assert.Equal(number, expected.ToNumber(CultureInfo.CurrentCulture));
    }
}

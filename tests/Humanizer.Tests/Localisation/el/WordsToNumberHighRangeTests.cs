namespace el;

[UseCulture("el")]
public class WordsToNumberHighRangeTests
{
    [Theory]
    [InlineData(1_000_000_000_000L, "ένα τρισεκατομμύριο")]
    [InlineData(1_000_000_000_001L, "ένα τρισεκατομμύριο ένα")]
    [InlineData(1_000_000_000_000_000L, "ένα τετράκις εκατομμύριο")]
    [InlineData(1_000_000_000_000_000_000L, "ένα πεντάκις εκατομμύριο")]
    public void HighRangeWordsRoundTrip(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords());
        Assert.Equal(number, expected.ToNumber(CultureInfo.CurrentCulture));
    }
}

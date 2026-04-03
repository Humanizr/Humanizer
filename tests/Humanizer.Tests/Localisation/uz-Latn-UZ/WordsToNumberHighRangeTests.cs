namespace uzLatn;

[UseCulture("uz-Latn-UZ")]
public class WordsToNumberHighRangeTests
{
    [Theory]
    [InlineData(1_000_000_000_000L, "bir trillion")]
    [InlineData(1_000_000_000_001L, "bir trillion bir")]
    [InlineData(1_000_000_000_000_000L, "bir kvadrillion")]
    [InlineData(1_000_000_000_000_000_000L, "bir kvintillion")]
    public void HighRangeWordsRoundTrip(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords());
        Assert.Equal(number, expected.ToNumber(CultureInfo.CurrentCulture));
    }
}

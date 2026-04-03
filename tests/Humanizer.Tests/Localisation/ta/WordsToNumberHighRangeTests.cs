namespace ta;

[UseCulture("ta")]
public class WordsToNumberHighRangeTests
{
    [Theory]
    [InlineData(1_001_000_001L, "நூறு கோடியே பத்து இலட்சத்து ஒன்று")]
    [InlineData(4_325_010_007_018L, "ஒரு கோடியே ஏழாயிரத்து பதினெட்டு")]
    [InlineData(1_000_000_000_000_000L, "ஒன்று சங்கம்")]
    [InlineData(1_000_000_000_000_000_000L, "ஒன்று அர்த்தம்")]
    public void HighRangeWordsRoundTrip(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords());
        Assert.Equal(number, expected.ToNumber(CultureInfo.CurrentCulture));
    }
}

namespace Humanizer.Tests.Localisation.hi;

[UseCulture("hi")]
public class HindiWordsToNumberTests
{
    static readonly CultureInfo Hi = new("hi");

    [Theory]
    [InlineData("दस हज़ार खरब", 1000000000000000)]
    public void WordsToNumber_MultipliesSmallerScaleByLargerScale(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Hi));
        Assert.True(words.TryToNumber(out var parsed, Hi, out var unrecognizedWord));
        Assert.Equal(expected, parsed);
        Assert.Null(unrecognizedWord);
    }

    [Theory]
    [InlineData(1000000000000000)]
    [InlineData(996259922455452055)]
    [InlineData(long.MaxValue)]
    public void WordsToNumber_RoundTripsLargeCardinals(long number) =>
        Assert.Equal(number, number.ToWords(Hi).ToNumber(Hi));
}

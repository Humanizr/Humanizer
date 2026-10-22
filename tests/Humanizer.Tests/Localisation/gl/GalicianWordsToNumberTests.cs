namespace Humanizer.Tests.Localisation.gl;

[UseCulture("gl")]
public class GalicianWordsToNumberTests
{
    static readonly CultureInfo Gl = new("gl");

    [Theory]
    [InlineData("mil billóns", 1000000000000000)]
    public void WordsToNumber_MultipliesSmallerScaleByLargerScale(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Gl));
        Assert.True(words.TryToNumber(out var parsed, Gl, out var unrecognizedWord));
        Assert.Equal(expected, parsed);
        Assert.Null(unrecognizedWord);
    }

    [Theory]
    [InlineData(1000000000)]
    [InlineData(1000000000000000)]
    [InlineData(996259922455452055)]
    [InlineData(long.MaxValue)]
    public void WordsToNumber_RoundTripsLargeCardinals(long number) =>
        Assert.Equal(number, number.ToWords(Gl).ToNumber(Gl));
}

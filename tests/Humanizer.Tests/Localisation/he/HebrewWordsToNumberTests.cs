namespace Humanizer.Tests.Localisation.he;

[UseCulture("he")]
public class HebrewWordsToNumberTests
{
    static readonly CultureInfo He = new("he");

    [Theory]
    [InlineData("אלף מיליארד", 1000000000000)]
    public void WordsToNumber_MultipliesSmallerScaleByLargerScale(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(He));
        Assert.True(words.TryToNumber(out var parsed, He, out var unrecognizedWord));
        Assert.Equal(expected, parsed);
        Assert.Null(unrecognizedWord);
    }

    [Theory]
    [InlineData(1000000000000)]
    [InlineData(1000000000000000)]
    [InlineData(996259922455452055)]
    public void WordsToNumber_RoundTripsLargeCardinals(long number) =>
        Assert.Equal(number, number.ToWords(He).ToNumber(He));
}

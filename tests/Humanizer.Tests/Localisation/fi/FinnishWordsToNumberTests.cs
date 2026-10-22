namespace Humanizer.Tests.Localisation.fi;

[UseCulture("fi")]
public class FinnishWordsToNumberTests
{
    static readonly CultureInfo Fi = new("fi");

    [Theory]
    [InlineData("neljä", 4)]
    [InlineData("seitsemäntoista", 17)]
    [InlineData("yhdeksänkymmentäyhdeksän", 99)]
    [InlineData("neljäsataaneljäkymmentäneljä", 444)]
    [InlineData("kaksikymmentä", 20)]
    public void WordsToNumber_ParsesWordsWithDiacritics(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Fi));
        Assert.True(words.TryToNumber(out var parsed, Fi, out var unrecognizedWord));
        Assert.Equal(expected, parsed);
        Assert.Null(unrecognizedWord);
    }

    [Theory]
    [InlineData(4)]
    [InlineData(97)]
    [InlineData(1974)]
    [InlineData(2147483647)]
    [InlineData(long.MaxValue)]
    public void WordsToNumber_RoundTripsCardinals(long number) =>
        Assert.Equal(number, number.ToWords(Fi).ToNumber(Fi));
}

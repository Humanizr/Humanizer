namespace Humanizer.Tests.Localisation.ms;

[UseCulture("ms")]
public class MalayWordsToNumberTests
{
    static readonly CultureInfo Ms = new("ms");

    [Theory]
    [InlineData("seratus dua puluh satu", 121)]
    [InlineData("sembilan ratus sembilan puluh sembilan", 999)]
    [InlineData("seratus dua puluh tiga juta empat ratus lima puluh enam ribu tujuh ratus lapan puluh sembilan", 123456789)]
    public void WordsToNumber_ParsesTensAfterHundreds(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Ms));
        Assert.True(words.TryToNumber(out var parsed, Ms, out var unrecognizedWord));
        Assert.Equal(expected, parsed);
        Assert.Null(unrecognizedWord);
    }

    [Theory]
    [InlineData(120)]
    [InlineData(999)]
    [InlineData(2147483647)]
    [InlineData(long.MaxValue)]
    [InlineData(-123456789)]
    public void WordsToNumber_RoundTripsCardinals(long number) =>
        Assert.Equal(number, number.ToWords(Ms).ToNumber(Ms));
}

namespace Humanizer.Tests;

[UseCulture("en-US")]
public class WordsToDecimalNumberTests_US
{
    [Theory]
    [InlineData("one point two", 1.2)]
    [InlineData("three point one four", 3.14)]
    [InlineData("zero point five", 0.5)]
    [InlineData("minus two point five", -2.5)]
    [InlineData("one hundred point two five", 100.25)]
    [InlineData("point five", 0.5)]
    public void ParsesEnglishDecimalPhrases(string words, decimal expected) =>
        Assert.Equal(expected, words.ToDecimalNumber(CultureInfo.CurrentCulture));

    [Fact]
    public void TryToDecimalNumberReportsUnrecognizedFractionToken()
    {
        Assert.False("three point mystery".TryToDecimalNumber(
            out var parsedNumber,
            CultureInfo.CurrentCulture,
            out var unrecognizedWord));
        Assert.Equal(0m, parsedNumber);
        Assert.Equal("mystery", unrecognizedWord);
    }

    [Fact]
    public void TryToDecimalNumberFailsWhenFractionExceedsDecimalPrecision()
    {
        var words = "zero point " + string.Join(" ", Enumerable.Repeat("one", 30));

        Assert.False(words.TryToDecimalNumber(
            out var parsedNumber,
            CultureInfo.CurrentCulture,
            out var unrecognizedWord));
        Assert.Equal(0m, parsedNumber);
        Assert.Equal("one", unrecognizedWord);
    }

    [Fact]
    public void IntegerToNumberDoesNotParseDecimalPhrases()
    {
        Assert.False("one point two".TryToNumber(out _, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal("point", unrecognizedWord);
    }
}

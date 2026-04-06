using System.Globalization;

namespace Humanizer.Tests;

[UseCulture("en-US")]
public class WordsToNumberLongTests
{
    [Fact]
    public void ToNumber_ParsesValuesBeyondInt32Range()
    {
        const long expected = 3000000000L;

        Assert.Equal(expected, "three billion".ToNumber(CultureInfo.CurrentCulture));
    }

    [Fact]
    public void TryToNumber_ParsesValuesBeyondInt32Range()
    {
        const long expected = 3000000000L;

        Assert.True("three billion".TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expected, parsedNumber);
        Assert.Null(unrecognizedWord);
    }

    [Theory]
    [InlineData("one quadrillion", 1000000000000000L)]
    [InlineData("one quintillion", 1000000000000000000L)]
    public void ToNumber_ParsesHighRangeEnglishValues(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(CultureInfo.CurrentCulture));
    }

    [Theory]
    [InlineData("one quadrillion", 1000000000000000L)]
    [InlineData("one quintillion", 1000000000000000000L)]
    public void TryToNumber_ParsesHighRangeEnglishValues(string words, long expected)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expected, parsedNumber);
        Assert.Null(unrecognizedWord);
    }
}

[UseCulture("en-GB")]
public class WordsToNumberLongTests_GB
{
    [Theory]
    [InlineData("one quadrillion", 1000000000000000L)]
    [InlineData("one quintillion", 1000000000000000000L)]
    public void ToNumber_ParsesHighRangeEnglishValues_GB(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(CultureInfo.CurrentCulture));
    }

    [Theory]
    [InlineData("one quadrillion", 1000000000000000L)]
    [InlineData("one quintillion", 1000000000000000000L)]
    public void TryToNumber_ParsesHighRangeEnglishValues_GB(string words, long expected)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(expected, parsedNumber);
        Assert.Null(unrecognizedWord);
    }
}
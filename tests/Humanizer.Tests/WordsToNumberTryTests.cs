namespace Humanizer.Tests;

/// <summary>
/// Covers the <see cref="WordsToNumberExtension.TryToNumber(string, out long, CultureInfo)"/>
/// two-parameter overload (without unrecognizedWord out param).
/// </summary>
[UseCulture("en-US")]
public class WordsToNumberTryTests
{
    [Theory]
    [InlineData("zero", 0)]
    [InlineData("one", 1)]
    [InlineData("eleven", 11)]
    [InlineData("ninety five", 95)]
    [InlineData("one hundred ninety six", 196)]
    [InlineData("one million two hundred thirty four thousand five hundred sixty seven", 1234567)]
    [InlineData("three billion", 3000000000L)]
    public void TryToNumber_TwoParamOverload_ValidInput_ReturnsTrue(string words, long expected)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture));
        Assert.Equal(expected, parsedNumber);
    }

    [Theory]
    [InlineData("invalidinput")]
    [InlineData("twenty sveen")]
    [InlineData("tenn")]
    public void TryToNumber_TwoParamOverload_InvalidInput_ReturnsFalse(string words)
    {
        Assert.False(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture));
        Assert.Equal(0, parsedNumber);
    }
}
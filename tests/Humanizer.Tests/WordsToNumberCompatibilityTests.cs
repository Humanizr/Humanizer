using System.Globalization;

namespace Humanizer.Tests;

[UseCulture("en-US")]
public class WordsToNumberCompatibilityTests
{
    [Fact]
    public void ToNumber_ReturnsLongValues()
    {
        Assert.Equal(105L, "one hundred five".ToNumber(CultureInfo.CurrentCulture));
    }

    [Fact]
    public void TryToNumber_LongOverload_ReturnsParsedValueWithinIntRange()
    {
        Assert.True("one hundred five".TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(105L, parsedNumber);
        Assert.Null(unrecognizedWord);
    }

    [Fact]
    public void WordsToNumberConverterContract_UsesLongValues()
    {
        IWordsToNumberConverter converter = new LongWordsToNumberConverter();

        Assert.Equal(3000000000L, converter.Convert("three billion"));
        Assert.True(converter.TryConvert("three billion", out var parsedValue, out var unrecognizedWord));
        Assert.Equal(3000000000L, parsedValue);
        Assert.Null(unrecognizedWord);
    }

    sealed class LongWordsToNumberConverter : IWordsToNumberConverter
    {
        public bool TryConvert(string words, out long parsedValue)
        {
            parsedValue = 3000000000L;
            return true;
        }

        public bool TryConvert(string words, out long parsedValue, out string? unrecognizedNumber)
        {
            parsedValue = 3000000000L;
            unrecognizedNumber = null;
            return true;
        }

        public long Convert(string words) => 3000000000L;
    }
}


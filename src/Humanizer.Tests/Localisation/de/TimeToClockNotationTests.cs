#if NET6_0_OR_GREATER

namespace de;

[UseCulture("de")]
public class TimeToClockNotationTests
{
    [Theory]
    [InlineData(00, 00, "zwölf Uhr nachts")]
    [InlineData(04, 00, "vier Uhr")]
    [InlineData(05, 01, "fünf Uhr eins")]
    [InlineData(06, 05, "fünf nach sechs")]
    [InlineData(07, 10, "zehn nach sieben")]
    [InlineData(08, 15, "viertel nach acht")]
    [InlineData(09, 20, "zwanzig nach neun")]
    [InlineData(10, 25, "fünf vor halb elf")]
    [InlineData(11, 30, "halb zwölf")]
    [InlineData(12, 00, "zwölf Uhr mittags")]
    [InlineData(15, 35, "fünf nach halb vier")]
    [InlineData(16, 40, "zwanzig vor fünf")]
    [InlineData(17, 45, "viertel vor sechs")]
    [InlineData(18, 50, "zehn vor sieben")]
    [InlineData(19, 55, "fünf vor acht")]
    [InlineData(20, 59, "acht Uhr neunundfünfzig")]
    public void ConvertToClockNotationTimeOnlyStringDe(int hours, int minutes, string expectedResult)
    {
        var actualResult = new TimeOnly(hours, minutes).ToClockNotation();
        Assert.Equal(expectedResult, actualResult);
    }

    [Theory]
    [InlineData(00, 00, "zwölf Uhr nachts")]
    [InlineData(04, 00, "vier Uhr")]
    [InlineData(05, 01, "fünf Uhr")]
    [InlineData(06, 05, "fünf nach sechs")]
    [InlineData(07, 10, "zehn nach sieben")]
    [InlineData(08, 15, "viertel nach acht")]
    [InlineData(09, 20, "zwanzig nach neun")]
    [InlineData(10, 25, "fünf vor halb elf")]
    [InlineData(11, 30, "halb zwölf")]
    [InlineData(12, 00, "zwölf Uhr mittags")]
    [InlineData(13, 23, "fünf vor halb zwei")]
    [InlineData(14, 32, "halb drei")]
    [InlineData(15, 35, "fünf nach halb vier")]
    [InlineData(16, 40, "zwanzig vor fünf")]
    [InlineData(17, 45, "viertel vor sechs")]
    [InlineData(18, 50, "zehn vor sieben")]
    [InlineData(19, 55, "fünf vor acht")]
    [InlineData(20, 59, "neun Uhr")]
    public void ConvertToRoundedClockNotationTimeOnlyStringDe(int hours, int minutes, string expectedResult)
    {
        var actualResult = new TimeOnly(hours, minutes).ToClockNotation(ClockNotationRounding.NearestFiveMinutes);
        Assert.Equal(expectedResult, actualResult);
    }
}

#endif

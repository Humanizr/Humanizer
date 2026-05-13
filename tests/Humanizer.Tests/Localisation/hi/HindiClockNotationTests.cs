namespace Humanizer.Tests.Localisation.hi;

#if NET6_0_OR_GREATER
[UseCulture("hi")]
public class HindiClockNotationTests
{
    [Theory]
    [InlineData(1, 0, "सुबह एक बजे")]
    [InlineData(1, 5, "सुबह एक बजकर पाँच मिनट")]
    [InlineData(7, 5, "सुबह सात बजकर पाँच मिनट")]
    [InlineData(12, 0, "दोपहर बारह बजे")]
    [InlineData(12, 30, "दोपहर बारह बजकर तीस मिनट")]
    [InlineData(13, 0, "दोपहर एक बजे")]
    [InlineData(13, 23, "दोपहर एक बजकर तेईस मिनट")]
    [InlineData(18, 0, "शाम छह बजे")]
    [InlineData(20, 0, "शाम आठ बजे")]
    [InlineData(21, 0, "रात नौ बजे")]
    [InlineData(23, 40, "रात ग्यारह बजकर चालीस मिनट")]
    public void ToClockNotation_ExactOutput(int hours, int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation());
    }

    [Theory]
    [InlineData(13, 23, "दोपहर एक बजकर पच्चीस मिनट")]
    public void ToClockNotation_Rounded_ExactOutput(int hours, int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation(ClockNotationRounding.NearestFiveMinutes));
    }
}
#endif
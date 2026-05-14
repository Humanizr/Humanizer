namespace Humanizer.Tests.Localisation.ne;

#if NET6_0_OR_GREATER
[UseCulture("ne")]
public class NepaliClockNotationTests
{
    [Theory]
    [InlineData(1, 0, "राति एक बजे")]
    [InlineData(1, 5, "राति एक बजेर पाँच मिनेट")]
    [InlineData(7, 5, "बिहान सात बजेर पाँच मिनेट")]
    [InlineData(12, 0, "दिउँसो बाह्र बजे")]
    [InlineData(12, 30, "दिउँसो बाह्र बजेर तिस मिनेट")]
    [InlineData(13, 0, "दिउँसो एक बजे")]
    [InlineData(13, 23, "दिउँसो एक बजेर तेइस मिनेट")]
    [InlineData(18, 0, "साँझ छ बजे")]
    [InlineData(20, 0, "साँझ आठ बजे")]
    [InlineData(21, 0, "राति नौ बजे")]
    [InlineData(23, 40, "राति एघार बजेर चालिस मिनेट")]
    public void ToClockNotation_ExactOutput(int hours, int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation());
    }

    [Theory]
    [InlineData(13, 23, "दिउँसो एक बजेर पच्चिस मिनेट")]
    public void ToClockNotation_Rounded_ExactOutput(int hours, int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation(ClockNotationRounding.NearestFiveMinutes));
    }
}
#endif
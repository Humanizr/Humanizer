namespace Humanizer.Tests.Localisation.ha;

#if NET6_0_OR_GREATER
[UseCulture("ha")]
public class HausaClockNotationTests
{
    [Theory]
    [InlineData(1, 0, "ƙarfe ɗaya na safe")]
    [InlineData(1, 5, "ƙarfe ɗaya da minti biyar na safe")]
    [InlineData(13, 23, "ƙarfe ɗaya da minti ashirin da uku na rana")]
    [InlineData(18, 0, "ƙarfe shida na yamma")]
    [InlineData(20, 0, "ƙarfe takwas na yamma")]
    [InlineData(21, 0, "ƙarfe tara na dare")]
    [InlineData(23, 40, "ƙarfe goma sha ɗaya da minti arba'in na dare")]
    public void ToClockNotation_ExactOutput(int hours, int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation());
    }

    [Fact]
    public void ToClockNotation_Rounded_ExactOutput()
    {
        Assert.Equal("ƙarfe ɗaya da minti ashirin da biyar na rana", new TimeOnly(13, 23).ToClockNotation(ClockNotationRounding.NearestFiveMinutes));
    }
}
#endif
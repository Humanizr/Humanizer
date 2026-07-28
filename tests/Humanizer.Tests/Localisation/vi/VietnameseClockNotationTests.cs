namespace Humanizer.Tests.Localisation.vi;

#if NET6_0_OR_GREATER
[UseCulture("vi")]
public class VietnameseClockNotationTests
{
    [Theory]
    [InlineData(0, "mười hai giờ tối")]
    [InlineData(1, "một giờ sáng")]
    [InlineData(5, "năm giờ sáng")]
    [InlineData(6, "sáu giờ sáng")]
    [InlineData(11, "mười một giờ sáng")]
    [InlineData(12, "mười hai giờ chiều")]
    [InlineData(20, "tám giờ chiều")]
    [InlineData(21, "chín giờ tối")]
    public void ToClockNotation_UsesDayPeriodBoundaries(int hour, string expected) =>
        Assert.Equal(expected, new TimeOnly(hour, 0).ToClockNotation());
}
#endif
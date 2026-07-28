namespace Humanizer.Tests.Localisation.bn;

#if NET6_0_OR_GREATER
[UseCulture("bn")]
public class BengaliClockNotationTests
{
    [Theory]
    [InlineData(0, 59, "রাত বারো উনষাট")]
    [InlineData(1, 0, "রাত একটা")]
    [InlineData(5, 59, "রাত পাঁচ উনষাট")]
    [InlineData(6, 0, "সকাল ছয়")]
    [InlineData(11, 59, "সকাল এগারো উনষাট")]
    [InlineData(12, 0, "দুপুর বারো")]
    [InlineData(20, 59, "দুপুর আট উনষাট")]
    [InlineData(21, 0, "রাত নয়")]
    public void ToClockNotation_UsesAuthoredDayPeriodsAtBoundaries(int hour, int minute, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hour, minute).ToClockNotation());
    }
}
#endif
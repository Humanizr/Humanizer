namespace Humanizer.Tests.Localisation.ar;

#if NET6_0_OR_GREATER
[UseCulture("ar")]
public class ArabicClockNotationTests
{
    [Theory]
    [InlineData(0, "الثانية عشرة مساءً")]
    [InlineData(20, "الثامنة بعد الظهر")]
    [InlineData(21, "التاسعة مساءً")]
    public void ToClockNotation_UsesDayPeriodBoundaries(int hour, string expected) =>
        Assert.Equal(expected, new TimeOnly(hour, 0).ToClockNotation());

    [Theory]
    [InlineData(1, "الواحدة صباحًا")]
    [InlineData(2, "الثانية صباحًا")]
    [InlineData(3, "الثالثة صباحًا")]
    [InlineData(4, "الرابعة صباحًا")]
    [InlineData(5, "الخامسة صباحًا")]
    [InlineData(6, "السادسة صباحًا")]
    [InlineData(7, "السابعة صباحًا")]
    [InlineData(8, "الثامنة صباحًا")]
    [InlineData(9, "التاسعة صباحًا")]
    [InlineData(10, "العاشرة صباحًا")]
    [InlineData(11, "الحادية عشرة صباحًا")]
    [InlineData(12, "الثانية عشرة بعد الظهر")]
    public void ToClockNotation_UsesEveryMappedHour(int hour, string expected) =>
        Assert.Equal(expected, new TimeOnly(hour, 0).ToClockNotation());
}
#endif
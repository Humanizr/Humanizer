namespace Humanizer.Tests.Localisation.ur;

#if NET6_0_OR_GREATER
[UseCulture("ur")]
public class UrduClockNotationTests
{
    [Theory]
    [InlineData(1, 0, "ایک بجے صبح سویرے")]
    [InlineData(7, 5, "سات بج کر پانچ منٹ صبح")]
    [InlineData(12, 0, "بارہ بجے دوپہر")]
    [InlineData(12, 30, "بارہ بج کر تیس منٹ دوپہر")]
    [InlineData(13, 23, "ایک بج کر تئیس منٹ دوپہر")]
    [InlineData(1, 5, "ایک بج کر پانچ منٹ صبح سویرے")]
    public void ToClockNotation_ExactOutput(int hours, int minutes, string expected)
    {
        var time = new TimeOnly(hours, minutes);
        var result = time.ToClockNotation();
        Assert.Equal(expected, result);
        UrduBidiControlSweep.AssertNoBidiControls(result);
    }

    [Theory]
    [InlineData(13, 23, "ایک بج کر پچیس منٹ دوپہر")]
    public void ToClockNotation_Rounded_ExactOutput(int hours, int minutes, string expected)
    {
        var time = new TimeOnly(hours, minutes);
        var result = time.ToClockNotation(ClockNotationRounding.NearestFiveMinutes);
        Assert.Equal(expected, result);
        UrduBidiControlSweep.AssertNoBidiControls(result);
    }

    [Theory]
    [InlineData(1, 5, "ایک بج کر پانچ منٹ صبح سویرے")]
    public void UrPk_InheritsClockOutput(int hours, int minutes, string expected)
    {
        using var _ = new CultureSwap(new CultureInfo("ur-PK"));
        var time = new TimeOnly(hours, minutes);
        var result = time.ToClockNotation();
        Assert.Equal(expected, result);
        UrduBidiControlSweep.AssertNoBidiControls(result);
    }

    [Theory]
    [InlineData(1, 5, "ایک بج کر پانچ منٹ صبح سویرے")]
    public void UrIn_InheritsClockOutput(int hours, int minutes, string expected)
    {
        using var _ = new CultureSwap(new CultureInfo("ur-IN"));
        var time = new TimeOnly(hours, minutes);
        var result = time.ToClockNotation();
        Assert.Equal(expected, result);
        UrduBidiControlSweep.AssertNoBidiControls(result);
    }
}
#endif
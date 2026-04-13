namespace Humanizer.Tests.Localisation.ur;

[UseCulture("ur")]
public class UrduCompassTests
{
    [Theory]
    [InlineData(0.0, "شمال")]
    [InlineData(90.0, "مشرق")]
    [InlineData(180.0, "جنوب")]
    [InlineData(270.0, "مغرب")]
    public void CardinalDirections(double angle, string expected)
    {
        var result = angle.ToHeading();
        Assert.Equal(expected, result);
        UrduBidiControlSweep.AssertNoBidiControls(result);
    }

    [Theory]
    [InlineData(45.0, "شمال مشرق")]
    [InlineData(135.0, "جنوب مشرق")]
    public void OrdinalDirections(double angle, string expected)
    {
        var result = angle.ToHeading();
        Assert.Equal(expected, result);
        UrduBidiControlSweep.AssertNoBidiControls(result);
    }
}
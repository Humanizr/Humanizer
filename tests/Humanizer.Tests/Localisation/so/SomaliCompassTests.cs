namespace Humanizer.Tests.Localisation.so;

[UseCulture("so")]
public class SomaliCompassTests
{
    [Theory]
    [InlineData(0.0, "waqooyi")]
    [InlineData(45.0, "waqooyi bari")]
    [InlineData(90.0, "bari")]
    [InlineData(135.0, "koonfur bari")]
    [InlineData(180.0, "koonfur")]
    [InlineData(225.0, "koonfur galbeed")]
    [InlineData(270.0, "galbeed")]
    [InlineData(315.0, "waqooyi galbeed")]
    public void FullDirections(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Full));
    }

    [Theory]
    [InlineData(0.0, "W")]
    [InlineData(45.0, "WB")]
    [InlineData(90.0, "B")]
    [InlineData(180.0, "K")]
    [InlineData(270.0, "G")]
    public void AbbreviatedDirections(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Abbreviated));
    }
}
namespace Humanizer.Tests.Localisation.sw;

[UseCulture("sw")]
public class SwahiliCompassTests
{
    [Theory]
    [InlineData(0.0, "kaskazini")]
    [InlineData(45.0, "kaskazini mashariki")]
    [InlineData(90.0, "mashariki")]
    [InlineData(135.0, "kusini mashariki")]
    [InlineData(180.0, "kusini")]
    [InlineData(225.0, "kusini magharibi")]
    [InlineData(270.0, "magharibi")]
    [InlineData(315.0, "kaskazini magharibi")]
    public void FullDirections(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Full));
    }

    [Theory]
    [InlineData(0.0, "K")]
    [InlineData(45.0, "KM")]
    [InlineData(90.0, "M")]
    [InlineData(180.0, "S")]
    [InlineData(270.0, "G")]
    public void AbbreviatedDirections(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Abbreviated));
    }
}
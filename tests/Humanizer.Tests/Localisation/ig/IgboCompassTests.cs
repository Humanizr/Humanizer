namespace Humanizer.Tests.Localisation.ig;

[UseCulture("ig")]
public class IgboCompassTests
{
    [Theory]
    [InlineData(0.0, "ugwu")]
    [InlineData(45.0, "ugwu ọwụwa anyanwụ")]
    [InlineData(90.0, "ọwụwa anyanwụ")]
    [InlineData(135.0, "ndịda ọwụwa anyanwụ")]
    [InlineData(180.0, "ndịda")]
    [InlineData(225.0, "ndịda ọdịda anyanwụ")]
    [InlineData(270.0, "ọdịda anyanwụ")]
    [InlineData(315.0, "ugwu ọdịda anyanwụ")]
    public void FullDirections(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Full));
    }

    [Theory]
    [InlineData(0.0, "U")]
    [InlineData(90.0, "Ọ")]
    [InlineData(180.0, "N")]
    [InlineData(270.0, "Ọd")]
    public void AbbreviatedDirections(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Abbreviated));
    }
}
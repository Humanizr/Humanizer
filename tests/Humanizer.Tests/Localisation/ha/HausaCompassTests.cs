namespace Humanizer.Tests.Localisation.ha;

[UseCulture("ha")]
public class HausaCompassTests
{
    [Theory]
    [InlineData(0.0, "arewa")]
    [InlineData(45.0, "arewa maso gabas")]
    [InlineData(90.0, "gabas")]
    [InlineData(135.0, "kudu maso gabas")]
    [InlineData(180.0, "kudu")]
    [InlineData(225.0, "kudu maso yamma")]
    [InlineData(270.0, "yamma")]
    [InlineData(315.0, "arewa maso yamma")]
    public void FullDirections(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Full));
    }

    [Theory]
    [InlineData(0.0, "A")]
    [InlineData(45.0, "AG")]
    [InlineData(90.0, "G")]
    [InlineData(180.0, "K")]
    [InlineData(270.0, "Y")]
    public void AbbreviatedDirections(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Abbreviated));
    }
}
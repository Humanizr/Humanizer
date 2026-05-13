namespace Humanizer.Tests.Localisation.pa;

[UseCulture("pa")]
public class PunjabiCompassTests
{
    [Theory]
    [InlineData(0.0, "ਉੱਤਰ")]
    [InlineData(45.0, "ਉੱਤਰ-ਪੂਰਬ")]
    [InlineData(90.0, "ਪੂਰਬ")]
    [InlineData(135.0, "ਦੱਖਣ-ਪੂਰਬ")]
    [InlineData(180.0, "ਦੱਖਣ")]
    [InlineData(225.0, "ਦੱਖਣ-ਪੱਛਮ")]
    [InlineData(270.0, "ਪੱਛਮ")]
    [InlineData(315.0, "ਉੱਤਰ-ਪੱਛਮ")]
    public void FullDirections(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Full));
    }

    [Theory]
    [InlineData(0.0, "ਉੱ")]
    [InlineData(45.0, "ਉੱ-ਪੂ")]
    [InlineData(90.0, "ਪੂ")]
    [InlineData(180.0, "ਦੱਖ")]
    [InlineData(270.0, "ਪੱਛ")]
    public void AbbreviatedDirections(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Abbreviated));
    }
}
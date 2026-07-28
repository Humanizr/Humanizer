namespace Humanizer.Tests.Localisation.hi;

[UseCulture("hi")]
public class HindiCompassTests
{
    [Theory]
    [InlineData(0.0, "उत्तर")]
    [InlineData(45.0, "उत्तर-पूर्व")]
    [InlineData(90.0, "पूर्व")]
    [InlineData(135.0, "दक्षिण-पूर्व")]
    [InlineData(180.0, "दक्षिण")]
    [InlineData(225.0, "दक्षिण-पश्चिम")]
    [InlineData(270.0, "पश्चिम")]
    [InlineData(315.0, "उत्तर-पश्चिम")]
    public void FullDirections(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Full));
    }

    [Theory]
    [InlineData(0.0, "उ")]
    [InlineData(45.0, "उ-पू")]
    [InlineData(90.0, "पू")]
    [InlineData(180.0, "द")]
    [InlineData(270.0, "प")]
    public void AbbreviatedDirections(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Abbreviated));
    }
}
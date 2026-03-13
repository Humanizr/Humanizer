namespace mt;

[UseCulture("mt")]
public class HeadingTests
{
    [InlineData(0, "T")]
    [InlineData(45, "G")]
    [InlineData(90, "L")]
    [InlineData(112.5, "XL")]
    [InlineData(135, "X")]
    [InlineData(180, "N")]
    [InlineData(270, "P")]
    [Theory]
    public void ToHeadingAbbreviated(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading());

    [InlineData(0, "XL")]
    [InlineData(45, "grigal")]
    [InlineData(90, "lvant")]
    [InlineData(112.5, "xlokk il-lvant")]
    [InlineData(135, "xlokk")]
    [InlineData(180, "nofsinhar")]
    [InlineData(270, "punent")]
    [Theory]
    public void ToHeading(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading(HeadingStyle.Full));
}

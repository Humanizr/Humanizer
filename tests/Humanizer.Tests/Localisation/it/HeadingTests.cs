namespace it;

[UseCulture("it")]
public class HeadingTests
{
    [Theory]
    [InlineData(0, "N")]
    [InlineData(45, "NE")]
    [InlineData(90, "E")]
    [InlineData(135, "SE")]
    [InlineData(180, "S")]
    [InlineData(225, "SO")]
    [InlineData(270, "O")]
    [InlineData(315, "NO")]
    public void ToHeadingAbbreviated(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading());

    [Theory]
    [InlineData(0, "nord")]
    [InlineData(45, "nord-est")]
    [InlineData(90, "est")]
    [InlineData(135, "sud-est")]
    [InlineData(180, "sud")]
    [InlineData(225, "sud-ovest")]
    [InlineData(270, "ovest")]
    [InlineData(315, "nord-ovest")]
    public void ToHeading(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading(HeadingStyle.Full));

    [Theory]
    [InlineData("N", 0)]
    [InlineData("NE", 45)]
    [InlineData("E", 90)]
    [InlineData("SE", 135)]
    [InlineData("S", 180)]
    [InlineData("SO", 225)]
    [InlineData("O", 270)]
    [InlineData("NO", 315)]
    public void FromShortHeading(string heading, double expected) =>
        Assert.Equal(expected, heading.FromAbbreviatedHeading());
}

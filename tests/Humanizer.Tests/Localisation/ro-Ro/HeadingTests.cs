namespace roRO;

[UseCulture("ro-RO")]
public class HeadingTests
{
    [Theory]
    [InlineData(0, "N")]
    [InlineData(45, "NE")]
    [InlineData(90, "E")]
    [InlineData(135, "SE")]
    [InlineData(180, "S")]
    [InlineData(225, "SV")]
    [InlineData(270, "V")]
    [InlineData(315, "NV")]
    public void ToHeadingAbbreviated(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading());

    [Theory]
    [InlineData(0, "nord")]
    [InlineData(45, "nord-est")]
    [InlineData(90, "est")]
    [InlineData(135, "sud-est")]
    [InlineData(180, "sud")]
    [InlineData(225, "sud-vest")]
    [InlineData(270, "vest")]
    [InlineData(315, "nord-vest")]
    public void ToHeading(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading(HeadingStyle.Full));

    [Theory]
    [InlineData("N", 0)]
    [InlineData("NE", 45)]
    [InlineData("E", 90)]
    [InlineData("SE", 135)]
    [InlineData("S", 180)]
    [InlineData("SV", 225)]
    [InlineData("V", 270)]
    [InlineData("NV", 315)]
    public void FromShortHeading(string heading, double expected) =>
        Assert.Equal(expected, heading.FromAbbreviatedHeading());
}

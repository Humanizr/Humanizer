namespace sv;

[UseCulture("sv-SE")]
public class HeadingTests
{
    [Theory]
    [InlineData(0, "N")]
    [InlineData(45, "NO")]
    [InlineData(90, "O")]
    [InlineData(135, "SO")]
    [InlineData(180, "S")]
    [InlineData(225, "SV")]
    [InlineData(270, "V")]
    [InlineData(315, "NV")]
    public void ToHeadingAbbreviated(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading());

    [Theory]
    [InlineData(0, "nord")]
    [InlineData(45, "nordost")]
    [InlineData(90, "ost")]
    [InlineData(135, "sydost")]
    [InlineData(180, "syd")]
    [InlineData(225, "sydväst")]
    [InlineData(270, "väst")]
    [InlineData(315, "nordväst")]
    public void ToHeading(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading(HeadingStyle.Full));

    [Theory]
    [InlineData("N", 0)]
    [InlineData("NO", 45)]
    [InlineData("O", 90)]
    [InlineData("SO", 135)]
    [InlineData("S", 180)]
    [InlineData("SV", 225)]
    [InlineData("V", 270)]
    [InlineData("NV", 315)]
    public void FromShortHeading(string heading, double expected) =>
        Assert.Equal(expected, heading.FromAbbreviatedHeading());
}

namespace nl;

[UseCulture("nl-NL")]
public class HeadingTests
{
    [Theory]
    [InlineData(0, "N")]
    [InlineData(45, "NO")]
    [InlineData(90, "O")]
    [InlineData(135, "ZO")]
    [InlineData(180, "Z")]
    [InlineData(225, "ZW")]
    [InlineData(270, "W")]
    [InlineData(315, "NW")]
    public void ToHeadingAbbreviated(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading());

    [Theory]
    [InlineData(0, "noord")]
    [InlineData(45, "noordoost")]
    [InlineData(90, "oost")]
    [InlineData(135, "zuidoost")]
    [InlineData(180, "zuid")]
    [InlineData(225, "zuidwest")]
    [InlineData(270, "west")]
    [InlineData(315, "noordwest")]
    public void ToHeading(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading(HeadingStyle.Full));

    [Theory]
    [InlineData("N", 0)]
    [InlineData("NO", 45)]
    [InlineData("O", 90)]
    [InlineData("ZO", 135)]
    [InlineData("Z", 180)]
    [InlineData("ZW", 225)]
    [InlineData("W", 270)]
    [InlineData("NW", 315)]
    public void FromShortHeading(string heading, double expected) =>
        Assert.Equal(expected, heading.FromAbbreviatedHeading());
}

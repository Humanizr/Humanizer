namespace uzCyrl;

[UseCulture("uz-Cyrl-UZ")]
public class HeadingTests
{
    [Theory]
    [InlineData(0, "N")]
    [InlineData(45, "NE")]
    [InlineData(90, "E")]
    [InlineData(135, "SE")]
    [InlineData(180, "S")]
    [InlineData(225, "SW")]
    [InlineData(270, "W")]
    [InlineData(315, "NW")]
    public void ToHeadingAbbreviated(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading());

    [Theory]
    [InlineData(0, "шимол")]
    [InlineData(45, "шимоли-шарқ")]
    [InlineData(90, "шарқ")]
    [InlineData(135, "жануби-шарқ")]
    [InlineData(180, "жануб")]
    [InlineData(225, "жануби-ғарб")]
    [InlineData(270, "ғарб")]
    [InlineData(315, "шимоли-ғарб")]
    public void ToHeading(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading(HeadingStyle.Full));

    [Theory]
    [InlineData("N", 0)]
    [InlineData("NE", 45)]
    [InlineData("E", 90)]
    [InlineData("SE", 135)]
    [InlineData("S", 180)]
    [InlineData("SW", 225)]
    [InlineData("W", 270)]
    [InlineData("NW", 315)]
    public void FromShortHeading(string heading, double expected) =>
        Assert.Equal(expected, heading.FromAbbreviatedHeading());
}

namespace pl;

[UseCulture("pl")]
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
    [InlineData(0, "północ")]
    [InlineData(45, "północny wschód")]
    [InlineData(90, "wschód")]
    [InlineData(135, "południowy wschód")]
    [InlineData(180, "południe")]
    [InlineData(225, "południowy zachód")]
    [InlineData(270, "zachód")]
    [InlineData(315, "północny zachód")]
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

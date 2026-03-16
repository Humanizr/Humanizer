namespace af;

[UseCulture("af")]
public class HeadingTests
{
    [Theory]
    [InlineData(0, "N")]
    [InlineData(45, "NO")]
    [InlineData(90, "O")]
    [InlineData(135, "SO")]
    [InlineData(180, "S")]
    [InlineData(225, "SW")]
    [InlineData(270, "W")]
    [InlineData(315, "NW")]
    public void ToHeadingAbbreviated(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading());

    [Theory]
    [InlineData(0, "noord")]
    [InlineData(45, "noordoos")]
    [InlineData(90, "oos")]
    [InlineData(135, "suidoos")]
    [InlineData(180, "suid")]
    [InlineData(225, "suidwes")]
    [InlineData(270, "wes")]
    [InlineData(315, "noordwes")]
    public void ToHeading(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading(HeadingStyle.Full));

    [Theory]
    [InlineData("N", 0)]
    [InlineData("NO", 45)]
    [InlineData("O", 90)]
    [InlineData("SO", 135)]
    [InlineData("S", 180)]
    [InlineData("SW", 225)]
    [InlineData("W", 270)]
    [InlineData("NW", 315)]
    public void FromShortHeading(string heading, double expected) =>
        Assert.Equal(expected, heading.FromAbbreviatedHeading());
}

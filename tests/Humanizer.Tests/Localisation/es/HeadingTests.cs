namespace es;

[UseCulture("es-ES")]
public class HeadingTests
{
    [Theory]
    [InlineData(0, "N")]
    [InlineData(22.5, "N-NE")]
    [InlineData(45, "NE")]
    [InlineData(67.5, "E-NE")]
    [InlineData(90, "E")]
    [InlineData(112.5, "E-SE")]
    [InlineData(135, "SE")]
    [InlineData(157.5, "S-SE")]
    [InlineData(180, "S")]
    [InlineData(202.5, "S-SO")]
    [InlineData(225, "SO")]
    [InlineData(247.5, "O-SO")]
    [InlineData(270, "O")]
    [InlineData(292.5, "O-NO")]
    [InlineData(315, "NO")]
    [InlineData(337.5, "N-NO")]
    public void ToHeadingAbbreviated(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading());

    [Theory]
    [InlineData(0, "norte")]
    [InlineData(22.5, "norte-noreste")]
    [InlineData(45, "noreste")]
    [InlineData(67.5, "este-noreste")]
    [InlineData(90, "este")]
    [InlineData(112.5, "este-sudeste")]
    [InlineData(135, "sudeste")]
    [InlineData(157.5, "sur-sudeste")]
    [InlineData(180, "sur")]
    [InlineData(202.5, "sur-sudoeste")]
    [InlineData(225, "sudoeste")]
    [InlineData(247.5, "oeste-sudoeste")]
    [InlineData(270, "oeste")]
    [InlineData(292.5, "oeste-noroeste")]
    [InlineData(315, "noroeste")]
    [InlineData(337.5, "norte-noroeste")]
    public void ToHeading(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading(HeadingStyle.Full));

    [Theory]
    [InlineData("N", 0)]
    [InlineData("N-NE", 22.5)]
    [InlineData("NE", 45)]
    [InlineData("E-NE", 67.5)]
    [InlineData("E", 90)]
    [InlineData("E-SE", 112.5)]
    [InlineData("SE", 135)]
    [InlineData("S-SE", 157.5)]
    [InlineData("S", 180)]
    [InlineData("S-SO", 202.5)]
    [InlineData("SO", 225)]
    [InlineData("O-SO", 247.5)]
    [InlineData("O", 270)]
    [InlineData("O-NO", 292.5)]
    [InlineData("NO", 315)]
    [InlineData("N-NO", 337.5)]
    public void FromShortHeading(string heading, double expected) =>
        Assert.Equal(expected, heading.FromAbbreviatedHeading(new("es-ES")));
}

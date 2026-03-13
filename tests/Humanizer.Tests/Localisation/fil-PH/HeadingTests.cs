namespace filPH;

[UseCulture("fil-PH")]
public class HeadingTests
{
    [Theory]
    [InlineData(0, "N")]
    [InlineData(22.5, "NNE")]
    [InlineData(45, "NE")]
    [InlineData(67.5, "ENE")]
    [InlineData(90, "E")]
    [InlineData(112.5, "ESE")]
    [InlineData(135, "SE")]
    [InlineData(157.5, "SSE")]
    [InlineData(180, "S")]
    [InlineData(202.5, "SSW")]
    [InlineData(225, "SW")]
    [InlineData(247.5, "WSW")]
    [InlineData(270, "W")]
    [InlineData(292.5, "WNW")]
    [InlineData(315, "NW")]
    [InlineData(337.5, "NNW")]
    public void ToHeadingAbbreviated(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading());

    [Theory]
    [InlineData(0, "hilaga")]
    [InlineData(22.5, "hilaga-hilagang-silangan")]
    [InlineData(45, "hilagang-silangan")]
    [InlineData(67.5, "silangan-hilagang-silangan")]
    [InlineData(90, "silangan")]
    [InlineData(112.5, "silangan-timog-silangan")]
    [InlineData(135, "timog-silangan")]
    [InlineData(157.5, "timog-timog-silangan")]
    [InlineData(180, "timog")]
    [InlineData(202.5, "timog-timog-kanluran")]
    [InlineData(225, "timog-kanluran")]
    [InlineData(247.5, "kanluran-timog-kanluran")]
    [InlineData(270, "kanluran")]
    [InlineData(292.5, "kanluran-hilagang-kanluran")]
    [InlineData(315, "hilagang-kanluran")]
    [InlineData(337.5, "hilaga-hilagang-kanluran")]
    public void ToHeading(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading(HeadingStyle.Full));

    [Theory]
    [InlineData("N", 0)]
    [InlineData("NNE", 22.5)]
    [InlineData("NE", 45)]
    [InlineData("ENE", 67.5)]
    [InlineData("E", 90)]
    [InlineData("ESE", 112.5)]
    [InlineData("SE", 135)]
    [InlineData("SSE", 157.5)]
    [InlineData("S", 180)]
    [InlineData("SSW", 202.5)]
    [InlineData("SW", 225)]
    [InlineData("WSW", 247.5)]
    [InlineData("W", 270)]
    [InlineData("WNW", 292.5)]
    [InlineData("NW", 315)]
    [InlineData("NNW", 337.5)]
    public void FromShortHeading(string heading, double expected) =>
        Assert.Equal(expected, heading.FromAbbreviatedHeading(new("fil-PH")));
}

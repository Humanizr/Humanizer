namespace cy;

[UseCulture("cy")]
public class HeadingTests
{
    [Theory]
    [InlineData(0, "north")]
    [InlineData(22.5, "north-northeast")]
    [InlineData(45, "northeast")]
    [InlineData(67.5, "east-northeast")]
    [InlineData(90, "east")]
    [InlineData(112.5, "east-southeast")]
    [InlineData(135, "southeast")]
    [InlineData(157.5, "south-southeast")]
    [InlineData(180, "south")]
    [InlineData(202.5, "south-southwest")]
    [InlineData(225, "southwest")]
    [InlineData(247.5, "west-southwest")]
    [InlineData(270, "west")]
    [InlineData(292.5, "west-northwest")]
    [InlineData(315, "northwest")]
    [InlineData(337.5, "north-northwest")]
    public void ToHeadingFull(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading(HeadingStyle.Full));

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
    public void ToHeadingShort(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading());
}
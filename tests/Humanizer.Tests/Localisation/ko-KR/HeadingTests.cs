namespace koKR;

[UseCulture("ko-KR")]
public class HeadingTests
{
    [Theory]
    [InlineData(0, "북")]
    [InlineData(22.5, "북북동")]
    [InlineData(45, "북동")]
    [InlineData(67.5, "동북동")]
    [InlineData(90, "동")]
    [InlineData(112.5, "동남동")]
    [InlineData(135, "남동")]
    [InlineData(157.5, "남남동")]
    [InlineData(180, "남")]
    [InlineData(202.5, "남남서")]
    [InlineData(225, "남서")]
    [InlineData(247.5, "서남서")]
    [InlineData(270, "서")]
    [InlineData(292.5, "서북서")]
    [InlineData(315, "북서")]
    [InlineData(337.5, "북북서")]
    public void ToHeadingAbbreviated(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading());

    [Theory]
    [InlineData(0, "북")]
    [InlineData(22.5, "북북동")]
    [InlineData(45, "북동")]
    [InlineData(67.5, "동북동")]
    [InlineData(90, "동")]
    [InlineData(112.5, "동남동")]
    [InlineData(135, "남동")]
    [InlineData(157.5, "남남동")]
    [InlineData(180, "남")]
    [InlineData(202.5, "남남서")]
    [InlineData(225, "남서")]
    [InlineData(247.5, "서남서")]
    [InlineData(270, "서")]
    [InlineData(292.5, "서북서")]
    [InlineData(315, "북서")]
    [InlineData(337.5, "북북서")]
    public void ToHeading(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading(HeadingStyle.Full));

    [Theory]
    [InlineData("북", 0)]
    [InlineData("북북동", 22.5)]
    [InlineData("북동", 45)]
    [InlineData("동북동", 67.5)]
    [InlineData("동", 90)]
    [InlineData("동남동", 112.5)]
    [InlineData("남동", 135)]
    [InlineData("남남동", 157.5)]
    [InlineData("남", 180)]
    [InlineData("남남서", 202.5)]
    [InlineData("남서", 225)]
    [InlineData("서남서", 247.5)]
    [InlineData("서", 270)]
    [InlineData("서북서", 292.5)]
    [InlineData("북서", 315)]
    [InlineData("북북서", 337.5)]
    public void FromShortHeading(string heading, double expected) =>
        Assert.Equal(expected, heading.FromAbbreviatedHeading());
}

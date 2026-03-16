namespace sl;

[UseCulture("sl-SI")]
public class HeadingTests
{
    [Theory]
    [InlineData(0, "S")]
    [InlineData(45, "SV")]
    [InlineData(90, "V")]
    [InlineData(135, "JV")]
    [InlineData(180, "J")]
    [InlineData(225, "JZ")]
    [InlineData(270, "Z")]
    [InlineData(315, "SZ")]
    public void ToHeadingAbbreviated(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading());

    [Theory]
    [InlineData(0, "sever")]
    [InlineData(45, "severovzhod")]
    [InlineData(90, "vzhod")]
    [InlineData(135, "jugovzhod")]
    [InlineData(180, "jug")]
    [InlineData(225, "jugozahod")]
    [InlineData(270, "zahod")]
    [InlineData(315, "severozahod")]
    public void ToHeading(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading(HeadingStyle.Full));

    [Theory]
    [InlineData("S", 0)]
    [InlineData("SV", 45)]
    [InlineData("V", 90)]
    [InlineData("JV", 135)]
    [InlineData("J", 180)]
    [InlineData("JZ", 225)]
    [InlineData("Z", 270)]
    [InlineData("SZ", 315)]
    public void FromShortHeading(string heading, double expected) =>
        Assert.Equal(expected, heading.FromAbbreviatedHeading());
}

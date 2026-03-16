namespace sk;

[UseCulture("sk-SK")]
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
    [InlineData(45, "severovýchod")]
    [InlineData(90, "východ")]
    [InlineData(135, "juhovýchod")]
    [InlineData(180, "juh")]
    [InlineData(225, "juhozápad")]
    [InlineData(270, "západ")]
    [InlineData(315, "severozápad")]
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

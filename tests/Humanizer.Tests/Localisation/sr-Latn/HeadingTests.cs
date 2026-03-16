namespace srLatn;

[UseCulture("sr-Latn")]
public class HeadingTests
{
    [Theory]
    [InlineData(0, "S")]
    [InlineData(45, "SI")]
    [InlineData(90, "I")]
    [InlineData(135, "JI")]
    [InlineData(180, "J")]
    [InlineData(225, "JZ")]
    [InlineData(270, "Z")]
    [InlineData(315, "SZ")]
    public void ToHeadingAbbreviated(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading());

    [Theory]
    [InlineData(0, "sever")]
    [InlineData(45, "severoistok")]
    [InlineData(90, "istok")]
    [InlineData(135, "jugoistok")]
    [InlineData(180, "jug")]
    [InlineData(225, "jugozapad")]
    [InlineData(270, "zapad")]
    [InlineData(315, "severozapad")]
    public void ToHeading(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading(HeadingStyle.Full));

    [Theory]
    [InlineData("S", 0)]
    [InlineData("SI", 45)]
    [InlineData("I", 90)]
    [InlineData("JI", 135)]
    [InlineData("J", 180)]
    [InlineData("JZ", 225)]
    [InlineData("Z", 270)]
    [InlineData("SZ", 315)]
    public void FromShortHeading(string heading, double expected) =>
        Assert.Equal(expected, heading.FromAbbreviatedHeading());
}

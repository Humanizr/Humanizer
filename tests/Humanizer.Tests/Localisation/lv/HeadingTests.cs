namespace lv;

[UseCulture("lv")]
public class HeadingTests
{
    [Theory]
    [InlineData(0, "Z")]
    [InlineData(45, "ZA")]
    [InlineData(90, "A")]
    [InlineData(135, "DA")]
    [InlineData(180, "D")]
    [InlineData(225, "DR")]
    [InlineData(270, "R")]
    [InlineData(315, "ZR")]
    public void ToHeadingAbbreviated(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading());

    [Theory]
    [InlineData(0, "ziemeļi")]
    [InlineData(45, "ziemeļaustrumi")]
    [InlineData(90, "austrumi")]
    [InlineData(135, "dienvidaustrumi")]
    [InlineData(180, "dienvidi")]
    [InlineData(225, "dienvidrietumi")]
    [InlineData(270, "rietumi")]
    [InlineData(315, "ziemeļrietumi")]
    public void ToHeading(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading(HeadingStyle.Full));

    [Theory]
    [InlineData("Z", 0)]
    [InlineData("ZA", 45)]
    [InlineData("A", 90)]
    [InlineData("DA", 135)]
    [InlineData("D", 180)]
    [InlineData("DR", 225)]
    [InlineData("R", 270)]
    [InlineData("ZR", 315)]
    public void FromShortHeading(string heading, double expected) =>
        Assert.Equal(expected, heading.FromAbbreviatedHeading());
}

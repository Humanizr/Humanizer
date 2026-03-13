namespace lt;

[UseCulture("lt")]
public class HeadingTests
{
    [Theory]
    [InlineData(0, "Š")]
    [InlineData(45, "ŠR")]
    [InlineData(90, "R")]
    [InlineData(135, "PR")]
    [InlineData(180, "P")]
    [InlineData(225, "PV")]
    [InlineData(270, "V")]
    [InlineData(315, "ŠV")]
    public void ToHeadingAbbreviated(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading());

    [Theory]
    [InlineData(0, "šiaurė")]
    [InlineData(45, "šiaurės rytai")]
    [InlineData(90, "rytai")]
    [InlineData(135, "pietryčiai")]
    [InlineData(180, "pietūs")]
    [InlineData(225, "pietvakariai")]
    [InlineData(270, "vakarai")]
    [InlineData(315, "šiaurės vakarai")]
    public void ToHeading(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading(HeadingStyle.Full));

    [Theory]
    [InlineData("Š", 0)]
    [InlineData("ŠR", 45)]
    [InlineData("R", 90)]
    [InlineData("PR", 135)]
    [InlineData("P", 180)]
    [InlineData("PV", 225)]
    [InlineData("V", 270)]
    [InlineData("ŠV", 315)]
    public void FromShortHeading(string heading, double expected) =>
        Assert.Equal(expected, heading.FromAbbreviatedHeading());
}

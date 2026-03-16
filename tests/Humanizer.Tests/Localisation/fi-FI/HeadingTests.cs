namespace fiFI;

[UseCulture("fi-FI")]
public class HeadingTests
{
    [Theory]
    [InlineData(0, "P")]
    [InlineData(45, "KO")]
    [InlineData(90, "I")]
    [InlineData(135, "KA")]
    [InlineData(180, "E")]
    [InlineData(225, "LO")]
    [InlineData(270, "L")]
    [InlineData(315, "LU")]
    public void ToHeadingAbbreviated(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading());

    [Theory]
    [InlineData(0, "pohjoinen")]
    [InlineData(45, "koillinen")]
    [InlineData(90, "itä")]
    [InlineData(135, "kaakko")]
    [InlineData(180, "etelä")]
    [InlineData(225, "lounas")]
    [InlineData(270, "länsi")]
    [InlineData(315, "luode")]
    public void ToHeading(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading(HeadingStyle.Full));

    [Theory]
    [InlineData("P", 0)]
    [InlineData("KO", 45)]
    [InlineData("I", 90)]
    [InlineData("KA", 135)]
    [InlineData("E", 180)]
    [InlineData("LO", 225)]
    [InlineData("L", 270)]
    [InlineData("LU", 315)]
    public void FromShortHeading(string heading, double expected) =>
        Assert.Equal(expected, heading.FromAbbreviatedHeading());
}

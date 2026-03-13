namespace he;

[UseCulture("he")]
public class HeadingTests
{
    [Theory]
    [InlineData(0, "צ")]
    [InlineData(45, "צמ")]
    [InlineData(90, "מ")]
    [InlineData(135, "דמ")]
    [InlineData(180, "ד")]
    [InlineData(225, "דמ")]
    [InlineData(270, "מ")]
    [InlineData(315, "צמ")]
    public void ToHeadingAbbreviated(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading());

    [Theory]
    [InlineData(0, "צפון")]
    [InlineData(45, "צפון מזרח")]
    [InlineData(90, "מזרח")]
    [InlineData(135, "דרום מזרח")]
    [InlineData(180, "דרום")]
    [InlineData(225, "דרום מערב")]
    [InlineData(270, "מערב")]
    [InlineData(315, "צפון מערב")]
    public void ToHeading(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading(HeadingStyle.Full));
}

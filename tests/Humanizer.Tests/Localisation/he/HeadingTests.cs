namespace he;

[UseCulture("he")]
public class HeadingTests
{
    [Theory]
    [InlineData(0, "צ")]
    [InlineData(45, "צמז")]
    [InlineData(90, "מז")]
    [InlineData(135, "דמז")]
    [InlineData(180, "ד")]
    [InlineData(225, "דמע")]
    [InlineData(270, "מע")]
    [InlineData(315, "צמע")]
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

    [Theory]
    [InlineData("צ", 0)]
    [InlineData("צמז", 45)]
    [InlineData("מז", 90)]
    [InlineData("דמז", 135)]
    [InlineData("ד", 180)]
    [InlineData("דמע", 225)]
    [InlineData("מע", 270)]
    [InlineData("צמע", 315)]
    public void FromShortHeading(string heading, double expected) =>
        Assert.Equal(expected, heading.FromAbbreviatedHeading());
}

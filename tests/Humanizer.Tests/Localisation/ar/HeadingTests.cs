namespace ar;

[UseCulture("ar")]
public class HeadingTests
{
    [Theory]
    [InlineData(0, "ش")]
    [InlineData(45, "ش.ق")]
    [InlineData(90, "ق")]
    [InlineData(135, "ج.ق")]
    [InlineData(180, "ج")]
    [InlineData(225, "ج.غ")]
    [InlineData(270, "غ")]
    [InlineData(315, "ش.غ")]
    public void ToHeadingAbbreviated(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading());

    [Theory]
    [InlineData(0, "شمال")]
    [InlineData(45, "شمال-شرق")]
    [InlineData(90, "شرق")]
    [InlineData(135, "جنوب-شرق")]
    [InlineData(180, "جنوب")]
    [InlineData(225, "جنوب-غرب")]
    [InlineData(270, "غرب")]
    [InlineData(315, "شمال-غرب")]
    public void ToHeading(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading(HeadingStyle.Full));

    [Theory]
    [InlineData("ش", 0)]
    [InlineData("ش.ق", 45)]
    [InlineData("ق", 90)]
    [InlineData("ج.ق", 135)]
    [InlineData("ج", 180)]
    [InlineData("ج.غ", 225)]
    [InlineData("غ", 270)]
    [InlineData("ش.غ", 315)]
    public void FromShortHeading(string heading, double expected) =>
        Assert.Equal(expected, heading.FromAbbreviatedHeading());
}

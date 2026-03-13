namespace fa;

[UseCulture("fa")]
public class HeadingTests
{
    [Theory]
    [InlineData(0, "ش")]
    [InlineData(45, "ش‌شر")]
    [InlineData(90, "شر")]
    [InlineData(135, "ج‌شر")]
    [InlineData(180, "ج")]
    [InlineData(225, "ج‌غر")]
    [InlineData(270, "غر")]
    [InlineData(315, "ش‌غر")]
    public void ToHeadingAbbreviated(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading());

    [Theory]
    [InlineData(0, "شمال")]
    [InlineData(45, "شمال‌شرق")]
    [InlineData(90, "شرق")]
    [InlineData(135, "جنوب‌شرق")]
    [InlineData(180, "جنوب")]
    [InlineData(225, "جنوب‌غرب")]
    [InlineData(270, "غرب")]
    [InlineData(315, "شمال‌غرب")]
    public void ToHeading(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading(HeadingStyle.Full));

    [Theory]
    [InlineData("ش", 0)]
    [InlineData("ش‌شر", 45)]
    [InlineData("شر", 90)]
    [InlineData("ج‌شر", 135)]
    [InlineData("ج", 180)]
    [InlineData("ج‌غر", 225)]
    [InlineData("غر", 270)]
    [InlineData("ش‌غر", 315)]
    public void FromShortHeading(string heading, double expected) =>
        Assert.Equal(expected, heading.FromAbbreviatedHeading());
}

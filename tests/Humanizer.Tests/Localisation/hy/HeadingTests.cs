namespace hy;

[UseCulture("hy")]
public class HeadingTests
{
    [Theory]
    [InlineData(0, "Հ")]
    [InlineData(45, "ՀԱ")]
    [InlineData(90, "Ա")]
    [InlineData(135, "ՀԱՐԱ")]
    [InlineData(180, "ՀԱՐ")]
    [InlineData(225, "ՀԱՐԱՄ")]
    [InlineData(270, "ԱՄ")]
    [InlineData(315, "ՀԱՄ")]
    public void ToHeadingAbbreviated(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading());

    [Theory]
    [InlineData(0, "հյուսիս")]
    [InlineData(45, "հյուսիս-արևելք")]
    [InlineData(90, "արևելք")]
    [InlineData(135, "հարավ-արևելք")]
    [InlineData(180, "հարավ")]
    [InlineData(225, "հարավ-արևմուտք")]
    [InlineData(270, "արևմուտք")]
    [InlineData(315, "հյուսիս-արևմուտք")]
    public void ToHeading(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading(HeadingStyle.Full));

    [Theory]
    [InlineData("Հ", 0)]
    [InlineData("ՀԱ", 45)]
    [InlineData("Ա", 90)]
    [InlineData("ՀԱՐԱ", 135)]
    [InlineData("ՀԱՐ", 180)]
    [InlineData("ՀԱՐԱՄ", 225)]
    [InlineData("ԱՄ", 270)]
    [InlineData("ՀԱՄ", 315)]
    public void FromShortHeading(string heading, double expected) =>
        Assert.Equal(expected, heading.FromAbbreviatedHeading());
}

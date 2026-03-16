namespace bg;

[UseCulture("bg-BG")]
public class HeadingTests
{
    [Theory]
    [InlineData(0, "С")]
    [InlineData(45, "СИ")]
    [InlineData(90, "И")]
    [InlineData(135, "ЮИ")]
    [InlineData(180, "Ю")]
    [InlineData(225, "ЮЗ")]
    [InlineData(270, "З")]
    [InlineData(315, "СЗ")]
    public void ToHeadingAbbreviated(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading());

    [Theory]
    [InlineData(0, "север")]
    [InlineData(45, "североизток")]
    [InlineData(90, "изток")]
    [InlineData(135, "югоизток")]
    [InlineData(180, "юг")]
    [InlineData(225, "югозапад")]
    [InlineData(270, "запад")]
    [InlineData(315, "северозапад")]
    public void ToHeading(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading(HeadingStyle.Full));

    [Theory]
    [InlineData("С", 0)]
    [InlineData("СИ", 45)]
    [InlineData("И", 90)]
    [InlineData("ЮИ", 135)]
    [InlineData("Ю", 180)]
    [InlineData("ЮЗ", 225)]
    [InlineData("З", 270)]
    [InlineData("СЗ", 315)]
    public void FromShortHeading(string heading, double expected) =>
        Assert.Equal(expected, heading.FromAbbreviatedHeading());
}

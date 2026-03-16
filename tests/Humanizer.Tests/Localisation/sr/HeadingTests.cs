namespace sr;

[UseCulture("sr")]
public class HeadingTests
{
    [Theory]
    [InlineData(0, "С")]
    [InlineData(45, "СИ")]
    [InlineData(90, "И")]
    [InlineData(135, "ЈИ")]
    [InlineData(180, "Ј")]
    [InlineData(225, "ЈЗ")]
    [InlineData(270, "З")]
    [InlineData(315, "СЗ")]
    public void ToHeadingAbbreviated(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading());

    [Theory]
    [InlineData(0, "север")]
    [InlineData(45, "североисток")]
    [InlineData(90, "исток")]
    [InlineData(135, "југоисток")]
    [InlineData(180, "југ")]
    [InlineData(225, "југозапад")]
    [InlineData(270, "запад")]
    [InlineData(315, "северозапад")]
    public void ToHeading(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading(HeadingStyle.Full));

    [Theory]
    [InlineData("С", 0)]
    [InlineData("СИ", 45)]
    [InlineData("И", 90)]
    [InlineData("ЈИ", 135)]
    [InlineData("Ј", 180)]
    [InlineData("ЈЗ", 225)]
    [InlineData("З", 270)]
    [InlineData("СЗ", 315)]
    public void FromShortHeading(string heading, double expected) =>
        Assert.Equal(expected, heading.FromAbbreviatedHeading());
}

namespace nb;

[UseCulture("nb")]
public class HeadingTests
{
    [Theory]
    [InlineData(0, "N")]
    [InlineData(45, "NØ")]
    [InlineData(90, "Ø")]
    [InlineData(135, "SØ")]
    [InlineData(180, "S")]
    [InlineData(225, "SV")]
    [InlineData(270, "V")]
    [InlineData(315, "NV")]
    public void ToHeadingAbbreviated(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading());

    [Theory]
    [InlineData(0, "nord")]
    [InlineData(45, "nordøst")]
    [InlineData(90, "øst")]
    [InlineData(135, "sørøst")]
    [InlineData(180, "sør")]
    [InlineData(225, "sørvest")]
    [InlineData(270, "vest")]
    [InlineData(315, "nordvest")]
    public void ToHeading(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading(HeadingStyle.Full));

    [Theory]
    [InlineData("N", 0)]
    [InlineData("NØ", 45)]
    [InlineData("Ø", 90)]
    [InlineData("SØ", 135)]
    [InlineData("S", 180)]
    [InlineData("SV", 225)]
    [InlineData("V", 270)]
    [InlineData("NV", 315)]
    public void FromShortHeading(string heading, double expected) =>
        Assert.Equal(expected, heading.FromAbbreviatedHeading());
}

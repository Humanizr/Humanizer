namespace da;

[UseCulture("da-DK")]
public class HeadingTests
{
    [Theory]
    [InlineData(0, "N")]
    [InlineData(22.5, "NNØ")]
    [InlineData(45, "NØ")]
    [InlineData(67.5, "ØNØ")]
    [InlineData(90, "Ø")]
    [InlineData(112.5, "ØSØ")]
    [InlineData(135, "SØ")]
    [InlineData(157.5, "SSØ")]
    [InlineData(180, "S")]
    [InlineData(202.5, "SSV")]
    [InlineData(225, "SV")]
    [InlineData(247.5, "VSV")]
    [InlineData(270, "V")]
    [InlineData(292.5, "VNV")]
    [InlineData(315, "NV")]
    [InlineData(337.5, "NNV")]
    public void ToHeadingAbbreviated(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading());

    [Theory]
    [InlineData(0, "nord")]
    [InlineData(22.5, "nordnordøst")]
    [InlineData(45, "nordøst")]
    [InlineData(67.5, "østnordøst")]
    [InlineData(90, "øst")]
    [InlineData(112.5, "østsydøst")]
    [InlineData(135, "sydøst")]
    [InlineData(157.5, "sydsydøst")]
    [InlineData(180, "syd")]
    [InlineData(202.5, "sydsydvest")]
    [InlineData(225, "sydvest")]
    [InlineData(247.5, "vestsydvest")]
    [InlineData(270, "vest")]
    [InlineData(292.5, "vestnordvest")]
    [InlineData(315, "nordvest")]
    [InlineData(337.5, "nordnordvest")]
    public void ToHeading(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading(HeadingStyle.Full));

    [Theory]
    [InlineData("N", 0)]
    [InlineData("NNØ", 22.5)]
    [InlineData("NØ", 45)]
    [InlineData("ØNØ", 67.5)]
    [InlineData("Ø", 90)]
    [InlineData("ØSØ", 112.5)]
    [InlineData("SØ", 135)]
    [InlineData("SSØ", 157.5)]
    [InlineData("S", 180)]
    [InlineData("SSV", 202.5)]
    [InlineData("SV", 225)]
    [InlineData("VSV", 247.5)]
    [InlineData("V", 270)]
    [InlineData("VNV", 292.5)]
    [InlineData("NV", 315)]
    [InlineData("NNV", 337.5)]
    public void FromShortHeading(string heading, double expected) =>
        Assert.Equal(expected, heading.FromAbbreviatedHeading(new("da-DK")));
}

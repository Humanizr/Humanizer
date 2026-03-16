namespace ptBR;

[UseCulture("pt-BR")]
public class HeadingTests
{
    [Theory]
    [InlineData(0, "N")]
    [InlineData(22.5, "NNE")]
    [InlineData(45, "NE")]
    [InlineData(67.5, "LNE")]
    [InlineData(90, "L")]
    [InlineData(112.5, "LSE")]
    [InlineData(135, "SE")]
    [InlineData(157.5, "SSE")]
    [InlineData(180, "S")]
    [InlineData(202.5, "SSO")]
    [InlineData(225, "SO")]
    [InlineData(247.5, "OSO")]
    [InlineData(270, "O")]
    [InlineData(292.5, "ONO")]
    [InlineData(315, "NO")]
    [InlineData(337.5, "NNO")]
    public void ToHeadingAbbreviated(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading());

    [Theory]
    [InlineData(0, "norte")]
    [InlineData(22.5, "norte-nordeste")]
    [InlineData(45, "nordeste")]
    [InlineData(67.5, "leste-nordeste")]
    [InlineData(90, "leste")]
    [InlineData(112.5, "leste-sudeste")]
    [InlineData(135, "sudeste")]
    [InlineData(157.5, "sul-sudeste")]
    [InlineData(180, "sul")]
    [InlineData(202.5, "sul-sudoeste")]
    [InlineData(225, "sudoeste")]
    [InlineData(247.5, "oeste-sudoeste")]
    [InlineData(270, "oeste")]
    [InlineData(292.5, "oeste-noroeste")]
    [InlineData(315, "noroeste")]
    [InlineData(337.5, "norte-noroeste")]
    public void ToHeading(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading(HeadingStyle.Full));

    [Theory]
    [InlineData("N", 0)]
    [InlineData("NNE", 22.5)]
    [InlineData("NE", 45)]
    [InlineData("LNE", 67.5)]
    [InlineData("L", 90)]
    [InlineData("LSE", 112.5)]
    [InlineData("SE", 135)]
    [InlineData("SSE", 157.5)]
    [InlineData("S", 180)]
    [InlineData("SSO", 202.5)]
    [InlineData("SO", 225)]
    [InlineData("OSO", 247.5)]
    [InlineData("O", 270)]
    [InlineData("ONO", 292.5)]
    [InlineData("NO", 315)]
    [InlineData("NNO", 337.5)]
    public void FromShortHeading(string heading, double expected) =>
        Assert.Equal(expected, heading.FromAbbreviatedHeading(new("pt-BR")));
}

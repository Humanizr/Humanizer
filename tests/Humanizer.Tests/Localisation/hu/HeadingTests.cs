namespace hu;

[UseCulture("hu")]
public class HeadingTests
{
    [Theory]
    [InlineData(0, "É")]
    [InlineData(22.5, "ÉÉK")]
    [InlineData(45, "ÉK")]
    [InlineData(67.5, "KÉK")]
    [InlineData(90, "K")]
    [InlineData(112.5, "KDK")]
    [InlineData(135, "DK")]
    [InlineData(157.5, "DDK")]
    [InlineData(180, "D")]
    [InlineData(202.5, "DDNy")]
    [InlineData(225, "DNy")]
    [InlineData(247.5, "NyDNy")]
    [InlineData(270, "Ny")]
    [InlineData(292.5, "NyÉNy")]
    [InlineData(315, "ÉNy")]
    [InlineData(337.5, "ÉÉNy")]
    public void ToHeadingAbbreviated(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading());

    [Theory]
    [InlineData(0, "észak")]
    [InlineData(22.5, "észak-északkelet")]
    [InlineData(45, "északkelet")]
    [InlineData(67.5, "kelet-északkelet")]
    [InlineData(90, "kelet")]
    [InlineData(112.5, "kelet-délkelet")]
    [InlineData(135, "délkelet")]
    [InlineData(157.5, "dél-délkelet")]
    [InlineData(180, "dél")]
    [InlineData(202.5, "dél-délnyugat")]
    [InlineData(225, "délnyugat")]
    [InlineData(247.5, "nyugat-délnyugat")]
    [InlineData(270, "nyugat")]
    [InlineData(292.5, "nyugat-északnyugat")]
    [InlineData(315, "északnyugat")]
    [InlineData(337.5, "észak-északnyugat")]
    public void ToHeading(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading(HeadingStyle.Full));

    [Theory]
    [InlineData("É", 0)]
    [InlineData("ÉÉK", 22.5)]
    [InlineData("ÉK", 45)]
    [InlineData("KÉK", 67.5)]
    [InlineData("K", 90)]
    [InlineData("KDK", 112.5)]
    [InlineData("DK", 135)]
    [InlineData("DDK", 157.5)]
    [InlineData("D", 180)]
    [InlineData("DDNy", 202.5)]
    [InlineData("DNy", 225)]
    [InlineData("NyDNy", 247.5)]
    [InlineData("Ny", 270)]
    [InlineData("NyÉNy", 292.5)]
    [InlineData("ÉNy", 315)]
    [InlineData("ÉÉNy", 337.5)]
    public void FromShortHeading(string heading, double expected) =>
        Assert.Equal(expected, heading.FromAbbreviatedHeading(new("hu")));
}

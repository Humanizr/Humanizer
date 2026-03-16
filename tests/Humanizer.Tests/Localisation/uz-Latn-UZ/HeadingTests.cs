namespace uzLatn;

[UseCulture("uz-Latn-UZ")]
public class HeadingTests
{
    [Theory]
    [InlineData(0, "Sh")]
    [InlineData(45, "ShSh")]
    [InlineData(90, "Sharq")]
    [InlineData(135, "JSh")]
    [InlineData(180, "J")]
    [InlineData(225, "JG")]
    [InlineData(270, "G")]
    [InlineData(315, "ShG")]
    public void ToHeadingAbbreviated(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading());

    [Theory]
    [InlineData(0, "shimol")]
    [InlineData(45, "shimoli-sharq")]
    [InlineData(90, "sharq")]
    [InlineData(135, "janubi-sharq")]
    [InlineData(180, "janub")]
    [InlineData(225, "janubi-g'arb")]
    [InlineData(270, "g'arb")]
    [InlineData(315, "shimoli-g'arb")]
    public void ToHeading(double heading, string expected) =>
        Assert.Equal(expected, heading.ToHeading(HeadingStyle.Full));
}

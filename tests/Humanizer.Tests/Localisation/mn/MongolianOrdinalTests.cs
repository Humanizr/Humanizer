namespace Humanizer.Tests.Localisation.mn;

[UseCulture("mn")]
public class MongolianOrdinalTests
{
    [Theory]
    [InlineData(1, "1-р")]
    [InlineData(2, "2-р")]
    [InlineData(21, "21-р")]
    public void Ordinalize_UsesMongolianNumericTemplate(int number, string expected)
    {
        Assert.Equal(expected, number.Ordinalize());
        Assert.Equal(expected, number.ToString(CultureInfo.InvariantCulture).Ordinalize());
    }
}
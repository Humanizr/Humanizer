namespace Humanizer.Tests.Localisation.sw;

[UseCulture("sw")]
public class SwahiliOrdinalTests
{
    static readonly CultureInfo Sw = new("sw");

    [Theory]
    [InlineData(1, "1")]
    [InlineData(2, "2")]
    [InlineData(21, "21")]
    public void Ordinalize_UsesSwahiliNumericTemplate(int number, string expected)
    {
        Assert.Equal(expected, number.Ordinalize(Sw));
        Assert.Equal(expected, number.ToString(CultureInfo.InvariantCulture).Ordinalize(Sw));
    }
}
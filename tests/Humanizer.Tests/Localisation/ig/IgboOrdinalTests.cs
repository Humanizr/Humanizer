namespace Humanizer.Tests.Localisation.ig;

[UseCulture("ig")]
public class IgboOrdinalTests
{
    [Theory]
    [InlineData(1, "nke 1")]
    [InlineData(2, "nke 2")]
    [InlineData(21, "nke 21")]
    public void Ordinalize_UsesIgboNumericTemplate(int number, string expected)
    {
        Assert.Equal(expected, number.Ordinalize());
        Assert.Equal(expected, number.ToString(CultureInfo.InvariantCulture).Ordinalize());
    }

    [Theory]
    [InlineData(1, "nke mbụ")]
    [InlineData(2, "nke abụọ")]
    [InlineData(21, "iri abụọ na nke mbụ")]
    [InlineData(-1, "mwepu otu")]
    public void NumberToOrdinalWords_UsesIgboOrdinalWords(int number, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords());
    }
}
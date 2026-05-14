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
    [InlineData(21, "nke iri abụọ na otu")]
    [InlineData(22, "nke iri abụọ na abụọ")]
    [InlineData(23, "nke iri abụọ na atọ")]
    [InlineData(105, "nke otu narị na ise")]
    [InlineData(-1, "mwepu nke mbụ")]
    public void NumberToOrdinalWords_UsesIgboOrdinalWords(int number, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords());
    }
}
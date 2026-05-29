namespace Humanizer.Tests.Localisation.ha;

[UseCulture("ha")]
public class HausaOrdinalTests
{
    [Theory]
    [InlineData(1, "na 1")]
    [InlineData(2, "na 2")]
    [InlineData(21, "na 21")]
    public void Ordinalize_UsesHausaNumericTemplate(int number, string expected)
    {
        Assert.Equal(expected, number.Ordinalize());
        Assert.Equal(expected, number.ToString(CultureInfo.InvariantCulture).Ordinalize());
    }

    [Theory]
    [InlineData(1, "na farko")]
    [InlineData(2, "na biyu")]
    [InlineData(21, "na ashirin da ɗaya")]
    [InlineData(-1, "debe na farko")]
    public void NumberToOrdinalWords_UsesHausaOrdinalWords(int number, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords());
    }
}
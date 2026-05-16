namespace Humanizer.Tests.Localisation.so;

[UseCulture("so")]
public class SomaliOrdinalTests
{
    [Theory]
    [InlineData(1, "1aad")]
    [InlineData(2, "2aad")]
    [InlineData(21, "21aad")]
    public void Ordinalize_UsesSomaliNumericTemplate(int number, string expected)
    {
        Assert.Equal(expected, number.Ordinalize());
        Assert.Equal(expected, number.ToString(CultureInfo.InvariantCulture).Ordinalize());
    }

    [Theory]
    [InlineData(1, "koowaad")]
    [InlineData(2, "labaad")]
    [InlineData(21, "kow iyo labaatanaad")]
    [InlineData(-1, "laga jaray koowaad")]
    public void NumberToOrdinalWords_UsesSomaliOrdinalWords(int number, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords());
    }
}
namespace Humanizer.Tests.Localisation.pt_BR;

[UseCulture("pt-BR")]
public class BrazilianPortugueseRegressionTests
{
    static readonly CultureInfo BrazilianPortuguese = new("pt-BR");

    [Theory]
    [InlineData("dezesseis", 16L)]
    [InlineData("dezessete", 17L)]
    [InlineData("dezenove", 19L)]
    public void WordsToNumber_ParsesBrazilianCardinals(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(BrazilianPortuguese));
        Assert.True(words.TryToNumber(out var result, BrazilianPortuguese));
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("um bilhão", 1_000_000_000L)]
    [InlineData("dois bilhões", 2_000_000_000L)]
    [InlineData("um trilhão", 1_000_000_000_000L)]
    [InlineData("dois trilhões", 2_000_000_000_000L)]
    [InlineData("um quadrilhão", 1_000_000_000_000_000L)]
    [InlineData("dois quadrilhões", 2_000_000_000_000_000L)]
    [InlineData("um quintilhão", 1_000_000_000_000_000_000L)]
    [InlineData("dois quintilhões", 2_000_000_000_000_000_000L)]
    public void WordsToNumber_ParsesBrazilianLargeScaleWords(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(BrazilianPortuguese));
        Assert.True(words.TryToNumber(out var result, BrazilianPortuguese));
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("uma", 1L)]
    [InlineData("duas", 2L)]
    [InlineData("duzentas", 200L)]
    [InlineData("trezentas", 300L)]
    [InlineData("quatrocentas", 400L)]
    [InlineData("quinhentas", 500L)]
    [InlineData("seiscentas", 600L)]
    [InlineData("setecentas", 700L)]
    [InlineData("oitocentas", 800L)]
    [InlineData("novecentas", 900L)]
    [InlineData("duzentas e uma", 201L)]
    [InlineData("primeira", 1L)]
    [InlineData("segunda", 2L)]
    [InlineData("vigésima primeira", 21L)]
    [InlineData("centésima", 100L)]
    [InlineData("centésima décima nona", 119L)]
    [InlineData("1ª", 1L)]
    public void WordsToNumber_PreservesInheritedFeminineForms(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(BrazilianPortuguese));
        Assert.True(words.TryToNumber(out var result, BrazilianPortuguese));
        Assert.Equal(expected, result);
    }

#if NET6_0_OR_GREATER
    [Theory]
    [InlineData(0, 40, "vinte para a uma")]
    [InlineData(0, 45, "quinze para a uma")]
    [InlineData(0, 50, "dez para a uma")]
    [InlineData(0, 55, "cinco para a uma")]
    public void ToClockNotation_UsesSingularArticleForNextHourOne(int hours, int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation());
    }
#endif
}
namespace Humanizer.Tests.Localisation.pt;

[UseCulture("pt")]
public class PortugueseNumberToWordsTests
{
    [Theory]
    [InlineData("mil", 1000)]
    [InlineData("milhar", 1000)]
    [InlineData("milhão", 1_000_000)]
    [InlineData("mil milhões", 1_000_000_000)]
    public void ToNumber_ParsesCardinalWordsToNumber(string words, long expected) =>
        Assert.Equal(expected, words.ToNumber(new CultureInfo("pt")));
}
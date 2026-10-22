namespace Humanizer.Tests.Localisation.es;

[UseCulture("es")]
public class SpanishWordsToNumberTests
{
    static readonly CultureInfo Es = new("es");

    [Theory]
    [InlineData("mil millones", 1000000000)]
    [InlineData("dos mil ciento cuarenta y siete millones cuatrocientos ochenta y tres mil seiscientos cuarenta y siete", 2147483647)]
    public void WordsToNumber_MultipliesSmallerScaleByLargerScale(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Es));
        Assert.True(words.TryToNumber(out var parsed, Es, out var unrecognizedWord));
        Assert.Equal(expected, parsed);
        Assert.Null(unrecognizedWord);
    }

    [Theory]
    [InlineData(1000000000)]
    [InlineData(2147483647)]
    [InlineData(991261547749)]
    public void WordsToNumber_RoundTripsLargeCardinals(long number) =>
        Assert.Equal(number, number.ToWords(Es).ToNumber(Es));
}

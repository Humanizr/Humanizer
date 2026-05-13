namespace Humanizer.Tests.Localisation.ha;

[UseCulture("ha")]
public class HausaNumberToWordsTests
{
    static readonly CultureInfo Ha = new("ha");

    [Theory]
    [InlineData(0, "sifili")]
    [InlineData(5, "biyar")]
    [InlineData(21, "ashirin da ɗaya")]
    [InlineData(99, "casa'in da tara")]
    [InlineData(100, "ɗari ɗaya")]
    [InlineData(105, "ɗari ɗaya da biyar")]
    [InlineData(120, "ɗari ɗaya da ashirin")]
    [InlineData(234, "ɗari biyu da talatin da huɗu")]
    [InlineData(1000, "dubu ɗaya")]
    [InlineData(2021, "dubu biyu da ashirin da ɗaya")]
    [InlineData(1234, "dubu ɗaya ɗari biyu da talatin da huɗu")]
    [InlineData(1000000, "miliyan ɗaya")]
    [InlineData(2000000, "miliyan biyu")]
    public void NumberToWords_ProducesExpectedHausaOutput(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Ha));
    }

    [Theory]
    [InlineData(-5, "debe biyar")]
    [InlineData(-1000, "debe dubu ɗaya")]
    public void NumberToWords_UsesHausaNegativePrefix(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Ha));
    }

    [Fact]
    public void ToWords_HandlesLongMinValueWithoutOverflow()
    {
        var words = long.MinValue.ToWords(Ha);
        Assert.StartsWith("debe kwintiliyan tara", words, StringComparison.Ordinal);
        Assert.Contains("da takwas", words, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(1, "na farko")]
    [InlineData(2, "na biyu")]
    [InlineData(21, "na ashirin da ɗaya")]
    [InlineData(100, "na ɗari ɗaya")]
    [InlineData(-1, "debe na farko")]
    [InlineData(-21, "debe na ashirin da ɗaya")]
    public void NumberToOrdinalWords_ProducesExpectedHausaOutput(int number, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(Ha));
    }

    [Theory]
    [InlineData(21, "ashirin da ɗaya")]
    [InlineData(99, "casa'in da tara")]
    [InlineData(105, "ɗari ɗaya da biyar")]
    [InlineData(120, "ɗari ɗaya da ashirin")]
    [InlineData(1234, "dubu ɗaya ɗari biyu da talatin da huɗu")]
    [InlineData(2021, "dubu biyu da ashirin da ɗaya")]
    [InlineData(2000000, "miliyan biyu")]
    public void WordsToNumber_RoundTripsSupportedHausaCardinals(long number, string words)
    {
        Assert.Equal(number, words.ToNumber(Ha));
        Assert.True(words.TryToNumber(out var parsed, Ha, out var unrecognizedWord));
        Assert.Equal(number, parsed);
        Assert.Null(unrecognizedWord);
    }

    [Theory]
    [InlineData("na farko", 1)]
    [InlineData("na ashirin da ɗaya", 21)]
    [InlineData("debe na farko", -1)]
    [InlineData("debe na ashirin da ɗaya", -21)]
    public void WordsToNumber_ParsesHausaOrdinals(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Ha));
    }
}
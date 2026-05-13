namespace Humanizer.Tests.Localisation.ha;

[UseCulture("ha")]
public class HausaNumberToWordsTests
{
    static readonly CultureInfo Ha = new("ha");

    [Theory]
    [InlineData(0, "sifili")]
    [InlineData(5, "biyar")]
    [InlineData(11, "goma sha ɗaya")]
    [InlineData(17, "goma sha bakwai")]
    [InlineData(21, "ashirin da ɗaya")]
    [InlineData(99, "casa'in da tara")]
    [InlineData(100, "ɗari ɗaya")]
    [InlineData(105, "ɗari ɗaya da biyar")]
    [InlineData(111, "ɗari ɗaya da goma sha ɗaya")]
    [InlineData(120, "ɗari ɗaya da ashirin")]
    [InlineData(234, "ɗari biyu da talatin da huɗu")]
    [InlineData(1000, "dubu ɗaya")]
    [InlineData(2021, "dubu biyu da ashirin da ɗaya")]
    [InlineData(100000, "dubu ɗari ɗaya")]
    [InlineData(20001, "dubu ashirin da kuma ɗaya")]
    [InlineData(21000, "dubu ashirin da ɗaya")]
    [InlineData(100001, "dubu ɗari ɗaya da ɗaya")]
    [InlineData(101000, "dubu ɗari ɗaya dubu ɗaya")]
    [InlineData(120001, "dubu ɗari ɗaya dubu ashirin da kuma ɗaya")]
    [InlineData(121000, "dubu ɗari ɗaya dubu ashirin da ɗaya")]
    [InlineData(123000, "dubu ɗari ɗaya dubu ashirin da uku")]
    [InlineData(1234, "dubu ɗaya ɗari biyu da talatin da huɗu")]
    [InlineData(1000000, "miliyan ɗaya")]
    [InlineData(2000000, "miliyan biyu")]
    [InlineData(100000001, "miliyan ɗari ɗaya da kuma ɗaya")]
    [InlineData(101000000, "miliyan ɗari ɗaya da ɗaya")]
    [InlineData(120000001, "miliyan ɗari ɗaya da ashirin da kuma ɗaya")]
    [InlineData(121000000, "miliyan ɗari ɗaya da ashirin da ɗaya")]
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
    [InlineData(11, "goma sha ɗaya")]
    [InlineData(17, "goma sha bakwai")]
    [InlineData(21, "ashirin da ɗaya")]
    [InlineData(99, "casa'in da tara")]
    [InlineData(105, "ɗari ɗaya da biyar")]
    [InlineData(111, "ɗari ɗaya da goma sha ɗaya")]
    [InlineData(120, "ɗari ɗaya da ashirin")]
    [InlineData(1234, "dubu ɗaya ɗari biyu da talatin da huɗu")]
    [InlineData(2021, "dubu biyu da ashirin da ɗaya")]
    [InlineData(100000, "dubu ɗari ɗaya")]
    [InlineData(20001, "dubu ashirin da kuma ɗaya")]
    [InlineData(21000, "dubu ashirin da ɗaya")]
    [InlineData(100001, "dubu ɗari ɗaya da ɗaya")]
    [InlineData(100023, "dubu ɗari ɗaya da ashirin da uku")]
    [InlineData(101000, "dubu ɗari ɗaya dubu ɗaya")]
    [InlineData(120001, "dubu ɗari ɗaya dubu ashirin da kuma ɗaya")]
    [InlineData(121000, "dubu ɗari ɗaya dubu ashirin da ɗaya")]
    [InlineData(123000, "dubu ɗari ɗaya dubu ashirin da uku")]
    [InlineData(2000000, "miliyan biyu")]
    [InlineData(100000001, "miliyan ɗari ɗaya da kuma ɗaya")]
    [InlineData(101000000, "miliyan ɗari ɗaya da ɗaya")]
    [InlineData(120000001, "miliyan ɗari ɗaya da ashirin da kuma ɗaya")]
    [InlineData(121000000, "miliyan ɗari ɗaya da ashirin da ɗaya")]
    public void WordsToNumber_RoundTripsSupportedHausaCardinals(long number, string words)
    {
        Assert.Equal(number, words.ToNumber(Ha));
        Assert.True(words.TryToNumber(out var parsed, Ha, out var unrecognizedWord));
        Assert.Equal(number, parsed);
        Assert.Null(unrecognizedWord);
    }

    [Theory]
    [InlineData("na farko", 1)]
    [InlineData("na goma sha ɗaya", 11)]
    [InlineData("na ashirin da ɗaya", 21)]
    [InlineData("debe na farko", -1)]
    [InlineData("debe na ashirin da ɗaya", -21)]
    public void WordsToNumber_ParsesHausaOrdinals(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Ha));
    }
}
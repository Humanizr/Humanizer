namespace Humanizer.Tests.Localisation.te;

[UseCulture("te")]
public class TeluguNumberToWordsTests
{
    static readonly CultureInfo Te = new("te");

    [Theory]
    [InlineData(0, "సున్నా")]
    [InlineData(5, "ఐదు")]
    [InlineData(21, "ఇరవై ఒకటి")]
    [InlineData(99, "తొంభై తొమ్మిది")]
    [InlineData(100, "వంద")]
    [InlineData(101, "నూట ఒకటి")]
    [InlineData(200, "రెండు వందలు")]
    [InlineData(201, "రెండు వందల ఒకటి")]
    [InlineData(1000, "వెయ్యి")]
    [InlineData(1001, "వెయ్యి ఒకటి")]
    [InlineData(2000, "రెండు వేలు")]
    [InlineData(2001, "రెండు వేల ఒకటి")]
    [InlineData(100000, "లక్ష")]
    [InlineData(200000, "రెండు లక్షలు")]
    [InlineData(200001, "రెండు లక్షల ఒకటి")]
    [InlineData(10000000, "కోటి")]
    [InlineData(20000000, "రెండు కోట్లు")]
    [InlineData(20000001, "రెండు కోట్ల ఒకటి")]
    [InlineData(12345678, "కోటి ఇరవై మూడు లక్షల నలభై ఐదు వేల ఆరు వందల డెబ్బై ఎనిమిది")]
    [InlineData(1001000001, "అరబ్ పది లక్షల ఒకటి")]
    [InlineData(4325010007018, "నలభై మూడు ఖరబ్ ఇరవై ఐదు అరబ్ కోటి ఏడు వేల పద్దెనిమిది")]
    public void NumberToWords_ProducesExpectedTeluguOutput(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Te));
    }

    [Theory]
    [InlineData(-5, "మైనస్ ఐదు")]
    [InlineData(-100000, "మైనస్ లక్ష")]
    public void NumberToWords_UsesTeluguNegativePrefix(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Te));
    }

    [Theory]
    [InlineData(1, "మొదటి")]
    [InlineData(2, "రెండవ")]
    [InlineData(21, "ఇరవై ఒకటవ")]
    [InlineData(101, "నూట ఒకటవ")]
    [InlineData(200, "రెండు వందలవ")]
    [InlineData(1000, "వెయ్యవ")]
    [InlineData(2000, "రెండు వేలవ")]
    [InlineData(200000, "రెండు లక్షలవ")]
    [InlineData(20000000, "రెండు కోట్లవ")]
    [InlineData(2000000000, "రెండు అరబ్‌వ")]
    public void NumberToOrdinalWords_ProducesExpectedTeluguOutput(int number, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(Te));
    }

    [Theory]
    [InlineData(21, "ఇరవై ఒకటి")]
    [InlineData(101, "నూట ఒకటి")]
    [InlineData(2001, "రెండు వేల ఒకటి")]
    [InlineData(200001, "రెండు లక్షల ఒకటి")]
    [InlineData(20000001, "రెండు కోట్ల ఒకటి")]
    [InlineData(12345678, "కోటి ఇరవై మూడు లక్షల నలభై ఐదు వేల ఆరు వందల డెబ్బై ఎనిమిది")]
    [InlineData(1001000001, "అరబ్ పది లక్షల ఒకటి")]
    [InlineData(4325010007018, "నలభై మూడు ఖరబ్ ఇరవై ఐదు అరబ్ కోటి ఏడు వేల పద్దెనిమిది")]
    public void WordsToNumber_RoundTripsTeluguCardinals(long number, string words)
    {
        Assert.Equal(number, words.ToNumber(Te));
        Assert.True(words.TryToNumber(out var parsed, Te, out var unrecognizedWord));
        Assert.Equal(number, parsed);
        Assert.Null(unrecognizedWord);
    }

    [Theory]
    [InlineData("ఇరవై ఒకటవ", 21)]
    [InlineData("నూట ఒకటవ", 101)]
    [InlineData("రెండు వందలవ", 200)]
    [InlineData("రెండు వేలవ", 2000)]
    [InlineData("రెండు లక్షలవ", 200000)]
    [InlineData("రెండు కోట్లవ", 20000000)]
    [InlineData("రెండు అరబ్‌వ", 2000000000)]
    [InlineData("మైనస్ ఐదవ", -5)]
    public void WordsToNumber_ParsesTeluguOrdinals(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Te));
    }

    [Fact]
    public void WordsToNumber_RoundTripsTeluguLongMinValue()
    {
        var words = long.MinValue.ToWords(Te);

        Assert.Equal(long.MinValue, words.ToNumber(Te));
        Assert.True(words.TryToNumber(out var parsed, Te, out var unrecognizedWord));
        Assert.Equal(long.MinValue, parsed);
        Assert.Null(unrecognizedWord);
    }
}
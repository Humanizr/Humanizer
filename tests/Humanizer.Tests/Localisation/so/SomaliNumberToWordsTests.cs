namespace Humanizer.Tests.Localisation.so;

[UseCulture("so")]
public class SomaliNumberToWordsTests
{
    static readonly CultureInfo So = new("so");

    [Theory]
    [InlineData(0, "eber")]
    [InlineData(5, "shan")]
    [InlineData(10, "toban")]
    [InlineData(11, "kow iyo toban")]
    [InlineData(19, "sagaal iyo toban")]
    [InlineData(21, "kow iyo labaatan")]
    [InlineData(99, "sagaal iyo sagaashan")]
    [InlineData(100, "boqol")]
    [InlineData(105, "boqol iyo shan")]
    [InlineData(120, "boqol iyo labaatan")]
    [InlineData(234, "laba boqol iyo afar iyo soddon")]
    [InlineData(1000, "kun")]
    [InlineData(2021, "laba kun kow iyo labaatan")]
    [InlineData(1234, "kun laba boqol iyo afar iyo soddon")]
    [InlineData(1000000, "milyan")]
    [InlineData(2000000, "laba milyan")]
    public void NumberToWords_ProducesExpectedSomaliOutput(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(So));
    }

    [Theory]
    [InlineData(-5, "laga jaray shan")]
    [InlineData(-1000, "laga jaray kun")]
    public void NumberToWords_UsesSomaliNegativePrefix(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(So));
    }

    [Theory]
    [InlineData(1, "koowaad")]
    [InlineData(2, "labaad")]
    [InlineData(3, "saddexaad")]
    [InlineData(21, "kow iyo labaatanaad")]
    [InlineData(100, "boqolaad")]
    [InlineData(-1, "laga jaray koowaad")]
    public void NumberToOrdinalWords_ProducesExpectedSomaliOutput(int number, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(So));
    }

    [Theory]
    [InlineData(10, "toban")]
    [InlineData(11, "kow iyo toban")]
    [InlineData(19, "sagaal iyo toban")]
    [InlineData(21, "kow iyo labaatan")]
    [InlineData(99, "sagaal iyo sagaashan")]
    [InlineData(105, "boqol iyo shan")]
    [InlineData(120, "boqol iyo labaatan")]
    [InlineData(1234, "kun laba boqol iyo afar iyo soddon")]
    [InlineData(2021, "laba kun kow iyo labaatan")]
    [InlineData(1000000, "milyan")]
    [InlineData(2000000, "laba milyan")]
    public void WordsToNumber_RoundTripsSomaliCardinals(long number, string words)
    {
        Assert.Equal(number, words.ToNumber(So));
        Assert.True(words.TryToNumber(out var parsed, So, out var unrecognizedWord));
        Assert.Equal(number, parsed);
        Assert.Null(unrecognizedWord);
    }

    [Theory]
    [InlineData("koowaad", 1)]
    [InlineData("kow iyo labaatanaad", 21)]
    [InlineData("laga jaray koowaad", -1)]
    public void WordsToNumber_ParsesSomaliOrdinals(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(So));
    }
}
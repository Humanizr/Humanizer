namespace Humanizer.Tests.Localisation.sw;

[UseCulture("sw")]
public class SwahiliNumberToWordsTests
{
    static readonly CultureInfo Sw = new("sw");

    [Theory]
    [InlineData(0, "sifuri")]
    [InlineData(5, "tano")]
    [InlineData(21, "ishirini na moja")]
    [InlineData(99, "tisini na tisa")]
    [InlineData(100, "mia moja")]
    [InlineData(105, "mia moja na tano")]
    [InlineData(120, "mia moja na ishirini")]
    [InlineData(234, "mia mbili na thelathini na nne")]
    [InlineData(1000, "elfu moja")]
    [InlineData(2021, "elfu mbili na ishirini na moja")]
    [InlineData(1234, "elfu moja mia mbili na thelathini na nne")]
    [InlineData(100000, "laki moja")]
    [InlineData(1000000, "milioni moja")]
    [InlineData(2000000, "milioni mbili")]
    public void NumberToWords_ProducesExpectedSwahiliOutput(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Sw));
    }

    [Theory]
    [InlineData(-5, "minus tano")]
    [InlineData(-1000, "minus elfu moja")]
    public void NumberToWords_UsesSwahiliNegativePrefix(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Sw));
    }

    [Fact]
    public void ToWords_HandlesLongMinValueWithoutOverflow()
    {
        var words = long.MinValue.ToWords(Sw);
        Assert.StartsWith("minus kwintilioni tisa", words, StringComparison.Ordinal);
        Assert.Contains("na nane", words, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(1, "kwanza")]
    [InlineData(2, "pili")]
    [InlineData(21, "ya ishirini na moja")]
    [InlineData(100, "ya mia moja")]
    public void NumberToOrdinalWords_ProducesExpectedSwahiliOutput(int number, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(Sw));
    }

    [Theory]
    [InlineData(21, "ishirini na moja")]
    [InlineData(99, "tisini na tisa")]
    [InlineData(105, "mia moja na tano")]
    [InlineData(120, "mia moja na ishirini")]
    [InlineData(1234, "elfu moja mia mbili na thelathini na nne")]
    [InlineData(2021, "elfu mbili na ishirini na moja")]
    [InlineData(100000, "laki moja")]
    [InlineData(2000000, "milioni mbili")]
    public void WordsToNumber_RoundTripsSwahiliCardinals(long number, string words)
    {
        Assert.Equal(number, words.ToNumber(Sw));
        Assert.True(words.TryToNumber(out var parsed, Sw, out var unrecognizedWord));
        Assert.Equal(number, parsed);
        Assert.Null(unrecognizedWord);
    }

    [Theory]
    [InlineData("kwanza", 1)]
    [InlineData("ya ishirini na moja", 21)]
    public void WordsToNumber_ParsesSwahiliOrdinals(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Sw));
    }
}
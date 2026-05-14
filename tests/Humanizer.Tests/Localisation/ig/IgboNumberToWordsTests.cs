namespace Humanizer.Tests.Localisation.ig;

[UseCulture("ig")]
public class IgboNumberToWordsTests
{
    static readonly CultureInfo Ig = new("ig");

    [Theory]
    [InlineData(0, "efu")]
    [InlineData(5, "ise")]
    [InlineData(10, "iri")]
    [InlineData(11, "iri na otu")]
    [InlineData(19, "iri na itoolu")]
    [InlineData(21, "iri abụọ na otu")]
    [InlineData(99, "iri itoolu na itoolu")]
    [InlineData(100, "otu narị")]
    [InlineData(105, "otu narị na ise")]
    [InlineData(120, "otu narị na iri abụọ")]
    [InlineData(234, "abụọ narị na iri atọ na anọ")]
    [InlineData(1000, "otu puku")]
    [InlineData(2021, "abụọ puku na iri abụọ na otu")]
    [InlineData(1234, "otu puku abụọ narị na iri atọ na anọ")]
    [InlineData(1000000, "otu nde")]
    [InlineData(2000000, "abụọ nde")]
    public void NumberToWords_ProducesExpectedIgboOutput(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Ig));
    }

    [Theory]
    [InlineData(-5, "mwepu ise")]
    [InlineData(-1000, "mwepu otu puku")]
    public void NumberToWords_UsesIgboNegativePrefix(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Ig));
    }

    [Theory]
    [InlineData(1, "nke mbụ")]
    [InlineData(2, "nke abụọ")]
    [InlineData(20, "nke iri abụọ")]
    [InlineData(21, "iri abụọ na nke mbụ")]
    [InlineData(-1, "mwepu otu")]
    public void NumberToOrdinalWords_ProducesExpectedIgboOutput(int number, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(Ig));
    }

    [Theory]
    [InlineData(10, "iri")]
    [InlineData(11, "iri na otu")]
    [InlineData(21, "iri abụọ na otu")]
    [InlineData(99, "iri itoolu na itoolu")]
    [InlineData(105, "otu narị na ise")]
    [InlineData(120, "otu narị na iri abụọ")]
    [InlineData(1234, "otu puku abụọ narị na iri atọ na anọ")]
    [InlineData(2021, "abụọ puku na iri abụọ na otu")]
    [InlineData(2000000, "abụọ nde")]
    public void WordsToNumber_RoundTripsIgboCardinals(long number, string words)
    {
        Assert.Equal(number, words.ToNumber(Ig));
        Assert.True(words.TryToNumber(out var parsed, Ig, out var unrecognizedWord));
        Assert.Equal(number, parsed);
        Assert.Null(unrecognizedWord);
    }

    [Theory]
    [InlineData("nke mbụ", 1)]
    [InlineData("nke iri abụọ", 20)]
    [InlineData("iri abụọ na nke mbụ", 21)]
    [InlineData("mwepu otu", -1)]
    public void WordsToNumber_ParsesIgboOrdinals(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Ig));
    }
}
namespace Humanizer.Tests.Localisation.ig;

[UseCulture("ig")]
public class IgboNumberToWordsTests
{
    static CultureInfo CreateIgboCulture() => new("ig");

    [Theory]
    [InlineData(0, "efu")]
    [InlineData(5, "ise")]
    [InlineData(10, "iri")]
    [InlineData(11, "iri na otu")]
    [InlineData(19, "iri na itoolu")]
    [InlineData(21, "iri abụọ na otu")]
    [InlineData(22, "iri abụọ na abụọ")]
    [InlineData(99, "iri itoolu na itoolu")]
    [InlineData(100, "otu narị")]
    [InlineData(105, "otu narị na ise")]
    [InlineData(120, "otu narị na iri abụọ")]
    [InlineData(122, "otu narị na iri abụọ na abụọ")]
    [InlineData(234, "abụọ narị na iri atọ na anọ")]
    [InlineData(1000, "otu puku")]
    [InlineData(2021, "abụọ puku na iri abụọ na otu")]
    [InlineData(1234, "otu puku abụọ narị na iri atọ na anọ")]
    [InlineData(1000000, "otu nde")]
    [InlineData(2000000, "abụọ nde")]
    public void NumberToWords_ProducesExpectedIgboOutput(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(CreateIgboCulture()));
    }

    [Theory]
    [InlineData(-5, "mwepu ise")]
    [InlineData(-1000, "mwepu otu puku")]
    public void NumberToWords_UsesIgboNegativePrefix(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(CreateIgboCulture()));
    }

    // Igbo ordinal compounds are formed with `nke` before the cardinal phrase; `first` uses `mbụ`.
    // Sources: https://elias.fas.harvard.edu/languages/igbo/beginning/5/numbers and
    // https://www.igbovillagesquare.com/2020/12/igbo-numbers-onuogugu.html
    [Theory]
    [InlineData(1, "nke mbụ")]
    [InlineData(2, "nke abụọ")]
    [InlineData(20, "nke iri abụọ")]
    [InlineData(21, "nke iri abụọ na otu")]
    [InlineData(22, "nke iri abụọ na abụọ")]
    [InlineData(23, "nke iri abụọ na atọ")]
    [InlineData(99, "nke iri itoolu na itoolu")]
    [InlineData(100, "nke otu narị")]
    [InlineData(105, "nke otu narị na ise")]
    [InlineData(1001, "nke otu puku na otu")]
    [InlineData(-1, "mwepu nke mbụ")]
    public void NumberToOrdinalWords_ProducesExpectedIgboOutput(int number, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(CreateIgboCulture()));
    }

    [Theory]
    [InlineData(10, "iri")]
    [InlineData(11, "iri na otu")]
    [InlineData(21, "iri abụọ na otu")]
    [InlineData(22, "iri abụọ na abụọ")]
    [InlineData(99, "iri itoolu na itoolu")]
    [InlineData(105, "otu narị na ise")]
    [InlineData(120, "otu narị na iri abụọ")]
    [InlineData(122, "otu narị na iri abụọ na abụọ")]
    [InlineData(1234, "otu puku abụọ narị na iri atọ na anọ")]
    [InlineData(2021, "abụọ puku na iri abụọ na otu")]
    [InlineData(2000000, "abụọ nde")]
    public void WordsToNumber_RoundTripsIgboCardinals(long number, string words)
    {
        Assert.Equal(number, words.ToNumber(CreateIgboCulture()));
        Assert.True(words.TryToNumber(out var parsed, CreateIgboCulture(), out var unrecognizedWord));
        Assert.Equal(number, parsed);
        Assert.Null(unrecognizedWord);
    }

    [Theory]
    [InlineData("na", "na")]
    [InlineData("nke", "nke")]
    [InlineData("na nke", "na")]
    [InlineData("na otu", "na")]
    [InlineData("otu na", "na")]
    [InlineData("otu nke abụọ", "nke")]
    public void WordsToNumber_RejectsDanglingIgnoredIgboPhrases(string words, string expectedUnrecognizedWord)
    {
        Assert.False(words.TryToNumber(out var parsed, CreateIgboCulture(), out var unrecognizedWord));
        Assert.Equal(0, parsed);
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);

        var exception = Assert.Throws<ArgumentException>(() => words.ToNumber(CreateIgboCulture()));
        Assert.Equal($"Unrecognized number word: {expectedUnrecognizedWord}", exception.Message);
    }

    [Theory]
    [InlineData("nke mbụ", 1)]
    [InlineData("nke iri abụọ", 20)]
    [InlineData("nke iri abụọ na otu", 21)]
    [InlineData("nke iri abụọ na abụọ", 22)]
    [InlineData("nke iri abụọ na atọ", 23)]
    [InlineData("nke iri itoolu na itoolu", 99)]
    [InlineData("nke otu narị", 100)]
    [InlineData("nke otu narị na ise", 105)]
    [InlineData("nke otu puku na otu", 1001)]
    [InlineData("mwepu nke mbụ", -1)]
    public void WordsToNumber_ParsesIgboOrdinals(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(CreateIgboCulture()));
    }
}
namespace Humanizer.Tests.Localisation.gu;

[UseCulture("gu")]
public class GujaratiNumberToWordsTests
{
    static readonly CultureInfo Gu = new("gu");

    [Theory]
    [InlineData(0, "શૂન્ય")]
    [InlineData(5, "પાંચ")]
    [InlineData(21, "એકવીસ")]
    [InlineData(37, "સડત્રીસ")]
    [InlineData(79, "ઓગણએંસી")]
    [InlineData(100, "એકસો")]
    [InlineData(101, "એકસો એક")]
    [InlineData(999, "નવસો નવાણું")]
    [InlineData(1000, "એક હજાર")]
    [InlineData(99999, "નવાણું હજાર નવસો નવાણું")]
    [InlineData(100000, "એક લાખ")]
    [InlineData(1234567, "બાર લાખ ચોત્રીસ હજાર પાંચસો સડસઠ")]
    [InlineData(9999999, "નવાણું લાખ નવાણું હજાર નવસો નવાણું")]
    [InlineData(10000000, "એક કરોડ")]
    [InlineData(12345678, "એક કરોડ તેવીસ લાખ પિસ્તાલીસ હજાર છસો અઠ્યોતેર")]
    [InlineData(1000000000, "એક અબજ")]
    [InlineData(100000000000, "એક ખરબ")]
    [InlineData(100000000000000, "દસ નીલ")]
    [InlineData(100000000000000000, "એક શંખ")]
    public void NumberToWords_ProducesExpectedOutput(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Gu));
    }

    [Theory]
    [InlineData(-5, "નકારાત્મક પાંચ")]
    [InlineData(-100000, "નકારાત્મક એક લાખ")]
    public void NumberToWords_UsesGujaratiNegativePrefix(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Gu));
    }

    [Theory]
    [InlineData(21, "એકવીસ")]
    [InlineData(37, "સડત્રીસ")]
    [InlineData(101, "એકસો એક")]
    [InlineData(99999, "નવાણું હજાર નવસો નવાણું")]
    [InlineData(100000, "એક લાખ")]
    [InlineData(10000000, "એક કરોડ")]
    [InlineData(12345678, "એક કરોડ તેવીસ લાખ પિસ્તાલીસ હજાર છસો અઠ્યોતેર")]
    [InlineData(4325010007019, "તેતાલીસ ખરબ પચ્ચીસ અબજ એક કરોડ સાત હજાર ઓગણીસ")]
    [InlineData(100000000000000, "દસ નીલ")]
    [InlineData(100000000000000000, "એક શંખ")]
    public void WordsToNumber_RoundTripsGujaratiCardinals(long number, string words)
    {
        Assert.Equal(number, words.ToNumber(Gu));
        Assert.True(words.TryToNumber(out var parsed, Gu, out var unrecognizedWord));
        Assert.Equal(number, parsed);
        Assert.Null(unrecognizedWord);
    }

    [Theory]
    [InlineData("માઇનસ પાંચ", -5)]
    [InlineData("બસો", 200)]
    [InlineData("નકારાત્મક પાંચ", -5)]
    [InlineData("એક કરોડ તેવીસ લાખ પિસ્તાલીસ હજાર છસો અઠ્યોતેર", 12345678)]
    public void WordsToNumber_AcceptsCommonGujaratiVariants(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Gu));
    }

    [Theory]
    [InlineData("એક દશાંશ બે")]
    [InlineData("નકારાત્મક એક દશાંશ પાંચ")]
    public void WordsToNumber_RejectsUnsupportedGujaratiDecimalPhrases(string words)
    {
        Assert.False(words.TryToNumber(out _, Gu));
        Assert.Throws<ArgumentException>(() => words.ToNumber(Gu));
    }

    [Theory]
    [InlineData(5, "પાંચ")]
    [InlineData(21, "એકવીસ")]
    public void ToTuple_UsesGujaratiNumberWords(int number, string expected)
    {
        Assert.Equal(expected, number.ToTuple(Gu));
    }
}
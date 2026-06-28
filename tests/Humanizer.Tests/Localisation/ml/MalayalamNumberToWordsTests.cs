namespace Humanizer.Tests.Localisation.ml;

[UseCulture("ml")]
public class MalayalamNumberToWordsTests
{
    static readonly CultureInfo Ml = new("ml");

    [Theory]
    [InlineData(0, "പൂജ്യം")]
    [InlineData(1, "ഒന്ന്")]
    [InlineData(5, "അഞ്ച്")]
    [InlineData(21, "ഇരുപത്തിയൊന്ന്")]
    [InlineData(99, "തൊണ്ണൂറ്റിയൊൻപത്")]
    [InlineData(100, "നൂറ്")]
    [InlineData(101, "നൂറ്റിയൊന്ന്")]
    [InlineData(110, "നൂറ്റിപത്ത്")]
    [InlineData(200, "ഇരുനൂറ്")]
    [InlineData(201, "ഇരുനൂറ്റിയൊന്ന്")]
    [InlineData(210, "ഇരുനൂറ്റിപത്ത്")]
    [InlineData(900, "തൊള്ളായിരം")]
    [InlineData(999, "തൊള്ളായിരത്തിതൊണ്ണൂറ്റിയൊൻപത്")]
    [InlineData(1000, "ആയിരം")]
    [InlineData(1001, "ആയിരത്തി ഒന്ന്")]
    [InlineData(100000, "ഒരു ലക്ഷം")]
    [InlineData(10000000, "ഒരു കോടി")]
    [InlineData(100_000_000_000_000_000L, "ഒരു ലക്ഷം ലക്ഷം കോടി")]
    [InlineData(12345678, "ഒരു കോടി ഇരുപത്തിമൂന്ന് ലക്ഷത്തി നാൽപ്പത്തിയഞ്ച് ആയിരത്തി അറുന്നൂറ്റിഎഴുപത്തിയെട്ട്")]
    public void NumberToWords_ProducesExpectedMalayalamOutput(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Ml));
    }

    [Theory]
    [InlineData(-5, "മൈനസ് അഞ്ച്")]
    [InlineData(-100000, "മൈനസ് ഒരു ലക്ഷം")]
    public void NumberToWords_UsesMalayalamNegativePrefix(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Ml));
    }

    [Theory]
    [InlineData(1, "ഒന്നാം")]
    [InlineData(2, "രണ്ടാം")]
    [InlineData(21, "ഇരുപത്തിയൊന്നാം")]
    [InlineData(101, "നൂറ്റിയൊന്നാം")]
    [InlineData(110, "നൂറ്റിപത്താം")]
    [InlineData(200, "ഇരുന്നൂറാം")]
    [InlineData(1000, "ആയിരത്താം")]
    [InlineData(100000, "ഒരു ലക്ഷത്താം")]
    [InlineData(10000000, "ഒരു കോടിയാം")]
    public void NumberToOrdinalWords_ProducesExpectedMalayalamOutput(int number, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(Ml));
    }

    [Theory]
    [InlineData(21, "ഇരുപത്തിയൊന്ന്")]
    [InlineData(101, "നൂറ്റിയൊന്ന്")]
    [InlineData(110, "നൂറ്റിപത്ത്")]
    [InlineData(210, "ഇരുനൂറ്റിപത്ത്")]
    [InlineData(1001, "ആയിരത്തി ഒന്ന്")]
    [InlineData(100000, "ഒരു ലക്ഷം")]
    [InlineData(10000000, "ഒരു കോടി")]
    [InlineData(1_001_000_001, "നൂറ് കോടി പത്ത് ലക്ഷത്തി ഒന്ന്")]
    [InlineData(4_325_010_007_018, "നാല് ലക്ഷം കോടി മുപ്പത്തിരണ്ട് ആയിരം കോടി അഞ്ഞൂറ്റിയൊന്ന് കോടി ഏഴ് ആയിരത്തി പതിനെട്ട്")]
    [InlineData(100_000_000_000_000_000L, "ഒരു ലക്ഷം ലക്ഷം കോടി")]
    [InlineData(12345678, "ഒരു കോടി ഇരുപത്തിമൂന്ന് ലക്ഷത്തി നാൽപ്പത്തിയഞ്ച് ആയിരത്തി അറുന്നൂറ്റിഎഴുപത്തിയെട്ട്")]
    public void WordsToNumber_RoundTripsMalayalamCardinals(long number, string words)
    {
        Assert.Equal(number, words.ToNumber(Ml));
        Assert.True(words.TryToNumber(out var parsed, Ml, out var unrecognizedWord));
        Assert.Equal(number, parsed);
        Assert.Null(unrecognizedWord);
    }

    [Theory]
    [InlineData("ഇരുപത്തിയൊന്നാം", 21)]
    [InlineData("നൂറ്റിയൊന്നാം", 101)]
    [InlineData("നൂറ്റിപത്താം", 110)]
    [InlineData("ഇരുനൂറ്റിപത്താം", 210)]
    [InlineData("ഇരുന്നൂറാം", 200)]
    [InlineData("ആയിരത്താം", 1000)]
    [InlineData("ഒരു ലക്ഷത്താം", 100000)]
    [InlineData("ഒരു കോടിയാം", 10000000)]
    [InlineData("രണ്ട് ആയിരം കോടിയാം", 20_000_000_000)]
    [InlineData("രണ്ട് ലക്ഷം കോടിയാം", 2_000_000_000_000)]
    [InlineData("മൈനസ് അഞ്ചാം", -5)]
    public void WordsToNumber_ParsesMalayalamOrdinals(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Ml));
    }

    [Fact]
    public void WordsToNumber_RoundTripsMalayalamLongMinValue()
    {
        var words = long.MinValue.ToWords(Ml);

        Assert.Equal(long.MinValue, words.ToNumber(Ml));
        Assert.True(words.TryToNumber(out var parsed, Ml, out var unrecognizedWord));
        Assert.Equal(long.MinValue, parsed);
        Assert.Null(unrecognizedWord);
    }

    [Fact]
    public void ToTuple_UsesMalayalamNumberWords()
    {
        Assert.Equal("ഇരുപത്തിയൊന്ന്", 21.ToTuple(Ml));
    }
}
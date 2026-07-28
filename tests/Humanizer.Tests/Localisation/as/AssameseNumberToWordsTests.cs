namespace Humanizer.Tests.Localisation.@as;

[UseCulture("as")]
public class AssameseNumberToWordsTests
{
    static readonly CultureInfo As = new("as");

    [Theory]
    [InlineData(0, "শূণ্য")]
    [InlineData(5, "পাঁচ")]
    [InlineData(21, "একৈছ")]
    [InlineData(79, "ঊনাশী")]
    [InlineData(100, "এশ")]
    [InlineData(101, "এশ এক")]
    [InlineData(999, "নশ নিৰানব্বৈ")]
    [InlineData(1000, "এক হাজাৰ")]
    [InlineData(99999, "নিৰানব্বৈ হাজাৰ নশ নিৰানব্বৈ")]
    [InlineData(100000, "এক লাখ")]
    [InlineData(1234567, "বাৰ লাখ চৌত্ৰিশ হাজাৰ পাঁচশ সাতষষ্ঠি")]
    [InlineData(10000000, "এক কোটি")]
    [InlineData(12345678, "এক কোটি তেইছ লাখ পঁয়তাল্লিশ হাজাৰ ছয়শ আঠসত্তৰ")]
    [InlineData(1000000000, "এশ কোটি")]
    [InlineData(100000000000, "এক মহাখৰ্ব")]
    public void NumberToWords_ProducesExpectedOutput(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(As));
    }

    [Theory]
    [InlineData(-5, "ঋণাত্মক পাঁচ")]
    [InlineData(-100000, "ঋণাত্মক এক লাখ")]
    public void NumberToWords_UsesAssameseNegativePrefix(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(As));
    }

    [Theory]
    [InlineData(21, "একৈছ")]
    [InlineData(101, "এশ এক")]
    [InlineData(99999, "নিৰানব্বৈ হাজাৰ নশ নিৰানব্বৈ")]
    [InlineData(100000, "এক লাখ")]
    [InlineData(10000000, "এক কোটি")]
    [InlineData(12345678, "এক কোটি তেইছ লাখ পঁয়তাল্লিশ হাজাৰ ছয়শ আঠসত্তৰ")]
    public void WordsToNumber_RoundTripsAssameseCardinals(long number, string words)
    {
        Assert.Equal(number, words.ToNumber(As));
        Assert.True(words.TryToNumber(out var parsed, As, out var unrecognizedWord));
        Assert.Equal(number, parsed);
        Assert.Null(unrecognizedWord);
    }

    [Theory]
    [InlineData("ঋণাত্মক পাঁচ", -5)]
    [InlineData("পঁচিশ", 25)]
    [InlineData("প্ৰথম", 1)]
    [InlineData("একৈছতম", 21)]
    [InlineData("লাখতম", 100000)]
    public void WordsToNumber_AcceptsAssameseOrdinalAndVariantTokens(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(As));
    }

    [Theory]
    [InlineData("এক দশমিক দুই")]
    [InlineData("ঋণাত্মক এক দশমিক পাঁচ")]
    public void WordsToNumber_RejectsUnsupportedAssameseDecimalPhrases(string words)
    {
        Assert.False(words.TryToNumber(out _, As));
        Assert.Throws<ArgumentException>(() => words.ToNumber(As));
    }

    [Theory]
    [InlineData(5, "পাঁচ")]
    [InlineData(21, "একৈছ")]
    public void ToTuple_UsesAssameseNumberWords(int number, string expected)
    {
        Assert.Equal(expected, number.ToTuple(As));
    }
}
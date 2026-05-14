namespace Humanizer.Tests.Localisation.ne;

[UseCulture("ne")]
public class NepaliNumberToWordsTests
{
    static readonly CultureInfo Ne = new("ne");

    [Theory]
    [InlineData(0, "शून्य")]
    [InlineData(5, "पाँच")]
    [InlineData(21, "एक्काइस")]
    [InlineData(79, "उनासी")]
    [InlineData(100, "एक सय")]
    [InlineData(101, "एक सय एक")]
    [InlineData(999, "नौ सय उनान्सय")]
    [InlineData(1000, "एक हजार")]
    [InlineData(99999, "उनान्सय हजार नौ सय उनान्सय")]
    [InlineData(100000, "एक लाख")]
    [InlineData(1234567, "बाह्र लाख चौँतीस हजार पाँच सय सतसट्ठी")]
    [InlineData(9999999, "उनान्सय लाख उनान्सय हजार नौ सय उनान्सय")]
    [InlineData(10000000, "एक करोड")]
    [InlineData(12345678, "एक करोड तेइस लाख पैँतालीस हजार छ सय अठहत्तर")]
    [InlineData(1000000000, "एक अर्ब")]
    [InlineData(100000000000, "एक खर्ब")]
    public void NumberToWords_ProducesExpectedOutput(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Ne));
    }

    [Theory]
    [InlineData(-5, "माइनस पाँच")]
    [InlineData(-100000, "माइनस एक लाख")]
    public void NumberToWords_UsesNepaliNegativePrefix(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Ne));
    }

    [Theory]
    [InlineData(21, "एक्काइस")]
    [InlineData(101, "एक सय एक")]
    [InlineData(99999, "उनान्सय हजार नौ सय उनान्सय")]
    [InlineData(100000, "एक लाख")]
    [InlineData(10000000, "एक करोड")]
    [InlineData(12345678, "एक करोड तेइस लाख पैँतालीस हजार छ सय अठहत्तर")]
    [InlineData(4325010007010, "त्रिचालिस खर्ब पच्चिस अर्ब एक करोड सात हजार दस")]
    public void WordsToNumber_RoundTripsNepaliCardinals(long number, string words)
    {
        Assert.Equal(number, words.ToNumber(Ne));
    }

    [Theory]
    [InlineData("माइनस पाँच", -5)]
    [InlineData("ऋणात्मक पाँच", -5)]
    [InlineData("एक अरब तेइस लाख पैँतालीस हजार छ सय अठहत्तर", 1002345678)]
    [InlineData("शुन्य", 0)]
    [InlineData("सुन्ना", 0)]
    public void WordsToNumber_AcceptsCommonNepaliVariants(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Ne));
    }

    [Theory]
    [InlineData("एक दशमलव दुई")]
    [InlineData("माइनस एक दशमलव पाँच")]
    public void WordsToNumber_RejectsUnsupportedNepaliDecimalPhrases(string words)
    {
        Assert.False(words.TryToNumber(out _, Ne));
        Assert.Throws<ArgumentException>(() => words.ToNumber(Ne));
    }

    [Theory]
    [InlineData(5, "पाँच")]
    [InlineData(21, "एक्काइस")]
    public void ToTuple_UsesNepaliNumberWords(int number, string expected)
    {
        Assert.Equal(expected, number.ToTuple(Ne));
    }
}
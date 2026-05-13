namespace Humanizer.Tests.Localisation.hi;

[UseCulture("hi")]
public class HindiNumberToWordsTests
{
    static readonly CultureInfo Hi = new("hi");

    [Theory]
    [InlineData(0, "शून्य")]
    [InlineData(5, "पाँच")]
    [InlineData(21, "इक्कीस")]
    [InlineData(79, "उन्यासी")]
    [InlineData(100, "एक सौ")]
    [InlineData(101, "एक सौ एक")]
    [InlineData(999, "नौ सौ निन्यानवे")]
    [InlineData(1000, "एक हज़ार")]
    [InlineData(99999, "निन्यानवे हज़ार नौ सौ निन्यानवे")]
    [InlineData(100000, "एक लाख")]
    [InlineData(1234567, "बारह लाख चौंतीस हज़ार पाँच सौ सड़सठ")]
    [InlineData(9999999, "निन्यानवे लाख निन्यानवे हज़ार नौ सौ निन्यानवे")]
    [InlineData(10000000, "एक करोड़")]
    [InlineData(12345678, "एक करोड़ तेईस लाख पैंतालीस हज़ार छह सौ अठहत्तर")]
    [InlineData(1000000000, "एक अरब")]
    [InlineData(100000000000, "एक खरब")]
    public void NumberToWords_ProducesExpectedOutput(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Hi));
    }

    [Theory]
    [InlineData(-5, "ऋणात्मक पाँच")]
    [InlineData(-100000, "ऋणात्मक एक लाख")]
    public void NumberToWords_UsesHindiNegativePrefix(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Hi));
    }

    [Theory]
    [InlineData(21, "इक्कीस")]
    [InlineData(101, "एक सौ एक")]
    [InlineData(99999, "निन्यानवे हज़ार नौ सौ निन्यानवे")]
    [InlineData(100000, "एक लाख")]
    [InlineData(10000000, "एक करोड़")]
    [InlineData(12345678, "एक करोड़ तेईस लाख पैंतालीस हज़ार छह सौ अठहत्तर")]
    [InlineData(4325010007018, "तैंतालीस खरब पच्चीस अरब एक करोड़ सात हज़ार अठारह")]
    public void WordsToNumber_RoundTripsHindiCardinals(long number, string words)
    {
        Assert.Equal(number, words.ToNumber(Hi));
    }

    [Theory]
    [InlineData("माइनस पांच", -5)]
    [InlineData("ऋणात्मक पाँच", -5)]
    [InlineData("एक करोड तेईस लाख पैंतालीस हजार छह सौ अठहत्तर", 12345678)]
    [InlineData("उनासी", 79)]
    public void WordsToNumber_AcceptsCommonHindiVariants(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Hi));
    }

    [Theory]
    [InlineData("एक दशमलव दो")]
    [InlineData("ऋणात्मक एक दशमलव पाँच")]
    public void WordsToNumber_RejectsUnsupportedHindiDecimalPhrases(string words)
    {
        Assert.False(words.TryToNumber(out _, Hi));
        Assert.Throws<ArgumentException>(() => words.ToNumber(Hi));
    }

    [Theory]
    [InlineData(5, "पाँच")]
    [InlineData(21, "इक्कीस")]
    public void ToTuple_UsesHindiNumberWords(int number, string expected)
    {
        Assert.Equal(expected, number.ToTuple(Hi));
    }
}
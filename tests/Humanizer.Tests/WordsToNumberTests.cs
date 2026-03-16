namespace Humanizer.Tests;

[UseCulture("en-US")]
public class WordsToNumberTests_US
{
    [Theory]
    [InlineData("zero", 0)]
    [InlineData("one", 1)]
    [InlineData("minus five", -5)]
    [InlineData("eleven", 11)]
    [InlineData("ninety five", 95)]
    [InlineData("hundred five", 105)]
    [InlineData("one hundred ninety six", 196)]
    [InlineData("minus one hundred and five", -105)]
    [InlineData("seventeenth", 17)]
    [InlineData("thirtieth", 30)]
    [InlineData("twenty-seventh", 27)]
    [InlineData("thirty-first", 31)]
    [InlineData("minus twenty-first", -21)]
    [InlineData("two thousand twenty three", 2023)]
    [InlineData("one million two hundred thirty four thousand five hundred sixty seven", 1234567)]
    [InlineData("one hundred and third", 103)]
    [InlineData("two hundred and first", 201)]
    [InlineData("five thousand and ninth", 5009)]
    [InlineData("17th", 17)]
    [InlineData("31st", 31)]
    [InlineData("100th", 100)]
    [InlineData("203rd", 203)]
    [InlineData("minus 21st", -21)]
    [InlineData("negative five", -5)]
    [InlineData("negative one hundred and five", -105)]
    [InlineData("negative twenty-first", -21)]
    public void ToNumber_US(string words, int expectedNumber) => Assert.Equal(expectedNumber, words.ToNumber(CultureInfo.CurrentCulture));

    [Theory]
    [InlineData("zero", 0, null)]
    [InlineData("one", 1, null)]
    [InlineData("minus five", -5, null)]
    [InlineData("eleven", 11, null)]
    [InlineData("ninety five", 95, null)]
    [InlineData("hundred five", 105, null)]
    [InlineData("one hundred ninety six", 196, null)]
    [InlineData("minus one hundred and five", -105, null)]
    [InlineData("seventeenth", 17, null)]
    [InlineData("thirtieth", 30, null)]
    [InlineData("twenty-seventh", 27, null)]
    [InlineData("thirty-first", 31, null)]
    [InlineData("minus twenty-first", -21, null)]
    [InlineData("two thousand twenty three", 2023, null)]
    [InlineData("one million two hundred thirty four thousand five hundred sixty seven", 1234567, null)]
    [InlineData("one hundred and third", 103, null)]
    [InlineData("two hundred and first", 201, null)]
    [InlineData("five thousand and ninth", 5009, null)]
    [InlineData("17th", 17, null)]
    [InlineData("31st", 31, null)]
    [InlineData("100th", 100, null)]
    [InlineData("203rd", 203, null)]
    [InlineData("minus 21st", -21, null)]
    [InlineData("negative five", -5, null)]
    [InlineData("negative one hundred and five", -105, null)]
    [InlineData("negative twenty-first", -21, null)]
    public void TryToNumber_ValidInput_US(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(unrecognizedWord, expectedUnrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }

    [Theory]
    [InlineData("twenty nine hello", 0, "hello")]
    [InlineData("mister three", 0, "mister")]
    [InlineData("tenn", 0, "tenn")]
    [InlineData("twenty sveen", 0, "sveen")]
    [InlineData("minus fift five", 0, "fift")]
    [InlineData("sixty two j", 0, "j")]
    [InlineData("two hundred , ninetyy sevennn", 0, "ninetyy")]
    [InlineData("invalidinput", 0, "invalidinput")]
    [InlineData("30rmd", 0, "30rmd")]
    [InlineData("negative energy", 0, "energy")]
    public void TryToNumber_InvalidInput_US(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.False(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(unrecognizedWord, expectedUnrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }

}

[UseCulture("en-GB")]
public class WordsToNumberTests_GB
{
    [Theory]
    [InlineData("zero", 0, null)]
    [InlineData("one", 1, null)]
    [InlineData("minus five", -5, null)]
    [InlineData("eleven", 11, null)]
    [InlineData("ninety five", 95, null)]
    [InlineData("hundred five", 105, null)]
    [InlineData("one hundred ninety six", 196, null)]
    [InlineData("minus one hundred and five", -105, null)]
    [InlineData("seventeenth", 17, null)]
    [InlineData("thirtieth", 30, null)]
    [InlineData("twenty-seventh", 27, null)]
    [InlineData("thirty-first", 31, null)]
    [InlineData("minus twenty-first", -21, null)]
    [InlineData("two thousand twenty three", 2023, null)]
    [InlineData("one million two hundred thirty four thousand five hundred sixty seven", 1234567, null)]
    [InlineData("one hundred and third", 103, null)]
    [InlineData("two hundred and first", 201, null)]
    [InlineData("five thousand and ninth", 5009, null)]
    [InlineData("17th", 17, null)]
    [InlineData("31st", 31, null)]
    [InlineData("100th", 100, null)]
    [InlineData("203rd", 203, null)]
    [InlineData("minus 21st", -21, null)]
    [InlineData("negative five", -5, null)]
    [InlineData("negative one hundred and five", -105, null)]
    [InlineData("negative twenty-first", -21, null)]
    public void TryToNumber_ValidInput_GB(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.True(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(unrecognizedWord, expectedUnrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }

    [Theory]
    [InlineData("twenty nine hello", 0, "hello")]
    [InlineData("mister three", 0, "mister")]
    [InlineData("tenn", 0, "tenn")]
    [InlineData("twenty sveen", 0, "sveen")]
    [InlineData("minus fift five", 0, "fift")]
    [InlineData("sixty two j", 0, "j")]
    [InlineData("two hundred , ninetyy sevennn", 0, "ninetyy")]
    [InlineData("invalidinput", 0, "invalidinput")]
    [InlineData("30rmd", 0, "30rmd")]
    [InlineData("negative energy", 0, "energy")]
    public void TryToNumber_InvalidInput_GB(string words, int expectedNumber, string? expectedUnrecognizedWord)
    {
        Assert.False(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(unrecognizedWord, expectedUnrecognizedWord);
        Assert.Equal(expectedNumber, parsedNumber);
    }

}
public class WordsToNumberTests_NonEnglish
{
    [Theory]
    [InlineData("es-ES", 21)]
    [InlineData("fr-FR", 42)]
    [InlineData("de-DE", 105)]
    [InlineData("pt-BR", 99)]
    [InlineData("ru-RU", 73)]
    [InlineData("ar", 18)]
    [InlineData("da-DK", 21)]
    [InlineData("fil-PH", 21)]
    [InlineData("id-ID", 21)]
    [InlineData("zh-CN", 12)]
    [InlineData("ja-JP", 31)]
    [InlineData("ms-MY", 21)]
    [InlineData("sk-SK", 21)]
    [InlineData("tr-TR", 64)]
    public void RoundTripsSupportedLocalizedWords(string cultureName, int number)
    {
        var culture = new CultureInfo(cultureName);
        var word = number.ToWords(culture);

        Assert.True(word.TryToNumber(out var parsedNumber, culture, out var unrecognizedWord));
        Assert.Equal(number, parsedNumber);
        Assert.Null(unrecognizedWord);
        Assert.Equal(number, word.ToNumber(culture));
    }

    [Theory]
    [InlineData("pt-BR", "123", 123)]
    [InlineData("zh-CN", "-42", -42)]
    public void Accepts_numeric_literals_for_supported_localized_cultures(string cultureName, string input, int expected)
    {
        var culture = new CultureInfo(cultureName);

        Assert.True(input.TryToNumber(out var parsedNumber, culture));
        Assert.Equal(expected, parsedNumber);
        Assert.Equal(expected, input.ToNumber(culture));
    }

    [Theory]
    [InlineData("pt-BR", 10001)]
    [InlineData("ru-RU", -10001)]
    public void Rejects_localized_round_trips_outside_supported_range(string cultureName, int number)
    {
        var culture = new CultureInfo(cultureName);
        var word = number.ToWords(culture);

        Assert.False(word.TryToNumber(out var parsedNumber, culture, out var unrecognizedWord));
        Assert.Equal(0, parsedNumber);
        Assert.False(string.IsNullOrWhiteSpace(unrecognizedWord));

        var ex = Assert.Throws<ArgumentException>(() => word.ToNumber(culture));
        Assert.Contains("round-trip values from -10000 to 10000", ex.Message);
    }

    [Theory]
    [InlineData("eo", "one")]
    [InlineData("ga-IE", "one")]
    public void ThrowsForUnsupportedCultureWithoutEnglishFallback(string cultureName, string word)
    {
        var culture = new CultureInfo(cultureName);

        var ex = Assert.Throws<NotSupportedException>(() => word.ToNumber(culture));
        Assert.Contains($"'{culture.Name}'", ex.Message);

        Assert.Throws<NotSupportedException>(() => word.TryToNumber(out _, culture));
    }
}

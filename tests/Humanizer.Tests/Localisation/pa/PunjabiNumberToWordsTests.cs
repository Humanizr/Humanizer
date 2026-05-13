namespace Humanizer.Tests.Localisation.pa;

[UseCulture("pa")]
public class PunjabiNumberToWordsTests
{
    static readonly CultureInfo Pa = new("pa");

    [Theory]
    [InlineData(0, "ਸਿਫ਼ਰ")]
    [InlineData(5, "ਪੰਜ")]
    [InlineData(21, "ਇੱਕੀ")]
    [InlineData(79, "ਉਨਾਸੀ")]
    [InlineData(100, "ਇੱਕ ਸੌ")]
    [InlineData(101, "ਇੱਕ ਸੌ ਇੱਕ")]
    [InlineData(999, "ਨੌਂ ਸੌ ਨੜਿਨਵੇਂ")]
    [InlineData(1000, "ਇੱਕ ਹਜ਼ਾਰ")]
    [InlineData(99999, "ਨੜਿਨਵੇਂ ਹਜ਼ਾਰ ਨੌਂ ਸੌ ਨੜਿਨਵੇਂ")]
    [InlineData(100000, "ਇੱਕ ਲੱਖ")]
    [InlineData(1234567, "ਬਾਰਾਂ ਲੱਖ ਚੌਂਤੀ ਹਜ਼ਾਰ ਪੰਜ ਸੌ ਸਤਾਹਠ")]
    [InlineData(9999999, "ਨੜਿਨਵੇਂ ਲੱਖ ਨੜਿਨਵੇਂ ਹਜ਼ਾਰ ਨੌਂ ਸੌ ਨੜਿਨਵੇਂ")]
    [InlineData(10000000, "ਇੱਕ ਕਰੋੜ")]
    [InlineData(12345678, "ਇੱਕ ਕਰੋੜ ਤੇਈ ਲੱਖ ਪੰਤਾਲੀ ਹਜ਼ਾਰ ਛੇ ਸੌ ਅਠੱਤਰ")]
    [InlineData(1000000000, "ਇੱਕ ਅਰਬ")]
    [InlineData(100000000000, "ਇੱਕ ਖਰਬ")]
    public void NumberToWords_ProducesExpectedOutput(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Pa));
    }

    [Theory]
    [InlineData(-5, "ਰਿਣਾਤਮਕ ਪੰਜ")]
    [InlineData(-100000, "ਰਿਣਾਤਮਕ ਇੱਕ ਲੱਖ")]
    public void NumberToWords_UsesPunjabiNegativePrefix(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Pa));
    }

    [Theory]
    [InlineData(21, "ਇੱਕੀ")]
    [InlineData(101, "ਇੱਕ ਸੌ ਇੱਕ")]
    [InlineData(99999, "ਨੜਿਨਵੇਂ ਹਜ਼ਾਰ ਨੌਂ ਸੌ ਨੜਿਨਵੇਂ")]
    [InlineData(100000, "ਇੱਕ ਲੱਖ")]
    [InlineData(10000000, "ਇੱਕ ਕਰੋੜ")]
    [InlineData(12345678, "ਇੱਕ ਕਰੋੜ ਤੇਈ ਲੱਖ ਪੰਤਾਲੀ ਹਜ਼ਾਰ ਛੇ ਸੌ ਅਠੱਤਰ")]
    [InlineData(4_325_010_007_018, "ਤਰਤਾਲੀ ਖਰਬ ਪੱਚੀ ਅਰਬ ਇੱਕ ਕਰੋੜ ਸੱਤ ਹਜ਼ਾਰ ਅਠਾਰਾਂ")]
    public void WordsToNumber_RoundTripsPunjabiCardinals(long number, string words)
    {
        Assert.Equal(number, words.ToNumber(Pa));
    }

    [Theory]
    [InlineData("ਮਾਇਨਸ ਪੰਜ", -5)]
    [InlineData("ਰਿਣਾਤਮਕ ਪੰਜ", -5)]
    [InlineData("ਇੱਕ ਕਰੋੜ ਤੇਈ ਲੱਖ ਪੰਤਾਲੀ ਹਜ਼ਾਰ ਛੇ ਸੌ ਅਠੱਤਰ", 12345678)]
    [InlineData("ਚੁਰੰਜਾ", 54)]
    public void WordsToNumber_AcceptsCommonPunjabiVariants(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Pa));
    }

    [Theory]
    [InlineData("ਇੱਕ ਦਸ਼ਮਲਵ ਦੋ")]
    [InlineData("ਰਿਣਾਤਮਕ ਇੱਕ ਦਸ਼ਮਲਵ ਪੰਜ")]
    public void WordsToNumber_RejectsUnsupportedPunjabiDecimalPhrases(string words)
    {
        Assert.False(words.TryToNumber(out _, Pa));
        Assert.Throws<ArgumentException>(() => words.ToNumber(Pa));
    }

    [Theory]
    [InlineData(5, "ਪੰਜ")]
    [InlineData(21, "ਇੱਕੀ")]
    public void ToTuple_UsesPunjabiNumberWords(int number, string expected)
    {
        Assert.Equal(expected, number.ToTuple(Pa));
    }
}
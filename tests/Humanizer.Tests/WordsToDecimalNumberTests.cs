namespace Humanizer.Tests;

[UseCulture("en-US")]
public class WordsToDecimalNumberTests
{
    [Theory]
    [InlineData("one point two", "1.2")]
    [InlineData("three point one four", "3.14")]
    [InlineData("zero point zero five", "0.05")]
    [InlineData("one point five zero", "1.50")]
    [InlineData("minus two point five", "-2.5")]
    [InlineData("minus zero point five", "-0.5")]
    [InlineData("negative point zero five", "-0.05")]
    [InlineData("one hundred and five point zero", "105.0")]
    [InlineData("point five", "0.5")]
    public void ParsesEnglishDecimalPhrasesAndPreservesScale(string words, string expected)
    {
        var parsed = words.ToDecimalNumber(CultureInfo.CurrentCulture);

        Assert.Equal(expected, parsed.ToString(CultureInfo.InvariantCulture));
    }

    [Theory]
    [InlineData("one", "one")]
    [InlineData("one point", "point")]
    [InlineData("point", "point")]
    [InlineData("one point two point three", "point")]
    [InlineData("one point two and three", "and")]
    [InlineData("one point ten", "ten")]
    [InlineData("one point 2", "2")]
    [InlineData("one dot two", "one dot two")]
    public void RejectsMalformedDecimalPhrases(string words, string expectedUnrecognizedWord)
    {
        var success = words.TryToDecimalNumber(
            out var parsed,
            CultureInfo.CurrentCulture,
            out var unrecognizedWord);

        Assert.False(success);
        Assert.Equal(0m, parsed);
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Throws<FormatException>(() => words.ToDecimalNumber(CultureInfo.CurrentCulture));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void TryToDecimalNumberReturnsFalseForEmptyInput(string words)
    {
        Assert.False(words.TryToDecimalNumber(out var parsed, CultureInfo.CurrentCulture));
        Assert.Equal(0m, parsed);
    }

    [Fact]
    public void NullInputUsesTryAndThrowConventions()
    {
        string words = null!;

        Assert.False(words.TryToDecimalNumber(out var parsed, CultureInfo.CurrentCulture));
        Assert.Equal(0m, parsed);
        Assert.Throws<ArgumentNullException>(() => words.ToDecimalNumber(CultureInfo.CurrentCulture));
    }

    [Fact]
    public void RejectsFractionBeyondDecimalScale()
    {
        var words = $"zero point {string.Join(" ", Enumerable.Repeat("one", 29))}";

        Assert.False(words.TryToDecimalNumber(
            out var parsed,
            CultureInfo.CurrentCulture,
            out var unrecognizedWord));
        Assert.Equal(0m, parsed);
        Assert.Equal("one", unrecognizedWord);
    }

    [Fact]
    public void RejectsValueThatCannotPreserveItsAuthoredScale()
    {
        var words = $"{long.MaxValue.ToWords(CultureInfo.CurrentCulture)} point one two three four five six seven eight nine zero";

        Assert.False(words.TryToDecimalNumber(out var parsed, CultureInfo.CurrentCulture));
        Assert.Equal(0m, parsed);
        Assert.Throws<FormatException>(() => words.ToDecimalNumber(CultureInfo.CurrentCulture));
    }

    [Fact]
    public void PreservesNegativeZeroSignAndScale()
    {
        var parsed = "minus zero point zero".ToDecimalNumber(CultureInfo.CurrentCulture);
        var bits = decimal.GetBits(parsed);

        Assert.Equal(1, bits[3] >> 16 & 0x7F);
        Assert.NotEqual(0, bits[3] & int.MinValue);
    }

    [Fact]
    public void UnsupportedCultureUsesTryAndThrowConventions()
    {
        var culture = new CultureInfo("fr-FR");

        Assert.False("one point two".TryToDecimalNumber(
            out var parsed,
            culture,
            out var unrecognizedWord));
        Assert.Equal(0m, parsed);
        Assert.Equal("one point two", unrecognizedWord);
        Assert.Throws<NotSupportedException>(() => "one point two".ToDecimalNumber(culture));
    }

    [Fact]
    public void IntegerToNumberSemanticsRemainUnchanged()
    {
        Assert.False("one point two".TryToNumber(
            out var parsed,
            CultureInfo.CurrentCulture,
            out var unrecognizedWord));
        Assert.Equal(0L, parsed);
        Assert.Equal("point", unrecognizedWord);
    }
}
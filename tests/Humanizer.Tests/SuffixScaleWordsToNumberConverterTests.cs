namespace Humanizer.Tests;

/// <summary>
/// Covers <see cref="SuffixScaleWordsToNumberConverter"/> branches exercised through
/// the Finnish (fi-FI) locale, which is the only locale using the suffix-scale engine.
/// </summary>
[UseCulture("fi-FI")]
public class SuffixScaleWordsToNumberConverterTests
{
    // --- Empty / whitespace input throws ArgumentException (lines 32-34) ---

    [Fact]
    public void Convert_EmptyInput_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => "".ToNumber(CultureInfo.CurrentCulture));
    }

    [Fact]
    public void Convert_WhitespaceInput_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => "   ".ToNumber(CultureInfo.CurrentCulture));
    }

    // --- Convert throws on unrecognized word (lines 17-19) ---

    [Fact]
    public void Convert_UnrecognizedWord_ThrowsArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(() => "yksi foobar".ToNumber(CultureInfo.CurrentCulture));
        Assert.Contains("foobar", ex.Message);
    }

    // --- Numeric digit parse path (lines 37-40) ---

    [Theory]
    [InlineData("0", 0)]
    [InlineData("42", 42)]
    [InlineData("-7", -7)]
    public void ToNumber_NumericString_ParsesDirectly(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(CultureInfo.CurrentCulture));
    }

    // --- TryConvert two-parameter overload without unrecognizedWord (line 27) ---

    [Fact]
    public void TryToNumber_TwoParamOverload_ValidInput_ReturnsTrue()
    {
        Assert.True("yksi".TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture));
        Assert.Equal(1, parsedNumber);
    }

    [Fact]
    public void TryToNumber_TwoParamOverload_InvalidInput_ReturnsFalse()
    {
        Assert.False("foobar".TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture));
        Assert.Equal(0, parsedNumber);
    }

    // --- Negative prefix loop (lines 46-56) ---

    [Theory]
    [InlineData("miinus yksi", -1)]
    [InlineData("miinus kolme", -3)]
    [InlineData("miinus kymmenen", -10)]
    public void ToNumber_NegativePrefix_ParsesCorrectly(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(CultureInfo.CurrentCulture));
    }

    // --- Basic cardinal map lookups ---

    [Theory]
    [InlineData("nolla", 0)]
    [InlineData("yksi", 1)]
    [InlineData("kaksi", 2)]
    [InlineData("kolme", 3)]
    [InlineData("kymmenen", 10)]
    [InlineData("yksitoista", 11)]
    [InlineData("kaksitoista", 12)]
    [InlineData("kolmetoista", 13)]
    public void ToNumber_CardinalValues(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(CultureInfo.CurrentCulture));
    }

    // --- Hundred singular token: sata + remainder (lines 164-172) ---

    [Theory]
    [InlineData("satayksi", 101)]
    [InlineData("satakaksi", 102)]
    [InlineData("sataviisi", 105)]
    [InlineData("satakymmenen", 110)]
    public void ToNumber_HundredSingular_WithRemainder(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(CultureInfo.CurrentCulture));
    }

    // --- Hundred plural token: multiplier + sataa (lines 174-189) ---

    [Theory]
    [InlineData("kaksisataa", 200)]
    [InlineData("kolmesataayksi", 301)]
    [InlineData("viisisataaviisi", 505)]
    public void ToNumber_HundredPlural_WithMultiplier(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(CultureInfo.CurrentCulture));
    }

    // --- Bare scale as standalone word ---

    [Theory]
    [InlineData("sata", 100)]
    [InlineData("tuhat", 1000)]
    [InlineData("miljoona", 1000000)]
    [InlineData("miljardi", 1000000000)]
    public void ToNumber_BareScaleStandalone(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(CultureInfo.CurrentCulture));
    }

    // --- Bare-scale lookahead: segment < 1000 followed by scale >= 1000 (lines 100-112) ---

    [Theory]
    [InlineData("kolme miljoonaa", 3000000)]
    [InlineData("kaksi miljardia", 2000000000)]
    [InlineData("viisi tuhatta", 5000)]
    [InlineData("kolme tuhatta yksi", 3001)]
    [InlineData("kaksi miljoonaa kolme tuhatta", 2003000)]
    public void ToNumber_BareScaleLookahead(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(CultureInfo.CurrentCulture));
    }

    // --- Scale compound: singular scale prefix (tuhat + remainder) (lines 139-147) ---

    [Fact]
    public void ToNumber_ScaleSingularCompound()
    {
        // "tuhatyksi" = tuhat (1000) + yksi (1) = 1001
        Assert.Equal(1001, "tuhatyksi".ToNumber(CultureInfo.CurrentCulture));
    }

    // --- Scale compound: plural scale (multiplier + tuhatta) (lines 149-161) ---

    [Theory]
    [InlineData("kaksituhatta", 2000)]
    [InlineData("kolmetuhattayksi", 3001)]
    public void ToNumber_ScalePluralCompound(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(CultureInfo.CurrentCulture));
    }

    // --- Multi-segment complex numbers ---

    [Theory]
    [InlineData("tuhat kaksi", 1002)]
    [InlineData("yksi miljoona", 1000000)]
    public void ToNumber_ComplexMultiSegment(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(CultureInfo.CurrentCulture));
    }

    // --- Lookahead pushback: segment < 1000 followed by non-scale token (line 111) ---

    [Fact]
    public void ToNumber_LookaheadPushback_NonScaleToken()
    {
        // "sata kaksi" = two tokens: "sata" (100 from bareScaleMap) + "kaksi" (2 from cardinalMap)
        // The first token "sata" is < 1000, triggers lookahead, reads "kaksi", which is not in
        // bareScaleMap as >= 1000, so pushback occurs (line 111) and "kaksi" is added separately.
        Assert.Equal(102, "sata kaksi".ToNumber(CultureInfo.CurrentCulture));
    }

    // --- TryToNumber failure reports unrecognized word (lines 91-94) ---

    [Theory]
    [InlineData("yksi foobar", "foobar")]
    [InlineData("kolme blah kaksi", "blah")]
    public void TryToNumber_InvalidInput_ReportsUnrecognizedWord(string words, string expectedUnrecognized)
    {
        Assert.False(words.TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(0, parsedNumber);
        Assert.Equal(expectedUnrecognized, unrecognizedWord);
    }

    // --- Normalization: casing ---

    [Theory]
    [InlineData("Yksi", 1)]
    [InlineData("KOLME", 3)]
    [InlineData("Sata", 100)]
    public void ToNumber_NormalizationHandling(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(CultureInfo.CurrentCulture));
    }

    // --- TryConvert failure path returns false with parsedValue=0 (lines 59-61) ---

    [Fact]
    public void TryToNumber_UnrecognizedPhrase_ReturnsDefaultValue()
    {
        Assert.False("xyz abc".TryToNumber(out var parsedNumber, CultureInfo.CurrentCulture, out var unrecognizedWord));
        Assert.Equal(0, parsedNumber);
        Assert.Equal("xyz", unrecognizedWord);
    }
}
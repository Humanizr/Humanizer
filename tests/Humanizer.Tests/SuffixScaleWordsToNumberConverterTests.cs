using System.Collections.Frozen;

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

/// <summary>
/// Tests for <see cref="SuffixScaleWordsToNumberConverter"/> compound-suffix branches
/// (tens suffix, teen suffix) that are unreachable through the Finnish locale because
/// Finnish cardinal map entries shadow the compound forms, and Finnish suffix tokens
/// contain diacritics that are stripped during normalization. These tests use a minimal
/// synthetic profile with ASCII-only suffix tokens to directly exercise those code paths.
/// </summary>
public class SuffixScaleWordsToNumberConverterDirectProfileTests
{
    /// <summary>
    /// Builds a minimal SuffixScaleWordsToNumberProfile with ASCII-only tokens so that
    /// the normalizer's diacritic stripping does not prevent suffix token matching.
    /// </summary>
    static SuffixScaleWordsToNumberConverter CreateMinimalConverter()
    {
        var cardinalMap = new Dictionary<string, long>(StringComparer.Ordinal)
        {
            ["zero"] = 0,
            ["one"] = 1,
            ["two"] = 2,
            ["three"] = 3,
            ["four"] = 4,
            ["five"] = 5,
            ["six"] = 6,
            ["seven"] = 7,
            ["eight"] = 8,
            ["nine"] = 9,
            ["ten"] = 10,
        }.ToFrozenDictionary(StringComparer.Ordinal);

        var bareScaleMap = new Dictionary<string, long>(StringComparer.Ordinal)
        {
            ["hundred"] = 100,
            ["thousand"] = 1000,
        }.ToFrozenDictionary(StringComparer.Ordinal);

        var scales = new SuffixScaleWord[]
        {
            new("thousand", "thousands", 1000),
        };

        var profile = new SuffixScaleWordsToNumberProfile(
            cardinalMap,
            bareScaleMap,
            scales,
            hundredSingularToken: "hundred",
            hundredPluralToken: "hundreds",
            tensSuffixToken: "ty",
            teenSuffixToken: "teen",
            negativePrefixes: ["minus "]);

        return new SuffixScaleWordsToNumberConverter(profile);
    }

    // --- Tens suffix branch (lines 191-206) ---

    [Theory]
    [InlineData("twoty", 20)]
    [InlineData("threety", 30)]
    [InlineData("fivety", 50)]
    [InlineData("ninetyeight", 98)]
    [InlineData("twotyfive", 25)]
    public void TryConvert_TensSuffix_ParsesCorrectly(string words, long expected)
    {
        var converter = CreateMinimalConverter();
        Assert.True(converter.TryConvert(words, out var parsedValue, out var unrecognizedWord));
        Assert.Equal(expected, parsedValue);
        Assert.Null(unrecognizedWord);
    }

    // --- Teen suffix branch (lines 208-219) ---

    [Theory]
    [InlineData("oneteen", 11)]
    [InlineData("twoteen", 12)]
    [InlineData("threeteen", 13)]
    [InlineData("nineteen", 19)]
    public void TryConvert_TeenSuffix_ParsesCorrectly(string words, long expected)
    {
        var converter = CreateMinimalConverter();
        Assert.True(converter.TryConvert(words, out var parsedValue, out var unrecognizedWord));
        Assert.Equal(expected, parsedValue);
        Assert.Null(unrecognizedWord);
    }

    // --- Tens suffix with remainder exercises TryParseOptional non-empty path (lines 231-239) ---

    [Fact]
    public void TryConvert_TensSuffixWithRemainder_ExercisesParseOptional()
    {
        var converter = CreateMinimalConverter();
        // "twotythree" = 2*10 + 3 = 23
        Assert.True(converter.TryConvert("twotythree", out var parsedValue, out var unrecognizedWord));
        Assert.Equal(23, parsedValue);
        Assert.Null(unrecognizedWord);
    }

    // --- Scale singular compound with remainder ---

    [Fact]
    public void TryConvert_ScaleSingularWithRemainder()
    {
        var converter = CreateMinimalConverter();
        // "thousandone" = 1000 + 1 = 1001
        Assert.True(converter.TryConvert("thousandone", out var parsedValue, out var unrecognizedWord));
        Assert.Equal(1001, parsedValue);
        Assert.Null(unrecognizedWord);
    }

    // --- Scale plural compound ---

    [Fact]
    public void TryConvert_ScalePluralCompound()
    {
        var converter = CreateMinimalConverter();
        // "twothousands" = 2 * 1000 = 2000
        Assert.True(converter.TryConvert("twothousands", out var parsedValue, out var unrecognizedWord));
        Assert.Equal(2000, parsedValue);
        Assert.Null(unrecognizedWord);
    }

    // --- Hundred plural compound ---

    [Fact]
    public void TryConvert_HundredPluralCompound()
    {
        var converter = CreateMinimalConverter();
        // "twohundreds" = 2 * 100 = 200
        Assert.True(converter.TryConvert("twohundreds", out var parsedValue, out var unrecognizedWord));
        Assert.Equal(200, parsedValue);
        Assert.Null(unrecognizedWord);
    }

    // --- Compound that fails all branches returns false ---

    [Fact]
    public void TryConvert_UnrecognizedCompound_ReturnsFalse()
    {
        var converter = CreateMinimalConverter();
        Assert.False(converter.TryConvert("xyzabc", out var parsedValue, out var unrecognizedWord));
        Assert.Equal(0, parsedValue);
        Assert.Equal("xyzabc", unrecognizedWord);
    }
}
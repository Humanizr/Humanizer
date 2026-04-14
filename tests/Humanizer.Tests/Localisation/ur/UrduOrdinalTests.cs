using System.Collections.Frozen;

namespace Humanizer.Tests.Localisation.ur;

/// <summary>
/// Verifies both ordinal API families (Ordinalize via IOrdinalizer and ToOrdinalWords via
/// INumberToWordsConverter) produce consistent gendered word-ordinal output for Urdu.
/// Also tests regional variant resolution (ur-PK, ur-IN) via parent culture walk.
/// </summary>
[UseCulture("ur")]
public class UrduOrdinalTests
{
    static readonly CultureInfo Ur = new("ur");
    static readonly CultureInfo UrPk = new("ur-PK");
    static readonly CultureInfo UrIn = new("ur-IN");

    // --- ToOrdinalWords (INumberToWordsConverter path) ---

    [Theory]
    [InlineData(1, GrammaticalGender.Masculine, "پہلا")]
    [InlineData(2, GrammaticalGender.Masculine, "دوسرا")]
    [InlineData(3, GrammaticalGender.Masculine, "تیسرا")]
    [InlineData(4, GrammaticalGender.Masculine, "چوتھا")]
    [InlineData(5, GrammaticalGender.Masculine, "پانچواں")]
    [InlineData(6, GrammaticalGender.Masculine, "چھٹا")]
    [InlineData(9, GrammaticalGender.Masculine, "نواں")]
    [InlineData(50, GrammaticalGender.Masculine, "پچاسواں")]
    [InlineData(100, GrammaticalGender.Masculine, "ایک سوواں")]
    [InlineData(101, GrammaticalGender.Masculine, "ایک سو ایکواں")]
    [InlineData(100000, GrammaticalGender.Masculine, "ایک لاکھواں")]
    [InlineData(1, GrammaticalGender.Feminine, "پہلی")]
    [InlineData(2, GrammaticalGender.Feminine, "دوسری")]
    [InlineData(3, GrammaticalGender.Feminine, "تیسری")]
    [InlineData(5, GrammaticalGender.Feminine, "پانچویں")]
    [InlineData(50, GrammaticalGender.Feminine, "پچاسویں")]
    [InlineData(101, GrammaticalGender.Feminine, "ایک سو ایکویں")]
    [InlineData(100, GrammaticalGender.Feminine, "ایک سوویں")]
    [InlineData(100000, GrammaticalGender.Feminine, "ایک لاکھویں")]
    public void ToOrdinalWords_GenderedOutput(int number, GrammaticalGender gender, string expected)
    {
        var result = number.ToOrdinalWords(gender, Ur);
        Assert.Equal(expected, result);
        UrduBidiControlSweep.AssertNoBidiControls(result);
    }

    [Theory]
    [InlineData(5, "پانچواں")]
    [InlineData(50, "پچاسواں")]
    public void ToOrdinalWords_GenderlessDefaultsToMasculine(int number, string expected)
    {
        var result = number.ToOrdinalWords(Ur);
        Assert.Equal(expected, result);
        UrduBidiControlSweep.AssertNoBidiControls(result);
    }

    [Theory]
    [InlineData(1, GrammaticalGender.Neuter, "پہلا")]
    [InlineData(5, GrammaticalGender.Neuter, "پانچواں")]
    public void ToOrdinalWords_NeuterFallsBackToMasculine(int number, GrammaticalGender gender, string expected)
    {
        var result = number.ToOrdinalWords(gender, Ur);
        Assert.Equal(expected, result);
        UrduBidiControlSweep.AssertNoBidiControls(result);
    }

    // --- Ordinalize (IOrdinalizer path) ---

    [Theory]
    [InlineData(5, GrammaticalGender.Masculine, "پانچواں")]
    [InlineData(5, GrammaticalGender.Feminine, "پانچویں")]
    [InlineData(50, GrammaticalGender.Masculine, "پچاسواں")]
    [InlineData(50, GrammaticalGender.Feminine, "پچاسویں")]
    [InlineData(100, GrammaticalGender.Masculine, "ایک سوواں")]
    [InlineData(100, GrammaticalGender.Feminine, "ایک سوویں")]
    [InlineData(101, GrammaticalGender.Masculine, "ایک سو ایکواں")]
    [InlineData(101, GrammaticalGender.Feminine, "ایک سو ایکویں")]
    [InlineData(100000, GrammaticalGender.Masculine, "ایک لاکھواں")]
    [InlineData(100000, GrammaticalGender.Feminine, "ایک لاکھویں")]
    public void Ordinalize_Int_GenderedOutput(int number, GrammaticalGender gender, string expected)
    {
        var result = number.Ordinalize(gender, Ur);
        Assert.Equal(expected, result);
        UrduBidiControlSweep.AssertNoBidiControls(result);
    }

    [Theory]
    [InlineData("5", GrammaticalGender.Masculine, "پانچواں")]
    [InlineData("5", GrammaticalGender.Feminine, "پانچویں")]
    [InlineData("5", GrammaticalGender.Neuter, "پانچواں")]
    public void Ordinalize_String_GenderedOutput(string numberString, GrammaticalGender gender, string expected)
    {
        var result = numberString.Ordinalize(gender, Ur);
        Assert.Equal(expected, result);
        UrduBidiControlSweep.AssertNoBidiControls(result);
    }

    [Theory]
    [InlineData(5, GrammaticalGender.Neuter, "پانچواں")]
    [InlineData(1, GrammaticalGender.Neuter, "پہلا")]
    public void Ordinalize_NeuterFallsBackToMasculine(int number, GrammaticalGender gender, string expected)
    {
        var result = number.Ordinalize(gender, Ur);
        Assert.Equal(expected, result);
        UrduBidiControlSweep.AssertNoBidiControls(result);
    }

    [Fact]
    public void Ordinalize_GenderlessDefaultsToMasculine()
    {
        var result = 5.Ordinalize(Ur);
        Assert.Equal("پانچواں", result);
        UrduBidiControlSweep.AssertNoBidiControls(result);
    }

    // --- Negative irregulars: both API paths produce parity ---

    [Theory]
    [InlineData(-1, GrammaticalGender.Masculine, "منفی پہلا")]
    [InlineData(-2, GrammaticalGender.Masculine, "منفی دوسرا")]
    [InlineData(-3, GrammaticalGender.Masculine, "منفی تیسرا")]
    [InlineData(-4, GrammaticalGender.Masculine, "منفی چوتھا")]
    [InlineData(-6, GrammaticalGender.Masculine, "منفی چھٹا")]
    [InlineData(-9, GrammaticalGender.Masculine, "منفی نواں")]
    [InlineData(-1, GrammaticalGender.Feminine, "منفی پہلی")]
    [InlineData(-2, GrammaticalGender.Feminine, "منفی دوسری")]
    [InlineData(-3, GrammaticalGender.Feminine, "منفی تیسری")]
    [InlineData(-5, GrammaticalGender.Masculine, "منفی پانچواں")]
    [InlineData(-5, GrammaticalGender.Feminine, "منفی پانچویں")]
    [InlineData(-50, GrammaticalGender.Masculine, "منفی پچاسواں")]
    public void NegativeOrdinals_BothPaths_ProduceConsistentOutput(int number, GrammaticalGender gender, string expected)
    {
        var toOrdinalWords = number.ToOrdinalWords(gender, Ur);
        var ordinalized = number.Ordinalize(gender, Ur);
        Assert.Equal(expected, toOrdinalWords);
        Assert.Equal(expected, ordinalized);
        UrduBidiControlSweep.AssertNoBidiControls(toOrdinalWords);
        UrduBidiControlSweep.AssertNoBidiControls(ordinalized);
    }

    // --- Both API paths produce consistent output ---

    [Theory]
    [InlineData(1, GrammaticalGender.Masculine, "پہلا")]
    [InlineData(5, GrammaticalGender.Masculine, "پانچواں")]
    [InlineData(5, GrammaticalGender.Feminine, "پانچویں")]
    [InlineData(100, GrammaticalGender.Masculine, "ایک سوواں")]
    public void ToOrdinalWords_And_Ordinalize_ProduceConsistentOutput(int number, GrammaticalGender gender, string expected)
    {
        var toOrdinalWords = number.ToOrdinalWords(gender, Ur);
        var ordinalized = number.Ordinalize(gender, Ur);
        Assert.Equal(expected, toOrdinalWords);
        Assert.Equal(expected, ordinalized);
        UrduBidiControlSweep.AssertNoBidiControls(toOrdinalWords);
        UrduBidiControlSweep.AssertNoBidiControls(ordinalized);
    }

    // --- Regression: negative irregulars must come from ordinalizer ExactReplacements, not converter ---

    /// <summary>
    /// Verifies the negative-irregular branch uses the ordinalizer's own ExactReplacements
    /// (not the converter's ConvertToOrdinal). A sentinel value that differs from the converter's
    /// output proves the ordinalizer path is taken. Before the fix, this test would return
    /// "منفی پہلا" (converter-derived) instead of "منفی آزمائشی" (ordinalizer sentinel).
    /// </summary>
    [Fact]
    public void NegativeExactReplacement_UsesOrdinalizerNotConverter()
    {
        var sentinel = "آزمائشی"; // "test" in Urdu — deliberately differs from converter's "پہلا"
        var ordinalizer = new NumberWordSuffixOrdinalizer(
            Ur,
            new NumberWordSuffixOrdinalizer.Options(
                Masculine: new NumberWordSuffixOrdinalizer.GenderBlock(
                    "واں",
                    new Dictionary<int, string> { [1] = sentinel }.ToFrozenDictionary()),
                Feminine: new NumberWordSuffixOrdinalizer.GenderBlock(
                    "ویں",
                    FrozenDictionary<int, string>.Empty),
                NeuterFallbackGender: GrammaticalGender.Masculine));

        var result = ordinalizer.Convert(-1, "-1", GrammaticalGender.Masculine);

        // Exact equality: locale's negative prefix "منفی " + ordinalizer's sentinel, not converter's ordinal
        Assert.Equal("منفی " + sentinel, result);
    }

    // --- Regional variant resolution via parent culture walk ---

    [Theory]
    [InlineData(5, GrammaticalGender.Masculine, "پانچواں")]
    [InlineData(5, GrammaticalGender.Feminine, "پانچویں")]
    public void Ordinalize_UrPk_ResolvesViaParentWalk(int number, GrammaticalGender gender, string expected)
    {
        var result = number.Ordinalize(gender, UrPk);
        Assert.Equal(expected, result);
        UrduBidiControlSweep.AssertNoBidiControls(result);
    }

    [Theory]
    [InlineData(5, GrammaticalGender.Masculine, "پانچواں")]
    [InlineData(5, GrammaticalGender.Feminine, "پانچویں")]
    public void Ordinalize_UrIn_ResolvesViaParentWalk(int number, GrammaticalGender gender, string expected)
    {
        var result = number.Ordinalize(gender, UrIn);
        Assert.Equal(expected, result);
        UrduBidiControlSweep.AssertNoBidiControls(result);
    }
}
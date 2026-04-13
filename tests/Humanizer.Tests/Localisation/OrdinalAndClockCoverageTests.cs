// Coverage tests for ordinal NumberToWords engines, PhraseClockNotationConverter,
// and WordFormTemplateOrdinalizer tail branches identified in
// artifacts/fn-9-local-coverage/uncovered.json.
//
// Targets:
// - TerminalOrdinalScaleNumberToWordsConverter: negative cardinals (lines 19-20),
//   ordinal zero (lines 49-50), ordinal negative (lines 55-58),
//   feminine unit ending "as" (line 198).
// - HarmonyOrdinalNumberToWordsConverter: out-of-range throw (lines 30-31).
// - PhraseClockNotationConverter: beforeHalf range template (line 406),
//   beforeHalf suffix calc (lines 359-361), additional range + Eifeler paths.
// - WordFormTemplateOrdinalizer: negative ordinals with AbsoluteCulture mode,
//   gender/wordForm combinations exercising branch coverage.

namespace Humanizer.Tests.Localisation;

public class OrdinalAndClockCoverageTests
{
    static readonly CultureInfo Lv = CultureInfo.GetCultureInfo("lv");
    static readonly CultureInfo Tr = CultureInfo.GetCultureInfo("tr");
    static readonly CultureInfo Az = CultureInfo.GetCultureInfo("az");
    static readonly CultureInfo Es = CultureInfo.GetCultureInfo("es");

    // ---------------------------------------------------------------------------
    //  TerminalOrdinalScaleNumberToWordsConverter (lv) -- negative + zero + feminine
    //  Covers: negative cardinal (lines 19-20), ordinal zero (lines 49-50),
    //          ordinal negative (lines 55-58), feminine unit ending (line 198)
    // ---------------------------------------------------------------------------

    [Theory]
    [InlineData(-1, "mīnus viens")]
    [InlineData(-5, "mīnus pieci")]
    [InlineData(-100, "mīnus simts")]
    [InlineData(-1000, "mīnus tūkstotis")]
    public void Latvian_NegativeCardinals(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Lv));
    }

    [Theory]
    [InlineData(-2, GrammaticalGender.Masculine, "mīnus divi")]
    [InlineData(-7, GrammaticalGender.Feminine, "mīnus septiņas")]
    public void Latvian_NegativeGenderedCardinals(long number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.ToWords(gender, Lv));
    }

    [Fact]
    public void Latvian_OrdinalZero()
    {
        Assert.Equal("nulle", 0.ToOrdinalWords(Lv));
    }

    [Theory]
    [InlineData(-1, GrammaticalGender.Masculine, "mīnus pirmais")]
    [InlineData(-5, GrammaticalGender.Masculine, "mīnus piektais")]
    [InlineData(-1, GrammaticalGender.Feminine, "mīnus pirmā")]
    [InlineData(-5, GrammaticalGender.Feminine, "mīnus piektā")]
    public void Latvian_NegativeOrdinals(int number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(gender, Lv));
    }

    [Theory]
    [InlineData(2, GrammaticalGender.Feminine, "divas")]
    [InlineData(4, GrammaticalGender.Feminine, "četras")]
    [InlineData(5, GrammaticalGender.Feminine, "piecas")]
    [InlineData(6, GrammaticalGender.Feminine, "sešas")]
    [InlineData(7, GrammaticalGender.Feminine, "septiņas")]
    [InlineData(8, GrammaticalGender.Feminine, "astoņas")]
    [InlineData(9, GrammaticalGender.Feminine, "deviņas")]
    public void Latvian_FeminineCardinalUnits(long number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.ToWords(gender, Lv));
    }

    [Theory]
    [InlineData(1, GrammaticalGender.Feminine, "viena")]
    [InlineData(3, GrammaticalGender.Feminine, "trīs")]
    public void Latvian_FeminineCardinalSpecialUnits(long number, GrammaticalGender gender, string expected)
    {
        // Unit 1 uses "a" suffix, unit 3 uses no suffix (trīs is invariant)
        Assert.Equal(expected, number.ToWords(gender, Lv));
    }

    [Theory]
    [InlineData(1, GrammaticalGender.Masculine, "pirmais")]
    [InlineData(1, GrammaticalGender.Feminine, "pirmā")]
    [InlineData(20, GrammaticalGender.Masculine, "divdesmitais")]
    [InlineData(20, GrammaticalGender.Feminine, "divdesmitā")]
    [InlineData(100, GrammaticalGender.Masculine, "simtais")]
    [InlineData(100, GrammaticalGender.Feminine, "simtā")]
    [InlineData(1000, GrammaticalGender.Masculine, "tūkstošais")]
    [InlineData(1000, GrammaticalGender.Feminine, "tūkstošā")]
    public void Latvian_GenderedOrdinals(int number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(gender, Lv));
    }

    [Theory]
    [InlineData(21, GrammaticalGender.Masculine, "divdesmit pirmais")]
    [InlineData(21, GrammaticalGender.Feminine, "divdesmit pirmā")]
    [InlineData(105, GrammaticalGender.Masculine, "simtu piektais")]
    [InlineData(1001, GrammaticalGender.Masculine, "tūkstoš pirmais")]
    public void Latvian_CompoundOrdinals(int number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(gender, Lv));
    }

    // ---------------------------------------------------------------------------
    //  HarmonyOrdinalNumberToWordsConverter (tr, az) -- out-of-range
    //  Covers: input < MinimumValue or > MaximumValue throw (lines 30-31)
    // ---------------------------------------------------------------------------

    [Fact]
    public void Turkish_LongMinValue_ThrowsNotImplementedException()
    {
        // Turkish MinimumValue = -9223372036854775807; long.MinValue = -9223372036854775808
        Assert.Throws<NotImplementedException>(() => long.MinValue.ToWords(Tr));
    }

    [Fact]
    public void Azerbaijani_LongMinValue_ThrowsNotImplementedException()
    {
        // Azerbaijani MinimumValue = int.MinValue (-2147483648); long.MinValue is below that
        Assert.Throws<NotImplementedException>(() => long.MinValue.ToWords(Az));
    }

    [Theory]
    [InlineData(-1, "eksi bir")]
    [InlineData(-5, "eksi beş")]
    [InlineData(-100, "eksi yüz")]
    public void Turkish_NegativeCardinals(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Tr));
    }

    [Theory]
    [InlineData(1, "birinci")]
    [InlineData(5, "beşinci")]
    [InlineData(10, "onuncu")]
    [InlineData(30, "otuzuncu")]
    public void Turkish_Ordinals(int number, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(Tr));
    }

    [Theory]
    [InlineData(-1, "mənfi bir")]
    [InlineData(-5, "mənfi beş")]
    public void Azerbaijani_NegativeCardinals(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Az));
    }

    // ---------------------------------------------------------------------------
    //  WordFormTemplateOrdinalizer (es) -- negative + gender/wordForm branches
    //  Covers: negative number with AbsoluteCulture mode (line 44),
    //          various gender + wordForm combinations for branch coverage
    // ---------------------------------------------------------------------------

    [Theory]
    [InlineData(-1, GrammaticalGender.Masculine, WordForm.Normal, "1.º")]
    [InlineData(-5, GrammaticalGender.Masculine, WordForm.Normal, "5.º")]
    [InlineData(-1, GrammaticalGender.Feminine, WordForm.Normal, "1.ª")]
    [InlineData(-5, GrammaticalGender.Feminine, WordForm.Normal, "5.ª")]
    [InlineData(-3, GrammaticalGender.Masculine, WordForm.Abbreviation, "3.er")]
    public void Spanish_NegativeOrdinals(int number, GrammaticalGender gender, WordForm wordForm, string expected)
    {
        Assert.Equal(expected, number.Ordinalize(gender, Es, wordForm));
    }

    [Fact]
    public void Spanish_ZeroOrdinal_ReturnsPlainZero()
    {
        Assert.Equal("0", 0.Ordinalize(GrammaticalGender.Masculine, Es));
    }

    [Fact]
    public void Spanish_IntMinValueOrdinal_ReturnsPlainZero()
    {
        Assert.Equal("0", int.MinValue.Ordinalize(GrammaticalGender.Masculine, Es));
    }

    [Theory]
    [InlineData(1, GrammaticalGender.Masculine, WordForm.Normal, "1.º")]
    [InlineData(1, GrammaticalGender.Masculine, WordForm.Abbreviation, "1.er")]
    [InlineData(1, GrammaticalGender.Feminine, WordForm.Normal, "1.ª")]
    [InlineData(1, GrammaticalGender.Feminine, WordForm.Abbreviation, "1.ª")]
    [InlineData(1, GrammaticalGender.Neuter, WordForm.Normal, "1.º")]
    [InlineData(1, GrammaticalGender.Neuter, WordForm.Abbreviation, "1.er")]
    [InlineData(3, GrammaticalGender.Masculine, WordForm.Normal, "3.º")]
    [InlineData(3, GrammaticalGender.Masculine, WordForm.Abbreviation, "3.er")]
    [InlineData(2, GrammaticalGender.Masculine, WordForm.Normal, "2.º")]
    [InlineData(2, GrammaticalGender.Masculine, WordForm.Abbreviation, "2.º")]
    [InlineData(11, GrammaticalGender.Masculine, WordForm.Normal, "11.º")]
    [InlineData(11, GrammaticalGender.Masculine, WordForm.Abbreviation, "11.er")]
    public void Spanish_OrdinalGenderWordForm(int number, GrammaticalGender gender, WordForm wordForm, string expected)
    {
        Assert.Equal(expected, number.Ordinalize(gender, Es, wordForm));
    }

#if NET6_0_OR_GREATER
    // ---------------------------------------------------------------------------
    //  PhraseClockNotationConverter -- range templates + Eifeler branches
    //  Covers: beforeHalf range (line 406), beforeHalf suffix calc (lines 359-361),
    //          additional range template paths, Eifeler rule interactions
    // ---------------------------------------------------------------------------

    [Theory]
    [InlineData(10, 27, "dräi Minutten vir hallwer eelef")]
    [InlineData(10, 28, "zwou Minutten vir hallwer eelef")]
    [InlineData(10, 29, "eng Minutt vir hallwer eelef")]
    [InlineData(3, 26, "véier Minutten vir hallwer véier")]
    public void Luxembourgish_BeforeHalfRange_UsesBeforeHalfTemplate(int hour, int minute, string expected)
    {
        using var _ = new CultureSwap(new("lb"));
        var time = new TimeOnly(hour, minute);
        Assert.Equal(expected, time.ToClockNotation());
    }

    [Theory]
    [InlineData(14, 31, "eng Minutt op hallwer dräi")]
    [InlineData(14, 33, "dräi Minutten op hallwer dräi")]
    [InlineData(14, 34, "véier Minutten op hallwer dräi")]
    public void Luxembourgish_AfterHalfRange_UsesAfterHalfTemplate(int hour, int minute, string expected)
    {
        using var _ = new CultureSwap(new("lb"));
        var time = new TimeOnly(hour, minute);
        Assert.Equal(expected, time.ToClockNotation());
    }

    [Theory]
    [InlineData(16, 36, "véieranzwanzeg Minutten vir fënnef")]
    [InlineData(16, 37, "dräianzwanzeg Minutten vir fënnef")]
    [InlineData(16, 38, "zweeanzwanzeg Minutten vir fënnef")]
    [InlineData(16, 41, "nonzéng Minutten vir fënnef")]
    [InlineData(16, 42, "uechtzéng Minutten vir fënnef")]
    [InlineData(16, 43, "siwwenzéng Minutten vir fënnef")]
    [InlineData(16, 44, "siechzéng Minutten vir fënnef")]
    public void Luxembourgish_BeforeNextRange_UsesBeforeNextTemplate(int hour, int minute, string expected)
    {
        using var _ = new CultureSwap(new("lb"));
        var time = new TimeOnly(hour, minute);
        Assert.Equal(expected, time.ToClockNotation());
    }

    [Theory]
    [InlineData(8, 1, "eng Minutt op aacht")]
    [InlineData(8, 2, "zwou Minutten op aacht")]
    [InlineData(8, 3, "dräi Minutten op aacht")]
    [InlineData(8, 4, "véier Minutten op aacht")]
    [InlineData(8, 6, "sechs Minutten op aacht")]
    [InlineData(8, 7, "siwe Minutten op aacht")]
    [InlineData(8, 8, "aacht Minutten op aacht")]
    [InlineData(8, 9, "néng Minutten op aacht")]
    [InlineData(8, 11, "eelef Minutten op aacht")]
    [InlineData(8, 12, "zwielef Minutten op aacht")]
    [InlineData(8, 13, "dräizéng Minutten op aacht")]
    [InlineData(8, 14, "véierzéng Minutten op aacht")]
    public void Luxembourgish_PastHourRange_UsesPastHourTemplate(int hour, int minute, string expected)
    {
        using var _ = new CultureSwap(new("lb"));
        var time = new TimeOnly(hour, minute);
        Assert.Equal(expected, time.ToClockNotation());
    }

    [Theory]
    [InlineData(8, 16, "siechzéng Minutten op aacht")]
    [InlineData(8, 17, "siwwenzéng Minutten op aacht")]
    [InlineData(8, 18, "uechtzéng Minutten op aacht")]
    [InlineData(8, 19, "nonzéng Minutten op aacht")]
    [InlineData(8, 21, "eenanzwanzeg Minutten op aacht")]
    [InlineData(8, 22, "zweeanzwanzeg Minutten op aacht")]
    [InlineData(8, 23, "dräianzwanzeg Minutten op aacht")]
    [InlineData(8, 24, "véieranzwanzeg Minutten op aacht")]
    public void Luxembourgish_PastHourRange_HighMinutes(int hour, int minute, string expected)
    {
        // Minutes 16-24 are in pastHour range (> 0 and < 25), NOT beforeHalf.
        // Template: '{minutes} {minuteSuffix} op {hour}'
        using var _ = new CultureSwap(new("lb"));
        var time = new TimeOnly(hour, minute);
        Assert.Equal(expected, time.ToClockNotation());
    }
#endif
}

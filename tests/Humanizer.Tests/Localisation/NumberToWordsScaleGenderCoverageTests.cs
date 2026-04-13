// Coverage tests targeting uncovered branches in the 8 NumberToWords scale/gender converter classes.
// Exercises reachable branches identified in artifacts/fn-9-local-coverage/uncovered.json.
// Defensive-only branches (exhaustive switch throws, unused enum arms) are not targeted here;
// they are absorbed in the aggregate coverage thresholds per the epic spec.

namespace Humanizer.Tests.Localisation;

public class NumberToWordsScaleGenderCoverageTests
{
    static readonly CultureInfo Nb = CultureInfo.GetCultureInfo("nb");
    static readonly CultureInfo Sv = CultureInfo.GetCultureInfo("sv");
    static readonly CultureInfo Bg = CultureInfo.GetCultureInfo("bg");
    static readonly CultureInfo El = CultureInfo.GetCultureInfo("el");
    static readonly CultureInfo Cs = CultureInfo.GetCultureInfo("cs");
    static readonly CultureInfo Sk = CultureInfo.GetCultureInfo("sk");
    static readonly CultureInfo Ar = CultureInfo.GetCultureInfo("ar");
    static readonly CultureInfo Lt = CultureInfo.GetCultureInfo("lt");
    static readonly CultureInfo Pl = CultureInfo.GetCultureInfo("pl");
    static readonly CultureInfo En = CultureInfo.GetCultureInfo("en");

    // ---------------------------------------------------------------------------
    //  GenderedNumberToWordsConverter -- Convert(long, WordForm) overload (line 28)
    // ---------------------------------------------------------------------------

    [Theory]
    [InlineData("lt", 5, "penki")]
    [InlineData("lt", 1, "vienas")]
    [InlineData("pl", 1, "jeden")]
    [InlineData("bg", 5, "пет")]
    public void GenderedConverter_ConvertWithWordForm_DelegatesToDefaultGender(string culture, long number, string expected)
    {
        var converter = Configurator.GetNumberToWordsConverter(CultureInfo.GetCultureInfo(culture));
        Assert.Equal(expected, converter.Convert(number, WordForm.Normal));
        Assert.Equal(expected, converter.Convert(number, WordForm.Abbreviation));
    }

    // ---------------------------------------------------------------------------
    //  ScaleStrategyNumberToWordsConverter (nb) -- Norwegian branches
    //  Covers: long.MinValue throw (lines 52-53), negative cardinals, ordinal zero
    //          (lines 70-71), gendered one-cardinal
    // ---------------------------------------------------------------------------

    [Fact]
    public void NorwegianBokmal_LongMinValue_ThrowsNotImplementedException()
    {
        Assert.Throws<NotImplementedException>(() => long.MinValue.ToWords(Nb));
    }

    [Theory]
    [InlineData(-5, "minus fem")]
    [InlineData(-1, "minus en")]
    [InlineData(-100, "minus hundre")]
    public void NorwegianBokmal_NegativeCardinals(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Nb));
    }

    [Fact]
    public void NorwegianBokmal_OrdinalZero()
    {
        Assert.Equal("nullte", 0.ToOrdinalWords(Nb));
    }

    [Theory]
    [InlineData(1, GrammaticalGender.Feminine, "ei")]
    [InlineData(1, GrammaticalGender.Neuter, "et")]
    public void NorwegianBokmal_GenderedOneCardinal(long number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.ToWords(gender, Nb));
    }

    // ---------------------------------------------------------------------------
    //  ScaleStrategyNumberToWordsConverter (sv) -- Swedish branches
    //  Covers: long.MinValue throw (lines 231-232), negative cardinals (lines 254-255),
    //          ordinal zero + ordinal tens (lines 323-332)
    // ---------------------------------------------------------------------------

    [Fact]
    public void Swedish_LongMinValue_ThrowsNotImplementedException()
    {
        Assert.Throws<NotImplementedException>(() => long.MinValue.ToWords(Sv));
    }

    [Theory]
    [InlineData(-5, "minus fem")]
    [InlineData(-1, "minus ett")]
    [InlineData(-100, "minus hundra")]
    public void Swedish_NegativeCardinals(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Sv));
    }

    [Theory]
    [InlineData(0, "nollte")]
    [InlineData(20, "tjugonde")]
    [InlineData(30, "trettionde")]
    [InlineData(40, "fyrtionde")]
    [InlineData(50, "femtionde")]
    [InlineData(60, "sextionde")]
    [InlineData(70, "sjuttionde")]
    [InlineData(80, "åttionde")]
    [InlineData(90, "nittionde")]
    [InlineData(21, "tjugoförsta")]
    [InlineData(1000, "ett tusende")]
    [InlineData(1000000, "en miljonte")]
    public void Swedish_OrdinalWords(int number, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(Sv));
    }

    // ---------------------------------------------------------------------------
    //  ConjoinedGenderedScaleNumberToWordsConverter (bg) -- negative + ordinal
    //  Covers: negative sign branch (lines 43, 46-48), gendered ordinals
    // ---------------------------------------------------------------------------

    [Theory]
    [InlineData(-5, GrammaticalGender.Masculine, "минус и пет")]
    [InlineData(-1, GrammaticalGender.Feminine, "минус и една")]
    [InlineData(-100, GrammaticalGender.Neuter, "минус и сто")]
    public void Bulgarian_NegativeCardinals(long number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.ToWords(gender, Bg));
    }

    [Theory]
    [InlineData(0, GrammaticalGender.Masculine, "нулев")]
    [InlineData(0, GrammaticalGender.Feminine, "нулева")]
    [InlineData(0, GrammaticalGender.Neuter, "нулево")]
    [InlineData(5, GrammaticalGender.Neuter, "пето")]
    [InlineData(20, GrammaticalGender.Masculine, "двадесети")]
    [InlineData(20, GrammaticalGender.Feminine, "двадесета")]
    [InlineData(100, GrammaticalGender.Masculine, "стотен")]
    [InlineData(100, GrammaticalGender.Feminine, "стотна")]
    [InlineData(1000, GrammaticalGender.Masculine, "една хиляден")]
    [InlineData(1000, GrammaticalGender.Feminine, "една хилядна")]
    [InlineData(1000000, GrammaticalGender.Masculine, "един милионен")]
    public void Bulgarian_GenderedOrdinals(int number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(gender, Bg));
    }

    // ---------------------------------------------------------------------------
    //  SegmentedScaleNumberToWordsConverter (el) -- edge branches
    //  Covers: overflow guard returning empty (lines 15-16), negative sign (lines 27-29),
    //          ordinal decomposition failures (lines 65-66, 76-77)
    // ---------------------------------------------------------------------------

    [Fact]
    public void Greek_LongMinValue_ReturnsEmptyString()
    {
        // long.MinValue has absolute value > maximumValue (long.MaxValue), triggering the overflow guard
        Assert.Equal(string.Empty, long.MinValue.ToWords(El));
    }

    [Theory]
    [InlineData(-5, "μείον πέντε")]
    [InlineData(-1000, "μείον χίλια")]
    public void Greek_NegativeCardinals(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(El));
    }

    [Theory]
    [InlineData(0, "")]
    [InlineData(9999, "")]
    [InlineData(10000, "")]
    [InlineData(2000, "")]
    [InlineData(5000, "")]
    public void Greek_OrdinalBeyondMap_ReturnsEmptyString(int number, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(El));
    }

    [Theory]
    [InlineData(1000, "χιλιοστός")]
    [InlineData(999, "εννιακοσιοστός ενενηκοστός ένατος")]
    [InlineData(111, "εκατοστός ενδέκατος")]
    [InlineData(1111, "χιλιοστός εκατοστός ενδέκατος")]
    [InlineData(1234, "χιλιοστός διακοσιοστός τριακοστός τέταρτος")]
    [InlineData(1001, "χιλιοστός πρώτος")]
    [InlineData(1999, "χιλιοστός εννιακοσιοστός ενενηκοστός ένατος")]
    public void Greek_ValidOrdinals(int number, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(El));
    }

    // ---------------------------------------------------------------------------
    //  WestSlavicGenderedNumberToWordsConverter (cs, sk) -- long.MinValue + trillion scale
    //  Covers: long.MinValue throw (lines 21-22), scaleNumber >= 1000 recursive branch (lines 64-66)
    // ---------------------------------------------------------------------------

    [Fact]
    public void Czech_LongMinValue_ThrowsNotImplementedException()
    {
        Assert.Throws<NotImplementedException>(() => long.MinValue.ToWords(Cs));
    }

    [Fact]
    public void Slovak_LongMinValue_ThrowsNotImplementedException()
    {
        Assert.Throws<NotImplementedException>(() => long.MinValue.ToWords(Sk));
    }

    [Theory]
    [InlineData(1_000_000_000_000L, "jeden bilion")]
    [InlineData(2_000_000_000_000L, "dva biliony")]
    [InlineData(5_000_000_000_000L, "pět bilionů")]
    public void Czech_TrillionScale_Cardinals(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Cs));
    }

    [Theory]
    [InlineData(1_234_000_000_000_000L, "jeden tisíc dvě stě třicet čtyři bilionů")]
    public void Czech_TrillionScale_WithThousandMultiplier(long number, string expected)
    {
        // Exercises the scaleNumber >= 1000 recursive branch in CollectScale (lines 64-66)
        Assert.Equal(expected, number.ToWords(Cs));
    }

    [Theory]
    [InlineData(1_000_000_000_000L, "jeden bilión")]
    [InlineData(2_345_000_000_000_000L, "dva tisíce tristo štyridsať päť biliónov")]
    public void Slovak_TrillionScale_Cardinals(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Sk));
    }

    // ---------------------------------------------------------------------------
    //  AppendedGroupNumberToWordsConverter (ar) -- twos branches + ordinal fallthrough
    //  Covers: AppendedTwos/Twos branch (lines 61-67), ordinal no-match fallthrough (line 225)
    // ---------------------------------------------------------------------------

    [Theory]
    [InlineData(2000L, "ألفان")]
    [InlineData(2_000_000L, "مليونان")]
    [InlineData(2002L, "ألفان و اثنان")]
    [InlineData(2_000_002_000L, "ملياران و ألفان")]
    [InlineData(2_002_000L, "مليونان و ألفان")]
    public void Arabic_TwosAndAppendedTwos_Cardinals(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Ar));
    }

    [Theory]
    [InlineData(22, GrammaticalGender.Feminine, "اثنتان و عشرون")]
    [InlineData(21, GrammaticalGender.Feminine, "واحدة و عشرون")]
    public void Arabic_FeminineOnesGroup(long number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.ToWords(gender, Ar));
    }

    [Theory]
    [InlineData(2, GrammaticalGender.Masculine, "الثاني")]
    [InlineData(2, GrammaticalGender.Feminine, "الثانية")]
    [InlineData(3, GrammaticalGender.Masculine, "الثالث")]
    [InlineData(3, GrammaticalGender.Feminine, "الثالثة")]
    [InlineData(5, GrammaticalGender.Masculine, "الخامس")]
    [InlineData(10, GrammaticalGender.Masculine, "العاشر")]
    [InlineData(10, GrammaticalGender.Feminine, "العاشرة")]
    [InlineData(11, GrammaticalGender.Masculine, "الحادي عشر")]
    [InlineData(21, GrammaticalGender.Masculine, "الحادي و العشرون")]
    [InlineData(100, GrammaticalGender.Masculine, "المئة")]
    [InlineData(200, GrammaticalGender.Masculine, "المئتان")]
    public void Arabic_GenderedOrdinals(int number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(gender, Ar));
    }

    // ---------------------------------------------------------------------------
    //  PluralizedScaleNumberToWordsConverter (lt) -- Lithuanian-specific branches
    //  Covers: Lithuanian form detector (lines 274-278), feminine gendered units,
    //          Lithuanian ordinal rendering
    // ---------------------------------------------------------------------------

    [Theory]
    [InlineData(1, GrammaticalGender.Feminine, "viena")]
    [InlineData(2, GrammaticalGender.Feminine, "dvi")]
    [InlineData(5, GrammaticalGender.Feminine, "penkios")]
    public void Lithuanian_FeminineCardinals(long number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.ToWords(gender, Lt));
    }

    [Theory]
    [InlineData(0, GrammaticalGender.Masculine, "nulinis")]
    [InlineData(0, GrammaticalGender.Feminine, "nulinė")]
    [InlineData(20, GrammaticalGender.Masculine, "dvidešimtas")]
    [InlineData(20, GrammaticalGender.Feminine, "dvidešimta")]
    [InlineData(21, GrammaticalGender.Masculine, "dvidešimt pirmas")]
    [InlineData(21, GrammaticalGender.Feminine, "dvidešimt pirma")]
    [InlineData(100, GrammaticalGender.Masculine, "šimtas")]
    [InlineData(100, GrammaticalGender.Feminine, "šimta")]
    [InlineData(200, GrammaticalGender.Feminine, "du šimta")]
    [InlineData(300, GrammaticalGender.Masculine, "trys šimtas")]
    [InlineData(1000, GrammaticalGender.Masculine, "tūkstantas")]
    [InlineData(1000, GrammaticalGender.Feminine, "tūkstanta")]
    [InlineData(2000, GrammaticalGender.Masculine, "du tūkstantas")]
    [InlineData(1000000, GrammaticalGender.Masculine, "milijonas")]
    [InlineData(1000000, GrammaticalGender.Feminine, "milijona")]
    public void Lithuanian_GenderedOrdinals(int number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(gender, Lt));
    }

    [Theory]
    [InlineData(-7, "minus septyni")]
    public void Lithuanian_NegativeCardinals(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Lt));
    }

    [Theory]
    [InlineData(1000L, "tūkstantis")]
    [InlineData(2000L, "du tūkstančiai")]
    [InlineData(5000L, "penki tūkstančiai")]
    [InlineData(11000L, "vienuolika tūkstančių")]
    [InlineData(21000L, "dvidešimt vienas tūkstantis")]
    [InlineData(1000000L, "milijonas")]
    [InlineData(2000000L, "du milijonai")]
    public void Lithuanian_ScaleFormDetector_Cardinals(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Lt));
    }

    // ---------------------------------------------------------------------------
    //  PluralizedScaleNumberToWordsConverter (pl) -- Polish-specific branches
    //  Covers: Polish gendered unit overrides, scale-form detector (Polish path)
    // ---------------------------------------------------------------------------

    [Theory]
    [InlineData(1, GrammaticalGender.Feminine, "jedna")]
    [InlineData(2, GrammaticalGender.Feminine, "dwie")]
    [InlineData(1, GrammaticalGender.Neuter, "jedno")]
    [InlineData(1001, GrammaticalGender.Masculine, "tysiąc jeden")]
    public void Polish_GenderedCardinals(long number, GrammaticalGender gender, string expected)
    {
        Assert.Equal(expected, number.ToWords(gender, Pl));
    }

    // ---------------------------------------------------------------------------
    //  ConjunctionalScaleNumberToWordsConverter (en) -- ordinal leading-one removal
    //  Covers: OmitLeadingOne strategy at line 171-174 (parts[0] == UnitsMap[1])
    // ---------------------------------------------------------------------------

    [Theory]
    [InlineData(1000, "thousandth")]
    [InlineData(100000, "hundred thousandth")]
    [InlineData(1000000, "millionth")]
    [InlineData(1000001, "million and first")]
    [InlineData(1000100, "million one hundredth")]
    [InlineData(1100, "thousand one hundredth")]
    public void English_OrdinalLeadingOneRemoval(int number, string expected)
    {
        Assert.Equal(expected, number.ToOrdinalWords(En));
    }
}

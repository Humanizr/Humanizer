// Coverage tests for words-to-number converter tail branches identified in
// artifacts/fn-9-local-coverage/uncovered.json.
//
// Targets:
// - VigesimalCompoundWordsToNumberConverter (fr): Convert throw path, TryConvert two-param
//   overload, empty input, vigesimal follower pushback, teen follower pushback,
//   IsVigesimalFollower returning false.
// - CompoundScaleWordsToNumberConverter (nb, is, sv): Convert throw path, TryConvert two-param
//   overload, empty input, tens fallback (empty and unit remainder), TryParseOptional (empty,
//   ignored-token prefix), sequenceMultiplierThreshold (is), large-scale compound split.
// - GreedyCompoundWordsToNumberConverter (it): Convert throw path, TryConvert two-param overload,
//   empty input, characters removed/replaced in normalization, previousWasSpace collapse,
//   ShouldIgnore, ordinal abbreviation no-suffix path, unknown token path.
// - LinkingAffixWordsToNumberConverter (fil): Convert throw path, TryConvert two-param overload,
//   empty input, teen prefix with recursive parse, linked suffix parse.
// - ContractedScaleWordsToNumberConverter (id, ms): Convert throw path, TryConvert two-param
//   overload, empty input, "dan" skip token, "belas" teen contraction.

namespace Humanizer.Tests.Localisation;

public class WordsToNumberConverterTailCoverageTests
{
    static readonly CultureInfo Fr = CultureInfo.GetCultureInfo("fr");
    static readonly CultureInfo Nb = CultureInfo.GetCultureInfo("nb");
    static readonly CultureInfo Is = CultureInfo.GetCultureInfo("is-IS");
    static readonly CultureInfo Sv = CultureInfo.GetCultureInfo("sv");
    static readonly CultureInfo It = CultureInfo.GetCultureInfo("it");
    static readonly CultureInfo Fil = CultureInfo.GetCultureInfo("fil");
    static readonly CultureInfo Id = CultureInfo.GetCultureInfo("id-ID");
    static readonly CultureInfo Ms = CultureInfo.GetCultureInfo("ms-MY");

    // ---------------------------------------------------------------------------
    //  VigesimalCompoundWordsToNumberConverter (fr)
    //  Covers: Convert throw (lines 14-15), TryConvert 2-param (line 23),
    //          empty input (lines 29-30), vigesimal pushback (lines 100-101),
    //          teen pushback (lines 113-114), IsVigesimalFollower false (line 175)
    // ---------------------------------------------------------------------------

    [Fact]
    public void French_Convert_ThrowsOnUnrecognizedWord()
    {
        Assert.Throws<ArgumentException>(() => "vingt xyzfoo".ToNumber(Fr));
    }

    [Fact]
    public void French_TryConvert_TwoParamOverload_ValidInput()
    {
        Assert.True("vingt et un".TryToNumber(out var value, Fr));
        Assert.Equal(21, value);
    }

    [Fact]
    public void French_TryConvert_TwoParamOverload_InvalidInput()
    {
        Assert.False("vingt xyzfoo".TryToNumber(out var value, Fr));
        Assert.Equal(0, value);
    }

    [Fact]
    public void French_EmptyInput_Throws()
    {
        Assert.Throws<ArgumentException>(() => "   ".ToNumber(Fr));
    }

    [Theory]
    [InlineData("quatre-vingt-un", 81)]
    [InlineData("quatre-vingt-dix-neuf", 99)]
    [InlineData("quatre-vingt-dix-sept", 97)]
    [InlineData("quatre-vingts", 80)]
    public void French_VigesimalCompound_VariousFollowers(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Fr));
    }

    [Theory]
    [InlineData("soixante et onze", 71)]
    [InlineData("soixante et douze", 72)]
    [InlineData("soixante-dix-sept", 77)]
    [InlineData("soixante et seize", 76)]
    public void French_TeenCompound_SixtyBase(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Fr));
    }

    [Theory]
    [InlineData("quatre-vingt-onze", 91)]
    [InlineData("quatre-vingt-treize", 93)]
    [InlineData("quatre-vingt-seize", 96)]
    public void French_TeenCompound_EightyBase(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Fr));
    }

    [Theory]
    [InlineData("cinq cent vingt et un", 521)]
    [InlineData("mille deux cent trente-quatre", 1234)]
    [InlineData("deux millions", 2000000)]
    [InlineData("trois milliards", 3000000000)]
    public void French_LargeScale_Numbers(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Fr));
    }

    [Theory]
    [InlineData("moins vingt", -20)]
    [InlineData("moins cent", -100)]
    public void French_Negative_Numbers(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Fr));
    }

    // The "quatre" vigesimal leader followed by a non-follower token: the peeked token
    // is pushed back and "quatre" is treated as cardinal 4.
    [Theory]
    [InlineData("quatre cent", 400)]
    [InlineData("quatre mille", 4000)]
    public void French_VigesimalLeader_FollowedByNonFollower_PushedBack(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Fr));
    }

    // ---------------------------------------------------------------------------
    //  CompoundScaleWordsToNumberConverter (nb, is, sv)
    //  Covers: Convert throw (lines 23-24), TryConvert 2-param (line 32),
    //          empty input (lines 38-39), tens fallback with empty remainder
    //          (lines 172-174), tens fallback with unit (line 182), TryParseOptional
    //          empty (lines 197-199), ignored token prefix (lines 206-208),
    //          sequenceMultiplierThreshold (is, line 121-124), large-scale compound
    //          split (lines 140-158)
    // ---------------------------------------------------------------------------

    [Fact]
    public void Norwegian_Convert_ThrowsOnUnrecognizedWord()
    {
        Assert.Throws<ArgumentException>(() => "tjue xyzbar".ToNumber(Nb));
    }

    [Fact]
    public void Norwegian_TryConvert_TwoParamOverload_ValidInput()
    {
        Assert.True("tjueen".TryToNumber(out var value, Nb));
        Assert.Equal(21, value);
    }

    [Fact]
    public void Norwegian_TryConvert_TwoParamOverload_InvalidInput()
    {
        Assert.False("tjue xyzbar".TryToNumber(out var value, Nb));
        Assert.Equal(0, value);
    }

    [Fact]
    public void Norwegian_EmptyInput_Throws()
    {
        Assert.Throws<ArgumentException>(() => "".ToNumber(Nb));
    }

    // Glued compound: tens stem + unit ("tjueen" = twenty-one)
    [Theory]
    [InlineData("tjueen", 21)]
    [InlineData("tjueto", 22)]
    [InlineData("trettitre", 33)]
    [InlineData("femtifire", 54)]
    [InlineData("nittini", 99)]
    public void Norwegian_GluedTensCompound(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Nb));
    }

    // Tens stem alone (remainder is empty after the stem match)
    [Theory]
    [InlineData("tjue", 20)]
    [InlineData("tretti", 30)]
    [InlineData("nitti", 90)]
    public void Norwegian_TensAlone(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Nb));
    }

    // Large-scale compound split: "ettusenethundreogtolv" = 1112
    [Theory]
    [InlineData("ettusenethundreogtolv", 1112)]
    [InlineData("tusenogen", 1001)]
    [InlineData("hundreogelleve", 111)]
    public void Norwegian_LargeScaleCompoundSplit(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Nb));
    }

    // Space-separated multi-token with "og" ignored token
    // Norwegian "hundre" = 100 is not >=1000, so without sequenceMultiplierThreshold it adds:
    // "to tusen" = 2*1000=2000, "tre" = 3, "hundre" = 100 -> 3+100=103, "og" skipped, "fire" = 4
    // -> 2000+107=2107
    [Theory]
    [InlineData("en million", 1000000)]
    [InlineData("to tusen og en", 2001)]
    [InlineData("tre tusen", 3000)]
    public void Norwegian_MultiToken_WithIgnoredToken(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Nb));
    }

    // Negative numbers
    [Theory]
    [InlineData("minus ti", -10)]
    [InlineData("minus hundre", -100)]
    public void Norwegian_Negative_Numbers(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Nb));
    }

    // Ordinal round-trip (exercises BuildOrdinalMap bridge)
    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(20)]
    [InlineData(100)]
    public void Norwegian_OrdinalRoundTrip(int number)
    {
        var converter = Configurator.NumberToWordsConverters.ResolveForCulture(Nb);
        var ordinal = converter.ConvertToOrdinal(number);
        Assert.True(ordinal.TryToNumber(out var parsedNumber, Nb, out var unrecognizedWord));
        Assert.Null(unrecognizedWord);
        Assert.Equal(number, parsedNumber);
    }

    // Icelandic: sequenceMultiplierThreshold = 100, exercises line 121-124
    [Theory]
    [InlineData("tvö hundruð sextíu", 260)]
    [InlineData("þrjú hundruð", 300)]
    [InlineData("fimm hundruð tuttugu og eitt", 521)]
    public void Icelandic_SequenceMultiplierThreshold(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Is));
    }

    // Icelandic: Convert throw path
    [Fact]
    public void Icelandic_Convert_ThrowsOnUnrecognizedWord()
    {
        Assert.Throws<ArgumentException>(() => "tuttugu xyzfoo".ToNumber(Is));
    }

    // Swedish compound-scale coverage
    [Theory]
    [InlineData("tjugoett", 21)]
    [InlineData("tjugofyra", 24)]
    [InlineData("trettiotre", 33)]
    [InlineData("nittionio", 99)]
    public void Swedish_GluedTensCompound(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Sv));
    }

    [Fact]
    public void Swedish_Convert_ThrowsOnUnrecognizedWord()
    {
        Assert.Throws<ArgumentException>(() => "tjugo xyzbar".ToNumber(Sv));
    }

    [Fact]
    public void Swedish_EmptyInput_Throws()
    {
        Assert.Throws<ArgumentException>(() => " ".ToNumber(Sv));
    }

    [Fact]
    public void Swedish_TryConvert_TwoParamOverload_ValidInput()
    {
        Assert.True("tjugoett".TryToNumber(out var value, Sv));
        Assert.Equal(21, value);
    }

    // Swedish ordinal round-trip
    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(50)]
    public void Swedish_OrdinalRoundTrip(int number)
    {
        var converter = Configurator.NumberToWordsConverters.ResolveForCulture(Sv);
        var ordinal = converter.ConvertToOrdinal(number);
        Assert.True(ordinal.TryToNumber(out var parsedNumber, Sv, out var unrecognizedWord));
        Assert.Null(unrecognizedWord);
        Assert.Equal(number, parsedNumber);
    }

    // ---------------------------------------------------------------------------
    //  GreedyCompoundWordsToNumberConverter (it)
    //  Covers: Convert throw (lines 19-20), TryConvert 2-param (line 28),
    //          empty input (lines 34-35), chars to remove (lines 164-165),
    //          chars to replace with space (lines 176-177), previousWasSpace
    //          collapse (lines 190-192), ShouldIgnore (lines 354-357),
    //          unknown token (lines 252-254), empty string (lines 219-221),
    //          ordinal abbreviation no-suffix (lines 285-287)
    // ---------------------------------------------------------------------------

    [Fact]
    public void Italian_Convert_ThrowsOnUnrecognizedWord()
    {
        Assert.Throws<ArgumentException>(() => "venti xyzbar".ToNumber(It));
    }

    [Fact]
    public void Italian_TryConvert_TwoParamOverload_ValidInput()
    {
        Assert.True("ventuno".TryToNumber(out var value, It));
        Assert.Equal(21, value);
    }

    [Fact]
    public void Italian_TryConvert_TwoParamOverload_InvalidInput()
    {
        Assert.False("venti xyzbar".TryToNumber(out var value, It));
        Assert.Equal(0, value);
    }

    [Fact]
    public void Italian_EmptyInput_Throws()
    {
        Assert.Throws<ArgumentException>(() => "  ".ToNumber(It));
    }

    // Characters removed during normalization: comma, period, apostrophes
    [Theory]
    [InlineData("ventuno", 21)]
    [InlineData("cento,due", 102)]
    public void Italian_NormalizationRemovesCharacters(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(It));
    }

    // Characters replaced with space: hyphen -> space
    [Theory]
    [InlineData("venti-otto", 28)]
    [InlineData("trenta-otto", 38)]
    public void Italian_NormalizationReplacesWithSpace(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(It));
    }

    // Text replacements: " ed " -> " ", " e " -> " "
    [Theory]
    [InlineData("venti e uno", 21)]
    [InlineData("trenta ed otto", 38)]
    [InlineData("cento e uno", 101)]
    public void Italian_TextReplacementsInNormalization(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(It));
    }

    // Diacritic removal: "ventitré" normalized to "ventitre"
    [Theory]
    [InlineData("ventitré", 23)]
    public void Italian_DiacriticRemoval(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(It));
    }

    // Greedy compound parsing: glued tokens
    [Theory]
    [InlineData("centoventidue", 122)]
    [InlineData("duemilatrecentoquaranta", 2340)]
    [InlineData("quattromilacinquecentosessantasette", 4567)]
    [InlineData("un milione duecentomila", 1200000)]
    public void Italian_GreedyGluedCompound(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(It));
    }

    // Ordinal abbreviations
    [Theory]
    [InlineData("1º", 1)]
    [InlineData("2ª", 2)]
    [InlineData("21º", 21)]
    [InlineData("100o", 100)]
    public void Italian_OrdinalAbbreviations(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(It));
    }

    // Invalid ordinal abbreviation suffix
    [Theory]
    [InlineData("3z")]
    public void Italian_InvalidOrdinalAbbreviation_ReturnsFalse(string words)
    {
        Assert.False(words.TryToNumber(out _, It, out _));
    }

    // Negative Italian
    [Theory]
    [InlineData("meno cinque", -5)]
    [InlineData("meno cento", -100)]
    public void Italian_Negative_Numbers(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(It));
    }

    // Italian ordinal words via BuildOrdinalMap
    [Theory]
    [InlineData("primo", 1)]
    [InlineData("prima", 1)]
    [InlineData("secondo", 2)]
    [InlineData("seconda", 2)]
    [InlineData("ventunesimo", 21)]
    [InlineData("ventunesima", 21)]
    [InlineData("centounesimo", 101)]
    [InlineData("centounesima", 101)]
    public void Italian_OrdinalWords(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(It));
    }

    // Hundred value and scale threshold handling
    [Theory]
    [InlineData("duecento", 200)]
    [InlineData("trecento", 300)]
    [InlineData("milleuno", 1001)]
    public void Italian_HundredAndScaleValues(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(It));
    }

    // ---------------------------------------------------------------------------
    //  LinkingAffixWordsToNumberConverter (fil)
    //  Covers: Convert throw (lines 15-16), TryConvert 2-param (line 24),
    //          empty input (lines 30-31), teen prefix recursive parse (lines 94-99),
    //          linked suffix parse (lines 159-161, 166-168), ShouldIgnore (lines 140-142)
    // ---------------------------------------------------------------------------

    [Fact]
    public void Filipino_Convert_ThrowsOnUnrecognizedWord()
    {
        Assert.Throws<ArgumentException>(() => "dalawampu xyzbar".ToNumber(Fil));
    }

    [Fact]
    public void Filipino_TryConvert_TwoParamOverload_ValidInput()
    {
        Assert.True("dalawampu't isa".TryToNumber(out var value, Fil));
        Assert.Equal(21, value);
    }

    [Fact]
    public void Filipino_TryConvert_TwoParamOverload_InvalidInput()
    {
        Assert.False("dalawampu xyzbar".TryToNumber(out var value, Fil));
        Assert.Equal(0, value);
    }

    [Fact]
    public void Filipino_EmptyInput_Throws()
    {
        Assert.Throws<ArgumentException>(() => "   ".ToNumber(Fil));
    }

    // Teen prefix: "labing-isa" = 10 + 1 = 11 (teenPrefix = "labing", recursive parse "isa" = 1)
    [Theory]
    [InlineData("labing-isa", 11)]
    [InlineData("labing-dalawa", 12)]
    [InlineData("labing-tatlo", 13)]
    [InlineData("labing-siyam", 19)]
    public void Filipino_TeenPrefix_Parsing(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Fil));
    }

    // Linked suffix: "dalawang" ends with "ng" so base "dalawa" = 2 is resolved
    [Theory]
    [InlineData("dalawang daan", 200)]
    [InlineData("tatlong libo", 3000)]
    [InlineData("limang daan", 500)]
    [InlineData("dalawang milyon", 2000000)]
    public void Filipino_LinkedSuffix_Parsing(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Fil));
    }

    // "at" is the ignored token
    [Theory]
    [InlineData("isang daan at lima", 105)]
    [InlineData("isang daan at dalawampu't tatlo", 123)]
    public void Filipino_IgnoredToken_At(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Fil));
    }

    // Negative
    [Theory]
    [InlineData("minus tatlo", -3)]
    [InlineData("minus isang daan", -100)]
    public void Filipino_Negative_Numbers(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Fil));
    }

    // Hundred value path: "daan" = 100 in cardinalMap; linked suffix "ng" on unit
    [Theory]
    [InlineData("isang daan", 100)]
    [InlineData("dalawang daan", 200)]
    [InlineData("limang daan", 500)]
    public void Filipino_HundredValue(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Fil));
    }

    // Scale threshold path
    [Theory]
    [InlineData("isang libo", 1000)]
    [InlineData("dalawang milyon", 2000000)]
    public void Filipino_ScaleThreshold(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Fil));
    }

    // ---------------------------------------------------------------------------
    //  ContractedScaleWordsToNumberConverter (id, ms)
    //  Covers: Convert throw (lines 26-27), TryConvert 2-param (line 35),
    //          empty input (lines 41-42), "dan" skip (lines 116-117),
    //          "belas" teen (lines 127-129), "puluh" tens (lines 131-132),
    //          hundred/scale paths
    // ---------------------------------------------------------------------------

    [Fact]
    public void Indonesian_Convert_ThrowsOnUnrecognizedWord()
    {
        Assert.Throws<ArgumentException>(() => "dua puluh xyzbar".ToNumber(Id));
    }

    [Fact]
    public void Indonesian_TryConvert_TwoParamOverload_ValidInput()
    {
        Assert.True("dua puluh satu".TryToNumber(out var value, Id));
        Assert.Equal(21, value);
    }

    [Fact]
    public void Indonesian_TryConvert_TwoParamOverload_InvalidInput()
    {
        Assert.False("dua puluh xyzbar".TryToNumber(out var value, Id));
        Assert.Equal(0, value);
    }

    [Fact]
    public void Indonesian_EmptyInput_Throws()
    {
        Assert.Throws<ArgumentException>(() => " ".ToNumber(Id));
    }

    // "belas" teen contraction: "dua belas" = 2 + 10 = 12
    [Theory]
    [InlineData("dua belas", 12)]
    [InlineData("tiga belas", 13)]
    [InlineData("sembilan belas", 19)]
    [InlineData("sebelas", 11)]
    public void Indonesian_BelasTeenContraction(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Id));
    }

    // "puluh" tens contraction: "dua puluh" = 2 * 10 = 20
    [Theory]
    [InlineData("dua puluh", 20)]
    [InlineData("tiga puluh", 30)]
    [InlineData("sembilan puluh", 90)]
    public void Indonesian_PuluhTensContraction(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Id));
    }

    // Hundred and scale paths
    [Theory]
    [InlineData("seratus", 100)]
    [InlineData("dua ratus", 200)]
    [InlineData("seribu", 1000)]
    [InlineData("dua ribu", 2000)]
    [InlineData("satu juta", 1000000)]
    public void Indonesian_HundredAndScale(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Id));
    }

    // Compound numbers with "dan" ignored token
    // "dan" is hard-coded in ContractedScaleWordsToNumberConverter.TryParseCardinal.
    // "dua ribu dan lima" = 2000 + 5 = 2005
    [Theory]
    [InlineData("dua ribu dan lima", 2005)]
    [InlineData("satu juta dan satu", 1000001)]
    public void Indonesian_CompoundNumbers_WithDan(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Id));
    }

    // Complex compound numbers
    // "dua ribu lima ratus" = dua(2) ribu(1000->scale: total=2000, cur=0) lima(5) ratus(100->cur=5*100=500)
    // Total = 2000+500 = 2500
    [Theory]
    [InlineData("dua ribu lima ratus", 2500)]
    [InlineData("tiga ratus", 300)]
    public void Indonesian_CompoundNumbers(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Id));
    }

    // Negative Indonesian
    [Theory]
    [InlineData("minus satu", -1)]
    [InlineData("minus seratus", -100)]
    public void Indonesian_Negative_Numbers(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Id));
    }

    // Malay: same engine, different tokens
    [Fact]
    public void Malay_Convert_ThrowsOnUnrecognizedWord()
    {
        Assert.Throws<ArgumentException>(() => "dua puluh xyzbar".ToNumber(Ms));
    }

    [Fact]
    public void Malay_EmptyInput_Throws()
    {
        Assert.Throws<ArgumentException>(() => "  ".ToNumber(Ms));
    }

    [Fact]
    public void Malay_TryConvert_TwoParamOverload_ValidInput()
    {
        Assert.True("dua puluh satu".TryToNumber(out var value, Ms));
        Assert.Equal(21, value);
    }

    // Malay "belas" and "puluh"
    [Theory]
    [InlineData("dua belas", 12)]
    [InlineData("tiga puluh", 30)]
    [InlineData("seratus lima", 105)]
    [InlineData("seribu", 1000)]
    public void Malay_ContractedScale_Numbers(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Ms));
    }

    [Theory]
    [InlineData("minus satu", -1)]
    public void Malay_Negative_Numbers(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Ms));
    }
}
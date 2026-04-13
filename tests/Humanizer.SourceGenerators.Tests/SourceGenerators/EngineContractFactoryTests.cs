using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;

using Humanizer;
using Humanizer.SourceGenerators;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

using Xunit;

namespace Humanizer.SourceGenerators.Tests;

/// <summary>
/// Tests covering uncovered branches in the three engine-contract factories:
/// <see cref="HumanizerSourceGenerator"/> nested types <c>WordsToNumberEngineContractFactory</c>,
/// <c>TimeOnlyToClockNotationEngineContractFactory</c>, and
/// <c>NumberToWordsEngineContractFactory</c>.
/// Fixture-driven tests scope assertions to the fixture's generated cache class.
/// Reflection-based tests exercise private factory methods for the token-map arm
/// (diverted by the YAML pipeline) and defensive exception paths (unsupported member
/// kinds, missing enum types, missing builder names).
/// </summary>
public class EngineContractFactoryTests
{
    // Shared lazy full-generation run result to avoid repeated expensive generator runs.
    static readonly Lazy<GeneratorDriverRunResult> fullGenerationResult = new(RunGeneratorWithAllLocales);

    // ──────────────────────────────────────────────────────────────────────
    //  WordsToNumberEngineContractFactory — via YAML pipeline
    // ──────────────────────────────────────────────────────────────────────

    [Fact]
    public void WordsToNumber_UnknownEngine_FallsThrough_ToConventionalExpression()
    {
        var runResult = RunGeneratorWithFixture("words-to-number-unknown-engine");

        var catalogSource = GetGeneratedSource(runResult, "WordsToNumberProfileCatalog.g.cs");
        Assert.Contains("new ExoticParserWordsToNumberConverter()", catalogSource);
    }

    [Fact]
    public void WordsToNumber_GreedyCompound_NoOrdinalMap_EmitsEmptyDictionary()
    {
        var runResult = RunGeneratorWithFixture("w2n-greedy-no-ordinal");

        var catalogSource = GetGeneratedSource(runResult, "WordsToNumberProfileCatalog.g.cs");
        var block = ExtractCacheClassBody(catalogSource, "zz_w2n_greedy_no_ordinal_cache");
        Assert.NotEmpty(block);
        Assert.Contains("new GreedyCompoundWordsToNumberConverter(", block);
        Assert.Contains("new Dictionary<string, long>(StringComparer.Ordinal).ToFrozenDictionary(StringComparer.Ordinal)", block);
    }

    [Fact]
    public void WordsToNumber_GreedyCompound_WithOrdinalNumberToWordsKind_EmitsBuilderCall()
    {
        var runResult = fullGenerationResult.Value;

        Assert.Empty(runResult.Diagnostics);
        var catalogSource = GetGeneratedSource(runResult, "WordsToNumberProfileCatalog.g.cs");

        var block = ExtractCacheClassBody(catalogSource, "it_cache");
        Assert.NotEmpty(block);
        Assert.Contains("GreedyCompoundWordsToNumberConverter.BuildOrdinalMap(", block);
        Assert.Contains("NumberToWordsProfileCatalog.Resolve(", block);
    }

    // ──────────────────────────────────────────────────────────────────────
    //  WordsToNumberEngineContractFactory — reflection-based direct call
    //  (token-map arm is diverted by the YAML pipeline; factory types are
    //  private, so reflection is required to reach this arm)
    // ──────────────────────────────────────────────────────────────────────

    [Fact]
    public void WordsToNumber_TokenMap_DirectCall_EmitsTokenMapConverter()
    {
        // Exercises the "token-map" arm and CreateTokenMapRulesExpression
        // (WordsToNumberEngineContractFactory lines 34, 158-185).
        var json = ParseJson("""
        {
            "engine": "token-map",
            "cardinalMap": { "one": 1, "two": 2 },
            "normalizationProfile": "lowercase-remove-periods"
        }
        """);

        var result = InvokeWordsToNumberFactoryCreate("token-map", json);

        Assert.StartsWith("new TokenMapWordsToNumberConverter(", result);
        Assert.Contains("CardinalMap =", result);
        Assert.Contains("NormalizationProfile = TokenMapNormalizationProfile.LowercaseRemovePeriods", result);
        Assert.Contains("AllowTerminalOrdinalToken = false", result);
        Assert.Contains("UseHundredMultiplier = false", result);
        Assert.Contains("AllowInvariantIntegerInput = false", result);
    }

    [Fact]
    public void WordsToNumber_TokenMap_WithOptionalFields_EmitsAllRulesProperties()
    {
        // Exercises optional branches within CreateTokenMapRulesExpression:
        // ordinalScaleMap, gluedOrdinalScaleSuffixes, compositeScaleMap, optional arrays,
        // and numeric overrides.
        var json = ParseJson("""
        {
            "engine": "token-map",
            "cardinalMap": { "one": 1 },
            "ordinalMap": { "first": 1 },
            "ordinalScaleMap": { "thousandth": 1000 },
            "gluedOrdinalScaleSuffixes": { "th": 1 },
            "compositeScaleMap": { "hundred": 100 },
            "normalizationProfile": "lowercase-remove-periods",
            "negativePrefixes": ["minus"],
            "negativeSuffixes": ["negative"],
            "ordinalPrefixes": ["the"],
            "ignoredTokens": ["and"],
            "leadingTokenPrefixesToTrim": ["a-"],
            "multiplierTokens": ["times"],
            "tokenSuffixesToStrip": ["-ish"],
            "ordinalAbbreviationSuffixes": ["st", "nd"],
            "teenSuffixTokens": ["teen"],
            "hundredSuffixTokens": ["hundred"],
            "allowTerminalOrdinalToken": true,
            "useHundredMultiplier": true,
            "allowInvariantIntegerInput": true,
            "teenBaseValue": 10,
            "hundredSuffixValue": 100,
            "unitTokenMinValue": 1,
            "unitTokenMaxValue": 9,
            "hundredSuffixMinValue": 200,
            "hundredSuffixMaxValue": 900,
            "scaleThreshold": 1000
        }
        """);

        var result = InvokeWordsToNumberFactoryCreate("token-map", json);

        // Assert actual emitted values, not just property labels
        Assert.Contains("[\"thousandth\"] = 1000", result);
        Assert.Contains("[\"th\"] = 1", result);
        Assert.Contains("[\"hundred\"] = 100", result);
        Assert.Contains("[\"first\"] = 1", result);
        Assert.Contains("NegativePrefixes = new string[] { \"minus\" }", result);
        Assert.Contains("NegativeSuffixes = new string[] { \"negative\" }", result);
        Assert.Contains("OrdinalPrefixes = new string[] { \"the\" }", result);
        Assert.Contains("IgnoredTokens = new string[] { \"and\" }", result);
        Assert.Contains("LeadingTokenPrefixesToTrim = new string[] { \"a-\" }", result);
        Assert.Contains("MultiplierTokens = new string[] { \"times\" }", result);
        Assert.Contains("TokenSuffixesToStrip = new string[] { \"-ish\" }", result);
        Assert.Contains("OrdinalAbbreviationSuffixes = new string[] { \"st\", \"nd\" }", result);
        Assert.Contains("TeenSuffixTokens = new string[] { \"teen\" }", result);
        Assert.Contains("HundredSuffixTokens = new string[] { \"hundred\" }", result);
        Assert.Contains("AllowTerminalOrdinalToken = true", result);
        Assert.Contains("UseHundredMultiplier = true", result);
        Assert.Contains("AllowInvariantIntegerInput = true", result);
        Assert.Contains("TeenBaseValue = 10", result);
        Assert.Contains("HundredSuffixValue = 100", result);
        Assert.Contains("UnitTokenMinValue = 1", result);
        Assert.Contains("UnitTokenMaxValue = 9", result);
        Assert.Contains("HundredSuffixMinValue = 200", result);
        Assert.Contains("HundredSuffixMaxValue = 900", result);
        Assert.Contains("ScaleThreshold = 1000", result);
    }

    // ──────────────────────────────────────────────────────────────────────
    //  TimeOnlyToClockNotationEngineContractFactory
    // ──────────────────────────────────────────────────────────────────────

    [Fact]
    public void ClockNotation_UnknownEngine_FallsThrough_ToConventionalExpression()
    {
        var runResult = RunGeneratorWithFixture("clock-unknown-engine");

        var catalogSource = GetGeneratedSource(runResult, "TimeOnlyToClockNotationProfileCatalog.g.cs");
        Assert.Contains("new ExoticClockTimeOnlyToClockNotationConverter()", catalogSource);
    }

    // ──────────────────────────────────────────────────────────────────────
    //  NumberToWordsEngineContractFactory — via YAML pipeline
    // ──────────────────────────────────────────────────────────────────────

    [Fact]
    public void NumberToWords_UnknownEngine_FallsThrough_ToConventionalExpression()
    {
        var runResult = RunGeneratorWithFixture("n2w-unknown-engine");

        var catalogSource = GetGeneratedSource(runResult, "NumberToWordsProfileCatalog.g.cs");
        Assert.Contains("new ExoticWriterNumberToWordsConverter()", catalogSource);
    }

    [Fact]
    public void NumberToWords_ConstructState_MissingThousandsSpecialCases_EmitsEmptyDictionary()
    {
        var runResult = RunGeneratorWithFixture("n2w-construct-state-missing-thousands-special");

        var catalogSource = GetGeneratedSource(runResult, "NumberToWordsProfileCatalog.g.cs");
        var block = ExtractCacheClassBody(catalogSource, "zz_n2w_construct_no_thousands_cache");
        Assert.NotEmpty(block);
        Assert.Contains("new ConstructStateScaleNumberToWordsConverter(new(", block);
        Assert.Contains("FrozenDictionary<int, string>.Empty", block);
    }

    [Fact]
    public void NumberToWords_VariantDecade_MissingTensEt_EmitsEmptyFrozenSet()
    {
        var runResult = RunGeneratorWithFixture("n2w-variant-decade-no-tens-et");

        var catalogSource = GetGeneratedSource(runResult, "NumberToWordsProfileCatalog.g.cs");
        var block = ExtractCacheClassBody(catalogSource, "zz_n2w_variant_decade_no_et_cache");
        Assert.NotEmpty(block);
        Assert.Contains("new VariantDecadeNumberToWordsConverter(new(", block);
        Assert.Contains("FrozenSet<int>.Empty", block);
    }

    [Fact]
    public void NumberToWords_InvertedTens_MissingOrdinalExceptions_EmitsEmptyStringDictionary()
    {
        var runResult = RunGeneratorWithFixture("n2w-inverted-tens-no-ordinal-exceptions");

        var catalogSource = GetGeneratedSource(runResult, "NumberToWordsProfileCatalog.g.cs");
        var block = ExtractCacheClassBody(catalogSource, "zz_n2w_inverted_tens_no_ordinal_cache");
        Assert.NotEmpty(block);
        Assert.Contains("new InvertedTensNumberToWordsConverter(new(", block);
        Assert.Contains("FrozenDictionary<string, string>.Empty", block);
    }

    [Fact]
    public void NumberToWords_InvertedTens_FallbackSourcePath_UsesAlternateProperty()
    {
        var runResult = RunGeneratorWithFixture("n2w-inverted-tens-fallback-joiner");

        var catalogSource = GetGeneratedSource(runResult, "NumberToWordsProfileCatalog.g.cs");
        var block = ExtractCacheClassBody(catalogSource, "zz_n2w_inverted_fallback_cache");
        Assert.NotEmpty(block);
        Assert.Contains("new InvertedTensNumberToWordsConverter(new(", block);
        // Both unitTensJoiner and unitTensAlternateJoiner emit "und" (fallback used).
        // The sequence is: unitTensJoiner, unitTensAlternateJoinerUnitEnding, unitTensAlternateJoiner
        Assert.Contains("\"und\", \"\", \"und\"", block);
    }

    [Fact]
    public void NumberToWords_JoinedScale_NullableMembers_EmitNull()
    {
        var runResult = RunGeneratorWithFixture("n2w-joined-scale-minimal");

        var catalogSource = GetGeneratedSource(runResult, "NumberToWordsProfileCatalog.g.cs");
        var block = ExtractCacheClassBody(catalogSource, "zz_n2w_joined_minimal_cache");
        Assert.NotEmpty(block);
        Assert.Contains("new JoinedScaleNumberToWordsConverter(new(", block);
        // ordinalExceptions, compoundOrdinalRemainder, compoundOrdinalWord,
        // compoundOrdinalExcludedValues all emit null
        Assert.Contains("null, null, null, null)", block);
    }

    [Fact]
    public void NumberToWords_ConstructState_CultureMember_EmitsCulture()
    {
        var runResult = fullGenerationResult.Value;

        Assert.Empty(runResult.Diagnostics);
        var catalogSource = GetGeneratedSource(runResult, "NumberToWordsProfileCatalog.g.cs");

        var heCaseLine = ExtractSwitchCaseLine(catalogSource, "he");
        Assert.NotEmpty(heCaseLine);
        Assert.Contains("new ConstructStateScaleNumberToWordsConverter(new(", heCaseLine);
        Assert.Contains(", culture)", heCaseLine);
    }

    [Fact]
    public void NumberToWords_HarmonyOrdinal_NullableCharStringDictionary_EmitsNull()
    {
        // Exercises nullable-char-string-dictionary member kind returning null.
        // Focused fixture omits ordinalSuffixes and tupleSuffixes.
        var runResult = RunGeneratorWithFixture("n2w-harmony-ordinal-no-char-dicts");

        var catalogSource = GetGeneratedSource(runResult, "NumberToWordsProfileCatalog.g.cs");
        var block = ExtractCacheClassBody(catalogSource, "zz_n2w_harmony_no_char_cache");
        Assert.NotEmpty(block);
        Assert.Contains("new HarmonyOrdinalNumberToWordsConverter(new(", block);
        // ordinalSuffixes (nullable-char-string-dictionary) -> null,
        // secondOrdinalSuffixCharacters (nullable-string) -> null,
        // ordinalSuffixPair (optional-string-array) -> Array.Empty<string>(),
        // tupleSuffixes (nullable-char-string-dictionary) -> null,
        // namedTuples (nullable-int-string-dictionary) -> null
        Assert.Contains("null, null, Array.Empty<string>(), null, null)", block);
    }

    // ──────────────────────────────────────────────────────────────────────
    //  Defensive exception paths — via reflection
    //  These branches are unreachable through normal YAML because the
    //  contract catalog is hard-coded with valid member kinds/builders.
    //  Reflection constructs invalid EngineContractMember instances directly.
    // ──────────────────────────────────────────────────────────────────────

    [Fact]
    public void NumberToWords_UnsupportedMemberKind_Throws()
    {
        // Exercises the _ default throw in NumberToWordsEngineContractFactory.CreateMemberValue.
        var member = CreateEngineContractMember(kind: "bogus-kind", sourcePath: "x");
        var root = ParseJson("""{ "x": "value" }""");

        var exception = InvokeCreateMemberValueExpectingException("NumberToWordsEngineContractFactory", root, member);
        Assert.Contains("Unsupported number-to-words contract member kind 'bogus-kind'", exception.Message);
    }

    [Fact]
    public void NumberToWords_EnumMemberWithoutEnumType_Throws()
    {
        // Exercises the enum null-check throw in NumberToWordsEngineContractFactory.CreateEnumValue.
        var member = CreateEngineContractMember(kind: "enum", sourcePath: "x", enumType: null);
        var root = ParseJson("""{ "x": "value" }""");

        var exception = InvokeCreateMemberValueExpectingException("NumberToWordsEngineContractFactory", root, member);
        Assert.Contains("Enum members require an enum type", exception.Message);
    }

    [Fact]
    public void NumberToWords_UnsupportedBuilderName_Throws()
    {
        // Exercises the _ default throw in NumberToWordsEngineContractFactory.CreateBuilderValue.
        var member = CreateEngineContractMember(kind: "builder", sourcePath: null, builder: "nonexistent-builder");
        var root = ParseJson("""{}""");

        var exception = InvokeCreateMemberValueExpectingException("NumberToWordsEngineContractFactory", root, member);
        Assert.Contains("Unsupported number-to-words builder 'nonexistent-builder'", exception.Message);
    }

    [Fact]
    public void NumberToWords_BuilderMemberWithoutBuilderName_Throws()
    {
        // Exercises the builder null-check throw in NumberToWordsEngineContractFactory.CreateBuilderValue.
        var member = CreateEngineContractMember(kind: "builder", sourcePath: null, builder: null);
        var root = ParseJson("""{}""");

        var exception = InvokeCreateMemberValueExpectingException("NumberToWordsEngineContractFactory", root, member);
        Assert.Contains("Builder members require a builder name", exception.Message);
    }

    [Fact]
    public void NumberToWords_EastAsianScaleBuilder_Throws()
    {
        // Exercises the "east-asian-scale-array" throw in NumberToWordsEngineContractFactory.CreateBuilderValue.
        var member = CreateEngineContractMember(kind: "builder", sourcePath: null, builder: "east-asian-scale-array");
        var root = ParseJson("""{}""");

        var exception = InvokeCreateMemberValueExpectingException("NumberToWordsEngineContractFactory", root, member);
        Assert.Contains("Unexpected east-asian scale builder", exception.Message);
    }

    [Fact]
    public void TimeOnlyToClockNotation_UnsupportedMemberKind_Throws()
    {
        // Exercises the _ default throw in TimeOnlyToClockNotationEngineContractFactory.CreateMemberValue.
        var member = CreateEngineContractMember(kind: "bogus-kind", sourcePath: "x");
        var root = ParseJson("""{ "x": "value" }""");

        var exception = InvokeClockCreateMemberValueExpectingException(root, member);
        Assert.Contains("Unsupported clock-notation contract member kind 'bogus-kind'", exception.Message);
    }

    [Fact]
    public void TimeOnlyToClockNotation_EnumMemberWithoutEnumType_Throws()
    {
        // Exercises the enum null-check throw in TimeOnlyToClockNotationEngineContractFactory.CreateEnumValue.
        var member = CreateEngineContractMember(kind: "enum", sourcePath: "x", enumType: null);
        var root = ParseJson("""{ "x": "value" }""");

        var exception = InvokeClockCreateMemberValueExpectingException(root, member);
        Assert.Contains("Enum members require an enum type", exception.Message);
    }

    [Fact]
    public void EngineContractUtilities_GetLeafPropertyName_NullPath_Throws()
    {
        // Exercises EngineContractUtilities.GetLeafPropertyName with null/empty path.
        var utilitiesType = GeneratorType.GetNestedType("EngineContractUtilities", BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("Could not find EngineContractUtilities type.");

        var method = utilitiesType.GetMethod("GetLeafPropertyName", BindingFlags.Public | BindingFlags.Static)
            ?? throw new InvalidOperationException("Could not find GetLeafPropertyName method.");

        var tie = Assert.Throws<TargetInvocationException>(() => method.Invoke(null, [null]));
        var exception = Assert.IsType<InvalidOperationException>(tie.InnerException);
        Assert.Contains("A property path is required", exception.Message);
    }

    // ──────────────────────────────────────────────────────────────────────
    //  Value-resolution branches — via reflection
    //  These exercise fallback, default, and missing-value branches within
    //  the factory value-resolution methods (GetStringValue, GetInt64Value, etc.)
    // ──────────────────────────────────────────────────────────────────────

    [Fact]
    public void TimeOnlyToClockNotation_StringMemberWithFallback_UsesFallback()
    {
        // Exercises GetStringValue fallback-source-path success branch.
        var member = CreateEngineContractMember(kind: "string", sourcePath: "primary", fallbackSourcePath: "fallback");
        var root = ParseJson("""{ "fallback": "fallback-value" }""");

        var result = InvokeClockCreateMemberValue(root, member);
        Assert.Equal("\"fallback-value\"", result);
    }

    [Fact]
    public void TimeOnlyToClockNotation_MissingRequiredString_Throws()
    {
        // Exercises GetStringValue missing-required-string throw.
        var member = CreateEngineContractMember(kind: "string", sourcePath: "missing");
        var root = ParseJson("""{}""");

        var exception = InvokeClockCreateMemberValueExpectingException(root, member);
        Assert.Contains("Missing required string property 'missing'", exception.Message);
    }

    [Fact]
    public void NumberToWords_MissingRequiredString_Throws()
    {
        // Exercises NumberToWordsEngineContractFactory.GetStringValue missing-required-string throw.
        var member = CreateEngineContractMember(kind: "string", sourcePath: "missing");
        var root = ParseJson("""{}""");

        var exception = InvokeCreateMemberValueExpectingException("NumberToWordsEngineContractFactory", root, member);
        Assert.Contains("Missing required string property 'missing'", exception.Message);
    }

    [Fact]
    public void NumberToWords_Int64MemberWithDefault_UsesDefault()
    {
        // Exercises GetInt64Value default-value branch.
        var member = CreateEngineContractMember(kind: "int64", sourcePath: "missing", defaultValue: "42");
        var root = ParseJson("""{}""");

        var result = InvokeNumberToWordsCreateMemberValue(root, member);
        Assert.Equal("42", result);
    }

    [Fact]
    public void NumberToWords_MissingRequiredInteger_Throws()
    {
        // Exercises GetInt64Value missing-required-integer throw.
        var member = CreateEngineContractMember(kind: "int64", sourcePath: "missing");
        var root = ParseJson("""{}""");

        var exception = InvokeCreateMemberValueExpectingException("NumberToWordsEngineContractFactory", root, member);
        Assert.Contains("Missing required integer property 'missing'", exception.Message);
    }

    // ──────────────────────────────────────────────────────────────────────
    //  TimeOnlyToClockNotation — full phrase-clock contract via fixture
    //  Exercises CreateConstructorValues multi-member branch, all member
    //  kinds (profile-object, enum, string, bool, optional-string-array),
    //  GetBooleanValue with data, CreateObjectValue with TypeName,
    //  CreateEnumValue happy path.
    // ──────────────────────────────────────────────────────────────────────

    [Fact]
    public void ClockNotation_PhraseClockFullContract_EmitsAllMembers()
    {
        var runResult = RunGeneratorWithFixture("clock-phrase-full-contract");

        var catalogSource = GetGeneratedSource(runResult, "TimeOnlyToClockNotationProfileCatalog.g.cs");
        var block = ExtractCacheClassBody(catalogSource, "zz_clock_phrase_full_cache");
        Assert.NotEmpty(block);
        Assert.Contains("new PhraseClockNotationConverter(new(", block);
        // enum values
        Assert.Contains("PhraseClockHourMode.H24", block);
        Assert.Contains("GrammaticalGender.Feminine", block);
        Assert.Contains("PhraseClockDayPeriodPosition.Prefix", block);
        // string values
        Assert.Contains("\"midnight\"", block);
        Assert.Contains("\"noon\"", block);
        // bool values
        Assert.Contains("true", block);
        // optional-string-array present
        Assert.Contains("new string[]", block);
    }

    // ──────────────────────────────────────────────────────────────────────
    //  NumberToWordsEngineContractFactory — nullable member PRESENT branches
    // ──────────────────────────────────────────────────────────────────────

    [Fact]
    public void NumberToWords_JoinedScale_NullableMembersPresent_EmitValues()
    {
        var runResult = RunGeneratorWithFixture("n2w-joined-scale-full-nullable");

        var catalogSource = GetGeneratedSource(runResult, "NumberToWordsProfileCatalog.g.cs");
        var block = ExtractCacheClassBody(catalogSource, "zz_n2w_joined_full_nullable_cache");
        Assert.NotEmpty(block);
        Assert.Contains("new JoinedScaleNumberToWordsConverter(new(", block);
        // nullable-int-string-dictionary present: ordinalExceptions with data
        Assert.Contains("[1] = \"first\"", block);
        Assert.Contains("[2] = \"second\"", block);
        // nullable-int64 present: compoundOrdinalRemainder = 100
        Assert.Contains("100", block);
        // nullable-string present: compoundOrdinalWord = "th", matchingOrdinalSuffix = "th"
        Assert.Contains("\"th\"", block);
        // nullable-int-set present: compoundOrdinalExcludedValues
        Assert.Contains("FrozenSet", block);
        Assert.DoesNotContain("null, null, null, null)", block);
    }

    [Fact]
    public void NumberToWords_InvertedTens_NullableStringStringDictionaryPresent_EmitsData()
    {
        var runResult = RunGeneratorWithFixture("n2w-inverted-tens-with-ordinal-exceptions");

        var catalogSource = GetGeneratedSource(runResult, "NumberToWordsProfileCatalog.g.cs");
        var block = ExtractCacheClassBody(catalogSource, "zz_n2w_inverted_ordinal_present_cache");
        Assert.NotEmpty(block);
        Assert.Contains("new InvertedTensNumberToWordsConverter(new(", block);
        // nullable-string-string-dictionary with data present
        Assert.Contains("[\"eins\"] = \"erste\"", block);
        Assert.Contains("[\"drei\"] = \"dritte\"", block);
    }

    [Fact]
    public void NumberToWords_HyphenatedScale_NullableIntStringDictionaryPresent_EmitsData()
    {
        var runResult = RunGeneratorWithFixture("n2w-hyphenated-with-exceptions");

        var catalogSource = GetGeneratedSource(runResult, "NumberToWordsProfileCatalog.g.cs");
        var block = ExtractCacheClassBody(catalogSource, "zz_n2w_hyphenated_with_exceptions_cache");
        Assert.NotEmpty(block);
        Assert.Contains("new HyphenatedScaleNumberToWordsConverter(new(", block);
        // nullable-int-string-dictionary present: ordinalUnitsExceptions
        Assert.Contains("[1] = \"first\"", block);
        // nullable-int-string-dictionary present: tupleMap
        Assert.Contains("[2] = \"double\"", block);
    }

    [Fact]
    public void NumberToWords_ContextualDecimal_NullableIntStringDictionaryPresent_EmitsData()
    {
        var runResult = RunGeneratorWithFixture("n2w-contextual-decimal-with-exceptions");

        var catalogSource = GetGeneratedSource(runResult, "NumberToWordsProfileCatalog.g.cs");
        var block = ExtractCacheClassBody(catalogSource, "zz_n2w_contextual_decimal_exc_cache");
        Assert.NotEmpty(block);
        Assert.Contains("new ContextualDecimalNumberToWordsConverter(new(", block);
        // nullable-int-string-dictionary present with MissingValue="empty": teenUnitExceptions
        Assert.Contains("[1] = \"on bir\"", block);
        // exactOrdinals present
        Assert.Contains("[0] = \"sifirinci\"", block);
    }

    [Fact]
    public void NumberToWords_VariantDecade_NullableIntSetPresent_EmitsData()
    {
        var runResult = RunGeneratorWithFixture("n2w-variant-decade-with-et");

        var catalogSource = GetGeneratedSource(runResult, "NumberToWordsProfileCatalog.g.cs");
        var block = ExtractCacheClassBody(catalogSource, "zz_n2w_variant_decade_with_et_cache");
        Assert.NotEmpty(block);
        Assert.Contains("new VariantDecadeNumberToWordsConverter(new(", block);
        // nullable-int-set present: tensUsingEtWhenUnitIsOne with data
        Assert.Contains("FrozenSet", block);
        // nullable-string present: specialSeventyOneWord
        Assert.Contains("\"soixante et onze\"", block);
        Assert.DoesNotContain("FrozenSet<int>.Empty", block);
    }

    // ──────────────────────────────────────────────────────────────────────
    //  Additional reflection-based member-kind tests
    // ──────────────────────────────────────────────────────────────────────

    [Fact]
    public void NumberToWords_Int32Member_EmitsCheckedCast()
    {
        // Exercises the "int32" arm in CreateMemberValue (line 68).
        var member = CreateEngineContractMember(kind: "int32", sourcePath: "value");
        var root = ParseJson("""{ "value": 70 }""");

        var result = InvokeNumberToWordsCreateMemberValue(root, member);
        Assert.Equal("70", result);
    }

    [Fact]
    public void NumberToWords_NullableInt64Member_Present_EmitsValue()
    {
        // Exercises the "nullable-int64" arm with data present (line 67).
        var member = CreateEngineContractMember(kind: "nullable-int64", sourcePath: "value");
        var root = ParseJson("""{ "value": 999 }""");

        var result = InvokeNumberToWordsCreateMemberValue(root, member);
        Assert.Equal("999", result);
    }

    [Fact]
    public void NumberToWords_NullableInt64Member_Missing_EmitsNull()
    {
        // Exercises the "nullable-int64" arm with missing data (line 67 null path).
        var member = CreateEngineContractMember(kind: "nullable-int64", sourcePath: "missing");
        var root = ParseJson("""{}""");

        var result = InvokeNumberToWordsCreateMemberValue(root, member);
        Assert.Equal("null", result);
    }

    [Fact]
    public void NumberToWords_NullableInt64Member_WithDefault_EmitsDefault()
    {
        // Exercises GetOptionalInt64Value default branch (line 263-264).
        var member = CreateEngineContractMember(kind: "nullable-int64", sourcePath: "missing", defaultValue: "50");
        var root = ParseJson("""{}""");

        var result = InvokeNumberToWordsCreateMemberValue(root, member);
        Assert.Equal("50", result);
    }

    [Fact]
    public void NumberToWords_NullableStringMember_Present_EmitsQuotedValue()
    {
        // Exercises the "nullable-string" arm with data present (line 64).
        var member = CreateEngineContractMember(kind: "nullable-string", sourcePath: "value");
        var root = ParseJson("""{ "value": "hello" }""");

        var result = InvokeNumberToWordsCreateMemberValue(root, member);
        Assert.Equal("\"hello\"", result);
    }

    [Fact]
    public void NumberToWords_NullableStringMember_WithFallback_UsesFallback()
    {
        // Exercises GetOptionalStringValue fallback branch (lines 218-220).
        var member = CreateEngineContractMember(kind: "nullable-string", sourcePath: "missing", fallbackSourcePath: "alt");
        var root = ParseJson("""{ "alt": "fallback-val" }""");

        var result = InvokeNumberToWordsCreateMemberValue(root, member);
        Assert.Equal("\"fallback-val\"", result);
    }

    [Fact]
    public void NumberToWords_NullableStringMember_WithDefault_EmitsDefault()
    {
        // Exercises GetOptionalStringValue default branch (line 223).
        var member = CreateEngineContractMember(kind: "nullable-string", sourcePath: "missing", defaultValue: "def");
        var root = ParseJson("""{}""");

        var result = InvokeNumberToWordsCreateMemberValue(root, member);
        Assert.Equal("\"def\"", result);
    }

    [Fact]
    public void NumberToWords_NullableStringMember_AllMissing_EmitsNull()
    {
        // Exercises the "nullable-string" arm when value is null (line 64 null path).
        var member = CreateEngineContractMember(kind: "nullable-string", sourcePath: "missing");
        var root = ParseJson("""{}""");

        var result = InvokeNumberToWordsCreateMemberValue(root, member);
        Assert.Equal("null", result);
    }

    [Fact]
    public void NumberToWords_BoolMember_DefaultTrue_EmitsTrue()
    {
        // Exercises GetBooleanValue default-value branch with "true" parsing (line 234).
        var member = CreateEngineContractMember(kind: "bool", sourcePath: "missing", defaultValue: "true");
        var root = ParseJson("""{}""");

        var result = InvokeNumberToWordsCreateMemberValue(root, member);
        Assert.Equal("true", result);
    }

    [Fact]
    public void NumberToWords_BoolMember_Present_EmitsActualValue()
    {
        // Exercises GetBooleanValue with data present (lines 228-231).
        var member = CreateEngineContractMember(kind: "bool", sourcePath: "flag");
        var root = ParseJson("""{ "flag": true }""");

        var result = InvokeNumberToWordsCreateMemberValue(root, member);
        Assert.Equal("true", result);
    }

    [Fact]
    public void NumberToWords_CultureMember_WithCulture_EmitsCulture()
    {
        // Exercises the "culture" arm with useCultureParameter=true (line 62 true path).
        var member = CreateEngineContractMember(kind: "culture");
        var root = ParseJson("""{}""");

        var result = InvokeNumberToWordsCreateMemberValueWithCulture(root, member, useCultureParameter: true);
        Assert.Equal("culture", result);
    }

    [Fact]
    public void NumberToWords_CultureMember_WithoutCulture_EmitsInvariant()
    {
        // Exercises the "culture" arm with useCultureParameter=false (line 62 false path).
        var member = CreateEngineContractMember(kind: "culture");
        var root = ParseJson("""{}""");

        var result = InvokeNumberToWordsCreateMemberValueWithCulture(root, member, useCultureParameter: false);
        Assert.Equal("CultureInfo.InvariantCulture", result);
    }

    [Fact]
    public void NumberToWords_StringMemberWithFallback_UsesFallback()
    {
        // Exercises NumberToWordsEngineContractFactory.GetStringValue fallback-source-path branch (lines 198-199).
        var member = CreateEngineContractMember(kind: "string", sourcePath: "primary", fallbackSourcePath: "fallback");
        var root = ParseJson("""{ "fallback": "fallback-value" }""");

        var result = InvokeNumberToWordsCreateMemberValue(root, member);
        Assert.Equal("\"fallback-value\"", result);
    }

    [Fact]
    public void NumberToWords_StringMemberWithDefault_UsesDefault()
    {
        // Exercises NumberToWordsEngineContractFactory.GetStringValue default-value branch (lines 204-205).
        var member = CreateEngineContractMember(kind: "string", sourcePath: "missing", defaultValue: "default-val");
        var root = ParseJson("""{}""");

        var result = InvokeNumberToWordsCreateMemberValue(root, member);
        Assert.Equal("\"default-val\"", result);
    }

    // ──────────────────────────────────────────────────────────────────────
    //  EngineContractUtilities — remaining branches
    // ──────────────────────────────────────────────────────────────────────

    [Fact]
    public void EngineContractUtilities_TryGetElement_NullPath_ReturnsRootElement()
    {
        // Exercises TryGetElement with null/whitespace path returning true and root (lines 18-20).
        var utilitiesType = GeneratorType.GetNestedType("EngineContractUtilities", BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("Could not find EngineContractUtilities type.");

        var method = utilitiesType.GetMethod("TryGetElement", BindingFlags.Public | BindingFlags.Static)
            ?? throw new InvalidOperationException("Could not find TryGetElement method.");

        var root = ParseJson("""{ "x": 1 }""");
        var parameters = new object?[] { root, null, null };
        var result = (bool)method.Invoke(null, parameters)!;

        Assert.True(result);
    }

    [Fact]
    public void EngineContractUtilities_GetRequiredElement_MissingProperty_Throws()
    {
        // Exercises GetRequiredElement throw path (line 13).
        var utilitiesType = GeneratorType.GetNestedType("EngineContractUtilities", BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("Could not find EngineContractUtilities type.");

        var method = utilitiesType.GetMethod("GetRequiredElement", BindingFlags.Public | BindingFlags.Static)
            ?? throw new InvalidOperationException("Could not find GetRequiredElement method.");

        var root = ParseJson("""{}""");
        var tie = Assert.Throws<TargetInvocationException>(() => method.Invoke(null, [root, "nonexistent"]));
        var exception = Assert.IsType<InvalidOperationException>(tie.InnerException);
        Assert.Contains("Missing required property 'nonexistent'", exception.Message);
    }

    [Fact]
    public void EngineContractUtilities_GetLeafPropertyName_DottedPath_ReturnsLastSegment()
    {
        // Exercises GetLeafPropertyName with dotted path (line 55 lastSeparator >= 0 branch).
        var utilitiesType = GeneratorType.GetNestedType("EngineContractUtilities", BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("Could not find EngineContractUtilities type.");

        var method = utilitiesType.GetMethod("GetLeafPropertyName", BindingFlags.Public | BindingFlags.Static)
            ?? throw new InvalidOperationException("Could not find GetLeafPropertyName method.");

        var result = (string)method.Invoke(null, ["parent.child.leaf"])!;
        Assert.Equal("leaf", result);
    }

    [Fact]
    public void EngineContractUtilities_GetLeafPropertyName_SimplePath_ReturnsWholePath()
    {
        // Exercises GetLeafPropertyName with non-dotted path (line 55 lastSeparator < 0 branch).
        var utilitiesType = GeneratorType.GetNestedType("EngineContractUtilities", BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("Could not find EngineContractUtilities type.");

        var method = utilitiesType.GetMethod("GetLeafPropertyName", BindingFlags.Public | BindingFlags.Static)
            ?? throw new InvalidOperationException("Could not find GetLeafPropertyName method.");

        var result = (string)method.Invoke(null, ["simple"])!;
        Assert.Equal("simple", result);
    }

    [Fact]
    public void TimeOnlyToClockNotation_StringMemberWithDefault_UsesDefault()
    {
        // Exercises TimeOnlyToClockNotationEngineContractFactory.GetStringValue default-value branch (lines 103-105).
        var member = CreateEngineContractMember(kind: "string", sourcePath: "missing", defaultValue: "default-val");
        var root = ParseJson("""{}""");

        var result = InvokeClockCreateMemberValue(root, member);
        Assert.Equal("\"default-val\"", result);
    }

    [Fact]
    public void TimeOnlyToClockNotation_BoolMemberWithDefault_UsesDefault()
    {
        // Exercises TimeOnlyToClockNotationEngineContractFactory.GetBooleanValue default branch (line 74).
        var member = CreateEngineContractMember(kind: "bool", sourcePath: "missing", defaultValue: "true");
        var root = ParseJson("""{}""");

        var result = InvokeClockCreateMemberValue(root, member);
        Assert.Equal("true", result);
    }

    [Fact]
    public void TimeOnlyToClockNotation_BoolMemberPresent_EmitsActualValue()
    {
        // Exercises TimeOnlyToClockNotationEngineContractFactory.GetBooleanValue present data branch (lines 68-71).
        var member = CreateEngineContractMember(kind: "bool", sourcePath: "flag");
        var root = ParseJson("""{ "flag": false }""");

        var result = InvokeClockCreateMemberValue(root, member);
        Assert.Equal("false", result);
    }

    [Fact]
    public void TimeOnlyToClockNotation_OptionalStringArrayPresent_EmitsArray()
    {
        // Exercises TimeOnlyToClockNotationEngineContractFactory.CreateMemberValue optional-string-array present branch.
        var member = CreateEngineContractMember(kind: "optional-string-array", sourcePath: "items");
        var root = ParseJson("""{ "items": ["a", "b"] }""");

        var result = InvokeClockCreateMemberValue(root, member);
        Assert.Contains("new string[]", result);
        Assert.Contains("\"a\"", result);
    }

    [Fact]
    public void TimeOnlyToClockNotation_OptionalStringArrayMissing_EmitsEmpty()
    {
        // Exercises TimeOnlyToClockNotationEngineContractFactory.CreateMemberValue optional-string-array missing branch.
        var member = CreateEngineContractMember(kind: "optional-string-array", sourcePath: "missing");
        var root = ParseJson("""{}""");

        var result = InvokeClockCreateMemberValue(root, member);
        Assert.Equal("Array.Empty<string>()", result);
    }

    [Fact]
    public void TimeOnlyToClockNotation_ProfileObjectWithTypeName_EmitsNamedConstructor()
    {
        // Exercises CreateObjectValue with non-null TypeName (line 87).
        var innerMember = CreateEngineContractMember(kind: "string", sourcePath: "name");
        var profileMember = CreateEngineContractMemberWithMembers(
            kind: "profile-object", sourcePath: "nested", typeName: "MyType",
            innerMembers: [innerMember]);
        var root = ParseJson("""{ "nested": { "name": "test" } }""");

        var result = InvokeClockCreateMemberValue(root, profileMember);
        Assert.StartsWith("new MyType(", result);
        Assert.Contains("\"test\"", result);
    }

    [Fact]
    public void TimeOnlyToClockNotation_ProfileObjectWithoutTypeName_EmitsAnonymousTuple()
    {
        // Exercises CreateObjectValue with null TypeName (line 86).
        var innerMember = CreateEngineContractMember(kind: "string", sourcePath: "name");
        var profileMember = CreateEngineContractMemberWithMembers(
            kind: "profile-object", sourcePath: "nested", typeName: null,
            innerMembers: [innerMember]);
        var root = ParseJson("""{ "nested": { "name": "test" } }""");

        var result = InvokeClockCreateMemberValue(root, profileMember);
        Assert.StartsWith("new(", result);
        Assert.Contains("\"test\"", result);
    }

    // ──────────────────────────────────────────────────────────────────────
    //  Full generation — consolidated builder coverage
    // ──────────────────────────────────────────────────────────────────────

    [Fact]
    public void FullGeneration_AllThreeFactories_ProcessRealLocalesWithoutErrors()
    {
        var runResult = fullGenerationResult.Value;

        Assert.Empty(runResult.Diagnostics);

        // NumberToWordsEngineContractFactory — verify builder-specific emitted shapes
        var n2wCatalog = GetGeneratedSource(runResult, "NumberToWordsProfileCatalog.g.cs");
        Assert.Contains("new HarmonyOrdinalScale[]", n2wCatalog);
        Assert.Contains("new ContractedOneScaleNumberToWordsConverter.Scale[]", n2wCatalog);
        Assert.Contains("new LinkingScale[]", n2wCatalog);
        Assert.Contains("new AgglutinativeOrdinalScale[]", n2wCatalog);
        Assert.Contains("new ConjunctionalScale[]", n2wCatalog);
        Assert.Contains("new JoinedScale[]", n2wCatalog);
        Assert.Contains("new EastSlavicScale[]", n2wCatalog);
        Assert.Contains("new PluralizedScale[]", n2wCatalog);
        Assert.Contains("new OrdinalPrefixScale[]", n2wCatalog);
        Assert.Contains("new InvertedTensScale[]", n2wCatalog);
        Assert.Contains("new SouthSlavicScale[]", n2wCatalog);
        Assert.Contains("new ScaleStrategyScale[]", n2wCatalog);
        Assert.Contains("new UnitLeadingCompoundScale[]", n2wCatalog);
        Assert.Contains("new ConjoinedGenderedScale[]", n2wCatalog);
        Assert.Contains("new SegmentedScale[]", n2wCatalog);
        Assert.Contains("new TerminalOrdinalScale[]", n2wCatalog);
        Assert.Contains("new ConstructStateScale[]", n2wCatalog);
        Assert.Contains("new GenderedScaleOrdinalNumberToWordsConverter.Scale[]", n2wCatalog);
        Assert.Contains("new HyphenatedScale[]", n2wCatalog);
        Assert.Contains("new TriadScaleNumberToWordsConverter.TriadScale[]", n2wCatalog);
        Assert.Contains("new LongScaleStemOrdinalNumberToWordsConverter.LargeScale[]", n2wCatalog);

        // WordsToNumberEngineContractFactory — all non-token-map engines
        var w2nCatalog = GetGeneratedSource(runResult, "WordsToNumberProfileCatalog.g.cs");
        Assert.Contains("new CompoundScaleWordsToNumberConverter(", w2nCatalog);
        Assert.Contains("new ContractedScaleWordsToNumberConverter(", w2nCatalog);
        Assert.Contains("new EastAsianPositionalWordsToNumberConverter(", w2nCatalog);
        Assert.Contains("new GreedyCompoundWordsToNumberConverter(", w2nCatalog);
        Assert.Contains("new InvertedTensWordsToNumberConverter(", w2nCatalog);
        Assert.Contains("new LinkingAffixWordsToNumberConverter(", w2nCatalog);
        Assert.Contains("new PrefixedTensScaleWordsToNumberConverter(", w2nCatalog);
        Assert.Contains("new SuffixScaleWordsToNumberConverter(", w2nCatalog);
        Assert.Contains("new VigesimalCompoundWordsToNumberConverter(", w2nCatalog);

        // TimeOnlyToClockNotationEngineContractFactory — phrase-clock contract
        var clockCatalog = GetGeneratedSource(runResult, "TimeOnlyToClockNotationProfileCatalog.g.cs");
        Assert.Contains("new PhraseClockNotationConverter(new(", clockCatalog);
    }

    // ──────────────────────────────────────────────────────────────────────
    //  Reflection helpers for private nested factory types
    // ──────────────────────────────────────────────────────────────────────

    static readonly Type GeneratorType = typeof(HumanizerSourceGenerator);

    /// <summary>
    /// Invokes <c>WordsToNumberEngineContractFactory.Create</c> via reflection.
    /// The factory and profile types are private nested types within
    /// <see cref="HumanizerSourceGenerator"/>, requiring reflection to access.
    /// Primary constructors compile to a standard constructor with all parameters.
    /// </summary>
    static string InvokeWordsToNumberFactoryCreate(string engine, JsonElement root)
    {
        var profileType = GeneratorType.GetNestedType("WordsToNumberProfileDefinition", BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("Could not find WordsToNumberProfileDefinition type.");
        var factoryType = GeneratorType.GetNestedType("WordsToNumberEngineContractFactory", BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("Could not find WordsToNumberEngineContractFactory type.");

        var constructor = profileType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
            .FirstOrDefault()
            ?? throw new InvalidOperationException("Could not find constructor for WordsToNumberProfileDefinition.");

        var profile = constructor.Invoke(["test-profile", engine, root]);

        var createMethod = factoryType.GetMethod("Create", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
            ?? throw new InvalidOperationException("Could not find Create method.");

        return (string)createMethod.Invoke(null, [profile])!;
    }

    /// <summary>
    /// Creates an <c>EngineContractMember</c> via reflection with the specified parameters.
    /// Unspecified parameters default to null/empty.
    /// </summary>
    static object CreateEngineContractMember(
        string kind,
        string? sourcePath = null,
        string? typeName = null,
        string? enumType = null,
        string? builder = null,
        string? defaultValue = null,
        string? fallbackSourcePath = null,
        string? missingValue = null)
    {
        var memberType = GeneratorType.GetNestedType("EngineContractMember", BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("Could not find EngineContractMember type.");

        var constructor = memberType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
            .FirstOrDefault()
            ?? throw new InvalidOperationException("Could not find EngineContractMember constructor.");

        // Primary constructor: kind, sourcePath, typeName, enumType, builder, defaultValue,
        // fallbackSourcePath, missingValue, ImmutableArray<EngineContractMember> members
        var emptyMembers = typeof(ImmutableArray).GetMethod("Create", Type.EmptyTypes)!
            .MakeGenericMethod(memberType).Invoke(null, null);

        return constructor.Invoke([kind, sourcePath, typeName, enumType, builder, defaultValue,
            fallbackSourcePath, missingValue, emptyMembers!]);
    }

    /// <summary>
    /// Creates an <c>EngineContractMember</c> via reflection with nested members.
    /// Used for testing profile-object members that contain sub-members.
    /// </summary>
    static object CreateEngineContractMemberWithMembers(
        string kind,
        string? sourcePath = null,
        string? typeName = null,
        params object[] innerMembers)
    {
        var memberType = GeneratorType.GetNestedType("EngineContractMember", BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("Could not find EngineContractMember type.");

        var constructor = memberType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
            .FirstOrDefault()
            ?? throw new InvalidOperationException("Could not find EngineContractMember constructor.");

        // Build ImmutableArray<EngineContractMember> from the inner members
        var createBuilderMethod = typeof(ImmutableArray).GetMethods()
            .First(m => m.Name == "CreateBuilder" && m.GetParameters().Length == 0)
            .MakeGenericMethod(memberType);
        var builder = createBuilderMethod.Invoke(null, null)!;
        var addMethod = builder.GetType().GetMethod("Add")!;
        foreach (var inner in innerMembers)
        {
            addMethod.Invoke(builder, [inner]);
        }

        var toImmutableMethod = builder.GetType().GetMethod("ToImmutable")!;
        var members = toImmutableMethod.Invoke(builder, null)!;

        return constructor.Invoke([kind, sourcePath, typeName, null, null, null, null, null, members]);
    }

    /// <summary>
    /// Invokes <c>NumberToWordsEngineContractFactory.CreateMemberValue</c> via reflection
    /// and returns the expected <see cref="InvalidOperationException"/> from the inner
    /// <see cref="TargetInvocationException"/>.
    /// </summary>
    static InvalidOperationException InvokeCreateMemberValueExpectingException(
        string factoryTypeName, JsonElement root, object member)
    {
        var factoryType = GeneratorType.GetNestedType(factoryTypeName, BindingFlags.NonPublic)
            ?? throw new InvalidOperationException($"Could not find {factoryTypeName} type.");

        var method = factoryType.GetMethod("CreateMemberValue", BindingFlags.NonPublic | BindingFlags.Static)
            ?? throw new InvalidOperationException("Could not find CreateMemberValue method.");

        // NumberToWordsEngineContractFactory.CreateMemberValue(root, member, useCultureParameter)
        var tie = Assert.Throws<TargetInvocationException>(() => method.Invoke(null, [root, member, false]));
        return Assert.IsType<InvalidOperationException>(tie.InnerException);
    }

    /// <summary>
    /// Invokes <c>TimeOnlyToClockNotationEngineContractFactory.CreateMemberValue</c> via reflection.
    /// </summary>
    static InvalidOperationException InvokeClockCreateMemberValueExpectingException(
        JsonElement root, object member)
    {
        var factoryType = GeneratorType.GetNestedType("TimeOnlyToClockNotationEngineContractFactory", BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("Could not find TimeOnlyToClockNotationEngineContractFactory type.");

        var method = factoryType.GetMethod("CreateMemberValue", BindingFlags.NonPublic | BindingFlags.Static)
            ?? throw new InvalidOperationException("Could not find CreateMemberValue method.");

        // TimeOnlyToClockNotationEngineContractFactory.CreateMemberValue(root, member)
        var tie = Assert.Throws<TargetInvocationException>(() => method.Invoke(null, [root, member]));
        return Assert.IsType<InvalidOperationException>(tie.InnerException);
    }

    /// <summary>
    /// Invokes <c>TimeOnlyToClockNotationEngineContractFactory.CreateMemberValue</c> and returns
    /// the string result (non-throwing path).
    /// </summary>
    static string InvokeClockCreateMemberValue(JsonElement root, object member)
    {
        var factoryType = GeneratorType.GetNestedType("TimeOnlyToClockNotationEngineContractFactory", BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("Could not find TimeOnlyToClockNotationEngineContractFactory type.");

        var method = factoryType.GetMethod("CreateMemberValue", BindingFlags.NonPublic | BindingFlags.Static)
            ?? throw new InvalidOperationException("Could not find CreateMemberValue method.");

        return (string)method.Invoke(null, [root, member])!;
    }

    /// <summary>
    /// Invokes <c>NumberToWordsEngineContractFactory.CreateMemberValue</c> and returns
    /// the string result (non-throwing path).
    /// </summary>
    static string InvokeNumberToWordsCreateMemberValue(JsonElement root, object member)
    {
        var factoryType = GeneratorType.GetNestedType("NumberToWordsEngineContractFactory", BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("Could not find NumberToWordsEngineContractFactory type.");

        var method = factoryType.GetMethod("CreateMemberValue", BindingFlags.NonPublic | BindingFlags.Static)
            ?? throw new InvalidOperationException("Could not find CreateMemberValue method.");

        return (string)method.Invoke(null, [root, member, false])!;
    }

    /// <summary>
    /// Invokes <c>NumberToWordsEngineContractFactory.CreateMemberValue</c> with explicit
    /// <paramref name="useCultureParameter"/> control.
    /// </summary>
    static string InvokeNumberToWordsCreateMemberValueWithCulture(
        JsonElement root, object member, bool useCultureParameter)
    {
        var factoryType = GeneratorType.GetNestedType("NumberToWordsEngineContractFactory", BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("Could not find NumberToWordsEngineContractFactory type.");

        var method = factoryType.GetMethod("CreateMemberValue", BindingFlags.NonPublic | BindingFlags.Static)
            ?? throw new InvalidOperationException("Could not find CreateMemberValue method.");

        return (string)method.Invoke(null, [root, member, useCultureParameter])!;
    }

    static JsonElement ParseJson(string json)
    {
        using var document = JsonDocument.Parse(json);
        return document.RootElement.Clone();
    }

    // ──────────────────────────────────────────────────────────────────────
    //  Generator pipeline helpers
    // ──────────────────────────────────────────────────────────────────────

    static GeneratorDriverRunResult RunGeneratorWithFixture(string fixtureName)
    {
        var fixturePath = Path.Combine(
            FixtureLoader.GetFixtureDirectory("EngineContracts"),
            fixtureName + ".yml");
        var additionalText = FixtureLoader.LoadAsAdditionalText(fixturePath);
        return RunGenerator(additionalText);
    }

    static GeneratorDriverRunResult RunGeneratorWithAllLocales()
    {
        return RunGenerator();
    }

    static GeneratorDriverRunResult RunGenerator(params AdditionalText[] extraAdditionalTexts)
    {
        var compilation = CreateCompilation();
        GeneratorDriver driver = CSharpGeneratorDriver.Create(
            [new HumanizerSourceGenerator().AsSourceGenerator()],
            GetAdditionalTexts(extraAdditionalTexts),
            (CSharpParseOptions)compilation.SyntaxTrees.Single().Options);

        driver = driver.RunGenerators(compilation);
        return driver.GetRunResult();
    }

    static CSharpCompilation CreateCompilation()
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(
            SourceText.From("namespace Humanizer; internal static class GeneratorHarness { }"),
            new CSharpParseOptions(LanguageVersion.Preview));

        return CSharpCompilation.Create(
            "Humanizer.SourceGenerator.EngineContractTests",
            [syntaxTree],
            GetMetadataReferences(),
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
    }

    static ImmutableArray<MetadataReference> GetMetadataReferences()
    {
        var references = new System.Collections.Generic.Dictionary<string, MetadataReference>(StringComparer.OrdinalIgnoreCase);

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (assembly.IsDynamic || string.IsNullOrWhiteSpace(assembly.Location))
            {
                continue;
            }

            references[assembly.Location] = MetadataReference.CreateFromFile(assembly.Location);
        }

        foreach (var assembly in new[]
        {
            typeof(object).Assembly,
            typeof(Enumerable).Assembly,
            typeof(System.Globalization.CultureInfo).Assembly,
            typeof(Configurator).Assembly,
            typeof(HumanizerSourceGenerator).Assembly
        })
        {
            if (!references.ContainsKey(assembly.Location))
            {
                references[assembly.Location] = MetadataReference.CreateFromFile(assembly.Location);
            }
        }

        return references.Values.ToImmutableArray();
    }

    static ImmutableArray<AdditionalText> GetAdditionalTexts(params AdditionalText[] extraAdditionalTexts)
    {
        var root = FindRepositoryRoot();
        var sourceRoot = Path.Combine(root, "src", "Humanizer");
        var filePaths = new System.Collections.Generic.List<string>();

        var localesDir = Path.Combine(sourceRoot, "Locales");
        if (Directory.Exists(localesDir))
        {
            filePaths.AddRange(Directory.GetFiles(localesDir, "*.yml", SearchOption.TopDirectoryOnly));
        }

        var additionalTexts = filePaths
            .OrderBy(static path => path, StringComparer.OrdinalIgnoreCase)
            .Select(static path => (AdditionalText)new FileAdditionalText(path))
            .ToImmutableArray();

        return extraAdditionalTexts.Length == 0
            ? additionalTexts
            : additionalTexts.AddRange(extraAdditionalTexts);
    }

    static string GetGeneratedSource(GeneratorDriverRunResult runResult, string hintName) =>
        runResult.Results
            .SelectMany(static result => result.GeneratedSources)
            .Single(source => source.HintName == hintName)
            .SourceText
            .ToString();

    static string ExtractCacheClassBody(string source, string className)
    {
        var marker = "static class " + className;
        var start = source.IndexOf(marker, StringComparison.Ordinal);
        if (start < 0)
        {
            return string.Empty;
        }

        var openBrace = source.IndexOf('{', start);
        if (openBrace < 0)
        {
            return string.Empty;
        }

        var depth = 1;
        for (var idx = openBrace + 1; idx < source.Length; idx++)
        {
            if (source[idx] == '{')
            {
                depth++;
            }
            else if (source[idx] == '}')
            {
                depth--;
                if (depth == 0)
                {
                    return source[start..(idx + 1)];
                }
            }
        }

        return string.Empty;
    }

    /// <summary>
    /// Extracts the complete <c>case "key": return ...;</c> line from a generated switch statement.
    /// </summary>
    static string ExtractSwitchCaseLine(string source, string caseKey)
    {
        var marker = "case \"" + caseKey + "\": return ";
        var start = source.IndexOf(marker, StringComparison.Ordinal);
        if (start < 0)
        {
            return string.Empty;
        }

        var end = source.IndexOf(';', start);
        return end < 0 ? string.Empty : source[start..(end + 1)];
    }

    static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "src", "Humanizer", "Humanizer.csproj")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new Xunit.Sdk.XunitException("Could not locate the repository root.");
    }

    sealed class FileAdditionalText(string path) : AdditionalText
    {
        public override string Path => path;

        public override SourceText GetText(CancellationToken cancellationToken = default) =>
            SourceText.From(File.ReadAllText(path), Encoding.UTF8);
    }
}

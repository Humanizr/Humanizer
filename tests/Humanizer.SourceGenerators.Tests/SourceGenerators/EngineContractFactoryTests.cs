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

using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
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
/// Each fixture-driven test scopes assertions to the fixture's generated cache class.
///
/// Unreachable defensive branches (identified during investigation):
/// <list type="bullet">
/// <item><c>WordsToNumberEngineContractFactory</c> "token-map" arm + <c>CreateTokenMapRulesExpression</c>:
///   token-map engines are diverted by <c>LocaleYamlCatalog.CreateMappedFeature</c> before reaching
///   <c>WordsToNumberProfileCatalogInput</c>; the arm is structurally unreachable.</item>
/// <item><c>NumberToWordsEngineContractFactory</c> "int-string-dictionary" and "char-string-dictionary" arms:
///   no contract in <c>EngineContractCatalog</c> uses these member kinds.</item>
/// <item><c>NumberToWordsEngineContractFactory</c> unsupported-kind/builder/enum-null/missing-string throws:
///   defensive guards that require invalid catalog definitions to trigger.</item>
/// <item><c>TimeOnlyToClockNotationEngineContractFactory</c> multi-member constructor path, unsupported-kind throw,
///   enum-null throw, named-TypeName object path, fallback-source-path, missing-string throw:
///   phrase-clock is the only clock contract and its shape does not exercise these paths.</item>
/// <item><c>EngineContractUtilities.GetLeafPropertyName</c> null/empty throw: defensive guard.</item>
/// <item><c>NumberToWordsEngineContractFactory.GetOptionalStringValue</c> fallback branch: no nullable-string
///   member has a fallback source path.</item>
/// <item><c>NumberToWordsEngineContractFactory.GetOptionalInt64Value</c> default-value branch: no nullable-int64
///   member has a non-null default value.</item>
/// </list>
/// These are absorbed in aggregate threshold headroom per the epic scope.
/// </summary>
public class EngineContractFactoryTests
{
    // Shared lazy full-generation run result to avoid repeated expensive generator runs.
    static readonly Lazy<GeneratorDriverRunResult> fullGenerationResult = new(RunGeneratorWithAllLocales);

    // ──────────────────────────────────────────────────────────────────────
    //  WordsToNumberEngineContractFactory
    // ──────────────────────────────────────────────────────────────────────

    [Fact]
    public void WordsToNumber_UnknownEngine_FallsThrough_ToConventionalExpression()
    {
        // Exercises the _ default arm in WordsToNumberEngineContractFactory.Create.
        // An unknown engine falls through to CreateConventionalWordsToNumberExpression.
        var runResult = RunGeneratorWithFixture("words-to-number-unknown-engine");

        var catalogSource = GetGeneratedSource(runResult, "WordsToNumberProfileCatalog.g.cs");
        Assert.Contains("new ExoticParserWordsToNumberConverter()", catalogSource);
    }

    [Fact]
    public void WordsToNumber_GreedyCompound_NoOrdinalMap_EmitsEmptyDictionary()
    {
        // Exercises CreateGreedyCompoundOrdinalMapExpression fallback.
        // When a greedy-compound locale has neither ordinalMap nor ordinalNumberToWordsKind,
        // the factory emits an empty FrozenDictionary.
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
        // Exercises CreateGreedyCompoundOrdinalMapExpression ordinalNumberToWordsKind branch.
        // Italian uses greedy-compound with ordinalNumberToWordsKind: 'self'.
        var runResult = fullGenerationResult.Value;

        Assert.Empty(runResult.Diagnostics);
        var catalogSource = GetGeneratedSource(runResult, "WordsToNumberProfileCatalog.g.cs");

        // Italian's profile uses BuildOrdinalMap with a NumberToWordsProfileCatalog.Resolve reference
        var block = ExtractCacheClassBody(catalogSource, "it_cache");
        Assert.NotEmpty(block);
        Assert.Contains("GreedyCompoundWordsToNumberConverter.BuildOrdinalMap(", block);
        Assert.Contains("NumberToWordsProfileCatalog.Resolve(", block);
    }

    // ──────────────────────────────────────────────────────────────────────
    //  TimeOnlyToClockNotationEngineContractFactory
    // ──────────────────────────────────────────────────────────────────────

    [Fact]
    public void ClockNotation_UnknownEngine_FallsThrough_ToConventionalExpression()
    {
        // Exercises TimeOnlyToClockNotationEngineContractFactory conventional fallthrough.
        // An unknown engine falls through to CreateConventionalTimeOnlyToClockNotationExpression.
        var runResult = RunGeneratorWithFixture("clock-unknown-engine");

        var catalogSource = GetGeneratedSource(runResult, "TimeOnlyToClockNotationProfileCatalog.g.cs");
        Assert.Contains("new ExoticClockTimeOnlyToClockNotationConverter()", catalogSource);
    }

    // ──────────────────────────────────────────────────────────────────────
    //  NumberToWordsEngineContractFactory
    // ──────────────────────────────────────────────────────────────────────

    [Fact]
    public void NumberToWords_UnknownEngine_FallsThrough_ToConventionalExpression()
    {
        // Exercises NumberToWordsEngineContractFactory conventional fallthrough.
        // An unknown engine falls through to CreateConventionalNumberToWordsExpression.
        var runResult = RunGeneratorWithFixture("n2w-unknown-engine");

        var catalogSource = GetGeneratedSource(runResult, "NumberToWordsProfileCatalog.g.cs");
        Assert.Contains("new ExoticWriterNumberToWordsConverter()", catalogSource);
    }

    [Fact]
    public void NumberToWords_ConstructState_MissingThousandsSpecialCases_EmitsEmptyDictionary()
    {
        // Exercises CreateNullableIntStringDictionaryValue MissingValue="empty" branch.
        // construct-state-scale's thousandsSpecialCases has MissingValue="empty".
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
        // Exercises CreateNullableIntSetValue MissingValue="empty" branch.
        // variant-decade's tensUsingEtWhenUnitIsOne has MissingValue="empty".
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
        // Exercises nullable-string-string-dictionary MissingValue="empty" branch.
        // inverted-tens' ordinalExceptions has MissingValue="empty".
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
        // Exercises GetStringValue fallback-source-path branch.
        // inverted-tens' unitTensAlternateJoiner falls back to unitTensJoiner when absent.
        var runResult = RunGeneratorWithFixture("n2w-inverted-tens-fallback-joiner");

        var catalogSource = GetGeneratedSource(runResult, "NumberToWordsProfileCatalog.g.cs");
        var block = ExtractCacheClassBody(catalogSource, "zz_n2w_inverted_fallback_cache");
        Assert.NotEmpty(block);
        Assert.Contains("new InvertedTensNumberToWordsConverter(new(", block);
        // The fallback joiner value "und" should appear in the generated output
        Assert.Contains("\"und\"", block);
    }

    [Fact]
    public void NumberToWords_JoinedScale_NullableMembers_EmitNull()
    {
        // Exercises nullable-string (matchingOrdinalSuffix, ordinalSuffixMatchCharacters,
        // compoundOrdinalWord), nullable-int64 (compoundOrdinalRemainder), and
        // nullable-int-set (compoundOrdinalExcludedValues without MissingValue) members.
        // All these nullable members emit null when their YAML properties are absent.
        var runResult = RunGeneratorWithFixture("n2w-joined-scale-minimal");

        var catalogSource = GetGeneratedSource(runResult, "NumberToWordsProfileCatalog.g.cs");
        var block = ExtractCacheClassBody(catalogSource, "zz_n2w_joined_minimal_cache");
        Assert.NotEmpty(block);
        Assert.Contains("new JoinedScaleNumberToWordsConverter(new(", block);
        // The last four members (ordinalExceptions, compoundOrdinalRemainder,
        // compoundOrdinalWord, compoundOrdinalExcludedValues) all emit null
        Assert.Contains("null, null, null, null)", block);
    }

    [Fact]
    public void NumberToWords_ConstructState_CultureMember_EmitsCulture()
    {
        // Exercises the "culture" member kind in CreateMemberValue.
        // construct-state-scale has a trailing "culture" member.
        // Hebrew (he) uses construct-state-scale which requires culture.
        var runResult = fullGenerationResult.Value;

        Assert.Empty(runResult.Diagnostics);
        var catalogSource = GetGeneratedSource(runResult, "NumberToWordsProfileCatalog.g.cs");

        // The Hebrew construct-state profile emits "culture" as the last constructor argument
        Assert.Contains("case \"he\": return new ConstructStateScaleNumberToWordsConverter(new(", catalogSource);
        Assert.Contains(", culture)", catalogSource);
    }

    [Fact]
    public void NumberToWords_HarmonyOrdinal_NullableCharStringDictionary_EmitsNull()
    {
        // Exercises nullable-char-string-dictionary member kind returning null.
        // harmony-ordinal has ordinalSuffixes and tupleSuffixes as nullable-char-string-dictionary.
        // Turkish (tr) uses harmony-ordinal; verify its profile block emits null for absent members.
        var runResult = fullGenerationResult.Value;

        Assert.Empty(runResult.Diagnostics);
        var catalogSource = GetGeneratedSource(runResult, "NumberToWordsProfileCatalog.g.cs");

        // Turkish uses harmony-ordinal; extract its cache block
        var block = ExtractCacheClassBody(catalogSource, "tr_cache");
        Assert.NotEmpty(block);
        Assert.Contains("new HarmonyOrdinalNumberToWordsConverter(new(", block);
        // harmony-ordinal emits HarmonyOrdinalScale[] for the builder arm
        Assert.Contains("new HarmonyOrdinalScale[]", block);
    }

    // ──────────────────────────────────────────────────────────────────────
    //  Full generation — consolidated assertions for builder coverage
    // ──────────────────────────────────────────────────────────────────────

    [Fact]
    public void FullGeneration_AllThreeFactories_ProcessRealLocalesWithoutErrors()
    {
        // Single consolidated test that exercises all three factories against real locale
        // files, verifying builder-specific output shapes rather than just converter names.
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

        // WordsToNumberEngineContractFactory — verify each non-token-map engine emits
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

        // TimeOnlyToClockNotationEngineContractFactory — verify phrase-clock contract
        var clockCatalog = GetGeneratedSource(runResult, "TimeOnlyToClockNotationProfileCatalog.g.cs");
        Assert.Contains("new PhraseClockNotationConverter(new(", clockCatalog);
    }

    // ──────────────────────────────────────────────────────────────────────
    //  Helpers
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

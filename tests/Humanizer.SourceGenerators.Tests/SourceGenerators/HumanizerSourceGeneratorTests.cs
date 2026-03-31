using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
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

public class HumanizerSourceGeneratorTests
{
    static readonly Lazy<ImmutableDictionary<string, string>> generatedSources = new(GenerateSources);

    [Fact]
    public void FormatterRegistryRegistrationsUseGeneratedProfilesForSharedFormatters()
    {
        var registrySource = GetGeneratedSource("FormatterRegistryRegistrations.g.cs");
        var catalogSource = GetGeneratedSource("FormatterProfileCatalog.g.cs");

        Assert.Contains("registry.Register(\"bg\", culture => FormatterProfileCatalog.Resolve(\"bulgarian\", culture));", registrySource);
        Assert.Contains("registry.Register(\"hr\", culture => FormatterProfileCatalog.Resolve(\"south-slavic-paucal\", culture));", registrySource);
        Assert.Contains("registry.Register(\"sr\", culture => FormatterProfileCatalog.Resolve(\"south-slavic-paucal\", culture));", registrySource);
        Assert.Contains("registry.Register(\"de\", culture => FormatterProfileCatalog.Resolve(\"trim-plural-suffix\", culture));", registrySource);
        Assert.Contains("registry.Register(\"lb\", culture => FormatterProfileCatalog.Resolve(\"luxembourgish\", culture));", registrySource);
        Assert.Contains("registry.Register(\"ro\", culture => FormatterProfileCatalog.Resolve(\"romanian\", culture));", registrySource);
        Assert.Contains("new ProfiledFormatter(culture, ", catalogSource);
        Assert.Contains("static FormatterProfile luxembourgish", catalogSource);
        Assert.Contains("static FormatterProfile romanian", catalogSource);
        Assert.Contains("static FormatterProfile swedish", catalogSource);
        Assert.Contains("static FormatterProfile trim_plural_suffix", catalogSource);
        Assert.DoesNotContain("new BulgarianFormatter", registrySource);
        Assert.DoesNotContain("new LuxembourgishFormatter", registrySource);
        Assert.DoesNotContain("new RomanianFormatter", registrySource);
        Assert.DoesNotContain("new SwedishFormatter", registrySource);
    }

    [Fact]
    public void OrdinalizerGenerationUsesCatalogOnlyForDataBackedProfiles()
    {
        var registrySource = GetGeneratedSource("OrdinalizerRegistryRegistrations.g.cs");
        var catalogSource = GetGeneratedSource("OrdinalizerProfileCatalog.g.cs");

        Assert.Contains("registry.Register(\"en\", culture => OrdinalizerProfileCatalog.Resolve(\"english\", culture));", registrySource);
        Assert.Contains("registry.Register(\"af\", culture => OrdinalizerProfileCatalog.Resolve(\"afrikaans\", culture));", registrySource);
        Assert.Contains("registry.Register(\"fr\", culture => OrdinalizerProfileCatalog.Resolve(\"french\", culture));", registrySource);
        Assert.Contains("registry.Register(\"de\", culture => OrdinalizerProfileCatalog.Resolve(\"period-suffix\", culture));", registrySource);
        Assert.Contains("registry.Register(\"es\", culture => OrdinalizerProfileCatalog.Resolve(\"spanish\", culture));", registrySource);

        Assert.Contains("case \"english\":", catalogSource);
        Assert.Contains("case \"afrikaans\":", catalogSource);
        Assert.Contains("case \"french\":", catalogSource);
        Assert.Contains("new ModuloSuffixOrdinalizer(", catalogSource);
        Assert.Contains("new TemplateOrdinalizer(", catalogSource);
        Assert.Contains("new WordFormTemplateOrdinalizer(", catalogSource);
        Assert.Contains("case \"period-suffix\":", catalogSource);
        Assert.Contains("case \"spanish\":", catalogSource);
        Assert.DoesNotContain("new SpanishOrdinalizer", catalogSource);
    }

    [Fact]
    public void TimeOnlyClockNotationProfilesAreFullyGenerated()
    {
        var source = GetGeneratedSource("TimeOnlyToClockNotationProfileCatalog.g.cs");

        Assert.Contains("case \"french\": return french;", source);
        Assert.Contains("case \"german\": return german;", source);
        Assert.Contains("case \"luxembourgish\": return luxembourgish;", source);
        Assert.Contains("new FrenchTimeOnlyToClockNotationConverter()", source);
        Assert.Contains("static ITimeOnlyToClockNotationConverter german => german_cache.Value;", source);
        Assert.Contains("static class german_cache", source);
        Assert.Contains("internal static readonly ITimeOnlyToClockNotationConverter Value = new GermanTimeOnlyToClockNotationConverter();", source);
        Assert.Contains("new LuxembourgishTimeOnlyToClockNotationConverter()", source);
        Assert.DoesNotContain("TryResolveCustom", source);
    }

    [Fact]
    public void NumberToWordsProfilesUseSharedFamilyConverters()
    {
        var source = GetGeneratedSource("NumberToWordsProfileCatalog.g.cs");
        var registrySource = GetGeneratedSource("NumberToWordsConverterRegistryRegistrations.g.cs");

        Assert.Contains("case \"english\": return english;", source);
        Assert.Contains("case \"indian\": return indian;", source);
        Assert.Contains("case \"farsi\": return farsi;", source);
        Assert.Contains("case \"armenian\": return armenian;", source);
        Assert.Contains("case \"arabic\": return arabic;", source);
        Assert.Contains("case \"catalan\": return catalan;", source);
        Assert.Contains("case \"tamil\": return tamil;", source);
        Assert.Contains("case \"thai\": return thai;", source);
        Assert.Contains("case \"central-kurdish\": return central-kurdish;", source.Replace('_', '-'));
        Assert.Contains("new EnglishFamilyNumberToWordsConverter(", source);
        Assert.Contains("new JoinedScaleNumberToWordsConverter(", source);
        Assert.Contains("new AgglutinativeOrdinalScaleNumberToWordsConverter(", source);
        Assert.Contains("new ContextualDecimalNumberToWordsConverter(", source);
        Assert.Contains("new LinkingScaleNumberToWordsConverter(", source);
        Assert.Contains("new AppendedGroupNumberToWordsConverter(new(", source);
        Assert.Contains("new HyphenatedOrdinalNumberToWordsConverter(new(", source);
        Assert.Contains("new IndianGroupingNumberToWordsConverter(new(", source);
        Assert.Contains("new LongScaleStemOrdinalNumberToWordsConverter(new(", source);
        Assert.Contains("new WestSlavicGenderedNumberToWordsConverter(", source);
        Assert.Contains("case \"russian\": return russian;", source);
        Assert.Contains("case \"ukrainian\": return ukrainian;", source);
        Assert.Contains("case \"greek\": return greek;", source);
        Assert.Contains("case \"hebrew\": return new ConstructStateScaleNumberToWordsConverter(new(", source);
        Assert.Contains("case \"italian\": return italian;", source);
        Assert.Contains("case \"latvian\": return latvian;", source);
        Assert.Contains("case \"romanian\": return romanian;", source);
        Assert.Contains("case \"polish\": return new PluralizedScaleNumberToWordsConverter(new(", source);
        Assert.Contains("case \"lithuanian\": return lithuanian;", source);
        Assert.Contains("registry.Register(\"hr\", culture => NumberToWordsProfileCatalog.Resolve(\"croatian\", culture));", registrySource);
        Assert.Contains("registry.Register(\"sr\", culture => NumberToWordsProfileCatalog.Resolve(\"serbian-cyrl\", culture));", registrySource);
        Assert.Contains("registry.Register(\"sr-Latn\", culture => NumberToWordsProfileCatalog.Resolve(\"serbian-latn\", culture));", registrySource);
        Assert.Contains("registry.Register(\"sl\", culture => NumberToWordsProfileCatalog.Resolve(\"slovenian\", culture));", registrySource);
        Assert.Contains("case \"norwegian-bokmal\": return norwegian_bokmal;", source);
        Assert.Contains("case \"swedish\": return swedish;", source);
        Assert.Contains("case \"danish\": return danish;", source);
        Assert.Contains("case \"afrikaans\": return afrikaans;", source);
        Assert.Contains("case \"dutch\": return dutch;", source);
        Assert.Contains("case \"icelandic\": return icelandic;", source);
        Assert.Contains("case \"slovenian\": return new SouthSlavicCardinalNumberToWordsConverter(new(", source);
        Assert.Contains("case \"luxembourgish\": return luxembourgish;", source);
        Assert.Contains("new InvertedTensNumberToWordsConverter(", source);
        Assert.Contains("new OrdinalPrefixScaleNumberToWordsConverter(", source);
        Assert.Contains("new ScandinavianFamilyNumberToWordsConverter(", source);
        Assert.Contains("new EastSlavicNumberToWordsConverter(", source);
        Assert.Contains("new SouthSlavicCardinalNumberToWordsConverter(", source);
        Assert.Contains("new GermanFamilyNumberToWordsConverter(new(", source);
        Assert.Contains("new SegmentedScaleNumberToWordsConverter(new(", source);
        Assert.Contains("new TriadScaleNumberToWordsConverter(new(", source);
        Assert.Contains("new ConstructStateScaleNumberToWordsConverter(new(", source);
        Assert.Contains("new GenderedScaleOrdinalNumberToWordsConverter(new(", source);
        Assert.Contains("new TerminalOrdinalScaleNumberToWordsConverter(new(", source);
        Assert.Contains("new PluralizedScaleNumberToWordsConverter(new(", source);
        Assert.Contains("new ConjoinedGenderedScaleNumberToWordsConverter(new(", source);
        Assert.Contains("new HyphenatedScaleNumberToWordsConverter(new(", source);
        Assert.Contains("new DualFormScaleNumberToWordsConverter(new(", source);
        Assert.DoesNotContain("new CzechNumberToWordsConverter", source);
        Assert.DoesNotContain("new SlovakNumberToWordsConverter", source);
        Assert.DoesNotContain("new AfrikaansNumberToWordsConverter", source);
        Assert.DoesNotContain("new DanishNumberToWordsConverter", source);
        Assert.DoesNotContain("new DutchNumberToWordsConverter", source);
        Assert.DoesNotContain("new ArabicNumberToWordsConverter", source);
        Assert.DoesNotContain("new ArmenianNumberToWordsConverter", source);
        Assert.DoesNotContain("new CatalanNumberToWordsConverter", source);
        Assert.DoesNotContain("new IcelandicNumberToWordsConverter", source);
        Assert.DoesNotContain("new ItalianNumberToWordsConverter", source);
        Assert.DoesNotContain("new RomanianNumberToWordsConverter", source);
        Assert.DoesNotContain("new NorwegianBokmalNumberToWordsConverter", source);
        Assert.DoesNotContain("new SwedishNumberToWordsConverter", source);
        Assert.DoesNotContain("new RussianNumberToWordsConverter", source);
        Assert.DoesNotContain("new GreekNumberToWordsConverter", source);
        Assert.DoesNotContain("new HebrewNumberToWordsConverter", source);
        Assert.DoesNotContain("new LatvianNumberToWordsConverter", source);
        Assert.DoesNotContain("new ThaiNumberToWordsConverter", source);
        Assert.DoesNotContain("new UkrainianNumberToWordsConverter", source);
        Assert.DoesNotContain("new PolishNumberToWordsConverter", source);
        Assert.DoesNotContain("new LithuanianNumberToWordsConverter", source);
        Assert.DoesNotContain("new FinnishNumberToWordsConverter", source);
        Assert.DoesNotContain("new FilipinoNumberToWordsConverter", source);
        Assert.DoesNotContain("new VietnameseNumberToWordsConverter", source);
        Assert.DoesNotContain("new CroatianNumberToWordsConverter", source);
        Assert.DoesNotContain("new GermanNumberToWordsConverter", source);
        Assert.DoesNotContain("new GermanSwissLiechtensteinNumberToWordsConverter", source);
        Assert.DoesNotContain("new LuxembourgishNumberToWordsConverter", source);
        Assert.DoesNotContain("new SlovenianNumberToWordsConverter", source);
        Assert.DoesNotContain("new SpanishNumberToWordsConverter", source);
        Assert.DoesNotContain("new TamilNumberToWordsConverter", source);
        Assert.DoesNotContain("new SerbianFamilyNumberToWordsConverter", source);
        Assert.DoesNotContain("new ConjoinedGenderedScaleNumberToWordsConverter()", source);
        Assert.DoesNotContain("new HyphenatedScaleNumberToWordsConverter()", source);
        Assert.DoesNotContain("new DualFormScaleNumberToWordsConverter()", source);
    }

    [Fact]
    public void WordsToNumberRegistrationsUseLexiconConvertersForKurdishAndVietnamese()
    {
        var registrySource = GetGeneratedSource("WordsToNumberConverterRegistryRegistrations.g.cs");
        var profileCatalogSource = GetGeneratedSource("WordsToNumberProfileCatalog.g.cs");
        var tokenMapIndexSource = GetGeneratedSource("TokenMapWordsToNumberConverters.Index.g.cs");

        Assert.Contains("registry.Register(\"ku\", culture => TokenMapWordsToNumberConverters.Ku);", registrySource);
        Assert.Contains("registry.Register(\"vi\", culture => TokenMapWordsToNumberConverters.Vi);", registrySource);
        Assert.DoesNotContain("case \"kurdish\":", profileCatalogSource);
        Assert.DoesNotContain("case \"vietnamese\":", profileCatalogSource);
        Assert.Contains("\"ku\" => Ku", tokenMapIndexSource);
        Assert.Contains("\"vi\" => Vi", tokenMapIndexSource);
    }

    [Fact]
    public void WordsToNumberProfilesUseSharedEnginesForEastAsianAndTokenMaps()
    {
        var source = GetGeneratedSource("WordsToNumberProfileCatalog.g.cs");

        Assert.Contains("case \"chinese\": return chinese;", source);
        Assert.Contains("case \"thai\": return thai;", source);
        Assert.Contains("new EastAsianPositionalWordsToNumberConverter(", source);
        Assert.Contains("case \"slovenian\": return slovenian;", source);
        Assert.Contains("new InvertedTensWordsToNumberConverter(", source);
        Assert.Contains("case \"arabic\": return arabic;", source);
        Assert.Contains("new SuffixScaleWordsToNumberConverter(new(", source);
        Assert.Contains("new PrefixedTensScaleWordsToNumberConverter(new(", source);
        Assert.Contains("new LinkingAffixWordsToNumberConverter(new(", source);
        Assert.Contains("new VigesimalCompoundWordsToNumberConverter(new(", source);
        Assert.Contains("new TokenMapWordsToNumberConverter(new()", source);
        Assert.Contains("case \"hebrew\": return hebrew;", source);
        Assert.Contains("case \"italian\": return italian;", source);
        Assert.Contains("new GreedyCompoundWordsToNumberConverter(new(", source);
        Assert.DoesNotContain("new ChineseWordsToNumberConverter()", source);
        Assert.DoesNotContain("new JapaneseWordsToNumberConverter()", source);
        Assert.DoesNotContain("new KoreanWordsToNumberConverter()", source);
        Assert.DoesNotContain("new ArabicWordsToNumberConverter()", source);
        Assert.DoesNotContain("new HebrewWordsToNumberConverter()", source);
        Assert.DoesNotContain("new ItalianWordsToNumberConverter()", source);
        Assert.DoesNotContain("new SlovenianWordsToNumberConverter()", source);
        Assert.DoesNotContain("new ThaiWordsToNumberConverter()", source);
        Assert.DoesNotContain("new VietnameseWordsToNumberConverter()", source);
        Assert.DoesNotContain("new FinnishWordsToNumberConverter()", source);
        Assert.DoesNotContain("new FrenchWordsToNumberConverter()", source);
        Assert.DoesNotContain("new FilipinoWordsToNumberConverter()", source);
        Assert.DoesNotContain("new HungarianWordsToNumberConverter()", source);
    }

    [Fact]
    public void TokenMapOrdinalMapsAreGeneratedFromLocaleData()
    {
        var azerbaijani = GetGeneratedSource("TokenMapWordsToNumberConverters.Az.g.cs");
        var spanish = GetGeneratedSource("TokenMapWordsToNumberConverters.Es.g.cs");
        var ukrainian = GetGeneratedSource("TokenMapWordsToNumberConverters.Uk.g.cs");

        Assert.DoesNotContain("TokenMapWordsToNumberOrdinalMapBuilder.Build(", azerbaijani);
        Assert.DoesNotContain("TokenMapWordsToNumberOrdinalMapBuilder.Build(", spanish);
        Assert.DoesNotContain("TokenMapWordsToNumberOrdinalMapBuilder.Build(", ukrainian);
        Assert.DoesNotContain("NumberToWordsProfileCatalog.Resolve(", azerbaijani);
        Assert.DoesNotContain("NumberToWordsProfileCatalog.Resolve(", spanish);
        Assert.DoesNotContain("NumberToWordsProfileCatalog.Resolve(", ukrainian);
        Assert.Contains("ExactOrdinalMap = ", azerbaijani);
        Assert.Contains("OrdinalScaleMap = ", spanish);
        Assert.Contains("OrdinalScaleMap = ", ukrainian);
        Assert.Contains("GluedOrdinalScaleSuffixes = ", spanish);
    }

    [Fact]
    public void TokenMapLocalesReportDiagnosticsAndSkipGenerationWhenInputIsMalformed()
    {
        const string invalidLocale = """
{
  "normalizationProfile": 42,
  "cardinalMap": {
    "one": "1"
  },
  "ordinalGenderVariant": "bogus",
  "negativePrefixes": [
    "minus ",
    5
  ],
  "scaleThreshold": "1000"
}
""";

        var runResult = RunGenerator(
            new InMemoryAdditionalText(
                @"E:\Dev\Humanizer\src\Humanizer\CodeGen\WordsToNumber\zz-invalid.json",
                invalidLocale));

        var diagnostics = runResult.Diagnostics
            .Where(static diagnostic => diagnostic.Id == "HSG001")
            .Select(static diagnostic => diagnostic.GetMessage())
            .ToArray();

        Assert.Contains(diagnostics, static message => message.Contains("zz-invalid.normalizationProfile", StringComparison.Ordinal));
        Assert.Contains(diagnostics, static message => message.Contains("zz-invalid.cardinalMap.one", StringComparison.Ordinal));
        Assert.Contains(diagnostics, static message => message.Contains("zz-invalid.ordinalGenderVariant", StringComparison.Ordinal));
        Assert.Contains(diagnostics, static message => message.Contains("zz-invalid.negativePrefixes[1]", StringComparison.Ordinal));
        Assert.Contains(diagnostics, static message => message.Contains("zz-invalid.scaleThreshold", StringComparison.Ordinal));
        Assert.DoesNotContain(runResult.Results[0].GeneratedSources, static source => source.HintName == "TokenMapWordsToNumberConverters.ZzInvalid.g.cs");
    }

    [Fact]
    public void GeneratedCatalogsUsePerEntryLazyHolders()
    {
        var formatterCatalog = GetGeneratedSource("FormatterProfileCatalog.g.cs");
        var numberToWordsCatalog = GetGeneratedSource("NumberToWordsProfileCatalog.g.cs");
        var wordsToNumberCatalog = GetGeneratedSource("WordsToNumberProfileCatalog.g.cs");
        var timeOnlyCatalog = GetGeneratedSource("TimeOnlyToClockNotationProfileCatalog.g.cs");
        var dateCatalog = GetGeneratedSource("OrdinalDateProfileCatalog.DateTo.g.cs");
        var dateOnlyCatalog = GetGeneratedSource("OrdinalDateProfileCatalog.DateOnly.g.cs");

        Assert.Contains("static FormatterProfile luxembourgish => luxembourgish_cache.Value;", formatterCatalog);
        Assert.Contains("static class luxembourgish_cache", formatterCatalog);
        Assert.DoesNotContain("static FormatterProfile luxembourgish { get; } =", formatterCatalog);

        Assert.Contains("static INumberToWordsConverter english => english_cache.Value;", numberToWordsCatalog);
        Assert.Contains("static class english_cache", numberToWordsCatalog);
        Assert.DoesNotContain("static INumberToWordsConverter english { get; } =", numberToWordsCatalog);

        Assert.Contains("static IWordsToNumberConverter arabic => arabic_cache.Value;", wordsToNumberCatalog);
        Assert.Contains("static class arabic_cache", wordsToNumberCatalog);
        Assert.DoesNotContain("static IWordsToNumberConverter arabic { get; } =", wordsToNumberCatalog);

        Assert.Contains("static ITimeOnlyToClockNotationConverter german => german_cache.Value;", timeOnlyCatalog);
        Assert.Contains("static class german_cache", timeOnlyCatalog);
        Assert.DoesNotContain("static ITimeOnlyToClockNotationConverter german { get; } =", timeOnlyCatalog);

        Assert.Contains("static IDateToOrdinalWordConverter spanish => spanish_cache.Value;", dateCatalog);
        Assert.Contains("static class spanish_cache", dateCatalog);
        Assert.DoesNotContain("static IDateToOrdinalWordConverter spanish { get; } =", dateCatalog);

        Assert.Contains("static IDateOnlyToOrdinalWordConverter spanish => spanish_cache.Value;", dateOnlyCatalog);
        Assert.Contains("static class spanish_cache", dateOnlyCatalog);
        Assert.DoesNotContain("static IDateOnlyToOrdinalWordConverter spanish { get; } =", dateOnlyCatalog);
    }

    [Fact]
    public void NumberToWordsProfilesUseSharedEastAsianEngine()
    {
        var source = GetGeneratedSource("NumberToWordsProfileCatalog.g.cs");

        Assert.Contains("case \"chinese\": return chinese;", source);
        Assert.Contains("case \"japanese\": return japanese;", source);
        Assert.Contains("case \"korean\": return korean;", source);
        Assert.Contains("new EastAsianGroupedNumberToWordsConverter(", source);
        Assert.DoesNotContain("new ChineseNumberToWordsConverter()", source);
        Assert.DoesNotContain("new JapaneseNumberToWordsConverter()", source);
        Assert.DoesNotContain("new KoreanNumberToWordsConverter()", source);
    }

    [Fact]
    public void NumberAndWordProfilesPreferSharedFamilyAndLexiconEngines()
    {
        var numberSource = GetGeneratedSource("NumberToWordsProfileCatalog.g.cs");
        var wordsRegistrySource = GetGeneratedSource("WordsToNumberConverterRegistryRegistrations.g.cs");
        var wordsCatalogSource = GetGeneratedSource("WordsToNumberProfileCatalog.g.cs");

        Assert.Contains("case \"english\": return english;", numberSource);
        Assert.Contains("case \"indian\": return indian;", numberSource);
        Assert.Contains("new EnglishFamilyNumberToWordsConverter(", numberSource);
        Assert.Contains("case \"farsi\": return farsi;", numberSource);
        Assert.Contains("case \"central-kurdish\": return central_kurdish;", numberSource);
        Assert.Contains("case \"bangla\": return bangla;", numberSource);
        Assert.Contains("new JoinedScaleNumberToWordsConverter(", numberSource);

        Assert.Contains("registry.Register(\"ku\", culture => TokenMapWordsToNumberConverters.Ku);", wordsRegistrySource);
        Assert.Contains("registry.Register(\"mt\", culture => TokenMapWordsToNumberConverters.Mt);", wordsRegistrySource);
        Assert.DoesNotContain("case \"kurdish\":", wordsCatalogSource);
        Assert.DoesNotContain("case \"maltese\":", wordsCatalogSource);
    }

    [Fact]
    public void ExistingNumberToWordsFamiliesPreferExplicitStrategiesOverLocaleSpecificBranches()
    {
        var sourceRoot = Path.Combine(FindRepositoryRoot(), "src", "Humanizer", "CodeGen", "Schemas", "NumberToWords");
        var englishSchema = File.ReadAllText(Path.Combine(sourceRoot, "english-family.json"));
        var frenchSchema = File.ReadAllText(Path.Combine(sourceRoot, "french-family.json"));
        var malaySchema = File.ReadAllText(Path.Combine(sourceRoot, "malay-family.json"));
        var portugueseSchema = File.ReadAllText(Path.Combine(sourceRoot, "portuguese-family.json"));
        var scandinavianSchema = File.ReadAllText(Path.Combine(sourceRoot, "scandinavian-family.json"));

        Assert.Contains("\"addAndMode\"", englishSchema);
        Assert.Contains("\"andStrategy\"", englishSchema);
        Assert.Contains("\"tupleSuffix\"", englishSchema);
        Assert.Contains("\"ordinalLeadingOneStrategy\"", englishSchema);
        Assert.DoesNotContain("\"respectAddAndFlag\"", englishSchema);
        Assert.DoesNotContain("\"useAndWithinGroup\"", englishSchema);
        Assert.DoesNotContain("\"useAndAfterScaleForSubHundredRemainder\"", englishSchema);
        Assert.DoesNotContain("\"omitLeadingOneInOrdinal\"", englishSchema);

        Assert.Contains("\"seventyStrategy\"", frenchSchema);
        Assert.Contains("\"ninetyStrategy\"", frenchSchema);
        Assert.Contains("\"specialSeventyOneWord\"", frenchSchema);
        Assert.Contains("\"pluralizeExactEighty\"", frenchSchema);
        Assert.Contains("\"tensUsingEtWhenUnitIsOne\"", frenchSchema);
        Assert.Contains("\"tensMap\"", frenchSchema);
        Assert.DoesNotContain("\"style\"", frenchSchema);

        Assert.Contains("\"billionCardinalStrategy\"", portugueseSchema);
        Assert.Contains("\"billionOrdinalStrategy\"", portugueseSchema);
        Assert.Contains("\"millionSingularWord\"", portugueseSchema);
        Assert.Contains("\"millionPluralWord\"", portugueseSchema);
        Assert.Contains("\"thousandWord\"", portugueseSchema);
        Assert.Contains("\"billionSingularWord\"", portugueseSchema);
        Assert.Contains("\"billionPluralWord\"", portugueseSchema);
        Assert.Contains("\"thousandOrdinalWord\"", portugueseSchema);
        Assert.Contains("\"millionOrdinalWord\"", portugueseSchema);
        Assert.Contains("\"billionOrdinalWord\"", portugueseSchema);
        Assert.DoesNotContain("\"style\"", portugueseSchema);

        Assert.Contains("\"oneWord\"", File.ReadAllText(Path.Combine(FindRepositoryRoot(), "src", "Humanizer", "CodeGen", "Profiles", "NumberToWords", "malay.json")));
        Assert.DoesNotContain("\"thousandOneWord\"", malaySchema);

        Assert.Contains("\"cardinalStrategy\"", scandinavianSchema);
        Assert.Contains("\"ordinalStrategy\"", scandinavianSchema);
        Assert.Contains("\"largeScaleRemainderJoiner\"", scandinavianSchema);
        Assert.Contains("\"exactLargeScaleOrdinalSuffix\"", scandinavianSchema);
        Assert.Contains("\"exactDefaultOrdinalSuffix\"", scandinavianSchema);
        Assert.Contains("\"tensOrdinalTrimEndCharacters\"", scandinavianSchema);
        Assert.Contains("\"tensOrdinalSuffix\"", scandinavianSchema);
        Assert.Contains("\"shortOrdinalUpperBoundExclusive\"", scandinavianSchema);
        Assert.Contains("\"shortOrdinalTrimEndCharacters\"", scandinavianSchema);
        Assert.Contains("\"shortOrdinalTrimmedSuffix\"", scandinavianSchema);
        Assert.Contains("\"shortOrdinalSuffix\"", scandinavianSchema);
        Assert.DoesNotContain("\"style\"", scandinavianSchema);

        var turkicSchema = File.ReadAllText(Path.Combine(sourceRoot, "turkic-family.json"));
        Assert.Contains("\"softenTerminalTBeforeSuffix\"", turkicSchema);
        Assert.Contains("\"dropTerminalVowelBeforeHarmonySuffix\"", turkicSchema);
    }

    [Fact]
    public void UzbekProfilesAreAbsorbedIntoTheSharedTurkicFamily()
    {
        var source = GetGeneratedSource("NumberToWordsProfileCatalog.g.cs");
        var profilesRoot = Path.Combine(FindRepositoryRoot(), "src", "Humanizer", "CodeGen", "Profiles", "NumberToWords");
        var uzbekLatinProfile = File.ReadAllText(Path.Combine(profilesRoot, "uzbek-latn.json"));
        var uzbekCyrillicProfile = File.ReadAllText(Path.Combine(profilesRoot, "uzbek-cyrl.json"));

        Assert.Contains("\"engine\": \"turkic-family\"", uzbekLatinProfile);
        Assert.Contains("\"engine\": \"turkic-family\"", uzbekCyrillicProfile);
        Assert.Contains("new TurkicFamilyNumberToWordsConverter(", source);
        Assert.DoesNotContain("new UzbekFamilyNumberToWordsConverter(", source);
    }

    static string GetGeneratedSource(string hintName) =>
        generatedSources.Value.TryGetValue(hintName, out var source)
            ? source
            : throw new Xunit.Sdk.XunitException($"Generated source '{hintName}' was not found.");

    static ImmutableDictionary<string, string> GenerateSources()
    {
        var runResult = RunGenerator();
        Assert.Empty(runResult.Diagnostics);
        Assert.Single(runResult.Results);

        return runResult.Results[0].GeneratedSources.ToImmutableDictionary(
            static source => source.HintName,
            static source => source.SourceText.ToString(),
            StringComparer.Ordinal);
    }

    static CSharpCompilation CreateCompilation()
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(
            SourceText.From("namespace Humanizer; internal static class GeneratorHarness { }"),
            new CSharpParseOptions(LanguageVersion.Preview));

        return CSharpCompilation.Create(
            "Humanizer.SourceGenerator.Tests",
            [syntaxTree],
            GetMetadataReferences(),
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
    }

    static ImmutableArray<MetadataReference> GetMetadataReferences()
    {
        var references = new Dictionary<string, MetadataReference>(StringComparer.OrdinalIgnoreCase);

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
            typeof(CultureInfo).Assembly,
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

    static ImmutableArray<AdditionalText> GetAdditionalTexts(params AdditionalText[] extraAdditionalTexts)
    {
        var root = FindRepositoryRoot();
        var sourceRoot = Path.Combine(root, "src", "Humanizer");
        var filePaths = new List<string>
        {
            Path.Combine(sourceRoot, "Properties", "Resources.resx")
        };

        AddFiles(filePaths, Path.Combine(sourceRoot, "CodeGen", "Locales"), "*.yml");
        AddFiles(filePaths, Path.Combine(sourceRoot, "CodeGen", "Profiles", "NumberToWords"), "*.json");
        AddFiles(filePaths, Path.Combine(sourceRoot, "CodeGen", "Schemas", "NumberToWords"), "*.json");
        AddFiles(filePaths, Path.Combine(sourceRoot, "CodeGen", "Profiles", "Formatters"), "*.json");
        AddFiles(filePaths, Path.Combine(sourceRoot, "CodeGen", "Profiles", "Ordinalizers"), "*.json");
        AddFiles(filePaths, Path.Combine(sourceRoot, "CodeGen", "Profiles", "OrdinalDates"), "*.json");
        AddFiles(filePaths, Path.Combine(sourceRoot, "CodeGen", "Profiles", "TimeOnlyToClockNotation"), "*.json");
        AddFiles(filePaths, Path.Combine(sourceRoot, "CodeGen", "Schemas", "TimeOnlyToClockNotation"), "*.json");
        AddFiles(filePaths, Path.Combine(sourceRoot, "CodeGen", "Profiles", "WordsToNumber"), "*.json");
        AddFiles(filePaths, Path.Combine(sourceRoot, "CodeGen", "Schemas", "WordsToNumber"), "*.json");
        AddFiles(filePaths, Path.Combine(sourceRoot, "CodeGen", "WordsToNumber"), "*.json");

        var additionalTexts = filePaths
            .OrderBy(static path => path, StringComparer.OrdinalIgnoreCase)
            .Select(static path => (AdditionalText)new FileAdditionalText(path))
            .ToImmutableArray();

        return extraAdditionalTexts.Length == 0
            ? additionalTexts
            : additionalTexts.AddRange(extraAdditionalTexts);
    }

    static void AddFiles(List<string> filePaths, string directory, string searchPattern)
    {
        if (!Directory.Exists(directory))
        {
            return;
        }

        filePaths.AddRange(Directory.GetFiles(directory, searchPattern, SearchOption.TopDirectoryOnly));
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
        readonly string path = path;

        public override string Path => path;

        public override SourceText GetText(CancellationToken cancellationToken = default) =>
            SourceText.From(File.ReadAllText(path), Encoding.UTF8);
    }

    sealed class InMemoryAdditionalText(string path, string text) : AdditionalText
    {
        readonly string path = path;
        readonly string text = text;

        public override string Path => path;

        public override SourceText GetText(CancellationToken cancellationToken = default) =>
            SourceText.From(text, Encoding.UTF8);
    }
}

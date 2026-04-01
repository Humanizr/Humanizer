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

        Assert.Contains("registry.Register(\"bg\", culture => FormatterProfileCatalog.Resolve(", registrySource);
        Assert.Contains("registry.Register(\"hr\", culture => FormatterProfileCatalog.Resolve(", registrySource);
        Assert.Contains("registry.Register(\"sr\", culture => FormatterProfileCatalog.Resolve(", registrySource);
        Assert.Contains("registry.Register(\"de\", culture => FormatterProfileCatalog.Resolve(", registrySource);
        Assert.Contains("new ProfiledFormatter(culture, ", catalogSource);
        Assert.DoesNotContain("new BulgarianFormatter", registrySource);
        Assert.DoesNotContain("new SwedishFormatter", registrySource);
    }

    [Fact]
    public void OrdinalizerGenerationUsesCatalogOnlyForDataBackedProfiles()
    {
        var registrySource = GetGeneratedSource("OrdinalizerRegistryRegistrations.g.cs");
        var catalogSource = GetGeneratedSource("OrdinalizerProfileCatalog.g.cs");

        Assert.Contains("registry.Register(\"en\", culture => OrdinalizerProfileCatalog.Resolve(", registrySource);
        Assert.Contains("registry.Register(\"af\", culture => OrdinalizerProfileCatalog.Resolve(", registrySource);
        Assert.Contains("registry.Register(\"fr\", culture => OrdinalizerProfileCatalog.Resolve(", registrySource);
        Assert.Contains("registry.Register(\"de\", culture => OrdinalizerProfileCatalog.Resolve(", registrySource);
        Assert.Contains("registry.Register(\"es\", culture => OrdinalizerProfileCatalog.Resolve(", registrySource);

        Assert.Contains("new ModuloSuffixOrdinalizer(", catalogSource);
        Assert.Contains("new TemplateOrdinalizer(", catalogSource);
        Assert.Contains("new WordFormTemplateOrdinalizer(", catalogSource);
        Assert.DoesNotContain("new SpanishOrdinalizer", catalogSource);
    }

    [Fact]
    public void TimeOnlyClockNotationProfilesAreFullyGenerated()
    {
        var source = GetGeneratedSource("TimeOnlyToClockNotationProfileCatalog.g.cs");

        Assert.Contains("new FrenchTimeOnlyToClockNotationConverter()", source);
        Assert.Contains("new GermanTimeOnlyToClockNotationConverter()", source);
        Assert.Contains("new LuxembourgishTimeOnlyToClockNotationConverter()", source);
        Assert.DoesNotContain("TryResolveCustom", source);
    }

    [Fact]
    public void NumberToWordsProfilesUseSharedFamilyConverters()
    {
        var source = GetGeneratedSource("NumberToWordsProfileCatalog.g.cs");
        var registrySource = GetGeneratedSource("NumberToWordsConverterRegistryRegistrations.g.cs");

        Assert.Contains("new ConjunctionalScaleNumberToWordsConverter(", source);
        Assert.Contains("new JoinedScaleNumberToWordsConverter(", source);
        Assert.Contains("new AgglutinativeOrdinalScaleNumberToWordsConverter(", source);
        Assert.Contains("new ContextualDecimalNumberToWordsConverter(", source);
        Assert.Contains("new LinkingScaleNumberToWordsConverter(", source);
        Assert.Contains("new AppendedGroupNumberToWordsConverter(new(", source);
        Assert.Contains("new HyphenatedOrdinalNumberToWordsConverter(new(", source);
        Assert.Contains("new IndianGroupingNumberToWordsConverter(new(", source);
        Assert.Contains("new LongScaleStemOrdinalNumberToWordsConverter(new(", source);
        Assert.Contains("new WestSlavicGenderedNumberToWordsConverter(", source);
        Assert.Contains("registry.Register(\"hr\", culture => NumberToWordsProfileCatalog.Resolve(", registrySource);
        Assert.Contains("registry.Register(\"sr\", culture => NumberToWordsProfileCatalog.Resolve(", registrySource);
        Assert.Contains("registry.Register(\"sr-Latn\", culture => NumberToWordsProfileCatalog.Resolve(", registrySource);
        Assert.Contains("registry.Register(\"sl\", culture => NumberToWordsProfileCatalog.Resolve(", registrySource);
        Assert.Contains("new InvertedTensNumberToWordsConverter(", source);
        Assert.Contains("new OrdinalPrefixScaleNumberToWordsConverter(", source);
        Assert.Contains("new ScaleStrategyNumberToWordsConverter(", source);
        Assert.Contains("new EastSlavicNumberToWordsConverter(", source);
        Assert.Contains("new SouthSlavicCardinalNumberToWordsConverter(", source);
        Assert.Contains("new UnitLeadingCompoundNumberToWordsConverter(new(", source);
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
        Assert.DoesNotContain("new ArabicNumberToWordsConverter(culture)", source);
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

        Assert.Contains("registry.Register(\"en\", culture => TokenMapWordsToNumberConverters.En);", registrySource);
        Assert.Contains("registry.Register(\"ku\", culture => TokenMapWordsToNumberConverters.Ku);", registrySource);
        Assert.Contains("registry.Register(\"vi\", culture => TokenMapWordsToNumberConverters.Vi);", registrySource);
        Assert.DoesNotContain("case \"en\":", profileCatalogSource);
        Assert.DoesNotContain("case \"kurdish\":", profileCatalogSource);
        Assert.DoesNotContain("case \"vietnamese\":", profileCatalogSource);
        Assert.Contains("\"en\" => En", tokenMapIndexSource);
        Assert.Contains("\"ku\" => Ku", tokenMapIndexSource);
        Assert.Contains("\"vi\" => Vi", tokenMapIndexSource);
    }

    [Fact]
    public void WordsToNumberProfilesUseSharedEnginesForEastAsianAndTokenMaps()
    {
        var source = GetGeneratedSource("WordsToNumberProfileCatalog.g.cs");

        Assert.Contains("new EastAsianPositionalWordsToNumberConverter(", source);
        Assert.Contains("new InvertedTensWordsToNumberConverter(", source);
        Assert.Contains("new SuffixScaleWordsToNumberConverter(new SuffixScaleWordsToNumberProfile(", source);
        Assert.Contains("new PrefixedTensScaleWordsToNumberConverter(new PrefixedTensScaleWordsToNumberProfile(", source);
        Assert.Contains("new LinkingAffixWordsToNumberConverter(new LinkingAffixWordsToNumberProfile(", source);
        Assert.Contains("new VigesimalCompoundWordsToNumberConverter(new VigesimalCompoundWordsToNumberProfile(", source);
        Assert.Contains("new GreedyCompoundWordsToNumberConverter(new GreedyCompoundWordsToNumberProfile(", source);
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
    public void LocalePhraseTablesUsePerLocaleLazyHoldersAndGeneratedArrays()
    {
        var source = GetGeneratedSource("LocalePhraseTableCatalog.g.cs");

        Assert.Contains("static partial class LocalePhraseTableCatalog", source);
        Assert.Contains("static partial LocalePhraseTable? ResolveCore(string localeCode)", source);
        Assert.Contains("\"en\" => en,", source);
        Assert.Contains("new LocalizedDatePhrase?[]", source);
        Assert.Contains("new LocalizedTimeSpanPhrase?[]", source);
        Assert.Contains("new LocalizedUnitPhrase?[]", source);
    }

    [Fact]
    public void LocalePhraseTablesInlineRepresentativePhrasesAndExactOverrides()
    {
        var source = GetGeneratedSource("LocalePhraseTableCatalog.g.cs");

        Assert.Contains("new PhraseTemplate(\"two\",", source);
        Assert.Contains("через два дня", source);
        Assert.Contains("vorgestern", source);
        Assert.Contains("PhraseCountPlacement.BeforeForm", source);
        Assert.Contains("new LocalizedPhraseForms(\"days\", \"day\"", source);
        Assert.Contains("new LocalizedUnitPhrase(new LocalizedPhraseForms(\"byte\"", source);
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
wordsToNumber:
  engine: 'token-map'
  normalizationProfile: 42
  cardinalMap:
    one: '1'
  ordinalGenderVariant: 'bogus'
  negativePrefixes:
    - 'minus '
    - 5
  scaleThreshold: '1000'
""";

        var runResult = RunGenerator(
            new InMemoryAdditionalText(
                @"E:\Dev\Humanizer\src\Humanizer\Locales\zz-invalid.yml",
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

        Assert.Contains("_cache.Value;", formatterCatalog);
        Assert.Contains("static class ", formatterCatalog);
        Assert.DoesNotContain("{ get; } =", formatterCatalog);

        Assert.Contains("_cache.Value;", numberToWordsCatalog);
        Assert.Contains("static class ", numberToWordsCatalog);
        Assert.DoesNotContain("{ get; } =", numberToWordsCatalog);

        Assert.Contains("_cache.Value;", wordsToNumberCatalog);
        Assert.Contains("static class ", wordsToNumberCatalog);
        Assert.DoesNotContain("{ get; } =", wordsToNumberCatalog);

        Assert.Contains("_cache.Value;", timeOnlyCatalog);
        Assert.Contains("static class ", timeOnlyCatalog);
        Assert.DoesNotContain("{ get; } =", timeOnlyCatalog);

        Assert.Contains("_cache.Value;", dateCatalog);
        Assert.Contains("static class ", dateCatalog);
        Assert.DoesNotContain("{ get; } =", dateCatalog);

        Assert.Contains("_cache.Value;", dateOnlyCatalog);
        Assert.Contains("static class ", dateOnlyCatalog);
        Assert.DoesNotContain("{ get; } =", dateOnlyCatalog);
    }

    [Fact]
    public void NumberToWordsProfilesUseSharedEastAsianEngine()
    {
        var source = GetGeneratedSource("NumberToWordsProfileCatalog.g.cs");

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

        Assert.Contains("new ConjunctionalScaleNumberToWordsConverter(", numberSource);
        Assert.Contains("new JoinedScaleNumberToWordsConverter(", numberSource);

        Assert.Contains("registry.Register(\"ku\", culture => TokenMapWordsToNumberConverters.Ku);", wordsRegistrySource);
        Assert.Contains("registry.Register(\"mt\", culture => TokenMapWordsToNumberConverters.Mt);", wordsRegistrySource);
        Assert.DoesNotContain("case \"kurdish\":", wordsCatalogSource);
        Assert.DoesNotContain("case \"maltese\":", wordsCatalogSource);
    }

    [Fact]
    public void ExistingNumberToWordsFamiliesPreferExplicitStrategiesOverLocaleSpecificBranches()
    {
        var schemaCatalog = GetSourceGeneratorFile("Common", "EngineContractCatalog.cs");

        Assert.Contains("Schema(\"conjunctional-scale\"", schemaCatalog);
        Assert.Contains("Member(\"enum\", \"addAndMode\", null, \"ConjunctionalScaleAddAndMode\"", schemaCatalog);
        Assert.Contains("Member(\"enum\", \"andStrategy\", null, \"ConjunctionalScaleAndStrategy\"", schemaCatalog);
        Assert.Contains("Member(\"string\", \"tupleSuffix\"", schemaCatalog);
        Assert.Contains("Member(\"enum\", \"ordinalLeadingOneStrategy\", null, \"ConjunctionalScaleOrdinalLeadingOneStrategy\"", schemaCatalog);
        Assert.DoesNotContain("respectAddAndFlag", schemaCatalog);
        Assert.DoesNotContain("useAndWithinGroup", schemaCatalog);
        Assert.DoesNotContain("useAndAfterScaleForSubHundredRemainder", schemaCatalog);
        Assert.DoesNotContain("omitLeadingOneInOrdinal", schemaCatalog);

        Assert.Contains("Schema(\"variant-decade\"", schemaCatalog);
        Assert.Contains("Member(\"enum\", \"seventyStrategy\", null, \"VariantDecadeSeventyStrategy\"", schemaCatalog);
        Assert.Contains("Member(\"enum\", \"ninetyStrategy\", null, \"VariantDecadeNinetyStrategy\"", schemaCatalog);
        Assert.Contains("Member(\"string\", \"specialSeventyOneWord\"", schemaCatalog);
        Assert.Contains("Member(\"bool\", \"pluralizeExactEighty\"", schemaCatalog);
        Assert.Contains("Member(\"nullable-int-set\", \"tensUsingEtWhenUnitIsOne\"", schemaCatalog);
        Assert.Contains("Member(\"string-array\", \"tensMap\"", schemaCatalog);
        Assert.DoesNotContain("Member(\"string\", \"style\"", schemaCatalog);

        Assert.Contains("Schema(\"billion-strategy\"", schemaCatalog);
        Assert.Contains("Member(\"profile-object\", \"cardinal\"", schemaCatalog);
        Assert.Contains("Member(\"profile-object\", \"ordinal\"", schemaCatalog);
        Assert.Contains("Member(\"string\", \"millionSingularWord\"", schemaCatalog);
        Assert.Contains("Member(\"enum\", \"billionStrategy\", null, \"BillionCardinalStrategy\"", schemaCatalog);
        Assert.Contains("Member(\"string\", \"millionSeparator\"", schemaCatalog);
        Assert.DoesNotContain("PortugueseBillionCardinalStrategy", schemaCatalog);
        Assert.DoesNotContain("PortugueseBillionOrdinalStrategy", schemaCatalog);

        Assert.Contains("oneWord:", GetLocaleFile("ms.yml"));
        Assert.DoesNotContain("thousandOneWord", schemaCatalog);

        Assert.Contains("Schema(\"scale-strategy\"", schemaCatalog);
        Assert.Contains("Member(\"enum\", \"cardinalStrategy\", null, \"ScaleStrategyCardinalMode\"", schemaCatalog);
        Assert.Contains("Member(\"enum\", \"ordinalStrategy\", null, \"ScaleStrategyOrdinalMode\"", schemaCatalog);
        Assert.Contains("Member(\"string\", \"largeScaleRemainderJoiner\"", schemaCatalog);
        Assert.Contains("Member(\"string\", \"exactLargeScaleOrdinalSuffix\"", schemaCatalog);
        Assert.Contains("Member(\"string\", \"exactDefaultOrdinalSuffix\"", schemaCatalog);
        Assert.Contains("Member(\"string\", \"tensOrdinalTrimEndCharacters\"", schemaCatalog);
        Assert.Contains("Member(\"string\", \"tensOrdinalSuffix\"", schemaCatalog);
        Assert.Contains("Member(\"int32\", \"shortOrdinalUpperBoundExclusive\"", schemaCatalog);
        Assert.Contains("Member(\"string\", \"shortOrdinalTrimEndCharacters\"", schemaCatalog);
        Assert.Contains("Member(\"string\", \"shortOrdinalTrimmedSuffix\"", schemaCatalog);
        Assert.Contains("Member(\"string\", \"shortOrdinalSuffix\"", schemaCatalog);
        Assert.DoesNotContain("Member(\"string\", \"style\"", schemaCatalog);

        Assert.Contains("Schema(\"unit-leading-compound\"", schemaCatalog);
        Assert.Contains("Member(\"enum\", \"tensJoinerTransform\", null, \"CompoundTensJoinerTransform\"", schemaCatalog);
        Assert.Contains("Member(\"string\", \"ordinalStemSuffix\"", schemaCatalog);
        Assert.Contains("Member(\"string-array\", \"unitsOrdinal\"", schemaCatalog);
        Assert.DoesNotContain("Member(\"string\", \"style\"", schemaCatalog);

        Assert.Contains("Schema(\"harmony-ordinal\"", schemaCatalog);
        Assert.Contains("Member(\"bool\", \"softenTerminalTBeforeSuffix\"", schemaCatalog);
        Assert.Contains("Member(\"bool\", \"dropTerminalVowelBeforeHarmonySuffix\"", schemaCatalog);

        Assert.Contains("Schema(\"conjoined-gendered-scale\"", schemaCatalog);
        Assert.DoesNotContain("Schema(\"bulgarian\"", schemaCatalog);
        Assert.Contains("Schema(\"hyphenated-scale\"", schemaCatalog);
        Assert.DoesNotContain("Schema(\"hungarian\"", schemaCatalog);
        Assert.Contains("Schema(\"dual-form-scale\"", schemaCatalog);
        Assert.DoesNotContain("Schema(\"maltese\"", schemaCatalog);
        Assert.Contains("engine: 'conjoined-gendered-scale'", GetLocaleFile("bg.yml"));
        Assert.DoesNotContain("engine: 'bulgarian'", GetLocaleFile("bg.yml"));
        Assert.Contains("engine: 'hyphenated-scale'", GetLocaleFile("hu.yml"));
        Assert.DoesNotContain("engine: 'hungarian'", GetLocaleFile("hu.yml"));
        Assert.Contains("engine: 'dual-form-scale'", GetLocaleFile("mt.yml"));
        Assert.DoesNotContain("engine: 'maltese'", GetLocaleFile("mt.yml"));
    }

    [Fact]
    public void UzbekProfilesAreAbsorbedIntoTheSharedHarmonyOrdinalFamily()
    {
        var source = GetGeneratedSource("NumberToWordsProfileCatalog.g.cs");
        var uzbekLatinProfile = GetLocaleFile("uz-Latn-UZ.yml");
        var uzbekCyrillicProfile = GetLocaleFile("uz-Cyrl-UZ.yml");

        Assert.Contains("engine: 'harmony-ordinal'", uzbekLatinProfile);
        Assert.Contains("engine: 'harmony-ordinal'", uzbekCyrillicProfile);
        Assert.Contains("new HarmonyOrdinalNumberToWordsConverter(", source);
        Assert.DoesNotContain("new UzbekFamilyNumberToWordsConverter(", source);
    }

    [Fact]
    public void UnitLeadingCompoundScalesAcceptNamedOrdinalCases()
    {
        const string locale = """
inherits: 'en'

numberToWords:
  engine: 'unit-leading-compound'
  zeroWord: 'zero'
  minusWord: 'minus'
  masculineOne: 'one'
  feminineOne: 'one'
  neuterOne: 'one'
  tensJoiner: 'and'
  ordinalStemSuffix: 's'
  masculineOrdinalEnding: 'th'
  feminineOrdinalEnding: 'th'
  neuterOrdinalEnding: 'th'
  unitsMap:
    - 'zero'
    - 'one'
    - 'two'
  compoundUnitsMap:
    - 'zero'
    - 'one'
    - 'two'
  tensMap:
    - 'zero'
    - 'ten'
    - 'twenty'
  unitsOrdinal:
    - 'zer'
    - 'fir'
    - 'sec'
  scales:
    -
      value: 1000
      addSpaceBeforeNextPart: true
      singularCardinal: 'one thousand'
      pluralCardinalFormat: '{0} thousand'
      ordinalSingular:
        terminal: 'one-thousandth'
        continuing: 'one-thousand'
      ordinalPlural:
        terminal: '{0}-thousandth'
        continuing: '{0}-thousand'
""";

        var runResult = RunGenerator(new InMemoryAdditionalText(
            @"E:\Dev\Humanizer\src\Humanizer\Locales\zz-semantic-unit-leading.yml",
            locale));

        Assert.Empty(runResult.Diagnostics);

        var source = runResult.Results[0].GeneratedSources
            .Single(static generatedSource => generatedSource.HintName == "NumberToWordsProfileCatalog.g.cs")
            .SourceText
            .ToString();

        Assert.Contains("new string[] { \"one-thousandth\", \"one-thousand\" }", source);
        Assert.Contains("new string[] { \"{0}-thousandth\", \"{0}-thousand\" }", source);
    }

    [Fact]
    public void FormatterProfilesAcceptGrammarAliasesAndPreferThemOverLegacyFields()
    {
        const string locale = """
formatter:
  engine: 'profiled'
  resourceKeyDetector: 'arabic-like'
  dataUnitDetector: 'singular-plural'
  prepositionMode: 'none'
grammar:
  pluralRule: 'russian'
  dataUnitPluralRule: 'slovenian'
  dataUnitNonIntegralForm: 'plural'
  prepositionMode: 'romanian-de'
  secondaryPlaceholderMode: 'luxembourgish-eifeler-n'
  timeUnitGenders:
    day: 'masculine'
    minute: 'feminine'
""";

        var runResult = RunGenerator(new InMemoryAdditionalText(
            @"E:\Dev\Humanizer\src\Humanizer\Locales\zz-grammar.yml",
            locale));

        Assert.Empty(runResult.Diagnostics);

        var source = runResult.Results[0].GeneratedSources
            .Single(static generatedSource => generatedSource.HintName == "FormatterProfileCatalog.g.cs")
            .SourceText
            .ToString();

        Assert.Contains(
            "new FormatterProfile(FormatterNumberDetectorKind.Russian, Array.Empty<FormatterDateFormOverride>(), Array.Empty<FormatterTimeSpanFormOverride>(), FormatterNumberDetectorKind.Slovenian, FormatterNumberForm.Plural, FormatterDataUnitFallbackTransform.None, FormatterPrepositionMode.RomanianDe, FormatterSecondaryPlaceholderMode.LuxembourgishEifelerN",
            source);
        Assert.Contains("[TimeUnit.Day] = GrammaticalGender.Masculine", source);
        Assert.Contains("[TimeUnit.Minute] = GrammaticalGender.Feminine", source);
    }

    [Fact]
    public void ChildLocaleMappingsMergeWithParentMappingsAtGenerationTime()
    {
        const string baseLocale = """
inherits: 'en'

numberToWords:
  engine: 'conjunctional-scale'
  minusWord: 'base-minus'
  andWord: 'base-and'
  hundredWord: 'base-hundred'
  hundredOrdinalWord: 'base-hundredth'
  tensUnitsSeparator: '~'
  defaultAddAnd: true
  addAndMode: 'use-caller-flag'
  andStrategy: 'within-group-only'
  tupleSuffix: '-base-tuple'
  ordinalLeadingOneStrategy: 'omit-leading-one'
  ordinalMode: 'english'
  unitsMap:
    - 'zero'
    - 'one'
  ordinalUnitsMap:
    - 'zeroth'
    - 'first'
  tensMap:
    - 'zero'
    - 'ten'
  ordinalTensMap:
    - 'zeroth'
    - 'tenth'
  scales:
    -
      value: 1000
      name: 'base-kilo'
      ordinalName: 'base-milli'
""";

        const string childLocale = """
inherits: 'zz-base'

numberToWords:
  engine: 'conjunctional-scale'
  minusWord: 'child-minus'
  scales:
    -
      value: 1000
      name: 'child-kilo'
      ordinalName: 'child-milli'
""";

        var runResult = RunGenerator(
            new InMemoryAdditionalText(
                @"E:\Dev\Humanizer\src\Humanizer\Locales\zz-base.yml",
                baseLocale),
            new InMemoryAdditionalText(
                @"E:\Dev\Humanizer\src\Humanizer\Locales\zz-child.yml",
                childLocale));

        Assert.Empty(runResult.Diagnostics);

        var source = runResult.Results[0].GeneratedSources
            .Single(static generatedSource => generatedSource.HintName == "NumberToWordsProfileCatalog.g.cs")
            .SourceText
            .ToString();

        var baseAndCount = CountOccurrences(source, "base-and");
        var baseHundredCount = CountOccurrences(source, "base-hundred");
        var baseScaleCount = CountOccurrences(source, "base-kilo");
        var childMinusCount = CountOccurrences(source, "child-minus");
        var childScaleCount = CountOccurrences(source, "child-kilo");

        Assert.Contains("case \"zz-child\": return", source);
        Assert.True(baseAndCount > baseScaleCount, "Inherited scalar fields should appear in both the parent and child generated profiles.");
        Assert.True(baseHundredCount > baseScaleCount, "Inherited scalar fields should appear in both the parent and child generated profiles.");
        Assert.True(childMinusCount > 0);
        Assert.True(childScaleCount > 0);
    }

    [Fact]
    public void SparseNumericLexicalTablesExpandIntoGeneratedArrays()
    {
        const string locale = """
numberToWords:
  engine: 'conjunctional-scale'
  minusWord: 'minus'
  andWord: 'and'
  hundredWord: 'hundred'
  hundredOrdinalWord: 'hundredth'
  tensUnitsSeparator: '-'
  defaultAddAnd: true
  addAndMode: 'use-caller-flag'
  andStrategy: 'within-group-only'
  tupleSuffix: '-tuple'
  ordinalLeadingOneStrategy: 'omit-leading-one'
  ordinalMode: 'english'
  unitsMap:
    1: 'one'
    2: 'two'
  ordinalUnitsMap:
    1: 'first'
    2: 'second'
  tensMap:
    2: 'twenty'
    3: 'thirty'
  ordinalTensMap:
    2: 'twentieth'
    3: 'thirtieth'
  scales:
    -
      value: 1000
      name: 'thousand'
      ordinalName: 'thousandth'
""";

        var runResult = RunGenerator(
            new InMemoryAdditionalText(
                @"E:\Dev\Humanizer\src\Humanizer\Locales\zz-sparse.yml",
                locale));

        Assert.Empty(runResult.Diagnostics);

        var source = runResult.Results[0].GeneratedSources
            .Single(static generatedSource => generatedSource.HintName == "NumberToWordsProfileCatalog.g.cs")
            .SourceText
            .ToString();

        Assert.Contains("new string[] { \"\", \"one\", \"two\" }", source);
        Assert.Contains("new string[] { \"\", \"\", \"twenty\", \"thirty\" }", source);
        Assert.Contains("new string[] { \"\", \"first\", \"second\" }", source);
        Assert.Contains("new string[] { \"\", \"\", \"twentieth\", \"thirtieth\" }", source);
    }

    static string GetGeneratedSource(string hintName) =>
        generatedSources.Value.TryGetValue(hintName, out var source)
            ? source
            : throw new Xunit.Sdk.XunitException($"Generated source '{hintName}' was not found.");

    static int CountOccurrences(string source, string value)
    {
        var count = 0;
        var index = 0;
        while ((index = source.IndexOf(value, index, StringComparison.Ordinal)) >= 0)
        {
            count++;
            index += value.Length;
        }

        return count;
    }

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
        var filePaths = new List<string>();

        AddFiles(filePaths, Path.Combine(sourceRoot, "Locales"), "*.yml");

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

    static string GetLocaleFile(string fileName) =>
        File.ReadAllText(Path.Combine(FindRepositoryRoot(), "src", "Humanizer", "Locales", fileName));

    static string GetSourceGeneratorFile(params string[] relativeSegments)
    {
        var pathSegments = new[] { FindRepositoryRoot(), "src", "Humanizer.SourceGenerators" }
            .Concat(relativeSegments)
            .ToArray();
        return File.ReadAllText(Path.Combine(pathSegments));
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

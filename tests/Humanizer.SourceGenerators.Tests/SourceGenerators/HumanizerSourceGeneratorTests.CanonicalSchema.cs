using System.Buffers.Binary;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using Xunit;

namespace Humanizer.SourceGenerators.Tests;

public partial class HumanizerSourceGeneratorTests
{
    static readonly string[] InflectionRegistryTrackingNames =
        ["InflectionRegistrySource", "LocaleRegistrySource"];

    [Fact]
    public void CanonicalLocaleSchemaAcceptsLocaleVariantOfAndSurfaces()
    {
        const string parentLocale = """
locale: 'zz-parent'
surfaces:
  durationCases:
    classification: 'not-applicable'
  list:
    engine: 'conjunction'
    pairTemplate: '{0} and {1}'
    finalTemplate: '{0} and {1}'
    serialTemplate: '{0}, {1}'
    oxfordComma: false
    cliticizesFinal: false
  phrases:
    relativeDate:
      now: 'now'
      never: 'never'
  number:
    parse:
      engine: 'token-map'
      normalizationProfile: 'LowercaseRemovePeriods'
      cardinalMap:
        one: 1
        huge: 2147483648
      ordinalMap:
        first: 1
""";

        const string childLocale = """
locale: 'zz-child'
variantOf: 'zz-parent'
surfaces:
  number:
    words:
      engine: 'variant-decade'
      minusWord: 'minus'
      seventyStrategy: 'regular'
      ninetyStrategy: 'regular'
      pluralizeExactEighty: false
      tensUsingEtWhenUnitIsOne:
        - 2
      tensMap:
        - 'zero'
        - 'ten'
        - 'twenty'
        - 'thirty'
        - 'forty'
        - 'fifty'
        - 'sixty'
        - 'seventy'
        - 'eighty'
        - 'ninety'
      scales:
        -
          value: 1000
          singular: 'thousand'
          plural: 'thousand'
          omitOne: true
""";

        var runResult = RunGenerator(
            new InMemoryAdditionalText(
                @"E:\Dev\Humanizer\src\Humanizer\Locales\zz-parent.yml",
                parentLocale),
            new InMemoryAdditionalText(
                @"E:\Dev\Humanizer\src\Humanizer\Locales\zz-child.yml",
                childLocale));

        Assert.Empty(runResult.Diagnostics);

        var registrySource = runResult.Results[0].GeneratedSources
            .Single(source => source.HintName == "CollectionFormatterRegistryRegistrations.g.cs")
            .SourceText
            .ToString();
        var phraseTableSource = runResult.Results[0].GeneratedSources
            .Single(source => source.HintName == "LocalePhraseTableCatalog.g.cs")
            .SourceText
            .ToString();
        var numberToWordsSource = runResult.Results[0].GeneratedSources
            .Single(source => source.HintName == "NumberToWordsProfileCatalog.g.cs")
            .SourceText
            .ToString();
        var wordsToNumberSource = runResult.Results[0].GeneratedSources
            .Single(source => source.HintName == "WordsToNumberProfileCatalog.g.cs")
            .SourceText
            .ToString();
        var parentTokenMapSource = runResult.Results[0].GeneratedSources
            .Single(source => source.HintName == "TokenMapWordsToNumberConverters.ZzParent.g.cs")
            .SourceText
            .ToString();
        var childTokenMapSource = runResult.Results[0].GeneratedSources
            .Single(source => source.HintName == "TokenMapWordsToNumberConverters.ZzChild.g.cs")
            .SourceText
            .ToString();

        Assert.Contains("registry.Register(\"zz-parent\"", registrySource, StringComparison.Ordinal);
        Assert.Contains("[\"zz-parent\"] = static () => zz_parent", phraseTableSource, StringComparison.Ordinal);
        Assert.Contains("case \"zz-child\": return", numberToWordsSource, StringComparison.Ordinal);
        Assert.Contains("new Dictionary<string, long>(StringComparer.Ordinal)", wordsToNumberSource, StringComparison.Ordinal);
        Assert.Contains("[\"huge\"] = 2147483648", parentTokenMapSource, StringComparison.Ordinal);
        Assert.Contains("[\"huge\"] = 2147483648", childTokenMapSource, StringComparison.Ordinal);
    }


    [Fact]
    public void CanonicalLocaleSchemaEmitsScaleLeadingCompoundNumberProfiles()
    {
        const string locale = """
locale: 'zz-scale'
surfaces:
  durationCases:
    classification: 'not-applicable'
  number:
    words:
      engine: 'scale-leading-compound'
      zeroWord: 'zero'
      minusWord: 'minus'
      conjunctionWord: 'and'
      ordinalPrefix: 'ord '
      unitsMap:
        - 'zero'
        - 'one'
        - 'two'
        - 'three'
        - 'four'
        - 'five'
        - 'six'
        - 'seven'
        - 'eight'
        - 'nine'
        - 'ten'
        - 'eleven'
        - 'twelve'
        - 'thirteen'
        - 'fourteen'
        - 'fifteen'
        - 'sixteen'
        - 'seventeen'
        - 'eighteen'
        - 'nineteen'
      tensMap:
        2: 'twenty'
        3: 'thirty'
        4: 'forty'
        5: 'fifty'
        6: 'sixty'
        7: 'seventy'
        8: 'eighty'
        9: 'ninety'
      scales:
        -
          value: 1000
          name: 'thousand'
        -
          value: 100
          name: 'hundred'
      ordinalMap:
        1: 'first'
    parse:
      engine: 'scale-leading-compound'
      minusWord: 'minus'
      conjunctionWord: 'and'
      ordinalPrefix: 'ord '
      unitsMap:
        zero: 0
        one: 1
        two: 2
        ten: 10
      tensMap:
        twenty: 20
        thirty: 30
      scales:
        -
          value: 1000
          name: 'thousand'
        -
          value: 100
          name: 'hundred'
      ordinalMap:
        first: 1
""";

        var runResult = RunGenerator(new InMemoryAdditionalText(
            "src/Humanizer/Locales/zz-scale.yml",
            locale));

        Assert.Empty(runResult.Diagnostics);

        var numberToWordsSource = runResult.Results[0].GeneratedSources
            .Single(source => source.HintName == "NumberToWordsProfileCatalog.g.cs")
            .SourceText
            .ToString();
        var wordsToNumberSource = runResult.Results[0].GeneratedSources
            .Single(source => source.HintName == "WordsToNumberProfileCatalog.g.cs")
            .SourceText
            .ToString();

        Assert.Contains("ScaleLeadingCompoundNumberToWordsConverter", numberToWordsSource, StringComparison.Ordinal);
        Assert.Contains("ScaleLeadingCompoundScale", numberToWordsSource, StringComparison.Ordinal);
        Assert.Contains("new string[] { \"\", \"\", \"twenty\", \"thirty\"", numberToWordsSource, StringComparison.Ordinal);
        Assert.Contains("ScaleLeadingCompoundWordsToNumberConverter", wordsToNumberSource, StringComparison.Ordinal);
        Assert.Contains("[\"twenty\"] = 20", wordsToNumberSource, StringComparison.Ordinal);
    }

    [Fact]
    public void CanonicalLocaleSchemaRejectsOldTopLevelFeatureBlocks()
    {
        const string oldSchemaLocale = """
collectionFormatter: 'oxford'
phrases:
  dateHumanize:
    now: 'now'
""";

        var runResult = RunGenerator(new InMemoryAdditionalText(
            @"E:\Dev\Humanizer\src\Humanizer\Locales\zz-old.yml",
            oldSchemaLocale,
            canonicalizeLegacySchema: false));

        var messages = runResult.Diagnostics
            .Where(diagnostic => diagnostic.Id == "HSG003")
            .Select(diagnostic => diagnostic.GetMessage())
            .ToArray();

        Assert.Contains(messages, message => message.Contains("Supported properties: locale, variantOf, surfaces", StringComparison.Ordinal));
    }

    [Fact]
    public void CanonicalLocaleSchemaReportsMissingVariantParent()
    {
        const string locale = """
locale: 'zz-child'
variantOf: 'zz-missing'
surfaces:
  formatter:
    engine: 'profiled'
    pluralRule: 'default'
""";

        var runResult = RunGenerator(new InMemoryAdditionalText(
            @"E:\Dev\Humanizer\src\Humanizer\Locales\zz-child.yml",
            locale));

        var messages = runResult.Diagnostics
            .Where(diagnostic => diagnostic.Id == "HSG003")
            .Select(diagnostic => diagnostic.GetMessage())
            .ToArray();

        Assert.Contains(messages, message => message.Contains("Inherited locale 'zz-missing' is not defined.", StringComparison.Ordinal));
    }

    [Fact]
    public void CanonicalLocaleSchemaRequiresLocaleToMatchFileName()
    {
        const string locale = """
locale: 'zz-other'
surfaces:
  number:
    parse:
      engine: 'token-map'
      cardinalMap:
        one: 1
""";

        var runResult = RunGenerator(new InMemoryAdditionalText(
            @"E:\Dev\Humanizer\src\Humanizer\Locales\zz-file.yml",
            locale));

        var messages = runResult.Diagnostics
            .Where(diagnostic => diagnostic.Id == "HSG003")
            .Select(diagnostic => diagnostic.GetMessage())
            .ToArray();

        Assert.Contains(messages, message => message.Contains("must match file locale 'zz-file'", StringComparison.Ordinal));
    }

    [Fact]
    public void InflectionProfilesInheritWithinTheSameLanguage()
    {
        const string parentLocale = """
locale: 'aa'
surfaces:
  durationCases:
    classification: 'not-applicable'
  inflection:
    cardinalRule: 'EnglishLike'
""";
        const string regionalLocale = """
locale: 'aa-ZZ'
variantOf: 'aa'
""";
        var inheritedRun = RunGenerator(
            new InMemoryAdditionalText("src/Humanizer/Locales/aa.yml", parentLocale),
            new InMemoryAdditionalText("src/Humanizer/Locales/aa-ZZ.yml", regionalLocale));

        Assert.Empty(inheritedRun.Diagnostics);
        var source = GetGeneratedSource(inheritedRun, "LocalizedInflectionCatalog.g.cs");
        Assert.Contains("[\"aa\"]", source, StringComparison.Ordinal);
        Assert.Contains("[\"aa-ZZ\"]", source, StringComparison.Ordinal);
    }

    [Fact]
    public void SyntheticInflectionSupportsCrLfLocaleText()
    {
        const string locale = """
locale: 'zz'
surfaces:
  durationCases:
    classification: 'not-applicable'
  list:
    engine: 'conjunction'
    value: 'and'
""";

        var runResult = RunGenerator(
            new InMemoryAdditionalText(
                "src/Humanizer/Locales/zz.yml",
                locale.Replace("\n", "\r\n", StringComparison.Ordinal)));

        Assert.Empty(runResult.Diagnostics);
        var source = GetGeneratedSource(runResult, "LocalizedInflectionCatalog.g.cs");
        Assert.Contains("[\"zz\"]", source, StringComparison.Ordinal);
    }

    [Fact]
    public void DurationCasesRequireEveryRootToClassifyTheSurface()
    {
        const string locale = """
locale: 'zz'
surfaces:
  inflection:
    cardinalRule: 'Other'
""";

        var runResult = RunGenerator(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Contains(
            runResult.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains(
                    "must classify the durationCases surface",
                    StringComparison.Ordinal));
        Assert.DoesNotContain(
            runResult.GeneratedTrees,
            static tree => tree.FilePath.EndsWith(
                "LocaleDurationCaseTableCatalog.g.cs",
                StringComparison.Ordinal));
    }

    [Fact]
    public void DurationCaseTablesCarryTheEffectiveFormatterNumberDetector()
    {
        var source = GetGeneratedSource("LocaleDurationCaseTableCatalog.g.cs");
        var hrStart = source.IndexOf("static class Locale_hr_cache", StringComparison.Ordinal);
        var huStart = source.IndexOf("static LocaleDurationCaseTable Locale_hu", hrStart, StringComparison.Ordinal);

        Assert.True(hrStart >= 0);
        Assert.True(huStart > hrStart);
        Assert.Contains(
            "FormatterNumberDetectorKind.SouthSlavic",
            source[hrStart..huStart],
            StringComparison.Ordinal);
    }

    [Fact]
    public void DurationCaseTablesPreserveRequiredNamedFormsMatchingDefault()
    {
        var source = GetGeneratedSource("LocaleDurationCaseTableCatalog.g.cs");
        var bsStart = source.IndexOf("static class Locale_bs_cache", StringComparison.Ordinal);
        var caStart = source.IndexOf("static LocaleDurationCaseTable Locale_ca", bsStart, StringComparison.Ordinal);

        Assert.True(bsStart >= 0);
        Assert.True(caStart > bsStart);
        Assert.Contains(
            "new LocalizedPhraseForms(\"dana\", null, \"dan\", null, \"dana\"",
            source[bsStart..caStart],
            StringComparison.Ordinal);
    }

    [Fact]
    public void InflectionProfilesRequireEveryRootOnceTheFeatureIsEnabled()
    {
        const string enabledLocale = """
locale: 'aa'
surfaces:
  durationCases:
    classification: 'not-applicable'
  inflection:
    cardinalRule: 'EnglishLike'
""";
        const string missingLocale = """
locale: 'zz'
surfaces:
  durationCases:
    classification: 'not-applicable'
  list:
    engine: 'conjunction'
    value: 'and'
""";

        var runResult = RunGenerator(
            new InMemoryAdditionalText("src/Humanizer/Locales/aa.yml", enabledLocale),
            new InMemoryAdditionalText(
                "src/Humanizer/Locales/zz.yml",
                missingLocale,
                addDefaultInflection: false));

        Assert.Contains(
            runResult.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains(
                    "Every root or cross-language locale profile must author surfaces.inflection explicitly.",
                    StringComparison.Ordinal));
    }

    [Fact]
    public void InflectionProfilesEmitEuropeanPortugueseCardinalOverride()
    {
        var runResult = RunGenerator();

        Assert.Empty(runResult.Diagnostics);
        var source = GetGeneratedSource(runResult, "LocalizedInflectionCatalog.g.cs");
        Assert.Contains("[\"pt-PT\"]", source, StringComparison.Ordinal);
        Assert.Contains(
            "[\"pt-PT\"] = CardinalPluralRuleKind.CatalanItalian",
            source,
            StringComparison.Ordinal);
    }

    [Fact]
    public void InflectionProfilesRejectRemovedLexiconProperties()
    {
        const string locale = """
locale: 'zz'
surfaces:
  durationCases:
    classification: 'not-applicable'
  inflection:
    cardinalRule: 'EnglishLike'
    disposition: 'selector-only'
""";

        var runResult = RunGenerator(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Contains(
            runResult.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains("unsupported property 'disposition'", StringComparison.Ordinal));
    }

    [Fact]
    public void AcceptedCultureResolverSeparatesLocaleAndInflectionOwnersWithoutRuntimeParentWalk()
    {
        var runResult = RunGenerator();

        Assert.Empty(runResult.Diagnostics);
        var resolver = GetGeneratedSource(runResult, "GeneratedCultureResolver.g.cs");
        var supportedCultures = GetGeneratedSource(runResult, "Configurator.SupportedCultures.g.cs");
        var acceptedNames = resolver
            .Split('\n')
            .Select(static line => line.Trim())
            .Where(static line => line.StartsWith("new(\"", StringComparison.Ordinal))
            .Select(static line => line.Split('"')[1])
            .ToArray();

        Assert.Contains(
            "new(\"fr-FR\", \"fr\", null, null)",
            resolver,
            StringComparison.Ordinal);
        Assert.Contains(
            "new(\"pt-PT\", \"pt\", null, null)",
            resolver,
            StringComparison.Ordinal);
        Assert.Contains(
            "new(\"ca-ES-valencia\", \"ca\", null, null)",
            resolver,
            StringComparison.Ordinal);
        var zhHansIndex = resolver.IndexOf(
            "new(\"zh-Hans\", \"zh-Hans\", null, null)",
            StringComparison.Ordinal);
        var zhHkIndex = resolver.IndexOf(
            "new(\"zh-HK\", \"zh-Hant\", null, null)",
            StringComparison.Ordinal);
        Assert.True(zhHansIndex >= 0 && zhHansIndex < zhHkIndex);
        Assert.Equal(
            acceptedNames.OrderBy(
                static name => name,
                StringComparer.OrdinalIgnoreCase),
            acceptedNames);
        Assert.Contains("LocaleProfileOwner", resolver, StringComparison.Ordinal);
        Assert.Contains("InflectionOwner", resolver, StringComparison.Ordinal);
        Assert.DoesNotContain("CultureInfo.Parent", resolver, StringComparison.Ordinal);
        Assert.DoesNotContain("current.Parent", supportedCultures, StringComparison.Ordinal);
    }

    [Fact]
    public void InertInflectionBundleMayBeStructurallyIncomplete()
    {
        const string locale = """
locale: 'zz'
surfaces:
  durationCases:
    classification: 'not-applicable'
  inflection:
    cardinalRule: 'EnglishLike'
    capability: 'inert'
    accepted-cultures:
      - 'zz'
      - 'zz-ZZ'
""";

        var runResult = RunGenerator(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.DoesNotContain(
            runResult.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains("inflection", StringComparison.OrdinalIgnoreCase));
        var resolver = GetGeneratedSource(runResult, "GeneratedCultureResolver.g.cs");
        Assert.Contains("new(\"zz-ZZ\", \"zz\", null, null)", resolver, StringComparison.Ordinal);
        Assert.DoesNotContain(
            runResult.Results[0].GeneratedSources,
            static source => source.HintName == "GeneratedInflection_zz.g.cs");
    }

    [Fact]
    public void ActiveInflectionBundleMustBeComplete()
    {
        const string locale = """
locale: 'zz'
surfaces:
  durationCases:
    classification: 'not-applicable'
  inflection:
    cardinalRule: 'EnglishLike'
    capability: 'display-by-category'
    accepted-cultures:
      - 'zz'
""";

        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Contains(
            runResult.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains(
                    "active inflection bundle must define scripts",
                    StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void ActiveInflectionBundleEmitsOneLazyOwnerTable()
    {
        const string locale = """
locale: 'zz'
surfaces:
  durationCases:
    classification: 'not-applicable'
  inflection:
    cardinalRule: 'EnglishLike'
    capability: 'display-by-category'
    accepted-cultures:
      - 'zz'
      - 'zz-Latn-ZZ'
    scripts:
      - 'Latn'
    casing: 'lower-title-upper'
    phrase-mode: 'exact-only'
    skip-simple-words:
      - 'news'
    lexemes:
      - id: 'zz.noun.cat'
        pos: 'noun'
        countability: 'count'
        forms:
          singular:
            preferred: 'cat'
            accepted:
              - 'cat'
          dictionary-plural:
            preferred: 'cats'
            accepted:
              - 'cats'
          display:
            one:
              preferred: 'cat'
              accepted:
                - 'cat'
            other:
              preferred: 'cats'
              accepted:
                - 'cats'
        sources:
          - 'fixture'
      - id: 'zz.noun.feline'
        pos: 'noun'
        countability: 'count'
        forms:
          singular:
            preferred: 'feline'
            accepted:
              - 'feline'
          dictionary-plural:
            preferred: 'cats'
            accepted:
              - 'cats'
          display:
            one:
              preferred: 'feline'
              accepted:
                - 'feline'
            other:
              preferred: 'cats'
              accepted:
                - 'cats'
        sources:
          - 'fixture'
    rules:
      - id: 'zz.forward.consonant-y'
        direction: 'forward'
        priority: 100
        scope:
          pos: 'noun'
          countability:
            - 'count'
          token: 'standalone'
          scripts:
            - 'Latn'
        match:
          suffix: 'y'
          preceding-not:
            - 'a'
            - 'e'
            - 'i'
            - 'o'
            - 'u'
        output:
          dictionary-plural: '{stem}ies'
          display:
            one: '{stem}y'
            other: '{stem}ies'
        hostile-exclusions:
          surfaces:
            - 'day'
        reverse:
          enabled: true
          requires-existing-lexeme: false
        sources:
          - 'fixture'
    sources:
      fixture:
        kind: 'project-history'
        locator: 'fixture'
    evidence:
      methodology: 'fixture-v1'
      pluralize:
        eligible: 100
        irregular: 100
        covered: 95
        attempted: 100
        correct: 99
        sources:
          - 'fixture'
      singularize:
        eligible: 100
        irregular: 100
        covered: 95
        attempted: 100
        correct: 99
        sources:
          - 'fixture'
""";

        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Empty(runResult.Diagnostics);
        var resolver = GetGeneratedSource(runResult, "GeneratedCultureResolver.g.cs");
        var owner = GetGeneratedSource(runResult, "GeneratedInflection_zz.g.cs");

        Assert.Contains("new(\"zz-Latn-ZZ\", \"zz\", \"zz\", null)", resolver, StringComparison.Ordinal);
        Assert.Contains("static class GeneratedInflection_zz", owner, StringComparison.Ordinal);
        Assert.Contains("static class Holder", owner, StringComparison.Ordinal);
        Assert.Contains("\"zz.noun.cat\"", owner, StringComparison.Ordinal);
        Assert.Contains("\"cats\"", owner, StringComparison.Ordinal);
        Assert.Contains("\"zz.forward.consonant-y\"", owner, StringComparison.Ordinal);
        Assert.Contains("\"{stem}ies\"", owner, StringComparison.Ordinal);
        Assert.Contains("InflectionLexemeEntry", owner, StringComparison.Ordinal);
        Assert.Contains("new ushort[]", owner, StringComparison.Ordinal);
        Assert.DoesNotContain("InflectionExactEntry", owner, StringComparison.Ordinal);
        Assert.DoesNotContain("accepted-singular", owner, StringComparison.Ordinal);
        Assert.DoesNotContain("acceptedSingular", owner, StringComparison.Ordinal);
        Assert.Equal(1, CountOccurrences(owner, "\"cat\""));
        Assert.Equal(1, CountOccurrences(owner, "\"cats\""));
    }

    [Fact]
    public void SyntheticOwnerAboveUShortEntryLimitUsesWideIndexes()
    {
        var syntheticEntries = Enumerable.Range(0, ushort.MaxValue + 1).ToArray();

        Assert.False(HumanizerSourceGenerator.RequiresWideInflectionIndexes(ushort.MaxValue));
        Assert.True(HumanizerSourceGenerator.RequiresWideInflectionIndexes(syntheticEntries.Length));
    }

    [Fact]
    public void LexemeSenseIsRepresentedInTheGeneratedOwner()
    {
        var locale = CompleteInflectionFixture(eligible: 100, irregular: 100, covered: 95)
            .Replace(
                "        countability: 'count'",
                "        countability: 'count'\n        sense: 'plant'",
                StringComparison.Ordinal);

        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Empty(runResult.Diagnostics);
        Assert.Contains(
            "// sense: plant",
            GetGeneratedSource(runResult, "GeneratedInflection_zz.g.cs"),
            StringComparison.Ordinal);
    }

    [Fact]
    public void LexemeSenseMustBeANonEmptyScalarWhenPresent()
    {
        var locale = CompleteInflectionFixture(eligible: 100, irregular: 100, covered: 95)
            .Replace(
                "        countability: 'count'",
                "        countability: 'count'\n        sense:\n          reviewed: 'plant'",
                StringComparison.Ordinal);

        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Contains(
            runResult.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains(
                    "sense must be a non-empty scalar",
                    StringComparison.Ordinal));
    }

    [Fact]
    public void HostileLexemeExclusionsMustReferenceKnownLexemes()
    {
        var locale = ProductiveInflectionFixture().Replace(
            """
        hostile-exclusions:
          surfaces:
            - 'day'
""",
            """
        hostile-exclusions:
          lexemes:
            - 'zz.noun.missing'
""",
            StringComparison.Ordinal);

        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Contains(
            runResult.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains(
                    "references unknown excluded lexeme 'zz.noun.missing'",
                    StringComparison.Ordinal));
    }

    [Fact]
    public void HostileLexemeExclusionsAreEmittedAsEligibilityIndexes()
    {
        var locale = ProductiveInflectionFixture().Replace(
            """
        hostile-exclusions:
          surfaces:
            - 'day'
""",
            """
        hostile-exclusions:
          lexemes:
            - 'zz.noun.cat'
          surfaces:
            - 'day'
""",
            StringComparison.Ordinal);

        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Empty(runResult.Diagnostics);
        var owner = GetGeneratedSource(runResult, "GeneratedInflection_zz.g.cs");
        Assert.Contains("// excluded lexemes: zz.noun.cat", owner, StringComparison.Ordinal);
        Assert.Contains("new ushort[]", owner, StringComparison.Ordinal);
    }

    [Fact]
    public void StaticallyIdenticalRuleRanksAndAffixesAreRejected()
    {
        var locale = AddSecondProductiveRule(
            ProductiveInflectionFixture(),
            suffix: "y",
            dictionaryPlural: "{stem}ys");

        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Contains(
            runResult.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains(
                    "unresolved direction/priority/suffix tie for 'y'",
                    StringComparison.Ordinal));
    }

    [Fact]
    public void NonOverlappingEqualLengthRulesRemainValidSchema()
    {
        var locale = AddSecondProductiveRule(
            ProductiveInflectionFixture(),
            suffix: "x",
            dictionaryPlural: "{stem}xes");

        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Empty(runResult.Diagnostics);
        Assert.Contains(
            "\"zz.forward.second\"",
            GetGeneratedSource(runResult, "GeneratedInflection_zz.g.cs"),
            StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(
        "        countability: 'count'",
        "        countability: 'count'\n        review: 'ignored'")]
    [InlineData(
        "          singular:\n            preferred: 'cat'",
        "          singular:\n            review: 'ignored'\n            preferred: 'cat'")]
    [InlineData(
        "      fixture:\n        kind: 'project-history'",
        "      fixture:\n        review: 'ignored'\n        kind: 'project-history'")]
    [InlineData(
        "      methodology: 'fixture-v1'",
        "      methodology: 'fixture-v1'\n      review: 'ignored'")]
    public void NestedInflectionMappingsRejectUnknownProperties(
        string original,
        string replacement)
    {
        var locale = CompleteInflectionFixture(eligible: 100, irregular: 100, covered: 95)
            .Replace(original, replacement, StringComparison.Ordinal);

        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Contains(
            runResult.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains(
                    "unsupported property 'review'",
                    StringComparison.Ordinal));
    }

    [Theory]
    [InlineData(
        "        direction: 'forward'",
        "        direction: 'forward'\n        review: 'ignored'")]
    [InlineData(
        "        match:\n          suffix: 'y'",
        "        match:\n          suffix: 'y'\n          review: 'ignored'")]
    [InlineData(
        "        hostile-exclusions:\n          surfaces:",
        "        hostile-exclusions:\n          review: 'ignored'\n          surfaces:")]
    [InlineData(
        "        reverse:\n          enabled: true",
        "        reverse:\n          review: 'ignored'\n          enabled: true")]
    public void NestedProductiveRuleMappingsRejectUnknownProperties(
        string original,
        string replacement)
    {
        var locale = ProductiveInflectionFixture().Replace(
            original,
            replacement,
            StringComparison.Ordinal);

        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Contains(
            runResult.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains(
                    "unsupported property 'review'",
                    StringComparison.Ordinal));
    }

    [Fact]
    public void ReverseRuleMetadataMustBeAMapping()
    {
        var locale = ProductiveInflectionFixture().Replace(
            """
        reverse:
          enabled: true
          requires-existing-lexeme: false
""",
            "        reverse: 'ignored'\n",
            StringComparison.Ordinal);

        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Contains(
            runResult.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains(
                    "surfaces.inflection.reverse must be a mapping",
                    StringComparison.Ordinal));
    }

    [Theory]
    [InlineData("enabled: true", "enabled: maybe")]
    [InlineData("enabled: true", "enabled:\n            reviewed: true")]
    [InlineData("requires-existing-lexeme: false", "requires-existing-lexeme: maybe")]
    [InlineData(
        "requires-existing-lexeme: false",
        "requires-existing-lexeme:\n            - false")]
    public void ReverseRuleBooleanMetadataMustBeBoolean(
        string original,
        string replacement)
    {
        var locale = ProductiveInflectionFixture().Replace(
            original,
            replacement,
            StringComparison.Ordinal);

        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Contains(
            runResult.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains(
                    "must be 'true' or 'false'",
                    StringComparison.Ordinal));
    }

    [Theory]
    [InlineData(
        "    phrase-mode: 'exact-only'",
        "    phraseMode: 'exact-only'\n    phrase-mode: 'bogus'")]
    [InlineData(
        "    phrase-mode: 'exact-only'",
        "    skipSimpleWords:\n      - 'news'\n    skip-simple-words:\n      - 'politics'\n    phrase-mode: 'exact-only'")]
    [InlineData(
        "          dictionary-plural: '{stem}ies'",
        "          dictionaryPlural: '{stem}ys'\n          dictionary-plural: '{stem}ies'")]
    [InlineData(
        "          preceding-not:",
        "          precedingNot:\n            - 'z'\n          preceding-not:")]
    [InlineData(
        "        hostile-exclusions:",
        "        hostileExclusions:\n          surfaces:\n            - 'other'\n        hostile-exclusions:")]
    [InlineData(
        "          requires-existing-lexeme: false",
        "          requiresExistingLexeme: true\n          requires-existing-lexeme: false")]
    public void ConflictingInflectionFieldAliasesAreRejected(
        string original,
        string replacement)
    {
        var locale = ProductiveInflectionFixture().Replace(
            original,
            replacement,
            StringComparison.Ordinal);

        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Contains(
            runResult.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains(
                    "conflicting aliases",
                    StringComparison.Ordinal));
    }

    [Fact]
    public void ConflictingAcceptedCultureAliasesAreRejected()
    {
        var locale = CompleteInflectionFixtureFor("zz").Replace(
            "    accepted-cultures:",
            "    acceptedCultures:\n      - 'zz-ZZ'\n    accepted-cultures:",
            StringComparison.Ordinal);

        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Contains(
            runResult.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains(
                    "conflicting aliases",
                    StringComparison.Ordinal));
    }

    [Theory]
    [InlineData("zz")]
    [InlineData("ZZ")]
    public void DuplicateAcceptedCulturesAreRejected(string duplicateName)
    {
        var locale = CompleteInflectionFixtureFor("zz").Replace(
            "      - 'zz'",
            $"      - 'zz'\n      - '{duplicateName}'",
            StringComparison.Ordinal);

        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Contains(
            runResult.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains(
                    "accepted culture 'zz' is listed more than once",
                    StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void ExactIndexesGroupAndSortWithTheRuntimeComparer()
    {
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText(
                "src/Humanizer/Locales/zz.yml",
                ComparerEquivalentGreekInflectionFixture()));

        Assert.Empty(runResult.Diagnostics);
        var owner = GetGeneratedSource(runResult, "GeneratedInflection_zz.g.cs");
        Assert.Equal(
            1,
            CountOccurrences(owner, "new InflectionLexemeEntry(\"σ\""));
        Assert.Contains(
            "new InflectionLexemeCandidate(0, (InflectionExactRole)",
            owner,
            StringComparison.Ordinal);
        Assert.Contains(
            "new InflectionLexemeCandidate(1, (InflectionExactRole)",
            owner,
            StringComparison.Ordinal);
    }

    [Fact]
    public void GeneratedUnicodeExactIndexesSearchWithRuntimeComparer()
    {
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText(
                "src/Humanizer/Locales/zz.yml",
                UnicodeComparerInflectionFixture()));
        Assert.Empty(runResult.Diagnostics);

        const string harness = """
namespace Humanizer;

public static class CompiledUnicodeComparerHarness
{
    public static string Run(string input, bool reverse)
    {
        var result = GeneratedInflection_zz.Bundle.Inflect(
            input,
            reverse ? InflectionDirection.Reverse : InflectionDirection.Forward,
            allowProductive: false,
            category: null);
        return result.Status.ToString();
    }
}
""";
        var parseOptions = new CSharpParseOptions(LanguageVersion.Preview);
        var cancellationToken = TestContext.Current.CancellationToken;
        var compilation = CSharpCompilation.Create(
            "Humanizer.Tests",
            [
                CSharpSyntaxTree.ParseText(
                    GetGeneratedSource(runResult, "GeneratedInflection_zz.g.cs"),
                    parseOptions,
                    cancellationToken: cancellationToken),
                CSharpSyntaxTree.ParseText(
                    harness,
                    parseOptions,
                    cancellationToken: cancellationToken)
            ],
            GetMetadataReferences(),
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithCryptoKeyFile(
                    Path.Combine(FindRepositoryRoot(), "Humanizer.snk"))
                .WithPublicSign(true));
        using var assemblyStream = new MemoryStream();
        var emitResult = compilation.Emit(
            assemblyStream,
            cancellationToken: cancellationToken);
        Assert.True(
            emitResult.Success,
            string.Join(Environment.NewLine, emitResult.Diagnostics));

        var assembly = Assembly.Load(assemblyStream.ToArray());
        var run = assembly
            .GetType("Humanizer.CompiledUnicodeComparerHarness")?
            .GetMethod("Run");
        Assert.NotNull(run);
        var queries = new[]
        {
            ("\uFF5A", false),
            ("\uFF5A", true),
            ("\U00010780", false),
            ("\U00010780", true),
            ("\u00DF", false),
            ("\u00DF", true),
            ("\u1E9E", false),
            ("\u1E9E", true),
            ("\u03B8", false),
            ("\u03B8", true),
            ("\u03F4", false),
            ("\u03F4", true)
        };
        var actual = queries
            .Select(query => run!.Invoke(null, [query.Item1, query.Item2]))
            .ToArray();

        Assert.Equal(
            [
                "Exact", "Invariant",
                "Exact", "Invariant",
                "Ambiguous", "Ambiguous",
                "Ambiguous", "Ambiguous",
                "Ambiguous", "Ambiguous",
                "Ambiguous", "Ambiguous"
            ],
            actual);
    }

    [Fact]
    public void Unicode16SimpleCaseComparersMatchForEveryCodePoint()
    {
        var runtimeBundle = typeof(Humanizer.Configurator).Assembly
            .GetType("Humanizer.InflectionBundle");
        Assert.NotNull(runtimeBundle);
        var runtimeCompare = runtimeBundle!
            .GetMethod(
                "CompareInflectionKeys",
                BindingFlags.NonPublic | BindingFlags.Static)?
            .CreateDelegate<Func<string, string, int>>();
        var runtimeFold = runtimeBundle
            .GetMethod(
                "FoldInflectionScalar",
                BindingFlags.NonPublic | BindingFlags.Static)?
            .CreateDelegate<Func<int, int>>();
        Assert.NotNull(runtimeCompare);
        Assert.NotNull(runtimeFold);

        using var foldHash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
        var hashBuffer = new byte[sizeof(int)];
        string? previous = null;
        var mappedCount = 0;
        for (var scalar = 0; scalar < 0x110000; scalar++)
        {
            var generatorFold = HumanizerSourceGenerator.FoldInflectionScalar(scalar);
            var runtimeFolded = runtimeFold!(scalar);
            if (generatorFold != runtimeFolded)
            {
                throw new Xunit.Sdk.XunitException(
                    $"Fold mismatch at U+{scalar:X}: generator U+{generatorFold:X}, runtime U+{runtimeFolded:X}.");
            }

            mappedCount += generatorFold == scalar ? 0 : 1;
            BinaryPrimitives.WriteInt32LittleEndian(hashBuffer, generatorFold);
            foldHash.AppendData(hashBuffer);

            var current = UnicodeCodePointString(scalar);
            var folded = generatorFold == scalar
                ? current
                : UnicodeCodePointString(generatorFold);
            AssertComparerParity(current, folded, expected: 0);
            AssertComparerParity(folded, current, expected: 0);
            if (previous is not null)
            {
                AssertComparerParity(previous, current, expected: null);
                AssertComparerParity(current, previous, expected: null);
            }

            previous = current;
        }

        Assert.Equal(1_484, mappedCount);
        Assert.Equal(
            "cd611463181fccee42283bcb523a37bb5364c4dd05028207a45afc288bec61f2", // DevSkim: ignore DS173237
            Convert.ToHexString(foldHash.GetHashAndReset()).ToLowerInvariant());

        void AssertComparerParity(
            string left,
            string right,
            int? expected)
        {
            var generatorResult = HumanizerSourceGenerator.CompareInflectionKeys(left, right);
            var runtimeResult = runtimeCompare!(left, right);
            if (generatorResult != runtimeResult ||
                expected is { } value && generatorResult != value)
            {
                throw new Xunit.Sdk.XunitException(
                    $"Comparer mismatch for '{FormatCodePoints(left)}' and '{FormatCodePoints(right)}': " +
                    $"generator {generatorResult}, runtime {runtimeResult}, expected {expected?.ToString() ?? "parity"}.");
            }
        }
    }

    [Fact]
    public void EqualPriorityPrefixRulesEmitLongestMatchFirst()
    {
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText(
                "src/Humanizer/Locales/zz.yml",
                OverlappingPrefixInflectionFixture()));

        Assert.Empty(runResult.Diagnostics);
        var owner = GetGeneratedSource(runResult, "GeneratedInflection_zz.g.cs");
        Assert.True(
            owner.IndexOf("\"zz.forward.z-long\"", StringComparison.Ordinal) <
            owner.IndexOf("\"zz.forward.a-short\"", StringComparison.Ordinal));
    }

    [Fact]
    public void EqualPriorityCrossKindRulesEmitLongestAffixFirst()
    {
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText(
                "src/Humanizer/Locales/zz.yml",
                OverlappingCrossKindInflectionFixture()));

        Assert.Empty(runResult.Diagnostics);
        var owner = GetGeneratedSource(runResult, "GeneratedInflection_zz.g.cs");
        Assert.True(
            owner.IndexOf("\"zz.forward.a-long-prefix\"", StringComparison.Ordinal) <
            owner.IndexOf("\"zz.forward.z-short-suffix\"", StringComparison.Ordinal));
    }

    [Theory]
    [MemberData(nameof(ReachableCategoryCases))]
    public void ReachableCategoriesExactlyMatchCardinalRuleOutputs(
        string cardinalRule,
        decimal[] witnesses)
    {
        var categories = SelectReachableCategories(cardinalRule, witnesses);
        Assert.Equal(SelectDeclaredReachableCategories(cardinalRule), categories);
        var locale = InflectionFixtureForCategories(cardinalRule, categories);
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Empty(runResult.Diagnostics);
        var owner = GetGeneratedSource(runResult, "GeneratedInflection_zz.g.cs");
        var expected = categories.ToHashSet(StringComparer.Ordinal);
        foreach (var category in new[] { "zero", "one", "two", "few", "many", "other" })
        {
            var enumReference = $"CardinalPluralCategory.{char.ToUpperInvariant(category[0])}{category.Substring(1)}";
            Assert.Equal(
                expected.Contains(category),
                owner.Contains(enumReference, StringComparison.Ordinal));
        }
    }

    static string[] SelectReachableCategories(
        string cardinalRule,
        decimal[] witnesses)
    {
        var assembly = typeof(Configurator).Assembly;
        var kindType = assembly.GetTypes().Single(
            static type => type.Name == "CardinalPluralRuleKind");
        var rulesType = assembly.GetTypes().Single(
            static type => type.Name == "CardinalPluralRules");
        var select = rulesType.GetMethod(
            "Select",
            BindingFlags.Public | BindingFlags.Static,
            null,
            [kindType, typeof(decimal)],
            null)
            ?? throw new InvalidOperationException("Cardinal selector was not found.");
        var kind = Enum.Parse(kindType, cardinalRule);
        return witnesses
            .Select(quantity => select.Invoke(null, [kind, quantity])?.ToString()
                ?? throw new InvalidOperationException("Cardinal selector returned null."))
            .Select(static category => category.ToLowerInvariant())
            .Distinct(StringComparer.Ordinal)
            .OrderBy(static category => Array.IndexOf(
                ["zero", "one", "two", "few", "many", "other"],
                category))
            .ToArray();
    }

    static string[] SelectDeclaredReachableCategories(string cardinalRule)
    {
        var metadataType = typeof(Configurator).Assembly.GetTypes().Single(
            static type => type.Name == "CardinalPluralRuleMetadata");
        var method = metadataType.GetMethod(
            "GetReachableCategories",
            BindingFlags.Public | BindingFlags.Static)
            ?? throw new InvalidOperationException("Cardinal rule metadata was not found.");
        return (string[])(method.Invoke(null, [cardinalRule])
            ?? throw new InvalidOperationException("Cardinal rule metadata returned null."));
    }

    [Theory]
    [InlineData(95, false)]
    [InlineData(94, true)]
    public void ActiveInflectionEvidenceEnforcesTheExactCoverageBoundary(
        int covered,
        bool expectsError)
    {
        var runResult = RunGeneratorIsolated(new InMemoryAdditionalText(
            "src/Humanizer/Locales/zz.yml",
            CompleteInflectionFixture(eligible: 100, irregular: 100, covered)));

        Assert.Equal(
            expectsError,
            runResult.Diagnostics.Any(static diagnostic =>
                diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains(
                    "95% irregular-occurrence coverage",
                    StringComparison.Ordinal)));
    }

    [Theory]
    [InlineData(1, false)]
    [InlineData(0, true)]
    public void ActiveInflectionNaCoverageRequiresPositiveEligibleCensus(
        int eligible,
        bool expectsError)
    {
        var runResult = RunGeneratorIsolated(new InMemoryAdditionalText(
            "src/Humanizer/Locales/zz.yml",
            CompleteInflectionFixture(eligible, irregular: 0, covered: 0)));

        Assert.Equal(
            expectsError,
            runResult.Diagnostics.Any(static diagnostic =>
                diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains(
                    "positive eligible census",
                    StringComparison.Ordinal)));
    }

    [Fact]
    public void InheritedActiveProfileResolvesToItsAuthoredAtomicOwner()
    {
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText(
                "src/Humanizer/Locales/zz.yml",
                CompleteInflectionFixtureFor("zz")),
            new InMemoryAdditionalText(
                "src/Humanizer/Locales/zz-Latn-ZZ.yml",
                """
locale: 'zz-Latn-ZZ'
variantOf: 'zz'
surfaces:
  durationCases:
    classification: 'not-applicable'
  list:
    engine: 'conjunction'
    pairTemplate: '{0} and {1}'
    finalTemplate: '{0} and {1}'
    serialTemplate: '{0}, {1}'
    oxfordComma: false
    cliticizesFinal: false
"""));

        Assert.Empty(runResult.Diagnostics);
        var resolver = GetGeneratedSource(runResult, "GeneratedCultureResolver.g.cs");
        var registry = GetGeneratedSource(runResult, "LocalizedInflectionCatalog.g.cs");

        Assert.Contains("new(\"zz-Latn-ZZ\", \"zz-Latn-ZZ\", \"zz\", null)", resolver, StringComparison.Ordinal);
        Assert.DoesNotContain("GeneratedInflection_zz_ZZ", registry, StringComparison.Ordinal);
        Assert.Single(
            runResult.Results[0].GeneratedSources,
            static source => source.HintName == "GeneratedInflection_zz.g.cs");
    }

    [Fact]
    public void AliasChainFlattensToTheTerminalAtomicOwner()
    {
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText(
                "src/Humanizer/Locales/zz-Latn.yml",
                CompleteInflectionFixtureFor("zz-Latn")),
            new InMemoryAdditionalText(
                "src/Humanizer/Locales/zz-Latn-XA.yml",
                AliasInflectionFixture("zz-Latn-XA", "zz-Latn", "zz-Latn")),
            new InMemoryAdditionalText(
                "src/Humanizer/Locales/zz-Latn-XC.yml",
                AliasInflectionFixture("zz-Latn-XC", "zz-Latn-XA", "zz-Latn-XA")));

        Assert.Empty(runResult.Diagnostics);
        var resolver = GetGeneratedSource(runResult, "GeneratedCultureResolver.g.cs");

        Assert.Contains(
            "new(\"zz-Latn-XC\", \"zz-Latn-XC\", \"zz-Latn\", null)",
            resolver,
            StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("zh-Hans-XT", "zh-Hant-XT", "Hani")]
    [InlineData("sr-Cyrl-XT", "sr-Latn-XT", "Cyrl")]
    [InlineData("pa-Guru-XT", "pa-Arab-XT", "Guru")]
    [InlineData("pa-Guru-XT", "pa-PK-XT", "Guru")]
    [InlineData("uz-Latn-XT", "uz-Cyrl-XT", "Latn")]
    [InlineData("uz-Latn-XT", "uz-AF-XT", "Latn")]
    public void AliasOwnerMustHaveTheSameEffectiveScript(
        string owner,
        string alias,
        string script)
    {
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText(
                $"src/Humanizer/Locales/{owner}.yml",
                CompleteInflectionFixtureFor(owner, script)),
            new InMemoryAdditionalText(
                $"src/Humanizer/Locales/{alias}.yml",
                AliasInflectionFixture(alias, owner, owner)));

        Assert.Contains(
            runResult.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains(
                    "effective script",
                    StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(
            runResult.Results[0].GeneratedSources,
            static source => source.HintName == "LocalizedInflectionCatalog.g.cs");
    }

    [Fact]
    public void DuplicateYamlProfileKeySuppressesCatalogOutput()
    {
        var locale = CompleteInflectionFixtureFor("zz").Replace(
            "    capability: 'display-by-category'",
            """
    capability: 'display-by-category'
    capability: 'display-by-category'
""",
            StringComparison.Ordinal);
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Contains(
            runResult.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains(
                    "Duplicate mapping key 'capability'",
                    StringComparison.Ordinal));
        Assert.DoesNotContain(
            runResult.Results[0].GeneratedSources,
            static source => source.HintName is
                "GeneratedInflection_zz.g.cs" or
                "LocalizedInflectionCatalog.g.cs" or
                "GeneratedCultureResolver.g.cs");
    }

    [Fact]
    public void DuplicateRootInflectionKeySuppressesCatalogOutput()
    {
        const string locale = """
locale: 'zz'
surfaces:
  durationCases:
    classification: 'not-applicable'
  inflection:
    cardinalRule: 'Other'
    capability: 'inert'
  inflection:
    cardinalRule: 'EnglishLike'
    capability: 'inert'
""";
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Contains(
            runResult.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains(
                    "Duplicate mapping key 'inflection'",
                    StringComparison.Ordinal));
        Assert.DoesNotContain(
            runResult.Results[0].GeneratedSources,
            static source => source.HintName is
                "LocalizedInflectionCatalog.g.cs" or
                "GeneratedCultureResolver.g.cs");
    }

    [Fact]
    public void DuplicateRegionalProfileKeySuppressesCatalogOutput()
    {
        var locale = CompleteInflectionFixtureFor("zz").Replace(
            "    accepted-cultures:",
            """
    regionalRules:
      zz-AA: 'Other'
      zz-AA: 'EnglishLike'
    accepted-cultures:
""",
            StringComparison.Ordinal);
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Contains(
            runResult.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains(
                    "Duplicate mapping key 'zz-AA'",
                    StringComparison.Ordinal));
        Assert.DoesNotContain(
            runResult.Results[0].GeneratedSources,
            static source => source.HintName is
                "GeneratedInflection_zz.g.cs" or
                "LocalizedInflectionCatalog.g.cs" or
                "GeneratedCultureResolver.g.cs");
    }

    [Fact]
    public void OwnerMustCoverCategoriesReachableByAcceptedRegionalRules()
    {
        var locale = CompleteInflectionFixtureFor("zz")
            .Replace(
                "      - 'zz'",
                """
      - 'zz'
      - 'zz-GB'
""",
                StringComparison.Ordinal)
            .Replace(
                "    scripts:",
                """
    regionalRules:
      zz-GB: 'Welsh'
    scripts:
""",
                StringComparison.Ordinal);
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Contains(
            runResult.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains("zz-GB", StringComparison.Ordinal) &&
                diagnostic.GetMessage().Contains("zero", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(
            runResult.Results[0].GeneratedSources,
            static source => source.HintName is
                "GeneratedInflection_zz.g.cs" or
                "LocalizedInflectionCatalog.g.cs" or
                "GeneratedCultureResolver.g.cs");
    }

    [Fact]
    public void SanitizedOwnerHintNamesMustBeUnique()
    {
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText(
                "src/Humanizer/Locales/zz-A.yml",
                CompleteInflectionFixtureFor("zz-A")),
            new InMemoryAdditionalText(
                "src/Humanizer/Locales/zz_A.yml",
                CompleteInflectionFixtureFor("zz_A")));

        Assert.Contains(
            runResult.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains(
                    "same generated identifier",
                    StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(
            runResult.Results[0].GeneratedSources,
            static source => source.HintName.StartsWith(
                "GeneratedInflection_",
                StringComparison.Ordinal));
    }

    [Fact]
    public void RuleTemplateMustContainExactlyOneStemPlaceholder()
    {
        var locale = ProductiveInflectionFixture().Replace(
            "dictionary-plural: '{stem}ies'",
            "dictionary-plural: '{stem}{stem}ies'",
            StringComparison.Ordinal);
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Contains(
            runResult.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains(
                    "exactly one",
                    StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(
            runResult.Results[0].GeneratedSources,
            static source => source.HintName is
                "GeneratedInflection_zz.g.cs" or
                "LocalizedInflectionCatalog.g.cs" or
                "GeneratedCultureResolver.g.cs");
    }

    [Fact]
    public void RuleScopeCountabilityMustBeSupported()
    {
        var locale = ProductiveInflectionFixture().Replace(
            "            - 'count'",
            "            - 'imaginary'",
            StringComparison.Ordinal);
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Contains(
            runResult.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains(
                    "countability 'imaginary'",
                    StringComparison.Ordinal));
    }

    [Fact]
    public void DirectionEvidenceMustCiteAnAuthoredSource()
    {
        var locale = ProductiveInflectionFixture().Replace(
            """
      pluralize:
        eligible: 100
        irregular: 100
        covered: 95
        attempted: 100
        correct: 99
        sources:
          - 'fixture'
""",
            """
      pluralize:
        eligible: 100
        irregular: 100
        covered: 95
        attempted: 100
        correct: 99
""",
            StringComparison.Ordinal);
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Contains(
            runResult.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains(
                    "evidence 'pluralize' must define sources",
                    StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void ReverseEnabledOnlyReversesAForwardRule()
    {
        var locale = ProductiveInflectionFixture().Replace(
            "direction: 'forward'",
            "direction: 'reverse'",
            StringComparison.Ordinal);
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Contains(
            runResult.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains(
                    "reverse.enabled is only valid for a forward rule",
                    StringComparison.Ordinal));
    }

    [Fact]
    public void ReverseEnabledRuleMustUseAnInvertibleStemPrefixTemplate()
    {
        var locale = ProductiveInflectionFixture().Replace(
            "dictionary-plural: '{stem}ies'",
            "dictionary-plural: 'ge{stem}'",
            StringComparison.Ordinal);
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Contains(
            runResult.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains(
                    "reverse-enabled output must start with '{stem}'",
                    StringComparison.Ordinal));
    }

    [Fact]
    public void DivergentInvariantBundleIsRejected()
    {
        var locale = CompleteInflectionFixtureFor("zz").Replace(
            "capability: 'display-by-category'",
            "capability: 'invariant'",
            StringComparison.Ordinal);
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Contains(
            runResult.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains(
                    "invariant",
                    StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void InvariantBundleCannotDefineProductiveRules()
    {
        var locale = ProductiveInflectionFixture().Replace(
            "capability: 'display-by-category'",
            "capability: 'invariant'",
            StringComparison.Ordinal);
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Contains(
            runResult.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains(
                    "invariant bundle cannot define productive rules",
                    StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void IdentityCompatibleInvariantBundleEmits()
    {
        var locale = CompleteInflectionFixtureFor("zz")
            .Replace(
                "capability: 'display-by-category'",
                "capability: 'invariant'",
                StringComparison.Ordinal)
            .Replace("'cats'", "'fish'", StringComparison.Ordinal)
            .Replace("'cat'", "'fish'", StringComparison.Ordinal);
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Empty(runResult.Diagnostics);
        var owner = GetGeneratedSource(runResult, "GeneratedInflection_zz.g.cs");
        Assert.Contains("\"fish\"", owner, StringComparison.Ordinal);
        Assert.DoesNotContain("\"cats\"", owner, StringComparison.Ordinal);
    }

    [Fact]
    public void EmptyAttestedInvariantBundleEmitsTerminalCapability()
    {
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText(
                "src/Humanizer/Locales/vi.yml",
                InvariantInflectionFixtureFor("vi")));

        Assert.Empty(runResult.Diagnostics);
        var owner = GetGeneratedSource(runResult, "GeneratedInflection_vi.g.cs");
        Assert.Contains("InflectionCapability.Invariant", owner, StringComparison.Ordinal);
        Assert.Contains("new(\"vi\", \"vi\", \"vi\"", GetGeneratedSource(
            runResult,
            "GeneratedCultureResolver.g.cs"), StringComparison.Ordinal);
    }

    [Fact]
    public void GeneratedInvariantOwnersCompileAndTerminateArbitraryNouns()
    {
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText(
                "src/Humanizer/Locales/vi.yml",
                InvariantInflectionFixtureFor("vi")),
            new InMemoryAdditionalText(
                "src/Humanizer/Locales/yo.yml",
                InvariantInflectionFixtureFor("yo")));
        Assert.Empty(runResult.Diagnostics);

        const string harness = """
namespace Humanizer;

public static class CompiledInvariantHarness
{
    public static string RunVi(string input) => Run(GeneratedInflection_vi.Bundle, input);
    public static string RunYo(string input) => Run(GeneratedInflection_yo.Bundle, input);

    static string Run(InflectionBundle bundle, string input)
    {
        var result = bundle.Inflect(
            input,
            InflectionDirection.Forward,
            allowProductive: true,
            category: CardinalPluralCategory.Other);
        return result.Status + "|" + object.ReferenceEquals(input, result.Value);
    }
}
""";
        var parseOptions = new CSharpParseOptions(LanguageVersion.Preview);
        var cancellationToken = TestContext.Current.CancellationToken;
        var compilation = CSharpCompilation.Create(
            "Humanizer.Tests",
            [
                CSharpSyntaxTree.ParseText(
                    GetGeneratedSource(runResult, "GeneratedInflection_vi.g.cs"),
                    parseOptions,
                    cancellationToken: cancellationToken),
                CSharpSyntaxTree.ParseText(
                    GetGeneratedSource(runResult, "GeneratedInflection_yo.g.cs"),
                    parseOptions,
                    cancellationToken: cancellationToken),
                CSharpSyntaxTree.ParseText(
                    harness,
                    parseOptions,
                    cancellationToken: cancellationToken)
            ],
            GetMetadataReferences(),
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithCryptoKeyFile(
                    Path.Combine(FindRepositoryRoot(), "Humanizer.snk"))
                .WithPublicSign(true));
        using var assemblyStream = new MemoryStream();
        var emitResult = compilation.Emit(
            assemblyStream,
            cancellationToken: cancellationToken);
        Assert.True(
            emitResult.Success,
            string.Join(Environment.NewLine, emitResult.Diagnostics));

        var assembly = Assembly.Load(assemblyStream.ToArray());
        var harnessType = assembly.GetType("Humanizer.CompiledInvariantHarness");
        Assert.NotNull(harnessType);
        Assert.Equal(
            "Invariant|True",
            harnessType!.GetMethod("RunVi")?.Invoke(null, ["người"]));
        Assert.Equal(
            "Invariant|True",
            harnessType.GetMethod("RunYo")?.Invoke(null, ["ọmọ"]));
    }

    [Fact]
    public void PartialPublicActivationSuppressesAllRouting()
    {
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText(
                "src/Humanizer/Locales/vi.yml",
                InvariantInflectionFixtureFor("vi").Replace(
                    "      - 'vi'",
                    """
      - 'vi'
      - 'vi-Cyrl-XT'
""",
                    StringComparison.Ordinal)),
            new InMemoryAdditionalText(
                "src/Humanizer/Locales/yo.yml",
                """
locale: 'yo'
surfaces:
  durationCases:
    classification: 'not-applicable'
  inflection:
    cardinalRule: 'Other'
    capability: 'inert'
"""));

        Assert.Contains(
            runResult.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains(
                    "partial inflection activation",
                    StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(
            runResult.Results[0].GeneratedSources,
            static source => source.HintName is
                "GeneratedCultureResolver.g.cs" or
                "Configurator.SupportedCultures.g.cs" or
                "LocalizedInflectionCatalog.g.cs");
    }

    [Fact]
    public void AnyAtomicActivationRequiresEveryAcceptedCultureToBeTerminal()
    {
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText(
                "src/Humanizer/Locales/zz-Latn.yml",
                CompleteInflectionFixtureFor("zz-Latn")),
            new InMemoryAdditionalText(
                "src/Humanizer/Locales/yy-Latn.yml",
                """
locale: 'yy-Latn'
surfaces:
  durationCases:
    classification: 'not-applicable'
  inflection:
    cardinalRule: 'Other'
    capability: 'inert'
"""));

        Assert.Contains(
            runResult.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains(
                    "partial inflection activation",
                    StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(
            runResult.Results[0].GeneratedSources,
            static source => source.HintName is
                "GeneratedCultureResolver.g.cs" or
                "Configurator.SupportedCultures.g.cs" or
                "LocalizedInflectionCatalog.g.cs");
    }

    [Fact]
    public void CompleteInvariantActivationGivesEveryAcceptedCultureATerminal()
    {
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText(
                "src/Humanizer/Locales/vi.yml",
                InvariantInflectionFixtureFor("vi").Replace(
                    "      - 'vi'",
                    """
      - 'vi'
      - 'vi-Cyrl-XT'
""",
                    StringComparison.Ordinal)),
            new InMemoryAdditionalText(
                "src/Humanizer/Locales/yo.yml",
                InvariantInflectionFixtureFor("yo")));

        Assert.Empty(runResult.Diagnostics);
        var resolver = GetGeneratedSource(runResult, "GeneratedCultureResolver.g.cs");
        Assert.DoesNotContain(", null, null)", resolver, StringComparison.Ordinal);
        Assert.Contains("InflectionStatus.Invariant", resolver, StringComparison.Ordinal);
    }

    [Fact]
    public void UnscriptedAzerbaijaniCannotAttachToCyrillicOwner()
    {
        var locale = CompleteInflectionFixtureFor("az-Cyrl", "Cyrl").Replace(
            "      - 'az-Cyrl'",
            """
      - 'az-Cyrl'
      - 'az'
""",
            StringComparison.Ordinal);
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText(
                "src/Humanizer/Locales/az-Cyrl.yml",
                locale));

        Assert.Empty(runResult.Diagnostics);
        var resolver = GetGeneratedSource(runResult, "GeneratedCultureResolver.g.cs");
        Assert.Contains("new(\"az\", \"az-Cyrl\", null", resolver, StringComparison.Ordinal);
        Assert.Contains(
            "new(\"az-Cyrl\", \"az-Cyrl\", \"az-Cyrl\"",
            resolver,
            StringComparison.Ordinal);
    }

    [Fact]
    public void UnmappedUnscriptedAcceptedCultureFailsClosed()
    {
        var locale = CompleteInflectionFixtureFor("zz-Latn").Replace(
            "      - 'zz-Latn'",
            """
      - 'zz-Latn'
      - 'zz-XT'
""",
            StringComparison.Ordinal);
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText(
                "src/Humanizer/Locales/zz-Latn.yml",
                locale));

        Assert.Empty(runResult.Diagnostics);
        Assert.Contains(
            "new(\"zz-XT\", \"zz-Latn\", null, InflectionStatus.Unsupported)",
            GetGeneratedSource(runResult, "GeneratedCultureResolver.g.cs"),
            StringComparison.Ordinal);
    }

    [Fact]
    public void UnmappedUnscriptedAliasFailsClosed()
    {
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText(
                "src/Humanizer/Locales/zz-Latn.yml",
                CompleteInflectionFixtureFor("zz-Latn")),
            new InMemoryAdditionalText(
                "src/Humanizer/Locales/zz-XT.yml",
                AliasInflectionFixture("zz-XT", "zz-Latn", "zz-Latn")));

        Assert.Contains(
            runResult.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains(
                    "effective script",
                    StringComparison.OrdinalIgnoreCase));
    }

    [Theory]
    [InlineData("кот")]
    [InlineData("cаt")]
    public void AuthoredFormsMustUseOnlyDeclaredScripts(string form)
    {
        var locale = CompleteInflectionFixtureFor("zz").Replace(
            "'cat'",
            $"'{form}'",
            StringComparison.Ordinal);
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Contains(
            runResult.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains(
                    "declared scripts",
                    StringComparison.OrdinalIgnoreCase));
    }

    [Theory]
    [InlineData("suffix: 'y'", "suffix: 'ы'")]
    [InlineData("dictionary-plural: '{stem}ies'", "dictionary-plural: '{stem}ы'")]
    public void ProductiveRuleLiteralsMustUseOnlyDeclaredScripts(
        string original,
        string replacement)
    {
        var locale = ProductiveInflectionFixture().Replace(
            original,
            replacement,
            StringComparison.Ordinal);
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Contains(
            runResult.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains(
                    "declared scripts",
                    StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void LowerTitleUpperRuleLiteralsAreNormalizedBeforeEmission()
    {
        var locale = ProductiveInflectionFixture()
            .Replace("casing: 'exact'", "casing: 'lower-title-upper'", StringComparison.Ordinal)
            .Replace("suffix: 'y'", "suffix: 'Y'", StringComparison.Ordinal)
            .Replace("{stem}ies", "{stem}ie\u0301s", StringComparison.Ordinal);
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Empty(runResult.Diagnostics);
        var owner = GetGeneratedSource(runResult, "GeneratedInflection_zz.g.cs");
        Assert.Contains("\"y\"", owner, StringComparison.Ordinal);
        Assert.Contains("\"{stem}iés\"", owner, StringComparison.Ordinal);
        Assert.DoesNotContain("\"Y\"", owner, StringComparison.Ordinal);
        Assert.DoesNotContain("\u0301", owner, StringComparison.Ordinal);
    }

    [Fact]
    public void LowerTitleUpperFormsAreNormalizedBeforeCollisionChecks()
    {
        var locale = CompleteInflectionFixtureFor("zz")
            .Replace("casing: 'exact'", "casing: 'lower-title-upper'", StringComparison.Ordinal)
            .Replace("'cat'", "'cafe\u0301'", StringComparison.Ordinal);
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Empty(runResult.Diagnostics);
        var owner = GetGeneratedSource(runResult, "GeneratedInflection_zz.g.cs");
        Assert.Contains("\"café\"", owner, StringComparison.Ordinal);
        Assert.DoesNotContain("\u0301", owner, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("lexemes", "[]")]
    [InlineData("rules", "[]")]
    [InlineData("evidence", "{}")]
    [InlineData("scripts", "[]")]
    public void AliasCapabilityRejectsDataPayload(
        string property,
        string value)
    {
        var locale = AliasInflectionFixture(
            "zz-Latn-XA",
            "zz-Latn",
            "zz-Latn").Replace(
                "    owner: 'zz-Latn'",
                $"    owner: 'zz-Latn'\n    {property}: {value}",
                StringComparison.Ordinal);
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText(
                "src/Humanizer/Locales/zz-Latn.yml",
                CompleteInflectionFixtureFor("zz-Latn")),
            new InMemoryAdditionalText(
                "src/Humanizer/Locales/zz-Latn-XA.yml",
                locale));

        Assert.Contains(
            runResult.Diagnostics,
            diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains(
                    $"alias defines unsupported property '{property}'",
                    StringComparison.OrdinalIgnoreCase));
    }

    [Theory]
    [InlineData("preferred")]
    [InlineData("direction")]
    [InlineData("output")]
    public void DuplicateNestedInflectionKeysAreRejected(string key)
    {
        var locale = key switch
        {
            "preferred" => CompleteInflectionFixtureFor("zz").Replace(
                "            preferred: 'cat'",
                "            preferred: 'cat'\n            preferred: 'cat'",
                StringComparison.Ordinal),
            "direction" => ProductiveInflectionFixture().Replace(
                "        direction: 'forward'",
                "        direction: 'forward'\n        direction: 'forward'",
                StringComparison.Ordinal),
            _ => ProductiveInflectionFixture().Replace(
                "        output:",
                "        output:\n          dictionary-plural: '{stem}ies'\n          display:\n            one: '{stem}y'\n            other: '{stem}ies'\n        output:",
                StringComparison.Ordinal)
        };
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Contains(
            runResult.Diagnostics,
            diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains(
                    $"Duplicate mapping key '{key}'",
                    StringComparison.Ordinal));
    }

    [Fact]
    public void EditingOneLocaleInvalidatesOnlyItsOwnerInput()
    {
        var compilation = CreateCompilation();
        AdditionalText vi = new InMemoryAdditionalText(
            "src/Humanizer/Locales/vi.yml",
            InvariantInflectionFixtureFor("vi"));
        var yo = new InMemoryAdditionalText(
            "src/Humanizer/Locales/yo.yml",
            InvariantInflectionFixtureFor("yo"));
        var options = new GeneratorDriverOptions(
            IncrementalGeneratorOutputKind.None,
            trackIncrementalGeneratorSteps: true);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(
            [new HumanizerSourceGenerator().AsSourceGenerator()],
            [vi, yo],
            (CSharpParseOptions)compilation.SyntaxTrees.Single().Options,
            optionsProvider: null,
            driverOptions: options);
        driver = driver.RunGenerators(
            compilation,
            TestContext.Current.CancellationToken);
        var changedVi = new InMemoryAdditionalText(
            "src/Humanizer/Locales/vi.yml",
            InvariantInflectionFixtureFor("vi").Replace(
                "casing: 'lower-title-upper'",
                "casing: 'exact'",
                StringComparison.Ordinal));

        driver = driver
            .ReplaceAdditionalText(vi, changedVi)
            .RunGenerators(
                compilation,
                TestContext.Current.CancellationToken);
        var steps = driver.GetRunResult().Results[0]
            .TrackedSteps["InflectionOwnerSource"];
        var reasons = steps
            .SelectMany(static step => step.Outputs)
            .Select(static output => output.Reason)
            .ToArray();

        Assert.Single(
            reasons,
            static reason => reason == IncrementalStepRunReason.Modified);
        Assert.Single(
            reasons,
            static reason => reason is
                IncrementalStepRunReason.Cached or
                IncrementalStepRunReason.Unchanged);
    }

    [Fact]
    public void FormOnlyEditDoesNotInvalidateInflectionRegistries()
    {
        var compilation = CreateCompilation();
        AdditionalText locale = new InMemoryAdditionalText(
            "src/Humanizer/Locales/zz.yml",
            CompleteInflectionFixture(eligible: 100, irregular: 100, covered: 95));
        var options = new GeneratorDriverOptions(
            IncrementalGeneratorOutputKind.None,
            trackIncrementalGeneratorSteps: true);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(
            [new HumanizerSourceGenerator().AsSourceGenerator()],
            [locale],
            (CSharpParseOptions)compilation.SyntaxTrees.Single().Options,
            optionsProvider: null,
            driverOptions: options);
        driver = driver.RunGenerators(
            compilation,
            TestContext.Current.CancellationToken);
        var changed = new InMemoryAdditionalText(
            "src/Humanizer/Locales/zz.yml",
            CompleteInflectionFixture(eligible: 100, irregular: 100, covered: 95)
                .Replace("'cat'", "'feline'", StringComparison.Ordinal)
                .Replace("'cats'", "'felines'", StringComparison.Ordinal));

        driver = driver
            .ReplaceAdditionalText(locale, changed)
            .RunGenerators(
                compilation,
                TestContext.Current.CancellationToken);

        Assert.All(
            InflectionRegistryTrackingNames,
            trackingName => Assert.All(
                driver.GetRunResult().Results[0].TrackedSteps[trackingName]
                    .SelectMany(static step => step.Outputs),
                static output => Assert.True(
                    output.Reason is IncrementalStepRunReason.Cached or
                        IncrementalStepRunReason.Unchanged,
                    $"Unexpected registry run reason: {output.Reason}.")));
    }

    [Theory]
    [InlineData("owner")]
    [InlineData("accepted-name")]
    [InlineData("script")]
    [InlineData("capability")]
    public void OwnershipMetadataEditInvalidatesInflectionRegistries(string change)
    {
        var baseline = change is "script"
            ? InvariantInflectionFixtureFor("zz")
            : CompleteInflectionFixture(eligible: 100, irregular: 100, covered: 95);
        var changedText = change switch
        {
            "owner" => baseline.Replace(
                "    capability: 'display-by-category'",
                "    capability: 'display-by-category'\n    owner: 'zz-owner'",
                StringComparison.Ordinal),
            "accepted-name" => baseline.Replace(
                "      - 'zz'",
                "      - 'zz'\n      - 'zz-Latn-ZZ'",
                StringComparison.Ordinal),
            "script" => baseline.Replace("'Latn'", "'Grek'", StringComparison.Ordinal),
            "capability" => baseline
                .Replace("'cats'", "'cat'", StringComparison.Ordinal)
                .Replace(
                    "    capability: 'display-by-category'",
                    "    capability: 'invariant'",
                    StringComparison.Ordinal),
            _ => throw new ArgumentOutOfRangeException(nameof(change))
        };
        var compilation = CreateCompilation();
        AdditionalText locale = new InMemoryAdditionalText(
            "src/Humanizer/Locales/zz.yml",
            baseline);
        var options = new GeneratorDriverOptions(
            IncrementalGeneratorOutputKind.None,
            trackIncrementalGeneratorSteps: true);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(
            [new HumanizerSourceGenerator().AsSourceGenerator()],
            [locale],
            (CSharpParseOptions)compilation.SyntaxTrees.Single().Options,
            optionsProvider: null,
            driverOptions: options);
        driver = driver.RunGenerators(
            compilation,
            TestContext.Current.CancellationToken);
        var changed = new InMemoryAdditionalText(
            "src/Humanizer/Locales/zz.yml",
            changedText);

        driver = driver
            .ReplaceAdditionalText(locale, changed)
            .RunGenerators(
                compilation,
                TestContext.Current.CancellationToken);

        Assert.All(
            InflectionRegistryTrackingNames,
            trackingName => Assert.Contains(
                driver.GetRunResult().Results[0].TrackedSteps[trackingName]
                    .SelectMany(static step => step.Outputs),
                static output => output.Reason == IncrementalStepRunReason.Modified));
    }

    [Theory]
    [InlineData("hi-XT", "Deva", "hi-Deva-XT", "hi-Latn-XT")]
    [InlineData("zh-Hans-XT", "Hani", "zh-Hans-XT", "zh-Hant-XT")]
    [InlineData("sr-Cyrl-XT", "Cyrl", "sr-Cyrl-XT", "sr-Latn-XT")]
    [InlineData("pa-Guru-XT", "Guru", "pa-Guru-XT", "pa-Arab-XT")]
    [InlineData("uz-Latn-XT", "Latn", "uz-Latn-XT", "uz-Cyrl-XT")]
    public void AcceptedCulturesAttachOnlyScriptCompatibleInflectionOwners(
        string owner,
        string script,
        string compatibleName,
        string incompatibleName)
    {
        var acceptedNames = string.Join(
            "\n",
            new[] { owner, compatibleName, incompatibleName }
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Select(static name => $"      - '{name}'"));
        var locale = CompleteInflectionFixtureFor(owner, script).Replace(
            $"      - '{owner}'",
            acceptedNames,
            StringComparison.Ordinal);
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText($"src/Humanizer/Locales/{owner}.yml", locale));

        Assert.Empty(runResult.Diagnostics);
        var resolver = GetGeneratedSource(runResult, "GeneratedCultureResolver.g.cs");
        Assert.Contains(
            $"new(\"{compatibleName}\", \"{owner}\", \"{owner}\", null)",
            resolver,
            StringComparison.Ordinal);
        Assert.Contains(
            $"new(\"{incompatibleName}\", \"{owner}\", null, InflectionStatus.Unsupported)",
            resolver,
            StringComparison.Ordinal);
    }

    [Fact]
    public void GuardsAreNormalizedToOwnerCasingAndNfc()
    {
        var locale = ProductiveInflectionFixture()
            .Replace(
                "    casing: 'exact'",
                """
    casing: 'lower-title-upper'
    skip-simple-words:
      - 'News'
""",
                StringComparison.Ordinal)
            .Replace("'day'", "'cafe\u0301'", StringComparison.Ordinal);
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Empty(runResult.Diagnostics);
        var owner = GetGeneratedSource(runResult, "GeneratedInflection_zz.g.cs");
        Assert.Contains("\"news\"", owner, StringComparison.Ordinal);
        Assert.DoesNotContain("\"News\"", owner, StringComparison.Ordinal);
        Assert.Contains("\"café\"", owner, StringComparison.Ordinal);
        Assert.DoesNotContain("\"cafe\u0301\"", owner, StringComparison.Ordinal);
    }

    [Fact]
    public void NormalizedSkipDuplicatesAreRejected()
    {
        var locale = CompleteInflectionFixtureFor("zz").Replace(
            "    phrase-mode: 'exact-only'",
            """
    phrase-mode: 'exact-only'
    skip-simple-words:
      - 'News'
      - 'news'
""",
            StringComparison.Ordinal).Replace(
                "    casing: 'exact'",
                "    casing: 'lower-title-upper'",
                StringComparison.Ordinal);
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Contains(
            runResult.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains(
                    "duplicate after normalization",
                    StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void NormalizedExclusionDuplicatesAreRejected()
    {
        var locale = ProductiveInflectionFixture().Replace(
            """
          surfaces:
            - 'day'
""",
            """
          surfaces:
            - 'café'
            - 'café'
""",
            StringComparison.Ordinal);
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Contains(
            runResult.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains(
                    "duplicate after normalization",
                    StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void OversizedGeneratedOwnerIsDiagnosedAndSuppressed()
    {
        var hugeForm = new string('a', 270_000);
        var locale = CompleteInflectionFixtureFor("zz").Replace(
            "'cat'",
            $"'{hugeForm}'",
            StringComparison.Ordinal);
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Contains(
            runResult.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains("256 KiB", StringComparison.Ordinal));
        Assert.DoesNotContain(
            runResult.Results[0].GeneratedSources,
            static source => source.HintName == "GeneratedInflection_zz.g.cs");
    }

    [Theory]
    [InlineData("selector", "unsupported quantity selector")]
    [InlineData("capability", "requires capability 'display-by-category'")]
    [InlineData("cardinal-rule", "requires cardinalRule 'Other'")]
    public void ExactNumericSingletonSelectorRequiresSupportedOwnerShape(
        string mutation,
        string expected)
    {
        var locale = mutation switch
        {
            "selector" => ExactNumericSingletonInflectionFixture().Replace(
                "exact-numeric-singleton",
                "nearest-numeric-singleton",
                StringComparison.Ordinal),
            "capability" => ExactNumericSingletonInflectionFixture().Replace(
                "capability: 'display-by-category'",
                "capability: 'invariant'",
                StringComparison.Ordinal),
            "cardinal-rule" => ExactNumericSingletonInflectionFixture().Replace(
                "cardinalRule: 'Other'",
                "cardinalRule: 'EnglishLike'",
                StringComparison.Ordinal),
            _ => throw new ArgumentOutOfRangeException(nameof(mutation))
        };
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Contains(
            runResult.Diagnostics,
            diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains(expected, StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(
            runResult.Results[0].GeneratedSources,
            static source => source.HintName == "GeneratedInflection_zz.g.cs");
    }

    [Fact]
    public void ExactNumericSingletonSelectorRequiresAnOptedLexeme()
    {
        var locale = ExactNumericSingletonInflectionFixture().Replace(
            """
            one:
              preferred: 'cat'
              accepted:
                - 'cat'
""",
            string.Empty,
            StringComparison.Ordinal);
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Contains(
            runResult.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains(
                    "at least one lexeme",
                    StringComparison.OrdinalIgnoreCase));
    }

    [Theory]
    [InlineData("one-preferred", "form 'one' must define preferred")]
    [InlineData("one-accepted", "form 'one' must define accepted")]
    [InlineData("other-preferred", "form 'other' must define preferred")]
    [InlineData("other-accepted", "form 'other' must define accepted")]
    [InlineData("sources", "lexeme 'zz.noun.cat' must define sources")]
    public void ExactNumericSingletonLexemeRequiresCompleteFormsAndSources(
        string mutation,
        string expected)
    {
        var locale = mutation switch
        {
            "one-preferred" => ExactNumericSingletonInflectionFixture().Replace(
                "              preferred: 'cat'\n              accepted:",
                "              accepted:",
                StringComparison.Ordinal),
            "one-accepted" => ExactNumericSingletonInflectionFixture().Replace(
                "              accepted:\n                - 'cat'\n            other:",
                "            other:",
                StringComparison.Ordinal),
            "other-preferred" => ExactNumericSingletonInflectionFixture().Replace(
                "              preferred: 'cats'\n              accepted:",
                "              accepted:",
                StringComparison.Ordinal),
            "other-accepted" => ExactNumericSingletonInflectionFixture().Replace(
                "              accepted:\n                - 'cats'\n        sources:",
                "        sources:",
                StringComparison.Ordinal),
            "sources" => ExactNumericSingletonInflectionFixture().Replace(
                "        sources:\n          - 'fixture'\n    sources:",
                "    sources:",
                StringComparison.Ordinal),
            _ => throw new ArgumentOutOfRangeException(nameof(mutation))
        };
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Contains(
            runResult.Diagnostics,
            diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains(expected, StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void UnreachableOneDisplayRequiresExactNumericSingletonSelector()
    {
        var locale = ExactNumericSingletonInflectionFixture().Replace(
            "    quantitySelector: 'exact-numeric-singleton'\n",
            string.Empty,
            StringComparison.Ordinal);
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Contains(
            runResult.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains(
                    "unreachable without quantitySelector",
                    StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void RegionalCardinalRuleCanReachOneWithoutQuantitySelector()
    {
        var locale = ExactNumericSingletonInflectionFixture()
            .Replace(
                "    quantitySelector: 'exact-numeric-singleton'\n",
                string.Empty,
                StringComparison.Ordinal)
            .Replace(
                "    accepted-cultures:",
                """
    regionalRules:
      zz-GB: 'EnglishLike'
    accepted-cultures:
""",
                StringComparison.Ordinal);
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Empty(runResult.Diagnostics);
        var owner = GetGeneratedSource(runResult, "GeneratedInflection_zz.g.cs");
        Assert.Contains(
            "InflectionQuantitySelector.None",
            owner,
            StringComparison.Ordinal);
    }

    [Fact]
    public void ExactNumericSingletonSelectorRejectsProductiveOneDisplay()
    {
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText(
                "src/Humanizer/Locales/zz.yml",
                ExactNumericSingletonProductiveFixture()));

        Assert.Contains(
            runResult.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains(
                    "cannot select productive rule",
                    StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void ExactNumericSingletonSelectorLeavesProductiveRulesOnOther()
    {
        var locale = ExactNumericSingletonProductiveFixture().Replace(
            """
          display:
            one: '{stem}y'
            other: '{stem}ies'
""",
            """
          display:
            other: '{stem}ies'
""",
            StringComparison.Ordinal);
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Empty(runResult.Diagnostics);
        var owner = GetGeneratedSource(runResult, "GeneratedInflection_zz.g.cs");
        Assert.Contains(
            "new(CardinalPluralCategory.Other, \"{stem}ies\")",
            owner,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "new(CardinalPluralCategory.One, \"{stem}y\")",
            owner,
            StringComparison.Ordinal);
    }

    [Fact]
    public void ExactNumericSingletonEmissionIsDeterministicAndWithinOwnerLimit()
    {
        var locale = ExactNumericSingletonInflectionFixture();
        var first = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));
        var second = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Empty(first.Diagnostics);
        Assert.Empty(second.Diagnostics);
        var firstOwner = GetGeneratedSource(first, "GeneratedInflection_zz.g.cs");
        var secondOwner = GetGeneratedSource(second, "GeneratedInflection_zz.g.cs");
        Assert.Equal(firstOwner, secondOwner);
        Assert.Contains(
            "InflectionQuantitySelector.ExactNumericSingleton",
            firstOwner,
            StringComparison.Ordinal);
        Assert.Contains("CardinalPluralCategory.One", firstOwner, StringComparison.Ordinal);
        Assert.Contains("CardinalPluralCategory.Other", firstOwner, StringComparison.Ordinal);
        Assert.True(Encoding.UTF8.GetByteCount(firstOwner) <= 256 * 1024);
    }

    static string CompleteInflectionFixtureFor(string localeCode, string script = "Latn")
    {
        var fixture = CompleteInflectionFixture(eligible: 100, irregular: 100, covered: 95)
            .Replace("zz", localeCode, StringComparison.Ordinal)
            .Replace("'Latn'", $"'{script}'", StringComparison.Ordinal);
        if (script == "Latn")
        {
            return fixture;
        }

        var (singular, plural) = script switch
        {
            "Arab" => ("ا", "ب"),
            "Armn" => ("ա", "բ"),
            "Beng" => ("ক", "খ"),
            "Cyrl" => ("а", "б"),
            "Deva" => ("क", "ख"),
            "Ethi" => ("ሀ", "ሁ"),
            "Geor" => ("ა", "ბ"),
            "Grek" => ("α", "β"),
            "Gujr" => ("ક", "ખ"),
            "Guru" => ("ਕ", "ਖ"),
            "Hani" => ("中", "文"),
            "Hebr" => ("א", "ב"),
            "Jpan" => ("あ", "い"),
            "Khmr" => ("ក", "ខ"),
            "Knda" => ("ಕ", "ಖ"),
            "Kore" => ("가", "나"),
            "Laoo" => ("ກ", "ຂ"),
            "Mlym" => ("ക", "ഖ"),
            "Mong" => ("ᠠ", "ᠡ"),
            "Mymr" => ("က", "ခ"),
            "Orya" => ("କ", "ଖ"),
            "Sinh" => ("ක", "ඛ"),
            "Taml" => ("க", "ங"),
            "Telu" => ("క", "ఖ"),
            "Thai" => ("ก", "ข"),
            _ => throw new ArgumentOutOfRangeException(nameof(script))
        };
        return fixture
            .Replace("'cat'", $"'{singular}'", StringComparison.Ordinal)
            .Replace("'cats'", $"'{plural}'", StringComparison.Ordinal);
    }

    static string InvariantInflectionFixtureFor(string localeCode) =>
        $"""
locale: '{localeCode}'
surfaces:
  durationCases:
    classification: 'not-applicable'
  inflection:
    cardinalRule: 'Other'
    capability: 'invariant'
    accepted-cultures:
      - '{localeCode}'
    scripts:
      - 'Latn'
    casing: 'lower-title-upper'
    phrase-mode: 'exact-only'
    lexemes: []
    sources:
      fixture:
        kind: 'grammar'
        locator: 'fixture'
    evidence:
      methodology: 'fixture-v1'
      pluralize:
        eligible: 100
        irregular: 0
        covered: 0
        sources:
          - 'fixture'
      singularize:
        eligible: 100
        irregular: 0
        covered: 0
        sources:
          - 'fixture'
""";

    static GeneratorDriverRunResult RunGeneratorIsolated(
        params AdditionalText[] additionalTexts)
    {
        var compilation = CreateCompilation();
        GeneratorDriver driver = CSharpGeneratorDriver.Create(
            [new HumanizerSourceGenerator().AsSourceGenerator()],
            additionalTexts,
            (CSharpParseOptions)compilation.SyntaxTrees.Single().Options);

        driver = driver.RunGenerators(compilation);
        return driver.GetRunResult();
    }

    static string InflectionFixtureForCategories(
        string cardinalRule,
        string[] categories)
    {
        var display = string.Join(
            "\n",
            categories.Select(static category =>
                $"            {category}:\n" +
                "              preferred: 'cat'\n" +
                "              accepted:\n" +
                "                - 'cat'"));
        return CompleteInflectionFixture(eligible: 100, irregular: 100, covered: 95)
            .Replace(
                "    cardinalRule: 'EnglishLike'",
                $"    cardinalRule: '{cardinalRule}'",
                StringComparison.Ordinal)
            .Replace(
                """
          display:
            one:
              preferred: 'cat'
              accepted:
                - 'cat'
            other:
              preferred: 'cats'
              accepted:
                - 'cats'
""",
                $"          display:\n{display}\n",
                StringComparison.Ordinal);
    }

    static string AliasInflectionFixture(
        string localeCode,
        string variantOf,
        string owner) =>
        $"""
locale: '{localeCode}'
variantOf: '{variantOf}'
surfaces:
  durationCases:
    classification: 'not-applicable'
  inflection:
    cardinalRule: 'EnglishLike'
    capability: 'alias'
    owner: '{owner}'
    accepted-cultures:
      - '{localeCode}'
""";

    static string ProductiveInflectionFixture() =>
        CompleteInflectionFixture(eligible: 100, irregular: 100, covered: 95).Replace(
            """
    sources:
      fixture:
""",
            """
    rules:
      - id: 'zz.forward.consonant-y'
        direction: 'forward'
        priority: 100
        scope:
          pos: 'noun'
          countability:
            - 'count'
          token: 'standalone'
          scripts:
            - 'Latn'
        match:
          suffix: 'y'
          preceding-not:
            - 'a'
            - 'e'
            - 'i'
            - 'o'
            - 'u'
        output:
          dictionary-plural: '{stem}ies'
          display:
            one: '{stem}y'
            other: '{stem}ies'
        hostile-exclusions:
          surfaces:
            - 'day'
        reverse:
          enabled: true
          requires-existing-lexeme: false
        sources:
          - 'fixture'
    sources:
      fixture:
""",
            StringComparison.Ordinal);

    static string ExactNumericSingletonInflectionFixture() =>
        CompleteInflectionFixture(eligible: 100, irregular: 100, covered: 95)
            .Replace(
                "    cardinalRule: 'EnglishLike'",
                "    cardinalRule: 'Other'",
                StringComparison.Ordinal)
            .Replace(
                "    capability: 'display-by-category'",
                """
    capability: 'display-by-category'
    quantitySelector: 'exact-numeric-singleton'
""",
                StringComparison.Ordinal);

    static string ExactNumericSingletonProductiveFixture() =>
        ProductiveInflectionFixture()
            .Replace(
                "    cardinalRule: 'EnglishLike'",
                "    cardinalRule: 'Other'",
                StringComparison.Ordinal)
            .Replace(
                "    capability: 'display-by-category'",
                """
    capability: 'display-by-category'
    quantitySelector: 'exact-numeric-singleton'
""",
                StringComparison.Ordinal);

    static string AddSecondProductiveRule(
        string fixture,
        string suffix,
        string dictionaryPlural) =>
        fixture.Replace(
            """
        sources:
          - 'fixture'
    sources:
""",
            $$"""
        sources:
          - 'fixture'
      - id: 'zz.forward.second'
        direction: 'forward'
        priority: 100
        scope:
          pos: 'noun'
          countability:
            - 'count'
          token: 'standalone'
          scripts:
            - 'Latn'
        match:
          suffix: '{{suffix}}'
        output:
          dictionary-plural: '{{dictionaryPlural}}'
          display:
            one: '{stem}{{suffix}}'
            other: '{{dictionaryPlural}}'
        sources:
          - 'fixture'
    sources:
""",
            StringComparison.Ordinal);

    static string ComparerEquivalentGreekInflectionFixture() =>
        CompleteInflectionFixtureFor("zz", "Grek")
            .Replace(
                "casing: 'exact'",
                "casing: 'lower-title-upper'",
                StringComparison.Ordinal)
            .Replace("'α'", "'σ'", StringComparison.Ordinal)
            .Replace(
                """
    sources:
      fixture:
""",
                """
      - id: 'zz.noun.final-sigma'
        pos: 'noun'
        countability: 'count'
        forms:
          singular:
            preferred: 'ς'
            accepted:
              - 'ς'
          dictionary-plural:
            preferred: 'γ'
            accepted:
              - 'γ'
          display:
            one:
              preferred: 'ς'
              accepted:
                - 'ς'
            other:
              preferred: 'γ'
              accepted:
                - 'γ'
        sources:
          - 'fixture'
    sources:
      fixture:
""",
                StringComparison.Ordinal);

    static string UnicodeComparerInflectionFixture() =>
        CompleteInflectionFixtureFor("zz")
            .Replace(
                "casing: 'exact'",
                "casing: 'lower-title-upper'",
                StringComparison.Ordinal)
            .Replace(
                "    scripts:\n      - 'Latn'",
                "    scripts:\n      - 'Latn'\n      - 'Grek'",
                StringComparison.Ordinal)
            .Replace("'cat'", "'\uFF5A'", StringComparison.Ordinal)
            .Replace("'cats'", "'\uFF5As'", StringComparison.Ordinal)
            .Replace(
                """
    sources:
      fixture:
""",
                $"""
      {UnicodeComparerLexeme("supplementary-latin", "\U00010780", "\U00010780s")}
      {UnicodeComparerLexeme("sharp-s", "\u00DF", "\u00DFs")}
      {UnicodeComparerLexeme("capital-sharp-s", "\u1E9E", "\u1E9Es")}
      {UnicodeComparerLexeme("theta", "\u03B8", "\u03B8\u03B8")}
      {UnicodeComparerLexeme("theta-symbol", "\u03F4", "\u03F4\u03F4")}
    sources:
      fixture:
""",
                StringComparison.Ordinal);

    static string UnicodeComparerLexeme(
        string id,
        string singular,
        string plural) =>
        $"""
- id: 'zz.noun.{id}'
        pos: 'noun'
        countability: 'count'
        forms:
          singular:
            preferred: '{singular}'
            accepted:
              - '{singular}'
          dictionary-plural:
            preferred: '{plural}'
            accepted:
              - '{plural}'
          display:
            one:
              preferred: '{singular}'
              accepted:
                - '{singular}'
            other:
              preferred: '{plural}'
              accepted:
                - '{plural}'
        sources:
          - 'fixture'
""";

    static string UnicodeCodePointString(int value) =>
        value <= char.MaxValue
            ? ((char)value).ToString()
            : char.ConvertFromUtf32(value);

    static string FormatCodePoints(string value) =>
        string.Join(
            " ",
            value.Select(static character => $"U+{(int)character:X4}"));

    static string OverlappingPrefixInflectionFixture() =>
        CompleteInflectionFixture(eligible: 100, irregular: 100, covered: 95)
            .Replace(
                """
    sources:
      fixture:
""",
                """
    rules:
      - id: 'zz.forward.a-short'
        direction: 'forward'
        priority: 100
        scope:
          pos: 'noun'
          countability:
            - 'count'
          token: 'standalone'
          scripts:
            - 'Latn'
        match:
          prefix: 'a'
        output:
          dictionary-plural: '{stem}s'
          display:
            one: '{stem}s'
            other: '{stem}s'
        sources:
          - 'fixture'
      - id: 'zz.forward.z-long'
        direction: 'forward'
        priority: 100
        scope:
          pos: 'noun'
          countability:
            - 'count'
          token: 'standalone'
          scripts:
            - 'Latn'
        match:
          prefix: 'ab'
        output:
          dictionary-plural: '{stem}z'
          display:
            one: '{stem}z'
            other: '{stem}z'
        sources:
          - 'fixture'
    sources:
      fixture:
""",
                StringComparison.Ordinal);

    static string OverlappingCrossKindInflectionFixture() =>
        OverlappingPrefixInflectionFixture()
            .Replace(
                "zz.forward.a-short",
                "zz.forward.a-long-prefix",
                StringComparison.Ordinal)
            .Replace(
                "          prefix: 'a'\n",
                "          prefix: 'abc'\n",
                StringComparison.Ordinal)
            .Replace(
                "zz.forward.z-long",
                "zz.forward.z-short-suffix",
                StringComparison.Ordinal)
            .Replace(
                "          prefix: 'ab'\n",
                "          suffix: 'c'\n",
                StringComparison.Ordinal);

    static string CompleteInflectionFixture(int eligible, int irregular, int covered) =>
        $"""
locale: 'zz'
surfaces:
  durationCases:
    classification: 'not-applicable'
  inflection:
    cardinalRule: 'EnglishLike'
    capability: 'display-by-category'
    accepted-cultures:
      - 'zz'
    scripts:
      - 'Latn'
    casing: 'exact'
    phrase-mode: 'exact-only'
    lexemes:
      - id: 'zz.noun.cat'
        pos: 'noun'
        countability: 'count'
        forms:
          singular:
            preferred: 'cat'
            accepted:
              - 'cat'
          dictionary-plural:
            preferred: 'cats'
            accepted:
              - 'cats'
          display:
            one:
              preferred: 'cat'
              accepted:
                - 'cat'
            other:
              preferred: 'cats'
              accepted:
                - 'cats'
        sources:
          - 'fixture'
    sources:
      fixture:
        kind: 'project-history'
        locator: 'fixture'
    evidence:
      methodology: 'fixture-v1'
      pluralize:
        eligible: {eligible}
        irregular: {irregular}
        covered: {covered}
        attempted: 100
        correct: 99
        sources:
          - 'fixture'
      singularize:
        eligible: {eligible}
        irregular: {irregular}
        covered: {covered}
        attempted: 100
        correct: 99
        sources:
          - 'fixture'
""";

    public static TheoryData<string, decimal[]> ReachableCategoryCases
    {
        get
        {
            var witnesses = Enumerable.Range(0, 201)
                .Select(static value => (decimal)value)
                .Concat(Enumerable.Range(0, 201).Select(static value => value / 10m))
                .Append(1_000_000m)
                .Append(2_000_000m)
                .Distinct()
                .ToArray();
            var kindType = typeof(Configurator).Assembly.GetTypes().Single(
                static type => type.Name == "CardinalPluralRuleKind");
            var result = new TheoryData<string, decimal[]>();
            foreach (var cardinalRule in Enum.GetNames(kindType))
            {
                result.Add(cardinalRule, witnesses);
            }

            return result;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

using Humanizer.SourceGenerators;

using Xunit;

namespace Humanizer.SourceGenerators.Tests;

public class CanonicalLocaleSchemaTests
{
    [Fact]
    public void CanonicalSchemaRequiresLocaleAndSurfacesAndRejectsLegacyTopLevelKeys()
    {
        var missingLocale = CreateCatalog(
            ("zz", """
surfaces:
  number:
    words:
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
        0: 'zero'
      ordinalUnitsMap:
        0: 'zeroth'
      tensMap:
        0: 'zero'
      ordinalTensMap:
        0: 'zeroth'
      scales: []
"""));

        var missingSurfaces = CreateCatalog(
            ("zz", "locale: 'zz'"));

        var legacyTopLevel = CreateCatalog(
            ("zz", """
locale: 'zz'
numberToWords:
  engine: 'conjunctional-scale'
  minusWord: 'minus'
"""));

        Assert.Contains(
            missingLocale.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains("required top-level property 'locale'", StringComparison.Ordinal));

        Assert.Contains(
            missingSurfaces.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains("required top-level property 'surfaces'", StringComparison.Ordinal));

        Assert.Contains(
            legacyTopLevel.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains("unsupported top-level property 'numberToWords'", StringComparison.Ordinal));
    }

    [Fact]
    public void CanonicalSurfacesMaterializeIntoResolvedLocaleFeatures()
    {
        var catalog = CreateCatalog(
            ("zz-base", """
locale: 'zz-base'
surfaces:
  list:
    engine: 'conjunction'
    value: 'and'
  formatter:
    engine: 'profiled'
    pluralRule: 'russian'
    dataUnitPluralRule: 'russian'
    timeUnitGenders:
      hour: 'feminine'
  phrases:
    relativeDate:
      now: 'now'
      past:
        day:
          single: 'yesterday'
  number:
    words:
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
        0: 'zero'
        1: 'one'
      ordinalUnitsMap:
        0: 'zeroth'
        1: 'first'
      tensMap:
        0: 'zero'
        1: 'ten'
      ordinalTensMap:
        0: 'zeroth'
        1: 'tenth'
      scales:
        -
          value: 1000
          name: 'thousand'
          ordinalName: 'thousandth'
    parse:
      engine: 'token-map'
      normalizationProfile: 'LowercaseRemovePeriods'
      cardinalMap:
        one: 1
      ordinalMap:
        first: 1
  ordinal:
    numeric:
      engine: 'template'
      masculine:
        defaultSuffix: 'th'
    date:
      pattern: '{day} MMMM yyyy'
      dayMode: 'Ordinal'
    dateOnly:
      pattern: '{day} MMMM yyyy'
      dayMode: 'OrdinalWhenDayIsOne'
  clock:
    engine: 'french'
  compass:
    full:
      - 'north'
      - 'north-northeast'
      - 'northeast'
      - 'east-northeast'
      - 'east'
      - 'east-southeast'
      - 'southeast'
      - 'south-southeast'
      - 'south'
      - 'south-southwest'
      - 'southwest'
      - 'west-southwest'
      - 'west'
      - 'west-northwest'
      - 'northwest'
      - 'north-northwest'
    short:
      - 'N'
      - 'NNE'
      - 'NE'
      - 'ENE'
      - 'E'
      - 'ESE'
      - 'SE'
      - 'SSE'
      - 'S'
      - 'SSW'
      - 'SW'
      - 'WSW'
      - 'W'
      - 'WNW'
      - 'NW'
      - 'NNW'
"""),
            ("zz-child", """
locale: 'zz-child'
variantOf: 'zz-base'
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
"""));

        Assert.Empty(catalog.Diagnostics);

        var baseLocale = catalog.Locales.Single(static locale => locale.LocaleCode == "zz-base");
        var childLocale = catalog.Locales.Single(static locale => locale.LocaleCode == "zz-child");

        Assert.Equal("conjunction", baseLocale.CollectionFormatter!.Kind);
        Assert.Equal("and", baseLocale.CollectionFormatter.Argument);
        Assert.Equal("russian", baseLocale.Grammar!.GetScalar("pluralRule"));
        Assert.Equal("russian", GetRequiredString(baseLocale.Formatter!, "pluralRule"));
        Assert.Equal("profiled", GetRequiredString(baseLocale.Formatter!, "engine"));
        Assert.Equal("conjunctional-scale", GetRequiredString(baseLocale.NumberToWords!, "engine"));
        Assert.Equal("token-map", GetRequiredString(baseLocale.WordsToNumber!, "engine"));
        Assert.Equal("template", GetRequiredString(baseLocale.Ordinalizer!, "engine"));
        Assert.Equal("{day} MMMM yyyy", GetRequiredString(baseLocale.DateToOrdinalWords!, "pattern"));
        Assert.Equal("{day} MMMM yyyy", GetRequiredString(baseLocale.DateOnlyToOrdinalWords!, "pattern"));
        Assert.Equal("french", GetRequiredString(baseLocale.TimeOnlyToClockNotation!, "engine"));
        Assert.Equal("north", baseLocale.Headings!.Full[0]);
        Assert.Equal("yesterday", baseLocale.Phrases!.DateHumanize.Past["day"].Single);

        Assert.Equal("variant-decade", GetRequiredString(childLocale.NumberToWords!, "engine"));
        Assert.Equal("token-map", GetRequiredString(childLocale.WordsToNumber!, "engine"));
        Assert.Equal("north", childLocale.Headings!.Full[0]);
    }

    [Fact]
    public void LegacySchemaMigrationProducesCanonicalRoundTrippableYaml()
    {
        const string legacyLocale = """
inherits: 'fr'
collectionFormatter:
  engine: 'conjunction'
  value: 'et'
formatter:
  engine: 'profiled'
  dataUnitFallbackTransform: 'trim-trailing-s'
grammar:
  pluralRule: 'french'
phrases:
  dateHumanize:
    now: 'maintenant'
numberToWords:
  engine: 'variant-decade'
  minusWord: 'moins'
  seventyStrategy: 'regular'
  ninetyStrategy: 'regular'
  pluralizeExactEighty: false
  tensUsingEtWhenUnitIsOne:
    - 2
  tensMap:
    - 'zéro'
    - 'dix'
    - 'vingt'
    - 'trente'
    - 'quarante'
    - 'cinquante'
    - 'soixante'
    - 'septante'
    - 'octante'
    - 'nonante'
ordinalizer:
  engine: 'template'
  masculine:
    defaultSuffix: 'ème'
dateToOrdinalWords:
  pattern: '{day} MMMM yyyy'
  dayMode: 'OrdinalWhenDayIsOne'
dateOnlyToOrdinalWords:
  pattern: '{day} MMMM yyyy'
  dayMode: 'OrdinalWhenDayIsOne'
timeOnlyToClockNotation:
  engine: 'french'
headings:
  full:
    - 'north'
    - 'north-northeast'
    - 'northeast'
    - 'east-northeast'
    - 'east'
    - 'east-southeast'
    - 'southeast'
    - 'south-southeast'
    - 'south'
    - 'south-southwest'
    - 'southwest'
    - 'west-southwest'
    - 'west'
    - 'west-northwest'
    - 'northwest'
    - 'north-northwest'
  short:
    - 'N'
    - 'NNE'
    - 'NE'
    - 'ENE'
    - 'E'
    - 'ESE'
    - 'SE'
    - 'SSE'
    - 'S'
    - 'SSW'
    - 'SW'
    - 'WSW'
    - 'W'
    - 'WNW'
    - 'NW'
    - 'NNW'
""";

        var canonicalYaml = HumanizerSourceGenerator.LegacyLocaleMigration.ConvertToCanonicalYaml("fr-CH", legacyLocale);
        var canonicalDocument = HumanizerSourceGenerator.CanonicalLocaleAuthoring.Parse("fr-CH", canonicalYaml);
        var roundTrippedYaml = HumanizerSourceGenerator.CanonicalLocaleAuthoring.Emit(canonicalDocument);

        Assert.Contains("locale: 'fr-CH'", canonicalYaml, StringComparison.Ordinal);
        Assert.Contains("variantOf: 'fr'", canonicalYaml, StringComparison.Ordinal);
        Assert.Contains("surfaces:", canonicalYaml, StringComparison.Ordinal);
        Assert.Contains("number:", canonicalYaml, StringComparison.Ordinal);
        Assert.Contains("words:", canonicalYaml, StringComparison.Ordinal);
        Assert.Contains("ordinal:", canonicalYaml, StringComparison.Ordinal);
        Assert.Equal(NormalizeNewlines(canonicalYaml), NormalizeNewlines(roundTrippedYaml));
    }

    [Fact]
    public void SemanticDiffIgnoresEquivalentCanonicalStructureButDetectsBehaviorChanges()
    {
        const string left = """
locale: 'zz'
surfaces:
  phrases:
    dataUnits:
      byte:
        forms:
          default: 'bytes'
          singular: 'byte'
          plural: 'bytes'
  number:
    parse:
      engine: 'token-map'
      normalizationProfile: 'LowercaseRemovePeriods'
      cardinalMap:
        one: 1
""";

        const string equivalent = """
locale: 'zz'
surfaces:
  phrases:
    dataUnits:
      byte:
        forms:
          default: 'bytes'
          singular: 'byte'
  number:
    parse:
      engine: 'token-map'
      normalizationProfile: 'LowercaseRemovePeriods'
      cardinalMap:
        one: 1
      allowInvariantIntegerInput: false
""";

        const string changed = """
locale: 'zz'
surfaces:
  phrases:
    dataUnits:
      byte:
        forms:
          default: 'octets'
          singular: 'octet'
  number:
    parse:
      engine: 'token-map'
      normalizationProfile: 'LowercaseRemovePeriods'
      cardinalMap:
        one: 1
""";

        var leftCatalog = CreateCatalog(("zz", left));
        var equivalentCatalog = CreateCatalog(("zz", equivalent));
        var changedCatalog = CreateCatalog(("zz", changed));

        Assert.Empty(leftCatalog.Diagnostics);
        Assert.Empty(equivalentCatalog.Diagnostics);
        Assert.Empty(changedCatalog.Diagnostics);

        Assert.Empty(HumanizerSourceGenerator.LocaleSemanticDiff.Compare(leftCatalog.Locales, equivalentCatalog.Locales));

        var differences = HumanizerSourceGenerator.LocaleSemanticDiff.Compare(leftCatalog.Locales, changedCatalog.Locales);

        Assert.NotEmpty(differences);
        Assert.Contains(differences, static difference => difference.Contains("zz", StringComparison.Ordinal));
    }

    [Fact]
    public void CheckedInCanonicalLocaleFilesRoundTripWithoutStructuralDrift()
    {
        foreach (var file in FindCheckedInLocaleFiles())
        {
            var localeCode = Path.GetFileNameWithoutExtension(file);
            var fileText = File.ReadAllText(file);
            var document = HumanizerSourceGenerator.CanonicalLocaleAuthoring.Parse(localeCode, fileText);
            var emitted = HumanizerSourceGenerator.CanonicalLocaleAuthoring.Emit(document);

            Assert.Equal(NormalizeNewlines(fileText), NormalizeNewlines(emitted));
        }
    }

    static string GetRequiredString(HumanizerSourceGenerator.LocaleFeature feature, string propertyName) =>
        feature.ProfileRoot.GetProperty(propertyName).GetString()!;

    static HumanizerSourceGenerator.LocaleCatalogInput CreateCatalog(params (string LocaleCode, string FileText)[] files) =>
        HumanizerSourceGenerator.LocaleCatalogInput.Create(ImmutableArray.CreateRange(
            files.Select(static file => (HumanizerSourceGenerator.LocaleDefinitionFile?)new HumanizerSourceGenerator.LocaleDefinitionFile(file.LocaleCode, file.FileText))));

    static string[] FindCheckedInLocaleFiles()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            var localeRoot = Path.Combine(directory.FullName, "src", "Humanizer", "Locales");
            if (Directory.Exists(localeRoot))
            {
                return Directory
                    .GetFiles(localeRoot, "*.yml", SearchOption.TopDirectoryOnly)
                    .OrderBy(static path => path, StringComparer.OrdinalIgnoreCase)
                    .ToArray();
            }

            directory = directory.Parent;
        }

        throw new Xunit.Sdk.XunitException("Could not locate the checked-in locale files.");
    }

    static string NormalizeNewlines(string value) =>
        value.Replace("\r\n", "\n", StringComparison.Ordinal);
}

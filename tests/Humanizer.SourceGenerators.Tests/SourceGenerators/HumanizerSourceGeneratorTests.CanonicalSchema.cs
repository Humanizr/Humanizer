using Xunit;

namespace Humanizer.SourceGenerators.Tests;

public partial class HumanizerSourceGeneratorTests
{
    [Fact]
    public void CanonicalLocaleSchemaAcceptsLocaleVariantOfAndSurfaces()
    {
        const string parentLocale = """
locale: 'zz-parent'
surfaces:
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
        Assert.Contains("\"zz-parent\" => zz_parent", phraseTableSource, StringComparison.Ordinal);
        Assert.Contains("case \"zz-child\": return", numberToWordsSource, StringComparison.Ordinal);
        Assert.Contains("new Dictionary<string, long>(StringComparer.Ordinal)", wordsToNumberSource, StringComparison.Ordinal);
        Assert.Contains("[\"huge\"] = 2147483648", parentTokenMapSource, StringComparison.Ordinal);
        Assert.Contains("[\"huge\"] = 2147483648", childTokenMapSource, StringComparison.Ordinal);
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
}
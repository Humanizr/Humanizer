using System.Collections.Immutable;

using Xunit;

namespace Humanizer.SourceGenerators.Tests;

public class LocalePhraseNormalizationTests
{
    [Fact]
    public void ScalarFormsCollapseToDefaultOnly()
    {
        var catalog = HumanizerSourceGenerator.LocalePhraseNormalization.ParseLocalePhraseCatalogForTests(
            "zz",
            """
            phrases:
              dataUnits:
                byte:
                  forms: 'byte'
            """);

        var phrase = catalog.DataUnit.Units["byte"];

        Assert.Equal("byte", phrase.Forms!.Default);
        Assert.Null(phrase.Forms.Singular);
        Assert.Null(phrase.Forms.Dual);
        Assert.Null(phrase.Forms.Paucal);
        Assert.Null(phrase.Forms.Plural);
    }

    [Fact]
    public void DuplicateFormsCollapseToDistinctCanonicalValues()
    {
        var catalog = HumanizerSourceGenerator.LocalePhraseNormalization.ParseLocalePhraseCatalogForTests(
            "zz",
            """
            phrases:
              dataUnits:
                day:
                  forms:
                    default: 'days'
                    singular: 'day'
                    dual: 'days'
                    paucal: 'days'
                    plural: 'days'
            """);

        var phrase = catalog.DataUnit.Units["day"];

        Assert.Equal("days", phrase.Forms!.Default);
        Assert.Equal("day", phrase.Forms.Singular);
        Assert.Null(phrase.Forms.Dual);
        Assert.Null(phrase.Forms.Paucal);
        Assert.Null(phrase.Forms.Plural);
    }

    [Fact]
    public void BeforeAndAfterCountTextAreNormalized()
    {
        var catalog = HumanizerSourceGenerator.LocalePhraseNormalization.ParseLocalePhraseCatalogForTests(
            "zz",
            """
            phrases:
              relativeDate:
                future:
                  day:
                    multiple:
                      beforeCount: 'in'
                      forms:
                        singular: 'day'
                        default: 'days'
                past:
                  day:
                    multiple:
                      afterCount: 'ago'
                      forms:
                        singular: 'day'
                        default: 'days'
            """);

        var future = catalog.DateHumanize.Future["day"].Multiple!;
        var past = catalog.DateHumanize.Past["day"].Multiple!;

        Assert.Equal("in", future.BeforeCountText);
        Assert.Null(future.AfterCountText);
        Assert.Equal("ago", past.AfterCountText);
        Assert.Null(past.BeforeCountText);
    }

    [Fact]
    public void NamedTemplatesPassThroughWithNamedPlaceholders()
    {
        var catalog = HumanizerSourceGenerator.LocalePhraseNormalization.ParseLocalePhraseCatalogForTests(
            "zz",
            """
            phrases:
              duration:
                day:
                  multiple:
                    forms: 'days'
                    template: '{count} {unit}'
            """);

        var template = catalog.TimeSpan.Units["day"].Multiple!.NamedTemplate!;

        Assert.Equal("{count} {unit}", template.Template);
        Assert.Equal(["count", "unit"], template.PlaceholderNames);
    }

    [Fact]
    public void CountPlacementNoneAllowsNamedPrepPlaceholdersInForms()
    {
        var catalog = HumanizerSourceGenerator.LocalePhraseNormalization.ParseLocalePhraseCatalogForTests(
            "zz",
            """
            phrases:
              relativeDate:
                past:
                  day:
                    multiple:
                      countPlacement: none
                      forms:
                        default: 'ago {count}{prep} days'
            """);

        var phrase = catalog.DateHumanize.Past["day"].Multiple!;

        Assert.Equal(HumanizerSourceGenerator.CountPlacement.None, phrase.CountPlacement);
        Assert.Equal("ago {count}{prep} days", phrase.Forms!.Default);
    }

    [Fact]
    public void CountedTemplatesRequireForms()
    {
        var exception = Assert.Throws<InvalidOperationException>(() =>
            HumanizerSourceGenerator.LocalePhraseNormalization.ParseLocalePhraseCatalogForTests(
                "zz",
                """
                phrases:
                  duration:
                    day:
                      multiple:
                        template: '{count} {unit}'
                """));

        Assert.Contains("must define forms", exception.Message);
    }

    [Fact]
    public void NamedTemplateObjectsCanPreserveExactTemplateNames()
    {
        var catalog = HumanizerSourceGenerator.LocalePhraseNormalization.ParseLocalePhraseCatalogForTests(
            "zz",
            """
            phrases:
              relativeDate:
                past:
                  day:
                    template:
                      name: 'two'
                      value: 'two days ago'
            """);

        var template = catalog.DateHumanize.Past["day"].NamedTemplate!;

        Assert.Equal("two", template.Name);
        Assert.Equal("two days ago", template.Template);
        Assert.Empty(template.PlaceholderNames);
    }

    [Fact]
    public void TimeSpanAgeCanUseExplicitTemplateEscapeHatch()
    {
        var catalog = HumanizerSourceGenerator.LocalePhraseNormalization.ParseLocalePhraseCatalogForTests(
            "zz",
            """
            phrases:
              duration:
                age:
                  template: '{value} old'
            """);

        Assert.Equal("{value} old", catalog.TimeSpan.Age);
    }

    [Fact]
    public void UnsupportedTemplatePlaceholdersAreRejected()
    {
        var countedException = Assert.Throws<InvalidOperationException>(() =>
            HumanizerSourceGenerator.LocalePhraseNormalization.ParseLocalePhraseCatalogForTests(
                "zz",
                """
                phrases:
                  duration:
                    day:
                      multiple:
                        forms: 'days'
                        template: '{foo} {unit}'
                """));

        Assert.Contains("{count}", countedException.Message);

        var ageException = Assert.Throws<InvalidOperationException>(() =>
            HumanizerSourceGenerator.LocalePhraseNormalization.ParseLocalePhraseCatalogForTests(
                "zz",
                """
                phrases:
                  duration:
                    age:
                      template: '{unit} old'
                """));

        Assert.Contains("{value}", ageException.Message);
    }

    [Fact]
    public void NumericPlaceholdersAreRejectedInsidePhraseSections()
    {
        var exception = Assert.Throws<InvalidOperationException>(() =>
            HumanizerSourceGenerator.LocalePhraseNormalization.ParseLocalePhraseCatalogForTests(
                "zz",
                """
                phrases:
                  relativeDate:
                    past:
                      day:
                        single: '{0} day ago'
                """));

        Assert.Contains("numeric placeholder", exception.Message);
    }

    [Fact]
    public void UnknownPhraseKeysAreRejected()
    {
        var exception = Assert.Throws<InvalidOperationException>(() =>
            HumanizerSourceGenerator.LocalePhraseNormalization.ParseLocalePhraseCatalogForTests(
                "zz",
                """
                phrases:
                  duration:
                    day:
                      singluar: 'day'
                """));

        Assert.Contains("unsupported property 'singluar'", exception.Message);
    }

    [Fact]
    public void StructurallyEmptyPhraseMappingsAreRejected()
    {
        var exception = Assert.Throws<InvalidOperationException>(() =>
            HumanizerSourceGenerator.LocalePhraseNormalization.ParseLocalePhraseCatalogForTests(
                "zz",
                """
                phrases:
                  duration:
                    day:
                      single: {}
                """));

        Assert.Contains("must define 'numeric', 'text', or 'words'", exception.Message);
    }

    [Fact]
    public void LocaleCatalogInputMergesInheritedPhraseMappings()
    {
        var catalog = CreateCatalog(
            ("zz", """
phrases:
  relativeDate:
    past:
      day:
        multiple:
          afterCount: 'ago'
          forms:
            singular: 'day'
            default: 'days'
  dataUnits:
    byte:
      forms:
        singular: 'byte'
        default: 'bytes'
"""),
            ("zz-child", """
inherits: 'zz'

phrases:
  relativeDate:
    past:
      day:
        single: 'yesterday'
  dataUnits:
    byte:
      symbol: 'B'
"""));

        Assert.Empty(catalog.Diagnostics);

        var locale = catalog.Locales.Single(static locale => locale.LocaleCode == "zz-child");
        var dayPhrase = locale.Phrases!.DateHumanize.Past["day"];
        var bytePhrase = locale.Phrases.DataUnit.Units["byte"];

        Assert.Equal("yesterday", dayPhrase.Single);
        Assert.Equal("ago", dayPhrase.Multiple!.AfterCountText);
        Assert.Equal("day", dayPhrase.Multiple.Forms!.Singular);
        Assert.Equal("days", dayPhrase.Multiple.Forms.Default);
        Assert.Equal("byte", bytePhrase.Forms!.Singular);
        Assert.Equal("bytes", bytePhrase.Forms.Default);
        Assert.Equal("B", bytePhrase.Symbol);
    }

    [Fact]
    public void LocaleCatalogInputAllowsExplicitNullToClearInheritedOptionalPhraseLeaves()
    {
        var catalog = CreateCatalog(
            ("zz", """
phrases:
  relativeDate:
    past:
      day:
        multiple:
          beforeCount: 'há'
          forms:
            singular: 'dia'
            default: 'dias'
"""),
            ("zz-child", """
inherits: 'zz'

phrases:
  relativeDate:
    past:
      day:
        multiple:
          beforeCount: null
          afterCount: 'atrás'
"""));

        Assert.Empty(catalog.Diagnostics);

        var locale = catalog.Locales.Single(static locale => locale.LocaleCode == "zz-child");
        var dayPhrase = locale.Phrases!.DateHumanize.Past["day"].Multiple!;

        Assert.Null(dayPhrase.BeforeCountText);
        Assert.Equal("atrás", dayPhrase.AfterCountText);
        Assert.Equal("dia", dayPhrase.Forms!.Singular);
        Assert.Equal("dias", dayPhrase.Forms.Default);
    }

    [Fact]
    public void LocaleCatalogInputReportsMalformedGrammarAndPhraseDataAsDiagnostics()
    {
        var catalog = CreateCatalog(
            ("zz", """
locale: 'zz'
surfaces:
  formatter:
    engine: 'profiled'
    pluralRule: 'broken'
  phrases:
    duration:
      day:
        singluar: 'day'
"""));

        var messages = catalog.Diagnostics
            .Where(static diagnostic => diagnostic.Id == "HSG003")
            .Select(static diagnostic => diagnostic.GetMessage())
            .ToArray();

        Assert.Contains(messages, static message => message.Contains("unsupported property 'singluar'", StringComparison.Ordinal));
    }

    [Fact]
    public void LocaleCatalogInputReportsInheritanceCyclesAsDiagnostics()
    {
        var catalog = CreateCatalog(
            ("zz-a", """
locale: 'zz-a'
variantOf: 'zz-b'
surfaces: {}
"""),
            ("zz-b", """
locale: 'zz-b'
variantOf: 'zz-a'
surfaces: {}
"""));

        Assert.Contains(
            catalog.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains("inheritance cycle", StringComparison.Ordinal));
    }

    [Fact]
    public void CheckedInEnglishLocaleOwnsRepresentativePhraseData()
    {
        var catalog = CreateCatalog(("en", GetLocaleFile("en")));

        Assert.Empty(catalog.Diagnostics);

        var locale = Assert.Single(catalog.Locales);
        var phrases = Assert.IsType<HumanizerSourceGenerator.LocalePhraseCatalog>(locale.Phrases);

        Assert.Equal("now", phrases.DateHumanize.Now);
        Assert.Equal("yesterday", phrases.DateHumanize.Past["day"].Single);
        Assert.Null(phrases.DateHumanize.Past["day"].NamedTemplate);
        Assert.Equal("{value} old", phrases.TimeSpan.Age);
        Assert.Equal("one day", phrases.TimeSpan.Units["day"].SingleWordsVariant);
        Assert.Equal("B", phrases.DataUnit.Units["byte"].Symbol);
        Assert.Equal("d", phrases.TimeUnit.Units["day"].Symbol);
    }

    [Fact]
    public void CheckedInJapaneseLocaleOwnsRepresentativePhraseData()
    {
        var catalog = CreateCatalog(("ja", GetLocaleFile("ja")));

        Assert.Empty(catalog.Diagnostics);

        var locale = Assert.Single(catalog.Locales);
        var phrases = Assert.IsType<HumanizerSourceGenerator.LocalePhraseCatalog>(locale.Phrases);

        Assert.Equal("今", phrases.DateHumanize.Now);
        Assert.Equal("昨日", phrases.DateHumanize.Past["day"].Single);
        Assert.Equal("two", phrases.DateHumanize.Past["day"].NamedTemplate?.Name);
        Assert.Equal("2 日前", phrases.DateHumanize.Past["day"].NamedTemplate?.Template);
        Assert.Equal("{value}", phrases.TimeSpan.Age);
        Assert.Equal("1 日", phrases.TimeSpan.Units["day"].SingleWordsVariant);
        Assert.Equal("B", phrases.DataUnit.Units["byte"].Symbol);
        Assert.Equal("日", phrases.TimeUnit.Units["day"].Symbol);
    }

    static HumanizerSourceGenerator.LocaleCatalogInput CreateCatalog(params (string LocaleCode, string FileText)[] files) =>
        HumanizerSourceGenerator.LocaleCatalogInput.Create(ImmutableArray.CreateRange(
            files.Select(static file =>
            {
                var fileText = file.FileText.Contains("locale:", StringComparison.Ordinal)
                    ? file.FileText
                    : HumanizerSourceGenerator.LegacyLocaleMigration.ConvertToCanonicalYaml(file.LocaleCode, file.FileText);
                return (HumanizerSourceGenerator.LocaleDefinitionFile?)new HumanizerSourceGenerator.LocaleDefinitionFile(file.LocaleCode, fileText);
            })));

    static string GetLocaleFile(string localeCode)
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            var localePath = Path.Combine(directory.FullName, "src", "Humanizer", "Locales", $"{localeCode}.yml");
            if (File.Exists(localePath))
            {
                return File.ReadAllText(localePath);
            }

            directory = directory.Parent;
        }

        throw new Xunit.Sdk.XunitException($"Could not locate the checked-in locale file for '{localeCode}'.");
    }
}
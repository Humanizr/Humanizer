using System.Collections.Immutable;

using Xunit;

namespace Humanizer.SourceGenerators.Tests;

public class LocalePhraseNormalizationTests
{
    [Fact]
    public void RelativeDateTodayIsNormalized()
    {
        var catalog = HumanizerSourceGenerator.LocalePhraseNormalization.ParseLocalePhraseCatalogForTests(
            "zz",
            """
            phrases:
              relativeDate:
                today: 'today'
            """);

        Assert.Equal("today", catalog.DateHumanize.Today);
    }

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
    public void ExplicitCaseContractPreservesDuplicateDurationForms()
    {
        const string phrases = """
            phrases:
              duration:
                day:
                  multiple:
                    forms:
                      default: 'days'
                      singular: 'day'
                      paucal: 'days'
            """;

        var ordinary = HumanizerSourceGenerator.LocalePhraseNormalization.ParseLocalePhraseCatalogForTests(
            "zz",
            phrases);
        var caseAware = HumanizerSourceGenerator.LocalePhraseNormalization.ParseLocalePhraseCatalogForTests(
            "zz",
            phrases,
            preserveDurationCaseForms: true);

        Assert.Null(ordinary.TimeSpan.Units["day"].Multiple!.Forms!.Paucal);
        Assert.Equal("days", caseAware.TimeSpan.Units["day"].Multiple!.Forms!.Paucal);
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
    public void AuthoredDurationClassificationReplacesInheritedDistinctCases()
    {
        var catalog = CreateCatalog(
            ("zz", """
durationCases:
  classification: distinct
  cases:
    dative:
      units:
        millisecond:
          sameAsNominative: true
        second:
          sameAsNominative: true
        minute:
          sameAsNominative: true
        hour:
          sameAsNominative: true
        day:
          sameAsNominative: true
        week:
          sameAsNominative: true
        month:
          sameAsNominative: true
        year:
          sameAsNominative: true
"""),
            ("zz-child", """
inherits: 'zz'

durationCases:
  classification: unsupported
"""));

        Assert.Empty(catalog.Diagnostics);

        var child = catalog.Locales.Single(static locale => locale.LocaleCode == "zz-child");
        var durationCases = HumanizerSourceGenerator.DurationCaseNormalization.ParseLegacyForTests(
            child.LocaleCode,
            child.ResolvedFeatures["durationCases"]);

        Assert.Equal(HumanizerSourceGenerator.DurationCaseClassification.Unsupported, durationCases.Classification);
        Assert.Empty(durationCases.Cases);
    }

    [Fact]
    public void AuthoredDurationUnitKindsReplaceInheritedPhrases()
    {
        var catalog = CreateCatalog(
            ("zz", """
durationCases:
  classification: distinct
  cases:
    dative:
      units:
        millisecond:
          sameAsNominative: true
        second:
          sameAsNominative: true
        minute:
          sameAsNominative: true
        hour:
          single:
            numeric: '1 parent-hour'
          multiple:
            forms: 'parent-hours'
        day:
          single:
            numeric: '1 parent-day'
          multiple:
            forms: 'parent-days'
        week:
          sameAsNominative: true
        month:
          sameAsNominative: true
        year:
          sameAsNominative: true
"""),
            ("zz-child", """
inherits: 'zz'

durationCases:
  classification: distinct
  cases:
    dative:
      units:
        millisecond:
          sameAsNominative: true
        second:
          sameAsNominative: true
        minute:
          sameAsNominative: true
        hour:
          sameAsNominative: true
        day:
          unsupported: true
        week:
          sameAsNominative: true
        month:
          sameAsNominative: true
        year:
          sameAsNominative: true
"""));

        Assert.Empty(catalog.Diagnostics);

        var child = catalog.Locales.Single(static locale => locale.LocaleCode == "zz-child");
        var durationCases = HumanizerSourceGenerator.DurationCaseNormalization.ParseLegacyForTests(
            child.LocaleCode,
            child.ResolvedFeatures["durationCases"]);
        var dative = durationCases.Cases["dative"];

        Assert.Equal(HumanizerSourceGenerator.DurationCaseUnitKind.SameAsNominative, dative.Units["hour"].Kind);
        Assert.Equal(HumanizerSourceGenerator.DurationCaseUnitKind.Unsupported, dative.Units["day"].Kind);
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
    public void DurationCasesRejectCrossScriptInheritance()
    {
        var catalog = CreateCatalog(
            ("uz", """
            locale: 'uz'
            surfaces:
              durationCases:
                classification: 'unsupported'
            """),
            ("uz-Cyrl-UZ", """
            locale: 'uz-Cyrl-UZ'
            variantOf: 'uz'
            surfaces: {}
            """));

        Assert.Contains(
            catalog.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains("script-incompatible", StringComparison.Ordinal));
    }

    [Fact]
    public void DurationCasesAllowVettedScriptlessVariantToInheritExplicitScriptParent()
    {
        var catalog = CreateCatalog(
            ("zh-Hans", """
            locale: 'zh-Hans'
            surfaces:
              durationCases:
                classification: 'not-applicable'
            """),
            ("zh-CN", """
            locale: 'zh-CN'
            variantOf: 'zh-Hans'
            surfaces: {}
            """));

        Assert.Empty(catalog.Diagnostics);
        Assert.True(
            catalog.Locales
                .Single(static locale => locale.LocaleCode == "zh-CN")
                .ResolvedFeatures
                .ContainsKey("durationCases"));
    }

    [Fact]
    public void DurationCasesRejectUnvettedScriptlessVariantInheritance()
    {
        var catalog = CreateCatalog(
            ("pa-Arab", """
            locale: 'pa-Arab'
            surfaces:
              durationCases:
                classification: 'unsupported'
            """),
            ("pa-PK", """
            locale: 'pa-PK'
            variantOf: 'pa-Arab'
            surfaces: {}
            """));

        Assert.Contains(
            catalog.Diagnostics,
            static diagnostic => diagnostic.Id == "HSG003" &&
                diagnostic.GetMessage().Contains("script-incompatible", StringComparison.Ordinal));
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

    [Fact]
    public void DurationCaseOverlayRequiresEveryTimeUnit()
    {
        var exception = Assert.Throws<InvalidOperationException>(
            () => ParseDurationCases(
                """
                classification: distinct
                cases:
                  dative:
                    units:
                      day:
                        sameAsNominative: true
                """));

        Assert.Contains("must explicitly define 'millisecond'", exception.Message);
    }

    [Fact]
    public void DurationCaseOverlayRejectsUnknownCases()
    {
        var exception = Assert.Throws<InvalidOperationException>(
            () => ParseDurationCases(
                """
                classification: distinct
                cases:
                  sublative:
                    units: {}
                """));

        Assert.Contains("unsupported non-nominative case 'sublative'", exception.Message);
    }

    [Fact]
    public void DurationCasesRejectUnknownRootProperties()
    {
        var exception = Assert.Throws<InvalidOperationException>(
            () => ParseDurationCases(
                """
                classification: not-applicable
                unexpected: true
                """));

        Assert.Contains("'zz.durationCases' defines unsupported property 'unexpected'", exception.Message);
    }

    [Fact]
    public void DurationCaseOverlayRejectsUnresolvedCldrInheritanceMarkers()
    {
        var exception = Assert.Throws<InvalidOperationException>(
            () => ParseDurationCases(
                """
                classification: distinct
                cases:
                  dative:
                    units:
                      millisecond:
                        single:
                          numeric: '↑↑↑'
                """));

        Assert.Contains("unresolved CLDR inheritance marker", exception.Message);
    }

    [Theory]
    [InlineData("same-as-nominative")]
    [InlineData("not-applicable")]
    [InlineData("unsupported")]
    public void NonDistinctDurationClassificationsRejectCaseData(string classification)
    {
        var exception = Assert.Throws<InvalidOperationException>(
            () => ParseDurationCases(
                "classification: " + classification + Environment.NewLine +
                "cases: {}"));

        Assert.Contains("only valid when classification is 'distinct'", exception.Message);
    }

    [Fact]
    public void UnsupportedDurationClassificationIsDistinctFromNotApplicable()
    {
        var catalog = ParseDurationCases("classification: unsupported");

        Assert.Equal(HumanizerSourceGenerator.DurationCaseClassification.Unsupported, catalog.Classification);
        Assert.Empty(catalog.Cases);
    }

    [Fact]
    public void ExplicitDurationCaseAliasesFlattenToAuthoredTerminal()
    {
        var catalog = ParseDurationCases(
            """
            classification: distinct
            citationCase: nominative
            inventory:
              - nominative
              - dative
              - accusative
            sources:
              native:
                kind: native-review
                url: 'https://example.test/reviews/1'
                revision: 'abc123'
                locator: 'comment-1'
                credit: 'Example reviewer'
                reviewed: '2026-07-29'
            cases:
              nominative:
                authored: base-duration
                provenance:
                  - native
              dative:
                sameRenderedAs: nominative
                provenance:
                  - native
              accusative:
                sameRenderedAs: dative
                provenance:
                  - native
            """);

        Assert.Equal(
            HumanizerSourceGenerator.DurationCaseUnitKind.SameAsNominative,
            catalog.Cases["accusative"].Units["day"].Kind);
    }

    [Fact]
    public void ExplicitDurationCasesRejectUnknownProvenanceSources()
    {
        var exception = Assert.Throws<InvalidOperationException>(
            () => ParseDurationCases(
                """
                classification: invariant
                citationCase: nominative
                inventory:
                  - nominative
                sources:
                  grammar:
                    kind: grammar
                    url: 'https://example.test/grammar'
                    revision: 'abc123'
                    locator: 'duration cases'
                    credit: 'Example grammar'
                cases:
                  nominative:
                    authored: base-duration
                    provenance:
                      - missing
                """));

        Assert.Contains("references unknown source 'missing'", exception.Message);
    }

    [Fact]
    public void ExplicitDurationCaseAliasesRejectCycles()
    {
        var exception = Assert.Throws<InvalidOperationException>(
            () => ParseDurationCases(
                """
                classification: distinct
                citationCase: nominative
                inventory:
                  - nominative
                  - dative
                  - accusative
                sources:
                  native:
                    kind: native-review
                    url: 'https://example.test/reviews/1'
                    revision: 'abc123'
                    locator: 'comment-1'
                    credit: 'Example reviewer'
                    reviewed: '2026-07-29'
                cases:
                  nominative:
                    authored: base-duration
                    provenance:
                      - native
                  dative:
                    sameRenderedAs: accusative
                    provenance:
                      - native
                  accusative:
                    sameRenderedAs: dative
                    provenance:
                      - native
                """));

        Assert.Contains("sameRenderedAs cycle", exception.Message);
    }

    [Fact]
    public void ExplicitDurationCasesSupportEvidencedUnitNotApplicable()
    {
        var catalog = ParseDurationCases(
            """
            classification: distinct
            citationCase: nominative
            inventory:
              - nominative
              - sociative
            sources:
              cldr:
                kind: cldr
                url: 'https://github.com/unicode-org/cldr/blob/acd6d88/common/main/ml.xml'
                revision: 'acd6d88'
                locator: 'duration-unit-millisecond'
                credit: 'Unicode CLDR contributors'
            cases:
              nominative:
                authored: base-duration
                provenance:
                  - cldr
              sociative:
                authored:
                  units:
                    millisecond:
                      notApplicable: 'The audited duration surface has no sociative millisecond.'
                      provenance:
                        - cldr
                    second:
                      sameRenderedAs: nominative
                      provenance:
                        - cldr
                    minute:
                      sameRenderedAs: nominative
                      provenance:
                        - cldr
                    hour:
                      sameRenderedAs: nominative
                      provenance:
                        - cldr
                    day:
                      sameRenderedAs: nominative
                      provenance:
                        - cldr
                    week:
                      sameRenderedAs: nominative
                      provenance:
                        - cldr
                    month:
                      sameRenderedAs: nominative
                      provenance:
                        - cldr
                    year:
                      sameRenderedAs: nominative
                      provenance:
                        - cldr
                provenance:
                  - cldr
            """);

        Assert.Equal(
            HumanizerSourceGenerator.DurationCaseUnitKind.NotApplicable,
            catalog.Cases["sociative"].Units["millisecond"].Kind);
        Assert.Equal(
            HumanizerSourceGenerator.DurationCaseUnitKind.SameAsNominative,
            catalog.Cases["sociative"].Units["second"].Kind);
    }

    [Fact]
    public void ExplicitDurationCasesSupportAnAbsolutiveCitationCase()
    {
        var catalog = ParseDurationCases(
            """
            classification: distinct
            citationCase: absolutive
            inventory:
              - absolutive
            sources:
              grammar:
                kind: grammar
                url: 'https://example.test/basque-grammar'
                revision: 'abc123'
                locator: 'case inventory'
                credit: 'Example grammar'
            cases:
              absolutive:
                authored: base-duration
                provenance:
                  - grammar
            """);

        Assert.Equal("absolutive", catalog.CitationCase);
        Assert.Equal(
            HumanizerSourceGenerator.DurationCaseUnitKind.SameAsNominative,
            catalog.Cases["absolutive"].Units["day"].Kind);
        Assert.DoesNotContain("nominative", catalog.Inventory);
    }

    [Fact]
    public void ExplicitDurationCasesRequireCitationCaseInInventory()
    {
        var exception = Assert.Throws<InvalidOperationException>(
            () => ParseDurationCases(
                """
                classification: distinct
                citationCase: absolutive
                inventory:
                  - nominative
                sources:
                  grammar:
                    kind: grammar
                    url: 'https://example.test/grammar'
                    revision: 'abc123'
                    locator: 'case inventory'
                    credit: 'Example grammar'
                cases:
                  nominative:
                    unsupported: 'test fixture'
                """));

        Assert.Contains("citationCase", exception.Message);
        Assert.Contains("inventory", exception.Message);
    }

    [Fact]
    public void ReleaseDurationCasesRejectUnsupportedRoot()
    {
        var catalog = ParseDurationCases(
            """
            classification: unsupported
            citationCase: nominative
            inventory:
              - nominative
            sources:
              grammar:
                kind: grammar
                url: 'https://example.com/grammar'
                revision: 'test'
                locator: 'test fixture'
                credit: 'Test'
            cases:
              nominative:
                unsupported: 'test fixture'
            """);

        var exception = Assert.Throws<InvalidOperationException>(
            () => HumanizerSourceGenerator.DurationCaseCoverageInput.ValidateReleaseCompleteness(catalog));

        Assert.Contains("zz.durationCases.classification", exception.Message);
    }

    [Fact]
    public void ReleaseDurationCasesRejectLegacyProfile()
    {
        var catalog = ParseDurationCases("classification: not-applicable");

        var exception = Assert.Throws<InvalidOperationException>(
            () => HumanizerSourceGenerator.DurationCaseCoverageInput.ValidateReleaseCompleteness(catalog));

        Assert.Contains("explicit inventory, reason, sources, and provenance", exception.Message);
    }

    [Fact]
    public void ReleaseDurationCasesRejectUnsupportedCase()
    {
        var catalog = ParseDurationCases(
            """
            classification: distinct
            citationCase: nominative
            inventory:
              - nominative
              - dative
            sources:
              grammar:
                kind: grammar
                url: 'https://example.test/grammar'
                revision: 'abc123'
                locator: 'duration cases'
                credit: 'Example grammar'
            cases:
              nominative:
                authored: base-duration
                provenance:
                  - grammar
              dative:
                unsupported: 'Awaiting sourced forms.'
            """);

        var exception = Assert.Throws<InvalidOperationException>(
            () => HumanizerSourceGenerator.DurationCaseCoverageInput.ValidateReleaseCompleteness(catalog));

        Assert.Contains("zz.durationCases.cases.dative", exception.Message);
    }

    [Fact]
    public void ReleaseDurationCasesRejectUnsupportedUnit()
    {
        var catalog = ParseDurationCases(
            """
            classification: distinct
            citationCase: nominative
            inventory:
              - nominative
              - dative
            sources:
              grammar:
                kind: grammar
                url: 'https://example.test/grammar'
                revision: 'abc123'
                locator: 'duration cases'
                credit: 'Example grammar'
            cases:
              nominative:
                authored: base-duration
                provenance:
                  - grammar
              dative:
                authored:
                  units:
                    millisecond:
                      unsupported: 'Awaiting sourced form.'
                    second:
                      sameRenderedAs: nominative
                      provenance:
                        - grammar
                    minute:
                      sameRenderedAs: nominative
                      provenance:
                        - grammar
                    hour:
                      sameRenderedAs: nominative
                      provenance:
                        - grammar
                    day:
                      sameRenderedAs: nominative
                      provenance:
                        - grammar
                    week:
                      sameRenderedAs: nominative
                      provenance:
                        - grammar
                    month:
                      sameRenderedAs: nominative
                      provenance:
                        - grammar
                    year:
                      sameRenderedAs: nominative
                      provenance:
                        - grammar
                provenance:
                  - grammar
            """);

        var exception = Assert.Throws<InvalidOperationException>(
            () => HumanizerSourceGenerator.DurationCaseCoverageInput.ValidateReleaseCompleteness(catalog));

        Assert.Contains(
            "zz.durationCases.cases.dative.authored.units.millisecond",
            exception.Message);
    }

    [Fact]
    public void ReleaseDurationCasesRequireEveryReachablePluralForm()
    {
        var locale = GetLocaleFile("ar").ReplaceLineEndings("\n");
        const string zeroForm = "                    zero: '{count} ملي ثانية'\n";
        var zeroFormIndex = locale.IndexOf(zeroForm, StringComparison.Ordinal);
        Assert.True(zeroFormIndex >= 0);
        locale = locale.Remove(zeroFormIndex, zeroForm.Length);

        var exception = Assert.Throws<InvalidOperationException>(
            () => HumanizerSourceGenerator.DurationCaseCoverageInput.Create(
                CreateCatalog(("ar", locale))));

        Assert.Contains(
            "ar.durationCases.cases.nominative.authored.units.millisecond.phrase.multiple.forms",
            exception.Message);
        Assert.Contains("reachable 'zero' form", exception.Message);
    }

    [Fact]
    public void ReleaseDurationCasesUseTheEffectiveFormatterNumberDetector()
    {
        var locale = GetLocaleFile("bs").ReplaceLineEndings("\n");
        const string singularForm = "                    singular: 'milisekunda'\n";
        var singularFormIndex = locale.IndexOf(singularForm, StringComparison.Ordinal);
        Assert.True(singularFormIndex >= 0);
        locale = locale.Remove(singularFormIndex, singularForm.Length);

        var exception = Assert.Throws<InvalidOperationException>(
            () => HumanizerSourceGenerator.DurationCaseCoverageInput.Create(
                CreateCatalog(("bs", locale))));

        Assert.Contains(
            "bs.durationCases.cases.nominative.authored.units.millisecond.phrase.multiple.forms",
            exception.Message);
        Assert.Contains("reachable 'singular' form", exception.Message);
    }

    [Fact]
    public void ExplicitSouthSlavicCaseRuleRequiresPaucalForms()
    {
        var exception = Assert.Throws<InvalidOperationException>(
            () => HumanizerSourceGenerator.DurationCaseNormalization.ParseForTests(
                "zz",
                InvariantCitationDurationCases,
                casePluralRule: "south-slavic",
                phrasesText: CreateSouthSlavicCitationDurationPhrases(includePaucal: false)));

        Assert.Contains("Case overlay 'zz.phrases.duration", exception.Message);
        Assert.Contains("reachable 'paucal' form", exception.Message);
    }

    [Fact]
    public void OrdinaryFormatterDetectorAllowsNamedFormFallback()
    {
        var coverage = HumanizerSourceGenerator.DurationCaseCoverageInput.Create(
            CreateCatalog(("am", GetLocaleFile("am"))));

        Assert.Contains(
            coverage.Catalogs,
            static catalog => catalog.LocaleCode == "am");
    }

    [Fact]
    public void CitationDurationCasesRequireNumericMultipleFormsForDedicatedCaseRule()
    {
        var exception = Assert.Throws<InvalidOperationException>(
            () => HumanizerSourceGenerator.DurationCaseNormalization.ParseForTests(
                "zz",
                """
                  classification: invariant
                  citationCase: nominative
                  inventory:
                    - nominative
                  sources:
                    grammar:
                      kind: grammar
                      url: 'https://example.test/grammar'
                      revision: 'abc123'
                      locator: 'duration cases'
                      credit: 'Example grammar'
                  cases:
                    nominative:
                      authored: base-duration
                      provenance:
                        - grammar
                """,
                casePluralRule: "arabic-cardinal",
                phrasesText:
                """
                  duration:
                    millisecond:
                      single: 'one millisecond'
                """));

        Assert.Contains("zz.phrases.duration.millisecond", exception.Message);
        Assert.Contains("numeric multiple forms", exception.Message);
    }

    [Fact]
    public void CitationDurationCasesRequireNumericMultipleFormsWithoutDedicatedCaseRule()
    {
        var exception = Assert.Throws<InvalidOperationException>(
            () => HumanizerSourceGenerator.DurationCaseNormalization.ParseForTests(
                "zz",
                InvariantCitationDurationCases,
                phrasesText:
                """
                  duration:
                    millisecond:
                      single: 'one millisecond'
                """));

        Assert.Contains("zz.phrases.duration.millisecond", exception.Message);
        Assert.Contains("numeric multiple forms", exception.Message);
    }

    [Fact]
    public void CitationDurationCasesAllowOrdinaryMultipleFallback()
    {
        var catalog = HumanizerSourceGenerator.DurationCaseNormalization.ParseForTests(
            "zz",
            InvariantCitationDurationCases,
            phrasesText:
            """
              duration:
                millisecond:
                  single: 'one millisecond'
                  multiple: '{count} milliseconds'
                second:
                  single: 'one second'
                  multiple: '{count} seconds'
                minute:
                  single: 'one minute'
                  multiple: '{count} minutes'
                hour:
                  single: 'one hour'
                  multiple: '{count} hours'
                day:
                  single: 'one day'
                  multiple: '{count} days'
                week:
                  single: 'one week'
                  multiple: '{count} weeks'
                month:
                  single: 'one month'
                  multiple: '{count} months'
                year:
                  single: 'one year'
                  multiple: '{count} years'
            """);

        Assert.Equal(
            HumanizerSourceGenerator.DurationCaseClassification.Invariant,
            catalog.Classification);
    }

    const string InvariantCitationDurationCases =
        """
          classification: invariant
          citationCase: nominative
          inventory:
            - nominative
          sources:
            grammar:
              kind: grammar
              url: 'https://example.test/grammar'
              revision: 'abc123'
              locator: 'duration cases'
              credit: 'Example grammar'
          cases:
            nominative:
              authored: base-duration
              provenance:
                - grammar
        """;

    const string CompleteCitationDurationPhrases =
        """
          duration:
            millisecond:
              single: 'one millisecond'
              multiple: '{count} milliseconds'
            second:
              single: 'one second'
              multiple: '{count} seconds'
            minute:
              single: 'one minute'
              multiple: '{count} minutes'
            hour:
              single: 'one hour'
              multiple: '{count} hours'
            day:
              single: 'one day'
              multiple: '{count} days'
            week:
              single: 'one week'
              multiple: '{count} weeks'
            month:
              single: 'one month'
              multiple: '{count} months'
            year:
              single: 'one year'
              multiple: '{count} years'
        """;

    static readonly string[] SouthSlavicDurationUnits =
    [
        "millisecond",
        "second",
        "minute",
        "hour",
        "day",
        "week",
        "month",
        "year"
    ];

    static string CreateSouthSlavicCitationDurationPhrases(bool includePaucal)
    {
        var paucal = includePaucal ? "\n          paucal: '{count} units'" : string.Empty;
        return "  duration:\n" + string.Join(
            "\n",
            SouthSlavicDurationUnits.Select(unit =>
                    $"    {unit}:\n" +
                    $"      single: 'one {unit}'\n" +
                    "      multiple:\n" +
                    "        forms:\n" +
                    "          default: '{count} units'\n" +
                    $"          singular: '{{count}} {unit}'{paucal}"));
    }

    static HumanizerSourceGenerator.DurationCaseCatalog ParseDurationCases(string body) =>
        HumanizerSourceGenerator.DurationCaseNormalization.ParseForTests(
            "zz",
            string.Join("\n", body.ReplaceLineEndings("\n").Split('\n').Select(static line => $"  {line}")),
            phrasesText: CompleteCitationDurationPhrases);

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
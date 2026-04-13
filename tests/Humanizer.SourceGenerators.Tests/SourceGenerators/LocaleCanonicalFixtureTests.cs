using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

using Humanizer.SourceGenerators;

using Xunit;

namespace Humanizer.SourceGenerators.Tests;

/// <summary>
/// Tests covering error paths and low-coverage branches in
/// <see cref="HumanizerSourceGenerator.CanonicalLocaleAuthoring"/>,
/// <see cref="HumanizerSourceGenerator.LocaleCatalogInput"/>,
/// <see cref="HumanizerSourceGenerator.LocaleDefinitionFile"/>, and
/// <see cref="HumanizerSourceGenerator.LocaleSemanticDiff"/>.
/// Each fixture targets exactly one named branch.
/// </summary>
public class LocaleCanonicalFixtureTests
{
    // ──────────────────────────────────────────────────────────────────────
    //  CanonicalLocaleAuthoring.Parse error branches
    // ──────────────────────────────────────────────────────────────────────

    [Fact]
    public void Parse_SurfacesNotMapping_ReportsError()
    {
        var catalog = CreateCatalogFromFixture("surfaces-not-mapping", "zz-surfaces-not-mapping");

        AssertDiagnosticContains(catalog, "HSG003", "surfaces' must be a mapping");
    }

    [Fact]
    public void Parse_UnsupportedSurfaceName_ReportsError()
    {
        var catalog = CreateCatalogFromFixture("unsupported-surface-name", "zz-unsupported-surface-name");

        AssertDiagnosticContains(catalog, "HSG003", "unsupported surface 'bogus'");
    }

    [Fact]
    public void Parse_SurfaceValueNotMapping_ReportsError()
    {
        var catalog = CreateCatalogFromFixture("surface-value-not-mapping", "zz-surface-value-not-mapping");

        AssertDiagnosticContains(catalog, "HSG003", "must be a mapping");
    }

    [Fact]
    public void Parse_LocaleMismatch_ReportsError()
    {
        var catalog = CreateCatalogFromFixture("locale-mismatch", "zz-locale-mismatch");

        AssertDiagnosticContains(catalog, "HSG003", "must match file locale");
    }

    // ──────────────────────────────────────────────────────────────────────
    //  CanonicalLocaleAuthoring.Parse — number surface error branches
    // ──────────────────────────────────────────────────────────────────────

    [Fact]
    public void Parse_NumberUnsupportedProperty_ReportsError()
    {
        var catalog = CreateCatalogFromFixture("number-unsupported-property", "zz-number-unsupported-property");

        AssertDiagnosticContains(catalog, "HSG003", "unsupported property 'bogus'");
    }

    [Fact]
    public void Parse_NumberWordsNotMapping_ReportsError()
    {
        var catalog = CreateCatalogFromFixture("number-words-not-mapping", "zz-number-words-not-mapping");

        AssertDiagnosticContains(catalog, "HSG003", "number.words' must be a mapping");
    }

    [Fact]
    public void Parse_NumberParseNotMapping_ReportsError()
    {
        var catalog = CreateCatalogFromFixture("number-parse-not-mapping", "zz-number-parse-not-mapping");

        AssertDiagnosticContains(catalog, "HSG003", "number.parse' must be a mapping");
    }

    [Fact]
    public void Parse_NumberFormattingNotMapping_ReportsError()
    {
        var catalog = CreateCatalogFromFixture("number-formatting-not-mapping", "zz-number-formatting-not-mapping");

        AssertDiagnosticContains(catalog, "HSG003", "number.formatting' must be a mapping");
    }

    [Fact]
    public void Parse_NumberFormattingUnsupportedProperty_ReportsError()
    {
        var catalog = CreateCatalogFromFixture("number-formatting-unsupported-property", "zz-number-formatting-unsupported-property");

        AssertDiagnosticContains(catalog, "HSG003", "unsupported property 'bogusKey'");
    }

    [Fact]
    public void Parse_NumberFormattingEmpty_ReportsError()
    {
        var catalog = CreateCatalogFromFixture("number-formatting-empty", "zz-number-formatting-empty");

        AssertDiagnosticContains(catalog, "HSG003", "must define at least one property");
    }

    [Fact]
    public void Parse_NumberFormattingPropertyNotScalar_ReportsError()
    {
        var catalog = CreateCatalogFromFixture("number-formatting-property-not-scalar", "zz-number-formatting-property-not-scalar");

        AssertDiagnosticContains(catalog, "HSG003", "must be a scalar string");
    }

    [Fact]
    public void Parse_NumberFormattingPropertyEmptyString_ReportsError()
    {
        var catalog = CreateCatalogFromFixture("number-formatting-property-empty-string", "zz-number-formatting-property-empty-string");

        AssertDiagnosticContains(catalog, "HSG003", "must be a non-empty string");
    }

    // ──────────────────────────────────────────────────────────────────────
    //  CanonicalLocaleAuthoring.Parse — ordinal surface error branches
    // ──────────────────────────────────────────────────────────────────────

    [Fact]
    public void Parse_OrdinalUnsupportedProperty_ReportsError()
    {
        var catalog = CreateCatalogFromFixture("ordinal-unsupported-property", "zz-ordinal-unsupported-property");

        AssertDiagnosticContains(catalog, "HSG003", "unsupported property 'bogus'");
    }

    [Fact]
    public void Parse_OrdinalNumericNotMapping_ReportsError()
    {
        var catalog = CreateCatalogFromFixture("ordinal-numeric-not-mapping", "zz-ordinal-numeric-not-mapping");

        AssertDiagnosticContains(catalog, "HSG003", "ordinal.numeric' must be a mapping");
    }

    [Fact]
    public void Parse_OrdinalDateNotMapping_ReportsError()
    {
        var catalog = CreateCatalogFromFixture("ordinal-date-not-mapping", "zz-ordinal-date-not-mapping");

        AssertDiagnosticContains(catalog, "HSG003", "ordinal.date' must be a mapping");
    }

    [Fact]
    public void Parse_OrdinalDateOnlyNotMapping_ReportsError()
    {
        var catalog = CreateCatalogFromFixture("ordinal-dateonly-not-mapping", "zz-ordinal-dateonly-not-mapping");

        AssertDiagnosticContains(catalog, "HSG003", "ordinal.dateOnly' must be a mapping");
    }

    // ──────────────────────────────────────────────────────────────────────
    //  CanonicalLocaleAuthoring.Parse — calendar surface error branches
    // ──────────────────────────────────────────────────────────────────────

    [Fact]
    public void Parse_CalendarUnsupportedProperty_ReportsError()
    {
        var catalog = CreateCatalogFromFixture("calendar-unsupported-property", "zz-calendar-unsupported-property");

        AssertDiagnosticContains(catalog, "HSG003", "unsupported property 'bogus'");
    }

    [Fact]
    public void Parse_CalendarMonthsWrongLength_ReportsError()
    {
        var catalog = CreateCatalogFromFixture("calendar-months-wrong-length", "zz-calendar-months-wrong-length");

        AssertDiagnosticContains(catalog, "HSG003", "exactly 12 strings");
    }

    [Fact]
    public void Parse_CalendarMonthItemNotScalar_ReportsError()
    {
        var catalog = CreateCatalogFromFixture("calendar-month-item-not-scalar", "zz-calendar-month-item-not-scalar");

        AssertDiagnosticContains(catalog, "HSG003", "months' items must be scalar strings");
    }

    [Fact]
    public void Parse_CalendarGenitiveWithoutMonths_ReportsError()
    {
        var catalog = CreateCatalogFromFixture("calendar-genitive-without-months", "zz-calendar-genitive-without-months");

        AssertDiagnosticContains(catalog, "HSG003", "requires 'months' to also be present");
    }

    [Fact]
    public void Parse_CalendarGenitiveWrongLength_ReportsError()
    {
        var catalog = CreateCatalogFromFixture("calendar-genitive-wrong-length", "zz-calendar-genitive-wrong-length");

        AssertDiagnosticContains(catalog, "HSG003", "monthsGenitive' must be a sequence of exactly 12 strings");
    }

    [Fact]
    public void Parse_CalendarGenitiveItemNotScalar_ReportsError()
    {
        var catalog = CreateCatalogFromFixture("calendar-genitive-item-not-scalar", "zz-calendar-genitive-item-not-scalar");

        AssertDiagnosticContains(catalog, "HSG003", "monthsGenitive' items must be scalar strings");
    }

    [Fact]
    public void Parse_CalendarHijriItemNotScalar_ReportsError()
    {
        var catalog = CreateCatalogFromFixture("calendar-hijri-item-not-scalar", "zz-calendar-hijri-item-not-scalar");

        AssertDiagnosticContains(catalog, "HSG003", "hijriMonths' items must be scalar strings");
    }

    [Fact]
    public void Parse_CalendarHijriDirectionalityControl_ReportsError()
    {
        // The fixture file contains an actual U+200E (LRM) character embedded in the
        // first hijriMonths entry. Verify the character is present for auditability.
        var fixturePath = Path.Combine(
            FixtureLoader.GetFixtureDirectory("CanonicalDiagnostics"),
            "calendar-hijri-directionality-control.yml");
        var fileText = File.ReadAllText(fixturePath);
        Assert.Contains('\u200E', fileText);

        var catalog = CreateCatalogFromFixture(
            "calendar-hijri-directionality-control",
            "zz-calendar-hijri-directionality-control");

        AssertDiagnosticContains(catalog, "HSG003", "must not contain directionality controls");
    }

    // ──────────────────────────────────────────────────────────────────────
    //  CanonicalLocaleAuthoring.Parse — list surface error branches
    // ──────────────────────────────────────────────────────────────────────

    [Fact]
    public void Parse_ListMissingTemplates_ReportsError()
    {
        var catalog = CreateCatalogFromFixture("list-missing-templates", "zz-list-missing-templates");

        AssertDiagnosticContains(catalog, "HSG003", "must define canonical list templates or legacy 'value'");
    }

    [Fact]
    public void Parse_ListTemplateBadFormat_ReportsError()
    {
        var catalog = CreateCatalogFromFixture("list-template-bad-format", "zz-list-template-bad-format");

        AssertDiagnosticContains(catalog, "HSG003", "must use '{0}' and '{1}'");
    }

    [Fact]
    public void Parse_ExplicitDefaultEngineInNumber_ReportsError()
    {
        var catalog = CreateCatalogFromFixture("explicit-default-engine-in-number", "zz-explicit-default-engine-in-number");

        AssertDiagnosticContains(catalog, "HSG003", "engine: 'default'");
    }

    // ──────────────────────────────────────────────────────────────────────
    //  CanonicalLocaleAuthoring — list normalization engine resolution
    // ──────────────────────────────────────────────────────────────────────

    [Theory]
    [InlineData("delimited", false, false, "{0}, {1}", "{0}, {1}", "{0}, {1}", "delimited")]
    [InlineData("conjunction", false, false, "{0} and {1}", "{0} and {1}", "{0} and {1}", "delimited")]
    [InlineData("conjunction", false, true, "{0} e{1}", "{0} e{1}", null, "clitic")]
    [InlineData("conjunction", true, false, "{0} and {1}", "{0}, and {1}", null, "oxford")]
    [InlineData("conjunction", false, false, "{0} and {1}", "{0}, and {1}", null, "conjunction")]
    public void NormalizeListSurface_ResolvesEngineCorrectly(
        string declaredEngine,
        bool oxfordComma,
        bool cliticizesFinal,
        string pairTemplate,
        string finalTemplate,
        string? serialTemplate,
        string expectedEngine)
    {
        var yaml = $"locale: 'zz-list-norm'\nsurfaces:\n  list:\n    engine: '{declaredEngine}'\n"
                   + $"    pairTemplate: '{pairTemplate}'\n"
                   + $"    finalTemplate: '{finalTemplate}'\n"
                   + (serialTemplate is not null ? $"    serialTemplate: '{serialTemplate}'\n" : "")
                   + (oxfordComma ? "    oxfordComma: true\n" : "")
                   + (cliticizesFinal ? "    cliticizesFinal: true\n" : "");

        var catalog = CreateCatalog(("zz-list-norm", yaml));

        Assert.Empty(catalog.Diagnostics);
        var locale = catalog.Locales.Single(static l => l.LocaleCode == "zz-list-norm");
        Assert.Equal(expectedEngine, locale.CollectionFormatter!.Kind);
    }

    [Fact]
    public void NormalizeListSurface_OxfordEngineWithoutTemplates_PassesThrough()
    {
        var yaml = "locale: 'zz-list-oxford'\nsurfaces:\n  list:\n    engine: 'oxford'\n";

        var catalog = CreateCatalog(("zz-list-oxford", yaml));

        Assert.Empty(catalog.Diagnostics);
        var locale = catalog.Locales.Single(static l => l.LocaleCode == "zz-list-oxford");
        Assert.Equal("oxford", locale.CollectionFormatter!.Kind);
    }

    [Fact]
    public void NormalizeListSurface_LegacyValuePassesThrough()
    {
        var yaml = "locale: 'zz-list-legacy'\nsurfaces:\n  list:\n    engine: 'conjunction'\n    value: 'and'\n";

        var catalog = CreateCatalog(("zz-list-legacy", yaml));

        Assert.Empty(catalog.Diagnostics);
        var locale = catalog.Locales.Single(static l => l.LocaleCode == "zz-list-legacy");
        Assert.Equal("conjunction", locale.CollectionFormatter!.Kind);
        Assert.Equal("and", locale.CollectionFormatter.Argument);
    }

    // ──────────────────────────────────────────────────────────────────────
    //  LocaleCatalogInput error branches
    // ──────────────────────────────────────────────────────────────────────

    [Fact]
    public void Catalog_DuplicateLocaleCode_ReportsError()
    {
        var files = ImmutableArray.Create<HumanizerSourceGenerator.LocaleDefinitionFile?>(
            new HumanizerSourceGenerator.LocaleDefinitionFile("zz-dup", "locale: 'zz-dup'\nsurfaces: {}"),
            new HumanizerSourceGenerator.LocaleDefinitionFile("zz-dup", "locale: 'zz-dup'\nsurfaces: {}"));

        var catalog = HumanizerSourceGenerator.LocaleCatalogInput.Create(files);

        AssertDiagnosticContains(catalog, "HSG003", "defined more than once");
    }

    [Fact]
    public void Catalog_InheritanceCycleDetected_ReportsError()
    {
        var catalog = CreateCatalog(
            ("zz-a", "locale: 'zz-a'\nvariantOf: 'zz-b'\nsurfaces: {}"),
            ("zz-b", "locale: 'zz-b'\nvariantOf: 'zz-a'\nsurfaces: {}"));

        AssertDiagnosticContains(catalog, "HSG003", "inheritance cycle");
    }

    [Fact]
    public void Catalog_MissingInheritedLocale_ReportsError()
    {
        var catalog = CreateCatalog(
            ("zz-orphan", "locale: 'zz-orphan'\nvariantOf: 'zz-nonexistent'\nsurfaces: {}"));

        AssertDiagnosticContains(catalog, "HSG003", "not defined");
    }

    [Fact]
    public void Catalog_NullFilesAreSkipped()
    {
        var files = ImmutableArray.Create<HumanizerSourceGenerator.LocaleDefinitionFile?>(
            null,
            new HumanizerSourceGenerator.LocaleDefinitionFile("zz-ok", "locale: 'zz-ok'\nsurfaces: {}"));

        var catalog = HumanizerSourceGenerator.LocaleCatalogInput.Create(files);

        Assert.Empty(catalog.Diagnostics);
        Assert.Single(catalog.Locales);
        Assert.Equal("zz-ok", catalog.Locales[0].LocaleCode);
    }

    // ──────────────────────────────────────────────────────────────────────
    //  LocaleCatalogInput — feature resolution error paths
    // ──────────────────────────────────────────────────────────────────────

    [Fact]
    public void Catalog_CompassSurfaceScalar_ReportsNotMapping()
    {
        // compass: 'scalar' hits the CanonicalLocaleAuthoring "surface value not a mapping" branch
        // for the compass surface specifically. The ResolveHeadings branch in LocaleCatalogInput
        // is unreachable because CanonicalLocaleAuthoring.Parse rejects non-mapping surfaces first.
        var catalog = CreateCatalog(
            ("zz-head", "locale: 'zz-head'\nsurfaces:\n  compass: 'scalar'\n"));

        AssertDiagnosticContains(catalog, "HSG003", "must be a mapping");
    }

    [Fact]
    public void Catalog_HeadingsUnsupportedProperty_ReportsError()
    {
        var yaml = "locale: 'zz-head2'\nsurfaces:\n  compass:\n    full:\n" + MakeHeadingSequence()
                   + "    short:\n" + MakeHeadingSequence()
                   + "    bogus: 'extra'\n";

        var catalog = CreateCatalog(("zz-head2", yaml));

        AssertDiagnosticContains(catalog, "HSG003", "unsupported property 'bogus'");
    }

    [Fact]
    public void Catalog_HeadingsMissingFull_ReportsError()
    {
        var yaml = "locale: 'zz-head3'\nsurfaces:\n  compass:\n    short:\n" + MakeHeadingSequence();

        var catalog = CreateCatalog(("zz-head3", yaml));

        AssertDiagnosticContains(catalog, "HSG003", "must define 'full'");
    }

    [Fact]
    public void Catalog_HeadingsFullNotSequence_ReportsError()
    {
        var yaml = "locale: 'zz-head4'\nsurfaces:\n  compass:\n    full: 'scalar'\n    short:\n" + MakeHeadingSequence();

        var catalog = CreateCatalog(("zz-head4", yaml));

        AssertDiagnosticContains(catalog, "HSG003", "must be a sequence");
    }

    [Fact]
    public void Catalog_HeadingsWrongCount_ReportsError()
    {
        var yaml = "locale: 'zz-head5'\nsurfaces:\n  compass:\n    full:\n      - 'N'\n    short:\n" + MakeHeadingSequence();

        var catalog = CreateCatalog(("zz-head5", yaml));

        AssertDiagnosticContains(catalog, "HSG003", "exactly 16 entries");
    }

    [Fact]
    public void Catalog_HeadingsItemNotScalar_ReportsError()
    {
        var yaml = "locale: 'zz-head6'\nsurfaces:\n  compass:\n    full:\n"
                   + "      -\n        nested: 'value'\n"
                   + string.Concat(Enumerable.Range(0, 15).Select(static _ => "      - 'N'\n"))
                   + "    short:\n" + MakeHeadingSequence();

        var catalog = CreateCatalog(("zz-head6", yaml));

        AssertDiagnosticContains(catalog, "HSG003", "must be a scalar");
    }

    [Fact]
    public void Catalog_HeadingsFullNotSequence_CaughtByTryResolveLocalePart()
    {
        // Exercises the TryResolveLocalePart catch-and-diagnostic path: ResolveHeadings
        // throws when full is not a sequence, TryResolveLocalePart catches the exception
        // and emits a diagnostic instead of crashing the generator.
        var catalog = CreateCatalog(
            ("zz-head-err", "locale: 'zz-head-err'\nsurfaces:\n  compass:\n    full: 'not-a-seq'\n    short: 'not-a-seq'\n"));

        AssertDiagnosticContains(catalog, "HSG003", "must be a sequence");
    }

    // ──────────────────────────────────────────────────────────────────────
    //  LocaleCatalogInput — merge semantics
    // ──────────────────────────────────────────────────────────────────────

    [Fact]
    public void Catalog_ChildEngineSwitch_ReplacesParentMapping()
    {
        var catalog = CreateCatalog(
            ("zz-parent", """
locale: 'zz-parent'
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
"""),
            ("zz-child-switch", """
locale: 'zz-child-switch'
variantOf: 'zz-parent'
surfaces:
  number:
    words:
      engine: 'variant-decade'
      minusWord: 'moins'
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
        var child = catalog.Locales.Single(static l => l.LocaleCode == "zz-child-switch");
        // When engine switches, child completely replaces parent. No 'andWord' from parent.
        Assert.Equal("variant-decade", child.NumberToWords!.ProfileRoot.GetProperty("engine").GetString());
        Assert.False(child.NumberToWords.ProfileRoot.TryGetProperty("andWord", out _));
    }

    // ──────────────────────────────────────────────────────────────────────
    //  LocaleDefinitionFile — filter branches
    // ──────────────────────────────────────────────────────────────────────

    [Fact]
    public void LocaleDefinitionFile_NonLocalesPath_ReturnsNull()
    {
        var text = new TestAdditionalText(@"E:\Dev\Humanizer\src\Humanizer\Resources\test.yml", "locale: 'test'\nsurfaces: {}");
        var result = HumanizerSourceGenerator.LocaleDefinitionFile.Create(text, TestContext.Current.CancellationToken);

        Assert.Null(result);
    }

    [Fact]
    public void LocaleDefinitionFile_NonYmlExtension_ReturnsNull()
    {
        var text = new TestAdditionalText(@"E:\Dev\Humanizer\src\Humanizer\Locales\test.txt", "locale: 'test'\nsurfaces: {}");
        var result = HumanizerSourceGenerator.LocaleDefinitionFile.Create(text, TestContext.Current.CancellationToken);

        Assert.Null(result);
    }

    [Fact]
    public void LocaleDefinitionFile_EmptyContent_ReturnsNull()
    {
        var text = new TestAdditionalText(@"E:\Dev\Humanizer\src\Humanizer\Locales\test.yml", "");
        var result = HumanizerSourceGenerator.LocaleDefinitionFile.Create(text, TestContext.Current.CancellationToken);

        Assert.Null(result);
    }

    [Fact]
    public void LocaleDefinitionFile_NullContent_ReturnsNull()
    {
        var text = new NullTextAdditionalText(@"E:\Dev\Humanizer\src\Humanizer\Locales\test.yml");
        var result = HumanizerSourceGenerator.LocaleDefinitionFile.Create(text, TestContext.Current.CancellationToken);

        Assert.Null(result);
    }

    [Fact]
    public void LocaleDefinitionFile_ValidPath_ReturnsDefinition()
    {
        var text = new TestAdditionalText(@"E:\Dev\Humanizer\src\Humanizer\Locales\en.yml", "locale: 'en'\nsurfaces: {}");
        var result = HumanizerSourceGenerator.LocaleDefinitionFile.Create(text, TestContext.Current.CancellationToken);

        Assert.NotNull(result);
        Assert.Equal("en", result!.LocaleCode);
    }

    [Fact]
    public void LocaleDefinitionFile_ForwardSlashPath_ReturnsDefinition()
    {
        var text = new TestAdditionalText("E:/Dev/Humanizer/src/Humanizer/Locales/fr.yml", "locale: 'fr'\nsurfaces: {}");
        var result = HumanizerSourceGenerator.LocaleDefinitionFile.Create(text, TestContext.Current.CancellationToken);

        Assert.NotNull(result);
        Assert.Equal("fr", result!.LocaleCode);
    }

    // ──────────────────────────────────────────────────────────────────────
    //  FixtureLoader — self-describing locale parsing
    // ──────────────────────────────────────────────────────────────────────

    [Fact]
    public void FixtureLoader_LoadAsAdditionalText_ParsesDeclaredLocaleFromYaml()
    {
        // Verify that LoadAsAdditionalText auto-detects the locale: from fixture YAML
        // so callers do not need to pass an explicit locale code override.
        var fixturePath = Path.Combine(
            FixtureLoader.GetFixtureDirectory("CanonicalDiagnostics"),
            "surfaces-not-mapping.yml");

        var additionalText = FixtureLoader.LoadAsAdditionalText(fixturePath);

        Assert.Contains("zz-surfaces-not-mapping", additionalText.Path);
    }

    [Fact]
    public void FixtureLoader_LoadAsAdditionalText_AllowsLocaleCodeOverride()
    {
        var fixturePath = Path.Combine(
            FixtureLoader.GetFixtureDirectory("CanonicalDiagnostics"),
            "surfaces-not-mapping.yml");

        var additionalText = FixtureLoader.LoadAsAdditionalText(fixturePath, "custom-override");

        Assert.Contains("custom-override", additionalText.Path);
    }

    [Fact]
    public void FixtureLoader_FromString_WrapsCorrectly()
    {
        var additionalText = FixtureLoader.FromString("zz-test", "locale: 'zz-test'\nsurfaces: {}");

        Assert.Contains("zz-test", additionalText.Path);
        Assert.NotNull(additionalText.GetText(TestContext.Current.CancellationToken));
    }

    // ──────────────────────────────────────────────────────────────────────
    //  LocaleSemanticDiff — diff branches
    // ──────────────────────────────────────────────────────────────────────

    [Fact]
    public void SemanticDiff_LocaleOnlyOnRight_ReportsExistsOnlyOnRight()
    {
        var left = CreateCatalog(
            ("zz-common", "locale: 'zz-common'\nsurfaces: {}"));
        var right = CreateCatalog(
            ("zz-common", "locale: 'zz-common'\nsurfaces: {}"),
            ("zz-right-only", "locale: 'zz-right-only'\nsurfaces: {}"));

        var differences = HumanizerSourceGenerator.LocaleSemanticDiff.Compare(left.Locales, right.Locales);

        Assert.Contains(differences, static d => d.Contains("zz-right-only", StringComparison.Ordinal) && d.Contains("only on the right", StringComparison.Ordinal));
    }

    [Fact]
    public void SemanticDiff_LocaleOnlyOnLeft_ReportsExistsOnlyOnLeft()
    {
        var left = CreateCatalog(
            ("zz-common", "locale: 'zz-common'\nsurfaces: {}"),
            ("zz-left-only", "locale: 'zz-left-only'\nsurfaces: {}"));
        var right = CreateCatalog(
            ("zz-common", "locale: 'zz-common'\nsurfaces: {}"));

        var differences = HumanizerSourceGenerator.LocaleSemanticDiff.Compare(left.Locales, right.Locales);

        Assert.Contains(differences, static d => d.Contains("zz-left-only", StringComparison.Ordinal) && d.Contains("only on the left", StringComparison.Ordinal));
    }

    [Fact]
    public void SemanticDiff_NullFeatures_AreEqualToNull()
    {
        var left = CreateCatalog(("zz-null", "locale: 'zz-null'\nsurfaces: {}"));
        var right = CreateCatalog(("zz-null", "locale: 'zz-null'\nsurfaces: {}"));

        var differences = HumanizerSourceGenerator.LocaleSemanticDiff.Compare(left.Locales, right.Locales);

        Assert.Empty(differences);
    }

    [Fact]
    public void SemanticDiff_WithPhrases_DetectsChanges()
    {
        var leftYaml = """
locale: 'zz-phrases'
surfaces:
  phrases:
    relativeDate:
      now: 'now'
      never: 'never'
      past:
        day:
          single: 'yesterday'
          multiple:
            forms:
              default: 'days'
              singular: 'day'
            countPlacement: 'before-form'
      future:
        day:
          single: 'tomorrow'
          multiple:
            forms:
              default: 'days'
              singular: 'day'
    duration:
      zero: 'zero'
      age:
        template: '{value} old'
      day:
        single:
          numeric: '1 day'
          words: 'one day'
        multiple:
          forms:
            default: 'days'
            singular: 'day'
          wordsVariant:
            forms:
              default: 'days'
    dataUnits:
      byte:
        forms:
          default: 'bytes'
          singular: 'byte'
    timeUnits:
      second:
        forms:
          default: 'seconds'
          singular: 'second'
        symbol: 's'
""";

        var rightYaml = """
locale: 'zz-phrases'
surfaces:
  phrases:
    relativeDate:
      now: 'NOW'
      never: 'never'
      past:
        day:
          single: 'yesterday'
          multiple:
            forms:
              default: 'days'
              singular: 'day'
            countPlacement: 'before-form'
      future:
        day:
          single: 'tomorrow'
          multiple:
            forms:
              default: 'days'
              singular: 'day'
    duration:
      zero: 'zero'
      age:
        template: '{value} old'
      day:
        single:
          numeric: '1 day'
          words: 'one day'
        multiple:
          forms:
            default: 'days'
            singular: 'day'
          wordsVariant:
            forms:
              default: 'days'
    dataUnits:
      byte:
        forms:
          default: 'bytes'
          singular: 'byte'
    timeUnits:
      second:
        forms:
          default: 'seconds'
          singular: 'second'
        symbol: 's'
""";

        var left = CreateCatalog(("zz-phrases", leftYaml));
        var right = CreateCatalog(("zz-phrases", rightYaml));

        Assert.Empty(left.Diagnostics);
        Assert.Empty(right.Diagnostics);

        var differences = HumanizerSourceGenerator.LocaleSemanticDiff.Compare(left.Locales, right.Locales);

        Assert.Contains(differences, static d => d.Contains("changed semantic behavior", StringComparison.Ordinal));
    }

    [Fact]
    public void SemanticDiff_IdenticalPhrases_NoDifferences()
    {
        var yaml = """
locale: 'zz-same'
surfaces:
  phrases:
    relativeDate:
      now: 'now'
      past:
        day:
          single: 'yesterday'
          multiple:
            forms:
              default: 'days'
              singular: 'day'
      future:
        day:
          single: 'tomorrow'
          multiple:
            forms:
              default: 'days'
    duration:
      zero: 'zero'
      day:
        single: 'day'
        multiple:
          forms:
            default: 'days'
    dataUnits:
      byte:
        forms:
          default: 'bytes'
          singular: 'byte'
    timeUnits:
      second:
        forms:
          default: 'seconds'
          singular: 'second'
""";

        var left = CreateCatalog(("zz-same", yaml));
        var right = CreateCatalog(("zz-same", yaml));

        Assert.Empty(left.Diagnostics);
        Assert.Empty(right.Diagnostics);

        var differences = HumanizerSourceGenerator.LocaleSemanticDiff.Compare(left.Locales, right.Locales);

        Assert.Empty(differences);
    }

    [Fact]
    public void SemanticDiff_WithNamedTemplates_IncludesTemplateInFingerprint()
    {
        var leftYaml = """
locale: 'zz-tmpl'
surfaces:
  phrases:
    relativeDate:
      now: 'now'
      past:
        day:
          single: 'yesterday'
          multiple:
            forms:
              default: 'days'
            template:
              name: 'ago'
              value: '{count} days ago'
      future:
        day:
          single: 'tomorrow'
    duration:
      zero: 'zero'
      day:
        single: 'day'
        multiple:
          forms:
            default: 'days'
          template:
            name: 'duration'
            value: '{count} days'
    dataUnits:
      byte:
        forms:
          default: 'bytes'
        template:
          name: 'unit'
          value: '{count} {unit}'
    timeUnits:
      second:
        forms:
          default: 'seconds'
        template:
          name: 'unit'
          value: '{count} {unit}'
""";

        var rightYaml = leftYaml.Replace("'{count} days ago'", "'{count} jours depuis'");

        var left = CreateCatalog(("zz-tmpl", leftYaml));
        var right = CreateCatalog(("zz-tmpl", rightYaml));

        Assert.Empty(left.Diagnostics);
        Assert.Empty(right.Diagnostics);

        var differences = HumanizerSourceGenerator.LocaleSemanticDiff.Compare(left.Locales, right.Locales);

        Assert.NotEmpty(differences);
    }

    [Fact]
    public void SemanticDiff_NormalizeJson_HandlesIntegerArrayAndBoolValueKinds()
    {
        // Exercises NormalizeJson branches for arrays, integer numbers, booleans, and string values.
        // The double (non-integer) and null branches in NormalizeJson are not reachable through
        // the YAML parser because unquoted scalars only emit integers and the parser has no
        // null-valued JSON element path.
        var yaml = """
locale: 'zz-json-kinds'
surfaces:
  number:
    parse:
      engine: 'token-map'
      normalizationProfile: 'LowercaseRemovePeriods'
      cardinalMap:
        one: 1
        big: 9999999999
      ordinalMap:
        first: 1
      scaleThreshold: 1000
      allowInvariantIntegerInput: true
      negativePrefixes:
        - 'minus '
""";

        var left = CreateCatalog(("zz-json-kinds", yaml));
        var right = CreateCatalog(("zz-json-kinds", yaml));

        Assert.Empty(left.Diagnostics);
        Assert.Empty(right.Diagnostics);

        var diffs = HumanizerSourceGenerator.LocaleSemanticDiff.Compare(left.Locales, right.Locales);
        Assert.Empty(diffs);
    }

    [Fact]
    public void SemanticDiff_NormalizeJson_OmitsFalseDefaultsForAllowInvariantIntegerInput()
    {
        // Tests the OmittedFalseDefaults branch: allowInvariantIntegerInput=false is omitted
        var leftYaml = """
locale: 'zz-omit-false'
surfaces:
  number:
    parse:
      engine: 'token-map'
      normalizationProfile: 'LowercaseRemovePeriods'
      cardinalMap:
        one: 1
      allowInvariantIntegerInput: false
""";

        var rightYaml = """
locale: 'zz-omit-false'
surfaces:
  number:
    parse:
      engine: 'token-map'
      normalizationProfile: 'LowercaseRemovePeriods'
      cardinalMap:
        one: 1
""";

        var left = CreateCatalog(("zz-omit-false", leftYaml));
        var right = CreateCatalog(("zz-omit-false", rightYaml));

        Assert.Empty(left.Diagnostics);
        Assert.Empty(right.Diagnostics);

        var diffs = HumanizerSourceGenerator.LocaleSemanticDiff.Compare(left.Locales, right.Locales);
        Assert.Empty(diffs);
    }

    // ──────────────────────────────────────────────────────────────────────
    //  LocaleCatalogInput — feature scalar resolution
    // ──────────────────────────────────────────────────────────────────────

    [Fact]
    public void Catalog_OxfordListWithoutTemplates_ResolvesAsMappedFeature()
    {
        // The oxford engine without templates passes through NormalizeListSurface as-is,
        // then CreateMappedFeature handles it via the collectionFormatter mapping branch.
        // The ResolveFeature scalar branch (SimpleYamlScalar -> LocaleFeature) is unreachable
        // through canonical authoring because all surfaces are validated as mappings.
        var yaml = """
locale: 'zz-oxford-cf'
surfaces:
  list:
    engine: 'oxford'
""";

        var catalog = CreateCatalog(("zz-oxford-cf", yaml));

        Assert.Empty(catalog.Diagnostics);
        var locale = catalog.Locales.Single(static l => l.LocaleCode == "zz-oxford-cf");
        Assert.Equal("oxford", locale.CollectionFormatter!.Kind);
    }

    [Fact]
    public void Catalog_FormatterProfileDetection_IsCorrect()
    {
        var yaml = """
locale: 'zz-fmt-prof'
surfaces:
  formatter:
    engine: 'profiled'
    pluralRule: 'russian'
""";

        var catalog = CreateCatalog(("zz-fmt-prof", yaml));

        Assert.Empty(catalog.Diagnostics);
        Assert.Contains("zz-fmt-prof", catalog.DataBackedFormatterProfiles);
    }

    [Fact]
    public void Catalog_OrdinalizerProfileDetection_IsCorrect()
    {
        var yaml = """
locale: 'zz-ord-prof'
surfaces:
  ordinal:
    numeric:
      engine: 'template'
      masculine:
        defaultSuffix: 'th'
""";

        var catalog = CreateCatalog(("zz-ord-prof", yaml));

        Assert.Empty(catalog.Diagnostics);
        Assert.Contains("zz-ord-prof", catalog.DataBackedOrdinalizerProfiles);
    }

    // ──────────────────────────────────────────────────────────────────────
    //  LocaleCatalogInput — Norwegian language family
    // ──────────────────────────────────────────────────────────────────────

    [Theory]
    [InlineData("nb", "nn")]
    [InlineData("nb", "no")]
    [InlineData("nn", "nb")]
    public void Catalog_NorwegianFamilySharing_AllowsInheritance(string parent, string child)
    {
        var catalog = CreateCatalog(
            (parent, $"locale: '{parent}'\nsurfaces: {{}}\n"),
            (child, $"locale: '{child}'\nvariantOf: '{parent}'\nsurfaces: {{}}\n"));

        Assert.Empty(catalog.Diagnostics);
        Assert.Equal(2, catalog.Locales.Length);
    }

    // ──────────────────────────────────────────────────────────────────────
    //  LocaleCatalogInput — ToJsonElement / WriteJson branches
    // ──────────────────────────────────────────────────────────────────────

    [Fact]
    public void ToJsonElement_WritesScalarTypes_Correctly()
    {
        var mapping = new HumanizerSourceGenerator.SimpleYamlMapping(
            ImmutableDictionary<string, HumanizerSourceGenerator.SimpleYamlValue>.Empty
                .WithComparers(StringComparer.Ordinal)
                .Add("str", new HumanizerSourceGenerator.SimpleYamlScalar("hello", true))
                .Add("num", new HumanizerSourceGenerator.SimpleYamlScalar("42", false))
                .Add("boolTrue", new HumanizerSourceGenerator.SimpleYamlScalar("true", false))
                .Add("boolFalse", new HumanizerSourceGenerator.SimpleYamlScalar("false", false))
                .Add("nullVal", new HumanizerSourceGenerator.SimpleYamlScalar("null", false))
                .Add("unquoted", new HumanizerSourceGenerator.SimpleYamlScalar("bare", false)));

        var json = HumanizerSourceGenerator.LocaleCatalogInput.ToJsonElement(mapping);

        Assert.Equal("hello", json.GetProperty("str").GetString());
        Assert.Equal(42, json.GetProperty("num").GetInt64());
        Assert.True(json.GetProperty("boolTrue").GetBoolean());
        Assert.False(json.GetProperty("boolFalse").GetBoolean());
        Assert.Equal(System.Text.Json.JsonValueKind.Null, json.GetProperty("nullVal").ValueKind);
        Assert.Equal("bare", json.GetProperty("unquoted").GetString());
    }

    [Fact]
    public void ToJsonElement_WritesSequence_Correctly()
    {
        var seq = new HumanizerSourceGenerator.SimpleYamlSequence(ImmutableArray.Create<HumanizerSourceGenerator.SimpleYamlValue>(
            new HumanizerSourceGenerator.SimpleYamlScalar("a", true),
            new HumanizerSourceGenerator.SimpleYamlScalar("1", false)));

        var json = HumanizerSourceGenerator.LocaleCatalogInput.ToJsonElement(seq);

        Assert.Equal(System.Text.Json.JsonValueKind.Array, json.ValueKind);
        Assert.Equal(2, json.GetArrayLength());
        Assert.Equal("a", json[0].GetString());
        Assert.Equal(1, json[1].GetInt64());
    }

    // ──────────────────────────────────────────────────────────────────────
    //  Helpers
    // ──────────────────────────────────────────────────────────────────────

    static HumanizerSourceGenerator.LocaleCatalogInput CreateCatalogFromFixture(string fixtureName, string localeCode)
    {
        var fixturePath = Path.Combine(
            FixtureLoader.GetFixtureDirectory("CanonicalDiagnostics"),
            fixtureName + ".yml");
        var fileText = File.ReadAllText(fixturePath);
        return CreateCatalog((localeCode, fileText));
    }

    static HumanizerSourceGenerator.LocaleCatalogInput CreateCatalog(params (string LocaleCode, string FileText)[] files) =>
        HumanizerSourceGenerator.LocaleCatalogInput.Create(ImmutableArray.CreateRange(
            files.Select(static file => (HumanizerSourceGenerator.LocaleDefinitionFile?)new HumanizerSourceGenerator.LocaleDefinitionFile(file.LocaleCode, file.FileText))));

    static void AssertDiagnosticContains(HumanizerSourceGenerator.LocaleCatalogInput catalog, string diagnosticId, string substring)
    {
        Assert.Contains(
            catalog.Diagnostics,
            diagnostic => diagnostic.Id == diagnosticId &&
                diagnostic.GetMessage().Contains(substring, StringComparison.Ordinal));
    }

    static string MakeHeadingSequence() =>
        string.Concat(Enumerable.Range(0, 16).Select(static _ => "      - 'N'\n"));

    sealed class TestAdditionalText(string path, string text) : Microsoft.CodeAnalysis.AdditionalText
    {
        public override string Path => path;

        public override Microsoft.CodeAnalysis.Text.SourceText GetText(System.Threading.CancellationToken cancellationToken = default) =>
            Microsoft.CodeAnalysis.Text.SourceText.From(text, System.Text.Encoding.UTF8);
    }

    sealed class NullTextAdditionalText(string path) : Microsoft.CodeAnalysis.AdditionalText
    {
        public override string Path => path;

        public override Microsoft.CodeAnalysis.Text.SourceText? GetText(System.Threading.CancellationToken cancellationToken = default) =>
            null;
    }
}

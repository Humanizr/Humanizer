using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using Xunit;

namespace Humanizer.SourceGenerators.Tests;

public partial class HumanizerSourceGeneratorTests
{
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
    [InlineData("\u1C89")]
    [InlineData("\u1C8A")]
    public void Unicode16CyrillicLettersAreAcceptedOnOlderGeneratorHosts(
        string form)
    {
        var locale = CompleteInflectionFixtureFor("zz", "Cyrl").Replace(
            "'а'",
            $"'{form}'",
            StringComparison.Ordinal);
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText("src/Humanizer/Locales/zz.yml", locale));

        Assert.Empty(runResult.Diagnostics);
    }

    [Fact]
    public void Unicode16AdjacentUnassignedScalarRemainsRejected()
    {
        var locale = CompleteInflectionFixtureFor("zz", "Cyrl").Replace(
            "'а'",
            "'\u1C8B'",
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

    [Theory]
    [InlineData("suffix: 'y'", "suffix: 'ы'")]
    [InlineData("            - 'a'", "            - 'а'")]
    [InlineData("dictionary-plural: '{stem}ies'", "dictionary-plural: '{stem}ы'")]
    [InlineData("one: '{stem}y'", "one: '{stem}ы'")]
    [InlineData("other: '{stem}ies'", "other: '{stem}ы'")]
    [InlineData("            - 'day'", "            - 'день'")]
    public void ProductiveRuleLiteralsMustUseOnlyRuleScriptsWhenOwnerSupportsMore(
        string original,
        string replacement)
    {
        var locale = ProductiveInflectionFixture()
            .Replace(
                "    scripts:\n      - 'Latn'\n    casing:",
                "    scripts:\n      - 'Latn'\n      - 'Cyrl'\n    casing:",
                StringComparison.Ordinal)
            .Replace(original, replacement, StringComparison.Ordinal);
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
    public void ProductiveRuleEmitsItsCompactScriptMask()
    {
        var runResult = RunGeneratorIsolated(
            new InMemoryAdditionalText(
                "src/Humanizer/Locales/zz.yml",
                ProductiveInflectionFixture()));

        Assert.Empty(runResult.Diagnostics);
        var owner = GetGeneratedSource(runResult, "GeneratedInflection_zz.g.cs");
        Assert.Contains(
            "true, false, (InflectionUnicodeScripts)65536u, (InflectionCountability)1)",
            owner,
            StringComparison.Ordinal);
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
using System.Collections.Immutable;
using System.Globalization;

namespace Humanizer.SourceGenerators;

public sealed partial class HumanizerSourceGenerator
{
    internal enum DurationCaseClassification
    {
        Distinct,
        Invariant,
        NotApplicable,
        Unsupported
    }

    internal enum DurationCaseUnitKind
    {
        Phrase,
        SameAsNominative,
        NotApplicable,
        Unsupported
    }

    internal enum DurationCaseRealizationKind
    {
        Authored,
        SameRenderedAs,
        NotApplicable,
        Unsupported
    }

    internal sealed class DurationCaseCatalog(
        string localeCode,
        DurationCaseClassification classification,
        string citationCase,
        ImmutableArray<string> inventory,
        ImmutableDictionary<string, DurationCaseRealization> realizations,
        ImmutableDictionary<string, DurationCaseSource> sources,
        ImmutableDictionary<string, DurationCaseOverlay> cases)
    {
        public string LocaleCode { get; } = localeCode;
        public DurationCaseClassification Classification { get; } = classification;
        public string CitationCase { get; } = citationCase;
        public ImmutableArray<string> Inventory { get; } = inventory;
        public ImmutableDictionary<string, DurationCaseRealization> Realizations { get; } = realizations;
        public ImmutableDictionary<string, DurationCaseSource> Sources { get; } = sources;
        public ImmutableDictionary<string, DurationCaseOverlay> Cases { get; } = cases;
    }

    internal sealed class DurationCaseOverlay(ImmutableDictionary<string, DurationCaseUnit> units)
    {
        public ImmutableDictionary<string, DurationCaseUnit> Units { get; } = units;
    }

    internal sealed class DurationCaseUnit(DurationCaseUnitKind kind, TimeSpanPhrase? phrase)
    {
        public DurationCaseUnitKind Kind { get; } = kind;
        public TimeSpanPhrase? Phrase { get; } = phrase;
    }

    internal sealed class DurationCaseRealization(
        DurationCaseRealizationKind kind,
        string? targetCase,
        ImmutableDictionary<string, DurationCaseUnitRealization> units,
        string? reason,
        ImmutableArray<string> provenance)
    {
        public DurationCaseRealizationKind Kind { get; } = kind;
        public string? TargetCase { get; } = targetCase;
        public ImmutableDictionary<string, DurationCaseUnitRealization> Units { get; } = units;
        public string? Reason { get; } = reason;
        public ImmutableArray<string> Provenance { get; } = provenance;
    }

    internal sealed class DurationCaseUnitRealization(
        DurationCaseRealizationKind kind,
        string? targetCase,
        TimeSpanPhrase? phrase,
        string? reason,
        ImmutableArray<string> provenance)
    {
        public DurationCaseRealizationKind Kind { get; } = kind;
        public string? TargetCase { get; } = targetCase;
        public TimeSpanPhrase? Phrase { get; } = phrase;
        public string? Reason { get; } = reason;
        public ImmutableArray<string> Provenance { get; } = provenance;
    }

    internal sealed class DurationCaseSource(
        string kind,
        string url,
        string revision,
        string locator,
        string credit,
        string? reviewed,
        string? notes)
    {
        public string Kind { get; } = kind;
        public string Url { get; } = url;
        public string Revision { get; } = revision;
        public string Locator { get; } = locator;
        public string Credit { get; } = credit;
        public string? Reviewed { get; } = reviewed;
        public string? Notes { get; } = notes;
    }

    internal sealed class DurationCaseCoverageInput(
        ImmutableArray<DurationCaseCatalog> catalogs,
        ImmutableArray<DurationCaseCoverageRow> rows)
    {
        public ImmutableArray<DurationCaseCatalog> Catalogs { get; } = catalogs;
        public ImmutableArray<DurationCaseCoverageRow> Rows { get; } = rows;

        public static DurationCaseCoverageInput Create(LocaleCatalogInput localeCatalog)
        {
            if (!localeCatalog.Diagnostics.IsEmpty)
            {
                throw new InvalidOperationException(
                    string.Join(
                        "\n",
                        localeCatalog.Diagnostics
                            .Select(static diagnostic => diagnostic.GetMessage())
                            .OrderBy(static message => message, StringComparer.Ordinal)));
            }

            var catalogs = ImmutableArray.CreateBuilder<DurationCaseCatalog>(localeCatalog.Locales.Length);
            var rows = ImmutableArray.CreateBuilder<DurationCaseCoverageRow>(localeCatalog.Locales.Length);
            foreach (var locale in localeCatalog.Locales)
            {
                if (!locale.ResolvedFeatures.TryGetValue("durationCases", out var value))
                {
                    throw new InvalidOperationException(
                        $"Locale '{locale.LocaleCode}' must classify the durationCases surface.");
                }

                locale.ResolvedFeatures.TryGetValue("phrases", out var phrases);
                var catalog = DurationCaseNormalization.Parse(
                    locale.LocaleCode,
                    value,
                    GetExplicitDurationCaseNumberDetector(locale),
                    phrases);
                catalogs.Add(catalog);
                rows.Add(new DurationCaseCoverageRow(
                    locale.LocaleCode,
                    locale.AuthoredFeatureNames.Contains("durationCases")
                        ? catalog.Classification switch
                        {
                            DurationCaseClassification.Distinct => "distinct",
                            DurationCaseClassification.Invariant => "invariant",
                            DurationCaseClassification.NotApplicable => "not-applicable",
                            _ => "unsupported"
                        }
                        : locale.VariantOf is not null
                            ? "same-language-inherited"
                            : throw new InvalidOperationException(
                                $"Locale '{locale.LocaleCode}' has no authored or same-language inherited durationCases classification."),
                    locale.VariantOf));
            }

            var immutableCatalogs = catalogs.MoveToImmutable();
            var legacyCatalogs = immutableCatalogs
                .Where(static catalog => catalog.Sources.IsEmpty)
                .Select(static catalog => catalog.LocaleCode)
                .OrderBy(static locale => locale, StringComparer.Ordinal)
                .ToArray();
            if (legacyCatalogs.Length > 0)
            {
                throw new InvalidOperationException(
                    $"Release duration-case locales must use the explicit inventory, reason, sources, and provenance contract: {string.Join(", ", legacyCatalogs)}.");
            }

            var unsupportedPathBuilder = ImmutableArray.CreateBuilder<string>();
            foreach (var catalog in immutableCatalogs)
            {
                unsupportedPathBuilder.AddRange(GetUnsupportedPaths(catalog));
            }

            var unsupportedPaths = unsupportedPathBuilder
                .OrderBy(static path => path, StringComparer.Ordinal)
                .ToArray();
            if (unsupportedPaths.Length > 0)
            {
                throw new InvalidOperationException(
                    $"Release duration-case completeness requires zero unsupported dispositions; found {unsupportedPaths.Length}: {string.Join(", ", unsupportedPaths)}.");
            }

            return new DurationCaseCoverageInput(immutableCatalogs, rows.MoveToImmutable());
        }

        internal static void ValidateReleaseCompleteness(DurationCaseCatalog catalog)
        {
            var path = $"{catalog.LocaleCode}.durationCases";
            if (catalog.Sources.IsEmpty)
            {
                throw new InvalidOperationException(
                    $"'{path}' must use the explicit inventory, reason, sources, and provenance contract in a release locale.");
            }

            var unsupportedPaths = GetUnsupportedPaths(catalog);
            if (unsupportedPaths.Length > 0)
            {
                throw new InvalidOperationException(
                    $"Release duration-case completeness requires zero unsupported dispositions; found {unsupportedPaths.Length}: {string.Join(", ", unsupportedPaths)}.");
            }
        }

        static ImmutableArray<string> GetUnsupportedPaths(DurationCaseCatalog catalog)
        {
            var paths = ImmutableArray.CreateBuilder<string>();
            var path = $"{catalog.LocaleCode}.durationCases";
            if (catalog.Classification == DurationCaseClassification.Unsupported)
            {
                paths.Add($"{path}.classification");
            }

            foreach (var realization in catalog.Realizations)
            {
                var casePath = $"{path}.cases.{realization.Key}";
                if (realization.Value.Kind == DurationCaseRealizationKind.Unsupported)
                {
                    paths.Add(casePath);
                }

                foreach (var unit in realization.Value.Units)
                {
                    if (unit.Value.Kind == DurationCaseRealizationKind.Unsupported)
                    {
                        paths.Add($"{casePath}.authored.units.{unit.Key}");
                    }
                }
            }

            return paths.ToImmutable();
        }
    }

    internal sealed class DurationCaseCoverageRow(string locale, string classification, string? inheritedFrom)
    {
        public string Locale { get; } = locale;
        public string Classification { get; } = classification;
        public string? InheritedFrom { get; } = inheritedFrom;
    }

    internal static class DurationCaseNormalization
    {
        static readonly string[] TimeUnits =
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

        internal static readonly string[] Cases =
        [
            "nominative",
            "genitive",
            "dative",
            "accusative",
            "instrumental",
            "prepositional",
            "ablative",
            "comitative",
            "ergative",
            "locative",
            "oblique",
            "partitive",
            "vocative",
            "elative",
            "illative",
            "sociative",
            "terminative",
            "translative",
            "absolutive",
            "additive",
            "inessive",
            "allative",
            "adessive",
            "essive",
            "abessive",
            "equative",
            "directive",
            "lative",
            "benefactive",
            "causal"
        ];

        public static DurationCaseCatalog Parse(
            string localeCode,
            SimpleYamlValue value,
            string? pluralRule = null,
            SimpleYamlValue? phrases = null)
        {
            var path = $"{localeCode}.durationCases";
            var mapping = ExpectMapping(value, path);
            var requiredMultipleForms = GetRequiredMultipleForms(pluralRule);
            var catalog = ParseExplicit(
                localeCode,
                mapping,
                path,
                requiredMultipleForms);
            ValidateCitationBaseDurationForms(
                catalog,
                phrases,
                requiredMultipleForms);
            return catalog;
        }

        internal static DurationCaseCatalog ParseLegacyForTests(string localeCode, SimpleYamlValue value)
        {
            var path = $"{localeCode}.durationCases";
            return ParseLegacyForTests(localeCode, ExpectMapping(value, path), path);
        }

        static DurationCaseCatalog ParseLegacyForTests(
            string localeCode,
            SimpleYamlMapping mapping,
            string path)
        {
            RejectUnknownKeys(mapping, path, ["classification", "cases"]);

            var classification = mapping.GetScalar("classification") switch
            {
                "distinct" => DurationCaseClassification.Distinct,
                "same-as-nominative" => DurationCaseClassification.Invariant,
                "not-applicable" => DurationCaseClassification.NotApplicable,
                "unsupported" => DurationCaseClassification.Unsupported,
                { } unsupported => throw new InvalidOperationException(
                    $"'{path}.classification' has unsupported value '{unsupported}'. " +
                    "Supported values: distinct, same-as-nominative, not-applicable, unsupported."),
                null => throw new InvalidOperationException($"'{path}' must define 'classification'.")
            };

            if (classification != DurationCaseClassification.Distinct)
            {
                if (mapping.TryGetValue("cases", out _))
                {
                    throw new InvalidOperationException(
                        $"'{path}.cases' is only valid when classification is 'distinct'.");
                }

                return new DurationCaseCatalog(
                    localeCode,
                    classification,
                    "nominative",
                    classification == DurationCaseClassification.Invariant ? ["nominative"] : [],
                    EmptyRealizations(),
                    EmptySources(),
                    classification == DurationCaseClassification.Invariant
                        ? ImmutableDictionary<string, DurationCaseOverlay>.Empty
                            .WithComparers(StringComparer.Ordinal)
                            .Add("nominative", CreateBaseDurationOverlay())
                        : ImmutableDictionary<string, DurationCaseOverlay>.Empty.WithComparers(StringComparer.Ordinal));
            }

            if (!mapping.TryGetValue("cases", out var casesValue))
            {
                throw new InvalidOperationException($"Distinct duration cases for '{localeCode}' must define 'cases'.");
            }

            var casesMapping = ExpectMapping(casesValue, $"{path}.cases");
            var cases = ImmutableDictionary.CreateBuilder<string, DurationCaseOverlay>(StringComparer.Ordinal);
            foreach (var caseName in casesMapping.Values.Keys.OrderBy(static name => name, StringComparer.Ordinal))
            {
                if (!Cases.Contains(caseName, StringComparer.Ordinal) || caseName == "nominative")
                {
                    throw new InvalidOperationException(
                        $"'{path}.cases' defines unsupported non-nominative case '{caseName}'.");
                }
            }

            foreach (var caseName in Cases)
            {
                if (caseName != "nominative" && casesMapping.TryGetValue(caseName, out var caseValue))
                {
                    cases[caseName] = ParseCase(caseValue, $"{path}.cases.{caseName}");
                }
            }

            if (cases.Count == 0)
            {
                throw new InvalidOperationException($"Distinct duration cases for '{localeCode}' must define at least one case.");
            }

            return new DurationCaseCatalog(
                localeCode,
                classification,
                "nominative",
                ["nominative", .. cases.Keys.OrderBy(static name => name, StringComparer.Ordinal)],
                EmptyRealizations(),
                EmptySources(),
                cases.ToImmutable().Add("nominative", CreateBaseDurationOverlay()));
        }

        internal static DurationCaseCatalog ParseForTests(
            string localeCode,
            string text,
            string? casePluralRule = null,
            string? phrasesText = null)
        {
            var root = SimpleYamlParser.Parse($"durationCases:\n{text}");
            if (!root.TryGetValue("durationCases", out var value))
            {
                throw new InvalidOperationException("Test input must define durationCases.");
            }

            SimpleYamlValue? phrases = null;
            if (phrasesText is not null)
            {
                var phrasesRoot = SimpleYamlParser.Parse($"phrases:\n{phrasesText}");
                _ = phrasesRoot.TryGetValue("phrases", out phrases);
            }

            var path = $"{localeCode}.durationCases";
            var mapping = ExpectMapping(value, path);
            return mapping.TryGetValue("inventory", out _) ||
                   mapping.TryGetValue("sources", out _) ||
                   mapping.TryGetValue("provenance", out _) ||
                   mapping.TryGetValue("reason", out _)
                ? Parse(localeCode, value, casePluralRule, phrases)
                : ParseLegacyForTests(localeCode, mapping, path);
        }

        static DurationCaseCatalog ParseExplicit(
            string localeCode,
            SimpleYamlMapping mapping,
            string path,
            ImmutableArray<string> requiredMultipleForms)
        {
            RejectUnknownKeys(
                mapping,
                path,
                ["classification", "citationCase", "inventory", "sources", "cases", "reason", "provenance"]);

            var classification = mapping.GetScalar("classification") switch
            {
                "distinct" => DurationCaseClassification.Distinct,
                "invariant" => DurationCaseClassification.Invariant,
                "not-applicable" => DurationCaseClassification.NotApplicable,
                "unsupported" => DurationCaseClassification.Unsupported,
                { } unsupported => throw new InvalidOperationException(
                    $"'{path}.classification' has unsupported value '{unsupported}'. " +
                    "Supported values: distinct, invariant, not-applicable, unsupported."),
                null => throw new InvalidOperationException($"'{path}' must define 'classification'.")
            };

            var sources = ParseSources(mapping, path);
            if (classification == DurationCaseClassification.NotApplicable)
            {
                var reason = RequireScalar(mapping, "reason", path);
                if (string.IsNullOrWhiteSpace(reason))
                {
                    throw new InvalidOperationException($"'{path}.reason' must not be empty.");
                }

                ValidateProvenance(ParseProvenance(mapping, path, required: true), sources, path);
                if (mapping.TryGetValue("inventory", out _) || mapping.TryGetValue("cases", out _))
                {
                    throw new InvalidOperationException(
                        $"'{path}' must not define inventory or cases when classification is 'not-applicable'.");
                }

                return new DurationCaseCatalog(
                    localeCode,
                    classification,
                    "nominative",
                    [],
                    EmptyRealizations(),
                    sources,
                    ImmutableDictionary<string, DurationCaseOverlay>.Empty.WithComparers(StringComparer.Ordinal));
            }

            var inventory = ParseInventory(mapping, path);
            var citationCase = RequireScalar(mapping, "citationCase", path);
            if (!inventory.Contains(citationCase, StringComparer.Ordinal))
            {
                throw new InvalidOperationException(
                    $"'{path}.citationCase' must name a case present in the inventory.");
            }

            if (!mapping.TryGetValue("cases", out var casesValue))
            {
                throw new InvalidOperationException($"'{path}' must define a realization for every inventory case.");
            }

            var casesMapping = ExpectMapping(casesValue, $"{path}.cases");
            RejectUnknownKeys(casesMapping, $"{path}.cases", inventory);
            var realizations = ImmutableDictionary.CreateBuilder<string, DurationCaseRealization>(StringComparer.Ordinal);
            foreach (var caseName in inventory)
            {
                if (!casesMapping.TryGetValue(caseName, out var realizationValue))
                {
                    throw new InvalidOperationException(
                        $"'{path}.cases' must explicitly realize inventory case '{caseName}'.");
                }

                realizations[caseName] = ParseRealization(
                    caseName,
                    citationCase,
                    realizationValue,
                    $"{path}.cases.{caseName}",
                    sources,
                    requiredMultipleForms);
            }

            var immutableRealizations = realizations.ToImmutable();
            var resolved = ResolveCases(inventory, immutableRealizations, path);
            return new DurationCaseCatalog(
                localeCode,
                classification,
                citationCase,
                inventory,
                immutableRealizations,
                sources,
                resolved);
        }

        static ImmutableArray<string> ParseInventory(SimpleYamlMapping mapping, string path)
        {
            if (!mapping.TryGetValue("inventory", out var inventoryValue) ||
                inventoryValue is not SimpleYamlSequence sequence)
            {
                throw new InvalidOperationException($"'{path}.inventory' must be a sequence.");
            }

            var inventory = ImmutableArray.CreateBuilder<string>(sequence.Items.Length);
            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var item in sequence.Items)
            {
                if (item is not SimpleYamlScalar scalar ||
                    !Cases.Contains(scalar.Value, StringComparer.Ordinal))
                {
                    throw new InvalidOperationException(
                        $"'{path}.inventory' contains an unsupported grammatical case.");
                }

                if (!seen.Add(scalar.Value))
                {
                    throw new InvalidOperationException(
                        $"'{path}.inventory' contains duplicate case '{scalar.Value}'.");
                }

                inventory.Add(scalar.Value);
            }

            if (inventory.Count == 0)
            {
                throw new InvalidOperationException($"'{path}.inventory' must not be empty.");
            }

            return inventory.MoveToImmutable();
        }

        static ImmutableDictionary<string, DurationCaseSource> ParseSources(
            SimpleYamlMapping mapping,
            string path)
        {
            if (!mapping.TryGetValue("sources", out var sourcesValue))
            {
                throw new InvalidOperationException($"'{path}' must define provenance sources.");
            }

            var sourcesMapping = ExpectMapping(sourcesValue, $"{path}.sources");
            var sources = ImmutableDictionary.CreateBuilder<string, DurationCaseSource>(StringComparer.Ordinal);
            foreach (var sourceEntry in sourcesMapping.Values.OrderBy(static entry => entry.Key, StringComparer.Ordinal))
            {
                var sourcePath = $"{path}.sources.{sourceEntry.Key}";
                var source = ExpectMapping(sourceEntry.Value, sourcePath);
                RejectUnknownKeys(
                    source,
                    sourcePath,
                    ["kind", "url", "revision", "locator", "credit", "reviewed", "notes"]);
                var kind = RequireScalar(source, "kind", sourcePath);
                if (kind is not ("cldr" or "grammar" or "corpus" or "native-review" or "native-authored"))
                {
                    throw new InvalidOperationException(
                        $"'{sourcePath}.kind' has unsupported value '{kind}'.");
                }

                var url = RequireScalar(source, "url", sourcePath);
                if (!Uri.TryCreate(url, UriKind.Absolute, out var parsedUrl) ||
                    parsedUrl.Scheme != Uri.UriSchemeHttps)
                {
                    throw new InvalidOperationException($"'{sourcePath}.url' must be an absolute HTTPS URL.");
                }

                var reviewed = source.GetScalar("reviewed");
                if (kind is "native-review" or "native-authored" &&
                    (reviewed is null ||
                     !DateTime.TryParseExact(
                         reviewed,
                         "yyyy-MM-dd",
                         CultureInfo.InvariantCulture,
                         DateTimeStyles.None,
                         out _)))
                {
                    throw new InvalidOperationException(
                        $"'{sourcePath}.reviewed' must be an ISO date for native evidence.");
                }

                sources[sourceEntry.Key] = new DurationCaseSource(
                    kind,
                    url,
                    RequireScalar(source, "revision", sourcePath),
                    RequireScalar(source, "locator", sourcePath),
                    RequireScalar(source, "credit", sourcePath),
                    reviewed,
                    source.GetScalar("notes"));
            }

            if (sources.Count == 0)
            {
                throw new InvalidOperationException($"'{path}.sources' must not be empty.");
            }

            return sources.ToImmutable();
        }

        static DurationCaseRealization ParseRealization(
            string caseName,
            string citationCase,
            SimpleYamlValue value,
            string path,
            ImmutableDictionary<string, DurationCaseSource> sources,
            ImmutableArray<string> requiredMultipleForms)
        {
            var mapping = ExpectMapping(value, path);
            RejectUnknownKeys(
                mapping,
                path,
                ["authored", "sameRenderedAs", "notApplicable", "unsupported", "provenance"]);
            var discriminators = new[]
            {
                mapping.TryGetValue("authored", out _),
                mapping.GetScalar("sameRenderedAs") is not null,
                mapping.GetScalar("notApplicable") is not null,
                mapping.GetScalar("unsupported") is not null
            };
            if (discriminators.Count(static enabled => enabled) != 1)
            {
                throw new InvalidOperationException(
                    $"'{path}' must define exactly one of authored, sameRenderedAs, notApplicable, or unsupported.");
            }

            if (mapping.TryGetValue("authored", out var authoredValue))
            {
                var provenance = ParseProvenance(mapping, path, required: true);
                ValidateProvenance(provenance, sources, path);
                if (authoredValue is SimpleYamlScalar scalar)
                {
                    if (caseName != citationCase ||
                        !string.Equals(scalar.Value, "base-duration", StringComparison.Ordinal))
                    {
                        throw new InvalidOperationException(
                            $"'{path}.authored' may reference base-duration only for citation case '{citationCase}'.");
                    }

                    return new DurationCaseRealization(
                        DurationCaseRealizationKind.Authored,
                        null,
                        EmptyUnitRealizations(),
                        null,
                        provenance);
                }

                var authored = ExpectMapping(authoredValue, $"{path}.authored");
                RejectUnknownKeys(authored, $"{path}.authored", ["units"]);
                if (!authored.TryGetValue("units", out var unitsValue))
                {
                    throw new InvalidOperationException($"'{path}.authored' must define all duration units.");
                }

                return new DurationCaseRealization(
                    DurationCaseRealizationKind.Authored,
                    null,
                    ParseUnitRealizations(
                        unitsValue,
                        $"{path}.authored.units",
                        sources,
                        requiredMultipleForms),
                    null,
                    provenance);
            }

            if (mapping.GetScalar("sameRenderedAs") is { } targetCase)
            {
                var provenance = ParseProvenance(mapping, path, required: true);
                ValidateProvenance(provenance, sources, path);
                return new DurationCaseRealization(
                    DurationCaseRealizationKind.SameRenderedAs,
                    targetCase,
                    EmptyUnitRealizations(),
                    null,
                    provenance);
            }

            if (mapping.GetScalar("notApplicable") is { } reason)
            {
                var provenance = ParseProvenance(mapping, path, required: true);
                ValidateProvenance(provenance, sources, path);
                return new DurationCaseRealization(
                    DurationCaseRealizationKind.NotApplicable,
                    null,
                    EmptyUnitRealizations(),
                    RequireReason(reason, $"{path}.notApplicable"),
                    provenance);
            }

            return new DurationCaseRealization(
                DurationCaseRealizationKind.Unsupported,
                null,
                EmptyUnitRealizations(),
                RequireReason(mapping.GetScalar("unsupported")!, $"{path}.unsupported"),
                ParseProvenance(mapping, path, required: false));
        }

        static ImmutableDictionary<string, DurationCaseUnitRealization> ParseUnitRealizations(
            SimpleYamlValue value,
            string path,
            ImmutableDictionary<string, DurationCaseSource> sources,
            ImmutableArray<string> requiredMultipleForms)
        {
            var mapping = ExpectMapping(value, path);
            RejectUnknownKeys(mapping, path, TimeUnits);
            var units = ImmutableDictionary.CreateBuilder<string, DurationCaseUnitRealization>(StringComparer.Ordinal);
            foreach (var unitName in TimeUnits)
            {
                if (!mapping.TryGetValue(unitName, out var unitValue))
                {
                    throw new InvalidOperationException($"'{path}' must explicitly define '{unitName}'.");
                }

                units[unitName] = ParseUnitRealization(
                    unitValue,
                    $"{path}.{unitName}",
                    sources,
                    requiredMultipleForms);
            }

            return units.ToImmutable();
        }

        static DurationCaseUnitRealization ParseUnitRealization(
            SimpleYamlValue value,
            string path,
            ImmutableDictionary<string, DurationCaseSource> sources,
            ImmutableArray<string> requiredMultipleForms)
        {
            if (ContainsInheritanceMarker(value))
            {
                throw new InvalidOperationException(
                    $"'{path}' contains an unresolved CLDR inheritance marker.");
            }

            var mapping = ExpectMapping(value, path);
            RejectUnknownKeys(
                mapping,
                path,
                ["phrase", "sameRenderedAs", "notApplicable", "unsupported", "provenance"]);
            var discriminators = new[]
            {
                mapping.TryGetValue("phrase", out _),
                mapping.GetScalar("sameRenderedAs") is not null,
                mapping.GetScalar("notApplicable") is not null,
                mapping.GetScalar("unsupported") is not null
            };
            if (discriminators.Count(static enabled => enabled) != 1)
            {
                throw new InvalidOperationException(
                    $"'{path}' must define exactly one of phrase, sameRenderedAs, notApplicable, or unsupported.");
            }

            if (mapping.TryGetValue("phrase", out var phraseValue))
            {
                var provenance = ParseProvenance(mapping, path, required: true);
                ValidateProvenance(provenance, sources, path);
                ValidateReachableMultipleForms(
                    phraseValue,
                    $"{path}.phrase",
                    requiredMultipleForms);
                var phrase = LocalePhraseNormalization.ParseTimeSpanPhrase(
                    phraseValue,
                    $"{path}.phrase",
                    preserveDuplicateForms: !requiredMultipleForms.IsEmpty);
                if (phrase.Single is null || phrase.Multiple?.Forms is null)
                {
                    throw new InvalidOperationException(
                        $"Case overlay '{path}.phrase' must explicitly define singular and numeric multiple forms.");
                }

                return new DurationCaseUnitRealization(
                    DurationCaseRealizationKind.Authored,
                    null,
                    phrase,
                    null,
                    provenance);
            }

            if (mapping.GetScalar("sameRenderedAs") is { } targetCase)
            {
                var provenance = ParseProvenance(mapping, path, required: true);
                ValidateProvenance(provenance, sources, path);
                return new DurationCaseUnitRealization(
                    DurationCaseRealizationKind.SameRenderedAs,
                    targetCase,
                    null,
                    null,
                    provenance);
            }

            if (mapping.GetScalar("notApplicable") is { } reason)
            {
                var provenance = ParseProvenance(mapping, path, required: true);
                ValidateProvenance(provenance, sources, path);
                return new DurationCaseUnitRealization(
                    DurationCaseRealizationKind.NotApplicable,
                    null,
                    null,
                    RequireReason(reason, $"{path}.notApplicable"),
                    provenance);
            }

            return new DurationCaseUnitRealization(
                DurationCaseRealizationKind.Unsupported,
                null,
                null,
                RequireReason(mapping.GetScalar("unsupported")!, $"{path}.unsupported"),
                ParseProvenance(mapping, path, required: false));
        }

        static ImmutableArray<string> GetRequiredMultipleForms(string? pluralRule) =>
            pluralRule switch
            {
                null or "none" => [],
                "singular-plural" => ["default", "plural"],
                "arabic-like" => ["default", "dual", "plural"],
                "arabic-cardinal" => ["default", "zero", "dual", "plural", "many"],
                "between2-and4-paucal" or "polish" => ["default", "paucal"],
                "south-slavic" or "russian" =>
                    ["default", "singular", "paucal"],
                "slovenian" => ["default", "dual", "paucal"],
                "lithuanian" => ["default", "singular", "plural"],
                _ => throw new InvalidOperationException(
                    $"Duration case phrases use unsupported plural rule '{pluralRule}'.")
            };

        static void ValidateReachableMultipleForms(
            SimpleYamlValue phraseValue,
            string path,
            ImmutableArray<string> requiredMultipleForms)
        {
            var phrase = ExpectMapping(phraseValue, path);
            if (!phrase.TryGetValue("multiple", out var multipleValue))
            {
                throw new InvalidOperationException(
                    $"Case overlay '{path}' must explicitly define numeric multiple forms.");
            }

            if (requiredMultipleForms.IsEmpty)
            {
                return;
            }

            var multiple = ExpectMapping(multipleValue, $"{path}.multiple");
            var hasScalarDefault = false;
            SimpleYamlMapping? forms = null;
            if (multiple.TryGetValue("forms", out var formsValue))
            {
                hasScalarDefault = formsValue is SimpleYamlScalar;
                forms = formsValue as SimpleYamlMapping;
            }
            else
            {
                forms = multiple;
            }

            foreach (var requiredForm in requiredMultipleForms)
            {
                if ((requiredForm == "default" && hasScalarDefault) ||
                    forms?.TryGetValue(requiredForm, out _) == true)
                {
                    continue;
                }

                throw new InvalidOperationException(
                    $"Case overlay '{path}.multiple.forms' must explicitly define reachable '{requiredForm}' form.");
            }
        }

        static void ValidateCitationBaseDurationForms(
            DurationCaseCatalog catalog,
            SimpleYamlValue? phrasesValue,
            ImmutableArray<string> requiredMultipleForms)
        {
            if (!catalog.Realizations.TryGetValue(catalog.CitationCase, out var citation) ||
                citation.Kind != DurationCaseRealizationKind.Authored ||
                citation.Units.Count != 0)
            {
                return;
            }

            var path = $"{catalog.LocaleCode}.phrases.duration";
            var phrases = phrasesValue is null
                ? throw new InvalidOperationException(
                    $"Case-aware citation phrases for '{catalog.LocaleCode}' require the duration phrase surface.")
                : ExpectMapping(phrasesValue, $"{catalog.LocaleCode}.phrases");
            if (!phrases.TryGetValue("duration", out var durationValue))
            {
                throw new InvalidOperationException(
                    $"Case-aware citation phrases for '{catalog.LocaleCode}' require '{path}'.");
            }

            var duration = ExpectMapping(durationValue, path);
            foreach (var unitName in TimeUnits)
            {
                if (!duration.TryGetValue(unitName, out var phraseValue))
                {
                    throw new InvalidOperationException(
                        $"Case-aware citation phrases for '{catalog.LocaleCode}' require '{path}.{unitName}'.");
                }

                ValidateReachableMultipleForms(
                    phraseValue,
                    $"{path}.{unitName}",
                    requiredMultipleForms);
            }
        }

        static ImmutableDictionary<string, DurationCaseOverlay> ResolveCases(
            ImmutableArray<string> inventory,
            ImmutableDictionary<string, DurationCaseRealization> realizations,
            string path)
        {
            var cases = ImmutableDictionary.CreateBuilder<string, DurationCaseOverlay>(StringComparer.Ordinal);
            foreach (var caseName in inventory)
            {
                var units = ImmutableDictionary.CreateBuilder<string, DurationCaseUnit>(StringComparer.Ordinal);
                foreach (var unitName in TimeUnits)
                {
                    units[unitName] = ResolveUnit(
                        caseName,
                        unitName,
                        inventory,
                        realizations,
                        new HashSet<string>(StringComparer.Ordinal),
                        path,
                        aliasing: false);
                }

                cases[caseName] = new DurationCaseOverlay(units.ToImmutable());
            }

            return cases.ToImmutable();
        }

        static DurationCaseUnit ResolveUnit(
            string caseName,
            string unitName,
            ImmutableArray<string> inventory,
            ImmutableDictionary<string, DurationCaseRealization> realizations,
            HashSet<string> resolving,
            string path,
            bool aliasing)
        {
            if (!inventory.Contains(caseName, StringComparer.Ordinal) ||
                !realizations.TryGetValue(caseName, out var realization))
            {
                throw new InvalidOperationException(
                    $"'{path}' references absent case '{caseName}'.");
            }

            var node = $"{caseName}/{unitName}";
            if (!resolving.Add(node))
            {
                throw new InvalidOperationException(
                    $"'{path}' contains a sameRenderedAs cycle at '{node}'.");
            }

            DurationCaseUnit result;
            switch (realization.Kind)
            {
                case DurationCaseRealizationKind.Authored:
                    if (realization.Units.IsEmpty)
                    {
                        result = new DurationCaseUnit(DurationCaseUnitKind.SameAsNominative, null);
                        break;
                    }

                    var unit = realization.Units[unitName];
                    result = unit.Kind switch
                    {
                        DurationCaseRealizationKind.Authored =>
                            new DurationCaseUnit(DurationCaseUnitKind.Phrase, unit.Phrase),
                        DurationCaseRealizationKind.SameRenderedAs =>
                            ResolveUnit(
                                unit.TargetCase!,
                                unitName,
                                inventory,
                                realizations,
                                resolving,
                                path,
                                aliasing: true),
                        DurationCaseRealizationKind.NotApplicable =>
                            new DurationCaseUnit(DurationCaseUnitKind.NotApplicable, null),
                        _ => new DurationCaseUnit(DurationCaseUnitKind.Unsupported, null)
                    };
                    break;
                case DurationCaseRealizationKind.SameRenderedAs:
                    result = ResolveUnit(
                        realization.TargetCase!,
                        unitName,
                        inventory,
                        realizations,
                        resolving,
                        path,
                        aliasing: true);
                    break;
                case DurationCaseRealizationKind.NotApplicable:
                    result = new DurationCaseUnit(DurationCaseUnitKind.NotApplicable, null);
                    break;
                default:
                    result = new DurationCaseUnit(DurationCaseUnitKind.Unsupported, null);
                    break;
            }

            _ = resolving.Remove(node);
            if (aliasing && result.Kind is DurationCaseUnitKind.NotApplicable or DurationCaseUnitKind.Unsupported)
            {
                throw new InvalidOperationException(
                    $"'{path}' aliases '{node}' to a case or unit that is not applicable or unsupported.");
            }

            return result;
        }

        static ImmutableArray<string> ParseProvenance(
            SimpleYamlMapping mapping,
            string path,
            bool required)
        {
            if (!mapping.TryGetValue("provenance", out var value))
            {
                return required
                    ? throw new InvalidOperationException($"'{path}' must define provenance.")
                    : [];
            }

            if (value is not SimpleYamlSequence sequence)
            {
                throw new InvalidOperationException($"'{path}.provenance' must be a sequence.");
            }

            var result = ImmutableArray.CreateBuilder<string>(sequence.Items.Length);
            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var item in sequence.Items)
            {
                if (item is not SimpleYamlScalar scalar || string.IsNullOrWhiteSpace(scalar.Value))
                {
                    throw new InvalidOperationException(
                        $"'{path}.provenance' must contain non-empty source identifiers.");
                }

                if (!seen.Add(scalar.Value))
                {
                    throw new InvalidOperationException(
                        $"'{path}.provenance' contains duplicate source '{scalar.Value}'.");
                }

                result.Add(scalar.Value);
            }

            if (required && result.Count == 0)
            {
                throw new InvalidOperationException($"'{path}.provenance' must not be empty.");
            }

            return result.MoveToImmutable();
        }

        static void ValidateProvenance(
            ImmutableArray<string> provenance,
            ImmutableDictionary<string, DurationCaseSource> sources,
            string path)
        {
            foreach (var sourceId in provenance)
            {
                if (!sources.ContainsKey(sourceId))
                {
                    throw new InvalidOperationException(
                        $"'{path}.provenance' references unknown source '{sourceId}'.");
                }
            }
        }

        static string RequireScalar(SimpleYamlMapping mapping, string key, string path) =>
            mapping.GetScalar(key)
            ?? throw new InvalidOperationException($"'{path}' must define scalar '{key}'.");

        static string RequireReason(string reason, string path) =>
            string.IsNullOrWhiteSpace(reason)
                ? throw new InvalidOperationException($"'{path}' must not be empty.")
                : reason;

        static ImmutableDictionary<string, DurationCaseRealization> EmptyRealizations() =>
            ImmutableDictionary<string, DurationCaseRealization>.Empty.WithComparers(StringComparer.Ordinal);

        static DurationCaseOverlay CreateBaseDurationOverlay()
        {
            var units = ImmutableDictionary.CreateBuilder<string, DurationCaseUnit>(StringComparer.Ordinal);
            foreach (var unit in TimeUnits)
            {
                units[unit] = new DurationCaseUnit(DurationCaseUnitKind.SameAsNominative, null);
            }

            return new DurationCaseOverlay(units.ToImmutable());
        }

        static ImmutableDictionary<string, DurationCaseUnitRealization> EmptyUnitRealizations() =>
            ImmutableDictionary<string, DurationCaseUnitRealization>.Empty.WithComparers(StringComparer.Ordinal);

        static ImmutableDictionary<string, DurationCaseSource> EmptySources() =>
            ImmutableDictionary<string, DurationCaseSource>.Empty.WithComparers(StringComparer.Ordinal);

        static DurationCaseOverlay ParseCase(SimpleYamlValue value, string path)
        {
            var mapping = ExpectMapping(value, path);
            RejectUnknownKeys(mapping, path, ["units"]);

            if (!mapping.TryGetValue("units", out var unitsValue))
            {
                throw new InvalidOperationException($"'{path}' must define all duration 'units'.");
            }

            var unitsMapping = ExpectMapping(unitsValue, $"{path}.units");
            RejectUnknownKeys(unitsMapping, $"{path}.units", TimeUnits);
            var units = ImmutableDictionary.CreateBuilder<string, DurationCaseUnit>(StringComparer.Ordinal);
            foreach (var unitName in TimeUnits)
            {
                if (!unitsMapping.TryGetValue(unitName, out var unitValue))
                {
                    throw new InvalidOperationException(
                        $"'{path}.units' must explicitly define '{unitName}'.");
                }

                units[unitName] = ParseUnit(unitValue, $"{path}.units.{unitName}");
            }

            return new DurationCaseOverlay(units.ToImmutable());
        }

        static DurationCaseUnit ParseUnit(SimpleYamlValue value, string path)
        {
            if (ContainsInheritanceMarker(value))
            {
                throw new InvalidOperationException(
                    $"'{path}' contains an unresolved CLDR inheritance marker.");
            }

            var mapping = ExpectMapping(value, path);
            if (mapping.GetScalar("sameAsNominative") is { } sameAsNominative)
            {
                RejectUnknownKeys(mapping, path, ["sameAsNominative"]);
                if (!bool.TryParse(sameAsNominative, out var enabled) || !enabled)
                {
                    throw new InvalidOperationException($"'{path}.sameAsNominative' must be true.");
                }

                return new DurationCaseUnit(DurationCaseUnitKind.SameAsNominative, null);
            }

            if (mapping.GetScalar("unsupported") is { } unsupported)
            {
                RejectUnknownKeys(mapping, path, ["unsupported"]);
                if (!bool.TryParse(unsupported, out var enabled) || !enabled)
                {
                    throw new InvalidOperationException($"'{path}.unsupported' must be true.");
                }

                return new DurationCaseUnit(DurationCaseUnitKind.Unsupported, null);
            }

            var phrase = LocalePhraseNormalization.ParseTimeSpanPhrase(value, path);
            if (phrase.Single is null ||
                phrase.Multiple?.Forms is null)
            {
                throw new InvalidOperationException(
                    $"Case overlay '{path}' must explicitly define singular and numeric multiple forms.");
            }

            return new DurationCaseUnit(DurationCaseUnitKind.Phrase, phrase);
        }

        static bool ContainsInheritanceMarker(SimpleYamlValue value) =>
            value switch
            {
                SimpleYamlScalar scalar => scalar.Value.Contains("↑↑↑", StringComparison.Ordinal),
                SimpleYamlMapping mapping => mapping.Values.Values.Any(ContainsInheritanceMarker),
                SimpleYamlSequence sequence => sequence.Items.Any(ContainsInheritanceMarker),
                _ => false
            };

        static SimpleYamlMapping ExpectMapping(SimpleYamlValue value, string path) =>
            value as SimpleYamlMapping
            ?? throw new InvalidOperationException($"'{path}' must be a mapping.");

        static void RejectUnknownKeys(SimpleYamlMapping mapping, string path, IReadOnlyCollection<string> supported)
        {
            foreach (var key in mapping.Values.Keys)
            {
                if (!supported.Contains(key))
                {
                    throw new InvalidOperationException(
                        $"'{path}' defines unsupported property '{key}'. Supported properties: {string.Join(", ", supported)}.");
                }
            }
        }
    }
}
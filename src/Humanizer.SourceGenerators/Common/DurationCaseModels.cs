using System.Collections.Immutable;

namespace Humanizer.SourceGenerators;

public sealed partial class HumanizerSourceGenerator
{
    internal enum DurationCaseClassification
    {
        Distinct,
        SameAsNominative,
        NotApplicable,
        Unsupported
    }

    internal enum DurationCaseUnitKind
    {
        Phrase,
        SameAsNominative,
        Unsupported
    }

    internal sealed class DurationCaseCatalog(
        string localeCode,
        DurationCaseClassification classification,
        ImmutableDictionary<string, DurationCaseOverlay> cases)
    {
        public string LocaleCode { get; } = localeCode;
        public DurationCaseClassification Classification { get; } = classification;
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

                var catalog = DurationCaseNormalization.Parse(locale.LocaleCode, value);
                catalogs.Add(catalog);
                rows.Add(new DurationCaseCoverageRow(
                    locale.LocaleCode,
                    locale.AuthoredFeatureNames.Contains("durationCases")
                        ? catalog.Classification switch
                        {
                            DurationCaseClassification.Distinct => "distinct",
                            DurationCaseClassification.SameAsNominative => "same-as-nominative",
                            DurationCaseClassification.NotApplicable => "not-applicable",
                            _ => "unsupported"
                        }
                        : locale.VariantOf is not null
                            ? "same-language-inherited"
                            : throw new InvalidOperationException(
                                $"Locale '{locale.LocaleCode}' has no authored or same-language inherited durationCases classification."),
                    locale.VariantOf));
            }

            return new DurationCaseCoverageInput(catalogs.MoveToImmutable(), rows.MoveToImmutable());
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

        static readonly string[] Cases =
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
            "translative"
        ];

        public static DurationCaseCatalog Parse(string localeCode, SimpleYamlValue value)
        {
            var path = $"{localeCode}.durationCases";
            var mapping = ExpectMapping(value, path);
            RejectUnknownKeys(mapping, path, ["classification", "cases"]);

            var classification = mapping.GetScalar("classification") switch
            {
                "distinct" => DurationCaseClassification.Distinct,
                "same-as-nominative" => DurationCaseClassification.SameAsNominative,
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
                    ImmutableDictionary<string, DurationCaseOverlay>.Empty.WithComparers(StringComparer.Ordinal));
            }

            if (!mapping.TryGetValue("cases", out var casesValue))
            {
                throw new InvalidOperationException($"Distinct duration cases for '{localeCode}' must define 'cases'.");
            }

            var casesMapping = ExpectMapping(casesValue, $"{path}.cases");
            var cases = ImmutableDictionary.CreateBuilder<string, DurationCaseOverlay>(StringComparer.Ordinal);
            foreach (var entry in casesMapping.Values)
            {
                if (!Cases.Contains(entry.Key, StringComparer.Ordinal) || entry.Key == "nominative")
                {
                    throw new InvalidOperationException(
                        $"'{path}.cases' defines unsupported non-nominative case '{entry.Key}'.");
                }

                cases[entry.Key] = ParseCase(entry.Value, $"{path}.cases.{entry.Key}");
            }

            if (cases.Count == 0)
            {
                throw new InvalidOperationException($"Distinct duration cases for '{localeCode}' must define at least one case.");
            }

            return new DurationCaseCatalog(localeCode, classification, cases.ToImmutable());
        }

        internal static DurationCaseCatalog ParseForTests(string localeCode, string text)
        {
            var root = SimpleYamlParser.Parse($"durationCases:\n{text}");
            if (!root.TryGetValue("durationCases", out var value))
            {
                throw new InvalidOperationException("Test input must define durationCases.");
            }

            return Parse(localeCode, value);
        }

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
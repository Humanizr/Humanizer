using System.Collections.Immutable;

namespace Humanizer.SourceGenerators;

public sealed partial class HumanizerSourceGenerator
{
    internal sealed class LocaleCoverageInput(
        ImmutableArray<LocaleCoverageCapability> capabilities,
        ImmutableArray<LocaleCoverageRow> locales)
    {
        static readonly ImmutableArray<CapabilityDefinition> Definitions =
        [
            new("list", "Lists", optional: false, ["collectionFormatter"], static locale => locale.CollectionFormatter is not null),
            new("formatter", "Formatter", optional: false, ["formatter"], static locale => locale.Formatter is not null),
            new("phrases", "Phrases", optional: false, ["phrases"], static locale => locale.Phrases is not null),
            new("inflection", "Cardinal plural rules", optional: false, ["inflection"], static locale => locale.Inflection is not null),
            new("numberWords", "Number to words", optional: false, ["numberToWords"], static locale => locale.NumberToWords is not null),
            new("numberParse", "Words to number", optional: false, ["wordsToNumber"], static locale => locale.WordsToNumber is not null),
            new("numericOrdinal", "Numeric ordinals", optional: false, ["ordinalizer"], static locale => locale.Ordinalizer is not null),
            new(
                "dateOrdinal",
                "Date ordinals",
                optional: false,
                ["dateToOrdinalWords", "dateOnlyToOrdinalWords"],
                static locale => locale.DateToOrdinalWords is not null && locale.DateOnlyToOrdinalWords is not null),
            new("clock", "Clock notation", optional: false, ["timeOnlyToClockNotation"], static locale => locale.TimeOnlyToClockNotation is not null),
            new("compass", "Compass headings", optional: false, ["headings"], static locale => locale.Headings is not null),
            new("calendarOverride", "Calendar override", optional: true, ["calendar"], static locale => locale.Calendar is not null),
            new(
                "numberFormattingOverride",
                "Number-format override",
                optional: true,
                ["numberFormatting"],
                static locale => locale.NumberFormatting is not null)
        ];

        public ImmutableArray<LocaleCoverageCapability> Capabilities { get; } = capabilities;
        public ImmutableArray<LocaleCoverageRow> Locales { get; } = locales;

        public static LocaleCoverageInput Create(LocaleCatalogInput catalog)
        {
            if (!catalog.Diagnostics.IsEmpty)
            {
                throw new InvalidOperationException(
                    string.Join(
                        "\n",
                        catalog.Diagnostics
                            .Select(static diagnostic => diagnostic.GetMessage())
                            .OrderBy(static message => message, StringComparer.Ordinal)));
            }
            if (catalog.Locales.IsEmpty)
            {
                throw new InvalidOperationException("Locale coverage requires at least one canonical locale.");
            }

            var rows = ImmutableArray.CreateBuilder<LocaleCoverageRow>(catalog.Locales.Length);
            foreach (var locale in catalog.Locales.OrderBy(static locale => locale.LocaleCode, StringComparer.Ordinal))
            {
                var coverage = ImmutableDictionary.CreateBuilder<string, LocaleCoverageStatus>(StringComparer.Ordinal);
                foreach (var definition in Definitions)
                {
                    var resolved = definition.IsResolved(locale);
                    if (!resolved && !definition.Optional)
                    {
                        throw new InvalidOperationException(
                            $"Locale '{locale.LocaleCode}' does not resolve required capability '{definition.Key}' from checked-in YAML.");
                    }

                    var contributes = definition.AuthoredFeatureNames.Any(locale.AuthoredFeatureNames.Contains);
                    LocaleCoverageStatus status;
                    if (contributes)
                    {
                        status = new LocaleCoverageStatus("own", null);
                    }
                    else if (resolved && locale.VariantOf is not null)
                    {
                        status = new LocaleCoverageStatus("via", locale.VariantOf);
                    }
                    else
                    {
                        status = new LocaleCoverageStatus(definition.Optional ? "platform" : "missing", null);
                    }
                    coverage[definition.Key] = status;
                }

                rows.Add(new LocaleCoverageRow(locale.LocaleCode, locale.VariantOf, coverage.ToImmutable()));
            }

            return new LocaleCoverageInput(
                [.. Definitions.Select(static definition => new LocaleCoverageCapability(definition.Key, definition.Label, definition.Optional))],
                rows.MoveToImmutable());
        }

        sealed class CapabilityDefinition(
            string key,
            string label,
            bool optional,
            ImmutableArray<string> authoredFeatureNames,
            Func<ResolvedLocaleDefinition, bool> isResolved)
        {
            public string Key { get; } = key;
            public string Label { get; } = label;
            public bool Optional { get; } = optional;
            public ImmutableArray<string> AuthoredFeatureNames { get; } = authoredFeatureNames;
            public Func<ResolvedLocaleDefinition, bool> IsResolved { get; } = isResolved;
        }
    }

    internal sealed class LocaleCoverageCapability(string key, string label, bool optional)
    {
        public string Key { get; } = key;
        public string Label { get; } = label;
        public bool Optional { get; } = optional;
    }

    internal sealed class LocaleCoverageRow(
        string locale,
        string? parent,
        ImmutableDictionary<string, LocaleCoverageStatus> capabilities)
    {
        public string Locale { get; } = locale;
        public string? Parent { get; } = parent;
        public ImmutableDictionary<string, LocaleCoverageStatus> Capabilities { get; } = capabilities;
    }

    internal sealed class LocaleCoverageStatus(string kind, string? via)
    {
        public string Kind { get; } = kind;
        public string? Via { get; } = via;
    }
}
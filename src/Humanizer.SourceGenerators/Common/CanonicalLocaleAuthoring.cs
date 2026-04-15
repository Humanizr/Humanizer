using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Humanizer.SourceGenerators;

public sealed partial class HumanizerSourceGenerator
{
    internal sealed class CanonicalLocaleDocument(
        string localeCode,
        string? variantOf,
        SimpleYamlMapping surfaces,
        string canonicalText)
    {
        public string LocaleCode { get; } = localeCode;
        public string? VariantOf { get; } = variantOf;
        public SimpleYamlMapping Surfaces { get; } = surfaces;
        public string CanonicalText { get; } = canonicalText;
    }

    internal static class CanonicalLocaleAuthoring
    {
        /// <summary>
        /// Parses the canonical locale YAML surface used by checked-in locale files.
        /// The schema is intentionally limited to <c>locale</c>, optional <c>variantOf</c>, and
        /// a <c>surfaces</c> mapping containing the supported authoring surfaces.
        /// </summary>
        static readonly string[] SupportedTopLevelNames =
        [
            "locale",
            "variantOf",
            "surfaces"
        ];

        /// <summary>
        /// Supported canonical surfaces in locale YAML.
        /// Number authoring is split into <c>number.words</c> and <c>number.parse</c> so writer
        /// and parser contracts stay aligned.
        /// </summary>
        static readonly string[] SupportedSurfaceNames =
        [
            "list",
            "formatter",
            "phrases",
            "number",
            "ordinal",
            "clock",
            "compass",
            "calendar"
        ];

        internal static CanonicalLocaleDocument Parse(string localeCode, string fileText)
        {
            var root = SimpleYamlParser.Parse(fileText);

            foreach (var property in root.Values.Keys)
            {
                if (!SupportedTopLevelNames.Contains(property, StringComparer.Ordinal))
                {
                    throw new InvalidOperationException(
                        $"Locale '{localeCode}' defines unsupported top-level property '{property}'. " +
                        $"Supported properties: {string.Join(", ", SupportedTopLevelNames)}.");
                }
            }

            var declaredLocale = root.GetScalar("locale")
                ?? throw new InvalidOperationException(
                    $"Locale '{localeCode}' must define required top-level property 'locale'.");

            if (!string.Equals(localeCode, declaredLocale, StringComparison.Ordinal))
            {
                throw new InvalidOperationException(
                    $"Locale '{declaredLocale}' must match file locale '{localeCode}'.");
            }

            var variantOf = root.GetScalar("variantOf");

            SimpleYamlMapping surfaces;
            if (!root.TryGetValue("surfaces", out var surfacesValue))
            {
                if (string.IsNullOrWhiteSpace(variantOf))
                {
                    throw new InvalidOperationException(
                        $"Locale '{localeCode}' must define required top-level property 'surfaces'.");
                }

                surfaces = new SimpleYamlMapping(
                    ImmutableDictionary<string, SimpleYamlValue>.Empty.WithComparers(StringComparer.Ordinal));
            }
            else if (surfacesValue is SimpleYamlMapping surfacesMapping)
            {
                surfaces = surfacesMapping;
            }
            else
            {
                throw new InvalidOperationException($"Locale '{localeCode}.surfaces' must be a mapping.");
            }

            foreach (var surface in surfaces.Values)
            {
                if (!SupportedSurfaceNames.Contains(surface.Key, StringComparer.Ordinal))
                {
                    throw new InvalidOperationException(
                        $"Locale '{localeCode}.surfaces' defines unsupported surface '{surface.Key}'. " +
                        $"Supported surfaces: {string.Join(", ", SupportedSurfaceNames)}.");
                }

                if (surface.Value is not SimpleYamlMapping)
                {
                    throw new InvalidOperationException(
                        $"Locale '{localeCode}.surfaces.{surface.Key}' must be a mapping.");
                }

                RejectExplicitDefaultEngines(localeCode, $"surfaces.{surface.Key}", surface.Value);
            }

            return new CanonicalLocaleDocument(
                localeCode,
                variantOf,
                surfaces,
                NormalizeCanonicalText(fileText));
        }

        internal static string Emit(CanonicalLocaleDocument document) => document.CanonicalText;

        internal static LocaleDefinition ToLocaleDefinition(CanonicalLocaleDocument document)
        {
            var features = ImmutableDictionary.CreateBuilder<string, SimpleYamlValue>(StringComparer.Ordinal);

            foreach (var surface in document.Surfaces.Values)
            {
                if (surface.Value is not SimpleYamlMapping surfaceMapping)
                {
                    throw new InvalidOperationException(
                        $"Locale '{document.LocaleCode}.surfaces.{surface.Key}' must be a mapping.");
                }

                switch (surface.Key)
                {
                    case "list":
                        features["collectionFormatter"] = NormalizeListSurface(document.LocaleCode, surfaceMapping);
                        break;

                    case "formatter":
                        features["formatter"] = surfaceMapping;
                        var grammar = ImmutableDictionary.CreateBuilder<string, SimpleYamlValue>(StringComparer.Ordinal);
                        foreach (var propertyName in new[]
                        {
                            "pluralRule",
                            "dataUnitPluralRule",
                            "dataUnitNonIntegralForm",
                            "prepositionMode",
                            "secondaryPlaceholderMode",
                            "timeUnitGenders"
                        })
                        {
                            if (surfaceMapping.TryGetValue(propertyName, out var propertyValue))
                            {
                                grammar[propertyName] = propertyValue;
                            }
                        }

                        if (grammar.Count > 0)
                        {
                            features["grammar"] = new SimpleYamlMapping(grammar.ToImmutable());
                        }

                        break;

                    case "phrases":
                        features["phrases"] = surfaceMapping;
                        break;

                    case "number":
                        AddNumberFeatures(document.LocaleCode, surfaceMapping, features);
                        break;

                    case "ordinal":
                        AddOrdinalFeatures(document.LocaleCode, surfaceMapping, features);
                        break;

                    case "clock":
                        features["timeOnlyToClockNotation"] = surfaceMapping;
                        break;

                    case "compass":
                        features["headings"] = surfaceMapping;
                        break;

                    case "calendar":
                        AddCalendarFeatures(document.LocaleCode, surfaceMapping, features);
                        break;
                }
            }

            return new LocaleDefinition(document.LocaleCode, document.VariantOf, features.ToImmutable());
        }

        static void AddNumberFeatures(
            string localeCode,
            SimpleYamlMapping numberSurface,
            ImmutableDictionary<string, SimpleYamlValue>.Builder features)
        {
            foreach (var property in numberSurface.Values.Keys)
            {
                if (property is not ("words" or "parse" or "formatting"))
                {
                    throw new InvalidOperationException(
                        $"Locale '{localeCode}.surfaces.number' defines unsupported property '{property}'. Supported properties: words, parse, formatting.");
                }
            }

            if (numberSurface.TryGetValue("words", out var wordsValue))
            {
                if (wordsValue is not SimpleYamlMapping wordsMapping)
                {
                    throw new InvalidOperationException(
                        $"Locale '{localeCode}.surfaces.number.words' must be a mapping, not a scalar or sequence.");
                }

                features["numberToWords"] = wordsMapping;
            }

            if (numberSurface.TryGetValue("parse", out var parseValue))
            {
                if (parseValue is not SimpleYamlMapping parseMapping)
                {
                    throw new InvalidOperationException(
                        $"Locale '{localeCode}.surfaces.number.parse' must be a mapping, not a scalar or sequence.");
                }

                features["wordsToNumber"] = parseMapping;
            }

            if (numberSurface.TryGetValue("formatting", out var fmtValue))
            {
                if (fmtValue is not SimpleYamlMapping fmtMapping)
                {
                    throw new InvalidOperationException(
                        $"Locale '{localeCode}.surfaces.number.formatting' must be a mapping, not a scalar or sequence.");
                }

                ValidateNumberFormattingBlock(localeCode, fmtMapping);
                features["numberFormatting"] = fmtMapping;
            }
        }

        static void ValidateNumberFormattingBlock(string localeCode, SimpleYamlMapping formatting)
        {
            foreach (var property in formatting.Values.Keys)
            {
                if (property is not ("decimalSeparator" or "negativeSign" or "groupSeparator"))
                {
                    throw new InvalidOperationException(
                        $"Locale '{localeCode}.surfaces.number.formatting' defines unsupported property '{property}'. Supported properties: decimalSeparator, negativeSign, groupSeparator.");
                }
            }

            ValidateOptionalFormattingProperty(localeCode, formatting, "decimalSeparator");
            ValidateOptionalFormattingProperty(localeCode, formatting, "negativeSign");
            ValidateOptionalFormattingProperty(localeCode, formatting, "groupSeparator");

            if (formatting.Values.Count == 0)
            {
                throw new InvalidOperationException(
                    $"Locale '{localeCode}.surfaces.number.formatting' must define at least one property.");
            }
        }

        static void ValidateOptionalFormattingProperty(string localeCode, SimpleYamlMapping formatting, string propertyName)
        {
            if (!formatting.TryGetValue(propertyName, out var value))
            {
                return;
            }

            if (value is not SimpleYamlScalar scalar)
            {
                throw new InvalidOperationException(
                    $"Locale '{localeCode}.surfaces.number.formatting.{propertyName}' must be a scalar string, not a mapping or sequence.");
            }

            if (string.IsNullOrEmpty(scalar.Value))
            {
                throw new InvalidOperationException(
                    $"Locale '{localeCode}.surfaces.number.formatting.{propertyName}' must be a non-empty string.");
            }
        }

        static void AddOrdinalFeatures(
            string localeCode,
            SimpleYamlMapping ordinalSurface,
            ImmutableDictionary<string, SimpleYamlValue>.Builder features)
        {
            foreach (var property in ordinalSurface.Values.Keys)
            {
                if (property is not ("numeric" or "date" or "dateOnly"))
                {
                    throw new InvalidOperationException(
                        $"Locale '{localeCode}.surfaces.ordinal' defines unsupported property '{property}'. Supported properties: numeric, date, dateOnly.");
                }
            }

            if (ordinalSurface.TryGetValue("numeric", out var numericValue))
            {
                if (numericValue is not SimpleYamlMapping numericMapping)
                {
                    throw new InvalidOperationException(
                        $"Locale '{localeCode}.surfaces.ordinal.numeric' must be a mapping, not a scalar or sequence.");
                }

                features["ordinalizer"] = numericMapping;
            }

            if (ordinalSurface.TryGetValue("date", out var dateValue))
            {
                if (dateValue is not SimpleYamlMapping dateMapping)
                {
                    throw new InvalidOperationException(
                        $"Locale '{localeCode}.surfaces.ordinal.date' must be a mapping, not a scalar or sequence.");
                }

                features["dateToOrdinalWords"] = dateMapping;
            }

            if (ordinalSurface.TryGetValue("dateOnly", out var dateOnlyValue))
            {
                if (dateOnlyValue is not SimpleYamlMapping dateOnlyMapping)
                {
                    throw new InvalidOperationException(
                        $"Locale '{localeCode}.surfaces.ordinal.dateOnly' must be a mapping, not a scalar or sequence.");
                }

                features["dateOnlyToOrdinalWords"] = dateOnlyMapping;
            }
        }

        static void AddCalendarFeatures(
            string localeCode,
            SimpleYamlMapping calendarSurface,
            ImmutableDictionary<string, SimpleYamlValue>.Builder features)
        {
            foreach (var property in calendarSurface.Values.Keys)
            {
                if (property is not ("months" or "monthsGenitive" or "hijriMonths"))
                {
                    throw new InvalidOperationException(
                        $"Locale '{localeCode}.surfaces.calendar' defines unsupported property '{property}'. Supported properties: months, monthsGenitive, hijriMonths.");
                }
            }

            var hasMonths = calendarSurface.TryGetValue("months", out var monthsValue);
            if (hasMonths)
            {
                if (monthsValue is not SimpleYamlSequence monthsSeq || monthsSeq.Items.Length != 12)
                {
                    throw new InvalidOperationException(
                        $"Locale '{localeCode}.surfaces.calendar.months' must be a sequence of exactly 12 strings.");
                }

                foreach (var item in monthsSeq.Items)
                {
                    if (item is not SimpleYamlScalar)
                    {
                        throw new InvalidOperationException(
                            $"Locale '{localeCode}.surfaces.calendar.months' items must be scalar strings.");
                    }
                }
            }

            if (calendarSurface.TryGetValue("monthsGenitive", out var monthsGenitiveValue))
            {
                if (!hasMonths)
                {
                    throw new InvalidOperationException(
                        $"Locale '{localeCode}.surfaces.calendar.monthsGenitive' requires 'months' to also be present.");
                }

                if (monthsGenitiveValue is not SimpleYamlSequence genitiveSeq || genitiveSeq.Items.Length != 12)
                {
                    throw new InvalidOperationException(
                        $"Locale '{localeCode}.surfaces.calendar.monthsGenitive' must be a sequence of exactly 12 strings.");
                }

                foreach (var item in genitiveSeq.Items)
                {
                    if (item is not SimpleYamlScalar)
                    {
                        throw new InvalidOperationException(
                            $"Locale '{localeCode}.surfaces.calendar.monthsGenitive' items must be scalar strings.");
                    }
                }
            }

            if (calendarSurface.TryGetValue("hijriMonths", out var hijriMonthsValue))
            {
                if (hijriMonthsValue is not SimpleYamlSequence hijriSeq || hijriSeq.Items.Length != 12)
                {
                    throw new InvalidOperationException(
                        $"Locale '{localeCode}.surfaces.calendar.hijriMonths' must be a sequence of exactly 12 strings.");
                }

                foreach (var item in hijriSeq.Items)
                {
                    if (item is not SimpleYamlScalar scalar)
                    {
                        throw new InvalidOperationException(
                            $"Locale '{localeCode}.surfaces.calendar.hijriMonths' items must be scalar strings.");
                    }

                    if (scalar.Value.IndexOfAny(new[] { '\u200E', '\u200F', '\u061C' }) >= 0)
                    {
                        throw new InvalidOperationException(
                            $"Locale '{localeCode}.surfaces.calendar.hijriMonths' must not contain directionality controls (U+200E, U+200F, U+061C).");
                    }
                }
            }

            features["calendar"] = calendarSurface;
        }

        static string NormalizeCanonicalText(string value)
        {
            var normalized = value.Replace("\r\n", "\n");
            normalized = normalized.TrimEnd('\n') + "\n";
            return normalized;
        }

        static readonly HashSet<string> ExplicitDefaultEnginePaths = new(StringComparer.Ordinal)
        {
            "surfaces.ordinal.numeric",
            "surfaces.ordinal.date",
            "surfaces.ordinal.dateOnly",
            "surfaces.clock"
        };

        static void RejectExplicitDefaultEngines(string localeCode, string path, SimpleYamlValue value)
        {
            switch (value)
            {
                case SimpleYamlMapping mapping:
                    if (string.Equals(mapping.GetScalar("engine"), "default", StringComparison.Ordinal))
                    {
                        if (ExplicitDefaultEnginePaths.Contains(path))
                        {
                            return;
                        }

                        throw new InvalidOperationException(
                            $"Locale '{localeCode}.{path}' must omit the block instead of declaring engine: 'default'.");
                    }

                    foreach (var entry in mapping.Values)
                    {
                        RejectExplicitDefaultEngines(localeCode, $"{path}.{entry.Key}", entry.Value);
                    }

                    break;

                case SimpleYamlSequence sequence:
                    for (var index = 0; index < sequence.Items.Length; index++)
                    {
                        RejectExplicitDefaultEngines(localeCode, $"{path}[{index}]", sequence.Items[index]);
                    }

                    break;
            }
        }

        static SimpleYamlMapping NormalizeListSurface(string localeCode, SimpleYamlMapping mapping)
        {
            if (mapping.TryGetValue("value", out _))
            {
                return mapping;
            }

            var engine = mapping.GetScalar("engine")
                ?? throw new InvalidOperationException($"Locale '{localeCode}.surfaces.list' must define 'engine'.");
            var pairTemplate = mapping.GetScalar("pairTemplate");
            var finalTemplate = mapping.GetScalar("finalTemplate") ?? pairTemplate;
            var serialTemplate = mapping.GetScalar("serialTemplate");
            if (engine == "oxford" && pairTemplate is null && finalTemplate is null)
            {
                return mapping;
            }

            if (pairTemplate is null || finalTemplate is null)
            {
                throw new InvalidOperationException(
                    $"Locale '{localeCode}.surfaces.list' must define canonical list templates or legacy 'value'.");
            }

            var values = ImmutableDictionary.CreateBuilder<string, SimpleYamlValue>(StringComparer.Ordinal);
            var oxfordComma = ReadBoolean(mapping, "oxfordComma");
            var cliticizesFinal = ReadBoolean(mapping, "cliticizesFinal");
            values["engine"] = new SimpleYamlScalar(
                oxfordComma || engine == "oxford"
                    ? "oxford"
                    : cliticizesFinal
                        ? "clitic"
                        : engine == "delimited" || pairTemplate == finalTemplate && pairTemplate == serialTemplate
                            ? "delimited"
                            : "conjunction",
                isQuoted: true);

            if (((SimpleYamlScalar)values["engine"]).Value != "oxford")
            {
                values["value"] = new SimpleYamlScalar(ExtractTemplateSeparator(finalTemplate), isQuoted: true);
            }

            return new SimpleYamlMapping(values.ToImmutable());
        }

        static bool ReadBoolean(SimpleYamlMapping mapping, string key) =>
            mapping.TryGetValue(key, out var value) &&
            value is SimpleYamlScalar scalar &&
            bool.TryParse(scalar.Value, out var result) &&
            result;

        static string ExtractTemplateSeparator(string template)
        {
            const string start = "{0}";
            const string end = "{1}";
            if (!template.StartsWith(start, StringComparison.Ordinal) ||
                !template.EndsWith(end, StringComparison.Ordinal))
            {
                throw new InvalidOperationException("Canonical list templates must use '{0}' and '{1}' as an infix template.");
            }

            return template.Substring(start.Length, template.Length - start.Length - end.Length).Trim();
        }
    }

    public static class LegacyLocaleMigration
    {
        static readonly string[] LegacyTopLevelNames =
        [
            "inherits",
            "collectionFormatter",
            "dateOnlyToOrdinalWords",
            "dateToOrdinalWords",
            "formatter",
            "grammar",
            "headings",
            "numberToWords",
            "ordinalizer",
            "phrases",
            "timeOnlyToClockNotation",
            "wordsToNumber"
        ];

        public static string ConvertToCanonicalYaml(string localeCode, string fileText)
        {
            var root = SimpleYamlParser.Parse(fileText);
            foreach (var property in root.Values.Keys)
            {
                if (!LegacyTopLevelNames.Contains(property, StringComparer.Ordinal))
                {
                    throw new InvalidOperationException(
                        $"Legacy locale '{localeCode}' defines unsupported top-level property '{property}'.");
                }
            }

            var builder = new StringBuilder();
            builder.Append("locale: ");
            builder.AppendLine(QuoteScalar(new SimpleYamlScalar(localeCode, true)));

            if (root.GetScalar("inherits") is { } variantOf)
            {
                builder.Append("variantOf: ");
                builder.AppendLine(QuoteScalar(new SimpleYamlScalar(variantOf, true)));
            }

            var hasInherits = root.GetScalar("inherits") is not null;
            var hasSurfaces = root.Values.Keys.Any(static key => key != "inherits");
            if (!hasSurfaces)
            {
                if (hasInherits)
                {
                    return builder.ToString();
                }

                builder.AppendLine();
                builder.AppendLine("surfaces: {}");
                return builder.ToString();
            }

            builder.AppendLine();
            builder.AppendLine("surfaces:");
            AppendMigratedSurface(builder, "list", root, "collectionFormatter", isCollectionFormatter: true);
            AppendMigratedFormatterSurface(builder, root);
            AppendMigratedPhrasesSurface(builder, root);
            AppendMigratedNumberSurface(builder, root);
            AppendMigratedOrdinalSurface(builder, root);
            AppendMigratedSurface(builder, "clock", root, "timeOnlyToClockNotation");
            AppendMigratedSurface(builder, "compass", root, "headings");

            return builder.ToString().TrimEnd('\r', '\n') + "\n";
        }

        static void AppendMigratedSurface(
            StringBuilder builder,
            string surfaceName,
            SimpleYamlMapping root,
            string legacyName,
            bool isCollectionFormatter = false)
        {
            if (!root.TryGetValue(legacyName, out var value))
            {
                return;
            }

            builder.Append("  ");
            builder.Append(surfaceName);
            builder.AppendLine(":");

            if (isCollectionFormatter && value is SimpleYamlScalar scalar)
            {
                AppendIndentedScalar(builder, 4, "engine", scalar);
                builder.AppendLine();
                return;
            }

            AppendYamlValue(builder, value, 4);
            builder.AppendLine();
        }

        static void AppendMigratedFormatterSurface(StringBuilder builder, SimpleYamlMapping root)
        {
            var hasFormatter = root.TryGetValue("formatter", out var formatterValue);
            var hasGrammar = root.TryGetValue("grammar", out var grammarValue);
            if (!hasFormatter && !hasGrammar)
            {
                return;
            }

            builder.AppendLine("  formatter:");

            if (hasFormatter && formatterValue is SimpleYamlMapping formatterMapping)
            {
                AppendYamlMapping(builder, formatterMapping, 4);
            }

            if (hasGrammar && grammarValue is SimpleYamlMapping grammarMapping)
            {
                AppendYamlMapping(builder, grammarMapping, 4);
            }

            builder.AppendLine();
        }

        static void AppendMigratedPhrasesSurface(StringBuilder builder, SimpleYamlMapping root)
        {
            if (!root.TryGetValue("phrases", out var phrasesValue) || phrasesValue is not SimpleYamlMapping phrasesMapping)
            {
                return;
            }

            builder.AppendLine("  phrases:");

            foreach (var entry in phrasesMapping.Values)
            {
                var canonicalName = entry.Key switch
                {
                    "dateHumanize" => "relativeDate",
                    "timeSpan" => "duration",
                    "dataUnit" => "dataUnits",
                    "timeUnit" => "timeUnits",
                    _ => entry.Key
                };

                if (entry.Value is SimpleYamlScalar scalar)
                {
                    AppendIndentedScalar(builder, 4, canonicalName, scalar);
                    continue;
                }

                builder.Append(' ', 4);
                builder.Append(canonicalName);
                builder.AppendLine(":");
                AppendYamlValue(builder, entry.Value, 6);
            }

            builder.AppendLine();
        }

        static void AppendMigratedNumberSurface(StringBuilder builder, SimpleYamlMapping root)
        {
            var hasWords = root.TryGetValue("numberToWords", out var wordsValue);
            var hasParse = root.TryGetValue("wordsToNumber", out var parseValue);
            if (!hasWords && !hasParse)
            {
                return;
            }

            builder.AppendLine("  number:");

            if (hasWords)
            {
                builder.AppendLine("    words:");
                AppendYamlValue(builder, wordsValue!, 6);
            }

            if (hasParse)
            {
                builder.AppendLine("    parse:");
                AppendYamlValue(builder, parseValue!, 6);
            }

            builder.AppendLine();
        }

        static void AppendMigratedOrdinalSurface(StringBuilder builder, SimpleYamlMapping root)
        {
            var hasNumeric = root.TryGetValue("ordinalizer", out var numericValue);
            var hasDate = root.TryGetValue("dateToOrdinalWords", out var dateValue);
            var hasDateOnly = root.TryGetValue("dateOnlyToOrdinalWords", out var dateOnlyValue);
            if (!hasNumeric && !hasDate && !hasDateOnly)
            {
                return;
            }

            builder.AppendLine("  ordinal:");

            if (hasNumeric && numericValue is SimpleYamlMapping numericMapping)
            {
                builder.AppendLine("    numeric:");
                AppendYamlMapping(builder, numericMapping, 6);
            }

            if (hasDate && dateValue is SimpleYamlMapping dateMapping)
            {
                builder.AppendLine("    date:");
                AppendYamlMapping(builder, dateMapping, 6);
            }

            if (hasDateOnly && dateOnlyValue is SimpleYamlMapping dateOnlyMapping)
            {
                builder.AppendLine("    dateOnly:");
                AppendYamlMapping(builder, dateOnlyMapping, 6);
            }

            builder.AppendLine();
        }

        static void AppendYamlValue(StringBuilder builder, SimpleYamlValue value, int indent)
        {
            switch (value)
            {
                case SimpleYamlMapping mapping:
                    AppendYamlMapping(builder, mapping, indent);
                    break;

                case SimpleYamlSequence sequence:
                    AppendYamlSequence(builder, sequence, indent);
                    break;

                case SimpleYamlScalar scalar:
                    builder.Append(' ', indent);
                    builder.AppendLine(QuoteScalar(scalar));
                    break;

                default:
                    throw new InvalidOperationException("Unsupported YAML node.");
            }
        }

        static void AppendYamlMapping(StringBuilder builder, SimpleYamlMapping mapping, int indent)
        {
            foreach (var entry in mapping.Values)
            {
                if (entry.Value is SimpleYamlScalar scalar)
                {
                    AppendIndentedScalar(builder, indent, entry.Key, scalar);
                    continue;
                }

                builder.Append(' ', indent);
                builder.Append(entry.Key);
                builder.AppendLine(":");
                AppendYamlValue(builder, entry.Value, indent + 2);
            }
        }

        static void AppendYamlSequence(StringBuilder builder, SimpleYamlSequence sequence, int indent)
        {
            foreach (var item in sequence.Items)
            {
                if (item is SimpleYamlScalar scalar)
                {
                    builder.Append(' ', indent);
                    builder.Append("- ");
                    builder.AppendLine(QuoteScalar(scalar));
                    continue;
                }

                builder.Append(' ', indent);
                builder.AppendLine("-");
                AppendYamlValue(builder, item, indent + 2);
            }
        }

        static void AppendIndentedScalar(StringBuilder builder, int indent, string name, SimpleYamlScalar value)
        {
            builder.Append(' ', indent);
            builder.Append(name);
            builder.Append(": ");
            builder.AppendLine(QuoteScalar(value));
        }

        static string QuoteScalar(SimpleYamlScalar scalar)
        {
            if (!scalar.IsQuoted)
            {
                return scalar.Value;
            }

            return "'" + scalar.Value.Replace("'", "''") + "'";
        }
    }

    internal static class LocaleSemanticDiff
    {
        static readonly ImmutableHashSet<string> OmittedFalseDefaults = ImmutableHashSet.Create(
            StringComparer.Ordinal,
            "allowInvariantIntegerInput");

        internal static ImmutableArray<string> Compare(
            ImmutableArray<ResolvedLocaleDefinition> left,
            ImmutableArray<ResolvedLocaleDefinition> right)
        {
            var differences = ImmutableArray.CreateBuilder<string>();
            var leftByLocale = left.ToImmutableDictionary(static locale => locale.LocaleCode, StringComparer.Ordinal);
            var rightByLocale = right.ToImmutableDictionary(static locale => locale.LocaleCode, StringComparer.Ordinal);

            var localeCodes = leftByLocale.Keys
                .Union(rightByLocale.Keys, StringComparer.Ordinal)
                .OrderBy(static localeCode => localeCode, StringComparer.Ordinal);

            foreach (var localeCode in localeCodes)
            {
                if (!leftByLocale.TryGetValue(localeCode, out var leftLocale))
                {
                    differences.Add($"Locale '{localeCode}' exists only on the right.");
                    continue;
                }

                if (!rightByLocale.TryGetValue(localeCode, out var rightLocale))
                {
                    differences.Add($"Locale '{localeCode}' exists only on the left.");
                    continue;
                }

                var leftFingerprint = JsonSerializer.Serialize(CreateSemanticFingerprint(leftLocale));
                var rightFingerprint = JsonSerializer.Serialize(CreateSemanticFingerprint(rightLocale));
                if (!string.Equals(leftFingerprint, rightFingerprint, StringComparison.Ordinal))
                {
                    differences.Add($"Locale '{localeCode}' changed semantic behavior.");
                }
            }

            return differences.ToImmutable();
        }

        static object CreateSemanticFingerprint(ResolvedLocaleDefinition locale) =>
            new
            {
                locale.LocaleCode,
                CollectionFormatter = CreateFeatureFingerprint(locale.CollectionFormatter),
                Grammar = locale.Grammar is null ? null : NormalizeJson(LocaleCatalogInput.ToJsonElement(locale.Grammar)),
                Headings = locale.Headings is null ? null : new
                {
                    Full = locale.Headings.Full.ToArray(),
                    Short = locale.Headings.Short.ToArray()
                },
                Phrases = locale.Phrases is null ? null : new
                {
                    DateHumanize = new
                    {
                        locale.Phrases.DateHumanize.Now,
                        locale.Phrases.DateHumanize.Never,
                        Past = CreateDatePhraseMap(locale.Phrases.DateHumanize.Past),
                        Future = CreateDatePhraseMap(locale.Phrases.DateHumanize.Future)
                    },
                    TimeSpan = new
                    {
                        locale.Phrases.TimeSpan.Zero,
                        locale.Phrases.TimeSpan.Age,
                        Units = CreateTimeSpanPhraseMap(locale.Phrases.TimeSpan.Units)
                    },
                    DataUnit = CreateDataUnitPhraseMap(locale.Phrases.DataUnit.Units),
                    TimeUnit = CreateTimeUnitPhraseMap(locale.Phrases.TimeUnit.Units)
                },
                Spellout = CreateFeatureFingerprint(locale.NumberToWords),
                Parse = CreateFeatureFingerprint(locale.WordsToNumber),
                Ordinal = CreateFeatureFingerprint(locale.Ordinalizer),
                OrdinalDate = CreateFeatureFingerprint(locale.DateToOrdinalWords),
                OrdinalDateOnly = CreateFeatureFingerprint(locale.DateOnlyToOrdinalWords),
                Clock = CreateFeatureFingerprint(locale.TimeOnlyToClockNotation),
                Formatter = CreateFeatureFingerprint(locale.Formatter),
                Calendar = locale.Calendar is null ? null : NormalizeJson(LocaleCatalogInput.ToJsonElement(locale.Calendar)),
                NumberFormatting = locale.NumberFormatting is null ? null : NormalizeJson(LocaleCatalogInput.ToJsonElement(locale.NumberFormatting))
            };

        static object? CreateFeatureFingerprint(LocaleFeature? feature)
        {
            if (feature is null)
            {
                return null;
            }

            return new
            {
                feature.Kind,
                feature.Argument,
                ProfileRoot = feature.ProfileRoot.ValueKind == JsonValueKind.Undefined
                    ? null
                    : NormalizeJson(feature.ProfileRoot)
            };
        }

        static IReadOnlyDictionary<string, object> CreateDatePhraseMap(ImmutableDictionary<string, DateHumanizePhrase> phrases) =>
            phrases
                .OrderBy(static entry => entry.Key, StringComparer.Ordinal)
                .ToDictionary(
                    static entry => entry.Key,
                    static entry => (object)new
                    {
                        entry.Value.Single,
                        Multiple = CreateCountedPhrase(entry.Value.Multiple),
                        Template = CreateTemplate(entry.Value.NamedTemplate)
                    },
                    StringComparer.Ordinal);

        static IReadOnlyDictionary<string, object> CreateTimeSpanPhraseMap(ImmutableDictionary<string, TimeSpanPhrase> phrases) =>
            phrases
                .OrderBy(static entry => entry.Key, StringComparer.Ordinal)
                .ToDictionary(
                    static entry => entry.Key,
                    static entry => (object)new
                    {
                        entry.Value.Single,
                        entry.Value.SingleWordsVariant,
                        Multiple = CreateCountedPhrase(entry.Value.Multiple),
                        MultipleWordsVariant = CreateCountedPhrase(entry.Value.MultipleWordsVariant),
                        Template = CreateTemplate(entry.Value.NamedTemplate)
                    },
                    StringComparer.Ordinal);

        static IReadOnlyDictionary<string, object> CreateDataUnitPhraseMap(ImmutableDictionary<string, DataUnitPhrase> phrases) =>
            phrases
                .OrderBy(static entry => entry.Key, StringComparer.Ordinal)
                .ToDictionary(
                    static entry => entry.Key,
                    static entry => (object)new
                    {
                        Forms = CreatePhraseForms(entry.Value.Forms),
                        entry.Value.Symbol,
                        Template = CreateTemplate(entry.Value.NamedTemplate)
                    },
                    StringComparer.Ordinal);

        static IReadOnlyDictionary<string, object> CreateTimeUnitPhraseMap(ImmutableDictionary<string, TimeUnitPhrase> phrases) =>
            phrases
                .OrderBy(static entry => entry.Key, StringComparer.Ordinal)
                .ToDictionary(
                    static entry => entry.Key,
                    static entry => (object)new
                    {
                        Forms = CreatePhraseForms(entry.Value.Forms),
                        entry.Value.Symbol,
                        Template = CreateTemplate(entry.Value.NamedTemplate)
                    },
                    StringComparer.Ordinal);

        static object? CreateCountedPhrase(CountedPhrase? phrase) =>
            phrase is null
                ? null
                : new
                {
                    Forms = CreatePhraseForms(phrase.Forms),
                    phrase.CountPlacement,
                    phrase.BeforeCountText,
                    phrase.AfterCountText,
                    Template = CreateTemplate(phrase.NamedTemplate)
                };

        static object? CreatePhraseForms(PhraseForms? forms) =>
            forms is null
                ? null
                : new
                {
                    forms.Default,
                    forms.Singular,
                    forms.Dual,
                    forms.Paucal,
                    forms.Plural
                };

        static object? CreateTemplate(NamedTemplatePhrase? template) =>
            template is null
                ? null
                : new
                {
                    template.Name,
                    template.Template,
                    Placeholders = template.PlaceholderNames.ToArray()
                };

        static object? NormalizeJson(JsonElement element)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    {
                        var ordered = new SortedDictionary<string, object?>(StringComparer.Ordinal);
                        foreach (var property in element.EnumerateObject().OrderBy(static property => property.Name, StringComparer.Ordinal))
                        {
                            if (property.Value.ValueKind == JsonValueKind.False &&
                                OmittedFalseDefaults.Contains(property.Name))
                            {
                                continue;
                            }

                            ordered[property.Name] = NormalizeJson(property.Value);
                        }

                        return ordered;
                    }

                case JsonValueKind.Array:
                    return element.EnumerateArray()
                        .Select(NormalizeJson)
                        .ToArray();

                case JsonValueKind.String:
                    return element.GetString();

                case JsonValueKind.Number:
                    return element.TryGetInt64(out var integer)
                        ? integer
                        : element.GetDouble();

                case JsonValueKind.True:
                    return true;

                case JsonValueKind.False:
                    return false;

                case JsonValueKind.Null:
                case JsonValueKind.Undefined:
                    return null;

                default:
                    return element.ToString();
            }
        }
    }
}
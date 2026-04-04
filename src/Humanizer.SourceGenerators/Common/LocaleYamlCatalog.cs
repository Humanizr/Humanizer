using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

using Microsoft.CodeAnalysis;

namespace Humanizer.SourceGenerators;

public sealed partial class HumanizerSourceGenerator
{
    /// <summary>
    /// Represents a checked-in locale-owned YAML document under <c>Locales</c>.
    /// These files are the single authoring surface for locale-specific generator data.
    /// </summary>
    internal sealed class LocaleDefinitionFile(string localeCode, string fileText)
    {
        public string LocaleCode { get; } = localeCode;
        public string FileText { get; } = fileText;

        public static LocaleDefinitionFile? Create(AdditionalText additionalText, CancellationToken cancellationToken)
        {
            var path = additionalText.Path.Replace('/', '\\');
            if (!path.Contains("\\Locales\\", StringComparison.OrdinalIgnoreCase) ||
                !path.EndsWith(".yml", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            var fileText = additionalText.GetText(cancellationToken)?.ToString();
            if (string.IsNullOrWhiteSpace(fileText))
            {
                return null;
            }

            return new LocaleDefinitionFile(Path.GetFileNameWithoutExtension(additionalText.Path), fileText!);
        }
    }

    /// <summary>
    /// Parses locale YAML once, resolves <c>variantOf</c> inheritance, and exposes locale-owned
    /// feature data to the profile catalogs, token-map generator, and registry generator.
    /// Supported non-English locales are expected to inherit from localized parents rather than
    /// falling back across languages for number surfaces.
    /// </summary>
    internal sealed class LocaleCatalogInput(
        ImmutableArray<ResolvedLocaleDefinition> locales,
        ImmutableArray<Diagnostic> diagnostics,
        ImmutableHashSet<string> dataBackedFormatterProfiles,
        ImmutableHashSet<string> dataBackedOrdinalizerProfiles)
    {
        // The compiler lowers the canonical locale authoring model into the legacy internal feature
        // buckets that the downstream generators already understand. The checked-in YAML surface is
        // intentionally closed over locale/variantOf/surfaces and a fixed set of canonical surfaces.
        static readonly string[] supportedFeatureNames =
        [
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

        static readonly string[] supportedTopLevelNames =
        [
            "locale",
            "variantOf",
            "surfaces"
        ];

        static readonly string[] supportedSurfaceNames =
        [
            "list",
            "formatter",
            "phrases",
            "number",
            "ordinal",
            "clock",
            "compass"
        ];

        static readonly string[] formatterGrammarKeys =
        [
            "pluralRule",
            "dataUnitPluralRule",
            "dataUnitNonIntegralForm",
            "prepositionMode",
            "secondaryPlaceholderMode",
            "timeUnitGenders"
        ];

        readonly ImmutableArray<ResolvedLocaleDefinition> locales = locales;
        readonly ImmutableArray<Diagnostic> diagnostics = diagnostics;
        readonly ImmutableHashSet<string> dataBackedFormatterProfiles = dataBackedFormatterProfiles;
        readonly ImmutableHashSet<string> dataBackedOrdinalizerProfiles = dataBackedOrdinalizerProfiles;

        public ImmutableArray<ResolvedLocaleDefinition> Locales => locales;
        public ImmutableArray<Diagnostic> Diagnostics => diagnostics;
        public ImmutableHashSet<string> DataBackedFormatterProfiles => dataBackedFormatterProfiles;
        public ImmutableHashSet<string> DataBackedOrdinalizerProfiles => dataBackedOrdinalizerProfiles;

        public static LocaleCatalogInput Create(ImmutableArray<LocaleDefinitionFile?> files)
        {
            var diagnostics = ImmutableArray.CreateBuilder<Diagnostic>();
            var parsedLocales = new Dictionary<string, LocaleDefinition>(StringComparer.Ordinal);

            foreach (var file in files)
            {
                if (file is null)
                {
                    continue;
                }

                try
                {
                    var parsedLocale = ParseLocaleDefinition(file.LocaleCode, file.FileText);
                    if (parsedLocales.ContainsKey(parsedLocale.LocaleCode))
                    {
                        throw new InvalidOperationException($"Locale '{parsedLocale.LocaleCode}' is defined more than once.");
                    }

                    parsedLocales[parsedLocale.LocaleCode] = parsedLocale;
                }
                catch (Exception exception)
                {
                    diagnostics.Add(Diagnostic.Create(
                        HumanizerSourceGenerator.Diagnostics.InvalidLocaleDefinition,
                        Location.None,
                        file.LocaleCode,
                        exception.Message));
                }
            }

            // Resolve inheritance once at the catalog boundary so every downstream generator sees a
            // fully merged locale payload instead of reimplementing merge semantics independently.
            // That keeps supported locale variants on the localized parent chain instead of
            // allowing each generator to guess its own fallback behavior.
            var resolvedLocales = ImmutableArray.CreateBuilder<ResolvedLocaleDefinition>();
            var resolving = new HashSet<string>(StringComparer.Ordinal);
            foreach (var localeCode in parsedLocales.Keys.OrderBy(static locale => locale, StringComparer.Ordinal))
            {
                try
                {
                    ResolveLocale(localeCode, parsedLocales, resolving, resolvedLocales, diagnostics);
                }
                catch (Exception exception)
                {
                    diagnostics.Add(Diagnostic.Create(
                        HumanizerSourceGenerator.Diagnostics.InvalidLocaleDefinition,
                        Location.None,
                        localeCode,
                        exception.Message));
                }
            }

            var formatterProfiles = resolvedLocales
                .Select(static locale => locale.Formatter)
                .Where(static feature => feature is { UsesGeneratedProfile: true })
                .Select(static feature => feature!.ProfileName!)
                .ToImmutableHashSet(StringComparer.Ordinal);

            var ordinalizerProfiles = resolvedLocales
                .Select(static locale => locale.Ordinalizer)
                .Where(static feature => feature is { UsesGeneratedProfile: true })
                .Select(static feature => feature!.ProfileName!)
                .ToImmutableHashSet(StringComparer.Ordinal);

            return new LocaleCatalogInput(
                resolvedLocales.ToImmutable(),
                diagnostics.ToImmutable(),
                formatterProfiles,
                ordinalizerProfiles);
        }

        static LocaleDefinition ParseLocaleDefinition(string localeCode, string fileText)
        {
            var document = CanonicalLocaleAuthoring.Parse(localeCode, fileText);
            return CanonicalLocaleAuthoring.ToLocaleDefinition(document);
        }

        static void MapCanonicalSurfacesToFeatures(
            string localeCode,
            SimpleYamlMapping surfaces,
            ImmutableDictionary<string, SimpleYamlValue>.Builder features)
        {
            foreach (var surface in surfaces.Values)
            {
                if (!supportedSurfaceNames.Contains(surface.Key, StringComparer.Ordinal))
                {
                    throw new InvalidOperationException(
                        $"Locale '{localeCode}.surfaces' defines unsupported surface '{surface.Key}'. " +
                        $"Supported surfaces: {string.Join(", ", supportedSurfaceNames)}.");
                }

                if (surface.Value is not SimpleYamlMapping surfaceMapping)
                {
                    throw new InvalidOperationException(
                        $"Locale '{localeCode}.surfaces.{surface.Key}' must be a mapping.");
                }

                switch (surface.Key)
                {
                    case "list":
                        features["collectionFormatter"] = surfaceMapping;
                        break;

                    case "formatter":
                        AddFormatterFeatures(surfaceMapping, features);
                        break;

                    case "phrases":
                        features["phrases"] = surfaceMapping;
                        break;

                    case "number":
                        AddNumberFeatures(localeCode, surfaceMapping, features);
                        break;

                    case "ordinal":
                        AddOrdinalFeatures(localeCode, surfaceMapping, features);
                        break;

                    case "clock":
                        features["timeOnlyToClockNotation"] = surfaceMapping;
                        break;

                    case "compass":
                        features["headings"] = surfaceMapping;
                        break;
                }
            }
        }

        static void AddNumberFeatures(
            string localeCode,
            SimpleYamlMapping numberSurface,
            ImmutableDictionary<string, SimpleYamlValue>.Builder features)
        {
            foreach (var property in numberSurface.Values.Keys)
            {
                if (property is not ("words" or "parse"))
                {
                    throw new InvalidOperationException(
                        $"Locale '{localeCode}.surfaces.number' defines unsupported property '{property}'. Supported properties: words, parse.");
                }
            }

            if (numberSurface.TryGetValue("words", out var wordsValue))
            {
                if (wordsValue is not SimpleYamlMapping wordsMapping)
                {
                    throw new InvalidOperationException(
                        $"Locale '{localeCode}.surfaces.number.words' must be a mapping.");
                }

                features["numberToWords"] = wordsMapping;
            }

            if (numberSurface.TryGetValue("parse", out var parseValue))
            {
                if (parseValue is not SimpleYamlMapping parseMapping)
                {
                    throw new InvalidOperationException(
                        $"Locale '{localeCode}.surfaces.number.parse' must be a mapping.");
                }

                features["wordsToNumber"] = parseMapping;
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
                        $"Locale '{localeCode}.surfaces.ordinal.numeric' must be a mapping.");
                }

                features["ordinalizer"] = numericMapping;
            }

            if (ordinalSurface.TryGetValue("date", out var dateValue))
            {
                if (dateValue is not SimpleYamlMapping dateMapping)
                {
                    throw new InvalidOperationException(
                        $"Locale '{localeCode}.surfaces.ordinal.date' must be a mapping.");
                }

                features["dateToOrdinalWords"] = dateMapping;
            }

            if (ordinalSurface.TryGetValue("dateOnly", out var dateOnlyValue))
            {
                if (dateOnlyValue is not SimpleYamlMapping dateOnlyMapping)
                {
                    throw new InvalidOperationException(
                        $"Locale '{localeCode}.surfaces.ordinal.dateOnly' must be a mapping.");
                }

                features["dateOnlyToOrdinalWords"] = dateOnlyMapping;
            }
        }

        static void AddFormatterFeatures(
            SimpleYamlMapping formatterSurface,
            ImmutableDictionary<string, SimpleYamlValue>.Builder features)
        {
            var grammarValues = ImmutableDictionary.CreateBuilder<string, SimpleYamlValue>(StringComparer.Ordinal);

            foreach (var entry in formatterSurface.Values)
            {
                if (formatterGrammarKeys.Contains(entry.Key, StringComparer.Ordinal))
                {
                    grammarValues[entry.Key] = entry.Value;
                }
            }

            features["formatter"] = formatterSurface;

            if (grammarValues.Count > 0)
            {
                features["grammar"] = new SimpleYamlMapping(grammarValues.ToImmutable());
            }
        }


        static ResolvedLocaleDefinition ResolveLocale(
            string localeCode,
            Dictionary<string, LocaleDefinition> parsedLocales,
            HashSet<string> resolving,
            ImmutableArray<ResolvedLocaleDefinition>.Builder cache,
            ImmutableArray<Diagnostic>.Builder diagnostics)
        {
            foreach (var cached in cache)
            {
                if (cached.LocaleCode == localeCode)
                {
                    return cached;
                }
            }

            if (!parsedLocales.TryGetValue(localeCode, out var locale))
            {
                throw new InvalidOperationException($"Locale '{localeCode}' is not defined.");
            }

            if (!resolving.Add(localeCode))
            {
                throw new InvalidOperationException($"Locale inheritance cycle detected at '{localeCode}'.");
            }

            ResolvedLocaleDefinition inherited;
            if (string.IsNullOrWhiteSpace(locale.Inherits))
            {
                inherited = ResolvedLocaleDefinition.Empty(localeCode);
            }
            else
            {
                var inheritedLocale = locale.Inherits!;
                if (!parsedLocales.ContainsKey(inheritedLocale))
                {
                    diagnostics.Add(Diagnostic.Create(
                        HumanizerSourceGenerator.Diagnostics.InvalidLocaleDefinition,
                        Location.None,
                        localeCode,
                        $"Inherited locale '{inheritedLocale}' is not defined."));
                    inherited = ResolvedLocaleDefinition.Empty(localeCode);
                }
                else
                {
                    if (!SharesLanguageFamily(localeCode, inheritedLocale))
                    {
                        diagnostics.Add(Diagnostic.Create(
                            HumanizerSourceGenerator.Diagnostics.InvalidLocaleDefinition,
                            Location.None,
                            localeCode,
                            $"Inherited locale '{inheritedLocale}' must stay within the same language family as '{localeCode}'."));
                        inherited = ResolvedLocaleDefinition.Empty(localeCode);
                    }
                    else
                    {
                    inherited = ResolveLocale(inheritedLocale, parsedLocales, resolving, cache, diagnostics);
                    }
                }
            }

            resolving.Remove(localeCode);

            var resolvedFeatures = ImmutableDictionary.CreateBuilder<string, SimpleYamlValue>(StringComparer.Ordinal);
            foreach (var featureName in supportedFeatureNames)
            {
                if (ResolveFeatureValue(locale.LocaleCode, featureName, locale.Features, inherited.ResolvedFeatures) is { } resolvedFeatureValue)
                {
                    resolvedFeatures[featureName] = resolvedFeatureValue;
                }
            }

            var resolvedFeatureMap = resolvedFeatures.ToImmutable();
            var grammar = TryResolveLocalePart(
                locale.LocaleCode,
                diagnostics,
                static (resolvedLocaleCode, features) => ResolveGrammar(resolvedLocaleCode, features),
                resolvedFeatureMap);
            var phraseCatalog = TryResolveLocalePart(
                locale.LocaleCode,
                diagnostics,
                static (resolvedLocaleCode, features) => ResolvePhraseCatalog(resolvedLocaleCode, features),
                resolvedFeatureMap);
            var headings = TryResolveLocalePart(
                locale.LocaleCode,
                diagnostics,
                static (resolvedLocaleCode, features) => ResolveHeadings(resolvedLocaleCode, features),
                resolvedFeatureMap);

            var resolved = new ResolvedLocaleDefinition(
                localeCode,
                resolvedFeatureMap,
                grammar,
                headings,
                phraseCatalog,
                TryResolveLocalePart(
                    locale.LocaleCode,
                    diagnostics,
                    static (resolvedLocaleCode, features) => ResolveFeature(resolvedLocaleCode, "collectionFormatter", features),
                    resolvedFeatureMap),
                TryResolveLocalePart(
                    locale.LocaleCode,
                    diagnostics,
                    static (resolvedLocaleCode, features) => ResolveFeature(resolvedLocaleCode, "dateOnlyToOrdinalWords", features),
                    resolvedFeatureMap),
                TryResolveLocalePart(
                    locale.LocaleCode,
                    diagnostics,
                    static (resolvedLocaleCode, features) => ResolveFeature(resolvedLocaleCode, "dateToOrdinalWords", features),
                    resolvedFeatureMap),
                TryResolveLocalePart(
                    locale.LocaleCode,
                    diagnostics,
                    static (resolvedLocaleCode, features) => ResolveFeature(resolvedLocaleCode, "formatter", features),
                    resolvedFeatureMap),
                TryResolveLocalePart(
                    locale.LocaleCode,
                    diagnostics,
                    static (resolvedLocaleCode, features) => ResolveFeature(resolvedLocaleCode, "numberToWords", features),
                    resolvedFeatureMap),
                TryResolveLocalePart(
                    locale.LocaleCode,
                    diagnostics,
                    static (resolvedLocaleCode, features) => ResolveFeature(resolvedLocaleCode, "ordinalizer", features),
                    resolvedFeatureMap),
                TryResolveLocalePart(
                    locale.LocaleCode,
                    diagnostics,
                    static (resolvedLocaleCode, features) => ResolveFeature(resolvedLocaleCode, "timeOnlyToClockNotation", features),
                    resolvedFeatureMap),
                TryResolveLocalePart(
                    locale.LocaleCode,
                    diagnostics,
                    static (resolvedLocaleCode, features) => ResolveFeature(resolvedLocaleCode, "wordsToNumber", features),
                    resolvedFeatureMap));

            cache.Add(resolved);
            return resolved;
        }

        static bool SharesLanguageFamily(string localeCode, string inheritedLocaleCode) =>
            string.Equals(GetLanguageFamily(localeCode), GetLanguageFamily(inheritedLocaleCode), StringComparison.OrdinalIgnoreCase);

        static string GetLanguageFamily(string localeCode)
        {
            var separatorIndex = localeCode.IndexOf('-');
            return separatorIndex >= 0
                ? localeCode.Substring(0, separatorIndex)
                : localeCode;
        }

        static SimpleYamlValue? ResolveFeatureValue(
            string localeCode,
            string featureName,
            ImmutableDictionary<string, SimpleYamlValue> features,
            ImmutableDictionary<string, SimpleYamlValue> inheritedFeatures)
        {
            features.TryGetValue(featureName, out var localValue);
            inheritedFeatures.TryGetValue(featureName, out var inheritedValue);
            return MergeFeatureValue(localeCode, featureName, inheritedValue, localValue);
        }

        static LocaleFeature? ResolveFeature(
            string localeCode,
            string featureName,
            ImmutableDictionary<string, SimpleYamlValue> features)
        {
            if (!features.TryGetValue(featureName, out var featureValue))
            {
                return null;
            }

            return featureValue switch
            {
                SimpleYamlScalar scalar => new LocaleFeature(
                    ownerLocaleCode: localeCode,
                    featureName: featureName,
                    kind: scalar.Value,
                    argument: null,
                    profileName: null,
                    profileRoot: default,
                    usesGeneratedProfile: false),
                SimpleYamlMapping mapping => CreateMappedFeature(localeCode, featureName, mapping),
                _ => throw new InvalidOperationException(
                    $"Feature '{featureName}' in locale '{localeCode}' must be a scalar or mapping.")
            };
        }

        static SimpleYamlMapping? ResolveGrammar(
            string localeCode,
            ImmutableDictionary<string, SimpleYamlValue> features)
        {
            if (!features.TryGetValue("grammar", out var grammarValue))
            {
                return null;
            }

            return grammarValue as SimpleYamlMapping
                ?? throw new InvalidOperationException($"Locale '{localeCode}.grammar' must be a mapping.");
        }

        static LocalePhraseCatalog? ResolvePhraseCatalog(
            string localeCode,
            ImmutableDictionary<string, SimpleYamlValue> features)
        {
            if (!features.TryGetValue("phrases", out var phraseValue))
            {
                return null;
            }

            return phraseValue is SimpleYamlMapping mapping
                ? LocalePhraseNormalization.Create(localeCode, mapping)
                : throw new InvalidOperationException($"Locale '{localeCode}.phrases' must be a mapping.");
        }

        static HeadingSet? ResolveHeadings(
            string localeCode,
            ImmutableDictionary<string, SimpleYamlValue> features)
        {
            if (!features.TryGetValue("headings", out var headingValue))
            {
                return null;
            }

            if (headingValue is not SimpleYamlMapping mapping)
            {
                throw new InvalidOperationException($"Locale '{localeCode}.headings' must be a mapping.");
            }

            foreach (var property in mapping.Values.Keys)
            {
                if (property is not ("full" or "short"))
                {
                    throw new InvalidOperationException(
                        $"Locale '{localeCode}.headings' defines unsupported property '{property}'. Supported properties: full, short.");
                }
            }

            return new HeadingSet(
                ParseHeadingSequence(mapping, "full", localeCode),
                ParseHeadingSequence(mapping, "short", localeCode));
        }

        static ImmutableArray<string> ParseHeadingSequence(SimpleYamlMapping mapping, string key, string localeCode)
        {
            if (!mapping.TryGetValue(key, out var headingValue))
            {
                throw new InvalidOperationException($"Locale '{localeCode}.headings' must define '{key}'.");
            }

            if (headingValue is not SimpleYamlSequence sequence)
            {
                throw new InvalidOperationException($"Locale '{localeCode}.headings.{key}' must be a sequence.");
            }

            if (sequence.Items.Length != 16)
            {
                throw new InvalidOperationException($"Locale '{localeCode}.headings.{key}' must contain exactly 16 entries.");
            }

            var headings = ImmutableArray.CreateBuilder<string>(16);
            for (var index = 0; index < sequence.Items.Length; index++)
            {
                if (sequence.Items[index] is not SimpleYamlScalar scalar)
                {
                    throw new InvalidOperationException($"Locale '{localeCode}.headings.{key}[{index}]' must be a scalar.");
                }

                headings.Add(scalar.Value);
            }

            return headings.MoveToImmutable();
        }

        static T? TryResolveLocalePart<T>(
            string localeCode,
            ImmutableArray<Diagnostic>.Builder diagnostics,
            Func<string, ImmutableDictionary<string, SimpleYamlValue>, T?> resolver,
            ImmutableDictionary<string, SimpleYamlValue> features)
            where T : class
        {
            try
            {
                return resolver(localeCode, features);
            }
            catch (Exception exception)
            {
                diagnostics.Add(Diagnostic.Create(
                    HumanizerSourceGenerator.Diagnostics.InvalidLocaleDefinition,
                    Location.None,
                    localeCode,
                    exception.Message));
                return null;
            }
        }

        /// <summary>
        /// Merges a child locale feature with its inherited parent feature during generation.
        /// Mappings merge recursively, while scalars and sequences replace the inherited value.
        /// If a child mapping switches to a different <c>engine</c>, the child mapping replaces
        /// the parent mapping entirely so fields from the old engine cannot leak into the new one.
        /// </summary>
        static SimpleYamlValue? MergeFeatureValue(
            string localeCode,
            string featureName,
            SimpleYamlValue? inheritedValue,
            SimpleYamlValue? localValue)
        {
            if (localValue is null)
            {
                return inheritedValue;
            }

            if (inheritedValue is null)
            {
                return localValue;
            }

            if (inheritedValue is SimpleYamlMapping inheritedMapping &&
                localValue is SimpleYamlMapping localMapping)
            {
                var inheritedEngine = inheritedMapping.GetScalar("engine");
                var localEngine = localMapping.GetScalar("engine");
                if (inheritedEngine is not null &&
                    localEngine is not null &&
                    !string.Equals(inheritedEngine, localEngine, StringComparison.Ordinal))
                {
                    return localMapping;
                }

                return MergeMappings($"{localeCode}.{featureName}", inheritedMapping, localMapping);
            }

            return localValue;
        }

        static SimpleYamlMapping MergeMappings(string path, SimpleYamlMapping inheritedMapping, SimpleYamlMapping localMapping)
        {
            var values = inheritedMapping.Values.ToBuilder();
            foreach (var entry in localMapping.Values)
            {
                values[entry.Key] = MergeNestedValue(
                    $"{path}.{entry.Key}",
                    values.TryGetValue(entry.Key, out var inheritedValue) ? inheritedValue : null,
                    entry.Value);
            }

            return new SimpleYamlMapping(values.ToImmutable());
        }

        static SimpleYamlValue MergeNestedValue(string path, SimpleYamlValue? inheritedValue, SimpleYamlValue localValue)
        {
            if (inheritedValue is SimpleYamlMapping inheritedMapping &&
                localValue is SimpleYamlMapping localMapping)
            {
                var inheritedEngine = inheritedMapping.GetScalar("engine");
                var localEngine = localMapping.GetScalar("engine");
                if (inheritedEngine is not null &&
                    localEngine is not null &&
                    !string.Equals(inheritedEngine, localEngine, StringComparison.Ordinal))
                {
                    return localMapping;
                }

                return MergeMappings(path, inheritedMapping, localMapping);
            }

            // Ordered lexical data such as scale lists and token tables stay explicit.
            // A child sequence replaces the parent sequence rather than trying to merge by index.
            return localValue;
        }

        static LocaleFeature CreateMappedFeature(string localeCode, string featureName, SimpleYamlMapping mapping)
        {
            if (featureName == "collectionFormatter")
            {
                var engine = mapping.GetScalar("engine")
                    ?? throw new InvalidOperationException($"Locale '{localeCode}.{featureName}' must declare an 'engine'.");
                return new LocaleFeature(localeCode, featureName, engine, mapping.GetScalar("value"), null, default, false);
            }

            if (featureName == "wordsToNumber" &&
                string.Equals(mapping.GetScalar("engine"), "token-map", StringComparison.Ordinal))
            {
                return new LocaleFeature(localeCode, featureName, "lexicon", localeCode, null, ToJsonElement(NormalizeFeatureReferences(localeCode, featureName, mapping)), false);
            }

            var normalizedMapping = NormalizeFeatureReferences(localeCode, featureName, mapping);
            return new LocaleFeature(
                ownerLocaleCode: localeCode,
                featureName,
                kind: CreateGeneratedProfileName(localeCode),
                argument: null,
                profileName: CreateGeneratedProfileName(localeCode),
                profileRoot: ToJsonElement(normalizedMapping),
                usesGeneratedProfile: true);
        }

        /// <summary>
        /// Rewrites author-facing symbolic references into the concrete profile keys used by the
        /// generated catalogs. Locale-owned YAML should be readable without requiring authors to
        /// know the generator's internal naming scheme, so <c>self</c> means "the profile emitted
        /// from this locale file for the referenced surface".
        /// </summary>
        static SimpleYamlMapping NormalizeFeatureReferences(string localeCode, string featureName, SimpleYamlMapping mapping)
        {
            // Today only WordsToNumber profiles need a cross-surface reference back to the locale's
            // NumberToWords profile in order to synthesize ordinal maps. Keep this normalization
            // narrow until another feature proves it needs the same authoring affordance.
            if (featureName != "wordsToNumber" ||
                mapping.GetScalar("ordinalNumberToWordsKind") is not "self")
            {
                return mapping;
            }

            var values = mapping.Values.SetItem("ordinalNumberToWordsKind", new SimpleYamlScalar(localeCode, isQuoted: true));
            return new SimpleYamlMapping(values);
        }

        internal static JsonElement ToJsonElement(SimpleYamlValue value)
        {
            using var stream = new MemoryStream();
            using (var writer = new Utf8JsonWriter(stream))
            {
                WriteJson(writer, value);
            }

            using var document = JsonDocument.Parse(stream.ToArray());
            return document.RootElement.Clone();
        }

        static void WriteJson(Utf8JsonWriter writer, SimpleYamlValue value)
        {
            switch (value)
            {
                case SimpleYamlScalar scalar:
                    WriteScalar(writer, scalar);
                    break;

                case SimpleYamlSequence sequence:
                    writer.WriteStartArray();
                    foreach (var item in sequence.Items)
                    {
                        WriteJson(writer, item);
                    }

                    writer.WriteEndArray();
                    break;

                case SimpleYamlMapping mapping:
                    writer.WriteStartObject();
                    foreach (var entry in mapping.Values)
                    {
                        writer.WritePropertyName(entry.Key);
                        WriteJson(writer, entry.Value);
                    }

                    writer.WriteEndObject();
                    break;

                default:
                    throw new InvalidOperationException("Unsupported YAML value node.");
            }
        }

        static void WriteScalar(Utf8JsonWriter writer, SimpleYamlScalar scalar)
        {
            if (scalar.IsQuoted)
            {
                writer.WriteStringValue(scalar.Value);
                return;
            }

            if (string.Equals(scalar.Value, "null", StringComparison.Ordinal))
            {
                writer.WriteNullValue();
                return;
            }

            if (string.Equals(scalar.Value, "true", StringComparison.OrdinalIgnoreCase))
            {
                writer.WriteBooleanValue(true);
                return;
            }

            if (string.Equals(scalar.Value, "false", StringComparison.OrdinalIgnoreCase))
            {
                writer.WriteBooleanValue(false);
                return;
            }

            if (long.TryParse(scalar.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var integer))
            {
                writer.WriteNumberValue(integer);
                return;
            }

            writer.WriteStringValue(scalar.Value);
        }

        /// <summary>
        /// Uses the owning locale code as the generated profile key for every profile catalog.
        /// Each surface has its own catalog, so adding the feature name only creates noisy,
        /// internal-only identifiers that authors then have to learn when features reference each
        /// other. Locale code keys keep the YAML model cohesive: one locale file owns one profile
        /// per generated surface, and cross-surface references can stay locale-centric.
        /// </summary>
        static string CreateGeneratedProfileName(string localeCode) => localeCode;
    }

    internal sealed class LocaleDefinition(
        string localeCode,
        string? inherits,
        ImmutableDictionary<string, SimpleYamlValue> features)
    {
        public string LocaleCode { get; } = localeCode;
        public string? Inherits { get; } = inherits;
        public ImmutableDictionary<string, SimpleYamlValue> Features { get; } = features;
    }

    internal sealed class ResolvedLocaleDefinition(
        string localeCode,
        ImmutableDictionary<string, SimpleYamlValue> resolvedFeatures,
        SimpleYamlMapping? grammar,
        HeadingSet? headings,
        LocalePhraseCatalog? phrases,
        LocaleFeature? collectionFormatter,
        LocaleFeature? dateOnlyToOrdinalWords,
        LocaleFeature? dateToOrdinalWords,
        LocaleFeature? formatter,
        LocaleFeature? numberToWords,
        LocaleFeature? ordinalizer,
        LocaleFeature? timeOnlyToClockNotation,
        LocaleFeature? wordsToNumber)
    {
        public string LocaleCode { get; } = localeCode;
        public ImmutableDictionary<string, SimpleYamlValue> ResolvedFeatures { get; } = resolvedFeatures;
        public SimpleYamlMapping? Grammar { get; } = grammar;
        public HeadingSet? Headings { get; } = headings;
        public LocalePhraseCatalog? Phrases { get; } = phrases;
        public LocaleFeature? CollectionFormatter { get; } = collectionFormatter;
        public LocaleFeature? DateOnlyToOrdinalWords { get; } = dateOnlyToOrdinalWords;
        public LocaleFeature? DateToOrdinalWords { get; } = dateToOrdinalWords;
        public LocaleFeature? Formatter { get; } = formatter;
        public LocaleFeature? NumberToWords { get; } = numberToWords;
        public LocaleFeature? Ordinalizer { get; } = ordinalizer;
        public LocaleFeature? TimeOnlyToClockNotation { get; } = timeOnlyToClockNotation;
        public LocaleFeature? WordsToNumber { get; } = wordsToNumber;

        public static ResolvedLocaleDefinition Empty(string localeCode) =>
            new(localeCode, ImmutableDictionary<string, SimpleYamlValue>.Empty.WithComparers(StringComparer.Ordinal), null, null, null, null, null, null, null, null, null, null, null);
    }

    internal sealed class LocaleFeature(
        string ownerLocaleCode,
        string featureName,
        string kind,
        string? argument,
        string? profileName,
        JsonElement profileRoot,
        bool usesGeneratedProfile)
    {
        public string OwnerLocaleCode { get; } = ownerLocaleCode;
        public string FeatureName { get; } = featureName;
        public string Kind { get; } = kind;
        public string? Argument { get; } = argument;
        public string? ProfileName { get; } = profileName;
        public JsonElement ProfileRoot { get; } = profileRoot;
        public bool UsesGeneratedProfile { get; } = usesGeneratedProfile;
    }

    internal abstract class SimpleYamlValue;

    internal sealed class SimpleYamlScalar(string value, bool isQuoted) : SimpleYamlValue
    {
        public string Value { get; } = value;
        public bool IsQuoted { get; } = isQuoted;
    }

    internal sealed class SimpleYamlSequence(ImmutableArray<SimpleYamlValue> items) : SimpleYamlValue
    {
        public ImmutableArray<SimpleYamlValue> Items { get; } = items;
    }

    internal sealed class SimpleYamlMapping : SimpleYamlValue
    {
        readonly ImmutableDictionary<string, SimpleYamlValue> values;

        public SimpleYamlMapping(ImmutableDictionary<string, SimpleYamlValue> values) => this.values = values;

        public ImmutableDictionary<string, SimpleYamlValue> Values => values;

        public bool TryGetValue(string key, out SimpleYamlValue value) =>
            values.TryGetValue(key, out value!);

        public string? GetScalar(string key) =>
            TryGetValue(key, out var value) && value is SimpleYamlScalar scalar ? scalar.Value : null;
    }

    /// <summary>
    /// Minimal YAML parser for the repository-owned locale authoring format.
    /// It intentionally supports only the constructs we check in:
    /// nested mappings, sequences, quoted/plain scalars, booleans, nulls, and integers.
    /// </summary>
    static class SimpleYamlParser
    {
        sealed class LineInfo(int indent, string content, int lineNumber)
        {
            public int Indent { get; } = indent;
            public string Content { get; } = content;
            public int LineNumber { get; } = lineNumber;
        }

        public static SimpleYamlMapping Parse(string text)
        {
            var lines = NormalizeLines(text);
            if (lines.Count == 0)
            {
                return new SimpleYamlMapping(ImmutableDictionary<string, SimpleYamlValue>.Empty.WithComparers(StringComparer.Ordinal));
            }

            var index = 0;
            var value = ParseBlock(lines, ref index, 0);
            if (value is not SimpleYamlMapping mapping)
            {
                throw new InvalidOperationException("Locale YAML root must be a mapping.");
            }

            return mapping;
        }

        static List<LineInfo> NormalizeLines(string text)
        {
            var result = new List<LineInfo>();
            var lines = text.Split(["\r\n", "\n"], StringSplitOptions.None);

            for (var lineNumber = 0; lineNumber < lines.Length; lineNumber++)
            {
                var rawLine = StripComment(lines[lineNumber]);
                if (string.IsNullOrWhiteSpace(rawLine))
                {
                    continue;
                }

                var indent = 0;
                while (indent < rawLine.Length && rawLine[indent] == ' ')
                {
                    indent++;
                }

                if ((indent & 1) != 0)
                {
                    throw new InvalidOperationException($"Unsupported indentation on line {lineNumber + 1}. Locale YAML uses 2-space indentation.");
                }

                result.Add(new LineInfo(indent, rawLine.Trim(), lineNumber + 1));
            }

            return result;
        }

        static string StripComment(string line)
        {
            var builder = new StringBuilder(line.Length);
            var inSingleQuote = false;
            var inDoubleQuote = false;

            for (var i = 0; i < line.Length; i++)
            {
                var c = line[i];
                if (c == '\'' && !inDoubleQuote)
                {
                    if (inSingleQuote && i + 1 < line.Length && line[i + 1] == '\'')
                    {
                        builder.Append("''");
                        i++;
                        continue;
                    }

                    inSingleQuote = !inSingleQuote;
                    builder.Append(c);
                    continue;
                }

                if (c == '"' && !inSingleQuote)
                {
                    inDoubleQuote = !inDoubleQuote;
                    builder.Append(c);
                    continue;
                }

                if (c == '#' && !inSingleQuote && !inDoubleQuote)
                {
                    break;
                }

                builder.Append(c);
            }

            return builder.ToString().TrimEnd();
        }

        static SimpleYamlValue ParseBlock(IReadOnlyList<LineInfo> lines, ref int index, int indent)
        {
            if (index >= lines.Count)
            {
                throw new InvalidOperationException("Unexpected end of YAML document.");
            }

            return IsSequenceItem(lines[index].Content)
                ? ParseSequence(lines, ref index, indent)
                : ParseMapping(lines, ref index, indent);
        }

        static SimpleYamlSequence ParseSequence(IReadOnlyList<LineInfo> lines, ref int index, int indent)
        {
            var items = ImmutableArray.CreateBuilder<SimpleYamlValue>();

            while (index < lines.Count)
            {
                var line = lines[index];
                if (line.Indent < indent)
                {
                    break;
                }

                if (line.Indent != indent)
                {
                    throw new InvalidOperationException($"Unexpected indentation on line {line.LineNumber}.");
                }

                if (!IsSequenceItem(line.Content))
                {
                    break;
                }

                var remainder = line.Content.Length == 1
                    ? string.Empty
                    : line.Content.Substring(2).TrimStart();
                index++;

                if (remainder.Length == 0)
                {
                    items.Add(ParseBlock(lines, ref index, indent + 2));
                    continue;
                }

                var inlineSeparator = FindMappingSeparator(remainder);
                if (inlineSeparator > 0)
                {
                    items.Add(ParseInlineSequenceMapping(lines, ref index, indent + 2, remainder, line.LineNumber));
                    continue;
                }

                items.Add(ParseScalar(remainder));
            }

            return new SimpleYamlSequence(items.ToImmutable());
        }

        static SimpleYamlMapping ParseInlineSequenceMapping(
            IReadOnlyList<LineInfo> lines,
            ref int index,
            int indent,
            string firstEntry,
            int lineNumber)
        {
            var values = ImmutableDictionary.CreateBuilder<string, SimpleYamlValue>(StringComparer.Ordinal);
            ParseMappingEntry(values, firstEntry, indent, lineNumber, lines, ref index);
            ParseMappingContinuation(values, lines, ref index, indent);
            return new SimpleYamlMapping(values.ToImmutable());
        }

        static SimpleYamlMapping ParseMapping(IReadOnlyList<LineInfo> lines, ref int index, int indent)
        {
            var values = ImmutableDictionary.CreateBuilder<string, SimpleYamlValue>(StringComparer.Ordinal);
            ParseMappingContinuation(values, lines, ref index, indent);
            return new SimpleYamlMapping(values.ToImmutable());
        }

        static void ParseMappingContinuation(
            ImmutableDictionary<string, SimpleYamlValue>.Builder values,
            IReadOnlyList<LineInfo> lines,
            ref int index,
            int indent)
        {
            while (index < lines.Count)
            {
                var line = lines[index];
                if (line.Indent < indent)
                {
                    break;
                }

                if (line.Indent != indent)
                {
                    throw new InvalidOperationException($"Unexpected indentation on line {line.LineNumber}.");
                }

                if (IsSequenceItem(line.Content))
                {
                    break;
                }

                ParseMappingEntry(values, line.Content, indent, line.LineNumber, lines, ref index);
            }
        }

        static void ParseMappingEntry(
            ImmutableDictionary<string, SimpleYamlValue>.Builder values,
            string content,
            int indent,
            int lineNumber,
            IReadOnlyList<LineInfo> lines,
            ref int index)
        {
            var separator = FindMappingSeparator(content);
            if (separator <= 0)
            {
                throw new InvalidOperationException($"Invalid mapping entry on line {lineNumber}.");
            }

            var rawKey = content.Substring(0, separator).Trim();
            var key = Unquote(rawKey);
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new InvalidOperationException($"Invalid mapping key on line {lineNumber}.");
            }

            var remainder = content.Substring(separator + 1).TrimStart();
            index++;

            if (remainder.Length == 0)
            {
                values[key] = ParseBlock(lines, ref index, indent + 2);
                return;
            }

            values[key] = ParseScalar(remainder);
        }

        static int FindMappingSeparator(string content)
        {
            var inSingleQuote = false;
            var inDoubleQuote = false;

            for (var i = 0; i < content.Length; i++)
            {
                var c = content[i];
                if (c == '\'' && !inDoubleQuote)
                {
                    if (inSingleQuote && i + 1 < content.Length && content[i + 1] == '\'')
                    {
                        i++;
                        continue;
                    }

                    inSingleQuote = !inSingleQuote;
                    continue;
                }

                if (c == '"' && !inSingleQuote)
                {
                    inDoubleQuote = !inDoubleQuote;
                    continue;
                }

                if (c == ':' && !inSingleQuote && !inDoubleQuote)
                {
                    return i;
                }
            }

            return -1;
        }

        static bool IsSequenceItem(string content) =>
            content == "-" || content.StartsWith("- ", StringComparison.Ordinal);

        static SimpleYamlValue ParseScalar(string value)
        {
            if (value == "[]")
            {
                return new SimpleYamlSequence(ImmutableArray<SimpleYamlValue>.Empty);
            }

            if (value == "{}")
            {
                return new SimpleYamlMapping(ImmutableDictionary<string, SimpleYamlValue>.Empty.WithComparers(StringComparer.Ordinal));
            }

            if (value.Length >= 2 &&
                ((value[0] == '\'' && value[value.Length - 1] == '\'') ||
                 (value[0] == '"' && value[value.Length - 1] == '"')))
            {
                return new SimpleYamlScalar(Unquote(value), true);
            }

            return new SimpleYamlScalar(value, false);
        }

        static string Unquote(string value)
        {
            if (value.Length < 2)
            {
                return value;
            }

            if (value[0] == '\'' && value[value.Length - 1] == '\'')
            {
                return value.Substring(1, value.Length - 2).Replace("''", "'");
            }

            if (value[0] == '"' && value[value.Length - 1] == '"')
            {
                return value.Substring(1, value.Length - 2)
                    .Replace("\\\"", "\"")
                    .Replace("\\\\", "\\");
            }

            return value;
        }
    }
}

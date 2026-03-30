using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Humanizer.SourceGenerators;

public sealed partial class HumanizerSourceGenerator
{
    sealed class RegistryEntry(string locale, string? profile, string? argument)
    {
        public string Locale { get; } = locale;
        public string? Profile { get; } = profile;
        public string? Argument { get; } = argument;
    }

    sealed class LocaleDefinitionFile(string localeCode, string fileText)
    {
        public string LocaleCode { get; } = localeCode;
        public string FileText { get; } = fileText;

        public static LocaleDefinitionFile? Create(AdditionalText additionalText, CancellationToken cancellationToken)
        {
            var path = additionalText.Path.Replace('/', '\\');
            if (!path.Contains("\\CodeGen\\Locales\\", StringComparison.OrdinalIgnoreCase) ||
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

    sealed class LocaleRegistryInput(
        ImmutableArray<ResolvedLocaleDefinition> locales,
        ImmutableArray<Diagnostic> diagnostics,
        ImmutableHashSet<string> dataBackedFormatterProfiles,
        ImmutableHashSet<string> dataBackedOrdinalizerProfiles)
    {
        readonly ImmutableArray<ResolvedLocaleDefinition> locales = locales;
        readonly ImmutableArray<Diagnostic> diagnostics = diagnostics;
        readonly ImmutableHashSet<string> dataBackedFormatterProfiles = dataBackedFormatterProfiles;
        readonly ImmutableHashSet<string> dataBackedOrdinalizerProfiles = dataBackedOrdinalizerProfiles;

        static readonly (string RegistryName, Func<ResolvedLocaleDefinition, LocaleFeature?> FeatureSelector)[] registrySelectors =
        [
            ("CollectionFormatterRegistry", static locale => locale.CollectionFormatter),
            ("DateOnlyToOrdinalWordsConverterRegistry", static locale => locale.DateOnlyToOrdinalWords),
            ("DateToOrdinalWordsConverterRegistry", static locale => locale.DateToOrdinalWords),
            ("FormatterRegistry", static locale => locale.Formatter),
            ("NumberToWordsConverterRegistry", static locale => locale.NumberToWords),
            ("OrdinalizerRegistry", static locale => locale.Ordinalizer),
            ("TimeOnlyToClockNotationConvertersRegistry", static locale => locale.TimeOnlyToClockNotation),
            ("WordsToNumberConverterRegistry", static locale => locale.WordsToNumber)
        ];

        public static LocaleRegistryInput Create(
            ImmutableArray<LocaleDefinitionFile?> files,
            ImmutableHashSet<string> dataBackedFormatterProfiles,
            ImmutableHashSet<string> dataBackedOrdinalizerProfiles)
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
                    parsedLocales[file.LocaleCode] = ParseLocaleDefinition(file.LocaleCode, file.FileText);
                }
                catch (Exception exception)
                {
                    diagnostics.Add(Diagnostic.Create(
                        Diagnostics.InvalidLocaleDefinition,
                        Location.None,
                        file.LocaleCode,
                        exception.Message));
                }
            }

            var resolvedLocales = ImmutableArray.CreateBuilder<ResolvedLocaleDefinition>();
            var resolving = new HashSet<string>(StringComparer.Ordinal);
            foreach (var localeCode in parsedLocales.Keys.OrderBy(static locale => locale, StringComparer.Ordinal))
            {
                ResolveLocale(localeCode, parsedLocales, resolving, resolvedLocales, diagnostics);
            }

            return new LocaleRegistryInput(
                resolvedLocales.ToImmutable(),
                diagnostics.ToImmutable(),
                dataBackedFormatterProfiles,
                dataBackedOrdinalizerProfiles);
        }

        public void Emit(SourceProductionContext context)
        {
            foreach (var diagnostic in diagnostics)
            {
                context.ReportDiagnostic(diagnostic);
            }

            if (locales.IsDefaultOrEmpty)
            {
                return;
            }

            foreach (var (registryName, featureSelector) in registrySelectors)
            {
                var registrations = ImmutableArray.CreateBuilder<RegistryEntry>();

                foreach (var locale in locales)
                {
                    var feature = featureSelector(locale);
                    if (feature is null)
                    {
                        continue;
                    }

                    registrations.Add(new RegistryEntry(locale.LocaleCode, feature.Kind, feature.Argument));
                }

                if (registrations.Count == 0)
                {
                    continue;
                }

                var helperName = registryName + "Registrations";
                var builder = new StringBuilder();
                var requiresNet6 = registryName is "DateOnlyToOrdinalWordsConverterRegistry" or "TimeOnlyToClockNotationConvertersRegistry";
                if (requiresNet6)
                {
                    builder.AppendLine("#if NET6_0_OR_GREATER");
                    builder.AppendLine();
                }

                builder.AppendLine("namespace Humanizer;");
                builder.AppendLine();
                builder.Append("internal static class ");
                builder.Append(helperName);
                builder.AppendLine();
                builder.AppendLine("{");
                builder.Append("    internal static void Register(");
                builder.Append(registryName);
                builder.AppendLine(" registry)");
                builder.AppendLine("    {");

                foreach (var registration in registrations.OrderBy(static registration => registration.Locale, StringComparer.Ordinal))
                {
                    var expression = registration.Profile is null
                        ? null
                        : RegistryExpressionFactory.Create(
                            registryName,
                            registration.Profile,
                            registration.Argument,
                            dataBackedFormatterProfiles,
                            dataBackedOrdinalizerProfiles);
                    if (expression is null)
                    {
                        context.ReportDiagnostic(Diagnostic.Create(
                            Diagnostics.UnknownRegistryProfile,
                            Location.None,
                            registryName,
                            registration.Profile ?? "<missing>",
                            registration.Locale));
                        continue;
                    }

                    builder.Append("        registry.Register(\"");
                    builder.Append(registration.Locale);
                    builder.Append("\", culture => ");
                    builder.Append(expression);
                    builder.AppendLine(");");
                }

                builder.AppendLine("    }");
                builder.AppendLine("}");

                if (requiresNet6)
                {
                    builder.AppendLine();
                    builder.AppendLine("#endif");
                }

                context.AddSource(helperName + ".g.cs", SourceText.From(builder.ToString(), Encoding.UTF8));
            }
        }

        static LocaleDefinition ParseLocaleDefinition(string localeCode, string fileText)
        {
            var root = SimpleYamlMapping.Parse(fileText);

            return new LocaleDefinition(
                localeCode,
                root.GetScalar("inherits"),
                ParseFeature(root, "collectionFormatter"),
                ParseFeature(root, "dateOnlyToOrdinalWords"),
                ParseFeature(root, "dateToOrdinalWords"),
                ParseFeature(root, "formatter"),
                ParseFeature(root, "numberToWords"),
                ParseFeature(root, "ordinalizer"),
                ParseFeature(root, "timeOnlyToClockNotation"),
                ParseFeature(root, "wordsToNumber"));
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
                        Diagnostics.InvalidLocaleDefinition,
                        Location.None,
                        localeCode,
                        $"Inherited locale '{inheritedLocale}' is not defined."));
                    inherited = ResolvedLocaleDefinition.Empty(localeCode);
                }
                else
                {
                    inherited = ResolveLocale(inheritedLocale, parsedLocales, resolving, cache, diagnostics);
                }
            }

            resolving.Remove(localeCode);

            var resolved = new ResolvedLocaleDefinition(
                localeCode,
                locale.CollectionFormatter ?? inherited.CollectionFormatter,
                locale.DateOnlyToOrdinalWords ?? inherited.DateOnlyToOrdinalWords,
                locale.DateToOrdinalWords ?? inherited.DateToOrdinalWords,
                locale.Formatter ?? inherited.Formatter,
                locale.NumberToWords ?? inherited.NumberToWords,
                locale.Ordinalizer ?? inherited.Ordinalizer,
                locale.TimeOnlyToClockNotation ?? inherited.TimeOnlyToClockNotation,
                locale.WordsToNumber ?? inherited.WordsToNumber);

            cache.Add(resolved);
            return resolved;
        }

        static LocaleFeature? ParseFeature(SimpleYamlMapping root, string key)
        {
            if (!root.TryGetValue(key, out var value))
            {
                return null;
            }

            return value switch
            {
                SimpleYamlScalar scalar => string.IsNullOrWhiteSpace(scalar.Value)
                    ? null
                    : new LocaleFeature(scalar.Value, null),
                SimpleYamlMapping mapping => new LocaleFeature(
                    GetRequiredScalar(mapping, "kind"),
                    GetScalar(mapping, "value") ?? GetScalar(mapping, "sourceLocale")),
                _ => throw new InvalidOperationException($"Feature '{key}' must be a scalar or mapping.")
            };
        }

        static string GetRequiredScalar(SimpleYamlMapping mapping, string key) =>
            GetScalar(mapping, key) ?? throw new InvalidOperationException($"Missing required scalar '{key}'.");

        static string? GetScalar(SimpleYamlMapping mapping, string key) =>
            mapping.TryGetValue(key, out var value) && value is SimpleYamlScalar scalar ? scalar.Value : null;
    }

    abstract class SimpleYamlValue;

    sealed class SimpleYamlScalar(string value) : SimpleYamlValue
    {
        public string Value { get; } = value;
    }

    sealed class SimpleYamlMapping(Dictionary<string, SimpleYamlValue> values) : SimpleYamlValue
    {
        readonly Dictionary<string, SimpleYamlValue> values = values;

        public bool TryGetValue(string key, out SimpleYamlValue value) =>
            values.TryGetValue(key, out value!);

        public string? GetScalar(string key) =>
            TryGetValue(key, out var value) && value is SimpleYamlScalar scalar ? scalar.Value : null;

        public static SimpleYamlMapping Parse(string text)
        {
            var rootValues = new Dictionary<string, SimpleYamlValue>(StringComparer.Ordinal);
            Dictionary<string, SimpleYamlValue>? currentMap = null;
            string? currentSection = null;
            var lines = text.Split(["\r\n", "\n"], StringSplitOptions.None);

            for (var lineNumber = 0; lineNumber < lines.Length; lineNumber++)
            {
                var rawLine = lines[lineNumber];
                var trimmed = rawLine.Trim();
                if (trimmed.Length == 0 || trimmed.StartsWith("#", StringComparison.Ordinal))
                {
                    continue;
                }

                var indent = CountIndent(rawLine);
                if (indent == 0)
                {
                    currentMap = null;
                    currentSection = null;
                    ParseEntry(rootValues, trimmed, out var key, out var value);
                    if (value is null)
                    {
                        currentSection = key;
                        currentMap = new Dictionary<string, SimpleYamlValue>(StringComparer.Ordinal);
                        rootValues[key] = new SimpleYamlMapping(currentMap);
                    }
                    else
                    {
                        rootValues[key] = new SimpleYamlScalar(Unquote(value));
                    }

                    continue;
                }

                if (indent != 2 || currentMap is null || currentSection is null)
                {
                    throw new InvalidOperationException($"Unsupported indentation on line {lineNumber + 1}.");
                }

                ParseEntry(currentMap, trimmed, out var nestedKey, out var nestedValue);
                if (nestedValue is null)
                {
                    throw new InvalidOperationException($"Nested mapping '{currentSection}.{nestedKey}' requires a scalar value.");
                }

                currentMap[nestedKey] = new SimpleYamlScalar(Unquote(nestedValue));
            }

            return new SimpleYamlMapping(rootValues);
        }

        static void ParseEntry(
            Dictionary<string, SimpleYamlValue> target,
            string line,
            out string key,
            out string? value)
        {
            var separatorIndex = line.IndexOf(':');
            if (separatorIndex <= 0)
            {
                throw new InvalidOperationException($"Invalid mapping entry '{line}'.");
            }

            key = line.Substring(0, separatorIndex).Trim();
            if (key.Length == 0)
            {
                throw new InvalidOperationException($"Invalid mapping entry '{line}'.");
            }

            var rawValue = line.Substring(separatorIndex + 1).Trim();
            value = rawValue.Length == 0 ? null : rawValue;
        }

        static int CountIndent(string line)
        {
            var count = 0;
            while (count < line.Length && line[count] == ' ')
            {
                count++;
            }

            return count;
        }

        static string Unquote(string value)
        {
            if (value.Length < 2)
            {
                return value;
            }

            var first = value[0];
            var last = value[value.Length - 1];
            return (first == '"' && last == '"') || (first == '\'' && last == '\'')
                ? value.Substring(1, value.Length - 2)
                : value;
        }
    }

    sealed class LocaleDefinition(
        string localeCode,
        string? inherits,
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
        public string? Inherits { get; } = inherits;
        public LocaleFeature? CollectionFormatter { get; } = collectionFormatter;
        public LocaleFeature? DateOnlyToOrdinalWords { get; } = dateOnlyToOrdinalWords;
        public LocaleFeature? DateToOrdinalWords { get; } = dateToOrdinalWords;
        public LocaleFeature? Formatter { get; } = formatter;
        public LocaleFeature? NumberToWords { get; } = numberToWords;
        public LocaleFeature? Ordinalizer { get; } = ordinalizer;
        public LocaleFeature? TimeOnlyToClockNotation { get; } = timeOnlyToClockNotation;
        public LocaleFeature? WordsToNumber { get; } = wordsToNumber;
    }

    sealed class ResolvedLocaleDefinition(
        string localeCode,
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
        public LocaleFeature? CollectionFormatter { get; } = collectionFormatter;
        public LocaleFeature? DateOnlyToOrdinalWords { get; } = dateOnlyToOrdinalWords;
        public LocaleFeature? DateToOrdinalWords { get; } = dateToOrdinalWords;
        public LocaleFeature? Formatter { get; } = formatter;
        public LocaleFeature? NumberToWords { get; } = numberToWords;
        public LocaleFeature? Ordinalizer { get; } = ordinalizer;
        public LocaleFeature? TimeOnlyToClockNotation { get; } = timeOnlyToClockNotation;
        public LocaleFeature? WordsToNumber { get; } = wordsToNumber;

        public static ResolvedLocaleDefinition Empty(string localeCode) =>
            new(localeCode, null, null, null, null, null, null, null, null);
    }

    sealed class LocaleFeature(string kind, string? argument)
    {
        public string Kind { get; } = kind;
        public string? Argument { get; } = argument;
    }
    static class Diagnostics
    {
        public static readonly DiagnosticDescriptor InvalidTokenMapData = new(
            id: "HSG001",
            title: "Invalid token-map source data",
            messageFormat: "Token map source data is invalid: {0}",
            category: "Humanizer.Generators",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor UnknownRegistryProfile = new(
            id: "HSG002",
            title: "Unknown registry profile",
            messageFormat: "Registry '{0}' does not recognize profile '{1}' for locale '{2}'",
            category: "Humanizer.Generators",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor InvalidLocaleDefinition = new(
            id: "HSG003",
            title: "Invalid locale definition",
            messageFormat: "Locale definition '{0}' is invalid: {1}",
            category: "Humanizer.Generators",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);
    }
}

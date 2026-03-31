using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Humanizer.SourceGenerators;

public sealed partial class HumanizerSourceGenerator
{
    sealed class TokenMapWordsToNumberInput(ImmutableArray<TokenMapLocaleDefinition> locales, ImmutableArray<Diagnostic> diagnostics)
    {
        static readonly string[] normalizationProfiles =
        [
            "CollapseWhitespace",
            "LowercaseRemovePeriods",
            "LowercaseReplacePeriodsWithSpaces",
            "LowercaseRemovePeriodsAndDiacritics",
            "PunctuationToSpacesRemoveDiacritics",
            "Persian"
        ];

        static readonly string[] ordinalGenderVariants =
        [
            "none",
            "masculine-and-feminine",
            "all"
        ];

        readonly ImmutableArray<TokenMapLocaleDefinition> locales = locales;
        readonly ImmutableArray<Diagnostic> diagnostics = diagnostics;

        public ImmutableArray<TokenMapLocaleDefinition> Locales => locales;

        public static TokenMapWordsToNumberInput Create(ImmutableArray<TokenMapLocaleFile?> files)
        {
            var diagnostics = ImmutableArray.CreateBuilder<Diagnostic>();
            var locales = ImmutableArray.CreateBuilder<TokenMapLocaleDefinition>();

            foreach (var file in files)
            {
                if (file is null)
                {
                    continue;
                }

                try
                {
                    using var document = JsonDocument.Parse(file.FileText);
                    if (document.RootElement.ValueKind != JsonValueKind.Object)
                    {
                        diagnostics.Add(CreateDiagnostic(file.LocaleCode, file.LocaleCode, "must be a JSON object."));
                        continue;
                    }

                    var locale = TryCreateLocale(file.LocaleCode, document.RootElement, diagnostics);
                    if (locale is not null)
                    {
                        locales.Add(locale);
                    }
                }
                catch (JsonException exception)
                {
                    diagnostics.Add(CreateDiagnostic(file.LocaleCode, file.LocaleCode, "contains invalid JSON: " + exception.Message));
                }
            }

            return new TokenMapWordsToNumberInput(locales.ToImmutable(), diagnostics.ToImmutable());
        }

        static TokenMapLocaleDefinition? TryCreateLocale(
            string localeCode,
            JsonElement localeElement,
            ImmutableArray<Diagnostic>.Builder diagnostics)
        {
            var hasErrors = false;

            var normalizationProfile = ReadRequiredEnumString(
                localeCode,
                localeElement,
                "normalizationProfile",
                normalizationProfiles,
                diagnostics,
                ref hasErrors);

            var ordinalNumberToWordsKind = ReadOptionalString(localeCode, localeElement, "ordinalNumberToWordsKind", diagnostics, ref hasErrors);
            var ordinalGenderVariant = ReadOptionalEnumString(
                localeCode,
                localeElement,
                "ordinalGenderVariant",
                ordinalGenderVariants,
                diagnostics,
                ref hasErrors);

            var exactOrdinalEntries = ReadMap(localeCode, localeElement, "ordinalMap", diagnostics, ref hasErrors, requireInt32: true);
            var ordinalScaleEntries = ReadMap(localeCode, localeElement, "ordinalScaleMap", diagnostics, ref hasErrors);
            var gluedOrdinalScaleSuffixEntries = ReadMap(localeCode, localeElement, "gluedOrdinalScaleSuffixes", diagnostics, ref hasErrors);
            var compositeScaleEntries = ReadMap(localeCode, localeElement, "compositeScaleMap", diagnostics, ref hasErrors);
            var negativePrefixes = ReadStringArray(localeCode, localeElement, "negativePrefixes", diagnostics, ref hasErrors);
            var negativeSuffixes = ReadStringArray(localeCode, localeElement, "negativeSuffixes", diagnostics, ref hasErrors);
            var ordinalPrefixes = ReadStringArray(localeCode, localeElement, "ordinalPrefixes", diagnostics, ref hasErrors);
            var ignoredTokens = ReadStringArray(localeCode, localeElement, "ignoredTokens", diagnostics, ref hasErrors);
            var multiplierTokens = ReadStringArray(localeCode, localeElement, "multiplierTokens", diagnostics, ref hasErrors);
            var tokenSuffixesToStrip = ReadStringArray(localeCode, localeElement, "tokenSuffixesToStrip", diagnostics, ref hasErrors);
            var ordinalAbbreviationSuffixes = ReadStringArray(localeCode, localeElement, "ordinalAbbreviationSuffixes", diagnostics, ref hasErrors);
            var aliases = ReadStringArray(localeCode, localeElement, "aliases", diagnostics, ref hasErrors);
            var allowTerminalOrdinalToken = ReadBoolean(localeCode, localeElement, "allowTerminalOrdinalToken", diagnostics, ref hasErrors);
            var useHundredMultiplier = ReadBoolean(localeCode, localeElement, "useHundredMultiplier", diagnostics, ref hasErrors);
            var allowInvariantIntegerInput = ReadBoolean(localeCode, localeElement, "allowInvariantIntegerInput", diagnostics, ref hasErrors);
            var scaleThreshold = ReadLong(localeCode, localeElement, "scaleThreshold", diagnostics, ref hasErrors);
            var cardinalEntries = ReadMap(localeCode, localeElement, "cardinalMap", diagnostics, ref hasErrors);

            if (hasErrors || normalizationProfile is null)
            {
                return null;
            }

            return new TokenMapLocaleDefinition(
                localeCode,
                normalizationProfile,
                ordinalNumberToWordsKind,
                ordinalGenderVariant,
                exactOrdinalEntries,
                ordinalScaleEntries,
                gluedOrdinalScaleSuffixEntries,
                compositeScaleEntries,
                negativePrefixes,
                negativeSuffixes,
                ordinalPrefixes,
                ignoredTokens,
                multiplierTokens,
                tokenSuffixesToStrip,
                ordinalAbbreviationSuffixes,
                aliases,
                allowTerminalOrdinalToken,
                useHundredMultiplier,
                allowInvariantIntegerInput,
                scaleThreshold,
                cardinalEntries);
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

            foreach (var locale in locales)
            {
                var propertyName = GetTokenMapPropertyName(locale.LocaleCode);
                var cacheName = propertyName + "_cache";
                var builder = new StringBuilder();
                builder.AppendLine("namespace Humanizer;");
                builder.AppendLine();
                builder.AppendLine("static partial class TokenMapWordsToNumberConverters");
                builder.AppendLine("{");
                builder.Append("    public static IWordsToNumberConverter ");
                builder.Append(propertyName);
                builder.Append(" => ");
                builder.Append(cacheName);
                builder.AppendLine(".Value;");
                builder.AppendLine();
                builder.Append("    static class ");
                builder.Append(cacheName);
                builder.AppendLine();
                builder.AppendLine("    {");
                builder.AppendLine("        internal static readonly IWordsToNumberConverter Value = new TokenMapWordsToNumberConverter(new()");
                builder.AppendLine("        {");
                AppendMap(builder, "            ", "CardinalMap", "long", locale.CardinalEntries);
                AppendMap(builder, "            ", "ExactOrdinalMap", "int", locale.ExactOrdinalEntries);
                AppendMap(builder, "            ", "OrdinalScaleMap", "long", locale.OrdinalScaleEntries);
                AppendMap(builder, "            ", "GluedOrdinalScaleSuffixes", "long", locale.GluedOrdinalScaleSuffixEntries);
                AppendMap(builder, "            ", "CompositeScaleMap", "long", locale.CompositeScaleEntries);
                builder.Append("            NormalizationProfile = TokenMapNormalizationProfile.");
                builder.Append(locale.NormalizationProfile);
                builder.AppendLine(",");
                AppendStringArray(builder, "            ", "NegativePrefixes", locale.NegativePrefixes);
                AppendStringArray(builder, "            ", "NegativeSuffixes", locale.NegativeSuffixes);
                AppendStringArray(builder, "            ", "OrdinalPrefixes", locale.OrdinalPrefixes);
                AppendStringArray(builder, "            ", "IgnoredTokens", locale.IgnoredTokens);
                AppendStringArray(builder, "            ", "MultiplierTokens", locale.MultiplierTokens);
                AppendStringArray(builder, "            ", "TokenSuffixesToStrip", locale.TokenSuffixesToStrip);
                AppendStringArray(builder, "            ", "OrdinalAbbreviationSuffixes", locale.OrdinalAbbreviationSuffixes);

                if (locale.AllowTerminalOrdinalToken)
                {
                    builder.AppendLine("            AllowTerminalOrdinalToken = true,");
                }

                if (locale.UseHundredMultiplier)
                {
                    builder.AppendLine("            UseHundredMultiplier = true,");
                }

                if (locale.AllowInvariantIntegerInput)
                {
                    builder.AppendLine("            AllowInvariantIntegerInput = true,");
                }

                if (locale.ScaleThreshold.HasValue)
                {
                    builder.Append("            ScaleThreshold = ");
                    builder.Append(locale.ScaleThreshold.Value.ToString(CultureInfo.InvariantCulture));
                    builder.AppendLine(",");
                }

                builder.AppendLine("        });");
                builder.AppendLine("    }");
                builder.AppendLine("}");
                context.AddSource("TokenMapWordsToNumberConverters." + propertyName + ".g.cs", SourceText.From(builder.ToString(), Encoding.UTF8));
            }

            var indexBuilder = new StringBuilder();
            indexBuilder.AppendLine("namespace Humanizer;");
            indexBuilder.AppendLine();
            indexBuilder.AppendLine("static partial class TokenMapWordsToNumberConverters");
            indexBuilder.AppendLine("{");
            indexBuilder.AppendLine("    public static IWordsToNumberConverter Resolve(string localeCode) =>");
            indexBuilder.AppendLine("        localeCode switch");
            indexBuilder.AppendLine("        {");

            foreach (var locale in locales.OrderBy(static locale => locale.LocaleCode, StringComparer.Ordinal))
            {
                indexBuilder.Append("            ");
                indexBuilder.Append(QuoteLiteral(locale.LocaleCode));
                indexBuilder.Append(" => ");
                indexBuilder.Append(GetTokenMapPropertyName(locale.LocaleCode));
                indexBuilder.AppendLine(",");

                foreach (var alias in locale.Aliases.OrderBy(static alias => alias, StringComparer.Ordinal))
                {
                    indexBuilder.Append("            ");
                    indexBuilder.Append(QuoteLiteral(alias));
                    indexBuilder.Append(" => ");
                    indexBuilder.Append(GetTokenMapPropertyName(locale.LocaleCode));
                    indexBuilder.AppendLine(",");
                }
            }

            indexBuilder.AppendLine("            _ => throw new ArgumentOutOfRangeException(nameof(localeCode), localeCode, \"Unknown token-map locale.\")");
            indexBuilder.AppendLine("        };");
            indexBuilder.AppendLine("}");
            context.AddSource("TokenMapWordsToNumberConverters.Index.g.cs", SourceText.From(indexBuilder.ToString(), Encoding.UTF8));
        }

        static void AppendStringArray(StringBuilder builder, string indent, string propertyName, ImmutableArray<string> values)
        {
            if (values.IsDefaultOrEmpty)
            {
                return;
            }

            builder.Append(indent);
            builder.Append(propertyName);
            builder.Append(" = [");

            for (var i = 0; i < values.Length; i++)
            {
                if (i > 0)
                {
                    builder.Append(", ");
                }

                builder.Append('"');
                builder.Append(Escape(values[i]));
                builder.Append('"');
            }

            builder.AppendLine("],");
        }

        static void AppendMap(StringBuilder builder, string indent, string propertyName, string valueType, ImmutableArray<TokenMapEntry> entries)
        {
            if (entries.IsDefaultOrEmpty)
            {
                return;
            }

            builder.Append(indent);
            builder.Append(propertyName);
            builder.Append(" = new Dictionary<string, ");
            builder.Append(valueType);
            builder.AppendLine(">(StringComparer.Ordinal)");
            builder.Append(indent);
            builder.AppendLine("{");

            foreach (var entry in entries)
            {
                builder.Append(indent);
                builder.Append("    [\"");
                builder.Append(Escape(entry.Key));
                builder.Append("\"] = ");
                builder.Append(entry.Value.ToString(CultureInfo.InvariantCulture));
                builder.AppendLine(",");
            }

            builder.Append(indent);
            builder.AppendLine("}.ToFrozenDictionary(StringComparer.Ordinal),");
        }

        static string? ReadRequiredEnumString(
            string localeCode,
            JsonElement element,
            string propertyName,
            string[] allowedValues,
            ImmutableArray<Diagnostic>.Builder diagnostics,
            ref bool hasErrors)
        {
            var value = ReadRequiredString(localeCode, element, propertyName, diagnostics, ref hasErrors);
            if (value is null)
            {
                return null;
            }

            if (!allowedValues.Contains(value, StringComparer.Ordinal))
            {
                diagnostics.Add(CreateDiagnostic(
                    localeCode,
                    localeCode + "." + propertyName,
                    "has invalid value '" + value + "'. Allowed values: " + string.Join(", ", allowedValues) + "."));
                hasErrors = true;
                return null;
            }

            return value;
        }

        static string? ReadOptionalEnumString(
            string localeCode,
            JsonElement element,
            string propertyName,
            string[] allowedValues,
            ImmutableArray<Diagnostic>.Builder diagnostics,
            ref bool hasErrors)
        {
            var value = ReadOptionalString(localeCode, element, propertyName, diagnostics, ref hasErrors);
            if (value is null)
            {
                return null;
            }

            if (!allowedValues.Contains(value, StringComparer.Ordinal))
            {
                diagnostics.Add(CreateDiagnostic(
                    localeCode,
                    localeCode + "." + propertyName,
                    "has invalid value '" + value + "'. Allowed values: " + string.Join(", ", allowedValues) + "."));
                hasErrors = true;
                return null;
            }

            return value;
        }

        static string? ReadRequiredString(
            string localeCode,
            JsonElement element,
            string propertyName,
            ImmutableArray<Diagnostic>.Builder diagnostics,
            ref bool hasErrors)
        {
            if (!element.TryGetProperty(propertyName, out var valueElement))
            {
                diagnostics.Add(CreateDiagnostic(localeCode, localeCode + "." + propertyName, "is required."));
                hasErrors = true;
                return null;
            }

            return ReadStringCore(localeCode, propertyName, valueElement, diagnostics, ref hasErrors);
        }

        static string? ReadOptionalString(
            string localeCode,
            JsonElement element,
            string propertyName,
            ImmutableArray<Diagnostic>.Builder diagnostics,
            ref bool hasErrors) =>
            element.TryGetProperty(propertyName, out var valueElement)
                ? ReadStringCore(localeCode, propertyName, valueElement, diagnostics, ref hasErrors)
                : null;

        static string? ReadStringCore(
            string localeCode,
            string propertyName,
            JsonElement valueElement,
            ImmutableArray<Diagnostic>.Builder diagnostics,
            ref bool hasErrors)
        {
            if (valueElement.ValueKind != JsonValueKind.String)
            {
                diagnostics.Add(CreateDiagnostic(localeCode, localeCode + "." + propertyName, "must be a JSON string."));
                hasErrors = true;
                return null;
            }

            var value = valueElement.GetString();
            if (string.IsNullOrWhiteSpace(value))
            {
                diagnostics.Add(CreateDiagnostic(localeCode, localeCode + "." + propertyName, "must be a non-empty string."));
                hasErrors = true;
                return null;
            }

            return value;
        }

        static ImmutableArray<string> ReadStringArray(
            string localeCode,
            JsonElement element,
            string propertyName,
            ImmutableArray<Diagnostic>.Builder diagnostics,
            ref bool hasErrors)
        {
            if (!element.TryGetProperty(propertyName, out var valuesElement))
            {
                return [];
            }

            if (valuesElement.ValueKind != JsonValueKind.Array)
            {
                diagnostics.Add(CreateDiagnostic(localeCode, localeCode + "." + propertyName, "must be a JSON array."));
                hasErrors = true;
                return [];
            }

            var values = ImmutableArray.CreateBuilder<string>();
            var index = 0;
            foreach (var valueElement in valuesElement.EnumerateArray())
            {
                var path = localeCode + "." + propertyName + "[" + index.ToString(CultureInfo.InvariantCulture) + "]";
                index++;

                if (valueElement.ValueKind != JsonValueKind.String)
                {
                    diagnostics.Add(CreateDiagnostic(localeCode, path, "must be a JSON string."));
                    hasErrors = true;
                    continue;
                }

                var value = valueElement.GetString();
                if (string.IsNullOrWhiteSpace(value))
                {
                    diagnostics.Add(CreateDiagnostic(localeCode, path, "must be a non-empty string."));
                    hasErrors = true;
                    continue;
                }

                values.Add(value!);
            }

            return values.ToImmutable();
        }

        static ImmutableArray<TokenMapEntry> ReadMap(
            string localeCode,
            JsonElement element,
            string propertyName,
            ImmutableArray<Diagnostic>.Builder diagnostics,
            ref bool hasErrors,
            bool requireInt32 = false)
        {
            if (!element.TryGetProperty(propertyName, out var mapElement))
            {
                return [];
            }

            if (mapElement.ValueKind != JsonValueKind.Object)
            {
                diagnostics.Add(CreateDiagnostic(localeCode, localeCode + "." + propertyName, "must be a JSON object."));
                hasErrors = true;
                return [];
            }

            var entries = ImmutableArray.CreateBuilder<TokenMapEntry>();
            foreach (var property in mapElement.EnumerateObject())
            {
                var path = localeCode + "." + propertyName + "." + property.Name;
                if (string.IsNullOrWhiteSpace(property.Name))
                {
                    diagnostics.Add(CreateDiagnostic(localeCode, path, "must have a non-empty key."));
                    hasErrors = true;
                    continue;
                }

                if (property.Value.ValueKind != JsonValueKind.Number || !property.Value.TryGetInt64(out var value))
                {
                    diagnostics.Add(CreateDiagnostic(localeCode, path, "must be an integer JSON number."));
                    hasErrors = true;
                    continue;
                }

                if (requireInt32 && (value > int.MaxValue || value < int.MinValue))
                {
                    diagnostics.Add(CreateDiagnostic(localeCode, path, "must fit within the Int32 range."));
                    hasErrors = true;
                    continue;
                }

                entries.Add(new TokenMapEntry(property.Name, value));
            }

            return entries.ToImmutable();
        }

        static long? ReadLong(
            string localeCode,
            JsonElement element,
            string propertyName,
            ImmutableArray<Diagnostic>.Builder diagnostics,
            ref bool hasErrors)
        {
            if (!element.TryGetProperty(propertyName, out var valueElement))
            {
                return null;
            }

            if (valueElement.ValueKind != JsonValueKind.Number || !valueElement.TryGetInt64(out var value))
            {
                diagnostics.Add(CreateDiagnostic(localeCode, localeCode + "." + propertyName, "must be an integer JSON number."));
                hasErrors = true;
                return null;
            }

            return value;
        }

        static bool ReadBoolean(
            string localeCode,
            JsonElement element,
            string propertyName,
            ImmutableArray<Diagnostic>.Builder diagnostics,
            ref bool hasErrors)
        {
            if (!element.TryGetProperty(propertyName, out var valueElement))
            {
                return false;
            }

            if (valueElement.ValueKind is not JsonValueKind.True and not JsonValueKind.False)
            {
                diagnostics.Add(CreateDiagnostic(localeCode, localeCode + "." + propertyName, "must be a JSON boolean."));
                hasErrors = true;
                return false;
            }

            return valueElement.GetBoolean();
        }

        static Diagnostic CreateDiagnostic(string localeCode, string propertyPath, string message) =>
            Diagnostic.Create(
                Diagnostics.InvalidTokenMapData,
                Location.None,
                propertyPath + " " + message + " (locale '" + localeCode + "')");
    }
}

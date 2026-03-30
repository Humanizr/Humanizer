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
    sealed class TokenMapWordsToNumberInput(ImmutableArray<TokenMapLocaleDefinition> locales, ImmutableArray<Diagnostic> diagnostics)
    {
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

                using var document = JsonDocument.Parse(file.FileText);
                var localeElement = document.RootElement;
                var normalizationProfile = GetString(localeElement, "normalizationProfile");
                if (string.IsNullOrWhiteSpace(normalizationProfile))
                {
                    diagnostics.Add(Diagnostic.Create(Diagnostics.InvalidTokenMapData, Location.None, "Locale entry is missing normalizationProfile"));
                    continue;
                }

                locales.Add(new TokenMapLocaleDefinition(
                    file.LocaleCode,
                    normalizationProfile!,
                    GetString(localeElement, "ordinalNumberToWordsKind"),
                    GetString(localeElement, "ordinalGenderVariant"),
                    GetMap(localeElement, "ordinalMap"),
                    GetMap(localeElement, "compositeScaleMap"),
                    GetStrings(localeElement, "negativePrefixes"),
                    GetStrings(localeElement, "negativeSuffixes"),
                    GetStrings(localeElement, "ordinalPrefixes"),
                    GetStrings(localeElement, "ignoredTokens"),
                    GetStrings(localeElement, "multiplierTokens"),
                    GetStrings(localeElement, "tokenSuffixesToStrip"),
                    GetStrings(localeElement, "ordinalAbbreviationSuffixes"),
                    GetStrings(localeElement, "aliases"),
                    GetBoolean(localeElement, "allowTerminalOrdinalToken"),
                    GetBoolean(localeElement, "useHundredMultiplier"),
                    GetBoolean(localeElement, "allowInvariantIntegerInput"),
                    GetLong(localeElement, "scaleThreshold"),
                    GetMap(localeElement, "cardinalMap")));
            }

            return new TokenMapWordsToNumberInput(locales.ToImmutable(), diagnostics.ToImmutable());
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
                var builder = new StringBuilder();
                builder.AppendLine("namespace Humanizer;");
                builder.AppendLine();
                builder.AppendLine("static partial class TokenMapWordsToNumberConverters");
                builder.AppendLine("{");
                builder.Append("    public static IWordsToNumberConverter ");
                builder.Append(GetTokenMapPropertyName(locale.LocaleCode));
                builder.AppendLine(" { get; } = new TokenMapWordsToNumberConverter(new()");
                builder.AppendLine("    {");
                builder.AppendLine("        CardinalMap = new Dictionary<string, long>(StringComparer.Ordinal)");
                builder.AppendLine("        {");

                foreach (var entry in locale.CardinalEntries)
                {
                    builder.Append("            [\"");
                    builder.Append(Escape(entry.Key));
                    builder.Append("\"] = ");
                    builder.Append(entry.Value.ToString(CultureInfo.InvariantCulture));
                    builder.AppendLine(",");
                }

                builder.AppendLine("        }.ToFrozenDictionary(StringComparer.Ordinal),");

                if (!string.IsNullOrWhiteSpace(locale.OrdinalNumberToWordsKind))
                {
                    builder.Append("        OrdinalMap = TokenMapWordsToNumberOrdinalMapBuilder.Build(");
                    builder.Append(QuoteLiteral(locale.OrdinalNumberToWordsKind!));
                    builder.Append(", TokenMapNormalizationProfile.");
                    builder.Append(locale.NormalizationProfile);
                    builder.Append(", TokenMapOrdinalGenderVariant.");
                    builder.Append(ToEnumMemberName(locale.OrdinalGenderVariant ?? "none"));
                    builder.AppendLine("),");
                }
                else if (!locale.OrdinalEntries.IsDefaultOrEmpty)
                {
                    AppendMap(builder, "OrdinalMap", "int", locale.OrdinalEntries);
                }

                if (!locale.CompositeScaleEntries.IsDefaultOrEmpty)
                {
                    AppendMap(builder, "CompositeScaleMap", "long", locale.CompositeScaleEntries);
                }

                builder.Append("        NormalizationProfile = TokenMapNormalizationProfile.");
                builder.Append(locale.NormalizationProfile);
                builder.AppendLine(",");

                AppendStringArray(builder, "NegativePrefixes", locale.NegativePrefixes);
                AppendStringArray(builder, "NegativeSuffixes", locale.NegativeSuffixes);
                AppendStringArray(builder, "OrdinalPrefixes", locale.OrdinalPrefixes);
                AppendStringArray(builder, "IgnoredTokens", locale.IgnoredTokens);
                AppendStringArray(builder, "MultiplierTokens", locale.MultiplierTokens);
                AppendStringArray(builder, "TokenSuffixesToStrip", locale.TokenSuffixesToStrip);
                AppendStringArray(builder, "OrdinalAbbreviationSuffixes", locale.OrdinalAbbreviationSuffixes);

                if (locale.AllowTerminalOrdinalToken)
                {
                    builder.AppendLine("        AllowTerminalOrdinalToken = true,");
                }

                if (locale.UseHundredMultiplier)
                {
                    builder.AppendLine("        UseHundredMultiplier = true,");
                }

                if (locale.AllowInvariantIntegerInput)
                {
                    builder.AppendLine("        AllowInvariantIntegerInput = true,");
                }

                if (locale.ScaleThreshold.HasValue)
                {
                    builder.Append("        ScaleThreshold = ");
                    builder.Append(locale.ScaleThreshold.Value.ToString(CultureInfo.InvariantCulture));
                    builder.AppendLine(",");
                }

                builder.AppendLine("    });");
                builder.AppendLine("}");
                context.AddSource("TokenMapWordsToNumberConverters." + GetTokenMapPropertyName(locale.LocaleCode) + ".g.cs", SourceText.From(builder.ToString(), Encoding.UTF8));
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

        static void AppendStringArray(StringBuilder builder, string propertyName, ImmutableArray<string> values)
        {
            if (values.IsDefaultOrEmpty)
            {
                return;
            }

            builder.Append("        ");
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

        static void AppendMap(StringBuilder builder, string propertyName, string valueType, ImmutableArray<TokenMapEntry> entries)
        {
            if (entries.IsDefaultOrEmpty)
            {
                return;
            }

            builder.Append("        ");
            builder.Append(propertyName);
            builder.Append(" = new Dictionary<string, ");
            builder.Append(valueType);
            builder.AppendLine(">(StringComparer.Ordinal)");
            builder.AppendLine("        {");

            foreach (var entry in entries)
            {
                builder.Append("            [\"");
                builder.Append(Escape(entry.Key));
                builder.Append("\"] = ");
                builder.Append(entry.Value.ToString(CultureInfo.InvariantCulture));
                builder.AppendLine(",");
            }

            builder.AppendLine("        }.ToFrozenDictionary(StringComparer.Ordinal),");
        }

        static string? GetString(JsonElement element, string propertyName) =>
            element.TryGetProperty(propertyName, out var value) && value.ValueKind == JsonValueKind.String
                ? value.GetString()
                : null;

        static ImmutableArray<string> GetStrings(JsonElement element, string propertyName)
        {
            if (!element.TryGetProperty(propertyName, out var valuesElement) || valuesElement.ValueKind != JsonValueKind.Array)
            {
                return [];
            }

            var values = ImmutableArray.CreateBuilder<string>();
            foreach (var valueElement in valuesElement.EnumerateArray())
            {
                if (valueElement.ValueKind != JsonValueKind.String)
                {
                    continue;
                }

                var value = valueElement.GetString();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    values.Add(value!);
                }
            }

            return values.ToImmutable();
        }

        static ImmutableArray<TokenMapEntry> GetMap(JsonElement element, string propertyName)
        {
            if (!element.TryGetProperty(propertyName, out var mapElement) || mapElement.ValueKind != JsonValueKind.Object)
            {
                return [];
            }

            var entries = ImmutableArray.CreateBuilder<TokenMapEntry>();
            foreach (var property in mapElement.EnumerateObject())
            {
                if (property.Value.ValueKind != JsonValueKind.Number || !property.Value.TryGetInt64(out var value))
                {
                    continue;
                }

                entries.Add(new TokenMapEntry(property.Name, value));
            }

            return entries.ToImmutable();
        }

        static long? GetLong(JsonElement element, string propertyName)
        {
            if (!element.TryGetProperty(propertyName, out var valueElement) || valueElement.ValueKind != JsonValueKind.Number)
            {
                return null;
            }

            return valueElement.TryGetInt64(out var value) ? value : null;
        }

        static bool GetBoolean(JsonElement element, string propertyName) =>
            element.TryGetProperty(propertyName, out var valueElement) &&
            valueElement.ValueKind == JsonValueKind.True;
    }

}

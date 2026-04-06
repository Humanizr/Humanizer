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
        static readonly string[] NormalizationProfiles =
        [
            "CollapseWhitespace",
            "LowercaseRemovePeriods",
            "LowercaseReplacePeriodsWithSpaces",
            "LowercaseRemovePeriodsAndDiacritics",
            "PunctuationToSpacesRemoveDiacritics",
            "Persian"
        ];

        static readonly string[] OrdinalGenderVariants =
        [
            "none",
            "masculine-and-feminine",
            "all"
        ];

        readonly ImmutableArray<TokenMapLocaleDefinition> locales = locales;
        readonly ImmutableArray<Diagnostic> diagnostics = diagnostics;

        public ImmutableArray<TokenMapLocaleDefinition> Locales => locales;

        public static TokenMapWordsToNumberInput Create(LocaleCatalogInput localeCatalog)
        {
            var diagnostics = ImmutableArray.CreateBuilder<Diagnostic>();
            var locales = ImmutableArray.CreateBuilder<TokenMapLocaleDefinition>();
            var seenLocales = new HashSet<string>(StringComparer.Ordinal);

            foreach (var locale in localeCatalog.Locales)
            {
                var feature = locale.WordsToNumber;
                if (feature is not { Kind: "lexicon" })
                {
                    continue;
                }

                try
                {
                    if (feature.ProfileRoot.ValueKind != JsonValueKind.Object)
                    {
                        diagnostics.Add(CreateDiagnostic(locale.LocaleCode, locale.LocaleCode, "must be a YAML mapping."));
                        continue;
                    }

                    var tokenMapLocale = TryCreateLocale(locale.LocaleCode, feature.ProfileRoot, diagnostics);
                    if (tokenMapLocale is not null && seenLocales.Add(tokenMapLocale.LocaleCode))
                    {
                        locales.Add(tokenMapLocale);
                    }
                }
                catch (Exception exception)
                {
                    diagnostics.Add(CreateDiagnostic(locale.LocaleCode, locale.LocaleCode, "contains invalid locale-owned YAML data: " + exception.Message));
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
                NormalizationProfiles,
                diagnostics,
                ref hasErrors);

            var ordinalNumberToWordsKind = ReadOptionalString(localeCode, localeElement, "ordinalNumberToWordsKind", diagnostics, ref hasErrors);
            var ordinalGenderVariant = ReadOptionalEnumString(
                localeCode,
                localeElement,
                "ordinalGenderVariant",
                OrdinalGenderVariants,
                diagnostics,
                ref hasErrors);

            var exactOrdinalEntries = ReadMap(localeCode, localeElement, "ordinalMap", diagnostics, ref hasErrors);
            var ordinalScaleEntries = ReadMap(localeCode, localeElement, "ordinalScaleMap", diagnostics, ref hasErrors);
            var gluedOrdinalScaleSuffixEntries = ReadMap(localeCode, localeElement, "gluedOrdinalScaleSuffixes", diagnostics, ref hasErrors);
            var compositeScaleEntries = ReadMap(localeCode, localeElement, "compositeScaleMap", diagnostics, ref hasErrors);
            var negativePrefixes = ReadStringArray(localeCode, localeElement, "negativePrefixes", diagnostics, ref hasErrors);
            var negativeSuffixes = ReadStringArray(localeCode, localeElement, "negativeSuffixes", diagnostics, ref hasErrors);
            var ordinalPrefixes = ReadStringArray(localeCode, localeElement, "ordinalPrefixes", diagnostics, ref hasErrors);
            var ignoredTokens = ReadStringArray(localeCode, localeElement, "ignoredTokens", diagnostics, ref hasErrors);
            var leadingTokenPrefixesToTrim = ReadStringArray(localeCode, localeElement, "leadingTokenPrefixesToTrim", diagnostics, ref hasErrors);
            var multiplierTokens = ReadStringArray(localeCode, localeElement, "multiplierTokens", diagnostics, ref hasErrors);
            var tokenSuffixesToStrip = ReadStringArray(localeCode, localeElement, "tokenSuffixesToStrip", diagnostics, ref hasErrors);
            var ordinalAbbreviationSuffixes = ReadStringArray(localeCode, localeElement, "ordinalAbbreviationSuffixes", diagnostics, ref hasErrors);
            var teenSuffixTokens = ReadStringArray(localeCode, localeElement, "teenSuffixTokens", diagnostics, ref hasErrors);
            var hundredSuffixTokens = ReadStringArray(localeCode, localeElement, "hundredSuffixTokens", diagnostics, ref hasErrors);
            var aliases = ReadStringArray(localeCode, localeElement, "aliases", diagnostics, ref hasErrors);
            var allowTerminalOrdinalToken = ReadBoolean(localeCode, localeElement, "allowTerminalOrdinalToken", diagnostics, ref hasErrors);
            var useHundredMultiplier = ReadBoolean(localeCode, localeElement, "useHundredMultiplier", diagnostics, ref hasErrors);
            var allowInvariantIntegerInput = ReadBoolean(localeCode, localeElement, "allowInvariantIntegerInput", diagnostics, ref hasErrors);
            var teenBaseValue = ReadLong(localeCode, localeElement, "teenBaseValue", diagnostics, ref hasErrors);
            var hundredSuffixValue = ReadLong(localeCode, localeElement, "hundredSuffixValue", diagnostics, ref hasErrors);
            var unitTokenMinValue = ReadLong(localeCode, localeElement, "unitTokenMinValue", diagnostics, ref hasErrors);
            var unitTokenMaxValue = ReadLong(localeCode, localeElement, "unitTokenMaxValue", diagnostics, ref hasErrors);
            var hundredSuffixMinValue = ReadLong(localeCode, localeElement, "hundredSuffixMinValue", diagnostics, ref hasErrors);
            var hundredSuffixMaxValue = ReadLong(localeCode, localeElement, "hundredSuffixMaxValue", diagnostics, ref hasErrors);
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
                leadingTokenPrefixesToTrim,
                multiplierTokens,
                tokenSuffixesToStrip,
                ordinalAbbreviationSuffixes,
                teenSuffixTokens,
                hundredSuffixTokens,
                aliases,
                allowTerminalOrdinalToken,
                useHundredMultiplier,
                allowInvariantIntegerInput,
                teenBaseValue,
                hundredSuffixValue,
                unitTokenMinValue,
                unitTokenMaxValue,
                hundredSuffixMinValue,
                hundredSuffixMaxValue,
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
                AppendMap(builder, "            ", "ExactOrdinalMap", "long", locale.ExactOrdinalEntries);
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
                AppendStringArray(builder, "            ", "LeadingTokenPrefixesToTrim", locale.LeadingTokenPrefixesToTrim);
                AppendStringArray(builder, "            ", "MultiplierTokens", locale.MultiplierTokens);
                AppendStringArray(builder, "            ", "TokenSuffixesToStrip", locale.TokenSuffixesToStrip);
                AppendStringArray(builder, "            ", "OrdinalAbbreviationSuffixes", locale.OrdinalAbbreviationSuffixes);
                AppendStringArray(builder, "            ", "TeenSuffixTokens", locale.TeenSuffixTokens);
                AppendStringArray(builder, "            ", "HundredSuffixTokens", locale.HundredSuffixTokens);

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

                AppendLongValue(builder, "            ", "TeenBaseValue", locale.TeenBaseValue, defaultValue: 10);
                AppendLongValue(builder, "            ", "HundredSuffixValue", locale.HundredSuffixValue, defaultValue: 100);
                AppendLongValue(builder, "            ", "UnitTokenMinValue", locale.UnitTokenMinValue, defaultValue: 1);
                AppendLongValue(builder, "            ", "UnitTokenMaxValue", locale.UnitTokenMaxValue, defaultValue: 9);
                AppendLongValue(builder, "            ", "HundredSuffixMinValue", locale.HundredSuffixMinValue, defaultValue: long.MaxValue);
                AppendLongValue(builder, "            ", "HundredSuffixMaxValue", locale.HundredSuffixMaxValue, defaultValue: long.MinValue);
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

        static void AppendLongValue(StringBuilder builder, string indent, string propertyName, long? value, long defaultValue)
        {
            if (value is not { } explicitValue || explicitValue == defaultValue)
            {
                return;
            }

            builder.Append(indent);
            builder.Append(propertyName);
            builder.Append(" = ");
            builder.Append(explicitValue.ToString(CultureInfo.InvariantCulture));
            builder.AppendLine(",");
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

            var normalizedValue = NormalizeEnumToken(value);
            if (!allowedValues.Any(allowed => NormalizeEnumToken(allowed) == normalizedValue))
            {
                diagnostics.Add(CreateDiagnostic(
                    localeCode,
                    localeCode + "." + propertyName,
                    "has invalid value '" + value + "'. Allowed values: " + string.Join(", ", allowedValues) + "."));
                hasErrors = true;
                return null;
            }

            return normalizedValue;
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

            var normalizedValue = NormalizeEnumToken(value);
            if (!allowedValues.Any(allowed => NormalizeEnumToken(allowed) == normalizedValue))
            {
                diagnostics.Add(CreateDiagnostic(
                    localeCode,
                    localeCode + "." + propertyName,
                    "has invalid value '" + value + "'. Allowed values: " + string.Join(", ", allowedValues) + "."));
                hasErrors = true;
                return null;
            }

            return normalizedValue;
        }

        static string NormalizeEnumToken(string value)
        {
            if (value.IndexOfAny(['-', '_']) < 0)
            {
                return value;
            }

            var segments = value.Split(['-', '_'], StringSplitOptions.RemoveEmptyEntries);
            var builder = new StringBuilder(value.Length);

            foreach (var segment in segments)
            {
                if (segment.Length == 0)
                {
                    continue;
                }

                builder.Append(char.ToUpperInvariant(segment[0]));
                if (segment.Length > 1)
                {
                    builder.Append(segment.Substring(1));
                }
            }

            return builder.ToString();
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
                diagnostics.Add(CreateDiagnostic(localeCode, localeCode + "." + propertyName, "must be a string."));
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
                diagnostics.Add(CreateDiagnostic(localeCode, localeCode + "." + propertyName, "must be a YAML sequence."));
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
                    diagnostics.Add(CreateDiagnostic(localeCode, path, "must be a string."));
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
            ref bool hasErrors)
        {
            if (!element.TryGetProperty(propertyName, out var mapElement))
            {
                return [];
            }

            if (mapElement.ValueKind != JsonValueKind.Object)
            {
                diagnostics.Add(CreateDiagnostic(localeCode, localeCode + "." + propertyName, "must be a YAML mapping."));
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
                    diagnostics.Add(CreateDiagnostic(localeCode, path, "must be an integer."));
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
                diagnostics.Add(CreateDiagnostic(localeCode, localeCode + "." + propertyName, "must be an integer."));
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
                diagnostics.Add(CreateDiagnostic(localeCode, localeCode + "." + propertyName, "must be a boolean."));
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
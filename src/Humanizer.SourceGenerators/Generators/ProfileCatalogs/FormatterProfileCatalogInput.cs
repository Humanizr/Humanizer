using System.Collections.Immutable;
using System.Globalization;
using System.Text;
using System.Text.Json;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Humanizer.SourceGenerators;

public sealed partial class HumanizerSourceGenerator
{
    sealed class FormatterProfileCatalogInput(ImmutableArray<FormatterProfileDefinition> profiles)
    {
        [Flags]
        enum TimeUnitMask
        {
            None = 0,
            Millisecond = 1 << 0,
            Second = 1 << 1,
            Minute = 1 << 2,
            Hour = 1 << 3,
            Day = 1 << 4,
            Week = 1 << 5,
            Month = 1 << 6,
            Year = 1 << 7,
            All = Millisecond | Second | Minute | Hour | Day | Week | Month | Year
        }

        [Flags]
        enum TenseMask
        {
            None = 0,
            Past = 1 << 0,
            Future = 1 << 1,
            Both = Past | Future
        }

        readonly ImmutableArray<FormatterProfileDefinition> profiles = profiles;

        public ImmutableHashSet<string> DataBackedProfileNames { get; } = profiles
            .Select(static profile => profile.ProfileName)
            .ToImmutableHashSet(StringComparer.Ordinal);

        public static FormatterProfileCatalogInput Create(LocaleCatalogInput localeCatalog)
        {
            var profiles = ImmutableArray.CreateBuilder<FormatterProfileDefinition>();
            var seenProfiles = new HashSet<string>(StringComparer.Ordinal);
            foreach (var locale in localeCatalog.Locales)
            {
                var feature = locale.Formatter;
                if (feature is not { UsesGeneratedProfile: true } ||
                    !seenProfiles.Add(feature.ProfileName!))
                {
                    continue;
                }

                profiles.Add(new FormatterProfileDefinition(
                    feature.ProfileName!,
                    GetRequiredString(feature.ProfileRoot, "engine"),
                    feature.ProfileRoot.Clone(),
                    locale.Grammar));
            }

            return new FormatterProfileCatalogInput(profiles.ToImmutable());
        }

        public void Emit(SourceProductionContext context)
        {
            if (profiles.IsDefaultOrEmpty)
            {
                return;
            }

            var generatedProfiles = profiles
                .OrderBy(static profile => profile.ProfileName, StringComparer.Ordinal)
                .ToArray();

            var builder = new StringBuilder();
            builder.AppendLine("using System;");
            builder.AppendLine("using System.Collections.Frozen;");
            builder.AppendLine("using System.Collections.Generic;");
            builder.AppendLine("using System.Globalization;");
            builder.AppendLine();
            builder.AppendLine("namespace Humanizer;");
            builder.AppendLine();
            builder.AppendLine("static partial class FormatterProfileCatalog");
            builder.AppendLine("{");
            builder.AppendLine("    public static IFormatter Resolve(string kind, CultureInfo culture)");
            builder.AppendLine("    {");
            builder.AppendLine("        return kind switch");
            builder.AppendLine("        {");

            foreach (var profile in generatedProfiles)
            {
                builder.Append("            ");
                builder.Append(QuoteLiteral(profile.ProfileName));
                builder.Append(" => new ProfiledFormatter(culture, ");
                builder.Append(GetCatalogPropertyName(profile.ProfileName));
                builder.AppendLine("),");
            }

            builder.AppendLine("            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, \"Unknown formatter profile.\")");
            builder.AppendLine("        };");
            builder.AppendLine("    }");
            builder.AppendLine();

            foreach (var profile in generatedProfiles)
            {
                AppendLazyCachedMember(
                    builder,
                    "    ",
                    "static",
                    "FormatterProfile",
                    GetCatalogPropertyName(profile.ProfileName),
                    CreateFormatterProfileExpression(profile));
                builder.AppendLine();
            }

            builder.AppendLine("}");
            context.AddSource("FormatterProfileCatalog.g.cs", SourceText.From(builder.ToString(), Encoding.UTF8));
        }

        static string CreateFormatterProfileExpression(FormatterProfileDefinition profile)
        {
            if (profile.Engine != "profiled")
            {
                throw new InvalidOperationException($"Unsupported formatter engine '{profile.Engine}'.");
            }

            var phraseDetector = GetGrammarScalar(profile.Grammar, "pluralRule") ?? GetOptionalString(profile.Root, "resourceKeyDetector");
            var dataUnitDetector = GetGrammarScalar(profile.Grammar, "dataUnitPluralRule") ?? GetOptionalString(profile.Root, "dataUnitDetector");
            var dataUnitNonIntegralForm = GetGrammarScalar(profile.Grammar, "dataUnitNonIntegralForm") ?? GetOptionalString(profile.Root, "dataUnitNonIntegralForm");
            var prepositionMode = GetGrammarScalar(profile.Grammar, "prepositionMode") ?? GetOptionalString(profile.Root, "prepositionMode");
            var secondaryPlaceholderMode = GetGrammarScalar(profile.Grammar, "secondaryPlaceholderMode") ?? GetOptionalString(profile.Root, "secondaryPlaceholderMode");
            var fallbackTransform = GetOptionalString(profile.Root, "dataUnitFallbackTransform");

            return "new FormatterProfile(" +
                   CreateFormatterNumberDetectorExpression(phraseDetector) + ", " +
                   CreateFormatterDateFormRuleArrayExpression(profile.Root, "exactDateForms") + ", " +
                   CreateFormatterTimeSpanFormRuleArrayExpression(profile.Root, "exactTimeSpanForms") + ", " +
                   CreateFormatterNumberDetectorExpression(dataUnitDetector) + ", " +
                   CreateFormatterNumberFormExpression(dataUnitNonIntegralForm) + ", " +
                   CreateFormatterFallbackTransformExpression(fallbackTransform) + ", " +
                   CreateFormatterPrepositionModeExpression(prepositionMode) + ", " +
                   CreateFormatterSecondaryPlaceholderModeExpression(secondaryPlaceholderMode) + ", " +
                   CreateTimeUnitGenderMapExpression(profile) +
                   ")";
        }

        static string? GetGrammarScalar(SimpleYamlMapping? grammar, string key) =>
            grammar?.GetScalar(key);

        static string CreateTimeUnitGenderMapExpression(FormatterProfileDefinition profile)
        {
            if (profile.Grammar?.TryGetValue("timeUnitGenders", out var grammarValue) == true &&
                grammarValue is SimpleYamlMapping grammarMapping)
            {
                return HumanizerSourceGenerator.CreateTimeUnitGenderMapExpression(
                    LocaleCatalogInput.ToJsonElement(grammarMapping));
            }

            return HumanizerSourceGenerator.CreateTimeUnitGenderMapExpression(profile.Root, "timeUnitGenders");
        }

        static string CreateFormatterDateFormRuleArrayExpression(JsonElement element, string propertyName)
        {
            if (!element.TryGetProperty(propertyName, out var property) || property.ValueKind != JsonValueKind.Array)
            {
                return "Array.Empty<FormatterDateFormRule>()";
            }

            var expressions = new List<string>();
            foreach (var item in property.EnumerateArray())
            {
                var units = ReadTimeUnitMask(item, "units", TimeUnitMask.All);
                var tenses = ReadTenseMask(item, "tenses", TenseMask.Both);
                if (units == TimeUnitMask.None || tenses == TenseMask.None)
                {
                    continue;
                }

                expressions.Add(
                    "new FormatterDateFormRule(" +
                    checked((int)GetRequiredInt64(item, "number")).ToString(CultureInfo.InvariantCulture) + ", " +
                    CreateTimeUnitMaskExpression(units) + ", " +
                    CreateTenseMaskExpression(tenses) + ", " +
                    CreateFormatterNumberFormExpression(GetRequiredString(item, "form")) +
                    ")");
            }

            return CreateRuleArrayExpression("FormatterDateFormRule", expressions);
        }

        static string CreateFormatterTimeSpanFormRuleArrayExpression(JsonElement element, string propertyName)
        {
            if (!element.TryGetProperty(propertyName, out var property) || property.ValueKind != JsonValueKind.Array)
            {
                return "Array.Empty<FormatterTimeSpanFormRule>()";
            }

            var expressions = new List<string>();
            foreach (var item in property.EnumerateArray())
            {
                var units = ReadTimeUnitMask(item, "units", TimeUnitMask.All);
                if (units == TimeUnitMask.None)
                {
                    continue;
                }

                expressions.Add(
                    "new FormatterTimeSpanFormRule(" +
                    checked((int)GetRequiredInt64(item, "number")).ToString(CultureInfo.InvariantCulture) + ", " +
                    CreateTimeUnitMaskExpression(units) + ", " +
                    CreateFormatterNumberFormExpression(GetRequiredString(item, "form")) +
                    ")");
            }

            return CreateRuleArrayExpression("FormatterTimeSpanFormRule", expressions);
        }

        static string CreateRuleArrayExpression(string elementType, IReadOnlyList<string> expressions) =>
            expressions.Count == 0
                ? "Array.Empty<" + elementType + ">()"
                : "new " + elementType + "[] { " + string.Join(", ", expressions) + " }";

        static TimeUnitMask ReadTimeUnitMask(JsonElement item, string propertyName, TimeUnitMask defaultMask)
        {
            if (!item.TryGetProperty(propertyName, out var property))
            {
                return defaultMask;
            }

            var mask = TimeUnitMask.None;
            foreach (var value in EnumerateSelectorStrings(property))
            {
                mask |= value.ToLowerInvariant() switch
                {
                    "all" => TimeUnitMask.All,
                    "millisecond" or "milliseconds" => TimeUnitMask.Millisecond,
                    "second" or "seconds" => TimeUnitMask.Second,
                    "minute" or "minutes" => TimeUnitMask.Minute,
                    "hour" or "hours" => TimeUnitMask.Hour,
                    "day" or "days" => TimeUnitMask.Day,
                    "week" or "weeks" => TimeUnitMask.Week,
                    "month" or "months" => TimeUnitMask.Month,
                    "year" or "years" => TimeUnitMask.Year,
                    _ => TimeUnitMask.None
                };
            }

            return mask;
        }

        static TenseMask ReadTenseMask(JsonElement item, string propertyName, TenseMask defaultMask)
        {
            if (!item.TryGetProperty(propertyName, out var property))
            {
                return defaultMask;
            }

            var mask = TenseMask.None;
            foreach (var value in EnumerateSelectorStrings(property))
            {
                mask |= value.ToLowerInvariant() switch
                {
                    "all" or "both" => TenseMask.Both,
                    "past" => TenseMask.Past,
                    "future" => TenseMask.Future,
                    _ => TenseMask.None
                };
            }

            return mask;
        }

        static IEnumerable<string> EnumerateSelectorStrings(JsonElement property)
        {
            if (property.ValueKind == JsonValueKind.String)
            {
                yield return property.GetString()!;
                yield break;
            }

            if (property.ValueKind != JsonValueKind.Array)
            {
                yield break;
            }

            foreach (var item in property.EnumerateArray())
            {
                if (item.ValueKind == JsonValueKind.String)
                {
                    yield return item.GetString()!;
                }
            }
        }

        static string CreateTimeUnitMaskExpression(TimeUnitMask mask)
        {
            if (mask == TimeUnitMask.All)
            {
                return "FormatterTimeUnitMask.All";
            }

            var parts = new List<string>();
            AppendTimeUnitMaskPart(parts, mask, TimeUnitMask.Millisecond, "Millisecond");
            AppendTimeUnitMaskPart(parts, mask, TimeUnitMask.Second, "Second");
            AppendTimeUnitMaskPart(parts, mask, TimeUnitMask.Minute, "Minute");
            AppendTimeUnitMaskPart(parts, mask, TimeUnitMask.Hour, "Hour");
            AppendTimeUnitMaskPart(parts, mask, TimeUnitMask.Day, "Day");
            AppendTimeUnitMaskPart(parts, mask, TimeUnitMask.Week, "Week");
            AppendTimeUnitMaskPart(parts, mask, TimeUnitMask.Month, "Month");
            AppendTimeUnitMaskPart(parts, mask, TimeUnitMask.Year, "Year");

            return parts.Count == 0
                ? "FormatterTimeUnitMask.None"
                : string.Join(" | ", parts);
        }

        static string CreateTenseMaskExpression(TenseMask mask)
        {
            if (mask == TenseMask.Both)
            {
                return "FormatterTenseMask.Both";
            }

            var parts = new List<string>();
            if ((mask & TenseMask.Past) != 0)
            {
                parts.Add("FormatterTenseMask.Past");
            }

            if ((mask & TenseMask.Future) != 0)
            {
                parts.Add("FormatterTenseMask.Future");
            }

            return parts.Count == 0
                ? "FormatterTenseMask.None"
                : string.Join(" | ", parts);
        }

        static void AppendTimeUnitMaskPart(List<string> parts, TimeUnitMask combined, TimeUnitMask flag, string name)
        {
            if ((combined & flag) != 0)
            {
                parts.Add("FormatterTimeUnitMask." + name);
            }
        }
    }
}
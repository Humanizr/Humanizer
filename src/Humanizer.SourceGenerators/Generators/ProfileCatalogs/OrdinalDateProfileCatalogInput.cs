using System.Collections.Immutable;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Humanizer.SourceGenerators;

public sealed partial class HumanizerSourceGenerator
{
    sealed class OrdinalDateProfileCatalogInput(
        ImmutableArray<OrdinalDateProfileDefinition> dateProfiles,
        ImmutableArray<OrdinalDateProfileDefinition> dateOnlyProfiles)
    {
        readonly ImmutableArray<OrdinalDateProfileDefinition> dateProfiles = dateProfiles;
        readonly ImmutableArray<OrdinalDateProfileDefinition> dateOnlyProfiles = dateOnlyProfiles;

        public static OrdinalDateProfileCatalogInput Create(LocaleCatalogInput localeCatalog)
        {
            var dateProfiles = ImmutableArray.CreateBuilder<OrdinalDateProfileDefinition>();
            var dateOnlyProfiles = ImmutableArray.CreateBuilder<OrdinalDateProfileDefinition>();
            var seenDate = new HashSet<string>(StringComparer.Ordinal);
            var seenDateOnly = new HashSet<string>(StringComparer.Ordinal);
            foreach (var locale in localeCatalog.Locales)
            {
                var months = ExtractCalendarMonths(locale.Calendar, "months");
                var monthsGenitive = ExtractCalendarMonths(locale.Calendar, "monthsGenitive");
                var hijriMonths = ExtractCalendarMonths(locale.Calendar, "hijriMonths");
                AddProfile(dateProfiles, locale.DateToOrdinalWords, seenDate, months, monthsGenitive, hijriMonths);
                AddProfile(dateOnlyProfiles, locale.DateOnlyToOrdinalWords, seenDateOnly, months, monthsGenitive, hijriMonths);
            }

            return new OrdinalDateProfileCatalogInput(dateProfiles.ToImmutable(), dateOnlyProfiles.ToImmutable());
        }

        static ImmutableArray<string> ExtractCalendarMonths(SimpleYamlMapping? calendar, string key)
        {
            if (calendar is null || !calendar.TryGetValue(key, out var value) || value is not SimpleYamlSequence sequence)
            {
                return ImmutableArray<string>.Empty;
            }

            var builder = ImmutableArray.CreateBuilder<string>(sequence.Items.Length);
            foreach (var item in sequence.Items)
            {
                if (item is not SimpleYamlScalar scalar)
                {
                    throw new InvalidOperationException($"calendar.{key} items must be strings.");
                }

                builder.Add(scalar.Value);
            }

            return builder.MoveToImmutable();
        }

        static void AddProfile(
            ImmutableArray<OrdinalDateProfileDefinition>.Builder profiles,
            LocaleFeature? feature,
            HashSet<string> seenProfiles,
            ImmutableArray<string> months,
            ImmutableArray<string> monthsGenitive,
            ImmutableArray<string> hijriMonths)
        {
            if (feature is not { UsesGeneratedProfile: true } ||
                !seenProfiles.Add(feature.ProfileName!))
            {
                return;
            }

            profiles.Add(new OrdinalDateProfileDefinition(
                feature.ProfileName!,
                GetOptionalString(feature.ProfileRoot, "engine") ?? "pattern",
                feature.ProfileRoot.Clone(),
                months,
                monthsGenitive,
                hijriMonths));
        }

        public void Emit(SourceProductionContext context)
        {
            EmitDateCatalog(
                context,
                dateProfiles,
                "DateToOrdinalWordsProfileCatalog",
                "IDateToOrdinalWordConverter",
                "PatternDateToOrdinalWordsConverter",
                "OrdinalDateProfileCatalog.DateTo.g.cs");

            EmitDateCatalog(
                context,
                dateOnlyProfiles,
                "DateOnlyToOrdinalWordsProfileCatalog",
                "IDateOnlyToOrdinalWordConverter",
                "PatternDateOnlyToOrdinalWordsConverter",
                "OrdinalDateProfileCatalog.DateOnly.g.cs",
                condition: "NET6_0_OR_GREATER");
        }

        void EmitDateCatalog(
            SourceProductionContext context,
            ImmutableArray<OrdinalDateProfileDefinition> profiles,
            string catalogName,
            string interfaceName,
            string converterTypeName,
            string sourceName,
            string? condition = null)
        {
            var builder = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(condition))
            {
                builder.Append("#if ");
                builder.AppendLine(condition);
                builder.AppendLine();
            }

            builder.AppendLine("using System;");
            builder.AppendLine();
            builder.AppendLine("namespace Humanizer;");
            builder.AppendLine();
            builder.Append("static class ");
            builder.Append(catalogName);
            builder.AppendLine();
            builder.AppendLine("{");
            builder.Append("    public static ");
            builder.Append(interfaceName);
            builder.AppendLine(" Resolve(string kind)");
            builder.AppendLine("    {");
            builder.AppendLine("        switch (kind)");
            builder.AppendLine("        {");

            foreach (var profile in profiles.OrderBy(static profile => profile.ProfileName, StringComparer.Ordinal))
            {
                builder.Append("            case ");
                builder.Append(QuoteLiteral(profile.ProfileName));
                builder.Append(": return ");
                builder.Append(GetCatalogPropertyName(profile.ProfileName));
                builder.AppendLine(";");
            }

            builder.AppendLine("            default: throw new ArgumentOutOfRangeException(nameof(kind), kind, \"Unknown ordinal date profile.\");");
            builder.AppendLine("        }");
            builder.AppendLine("    }");
            builder.AppendLine();

            foreach (var profile in profiles.OrderBy(static profile => profile.ProfileName, StringComparer.Ordinal))
            {
                AppendLazyCachedMember(
                    builder,
                    "    ",
                    "static",
                    interfaceName,
                    GetCatalogPropertyName(profile.ProfileName),
                    CreateConverterExpression(profile, converterTypeName));
                builder.AppendLine();
            }

            builder.AppendLine("}");

            if (!string.IsNullOrWhiteSpace(condition))
            {
                builder.AppendLine();
                builder.AppendLine("#endif");
            }

            context.AddSource(sourceName, SourceText.From(builder.ToString(), Encoding.UTF8));
        }

        static string CreateConverterExpression(OrdinalDateProfileDefinition profile, string converterTypeName) =>
            profile.Engine switch
            {
                "default" => converterTypeName switch
                {
                    "PatternDateToOrdinalWordsConverter" => "new DefaultDateToOrdinalWordConverter()",
                    "PatternDateOnlyToOrdinalWordsConverter" => "new DefaultDateOnlyToOrdinalWordConverter()",
                    _ => throw new InvalidOperationException($"Unsupported default ordinal-date converter '{converterTypeName}'.")
                },
                "pattern" => CreatePatternExpression(profile, converterTypeName),
                _ => throw new InvalidOperationException($"Unsupported ordinal date engine '{profile.Engine}'.")
            };

        static string CreatePatternExpression(OrdinalDateProfileDefinition profile, string converterTypeName)
        {
            var pattern = GetRequiredString(profile.Root, "pattern");
            var hasGregorianMonths = profile.Months.Length > 0;
            var hasHijriMonths = profile.HijriMonths.Length > 0;
            var hasAnyMonthOverride = hasGregorianMonths || hasHijriMonths;

            if (hasAnyMonthOverride && ContainsAbbreviatedMonth(pattern))
            {
                throw new InvalidOperationException(
                    $"Ordinal date profile '{profile.ProfileName}' uses MMM (abbreviated month) " +
                    "which is not supported when calendar month overrides are active. " +
                    "Only MMMM (full month name) substitution is supported.");
            }

            if (hasAnyMonthOverride)
            {
                var mmmmCount = CountUnescapedFullMonth(pattern);
                if (mmmmCount == 0)
                {
                    throw new InvalidOperationException(
                        $"Ordinal date profile '{profile.ProfileName}' has calendar month overrides " +
                        "but pattern does not contain an unescaped MMMM specifier.");
                }

                if (mmmmCount > 1)
                {
                    throw new InvalidOperationException(
                        $"Ordinal date profile '{profile.ProfileName}' has calendar month overrides " +
                        "but pattern contains multiple unescaped MMMM specifiers. Only one is supported.");
                }
            }

            var calendarMode = GetOptionalString(profile.Root, "calendarMode");
            var hasCalendarMode = calendarMode is not null && !string.Equals(calendarMode, "Gregorian", StringComparison.OrdinalIgnoreCase);

            var expr = "new " + converterTypeName + "(new OrdinalDatePattern(" +
                       QuoteLiteral(pattern) +
                       ", OrdinalDateDayMode." +
                       GetRequiredString(profile.Root, "dayMode");

            if (hasCalendarMode)
            {
                var normalized = char.ToUpperInvariant(calendarMode![0]) + calendarMode.Substring(1).ToLowerInvariant();
                expr += ", OrdinalDateCalendarMode." + normalized;
            }

            if (hasAnyMonthOverride)
            {
                if (!hasCalendarMode)
                {
                    expr += ", OrdinalDateCalendarMode.Gregorian";
                }

                expr += hasGregorianMonths
                    ? ", new string[] { " + string.Join(", ", profile.Months.Select(QuoteLiteral)) + " }"
                    : ", null";

                expr += profile.MonthsGenitive.Length > 0
                    ? ", new string[] { " + string.Join(", ", profile.MonthsGenitive.Select(QuoteLiteral)) + " }"
                    : ", null";

                if (hasHijriMonths)
                {
                    expr += ", new string[] { " + string.Join(", ", profile.HijriMonths.Select(QuoteLiteral)) + " }";
                }
            }

            return expr + "))";
        }

        /// <summary>
        /// Returns true if the pattern contains an unescaped MMM that is NOT part of MMMM.
        /// </summary>
        static bool ContainsAbbreviatedMonth(string pattern)
        {
            var inQuote = false;
            for (var i = 0; i < pattern.Length; i++)
            {
                if (pattern[i] == '\'')
                {
                    inQuote = !inQuote;
                    continue;
                }

                if (inQuote)
                {
                    continue;
                }

                if (i + 2 < pattern.Length &&
                    pattern[i] == 'M' && pattern[i + 1] == 'M' && pattern[i + 2] == 'M')
                {
                    // Check if it's exactly MMM (not MMMM).
                    if (i + 3 < pattern.Length && pattern[i + 3] == 'M')
                    {
                        // It's MMMM or longer — skip ahead past all Ms.
                        while (i < pattern.Length && pattern[i] == 'M')
                        {
                            i++;
                        }

                        i--; // Loop will increment.
                        continue;
                    }

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Counts the number of unescaped MMMM (4+ M) specifiers in the pattern.
        /// </summary>
        static int CountUnescapedFullMonth(string pattern)
        {
            var count = 0;
            var inQuote = false;
            for (var i = 0; i < pattern.Length; i++)
            {
                if (pattern[i] == '\'')
                {
                    inQuote = !inQuote;
                    continue;
                }

                if (inQuote)
                {
                    continue;
                }

                if (pattern[i] == 'M')
                {
                    var start = i;
                    while (i < pattern.Length && pattern[i] == 'M')
                    {
                        i++;
                    }

                    if (i - start >= 4)
                    {
                        count++;
                    }

                    i--; // Loop will increment.
                }
            }

            return count;
        }
    }

}
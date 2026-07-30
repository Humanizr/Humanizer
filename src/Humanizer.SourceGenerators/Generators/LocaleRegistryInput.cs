using System.Collections.Immutable;
using System.Text;

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

    sealed class LocaleRegistryInput(
        ImmutableArray<ResolvedLocaleDefinition> locales,
        ImmutableArray<Diagnostic> diagnostics,
        ImmutableHashSet<string> dataBackedFormatterProfiles,
        ImmutableHashSet<string> dataBackedOrdinalizerProfiles)
    {
        /// <summary>
        /// Emits the registry wiring for the resolved locale set.
        /// The catalog already resolved inheritance, so each registration reflects the localized
        /// parent chain and the current canonical locale feature set.
        /// </summary>
        readonly ImmutableArray<ResolvedLocaleDefinition> locales = locales;
        readonly ImmutableArray<Diagnostic> diagnostics = diagnostics;
        readonly ImmutableHashSet<string> dataBackedFormatterProfiles = dataBackedFormatterProfiles;
        readonly ImmutableHashSet<string> dataBackedOrdinalizerProfiles = dataBackedOrdinalizerProfiles;

        static readonly (string RegistryName, Func<ResolvedLocaleDefinition, LocaleFeature?> FeatureSelector)[] RegistrySelectors =
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

        public static LocaleRegistryInput Create(LocaleCatalogInput localeCatalog) =>
            new(
                localeCatalog.Locales,
                localeCatalog.Diagnostics,
                localeCatalog.DataBackedFormatterProfiles,
                localeCatalog.DataBackedOrdinalizerProfiles);

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

            foreach (var (registryName, featureSelector) in RegistrySelectors)
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

            EmitWordsToDecimalNumberRegistrations(context);
            EmitSupportedCultureApi(context);
            EmitNumberFormattingOverrides(context);
        }

        void EmitWordsToDecimalNumberRegistrations(SourceProductionContext context)
        {
            var builder = new StringBuilder();
            builder.AppendLine("using System;");
            builder.AppendLine();
            builder.AppendLine("namespace Humanizer;");
            builder.AppendLine();
            builder.AppendLine("internal static class WordsToDecimalNumberConverterRegistryRegistrations");
            builder.AppendLine("{");
            builder.AppendLine("    internal static void Register(WordsToDecimalNumberConverterRegistry registry)");
            builder.AppendLine("    {");

            foreach (var locale in locales.OrderBy(static locale => locale.LocaleCode, StringComparer.Ordinal))
            {
                if (locale.WordsToNumber is not { } wordsToNumber ||
                    GetOptionalString(wordsToNumber.ProfileRoot, "decimalMarker") is not { } decimalMarker)
                {
                    continue;
                }

                var root = wordsToNumber.ProfileRoot;
                var negativePrefixes = root.TryGetProperty("negativePrefixes", out _)
                    ? CreateOptionalStringArrayExpression(root, "negativePrefixes")
                    : GetOptionalString(root, "minusWord") is { } minusWord
                        ? "new string[] { " + QuoteLiteral(minusWord + " ") + " }"
                        : "Array.Empty<string>()";

                builder.Append("        registry.Register(");
                builder.Append(QuoteLiteral(locale.LocaleCode));
                builder.Append(", culture => new LocalizedWordsToDecimalNumberConverter(culture, ");
                builder.Append(QuoteLiteral(decimalMarker));
                builder.Append(", ");
                builder.Append(negativePrefixes);
                builder.Append(", ");
                builder.Append(CreateOptionalStringArrayExpression(root, "negativeSuffixes"));
                builder.AppendLine("));");
            }

            builder.AppendLine("    }");
            builder.AppendLine("}");

            context.AddSource(
                "WordsToDecimalNumberConverterRegistryRegistrations.g.cs",
                SourceText.From(builder.ToString(), Encoding.UTF8));
        }

        void EmitSupportedCultureApi(SourceProductionContext context)
        {
            var builder = new StringBuilder();
            builder.AppendLine("using System;");
            builder.AppendLine("using System.Collections.Generic;");
            builder.AppendLine("using System.Globalization;");
            builder.AppendLine();
            builder.AppendLine("namespace Humanizer;");
            builder.AppendLine();
            builder.AppendLine("public static partial class Configurator");
            builder.AppendLine("{");
            builder.AppendLine("    /// <summary>");
            builder.AppendLine("    /// Determines whether Humanizer includes complete generated locale support for the specified culture.");
            builder.AppendLine("    /// </summary>");
            builder.AppendLine("    /// <param name=\"culture\">The culture to check.</param>");
            builder.AppendLine("    /// <returns><see langword=\"true\"/> when the culture or one of its named parents has generated locale support; otherwise, <see langword=\"false\"/>.</returns>");
            builder.AppendLine("    /// <remarks>");
            builder.AppendLine("    /// This considers parent-culture fallback, but does not report support merely because Humanizer can fall back to its default English localizers.");
            builder.AppendLine("    /// </remarks>");
            builder.AppendLine("    /// <exception cref=\"ArgumentNullException\">Thrown when <paramref name=\"culture\"/> is <c>null</c>.</exception>");
            builder.AppendLine("    public static bool IsCultureSupported(CultureInfo culture)");
            builder.AppendLine("    {");
            builder.AppendLine("        if (culture is null)");
            builder.AppendLine("        {");
            builder.AppendLine("            throw new ArgumentNullException(nameof(culture));");
            builder.AppendLine("        }");
            builder.AppendLine();
            builder.AppendLine("        for (var current = culture; !string.IsNullOrEmpty(current.Name); current = current.Parent)");
            builder.AppendLine("        {");
            builder.AppendLine("            if (SupportedLocaleCodes.Contains(current.Name))");
            builder.AppendLine("            {");
            builder.AppendLine("                return true;");
            builder.AppendLine("            }");
            builder.AppendLine("        }");
            builder.AppendLine();
            builder.AppendLine("        return false;");
            builder.AppendLine("    }");
            builder.AppendLine();
            builder.AppendLine("    static readonly HashSet<string> SupportedLocaleCodes = new(StringComparer.OrdinalIgnoreCase)");
            builder.AppendLine("    {");

            foreach (var locale in locales.OrderBy(static locale => locale.LocaleCode, StringComparer.Ordinal))
            {
                builder.Append("        \"");
                builder.Append(locale.LocaleCode);
                builder.AppendLine("\",");
            }

            builder.AppendLine("    };");
            builder.AppendLine("}");

            context.AddSource("Configurator.SupportedCultures.g.cs", SourceText.From(builder.ToString(), Encoding.UTF8));
        }

        void EmitNumberFormattingOverrides(SourceProductionContext context)
        {
            var decimalOverrides = new List<(string Locale, string Value)>();
            var negativeSignOverrides = new List<(string Locale, string Value)>();
            var groupSeparatorOverrides = new List<(string Locale, string Value)>();

            foreach (var locale in locales)
            {
                if (locale.NumberFormatting is null)
                {
                    continue;
                }

                var decimalSeparator = locale.NumberFormatting.GetScalar("decimalSeparator");
                if (decimalSeparator is not null)
                {
                    decimalOverrides.Add((locale.LocaleCode, decimalSeparator));
                }

                var negativeSign = locale.NumberFormatting.GetScalar("negativeSign");
                if (negativeSign is not null)
                {
                    negativeSignOverrides.Add((locale.LocaleCode, negativeSign));
                }

                var groupSeparator = locale.NumberFormatting.GetScalar("groupSeparator");
                if (groupSeparator is not null)
                {
                    groupSeparatorOverrides.Add((locale.LocaleCode, groupSeparator));
                }
            }

            if (decimalOverrides.Count == 0 && negativeSignOverrides.Count == 0 && groupSeparatorOverrides.Count == 0)
            {
                return;
            }

            var builder = new StringBuilder();
            builder.AppendLine("#nullable enable");
            builder.AppendLine("using System.Collections.Concurrent;");
            builder.AppendLine("using System.Collections.Generic;");
            builder.AppendLine("using System.Globalization;");
            builder.AppendLine();
            builder.AppendLine("namespace Humanizer;");
            builder.AppendLine();
            builder.AppendLine("internal static class LocaleNumberFormattingOverrides");
            builder.AppendLine("{");

            EmitOverrideDictionary(builder, "DecimalSeparatorOverrides", decimalOverrides);
            EmitOverrideDictionary(builder, "NegativeSignOverrides", negativeSignOverrides);
            EmitOverrideDictionary(builder, "GroupSeparatorOverrides", groupSeparatorOverrides);

            builder.AppendLine("    static readonly ConcurrentDictionary<string, NumberFormatInfo> CachedNumberFormats = new(System.StringComparer.OrdinalIgnoreCase);");
            builder.AppendLine();

            EmitTryGetOverride(builder, "TryGetDecimalSeparator", "DecimalSeparatorOverrides");
            EmitTryGetOverride(builder, "TryGetNegativeSign", "NegativeSignOverrides");
            EmitTryGetOverride(builder, "TryGetGroupSeparator", "GroupSeparatorOverrides");

            // Existing parse-path method: applies only decimal separator override
            builder.AppendLine("    internal static NumberFormatInfo GetCachedNumberFormat(CultureInfo culture, string decimalSeparator)");
            builder.AppendLine("    {");
            builder.AppendLine("        return CachedNumberFormats.GetOrAdd(culture.Name, _ =>");
            builder.AppendLine("        {");
            builder.AppendLine("            var nfi = (NumberFormatInfo)culture.NumberFormat.Clone();");
            builder.AppendLine("            nfi.NumberDecimalSeparator = decimalSeparator;");
            builder.AppendLine("            return nfi;");
            builder.AppendLine("        });");
            builder.AppendLine("    }");
            builder.AppendLine();

            // New formatting-path method: applies all overrides (decimal, negative sign, group separator)
            builder.AppendLine("    internal static NumberFormatInfo GetFormattingNumberFormat(CultureInfo culture)");
            builder.AppendLine("    {");
            builder.AppendLine("        var hasDecimal = TryGetDecimalSeparator(culture, out var decSep);");
            builder.AppendLine("        var hasNegative = TryGetNegativeSign(culture, out var negSign);");
            builder.AppendLine("        var hasGroup = TryGetGroupSeparator(culture, out var grpSep);");
            builder.AppendLine();
            builder.AppendLine("        if (!hasDecimal && !hasNegative && !hasGroup)");
            builder.AppendLine("        {");
            builder.AppendLine("            return culture.NumberFormat;");
            builder.AppendLine("        }");
            builder.AppendLine();
            builder.AppendLine("        return CachedNumberFormats.GetOrAdd(\"fmt:\" + culture.Name, _ =>");
            builder.AppendLine("        {");
            builder.AppendLine("            var nfi = (NumberFormatInfo)culture.NumberFormat.Clone();");
            builder.AppendLine("            if (hasDecimal)");
            builder.AppendLine("            {");
            builder.AppendLine("                nfi.NumberDecimalSeparator = decSep!;");
            builder.AppendLine("            }");
            builder.AppendLine();
            builder.AppendLine("            if (hasNegative)");
            builder.AppendLine("            {");
            builder.AppendLine("                nfi.NegativeSign = negSign!;");
            builder.AppendLine("            }");
            builder.AppendLine();
            builder.AppendLine("            if (hasGroup)");
            builder.AppendLine("            {");
            builder.AppendLine("                nfi.NumberGroupSeparator = grpSep!;");
            builder.AppendLine("            }");
            builder.AppendLine();
            builder.AppendLine("            return nfi;");
            builder.AppendLine("        });");
            builder.AppendLine("    }");
            builder.AppendLine();

            builder.AppendLine("    internal static string GetDecimalSeparator(CultureInfo culture)");
            builder.AppendLine("    {");
            builder.AppendLine("        return TryGetDecimalSeparator(culture, out var sep)");
            builder.AppendLine("            ? sep!");
            builder.AppendLine("            : culture.NumberFormat.NumberDecimalSeparator;");
            builder.AppendLine("    }");
            builder.AppendLine("}");

            context.AddSource("LocaleNumberFormattingOverrides.g.cs", SourceText.From(builder.ToString(), Encoding.UTF8));
        }

        static void EmitOverrideDictionary(StringBuilder builder, string fieldName, List<(string Locale, string Value)> overrides)
        {
            builder.Append("    static readonly Dictionary<string, string> ");
            builder.Append(fieldName);

            if (overrides.Count == 0)
            {
                builder.AppendLine(" = new(System.StringComparer.OrdinalIgnoreCase);");
            }
            else
            {
                builder.AppendLine(" = new(System.StringComparer.OrdinalIgnoreCase)");
                builder.AppendLine("    {");

                foreach (var (locale, value) in overrides.OrderBy(static o => o.Locale, StringComparer.Ordinal))
                {
                    builder.Append("        { \"");
                    builder.Append(locale);
                    builder.Append("\", \"");
                    builder.Append(value.Replace("\\", "\\\\").Replace("\"", "\\\""));
                    builder.AppendLine("\" },");
                }

                builder.AppendLine("    };");
            }

            builder.AppendLine();
        }

        static void EmitTryGetOverride(StringBuilder builder, string methodName, string dictionaryName)
        {
            builder.Append("    internal static bool ");
            builder.Append(methodName);
            builder.AppendLine("(CultureInfo culture, out string? value)");
            builder.AppendLine("    {");
            builder.AppendLine("        var current = culture;");
            builder.AppendLine("        while (current is not null && !string.IsNullOrEmpty(current.Name))");
            builder.AppendLine("        {");
            builder.Append("            if (");
            builder.Append(dictionaryName);
            builder.AppendLine(".TryGetValue(current.Name, out value))");
            builder.AppendLine("            {");
            builder.AppendLine("                return true;");
            builder.AppendLine("            }");
            builder.AppendLine();
            builder.AppendLine("            current = current.Parent;");
            builder.AppendLine("        }");
            builder.AppendLine();
            builder.AppendLine("        value = null;");
            builder.AppendLine("        return false;");
            builder.AppendLine("    }");
            builder.AppendLine();
        }
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
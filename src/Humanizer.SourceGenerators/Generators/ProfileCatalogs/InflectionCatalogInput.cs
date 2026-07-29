using System.Collections.Immutable;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Humanizer.SourceGenerators;

public sealed partial class HumanizerSourceGenerator
{
    sealed class InflectionCatalogInput(
        ImmutableArray<InflectionProfileInput> profiles,
        ImmutableArray<Diagnostic> diagnostics)
    {
        static readonly ImmutableHashSet<string> CardinalRules =
            new[]
            {
                "Other",
                "AmharicLike",
                "Armenian",
                "EnglishLike",
                "Sinhala",
                "Punjabi",
                "One",
                "Danish",
                "Icelandic",
                "Macedonian",
                "Filipino",
                "Latvian",
                "Hebrew",
                "Romanian",
                "SouthSlavic",
                "French",
                "Portuguese",
                "CatalanItalian",
                "Spanish",
                "Slovenian",
                "CzechSlovak",
                "Polish",
                "Belarusian",
                "Lithuanian",
                "RussianUkrainian",
                "Maltese",
                "Irish",
                "Arabic",
                "Welsh"
            }.ToImmutableHashSet(StringComparer.Ordinal);

        readonly ImmutableArray<InflectionProfileInput> profiles = profiles;
        readonly ImmutableArray<Diagnostic> diagnostics = diagnostics;

        public static InflectionCatalogInput Create(LocaleCatalogInput localeCatalog)
        {
            var profiles = ImmutableArray.CreateBuilder<InflectionProfileInput>();
            var diagnostics = ImmutableArray.CreateBuilder<Diagnostic>();
            if (!localeCatalog.Locales.Any(static locale =>
                    locale.AuthoredFeatureNames.Contains("inflection")))
            {
                return new InflectionCatalogInput(profiles.ToImmutable(), diagnostics.ToImmutable());
            }

            foreach (var locale in localeCatalog.Locales)
            {
                var requiresAuthoredProfile = locale.VariantOf is null ||
                    !SharesLanguageSubtag(locale.LocaleCode, locale.VariantOf);
                if (requiresAuthoredProfile &&
                    !locale.AuthoredFeatureNames.Contains("inflection"))
                {
                    diagnostics.Add(CreateDiagnostic(
                        locale.LocaleCode,
                        "Every root or cross-language locale profile must author surfaces.inflection explicitly."));
                    continue;
                }

                if (locale.Inflection is null)
                {
                    diagnostics.Add(CreateDiagnostic(
                        locale.LocaleCode,
                        "No resolved inflection profile is available."));
                    continue;
                }

                try
                {
                    var profile = ParseProfile(locale.LocaleCode, locale.Inflection);
                    profiles.Add(profile);
                    if (locale.AuthoredFeatureNames.Contains("inflection"))
                    {
                        profiles.AddRange(ParseRegionalProfiles(locale.LocaleCode, locale.Inflection));
                    }
                }
                catch (InvalidOperationException exception)
                {
                    diagnostics.Add(CreateDiagnostic(locale.LocaleCode, exception.Message));
                }
            }

            return new InflectionCatalogInput(profiles.ToImmutable(), diagnostics.ToImmutable());
        }

        static bool SharesLanguageSubtag(string localeCode, string inheritedLocaleCode) =>
            string.Equals(
                GetLanguageSubtag(localeCode),
                GetLanguageSubtag(inheritedLocaleCode),
                StringComparison.OrdinalIgnoreCase);

        static string GetLanguageSubtag(string localeCode)
        {
            var separatorIndex = localeCode.IndexOf('-');
            return separatorIndex >= 0
                ? localeCode.Substring(0, separatorIndex)
                : localeCode;
        }

        public void Emit(SourceProductionContext context)
        {
            foreach (var diagnostic in diagnostics)
            {
                context.ReportDiagnostic(diagnostic);
            }

            if (profiles.IsDefaultOrEmpty)
            {
                return;
            }

            var builder = new StringBuilder();
            builder.AppendLine("#nullable enable");
            builder.AppendLine();
            builder.AppendLine("using System.Collections.Frozen;");
            builder.AppendLine("using System.Collections.Generic;");
            builder.AppendLine();
            builder.AppendLine("namespace Humanizer;");
            builder.AppendLine();
            builder.AppendLine("static partial class LocalizedInflectionCatalog");
            builder.AppendLine("{");
            builder.AppendLine("    private static partial bool TryResolveCore(string localeCode, out CardinalPluralRuleKind rule) =>");
            builder.AppendLine("        Rules.TryGetValue(localeCode, out rule);");
            builder.AppendLine();
            builder.AppendLine("    static readonly FrozenDictionary<string, CardinalPluralRuleKind> Rules = new Dictionary<string, CardinalPluralRuleKind>(StringComparer.Ordinal)");
            builder.AppendLine("    {");
            foreach (var profile in profiles.OrderBy(static profile => profile.LocaleCode, StringComparer.Ordinal))
            {
                builder.Append("        [");
                builder.Append(QuoteLiteral(profile.LocaleCode));
                builder.Append("] = CardinalPluralRuleKind.");
                builder.Append(profile.CardinalRule);
                builder.AppendLine(",");
            }

            builder.AppendLine("    }.ToFrozenDictionary(StringComparer.Ordinal);");
            builder.AppendLine("}");
            context.AddSource("LocalizedInflectionCatalog.g.cs", SourceText.From(builder.ToString(), Encoding.UTF8));
        }

        static InflectionProfileInput ParseProfile(string localeCode, SimpleYamlMapping mapping)
        {
            foreach (var property in mapping.Values.Keys.Where(
                         static property => property is not ("cardinalRule" or "regionalRules")))
            {
                throw new InvalidOperationException(
                    $"surfaces.inflection defines unsupported property '{property}'.");
            }

            var cardinalRule = mapping.GetScalar("cardinalRule")
                ?? throw new InvalidOperationException("surfaces.inflection must define cardinalRule.");
            if (!CardinalRules.Contains(cardinalRule))
            {
                throw new InvalidOperationException(
                    $"surfaces.inflection cardinalRule '{cardinalRule}' is not a CLDR 48.2 rule used by Humanizer.");
            }

            return new InflectionProfileInput(localeCode, cardinalRule);
        }

        static ImmutableArray<InflectionProfileInput> ParseRegionalProfiles(
            string localeCode,
            SimpleYamlMapping mapping)
        {
            if (!mapping.TryGetValue("regionalRules", out var regionalRulesValue))
            {
                return [];
            }

            if (regionalRulesValue is not SimpleYamlMapping regionalRules)
            {
                throw new InvalidOperationException("surfaces.inflection.regionalRules must be a mapping.");
            }

            var profiles = ImmutableArray.CreateBuilder<InflectionProfileInput>();
            foreach (var regionalRule in regionalRules.Values.OrderBy(static entry => entry.Key, StringComparer.Ordinal))
            {
                if (!SharesLanguageSubtag(localeCode, regionalRule.Key) ||
                    string.Equals(localeCode, regionalRule.Key, StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException(
                        $"surfaces.inflection.regionalRules locale '{regionalRule.Key}' must be a distinct regional culture of '{localeCode}'.");
                }

                if (regionalRule.Value is not SimpleYamlScalar ruleScalar ||
                    !CardinalRules.Contains(ruleScalar.Value))
                {
                    throw new InvalidOperationException(
                        $"surfaces.inflection.regionalRules.{regionalRule.Key} must name a CLDR 48.2 rule used by Humanizer.");
                }

                profiles.Add(new InflectionProfileInput(
                    regionalRule.Key,
                    ruleScalar.Value));
            }

            return profiles.ToImmutable();
        }

        static Diagnostic CreateDiagnostic(string localeCode, string message) =>
            Diagnostic.Create(
                Diagnostics.InvalidLocaleDefinition,
                Location.None,
                localeCode,
                message);
    }

    sealed class InflectionProfileInput(
        string localeCode,
        string cardinalRule)
    {
        public string LocaleCode { get; } = localeCode;
        public string CardinalRule { get; } = cardinalRule;
    }
}
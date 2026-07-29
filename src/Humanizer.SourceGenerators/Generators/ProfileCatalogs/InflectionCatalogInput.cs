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
        static readonly ImmutableDictionary<string, ImmutableHashSet<string>> ReachableCategories =
            new Dictionary<string, string[]>(StringComparer.Ordinal)
            {
                ["Other"] = ["other"],
                ["AmharicLike"] = ["one", "other"],
                ["Armenian"] = ["one", "other"],
                ["EnglishLike"] = ["one", "other"],
                ["Sinhala"] = ["one", "other"],
                ["Punjabi"] = ["one", "other"],
                ["One"] = ["one", "other"],
                ["Danish"] = ["one", "other"],
                ["Icelandic"] = ["one", "other"],
                ["Macedonian"] = ["one", "other"],
                ["Filipino"] = ["one", "other"],
                ["Latvian"] = ["zero", "one", "other"],
                ["Hebrew"] = ["one", "two", "other"],
                ["Romanian"] = ["one", "few", "other"],
                ["SouthSlavic"] = ["one", "few", "other"],
                ["French"] = ["one", "many", "other"],
                ["Portuguese"] = ["one", "many", "other"],
                ["CatalanItalian"] = ["one", "many", "other"],
                ["Spanish"] = ["one", "many", "other"],
                ["Slovenian"] = ["one", "two", "few", "other"],
                ["CzechSlovak"] = ["one", "few", "many", "other"],
                ["Polish"] = ["one", "few", "many", "other"],
                ["Belarusian"] = ["one", "few", "many", "other"],
                ["Lithuanian"] = ["one", "few", "many", "other"],
                ["RussianUkrainian"] = ["one", "few", "many", "other"],
                ["Maltese"] = ["one", "two", "few", "many", "other"],
                ["Irish"] = ["one", "two", "few", "many", "other"],
                ["Arabic"] = ["zero", "one", "two", "few", "many", "other"],
                ["Welsh"] = ["zero", "one", "two", "few", "many", "other"]
            }.ToImmutableDictionary(
                static entry => entry.Key,
                static entry => entry.Value.ToImmutableHashSet(StringComparer.Ordinal),
                StringComparer.Ordinal);

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
                        profiles.AddRange(ParseRegionalProfiles(locale.LocaleCode, locale.Inflection, profile));
                    }
                }
                catch (Exception exception)
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
            builder.AppendLine("using System;");
            builder.AppendLine("using System.Collections.Frozen;");
            builder.AppendLine("using System.Collections.Generic;");
            builder.AppendLine();
            builder.AppendLine("namespace Humanizer;");
            builder.AppendLine();
            builder.AppendLine("static partial class LocalizedInflectionCatalog");
            builder.AppendLine("{");
            builder.AppendLine("    private static partial bool TryResolveCore(string localeCode, out LocalizedInflectionProfile? profile)");
            builder.AppendLine("    {");
            builder.AppendLine("        if (Profiles.TryGetValue(localeCode, out var factory))");
            builder.AppendLine("        {");
            builder.AppendLine("            profile = factory();");
            builder.AppendLine("            return true;");
            builder.AppendLine("        }");
            builder.AppendLine();
            builder.AppendLine("        profile = null;");
            builder.AppendLine("        return false;");
            builder.AppendLine("    }");
            builder.AppendLine();
            builder.AppendLine("    static readonly FrozenDictionary<string, Func<LocalizedInflectionProfile>> Profiles = new Dictionary<string, Func<LocalizedInflectionProfile>>(StringComparer.Ordinal)");
            builder.AppendLine("    {");
            foreach (var profile in profiles.OrderBy(static profile => profile.LocaleCode, StringComparer.Ordinal))
            {
                builder.Append("        [");
                builder.Append(QuoteLiteral(profile.LocaleCode));
                builder.Append("] = static () => ");
                builder.Append(GetCatalogPropertyName(profile.LocaleCode));
                builder.AppendLine(",");
            }

            builder.AppendLine("    }.ToFrozenDictionary(StringComparer.Ordinal);");
            builder.AppendLine();

            foreach (var profile in profiles.OrderBy(static profile => profile.LocaleCode, StringComparer.Ordinal))
            {
                AppendLazyCachedMember(
                    builder,
                    "    ",
                    "static",
                    "LocalizedInflectionProfile",
                    GetCatalogPropertyName(profile.LocaleCode),
                    CreateProfileExpression(profile));
                builder.AppendLine();
            }

            builder.AppendLine("}");
            context.AddSource("LocalizedInflectionCatalog.g.cs", SourceText.From(builder.ToString(), Encoding.UTF8));
        }

        static InflectionProfileInput ParseProfile(string localeCode, SimpleYamlMapping mapping)
        {
            foreach (var property in mapping.Values.Keys)
            {
                if (property is not ("cardinalRule" or "disposition" or "source" or "lexemes" or "regionalRules"))
                {
                    throw new InvalidOperationException(
                        $"surfaces.inflection defines unsupported property '{property}'.");
                }
            }

            var cardinalRule = mapping.GetScalar("cardinalRule")
                ?? throw new InvalidOperationException("surfaces.inflection must define cardinalRule.");
            if (!ReachableCategories.TryGetValue(cardinalRule, out var reachableCategories))
            {
                throw new InvalidOperationException(
                    $"surfaces.inflection cardinalRule '{cardinalRule}' is not a CLDR 48.2 rule used by Humanizer.");
            }

            var disposition = mapping.GetScalar("disposition")
                ?? throw new InvalidOperationException("surfaces.inflection must define disposition.");
            if (disposition is not ("selector-only" or "lexicon"))
            {
                throw new InvalidOperationException(
                    "surfaces.inflection disposition must be 'selector-only' or 'lexicon'; productive and invariant claims require separate linguistic review.");
            }

            var source = mapping.GetScalar("source")
                ?? throw new InvalidOperationException("surfaces.inflection must define source.");
            if (string.IsNullOrWhiteSpace(source))
            {
                throw new InvalidOperationException("surfaces.inflection source must not be empty.");
            }

            var lexemes = ImmutableArray<InflectionLexemeInput>.Empty;
            if (mapping.TryGetValue("lexemes", out var lexemeValue))
            {
                if (lexemeValue is not SimpleYamlMapping lexemeMapping)
                {
                    throw new InvalidOperationException("surfaces.inflection.lexemes must be a mapping.");
                }

                var builder = ImmutableArray.CreateBuilder<InflectionLexemeInput>();
                var normalizedLemmas = new HashSet<string>(StringComparer.Ordinal);
                foreach (var lexeme in lexemeMapping.Values.OrderBy(static entry => entry.Key, StringComparer.Ordinal))
                {
                    if (lexeme.Value is not SimpleYamlMapping forms)
                    {
                        throw new InvalidOperationException(
                            $"surfaces.inflection.lexemes.{lexeme.Key} must be a mapping.");
                    }

                    var normalizedLemma = lexeme.Key.Normalize(NormalizationForm.FormC);
                    if (!normalizedLemmas.Add(normalizedLemma))
                    {
                        throw new InvalidOperationException(
                            $"surfaces.inflection.lexemes contains canonically equivalent lemma '{lexeme.Key}'.");
                    }

                    foreach (var property in forms.Values.Keys)
                    {
                        if (property is not ("zero" or "one" or "two" or "few" or "many" or "other"))
                        {
                            throw new InvalidOperationException(
                                $"surfaces.inflection.lexemes.{lexeme.Key} defines unsupported category '{property}'.");
                        }
                    }

                    var authoredForms = forms.Values.ToImmutableDictionary(
                        static entry => entry.Key,
                        static entry => entry.Value is SimpleYamlScalar scalar
                            ? scalar.Value.Normalize(NormalizationForm.FormC)
                            : throw new InvalidOperationException("Inflection forms must be scalar strings."),
                        StringComparer.Ordinal);
                    var missing = reachableCategories.Except(authoredForms.Keys, StringComparer.Ordinal).ToArray();
                    if (missing.Length > 0)
                    {
                        throw new InvalidOperationException(
                            $"surfaces.inflection.lexemes.{lexeme.Key} is missing reachable categories: {string.Join(", ", missing)}.");
                    }

                    if (authoredForms.Values.Any(string.IsNullOrWhiteSpace))
                    {
                        throw new InvalidOperationException(
                            $"surfaces.inflection.lexemes.{lexeme.Key} contains an empty form.");
                    }

                    builder.Add(new InflectionLexemeInput(normalizedLemma, authoredForms));
                }

                lexemes = builder.ToImmutable();
            }

            if (disposition == "lexicon" && lexemes.IsEmpty)
            {
                throw new InvalidOperationException(
                    "surfaces.inflection disposition 'lexicon' requires at least one exact lexeme.");
            }

            if (disposition == "selector-only" && !lexemes.IsEmpty)
            {
                throw new InvalidOperationException(
                    "surfaces.inflection disposition 'selector-only' must not define exact lexemes.");
            }

            return new InflectionProfileInput(localeCode, cardinalRule, lexemes);
        }

        static ImmutableArray<InflectionProfileInput> ParseRegionalProfiles(
            string localeCode,
            SimpleYamlMapping mapping,
            InflectionProfileInput profile)
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
                    !ReachableCategories.TryGetValue(ruleScalar.Value, out var reachableCategories))
                {
                    throw new InvalidOperationException(
                        $"surfaces.inflection.regionalRules.{regionalRule.Key} must name a CLDR 48.2 rule used by Humanizer.");
                }

                foreach (var lexeme in profile.Lexemes)
                {
                    var missing = reachableCategories.Except(lexeme.Forms.Keys, StringComparer.Ordinal).ToArray();
                    if (missing.Length > 0)
                    {
                        throw new InvalidOperationException(
                            $"surfaces.inflection.lexemes.{lexeme.Lemma} is missing categories required by regional rule '{regionalRule.Key}': {string.Join(", ", missing)}.");
                    }
                }

                profiles.Add(new InflectionProfileInput(
                    regionalRule.Key,
                    ruleScalar.Value,
                    profile.Lexemes));
            }

            return profiles.ToImmutable();
        }

        static string CreateProfileExpression(InflectionProfileInput profile)
        {
            var builder = new StringBuilder();
            builder.Append("new LocalizedInflectionProfile(CardinalPluralRuleKind.");
            builder.Append(profile.CardinalRule);
            builder.Append(", new Dictionary<string, CardinalInflectionForms>(StringComparer.Ordinal) { ");
            foreach (var lexeme in profile.Lexemes)
            {
                builder.Append('[');
                builder.Append(QuoteLiteral(lexeme.Lemma));
                builder.Append("] = new CardinalInflectionForms(");
                builder.Append(QuoteLiteral(lexeme.Lemma));
                builder.Append(", ");
                builder.Append(QuoteLiteral(lexeme.Forms["other"]));
                builder.Append(", ");
                builder.Append(QuoteOrNull(GetForm(lexeme, "zero")));
                builder.Append(", ");
                builder.Append(QuoteOrNull(GetForm(lexeme, "one")));
                builder.Append(", ");
                builder.Append(QuoteOrNull(GetForm(lexeme, "two")));
                builder.Append(", ");
                builder.Append(QuoteOrNull(GetForm(lexeme, "few")));
                builder.Append(", ");
                builder.Append(QuoteOrNull(GetForm(lexeme, "many")));
                builder.Append("), ");
            }

            builder.Append("})");
            return builder.ToString();
        }

        static string? GetForm(InflectionLexemeInput lexeme, string category) =>
            lexeme.Forms.TryGetValue(category, out var form) ? form : null;

        static string QuoteOrNull(string? value) =>
            value is null ? "null" : QuoteLiteral(value);

        static Diagnostic CreateDiagnostic(string localeCode, string message) =>
            Diagnostic.Create(
                Diagnostics.InvalidLocaleDefinition,
                Location.None,
                localeCode,
                message);
    }

    sealed class InflectionProfileInput(
        string localeCode,
        string cardinalRule,
        ImmutableArray<InflectionLexemeInput> lexemes)
    {
        public string LocaleCode { get; } = localeCode;
        public string CardinalRule { get; } = cardinalRule;
        public ImmutableArray<InflectionLexemeInput> Lexemes { get; } = lexemes;
    }

    sealed class InflectionLexemeInput(
        string lemma,
        ImmutableDictionary<string, string> forms)
    {
        public string Lemma { get; } = lemma;
        public ImmutableDictionary<string, string> Forms { get; } = forms;
    }
}
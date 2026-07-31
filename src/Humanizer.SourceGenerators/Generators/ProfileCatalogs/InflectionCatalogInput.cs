using System.Collections.Immutable;
using System.Globalization;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Humanizer.SourceGenerators;

public sealed partial class HumanizerSourceGenerator
{
    internal static int CompareInflectionKeys(string left, string right) =>
        InflectionCatalogInput.CompareSimpleCase(left, right);

    internal static int FoldInflectionScalar(int scalar) =>
        InflectionCatalogInput.FoldSimpleCase(scalar);

    // Owners use ushort indexes through 65,535 entries, then fall back to int indexes.
    internal static bool RequiresWideInflectionIndexes(int entryCount) =>
        entryCount > ushort.MaxValue;

    sealed class InflectionCatalogInput(
        ImmutableArray<InflectionProfileInput> profiles,
        ImmutableArray<InflectionOwnerInput> owners,
        ImmutableArray<Diagnostic> diagnostics,
        bool hasErrors)
    {
        internal const int MaximumOwnerSourceBytes = 256 * 1024;
        const string ExactNumericSingletonSelector = "exact-numeric-singleton";

        static readonly ImmutableHashSet<string> CardinalRules =
            new[]
            {
                "Other", "AmharicLike", "Armenian", "EnglishLike", "Sinhala", "Punjabi",
                "One", "Danish", "Icelandic", "Macedonian", "Filipino", "Latvian",
                "Hebrew", "Romanian", "SouthSlavic", "French", "Portuguese",
                "CatalanItalian", "Spanish", "Slovenian", "CzechSlovak", "Polish",
                "Belarusian", "Lithuanian", "RussianUkrainian", "Maltese", "Irish",
                "Arabic", "Welsh"
            }.ToImmutableHashSet(StringComparer.Ordinal);

        static readonly ImmutableHashSet<string> Countabilities =
            new[] { "count", "mass", "collective", "plural-only" }
                .ToImmutableHashSet(StringComparer.Ordinal);

        static readonly ImmutableHashSet<string> PluralCategories =
            new[] { "zero", "one", "two", "few", "many", "other" }
                .ToImmutableHashSet(StringComparer.Ordinal);

        static readonly ImmutableHashSet<string> Scripts =
            new[]
            {
                "Arab", "Armn", "Beng", "Cyrl", "Deva", "Geor", "Grek", "Gujr",
                "Guru", "Hani", "Hebr", "Jpan", "Khmr", "Knda", "Kore", "Laoo",
                "Latn", "Mlym", "Mymr", "Orya", "Taml", "Telu", "Thai", "Ethi",
                "Mong", "Sinh"
            }.ToImmutableHashSet(StringComparer.Ordinal);

        readonly ImmutableArray<InflectionProfileInput> profiles = profiles;
        readonly ImmutableArray<InflectionOwnerInput> owners = owners;
        readonly ImmutableArray<Diagnostic> diagnostics = diagnostics;
        readonly bool hasErrors = hasErrors;

        public bool HasErrors => hasErrors;

        public static InflectionCatalogInput Create(
            LocaleCatalogInput localeCatalog,
            InflectionOwnerSourceCatalog ownerSourceCatalog)
        {
            var profiles = ImmutableArray.CreateBuilder<InflectionProfileInput>();
            var owners = ImmutableArray.CreateBuilder<InflectionOwnerInput>();
            var diagnostics = ImmutableArray.CreateBuilder<Diagnostic>();
            if (!localeCatalog.Diagnostics.IsDefaultOrEmpty)
            {
                return new(
                    profiles.ToImmutable(),
                    owners.ToImmutable(),
                    diagnostics.ToImmutable(),
                    hasErrors: true);
            }

            if (!localeCatalog.Locales.Any(static locale =>
                    locale.AuthoredFeatureNames.Contains("inflection")))
            {
                return new(
                    profiles.ToImmutable(),
                    owners.ToImmutable(),
                    diagnostics.ToImmutable(),
                    hasErrors: false);
            }

            foreach (var locale in localeCatalog.Locales)
            {
                _ = localeCatalog.InflectionOwners.TryGetTerminalOwner(
                    locale.LocaleCode,
                    out var terminalOwner);
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
                    if (!locale.AuthoredFeatureNames.Contains("inflection"))
                    {
                        profiles.Add(new(
                            locale.LocaleCode,
                            ParseCardinalRule(locale.Inflection),
                            terminalOwner));
                        continue;
                    }

                    var profile = ParseProfile(locale.LocaleCode, locale.Inflection);
                    profiles.Add(new(profile.LocaleCode, profile.CardinalRule, terminalOwner));
                    profiles.AddRange(ParseRegionalProfiles(
                        locale.LocaleCode,
                        locale.Inflection,
                        terminalOwner));

                    if (profile.Owner is not null &&
                        localeCatalog.InflectionOwners.AtomicOwners.Contains(profile.Owner.Owner))
                    {
                        owners.Add(profile.Owner);
                    }
                }
                catch (InvalidOperationException exception)
                {
                    diagnostics.Add(CreateDiagnostic(locale.LocaleCode, exception.Message));
                }
            }

            var duplicateOwner = owners
                .GroupBy(static owner => owner.Owner, StringComparer.Ordinal)
                .FirstOrDefault(static group => group.Count() > 1);
            if (duplicateOwner is not null)
            {
                diagnostics.Add(CreateDiagnostic(
                    duplicateOwner.Key,
                    $"Atomic inflection owner '{duplicateOwner.Key}' is defined more than once."));
            }

            var duplicateProfile = profiles
                .GroupBy(static profile => profile.LocaleCode, StringComparer.Ordinal)
                .FirstOrDefault(static group => group.Count() > 1);
            if (duplicateProfile is not null)
            {
                diagnostics.Add(CreateDiagnostic(
                    duplicateProfile.Key,
                    $"Inflection profile key '{duplicateProfile.Key}' is defined more than once."));
            }

            ValidateReachableCategories(profiles, owners, diagnostics);

            var sanitizedCollision = owners
                .GroupBy(static owner => SanitizeIdentifier(owner.Owner), StringComparer.Ordinal)
                .FirstOrDefault(static group =>
                    group.Select(static owner => owner.Owner)
                        .Distinct(StringComparer.Ordinal)
                        .Skip(1)
                        .Any());
            if (sanitizedCollision is not null)
            {
                diagnostics.Add(CreateDiagnostic(
                    sanitizedCollision.Key,
                    $"Atomic inflection owners '{string.Join(
                        "', '",
                        sanitizedCollision.Select(static owner => owner.Owner).OrderBy(
                            static owner => owner,
                            StringComparer.Ordinal))}' produce the same generated identifier '{sanitizedCollision.Key}'."));
            }

            diagnostics.AddRange(ownerSourceCatalog.Diagnostics);

            return new(
                profiles.ToImmutable(),
                owners.ToImmutable(),
                diagnostics.ToImmutable(),
                hasErrors: diagnostics.Count > 0);
        }

        static string ParseCardinalRule(SimpleYamlMapping mapping)
        {
            var cardinalRule = mapping.GetScalar("cardinalRule")
                ?? throw new InvalidOperationException(
                    "surfaces.inflection must define cardinalRule.");
            return CardinalRules.Contains(cardinalRule)
                ? cardinalRule
                : throw new InvalidOperationException(
                    $"surfaces.inflection cardinalRule '{cardinalRule}' is not a CLDR 48.2 rule used by Humanizer.");
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

        public void EmitDiagnostics(SourceProductionContext context)
        {
            foreach (var diagnostic in diagnostics)
            {
                context.ReportDiagnostic(diagnostic);
            }
        }

        public InflectionOwnerEmissionState GetOwnerEmissionState() =>
            new(
                hasErrors,
                owners
                    .Select(static owner => owner.Owner)
                    .OrderBy(static owner => owner, StringComparer.Ordinal)
                    .ToImmutableArray());

        public InflectionRegistryInput GetRegistryInput(LocaleCatalogInput localeCatalog) =>
            InflectionRegistryInput.Create(
                profiles,
                owners,
                localeCatalog.AcceptedCultures,
                hasErrors);

        internal sealed class InflectionRegistryInput(
            ImmutableArray<InflectionProfileInput> profiles,
            ImmutableArray<string> owners,
            string ownershipKey,
            bool suppressOutput) : IEquatable<InflectionRegistryInput>
        {
            readonly ImmutableArray<InflectionProfileInput> profiles = profiles;
            readonly ImmutableArray<string> owners = owners;
            readonly string ownershipKey = ownershipKey;
            readonly bool suppressOutput = suppressOutput;

            public string OwnershipKey => ownershipKey;
            public bool HasErrors => suppressOutput;

            public static InflectionRegistryInput Create(
                ImmutableArray<InflectionProfileInput> profiles,
                ImmutableArray<InflectionOwnerInput> owners,
                ImmutableArray<AcceptedCultureInput> acceptedCultures,
                bool suppressOutput)
            {
                var key = new StringBuilder();
                key.Append(suppressOutput ? '1' : '0');
                foreach (var profile in profiles.OrderBy(
                             static profile => profile.LocaleCode,
                             StringComparer.Ordinal))
                {
                    AppendKeyPart(key, profile.LocaleCode);
                    AppendKeyPart(key, profile.CardinalRule);
                    AppendKeyPart(key, profile.Owner);
                }

                foreach (var owner in owners.OrderBy(
                             static owner => owner.Owner,
                             StringComparer.Ordinal))
                {
                    AppendKeyPart(key, owner.Owner);
                    AppendKeyPart(key, owner.CardinalRule);
                    AppendKeyPart(key, owner.Capability);
                    AppendKeyPart(key, owner.Casing);
                    foreach (var script in owner.Scripts)
                    {
                        AppendKeyPart(key, script);
                    }
                }

                foreach (var accepted in acceptedCultures)
                {
                    AppendKeyPart(key, accepted.Name);
                    AppendKeyPart(key, accepted.LocaleProfileOwner);
                    AppendKeyPart(key, accepted.InflectionOwner);
                    AppendKeyPart(key, accepted.InflectionTerminal);
                }

                return new(
                    profiles,
                    owners
                        .Select(static owner => owner.Owner)
                        .OrderBy(static owner => owner, StringComparer.Ordinal)
                        .ToImmutableArray(),
                    key.ToString(),
                    suppressOutput);
            }

            public void Emit(SourceProductionContext context)
            {
                if (suppressOutput || profiles.IsDefaultOrEmpty)
                {
                    return;
                }

                var builder = new StringBuilder();
                builder.AppendLine("#nullable enable");
                builder.AppendLine();
                builder.AppendLine("namespace Humanizer;");
                builder.AppendLine();
                builder.AppendLine("static partial class LocalizedInflectionCatalog");
                builder.AppendLine("{");
                builder.AppendLine("    private static partial bool TryResolveRuleCore(string localeCode, out CardinalPluralRuleKind rule)");
                builder.AppendLine("    {");
                builder.AppendLine("        switch (localeCode)");
                builder.AppendLine("        {");
                foreach (var profile in profiles.OrderBy(static profile => profile.LocaleCode, StringComparer.Ordinal))
                {
                    builder.Append("            // [");
                    builder.Append(QuoteLiteral(profile.LocaleCode));
                    builder.Append("] = CardinalPluralRuleKind.");
                    builder.AppendLine(profile.CardinalRule);
                    builder.Append("            case ");
                    builder.Append(QuoteLiteral(profile.LocaleCode));
                    builder.Append(": rule = CardinalPluralRuleKind.");
                    builder.Append(profile.CardinalRule);
                    builder.AppendLine("; return true;");
                }

                builder.AppendLine("            default: rule = default; return false;");
                builder.AppendLine("        }");
                builder.AppendLine("    }");
                builder.AppendLine();
                builder.AppendLine("    private static partial bool TryResolveBundleCore(string owner, out InflectionBundle? bundle)");
                builder.AppendLine("    {");
                builder.AppendLine("        switch (owner)");
                builder.AppendLine("        {");
                foreach (var owner in owners)
                {
                    builder.Append("            case ");
                    builder.Append(QuoteLiteral(owner));
                    builder.Append(": bundle = GeneratedInflection_");
                    builder.Append(SanitizeIdentifier(owner));
                    builder.AppendLine(".Bundle; return true;");
                }

                builder.AppendLine("            default: bundle = null; return false;");
                builder.AppendLine("        }");
                builder.AppendLine("    }");
                builder.AppendLine("}");
                context.AddSource(
                    "LocalizedInflectionCatalog.g.cs",
                    SourceText.From(builder.ToString(), Encoding.UTF8));
            }

            public bool Equals(InflectionRegistryInput? other) =>
                other is not null &&
                string.Equals(ownershipKey, other.ownershipKey, StringComparison.Ordinal);

            public override bool Equals(object? obj) =>
                Equals(obj as InflectionRegistryInput);

            public override int GetHashCode() =>
                StringComparer.Ordinal.GetHashCode(ownershipKey);

            static void AppendKeyPart(StringBuilder builder, string? value)
            {
                builder.Append(value?.Length ?? -1);
                builder.Append(':');
                builder.Append(value);
                builder.Append('|');
            }
        }

        internal static void EmitOwner(
            SourceProductionContext context,
            string owner,
            string source)
        {
            context.AddSource(
                $"GeneratedInflection_{SanitizeIdentifier(owner)}.g.cs",
                SourceText.From(source, Encoding.UTF8));
        }

        internal static string BuildOwnerSource(InflectionOwnerInput owner)
        {
            var table = BuildLexemeTable(owner);
            var wideEntryIndexes = RequiresWideInflectionIndexes(table.Entries.Length);
            var wideLexemeIndexes = RequiresWideInflectionIndexes(owner.Lexemes.Length);
            var builder = new StringBuilder();
            builder.AppendLine("#nullable enable");
            builder.AppendLine();
            builder.AppendLine("namespace Humanizer;");
            builder.AppendLine();
            builder.Append("static class GeneratedInflection_");
            builder.AppendLine(SanitizeIdentifier(owner.Owner));
            builder.AppendLine("{");
            builder.AppendLine("    internal static InflectionBundle Bundle => Holder.Value;");
            builder.AppendLine();
            builder.AppendLine("    static class Holder");
            builder.AppendLine("    {");
            builder.AppendLine("        internal static readonly InflectionBundle Value = new(");
            builder.Append("            ");
            builder.Append(QuoteLiteral(owner.Owner));
            builder.AppendLine(",");
            builder.Append("            CardinalPluralRuleKind.");
            builder.Append(owner.CardinalRule);
            builder.AppendLine(",");
            builder.Append("            InflectionCapability.");
            builder.Append(ToEnumName(owner.Capability));
            builder.AppendLine(",");
            builder.Append("            InflectionQuantitySelector.");
            builder.Append(ToEnumName(owner.QuantitySelector));
            builder.AppendLine(",");
            builder.Append("            InflectionCasing.");
            builder.Append(ToEnumName(owner.Casing));
            builder.AppendLine(",");
            EmitStringArray(builder, owner.Scripts, 12);
            builder.AppendLine(",");
            EmitStringArray(builder, owner.SkipSimpleWords, 12);
            builder.AppendLine(",");
            builder.AppendLine("            [");
            for (var lexemeIndex = 0; lexemeIndex < owner.Lexemes.Length; lexemeIndex++)
            {
                var lexeme = owner.Lexemes[lexemeIndex];
                var record = table.Lexemes[lexemeIndex];
                if (lexeme.Sense is { } sense)
                {
                    builder.Append("                // sense: ");
                    builder.AppendLine(sense);
                }

                builder.AppendLine("                new InflectionLexemeRecord(");
                builder.Append("                    ");
                builder.Append(QuoteLiteral(lexeme.Id));
                builder.AppendLine(",");
                builder.Append("                    ");
                builder.Append(record.SingularEntryIndex.ToString(CultureInfo.InvariantCulture));
                builder.AppendLine(",");
                builder.Append("                    ");
                builder.Append(record.DictionaryPluralEntryIndex.ToString(CultureInfo.InvariantCulture));
                builder.AppendLine(",");
                builder.AppendLine("                    [");
                foreach (var display in record.Display)
                {
                    builder.Append("                        new(CardinalPluralCategory.");
                    builder.Append(ToEnumName(display.Category));
                    builder.Append(", ");
                    builder.Append(display.PreferredEntryIndex.ToString(CultureInfo.InvariantCulture));
                    builder.AppendLine("),");
                }

                builder.AppendLine("                    ]),");
            }

            builder.AppendLine("            ],");
            builder.AppendLine("            [");
            foreach (var entry in table.Entries)
            {
                builder.Append("                new InflectionLexemeEntry(");
                builder.Append(QuoteLiteral(entry.Value));
                builder.Append(", ");
                builder.Append(entry.CandidateOffset.ToString(CultureInfo.InvariantCulture));
                builder.Append(", ");
                builder.Append(entry.CandidateCount.ToString(CultureInfo.InvariantCulture));
                builder.AppendLine("),");
            }

            builder.AppendLine("            ],");
            builder.AppendLine("            [");
            foreach (var candidate in table.Candidates)
            {
                builder.Append("                new InflectionLexemeCandidate(");
                builder.Append(candidate.LexemeIndex.ToString(CultureInfo.InvariantCulture));
                builder.Append(", (InflectionExactRole)");
                builder.Append(candidate.Roles.ToString(CultureInfo.InvariantCulture));
                builder.AppendLine("),");
            }

            builder.AppendLine("            ],");
            EmitIndexArray(builder, table.ForwardEntries, wideEntryIndexes);
            builder.AppendLine(",");
            EmitIndexArray(builder, table.ReverseEntries, wideEntryIndexes);
            builder.AppendLine(",");
            builder.AppendLine("            [");
            foreach (var rule in owner.Rules)
            {
                builder.Append("                new(");
                builder.Append(QuoteLiteral(rule.Id));
                builder.Append(", InflectionDirection.");
                builder.Append(ToEnumName(rule.Direction));
                builder.Append(", ");
                builder.Append(rule.Priority.ToString(CultureInfo.InvariantCulture));
                builder.Append(", ");
                builder.Append(QuoteLiteral(rule.Prefix));
                builder.Append(", ");
                builder.Append(QuoteLiteral(rule.Suffix));
                builder.Append(", ");
                EmitStringArray(builder, rule.PrecedingNot, 0);
                builder.Append(", ");
                builder.Append(QuoteLiteral(rule.DictionaryPlural));
                builder.Append(", ");
                builder.Append('[');
                foreach (var display in rule.Display.OrderBy(static entry => entry.Key, StringComparer.Ordinal))
                {
                    builder.Append("new(CardinalPluralCategory.");
                    builder.Append(ToEnumName(display.Key));
                    builder.Append(", ");
                    builder.Append(QuoteLiteral(display.Value));
                    builder.Append("), ");
                }

                builder.Append("], ");
                EmitStringArray(builder, rule.ExcludedSurfaces, 0);
                builder.Append(", ");
                if (!rule.ExcludedLexemes.IsDefaultOrEmpty)
                {
                    builder.Append("// excluded lexemes: ");
                    builder.AppendLine(string.Join(", ", rule.ExcludedLexemes));
                    builder.Append("                    ");
                }

                EmitIndexArray(
                    builder,
                    rule.ExcludedLexemes
                        .Select(excluded => FindLexemeIndex(owner.Lexemes, excluded))
                        .ToImmutableArray(),
                    wideLexemeIndexes);
                builder.Append(", ");
                builder.Append(rule.ReverseEnabled ? "true" : "false");
                builder.Append(", ");
                builder.Append(rule.RequiresExistingLexeme ? "true" : "false");
                builder.AppendLine("),");
            }

            builder.AppendLine("            ]);");
            builder.AppendLine("    }");
            builder.AppendLine("}");
            return builder.ToString();
        }

        static void EmitIndexArray(
            StringBuilder builder,
            ImmutableArray<int> indexes,
            bool wide)
        {
            builder.Append(wide ? "new int[] { " : "new ushort[] { ");
            for (var index = 0; index < indexes.Length; index++)
            {
                if (index > 0)
                {
                    builder.Append(", ");
                }

                builder.Append(indexes[index].ToString(CultureInfo.InvariantCulture));
            }

            builder.Append('}');
        }

        static int FindLexemeIndex(
            ImmutableArray<InflectionLexemeInput> lexemes,
            string id)
        {
            for (var index = 0; index < lexemes.Length; index++)
            {
                if (string.Equals(lexemes[index].Id, id, StringComparison.Ordinal))
                {
                    return index;
                }
            }

            throw new InvalidOperationException(
                $"Inflection rule references unknown excluded lexeme '{id}'.");
        }

        static InflectionLexemeTableInput BuildLexemeTable(InflectionOwnerInput owner)
        {
            var comparer = owner.Casing == "lower-title-upper"
                ? InflectionSimpleCaseComparer.Instance
                : StringComparer.Ordinal;
            var candidatesBySurface =
                new Dictionary<string, Dictionary<int, int>>(comparer);
            for (var index = 0; index < owner.Lexemes.Length; index++)
            {
                var lexeme = owner.Lexemes[index];
                AddExactCandidates(
                    candidatesBySurface,
                    lexeme.Singular.Accepted,
                    index,
                    role: 1);
                AddExactCandidates(
                    candidatesBySurface,
                    lexeme.DictionaryPlural.Accepted,
                    index,
                    role: 2);
                foreach (var display in lexeme.Display)
                {
                    AddExactCandidates(
                        candidatesBySurface,
                        display.Value.Accepted,
                        index,
                        GetDisplayRole(display.Key));
                }
            }

            var entries = ImmutableArray.CreateBuilder<InflectionLexemeEntryInput>();
            var candidates = ImmutableArray.CreateBuilder<InflectionLexemeCandidateInput>();
            var entryIndexBySurface = new Dictionary<string, int>(comparer);
            foreach (var pair in candidatesBySurface.OrderBy(
                         static pair => pair.Key,
                         comparer))
            {
                var entryIndex = entries.Count;
                entryIndexBySurface.Add(pair.Key, entryIndex);
                var candidateOffset = candidates.Count;
                foreach (var candidate in pair.Value.OrderBy(
                             static candidate => candidate.Key))
                {
                    candidates.Add(new(candidate.Key, candidate.Value));
                }

                entries.Add(new(
                    pair.Key,
                    candidateOffset,
                    candidates.Count - candidateOffset));
            }

            var lexemes = ImmutableArray.CreateBuilder<InflectionLexemeRecordInput>();
            foreach (var lexeme in owner.Lexemes)
            {
                lexemes.Add(new(
                    entryIndexBySurface[lexeme.Singular.Preferred],
                    entryIndexBySurface[lexeme.DictionaryPlural.Preferred],
                    lexeme.Display
                        .OrderBy(static display => display.Key, StringComparer.Ordinal)
                        .Select(display => new InflectionLexemeDisplayInput(
                            display.Key,
                            entryIndexBySurface[display.Value.Preferred]))
                        .ToImmutableArray()));
            }

            var allIndexes = Enumerable.Range(0, entries.Count).ToImmutableArray();
            return new(
                lexemes.ToImmutable(),
                entries.ToImmutable(),
                candidates.ToImmutable(),
                allIndexes,
                allIndexes);
        }

        static void AddExactCandidates(
            Dictionary<string, Dictionary<int, int>> candidatesBySurface,
            ImmutableArray<string> forms,
            int lexemeIndex,
            int role)
        {
            foreach (var form in forms)
            {
                if (!candidatesBySurface.TryGetValue(form, out var candidates))
                {
                    candidates = [];
                    candidatesBySurface[form] = candidates;
                }

                candidates.TryGetValue(lexemeIndex, out var roles);
                candidates[lexemeIndex] = roles | role;
            }
        }

        static int GetDisplayRole(string category) =>
            category switch
            {
                "zero" => 4,
                "one" => 8,
                "two" => 16,
                "few" => 32,
                "many" => 64,
                "other" => 128,
                _ => throw new InvalidOperationException(
                    $"Unsupported inflection display category '{category}'.")
            };

        // Unicode 16.0.0 CaseFolding.txt C+S mappings, compressed into 697
        // source ranges from 1,484 mappings. Source SHA-256:
        // 6f1f9c588eb4a5c718d9e8f93b782685e5c7fec872cf05e8e6878053599e09bb.
        const int SimpleCaseFoldRangeCount = 697;
        const string SimpleCaseFoldData =
            "QRlAdACODAsWQBgGQCgAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIA" +
            "AgIAAgIAAgIAAgQAAgIAAgIAAgMAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgMAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIA" +
            "AgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIA8QEBAAICAAICAAICAJcEAgCkAwEAAgIAAgIAnAMBAAICAZoD" +
            "AgACAwCeAQEAlAMBAJYDAQACAgCaAwEAngMCAKYDAQCiAwEAAgQApgMBAKoDAgCsAwEAAgIAAgIAAgIAtAMBAAICALQDAwACAgC0" +
            "AwEAAgIBsgMCAAICAAICALYDAQACBAACCAAEAQACAgAEAQACAgAEAQACAgACAgACAgACAgACAgACAgACAgACAgACAwACAgACAgAC" +
            "AgACAgACAgACAgACAgACAgACAwAEAQACAgACAgDBAQEAbwEAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIA" +
            "AgIAAgIAAgIAAgIAAgIAAgIAAgIAgwICAAICAAICAAICAAICAAICAAICAAICAAICAAIIANaoAQEAAgIAxQIBANCoAQMAAgIAhQMB" +
            "AIoBAQCOAQEAAgIAAgIAAgIAAgIAAvcBAOgBKwACAgACBAACCQDoAQcATAICSgQAgAECAX4DEEASCEAfAAINABABADsBADEEAB0B" +
            "ACsCAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAGsBAF8DAHcBAH8CAAICAA0BAAIDAoMCAw+gARAfQFAAAgIA" +
            "AgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgoAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIA" +
            "AgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAHgEAAgIAAgIAAgIAAgIAAgIAAgIA" +
            "AgMAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIA" +
            "AgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgMlYO8W" +
            "JcBxJwDAcQYAwHGrBgUPiBEAm2EBAJlhAQCHYQEBg2ECAIVhAQD3YAEAx2ABAIanBAEAAgcq/y4tAv8uwwIAAgIAAgIAAgIAAgIA" +
            "AgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIA" +
            "AgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIA" +
            "AgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgcAcwMA/XYCAAICAAIC" +
            "AAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAIC" +
            "AAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAIKBw8QBQ8QBw8QBw8Q" +
            "BQ8RAA8CAA8CAA8CAA8JBw8gBw8QBw8QBw8QAQ8CAZMBAgARAgCJcAoDqwEEABEHAIVxBQEPAgHHAQkA5XAFAQ8CAd8BAgANDAH/" +
            "AQIB+wECABGqAgC5dQQA/YIBAQCLgQEHADguDyAjAAKzBhk0yg4vYGAAAgIA7acBAQDLOwEAzacBAwACAgACAgACAgC3qAEBAPmn" +
            "AQEAvagBAQC7qAECAAIDAAIJAf2oAQIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIA" +
            "AgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIA" +
            "AgIAAgIAAgIAAgIAAgIAAgIAAgIAAgkAAgIAAgUAAs7yAQACAgACAgACAgACAgACAgACAgACAgACAgACAgACAgACAgACAgACAgAC" +
            "AgACAgACAgACAgACAgACAgACAgACAgACAgACFAACAgACAgACAgACAgACAgACAgACAgACAgACAgACAgACAgACAgACAgACiAEAAgIA" +
            "AgIAAgIAAgIAAgIAAgIAAgQAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIA" +
            "AgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgsAAgIAAgIAh6gEAQACAgACAgACAgACAgACBQACAgDPlAUDAAICAAIE" +
            "AAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAIeVBQEAnZUFAQCVlQUBAIGVBQEAh5UFAgCjlAUBANOUBQEAqZQFAQDADgEA" +
            "AgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAXwEAhZUFAQDvqAQBAAICAAICAM2VBQEAAgQAAgYAAgIAAgIAAgIAgZkFGQAC+wZPn98E" +
            "lZ8BAAKcCBlA3wknULABI1DAAQpODA5OEAZOCAFO7A0ygAHQARVA0BYfQKCrAR9AwPUBIUQ=";

        static readonly int[] SimpleCaseFoldRanges = DecodeSimpleCaseFoldRanges();

        internal static int CompareSimpleCase(string left, string right)
        {
            var leftIndex = 0;
            var rightIndex = 0;
            while (leftIndex < left.Length && rightIndex < right.Length)
            {
                var leftScalar = FoldSimpleCase(ReadScalar(left, ref leftIndex));
                var rightScalar = FoldSimpleCase(ReadScalar(right, ref rightIndex));
                if (leftScalar != rightScalar)
                {
                    return leftScalar.CompareTo(rightScalar);
                }
            }

            return leftIndex < left.Length
                ? 1
                : rightIndex < right.Length
                    ? -1
                    : 0;
        }

        internal static int FoldSimpleCase(int scalar)
        {
            var low = 0;
            var high = SimpleCaseFoldRangeCount - 1;
            while (low <= high)
            {
                var middle = low + ((high - low) / 2);
                var offset = middle * 3;
                var first = SimpleCaseFoldRanges[offset];
                var last = SimpleCaseFoldRanges[offset + 1];
                if (scalar < first)
                {
                    high = middle - 1;
                }
                else if (scalar > last)
                {
                    low = middle + 1;
                }
                else
                {
                    return scalar + SimpleCaseFoldRanges[offset + 2];
                }
            }

            return scalar;
        }

        static int ReadScalar(string value, ref int index)
        {
            var first = value[index++];
            if (char.IsHighSurrogate(first) &&
                index < value.Length &&
                char.IsLowSurrogate(value[index]))
            {
                return char.ConvertToUtf32(first, value[index++]);
            }

            return first;
        }

        static int[] DecodeSimpleCaseFoldRanges()
        {
            var bytes = Convert.FromBase64String(SimpleCaseFoldData);
            var ranges = new int[SimpleCaseFoldRangeCount * 3];
            var byteIndex = 0;
            var previousFirst = 0;
            for (var rangeIndex = 0; rangeIndex < SimpleCaseFoldRangeCount; rangeIndex++)
            {
                var offset = rangeIndex * 3;
                var first = previousFirst + ReadVarUInt(bytes, ref byteIndex);
                var length = ReadVarUInt(bytes, ref byteIndex);
                var encodedDelta = ReadVarUInt(bytes, ref byteIndex);
                ranges[offset] = first;
                ranges[offset + 1] = first + length;
                ranges[offset + 2] = (encodedDelta & 1) == 0
                    ? encodedDelta / 2
                    : -((encodedDelta + 1) / 2);
                previousFirst = first;
            }

            return ranges;
        }

        static int ReadVarUInt(byte[] bytes, ref int index)
        {
            var value = 0;
            var shift = 0;
            byte current;
            do
            {
                current = bytes[index++];
                value |= (current & 0x7F) << shift;
                shift += 7;
            }
            while ((current & 0x80) != 0);

            return value;
        }

        sealed class InflectionSimpleCaseComparer : StringComparer
        {
            internal static readonly InflectionSimpleCaseComparer Instance = new();

            public override int Compare(string? left, string? right)
            {
                if (ReferenceEquals(left, right))
                {
                    return 0;
                }

                if (left is null)
                {
                    return -1;
                }

                return right is null
                    ? 1
                    : CompareSimpleCase(left, right);
            }

            public override bool Equals(string? left, string? right) =>
                Compare(left, right) == 0;

            public override int GetHashCode(string value)
            {
                var hash = 17;
                for (var index = 0; index < value.Length;)
                {
                    hash = unchecked(
                        (hash * 31) +
                        FoldSimpleCase(ReadScalar(value, ref index)));
                }

                return hash;
            }
        }

        static void EmitStringArray(
            StringBuilder builder,
            ImmutableArray<string> values,
            int indent)
        {
            if (values.IsDefaultOrEmpty)
            {
                builder.Append("[]");
                return;
            }

            builder.Append('[');
            for (var index = 0; index < values.Length; index++)
            {
                if (index > 0)
                {
                    builder.Append(", ");
                }

                builder.Append(QuoteLiteral(values[index]));
            }

            builder.Append(']');
        }

        static string SanitizeIdentifier(string value)
        {
            var builder = new StringBuilder(value.Length);
            foreach (var character in value)
            {
                builder.Append(char.IsLetterOrDigit(character) ? character : '_');
            }

            return builder.ToString();
        }

        static string ToEnumName(string value) =>
            string.Concat(
                value.Split(['-'], StringSplitOptions.RemoveEmptyEntries)
                    .Select(static part => char.ToUpperInvariant(part[0]) + part.Substring(1)));

        static ParsedProfile ParseProfile(string localeCode, SimpleYamlMapping mapping)
        {
            ValidateAliases(mapping, "acceptedCultures", "accepted-cultures");
            ValidateAliases(mapping, "phraseMode", "phrase-mode");
            ValidateAliases(mapping, "skipSimpleWords", "skip-simple-words");
            var supportedProperties = new HashSet<string>(StringComparer.Ordinal)
            {
                "acceptedCultures", "capability", "cardinalRule", "casing", "evidence",
                "accepted-cultures", "lexemes", "owner", "phraseMode", "phrase-mode",
                "quantitySelector", "regionalRules", "rules", "scripts", "skipSimpleWords",
                "skip-simple-words", "sources"
            };
            foreach (var property in mapping.Values.Keys.Where(property => !supportedProperties.Contains(property)))
            {
                throw new InvalidOperationException(
                    $"surfaces.inflection defines unsupported property '{property}'.");
            }

            var cardinalRule = ParseCardinalRule(mapping);
            var quantitySelector = ParseQuantitySelector(mapping);

            var capability = mapping.GetScalar("capability");
            if (quantitySelector is not null &&
                capability != "display-by-category")
            {
                throw new InvalidOperationException(
                    "Inflection quantitySelector 'exact-numeric-singleton' requires capability 'display-by-category'.");
            }

            if (quantitySelector is not null &&
                cardinalRule != "Other")
            {
                throw new InvalidOperationException(
                    "Inflection quantitySelector 'exact-numeric-singleton' requires cardinalRule 'Other'.");
            }

            if (capability is null or "inert")
            {
                return new(localeCode, cardinalRule, null);
            }

            if (capability == "alias")
            {
                var aliasProperties = new HashSet<string>(StringComparer.Ordinal)
                {
                    "acceptedCultures", "accepted-cultures", "capability", "cardinalRule", "owner"
                };
                var unsupported = mapping.Values.Keys.FirstOrDefault(
                    property => !aliasProperties.Contains(property));
                if (unsupported is not null)
                {
                    throw new InvalidOperationException(
                        $"Inflection alias defines unsupported property '{unsupported}'.");
                }

                if (mapping.GetScalar("owner") is not { Length: > 0 })
                {
                    throw new InvalidOperationException(
                        "Inflection alias must define its atomic owner.");
                }

                return new(localeCode, cardinalRule, null);
            }

            if (capability is not ("display-by-category" or "invariant"))
            {
                throw new InvalidOperationException(
                    $"surfaces.inflection capability '{capability}' is unsupported.");
            }

            var scripts = GetRequiredStrings(mapping, "scripts", "active inflection bundle must define scripts");
            if (scripts.Any(static script => !Scripts.Contains(script)))
            {
                throw new InvalidOperationException(
                    "Active inflection bundle declares an unsupported Unicode script.");
            }

            var casing = GetRequiredScalar(mapping, "casing", "active inflection bundle must define casing");
            if (casing is not ("exact" or "lower-title-upper" or "none"))
            {
                throw new InvalidOperationException($"Unsupported inflection casing mode '{casing}'.");
            }

            var phraseMode = GetRequiredScalar(
                mapping,
                "phraseMode",
                "phrase-mode",
                "active inflection bundle must define phraseMode");
            if (phraseMode != "exact-only")
            {
                throw new InvalidOperationException("Active inflection phraseMode must be 'exact-only'.");
            }

            var sources = GetRequiredMapping(mapping, "sources", "active inflection bundle must define sources");
            ValidateSourceDefinitions(sources);
            var sourceIds = sources.Values.Keys.ToImmutableHashSet(StringComparer.Ordinal);
            var lexemes = ParseLexemes(
                mapping,
                cardinalRule,
                sourceIds,
                scripts,
                casing,
                allowEmpty: capability == "invariant");
            var rules = ParseRules(
                mapping,
                sourceIds,
                lexemes
                    .Select(static lexeme => lexeme.Id)
                    .ToImmutableHashSet(StringComparer.Ordinal),
                scripts,
                cardinalRule,
                casing);
            ValidateQuantitySelector(
                mapping,
                cardinalRule,
                quantitySelector,
                lexemes,
                rules);
            ValidateInvariantCapability(capability, casing, lexemes, rules);
            ValidateEvidence(mapping, rules, sourceIds);

            return new(
                localeCode,
                cardinalRule,
                new(
                    mapping.GetScalar("owner") ?? localeCode,
                    cardinalRule,
                    capability,
                    quantitySelector ?? "none",
                    scripts,
                    casing,
                    phraseMode,
                    NormalizeGuards(
                        GetOptionalStrings(mapping, "skipSimpleWords", "skip-simple-words"),
                        casing,
                        scripts,
                        "skipSimpleWords"),
                    lexemes,
                    rules));
        }

        static string? ParseQuantitySelector(SimpleYamlMapping mapping)
        {
            if (!mapping.TryGetValue("quantitySelector", out var value))
            {
                return null;
            }

            return value is SimpleYamlScalar { Value: ExactNumericSingletonSelector }
                ? ExactNumericSingletonSelector
                : throw new InvalidOperationException(
                    "Inflection defines unsupported quantity selector; expected 'exact-numeric-singleton'.");
        }

        static void ValidateQuantitySelector(
            SimpleYamlMapping mapping,
            string cardinalRule,
            string? quantitySelector,
            ImmutableArray<InflectionLexemeInput> lexemes,
            ImmutableArray<InflectionRuleInput> rules)
        {
            var hasOptedLexeme = lexemes.Any(static lexeme =>
                lexeme.Display.ContainsKey("one"));
            if (quantitySelector is not null)
            {
                if (!hasOptedLexeme)
                {
                    throw new InvalidOperationException(
                        "Inflection quantitySelector 'exact-numeric-singleton' requires at least one lexeme with an authored 'one' display.");
                }

                if (rules.Any(static rule => rule.Display.ContainsKey("one")))
                {
                    throw new InvalidOperationException(
                        "Inflection quantitySelector 'exact-numeric-singleton' cannot select productive rule display.");
                }

                return;
            }

            if (!IsCategoryReachable(mapping, cardinalRule, "one") &&
                (hasOptedLexeme ||
                 rules.Any(static rule => rule.Display.ContainsKey("one"))))
            {
                throw new InvalidOperationException(
                    "Inflection display category 'one' is unreachable without quantitySelector 'exact-numeric-singleton'.");
            }
        }

        static bool IsCategoryReachable(
            SimpleYamlMapping mapping,
            string cardinalRule,
            string category)
        {
            if (ReachableCategories(cardinalRule).Contains(category, StringComparer.Ordinal))
            {
                return true;
            }

            return mapping.TryGetValue("regionalRules", out var regionalRulesValue) &&
                regionalRulesValue is SimpleYamlMapping regionalRules &&
                regionalRules.Values.Any(entry =>
                    entry.Value is SimpleYamlScalar rule &&
                    CardinalRules.Contains(rule.Value) &&
                    ReachableCategories(rule.Value).Contains(category, StringComparer.Ordinal));
        }

        static ImmutableArray<InflectionLexemeInput> ParseLexemes(
            SimpleYamlMapping mapping,
            string cardinalRule,
            ImmutableHashSet<string> sourceIds,
            ImmutableArray<string> ownerScripts,
            string casing,
            bool allowEmpty)
        {
            if (!mapping.TryGetValue("lexemes", out var lexemeValue) ||
                lexemeValue is not SimpleYamlSequence lexemeSequence)
            {
                throw new InvalidOperationException("Active inflection bundle must define lexemes.");
            }

            if (lexemeSequence.Items.IsEmpty)
            {
                return allowEmpty
                    ? []
                    : throw new InvalidOperationException(
                        "Active inflection bundle must define lexemes.");
            }

            var ids = new HashSet<string>(StringComparer.Ordinal);
            var lexemes = ImmutableArray.CreateBuilder<InflectionLexemeInput>();
            foreach (var value in lexemeSequence.Items)
            {
                if (value is not SimpleYamlMapping lexeme)
                {
                    throw new InvalidOperationException("Inflection lexemes must be mappings.");
                }

                ValidateProperties(
                    lexeme,
                    "Inflection lexeme",
                    "id", "pos", "countability", "sense", "forms", "sources");
                var id = GetRequiredScalar(lexeme, "id", "Inflection lexeme must define id");
                if (!ids.Add(id))
                {
                    throw new InvalidOperationException($"Duplicate inflection lexeme id '{id}'.");
                }

                if (GetRequiredScalar(lexeme, "pos", $"Inflection lexeme '{id}' must define pos") != "noun")
                {
                    throw new InvalidOperationException($"Inflection lexeme '{id}' pos must be 'noun'.");
                }

                var countability = GetRequiredScalar(
                    lexeme,
                    "countability",
                    $"Inflection lexeme '{id}' must define countability");
                if (!Countabilities.Contains(countability))
                {
                    throw new InvalidOperationException(
                        $"Inflection lexeme '{id}' countability '{countability}' is unsupported.");
                }

                string? sense = null;
                if (lexeme.TryGetValue("sense", out var senseValue))
                {
                    if (senseValue is not SimpleYamlScalar senseScalar ||
                        string.IsNullOrWhiteSpace(senseScalar.Value))
                    {
                        throw new InvalidOperationException(
                            $"Inflection lexeme '{id}' sense must be a non-empty scalar.");
                    }

                    sense = senseScalar.Value;
                }

                ValidateSources(lexeme, sourceIds, $"Inflection lexeme '{id}'");
                var forms = GetRequiredMapping(lexeme, "forms", $"Inflection lexeme '{id}' must define forms");
                ValidateProperties(
                    forms,
                    $"Inflection lexeme '{id}' forms",
                    "singular", "dictionaryPlural", "dictionary-plural", "display");
                var singular = ParseForm(forms, "singular", alternateName: null, id, ownerScripts, casing);
                var dictionaryPlural = ParseForm(
                    forms,
                    "dictionaryPlural",
                    "dictionary-plural",
                    id,
                    ownerScripts,
                    casing);
                var displayMapping = GetRequiredMapping(
                    forms,
                    "display",
                    $"Inflection lexeme '{id}' must define display forms");
                var display = ImmutableDictionary.CreateBuilder<string, InflectionFormInput>(StringComparer.Ordinal);
                foreach (var category in ReachableCategories(cardinalRule))
                {
                    display[category] = ParseForm(
                        displayMapping,
                        category,
                        alternateName: null,
                        id,
                        ownerScripts,
                        casing);
                }

                foreach (var category in displayMapping.Values.Keys.OrderBy(
                             static value => value,
                             StringComparer.Ordinal))
                {
                    if (!PluralCategories.Contains(category))
                    {
                        throw new InvalidOperationException(
                            $"Inflection lexeme '{id}' defines unsupported display category '{category}'.");
                    }

                    if (!display.ContainsKey(category))
                    {
                        display[category] = ParseForm(
                            displayMapping,
                            category,
                            alternateName: null,
                            id,
                            ownerScripts,
                            casing);
                    }
                }

                lexemes.Add(new(
                    id,
                    sense,
                    singular,
                    dictionaryPlural,
                    display.ToImmutable()));
            }

            return lexemes.ToImmutable();
        }

        static InflectionFormInput ParseForm(
            SimpleYamlMapping parent,
            string name,
            string? alternateName,
            string lexemeId,
            ImmutableArray<string> ownerScripts,
            string casing)
        {
            var mapping = GetRequiredMapping(
                parent,
                name,
                alternateName,
                $"Inflection lexeme '{lexemeId}' must define form '{name}'");
            ValidateProperties(
                mapping,
                $"Inflection lexeme '{lexemeId}' form '{name}'",
                "preferred", "accepted");
            var subject = $"lexeme '{lexemeId}' form '{name}'";
            var preferred = NormalizeAuthoredText(
                GetRequiredScalar(
                    mapping,
                    "preferred",
                    $"Inflection lexeme '{lexemeId}' form '{name}' must define preferred"),
                casing,
                ownerScripts,
                subject,
                allowStemPlaceholder: false,
                allowNonLetters: true);
            var accepted = NormalizeAuthoredTexts(
                GetRequiredStrings(
                    mapping,
                    "accepted",
                    $"Inflection lexeme '{lexemeId}' form '{name}' must define accepted"),
                casing,
                ownerScripts,
                subject,
                allowStemPlaceholder: false,
                allowNonLetters: true);
            if (!accepted.Contains(preferred, StringComparer.Ordinal))
            {
                throw new InvalidOperationException(
                    $"Inflection lexeme '{lexemeId}' form '{name}' preferred value must be accepted.");
            }

            return new(preferred, accepted);
        }

        static ImmutableArray<InflectionRuleInput> ParseRules(
            SimpleYamlMapping mapping,
            ImmutableHashSet<string> sourceIds,
            ImmutableHashSet<string> lexemeIds,
            ImmutableArray<string> ownerScripts,
            string cardinalRule,
            string casing)
        {
            if (!mapping.TryGetValue("rules", out var value))
            {
                return [];
            }

            if (value is not SimpleYamlSequence sequence)
            {
                throw new InvalidOperationException("Inflection rules must be a block sequence.");
            }

            var rules = ImmutableArray.CreateBuilder<InflectionRuleInput>();
            var ids = new HashSet<string>(StringComparer.Ordinal);
            foreach (var item in sequence.Items)
            {
                if (item is not SimpleYamlMapping rule)
                {
                    throw new InvalidOperationException("Inflection rules must be mappings.");
                }

                ValidateProperties(
                    rule,
                    "Inflection rule",
                    "id", "direction", "priority", "scope", "match", "output",
                    "hostileExclusions", "hostile-exclusions", "reverse", "sources");
                var id = GetRequiredScalar(rule, "id", "Inflection rule must define id");
                if (!ids.Add(id))
                {
                    throw new InvalidOperationException($"Duplicate inflection rule id '{id}'.");
                }

                ValidateSources(rule, sourceIds, $"Inflection rule '{id}'");
                var direction = GetRequiredScalar(rule, "direction", $"Inflection rule '{id}' must define direction");
                if (direction is not ("forward" or "reverse"))
                {
                    throw new InvalidOperationException($"Inflection rule '{id}' has unsupported direction '{direction}'.");
                }

                var priority = GetRequiredInt(rule, "priority", $"Inflection rule '{id}' must define priority");
                var scope = GetRequiredMapping(rule, "scope", $"Inflection rule '{id}' must define scope");
                ValidateProperties(
                    scope,
                    $"Inflection rule '{id}' scope",
                    "pos", "countability", "token", "scripts");
                if (GetRequiredScalar(scope, "pos", $"Inflection rule '{id}' scope must define pos") != "noun" ||
                    GetRequiredScalar(scope, "token", $"Inflection rule '{id}' scope must define token") != "standalone")
                {
                    throw new InvalidOperationException(
                        $"Inflection rule '{id}' scope must target standalone nouns.");
                }

                var countabilities = GetRequiredStrings(
                    scope,
                    "countability",
                    $"Inflection rule '{id}' scope must define countability");
                foreach (var countability in countabilities)
                {
                    if (!Countabilities.Contains(countability))
                    {
                        throw new InvalidOperationException(
                            $"Inflection rule '{id}' scope countability '{countability}' is unsupported.");
                    }
                }
                var ruleScripts = GetRequiredStrings(
                    scope,
                    "scripts",
                    $"Inflection rule '{id}' scope must define scripts");
                if (ruleScripts.Any(script => !ownerScripts.Contains(script, StringComparer.Ordinal)))
                {
                    throw new InvalidOperationException(
                        $"Inflection rule '{id}' scope uses a script not declared by its owner.");
                }

                var match = GetRequiredMapping(rule, "match", $"Inflection rule '{id}' must define match");
                ValidateProperties(
                    match,
                    $"Inflection rule '{id}' match",
                    "prefix", "suffix", "precedingNot", "preceding-not");
                var prefix = NormalizeAuthoredText(
                    match.GetScalar("prefix") ?? string.Empty,
                    casing,
                    ownerScripts,
                    $"rule '{id}' prefix",
                    allowStemPlaceholder: false,
                    allowNonLetters: false);
                var suffix = NormalizeAuthoredText(
                    match.GetScalar("suffix") ?? string.Empty,
                    casing,
                    ownerScripts,
                    $"rule '{id}' suffix",
                    allowStemPlaceholder: false,
                    allowNonLetters: false);
                if ((prefix.Length == 0) == (suffix.Length == 0))
                {
                    throw new InvalidOperationException(
                        $"Inflection rule '{id}' must define exactly one non-empty prefix or suffix.");
                }

                var precedingNot = NormalizeGuards(
                    GetOptionalStrings(match, "precedingNot", "preceding-not"),
                    casing,
                    ownerScripts,
                    $"rule '{id}' precedingNot");
                var output = GetRequiredMapping(rule, "output", $"Inflection rule '{id}' must define output");
                ValidateProperties(
                    output,
                    $"Inflection rule '{id}' output",
                    "dictionaryPlural", "dictionary-plural", "display");
                var dictionaryPlural = NormalizeAuthoredText(
                    GetRequiredScalar(
                        output,
                        "dictionaryPlural",
                        "dictionary-plural",
                        $"Inflection rule '{id}' must define dictionaryPlural output"),
                    casing,
                    ownerScripts,
                    $"rule '{id}' dictionaryPlural output",
                    allowStemPlaceholder: true,
                    allowNonLetters: false);
                ValidateTemplate(id, dictionaryPlural);
                var display = ImmutableDictionary.CreateBuilder<string, string>(StringComparer.Ordinal);
                var displayMapping = GetRequiredMapping(
                    output,
                    "display",
                    $"Inflection rule '{id}' must define display output");
                foreach (var category in ReachableCategories(cardinalRule))
                {
                    var template = NormalizeAuthoredText(
                        GetRequiredScalar(
                            displayMapping,
                            category,
                            $"Inflection rule '{id}' must define display output '{category}'"),
                        casing,
                        ownerScripts,
                        $"rule '{id}' display output '{category}'",
                        allowStemPlaceholder: true,
                        allowNonLetters: false);
                    ValidateTemplate(id, template);
                    display[category] = template;
                }

                foreach (var category in displayMapping.Values.Keys.OrderBy(
                             static value => value,
                             StringComparer.Ordinal))
                {
                    if (!PluralCategories.Contains(category))
                    {
                        throw new InvalidOperationException(
                            $"Inflection rule '{id}' defines unsupported display category '{category}'.");
                    }

                    if (!display.ContainsKey(category))
                    {
                        var template = NormalizeAuthoredText(
                            GetRequiredScalar(
                                displayMapping,
                                category,
                                $"Inflection rule '{id}' must define display output '{category}'"),
                            casing,
                            ownerScripts,
                            $"rule '{id}' display output '{category}'",
                            allowStemPlaceholder: true,
                            allowNonLetters: false);
                        ValidateTemplate(id, template);
                        display[category] = template;
                    }
                }

                var hostileExclusions = GetOptionalMapping(
                    rule,
                    "hostileExclusions",
                    "hostile-exclusions");
                if (hostileExclusions is not null)
                {
                    ValidateProperties(
                        hostileExclusions,
                        $"Inflection rule '{id}' hostile exclusions",
                        "lexemes", "surfaces");
                }

                var excludedSurfaces = hostileExclusions is null
                    ? ImmutableArray<string>.Empty
                    : NormalizeGuards(
                        GetOptionalStrings(hostileExclusions, "surfaces"),
                        casing,
                        ownerScripts,
                        $"rule '{id}' hostile exclusions");
                var excludedLexemes = hostileExclusions is null
                    ? ImmutableArray<string>.Empty
                    : GetOptionalStrings(hostileExclusions, "lexemes");
                if (excludedLexemes.Distinct(StringComparer.Ordinal).Count() !=
                    excludedLexemes.Length)
                {
                    throw new InvalidOperationException(
                        $"Inflection rule '{id}' contains a duplicate excluded lexeme.");
                }

                foreach (var excludedLexeme in excludedLexemes)
                {
                    if (!lexemeIds.Contains(excludedLexeme))
                    {
                        throw new InvalidOperationException(
                            $"Inflection rule '{id}' references unknown excluded lexeme '{excludedLexeme}'.");
                    }
                }

                var reverse = GetOptionalMapping(
                    rule,
                    "reverse",
                    alternateName: null);
                if (reverse is not null)
                {
                    ValidateProperties(
                        reverse,
                        $"Inflection rule '{id}' reverse",
                        "enabled", "requiresExistingLexeme", "requires-existing-lexeme");
                }

                var reverseEnabled = GetOptionalBoolean(
                    reverse,
                    "enabled",
                    alternateName: null,
                    defaultValue: false);
                if (direction == "reverse" && reverseEnabled)
                {
                    throw new InvalidOperationException(
                        $"Inflection rule '{id}' reverse.enabled is only valid for a forward rule.");
                }

                if (reverseEnabled &&
                    !dictionaryPlural.StartsWith("{stem}", StringComparison.Ordinal))
                {
                    throw new InvalidOperationException(
                        $"Inflection rule '{id}' reverse-enabled output must start with '{{stem}}'.");
                }

                rules.Add(new(
                    id,
                    direction,
                    priority,
                    prefix,
                    suffix,
                    precedingNot,
                    dictionaryPlural,
                    display.ToImmutable(),
                    excludedSurfaces,
                    excludedLexemes,
                    reverseEnabled,
                    GetOptionalBoolean(
                        reverse,
                        "requiresExistingLexeme",
                        "requires-existing-lexeme",
                        defaultValue: true)));
            }

            var conflict = rules
                .GroupBy(static rule => (rule.Direction, rule.Priority, rule.Prefix, rule.Suffix))
                .FirstOrDefault(static group => group.Count() > 1);
            if (conflict is not null)
            {
                throw new InvalidOperationException(
                    $"Inflection rules have an unresolved direction/priority/suffix tie for '{conflict.Key.Suffix}'.");
            }

            return rules
                .OrderByDescending(static rule => rule.Priority)
                .ThenByDescending(static rule =>
                    rule.Prefix.Length > 0
                        ? rule.Prefix.Length
                        : rule.Suffix.Length)
                .ThenBy(static rule => rule.Id, StringComparer.Ordinal)
                .ToImmutableArray();
        }

        static void ValidateEvidence(
            SimpleYamlMapping mapping,
            ImmutableArray<InflectionRuleInput> rules,
            ImmutableHashSet<string> sourceIds)
        {
            var evidence = GetRequiredMapping(
                mapping,
                "evidence",
                "Active inflection bundle must define evidence");
            ValidateProperties(
                evidence,
                "Inflection evidence",
                "methodology", "pluralize", "singularize");
            _ = GetRequiredScalar(evidence, "methodology", "Inflection evidence must define methodology");
            ValidateDirectionEvidence(
                evidence,
                "pluralize",
                rules.Any(static rule => rule.Direction == "forward"),
                sourceIds);
            ValidateDirectionEvidence(
                evidence,
                "singularize",
                rules.Any(static rule => rule.Direction == "reverse" || rule.ReverseEnabled),
                sourceIds);
        }

        static void ValidateDirectionEvidence(
            SimpleYamlMapping evidence,
            string direction,
            bool hasProductiveRules,
            ImmutableHashSet<string> sourceIds)
        {
            var values = GetRequiredMapping(
                evidence,
                direction,
                $"Inflection evidence must define '{direction}'");
            ValidateProperties(
                values,
                $"Inflection evidence '{direction}'",
                "eligible", "irregular", "covered", "attempted", "correct", "sources");
            var eligible = GetRequiredInt(values, "eligible", $"Inflection evidence '{direction}' must define eligible");
            var irregular = GetRequiredInt(values, "irregular", $"Inflection evidence '{direction}' must define irregular");
            var covered = GetRequiredInt(values, "covered", $"Inflection evidence '{direction}' must define covered");
            ValidateSources(
                values,
                sourceIds,
                $"Inflection evidence '{direction}'");
            if (eligible < 0 || irregular < 0 || covered < 0 ||
                covered > irregular || irregular > eligible)
            {
                throw new InvalidOperationException(
                    $"Inflection evidence '{direction}' must satisfy 0 <= covered <= irregular <= eligible.");
            }

            if (irregular > 0 && covered * 100L < irregular * 95L)
            {
                throw new InvalidOperationException(
                    $"Inflection evidence '{direction}' does not meet 95% irregular-occurrence coverage.");
            }

            if (irregular == 0 && eligible == 0)
            {
                throw new InvalidOperationException(
                    $"Inflection evidence '{direction}' N/A coverage requires a positive eligible census.");
            }

            if (!hasProductiveRules)
            {
                return;
            }

            var attempted = GetRequiredInt(values, "attempted", $"Productive evidence '{direction}' must define attempted");
            var correct = GetRequiredInt(values, "correct", $"Productive evidence '{direction}' must define correct");
            if (attempted < 100 || correct < 0 || correct > attempted ||
                correct * 100L < attempted * 99L)
            {
                throw new InvalidOperationException(
                    $"Productive evidence '{direction}' requires at least 100 attempts and 99% correctness.");
            }
        }

        static void ValidateSources(
            SimpleYamlMapping mapping,
            ImmutableHashSet<string> sourceIds,
            string subject)
        {
            foreach (var source in GetRequiredStrings(mapping, "sources", $"{subject} must define sources"))
            {
                if (!sourceIds.Contains(source))
                {
                    throw new InvalidOperationException($"{subject} references unknown source '{source}'.");
                }
            }
        }

        static void ValidateSourceDefinitions(SimpleYamlMapping sources)
        {
            foreach (var entry in sources.Values)
            {
                if (entry.Value is not SimpleYamlMapping source ||
                    source.GetScalar("kind") is not { Length: > 0 } ||
                    source.GetScalar("locator") is not { Length: > 0 })
                {
                    throw new InvalidOperationException(
                        $"Inflection source '{entry.Key}' must define non-empty kind and locator.");
                }

                ValidateProperties(
                    source,
                    $"Inflection source '{entry.Key}'",
                    "kind", "locator", "revision", "credit");
                foreach (var optionalProperty in new[] { "revision", "credit" })
                {
                    if (source.TryGetValue(optionalProperty, out var optionalValue) &&
                        (optionalValue is not SimpleYamlScalar optionalScalar ||
                         string.IsNullOrWhiteSpace(optionalScalar.Value)))
                    {
                        throw new InvalidOperationException(
                            $"Inflection source '{entry.Key}' {optionalProperty} must be a non-empty scalar.");
                    }
                }
            }
        }

        static void ValidateTemplate(string ruleId, string template)
        {
            var marker = template.IndexOf("{stem}", StringComparison.Ordinal);
            var remainder = template.Replace("{stem}", string.Empty);
            if (marker < 0 ||
                template.IndexOf("{stem}", marker + "{stem}".Length, StringComparison.Ordinal) >= 0 ||
                remainder.Contains('{') ||
                remainder.Contains('}'))
            {
                throw new InvalidOperationException(
                    $"Inflection rule '{ruleId}' output must contain exactly one bounded '{{stem}}' placeholder.");
            }
        }

        static void ValidateInvariantCapability(
            string capability,
            string casing,
            ImmutableArray<InflectionLexemeInput> lexemes,
            ImmutableArray<InflectionRuleInput> rules)
        {
            if (capability != "invariant")
            {
                return;
            }

            if (!rules.IsEmpty)
            {
                throw new InvalidOperationException(
                    "Invariant bundle cannot define productive rules.");
            }

            foreach (var lexeme in lexemes)
            {
                var forms = lexeme.Singular.Accepted
                    .Concat(lexeme.DictionaryPlural.Accepted)
                    .Concat(lexeme.Display.Values.SelectMany(static form => form.Accepted))
                    .Distinct(casing == "lower-title-upper"
                        ? InflectionSimpleCaseComparer.Instance
                        : StringComparer.Ordinal);
                if (forms.Skip(1).Any())
                {
                    throw new InvalidOperationException(
                        $"Invariant inflection lexeme '{lexeme.Id}' has divergent reachable forms.");
                }
            }
        }

        static ImmutableArray<string> NormalizeAuthoredTexts(
            ImmutableArray<string> values,
            string casing,
            ImmutableArray<string> ownerScripts,
            string subject,
            bool allowStemPlaceholder,
            bool allowNonLetters)
        {
            var normalized = ImmutableArray.CreateBuilder<string>(values.Length);
            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var value in values)
            {
                var form = NormalizeAuthoredText(
                    value,
                    casing,
                    ownerScripts,
                    subject,
                    allowStemPlaceholder,
                    allowNonLetters);
                if (!seen.Add(form))
                {
                    throw new InvalidOperationException(
                        $"Inflection {subject} contains a duplicate after normalization.");
                }

                normalized.Add(form);
            }

            return normalized.ToImmutable();
        }

        static string NormalizeAuthoredText(
            string value,
            string casing,
            ImmutableArray<string> ownerScripts,
            string subject,
            bool allowStemPlaceholder,
            bool allowNonLetters)
        {
            string normalized;
            try
            {
                normalized = value.IsNormalized(NormalizationForm.FormC)
                    ? value
                    : value.Normalize(NormalizationForm.FormC);
            }
            catch (ArgumentException)
            {
                throw new InvalidOperationException(
                    $"Inflection {subject} contains invalid Unicode.");
            }

            if (casing == "lower-title-upper")
            {
                var lower = normalized.ToLowerInvariant();
                if (lower.Length != normalized.Length ||
                    normalized.ToUpperInvariant().Length != normalized.Length)
                {
                    throw new InvalidOperationException(
                        $"Inflection {subject} has an unsupported casing expansion.");
                }

                normalized = lower;
            }

            var literal = allowStemPlaceholder
                ? normalized.Replace("{stem}", string.Empty)
                : normalized;
            if (literal.Length > 0 &&
                !HasOnlyDeclaredScripts(literal, ownerScripts, allowNonLetters))
            {
                throw new InvalidOperationException(
                    $"Inflection {subject} contains text outside its declared scripts.");
            }

            return normalized;
        }

        static bool HasOnlyDeclaredScripts(
            string value,
            ImmutableArray<string> ownerScripts,
            bool allowNonLetters)
        {
            var allowedScripts = GeneratorUnicodeScripts.None;
            foreach (var script in ownerScripts)
            {
                allowedScripts |= GetScript(script);
            }

            var detectedScripts = allowedScripts;
            var hasLetter = false;
            for (var index = 0; index < value.Length; index++)
            {
                int scalar;
                try
                {
                    scalar = char.ConvertToUtf32(value, index);
                }
                catch (ArgumentException)
                {
                    return false;
                }

                var category = CharUnicodeInfo.GetUnicodeCategory(value, index);
                if (char.IsHighSurrogate(value[index]))
                {
                    index++;
                }

                var isLetter = category is UnicodeCategory.UppercaseLetter or
                    UnicodeCategory.LowercaseLetter or
                    UnicodeCategory.TitlecaseLetter or
                    UnicodeCategory.ModifierLetter or
                    UnicodeCategory.OtherLetter;
                var isMark = category is UnicodeCategory.NonSpacingMark or
                    UnicodeCategory.SpacingCombiningMark or
                    UnicodeCategory.EnclosingMark;
                if (!isLetter && !isMark)
                {
                    if (allowNonLetters)
                    {
                        continue;
                    }

                    return false;
                }

                var scalarScripts = GetScripts(scalar);
                if ((scalarScripts & allowedScripts) == GeneratorUnicodeScripts.None)
                {
                    return false;
                }

                detectedScripts &= scalarScripts;
                if (detectedScripts == GeneratorUnicodeScripts.None)
                {
                    return false;
                }

                hasLetter |= isLetter;
            }

            return hasLetter;
        }

        static GeneratorUnicodeScripts GetScript(string script) =>
            script switch
            {
                "Arab" => GeneratorUnicodeScripts.Arab,
                "Armn" => GeneratorUnicodeScripts.Armn,
                "Beng" => GeneratorUnicodeScripts.Beng,
                "Cyrl" => GeneratorUnicodeScripts.Cyrl,
                "Deva" => GeneratorUnicodeScripts.Deva,
                "Ethi" => GeneratorUnicodeScripts.Ethi,
                "Geor" => GeneratorUnicodeScripts.Geor,
                "Grek" => GeneratorUnicodeScripts.Grek,
                "Gujr" => GeneratorUnicodeScripts.Gujr,
                "Guru" => GeneratorUnicodeScripts.Guru,
                "Hani" => GeneratorUnicodeScripts.Hani,
                "Hebr" => GeneratorUnicodeScripts.Hebr,
                "Jpan" => GeneratorUnicodeScripts.Jpan,
                "Khmr" => GeneratorUnicodeScripts.Khmr,
                "Knda" => GeneratorUnicodeScripts.Knda,
                "Kore" => GeneratorUnicodeScripts.Kore,
                "Laoo" => GeneratorUnicodeScripts.Laoo,
                "Latn" => GeneratorUnicodeScripts.Latn,
                "Mlym" => GeneratorUnicodeScripts.Mlym,
                "Mong" => GeneratorUnicodeScripts.Mong,
                "Mymr" => GeneratorUnicodeScripts.Mymr,
                "Orya" => GeneratorUnicodeScripts.Orya,
                "Sinh" => GeneratorUnicodeScripts.Sinh,
                "Taml" => GeneratorUnicodeScripts.Taml,
                "Telu" => GeneratorUnicodeScripts.Telu,
                "Thai" => GeneratorUnicodeScripts.Thai,
                _ => GeneratorUnicodeScripts.None
            };

        static GeneratorUnicodeScripts GetScripts(int scalar)
        {
            var low = 0;
            var high = UnicodeScriptRanges.Length - 1;
            while (low <= high)
            {
                var middle = low + ((high - low) / 2);
                var range = UnicodeScriptRanges[middle];
                if (scalar < range.First)
                {
                    high = middle - 1;
                }
                else if (scalar > range.Last)
                {
                    low = middle + 1;
                }
                else
                {
                    return range.Scripts;
                }
            }

            return GeneratorUnicodeScripts.None;
        }

        // Checked generated table from Unicode 16.0.0 Scripts.txt
        // (SHA-256 9e88f0a677df47311106340be8ede2ecdacd9c1c931831218d2be6d5508e0039)
        // and ScriptExtensions.txt
        // (SHA-256 049117ce26b9769fe2749b06eef51a50a89faef4a97764dd2d81daa715980700).
        // Common scalars without an exact supported Script_Extensions value are omitted;
        // unlisted Inherited scalars inherit the surrounding declared script.
        static readonly GeneratorUnicodeScriptRange[] UnicodeScriptRanges =
        [
            new(0x0041, 0x005A, GeneratorUnicodeScripts.Latn),
            new(0x0061, 0x007A, GeneratorUnicodeScripts.Latn),
            new(0x00AA, 0x00AA, GeneratorUnicodeScripts.Latn),
            new(0x00B7, 0x00B7, GeneratorUnicodeScripts.Geor | GeneratorUnicodeScripts.Grek | GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore | GeneratorUnicodeScripts.Latn),
            new(0x00BA, 0x00BA, GeneratorUnicodeScripts.Latn),
            new(0x00C0, 0x00D6, GeneratorUnicodeScripts.Latn),
            new(0x00D8, 0x00F6, GeneratorUnicodeScripts.Latn),
            new(0x00F8, 0x02B8, GeneratorUnicodeScripts.Latn),
            new(0x02BC, 0x02BC, GeneratorUnicodeScripts.Beng | GeneratorUnicodeScripts.Cyrl | GeneratorUnicodeScripts.Deva | GeneratorUnicodeScripts.Latn | GeneratorUnicodeScripts.Thai),
            new(0x02C7, 0x02C7, GeneratorUnicodeScripts.Latn),
            new(0x02C9, 0x02CB, GeneratorUnicodeScripts.Latn),
            new(0x02CD, 0x02CD, GeneratorUnicodeScripts.Latn),
            new(0x02D7, 0x02D7, GeneratorUnicodeScripts.Latn | GeneratorUnicodeScripts.Thai),
            new(0x02D9, 0x02D9, GeneratorUnicodeScripts.Latn),
            new(0x02E0, 0x02E4, GeneratorUnicodeScripts.Latn),
            new(0x0300, 0x0301, GeneratorUnicodeScripts.Cyrl | GeneratorUnicodeScripts.Grek | GeneratorUnicodeScripts.Latn),
            new(0x0302, 0x0302, GeneratorUnicodeScripts.Cyrl | GeneratorUnicodeScripts.Latn),
            new(0x0303, 0x0303, GeneratorUnicodeScripts.Latn | GeneratorUnicodeScripts.Thai),
            new(0x0304, 0x0304, GeneratorUnicodeScripts.Cyrl | GeneratorUnicodeScripts.Grek | GeneratorUnicodeScripts.Latn),
            new(0x0305, 0x0305, GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Latn),
            new(0x0306, 0x0306, GeneratorUnicodeScripts.Cyrl | GeneratorUnicodeScripts.Grek | GeneratorUnicodeScripts.Latn),
            new(0x0307, 0x0307, GeneratorUnicodeScripts.Hebr | GeneratorUnicodeScripts.Latn),
            new(0x0308, 0x0308, GeneratorUnicodeScripts.Armn | GeneratorUnicodeScripts.Cyrl | GeneratorUnicodeScripts.Grek | GeneratorUnicodeScripts.Hebr | GeneratorUnicodeScripts.Latn),
            new(0x0309, 0x030A, GeneratorUnicodeScripts.Latn),
            new(0x030B, 0x030B, GeneratorUnicodeScripts.Cyrl | GeneratorUnicodeScripts.Latn),
            new(0x030C, 0x030D, GeneratorUnicodeScripts.Latn),
            new(0x030E, 0x030E, GeneratorUnicodeScripts.Ethi | GeneratorUnicodeScripts.Latn),
            new(0x030F, 0x030F, GeneratorUnicodeScripts.All),
            new(0x0310, 0x0310, GeneratorUnicodeScripts.Latn),
            new(0x0311, 0x0311, GeneratorUnicodeScripts.Cyrl | GeneratorUnicodeScripts.Latn),
            new(0x0312, 0x0312, GeneratorUnicodeScripts.All),
            new(0x0313, 0x0313, GeneratorUnicodeScripts.Grek | GeneratorUnicodeScripts.Latn),
            new(0x0314, 0x031F, GeneratorUnicodeScripts.All),
            new(0x0320, 0x0320, GeneratorUnicodeScripts.Latn),
            new(0x0321, 0x0322, GeneratorUnicodeScripts.All),
            new(0x0323, 0x0323, GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Latn),
            new(0x0324, 0x0325, GeneratorUnicodeScripts.Latn),
            new(0x0326, 0x032C, GeneratorUnicodeScripts.All),
            new(0x032D, 0x032E, GeneratorUnicodeScripts.Latn),
            new(0x032F, 0x032F, GeneratorUnicodeScripts.All),
            new(0x0330, 0x0330, GeneratorUnicodeScripts.Latn),
            new(0x0331, 0x0331, GeneratorUnicodeScripts.Latn | GeneratorUnicodeScripts.Thai),
            new(0x0332, 0x0341, GeneratorUnicodeScripts.All),
            new(0x0342, 0x0342, GeneratorUnicodeScripts.Grek),
            new(0x0343, 0x0344, GeneratorUnicodeScripts.All),
            new(0x0345, 0x0345, GeneratorUnicodeScripts.Grek),
            new(0x0346, 0x0357, GeneratorUnicodeScripts.All),
            new(0x0358, 0x0358, GeneratorUnicodeScripts.Latn),
            new(0x0359, 0x035D, GeneratorUnicodeScripts.All),
            new(0x035E, 0x035E, GeneratorUnicodeScripts.Latn),
            new(0x035F, 0x0362, GeneratorUnicodeScripts.All),
            new(0x0363, 0x036F, GeneratorUnicodeScripts.Latn),
            new(0x0370, 0x0377, GeneratorUnicodeScripts.Grek),
            new(0x037A, 0x037D, GeneratorUnicodeScripts.Grek),
            new(0x037F, 0x037F, GeneratorUnicodeScripts.Grek),
            new(0x0384, 0x0384, GeneratorUnicodeScripts.Grek),
            new(0x0386, 0x0386, GeneratorUnicodeScripts.Grek),
            new(0x0388, 0x038A, GeneratorUnicodeScripts.Grek),
            new(0x038C, 0x038C, GeneratorUnicodeScripts.Grek),
            new(0x038E, 0x03A1, GeneratorUnicodeScripts.Grek),
            new(0x03A3, 0x03E1, GeneratorUnicodeScripts.Grek),
            new(0x03F0, 0x03FF, GeneratorUnicodeScripts.Grek),
            new(0x0400, 0x0484, GeneratorUnicodeScripts.Cyrl),
            new(0x0485, 0x0486, GeneratorUnicodeScripts.Cyrl | GeneratorUnicodeScripts.Latn),
            new(0x0487, 0x052F, GeneratorUnicodeScripts.Cyrl),
            new(0x0531, 0x0556, GeneratorUnicodeScripts.Armn),
            new(0x0559, 0x0588, GeneratorUnicodeScripts.Armn),
            new(0x0589, 0x0589, GeneratorUnicodeScripts.Armn | GeneratorUnicodeScripts.Geor),
            new(0x058A, 0x058A, GeneratorUnicodeScripts.Armn),
            new(0x058D, 0x058F, GeneratorUnicodeScripts.Armn),
            new(0x0591, 0x05C7, GeneratorUnicodeScripts.Hebr),
            new(0x05D0, 0x05EA, GeneratorUnicodeScripts.Hebr),
            new(0x05EF, 0x05F4, GeneratorUnicodeScripts.Hebr),
            new(0x0600, 0x0604, GeneratorUnicodeScripts.Arab),
            new(0x0606, 0x06DC, GeneratorUnicodeScripts.Arab),
            new(0x06DE, 0x06FF, GeneratorUnicodeScripts.Arab),
            new(0x0750, 0x077F, GeneratorUnicodeScripts.Arab),
            new(0x0870, 0x088E, GeneratorUnicodeScripts.Arab),
            new(0x0890, 0x0891, GeneratorUnicodeScripts.Arab),
            new(0x0897, 0x08E1, GeneratorUnicodeScripts.Arab),
            new(0x08E3, 0x08FF, GeneratorUnicodeScripts.Arab),
            new(0x0900, 0x0950, GeneratorUnicodeScripts.Deva),
            new(0x0951, 0x0952, GeneratorUnicodeScripts.Beng | GeneratorUnicodeScripts.Deva | GeneratorUnicodeScripts.Gujr | GeneratorUnicodeScripts.Guru | GeneratorUnicodeScripts.Knda | GeneratorUnicodeScripts.Latn | GeneratorUnicodeScripts.Mlym | GeneratorUnicodeScripts.Orya | GeneratorUnicodeScripts.Taml | GeneratorUnicodeScripts.Telu),
            new(0x0953, 0x0954, GeneratorUnicodeScripts.All),
            new(0x0955, 0x0963, GeneratorUnicodeScripts.Deva),
            new(0x0964, 0x0965, GeneratorUnicodeScripts.Beng | GeneratorUnicodeScripts.Deva | GeneratorUnicodeScripts.Gujr | GeneratorUnicodeScripts.Guru | GeneratorUnicodeScripts.Knda | GeneratorUnicodeScripts.Mlym | GeneratorUnicodeScripts.Orya | GeneratorUnicodeScripts.Sinh | GeneratorUnicodeScripts.Taml | GeneratorUnicodeScripts.Telu),
            new(0x0966, 0x097F, GeneratorUnicodeScripts.Deva),
            new(0x0980, 0x0983, GeneratorUnicodeScripts.Beng),
            new(0x0985, 0x098C, GeneratorUnicodeScripts.Beng),
            new(0x098F, 0x0990, GeneratorUnicodeScripts.Beng),
            new(0x0993, 0x09A8, GeneratorUnicodeScripts.Beng),
            new(0x09AA, 0x09B0, GeneratorUnicodeScripts.Beng),
            new(0x09B2, 0x09B2, GeneratorUnicodeScripts.Beng),
            new(0x09B6, 0x09B9, GeneratorUnicodeScripts.Beng),
            new(0x09BC, 0x09C4, GeneratorUnicodeScripts.Beng),
            new(0x09C7, 0x09C8, GeneratorUnicodeScripts.Beng),
            new(0x09CB, 0x09CE, GeneratorUnicodeScripts.Beng),
            new(0x09D7, 0x09D7, GeneratorUnicodeScripts.Beng),
            new(0x09DC, 0x09DD, GeneratorUnicodeScripts.Beng),
            new(0x09DF, 0x09E3, GeneratorUnicodeScripts.Beng),
            new(0x09E6, 0x09FE, GeneratorUnicodeScripts.Beng),
            new(0x0A01, 0x0A03, GeneratorUnicodeScripts.Guru),
            new(0x0A05, 0x0A0A, GeneratorUnicodeScripts.Guru),
            new(0x0A0F, 0x0A10, GeneratorUnicodeScripts.Guru),
            new(0x0A13, 0x0A28, GeneratorUnicodeScripts.Guru),
            new(0x0A2A, 0x0A30, GeneratorUnicodeScripts.Guru),
            new(0x0A32, 0x0A33, GeneratorUnicodeScripts.Guru),
            new(0x0A35, 0x0A36, GeneratorUnicodeScripts.Guru),
            new(0x0A38, 0x0A39, GeneratorUnicodeScripts.Guru),
            new(0x0A3C, 0x0A3C, GeneratorUnicodeScripts.Guru),
            new(0x0A3E, 0x0A42, GeneratorUnicodeScripts.Guru),
            new(0x0A47, 0x0A48, GeneratorUnicodeScripts.Guru),
            new(0x0A4B, 0x0A4D, GeneratorUnicodeScripts.Guru),
            new(0x0A51, 0x0A51, GeneratorUnicodeScripts.Guru),
            new(0x0A59, 0x0A5C, GeneratorUnicodeScripts.Guru),
            new(0x0A5E, 0x0A5E, GeneratorUnicodeScripts.Guru),
            new(0x0A66, 0x0A76, GeneratorUnicodeScripts.Guru),
            new(0x0A81, 0x0A83, GeneratorUnicodeScripts.Gujr),
            new(0x0A85, 0x0A8D, GeneratorUnicodeScripts.Gujr),
            new(0x0A8F, 0x0A91, GeneratorUnicodeScripts.Gujr),
            new(0x0A93, 0x0AA8, GeneratorUnicodeScripts.Gujr),
            new(0x0AAA, 0x0AB0, GeneratorUnicodeScripts.Gujr),
            new(0x0AB2, 0x0AB3, GeneratorUnicodeScripts.Gujr),
            new(0x0AB5, 0x0AB9, GeneratorUnicodeScripts.Gujr),
            new(0x0ABC, 0x0AC5, GeneratorUnicodeScripts.Gujr),
            new(0x0AC7, 0x0AC9, GeneratorUnicodeScripts.Gujr),
            new(0x0ACB, 0x0ACD, GeneratorUnicodeScripts.Gujr),
            new(0x0AD0, 0x0AD0, GeneratorUnicodeScripts.Gujr),
            new(0x0AE0, 0x0AE3, GeneratorUnicodeScripts.Gujr),
            new(0x0AE6, 0x0AF1, GeneratorUnicodeScripts.Gujr),
            new(0x0AF9, 0x0AFF, GeneratorUnicodeScripts.Gujr),
            new(0x0B01, 0x0B03, GeneratorUnicodeScripts.Orya),
            new(0x0B05, 0x0B0C, GeneratorUnicodeScripts.Orya),
            new(0x0B0F, 0x0B10, GeneratorUnicodeScripts.Orya),
            new(0x0B13, 0x0B28, GeneratorUnicodeScripts.Orya),
            new(0x0B2A, 0x0B30, GeneratorUnicodeScripts.Orya),
            new(0x0B32, 0x0B33, GeneratorUnicodeScripts.Orya),
            new(0x0B35, 0x0B39, GeneratorUnicodeScripts.Orya),
            new(0x0B3C, 0x0B44, GeneratorUnicodeScripts.Orya),
            new(0x0B47, 0x0B48, GeneratorUnicodeScripts.Orya),
            new(0x0B4B, 0x0B4D, GeneratorUnicodeScripts.Orya),
            new(0x0B55, 0x0B57, GeneratorUnicodeScripts.Orya),
            new(0x0B5C, 0x0B5D, GeneratorUnicodeScripts.Orya),
            new(0x0B5F, 0x0B63, GeneratorUnicodeScripts.Orya),
            new(0x0B66, 0x0B77, GeneratorUnicodeScripts.Orya),
            new(0x0B82, 0x0B83, GeneratorUnicodeScripts.Taml),
            new(0x0B85, 0x0B8A, GeneratorUnicodeScripts.Taml),
            new(0x0B8E, 0x0B90, GeneratorUnicodeScripts.Taml),
            new(0x0B92, 0x0B95, GeneratorUnicodeScripts.Taml),
            new(0x0B99, 0x0B9A, GeneratorUnicodeScripts.Taml),
            new(0x0B9C, 0x0B9C, GeneratorUnicodeScripts.Taml),
            new(0x0B9E, 0x0B9F, GeneratorUnicodeScripts.Taml),
            new(0x0BA3, 0x0BA4, GeneratorUnicodeScripts.Taml),
            new(0x0BA8, 0x0BAA, GeneratorUnicodeScripts.Taml),
            new(0x0BAE, 0x0BB9, GeneratorUnicodeScripts.Taml),
            new(0x0BBE, 0x0BC2, GeneratorUnicodeScripts.Taml),
            new(0x0BC6, 0x0BC8, GeneratorUnicodeScripts.Taml),
            new(0x0BCA, 0x0BCD, GeneratorUnicodeScripts.Taml),
            new(0x0BD0, 0x0BD0, GeneratorUnicodeScripts.Taml),
            new(0x0BD7, 0x0BD7, GeneratorUnicodeScripts.Taml),
            new(0x0BE6, 0x0BFA, GeneratorUnicodeScripts.Taml),
            new(0x0C00, 0x0C0C, GeneratorUnicodeScripts.Telu),
            new(0x0C0E, 0x0C10, GeneratorUnicodeScripts.Telu),
            new(0x0C12, 0x0C28, GeneratorUnicodeScripts.Telu),
            new(0x0C2A, 0x0C39, GeneratorUnicodeScripts.Telu),
            new(0x0C3C, 0x0C44, GeneratorUnicodeScripts.Telu),
            new(0x0C46, 0x0C48, GeneratorUnicodeScripts.Telu),
            new(0x0C4A, 0x0C4D, GeneratorUnicodeScripts.Telu),
            new(0x0C55, 0x0C56, GeneratorUnicodeScripts.Telu),
            new(0x0C58, 0x0C5A, GeneratorUnicodeScripts.Telu),
            new(0x0C5D, 0x0C5D, GeneratorUnicodeScripts.Telu),
            new(0x0C60, 0x0C63, GeneratorUnicodeScripts.Telu),
            new(0x0C66, 0x0C6F, GeneratorUnicodeScripts.Telu),
            new(0x0C77, 0x0C7F, GeneratorUnicodeScripts.Telu),
            new(0x0C80, 0x0C8C, GeneratorUnicodeScripts.Knda),
            new(0x0C8E, 0x0C90, GeneratorUnicodeScripts.Knda),
            new(0x0C92, 0x0CA8, GeneratorUnicodeScripts.Knda),
            new(0x0CAA, 0x0CB3, GeneratorUnicodeScripts.Knda),
            new(0x0CB5, 0x0CB9, GeneratorUnicodeScripts.Knda),
            new(0x0CBC, 0x0CC4, GeneratorUnicodeScripts.Knda),
            new(0x0CC6, 0x0CC8, GeneratorUnicodeScripts.Knda),
            new(0x0CCA, 0x0CCD, GeneratorUnicodeScripts.Knda),
            new(0x0CD5, 0x0CD6, GeneratorUnicodeScripts.Knda),
            new(0x0CDD, 0x0CDE, GeneratorUnicodeScripts.Knda),
            new(0x0CE0, 0x0CE3, GeneratorUnicodeScripts.Knda),
            new(0x0CE6, 0x0CEF, GeneratorUnicodeScripts.Knda),
            new(0x0CF1, 0x0CF3, GeneratorUnicodeScripts.Knda),
            new(0x0D00, 0x0D0C, GeneratorUnicodeScripts.Mlym),
            new(0x0D0E, 0x0D10, GeneratorUnicodeScripts.Mlym),
            new(0x0D12, 0x0D44, GeneratorUnicodeScripts.Mlym),
            new(0x0D46, 0x0D48, GeneratorUnicodeScripts.Mlym),
            new(0x0D4A, 0x0D4F, GeneratorUnicodeScripts.Mlym),
            new(0x0D54, 0x0D63, GeneratorUnicodeScripts.Mlym),
            new(0x0D66, 0x0D7F, GeneratorUnicodeScripts.Mlym),
            new(0x0D81, 0x0D83, GeneratorUnicodeScripts.Sinh),
            new(0x0D85, 0x0D96, GeneratorUnicodeScripts.Sinh),
            new(0x0D9A, 0x0DB1, GeneratorUnicodeScripts.Sinh),
            new(0x0DB3, 0x0DBB, GeneratorUnicodeScripts.Sinh),
            new(0x0DBD, 0x0DBD, GeneratorUnicodeScripts.Sinh),
            new(0x0DC0, 0x0DC6, GeneratorUnicodeScripts.Sinh),
            new(0x0DCA, 0x0DCA, GeneratorUnicodeScripts.Sinh),
            new(0x0DCF, 0x0DD4, GeneratorUnicodeScripts.Sinh),
            new(0x0DD6, 0x0DD6, GeneratorUnicodeScripts.Sinh),
            new(0x0DD8, 0x0DDF, GeneratorUnicodeScripts.Sinh),
            new(0x0DE6, 0x0DEF, GeneratorUnicodeScripts.Sinh),
            new(0x0DF2, 0x0DF4, GeneratorUnicodeScripts.Sinh),
            new(0x0E01, 0x0E3A, GeneratorUnicodeScripts.Thai),
            new(0x0E40, 0x0E5B, GeneratorUnicodeScripts.Thai),
            new(0x0E81, 0x0E82, GeneratorUnicodeScripts.Laoo),
            new(0x0E84, 0x0E84, GeneratorUnicodeScripts.Laoo),
            new(0x0E86, 0x0E8A, GeneratorUnicodeScripts.Laoo),
            new(0x0E8C, 0x0EA3, GeneratorUnicodeScripts.Laoo),
            new(0x0EA5, 0x0EA5, GeneratorUnicodeScripts.Laoo),
            new(0x0EA7, 0x0EBD, GeneratorUnicodeScripts.Laoo),
            new(0x0EC0, 0x0EC4, GeneratorUnicodeScripts.Laoo),
            new(0x0EC6, 0x0EC6, GeneratorUnicodeScripts.Laoo),
            new(0x0EC8, 0x0ECE, GeneratorUnicodeScripts.Laoo),
            new(0x0ED0, 0x0ED9, GeneratorUnicodeScripts.Laoo),
            new(0x0EDC, 0x0EDF, GeneratorUnicodeScripts.Laoo),
            new(0x1000, 0x109F, GeneratorUnicodeScripts.Mymr),
            new(0x10A0, 0x10C5, GeneratorUnicodeScripts.Geor),
            new(0x10C7, 0x10C7, GeneratorUnicodeScripts.Geor),
            new(0x10CD, 0x10CD, GeneratorUnicodeScripts.Geor),
            new(0x10D0, 0x10FA, GeneratorUnicodeScripts.Geor),
            new(0x10FB, 0x10FB, GeneratorUnicodeScripts.Geor | GeneratorUnicodeScripts.Latn),
            new(0x10FC, 0x10FF, GeneratorUnicodeScripts.Geor),
            new(0x1100, 0x11FF, GeneratorUnicodeScripts.Kore),
            new(0x1200, 0x1248, GeneratorUnicodeScripts.Ethi),
            new(0x124A, 0x124D, GeneratorUnicodeScripts.Ethi),
            new(0x1250, 0x1256, GeneratorUnicodeScripts.Ethi),
            new(0x1258, 0x1258, GeneratorUnicodeScripts.Ethi),
            new(0x125A, 0x125D, GeneratorUnicodeScripts.Ethi),
            new(0x1260, 0x1288, GeneratorUnicodeScripts.Ethi),
            new(0x128A, 0x128D, GeneratorUnicodeScripts.Ethi),
            new(0x1290, 0x12B0, GeneratorUnicodeScripts.Ethi),
            new(0x12B2, 0x12B5, GeneratorUnicodeScripts.Ethi),
            new(0x12B8, 0x12BE, GeneratorUnicodeScripts.Ethi),
            new(0x12C0, 0x12C0, GeneratorUnicodeScripts.Ethi),
            new(0x12C2, 0x12C5, GeneratorUnicodeScripts.Ethi),
            new(0x12C8, 0x12D6, GeneratorUnicodeScripts.Ethi),
            new(0x12D8, 0x1310, GeneratorUnicodeScripts.Ethi),
            new(0x1312, 0x1315, GeneratorUnicodeScripts.Ethi),
            new(0x1318, 0x135A, GeneratorUnicodeScripts.Ethi),
            new(0x135D, 0x137C, GeneratorUnicodeScripts.Ethi),
            new(0x1380, 0x1399, GeneratorUnicodeScripts.Ethi),
            new(0x1780, 0x17DD, GeneratorUnicodeScripts.Khmr),
            new(0x17E0, 0x17E9, GeneratorUnicodeScripts.Khmr),
            new(0x17F0, 0x17F9, GeneratorUnicodeScripts.Khmr),
            new(0x1800, 0x1819, GeneratorUnicodeScripts.Mong),
            new(0x1820, 0x1878, GeneratorUnicodeScripts.Mong),
            new(0x1880, 0x18AA, GeneratorUnicodeScripts.Mong),
            new(0x19E0, 0x19FF, GeneratorUnicodeScripts.Khmr),
            new(0x1AB0, 0x1ACE, GeneratorUnicodeScripts.All),
            new(0x1C80, 0x1C8A, GeneratorUnicodeScripts.Cyrl),
            new(0x1C90, 0x1CBA, GeneratorUnicodeScripts.Geor),
            new(0x1CBD, 0x1CBF, GeneratorUnicodeScripts.Geor),
            new(0x1CD0, 0x1CD0, GeneratorUnicodeScripts.Beng | GeneratorUnicodeScripts.Deva | GeneratorUnicodeScripts.Knda),
            new(0x1CD1, 0x1CD1, GeneratorUnicodeScripts.Deva),
            new(0x1CD2, 0x1CD2, GeneratorUnicodeScripts.Beng | GeneratorUnicodeScripts.Deva | GeneratorUnicodeScripts.Knda),
            new(0x1CD3, 0x1CD3, GeneratorUnicodeScripts.Deva | GeneratorUnicodeScripts.Knda),
            new(0x1CD4, 0x1CD4, GeneratorUnicodeScripts.Deva),
            new(0x1CD5, 0x1CD6, GeneratorUnicodeScripts.Beng | GeneratorUnicodeScripts.Deva),
            new(0x1CD7, 0x1CD7, GeneratorUnicodeScripts.Deva),
            new(0x1CD8, 0x1CD8, GeneratorUnicodeScripts.Beng | GeneratorUnicodeScripts.Deva),
            new(0x1CD9, 0x1CD9, GeneratorUnicodeScripts.Deva),
            new(0x1CDA, 0x1CDA, GeneratorUnicodeScripts.Deva | GeneratorUnicodeScripts.Knda | GeneratorUnicodeScripts.Mlym | GeneratorUnicodeScripts.Orya | GeneratorUnicodeScripts.Taml | GeneratorUnicodeScripts.Telu),
            new(0x1CDB, 0x1CE0, GeneratorUnicodeScripts.Deva),
            new(0x1CE1, 0x1CE1, GeneratorUnicodeScripts.Beng | GeneratorUnicodeScripts.Deva),
            new(0x1CE2, 0x1CE9, GeneratorUnicodeScripts.Deva),
            new(0x1CEA, 0x1CEA, GeneratorUnicodeScripts.Beng | GeneratorUnicodeScripts.Deva),
            new(0x1CEB, 0x1CEC, GeneratorUnicodeScripts.Deva),
            new(0x1CED, 0x1CED, GeneratorUnicodeScripts.Beng | GeneratorUnicodeScripts.Deva),
            new(0x1CEE, 0x1CF1, GeneratorUnicodeScripts.Deva),
            new(0x1CF2, 0x1CF2, GeneratorUnicodeScripts.Beng | GeneratorUnicodeScripts.Deva | GeneratorUnicodeScripts.Knda | GeneratorUnicodeScripts.Mlym | GeneratorUnicodeScripts.Orya | GeneratorUnicodeScripts.Sinh | GeneratorUnicodeScripts.Telu),
            new(0x1CF3, 0x1CF3, GeneratorUnicodeScripts.Deva),
            new(0x1CF4, 0x1CF4, GeneratorUnicodeScripts.Deva | GeneratorUnicodeScripts.Knda),
            new(0x1CF5, 0x1CF6, GeneratorUnicodeScripts.Beng | GeneratorUnicodeScripts.Deva),
            new(0x1CF7, 0x1CF7, GeneratorUnicodeScripts.Beng),
            new(0x1CF8, 0x1CF9, GeneratorUnicodeScripts.Deva),
            new(0x1D00, 0x1D25, GeneratorUnicodeScripts.Latn),
            new(0x1D26, 0x1D2A, GeneratorUnicodeScripts.Grek),
            new(0x1D2B, 0x1D2B, GeneratorUnicodeScripts.Cyrl),
            new(0x1D2C, 0x1D5C, GeneratorUnicodeScripts.Latn),
            new(0x1D5D, 0x1D61, GeneratorUnicodeScripts.Grek),
            new(0x1D62, 0x1D65, GeneratorUnicodeScripts.Latn),
            new(0x1D66, 0x1D6A, GeneratorUnicodeScripts.Grek),
            new(0x1D6B, 0x1D77, GeneratorUnicodeScripts.Latn),
            new(0x1D78, 0x1D78, GeneratorUnicodeScripts.Cyrl),
            new(0x1D79, 0x1DBE, GeneratorUnicodeScripts.Latn),
            new(0x1DBF, 0x1DC1, GeneratorUnicodeScripts.Grek),
            new(0x1DC2, 0x1DF7, GeneratorUnicodeScripts.All),
            new(0x1DF8, 0x1DF8, GeneratorUnicodeScripts.Cyrl | GeneratorUnicodeScripts.Latn),
            new(0x1DF9, 0x1DF9, GeneratorUnicodeScripts.All),
            new(0x1DFB, 0x1DFF, GeneratorUnicodeScripts.All),
            new(0x1E00, 0x1EFF, GeneratorUnicodeScripts.Latn),
            new(0x1F00, 0x1F15, GeneratorUnicodeScripts.Grek),
            new(0x1F18, 0x1F1D, GeneratorUnicodeScripts.Grek),
            new(0x1F20, 0x1F45, GeneratorUnicodeScripts.Grek),
            new(0x1F48, 0x1F4D, GeneratorUnicodeScripts.Grek),
            new(0x1F50, 0x1F57, GeneratorUnicodeScripts.Grek),
            new(0x1F59, 0x1F59, GeneratorUnicodeScripts.Grek),
            new(0x1F5B, 0x1F5B, GeneratorUnicodeScripts.Grek),
            new(0x1F5D, 0x1F5D, GeneratorUnicodeScripts.Grek),
            new(0x1F5F, 0x1F7D, GeneratorUnicodeScripts.Grek),
            new(0x1F80, 0x1FB4, GeneratorUnicodeScripts.Grek),
            new(0x1FB6, 0x1FC4, GeneratorUnicodeScripts.Grek),
            new(0x1FC6, 0x1FD3, GeneratorUnicodeScripts.Grek),
            new(0x1FD6, 0x1FDB, GeneratorUnicodeScripts.Grek),
            new(0x1FDD, 0x1FEF, GeneratorUnicodeScripts.Grek),
            new(0x1FF2, 0x1FF4, GeneratorUnicodeScripts.Grek),
            new(0x1FF6, 0x1FFE, GeneratorUnicodeScripts.Grek),
            new(0x200C, 0x200D, GeneratorUnicodeScripts.All),
            new(0x202F, 0x202F, GeneratorUnicodeScripts.Latn | GeneratorUnicodeScripts.Mong),
            new(0x204F, 0x204F, GeneratorUnicodeScripts.Arab),
            new(0x205A, 0x205A, GeneratorUnicodeScripts.Geor),
            new(0x205D, 0x205D, GeneratorUnicodeScripts.Grek),
            new(0x2071, 0x2071, GeneratorUnicodeScripts.Latn),
            new(0x207F, 0x207F, GeneratorUnicodeScripts.Latn),
            new(0x2090, 0x209C, GeneratorUnicodeScripts.Latn),
            new(0x20D0, 0x20EF, GeneratorUnicodeScripts.All),
            new(0x20F0, 0x20F0, GeneratorUnicodeScripts.Deva | GeneratorUnicodeScripts.Latn),
            new(0x2126, 0x2126, GeneratorUnicodeScripts.Grek),
            new(0x212A, 0x212B, GeneratorUnicodeScripts.Latn),
            new(0x2132, 0x2132, GeneratorUnicodeScripts.Latn),
            new(0x214E, 0x214E, GeneratorUnicodeScripts.Latn),
            new(0x2160, 0x2188, GeneratorUnicodeScripts.Latn),
            new(0x2C60, 0x2C7F, GeneratorUnicodeScripts.Latn),
            new(0x2D00, 0x2D25, GeneratorUnicodeScripts.Geor),
            new(0x2D27, 0x2D27, GeneratorUnicodeScripts.Geor),
            new(0x2D2D, 0x2D2D, GeneratorUnicodeScripts.Geor),
            new(0x2D80, 0x2D96, GeneratorUnicodeScripts.Ethi),
            new(0x2DA0, 0x2DA6, GeneratorUnicodeScripts.Ethi),
            new(0x2DA8, 0x2DAE, GeneratorUnicodeScripts.Ethi),
            new(0x2DB0, 0x2DB6, GeneratorUnicodeScripts.Ethi),
            new(0x2DB8, 0x2DBE, GeneratorUnicodeScripts.Ethi),
            new(0x2DC0, 0x2DC6, GeneratorUnicodeScripts.Ethi),
            new(0x2DC8, 0x2DCE, GeneratorUnicodeScripts.Ethi),
            new(0x2DD0, 0x2DD6, GeneratorUnicodeScripts.Ethi),
            new(0x2DD8, 0x2DDE, GeneratorUnicodeScripts.Ethi),
            new(0x2DE0, 0x2DFF, GeneratorUnicodeScripts.Cyrl),
            new(0x2E17, 0x2E17, GeneratorUnicodeScripts.Latn),
            new(0x2E31, 0x2E31, GeneratorUnicodeScripts.Geor),
            new(0x2E41, 0x2E41, GeneratorUnicodeScripts.Arab),
            new(0x2E43, 0x2E43, GeneratorUnicodeScripts.Cyrl),
            new(0x2E80, 0x2E99, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore),
            new(0x2E9B, 0x2EF3, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore),
            new(0x2F00, 0x2FD5, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore),
            new(0x2FF0, 0x2FFF, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore),
            new(0x3001, 0x3002, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore | GeneratorUnicodeScripts.Mong),
            new(0x3003, 0x3003, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore),
            new(0x3005, 0x3007, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore),
            new(0x3008, 0x300B, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore | GeneratorUnicodeScripts.Mong),
            new(0x300C, 0x3011, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore),
            new(0x3013, 0x301F, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore),
            new(0x3021, 0x302D, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore),
            new(0x302E, 0x302F, GeneratorUnicodeScripts.Kore),
            new(0x3030, 0x3030, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore),
            new(0x3031, 0x3035, GeneratorUnicodeScripts.Jpan),
            new(0x3037, 0x303F, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore),
            new(0x3041, 0x3096, GeneratorUnicodeScripts.Jpan),
            new(0x3099, 0x30FA, GeneratorUnicodeScripts.Jpan),
            new(0x30FB, 0x30FB, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore),
            new(0x30FC, 0x30FF, GeneratorUnicodeScripts.Jpan),
            new(0x3131, 0x318E, GeneratorUnicodeScripts.Kore),
            new(0x3190, 0x319F, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore),
            new(0x31C0, 0x31E5, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore),
            new(0x31EF, 0x31EF, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore),
            new(0x31F0, 0x31FF, GeneratorUnicodeScripts.Jpan),
            new(0x3200, 0x321E, GeneratorUnicodeScripts.Kore),
            new(0x3220, 0x3247, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore),
            new(0x3260, 0x327E, GeneratorUnicodeScripts.Kore),
            new(0x3280, 0x32B0, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore),
            new(0x32C0, 0x32CB, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore),
            new(0x32D0, 0x32FE, GeneratorUnicodeScripts.Jpan),
            new(0x32FF, 0x32FF, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore),
            new(0x3300, 0x3357, GeneratorUnicodeScripts.Jpan),
            new(0x3358, 0x3370, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore),
            new(0x337B, 0x337F, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore),
            new(0x33E0, 0x33FE, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore),
            new(0x3400, 0x4DBF, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore),
            new(0x4E00, 0x9FFF, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore),
            new(0xA640, 0xA69F, GeneratorUnicodeScripts.Cyrl),
            new(0xA700, 0xA707, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore | GeneratorUnicodeScripts.Latn),
            new(0xA722, 0xA787, GeneratorUnicodeScripts.Latn),
            new(0xA78B, 0xA7CD, GeneratorUnicodeScripts.Latn),
            new(0xA7D0, 0xA7D1, GeneratorUnicodeScripts.Latn),
            new(0xA7D3, 0xA7D3, GeneratorUnicodeScripts.Latn),
            new(0xA7D5, 0xA7DC, GeneratorUnicodeScripts.Latn),
            new(0xA7F2, 0xA7FF, GeneratorUnicodeScripts.Latn),
            new(0xA830, 0xA832, GeneratorUnicodeScripts.Deva | GeneratorUnicodeScripts.Gujr | GeneratorUnicodeScripts.Guru | GeneratorUnicodeScripts.Knda | GeneratorUnicodeScripts.Mlym),
            new(0xA833, 0xA835, GeneratorUnicodeScripts.Deva | GeneratorUnicodeScripts.Gujr | GeneratorUnicodeScripts.Guru | GeneratorUnicodeScripts.Knda),
            new(0xA836, 0xA839, GeneratorUnicodeScripts.Deva | GeneratorUnicodeScripts.Gujr | GeneratorUnicodeScripts.Guru),
            new(0xA8E0, 0xA8F0, GeneratorUnicodeScripts.Deva),
            new(0xA8F1, 0xA8F1, GeneratorUnicodeScripts.Beng | GeneratorUnicodeScripts.Deva),
            new(0xA8F2, 0xA8F2, GeneratorUnicodeScripts.Deva),
            new(0xA8F3, 0xA8F3, GeneratorUnicodeScripts.Deva | GeneratorUnicodeScripts.Taml),
            new(0xA8F4, 0xA8FF, GeneratorUnicodeScripts.Deva),
            new(0xA92E, 0xA92E, GeneratorUnicodeScripts.Latn | GeneratorUnicodeScripts.Mymr),
            new(0xA960, 0xA97C, GeneratorUnicodeScripts.Kore),
            new(0xA9E0, 0xA9FE, GeneratorUnicodeScripts.Mymr),
            new(0xAA60, 0xAA7F, GeneratorUnicodeScripts.Mymr),
            new(0xAB01, 0xAB06, GeneratorUnicodeScripts.Ethi),
            new(0xAB09, 0xAB0E, GeneratorUnicodeScripts.Ethi),
            new(0xAB11, 0xAB16, GeneratorUnicodeScripts.Ethi),
            new(0xAB20, 0xAB26, GeneratorUnicodeScripts.Ethi),
            new(0xAB28, 0xAB2E, GeneratorUnicodeScripts.Ethi),
            new(0xAB30, 0xAB5A, GeneratorUnicodeScripts.Latn),
            new(0xAB5C, 0xAB64, GeneratorUnicodeScripts.Latn),
            new(0xAB65, 0xAB65, GeneratorUnicodeScripts.Grek),
            new(0xAB66, 0xAB69, GeneratorUnicodeScripts.Latn),
            new(0xAC00, 0xD7A3, GeneratorUnicodeScripts.Kore),
            new(0xD7B0, 0xD7C6, GeneratorUnicodeScripts.Kore),
            new(0xD7CB, 0xD7FB, GeneratorUnicodeScripts.Kore),
            new(0xF900, 0xFA6D, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore),
            new(0xFA70, 0xFAD9, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore),
            new(0xFB00, 0xFB06, GeneratorUnicodeScripts.Latn),
            new(0xFB13, 0xFB17, GeneratorUnicodeScripts.Armn),
            new(0xFB1D, 0xFB36, GeneratorUnicodeScripts.Hebr),
            new(0xFB38, 0xFB3C, GeneratorUnicodeScripts.Hebr),
            new(0xFB3E, 0xFB3E, GeneratorUnicodeScripts.Hebr),
            new(0xFB40, 0xFB41, GeneratorUnicodeScripts.Hebr),
            new(0xFB43, 0xFB44, GeneratorUnicodeScripts.Hebr),
            new(0xFB46, 0xFB4F, GeneratorUnicodeScripts.Hebr),
            new(0xFB50, 0xFBC2, GeneratorUnicodeScripts.Arab),
            new(0xFBD3, 0xFD8F, GeneratorUnicodeScripts.Arab),
            new(0xFD92, 0xFDC7, GeneratorUnicodeScripts.Arab),
            new(0xFDCF, 0xFDCF, GeneratorUnicodeScripts.Arab),
            new(0xFDF0, 0xFDFF, GeneratorUnicodeScripts.Arab),
            new(0xFE00, 0xFE0F, GeneratorUnicodeScripts.All),
            new(0xFE20, 0xFE2D, GeneratorUnicodeScripts.All),
            new(0xFE2E, 0xFE2F, GeneratorUnicodeScripts.Cyrl),
            new(0xFE45, 0xFE46, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore),
            new(0xFE70, 0xFE74, GeneratorUnicodeScripts.Arab),
            new(0xFE76, 0xFEFC, GeneratorUnicodeScripts.Arab),
            new(0xFF21, 0xFF3A, GeneratorUnicodeScripts.Latn),
            new(0xFF41, 0xFF5A, GeneratorUnicodeScripts.Latn),
            new(0xFF61, 0xFF65, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore),
            new(0xFF66, 0xFF9F, GeneratorUnicodeScripts.Jpan),
            new(0xFFA0, 0xFFBE, GeneratorUnicodeScripts.Kore),
            new(0xFFC2, 0xFFC7, GeneratorUnicodeScripts.Kore),
            new(0xFFCA, 0xFFCF, GeneratorUnicodeScripts.Kore),
            new(0xFFD2, 0xFFD7, GeneratorUnicodeScripts.Kore),
            new(0xFFDA, 0xFFDC, GeneratorUnicodeScripts.Kore),
            new(0x10140, 0x1018E, GeneratorUnicodeScripts.Grek),
            new(0x101A0, 0x101A0, GeneratorUnicodeScripts.Grek),
            new(0x101FD, 0x101FD, GeneratorUnicodeScripts.All),
            new(0x102E0, 0x102FB, GeneratorUnicodeScripts.Arab),
            new(0x10780, 0x10785, GeneratorUnicodeScripts.Latn),
            new(0x10787, 0x107B0, GeneratorUnicodeScripts.Latn),
            new(0x107B2, 0x107BA, GeneratorUnicodeScripts.Latn),
            new(0x10E60, 0x10E7E, GeneratorUnicodeScripts.Arab),
            new(0x10EC2, 0x10EC4, GeneratorUnicodeScripts.Arab),
            new(0x10EFC, 0x10EFF, GeneratorUnicodeScripts.Arab),
            new(0x111E1, 0x111F4, GeneratorUnicodeScripts.Sinh),
            new(0x11301, 0x11301, GeneratorUnicodeScripts.Taml),
            new(0x11303, 0x11303, GeneratorUnicodeScripts.Taml),
            new(0x1133B, 0x1133C, GeneratorUnicodeScripts.Taml),
            new(0x11660, 0x1166C, GeneratorUnicodeScripts.Mong),
            new(0x116D0, 0x116E3, GeneratorUnicodeScripts.Mymr),
            new(0x11B00, 0x11B09, GeneratorUnicodeScripts.Deva),
            new(0x11FC0, 0x11FF1, GeneratorUnicodeScripts.Taml),
            new(0x11FFF, 0x11FFF, GeneratorUnicodeScripts.Taml),
            new(0x16FE2, 0x16FE3, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore),
            new(0x16FF0, 0x16FF1, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore),
            new(0x1AFF0, 0x1AFF3, GeneratorUnicodeScripts.Jpan),
            new(0x1AFF5, 0x1AFFB, GeneratorUnicodeScripts.Jpan),
            new(0x1AFFD, 0x1AFFE, GeneratorUnicodeScripts.Jpan),
            new(0x1B000, 0x1B122, GeneratorUnicodeScripts.Jpan),
            new(0x1B132, 0x1B132, GeneratorUnicodeScripts.Jpan),
            new(0x1B150, 0x1B152, GeneratorUnicodeScripts.Jpan),
            new(0x1B155, 0x1B155, GeneratorUnicodeScripts.Jpan),
            new(0x1B164, 0x1B167, GeneratorUnicodeScripts.Jpan),
            new(0x1CF00, 0x1CF2D, GeneratorUnicodeScripts.All),
            new(0x1CF30, 0x1CF46, GeneratorUnicodeScripts.All),
            new(0x1D167, 0x1D169, GeneratorUnicodeScripts.All),
            new(0x1D17B, 0x1D182, GeneratorUnicodeScripts.All),
            new(0x1D185, 0x1D18B, GeneratorUnicodeScripts.All),
            new(0x1D1AA, 0x1D1AD, GeneratorUnicodeScripts.All),
            new(0x1D200, 0x1D245, GeneratorUnicodeScripts.Grek),
            new(0x1D360, 0x1D371, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore),
            new(0x1DF00, 0x1DF1E, GeneratorUnicodeScripts.Latn),
            new(0x1DF25, 0x1DF2A, GeneratorUnicodeScripts.Latn),
            new(0x1E030, 0x1E06D, GeneratorUnicodeScripts.Cyrl),
            new(0x1E08F, 0x1E08F, GeneratorUnicodeScripts.Cyrl),
            new(0x1E7E0, 0x1E7E6, GeneratorUnicodeScripts.Ethi),
            new(0x1E7E8, 0x1E7EB, GeneratorUnicodeScripts.Ethi),
            new(0x1E7ED, 0x1E7EE, GeneratorUnicodeScripts.Ethi),
            new(0x1E7F0, 0x1E7FE, GeneratorUnicodeScripts.Ethi),
            new(0x1EE00, 0x1EE03, GeneratorUnicodeScripts.Arab),
            new(0x1EE05, 0x1EE1F, GeneratorUnicodeScripts.Arab),
            new(0x1EE21, 0x1EE22, GeneratorUnicodeScripts.Arab),
            new(0x1EE24, 0x1EE24, GeneratorUnicodeScripts.Arab),
            new(0x1EE27, 0x1EE27, GeneratorUnicodeScripts.Arab),
            new(0x1EE29, 0x1EE32, GeneratorUnicodeScripts.Arab),
            new(0x1EE34, 0x1EE37, GeneratorUnicodeScripts.Arab),
            new(0x1EE39, 0x1EE39, GeneratorUnicodeScripts.Arab),
            new(0x1EE3B, 0x1EE3B, GeneratorUnicodeScripts.Arab),
            new(0x1EE42, 0x1EE42, GeneratorUnicodeScripts.Arab),
            new(0x1EE47, 0x1EE47, GeneratorUnicodeScripts.Arab),
            new(0x1EE49, 0x1EE49, GeneratorUnicodeScripts.Arab),
            new(0x1EE4B, 0x1EE4B, GeneratorUnicodeScripts.Arab),
            new(0x1EE4D, 0x1EE4F, GeneratorUnicodeScripts.Arab),
            new(0x1EE51, 0x1EE52, GeneratorUnicodeScripts.Arab),
            new(0x1EE54, 0x1EE54, GeneratorUnicodeScripts.Arab),
            new(0x1EE57, 0x1EE57, GeneratorUnicodeScripts.Arab),
            new(0x1EE59, 0x1EE59, GeneratorUnicodeScripts.Arab),
            new(0x1EE5B, 0x1EE5B, GeneratorUnicodeScripts.Arab),
            new(0x1EE5D, 0x1EE5D, GeneratorUnicodeScripts.Arab),
            new(0x1EE5F, 0x1EE5F, GeneratorUnicodeScripts.Arab),
            new(0x1EE61, 0x1EE62, GeneratorUnicodeScripts.Arab),
            new(0x1EE64, 0x1EE64, GeneratorUnicodeScripts.Arab),
            new(0x1EE67, 0x1EE6A, GeneratorUnicodeScripts.Arab),
            new(0x1EE6C, 0x1EE72, GeneratorUnicodeScripts.Arab),
            new(0x1EE74, 0x1EE77, GeneratorUnicodeScripts.Arab),
            new(0x1EE79, 0x1EE7C, GeneratorUnicodeScripts.Arab),
            new(0x1EE7E, 0x1EE7E, GeneratorUnicodeScripts.Arab),
            new(0x1EE80, 0x1EE89, GeneratorUnicodeScripts.Arab),
            new(0x1EE8B, 0x1EE9B, GeneratorUnicodeScripts.Arab),
            new(0x1EEA1, 0x1EEA3, GeneratorUnicodeScripts.Arab),
            new(0x1EEA5, 0x1EEA9, GeneratorUnicodeScripts.Arab),
            new(0x1EEAB, 0x1EEBB, GeneratorUnicodeScripts.Arab),
            new(0x1EEF0, 0x1EEF1, GeneratorUnicodeScripts.Arab),
            new(0x1F200, 0x1F200, GeneratorUnicodeScripts.Jpan),
            new(0x1F250, 0x1F251, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore),
            new(0x20000, 0x2A6DF, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore),
            new(0x2A700, 0x2B739, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore),
            new(0x2B740, 0x2B81D, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore),
            new(0x2B820, 0x2CEA1, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore),
            new(0x2CEB0, 0x2EBE0, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore),
            new(0x2EBF0, 0x2EE5D, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore),
            new(0x2F800, 0x2FA1D, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore),
            new(0x30000, 0x3134A, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore),
            new(0x31350, 0x323AF, GeneratorUnicodeScripts.Hani | GeneratorUnicodeScripts.Jpan | GeneratorUnicodeScripts.Kore),
            new(0xE0100, 0xE01EF, GeneratorUnicodeScripts.All),
        ];

        static ImmutableArray<string> NormalizeGuards(
            ImmutableArray<string> values,
            string casing,
            ImmutableArray<string> ownerScripts,
            string subject)
        {
            return NormalizeAuthoredTexts(
                values,
                casing,
                ownerScripts,
                subject,
                allowStemPlaceholder: false,
                allowNonLetters: false);
        }

        [Flags]
        enum GeneratorUnicodeScripts : uint
        {
            None = 0,
            Arab = 1 << 0,
            Armn = 1 << 1,
            Beng = 1 << 2,
            Cyrl = 1 << 3,
            Deva = 1 << 4,
            Geor = 1 << 5,
            Grek = 1 << 6,
            Gujr = 1 << 7,
            Guru = 1 << 8,
            Hani = 1 << 9,
            Hebr = 1 << 10,
            Jpan = 1 << 11,
            Khmr = 1 << 12,
            Knda = 1 << 13,
            Kore = 1 << 14,
            Laoo = 1 << 15,
            Latn = 1 << 16,
            Mlym = 1 << 17,
            Mymr = 1 << 18,
            Orya = 1 << 19,
            Taml = 1 << 20,
            Telu = 1 << 21,
            Thai = 1 << 22,
            Ethi = 1 << 23,
            Mong = 1 << 24,
            Sinh = 1 << 25,
            Han = Hani | Jpan | Kore,
            All = 0x03FFFFFF
        }

        readonly struct GeneratorUnicodeScriptRange(
            int first,
            int last,
            GeneratorUnicodeScripts scripts)
        {
            public int First { get; } = first;
            public int Last { get; } = last;
            public GeneratorUnicodeScripts Scripts { get; } = scripts;
        }

        static string[] ReachableCategories(string cardinalRule) =>
            CardinalPluralRuleMetadata.GetReachableCategories(cardinalRule);

        static void ValidateReachableCategories(
            ImmutableArray<InflectionProfileInput>.Builder profiles,
            ImmutableArray<InflectionOwnerInput>.Builder owners,
            ImmutableArray<Diagnostic>.Builder diagnostics)
        {
            foreach (var profile in profiles)
            {
                if (profile.Owner is null)
                {
                    continue;
                }

                var owner = owners.FirstOrDefault(owner =>
                    string.Equals(owner.Owner, profile.Owner, StringComparison.Ordinal));
                if (owner is null || owner.Capability == "invariant")
                {
                    continue;
                }

                foreach (var category in ReachableCategories(profile.CardinalRule))
                {
                    var missingLexeme = owner.Lexemes.FirstOrDefault(
                        lexeme => !lexeme.Display.ContainsKey(category));
                    if (missingLexeme is not null)
                    {
                        diagnostics.Add(CreateDiagnostic(
                            owner.Owner,
                            $"Atomic inflection owner '{owner.Owner}' lexeme '{missingLexeme.Id}' must define display category '{category}' reachable through accepted profile '{profile.LocaleCode}'."));
                        continue;
                    }

                    var missingRule = owner.Rules.FirstOrDefault(
                        rule => !rule.Display.ContainsKey(category));
                    if (missingRule is not null)
                    {
                        diagnostics.Add(CreateDiagnostic(
                            owner.Owner,
                            $"Atomic inflection owner '{owner.Owner}' rule '{missingRule.Id}' must define display category '{category}' reachable through accepted profile '{profile.LocaleCode}'."));
                    }
                }
            }
        }

        static string GetRequiredScalar(SimpleYamlMapping mapping, string name, string message) =>
            mapping.GetScalar(name) is { Length: > 0 } value
                ? value
                : throw new InvalidOperationException(message + ".");

        static void ValidateProperties(
            SimpleYamlMapping mapping,
            string subject,
            params string[] allowedProperties)
        {
            var unsupported = mapping.Values.Keys.FirstOrDefault(
                property => !allowedProperties.Contains(property, StringComparer.Ordinal));
            if (unsupported is not null)
            {
                throw new InvalidOperationException(
                    $"{subject} defines unsupported property '{unsupported}'.");
            }
        }

        static void ValidateAliases(
            SimpleYamlMapping mapping,
            string name,
            string? alternateName)
        {
            if (alternateName is not null &&
                mapping.Values.ContainsKey(name) &&
                mapping.Values.ContainsKey(alternateName))
            {
                throw new InvalidOperationException(
                    $"surfaces.inflection defines conflicting aliases '{name}' and '{alternateName}'.");
            }
        }

        static string GetRequiredScalar(
            SimpleYamlMapping mapping,
            string name,
            string alternateName,
            string message) =>
            GetScalar(mapping, name, alternateName) is { Length: > 0 } value
                ? value
                : throw new InvalidOperationException(message + ".");

        static int GetRequiredInt(SimpleYamlMapping mapping, string name, string message) =>
            int.TryParse(mapping.GetScalar(name), NumberStyles.None, CultureInfo.InvariantCulture, out var value)
                ? value
                : throw new InvalidOperationException(message + ".");

        static SimpleYamlMapping GetRequiredMapping(
            SimpleYamlMapping mapping,
            string name,
            string message) =>
            mapping.TryGetValue(name, out var value) && value is SimpleYamlMapping result
                ? result
                : throw new InvalidOperationException(message + ".");

        static SimpleYamlMapping GetRequiredMapping(
            SimpleYamlMapping mapping,
            string name,
            string? alternateName,
            string message) =>
            GetOptionalMapping(mapping, name, alternateName)
                ?? throw new InvalidOperationException(message + ".");

        static SimpleYamlMapping? GetOptionalMapping(
            SimpleYamlMapping mapping,
            string name,
            string? alternateName)
        {
            ValidateAliases(mapping, name, alternateName);
            if (mapping.TryGetValue(name, out var value) ||
                alternateName is not null && mapping.TryGetValue(alternateName, out value))
            {
                return value as SimpleYamlMapping
                    ?? throw new InvalidOperationException(
                        $"surfaces.inflection.{name} must be a mapping.");
            }

            return null;
        }

        static ImmutableArray<string> GetRequiredStrings(
            SimpleYamlMapping mapping,
            string name,
            string message)
        {
            var result = GetOptionalStrings(mapping, name);
            return result.IsEmpty
                ? throw new InvalidOperationException(message + ".")
                : result;
        }

        static ImmutableArray<string> GetOptionalStrings(SimpleYamlMapping mapping, string name)
            => GetOptionalStrings(mapping, name, alternateName: null);

        static ImmutableArray<string> GetOptionalStrings(
            SimpleYamlMapping mapping,
            string name,
            string? alternateName)
        {
            ValidateAliases(mapping, name, alternateName);
            if (!mapping.TryGetValue(name, out var value) &&
                (alternateName is null || !mapping.TryGetValue(alternateName, out value)))
            {
                return [];
            }

            if (value is not SimpleYamlSequence sequence)
            {
                throw new InvalidOperationException(
                    $"surfaces.inflection.{name} must be a block sequence.");
            }

            var values = ImmutableArray.CreateBuilder<string>();
            foreach (var item in sequence.Items)
            {
                if (item is not SimpleYamlScalar scalar || string.IsNullOrWhiteSpace(scalar.Value))
                {
                    throw new InvalidOperationException(
                        $"surfaces.inflection.{name} must contain non-empty scalar values.");
                }

                values.Add(scalar.Value);
            }

            return values.ToImmutable();
        }

        static bool GetOptionalBoolean(
            SimpleYamlMapping? mapping,
            string name,
            string? alternateName,
            bool defaultValue)
        {
            if (mapping is null)
            {
                return defaultValue;
            }

            ValidateAliases(mapping, name, alternateName);
            if (!mapping.TryGetValue(name, out var value) &&
                (alternateName is null || !mapping.TryGetValue(alternateName, out value)))
            {
                return defaultValue;
            }

            return value is SimpleYamlScalar scalar
                ? scalar.Value switch
                {
                    "true" => true,
                    "false" => false,
                    _ => throw new InvalidOperationException(
                        $"surfaces.inflection.{name} must be 'true' or 'false'.")
                }
                : throw new InvalidOperationException(
                    $"surfaces.inflection.{name} must be 'true' or 'false'.");
        }

        static string? GetScalar(
            SimpleYamlMapping? mapping,
            string name,
            string alternateName)
        {
            if (mapping is null)
            {
                return null;
            }

            ValidateAliases(mapping, name, alternateName);
            return mapping.GetScalar(name) ?? mapping.GetScalar(alternateName);
        }

        static ImmutableArray<InflectionProfileInput> ParseRegionalProfiles(
            string localeCode,
            SimpleYamlMapping mapping,
            string? owner)
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

                profiles.Add(new(regionalRule.Key, ruleScalar.Value, owner));
            }

            return profiles.ToImmutable();
        }

        internal static Diagnostic CreateDiagnostic(string localeCode, string message) =>
            Diagnostic.Create(
                Diagnostics.InvalidLocaleDefinition,
                Location.None,
                localeCode,
                message);

        internal static InflectionOwnerInput? ParseOwner(LocaleDefinitionFile? file)
        {
            if (file is null)
            {
                return null;
            }

            try
            {
                var document = CanonicalLocaleAuthoring.Parse(file.LocaleCode, file.FileText);
                var definition = CanonicalLocaleAuthoring.ToLocaleDefinition(document);
                return definition.Features.TryGetValue("inflection", out var value) &&
                    value is SimpleYamlMapping mapping
                        ? ParseProfile(file.LocaleCode, mapping).Owner
                        : null;
            }
            catch (InvalidOperationException)
            {
                // The collected catalog reports canonical diagnostics once. Per-locale emission
                // stays silent and simply withholds invalid output.
                return null;
            }
        }
    }

    sealed class InflectionOwnerEmissionState(
        bool hasErrors,
        ImmutableArray<string> owners) : IEquatable<InflectionOwnerEmissionState>
    {
        readonly bool hasErrors = hasErrors;
        readonly ImmutableArray<string> owners = owners;

        public bool CanEmitOwner(string owner) =>
            !hasErrors && owners.Contains(owner, StringComparer.Ordinal);

        public bool Equals(InflectionOwnerEmissionState? other) =>
            other is not null &&
            hasErrors == other.hasErrors &&
            owners.SequenceEqual(other.owners, StringComparer.Ordinal);

        public override bool Equals(object? obj) =>
            Equals(obj as InflectionOwnerEmissionState);

        public override int GetHashCode()
        {
            var hash = hasErrors ? 1 : 0;
            foreach (var owner in owners)
            {
                hash = unchecked((hash * 397) ^ StringComparer.Ordinal.GetHashCode(owner));
            }

            return hash;
        }
    }

    sealed class PerLocaleInflectionSourceInput(
        string? owner,
        string? source,
        Diagnostic? diagnostic) : IEquatable<PerLocaleInflectionSourceInput>
    {
        public string? Owner { get; } = owner;
        public string? Source { get; } = source;
        public Diagnostic? Diagnostic { get; } = diagnostic;

        public static PerLocaleInflectionSourceInput Create(LocaleDefinitionFile? file)
        {
            var owner = InflectionCatalogInput.ParseOwner(file);
            if (owner is null)
            {
                return new(null, null, null);
            }

            var source = InflectionCatalogInput.BuildOwnerSource(owner);
            return Encoding.UTF8.GetByteCount(source) <=
                InflectionCatalogInput.MaximumOwnerSourceBytes
                ? new(owner.Owner, source, null)
                : new(
                    owner.Owner,
                    null,
                    InflectionCatalogInput.CreateDiagnostic(
                        owner.Owner,
                        $"Generated atomic inflection owner '{owner.Owner}' exceeds the 256 KiB syntax-tree ceiling."));
        }

        public bool Equals(PerLocaleInflectionSourceInput? other) =>
            other is not null &&
            string.Equals(Owner, other.Owner, StringComparison.Ordinal) &&
            string.Equals(Source, other.Source, StringComparison.Ordinal) &&
            string.Equals(
                Diagnostic?.GetMessage(),
                other.Diagnostic?.GetMessage(),
                StringComparison.Ordinal);

        public override bool Equals(object? obj) =>
            Equals(obj as PerLocaleInflectionSourceInput);

        public override int GetHashCode()
        {
            var hash = Owner is null ? 0 : StringComparer.Ordinal.GetHashCode(Owner);
            hash = unchecked((hash * 397) ^
                (Source is null ? 0 : StringComparer.Ordinal.GetHashCode(Source)));
            return unchecked((hash * 397) ^
                (Diagnostic?.GetMessage() is { } message
                    ? StringComparer.Ordinal.GetHashCode(message)
                    : 0));
        }
    }

    sealed class InflectionOwnerSourceCatalog(ImmutableArray<Diagnostic> diagnostics)
    {
        public ImmutableArray<Diagnostic> Diagnostics { get; } = diagnostics;

        public static InflectionOwnerSourceCatalog Create(
            ImmutableArray<PerLocaleInflectionSourceInput> sources) =>
            new(
                sources
                    .Select(static source => source.Diagnostic)
                    .Where(static diagnostic => diagnostic is not null)
                    .Cast<Diagnostic>()
                    .ToImmutableArray());
    }

    sealed class PerLocaleInflectionInput(PerLocaleInflectionSourceInput? source)
    {
        readonly PerLocaleInflectionSourceInput? source = source;

        public static PerLocaleInflectionInput Create(
            PerLocaleInflectionSourceInput source,
            InflectionOwnerEmissionState state)
        {
            return new(source.Owner is not null &&
                source.Source is not null &&
                state.CanEmitOwner(source.Owner)
                ? source
                : null);
        }

        public void Emit(SourceProductionContext context)
        {
            if (source?.Owner is { } owner &&
                source.Source is { } generatedSource)
            {
                InflectionCatalogInput.EmitOwner(context, owner, generatedSource);
            }
        }
    }

    sealed class ParsedProfile(
        string localeCode,
        string cardinalRule,
        InflectionOwnerInput? owner)
    {
        public string LocaleCode { get; } = localeCode;
        public string CardinalRule { get; } = cardinalRule;
        public InflectionOwnerInput? Owner { get; } = owner;
    }

    sealed class InflectionProfileInput(
        string localeCode,
        string cardinalRule,
        string? owner)
    {
        public string LocaleCode { get; } = localeCode;
        public string CardinalRule { get; } = cardinalRule;
        public string? Owner { get; } = owner;
    }

    sealed class InflectionOwnerInput(
        string owner,
        string cardinalRule,
        string capability,
        string quantitySelector,
        ImmutableArray<string> scripts,
        string casing,
        string phraseMode,
        ImmutableArray<string> skipSimpleWords,
        ImmutableArray<InflectionLexemeInput> lexemes,
        ImmutableArray<InflectionRuleInput> rules)
    {
        public string Owner { get; } = owner;
        public string CardinalRule { get; } = cardinalRule;
        public string Capability { get; } = capability;
        public string QuantitySelector { get; } = quantitySelector;
        public ImmutableArray<string> Scripts { get; } = scripts;
        public string Casing { get; } = casing;
        public string PhraseMode { get; } = phraseMode;
        public ImmutableArray<string> SkipSimpleWords { get; } = skipSimpleWords;
        public ImmutableArray<InflectionLexemeInput> Lexemes { get; } = lexemes;
        public ImmutableArray<InflectionRuleInput> Rules { get; } = rules;
    }

    sealed class InflectionLexemeInput(
        string id,
        string? sense,
        InflectionFormInput singular,
        InflectionFormInput dictionaryPlural,
        ImmutableDictionary<string, InflectionFormInput> display)
    {
        public string Id { get; } = id;
        public string? Sense { get; } = sense;
        public InflectionFormInput Singular { get; } = singular;
        public InflectionFormInput DictionaryPlural { get; } = dictionaryPlural;
        public ImmutableDictionary<string, InflectionFormInput> Display { get; } = display;
    }

    sealed class InflectionFormInput(
        string preferred,
        ImmutableArray<string> accepted)
    {
        public string Preferred { get; } = preferred;
        public ImmutableArray<string> Accepted { get; } = accepted;
    }

    sealed class InflectionRuleInput(
        string id,
        string direction,
        int priority,
        string prefix,
        string suffix,
        ImmutableArray<string> precedingNot,
        string dictionaryPlural,
        ImmutableDictionary<string, string> display,
        ImmutableArray<string> excludedSurfaces,
        ImmutableArray<string> excludedLexemes,
        bool reverseEnabled,
        bool requiresExistingLexeme)
    {
        public string Id { get; } = id;
        public string Direction { get; } = direction;
        public int Priority { get; } = priority;
        public string Prefix { get; } = prefix;
        public string Suffix { get; } = suffix;
        public ImmutableArray<string> PrecedingNot { get; } = precedingNot;
        public string DictionaryPlural { get; } = dictionaryPlural;
        public ImmutableDictionary<string, string> Display { get; } = display;
        public ImmutableArray<string> ExcludedSurfaces { get; } = excludedSurfaces;
        public ImmutableArray<string> ExcludedLexemes { get; } = excludedLexemes;
        public bool ReverseEnabled { get; } = reverseEnabled;
        public bool RequiresExistingLexeme { get; } = requiresExistingLexeme;
    }

    sealed class InflectionLexemeTableInput(
        ImmutableArray<InflectionLexemeRecordInput> lexemes,
        ImmutableArray<InflectionLexemeEntryInput> entries,
        ImmutableArray<InflectionLexemeCandidateInput> candidates,
        ImmutableArray<int> forwardEntries,
        ImmutableArray<int> reverseEntries)
    {
        public ImmutableArray<InflectionLexemeRecordInput> Lexemes { get; } = lexemes;
        public ImmutableArray<InflectionLexemeEntryInput> Entries { get; } = entries;
        public ImmutableArray<InflectionLexemeCandidateInput> Candidates { get; } = candidates;
        public ImmutableArray<int> ForwardEntries { get; } = forwardEntries;
        public ImmutableArray<int> ReverseEntries { get; } = reverseEntries;
    }

    sealed class InflectionLexemeRecordInput(
        int singularEntryIndex,
        int dictionaryPluralEntryIndex,
        ImmutableArray<InflectionLexemeDisplayInput> display)
    {
        public int SingularEntryIndex { get; } = singularEntryIndex;
        public int DictionaryPluralEntryIndex { get; } = dictionaryPluralEntryIndex;
        public ImmutableArray<InflectionLexemeDisplayInput> Display { get; } = display;
    }

    sealed class InflectionLexemeDisplayInput(
        string category,
        int preferredEntryIndex)
    {
        public string Category { get; } = category;
        public int PreferredEntryIndex { get; } = preferredEntryIndex;
    }

    sealed class InflectionLexemeEntryInput(
        string value,
        int candidateOffset,
        int candidateCount)
    {
        public string Value { get; } = value;
        public int CandidateOffset { get; } = candidateOffset;
        public int CandidateCount { get; } = candidateCount;
    }

    sealed class InflectionLexemeCandidateInput(
        int lexemeIndex,
        int roles)
    {
        public int LexemeIndex { get; } = lexemeIndex;
        public int Roles { get; } = roles;
    }
}
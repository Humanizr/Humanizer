using System.Collections.Immutable;
using System.Globalization;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Humanizer.SourceGenerators;

public sealed partial class HumanizerSourceGenerator
{
    internal static int CompareInflectionKeys(string left, string right) =>
        global::Humanizer.InflectionUnicodeData.CompareSimpleCase(left, right);

    internal static int FoldInflectionScalar(int scalar) =>
        global::Humanizer.InflectionUnicodeData.FoldSimpleCase(scalar);

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

                builder.Append("                    ], InflectionCountability.");
                builder.Append(ToEnumName(lexeme.Countability));
                builder.AppendLine("),");
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
                builder.Append(", ");
                builder.Append("(InflectionUnicodeScripts)");
                builder.Append(GetScriptMask(rule.Scripts).ToString(CultureInfo.InvariantCulture));
                builder.Append('u');
                builder.Append(", (InflectionCountability)");
                builder.Append(GetCountabilityMask(rule.Countabilities).ToString(CultureInfo.InvariantCulture));
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
                ? global::Humanizer.InflectionUnicodeData.SimpleCaseComparer.Instance
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
                    countability,
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
                    ruleScripts,
                    $"rule '{id}' prefix",
                    allowStemPlaceholder: false,
                    allowNonLetters: false);
                var suffix = NormalizeAuthoredText(
                    match.GetScalar("suffix") ?? string.Empty,
                    casing,
                    ruleScripts,
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
                    ruleScripts,
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
                    ruleScripts,
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
                        ruleScripts,
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
                            ruleScripts,
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
                        ruleScripts,
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
                    countabilities,
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
                        defaultValue: true),
                    ruleScripts));
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
                        ? global::Humanizer.InflectionUnicodeData.SimpleCaseComparer.Instance
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
            var allowedScripts =
                (global::Humanizer.InflectionUnicodeScripts)GetScriptMask(ownerScripts);

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

                var pinnedLetter = false;
                var pinnedMark = false;
                if (category == UnicodeCategory.OtherNotAssigned)
                {
                    _ = global::Humanizer.InflectionUnicodeData.TryGetPinnedLetterOrMark(
                        scalar,
                        out pinnedLetter,
                        out pinnedMark);
                }

                var isLetter = pinnedLetter ||
                    category is UnicodeCategory.UppercaseLetter or
                    UnicodeCategory.LowercaseLetter or
                    UnicodeCategory.TitlecaseLetter or
                    UnicodeCategory.ModifierLetter or
                    UnicodeCategory.OtherLetter;
                var isMark = pinnedMark ||
                    category is UnicodeCategory.NonSpacingMark or
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

                var scalarScripts =
                    global::Humanizer.InflectionUnicodeData.GetScripts(scalar);
                if ((scalarScripts & allowedScripts) ==
                    global::Humanizer.InflectionUnicodeScripts.None)
                {
                    return false;
                }

                detectedScripts &= scalarScripts;
                if (detectedScripts == global::Humanizer.InflectionUnicodeScripts.None)
                {
                    return false;
                }

                hasLetter |= isLetter;
            }

            return hasLetter;
        }

        static uint GetScriptMask(ImmutableArray<string> scripts)
        {
            var mask = global::Humanizer.InflectionUnicodeScripts.None;
            foreach (var script in scripts)
            {
                _ = global::Humanizer.InflectionUnicodeData.TryGetScript(
                    script,
                    out var scriptValue);
                mask |= scriptValue;
            }

            return (uint)mask;
        }

        static byte GetCountabilityMask(ImmutableArray<string> countabilities)
        {
            byte mask = 0;
            foreach (var countability in countabilities)
            {
                mask |= countability switch
                {
                    "count" => 1 << 0,
                    "mass" => 1 << 1,
                    "collective" => 1 << 2,
                    "plural-only" => 1 << 3,
                    _ => throw new InvalidOperationException(
                        $"Unsupported inflection countability '{countability}'.")
                };
            }

            return mask;
        }

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
        string countability,
        string? sense,
        InflectionFormInput singular,
        InflectionFormInput dictionaryPlural,
        ImmutableDictionary<string, InflectionFormInput> display)
    {
        public string Id { get; } = id;
        public string Countability { get; } = countability;
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
        ImmutableArray<string> countabilities,
        string prefix,
        string suffix,
        ImmutableArray<string> precedingNot,
        string dictionaryPlural,
        ImmutableDictionary<string, string> display,
        ImmutableArray<string> excludedSurfaces,
        ImmutableArray<string> excludedLexemes,
        bool reverseEnabled,
        bool requiresExistingLexeme,
        ImmutableArray<string> scripts)
    {
        public string Id { get; } = id;
        public string Direction { get; } = direction;
        public int Priority { get; } = priority;
        public ImmutableArray<string> Countabilities { get; } = countabilities;
        public string Prefix { get; } = prefix;
        public string Suffix { get; } = suffix;
        public ImmutableArray<string> PrecedingNot { get; } = precedingNot;
        public string DictionaryPlural { get; } = dictionaryPlural;
        public ImmutableDictionary<string, string> Display { get; } = display;
        public ImmutableArray<string> ExcludedSurfaces { get; } = excludedSurfaces;
        public ImmutableArray<string> ExcludedLexemes { get; } = excludedLexemes;
        public bool ReverseEnabled { get; } = reverseEnabled;
        public bool RequiresExistingLexeme { get; } = requiresExistingLexeme;
        public ImmutableArray<string> Scripts { get; } = scripts;
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
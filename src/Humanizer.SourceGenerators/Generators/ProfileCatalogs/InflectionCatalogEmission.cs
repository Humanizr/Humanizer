using System.Collections.Immutable;
using System.Globalization;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Humanizer.SourceGenerators;

public sealed partial class HumanizerSourceGenerator
{
    sealed partial class InflectionCatalogInput
    {
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
    }
}
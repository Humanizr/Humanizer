using System.Collections.Immutable;
using System.Globalization;
using System.Text;

using Microsoft.CodeAnalysis;

namespace Humanizer.SourceGenerators;

public sealed partial class HumanizerSourceGenerator
{
    sealed partial class InflectionCatalogInput
    {
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
                if (!TryNormalizeSimpleLower(normalized, out var lower))
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

        static bool TryNormalizeSimpleLower(string value, out string normalized)
        {
            var builder = new StringBuilder(value.Length);
            for (var index = 0; index < value.Length;)
            {
                var scalarOffset = index;
                var first = value[index++];
                int scalar;
                if (char.IsHighSurrogate(first))
                {
                    if (index >= value.Length || !char.IsLowSurrogate(value[index]))
                    {
                        normalized = value;
                        return false;
                    }

                    scalar = char.ConvertToUtf32(first, value[index++]);
                }
                else
                {
                    if (char.IsLowSurrogate(first))
                    {
                        normalized = value;
                        return false;
                    }

                    scalar = first;
                }

                var scalarLength = index - scalarOffset;
                var lower = global::Humanizer.InflectionUnicodeData.ToLowerSimple(scalar);
                var upper = global::Humanizer.InflectionUnicodeData.ToUpperSimple(scalar);
                if ((lower <= char.MaxValue ? 1 : 2) != scalarLength ||
                    (upper <= char.MaxValue ? 1 : 2) != scalarLength)
                {
                    normalized = value;
                    return false;
                }

                builder.Append(char.ConvertFromUtf32(lower));
            }

            normalized = builder.ToString();
            return true;
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
    }
}
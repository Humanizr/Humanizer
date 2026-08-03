using System.Collections.Immutable;
using System.Globalization;

using Microsoft.CodeAnalysis;

namespace Humanizer.SourceGenerators;

public sealed partial class HumanizerSourceGenerator
{
    sealed partial class InflectionCatalogInput
    {
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
}
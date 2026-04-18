using System.Globalization;
using System.Text.Json;

namespace Humanizer.SourceGenerators;

public sealed partial class HumanizerSourceGenerator
{
    /// <summary>
    /// Emits words-to-number profile construction code directly from the resolved locale payload.
    /// This factory intentionally avoids the generic argument-interpreter pattern because the
    /// words-to-number engine set is small and stable enough to keep explicit, readable emitters.
    /// </summary>
    static class WordsToNumberEngineContractFactory
    {
        /// <summary>
        /// Emits direct constructor expressions for the words-to-number engines. Unlike
        /// <c>numberToWords</c>, this surface still benefits from explicit per-engine emitters
        /// because the parsing engines have fewer families and each one owns materially different
        /// lexical normalization rules.
        /// </summary>
        public static string Create(WordsToNumberProfileDefinition profile) =>
            profile.Engine switch
            {
                "compound-scale" => CreateCompoundScaleExpression(profile),
                "contracted-scale" => CreateContractedScaleExpression(profile.Root),
                "east-asian-positional" => CreateEastAsianPositionalExpression(profile.Root),
                "greedy-compound" => CreateGreedyCompoundExpression(profile),
                "inverted-tens" => CreateInvertedTensExpression(profile.Root),
                "linking-affix" => CreateLinkingAffixExpression(profile.Root),
                "prefixed-tens-scale" => CreatePrefixedTensScaleExpression(profile.Root),
                "suffix-scale" => CreateSuffixScaleExpression(profile.Root),
                "token-map" => "new TokenMapWordsToNumberConverter(" + CreateTokenMapRulesExpression(profile.Root) + ")",
                "vigesimal-compound" => CreateVigesimalCompoundExpression(profile.Root),
                _ => CreateConventionalWordsToNumberExpression(profile.Engine)
            };

        static string CreateCompoundScaleExpression(WordsToNumberProfileDefinition profile)
        {
            var root = profile.Root;

            // Compound-scale locales resolve most tokens through a cardinal map, then layer ordinal
            // interpretation and sequence-multiplier heuristics on top. The generator emits those
            // pieces directly so the runtime parser only walks tokens.
            return "new CompoundScaleWordsToNumberConverter(new CompoundScaleWordsToNumberProfile(" +
                CreateStringLongFrozenDictionaryExpression(EngineContractUtilities.GetRequiredElement(root, "cardinalMap")) + ", " +
                CreateStringArrayExpression(EngineContractUtilities.GetRequiredElement(root, "tens")) + ", " +
                CreateStringArrayExpression(EngineContractUtilities.GetRequiredElement(root, "largeScales")) + ", " +
                QuoteLiteral(GetOptionalString(root, "ignoredToken") ?? string.Empty) + ", " +
                CreateWordsOrdinalMapExpression(root, "CompoundScaleWordsToNumberConverter.BuildOrdinalMap") + ", " +
                CreateOptionalStringArrayExpression(root, "negativePrefixes") + ", " +
                (GetOptionalInt64(root, "sequenceMultiplierThreshold")?.ToString(CultureInfo.InvariantCulture) ?? "null") +
                "))";
        }

        static string CreateContractedScaleExpression(JsonElement root) =>
            "new ContractedScaleWordsToNumberConverter(new ContractedScaleWordsToNumberProfile(" +
            QuoteLiteral(GetRequiredString(root, "minusWord")) + ", " +
            CreateStringLongFrozenDictionaryExpression(EngineContractUtilities.GetRequiredElement(root, "cardinalMap")) +
            "))";

        static string CreateEastAsianPositionalExpression(JsonElement root) =>
            "new EastAsianPositionalWordsToNumberConverter(new EastAsianPositionalWordsToNumberProfile(" +
            CreateStringLongFrozenDictionaryExpression(EngineContractUtilities.GetRequiredElement(root, "digits")) + ", " +
            CreateStringLongFrozenDictionaryExpression(EngineContractUtilities.GetRequiredElement(root, "smallUnits")) + ", " +
            CreateStringLongFrozenDictionaryExpression(EngineContractUtilities.GetRequiredElement(root, "largeUnits")) + ", " +
            CreateOptionalStringArrayExpression(root, "negativePrefixes") + ", " +
            QuoteLiteral(GetOptionalString(root, "ordinalPrefix") ?? string.Empty) + ", " +
            QuoteLiteral(GetOptionalString(root, "ordinalSuffix") ?? string.Empty) + ", " +
            CreateOptionalStringLongFrozenDictionaryExpression(root, "ordinalMap") +
            "))";

        static string CreateGreedyCompoundExpression(WordsToNumberProfileDefinition profile)
        {
            var root = profile.Root;
            return "new GreedyCompoundWordsToNumberConverter(new GreedyCompoundWordsToNumberProfile(" +
                CreateStringLongFrozenDictionaryExpression(EngineContractUtilities.GetRequiredElement(root, "cardinalMap")) + ", " +
                CreateGreedyCompoundOrdinalMapExpression(profile) + ", " +
                CreateOptionalStringArrayExpression(root, "negativePrefixes") + ", " +
                CreateOptionalStringArrayExpression(root, "ignoredTokens") + ", " +
                CreateOptionalStringArrayExpression(root, "ordinalAbbreviationSuffixes") + ", " +
                QuoteLiteral(GetOptionalString(root, "charactersToRemove") ?? string.Empty) + ", " +
                QuoteLiteral(GetOptionalString(root, "charactersToReplaceWithSpace") ?? string.Empty) + ", " +
                CreateStringReplacementArrayExpression(root, "textReplacements") + ", " +
                (GetBoolean(root, "lowercase") ? "true" : "false") + ", " +
                (GetBoolean(root, "removeDiacritics") ? "true" : "false") + ", " +
                (GetOptionalInt64(root, "hundredValue")?.ToString(CultureInfo.InvariantCulture) ?? "100") + ", " +
                (GetOptionalInt64(root, "scaleThreshold")?.ToString(CultureInfo.InvariantCulture) ?? "1000") +
                "))";
        }

        static string CreateInvertedTensExpression(JsonElement root) =>
            "new InvertedTensWordsToNumberConverter(new InvertedTensWordsToNumberProfile(" +
            CreateStringLongFrozenDictionaryExpression(EngineContractUtilities.GetRequiredElement(root, "cardinalMap")) + ", " +
            CreateStringLongFrozenDictionaryExpression(EngineContractUtilities.GetRequiredElement(root, "unitMap")) + ", " +
            CreateInvertedTensTokenArrayExpression(EngineContractUtilities.GetRequiredElement(root, "tensTokens")) + ", " +
            QuoteLiteral(GetRequiredString(root, "tensLinker")) + ", " +
            CreateStringArrayExpression(EngineContractUtilities.GetRequiredElement(root, "scaleTokens")) + ", " +
            CreateWordsOrdinalMapExpression(root, "InvertedTensWordsToNumberConverter.BuildOrdinalMap") + ", " +
            CreateOptionalStringArrayExpression(root, "negativePrefixes") + ", " +
            CreateOptionalStringArrayExpression(root, "ignoredTokens") + ", " +
            CreateOptionalStringArrayExpression(root, "ordinalSuffixes") + ", " +
            CreateStringReplacementArrayExpression(root, "unitPartReplacements") + ", " +
            (GetBoolean(root, "allowInvariantIntegerInput") ? "true" : "false") +
            "))";

        static string CreateLinkingAffixExpression(JsonElement root) =>
            "new LinkingAffixWordsToNumberConverter(new LinkingAffixWordsToNumberProfile(" +
            CreateStringLongFrozenDictionaryExpression(EngineContractUtilities.GetRequiredElement(root, "cardinalMap")) + ", " +
            QuoteLiteral(GetRequiredString(root, "teenPrefix")) + ", " +
            GetRequiredInt64(root, "teenBaseValue").ToString(CultureInfo.InvariantCulture) + ", " +
            CreateStringArrayExpression(EngineContractUtilities.GetRequiredElement(root, "linkedSuffixes")) + ", " +
            CreateOptionalStringArrayExpression(root, "ignoredTokens") + ", " +
            CreateOptionalStringArrayExpression(root, "negativePrefixes") +
            "))";

        static string CreatePrefixedTensScaleExpression(JsonElement root) =>
            "new PrefixedTensScaleWordsToNumberConverter(new PrefixedTensScaleWordsToNumberProfile(" +
            CreateStringLongFrozenDictionaryExpression(EngineContractUtilities.GetRequiredElement(root, "cardinalMap")) + ", " +
            CreateStringLongFrozenDictionaryExpression(EngineContractUtilities.GetRequiredElement(root, "tensMap")) + ", " +
            CreatePrefixedScaleWordArrayExpression(EngineContractUtilities.GetRequiredElement(root, "scales")) + ", " +
            CreatePrefixedTensRuleArrayExpression(EngineContractUtilities.GetRequiredElement(root, "prefixedTens")) + ", " +
            CreateOptionalStringArrayExpression(root, "negativePrefixes") +
            "))";

        static string CreateSuffixScaleExpression(JsonElement root) =>
            "new SuffixScaleWordsToNumberConverter(new SuffixScaleWordsToNumberProfile(" +
            CreateStringLongFrozenDictionaryExpression(EngineContractUtilities.GetRequiredElement(root, "cardinalMap")) + ", " +
            CreateStringLongFrozenDictionaryExpression(EngineContractUtilities.GetRequiredElement(root, "bareScaleMap")) + ", " +
            CreateSuffixScaleWordArrayExpression(EngineContractUtilities.GetRequiredElement(root, "scales")) + ", " +
            QuoteLiteral(GetRequiredString(root, "hundredSingularToken")) + ", " +
            QuoteLiteral(GetRequiredString(root, "hundredPluralToken")) + ", " +
            QuoteLiteral(GetRequiredString(root, "tensSuffixToken")) + ", " +
            QuoteLiteral(GetRequiredString(root, "teenSuffixToken")) + ", " +
            CreateOptionalStringArrayExpression(root, "negativePrefixes") +
            "))";

        static string CreateVigesimalCompoundExpression(JsonElement root) =>
            "new VigesimalCompoundWordsToNumberConverter(new VigesimalCompoundWordsToNumberProfile(" +
            CreateStringLongFrozenDictionaryExpression(EngineContractUtilities.GetRequiredElement(root, "cardinalMap")) + ", " +
            CreateStringLongFrozenDictionaryExpression(EngineContractUtilities.GetRequiredElement(root, "ordinalMap")) + ", " +
            CreateOptionalStringArrayExpression(root, "negativePrefixes") + ", " +
            CreateOptionalStringArrayExpression(root, "ignoredTokens") + ", " +
            QuoteLiteral(GetRequiredString(root, "vigesimalLeadingToken")) + ", " +
            CreateStringArrayExpression(EngineContractUtilities.GetRequiredElement(root, "vigesimalFollowerTokens")) + ", " +
            GetRequiredInt64(root, "vigesimalValue").ToString(CultureInfo.InvariantCulture) + ", " +
            QuoteLiteral(GetRequiredString(root, "teenLeaderToken")) + ", " +
            CreateLongFrozenSetExpression(EngineContractUtilities.GetRequiredElement(root, "teenLeaderBases")) +
            "))";

        /// <summary>
        /// Token-map is the most data-driven parser family: the YAML describes normalization,
        /// token stripping, multiplier behavior, and ordinal handling, and the generated output
        /// becomes a single immutable rules object consumed by <c>TokenMapWordsToNumberConverter</c>.
        /// </summary>
        static string CreateTokenMapRulesExpression(JsonElement root)
        {
            var assignments = new[]
            {
                RuleAssignment("CardinalMap", CreateStringLongFrozenDictionaryExpression(EngineContractUtilities.GetRequiredElement(root, "cardinalMap"))),
                RuleAssignment("ExactOrdinalMap", CreateOptionalStringLongFrozenDictionaryExpression(root, "ordinalMap")),
                RuleAssignment("OrdinalScaleMap", CreateOptionalStringLongMapOrNull(root, "ordinalScaleMap")),
                RuleAssignment("GluedOrdinalScaleSuffixes", CreateOptionalStringLongMapOrNull(root, "gluedOrdinalScaleSuffixes")),
                RuleAssignment("CompositeScaleMap", CreateOptionalStringLongMapOrNull(root, "compositeScaleMap")),
                RuleAssignment("NormalizationProfile", "TokenMapNormalizationProfile." + ToEnumMemberName(GetRequiredString(root, "normalizationProfile"))),
                RuleAssignment("NegativePrefixes", CreateOptionalStringArrayExpression(root, "negativePrefixes")),
                RuleAssignment("NegativeSuffixes", CreateOptionalStringArrayExpression(root, "negativeSuffixes")),
                RuleAssignment("OrdinalPrefixes", CreateOptionalStringArrayExpression(root, "ordinalPrefixes")),
                RuleAssignment("IgnoredTokens", CreateOptionalStringArrayExpression(root, "ignoredTokens")),
                RuleAssignment("LeadingTokenPrefixesToTrim", CreateOptionalStringArrayExpression(root, "leadingTokenPrefixesToTrim")),
                RuleAssignment("MultiplierTokens", CreateOptionalStringArrayExpression(root, "multiplierTokens")),
                RuleAssignment("TokenSuffixesToStrip", CreateOptionalStringArrayExpression(root, "tokenSuffixesToStrip")),
                RuleAssignment("OrdinalAbbreviationSuffixes", CreateOptionalStringArrayExpression(root, "ordinalAbbreviationSuffixes")),
                RuleAssignment("TeenSuffixTokens", CreateOptionalStringArrayExpression(root, "teenSuffixTokens")),
                RuleAssignment("HundredSuffixTokens", CreateOptionalStringArrayExpression(root, "hundredSuffixTokens")),
                RuleAssignment("AllowTerminalOrdinalToken", CreateBooleanLiteral(GetBoolean(root, "allowTerminalOrdinalToken"))),
                RuleAssignment("UseHundredMultiplier", CreateBooleanLiteral(GetBoolean(root, "useHundredMultiplier"))),
                RuleAssignment("AllowInvariantIntegerInput", CreateBooleanLiteral(GetBoolean(root, "allowInvariantIntegerInput"))),
                RuleAssignment("TeenBaseValue", CreateOptionalInt64Literal(root, "teenBaseValue", "10")),
                RuleAssignment("HundredSuffixValue", CreateOptionalInt64Literal(root, "hundredSuffixValue", "100")),
                RuleAssignment("UnitTokenMinValue", CreateOptionalInt64Literal(root, "unitTokenMinValue", "1")),
                RuleAssignment("UnitTokenMaxValue", CreateOptionalInt64Literal(root, "unitTokenMaxValue", "9")),
                RuleAssignment("HundredSuffixMinValue", CreateOptionalInt64Literal(root, "hundredSuffixMinValue", "long.MaxValue")),
                RuleAssignment("HundredSuffixMaxValue", CreateOptionalInt64Literal(root, "hundredSuffixMaxValue", "long.MinValue")),
                RuleAssignment("ScaleThreshold", CreateOptionalInt64Literal(root, "scaleThreshold", "1000"))
            };

            return "new() { " + string.Join(", ", assignments) + " }";
        }

        static string RuleAssignment(string propertyName, string expression) =>
            propertyName + " = " + expression;

        static string CreateOptionalStringLongMapOrNull(JsonElement root, string propertyName) =>
            root.TryGetProperty(propertyName, out var property)
                ? CreateStringLongFrozenDictionaryExpression(property)
                : "null";

        static string CreateBooleanLiteral(bool value) =>
            value ? "true" : "false";

        static string CreateOptionalInt64Literal(JsonElement root, string propertyName, string defaultExpression) =>
            GetOptionalInt64(root, propertyName)?.ToString(CultureInfo.InvariantCulture) ?? defaultExpression;

        static string CreateGreedyCompoundOrdinalMapExpression(WordsToNumberProfileDefinition profile)
        {
            if (profile.Root.TryGetProperty("ordinalMap", out var ordinalMap) && ordinalMap.ValueKind == JsonValueKind.Object)
            {
                return CreateStringLongFrozenDictionaryExpression(ordinalMap);
            }

            // Some locales derive ordinal parsing data from their number-to-words profile instead of
            // spelling every ordinal token out in YAML. That bridge is generation-time only: the
            // runtime still receives an immutable map.
            if (profile.Root.TryGetProperty("ordinalNumberToWordsKind", out var ordinalProfile) && ordinalProfile.ValueKind == JsonValueKind.String)
            {
                return "GreedyCompoundWordsToNumberConverter.BuildOrdinalMap(" +
                    "NumberToWordsProfileCatalog.Resolve(" + QuoteLiteral(ordinalProfile.GetString()!) + ", CultureInfo.InvariantCulture), " +
                    QuoteLiteral(GetOptionalString(profile.Root, "charactersToRemove") ?? string.Empty) + ", " +
                    QuoteLiteral(GetOptionalString(profile.Root, "charactersToReplaceWithSpace") ?? string.Empty) + ", " +
                    CreateStringReplacementArrayExpression(profile.Root, "textReplacements") +
                    (GetBoolean(profile.Root, "lowercase") ? ", lowercase: true" : string.Empty) +
                    (GetBoolean(profile.Root, "removeDiacritics") ? ", removeDiacritics: true" : string.Empty) +
                    ")";
            }

            return "new Dictionary<string, long>(StringComparer.Ordinal).ToFrozenDictionary(StringComparer.Ordinal)";
        }
    }
}
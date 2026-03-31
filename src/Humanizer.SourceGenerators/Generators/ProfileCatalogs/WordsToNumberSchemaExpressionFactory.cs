using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.Json;

namespace Humanizer.SourceGenerators;

public sealed partial class HumanizerSourceGenerator
{
    static class WordsToNumberSchemaExpressionFactory
    {
        public static string Create(WordsToNumberProfileDefinition profile, ImmutableDictionary<string, ExpressionSchema> schemas)
        {
            if (!schemas.TryGetValue(profile.Engine, out var schema))
            {
                return CreateConventionalWordsToNumberExpression(profile.Engine);
            }

            var runtimeType = schema.RuntimeType ?? GetConventionalWordsToNumberTypeName(profile.Engine);
            return "new " + runtimeType + "(" + CreateArguments(profile, profile.Root, schema.Arguments) + ")";
        }

        static string CreateArguments(
            WordsToNumberProfileDefinition profile,
            JsonElement root,
            ImmutableArray<ExpressionArgumentSchema> arguments) =>
            string.Join(", ", arguments.Select(argument => CreateArgument(profile, root, argument)));

        static string CreateArgument(WordsToNumberProfileDefinition profile, JsonElement root, ExpressionArgumentSchema argument) =>
            argument.Kind switch
            {
                "profile-object" => CreateObjectArgument(profile, root, argument),
                "string" => QuoteLiteral(GetStringValue(root, argument)),
                "nullable-string" => GetOptionalStringValue(root, argument) is { } stringValue ? QuoteLiteral(stringValue) : "null",
                "bool" => GetBooleanValue(root, argument) ? "true" : "false",
                "int32" => checked((int)GetInt64Value(root, argument)).ToString(CultureInfo.InvariantCulture),
                "int64" => GetInt64Value(root, argument).ToString(CultureInfo.InvariantCulture),
                "nullable-int64" => GetOptionalInt64Value(root, argument)?.ToString(CultureInfo.InvariantCulture) ?? "null",
                "enum" => CreateEnumValue(root, argument),
                "optional-string-array" => ExpressionSchemaUtilities.TryGetElement(root, argument.PropertyPath, out var optionalArray)
                    ? CreateStringArrayExpression(optionalArray)
                    : "Array.Empty<string>()",
                "string-array" => CreateStringArrayExpression(ExpressionSchemaUtilities.GetRequiredElement(root, argument.PropertyPath)),
                "int-string-dictionary" => CreateStringIntFrozenDictionaryExpression(ExpressionSchemaUtilities.GetRequiredElement(root, argument.PropertyPath)),
                "nullable-int-string-dictionary" => CreateOptionalStringIntFrozenDictionaryExpression(root, ExpressionSchemaUtilities.GetLeafPropertyName(argument.PropertyPath)),
                "string-long-dictionary" => CreateStringLongFrozenDictionaryExpression(ExpressionSchemaUtilities.GetRequiredElement(root, argument.PropertyPath)),
                "builder" => CreateBuilderArgument(profile, root, argument),
                _ => throw new InvalidOperationException($"Unsupported words-to-number schema argument kind '{argument.Kind}'.")
            };

        static string CreateObjectArgument(WordsToNumberProfileDefinition profile, JsonElement root, ExpressionArgumentSchema argument)
        {
            var objectRoot = string.IsNullOrWhiteSpace(argument.PropertyPath)
                ? root
                : ExpressionSchemaUtilities.GetRequiredElement(root, argument.PropertyPath);
            return argument.TypeName is null
                ? "new(" + CreateArguments(profile, objectRoot, argument.Arguments) + ")"
                : "new " + argument.TypeName + "(" + CreateArguments(profile, objectRoot, argument.Arguments) + ")";
        }

        static string CreateEnumValue(JsonElement root, ExpressionArgumentSchema argument)
        {
            if (argument.EnumType is null)
            {
                throw new InvalidOperationException("Enum arguments require an enum type.");
            }

            return argument.EnumType + "." + ToEnumMemberName(GetStringValue(root, argument));
        }

        static string CreateBuilderArgument(WordsToNumberProfileDefinition profile, JsonElement root, ExpressionArgumentSchema argument)
        {
            if (argument.Builder is null)
            {
                throw new InvalidOperationException("Builder arguments require a builder name.");
            }

            JsonElement builderRoot;
            if (string.IsNullOrWhiteSpace(argument.PropertyPath))
            {
                builderRoot = root;
            }
            else if (!ExpressionSchemaUtilities.TryGetElement(root, argument.PropertyPath, out builderRoot))
            {
                return argument.MissingValue switch
                {
                    "empty" when argument.Builder == "string-replacement-array" => "Array.Empty<StringReplacement>()",
                    "empty" when argument.Builder == "int-set" => "FrozenSet<int>.Empty",
                    _ => throw new InvalidOperationException($"Missing required property '{argument.PropertyPath}'.")
                };
            }

            return argument.Builder switch
            {
                "inverted-tens-token-array" => CreateInvertedTensTokenArrayExpression(builderRoot),
                "inverted-tens-ordinal-map" => CreateWordsOrdinalMapExpression(root, "InvertedTensWordsToNumberConverter.BuildOrdinalMap"),
                "scandinavian-ordinal-map" => CreateWordsOrdinalMapExpression(root, "ScandinavianCompoundWordsToNumberConverter.BuildOrdinalMap"),
                "suffix-scale-word-array" => CreateSuffixScaleWordArrayExpression(builderRoot),
                "prefixed-scale-word-array" => CreatePrefixedScaleWordArrayExpression(builderRoot),
                "prefixed-tens-rule-array" => CreatePrefixedTensRuleArrayExpression(builderRoot),
                "int-set" => CreateIntFrozenSetExpression(builderRoot),
                "string-replacement-array" => CreateStringReplacementArrayExpression(root, ExpressionSchemaUtilities.GetLeafPropertyName(argument.PropertyPath)),
                "greedy-compound-ordinal-map" => CreateGreedyCompoundOrdinalMapExpression(profile),
                "token-map-rules-object" => CreateTokenMapRulesExpression(root),
                _ => throw new InvalidOperationException($"Unsupported words-to-number builder '{argument.Builder}'.")
            };
        }

        static string CreateTokenMapRulesExpression(JsonElement root) =>
            "new() { " +
            "CardinalMap = " + CreateStringLongFrozenDictionaryExpression(ExpressionSchemaUtilities.GetRequiredElement(root, "cardinalMap")) + ", " +
            "ExactOrdinalMap = " + CreateOptionalStringIntFrozenDictionaryExpression(root, "ordinalMap") + ", " +
            "OrdinalScaleMap = " + (root.TryGetProperty("ordinalScaleMap", out var ordinalScaleMap) ? CreateStringLongFrozenDictionaryExpression(ordinalScaleMap) : "null") + ", " +
            "GluedOrdinalScaleSuffixes = " + (root.TryGetProperty("gluedOrdinalScaleSuffixes", out var gluedOrdinalScaleSuffixes) ? CreateStringLongFrozenDictionaryExpression(gluedOrdinalScaleSuffixes) : "null") + ", " +
            "CompositeScaleMap = " + (root.TryGetProperty("compositeScaleMap", out var compositeScaleMap) ? CreateStringLongFrozenDictionaryExpression(compositeScaleMap) : "null") + ", " +
            "NormalizationProfile = TokenMapNormalizationProfile." + ToEnumMemberName(GetRequiredString(root, "normalizationProfile")) + ", " +
            "NegativePrefixes = " + CreateOptionalStringArrayExpression(root, "negativePrefixes") + ", " +
            "NegativeSuffixes = " + CreateOptionalStringArrayExpression(root, "negativeSuffixes") + ", " +
            "OrdinalPrefixes = " + CreateOptionalStringArrayExpression(root, "ordinalPrefixes") + ", " +
            "IgnoredTokens = " + CreateOptionalStringArrayExpression(root, "ignoredTokens") + ", " +
            "LeadingTokenPrefixesToTrim = " + CreateOptionalStringArrayExpression(root, "leadingTokenPrefixesToTrim") + ", " +
            "MultiplierTokens = " + CreateOptionalStringArrayExpression(root, "multiplierTokens") + ", " +
            "TokenSuffixesToStrip = " + CreateOptionalStringArrayExpression(root, "tokenSuffixesToStrip") + ", " +
            "OrdinalAbbreviationSuffixes = " + CreateOptionalStringArrayExpression(root, "ordinalAbbreviationSuffixes") + ", " +
            "TeenSuffixTokens = " + CreateOptionalStringArrayExpression(root, "teenSuffixTokens") + ", " +
            "HundredSuffixTokens = " + CreateOptionalStringArrayExpression(root, "hundredSuffixTokens") + ", " +
            "AllowTerminalOrdinalToken = " + (GetBoolean(root, "allowTerminalOrdinalToken") ? "true" : "false") + ", " +
            "UseHundredMultiplier = " + (GetBoolean(root, "useHundredMultiplier") ? "true" : "false") + ", " +
            "AllowInvariantIntegerInput = " + (GetBoolean(root, "allowInvariantIntegerInput") ? "true" : "false") + ", " +
            "TeenBaseValue = " + (GetOptionalInt64(root, "teenBaseValue")?.ToString(CultureInfo.InvariantCulture) ?? "10") + ", " +
            "HundredSuffixValue = " + (GetOptionalInt64(root, "hundredSuffixValue")?.ToString(CultureInfo.InvariantCulture) ?? "100") + ", " +
            "UnitTokenMinValue = " + (GetOptionalInt64(root, "unitTokenMinValue")?.ToString(CultureInfo.InvariantCulture) ?? "1") + ", " +
            "UnitTokenMaxValue = " + (GetOptionalInt64(root, "unitTokenMaxValue")?.ToString(CultureInfo.InvariantCulture) ?? "9") + ", " +
            "HundredSuffixMinValue = " + (GetOptionalInt64(root, "hundredSuffixMinValue")?.ToString(CultureInfo.InvariantCulture) ?? "long.MaxValue") + ", " +
            "HundredSuffixMaxValue = " + (GetOptionalInt64(root, "hundredSuffixMaxValue")?.ToString(CultureInfo.InvariantCulture) ?? "long.MinValue") + ", " +
            "ScaleThreshold = " + (GetOptionalInt64(root, "scaleThreshold")?.ToString(CultureInfo.InvariantCulture) ?? "1000") +
            " }";

        static string CreateGreedyCompoundOrdinalMapExpression(WordsToNumberProfileDefinition profile)
        {
            if (profile.Root.TryGetProperty("ordinalMap", out var ordinalMap) && ordinalMap.ValueKind == JsonValueKind.Object)
            {
                return CreateStringIntFrozenDictionaryExpression(ordinalMap);
            }

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

            return "new Dictionary<string, int>(StringComparer.Ordinal).ToFrozenDictionary(StringComparer.Ordinal)";
        }

        static string GetStringValue(JsonElement root, ExpressionArgumentSchema argument)
        {
            if (ExpressionSchemaUtilities.TryGetOptionalString(root, argument.PropertyPath, out var value))
            {
                return value!;
            }

            if (argument.DefaultPropertyPath is not null && ExpressionSchemaUtilities.TryGetOptionalString(root, argument.DefaultPropertyPath, out var fallback))
            {
                return fallback!;
            }

            if (argument.DefaultValue is not null)
            {
                return argument.DefaultValue;
            }

            throw new InvalidOperationException($"Missing required string property '{argument.PropertyPath}'.");
        }

        static string? GetOptionalStringValue(JsonElement root, ExpressionArgumentSchema argument)
        {
            if (ExpressionSchemaUtilities.TryGetOptionalString(root, argument.PropertyPath, out var value))
            {
                return value;
            }

            if (argument.DefaultPropertyPath is not null && ExpressionSchemaUtilities.TryGetOptionalString(root, argument.DefaultPropertyPath, out var fallback))
            {
                return fallback;
            }

            return argument.DefaultValue;
        }

        static bool GetBooleanValue(JsonElement root, ExpressionArgumentSchema argument)
        {
            if (ExpressionSchemaUtilities.TryGetElement(root, argument.PropertyPath, out var value) &&
                value.ValueKind is JsonValueKind.True or JsonValueKind.False)
            {
                return value.GetBoolean();
            }

            return bool.TryParse(argument.DefaultValue, out var defaultValue) && defaultValue;
        }

        static long GetInt64Value(JsonElement root, ExpressionArgumentSchema argument)
        {
            if (ExpressionSchemaUtilities.TryGetElement(root, argument.PropertyPath, out var value) &&
                value.ValueKind == JsonValueKind.Number &&
                value.TryGetInt64(out var number))
            {
                return number;
            }

            if (argument.DefaultValue is not null && long.TryParse(argument.DefaultValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out number))
            {
                return number;
            }

            throw new InvalidOperationException($"Missing required integer property '{argument.PropertyPath}'.");
        }

        static long? GetOptionalInt64Value(JsonElement root, ExpressionArgumentSchema argument)
        {
            if (ExpressionSchemaUtilities.TryGetElement(root, argument.PropertyPath, out var value) &&
                value.ValueKind == JsonValueKind.Number &&
                value.TryGetInt64(out var number))
            {
                return number;
            }

            return argument.DefaultValue is not null &&
                   long.TryParse(argument.DefaultValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out number)
                ? number
                : null;
        }
    }
}

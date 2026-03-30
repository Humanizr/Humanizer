using System;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.Json;

namespace Humanizer.SourceGenerators;

public sealed partial class HumanizerSourceGenerator
{
    static class NumberToWordsSchemaExpressionFactory
    {
        public static string Create(
            NumberToWordsProfileDefinition profile,
            ImmutableDictionary<string, ExpressionSchema> schemas,
            bool useCultureParameter)
        {
            if (!schemas.TryGetValue(profile.Engine, out var schema))
            {
                return CreateConventionalNumberToWordsExpression(profile.Engine, useCultureParameter);
            }

            var runtimeType = schema.RuntimeType ?? GetConventionalNumberToWordsTypeName(profile.Engine);
            return "new " + runtimeType + "(" + CreateArguments(profile.Root, schema.Arguments, useCultureParameter) + ")";
        }

        static string CreateArguments(
            JsonElement root,
            ImmutableArray<ExpressionArgumentSchema> arguments,
            bool useCultureParameter) =>
            string.Join(", ", arguments.Select(argument => CreateArgument(root, argument, useCultureParameter)));

        static string CreateArgument(JsonElement root, ExpressionArgumentSchema argument, bool useCultureParameter) =>
            argument.Kind switch
            {
                "profile-object" => CreateObjectArgument(root, argument, useCultureParameter),
                "culture" => useCultureParameter ? "culture" : "CultureInfo.InvariantCulture",
                "string" => QuoteLiteral(GetStringValue(root, argument)),
                "nullable-string" => GetOptionalStringValue(root, argument) is { } stringValue ? QuoteLiteral(stringValue) : "null",
                "bool" => GetBooleanValue(root, argument) ? "true" : "false",
                "int64" => GetInt64Value(root, argument).ToString(CultureInfo.InvariantCulture),
                "nullable-int64" => GetOptionalInt64Value(root, argument)?.ToString(CultureInfo.InvariantCulture) ?? "null",
                "int32" => checked((int)GetInt64Value(root, argument)).ToString(CultureInfo.InvariantCulture),
                "enum" => CreateEnumValue(root, argument),
                "string-array" => CreateStringArrayExpression(ExpressionSchemaUtilities.GetRequiredElement(root, argument.PropertyPath)),
                "optional-string-array" => ExpressionSchemaUtilities.TryGetElement(root, argument.PropertyPath, out var optionalArray)
                    ? CreateStringArrayExpression(optionalArray)
                    : "Array.Empty<string>()",
                "int-string-dictionary" => CreateStringIntFrozenDictionaryExpression(ExpressionSchemaUtilities.GetRequiredElement(root, argument.PropertyPath)),
                "nullable-int-string-dictionary" => CreateNullableIntStringDictionaryValue(root, argument),
                "string-string-dictionary" => CreateStringStringFrozenDictionaryExpression(ExpressionSchemaUtilities.GetRequiredElement(root, argument.PropertyPath)),
                "nullable-string-string-dictionary" => ExpressionSchemaUtilities.TryGetElement(root, argument.PropertyPath, out var optionalStringDictionary)
                    ? CreateOptionalStringStringFrozenDictionaryExpression(root, ExpressionSchemaUtilities.GetLeafPropertyName(argument.PropertyPath))
                    : argument.MissingValue == "empty"
                        ? "FrozenDictionary<string, string>.Empty"
                        : "null",
                "char-string-dictionary" => CreateCharStringFrozenDictionaryExpression(ExpressionSchemaUtilities.GetRequiredElement(root, argument.PropertyPath)),
                "nullable-char-string-dictionary" => ExpressionSchemaUtilities.TryGetElement(root, argument.PropertyPath, out var optionalCharDictionary)
                    ? CreateCharStringFrozenDictionaryExpression(optionalCharDictionary)
                    : "null",
                "nullable-int-set" => CreateNullableIntSetValue(root, argument),
                "builder" => CreateBuilderArgument(root, argument),
                _ => throw new InvalidOperationException($"Unsupported number-to-words schema argument kind '{argument.Kind}'.")
            };

        static string CreateObjectArgument(JsonElement root, ExpressionArgumentSchema argument, bool useCultureParameter)
        {
            var objectRoot = string.IsNullOrWhiteSpace(argument.PropertyPath)
                ? root
                : ExpressionSchemaUtilities.GetRequiredElement(root, argument.PropertyPath);
            return argument.TypeName is null
                ? "new(" + CreateArguments(objectRoot, argument.Arguments, useCultureParameter) + ")"
                : "new " + argument.TypeName + "(" + CreateArguments(objectRoot, argument.Arguments, useCultureParameter) + ")";
        }

        static string CreateEnumValue(JsonElement root, ExpressionArgumentSchema argument)
        {
            if (argument.EnumType is null)
            {
                throw new InvalidOperationException("Enum arguments require an enum type.");
            }

            return argument.EnumType + "." + ToEnumMemberName(GetStringValue(root, argument));
        }

        static string CreateNullableIntStringDictionaryValue(JsonElement root, ExpressionArgumentSchema argument)
        {
            if (ExpressionSchemaUtilities.TryGetElement(root, argument.PropertyPath, out _))
            {
                return CreateNullableIntStringFrozenDictionaryExpression(root, ExpressionSchemaUtilities.GetLeafPropertyName(argument.PropertyPath));
            }

            return argument.MissingValue switch
            {
                "empty" => "FrozenDictionary<int, string>.Empty",
                _ => "null"
            };
        }

        static string CreateNullableIntSetValue(JsonElement root, ExpressionArgumentSchema argument)
        {
            if (ExpressionSchemaUtilities.TryGetElement(root, argument.PropertyPath, out _))
            {
                return CreateNullableIntFrozenSetExpression(root, ExpressionSchemaUtilities.GetLeafPropertyName(argument.PropertyPath));
            }

            return argument.MissingValue switch
            {
                "empty" => "FrozenSet<int>.Empty",
                _ => "null"
            };
        }

        static string CreateBuilderArgument(JsonElement root, ExpressionArgumentSchema argument)
        {
            if (argument.Builder is null)
            {
                throw new InvalidOperationException("Builder arguments require a builder name.");
            }

            var builderRoot = string.IsNullOrWhiteSpace(argument.PropertyPath)
                ? root
                : ExpressionSchemaUtilities.GetRequiredElement(root, argument.PropertyPath);

            return argument.Builder switch
            {
                "turkic-scale-array" => CreateTurkicScaleArrayExpression(builderRoot),
                "malay-scale-array" => CreateMalayScaleArrayExpression(builderRoot),
                "linking-scale-array" => CreateLinkingScaleArrayExpression(builderRoot),
                "linking-suffix-rule-array" => CreateLinkingSuffixRuleArrayExpression(builderRoot),
                "agglutinative-ordinal-scale-array" => CreateAgglutinativeOrdinalScaleArrayExpression(builderRoot),
                "contextual-decimal-scale-array" => CreateContextualDecimalScaleArrayExpression(builderRoot),
                "english-family-scale-array" => CreateEnglishFamilyScaleArrayExpression(builderRoot),
                "joined-scale-array" => CreateJoinedScaleArrayExpression(builderRoot),
                "west-slavic-scale-forms" => CreateWestSlavicScaleFormsExpression(builderRoot),
                "east-asian-scale-array" => throw new InvalidOperationException("Unexpected east-asian scale builder."),
                "east-slavic-scale-array" => CreateEastSlavicScaleArrayExpression(builderRoot),
                "east-slavic-gender-ending" => CreateEastSlavicGenderEndingExpression(builderRoot),
                "pluralized-scale-array" => CreatePluralizedScaleArrayExpression(builderRoot),
                "ordinal-prefix-scale-array" => CreateOrdinalPrefixScaleArrayExpression(builderRoot),
                "inverted-tens-scale-array" => CreateInvertedTensScaleArrayExpression(builderRoot),
                "south-slavic-scale-array" => CreateSouthSlavicScaleArrayExpression(builderRoot),
                "scandinavian-scale-array" => CreateScandinavianScaleArrayExpression(builderRoot),
                "german-family-scale-array" => CreateGermanFamilyScaleArrayExpression(builderRoot),
                "conjoined-gendered-scale-array" => CreateConjoinedGenderedScaleArrayExpression(builderRoot),
                "segmented-scale-array" => CreateSegmentedScaleArrayExpression(builderRoot),
                "terminal-ordinal-scale-array" => CreateTerminalOrdinalScaleArrayExpression(builderRoot),
                "construct-state-scale-array" => CreateConstructStateScaleArrayExpression(builderRoot),
                "hyphenated-scale" => CreateHyphenatedScaleExpression(builderRoot),
                "hyphenated-scale-array" => CreateHyphenatedScaleArrayExpression(builderRoot),
                "dual-form-scale" => CreateDualFormScaleExpression(builderRoot),
                "triad-scale-array" => CreateTriadScaleArrayExpression(builderRoot),
                "gendered-scale-ordinal-array" => CreateGenderedScaleOrdinalArrayExpression(builderRoot),
                "long-scale-word-array" => CreateLongScaleWordArrayExpression(builderRoot),
                _ => throw new InvalidOperationException($"Unsupported number-to-words builder '{argument.Builder}'.")
            };
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

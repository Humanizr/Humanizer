using System;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.Json;

namespace Humanizer.SourceGenerators;

public sealed partial class HumanizerSourceGenerator
{
    /// <summary>
    /// Binds resolved locale YAML for number-to-words engines onto the typed structural contracts
    /// declared in <see cref="EngineContractCatalog"/> and emits direct constructor expressions for
    /// the generated profile catalog.
    /// </summary>
    static class NumberToWordsEngineContractFactory
    {
        /// <summary>
        /// Resolves a locale's <c>numberToWords</c> YAML block to a concrete runtime-converter
        /// constructor expression. The important split is:
        /// <list type="bullet">
        /// <item><description>structural engines are described in <see cref="EngineContractCatalog"/> and emitted member-by-member;</description></item>
        /// <item><description>legacy conventional engines still fall back to type-name construction until they are generalized.</description></item>
        /// </list>
        /// </summary>
        public static string Create(
            NumberToWordsProfileDefinition profile,
            ImmutableDictionary<string, EngineContract> contracts,
            bool useCultureParameter)
        {
            if (!contracts.TryGetValue(profile.Engine, out var contract))
            {
                return CreateConventionalNumberToWordsExpression(profile.Engine, useCultureParameter);
            }

            var runtimeType = contract.RuntimeType ?? GetConventionalNumberToWordsTypeName(profile.Engine);
            return "new " + runtimeType + "(" + CreateConstructorValues(profile.Root, contract.Members, useCultureParameter) + ")";
        }

        /// <summary>
        /// Walks the contract members in declaration order so the generated constructor arguments
        /// stay aligned with the runtime profile type. The generator resolves inheritance and YAML
        /// merges before this point, so this method can treat the <see cref="JsonElement"/> as the
        /// fully materialized locale payload.
        /// </summary>
        static string CreateConstructorValues(
            JsonElement root,
            ImmutableArray<EngineContractMember> members,
            bool useCultureParameter) =>
            string.Join(", ", members.Select(member => CreateMemberValue(root, member, useCultureParameter)));

        /// <summary>
        /// Converts one contract member into its generated C# literal or nested-constructor form.
        /// This is the bridge between the YAML authoring DSL and the strongly typed runtime profile
        /// objects: every supported <c>kind</c> is emitted as a direct expression with no runtime
        /// parsing left behind.
        /// </summary>
        static string CreateMemberValue(JsonElement root, EngineContractMember member, bool useCultureParameter) =>
            member.Kind switch
            {
                "profile-object" => CreateObjectValue(root, member, useCultureParameter),
                "culture" => useCultureParameter ? "culture" : "CultureInfo.InvariantCulture",
                "string" => QuoteLiteral(GetStringValue(root, member)),
                "nullable-string" => GetOptionalStringValue(root, member) is { } stringValue ? QuoteLiteral(stringValue) : "null",
                "bool" => GetBooleanValue(root, member) ? "true" : "false",
                "int64" => GetInt64Value(root, member).ToString(CultureInfo.InvariantCulture),
                "nullable-int64" => GetOptionalInt64Value(root, member)?.ToString(CultureInfo.InvariantCulture) ?? "null",
                "int32" => checked((int)GetInt64Value(root, member)).ToString(CultureInfo.InvariantCulture),
                "enum" => CreateEnumValue(root, member),
                "string-array" => CreateStringArrayExpression(EngineContractUtilities.GetRequiredElement(root, member.SourcePath)),
                "optional-string-array" => EngineContractUtilities.TryGetElement(root, member.SourcePath, out var optionalArray)
                    ? CreateStringArrayExpression(optionalArray)
                    : "Array.Empty<string>()",
                "int-string-dictionary" => CreateStringIntFrozenDictionaryExpression(EngineContractUtilities.GetRequiredElement(root, member.SourcePath)),
                "nullable-int-string-dictionary" => CreateNullableIntStringDictionaryValue(root, member),
                "string-string-dictionary" => CreateStringStringFrozenDictionaryExpression(EngineContractUtilities.GetRequiredElement(root, member.SourcePath)),
                "nullable-string-string-dictionary" => EngineContractUtilities.TryGetElement(root, member.SourcePath, out var optionalStringDictionary)
                    ? CreateOptionalStringStringFrozenDictionaryExpression(root, EngineContractUtilities.GetLeafPropertyName(member.SourcePath))
                    : member.MissingValue == "empty"
                        ? "FrozenDictionary<string, string>.Empty"
                        : "null",
                "char-string-dictionary" => CreateCharStringFrozenDictionaryExpression(EngineContractUtilities.GetRequiredElement(root, member.SourcePath)),
                "nullable-char-string-dictionary" => EngineContractUtilities.TryGetElement(root, member.SourcePath, out var optionalCharDictionary)
                    ? CreateCharStringFrozenDictionaryExpression(optionalCharDictionary)
                    : "null",
                "nullable-int-set" => CreateNullableIntSetValue(root, member),
                "builder" => CreateBuilderValue(root, member),
                _ => throw new InvalidOperationException($"Unsupported number-to-words contract member kind '{member.Kind}'.")
            };

        static string CreateObjectValue(JsonElement root, EngineContractMember member, bool useCultureParameter)
        {
            var objectRoot = string.IsNullOrWhiteSpace(member.SourcePath)
                ? root
                : EngineContractUtilities.GetRequiredElement(root, member.SourcePath);

            // Nested profile objects let the YAML stay grouped by meaning while the generated code
            // still calls an explicit runtime constructor shape. This keeps the authoring surface
            // readable without paying any runtime mapping cost.
            return member.TypeName is null
                ? "new(" + CreateConstructorValues(objectRoot, member.Members, useCultureParameter) + ")"
                : "new " + member.TypeName + "(" + CreateConstructorValues(objectRoot, member.Members, useCultureParameter) + ")";
        }

        static string CreateEnumValue(JsonElement root, EngineContractMember member)
        {
            if (member.EnumType is null)
            {
                throw new InvalidOperationException("Enum members require an enum type.");
            }

            return member.EnumType + "." + ToEnumMemberName(GetStringValue(root, member));
        }

        static string CreateNullableIntStringDictionaryValue(JsonElement root, EngineContractMember member)
        {
            if (EngineContractUtilities.TryGetElement(root, member.SourcePath, out _))
            {
                return CreateNullableIntStringFrozenDictionaryExpression(root, EngineContractUtilities.GetLeafPropertyName(member.SourcePath));
            }

            return member.MissingValue switch
            {
                "empty" => "FrozenDictionary<int, string>.Empty",
                _ => "null"
            };
        }

        static string CreateNullableIntSetValue(JsonElement root, EngineContractMember member)
        {
            if (EngineContractUtilities.TryGetElement(root, member.SourcePath, out _))
            {
                return CreateNullableIntFrozenSetExpression(root, EngineContractUtilities.GetLeafPropertyName(member.SourcePath));
            }

            return member.MissingValue switch
            {
                "empty" => "FrozenSet<int>.Empty",
                _ => "null"
            };
        }

        static string CreateBuilderValue(JsonElement root, EngineContractMember member)
        {
            if (member.Builder is null)
            {
                throw new InvalidOperationException("Builder members require a builder name.");
            }

            var builderRoot = string.IsNullOrWhiteSpace(member.SourcePath)
                ? root
                : EngineContractUtilities.GetRequiredElement(root, member.SourcePath);

            // Builders exist for repeated structured rows such as scale tables, suffix rules, and
            // gender-ending records. Keeping these explicit makes the generated output easier to
            // audit than trying to encode a second generic mini-language for nested collections.
            return member.Builder switch
            {
                "harmony-ordinal-scale-array" => CreateHarmonyOrdinalScaleArrayExpression(builderRoot),
                "contracted-one-scale-array" => CreateContractedOneScaleArrayExpression(builderRoot),
                "linking-scale-array" => CreateLinkingScaleArrayExpression(builderRoot),
                "linking-suffix-rule-array" => CreateLinkingSuffixRuleArrayExpression(builderRoot),
                "agglutinative-ordinal-scale-array" => CreateAgglutinativeOrdinalScaleArrayExpression(builderRoot),
                "contextual-decimal-scale-array" => CreateContextualDecimalScaleArrayExpression(builderRoot),
                "conjunctional-scale-array" => CreateConjunctionalScaleArrayExpression(builderRoot),
                "joined-scale-array" => CreateJoinedScaleArrayExpression(builderRoot),
                "west-slavic-scale-forms" => CreateWestSlavicScaleFormsExpression(builderRoot),
                "east-asian-scale-array" => throw new InvalidOperationException("Unexpected east-asian scale builder."),
                "east-slavic-scale-array" => CreateEastSlavicScaleArrayExpression(builderRoot),
                "east-slavic-gender-ending" => CreateEastSlavicGenderEndingExpression(builderRoot),
                "pluralized-scale-array" => CreatePluralizedScaleArrayExpression(builderRoot),
                "ordinal-prefix-scale-array" => CreateOrdinalPrefixScaleArrayExpression(builderRoot),
                "inverted-tens-scale-array" => CreateInvertedTensScaleArrayExpression(builderRoot),
                "south-slavic-scale-array" => CreateSouthSlavicScaleArrayExpression(builderRoot),
                "scandinavian-scale-array" => CreateScaleStrategyScaleArrayExpression(builderRoot),
                "unit-leading-compound-scale-array" => CreateUnitLeadingCompoundScaleArrayExpression(builderRoot),
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
                _ => throw new InvalidOperationException($"Unsupported number-to-words builder '{member.Builder}'.")
            };
        }

        static string GetStringValue(JsonElement root, EngineContractMember member)
        {
            if (EngineContractUtilities.TryGetOptionalString(root, member.SourcePath, out var value))
            {
                return value!;
            }

            if (member.FallbackSourcePath is not null && EngineContractUtilities.TryGetOptionalString(root, member.FallbackSourcePath, out var fallback))
            {
                return fallback!;
            }

            if (member.DefaultValue is not null)
            {
                return member.DefaultValue;
            }

            throw new InvalidOperationException($"Missing required string property '{member.SourcePath}'.");
        }

        static string? GetOptionalStringValue(JsonElement root, EngineContractMember member)
        {
            if (EngineContractUtilities.TryGetOptionalString(root, member.SourcePath, out var value))
            {
                return value;
            }

            if (member.FallbackSourcePath is not null && EngineContractUtilities.TryGetOptionalString(root, member.FallbackSourcePath, out var fallback))
            {
                return fallback;
            }

            return member.DefaultValue;
        }

        static bool GetBooleanValue(JsonElement root, EngineContractMember member)
        {
            if (EngineContractUtilities.TryGetElement(root, member.SourcePath, out var value) &&
                value.ValueKind is JsonValueKind.True or JsonValueKind.False)
            {
                return value.GetBoolean();
            }

            return bool.TryParse(member.DefaultValue, out var defaultValue) && defaultValue;
        }

        static long GetInt64Value(JsonElement root, EngineContractMember member)
        {
            if (EngineContractUtilities.TryGetElement(root, member.SourcePath, out var value) &&
                value.ValueKind == JsonValueKind.Number &&
                value.TryGetInt64(out var number))
            {
                return number;
            }

            if (member.DefaultValue is not null && long.TryParse(member.DefaultValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out number))
            {
                return number;
            }

            throw new InvalidOperationException($"Missing required integer property '{member.SourcePath}'.");
        }

        static long? GetOptionalInt64Value(JsonElement root, EngineContractMember member)
        {
            if (EngineContractUtilities.TryGetElement(root, member.SourcePath, out var value) &&
                value.ValueKind == JsonValueKind.Number &&
                value.TryGetInt64(out var number))
            {
                return number;
            }

            return member.DefaultValue is not null &&
                   long.TryParse(member.DefaultValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out number)
                ? number
                : null;
        }

    }
}

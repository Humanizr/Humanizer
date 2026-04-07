using System.Collections.Immutable;
using System.Linq;
using System.Text.Json;

namespace Humanizer.SourceGenerators;

public sealed partial class HumanizerSourceGenerator
{
    /// <summary>
    /// Emits constructor expressions for generated clock-notation profiles by binding resolved
    /// locale YAML onto the typed structural contracts in <see cref="EngineContractCatalog"/>.
    /// </summary>
    static class TimeOnlyToClockNotationEngineContractFactory
    {
        /// <summary>
        /// Resolves a locale clock-notation definition to a concrete runtime constructor
        /// expression. Fully generalized clock engines flow through the typed contract catalog,
        /// while accepted residual leaf converters still use conventional type-name construction.
        /// </summary>
        public static string Create(
            TimeOnlyToClockNotationProfileDefinition profile,
            ImmutableDictionary<string, EngineContract> contracts)
        {
            if (!contracts.TryGetValue(profile.Engine, out var contract))
            {
                return CreateConventionalTimeOnlyToClockNotationExpression(profile.Engine);
            }

            var runtimeType = contract.RuntimeType ?? GetClockNotationEngineTypeName(profile.Engine);
            return CreateParameterizedExpression(runtimeType, CreateConstructorValues(profile.Root, contract.Members));
        }

        /// <summary>
        /// Clock-notation contracts are intentionally shallow. Most engines expose a single profile
        /// object, so this helper preserves that compact generated shape instead of wrapping
        /// everything in an unnecessary extra tuple constructor.
        /// </summary>
        static string CreateConstructorValues(JsonElement root, ImmutableArray<EngineContractMember> members) =>
            members.Length == 1 && members[0].Kind == "profile-object"
                ? CreateMemberValue(root, members[0])
                : "new(" + string.Join(", ", members.Select(member => CreateMemberValue(root, member))) + ")";

        static string CreateMemberValue(JsonElement root, EngineContractMember member) =>
            member.Kind switch
            {
                "profile-object" => CreateObjectValue(root, member),
                "string" => QuoteLiteral(GetStringValue(root, member)),
                "enum" => CreateEnumValue(root, member),
                "bool" => GetBooleanValue(root, member) ? "true" : "false",
                _ => throw new InvalidOperationException($"Unsupported clock-notation contract member kind '{member.Kind}'.")
            };

        static string CreateEnumValue(JsonElement root, EngineContractMember member)
        {
            if (member.EnumType is null)
            {
                throw new InvalidOperationException("Enum members require an enum type.");
            }

            return member.EnumType + "." + ToEnumMemberName(GetStringValue(root, member));
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

        static string CreateObjectValue(JsonElement root, EngineContractMember member)
        {
            var objectRoot = string.IsNullOrWhiteSpace(member.SourcePath)
                ? root
                : EngineContractUtilities.GetRequiredElement(root, member.SourcePath);

            // Nested objects keep the YAML organized by concept while still emitting a plain,
            // parse-free immutable runtime profile.
            return member.TypeName is null
                ? "new(" + string.Join(", ", member.Members.Select(nested => CreateMemberValue(objectRoot, nested))) + ")"
                : "new " + member.TypeName + "(" + string.Join(", ", member.Members.Select(nested => CreateMemberValue(objectRoot, nested))) + ")";
        }

        static string GetStringValue(JsonElement root, EngineContractMember member)
        {
            if (EngineContractUtilities.TryGetOptionalString(root, member.SourcePath, out var value))
            {
                return value!;
            }

            if (member.FallbackSourcePath is not null &&
                EngineContractUtilities.TryGetOptionalString(root, member.FallbackSourcePath, out var fallback))
            {
                return fallback!;
            }

            if (member.DefaultValue is not null)
            {
                return member.DefaultValue;
            }

            throw new InvalidOperationException($"Missing required string property '{member.SourcePath}'.");
        }
    }
}
using System.Collections.Immutable;

namespace Humanizer.SourceGenerators;

public sealed partial class HumanizerSourceGenerator
{
    /// <summary>
    /// Describes one structural generator contract for a profile engine.
    /// A contract answers two questions for the profile catalogs:
    /// which runtime type should be instantiated and which ordered constructor values
    /// should be materialized from locale-owned YAML data.
    /// </summary>
    sealed class EngineContract(string engineName, string? runtimeType, ImmutableArray<EngineContractMember> members)
    {
        /// <summary>
        /// Gets the structural engine key resolved from locale YAML.
        /// This is intentionally a structural identifier such as
        /// <c>billion-strategy</c>, not a locale code.
        /// </summary>
        public string EngineName { get; } = engineName;

        /// <summary>
        /// Gets the concrete runtime type to instantiate when the schema does not
        /// follow the conventional type-name mapping for the engine key.
        /// </summary>
        public string? RuntimeType { get; } = runtimeType;

        /// <summary>
        /// Gets the ordered member descriptors that the expression factories
        /// convert into generated constructor values.
        /// </summary>
        public ImmutableArray<EngineContractMember> Members { get; } = members;
    }

    /// <summary>
    /// Describes one constructor-bound member, nested object, or builder invocation
    /// inside a structural contract. These descriptors are generator implementation
    /// code, not user-authored data, which is why the model keeps explicit
    /// plumbing details such as property paths and fallback rules.
    /// </summary>
    sealed class EngineContractMember(
        string kind,
        string? sourcePath,
        string? typeName,
        string? enumType,
        string? builder,
        string? defaultValue,
        string? fallbackSourcePath,
        string? missingValue,
        ImmutableArray<EngineContractMember> members)
    {
        /// <summary>
        /// Gets the structural member kind understood by the expression factories,
        /// for example <c>string</c>, <c>enum</c>, <c>profile-object</c>, or
        /// <c>builder</c>.
        /// </summary>
        public string Kind { get; } = kind;

        /// <summary>
        /// Gets the YAML/JSON property path read from the profile payload when this
        /// argument is materialized.
        /// </summary>
        public string? SourcePath { get; } = sourcePath;

        /// <summary>
        /// Gets the explicit runtime type to construct for nested profile objects
        /// when anonymous tuple-style construction is not sufficient.
        /// </summary>
        public string? TypeName { get; } = typeName;

        /// <summary>
        /// Gets the enum type used when <see cref="Kind"/> is <c>enum</c>.
        /// </summary>
        public string? EnumType { get; } = enumType;

        /// <summary>
        /// Gets the shared builder name used to materialize complex arrays or
        /// helper objects from structured payloads.
        /// </summary>
        public string? Builder { get; } = builder;

        /// <summary>
        /// Gets the literal fallback emitted when the source payload omits the
        /// value and no fallback property is configured.
        /// </summary>
        public string? DefaultValue { get; } = defaultValue;

        /// <summary>
        /// Gets the alternate property path consulted when the primary path is
        /// absent.
        /// </summary>
        public string? FallbackSourcePath { get; } = fallbackSourcePath;

        /// <summary>
        /// Gets the missing-value policy token consumed by the expression factories,
        /// typically to distinguish between <c>null</c>, empty collections, and
        /// special derived fallbacks.
        /// </summary>
        public string? MissingValue { get; } = missingValue;

        /// <summary>
        /// Gets nested member descriptors for object and builder-style members.
        /// </summary>
        public ImmutableArray<EngineContractMember> Members { get; } = members;
    }
}
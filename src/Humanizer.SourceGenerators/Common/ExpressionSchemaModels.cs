using System.Collections.Immutable;

namespace Humanizer.SourceGenerators;

public sealed partial class HumanizerSourceGenerator
{
    sealed class ExpressionSchema(string schemaName, string? runtimeType, ImmutableArray<ExpressionArgumentSchema> arguments)
    {
        public string SchemaName { get; } = schemaName;
        public string? RuntimeType { get; } = runtimeType;
        public ImmutableArray<ExpressionArgumentSchema> Arguments { get; } = arguments;
    }

    sealed class ExpressionArgumentSchema(
        string kind,
        string? propertyPath,
        string? typeName,
        string? enumType,
        string? builder,
        string? defaultValue,
        string? defaultPropertyPath,
        string? missingValue,
        ImmutableArray<ExpressionArgumentSchema> arguments)
    {
        public string Kind { get; } = kind;
        public string? PropertyPath { get; } = propertyPath;
        public string? TypeName { get; } = typeName;
        public string? EnumType { get; } = enumType;
        public string? Builder { get; } = builder;
        public string? DefaultValue { get; } = defaultValue;
        public string? DefaultPropertyPath { get; } = defaultPropertyPath;
        public string? MissingValue { get; } = missingValue;
        public ImmutableArray<ExpressionArgumentSchema> Arguments { get; } = arguments;
    }
}

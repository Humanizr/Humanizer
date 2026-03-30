using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.Json;

namespace Humanizer.SourceGenerators;

public sealed partial class HumanizerSourceGenerator
{
    static class ExpressionSchemaLoader
    {
        public static ImmutableDictionary<string, ExpressionSchema> Load(ImmutableArray<JsonSchemaFile?> files)
        {
            var schemas = ImmutableDictionary.CreateBuilder<string, ExpressionSchema>(StringComparer.Ordinal);

            foreach (var file in files)
            {
                if (file is null)
                {
                    continue;
                }

                using var document = JsonDocument.Parse(file.FileText);
                var root = document.RootElement;
                var runtimeType = GetOptionalString(root, "runtimeType");
                var arguments = root.TryGetProperty("arguments", out var argumentsElement)
                    ? ParseArguments(argumentsElement)
                    : ImmutableArray<ExpressionArgumentSchema>.Empty;
                schemas[file.SchemaName] = new ExpressionSchema(file.SchemaName, runtimeType, arguments);
            }

            return schemas.ToImmutable();
        }

        static ImmutableArray<ExpressionArgumentSchema> ParseArguments(JsonElement argumentsElement)
        {
            if (argumentsElement.ValueKind != JsonValueKind.Array)
            {
                throw new InvalidOperationException("Schema arguments must be a JSON array.");
            }

            var arguments = ImmutableArray.CreateBuilder<ExpressionArgumentSchema>();
            foreach (var item in argumentsElement.EnumerateArray())
            {
                arguments.Add(ParseArgument(item));
            }

            return arguments.ToImmutable();
        }

        static ExpressionArgumentSchema ParseArgument(JsonElement argumentElement)
        {
            if (argumentElement.ValueKind != JsonValueKind.Object)
            {
                throw new InvalidOperationException("Schema argument must be a JSON object.");
            }

            var kind = GetRequiredString(argumentElement, "kind");
            var nestedArguments = argumentElement.TryGetProperty("arguments", out var nestedArgumentsElement)
                ? ParseArguments(nestedArgumentsElement)
                : ImmutableArray<ExpressionArgumentSchema>.Empty;

            return new ExpressionArgumentSchema(
                kind,
                GetOptionalString(argumentElement, "property"),
                GetOptionalString(argumentElement, "typeName"),
                GetOptionalString(argumentElement, "enumType"),
                GetOptionalString(argumentElement, "builder"),
                GetOptionalString(argumentElement, "defaultValue"),
                GetOptionalString(argumentElement, "defaultProperty"),
                GetOptionalString(argumentElement, "missingValue"),
                nestedArguments);
        }
    }
}

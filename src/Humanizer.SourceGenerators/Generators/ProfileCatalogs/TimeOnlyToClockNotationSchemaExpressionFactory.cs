using System.Collections.Immutable;
using System.Linq;
using System.Text.Json;

namespace Humanizer.SourceGenerators;

public sealed partial class HumanizerSourceGenerator
{
    static class TimeOnlyToClockNotationSchemaExpressionFactory
    {
        public static string Create(
            TimeOnlyToClockNotationProfileDefinition profile,
            ImmutableDictionary<string, ExpressionSchema> schemas)
        {
            if (!schemas.TryGetValue(profile.Engine, out var schema))
            {
                return CreateConventionalTimeOnlyToClockNotationExpression(profile.Engine);
            }

            var runtimeType = schema.RuntimeType ?? GetClockNotationEngineTypeName(profile.Engine);
            return CreateParameterizedExpression(runtimeType, CreateArguments(profile.Root, schema.Arguments));
        }

        static string CreateArguments(JsonElement root, ImmutableArray<ExpressionArgumentSchema> arguments) =>
            arguments.Length == 1 && arguments[0].Kind == "profile-object"
                ? CreateArgument(root, arguments[0])
                : "new(" + string.Join(", ", arguments.Select(argument => CreateArgument(root, argument))) + ")";

        static string CreateArgument(JsonElement root, ExpressionArgumentSchema argument) =>
            argument.Kind switch
            {
                "profile-object" => CreateObjectArgument(root, argument),
                "string" => QuoteLiteral(GetStringValue(root, argument)),
                _ => throw new InvalidOperationException($"Unsupported clock-notation schema argument kind '{argument.Kind}'.")
            };

        static string CreateObjectArgument(JsonElement root, ExpressionArgumentSchema argument)
        {
            var objectRoot = string.IsNullOrWhiteSpace(argument.PropertyPath)
                ? root
                : ExpressionSchemaUtilities.GetRequiredElement(root, argument.PropertyPath);
            return argument.TypeName is null
                ? "new(" + string.Join(", ", argument.Arguments.Select(nested => CreateArgument(objectRoot, nested))) + ")"
                : "new " + argument.TypeName + "(" + string.Join(", ", argument.Arguments.Select(nested => CreateArgument(objectRoot, nested))) + ")";
        }

        static string GetStringValue(JsonElement root, ExpressionArgumentSchema argument)
        {
            if (ExpressionSchemaUtilities.TryGetOptionalString(root, argument.PropertyPath, out var value))
            {
                return value!;
            }

            if (argument.DefaultPropertyPath is not null &&
                ExpressionSchemaUtilities.TryGetOptionalString(root, argument.DefaultPropertyPath, out var fallback))
            {
                return fallback!;
            }

            if (argument.DefaultValue is not null)
            {
                return argument.DefaultValue;
            }

            throw new InvalidOperationException($"Missing required string property '{argument.PropertyPath}'.");
        }
    }
}

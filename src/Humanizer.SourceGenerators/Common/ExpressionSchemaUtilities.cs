using System;
using System.Text.Json;

namespace Humanizer.SourceGenerators;

public sealed partial class HumanizerSourceGenerator
{
    static class ExpressionSchemaUtilities
    {
        public static JsonElement GetRequiredElement(JsonElement root, string? propertyPath) =>
            TryGetElement(root, propertyPath, out var element)
                ? element
                : throw new InvalidOperationException($"Missing required property '{propertyPath}'.");

        public static bool TryGetElement(JsonElement root, string? propertyPath, out JsonElement element)
        {
            element = root;
            if (string.IsNullOrWhiteSpace(propertyPath))
            {
                return true;
            }

            foreach (var segment in propertyPath!.Split('.'))
            {
                if (!element.TryGetProperty(segment, out element))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool TryGetOptionalString(JsonElement root, string? propertyPath, out string? value)
        {
            if (TryGetElement(root, propertyPath, out var property) && property.ValueKind == JsonValueKind.String)
            {
                value = property.GetString();
                return true;
            }

            value = null;
            return false;
        }

        public static string GetLeafPropertyName(string? propertyPath)
        {
            if (string.IsNullOrWhiteSpace(propertyPath))
            {
                throw new InvalidOperationException("A property path is required.");
            }

            var path = propertyPath!;
            var lastSeparator = path.LastIndexOf('.');
            return lastSeparator >= 0 ? path.Substring(lastSeparator + 1) : path;
        }
    }
}

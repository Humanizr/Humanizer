using System.Text.Json;

namespace Humanizer.SourceGenerators;

public sealed partial class HumanizerSourceGenerator
{
    static class EngineContractUtilities
    {
        public static JsonElement GetRequiredElement(JsonElement root, string? sourcePath) =>
            TryGetElement(root, sourcePath, out var element)
                ? element
                : throw new InvalidOperationException($"Missing required property '{sourcePath}'.");

        public static bool TryGetElement(JsonElement root, string? sourcePath, out JsonElement element)
        {
            element = root;
            if (string.IsNullOrWhiteSpace(sourcePath))
            {
                return true;
            }

            foreach (var segment in sourcePath!.Split('.'))
            {
                if (!element.TryGetProperty(segment, out element))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool TryGetOptionalString(JsonElement root, string? sourcePath, out string? value)
        {
            if (TryGetElement(root, sourcePath, out var property) && property.ValueKind == JsonValueKind.String)
            {
                value = property.GetString();
                return true;
            }

            value = null;
            return false;
        }

        public static string GetLeafPropertyName(string? sourcePath)
        {
            if (string.IsNullOrWhiteSpace(sourcePath))
            {
                throw new InvalidOperationException("A property path is required.");
            }

            var path = sourcePath!;
            var lastSeparator = path.LastIndexOf('.');
            return lastSeparator >= 0 ? path.Substring(lastSeparator + 1) : path;
        }
    }
}
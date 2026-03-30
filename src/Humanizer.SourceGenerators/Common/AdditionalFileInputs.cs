using System;
using System.IO;

using Microsoft.CodeAnalysis;

namespace Humanizer.SourceGenerators;

public sealed partial class HumanizerSourceGenerator
{
    sealed class TokenMapLocaleFile(string localeCode, string fileText)
    {
        public string LocaleCode { get; } = localeCode;
        public string FileText { get; } = fileText;

        public static TokenMapLocaleFile? Create(AdditionalText additionalText, CancellationToken cancellationToken)
        {
            var path = additionalText.Path.Replace('/', '\\');
            if (!path.Contains("\\CodeGen\\WordsToNumber\\", StringComparison.OrdinalIgnoreCase) ||
                !path.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            var fileText = additionalText.GetText(cancellationToken)?.ToString();
            if (string.IsNullOrWhiteSpace(fileText))
            {
                return null;
            }

            var localeCode = Path.GetFileNameWithoutExtension(additionalText.Path);
            return new TokenMapLocaleFile(localeCode, fileText!);
        }
    }

    sealed class JsonProfileFile(string profileName, string fileText)
    {
        public string ProfileName { get; } = profileName;
        public string FileText { get; } = fileText;

        public static JsonProfileFile? Create(AdditionalText additionalText, CancellationToken cancellationToken, string pathSegment)
        {
            var path = additionalText.Path.Replace('/', '\\');
            if (!path.Contains(pathSegment, StringComparison.OrdinalIgnoreCase) ||
                !path.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            var fileText = additionalText.GetText(cancellationToken)?.ToString();
            if (string.IsNullOrWhiteSpace(fileText))
            {
                return null;
            }

            return new JsonProfileFile(Path.GetFileNameWithoutExtension(additionalText.Path), fileText!);
        }
    }

    sealed class JsonSchemaFile(string schemaName, string fileText)
    {
        public string SchemaName { get; } = schemaName;
        public string FileText { get; } = fileText;

        public static JsonSchemaFile? Create(AdditionalText additionalText, CancellationToken cancellationToken, string pathSegment)
        {
            var path = additionalText.Path.Replace('/', '\\');
            if (!path.Contains(pathSegment, StringComparison.OrdinalIgnoreCase) ||
                !path.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            var fileText = additionalText.GetText(cancellationToken)?.ToString();
            if (string.IsNullOrWhiteSpace(fileText))
            {
                return null;
            }

            return new JsonSchemaFile(Path.GetFileNameWithoutExtension(additionalText.Path), fileText!);
        }
    }
}

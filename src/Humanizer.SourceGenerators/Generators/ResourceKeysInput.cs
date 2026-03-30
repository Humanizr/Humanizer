using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Humanizer.SourceGenerators;

public sealed partial class HumanizerSourceGenerator
{
    abstract class GeneratorInput
    {
        public static GeneratorInput? Create(AdditionalText additionalText, CancellationToken cancellationToken)
        {
            var fileName = Path.GetFileName(additionalText.Path);
            var fileText = additionalText.GetText(cancellationToken)?.ToString();
            if (string.IsNullOrWhiteSpace(fileText))
            {
                return null;
            }

            if (string.Equals(fileName, "Resources.resx", StringComparison.OrdinalIgnoreCase))
            {
                return new ResourceKeysInput(fileText!);
            }

            return null;
        }

        public abstract void Emit(SourceProductionContext context);
    }

    sealed class ResourceKeysInput(string fileText) : GeneratorInput
    {
        readonly string fileText = fileText;

        public override void Emit(SourceProductionContext context)
        {
            var document = XDocument.Parse(fileText, LoadOptions.None);
            var dataNames = document.Root?
                .Elements("data")
                .Select(static x => x.Attribute("name")?.Value)
                .Where(static x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.Ordinal)
                .OrderBy(static x => x, StringComparer.Ordinal)
                .ToArray();

            if (dataNames is null || dataNames.Length == 0)
            {
                return;
            }

            var builder = new StringBuilder();
            builder.AppendLine("namespace Humanizer;");
            builder.AppendLine();
            builder.AppendLine("public partial class ResourceKeys");
            builder.AppendLine("{");
            builder.AppendLine("    internal static class Names");
            builder.AppendLine("    {");

            foreach (var dataName in dataNames)
            {
                builder.Append("        internal const string ");
                builder.Append(SanitizeIdentifier(dataName!));
                builder.Append(" = \"");
                builder.Append(dataName);
                builder.AppendLine("\";");
            }

            builder.AppendLine("    }");
            builder.AppendLine("}");

            context.AddSource("ResourceKeys.Generated.g.cs", SourceText.From(builder.ToString(), Encoding.UTF8));
        }
    }
}

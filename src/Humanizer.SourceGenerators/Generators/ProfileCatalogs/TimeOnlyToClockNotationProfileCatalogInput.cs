using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.Json;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Humanizer.SourceGenerators;

public sealed partial class HumanizerSourceGenerator
{
    sealed class TimeOnlyToClockNotationProfileCatalogInput(
        ImmutableArray<TimeOnlyToClockNotationProfileDefinition> profiles,
        ImmutableDictionary<string, ExpressionSchema> schemas)
    {
        readonly ImmutableArray<TimeOnlyToClockNotationProfileDefinition> profiles = profiles;
        readonly ImmutableDictionary<string, ExpressionSchema> schemas = schemas;

        public static TimeOnlyToClockNotationProfileCatalogInput Create(
            ImmutableArray<JsonProfileFile?> files,
            ImmutableArray<JsonSchemaFile?> schemaFiles)
        {
            var profiles = ImmutableArray.CreateBuilder<TimeOnlyToClockNotationProfileDefinition>();
            foreach (var file in files)
            {
                if (file is null)
                {
                    continue;
                }

                using var document = JsonDocument.Parse(file.FileText);
                var root = document.RootElement;
                profiles.Add(new TimeOnlyToClockNotationProfileDefinition(
                    file.ProfileName,
                    GetRequiredString(root, "engine"),
                    root.Clone()));
            }

            return new TimeOnlyToClockNotationProfileCatalogInput(
                profiles.ToImmutable(),
                ExpressionSchemaLoader.Load(schemaFiles));
        }

        public void Emit(SourceProductionContext context)
        {
            if (profiles.IsDefaultOrEmpty)
            {
                return;
            }

            var builder = new StringBuilder();
            builder.AppendLine("#if NET6_0_OR_GREATER");
            builder.AppendLine();
            builder.AppendLine("using System;");
            builder.AppendLine();
            builder.AppendLine("namespace Humanizer;");
            builder.AppendLine();
            builder.AppendLine("static partial class TimeOnlyToClockNotationProfileCatalog");
            builder.AppendLine("{");
            builder.AppendLine("    public static ITimeOnlyToClockNotationConverter Resolve(string kind)");
            builder.AppendLine("    {");
            builder.AppendLine("        switch (kind)");
            builder.AppendLine("        {");

            foreach (var profile in profiles.OrderBy(static profile => profile.ProfileName, StringComparer.Ordinal))
            {
                builder.Append("            case ");
                builder.Append(QuoteLiteral(profile.ProfileName));
                builder.Append(": return ");
                builder.Append(GetCatalogPropertyName(profile.ProfileName));
                builder.AppendLine(";");
            }

            builder.AppendLine("            default: throw new ArgumentOutOfRangeException(nameof(kind), kind, \"Unknown clock-notation profile.\");");
            builder.AppendLine("        }");
            builder.AppendLine("    }");
            builder.AppendLine();

            foreach (var profile in profiles.OrderBy(static profile => profile.ProfileName, StringComparer.Ordinal))
            {
                builder.Append("    static ITimeOnlyToClockNotationConverter ");
                builder.Append(GetCatalogPropertyName(profile.ProfileName));
                builder.Append(" { get; } = ");
                builder.Append(TimeOnlyToClockNotationSchemaExpressionFactory.Create(profile, schemas));
                builder.AppendLine(";");
                builder.AppendLine();
            }

            builder.AppendLine("}");
            builder.AppendLine();
            builder.AppendLine("#endif");
            context.AddSource("TimeOnlyToClockNotationProfileCatalog.g.cs", SourceText.From(builder.ToString(), Encoding.UTF8));
        }
    }
}

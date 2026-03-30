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
    sealed class NumberToWordsProfileCatalogInput(
        ImmutableArray<NumberToWordsProfileDefinition> profiles,
        ImmutableDictionary<string, ExpressionSchema> schemas)
    {
        readonly ImmutableArray<NumberToWordsProfileDefinition> profiles = profiles;
        readonly ImmutableDictionary<string, ExpressionSchema> schemas = schemas;

        public static NumberToWordsProfileCatalogInput Create(
            ImmutableArray<JsonProfileFile?> files,
            ImmutableArray<JsonSchemaFile?> schemaFiles)
        {
            var profiles = ImmutableArray.CreateBuilder<NumberToWordsProfileDefinition>();
            foreach (var file in files)
            {
                if (file is null)
                {
                    continue;
                }

                using var document = JsonDocument.Parse(file.FileText);
                var root = document.RootElement;
                profiles.Add(new NumberToWordsProfileDefinition(
                    file.ProfileName,
                    GetRequiredString(root, "engine"),
                    root.Clone()));
            }

            return new NumberToWordsProfileCatalogInput(
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
            builder.AppendLine("using System;");
            builder.AppendLine("using System.Collections.Frozen;");
            builder.AppendLine("using System.Collections.Generic;");
            builder.AppendLine("using System.Globalization;");
            builder.AppendLine();
            builder.AppendLine("namespace Humanizer;");
            builder.AppendLine();
            builder.AppendLine("static partial class NumberToWordsProfileCatalog");
            builder.AppendLine("{");
            builder.AppendLine("    public static INumberToWordsConverter Resolve(string kind, CultureInfo culture)");
            builder.AppendLine("    {");
            builder.AppendLine("        switch (kind)");
            builder.AppendLine("        {");

            foreach (var profile in profiles.OrderBy(static profile => profile.ProfileName, StringComparer.Ordinal))
            {
                builder.Append("            case ");
                builder.Append(QuoteLiteral(profile.ProfileName));
                builder.Append(": return ");
                builder.Append(RequiresCulture(profile)
                    ? NumberToWordsSchemaExpressionFactory.Create(profile, schemas, useCultureParameter: true)
                    : GetCatalogPropertyName(profile.ProfileName));
                builder.AppendLine(";");
            }

            builder.AppendLine("        }");
            builder.AppendLine("        throw new ArgumentOutOfRangeException(nameof(kind), kind, \"Unknown number-to-words profile.\");");
            builder.AppendLine("    }");
            builder.AppendLine();

            foreach (var profile in profiles.OrderBy(static profile => profile.ProfileName, StringComparer.Ordinal))
            {
                if (RequiresCulture(profile))
                {
                    continue;
                }

                builder.Append("    static INumberToWordsConverter ");
                builder.Append(GetCatalogPropertyName(profile.ProfileName));
                builder.Append(" { get; } = ");
                builder.Append(NumberToWordsSchemaExpressionFactory.Create(profile, schemas, useCultureParameter: false));
                builder.AppendLine(";");
                builder.AppendLine();
            }

            builder.AppendLine("}");
            context.AddSource("NumberToWordsProfileCatalog.g.cs", SourceText.From(builder.ToString(), Encoding.UTF8));
        }

        static bool RequiresCulture(NumberToWordsProfileDefinition profile) =>
            GetBoolean(profile.Root, "useCulture");
    }

}

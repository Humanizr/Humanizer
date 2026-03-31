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
    sealed class WordsToNumberProfileCatalogInput(
        ImmutableArray<WordsToNumberProfileDefinition> profiles,
        ImmutableDictionary<string, ExpressionSchema> schemas)
    {
        readonly ImmutableArray<WordsToNumberProfileDefinition> profiles = profiles;
        readonly ImmutableDictionary<string, ExpressionSchema> schemas = schemas;

        public static WordsToNumberProfileCatalogInput Create(
            ImmutableArray<JsonProfileFile?> files,
            ImmutableArray<JsonSchemaFile?> schemaFiles)
        {
            var profiles = ImmutableArray.CreateBuilder<WordsToNumberProfileDefinition>();
            foreach (var file in files)
            {
                if (file is null)
                {
                    continue;
                }

                using var document = JsonDocument.Parse(file.FileText);
                var root = document.RootElement;
                profiles.Add(new WordsToNumberProfileDefinition(
                    file.ProfileName,
                    GetRequiredString(root, "engine"),
                    root.Clone()));
            }

            return new WordsToNumberProfileCatalogInput(
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
            builder.AppendLine("using System.Collections.Generic;");
            builder.AppendLine("using System.Globalization;");
            builder.AppendLine();
            builder.AppendLine("namespace Humanizer;");
            builder.AppendLine();
            builder.AppendLine("static partial class WordsToNumberProfileCatalog");
            builder.AppendLine("{");
            builder.AppendLine("    public static IWordsToNumberConverter Resolve(string kind)");
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

            builder.AppendLine("        }");
            builder.AppendLine("        throw new ArgumentOutOfRangeException(nameof(kind), kind, \"Unknown words-to-number profile.\");");
            builder.AppendLine("    }");
            builder.AppendLine();

            foreach (var profile in profiles.OrderBy(static profile => profile.ProfileName, StringComparer.Ordinal))
            {
                AppendLazyCachedMember(
                    builder,
                    "    ",
                    "static",
                    "IWordsToNumberConverter",
                    GetCatalogPropertyName(profile.ProfileName),
                    WordsToNumberSchemaExpressionFactory.Create(profile, schemas));
                builder.AppendLine();
            }

            builder.AppendLine("}");
            context.AddSource("WordsToNumberProfileCatalog.g.cs", SourceText.From(builder.ToString(), Encoding.UTF8));
        }
    }
}

using System.Collections.Immutable;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Humanizer.SourceGenerators;

public sealed partial class HumanizerSourceGenerator
{
    sealed class HeadingTableCatalogInput(ImmutableArray<ResolvedLocaleDefinition> locales)
    {
        readonly ImmutableArray<ResolvedLocaleDefinition> locales = locales;

        public static HeadingTableCatalogInput Create(LocaleCatalogInput localeCatalog) =>
            new(
                localeCatalog.Locales
                    .Where(static locale => locale.Headings is not null)
                    .ToImmutableArray());

        public void Emit(SourceProductionContext context)
        {
            if (locales.IsDefaultOrEmpty)
            {
                return;
            }

            var builder = new StringBuilder();
            builder.AppendLine("#nullable enable");
            builder.AppendLine();
            builder.AppendLine("using System;");
            builder.AppendLine("using System.Collections.Frozen;");
            builder.AppendLine("using System.Collections.Generic;");
            builder.AppendLine();
            builder.AppendLine("namespace Humanizer;");
            builder.AppendLine();
            builder.AppendLine("static partial class HeadingTableCatalog");
            builder.AppendLine("{");
            builder.AppendLine("    internal static partial HeadingTable? ResolveCore(string localeCode)");
            builder.AppendLine("    {");
            builder.AppendLine("        return Factories.TryGetValue(localeCode, out var factory)");
            builder.AppendLine("            ? factory()");
            builder.AppendLine("            : null;");
            builder.AppendLine("    }");
            builder.AppendLine();
            builder.AppendLine("    static readonly FrozenDictionary<string, Func<HeadingTable>> Factories = new Dictionary<string, Func<HeadingTable>>(StringComparer.Ordinal)");
            builder.AppendLine("    {");

            foreach (var locale in locales)
            {
                builder.Append("            ");
                builder.Append('[');
                builder.Append(QuoteLiteral(locale.LocaleCode));
                builder.Append("] = static () => ");
                builder.Append(GetCatalogPropertyName(locale.LocaleCode));
                builder.AppendLine(",");
            }

            builder.AppendLine("    }.ToFrozenDictionary(StringComparer.Ordinal);");
            builder.AppendLine();

            foreach (var locale in locales)
            {
                AppendLazyCachedMember(
                    builder,
                    "    ",
                    "static",
                    "HeadingTable",
                    GetCatalogPropertyName(locale.LocaleCode),
                    CreateHeadingTableExpression(locale.Headings!));
                builder.AppendLine();
            }

            builder.AppendLine("}");
            context.AddSource("HeadingTableCatalog.g.cs", SourceText.From(builder.ToString(), Encoding.UTF8));
        }

        static string CreateHeadingTableExpression(HeadingSet headings) =>
            "new HeadingTable(" + CreateArrayExpression(headings.Full) + ", " + CreateArrayExpression(headings.Short) + ")";

        static string CreateArrayExpression(ImmutableArray<string> values) =>
            "[" + string.Join(", ", values.Select(QuoteLiteral)) + "]";
    }
}
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
    sealed class OrdinalDateProfileCatalogInput(ImmutableArray<OrdinalDateProfileDefinition> profiles)
    {
        readonly ImmutableArray<OrdinalDateProfileDefinition> profiles = profiles;

        public static OrdinalDateProfileCatalogInput Create(LocaleCatalogInput localeCatalog)
        {
            var profiles = ImmutableArray.CreateBuilder<OrdinalDateProfileDefinition>();
            var seenProfiles = new HashSet<string>(StringComparer.Ordinal);
            foreach (var locale in localeCatalog.Locales)
            {
                AddProfile(profiles, locale.DateToOrdinalWords, seenProfiles);
                AddProfile(profiles, locale.DateOnlyToOrdinalWords, seenProfiles);
            }

            return new OrdinalDateProfileCatalogInput(profiles.ToImmutable());
        }

        static void AddProfile(
            ImmutableArray<OrdinalDateProfileDefinition>.Builder profiles,
            LocaleFeature? feature,
            HashSet<string> seenProfiles)
        {
            if (feature is not { UsesGeneratedProfile: true } ||
                !seenProfiles.Add(feature.ProfileName!))
            {
                return;
            }

            profiles.Add(new OrdinalDateProfileDefinition(
                feature.ProfileName!,
                GetRequiredString(feature.ProfileRoot, "pattern"),
                GetRequiredString(feature.ProfileRoot, "dayMode")));
        }

        public void Emit(SourceProductionContext context)
        {
            EmitDateCatalog(
                context,
                "DateToOrdinalWordsProfileCatalog",
                "IDateToOrdinalWordConverter",
                "PatternDateToOrdinalWordsConverter",
                "OrdinalDateProfileCatalog.DateTo.g.cs");

            EmitDateCatalog(
                context,
                "DateOnlyToOrdinalWordsProfileCatalog",
                "IDateOnlyToOrdinalWordConverter",
                "PatternDateOnlyToOrdinalWordsConverter",
                "OrdinalDateProfileCatalog.DateOnly.g.cs",
                condition: "NET6_0_OR_GREATER");
        }

        void EmitDateCatalog(
            SourceProductionContext context,
            string catalogName,
            string interfaceName,
            string converterTypeName,
            string sourceName,
            string? condition = null)
        {
            var builder = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(condition))
            {
                builder.Append("#if ");
                builder.AppendLine(condition);
                builder.AppendLine();
            }

            builder.AppendLine("using System;");
            builder.AppendLine();
            builder.AppendLine("namespace Humanizer;");
            builder.AppendLine();
            builder.Append("static class ");
            builder.Append(catalogName);
            builder.AppendLine();
            builder.AppendLine("{");
            builder.Append("    public static ");
            builder.Append(interfaceName);
            builder.AppendLine(" Resolve(string kind)");
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

            builder.AppendLine("            default: throw new ArgumentOutOfRangeException(nameof(kind), kind, \"Unknown ordinal date profile.\");");
            builder.AppendLine("        }");
            builder.AppendLine("    }");
            builder.AppendLine();

            foreach (var profile in profiles.OrderBy(static profile => profile.ProfileName, StringComparer.Ordinal))
            {
                AppendLazyCachedMember(
                    builder,
                    "    ",
                    "static",
                    interfaceName,
                    GetCatalogPropertyName(profile.ProfileName),
                    "new " + converterTypeName + "(new OrdinalDatePattern(" +
                    QuoteLiteral(profile.Pattern) +
                    ", OrdinalDateDayMode." +
                    profile.DayMode +
                    "))");
                builder.AppendLine();
            }

            builder.AppendLine("}");

            if (!string.IsNullOrWhiteSpace(condition))
            {
                builder.AppendLine();
                builder.AppendLine("#endif");
            }

            context.AddSource(sourceName, SourceText.From(builder.ToString(), Encoding.UTF8));
        }
    }

}

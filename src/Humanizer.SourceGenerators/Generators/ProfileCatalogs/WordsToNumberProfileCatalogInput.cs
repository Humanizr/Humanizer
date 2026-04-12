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
    sealed class WordsToNumberProfileCatalogInput(ImmutableArray<WordsToNumberProfileDefinition> profiles)
    {
        /// <summary>
        /// Emits the generated words-to-number profile catalog.
        /// The catalog must stay aligned with the locale-authored writer data so supported locales
        /// can round-trip their natural high-range forms.
        /// </summary>
        readonly ImmutableArray<WordsToNumberProfileDefinition> profiles = profiles;

        public static WordsToNumberProfileCatalogInput Create(LocaleCatalogInput localeCatalog)
        {
            var profiles = ImmutableArray.CreateBuilder<WordsToNumberProfileDefinition>();
            var seenProfiles = new HashSet<string>(StringComparer.Ordinal);
            foreach (var locale in localeCatalog.Locales)
            {
                var feature = locale.WordsToNumber;
                if (feature is not { UsesGeneratedProfile: true } ||
                    !seenProfiles.Add(feature.ProfileName!))
                {
                    continue;
                }

                profiles.Add(new WordsToNumberProfileDefinition(
                    feature.ProfileName!,
                    GetRequiredString(feature.ProfileRoot, "engine"),
                    feature.ProfileRoot.Clone()));
            }

            return new WordsToNumberProfileCatalogInput(profiles.ToImmutable());
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
                    WordsToNumberEngineContractFactory.Create(profile));
                builder.AppendLine();
            }

            builder.AppendLine("}");
            context.AddSource("WordsToNumberProfileCatalog.g.cs", SourceText.From(builder.ToString(), Encoding.UTF8));
        }
    }
}
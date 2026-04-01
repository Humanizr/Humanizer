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
        ImmutableDictionary<string, EngineContract> contracts)
    {
        readonly ImmutableArray<NumberToWordsProfileDefinition> profiles = profiles;
        readonly ImmutableDictionary<string, EngineContract> contracts = contracts;

        public static NumberToWordsProfileCatalogInput Create(LocaleCatalogInput localeCatalog)
        {
            var profiles = ImmutableArray.CreateBuilder<NumberToWordsProfileDefinition>();
            var seenProfiles = new HashSet<string>(StringComparer.Ordinal);
            foreach (var locale in localeCatalog.Locales)
            {
                var feature = locale.NumberToWords;
                if (feature is not { UsesGeneratedProfile: true } ||
                    !seenProfiles.Add(feature.ProfileName!))
                {
                    continue;
                }

                profiles.Add(new NumberToWordsProfileDefinition(
                    feature.ProfileName!,
                    GetRequiredString(feature.ProfileRoot, "engine"),
                    feature.ProfileRoot.Clone()));
            }

            return new NumberToWordsProfileCatalogInput(
                profiles.ToImmutable(),
                EngineContractCatalog.NumberToWords);
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
                var expression = CreateProfileExpression(profile, useCultureParameter: RequiresCulture(profile));
                builder.Append("            case ");
                builder.Append(QuoteLiteral(profile.ProfileName));
                builder.Append(": return ");
                builder.Append(RequiresCulture(profile) ? expression : GetCatalogPropertyName(profile.ProfileName));
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

                AppendLazyCachedMember(
                    builder,
                    "    ",
                    "static",
                    "INumberToWordsConverter",
                    GetCatalogPropertyName(profile.ProfileName),
                    CreateProfileExpression(profile, useCultureParameter: false));
                builder.AppendLine();
            }

            builder.AppendLine("}");
            context.AddSource("NumberToWordsProfileCatalog.g.cs", SourceText.From(builder.ToString(), Encoding.UTF8));
        }

        static bool RequiresCulture(NumberToWordsProfileDefinition profile) =>
            GetBoolean(profile.Root, "useCulture");

        string CreateProfileExpression(NumberToWordsProfileDefinition profile, bool useCultureParameter)
        {
            try
            {
                return NumberToWordsEngineContractFactory.Create(profile, contracts, useCultureParameter);
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException(
                    $"Failed to generate number-to-words profile '{profile.ProfileName}' using engine '{profile.Engine}': {exception.Message}",
                    exception);
            }
        }
    }

}

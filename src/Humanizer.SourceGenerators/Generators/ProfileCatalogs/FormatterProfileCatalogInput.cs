using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Humanizer.SourceGenerators;

public sealed partial class HumanizerSourceGenerator
{
    sealed class FormatterProfileCatalogInput(ImmutableArray<FormatterProfileDefinition> profiles)
    {
        readonly ImmutableArray<FormatterProfileDefinition> profiles = profiles;

        public ImmutableHashSet<string> DataBackedProfileNames { get; } = profiles
            .Select(static profile => profile.ProfileName)
            .ToImmutableHashSet(StringComparer.Ordinal);

        public static FormatterProfileCatalogInput Create(LocaleCatalogInput localeCatalog)
        {
            var profiles = ImmutableArray.CreateBuilder<FormatterProfileDefinition>();
            var seenProfiles = new HashSet<string>(StringComparer.Ordinal);
            foreach (var locale in localeCatalog.Locales)
            {
                var feature = locale.Formatter;
                if (feature is not { UsesGeneratedProfile: true } ||
                    !seenProfiles.Add(feature.ProfileName!))
                {
                    continue;
                }

                profiles.Add(new FormatterProfileDefinition(
                    feature.ProfileName!,
                    GetRequiredString(feature.ProfileRoot, "engine"),
                    feature.ProfileRoot.Clone()));
            }

            return new FormatterProfileCatalogInput(profiles.ToImmutable());
        }

        public void Emit(SourceProductionContext context)
        {
            if (profiles.IsDefaultOrEmpty)
            {
                return;
            }

            var generatedProfiles = profiles
                .OrderBy(static profile => profile.ProfileName, StringComparer.Ordinal)
                .ToArray();

            var builder = new StringBuilder();
            builder.AppendLine("using System;");
            builder.AppendLine("using System.Collections.Frozen;");
            builder.AppendLine("using System.Collections.Generic;");
            builder.AppendLine("using System.Globalization;");
            builder.AppendLine();
            builder.AppendLine("namespace Humanizer;");
            builder.AppendLine();
            builder.AppendLine("static partial class FormatterProfileCatalog");
            builder.AppendLine("{");
            builder.AppendLine("    public static IFormatter Resolve(string kind, CultureInfo culture)");
            builder.AppendLine("    {");
            builder.AppendLine("        return kind switch");
            builder.AppendLine("        {");

            foreach (var profile in generatedProfiles)
            {
                builder.Append("            ");
                builder.Append(QuoteLiteral(profile.ProfileName));
                builder.Append(" => new ProfiledFormatter(culture, ");
                builder.Append(GetCatalogPropertyName(profile.ProfileName));
                builder.AppendLine("),");
            }

            builder.AppendLine("            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, \"Unknown formatter profile.\")");
            builder.AppendLine("        };");
            builder.AppendLine("    }");
            builder.AppendLine();

            foreach (var profile in generatedProfiles)
            {
                AppendLazyCachedMember(
                    builder,
                    "    ",
                    "static",
                    "FormatterProfile",
                    GetCatalogPropertyName(profile.ProfileName),
                    CreateFormatterProfileExpression(profile));
                builder.AppendLine();
            }

            builder.AppendLine("}");
            context.AddSource("FormatterProfileCatalog.g.cs", SourceText.From(builder.ToString(), Encoding.UTF8));
        }

        static string CreateFormatterProfileExpression(FormatterProfileDefinition profile)
        {
            if (profile.Engine != "profiled")
            {
                throw new InvalidOperationException($"Unsupported formatter engine '{profile.Engine}'.");
            }

            return "new FormatterProfile(" +
                   CreateFormatterNumberDetectorExpression(profile.Root, "resourceKeyDetector") + ", " +
                   CreateFormatterSuffixMapExpression(profile.Root, "resourceKeySuffixes") + ", " +
                   CreateFormatterResourceKeyOverrideArrayExpression(profile.Root, "resourceKeyOverrides") + ", " +
                   CreateFormatterNumberDetectorExpression(profile.Root, "dataUnitDetector") + ", " +
                   CreateFormatterSuffixMapExpression(profile.Root, "dataUnitSuffixes") + ", " +
                   CreateFormatterNumberFormExpression(profile.Root, "dataUnitNonIntegralForm") + ", " +
                   CreateFormatterFallbackTransformExpression(profile.Root, "dataUnitFallbackTransform") + ", " +
                   CreateFormatterPrepositionModeExpression(profile.Root, "prepositionMode") + ", " +
                   CreateFormatterSecondaryPlaceholderModeExpression(profile.Root, "secondaryPlaceholderMode") + ", " +
                   CreateTimeUnitGenderMapExpression(profile.Root, "timeUnitGenders") +
                   ")";
        }
    }
}

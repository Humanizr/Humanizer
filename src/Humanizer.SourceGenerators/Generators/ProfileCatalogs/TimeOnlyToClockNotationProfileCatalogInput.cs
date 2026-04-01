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
    /// <summary>
    /// Builds the generated clock-notation profile catalog from locale-owned YAML.
    /// Only structurally shared clock-notation engines are emitted here; accepted
    /// residual locale leaves remain handwritten and are registered directly.
    /// </summary>
    sealed class TimeOnlyToClockNotationProfileCatalogInput(
        ImmutableArray<TimeOnlyToClockNotationProfileDefinition> profiles,
        ImmutableDictionary<string, EngineContract> contracts)
    {
        readonly ImmutableArray<TimeOnlyToClockNotationProfileDefinition> profiles = profiles;
        readonly ImmutableDictionary<string, EngineContract> contracts = contracts;

        public static TimeOnlyToClockNotationProfileCatalogInput Create(LocaleCatalogInput localeCatalog)
        {
            var profiles = ImmutableArray.CreateBuilder<TimeOnlyToClockNotationProfileDefinition>();
            var seenProfiles = new HashSet<string>(StringComparer.Ordinal);
            foreach (var locale in localeCatalog.Locales)
            {
                var feature = locale.TimeOnlyToClockNotation;
                if (feature is not { UsesGeneratedProfile: true } ||
                    !seenProfiles.Add(feature.ProfileName!))
                {
                    continue;
                }

                profiles.Add(new TimeOnlyToClockNotationProfileDefinition(
                    feature.ProfileName!,
                    GetRequiredString(feature.ProfileRoot, "engine"),
                    feature.ProfileRoot.Clone()));
            }

            return new TimeOnlyToClockNotationProfileCatalogInput(
                profiles.ToImmutable(),
                EngineContractCatalog.TimeOnlyToClockNotation);
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
                AppendLazyCachedMember(
                    builder,
                    "    ",
                    "static",
                    "ITimeOnlyToClockNotationConverter",
                    GetCatalogPropertyName(profile.ProfileName),
                    TimeOnlyToClockNotationEngineContractFactory.Create(profile, contracts));
                builder.AppendLine();
            }

            builder.AppendLine("}");
            builder.AppendLine();
            builder.AppendLine("#endif");
            context.AddSource("TimeOnlyToClockNotationProfileCatalog.g.cs", SourceText.From(builder.ToString(), Encoding.UTF8));
        }
    }
}

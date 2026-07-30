using System.Collections.Immutable;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Humanizer.SourceGenerators;

public sealed partial class HumanizerSourceGenerator
{
    sealed class MetricScaleWordCatalogInput(ImmutableArray<MetricScaleWordProfileDefinition> profiles)
    {
        readonly ImmutableArray<MetricScaleWordProfileDefinition> profiles = profiles;

        public static MetricScaleWordCatalogInput Create(LocaleCatalogInput localeCatalog)
        {
            var profiles = localeCatalog.Locales
                .Select(static locale =>
                {
                    if (locale.NumberToWords is not { UsesGeneratedProfile: true } feature)
                    {
                        return new MetricScaleWordProfileDefinition(
                            locale.LocaleCode,
                            ImmutableArray<MetricScaleWordDefinition>.Empty);
                    }

                    var profile = new NumberToWordsProfileDefinition(
                        feature.ProfileName!,
                        GetRequiredString(feature.ProfileRoot, "engine"),
                        feature.ProfileRoot);
                    try
                    {
                        return new MetricScaleWordProfileDefinition(
                            locale.LocaleCode,
                            NumberToWordsEngineContractFactory.CreateMetricScaleWords(
                                profile,
                                EngineContractCatalog.NumberToWords));
                    }
                    catch (Exception exception)
                    {
                        throw new InvalidOperationException(
                            $"Failed to project metric scale words for locale '{locale.LocaleCode}' " +
                            $"using engine '{profile.Engine}': {exception.Message}",
                            exception);
                    }
                })
                .Where(static profile => !profile.ScaleWords.IsDefaultOrEmpty)
                .ToImmutableArray();

            return new(profiles);
        }

        public void Emit(SourceProductionContext context)
        {
            var builder = new StringBuilder();
            builder.AppendLine("#nullable enable");
            builder.AppendLine("namespace Humanizer;");
            builder.AppendLine();
            builder.AppendLine("static partial class LocalizedMetricScaleWordCatalog");
            builder.AppendLine("{");
            builder.AppendLine("    private static partial bool TryResolveCore(string localeCode, char symbol, CardinalPluralCategory category, out string value)");
            builder.AppendLine("    {");
            builder.AppendLine("        switch (localeCode)");
            builder.AppendLine("        {");

            foreach (var profile in profiles.OrderBy(static profile => profile.LocaleCode, StringComparer.Ordinal))
            {
                builder.Append("            case ");
                builder.Append(QuoteLiteral(profile.LocaleCode));
                builder.AppendLine(":");
                builder.AppendLine("                switch (symbol)");
                builder.AppendLine("                {");

                foreach (var scaleWord in profile.ScaleWords)
                {
                    builder.Append("                    case '");
                    builder.Append(scaleWord.Symbol);
                    builder.Append("': ");
                    if (scaleWord.Plural is null)
                    {
                        builder.Append("if (category != CardinalPluralCategory.One) break; value = ");
                        builder.Append(QuoteLiteral(scaleWord.Singular));
                    }
                    else if (scaleWord.Two is not null || scaleWord.Few is not null)
                    {
                        builder.Append("value = ");
                        builder.Append("MetricScaleWordForms.Select(category, ");
                        builder.Append(QuoteLiteral(scaleWord.Singular));
                        builder.Append(", ");
                        builder.Append(QuoteLiteral(scaleWord.Plural));
                        builder.Append(", ");
                        builder.Append(scaleWord.Two is null ? "null" : QuoteLiteral(scaleWord.Two));
                        builder.Append(", ");
                        builder.Append(scaleWord.Few is null ? "null" : QuoteLiteral(scaleWord.Few));
                        builder.Append(')');
                    }
                    else if (scaleWord.Singular == scaleWord.Plural)
                    {
                        builder.Append("value = ");
                        builder.Append(QuoteLiteral(scaleWord.Singular));
                    }
                    else
                    {
                        builder.Append("value = ");
                        builder.Append("category == CardinalPluralCategory.One ? ");
                        builder.Append(QuoteLiteral(scaleWord.Singular));
                        builder.Append(" : ");
                        builder.Append(QuoteLiteral(scaleWord.Plural));
                    }

                    builder.AppendLine("; return true;");
                }

                builder.AppendLine("                }");
                builder.AppendLine("                break;");
            }

            builder.AppendLine("        }");
            builder.AppendLine();
            builder.AppendLine("        value = string.Empty;");
            builder.AppendLine("        return false;");
            builder.AppendLine("    }");
            builder.AppendLine("}");

            context.AddSource("LocalizedMetricScaleWordCatalog.g.cs", SourceText.From(builder.ToString(), Encoding.UTF8));
        }
    }
}
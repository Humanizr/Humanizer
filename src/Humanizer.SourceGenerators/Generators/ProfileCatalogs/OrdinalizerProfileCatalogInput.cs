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
    sealed class OrdinalizerProfileCatalogInput(ImmutableArray<OrdinalizerProfileDefinition> profiles)
    {
        readonly ImmutableArray<OrdinalizerProfileDefinition> profiles = profiles;
        public ImmutableHashSet<string> DataBackedProfileNames { get; } = profiles
            .Select(static profile => profile.ProfileName)
            .ToImmutableHashSet(StringComparer.Ordinal);

        public static OrdinalizerProfileCatalogInput Create(LocaleCatalogInput localeCatalog)
        {
            var profiles = ImmutableArray.CreateBuilder<OrdinalizerProfileDefinition>();
            var seenProfiles = new HashSet<string>(StringComparer.Ordinal);
            foreach (var locale in localeCatalog.Locales)
            {
                var feature = locale.Ordinalizer;
                if (feature is not { UsesGeneratedProfile: true } ||
                    !seenProfiles.Add(feature.ProfileName!))
                {
                    continue;
                }

                profiles.Add(new OrdinalizerProfileDefinition(
                    feature.ProfileName!,
                    GetRequiredString(feature.ProfileRoot, "engine"),
                    feature.ProfileRoot.Clone()));
            }

            return new OrdinalizerProfileCatalogInput(profiles.ToImmutable());
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
            builder.AppendLine("using System.Globalization;");
            builder.AppendLine();
            builder.AppendLine("namespace Humanizer;");
            builder.AppendLine();
            builder.AppendLine("static partial class OrdinalizerProfileCatalog");
            builder.AppendLine("{");
            builder.AppendLine("    public static IOrdinalizer Resolve(string kind, CultureInfo culture)");
            builder.AppendLine("    {");
            builder.AppendLine("        switch (kind)");
            builder.AppendLine("        {");

            foreach (var profile in generatedProfiles)
            {
                builder.Append("            case ");
                builder.Append(QuoteLiteral(profile.ProfileName));
                builder.Append(": return ");
                builder.Append(RequiresCulture(profile)
                    ? CreateOrdinalizerExpression(profile, useCultureParameter: true)
                    : GetCatalogPropertyName(profile.ProfileName));
                builder.AppendLine(";");
            }

            builder.AppendLine("        }");
            builder.AppendLine("        throw new ArgumentOutOfRangeException(nameof(kind), kind, \"Unknown ordinalizer profile.\");");
            builder.AppendLine("    }");
            builder.AppendLine();

            foreach (var profile in generatedProfiles)
            {
                if (RequiresCulture(profile))
                {
                    continue;
                }

                AppendLazyCachedMember(
                    builder,
                    "    ",
                    "static",
                    "IOrdinalizer",
                    GetCatalogPropertyName(profile.ProfileName),
                    CreateOrdinalizerExpression(profile, useCultureParameter: false));
                builder.AppendLine();
            }

            builder.AppendLine("}");
            context.AddSource("OrdinalizerProfileCatalog.g.cs", SourceText.From(builder.ToString(), Encoding.UTF8));
        }

        static bool RequiresCulture(OrdinalizerProfileDefinition profile) =>
            profile.Engine == "number-word-suffix" ||
            GetBoolean(profile.Root, "useCulture");

        static string CreateOrdinalizerExpression(OrdinalizerProfileDefinition profile, bool useCultureParameter)
        {
            return profile.Engine switch
            {
                "suffix" => CreateSuffixOrdinalizerExpression(profile.Root),
                "modulo-suffix" => CreateModuloSuffixOrdinalizerExpression(profile.Root),
                "template" => CreateTemplateOrdinalizerExpression(profile.Root),
                "word-form-template" => CreateWordFormTemplateOrdinalizerExpression(profile.Root, useCultureParameter),
                "number-word-suffix" => CreateNumberWordSuffixOrdinalizerExpression(profile.Root, useCultureParameter),
                _ => CreateConventionalOrdinalizerExpression(profile.Engine, useCultureParameter)
            };
        }

        static string CreateSuffixOrdinalizerExpression(JsonElement root)
        {
            var masculineSuffix = GetRequiredString(root, "masculineSuffix");
            var feminineSuffix = GetOptionalString(root, "feminineSuffix") ?? masculineSuffix;
            var neuterSuffix = GetOptionalString(root, "neuterSuffix") ?? masculineSuffix;
            return "new SuffixOrdinalizer(" +
                   QuoteLiteral(masculineSuffix) + ", " +
                   QuoteLiteral(feminineSuffix) + ", " +
                   QuoteLiteral(neuterSuffix) +
                   (GetBoolean(root, "zeroAsPlainNumber") ? ", zeroAsPlainNumber: true)" : ")");
        }

        static string CreateModuloSuffixOrdinalizerExpression(JsonElement root)
        {
            var lastTwoDigitsRange = root.TryGetProperty("lastTwoDigitsRange", out var range) && range.ValueKind == JsonValueKind.Object
                ? "new ModuloSuffixOrdinalizer.RangeRule(" +
                  GetRequiredInt64(range, "start").ToString(CultureInfo.InvariantCulture) + ", " +
                  GetRequiredInt64(range, "end").ToString(CultureInfo.InvariantCulture) + ", " +
                  QuoteLiteral(GetRequiredString(range, "suffix")) + ")"
                : "null";
            var absoluteAtLeast = root.TryGetProperty("absoluteAtLeast", out var threshold) && threshold.ValueKind == JsonValueKind.Object
                ? GetRequiredInt64(threshold, "value").ToString(CultureInfo.InvariantCulture)
                : "null";
            var absoluteAtLeastSuffix = root.TryGetProperty("absoluteAtLeast", out threshold) && threshold.ValueKind == JsonValueKind.Object
                ? QuoteLiteral(GetRequiredString(threshold, "suffix"))
                : "null";
            return "new ModuloSuffixOrdinalizer(new(" +
                   QuoteLiteral(GetRequiredString(root, "defaultSuffix")) + ", " +
                   CreateNullableIntStringFrozenDictionaryExpression(root, "exactSuffixes").Replace("null", "FrozenDictionary<int, string>.Empty") + ", " +
                   CreateNullableIntStringFrozenDictionaryExpression(root, "lastDigitSuffixes").Replace("null", "FrozenDictionary<int, string>.Empty") + ", " +
                   lastTwoDigitsRange + ", " +
                   absoluteAtLeast + ", " +
                   absoluteAtLeastSuffix + ", " +
                   (GetBoolean(root, "useAbsoluteValue") ? "true" : "false") +
                   "))";
        }

        static string CreateTemplateOrdinalizerExpression(JsonElement root)
        {
            var masculinePattern = CreateOrdinalizerPatternExpression(GetRequiredProperty(root, "masculine"));
            var femininePattern = root.TryGetProperty("feminine", out var feminine) && feminine.ValueKind == JsonValueKind.Object
                ? CreateOrdinalizerPatternExpression(feminine)
                : masculinePattern;
            var neuterPattern = root.TryGetProperty("neuter", out var neuter) && neuter.ValueKind == JsonValueKind.Object
                ? CreateOrdinalizerPatternExpression(neuter)
                : masculinePattern;
            return "new TemplateOrdinalizer(new(" +
                   masculinePattern + ", " +
                   femininePattern + ", " +
                   neuterPattern + ", " +
                   (GetBoolean(root, "zeroAsPlainNumber") ? "true" : "false") + ", " +
                   (GetBoolean(root, "minValueAsPlainNumber") ? "true" : "false") + ", " +
                   "TemplateOrdinalizer.NegativeNumberMode." + ToEnumMemberName(GetOptionalString(root, "negativeNumberMode") ?? "none") +
                   "))";
        }

        static string CreateWordFormTemplateOrdinalizerExpression(JsonElement root, bool useCultureParameter)
        {
            var masculinePatternSet = CreateWordFormPatternSetExpression(GetRequiredProperty(root, "masculine"));
            var femininePatternSet = root.TryGetProperty("feminine", out var feminine) && feminine.ValueKind == JsonValueKind.Object
                ? CreateWordFormPatternSetExpression(feminine)
                : masculinePatternSet;
            var neuterPatternSet = root.TryGetProperty("neuter", out var neuter) && neuter.ValueKind == JsonValueKind.Object
                ? CreateWordFormPatternSetExpression(neuter)
                : masculinePatternSet;

            return "new WordFormTemplateOrdinalizer(" +
                   (useCultureParameter ? "culture" : "CultureInfo.InvariantCulture") + ", new(" +
                   masculinePatternSet + ", " +
                   femininePatternSet + ", " +
                   neuterPatternSet + ", " +
                   (GetBoolean(root, "zeroAsPlainNumber") ? "true" : "false") + ", " +
                   (GetBoolean(root, "minValueAsPlainNumber") ? "true" : "false") + ", " +
                   "WordFormTemplateOrdinalizer.NegativeNumberMode." + ToEnumMemberName(GetOptionalString(root, "negativeNumberMode") ?? "none") +
                   "))";
        }

        static string CreateOrdinalizerPatternExpression(JsonElement root) =>
            "new(" +
            QuoteLiteral(GetOptionalString(root, "prefix") ?? string.Empty) + ", " +
            QuoteLiteral(GetRequiredString(root, "defaultSuffix")) + ", " +
            CreateNullableIntStringFrozenDictionaryExpression(root, "exactReplacements").Replace("null", "FrozenDictionary<int, string>.Empty") + ", " +
            CreateNullableIntStringFrozenDictionaryExpression(root, "exactSuffixes").Replace("null", "FrozenDictionary<int, string>.Empty") + ", " +
            CreateNullableIntStringFrozenDictionaryExpression(root, "lastDigitSuffixes").Replace("null", "FrozenDictionary<int, string>.Empty") +
            ")";

        static string CreateWordFormPatternSetExpression(JsonElement root)
        {
            var normalPattern = CreateOrdinalizerPatternExpression(root);
            var abbreviationPattern = root.TryGetProperty("abbreviation", out var abbreviation) && abbreviation.ValueKind == JsonValueKind.Object
                ? CreateOrdinalizerPatternExpression(abbreviation)
                : normalPattern;

            return "new(" + normalPattern + ", " + abbreviationPattern + ")";
        }

        static string CreateNumberWordSuffixOrdinalizerExpression(JsonElement root, bool useCultureParameter)
        {
            var masculineBlock = CreateNumberWordSuffixGenderBlockExpression(GetRequiredProperty(root, "masculine"));
            var feminineBlock = root.TryGetProperty("feminine", out var feminine) && feminine.ValueKind == JsonValueKind.Object
                ? CreateNumberWordSuffixGenderBlockExpression(feminine)
                : masculineBlock;

            var neuterFallback = GetOptionalString(root, "neuterFallback") ?? "masculine";
            var neuterGender = neuterFallback.Equals("feminine", StringComparison.OrdinalIgnoreCase)
                ? "GrammaticalGender.Feminine"
                : "GrammaticalGender.Masculine";

            return "new NumberWordSuffixOrdinalizer(" +
                   (useCultureParameter ? "culture" : "CultureInfo.InvariantCulture") + ", new(" +
                   masculineBlock + ", " +
                   feminineBlock + ", " +
                   neuterGender +
                   "))";
        }

        static string CreateNumberWordSuffixGenderBlockExpression(JsonElement root) =>
            "new(" +
            QuoteLiteral(GetRequiredString(root, "defaultSuffix")) + ", " +
            CreateNullableIntStringFrozenDictionaryExpression(root, "exactReplacements").Replace("null", "FrozenDictionary<int, string>.Empty") +
            ")";

        static string CreateConventionalOrdinalizerExpression(string engine, bool useCultureParameter)
        {
            var typeName = ToEnumMemberName(engine) + "Ordinalizer";
            return useCultureParameter
                ? "new " + typeName + "(culture)"
                : "new " + typeName + "()";
        }
    }

}
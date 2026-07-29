using System.Collections.Immutable;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Humanizer.SourceGenerators;

public sealed partial class HumanizerSourceGenerator
{
    sealed class LocaleDurationCaseTableCatalogInput(ImmutableArray<DurationCaseCatalog> catalogs)
    {
        static readonly string[] TimeUnitOrder =
        [
            "millisecond",
            "second",
            "minute",
            "hour",
            "day",
            "week",
            "month",
            "year"
        ];

        static readonly string[] CaseOrder =
        [
            "nominative",
            "genitive",
            "dative",
            "accusative",
            "instrumental",
            "prepositional",
            "ablative",
            "comitative",
            "ergative",
            "locative",
            "oblique",
            "partitive",
            "vocative",
            "elative",
            "illative",
            "sociative",
            "terminative",
            "translative"
        ];

        readonly ImmutableArray<DurationCaseCatalog> catalogs = catalogs;

        public static LocaleDurationCaseTableCatalogInput Create(LocaleCatalogInput localeCatalog)
        {
            if (!localeCatalog.Diagnostics.IsEmpty)
            {
                return new LocaleDurationCaseTableCatalogInput([]);
            }

            var coverage = DurationCaseCoverageInput.Create(localeCatalog);
            return new LocaleDurationCaseTableCatalogInput(coverage.Catalogs);
        }

        public void Emit(SourceProductionContext context)
        {
            if (catalogs.IsDefaultOrEmpty)
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
            builder.AppendLine("static partial class LocaleDurationCaseTableCatalog");
            builder.AppendLine("{");
            builder.AppendLine("    internal static partial LocaleDurationCaseTable? ResolveCore(string localeCode) =>");
            builder.AppendLine("        Factories.TryGetValue(localeCode, out var factory) ? factory() : null;");
            builder.AppendLine();
            builder.AppendLine("    static readonly FrozenDictionary<string, Func<LocaleDurationCaseTable>> Factories = new Dictionary<string, Func<LocaleDurationCaseTable>>(StringComparer.Ordinal)");
            builder.AppendLine("    {");

            foreach (var catalog in catalogs.OrderBy(static catalog => catalog.LocaleCode, StringComparer.Ordinal))
            {
                builder.Append("        [");
                builder.Append(QuoteLiteral(catalog.LocaleCode));
                builder.Append("] = static () => ");
                builder.Append(GetPropertyName(catalog.LocaleCode));
                builder.AppendLine(",");
            }

            builder.AppendLine("    }.ToFrozenDictionary(StringComparer.Ordinal);");
            builder.AppendLine();

            foreach (var catalog in catalogs.OrderBy(static catalog => catalog.LocaleCode, StringComparer.Ordinal))
            {
                AppendLazyCachedMember(
                    builder,
                    "    ",
                    "static",
                    "LocaleDurationCaseTable",
                    GetPropertyName(catalog.LocaleCode),
                    "new LocaleDurationCaseTable(LocaleDurationCaseClassification." +
                    catalog.Classification +
                    ", " +
                    CreateCasesExpression(catalog) +
                    ")");
                builder.AppendLine();
            }

            builder.AppendLine("}");
            context.AddSource("LocaleDurationCaseTableCatalog.g.cs", SourceText.From(builder.ToString(), Encoding.UTF8));
        }

        static string CreateCasesExpression(DurationCaseCatalog catalog)
        {
            var builder = new StringBuilder("new LocalizedDurationCase?[] { ");
            for (var index = 0; index < CaseOrder.Length; index++)
            {
                if (index > 0)
                {
                    builder.Append(", ");
                }

                builder.Append(catalog.Cases.TryGetValue(CaseOrder[index], out var overlay)
                    ? CreateCaseExpression(overlay)
                    : "null");
            }

            builder.Append(" }");
            return builder.ToString();
        }

        static string CreateCaseExpression(DurationCaseOverlay overlay)
        {
            var builder = new StringBuilder("new LocalizedDurationCase(new LocalizedDurationCaseUnit[] { ");
            for (var index = 0; index < TimeUnitOrder.Length; index++)
            {
                if (index > 0)
                {
                    builder.Append(", ");
                }

                builder.Append(CreateUnitExpression(overlay.Units[TimeUnitOrder[index]]));
            }

            builder.Append(" })");
            return builder.ToString();
        }

        static string CreateUnitExpression(DurationCaseUnit unit) =>
            unit.Kind switch
            {
                DurationCaseUnitKind.SameAsNominative =>
                    "new LocalizedDurationCaseUnit(LocalizedDurationCaseUnitKind.SameAsNominative)",
                DurationCaseUnitKind.Unsupported =>
                    "new LocalizedDurationCaseUnit(LocalizedDurationCaseUnitKind.Unsupported)",
                _ =>
                    "new LocalizedDurationCaseUnit(LocalizedDurationCaseUnitKind.Phrase, " +
                    CreateTimeSpanPhraseExpression(unit.Phrase!) + ")"
            };

        static string CreateTimeSpanPhraseExpression(TimeSpanPhrase phrase) =>
            "new LocalizedTimeSpanPhrase(" +
            QuoteOrNull(phrase.Single) + ", " +
            QuoteOrNull(phrase.SingleWordsVariant) + ", " +
            CreateCountedPhraseExpression(phrase.Multiple) + ", " +
            CreateCountedPhraseExpression(phrase.MultipleWordsVariant) + ", " +
            CreateTemplateExpression(phrase.NamedTemplate) +
            ")";

        static string CreateCountedPhraseExpression(CountedPhrase? phrase) =>
            phrase is null
                ? "null"
                : "new LocalizedCountedPhrase(" +
                  CreatePhraseFormsExpression(phrase.Forms) + ", " +
                  "PhraseCountPlacement." + MapCountPlacement(phrase.CountPlacement) + ", " +
                  QuoteOrNull(phrase.BeforeCountText) + ", " +
                  QuoteOrNull(phrase.AfterCountText) + ", " +
                  CreateTemplateExpression(phrase.NamedTemplate) +
                  ")";

        static string CreatePhraseFormsExpression(PhraseForms? forms) =>
            forms is null
                ? "null"
                : "new LocalizedPhraseForms(" +
                  QuoteLiteral(forms.Default) + ", " +
                  QuoteOrNull(forms.Singular) + ", " +
                  QuoteOrNull(forms.Dual) + ", " +
                  QuoteOrNull(forms.Paucal) + ", " +
                  QuoteOrNull(forms.Plural) +
                  ")";

        static string CreateTemplateExpression(NamedTemplatePhrase? template) =>
            template is null
                ? "null"
                : "new PhraseTemplate(" + QuoteOrNull(template.Name) + ", " + QuoteLiteral(template.Template) + ")";

        static string QuoteOrNull(string? value) =>
            value is null ? "null" : QuoteLiteral(value);

        static string MapCountPlacement(CountPlacement countPlacement) =>
            countPlacement switch
            {
                CountPlacement.None => "None",
                CountPlacement.BeforeForm => "BeforeForm",
                CountPlacement.AfterForm => "AfterForm",
                _ => "None"
            };

        static string GetPropertyName(string localeCode) =>
            "Locale_" + SanitizeIdentifier(localeCode);
    }
}
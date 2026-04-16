using System.Collections.Immutable;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Humanizer.SourceGenerators;

public sealed partial class HumanizerSourceGenerator
{
    sealed class LocalePhraseTableCatalogInput(ImmutableArray<LocalePhraseCatalog> catalogs)
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

        static readonly string[] DataUnitOrder =
        [
            "bit",
            "byte",
            "kilobyte",
            "megabyte",
            "gigabyte",
            "terabyte"
        ];

        readonly ImmutableArray<LocalePhraseCatalog> catalogs = catalogs;

        public static LocalePhraseTableCatalogInput Create(LocaleCatalogInput localeCatalog) =>
            new(
                [.. localeCatalog.Locales
                    .Where(static locale => locale.Phrases is not null)
                    .Select(static locale => locale.Phrases!)
                    .OrderBy(static catalog => catalog.LocaleCode, StringComparer.Ordinal)]);

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
            builder.AppendLine();
            builder.AppendLine("namespace Humanizer;");
            builder.AppendLine();
            builder.AppendLine("static partial class LocalePhraseTableCatalog");
            builder.AppendLine("{");
            builder.AppendLine("    internal static partial LocalePhraseTable? ResolveCore(string localeCode) =>");
            builder.AppendLine("        localeCode switch");
            builder.AppendLine("        {");

            foreach (var catalog in catalogs)
            {
                builder.Append("            ");
                builder.Append(QuoteLiteral(catalog.LocaleCode));
                builder.Append(" => ");
                builder.Append(GetCatalogPropertyName(catalog.LocaleCode));
                builder.AppendLine(",");
            }

            builder.AppendLine("            _ => null");
            builder.AppendLine("        };");
            builder.AppendLine();

            foreach (var catalog in catalogs)
            {
                AppendLazyCachedMember(
                    builder,
                    "    ",
                    "static",
                    "LocalePhraseTable",
                    GetCatalogPropertyName(catalog.LocaleCode),
                    CreateLocalePhraseTableExpression(catalog));
                builder.AppendLine();
            }

            builder.AppendLine("}");
            context.AddSource("LocalePhraseTableCatalog.g.cs", SourceText.From(builder.ToString(), Encoding.UTF8));
        }

        static string CreateLocalePhraseTableExpression(LocalePhraseCatalog catalog) =>
            "new LocalePhraseTable(" +
            QuoteOrNull(catalog.DateHumanize.Now) + ", " +
            QuoteOrNull(catalog.DateHumanize.Never) + ", " +
            QuoteOrNull(catalog.TimeSpan.Zero) + ", " +
            QuoteOrNull(catalog.TimeSpan.Age) + ", " +
            CreateDatePhraseArrayExpression(catalog.DateHumanize.Past) + ", " +
            CreateDatePhraseArrayExpression(catalog.DateHumanize.Future) + ", " +
            CreateTimeSpanPhraseArrayExpression(catalog.TimeSpan.Units) + ", " +
            CreateUnitPhraseArrayExpression(catalog.DataUnit.Units, DataUnitOrder) + ", " +
            CreateUnitPhraseArrayExpression(catalog.TimeUnit.Units, TimeUnitOrder) +
            ")";

        static string CreateDatePhraseArrayExpression(ImmutableDictionary<string, DateHumanizePhrase> units)
        {
            var builder = new StringBuilder();
            builder.Append("new LocalizedDatePhrase?[] { ");
            AppendArrayEntries(builder, TimeUnitOrder, units, static phrase => CreateDatePhraseExpression(phrase));
            builder.Append(" }");
            return builder.ToString();
        }

        static string CreateTimeSpanPhraseArrayExpression(ImmutableDictionary<string, TimeSpanPhrase> units)
        {
            var builder = new StringBuilder();
            builder.Append("new LocalizedTimeSpanPhrase?[] { ");
            AppendArrayEntries(builder, TimeUnitOrder, units, static phrase => CreateTimeSpanPhraseExpression(phrase));
            builder.Append(" }");
            return builder.ToString();
        }

        static string CreateUnitPhraseArrayExpression(
            ImmutableDictionary<string, DataUnitPhrase> units,
            IReadOnlyList<string> order)
        {
            var builder = new StringBuilder();
            builder.Append("new LocalizedUnitPhrase?[] { ");
            AppendArrayEntries(builder, order, units, static phrase => CreateUnitPhraseExpression(phrase.Forms, phrase.Symbol, phrase.NamedTemplate));
            builder.Append(" }");
            return builder.ToString();
        }

        static string CreateUnitPhraseArrayExpression(
            ImmutableDictionary<string, TimeUnitPhrase> units,
            IReadOnlyList<string> order)
        {
            var builder = new StringBuilder();
            builder.Append("new LocalizedUnitPhrase?[] { ");
            AppendArrayEntries(builder, order, units, static phrase => CreateUnitPhraseExpression(phrase.Forms, phrase.Symbol, phrase.NamedTemplate));
            builder.Append(" }");
            return builder.ToString();
        }

        static void AppendArrayEntries<TPhrase>(
            StringBuilder builder,
            IReadOnlyList<string> order,
            ImmutableDictionary<string, TPhrase> phrases,
            Func<TPhrase, string> expressionFactory)
        {
            for (var index = 0; index < order.Count; index++)
            {
                if (index > 0)
                {
                    builder.Append(", ");
                }

                builder.Append(phrases.TryGetValue(order[index], out var phrase)
                    ? expressionFactory(phrase)
                    : "null");
            }
        }

        static string CreateDatePhraseExpression(DateHumanizePhrase phrase) =>
            "new LocalizedDatePhrase(" +
            QuoteOrNull(phrase.Single) + ", " +
            CreateCountedPhraseExpression(phrase.Multiple) + ", " +
            CreateTemplateExpression(phrase.NamedTemplate) +
            ")";

        static string CreateTimeSpanPhraseExpression(TimeSpanPhrase phrase) =>
            "new LocalizedTimeSpanPhrase(" +
            QuoteOrNull(phrase.Single) + ", " +
            QuoteOrNull(phrase.SingleWordsVariant) + ", " +
            CreateCountedPhraseExpression(phrase.Multiple) + ", " +
            CreateCountedPhraseExpression(phrase.MultipleWordsVariant) + ", " +
            CreateTemplateExpression(phrase.NamedTemplate) +
            ")";

        static string CreateUnitPhraseExpression(PhraseForms? forms, string? symbol, NamedTemplatePhrase? template) =>
            "new LocalizedUnitPhrase(" +
            CreatePhraseFormsExpression(forms) + ", " +
            QuoteOrNull(symbol) + ", " +
            CreateTemplateExpression(template) +
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
    }
}
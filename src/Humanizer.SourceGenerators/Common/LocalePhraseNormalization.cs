using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

namespace Humanizer.SourceGenerators;

public sealed partial class HumanizerSourceGenerator
{
    internal static class LocalePhraseNormalization
    {
        static readonly Regex placeholderRegex = new(@"\{(?<name>[^}]+)\}", RegexOptions.Compiled);

        internal static LocalePhraseCatalog Create(string localeCode, SimpleYamlMapping phrases)
        {
            RejectUnknownKeys(phrases, $"{localeCode}.phrases", ["dateHumanize", "timeSpan", "dataUnit", "timeUnit"]);

            var dateHumanize = phrases.TryGetValue("dateHumanize", out var dateHumanizeValue)
                ? ParseDateHumanize(localeCode, dateHumanizeValue, $"{localeCode}.phrases.dateHumanize")
                : DateHumanizePhraseSet.Empty;

            var timeSpan = phrases.TryGetValue("timeSpan", out var timeSpanValue)
                ? ParseTimeSpan(localeCode, timeSpanValue, $"{localeCode}.phrases.timeSpan")
                : TimeSpanPhraseSet.Empty;

            var dataUnit = phrases.TryGetValue("dataUnit", out var dataUnitValue)
                ? ParseDataUnit(localeCode, dataUnitValue, $"{localeCode}.phrases.dataUnit")
                : DataUnitPhraseSet.Empty;

            var timeUnit = phrases.TryGetValue("timeUnit", out var timeUnitValue)
                ? ParseTimeUnit(localeCode, timeUnitValue, $"{localeCode}.phrases.timeUnit")
                : TimeUnitPhraseSet.Empty;

            return new LocalePhraseCatalog(localeCode, dateHumanize, timeSpan, dataUnit, timeUnit);
        }

        internal static LocalePhraseCatalog ParseLocalePhraseCatalogForTests(string localeCode, string fileText)
        {
            var root = SimpleYamlParser.Parse(fileText);
            if (!root.TryGetValue("phrases", out var phrases) || phrases is not SimpleYamlMapping phraseMapping)
            {
                throw new InvalidOperationException($"Locale '{localeCode}' does not define a 'phrases' mapping.");
            }

            return Create(localeCode, phraseMapping);
        }

        static DateHumanizePhraseSet ParseDateHumanize(string localeCode, SimpleYamlValue value, string path)
        {
            var mapping = ExpectMapping(value, path);
            RejectUnknownKeys(mapping, path, ["now", "never", "past", "future"]);
            var past = mapping.TryGetValue("past", out var pastValue)
                ? ParseDateHumanizeUnits(pastValue, $"{path}.past")
                : ImmutableDictionary<string, DateHumanizePhrase>.Empty.WithComparers(StringComparer.Ordinal);
            var future = mapping.TryGetValue("future", out var futureValue)
                ? ParseDateHumanizeUnits(futureValue, $"{path}.future")
                : ImmutableDictionary<string, DateHumanizePhrase>.Empty.WithComparers(StringComparer.Ordinal);

            return new DateHumanizePhraseSet(
                GetOptionalLiteral(mapping, "now", $"{path}.now"),
                GetOptionalLiteral(mapping, "never", $"{path}.never"),
                past,
                future);
        }

        static ImmutableDictionary<string, DateHumanizePhrase> ParseDateHumanizeUnits(SimpleYamlValue value, string path)
        {
            var mapping = ExpectMapping(value, path);
            var builder = ImmutableDictionary.CreateBuilder<string, DateHumanizePhrase>(StringComparer.Ordinal);

            foreach (var unit in mapping.Values)
            {
                builder[unit.Key] = ParseDateHumanizePhrase(unit.Value, $"{path}.{unit.Key}");
            }

            return builder.ToImmutable();
        }

        static DateHumanizePhrase ParseDateHumanizePhrase(SimpleYamlValue value, string path)
        {
            if (value is SimpleYamlScalar scalar)
            {
                return new DateHumanizePhrase(ValidateLiteralText(path, scalar.Value));
            }

            var mapping = ExpectMapping(value, path);
            RejectUnknownKeys(mapping, path, ["single", "multiple", "template"]);

            var single = GetOptionalLiteral(mapping, "single", $"{path}.single");
            var multiple = mapping.TryGetValue("multiple", out var multipleValue)
                ? ParseCountedPhrase(multipleValue, $"{path}.multiple")
                : null;
            var template = mapping.TryGetValue("template", out var templateValue)
                ? ParseNamedTemplate(templateValue, $"{path}.template", [])
                : null;
            if (single is null && multiple is null && template is null)
            {
                throw new InvalidOperationException($"Phrase '{path}' must define 'single', 'multiple', or 'template'.");
            }

            return new DateHumanizePhrase(single, multiple, template);
        }

        static TimeSpanPhraseSet ParseTimeSpan(string localeCode, SimpleYamlValue value, string path)
        {
            var mapping = ExpectMapping(value, path);
            var builder = ImmutableDictionary.CreateBuilder<string, TimeSpanPhrase>(StringComparer.Ordinal);

            foreach (var entry in mapping.Values)
            {
                if (entry.Key is "zero" or "age")
                {
                    continue;
                }

                builder[entry.Key] = ParseTimeSpanPhrase(entry.Value, $"{path}.{entry.Key}");
            }

            return new TimeSpanPhraseSet(
                GetOptionalLiteral(mapping, "zero", $"{path}.zero"),
                GetOptionalLiteralOrExplicitTemplate(mapping, "age", $"{path}.age"),
                builder.ToImmutable());
        }

        static TimeSpanPhrase ParseTimeSpanPhrase(SimpleYamlValue value, string path)
        {
            if (value is SimpleYamlScalar scalar)
            {
                return new TimeSpanPhrase(ValidateLiteralText(path, scalar.Value));
            }

            var mapping = ExpectMapping(value, path);
            RejectUnknownKeys(mapping, path, ["single", "multiple", "template"]);
            var (single, singleWordsVariant) = mapping.TryGetValue("single", out var singleValue)
                ? ParseSinglePhraseWithWordsVariant(singleValue, $"{path}.single")
                : default;

            var multiple = mapping.TryGetValue("multiple", out var multipleValue)
                ? ParseCountedPhrase(multipleValue, $"{path}.multiple", ["wordsVariant"])
                : null;

            CountedPhrase? multipleWordsVariant = null;
            if (mapping.TryGetValue("multiple", out multipleValue) &&
                multipleValue is SimpleYamlMapping multipleMapping &&
                multipleMapping.TryGetValue("wordsVariant", out var wordsVariant))
            {
                multipleWordsVariant = ParseCountedPhrase(wordsVariant, $"{path}.multiple.wordsVariant");
            }

            var template = mapping.TryGetValue("template", out var templateValue)
                ? ParseNamedTemplate(templateValue, $"{path}.template", [])
                : null;
            if (single is null &&
                singleWordsVariant is null &&
                multiple is null &&
                multipleWordsVariant is null &&
                template is null)
            {
                throw new InvalidOperationException($"Phrase '{path}' must define 'single', 'multiple', or 'template'.");
            }

            return new TimeSpanPhrase(
                single,
                singleWordsVariant,
                multiple,
                multipleWordsVariant,
                template);
        }

        static DataUnitPhraseSet ParseDataUnit(string localeCode, SimpleYamlValue value, string path)
        {
            var mapping = ExpectMapping(value, path);
            var builder = ImmutableDictionary.CreateBuilder<string, DataUnitPhrase>(StringComparer.Ordinal);

            foreach (var entry in mapping.Values)
            {
                builder[entry.Key] = ParseDataUnitPhrase(entry.Value, $"{path}.{entry.Key}");
            }

            return new DataUnitPhraseSet(builder.ToImmutable());
        }

        static DataUnitPhrase ParseDataUnitPhrase(SimpleYamlValue value, string path)
        {
            if (value is SimpleYamlScalar scalar)
            {
                return new DataUnitPhrase(PhraseForms.FromScalar(ValidateLiteralText(path, scalar.Value)));
            }

            var mapping = ExpectMapping(value, path);
            RejectUnknownKeys(mapping, path, ["forms", "default", "singular", "dual", "paucal", "plural", "symbol", "template"]);

            var forms = ParseOptionalPhraseForms(mapping, path);
            var symbol = GetOptionalLiteral(mapping, "symbol", $"{path}.symbol");
            var template = mapping.TryGetValue("template", out var templateValue)
                ? ParseNamedTemplate(templateValue, $"{path}.template", ["count", "unit"])
                : null;
            if (forms is null && symbol is null && template is null)
            {
                throw new InvalidOperationException($"Phrase '{path}' must define forms, 'symbol', or 'template'.");
            }

            return new DataUnitPhrase(forms, symbol, template);
        }

        static TimeUnitPhraseSet ParseTimeUnit(string localeCode, SimpleYamlValue value, string path)
        {
            var mapping = ExpectMapping(value, path);
            var builder = ImmutableDictionary.CreateBuilder<string, TimeUnitPhrase>(StringComparer.Ordinal);

            foreach (var entry in mapping.Values)
            {
                builder[entry.Key] = ParseTimeUnitPhrase(entry.Value, $"{path}.{entry.Key}");
            }

            return new TimeUnitPhraseSet(builder.ToImmutable());
        }

        static TimeUnitPhrase ParseTimeUnitPhrase(SimpleYamlValue value, string path)
        {
            if (value is SimpleYamlScalar scalar)
            {
                return new TimeUnitPhrase(PhraseForms.FromScalar(ValidateLiteralText(path, scalar.Value)));
            }

            var mapping = ExpectMapping(value, path);
            RejectUnknownKeys(mapping, path, ["forms", "default", "singular", "dual", "paucal", "plural", "symbol", "template"]);

            var forms = ParseOptionalPhraseForms(mapping, path);
            var symbol = GetOptionalLiteral(mapping, "symbol", $"{path}.symbol");
            var template = mapping.TryGetValue("template", out var templateValue)
                ? ParseNamedTemplate(templateValue, $"{path}.template", ["count", "unit"])
                : null;
            if (forms is null && symbol is null && template is null)
            {
                throw new InvalidOperationException($"Phrase '{path}' must define forms, 'symbol', or 'template'.");
            }

            return new TimeUnitPhrase(forms, symbol, template);
        }

        static CountedPhrase ParseCountedPhrase(SimpleYamlValue value, string path, params string[] allowedExtraKeys)
        {
            if (value is SimpleYamlScalar scalar)
            {
                return new CountedPhrase(
                    PhraseForms.FromScalar(ValidateLiteralText(path, scalar.Value)).CollapseDuplicates());
            }

            var mapping = ExpectMapping(value, path);
            RejectUnknownKeys(
                mapping,
                path,
                ["forms", "default", "singular", "dual", "paucal", "plural", "template", "countPlacement", "beforeCount", "afterCount", .. allowedExtraKeys]);
            var countPlacement = ParseCountPlacement(mapping, path);
            var formPlaceholders = countPlacement == CountPlacement.None
                ? ["count", "prep"]
                : Array.Empty<string>();
            var forms = ParseOptionalPhraseForms(mapping, path, formPlaceholders);
            var namedTemplate = mapping.TryGetValue("template", out var templateValue)
                ? ParseNamedTemplate(templateValue, $"{path}.template", ["count", "unit", "prep"])
                : null;

            if (forms is null)
            {
                throw new InvalidOperationException($"Phrase '{path}' must define forms.");
            }

            return new CountedPhrase(
                forms?.CollapseDuplicates(),
                countPlacement,
                GetOptionalLiteral(mapping, "beforeCount", $"{path}.beforeCount", ["prep"]),
                GetOptionalLiteral(mapping, "afterCount", $"{path}.afterCount", ["prep"]),
                namedTemplate);
        }

        static (string? Single, string? WordsVariant) ParseSinglePhraseWithWordsVariant(SimpleYamlValue value, string path)
        {
            if (value is SimpleYamlScalar scalar)
            {
                return (ValidateLiteralText(path, scalar.Value), null);
            }

            var mapping = ExpectMapping(value, path);
            RejectUnknownKeys(mapping, path, ["numeric", "text", "words"]);

            var single = GetOptionalLiteral(mapping, "numeric", $"{path}.numeric") ??
                GetOptionalLiteral(mapping, "text", $"{path}.text");
            var wordsVariant = GetOptionalLiteral(mapping, "words", $"{path}.words");
            if (single is null && wordsVariant is null)
            {
                throw new InvalidOperationException($"Phrase '{path}' must define 'numeric', 'text', or 'words'.");
            }

            return (single, wordsVariant);
        }

        static PhraseForms? ParseOptionalPhraseForms(SimpleYamlMapping mapping, string path, params string[] allowedPlaceholders)
        {
            if (mapping.TryGetValue("forms", out var formsValue))
            {
                return ParsePhraseForms(formsValue, $"{path}.forms", allowedPlaceholders);
            }

            if (TryParseDirectPhraseForms(mapping, path, out var directForms, allowedPlaceholders))
            {
                return directForms;
            }

            return null;
        }

        static PhraseForms ParsePhraseForms(SimpleYamlValue value, string path, params string[] allowedPlaceholders)
        {
            if (value is SimpleYamlScalar scalar)
            {
                return PhraseForms.FromScalar(ValidateLiteralText(path, scalar.Value, allowedPlaceholders)).CollapseDuplicates();
            }

            var mapping = ExpectMapping(value, path);
            RejectUnknownKeys(mapping, path, ["default", "singular", "dual", "paucal", "plural"]);
            if (!TryParseDirectPhraseForms(mapping, path, out var forms, allowedPlaceholders))
            {
                throw new InvalidOperationException($"Phrase forms '{path}' must define at least one form.");
            }

            return forms.CollapseDuplicates();
        }

        static bool TryParseDirectPhraseForms(SimpleYamlMapping mapping, string path, out PhraseForms forms, params string[] allowedPlaceholders)
        {
            var defaultValue = GetOptionalLiteral(mapping, "default", $"{path}.default", allowedPlaceholders);
            var singular = GetOptionalLiteral(mapping, "singular", $"{path}.singular", allowedPlaceholders);
            var dual = GetOptionalLiteral(mapping, "dual", $"{path}.dual", allowedPlaceholders);
            var paucal = GetOptionalLiteral(mapping, "paucal", $"{path}.paucal", allowedPlaceholders);
            var plural = GetOptionalLiteral(mapping, "plural", $"{path}.plural", allowedPlaceholders);

            defaultValue ??= singular ?? dual ?? paucal ?? plural;
            if (defaultValue is null)
            {
                forms = null!;
                return false;
            }

            forms = new PhraseForms(defaultValue, singular, dual, paucal, plural);
            return true;
        }

        static CountPlacement ParseCountPlacement(SimpleYamlMapping mapping, string path) =>
            GetOptionalLiteral(mapping, "countPlacement", $"{path}.countPlacement") switch
            {
                null or "" or "before-form" or "before" => CountPlacement.BeforeForm,
                "after-form" or "after" => CountPlacement.AfterForm,
                "none" => CountPlacement.None,
                var value => throw new InvalidOperationException($"Phrase '{path}.countPlacement' uses unsupported count placement '{value}'.")
            };

        static NamedTemplatePhrase ParseNamedTemplate(SimpleYamlValue value, string path, params string[] allowedPlaceholders)
        {
            if (value is SimpleYamlScalar scalar)
            {
                return CreateNamedTemplatePhrase(null, scalar.Value, path, allowedPlaceholders);
            }

            var mapping = ExpectMapping(value, path);
            RejectUnknownKeys(mapping, path, ["name", "value"]);
            var name = GetOptionalLiteral(mapping, "name", $"{path}.name");
            if (!mapping.TryGetValue("value", out var templateValue) || templateValue is not SimpleYamlScalar templateScalar)
            {
                throw new InvalidOperationException($"Template '{path}' must define a scalar 'value'.");
            }

            return CreateNamedTemplatePhrase(name, templateScalar.Value, path, allowedPlaceholders);
        }

        static NamedTemplatePhrase CreateNamedTemplatePhrase(string? templateName, string template, string path, params string[] allowedPlaceholders)
        {
            var placeholders = ImmutableArray.CreateBuilder<string>();

            foreach (Match match in placeholderRegex.Matches(template))
            {
                var placeholderName = match.Groups["name"].Value;
                if (int.TryParse(placeholderName, out _))
                {
                    throw new InvalidOperationException($"Template '{path}' cannot use numeric placeholder '{{{placeholderName}}}'.");
                }

                if (placeholderName.Length == 0 || !char.IsLetter(placeholderName[0]) || placeholderName.Any(static c => !char.IsLetterOrDigit(c) && c != '_'))
                {
                    throw new InvalidOperationException($"Template '{path}' uses unsupported placeholder '{{{placeholderName}}}'.");
                }

                if (!allowedPlaceholders.Contains(placeholderName, StringComparer.Ordinal))
                {
                    throw new InvalidOperationException(
                        $"Template '{path}' can only use placeholders: {string.Join(", ", allowedPlaceholders.Select(static value => $"{{{value}}}"))}.");
                }

                if (!placeholders.Contains(placeholderName))
                {
                    placeholders.Add(placeholderName);
                }
            }

            return new NamedTemplatePhrase(templateName, template, placeholders.ToImmutable());
        }

        static string? GetOptionalLiteral(SimpleYamlMapping mapping, string key, string path, params string[] allowedPlaceholders)
        {
            if (!mapping.TryGetValue(key, out var value))
            {
                return null;
            }

            if (value is not SimpleYamlScalar scalar)
            {
                throw new InvalidOperationException($"Phrase '{path}' must be a scalar string.");
            }

            return ValidateLiteralText(path, scalar.Value, allowedPlaceholders);
        }

        static string? GetOptionalLiteralOrExplicitTemplate(SimpleYamlMapping mapping, string key, string path)
        {
            if (!mapping.TryGetValue(key, out var value))
            {
                return null;
            }

            if (value is SimpleYamlScalar scalar)
            {
                return ValidateLiteralText(path, scalar.Value);
            }

            var templateMapping = ExpectMapping(value, path);
            RejectUnknownKeys(templateMapping, path, ["template"]);
            if (!templateMapping.TryGetValue("template", out var templateValue))
            {
                throw new InvalidOperationException($"Phrase '{path}' must define a scalar string or an explicit 'template'.");
            }

            return ParseNamedTemplate(templateValue, $"{path}.template", ["value"]).Template;
        }

        static string ValidateLiteralText(string path, string text, params string[] allowedPlaceholders)
        {
            foreach (Match match in placeholderRegex.Matches(text))
            {
                var placeholder = match.Groups["name"].Value;
                if (int.TryParse(placeholder, out _))
                {
                    throw new InvalidOperationException($"Phrase '{path}' cannot use numeric placeholder '{{{placeholder}}}'.");
                }

                if (placeholder.Length == 0 || !char.IsLetter(placeholder[0]) || placeholder.Any(static c => !char.IsLetterOrDigit(c) && c != '_'))
                {
                    throw new InvalidOperationException($"Phrase '{path}' uses unsupported placeholder '{{{placeholder}}}'.");
                }

                if (!allowedPlaceholders.Contains(placeholder, StringComparer.Ordinal))
                {
                    throw new InvalidOperationException(
                        allowedPlaceholders.Length == 0
                            ? $"Phrase '{path}' can only use placeholders in explicit 'template' entries."
                            : $"Phrase '{path}' can only use placeholders: {string.Join(", ", allowedPlaceholders.Select(static value => $"{{{value}}}"))}.");
                }
            }

            return text;
        }

        static SimpleYamlMapping ExpectMapping(SimpleYamlValue value, string path) =>
            value as SimpleYamlMapping ??
            throw new InvalidOperationException($"Phrase section '{path}' must be a mapping.");

        static void RejectUnknownKeys(SimpleYamlMapping mapping, string path, params string[] allowedKeys)
        {
            foreach (var key in mapping.Values.Keys)
            {
                if (!allowedKeys.Contains(key, StringComparer.Ordinal))
                {
                    throw new InvalidOperationException(
                        $"Phrase section '{path}' defines unsupported property '{key}'. Supported properties: {string.Join(", ", allowedKeys)}.");
                }
            }
        }
    }
}

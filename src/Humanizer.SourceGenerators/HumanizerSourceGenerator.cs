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

[Generator]
public sealed class HumanizerSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var additionalFiles = context.AdditionalTextsProvider
            .Select(static (additionalText, cancellationToken) => GeneratorInput.Create(additionalText, cancellationToken))
            .Where(static input => input is not null);

        context.RegisterSourceOutput(additionalFiles, static (productionContext, input) =>
        {
            if (input is null)
            {
                return;
            }

            input.Emit(productionContext);
        });
    }

    abstract class GeneratorInput
    {
        public static GeneratorInput? Create(AdditionalText additionalText, CancellationToken cancellationToken)
        {
            var fileName = Path.GetFileName(additionalText.Path);
            var fileText = additionalText.GetText(cancellationToken)?.ToString();
            if (string.IsNullOrWhiteSpace(fileText))
            {
                return null;
            }

            if (string.Equals(fileName, "Resources.resx", StringComparison.OrdinalIgnoreCase))
            {
                return new ResourceKeysInput(fileText!);
            }

            if (fileName.EndsWith("Registry.manifest", StringComparison.OrdinalIgnoreCase))
            {
                return RegistryManifestInput.Create(fileName, fileText!);
            }

            if (string.Equals(fileName, "TokenMapWordsToNumberConverters.json", StringComparison.OrdinalIgnoreCase))
            {
                return TokenMapWordsToNumberInput.Create(fileText!);
            }

            return null;
        }

        public abstract void Emit(SourceProductionContext context);
    }

    sealed class ResourceKeysInput(string fileText) : GeneratorInput
    {
        readonly string fileText = fileText;

        public override void Emit(SourceProductionContext context)
        {
            var document = XDocument.Parse(fileText, LoadOptions.None);
            var dataNames = document.Root?
                .Elements("data")
                .Select(static x => x.Attribute("name")?.Value)
                .Where(static x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.Ordinal)
                .OrderBy(static x => x, StringComparer.Ordinal)
                .ToArray();

            if (dataNames is null || dataNames.Length == 0)
            {
                return;
            }

            var builder = new StringBuilder();
            builder.AppendLine("namespace Humanizer;");
            builder.AppendLine();
            builder.AppendLine("public partial class ResourceKeys");
            builder.AppendLine("{");
            builder.AppendLine("    internal static class Names");
            builder.AppendLine("    {");

            foreach (var dataName in dataNames)
            {
                builder.Append("        internal const string ");
                builder.Append(SanitizeIdentifier(dataName!));
                builder.Append(" = \"");
                builder.Append(dataName);
                builder.AppendLine("\";");
            }

            builder.AppendLine("    }");
            builder.AppendLine("}");

            context.AddSource("ResourceKeys.Generated.g.cs", SourceText.From(builder.ToString(), Encoding.UTF8));
        }
    }

    sealed class RegistryManifestInput(string registryName, ImmutableArray<RegistryEntry> registrations, string? condition, ImmutableArray<Diagnostic> diagnostics) : GeneratorInput
    {
        readonly string registryName = registryName;
        readonly ImmutableArray<RegistryEntry> registrations = registrations;
        readonly string? condition = condition;
        readonly ImmutableArray<Diagnostic> diagnostics = diagnostics;

        public static RegistryManifestInput Create(string fileName, string fileText)
        {
            var registryName = Path.GetFileNameWithoutExtension(fileName);
            var seenLocales = new HashSet<string>(StringComparer.Ordinal);
            var registrations = ImmutableArray.CreateBuilder<RegistryEntry>();
            var diagnostics = ImmutableArray.CreateBuilder<Diagnostic>();
            string? condition = null;

            var lines = fileText.Split(["\r\n", "\n"], StringSplitOptions.None);
            for (var lineNumber = 0; lineNumber < lines.Length; lineNumber++)
            {
                var line = lines[lineNumber].Trim();
                if (line.Length == 0 || line.StartsWith("#", StringComparison.Ordinal))
                {
                    continue;
                }

                if (line.StartsWith("@if ", StringComparison.Ordinal))
                {
                    condition = line.Substring("@if ".Length).Trim();
                    continue;
                }

                var firstSeparatorIndex = line.IndexOf('|');
                if (firstSeparatorIndex <= 0 || firstSeparatorIndex == line.Length - 1)
                {
                    diagnostics.Add(Diagnostic.Create(
                        Diagnostics.InvalidManifestLine,
                        Location.None,
                        fileName,
                        lineNumber + 1));
                    continue;
                }

                var secondSeparatorIndex = line.IndexOf('|', firstSeparatorIndex + 1);
                if (secondSeparatorIndex < 0)
                {
                    secondSeparatorIndex = line.Length;
                }

                var locale = line.Substring(0, firstSeparatorIndex).Trim();
                var profile = line.Substring(firstSeparatorIndex + 1, secondSeparatorIndex - firstSeparatorIndex - 1).Trim();
                var argument = secondSeparatorIndex == line.Length
                    ? null
                    : line.Substring(secondSeparatorIndex + 1).Trim();

                if (string.IsNullOrWhiteSpace(profile))
                {
                    diagnostics.Add(Diagnostic.Create(
                        Diagnostics.InvalidManifestLine,
                        Location.None,
                        fileName,
                        lineNumber + 1));
                    continue;
                }

                if (!seenLocales.Add(locale))
                {
                    diagnostics.Add(Diagnostic.Create(
                        Diagnostics.DuplicateLocale,
                        Location.None,
                        locale,
                        fileName));
                    continue;
                }

                registrations.Add(new RegistryEntry(locale, profile, argument));
            }

            return new RegistryManifestInput(registryName, registrations.ToImmutable(), condition, diagnostics.ToImmutable());
        }

        public override void Emit(SourceProductionContext context)
        {
            foreach (var diagnostic in diagnostics)
            {
                context.ReportDiagnostic(diagnostic);
            }

            if (registrations.IsDefaultOrEmpty)
            {
                return;
            }

            var helperName = registryName + "Registrations";
            var builder = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(condition))
            {
                builder.Append("#if ");
                builder.AppendLine(condition);
                builder.AppendLine();
            }

            builder.AppendLine("namespace Humanizer;");
            builder.AppendLine();
            builder.Append("internal static class ");
            builder.Append(helperName);
            builder.AppendLine();
            builder.AppendLine("{");
            builder.Append("    internal static void Register(");
            builder.Append(registryName);
            builder.AppendLine(" registry)");
            builder.AppendLine("    {");

            foreach (var registration in registrations)
            {
                var expression = RegistryExpressionFactory.Create(registryName, registration.Profile, registration.Argument);
                if (expression is null)
                {
                    context.ReportDiagnostic(Diagnostic.Create(
                        Diagnostics.UnknownRegistryProfile,
                        Location.None,
                        registryName,
                        registration.Profile,
                        registration.Locale));
                    continue;
                }

                builder.Append("        registry.Register(\"");
                builder.Append(registration.Locale);
                builder.Append("\", culture => ");
                builder.Append(expression);
                builder.AppendLine(");");
            }

            builder.AppendLine("    }");
            builder.AppendLine("}");

            if (!string.IsNullOrWhiteSpace(condition))
            {
                builder.AppendLine();
                builder.AppendLine("#endif");
            }

            context.AddSource(helperName + ".g.cs", SourceText.From(builder.ToString(), Encoding.UTF8));
        }
    }

    sealed class TokenMapWordsToNumberInput(ImmutableArray<TokenMapLocaleDefinition> locales, ImmutableArray<Diagnostic> diagnostics) : GeneratorInput
    {
        readonly ImmutableArray<TokenMapLocaleDefinition> locales = locales;
        readonly ImmutableArray<Diagnostic> diagnostics = diagnostics;

        public static TokenMapWordsToNumberInput Create(string fileText)
        {
            var diagnostics = ImmutableArray.CreateBuilder<Diagnostic>();
            var locales = ImmutableArray.CreateBuilder<TokenMapLocaleDefinition>();
            using var document = JsonDocument.Parse(fileText);
            if (!document.RootElement.TryGetProperty("locales", out var localesElement) || localesElement.ValueKind != JsonValueKind.Array)
            {
                diagnostics.Add(Diagnostic.Create(Diagnostics.InvalidTokenMapXml, Location.None, "Missing locales array"));
                return new TokenMapWordsToNumberInput(locales.ToImmutable(), diagnostics.ToImmutable());
            }

            foreach (var localeElement in localesElement.EnumerateArray())
            {
                var name = GetString(localeElement, "name");
                var normalizationProfile = GetString(localeElement, "normalizationProfile");
                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(normalizationProfile))
                {
                    diagnostics.Add(Diagnostic.Create(Diagnostics.InvalidTokenMapXml, Location.None, "Locale entry is missing Name or NormalizationProfile"));
                    continue;
                }

                locales.Add(new TokenMapLocaleDefinition(
                    name!,
                    normalizationProfile!,
                    GetString(localeElement, "ordinalBuilder"),
                    GetStrings(localeElement, "negativePrefixes"),
                    GetStrings(localeElement, "ignoredTokens"),
                    GetStrings(localeElement, "tokenSuffixesToStrip"),
                    GetStrings(localeElement, "ordinalAbbreviationSuffixes"),
                    GetBoolean(localeElement, "useHundredMultiplier"),
                    GetBoolean(localeElement, "allowInvariantIntegerInput"),
                    GetLong(localeElement, "scaleThreshold"),
                    GetMap(localeElement)));
            }

            return new TokenMapWordsToNumberInput(locales.ToImmutable(), diagnostics.ToImmutable());
        }

        public override void Emit(SourceProductionContext context)
        {
            foreach (var diagnostic in diagnostics)
            {
                context.ReportDiagnostic(diagnostic);
            }

            if (locales.IsDefaultOrEmpty)
            {
                return;
            }

            var builder = new StringBuilder();
            builder.AppendLine("namespace Humanizer;");
            builder.AppendLine();
            builder.AppendLine("static class TokenMapWordsToNumberConverters");
            builder.AppendLine("{");

            foreach (var locale in locales)
            {
                builder.Append("    public static IWordsToNumberConverter ");
                builder.Append(locale.Name);
                builder.AppendLine(" { get; } = new TokenMapWordsToNumberConverter(new()");
                builder.AppendLine("    {");
                builder.AppendLine("        CardinalMap = new Dictionary<string, long>(StringComparer.Ordinal)");
                builder.AppendLine("        {");

                foreach (var entry in locale.CardinalEntries)
                {
                    builder.Append("            [\"");
                    builder.Append(Escape(entry.Key));
                    builder.Append("\"] = ");
                    builder.Append(entry.Value.ToString(CultureInfo.InvariantCulture));
                    builder.AppendLine(",");
                }

                builder.AppendLine("        }.ToFrozenDictionary(StringComparer.Ordinal),");

                if (!string.IsNullOrWhiteSpace(locale.OrdinalBuilder))
                {
                    builder.Append("        OrdinalMap = ");
                    builder.Append(locale.OrdinalBuilder);
                    builder.AppendLine("(),");
                }

                builder.Append("        NormalizationProfile = TokenMapNormalizationProfile.");
                builder.Append(locale.NormalizationProfile);
                builder.AppendLine(",");

                AppendStringArray(builder, "NegativePrefixes", locale.NegativePrefixes);
                AppendStringArray(builder, "IgnoredTokens", locale.IgnoredTokens);
                AppendStringArray(builder, "TokenSuffixesToStrip", locale.TokenSuffixesToStrip);
                AppendStringArray(builder, "OrdinalAbbreviationSuffixes", locale.OrdinalAbbreviationSuffixes);

                if (locale.UseHundredMultiplier)
                {
                    builder.AppendLine("        UseHundredMultiplier = true,");
                }

                if (locale.AllowInvariantIntegerInput)
                {
                    builder.AppendLine("        AllowInvariantIntegerInput = true,");
                }

                if (locale.ScaleThreshold.HasValue)
                {
                    builder.Append("        ScaleThreshold = ");
                    builder.Append(locale.ScaleThreshold.Value.ToString(CultureInfo.InvariantCulture));
                    builder.AppendLine(",");
                }

                builder.AppendLine("    });");
                builder.AppendLine();
            }

            AppendOrdinalBuilder(builder, "Azerbaijani", "AzerbaijaniNumberToWordsConverter", "TokenMapNormalizationProfile.CollapseWhitespace", GrammaticalGenderVariant.None);
            AppendOrdinalBuilder(builder, "Catalan", "CatalanNumberToWordsConverter", "TokenMapNormalizationProfile.LowercaseRemovePeriods", GrammaticalGenderVariant.MasculineAndFeminine);
            AppendOrdinalBuilder(builder, "Russian", "RussianNumberToWordsConverter", "TokenMapNormalizationProfile.LowercaseRemovePeriods", GrammaticalGenderVariant.All);
            AppendOrdinalBuilder(builder, "Turkish", "TurkishNumberToWordConverter", "TokenMapNormalizationProfile.CollapseWhitespace", GrammaticalGenderVariant.None);
            AppendOrdinalBuilder(builder, "Ukrainian", "UkrainianNumberToWordsConverter", "TokenMapNormalizationProfile.LowercaseRemovePeriods", GrammaticalGenderVariant.All);

            builder.AppendLine("    static string NormalizeLowercase(string words) =>");
            builder.AppendLine("        TokenMapWordsToNumberNormalizer.Normalize(words, TokenMapNormalizationProfile.LowercaseRemovePeriods);");
            builder.AppendLine("}");

            context.AddSource("TokenMapWordsToNumberConverters.g.cs", SourceText.From(builder.ToString(), Encoding.UTF8));
        }

        static void AppendStringArray(StringBuilder builder, string propertyName, ImmutableArray<string> values)
        {
            if (values.IsDefaultOrEmpty)
            {
                return;
            }

            builder.Append("        ");
            builder.Append(propertyName);
            builder.Append(" = [");

            for (var i = 0; i < values.Length; i++)
            {
                if (i > 0)
                {
                    builder.Append(", ");
                }

                builder.Append('"');
                builder.Append(Escape(values[i]));
                builder.Append('"');
            }

            builder.AppendLine("],");
        }

        static void AppendOrdinalBuilder(StringBuilder builder, string name, string converterTypeName, string normalizationProfile, GrammaticalGenderVariant grammaticalGenderVariant)
        {
            builder.Append("    static FrozenDictionary<string, int> Build");
            builder.Append(name);
            builder.AppendLine("OrdinalMap()");
            builder.AppendLine("    {");
            builder.Append("        var converter = new ");
            builder.Append(converterTypeName);
            builder.AppendLine("();");
            builder.AppendLine("        var ordinals = new Dictionary<string, int>(StringComparer.Ordinal);");
            builder.AppendLine();
            builder.AppendLine("        for (var number = 1; number <= 200; number++)");
            builder.AppendLine("        {");

            switch (grammaticalGenderVariant)
            {
                case GrammaticalGenderVariant.None:
                    builder.Append("            ordinals[TokenMapWordsToNumberNormalizer.Normalize(converter.ConvertToOrdinal(number), ");
                    builder.Append(normalizationProfile);
                    builder.AppendLine(")] = number;");
                    break;
                case GrammaticalGenderVariant.MasculineAndFeminine:
                    builder.AppendLine("            ordinals[NormalizeLowercase(converter.ConvertToOrdinal(number, GrammaticalGender.Masculine))] = number;");
                    builder.AppendLine("            ordinals[NormalizeLowercase(converter.ConvertToOrdinal(number, GrammaticalGender.Feminine))] = number;");
                    break;
                case GrammaticalGenderVariant.All:
                    builder.AppendLine("            ordinals[NormalizeLowercase(converter.ConvertToOrdinal(number))] = number;");
                    builder.AppendLine("            ordinals[NormalizeLowercase(converter.ConvertToOrdinal(number, GrammaticalGender.Feminine))] = number;");
                    builder.AppendLine("            ordinals[NormalizeLowercase(converter.ConvertToOrdinal(number, GrammaticalGender.Neuter))] = number;");
                    break;
            }

            builder.AppendLine("        }");
            builder.AppendLine();
            builder.AppendLine("        return ordinals.ToFrozenDictionary(StringComparer.Ordinal);");
            builder.AppendLine("    }");
            builder.AppendLine();
        }

        static string? GetString(JsonElement element, string propertyName) =>
            element.TryGetProperty(propertyName, out var value) && value.ValueKind == JsonValueKind.String
                ? value.GetString()
                : null;

        static ImmutableArray<string> GetStrings(JsonElement element, string propertyName)
        {
            if (!element.TryGetProperty(propertyName, out var valuesElement) || valuesElement.ValueKind != JsonValueKind.Array)
            {
                return [];
            }

            var values = ImmutableArray.CreateBuilder<string>();
            foreach (var valueElement in valuesElement.EnumerateArray())
            {
                if (valueElement.ValueKind != JsonValueKind.String)
                {
                    continue;
                }

                var value = valueElement.GetString();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    values.Add(value!);
                }
            }

            return values.ToImmutable();
        }

        static ImmutableArray<TokenMapEntry> GetMap(JsonElement element)
        {
            if (!element.TryGetProperty("cardinalMap", out var mapElement) || mapElement.ValueKind != JsonValueKind.Object)
            {
                return [];
            }

            var entries = ImmutableArray.CreateBuilder<TokenMapEntry>();
            foreach (var property in mapElement.EnumerateObject())
            {
                if (property.Value.ValueKind != JsonValueKind.Number || !property.Value.TryGetInt64(out var value))
                {
                    continue;
                }

                entries.Add(new TokenMapEntry(property.Name, value));
            }

            return entries.ToImmutable();
        }

        static long? GetLong(JsonElement element, string propertyName)
        {
            if (!element.TryGetProperty(propertyName, out var valueElement) || valueElement.ValueKind != JsonValueKind.Number)
            {
                return null;
            }

            return valueElement.TryGetInt64(out var value) ? value : null;
        }

        static bool GetBoolean(JsonElement element, string propertyName) =>
            element.TryGetProperty(propertyName, out var valueElement) &&
            valueElement.ValueKind == JsonValueKind.True;
    }

    static class RegistryExpressionFactory
    {
        public static string? Create(string registryName, string profile, string? argument) =>
            registryName switch
            {
                "CollectionFormatterRegistry" => CreateCollectionFormatter(profile, argument),
                "DateOnlyToOrdinalWordsConverterRegistry" => CreateDateOnlyToOrdinalWords(profile),
                "DateToOrdinalWordsConverterRegistry" => CreateDateToOrdinalWords(profile),
                "FormatterRegistry" => CreateFormatter(profile),
                "NumberToWordsConverterRegistry" => CreateNumberToWords(profile),
                "OrdinalizerRegistry" => CreateOrdinalizer(profile, argument),
                "TimeOnlyToClockNotationConvertersRegistry" => CreateTimeOnlyToClockNotation(profile),
                "WordsToNumberConverterRegistry" => CreateWordsToNumber(profile),
                _ => null
            };

        static string? CreateCollectionFormatter(string profile, string? argument) =>
            profile switch
            {
                "oxford" => Parameterless("OxfordStyleCollectionFormatter"),
                "default" => StringConstructor("DefaultCollectionFormatter", argument),
                "clitic" => StringConstructor("CliticCollectionFormatter", argument),
                "delimited" => StringConstructor("DelimitedCollectionFormatter", argument),
                _ => null
            };

        static string? CreateDateOnlyToOrdinalWords(string profile) =>
            profile switch
            {
                "default" => Parameterless("DefaultDateOnlyToOrdinalWordConverter"),
                "us" => Parameterless("UsDateOnlyToOrdinalWordsConverter"),
                "ca" => Parameterless("CaDateOnlyToOrdinalWordsConverter"),
                "long" => Parameterless("LongDateOnlyToOrdinalWordsConverter"),
                "es" => Parameterless("EsDateOnlyToOrdinalWordsConverter"),
                "fr" => Parameterless("FrDateOnlyToOrdinalWordsConverter"),
                "lt" => Parameterless("LtDateOnlyToOrdinalWordsConverter"),
                "lv" => Parameterless("LvDateOnlyToOrdinalWordsConverter"),
                "pt-br" => Parameterless("PtBrDateOnlyToOrdinalWordsConverter"),
                _ => null
            };

        static string? CreateDateToOrdinalWords(string profile) =>
            profile switch
            {
                "default" => Parameterless("DefaultDateToOrdinalWordConverter"),
                "us" => Parameterless("UsDateToOrdinalWordsConverter"),
                "ca" => Parameterless("CaDateToOrdinalWordsConverter"),
                "long" => Parameterless("LongDateToOrdinalWordsConverter"),
                "es" => Parameterless("EsDateToOrdinalWordsConverter"),
                "fr" => Parameterless("FrDateToOrdinalWordsConverter"),
                "lt" => Parameterless("LtDateToOrdinalWordsConverter"),
                "lv" => Parameterless("LvDateToOrdinalWordsConverter"),
                "pt-br" => Parameterless("PtBrDateToOrdinalWordsConverter"),
                _ => null
            };

        static string? CreateFormatter(string profile) =>
            profile switch
            {
                "arabic" => CultureConstructor("ArabicFormatter"),
                "german" => CultureConstructor("GermanFormatter"),
                "hebrew" => CultureConstructor("HebrewFormatter"),
                "romanian" => CultureConstructor("RomanianFormatter"),
                "russian" => CultureConstructor("RussianFormatter"),
                "slovenian" => CultureConstructor("SlovenianFormatter"),
                "croatian" => CultureConstructor("CroatianFormatter"),
                "serbian" => CultureConstructor("SerbianFormatter"),
                "ukrainian" => CultureConstructor("UkrainianFormatter"),
                "french" => CultureConstructor("FrenchFormatter"),
                "czech-slovak-polish" => CultureConstructor("CzechSlovakPolishFormatter"),
                "bulgarian" => CultureConstructor("BulgarianFormatter"),
                "swedish" => CultureConstructor("SwedishFormatter"),
                "trim-plural-suffix" => CultureConstructor("TrimPluralSuffixFormatter"),
                "finnish" => CultureConstructor("FinnishFormatter"),
                "hungarian" => CultureConstructor("HungarianFormatter"),
                "icelandic" => CultureConstructor("IcelandicFormatter"),
                "latvian" => CultureConstructor("LatvianFormatter"),
                "maltese" => CultureConstructor("MalteseFormatter"),
                "italian" => CultureConstructor("ItalianFormatter"),
                "lithuanian" => CultureConstructor("LithuanianFormatter"),
                "luxembourgish" => CultureConstructor("LuxembourgishFormatter"),
                "catalan" => CultureConstructor("CatalanFormatter"),
                _ => null
            };

        static string? CreateNumberToWords(string profile) =>
            profile switch
            {
                "afrikaans" => Parameterless("AfrikaansNumberToWordsConverter"),
                "english" => Parameterless("EnglishNumberToWordsConverter"),
                "arabic" => Parameterless("ArabicNumberToWordsConverter"),
                "czech" => CultureConstructor("CzechNumberToWordsConverter"),
                "danish" => Parameterless("DanishNumberToWordsConverter"),
                "farsi" => Parameterless("FarsiNumberToWordsConverter"),
                "spanish" => Parameterless("SpanishNumberToWordsConverter"),
                "polish" => CultureConstructor("PolishNumberToWordsConverter"),
                "portuguese" => Parameterless("PortugueseNumberToWordsConverter"),
                "brazilian-portuguese" => Parameterless("BrazilianPortugueseNumberToWordsConverter"),
                "romanian" => Parameterless("RomanianNumberToWordsConverter"),
                "russian" => Parameterless("RussianNumberToWordsConverter"),
                "finnish" => Parameterless("FinnishNumberToWordsConverter"),
                "filipino" => Parameterless("FilipinoNumberToWordsConverter"),
                "french-belgian" => Parameterless("FrenchBelgianNumberToWordsConverter"),
                "french-swiss" => Parameterless("FrenchSwissNumberToWordsConverter"),
                "french" => Parameterless("FrenchNumberToWordsConverter"),
                "indonesian" => Parameterless("IndonesianNumberToWordsConverter"),
                "malay" => Parameterless("MalayNumberToWordsConverter"),
                "dutch" => Parameterless("DutchNumberToWordsConverter"),
                "hebrew" => CultureConstructor("HebrewNumberToWordsConverter"),
                "slovenian" => CultureConstructor("SlovenianNumberToWordsConverter"),
                "slovak" => CultureConstructor("SlovakNumberToWordsConverter"),
                "german" => Parameterless("GermanNumberToWordsConverter"),
                "german-swiss-liechtenstein" => Parameterless("GermanSwissLiechtensteinNumberToWordsConverter"),
                "bangla" => Parameterless("BanglaNumberToWordsConverter"),
                "turkish" => Parameterless("TurkishNumberToWordConverter"),
                "icelandic" => Parameterless("IcelandicNumberToWordsConverter"),
                "italian" => Parameterless("ItalianNumberToWordsConverter"),
                "maltese" => Parameterless("MalteseNumberToWordsConvertor"),
                "ukrainian" => Parameterless("UkrainianNumberToWordsConverter"),
                "uzbek-latn" => Parameterless("UzbekLatnNumberToWordConverter"),
                "uzbek-cyrl" => Parameterless("UzbekCyrlNumberToWordConverter"),
                "swedish" => Parameterless("SwedishNumberToWordsConverter"),
                "serbian-cyrl" => CultureConstructor("SerbianCyrlNumberToWordsConverter"),
                "serbian-latn" => CultureConstructor("SerbianNumberToWordsConverter"),
                "tamil" => Parameterless("TamilNumberToWordsConverter"),
                "croatian" => CultureConstructor("CroatianNumberToWordsConverter"),
                "norwegian-bokmal" => Parameterless("NorwegianBokmalNumberToWordsConverter"),
                "vietnamese" => Parameterless("VietnameseNumberToWordsConverter"),
                "chinese" => Parameterless("ChineseNumberToWordsConverter"),
                "bulgarian" => Parameterless("BulgarianNumberToWordsConverter"),
                "armenian" => Parameterless("ArmenianNumberToWordsConverter"),
                "azerbaijani" => Parameterless("AzerbaijaniNumberToWordsConverter"),
                "japanese" => Parameterless("JapaneseNumberToWordsConverter"),
                "central-kurdish" => Parameterless("CentralKurdishNumberToWordsConverter"),
                "greek" => Parameterless("GreekNumberToWordsConverter"),
                "thai" => Parameterless("ThaiNumberToWordsConverter"),
                "latvian" => Parameterless("LatvianNumberToWordsConverter"),
                "korean" => Parameterless("KoreanNumberToWordsConverter"),
                "indian" => Parameterless("IndianNumberToWordsConverter"),
                "lithuanian" => Parameterless("LithuanianNumberToWordsConverter"),
                "luxembourgish" => Parameterless("LuxembourgishNumberToWordsConverter"),
                "hungarian" => Parameterless("HungarianNumberToWordsConverter"),
                "catalan" => Parameterless("CatalanNumberToWordsConverter"),
                _ => null
            };

        static string? CreateOrdinalizer(string profile, string? argument) =>
            profile switch
            {
                "afrikaans" => Parameterless("AfrikaansOrdinalizer"),
                "default" => Parameterless("DefaultOrdinalizer"),
                "azerbaijani" => Parameterless("AzerbaijaniOrdinalizer"),
                "numeric-suffix" => StringConstructor("NumericSuffixOrdinalizer", argument),
                "catalan" => Parameterless("CatalanOrdinalizer"),
                "german" => Parameterless("GermanOrdinalizer"),
                "english" => Parameterless("EnglishOrdinalizer"),
                "spanish" => CultureConstructor("SpanishOrdinalizer"),
                "french" => Parameterless("FrenchOrdinalizer"),
                "hungarian" => Parameterless("HungarianOrdinalizer"),
                "armenian" => Parameterless("ArmenianOrdinalizer"),
                "icelandic" => Parameterless("IcelandicOrdinalizer"),
                "italian" => Parameterless("ItalianOrdinalizer"),
                "luxembourgish" => Parameterless("LuxembourgishOrdinalizer"),
                "dutch" => Parameterless("DutchOrdinalizer"),
                "portuguese" => Parameterless("PortugueseOrdinalizer"),
                "romanian" => Parameterless("RomanianOrdinalizer"),
                "russian" => Parameterless("RussianOrdinalizer"),
                "swedish" => Parameterless("SwedishOrdinalizer"),
                "turkish" => Parameterless("TurkishOrdinalizer"),
                "ukrainian" => Parameterless("UkrainianOrdinalizer"),
                "uzbek-cyrillic" => Parameterless("UzbekCyrillicOrdinalizer"),
                "uzbek-latin" => Parameterless("UzbekLatinOrdinalizer"),
                _ => null
            };

        static string? CreateTimeOnlyToClockNotation(string profile) =>
            profile switch
            {
                "default" => CultureConstructor("DefaultTimeOnlyToClockNotationConverter"),
                "ca" => Parameterless("CaTimeOnlyToClockNotationConverter"),
                "german" => Parameterless("GermanTimeOnlyToClockNotationConverter"),
                "es" => Parameterless("EsTimeOnlyToClockNotationConverter"),
                "fr" => Parameterless("FrTimeOnlyToClockNotationConverter"),
                "lb" => Parameterless("LbTimeOnlyToClockNotationConverter"),
                "portuguese" => Parameterless("PortugueseTimeOnlyToClockNotationConverter"),
                "brazilian-portuguese" => Parameterless("BrazilianPortugueseTimeOnlyToClockNotationConverter"),
                _ => null
            };

        static string? CreateWordsToNumber(string profile) =>
            profile switch
            {
                "tokenmap-afrikaans" => "TokenMapWordsToNumberConverters.Afrikaans",
                "tokenmap-armenian" => "TokenMapWordsToNumberConverters.Armenian",
                "tokenmap-bulgarian" => "TokenMapWordsToNumberConverters.Bulgarian",
                "tokenmap-bengali" => "TokenMapWordsToNumberConverters.Bengali",
                "tokenmap-catalan" => "TokenMapWordsToNumberConverters.Catalan",
                "tokenmap-croatian" => "TokenMapWordsToNumberConverters.Croatian",
                "tokenmap-czech" => "TokenMapWordsToNumberConverters.Czech",
                "tokenmap-greek" => "TokenMapWordsToNumberConverters.Greek",
                "tokenmap-latvian" => "TokenMapWordsToNumberConverters.Latvian",
                "tokenmap-lithuanian" => "TokenMapWordsToNumberConverters.Lithuanian",
                "tokenmap-polish" => "TokenMapWordsToNumberConverters.Polish",
                "tokenmap-russian" => "TokenMapWordsToNumberConverters.Russian",
                "tokenmap-serbian-cyrillic" => "TokenMapWordsToNumberConverters.SerbianCyrillic",
                "tokenmap-serbian-latin" => "TokenMapWordsToNumberConverters.SerbianLatin",
                "tokenmap-turkish" => "TokenMapWordsToNumberConverters.Turkish",
                "tokenmap-ukrainian" => "TokenMapWordsToNumberConverters.Ukrainian",
                "tokenmap-uzbek-cyrl" => "TokenMapWordsToNumberConverters.UzbekCyrl",
                "tokenmap-uzbek-latn" => "TokenMapWordsToNumberConverters.UzbekLatn",
                "tokenmap-azerbaijani" => "TokenMapWordsToNumberConverters.Azerbaijani",
                "english" => Parameterless("EnglishWordsToNumberConverter"),
                "arabic" => Parameterless("ArabicWordsToNumberConverter"),
                "danish" => Parameterless("DanishWordsToNumberConverter"),
                "german" => Parameterless("GermanWordsToNumberConverter"),
                "finnish" => Parameterless("FinnishWordsToNumberConverter"),
                "french" => Parameterless("FrenchWordsToNumberConverter"),
                "hebrew" => Parameterless("HebrewWordsToNumberConverter"),
                "kurdish" => Parameterless("KurdishWordsToNumberConverter"),
                "luxembourgish" => Parameterless("LuxembourgishWordsToNumberConverter"),
                "hungarian" => Parameterless("HungarianWordsToNumberConverter"),
                "maltese" => Parameterless("MalteseWordsToNumberConverter"),
                "portuguese" => Parameterless("PortugueseWordsToNumberConverter"),
                "italian" => Parameterless("ItalianWordsToNumberConverter"),
                "romanian" => Parameterless("RomanianWordsToNumberConverter"),
                "slovenian" => Parameterless("SlovenianWordsToNumberConverter"),
                "spanish" => Parameterless("SpanishWordsToNumberConverter"),
                "persian" => Parameterless("PersianWordsToNumberConverter"),
                "dutch" => Parameterless("DutchWordsToNumberConverter"),
                "swedish" => Parameterless("SwedishWordsToNumberConverter"),
                "thai" => Parameterless("ThaiWordsToNumberConverter"),
                "icelandic" => Parameterless("IcelandicWordsToNumberConverter"),
                "norwegian-bokmal" => Parameterless("NorwegianBokmalWordsToNumberConverter"),
                "filipino" => Parameterless("FilipinoWordsToNumberConverter"),
                "indonesian" => Parameterless("IndonesianWordsToNumberConverter"),
                "malay" => Parameterless("MalayWordsToNumberConverter"),
                "slovak" => Parameterless("SlovakWordsToNumberConverter"),
                "japanese" => Parameterless("JapaneseWordsToNumberConverter"),
                "korean" => Parameterless("KoreanWordsToNumberConverter"),
                "vietnamese" => Parameterless("VietnameseWordsToNumberConverter"),
                "chinese" => Parameterless("ChineseWordsToNumberConverter"),
                _ => null
            };

        static string Parameterless(string typeName) => "new " + typeName + "()";

        static string CultureConstructor(string typeName) => "new " + typeName + "(culture)";

        static string? StringConstructor(string typeName, string? argument) =>
            argument is null ? null : "new " + typeName + "(" + Quote(argument) + ")";

        static string Quote(string value) => "\"" + Escape(value) + "\"";
    }

    static string Escape(string value) => value.Replace("\\", "\\\\").Replace("\"", "\\\"");

    static string SanitizeIdentifier(string key)
    {
        var builder = new StringBuilder(key.Length);
        foreach (var c in key)
        {
            builder.Append(char.IsLetterOrDigit(c) || c == '_' ? c : '_');
        }

        if (builder.Length == 0 || !char.IsLetter(builder[0]) && builder[0] != '_')
        {
            builder.Insert(0, '_');
        }

        return builder.ToString();
    }

    sealed class RegistryEntry(string locale, string profile, string? argument)
    {
        public string Locale { get; } = locale;
        public string Profile { get; } = profile;
        public string? Argument { get; } = argument;
    }

    sealed class TokenMapLocaleDefinition(
        string name,
        string normalizationProfile,
        string? ordinalBuilder,
        ImmutableArray<string> negativePrefixes,
        ImmutableArray<string> ignoredTokens,
        ImmutableArray<string> tokenSuffixesToStrip,
        ImmutableArray<string> ordinalAbbreviationSuffixes,
        bool useHundredMultiplier,
        bool allowInvariantIntegerInput,
        long? scaleThreshold,
        ImmutableArray<TokenMapEntry> cardinalEntries)
    {
        public string Name { get; } = name;
        public string NormalizationProfile { get; } = normalizationProfile;
        public string? OrdinalBuilder { get; } = ordinalBuilder;
        public ImmutableArray<string> NegativePrefixes { get; } = negativePrefixes;
        public ImmutableArray<string> IgnoredTokens { get; } = ignoredTokens;
        public ImmutableArray<string> TokenSuffixesToStrip { get; } = tokenSuffixesToStrip;
        public ImmutableArray<string> OrdinalAbbreviationSuffixes { get; } = ordinalAbbreviationSuffixes;
        public bool UseHundredMultiplier { get; } = useHundredMultiplier;
        public bool AllowInvariantIntegerInput { get; } = allowInvariantIntegerInput;
        public long? ScaleThreshold { get; } = scaleThreshold;
        public ImmutableArray<TokenMapEntry> CardinalEntries { get; } = cardinalEntries;
    }

    sealed class TokenMapEntry(string key, long value)
    {
        public string Key { get; } = key;
        public long Value { get; } = value;
    }

    enum GrammaticalGenderVariant
    {
        None,
        MasculineAndFeminine,
        All
    }

    static class Diagnostics
    {
        public static readonly DiagnosticDescriptor DuplicateLocale = new(
            id: "HSG001",
            title: "Duplicate locale registration",
            messageFormat: "Locale '{0}' appears more than once in manifest '{1}'",
            category: "Humanizer.Generators",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor InvalidManifestLine = new(
            id: "HSG002",
            title: "Invalid registry manifest line",
            messageFormat: "Manifest '{0}' has an invalid entry on line {1}",
            category: "Humanizer.Generators",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor InvalidTokenMapXml = new(
            id: "HSG003",
            title: "Invalid token-map source data",
            messageFormat: "Token map source data is invalid: {0}",
            category: "Humanizer.Generators",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor UnknownRegistryProfile = new(
            id: "HSG004",
            title: "Unknown registry profile",
            messageFormat: "Registry '{0}' does not recognize profile '{1}' for locale '{2}'",
            category: "Humanizer.Generators",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);
    }
}

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
    delegate string? JsonConstructorArgumentEmitter(JsonElement element);

    static class RegistryExpressionFactory
    {
        public static string? Create(
            string registryName,
            string profile,
            string? argument,
            ImmutableHashSet<string> dataBackedFormatterProfiles,
            ImmutableHashSet<string> dataBackedOrdinalizerProfiles) =>
            registryName switch
            {
                "CollectionFormatterRegistry" => CreateCollectionFormatter(profile, argument),
                "DateOnlyToOrdinalWordsConverterRegistry" => CreateDateOnlyToOrdinalWords(profile),
                "DateToOrdinalWordsConverterRegistry" => CreateDateToOrdinalWords(profile),
                "FormatterRegistry" => CreateFormatter(profile, dataBackedFormatterProfiles),
                "NumberToWordsConverterRegistry" => CreateNumberToWords(profile),
                "OrdinalizerRegistry" => CreateOrdinalizer(profile, dataBackedOrdinalizerProfiles),
                "TimeOnlyToClockNotationConvertersRegistry" => CreateTimeOnlyToClockNotation(profile),
                "WordsToNumberConverterRegistry" => CreateWordsToNumber(profile, argument),
                _ => null
            };

        static string? CreateCollectionFormatter(string profile, string? argument) =>
            profile switch
            {
                "oxford" => Parameterless("OxfordStyleCollectionFormatter"),
                "conjunction" => StringConstructor("DefaultCollectionFormatter", argument),
                "clitic" => StringConstructor("CliticCollectionFormatter", argument),
                "delimited" => StringConstructor("DelimitedCollectionFormatter", argument),
                _ => null
            };

        static string? CreateDateOnlyToOrdinalWords(string profile) =>
            "DateOnlyToOrdinalWordsProfileCatalog.Resolve(" + Quote(profile) + ")";

        static string? CreateDateToOrdinalWords(string profile) =>
            "DateToOrdinalWordsProfileCatalog.Resolve(" + Quote(profile) + ")";

        static string? CreateFormatter(string profile, ImmutableHashSet<string> dataBackedFormatterProfiles) =>
            dataBackedFormatterProfiles.Contains(profile)
                ? "FormatterProfileCatalog.Resolve(" + Quote(profile) + ", culture)"
                : "new " + CreateTypeName(profile, "Formatter") + "(culture)";

        static string? CreateNumberToWords(string profile) =>
            "NumberToWordsProfileCatalog.Resolve(" + Quote(profile) + ", culture)";

        static string? CreateOrdinalizer(string profile, ImmutableHashSet<string> dataBackedOrdinalizerProfiles) =>
            dataBackedOrdinalizerProfiles.Contains(profile)
                ? "OrdinalizerProfileCatalog.Resolve(" + Quote(profile) + ", culture)"
                : "new " + ToEnumMemberName(profile) + "Ordinalizer()";

        static string? CreateTimeOnlyToClockNotation(string profile) =>
            "TimeOnlyToClockNotationProfileCatalog.Resolve(" + Quote(profile) + ")";

        static string? CreateWordsToNumber(string profile, string? argument) =>
            profile switch
            {
                "lexicon" => argument is null ? null : "TokenMapWordsToNumberConverters." + GetTokenMapPropertyName(argument),
                _ => "WordsToNumberProfileCatalog.Resolve(" + Quote(profile) + ")"
            };

        static string Parameterless(string typeName) => "new " + typeName + "()";

        static string? StringConstructor(string typeName, string? argument) =>
            argument is null ? null : "new " + typeName + "(" + Quote(argument) + ")";

        static string Quote(string value) => "\"" + Escape(value) + "\"";
    }

    static string GetTokenMapPropertyName(string localeCode)
    {
        var segments = localeCode.Split(['-', '_'], StringSplitOptions.RemoveEmptyEntries);
        var builder = new StringBuilder(localeCode.Length);

        foreach (var segment in segments)
        {
            var lower = segment.ToLowerInvariant();
            builder.Append(char.ToUpperInvariant(lower[0]));
            if (lower.Length > 1)
            {
                builder.Append(lower, 1, lower.Length - 1);
            }
        }

        return builder.ToString();
    }

    static string Escape(string value) => value.Replace("\\", "\\\\").Replace("\"", "\\\"");

    static string QuoteLiteral(string value) => "\"" + Escape(value) + "\"";

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

    static string GetCatalogPropertyName(string profileName) => SanitizeIdentifier(profileName);

    static void AppendLazyCachedMember(
        StringBuilder builder,
        string indent,
        string accessModifier,
        string typeName,
        string propertyName,
        string expression)
    {
        var cacheName = propertyName + "_cache";
        builder.Append(indent);
        builder.Append(accessModifier);
        builder.Append(' ');
        builder.Append(typeName);
        builder.Append(' ');
        builder.Append(propertyName);
        builder.Append(" => ");
        builder.Append(cacheName);
        builder.AppendLine(".Value;");
        builder.AppendLine();
        builder.Append(indent);
        builder.Append("static class ");
        builder.Append(cacheName);
        builder.AppendLine();
        builder.Append(indent);
        builder.AppendLine("{");
        builder.Append(indent);
        builder.Append("    internal static readonly ");
        builder.Append(typeName);
        builder.Append(" Value = ");
        builder.Append(expression);
        builder.AppendLine(";");
        builder.Append(indent);
        builder.AppendLine("}");
    }

    static string CreateParameterizedExpression(string typeName, string argumentExpression, string? trailingArgument = null)
    {
        if (string.IsNullOrEmpty(trailingArgument))
        {
            return "new " + typeName + "(" + argumentExpression + ")";
        }

        return "new " + typeName + "(" + argumentExpression + ", " + trailingArgument + ")";
    }

    static string GetConventionalNumberToWordsTypeName(string engine) => CreateTypeName(engine, "NumberToWordsConverter");

    static string GetConventionalWordsToNumberTypeName(string engine) => CreateTypeName(engine, "WordsToNumberConverter");

    static string GetConventionalTimeOnlyToClockNotationTypeName(string engine) => CreateTypeName(engine, "TimeOnlyToClockNotationConverter");

    static string GetClockNotationEngineTypeName(string engine) => CreateTypeName(engine, "ClockNotationConverter");

    static string ToEnumMemberName(string value)
    {
        var segments = value.Split(['-', '_'], StringSplitOptions.RemoveEmptyEntries);
        var builder = new StringBuilder(value.Length);

        foreach (var segment in segments)
        {
            var lower = segment.ToLowerInvariant();
            builder.Append(char.ToUpperInvariant(lower[0]));
            if (lower.Length > 1)
            {
                builder.Append(lower, 1, lower.Length - 1);
            }
        }

        return builder.ToString();
    }

    static string GetRequiredString(JsonElement element, string propertyName) =>
        GetOptionalString(element, propertyName) ?? throw new InvalidOperationException($"Missing required property '{propertyName}'.");

    static string? GetOptionalString(JsonElement element, string propertyName) =>
        element.TryGetProperty(propertyName, out var property) && property.ValueKind == JsonValueKind.String
            ? property.GetString()
            : null;

    static bool GetBoolean(JsonElement element, string propertyName) =>
        element.TryGetProperty(propertyName, out var valueElement) &&
        valueElement.ValueKind == JsonValueKind.True;

    static JsonElement GetRequiredProperty(JsonElement element, string propertyName) =>
        element.TryGetProperty(propertyName, out var property)
            ? property
            : throw new InvalidOperationException($"Missing required property '{propertyName}'.");

    static long GetRequiredInt64(JsonElement element, string propertyName)
    {
        var property = GetRequiredProperty(element, propertyName);
        return property.ValueKind == JsonValueKind.Number && property.TryGetInt64(out var value)
            ? value
            : throw new InvalidOperationException($"Property '{propertyName}' must be an integer.");
    }

    static long? GetOptionalInt64(JsonElement element, string propertyName)
    {
        if (!element.TryGetProperty(propertyName, out var property) || property.ValueKind != JsonValueKind.Number)
        {
            return null;
        }

        return property.TryGetInt64(out var value) ? value : null;
    }

    static string CreateStringArrayExpression(JsonElement arrayElement)
    {
        if (arrayElement.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidOperationException("Expected JSON array.");
        }

        var builder = new StringBuilder("new string[] { ");
        var first = true;

        foreach (var item in arrayElement.EnumerateArray())
        {
            if (item.ValueKind != JsonValueKind.String)
            {
                continue;
            }

            if (!first)
            {
                builder.Append(", ");
            }

            builder.Append(QuoteLiteral(item.GetString()!));
            first = false;
        }

        builder.Append(" }");
        return builder.ToString();
    }

    static string CreateOptionalStringArrayExpression(JsonElement element, string propertyName) =>
        element.TryGetProperty(propertyName, out var property) && property.ValueKind == JsonValueKind.Array
            ? CreateStringArrayExpression(property)
            : "Array.Empty<string>()";

    static string CreateFormatterSuffixMapExpression(JsonElement element, string propertyName)
    {
        if (!element.TryGetProperty(propertyName, out var property) || property.ValueKind != JsonValueKind.Object)
        {
            return "new FormatterSuffixMap(string.Empty, string.Empty, string.Empty, string.Empty)";
        }

        return "new FormatterSuffixMap(" +
               QuoteLiteral(GetOptionalString(property, "singular") ?? string.Empty) + ", " +
               QuoteLiteral(GetOptionalString(property, "dual") ?? string.Empty) + ", " +
               QuoteLiteral(GetOptionalString(property, "paucal") ?? string.Empty) + ", " +
               QuoteLiteral(GetOptionalString(property, "plural") ?? string.Empty) + ")";
    }

    static string CreateFormatterNumberDetectorExpression(JsonElement element, string propertyName) =>
        "FormatterNumberDetectorKind." + ToEnumMemberName(GetOptionalString(element, propertyName) ?? "none");

    static string CreateFormatterNumberFormExpression(JsonElement element, string propertyName, string defaultValue = "default") =>
        "FormatterNumberForm." + ToEnumMemberName(GetOptionalString(element, propertyName) ?? defaultValue);

    static string CreateFormatterFallbackTransformExpression(JsonElement element, string propertyName) =>
        "FormatterDataUnitFallbackTransform." + ToEnumMemberName(GetOptionalString(element, propertyName) ?? "none");

    static string CreateFormatterPrepositionModeExpression(JsonElement element, string propertyName) =>
        "FormatterPrepositionMode." + ToEnumMemberName(GetOptionalString(element, propertyName) ?? "none");

    static string CreateFormatterSecondaryPlaceholderModeExpression(JsonElement element, string propertyName) =>
        "FormatterSecondaryPlaceholderMode." + ToEnumMemberName(GetOptionalString(element, propertyName) ?? "none");

    static string CreateFormatterResourceKeyOverrideArrayExpression(JsonElement element, string propertyName)
    {
        if (!element.TryGetProperty(propertyName, out var property) || property.ValueKind != JsonValueKind.Array)
        {
            return "Array.Empty<FormatterResourceKeyOverride>()";
        }

        return CreateTypedConstructorArrayExpression(
            "FormatterResourceKeyOverride",
            property,
            RequiredInt64Argument("number"),
            RequiredStringArgument("suffix"),
            static item => item.TryGetProperty("keys", out var keys) && keys.ValueKind == JsonValueKind.Array
                ? CreateStringArrayExpression(keys)
                : "Array.Empty<string>()",
            static item => item.TryGetProperty("prefixes", out var prefixes) && prefixes.ValueKind == JsonValueKind.Array
                ? CreateStringArrayExpression(prefixes)
                : "Array.Empty<string>()");
    }

    static string CreateTimeUnitGenderMapExpression(JsonElement element, string propertyName)
    {
        if (!element.TryGetProperty(propertyName, out var property) || property.ValueKind != JsonValueKind.Object)
        {
            return "FrozenDictionary<TimeUnit, GrammaticalGender>.Empty";
        }

        var builder = new StringBuilder("new Dictionary<TimeUnit, GrammaticalGender> { ");
        var first = true;

        foreach (var entry in property.EnumerateObject())
        {
            if (entry.Value.ValueKind != JsonValueKind.String)
            {
                continue;
            }

            if (!first)
            {
                builder.Append(", ");
            }

            builder.Append("[TimeUnit.");
            builder.Append(ToEnumMemberName(entry.Name));
            builder.Append("] = GrammaticalGender.");
            builder.Append(ToEnumMemberName(entry.Value.GetString()!));
            first = false;
        }

        builder.Append(" }.ToFrozenDictionary()");
        return builder.ToString();
    }

    static string CreateTargetTypedConstructorExpression(JsonElement element, params JsonConstructorArgumentEmitter[] emitters)
    {
        if (element.ValueKind != JsonValueKind.Object)
        {
            throw new InvalidOperationException("Expected JSON object.");
        }

        var builder = new StringBuilder("new(");
        AppendConstructorArguments(builder, element, emitters);
        builder.Append(')');
        return builder.ToString();
    }

    static string CreateTypedConstructorArrayExpression(string elementTypeName, JsonElement arrayElement, params JsonConstructorArgumentEmitter[] emitters)
    {
        if (arrayElement.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidOperationException("Expected JSON array.");
        }

        var builder = new StringBuilder("new ");
        builder.Append(elementTypeName);
        builder.Append("[] { ");
        var first = true;

        foreach (var item in arrayElement.EnumerateArray())
        {
            if (item.ValueKind != JsonValueKind.Object)
            {
                continue;
            }

            if (!first)
            {
                builder.Append(", ");
            }

            builder.Append(CreateTargetTypedConstructorExpression(item, emitters));
            first = false;
        }

        builder.Append(" }");
        return builder.ToString();
    }

    static void AppendConstructorArguments(StringBuilder builder, JsonElement element, params JsonConstructorArgumentEmitter[] emitters)
    {
        var first = true;

        foreach (var emitter in emitters)
        {
            var argument = emitter(element);
            if (argument is null)
            {
                continue;
            }

            if (!first)
            {
                builder.Append(", ");
            }

            builder.Append(argument);
            first = false;
        }
    }

    static JsonConstructorArgumentEmitter RequiredInt64Argument(string propertyName, string suffix = "") =>
        element => GetRequiredInt64(element, propertyName).ToString(CultureInfo.InvariantCulture) + suffix;

    static JsonConstructorArgumentEmitter RequiredStringArgument(string propertyName) =>
        element => QuoteLiteral(GetRequiredString(element, propertyName));

    static JsonConstructorArgumentEmitter RequiredEnumArgument(string propertyName, string enumTypeName) =>
        element => enumTypeName + "." + ToEnumMemberName(GetRequiredString(element, propertyName));

    static JsonConstructorArgumentEmitter OptionalEnumArgument(string propertyName, string enumTypeName, string defaultValue) =>
        element => enumTypeName + "." + ToEnumMemberName(GetOptionalString(element, propertyName) ?? defaultValue);

    static JsonConstructorArgumentEmitter OptionalStringArgument(string propertyName, string defaultValue = "") =>
        element => QuoteLiteral(GetOptionalString(element, propertyName) ?? defaultValue);

    static JsonConstructorArgumentEmitter OptionalStringArgumentOrFallback(string propertyName, string fallbackPropertyName) =>
        element => QuoteLiteral(GetOptionalString(element, propertyName) ?? GetRequiredString(element, fallbackPropertyName));

    static JsonConstructorArgumentEmitter OptionalStringArgumentOmitIfMissing(string propertyName) =>
        element => GetOptionalString(element, propertyName) is { } value ? QuoteLiteral(value) : null;

    static JsonConstructorArgumentEmitter BooleanArgument(string propertyName) =>
        element => GetBoolean(element, propertyName) ? "true" : "false";

    static JsonConstructorArgumentEmitter OptionalTrueArgument(string propertyName) =>
        element => GetBoolean(element, propertyName) ? "true" : null;

    static JsonConstructorArgumentEmitter OptionalBooleanArgument(string propertyName) =>
        element => element.TryGetProperty(propertyName, out var property) && property.ValueKind is JsonValueKind.True or JsonValueKind.False
            ? (property.GetBoolean() ? "true" : "false")
            : null;

    static string CreateOptionalStringIntFrozenDictionaryExpression(JsonElement element, string propertyName) =>
        element.TryGetProperty(propertyName, out var property) && property.ValueKind == JsonValueKind.Object
            ? CreateStringIntFrozenDictionaryExpression(property)
            : "null";

    static string CreateStringIntFrozenDictionaryExpression(JsonElement objectElement) =>
        CreateStringNumberFrozenDictionaryExpression(objectElement, "int", static value => checked((int)value));

    static string CreateStringLongFrozenDictionaryExpression(JsonElement objectElement) =>
        CreateStringNumberFrozenDictionaryExpression(objectElement, "long", static value => value);

    static string CreateStringNumberFrozenDictionaryExpression(JsonElement objectElement, string valueType, Func<long, long> normalize)
    {
        if (objectElement.ValueKind != JsonValueKind.Object)
        {
            throw new InvalidOperationException("Expected JSON object.");
        }

        var builder = new StringBuilder();
        builder.Append("new Dictionary<string, ");
        builder.Append(valueType);
        builder.Append(">(StringComparer.Ordinal) { ");
        var first = true;

        foreach (var property in objectElement.EnumerateObject())
        {
            if (property.Value.ValueKind != JsonValueKind.Number || !property.Value.TryGetInt64(out var value))
            {
                continue;
            }

            if (!first)
            {
                builder.Append(", ");
            }

            builder.Append('[');
            builder.Append(QuoteLiteral(property.Name));
            builder.Append("] = ");
            builder.Append(normalize(value).ToString(CultureInfo.InvariantCulture));
            first = false;
        }

        builder.Append(" }.ToFrozenDictionary(StringComparer.Ordinal)");
        return builder.ToString();
    }

    static string CreateCharStringFrozenDictionaryExpression(JsonElement objectElement)
    {
        if (objectElement.ValueKind != JsonValueKind.Object)
        {
            throw new InvalidOperationException("Expected JSON object.");
        }

        var builder = new StringBuilder("new Dictionary<char, string> { ");
        var first = true;

        foreach (var property in objectElement.EnumerateObject())
        {
            if (property.Name.Length != 1 || property.Value.ValueKind != JsonValueKind.String)
            {
                continue;
            }

            if (!first)
            {
                builder.Append(", ");
            }

            builder.Append("['");
            builder.Append(EscapeCharLiteral(property.Name[0]));
            builder.Append("'] = ");
            builder.Append(QuoteLiteral(property.Value.GetString()!));
            first = false;
        }

        builder.Append(" }.ToFrozenDictionary()");
        return builder.ToString();
    }

    static string CreateCharIntFrozenDictionaryExpression(JsonElement objectElement)
    {
        if (objectElement.ValueKind != JsonValueKind.Object)
        {
            throw new InvalidOperationException("Expected JSON object.");
        }

        var builder = new StringBuilder("new Dictionary<char, int> { ");
        var first = true;

        foreach (var property in objectElement.EnumerateObject())
        {
            if (property.Name.Length != 1 ||
                property.Value.ValueKind != JsonValueKind.Number ||
                !property.Value.TryGetInt32(out var value))
            {
                continue;
            }

            if (!first)
            {
                builder.Append(", ");
            }

            builder.Append("['");
            builder.Append(EscapeCharLiteral(property.Name[0]));
            builder.Append("'] = ");
            builder.Append(value.ToString(CultureInfo.InvariantCulture));
            first = false;
        }

        builder.Append(" }.ToFrozenDictionary()");
        return builder.ToString();
    }

    static string CreateNullableCharStringFrozenDictionaryExpression(JsonElement element, string propertyName) =>
        element.TryGetProperty(propertyName, out var property) && property.ValueKind == JsonValueKind.Object
            ? CreateCharStringFrozenDictionaryExpression(property)
            : "null";

    static string CreateNullableIntStringFrozenDictionaryExpression(JsonElement element, string propertyName)
    {
        if (!element.TryGetProperty(propertyName, out var property) || property.ValueKind != JsonValueKind.Object)
        {
            return "null";
        }

        var builder = new StringBuilder("new Dictionary<int, string> { ");
        var first = true;

        foreach (var entry in property.EnumerateObject())
        {
            if (!int.TryParse(entry.Name, NumberStyles.Integer, CultureInfo.InvariantCulture, out var key) ||
                entry.Value.ValueKind != JsonValueKind.String)
            {
                continue;
            }

            if (!first)
            {
                builder.Append(", ");
            }

            builder.Append('[');
            builder.Append(key.ToString(CultureInfo.InvariantCulture));
            builder.Append("] = ");
            builder.Append(QuoteLiteral(entry.Value.GetString()!));
            first = false;
        }

        builder.Append(" }.ToFrozenDictionary()");
        return builder.ToString();
    }

    static string CreateOptionalIntStringFrozenDictionaryOrEmptyExpression(JsonElement element, string propertyName)
    {
        if (!element.TryGetProperty(propertyName, out var property) || property.ValueKind != JsonValueKind.Object)
        {
            return "FrozenDictionary<int, string>.Empty";
        }

        var builder = new StringBuilder("new Dictionary<int, string> { ");
        var first = true;

        foreach (var entry in property.EnumerateObject())
        {
            if (!int.TryParse(entry.Name, NumberStyles.Integer, CultureInfo.InvariantCulture, out var key) ||
                entry.Value.ValueKind != JsonValueKind.String)
            {
                continue;
            }

            if (!first)
            {
                builder.Append(", ");
            }

            builder.Append('[');
            builder.Append(key.ToString(CultureInfo.InvariantCulture));
            builder.Append("] = ");
            builder.Append(QuoteLiteral(entry.Value.GetString()!));
            first = false;
        }

        builder.Append(" }.ToFrozenDictionary()");
        return builder.ToString();
    }

    static string CreateOptionalStringStringFrozenDictionaryExpression(JsonElement element, string propertyName) =>
        element.TryGetProperty(propertyName, out var property) && property.ValueKind == JsonValueKind.Object
            ? CreateStringStringFrozenDictionaryExpression(property)
            : "FrozenDictionary<string, string>.Empty";

    static string CreateStringStringFrozenDictionaryExpression(JsonElement objectElement)
    {
        if (objectElement.ValueKind != JsonValueKind.Object)
        {
            throw new InvalidOperationException("Expected JSON object.");
        }

        var builder = new StringBuilder("new Dictionary<string, string>(StringComparer.Ordinal) { ");
        var first = true;

        foreach (var property in objectElement.EnumerateObject())
        {
            if (property.Value.ValueKind != JsonValueKind.String)
            {
                continue;
            }

            if (!first)
            {
                builder.Append(", ");
            }

            builder.Append('[');
            builder.Append(QuoteLiteral(property.Name));
            builder.Append("] = ");
            builder.Append(QuoteLiteral(property.Value.GetString()!));
            first = false;
        }

        builder.Append(" }.ToFrozenDictionary(StringComparer.Ordinal)");
        return builder.ToString();
    }

    static string CreateTurkicScaleArrayExpression(JsonElement arrayElement)
    {
        if (arrayElement.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidOperationException("Expected JSON array.");
        }

        var builder = new StringBuilder("new TurkicScale[] { ");
        var first = true;

        foreach (var item in arrayElement.EnumerateArray())
        {
            if (item.ValueKind != JsonValueKind.Object)
            {
                continue;
            }

            if (!first)
            {
                builder.Append(", ");
            }

            builder.Append("new(");
            builder.Append(GetRequiredInt64(item, "value").ToString(CultureInfo.InvariantCulture));
            builder.Append(", ");
            builder.Append(QuoteLiteral(GetRequiredString(item, "name")));

            var omitOneWhenSingular = GetBoolean(item, "omitOneWhenSingular");
            var allowBareHundredInCount = GetBoolean(item, "allowBareHundredInCount");

            if (omitOneWhenSingular || allowBareHundredInCount)
            {
                builder.Append(", ");
                builder.Append(omitOneWhenSingular ? "true" : "false");
            }

            if (allowBareHundredInCount)
            {
                builder.Append(", true");
            }

            builder.Append(')');
            first = false;
        }

        builder.Append(" }");
        return builder.ToString();
    }

    static string CreateMalayScaleArrayExpression(JsonElement arrayElement)
    {
        if (arrayElement.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidOperationException("Expected JSON array.");
        }

        var builder = new StringBuilder("new MalayFamilyNumberToWordsConverter.Scale[] { ");
        var first = true;

        foreach (var item in arrayElement.EnumerateArray())
        {
            if (item.ValueKind != JsonValueKind.Object)
            {
                continue;
            }

            if (!first)
            {
                builder.Append(", ");
            }

            builder.Append("new(");
            builder.Append(GetRequiredInt64(item, "value").ToString(CultureInfo.InvariantCulture));
            builder.Append(", ");
            builder.Append(QuoteLiteral(GetRequiredString(item, "name")));
            if (GetOptionalString(item, "oneWord") is { } oneWord)
            {
                builder.Append(", ");
                builder.Append(QuoteLiteral(oneWord));
            }

            builder.Append(')');
            first = false;
        }

        builder.Append(" }");
        return builder.ToString();
    }

    static string CreateLinkingScaleArrayExpression(JsonElement arrayElement)
        => CreateTypedConstructorArrayExpression(
            "LinkingScale",
            arrayElement,
            RequiredInt64Argument("value"),
            RequiredStringArgument("name"));

    static string CreateLinkingSuffixRuleArrayExpression(JsonElement arrayElement)
        => CreateTypedConstructorArrayExpression(
            "LinkingSuffixRule",
            arrayElement,
            RequiredStringArgument("suffix"),
            RequiredStringArgument("replacement"));

    static string CreateAgglutinativeOrdinalScaleArrayExpression(JsonElement arrayElement)
        => CreateTypedConstructorArrayExpression(
            "AgglutinativeOrdinalScale",
            arrayElement,
            RequiredInt64Argument("value"),
            RequiredStringArgument("singularCardinal"),
            RequiredStringArgument("pluralCardinal"),
            RequiredStringArgument("ordinalWord"));

    static string CreateContextualDecimalScaleArrayExpression(JsonElement arrayElement)
        => CreateTypedConstructorArrayExpression(
            "ContextualDecimalScale",
            arrayElement,
            RequiredInt64Argument("value"),
            RequiredStringArgument("name"));

    static string CreateEnglishFamilyScaleArrayExpression(JsonElement arrayElement)
    {
        if (arrayElement.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidOperationException("Expected JSON array.");
        }

        var builder = new StringBuilder("new EnglishFamilyScale[] { ");
        var first = true;

        foreach (var item in arrayElement.EnumerateArray())
        {
            if (item.ValueKind != JsonValueKind.Object)
            {
                continue;
            }

            if (!first)
            {
                builder.Append(", ");
            }

            builder.Append("new(");
            builder.Append(GetRequiredInt64(item, "value").ToString(CultureInfo.InvariantCulture));
            builder.Append(", ");
            builder.Append(QuoteLiteral(GetRequiredString(item, "name")));
            builder.Append(", ");
            builder.Append(QuoteLiteral(GetRequiredString(item, "ordinalName")));
            builder.Append(')');
            first = false;
        }

        builder.Append(" }");
        return builder.ToString();
    }

    static string CreateJoinedScaleArrayExpression(JsonElement arrayElement)
        => CreateTypedConstructorArrayExpression(
            "JoinedScale",
            arrayElement,
            RequiredInt64Argument("value"),
            RequiredStringArgument("name"),
            OptionalTrueArgument("omitOneWhenSingular"));

    static string CreateOrdinalPrefixScaleArrayExpression(JsonElement arrayElement)
        => CreateTypedConstructorArrayExpression(
            "OrdinalPrefixScale",
            arrayElement,
            RequiredInt64Argument("value"),
            RequiredStringArgument("singular"),
            RequiredStringArgument("plural"),
            RequiredStringArgument("ordinalPrefix"),
            RequiredEnumArgument("gender", "GrammaticalGender"));

    static string CreateNullableIntFrozenSetExpression(JsonElement element, string propertyName)
    {
        if (!element.TryGetProperty(propertyName, out var property) || property.ValueKind != JsonValueKind.Array)
        {
            return "null";
        }

        var builder = new StringBuilder("new[] { ");
        var first = true;

        foreach (var item in property.EnumerateArray())
        {
            if (item.ValueKind != JsonValueKind.Number || !item.TryGetInt64(out var value))
            {
                continue;
            }

            if (!first)
            {
                builder.Append(", ");
            }

            builder.Append(checked((int)value).ToString(CultureInfo.InvariantCulture));
            first = false;
        }

        builder.Append(" }.ToFrozenSet()");
        return builder.ToString();
    }

    static string CreateOptionalIntFrozenSetOrEmptyExpression(JsonElement element, string propertyName)
    {
        var expression = CreateNullableIntFrozenSetExpression(element, propertyName);
        return expression == "null"
            ? "FrozenSet<int>.Empty"
            : expression;
    }

    static string CreateEastSlavicScaleArrayExpression(JsonElement arrayElement)
        => CreateTypedConstructorArrayExpression(
            "EastSlavicScale",
            arrayElement,
            RequiredInt64Argument("value", "UL"),
            RequiredEnumArgument("gender", "GrammaticalGender"),
            RequiredStringArgument("singular"),
            RequiredStringArgument("paucal"),
            RequiredStringArgument("plural"),
            OptionalStringArgumentOmitIfMissing("ordinalStem"));

    static string CreatePluralizedScaleArrayExpression(JsonElement arrayElement)
        => CreateTypedConstructorArrayExpression(
            "PluralizedScale",
            arrayElement,
            RequiredInt64Argument("value", "UL"),
            RequiredEnumArgument("countGender", "GrammaticalGender"),
            RequiredStringArgument("singular"),
            RequiredStringArgument("paucal"),
            RequiredStringArgument("plural"),
            element =>
            {
                if (GetOptionalString(element, "ordinalStem") is { } ordinalStem)
                {
                    return QuoteLiteral(ordinalStem);
                }

                return element.TryGetProperty("omitLeadingOne", out var property) && property.ValueKind is JsonValueKind.True or JsonValueKind.False
                    ? "null"
                    : null;
            },
            OptionalBooleanArgument("omitLeadingOne"));

    static string CreateEastSlavicGenderEndingExpression(JsonElement objectElement) =>
        "new(" +
        QuoteLiteral(GetRequiredString(objectElement, "default")) + ", " +
        CreateNullableIntStringFrozenDictionaryExpression(objectElement, "overrides").Replace("null", "FrozenDictionary<int, string>.Empty") +
        ")";

    static string CreateSouthSlavicScaleArrayExpression(JsonElement arrayElement)
        => CreateTypedConstructorArrayExpression(
            "SouthSlavicScale",
            arrayElement,
            RequiredInt64Argument("value", "UL"),
            RequiredEnumArgument("gender", "GrammaticalGender"),
            OptionalStringArgumentOrFallback("oneForm", "singular"),
            RequiredStringArgument("singular"),
            RequiredStringArgument("paucal"),
            RequiredStringArgument("plural"),
            OptionalStringArgument("dual"),
            OptionalStringArgument("trialQuadral"));

    static string CreateInvertedTensScaleArrayExpression(JsonElement arrayElement)
        => CreateTypedConstructorArrayExpression(
            "InvertedTensScale",
            arrayElement,
            RequiredInt64Argument("value"),
            RequiredStringArgument("oneForm"),
            RequiredStringArgument("manyForm"),
            RequiredStringArgument("countJoiner"),
            RequiredStringArgument("remainderSeparator"));

    static string CreateScandinavianScaleArrayExpression(JsonElement arrayElement)
        => CreateTypedConstructorArrayExpression(
            "ScandinavianScale",
            arrayElement,
            RequiredInt64Argument("value"),
            RequiredStringArgument("name"),
            RequiredStringArgument("plural"),
            OptionalStringArgument("prefix"),
            OptionalStringArgument("postfix"),
            OptionalStringArgument("pluralSuffix"),
            OptionalStringArgument("ordinalSuffix"),
            BooleanArgument("displayOneUnit"),
            OptionalEnumArgument("gender", "GrammaticalGender", "masculine"));

    static string CreateGermanFamilyScaleArrayExpression(JsonElement arrayElement)
        => CreateTypedConstructorArrayExpression(
            "GermanFamilyScale",
            arrayElement,
            RequiredInt64Argument("value"),
            BooleanArgument("addSpaceBeforeNextPart"),
            OptionalEnumArgument("countGender", "GrammaticalGender", "masculine"),
            RequiredStringArgument("singularCardinal"),
            RequiredStringArgument("pluralCardinalFormat"),
            static element => element.TryGetProperty("ordinalSingular", out var ordinalSingular) && ordinalSingular.ValueKind == JsonValueKind.Array
                ? CreateStringArrayExpression(ordinalSingular)
                : throw new InvalidOperationException("Missing required property 'ordinalSingular'."),
            static element => element.TryGetProperty("ordinalPlural", out var ordinalPlural) && ordinalPlural.ValueKind == JsonValueKind.Array
                ? CreateStringArrayExpression(ordinalPlural)
                : throw new InvalidOperationException("Missing required property 'ordinalPlural'."),
            OptionalStringArgument("countWordFormNextWord"));

    static string CreateConjoinedGenderedScaleArrayExpression(JsonElement arrayElement)
        => CreateTypedConstructorArrayExpression(
            "ConjoinedGenderedScale",
            arrayElement,
            RequiredInt64Argument("divisor"),
            RequiredEnumArgument("gender", "GrammaticalGender"),
            RequiredStringArgument("singular"),
            RequiredStringArgument("plural"),
            RequiredStringArgument("ordinalStem"));

    static string CreateSegmentedScaleArrayExpression(JsonElement arrayElement)
        => CreateTypedConstructorArrayExpression(
            "SegmentedScale",
            arrayElement,
            RequiredInt64Argument("value"),
            RequiredStringArgument("singular"),
            RequiredStringArgument("plural"),
            RequiredEnumArgument("countVariant", "SegmentedScaleVariant"),
            RequiredEnumArgument("singularRemainderVariant", "SegmentedScaleVariant"),
            RequiredEnumArgument("pluralRemainderVariant", "SegmentedScaleVariant"));

    static string CreateTerminalOrdinalScaleArrayExpression(JsonElement arrayElement)
        => CreateTypedConstructorArrayExpression(
            "TerminalOrdinalScale",
            arrayElement,
            RequiredInt64Argument("value"),
            RequiredStringArgument("exactSingularCardinal"),
            RequiredStringArgument("singularWithRemainderCardinal"),
            RequiredStringArgument("pluralCardinal"),
            RequiredStringArgument("ordinalStem"));

    static string CreateConstructStateScaleArrayExpression(JsonElement arrayElement)
        => CreateTypedConstructorArrayExpression(
            "ConstructStateScale",
            arrayElement,
            element => checked((int)GetRequiredInt64(element, "value")).ToString(CultureInfo.InvariantCulture),
            RequiredStringArgument("singular"),
            RequiredStringArgument("dualPrefix"),
            RequiredEnumArgument("countGender", "GrammaticalGender"));

    static string CreateHyphenatedScaleExpression(JsonElement element) =>
        CreateTargetTypedConstructorExpression(
            element,
            RequiredInt64Argument("divisor"),
            RequiredStringArgument("cardinal"),
            RequiredStringArgument("ordinal"));

    static string CreateHyphenatedScaleArrayExpression(JsonElement arrayElement)
        => CreateTypedConstructorArrayExpression(
            "HyphenatedScale",
            arrayElement,
            RequiredInt64Argument("divisor"),
            RequiredStringArgument("cardinal"),
            RequiredStringArgument("ordinal"));

    static string CreateDualFormScaleExpression(JsonElement element) =>
        CreateTargetTypedConstructorExpression(
            element,
            RequiredStringArgument("singular"),
            RequiredStringArgument("dual"),
            RequiredStringArgument("plural"),
            BooleanArgument("usePrefixMapForLowerDigits"));

    static string CreateTriadScaleArrayExpression(JsonElement arrayElement)
        => CreateTypedConstructorArrayExpression(
            "TriadScaleNumberToWordsConverter.TriadScale",
            arrayElement,
            element => checked((int)GetRequiredInt64(element, "value")).ToString(CultureInfo.InvariantCulture),
            RequiredStringArgument("singular"),
            RequiredStringArgument("plural"),
            RequiredStringArgument("countToScaleJoiner"),
            BooleanArgument("countUsesFinalAccent"),
            BooleanArgument("appendTrailingSpace"),
            RequiredStringArgument("ordinalCompactionMatch"),
            RequiredStringArgument("ordinalCompactionReplacement"),
            BooleanArgument("removeLeadingOneOnExactOrdinal"),
            RequiredStringArgument("exactOrdinalSuffix"));

    static string CreateGenderedScaleOrdinalArrayExpression(JsonElement arrayElement)
        => CreateTypedConstructorArrayExpression(
            "GenderedScaleOrdinalNumberToWordsConverter.Scale",
            arrayElement,
            RequiredStringArgument("singular"),
            RequiredStringArgument("plural"),
            RequiredEnumArgument("countGender", "GrammaticalGender"));

    static string CreateLongScaleWordArrayExpression(JsonElement arrayElement)
        => CreateTypedConstructorArrayExpression(
            "LongScaleStemOrdinalNumberToWordsConverter.LargeScale",
            arrayElement,
            RequiredInt64Argument("value"),
            RequiredStringArgument("singularPrefix"),
            RequiredStringArgument("singular"),
            RequiredStringArgument("plural"));

    static string CreateWestSlavicScaleFormsExpression(JsonElement objectElement)
    {
        var builder = new StringBuilder("new(");
        builder.Append(QuoteLiteral(GetRequiredString(objectElement, "singular")));
        builder.Append(", ");
        builder.Append(QuoteLiteral(GetRequiredString(objectElement, "paucal")));
        builder.Append(", ");
        builder.Append(QuoteLiteral(GetRequiredString(objectElement, "plural")));

        if (GetBoolean(objectElement, "omitLeadingOne"))
        {
            builder.Append(", true");
        }

        builder.Append(')');
        return builder.ToString();
    }

    static string CreateInvertedTensTokenArrayExpression(JsonElement arrayElement)
    {
        if (arrayElement.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidOperationException("Expected JSON array.");
        }

        var builder = new StringBuilder("new InvertedTensToken[] { ");
        var first = true;

        foreach (var item in arrayElement.EnumerateArray())
        {
            if (item.ValueKind != JsonValueKind.Object)
            {
                continue;
            }

            if (!first)
            {
                builder.Append(", ");
            }

            builder.Append("new(");
            builder.Append(QuoteLiteral(GetRequiredString(item, "word")));
            builder.Append(", ");
            builder.Append(GetRequiredInt64(item, "value").ToString(CultureInfo.InvariantCulture));
            builder.Append(')');
            first = false;
        }

        builder.Append(" }");
        return builder.ToString();
    }

    static string CreateStringReplacementArrayExpression(JsonElement element, string propertyName)
    {
        if (!element.TryGetProperty(propertyName, out var property) || property.ValueKind != JsonValueKind.Array)
        {
            return "Array.Empty<StringReplacement>()";
        }

        var builder = new StringBuilder("new StringReplacement[] { ");
        var first = true;

        foreach (var item in property.EnumerateArray())
        {
            if (item.ValueKind != JsonValueKind.Object)
            {
                continue;
            }

            if (!first)
            {
                builder.Append(", ");
            }

            builder.Append("new(");
            builder.Append(QuoteLiteral(GetOptionalString(item, "oldValue") ?? GetRequiredString(item, "old")));
            builder.Append(", ");
            builder.Append(QuoteLiteral(GetOptionalString(item, "newValue") ?? GetRequiredString(item, "new")));
            builder.Append(')');
            first = false;
        }

        builder.Append(" }");
        return builder.ToString();
    }

    static string CreateSuffixScaleWordArrayExpression(JsonElement arrayElement)
        => CreateTypedConstructorArrayExpression(
            "SuffixScaleWord",
            arrayElement,
            RequiredStringArgument("singular"),
            RequiredStringArgument("plural"),
            RequiredInt64Argument("value"));

    static string CreatePrefixedScaleWordArrayExpression(JsonElement arrayElement)
        => CreateTypedConstructorArrayExpression(
            "PrefixedScaleWord",
            arrayElement,
            RequiredStringArgument("token"),
            element => checked((int)GetRequiredInt64(element, "value")).ToString(CultureInfo.InvariantCulture));

    static string CreatePrefixedTensRuleArrayExpression(JsonElement arrayElement)
        => CreateTypedConstructorArrayExpression(
            "PrefixedTensRule",
            arrayElement,
            RequiredStringArgument("prefix"),
            element => checked((int)GetRequiredInt64(element, "baseValue")).ToString(CultureInfo.InvariantCulture));

    static string CreateIntFrozenSetExpression(JsonElement arrayElement)
    {
        if (arrayElement.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidOperationException("Expected JSON array.");
        }

        var builder = new StringBuilder("new[] { ");
        var first = true;

        foreach (var item in arrayElement.EnumerateArray())
        {
            if (item.ValueKind != JsonValueKind.Number || !item.TryGetInt64(out var value))
            {
                continue;
            }

            if (!first)
            {
                builder.Append(", ");
            }

            builder.Append(checked((int)value).ToString(CultureInfo.InvariantCulture));
            first = false;
        }

        builder.Append(" }.ToFrozenSet()");
        return builder.ToString();
    }

    static string CreateWordsOrdinalMapExpression(JsonElement root, string builderMethodName)
    {
        if (root.TryGetProperty("ordinalNumberToWordsKind", out var ordinalProfile) && ordinalProfile.ValueKind == JsonValueKind.String)
        {
            return builderMethodName + "(NumberToWordsProfileCatalog.Resolve(" +
                QuoteLiteral(ordinalProfile.GetString()!) +
                ", CultureInfo.InvariantCulture))";
        }

        return root.TryGetProperty("ordinalMap", out var ordinalMap) && ordinalMap.ValueKind == JsonValueKind.Object
            ? CreateStringIntFrozenDictionaryExpression(ordinalMap)
            : "new Dictionary<string, int>(StringComparer.Ordinal).ToFrozenDictionary(StringComparer.Ordinal)";
    }

    static string CreateOptionalSequenceMultiplierThreshold(JsonElement root)
    {
        var value = GetOptionalInt64(root, "sequenceMultiplierThreshold");
        return value.HasValue
            ? ", " + value.Value.ToString(CultureInfo.InvariantCulture)
            : string.Empty;
    }

    static string CreateConventionalNumberToWordsExpression(string engine, bool useCultureParameter)
    {
        var typeName = GetConventionalNumberToWordsTypeName(engine);
        return useCultureParameter
            ? "new " + typeName + "(culture)"
            : "new " + typeName + "()";
    }

    static string CreateConventionalWordsToNumberExpression(string engine) =>
        "new " + GetConventionalWordsToNumberTypeName(engine) + "()";

    static string CreateConventionalTimeOnlyToClockNotationExpression(string engine) =>
        "new " + GetConventionalTimeOnlyToClockNotationTypeName(engine) + "()";

    static string CreateTypeName(string value, string suffix) => ToEnumMemberName(value) + suffix;

    static string EscapeCharLiteral(char value) =>
        value switch
        {
            '\'' => "\\'",
            '\\' => "\\\\",
            _ => value.ToString()
        };

}

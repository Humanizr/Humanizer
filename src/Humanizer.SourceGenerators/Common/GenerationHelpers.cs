using System.Collections.Immutable;
using System.Globalization;
using System.Text;
using System.Text.Json;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Humanizer.SourceGenerators;

public sealed partial class HumanizerSourceGenerator
{
    delegate string? JsonConstructorValueEmitter(JsonElement element);

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
            profile == "default"
                ? "new DefaultDateOnlyToOrdinalWordConverter()"
                : "DateOnlyToOrdinalWordsProfileCatalog.Resolve(" + Quote(profile) + ")";

        static string? CreateDateToOrdinalWords(string profile) =>
            profile == "default"
                ? "new DefaultDateToOrdinalWordConverter()"
                : "DateToOrdinalWordsProfileCatalog.Resolve(" + Quote(profile) + ")";

        static string? CreateFormatter(string profile, ImmutableHashSet<string> dataBackedFormatterProfiles) =>
            profile == "default"
                ? "new DefaultFormatter(culture)"
                : dataBackedFormatterProfiles.Contains(profile)
                ? "FormatterProfileCatalog.Resolve(" + Quote(profile) + ", culture)"
                : "new " + CreateTypeName(profile, "Formatter") + "(culture)";

        static string? CreateNumberToWords(string profile) =>
            "NumberToWordsProfileCatalog.Resolve(" + Quote(profile) + ", culture)";

        static string? CreateOrdinalizer(string profile, ImmutableHashSet<string> dataBackedOrdinalizerProfiles) =>
            profile == "default"
                ? "new DefaultOrdinalizer()"
                : dataBackedOrdinalizerProfiles.Contains(profile)
                ? "OrdinalizerProfileCatalog.Resolve(" + Quote(profile) + ", culture)"
                : "new " + ToEnumMemberName(profile) + "Ordinalizer()";

        static string? CreateTimeOnlyToClockNotation(string profile) =>
            profile == "default"
                ? "TimeOnlyToClockNotationProfileCatalog.Resolve(" + Quote("en") + ")"
                : "TimeOnlyToClockNotationProfileCatalog.Resolve(" + Quote(profile) + ")";

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

        var identifier = builder.ToString();
        return SyntaxFacts.GetKeywordKind(identifier) != SyntaxKind.None ||
               SyntaxFacts.GetContextualKeywordKind(identifier) != SyntaxKind.None
            ? "_" + identifier
            : identifier;
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

    static string CreateParameterizedExpression(string typeName, string constructorValueExpression, string? trailingValue = null)
    {
        if (string.IsNullOrEmpty(trailingValue))
        {
            return "new " + typeName + "(" + constructorValueExpression + ")";
        }

        return "new " + typeName + "(" + constructorValueExpression + ", " + trailingValue + ")";
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

    /// <summary>
    /// Emits a generated <c>string[]</c> from either a plain YAML sequence or a sparse
    /// numeric-slot mapping.
    ///
    /// Supported source shapes:
    ///
    /// 1. Plain array:
    ///    <code>
    ///    unitsMap:
    ///      - 'zero'
    ///      - 'one'
    ///    </code>
    ///
    /// 2. Indexed mapping:
    ///    <code>
    ///    tensMap:
    ///      2: 'twenty'
    ///      3: 'thirty'
    ///    </code>
    ///
    /// The numeric-slot form keeps YAML readable by replacing alignment hacks such as leading
    /// empty strings with explicit indices. Missing slots are emitted as empty strings.
    /// </summary>
    static string CreateStringArrayExpression(JsonElement arrayElement)
    {
        if (arrayElement.ValueKind == JsonValueKind.Object)
        {
            return CreateNumericSlotStringArrayExpression(arrayElement);
        }

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

    static string CreateNumericSlotStringArrayExpression(JsonElement objectElement)
    {
        if (!objectElement.EnumerateObject().Any())
        {
            return "Array.Empty<string>()";
        }

        var indexedValues = new SortedDictionary<int, string>();
        foreach (var property in objectElement.EnumerateObject())
        {
            if (!int.TryParse(property.Name, NumberStyles.Integer, CultureInfo.InvariantCulture, out var index) || index < 0)
            {
                throw new InvalidOperationException($"Indexed string arrays require non-negative integer keys. Invalid key '{property.Name}'.");
            }

            if (property.Value.ValueKind != JsonValueKind.String)
            {
                throw new InvalidOperationException($"Indexed string arrays require string values. Property '{property.Name}' was {property.Value.ValueKind}.");
            }

            indexedValues[index] = property.Value.GetString()!;
        }

        var builder = new StringBuilder("new string[] { ");
        var first = true;
        var lastIndex = indexedValues.Keys.Max();

        for (var index = 0; index <= lastIndex; index++)
        {
            if (!first)
            {
                builder.Append(", ");
            }

            builder.Append(QuoteLiteral(indexedValues.TryGetValue(index, out var value) ? value : string.Empty));
            first = false;
        }

        builder.Append(" }");
        return builder.ToString();
    }

    static string CreateOptionalStringArrayExpression(JsonElement element, string propertyName) =>
        element.TryGetProperty(propertyName, out var property) &&
        property.ValueKind is JsonValueKind.Array or JsonValueKind.Object
            ? CreateStringArrayExpression(property)
            : "Array.Empty<string>()";

    static string CreateFormatterNumberDetectorExpression(JsonElement element, string propertyName) =>
        CreateFormatterNumberDetectorExpression(GetOptionalString(element, propertyName));

    static string CreateFormatterNumberDetectorExpression(string? value) =>
        "FormatterNumberDetectorKind." + ToEnumMemberName(value ?? "none");

    static string CreateFormatterNumberFormExpression(JsonElement element, string propertyName, string defaultValue = "default") =>
        CreateFormatterNumberFormExpression(GetOptionalString(element, propertyName), defaultValue);

    static string CreateFormatterNumberFormExpression(string? value, string defaultValue = "default") =>
        "FormatterNumberForm." + ToEnumMemberName(value ?? defaultValue);

    static string CreateFormatterFallbackTransformExpression(JsonElement element, string propertyName) =>
        CreateFormatterFallbackTransformExpression(GetOptionalString(element, propertyName));

    static string CreateFormatterFallbackTransformExpression(string? value) =>
        "FormatterDataUnitFallbackTransform." + ToEnumMemberName(value ?? "none");

    static string CreateFormatterPrepositionModeExpression(JsonElement element, string propertyName) =>
        CreateFormatterPrepositionModeExpression(GetOptionalString(element, propertyName));

    static string CreateFormatterPrepositionModeExpression(string? value) =>
        "FormatterPrepositionMode." + ToEnumMemberName(value ?? "none");

    static string CreateFormatterSecondaryPlaceholderModeExpression(JsonElement element, string propertyName) =>
        CreateFormatterSecondaryPlaceholderModeExpression(GetOptionalString(element, propertyName));

    static string CreateFormatterSecondaryPlaceholderModeExpression(string? value) =>
        "FormatterSecondaryPlaceholderMode." + ToEnumMemberName(value ?? "none");

    static string CreateTimeUnitGenderMapExpression(JsonElement element, string propertyName)
    {
        if (!element.TryGetProperty(propertyName, out var property) || property.ValueKind != JsonValueKind.Object)
        {
            return "FrozenDictionary<TimeUnit, GrammaticalGender>.Empty";
        }

        return CreateTimeUnitGenderMapExpression(property);
    }

    static string CreateTimeUnitGenderMapExpression(JsonElement property)
    {
        if (property.ValueKind != JsonValueKind.Object)
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

    static string CreateTargetTypedConstructorExpression(JsonElement element, params JsonConstructorValueEmitter[] emitters)
    {
        if (element.ValueKind != JsonValueKind.Object)
        {
            throw new InvalidOperationException("Expected JSON object.");
        }

        var builder = new StringBuilder("new(");
        AppendConstructorValues(builder, element, emitters);
        builder.Append(')');
        return builder.ToString();
    }

    static string CreateTypedConstructorArrayExpression(string elementTypeName, JsonElement arrayElement, params JsonConstructorValueEmitter[] emitters)
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

    static void AppendConstructorValues(StringBuilder builder, JsonElement element, params JsonConstructorValueEmitter[] emitters)
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

    static JsonConstructorValueEmitter RequiredInt64Value(string propertyName, string suffix = "") =>
        element => GetRequiredInt64(element, propertyName).ToString(CultureInfo.InvariantCulture) + suffix;

    static JsonConstructorValueEmitter RequiredStringValue(string propertyName) =>
        element => QuoteLiteral(GetRequiredString(element, propertyName));

    static JsonConstructorValueEmitter RequiredEnumValue(string propertyName, string enumTypeName) =>
        element => enumTypeName + "." + ToEnumMemberName(GetRequiredString(element, propertyName));

    static JsonConstructorValueEmitter OptionalEnumValue(string propertyName, string enumTypeName, string defaultValue) =>
        element => enumTypeName + "." + ToEnumMemberName(GetOptionalString(element, propertyName) ?? defaultValue);

    static JsonConstructorValueEmitter OptionalStringValue(string propertyName, string defaultValue = "") =>
        element => QuoteLiteral(GetOptionalString(element, propertyName) ?? defaultValue);

    static JsonConstructorValueEmitter OptionalStringValueOrFallback(string propertyName, string fallbackPropertyName) =>
        element => QuoteLiteral(GetOptionalString(element, propertyName) ?? GetRequiredString(element, fallbackPropertyName));

    static JsonConstructorValueEmitter OptionalStringValueOmitIfMissing(string propertyName) =>
        element => GetOptionalString(element, propertyName) is { } value ? QuoteLiteral(value) : null;

    static JsonConstructorValueEmitter BooleanValue(string propertyName) =>
        element => GetBoolean(element, propertyName) ? "true" : "false";

    static JsonConstructorValueEmitter OptionalTrueValue(string propertyName) =>
        element => GetBoolean(element, propertyName) ? "true" : null;

    static JsonConstructorValueEmitter OptionalBooleanValue(string propertyName) =>
        element => element.TryGetProperty(propertyName, out var property) && property.ValueKind is JsonValueKind.True or JsonValueKind.False
            ? (property.GetBoolean() ? "true" : "false")
            : null;

    static string CreateOptionalStringIntFrozenDictionaryExpression(JsonElement element, string propertyName) =>
        element.TryGetProperty(propertyName, out var property) && property.ValueKind == JsonValueKind.Object
            ? CreateStringIntFrozenDictionaryExpression(property)
            : "null";

    static string CreateOptionalStringLongFrozenDictionaryExpression(JsonElement element, string propertyName) =>
        element.TryGetProperty(propertyName, out var property) && property.ValueKind == JsonValueKind.Object
            ? CreateStringLongFrozenDictionaryExpression(property)
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

    static string CreateHarmonyOrdinalScaleArrayExpression(JsonElement arrayElement)
    {
        if (arrayElement.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidOperationException("Expected JSON array.");
        }

        var builder = new StringBuilder("new HarmonyOrdinalScale[] { ");
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

    static string CreateContractedOneScaleArrayExpression(JsonElement arrayElement)
    {
        if (arrayElement.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidOperationException("Expected JSON array.");
        }

        var builder = new StringBuilder("new ContractedOneScaleNumberToWordsConverter.Scale[] { ");
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
            RequiredInt64Value("value"),
            RequiredStringValue("name"));

    static string CreateLinkingSuffixRuleArrayExpression(JsonElement arrayElement)
        => CreateTypedConstructorArrayExpression(
            "LinkingSuffixRule",
            arrayElement,
            RequiredStringValue("suffix"),
            RequiredStringValue("replacement"));

    static string CreateAgglutinativeOrdinalScaleArrayExpression(JsonElement arrayElement)
        => CreateTypedConstructorArrayExpression(
            "AgglutinativeOrdinalScale",
            arrayElement,
            RequiredInt64Value("value"),
            RequiredStringValue("singularCardinal"),
            RequiredStringValue("pluralCardinal"),
            RequiredStringValue("ordinalWord"));

    static string CreateContextualDecimalScaleArrayExpression(JsonElement arrayElement)
        => CreateTypedConstructorArrayExpression(
            "ContextualDecimalScale",
            arrayElement,
            RequiredInt64Value("value"),
            RequiredStringValue("name"));

    static string CreateConjunctionalScaleArrayExpression(JsonElement arrayElement)
    {
        if (arrayElement.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidOperationException("Expected JSON array.");
        }

        var builder = new StringBuilder("new ConjunctionalScale[] { ");
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
            RequiredInt64Value("value"),
            RequiredStringValue("name"),
            OptionalTrueValue("omitOneWhenSingular"));

    static string CreateOrdinalPrefixScaleArrayExpression(JsonElement arrayElement)
        => CreateTypedConstructorArrayExpression(
            "OrdinalPrefixScale",
            arrayElement,
            RequiredInt64Value("value"),
            RequiredStringValue("singular"),
            RequiredStringValue("plural"),
            RequiredStringValue("ordinalPrefix"),
            RequiredEnumValue("gender", "GrammaticalGender"));

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
            RequiredInt64Value("value", "UL"),
            RequiredEnumValue("gender", "GrammaticalGender"),
            RequiredStringValue("singular"),
            RequiredStringValue("paucal"),
            RequiredStringValue("plural"),
            OptionalStringValueOmitIfMissing("ordinalStem"));

    static string CreatePluralizedScaleArrayExpression(JsonElement arrayElement)
        => CreateTypedConstructorArrayExpression(
            "PluralizedScale",
            arrayElement,
            RequiredInt64Value("value", "UL"),
            RequiredEnumValue("countGender", "GrammaticalGender"),
            RequiredStringValue("singular"),
            RequiredStringValue("paucal"),
            RequiredStringValue("plural"),
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
            OptionalBooleanValue("omitLeadingOne"));

    static string CreateEastSlavicGenderEndingExpression(JsonElement objectElement) =>
        "new(" +
        QuoteLiteral(GetRequiredString(objectElement, "default")) + ", " +
        CreateNullableIntStringFrozenDictionaryExpression(objectElement, "exactEndings").Replace("null", "FrozenDictionary<int, string>.Empty") +
        ")";

    static string CreateSouthSlavicScaleArrayExpression(JsonElement arrayElement)
        => CreateTypedConstructorArrayExpression(
            "SouthSlavicScale",
            arrayElement,
            RequiredInt64Value("value", "UL"),
            RequiredEnumValue("gender", "GrammaticalGender"),
            OptionalStringValueOrFallback("oneForm", "singular"),
            RequiredStringValue("singular"),
            RequiredStringValue("paucal"),
            RequiredStringValue("plural"),
            OptionalStringValue("dual"),
            OptionalStringValue("trialQuadral"));

    static string CreateInvertedTensScaleArrayExpression(JsonElement arrayElement)
        => CreateTypedConstructorArrayExpression(
            "InvertedTensScale",
            arrayElement,
            RequiredInt64Value("value"),
            RequiredStringValue("oneForm"),
            RequiredStringValue("manyForm"),
            OptionalStringValue("countJoiner"),
            RequiredStringValue("remainderSeparator"));

    static string CreateScaleStrategyScaleArrayExpression(JsonElement arrayElement)
        => CreateTypedConstructorArrayExpression(
            "ScaleStrategyScale",
            arrayElement,
            RequiredInt64Value("value"),
            RequiredStringValue("name"),
            RequiredStringValue("plural"),
            OptionalStringValue("prefix"),
            OptionalStringValue("postfix"),
            OptionalStringValue("pluralSuffix"),
            OptionalStringValue("ordinalSuffix"),
            BooleanValue("displayOneUnit"),
            OptionalEnumValue("gender", "GrammaticalGender", "masculine"));

    static string CreateUnitLeadingCompoundScaleArrayExpression(JsonElement arrayElement)
        => CreateTypedConstructorArrayExpression(
            "UnitLeadingCompoundScale",
            arrayElement,
            RequiredInt64Value("value"),
            BooleanValue("addSpaceBeforeNextPart"),
            OptionalEnumValue("countGender", "GrammaticalGender", "masculine"),
            RequiredStringValue("singularCardinal"),
            RequiredStringValue("pluralCardinalFormat"),
            static element => CreateUnitLeadingCompoundOrdinalPairExpression(element, "ordinalSingular"),
            static element => CreateUnitLeadingCompoundOrdinalPairExpression(element, "ordinalPlural"),
            OptionalStringValue("countWordFormNextWord"));

    static string CreateUnitLeadingCompoundOrdinalPairExpression(JsonElement element, string propertyName)
    {
        if (!element.TryGetProperty(propertyName, out var property))
        {
            throw new InvalidOperationException($"Missing required property '{propertyName}'.");
        }

        return property.ValueKind switch
        {
            JsonValueKind.String => "new string[] { " + QuoteLiteral(property.GetString()!) + ", " + QuoteLiteral(property.GetString()!) + " }",
            JsonValueKind.Array => CreateStringArrayExpression(property),
            JsonValueKind.Object => CreateTerminalContinuingStringArrayExpression(property, propertyName),
            _ => throw new InvalidOperationException($"Property '{propertyName}' must be a string, array, or mapping.")
        };
    }

    static string CreateTerminalContinuingStringArrayExpression(JsonElement objectElement, string propertyName)
    {
        if (objectElement.ValueKind != JsonValueKind.Object)
        {
            throw new InvalidOperationException($"Property '{propertyName}' must be a mapping.");
        }

        return "new string[] { " +
               QuoteLiteral(GetRequiredString(objectElement, "terminal")) + ", " +
               QuoteLiteral(GetRequiredString(objectElement, "continuing")) +
               " }";
    }

    static string CreateConjoinedGenderedScaleArrayExpression(JsonElement arrayElement)
        => CreateTypedConstructorArrayExpression(
            "ConjoinedGenderedScale",
            arrayElement,
            RequiredInt64Value("divisor"),
            RequiredEnumValue("gender", "GrammaticalGender"),
            RequiredStringValue("singular"),
            RequiredStringValue("plural"),
            RequiredStringValue("ordinalStem"));

    static string CreateSegmentedScaleArrayExpression(JsonElement arrayElement)
        => CreateTypedConstructorArrayExpression(
            "SegmentedScale",
            arrayElement,
            RequiredInt64Value("value"),
            RequiredStringValue("singular"),
            RequiredStringValue("plural"),
            RequiredEnumValue("countVariant", "SegmentedScaleVariant"),
            RequiredEnumValue("singularRemainderVariant", "SegmentedScaleVariant"),
            RequiredEnumValue("pluralRemainderVariant", "SegmentedScaleVariant"));

    static string CreateTerminalOrdinalScaleArrayExpression(JsonElement arrayElement)
        => CreateTypedConstructorArrayExpression(
            "TerminalOrdinalScale",
            arrayElement,
            RequiredInt64Value("value"),
            RequiredStringValue("exactSingularCardinal"),
            RequiredStringValue("singularWithRemainderCardinal"),
            RequiredStringValue("pluralCardinal"),
            RequiredStringValue("ordinalStem"));

    static string CreateConstructStateScaleArrayExpression(JsonElement arrayElement)
        => CreateTypedConstructorArrayExpression(
            "ConstructStateScale",
            arrayElement,
            RequiredInt64Value("value"),
            RequiredStringValue("singular"),
            RequiredStringValue("dualPrefix"),
            RequiredEnumValue("countGender", "GrammaticalGender"));

    static string CreateHyphenatedScaleExpression(JsonElement element) =>
        CreateTargetTypedConstructorExpression(
            element,
            RequiredInt64Value("divisor"),
            RequiredStringValue("cardinal"),
            RequiredStringValue("ordinal"));

    static string CreateHyphenatedScaleArrayExpression(JsonElement arrayElement)
        => CreateTypedConstructorArrayExpression(
            "HyphenatedScale",
            arrayElement,
            RequiredInt64Value("divisor"),
            RequiredStringValue("cardinal"),
            RequiredStringValue("ordinal"));

    static string CreateDualFormScaleExpression(JsonElement element) =>
        CreateTargetTypedConstructorExpression(
            element,
            RequiredStringValue("singular"),
            RequiredStringValue("dual"),
            RequiredStringValue("plural"),
            BooleanValue("usePrefixMapForLowerDigits"));

    static string CreateTriadScaleArrayExpression(JsonElement arrayElement)
        => CreateTypedConstructorArrayExpression(
            "TriadScaleNumberToWordsConverter.TriadScale",
            arrayElement,
            element => checked((int)GetRequiredInt64(element, "value")).ToString(CultureInfo.InvariantCulture),
            RequiredStringValue("singular"),
            RequiredStringValue("plural"),
            OptionalStringValue("countToScaleJoiner"),
            BooleanValue("countUsesFinalAccent"),
            BooleanValue("appendTrailingSpace"),
            OptionalStringValue("ordinalCompactionMatch"),
            OptionalStringValue("ordinalCompactionReplacement"),
            BooleanValue("removeLeadingOneOnExactOrdinal"),
            OptionalStringValue("exactOrdinalSuffix"));

    static string CreateGenderedScaleOrdinalArrayExpression(JsonElement arrayElement)
        => CreateTypedConstructorArrayExpression(
            "GenderedScaleOrdinalNumberToWordsConverter.Scale",
            arrayElement,
            RequiredStringValue("singular"),
            RequiredStringValue("plural"),
            RequiredEnumValue("countGender", "GrammaticalGender"));

    static string CreateLongScaleWordArrayExpression(JsonElement arrayElement)
        => CreateTypedConstructorArrayExpression(
            "LongScaleStemOrdinalNumberToWordsConverter.LargeScale",
            arrayElement,
            RequiredInt64Value("value"),
            RequiredStringValue("singularPrefix"),
            RequiredStringValue("singular"),
            RequiredStringValue("plural"));

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
        if (arrayElement.ValueKind is not (JsonValueKind.Array or JsonValueKind.Object))
        {
            throw new InvalidOperationException("Expected JSON array or mapping.");
        }

        var builder = new StringBuilder("new InvertedTensToken[] { ");
        var first = true;

        if (arrayElement.ValueKind == JsonValueKind.Object)
        {
            foreach (var property in arrayElement.EnumerateObject()
                         .Select(static property =>
                         {
                             if (property.Value.ValueKind != JsonValueKind.Number || !property.Value.TryGetInt64(out var value))
                             {
                                 throw new InvalidOperationException($"Inverted tens token '{property.Name}' must map to an integer value.");
                             }

                             return (property.Name, Value: value);
                         })
                         .OrderByDescending(static entry => entry.Value)
                         .ThenBy(static entry => entry.Name, StringComparer.Ordinal))
            {
                if (!first)
                {
                    builder.Append(", ");
                }

                builder.Append("new(");
                builder.Append(QuoteLiteral(property.Name));
                builder.Append(", ");
                builder.Append(property.Value.ToString(CultureInfo.InvariantCulture));
                builder.Append(')');
                first = false;
            }
        }
        else
        {
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
            RequiredStringValue("singular"),
            RequiredStringValue("plural"),
            RequiredInt64Value("value"));

    static string CreatePrefixedScaleWordArrayExpression(JsonElement arrayElement)
    {
        if (arrayElement.ValueKind is not (JsonValueKind.Array or JsonValueKind.Object))
        {
            throw new InvalidOperationException("Expected JSON array or mapping.");
        }

        var builder = new StringBuilder("new PrefixedScaleWord[] { ");
        var first = true;

        if (arrayElement.ValueKind == JsonValueKind.Object)
        {
            foreach (var property in arrayElement.EnumerateObject()
                         .Select(static property =>
                         {
                             if (property.Value.ValueKind != JsonValueKind.Number || !property.Value.TryGetInt64(out var value))
                             {
                                 throw new InvalidOperationException($"Prefixed scale token '{property.Name}' must map to an integer value.");
                             }

                             return (property.Name, Value: value);
                         })
                         .OrderByDescending(static entry => entry.Value)
                         .ThenBy(static entry => entry.Name, StringComparer.Ordinal))
            {
                if (!first)
                {
                    builder.Append(", ");
                }

                builder.Append("new(");
                builder.Append(QuoteLiteral(property.Name));
                builder.Append(", ");
                builder.Append(property.Value.ToString(CultureInfo.InvariantCulture));
                builder.Append(')');
                first = false;
            }
        }
        else
        {
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
                builder.Append(QuoteLiteral(GetRequiredString(item, "token")));
                builder.Append(", ");
                builder.Append(GetRequiredInt64(item, "value").ToString(CultureInfo.InvariantCulture));
                builder.Append(')');
                first = false;
            }
        }

        builder.Append(" }");
        return builder.ToString();
    }

    static string CreatePrefixedTensRuleArrayExpression(JsonElement arrayElement)
    {
        if (arrayElement.ValueKind is not (JsonValueKind.Array or JsonValueKind.Object))
        {
            throw new InvalidOperationException("Expected JSON array or mapping.");
        }

        var builder = new StringBuilder("new PrefixedTensRule[] { ");
        var first = true;

        if (arrayElement.ValueKind == JsonValueKind.Object)
        {
            foreach (var property in arrayElement.EnumerateObject()
                         .Select(static property =>
                         {
                             if (property.Value.ValueKind != JsonValueKind.Number || !property.Value.TryGetInt64(out var value))
                             {
                                 throw new InvalidOperationException($"Prefixed tens prefix '{property.Name}' must map to an integer base value.");
                             }

                             return (property.Name, Value: value);
                         })
                         .OrderByDescending(static entry => entry.Name.Length)
                         .ThenBy(static entry => entry.Name, StringComparer.Ordinal))
            {
                if (!first)
                {
                    builder.Append(", ");
                }

                builder.Append("new(");
                builder.Append(QuoteLiteral(property.Name));
                builder.Append(", ");
                builder.Append(property.Value.ToString(CultureInfo.InvariantCulture));
                builder.Append(')');
                first = false;
            }
        }
        else
        {
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
                builder.Append(QuoteLiteral(GetRequiredString(item, "prefix")));
                builder.Append(", ");
                builder.Append(GetRequiredInt64(item, "baseValue").ToString(CultureInfo.InvariantCulture));
                builder.Append(')');
                first = false;
            }
        }

        builder.Append(" }");
        return builder.ToString();
    }

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

    static string CreateLongFrozenSetExpression(JsonElement arrayElement)
    {
        if (arrayElement.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidOperationException("Expected JSON array.");
        }

        var builder = new StringBuilder("new long[] { ");
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

            builder.Append(value.ToString(CultureInfo.InvariantCulture));
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
            ? CreateStringLongFrozenDictionaryExpression(ordinalMap)
            : "new Dictionary<string, long>(StringComparer.Ordinal).ToFrozenDictionary(StringComparer.Ordinal)";
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
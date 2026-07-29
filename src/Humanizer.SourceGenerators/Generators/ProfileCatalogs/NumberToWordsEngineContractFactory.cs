using System.Collections.Immutable;
using System.Globalization;
using System.Text.Json;

namespace Humanizer.SourceGenerators;

public sealed partial class HumanizerSourceGenerator
{
    /// <summary>
    /// Binds resolved locale YAML for number-to-words engines onto the typed structural contracts
    /// declared in <see cref="EngineContractCatalog"/> and emits direct constructor expressions for
    /// the generated profile catalog.
    /// </summary>
    static class NumberToWordsEngineContractFactory
    {
        /// <summary>
        /// Resolves a locale's <c>numberToWords</c> YAML block to a concrete runtime-converter
        /// constructor expression. The important split is:
        /// <list type="bullet">
        /// <item><description>structural engines are described in <see cref="EngineContractCatalog"/> and emitted member-by-member;</description></item>
        /// <item><description>legacy conventional engines still fall back to type-name construction until they are generalized.</description></item>
        /// </list>
        /// </summary>
        public static string Create(
            NumberToWordsProfileDefinition profile,
            ImmutableDictionary<string, EngineContract> contracts,
            bool useCultureParameter)
        {
            if (!contracts.TryGetValue(profile.Engine, out var contract))
            {
                return CreateConventionalNumberToWordsExpression(profile.Engine, useCultureParameter);
            }

            var runtimeType = contract.RuntimeType ?? GetConventionalNumberToWordsTypeName(profile.Engine);
            return "new " + runtimeType + "(" + CreateConstructorValues(profile.Root, contract.Members, useCultureParameter) + ")";
        }

        public static ImmutableArray<MetricScaleWordDefinition> CreateMetricScaleWords(
            NumberToWordsProfileDefinition profile,
            ImmutableDictionary<string, EngineContract> contracts)
        {
            if (!contracts.TryGetValue(profile.Engine, out var contract))
            {
                return ImmutableArray<MetricScaleWordDefinition>.Empty;
            }

            var words = new Dictionary<char, MetricScaleWordDefinition>();
            CollectMetricScaleWords(profile.Root, contract.Members, words);
            AddEngineMetricScaleWords(profile.Engine, profile.Root, words);
            return words.Values.OrderBy(static word => word.Symbol).ToImmutableArray();
        }

        /// <summary>
        /// Walks the contract members in declaration order so the generated constructor arguments
        /// stay aligned with the runtime profile type. The generator resolves inheritance and YAML
        /// merges before this point, so this method can treat the <see cref="JsonElement"/> as the
        /// fully materialized locale payload.
        /// </summary>
        static string CreateConstructorValues(
            JsonElement root,
            ImmutableArray<EngineContractMember> members,
            bool useCultureParameter) =>
            string.Join(", ", members.Select(member => CreateMemberValue(root, member, useCultureParameter)));

        static void CollectMetricScaleWords(
            JsonElement root,
            ImmutableArray<EngineContractMember> members,
            Dictionary<char, MetricScaleWordDefinition> words)
        {
            foreach (var member in members)
            {
                if (member.Kind is "profile-object" or "optional-profile-object")
                {
                    if (!EngineContractUtilities.TryGetElement(root, member.SourcePath, out var objectRoot))
                    {
                        continue;
                    }

                    CollectMetricScaleWords(objectRoot, member.Members, words);
                    continue;
                }

                if (member.Kind != "builder" ||
                    member.MetricScaleWords is not { } scaleWordContract ||
                    !EngineContractUtilities.TryGetElement(root, member.SourcePath, out var builderRoot))
                {
                    continue;
                }

                AddContractMetricScaleWords(scaleWordContract, root, builderRoot, words);
            }
        }

        static void AddContractMetricScaleWords(
            MetricScaleWordContract contract,
            JsonElement profileRoot,
            JsonElement root,
            Dictionary<char, MetricScaleWordDefinition> words)
        {
            if (contract.FixedValue is { } fixedValue)
            {
                AddContractMetricScaleWord(contract, fixedValue, profileRoot, root, words);
                return;
            }

            if (contract.ValueProperty is null)
            {
                throw new InvalidOperationException("Array-backed metric scale word contracts require a value property.");
            }

            foreach (var row in root.EnumerateArray())
            {
                AddContractMetricScaleWord(
                    contract,
                    GetRequiredInt64(row, contract.ValueProperty),
                    profileRoot,
                    row,
                    words);
            }
        }

        static void AddContractMetricScaleWord(
            MetricScaleWordContract contract,
            long value,
            JsonElement profileRoot,
            JsonElement row,
            Dictionary<char, MetricScaleWordDefinition> words)
        {
            if (!TryGetMetricSymbol(value, out var symbol))
            {
                return;
            }

            var singular = contract.SingularOptional
                ? GetOptionalString(row, contract.SingularProperty)?.Trim()
                : GetRequiredString(row, contract.SingularProperty).Trim();
            if (singular is null || singular.Length == 0)
            {
                return;
            }

            if ((contract.Transform & MetricScaleWordTransform.TrimFirstWord) != 0)
            {
                singular = RemoveFirstWord(singular);
            }
            else if ((contract.Transform & MetricScaleWordTransform.TrimAuthoredOne) != 0)
            {
                singular = RemoveAuthoredOne(singular, profileRoot);
            }

            var plural = contract.PluralProperty is null
                ? singular
                : GetOptionalString(row, contract.PluralProperty)?.Trim() ?? singular;
            if ((contract.Transform & MetricScaleWordTransform.StripPluralFormatPlaceholder) != 0)
            {
                plural = plural.Replace("{0}", string.Empty).Trim();
            }

            words[symbol] = new(symbol, singular, plural);
        }

        static void AddEngineMetricScaleWords(
            string engine,
            JsonElement root,
            Dictionary<char, MetricScaleWordDefinition> words)
        {
            switch (engine)
            {
                case "appended-group":
                    var groups = EngineContractUtilities.GetRequiredElement(root, "groups");
                    var pluralGroups = EngineContractUtilities.GetRequiredElement(root, "pluralGroups");
                    const string groupSymbols = "kMGTPEZY";
                    for (var index = 1; index <= groupSymbols.Length; index++)
                    {
                        if (!TryGetIndexedString(groups, index, out var singular) ||
                            !TryGetIndexedString(pluralGroups, index, out var plural))
                        {
                            continue;
                        }

                        AddMetricScaleWord(
                            groupSymbols[index - 1],
                            singular,
                            plural,
                            words);
                    }

                    break;
                case "billion-strategy":
                    var cardinal = EngineContractUtilities.GetRequiredElement(root, "cardinal");
                    if (!cardinal.TryGetProperty("scales", out _))
                    {
                        AddMetricScaleWord(
                            1_000_000,
                            GetRequiredString(cardinal, "millionSingularWord"),
                            GetRequiredString(cardinal, "millionPluralWord"),
                            words);
                        if (GetRequiredString(cardinal, "billionStrategy") == "billion-word")
                        {
                            AddMetricScaleWord(
                                1_000_000_000,
                                GetRequiredString(cardinal, "billionSingularWord"),
                                GetRequiredString(cardinal, "billionPluralWord"),
                                words);
                        }
                    }

                    break;
                case "east-asian-grouped":
                    var smallUnitWords = EngineContractUtilities.GetRequiredElement(root, "smallUnitWords");
                    if (TryGetIndexedString(smallUnitWords, 3, out var thousand))
                    {
                        AddMetricScaleWord(1_000, thousand, null, words);
                    }

                    var largeUnits = EngineContractUtilities.GetRequiredElement(root, "largeUnits");
                    foreach (var index in new[] { 3, 6 })
                    {
                        if (TryGetIndexedString(largeUnits, index, out var scaleWord))
                        {
                            AddMetricScaleWord(
                                index == 3 ? 'T' : 'Y',
                                scaleWord,
                                null,
                                words);
                        }
                    }

                    break;
                case "indian-grouping":
                    AddMetricScaleWord(1_000_000_000_000_000, GetRequiredString(root, "quadrillionWord"), null, words);
                    AddMetricScaleWord(1_000_000_000_000_000_000, GetRequiredString(root, "quintillionWord"), null, words);
                    break;
                case "indian-grouping-gendered":
                    AddMetricScaleWord(1_000, GetRequiredString(root, "thousandWord"), null, words);
                    AddMetricScaleWord(1_000_000_000, GetRequiredString(root, "arabWord"), null, words);
                    break;
            }
        }

        static void AddMetricScaleWord(
            long value,
            string singular,
            string? plural,
            Dictionary<char, MetricScaleWordDefinition> words)
        {
            if (TryGetMetricSymbol(value, out var symbol))
            {
                words[symbol] = new(symbol, singular.Trim(), plural?.Trim() ?? singular.Trim());
            }
        }

        static void AddMetricScaleWord(
            char symbol,
            string singular,
            string? plural,
            Dictionary<char, MetricScaleWordDefinition> words) =>
            words[symbol] = new(symbol, singular.Trim(), plural?.Trim() ?? singular.Trim());

        static string RemoveFirstWord(string value)
        {
            var separator = value.IndexOf(' ');
            return separator < 0 ? value : value.Substring(separator + 1).Trim();
        }

        static string RemoveAuthoredOne(string value, JsonElement root)
        {
            if (!EngineContractUtilities.TryGetElement(root, "unitsMap", out var units) ||
                !TryGetIndexedString(units, 1, out var one))
            {
                return value;
            }

            return value.StartsWith(one + " ", StringComparison.Ordinal)
                ? value.Substring(one.Length + 1).Trim()
                : value;
        }

        static bool TryGetIndexedString(JsonElement values, int index, out string value)
        {
            JsonElement item;
            if (values.ValueKind == JsonValueKind.Array)
            {
                if (index >= values.GetArrayLength())
                {
                    value = string.Empty;
                    return false;
                }

                item = values[index];
            }
            else if (values.ValueKind == JsonValueKind.Object &&
                     values.TryGetProperty(index.ToString(CultureInfo.InvariantCulture), out var property))
            {
                item = property;
            }
            else
            {
                value = string.Empty;
                return false;
            }

            value = item.ValueKind == JsonValueKind.String ? item.GetString()! : string.Empty;
            return value.Length > 0;
        }

        static bool TryGetMetricSymbol(long value, out char symbol)
        {
            symbol = value switch
            {
                1_000 => 'k',
                1_000_000 => 'M',
                1_000_000_000 => 'G',
                1_000_000_000_000 => 'T',
                1_000_000_000_000_000 => 'P',
                1_000_000_000_000_000_000 => 'E',
                _ => default
            };
            return symbol != default;
        }

        /// <summary>
        /// Converts one contract member into its generated C# literal or nested-constructor form.
        /// This is the bridge between the YAML authoring DSL and the strongly typed runtime profile
        /// objects: every supported <c>kind</c> is emitted as a direct expression with no runtime
        /// parsing left behind.
        /// </summary>
        static string CreateMemberValue(JsonElement root, EngineContractMember member, bool useCultureParameter) =>
            member.Kind switch
            {
                "profile-object" => CreateObjectValue(root, member, useCultureParameter),
                "optional-profile-object" => EngineContractUtilities.TryGetElement(root, member.SourcePath, out _)
                    ? CreateObjectValue(root, member, useCultureParameter)
                    : "null",
                "culture" => useCultureParameter ? "culture" : "CultureInfo.InvariantCulture",
                "string" => QuoteLiteral(GetStringValue(root, member)),
                "nullable-string" => GetOptionalStringValue(root, member) is { } stringValue ? QuoteLiteral(stringValue) : "null",
                "bool" => GetBooleanValue(root, member) ? "true" : "false",
                "presence-bool" => EngineContractUtilities.TryGetElement(root, member.SourcePath, out _) ? "true" : "false",
                "int64" => GetInt64Value(root, member).ToString(CultureInfo.InvariantCulture),
                "nullable-int64" => GetOptionalInt64Value(root, member)?.ToString(CultureInfo.InvariantCulture) ?? "null",
                "int32" => checked((int)GetInt64Value(root, member)).ToString(CultureInfo.InvariantCulture),
                "enum" => CreateEnumValue(root, member),
                "string-array" => CreateStringArrayExpression(EngineContractUtilities.GetRequiredElement(root, member.SourcePath)),
                "optional-string-array" => EngineContractUtilities.TryGetElement(root, member.SourcePath, out var optionalArray)
                    ? CreateStringArrayExpression(optionalArray)
                    : "Array.Empty<string>()",
                "int-string-dictionary" => CreateStringIntFrozenDictionaryExpression(EngineContractUtilities.GetRequiredElement(root, member.SourcePath)),
                "nullable-int-string-dictionary" => CreateNullableIntStringDictionaryValue(root, member),
                "string-string-dictionary" => CreateStringStringFrozenDictionaryExpression(EngineContractUtilities.GetRequiredElement(root, member.SourcePath)),
                "nullable-string-string-dictionary" => EngineContractUtilities.TryGetElement(root, member.SourcePath, out var optionalStringDictionary)
                    ? CreateOptionalStringStringFrozenDictionaryExpression(root, EngineContractUtilities.GetLeafPropertyName(member.SourcePath))
                    : member.MissingValue == "empty"
                        ? "FrozenDictionary<string, string>.Empty"
                        : "null",
                "char-string-dictionary" => CreateCharStringFrozenDictionaryExpression(EngineContractUtilities.GetRequiredElement(root, member.SourcePath)),
                "nullable-char-string-dictionary" => EngineContractUtilities.TryGetElement(root, member.SourcePath, out var optionalCharDictionary)
                    ? CreateCharStringFrozenDictionaryExpression(optionalCharDictionary)
                    : "null",
                "nullable-int-set" => CreateNullableIntSetValue(root, member),
                "builder" => CreateBuilderValue(root, member),
                _ => throw new InvalidOperationException($"Unsupported number-to-words contract member kind '{member.Kind}'.")
            };

        static string CreateObjectValue(JsonElement root, EngineContractMember member, bool useCultureParameter)
        {
            var objectRoot = string.IsNullOrWhiteSpace(member.SourcePath)
                ? root
                : EngineContractUtilities.GetRequiredElement(root, member.SourcePath);

            // Nested profile objects let the YAML stay grouped by meaning while the generated code
            // still calls an explicit runtime constructor shape. This keeps the authoring surface
            // readable without paying any runtime mapping cost.
            return member.TypeName is null
                ? "new(" + CreateConstructorValues(objectRoot, member.Members, useCultureParameter) + ")"
                : "new " + member.TypeName + "(" + CreateConstructorValues(objectRoot, member.Members, useCultureParameter) + ")";
        }

        static string CreateEnumValue(JsonElement root, EngineContractMember member)
        {
            if (member.EnumType is null)
            {
                throw new InvalidOperationException("Enum members require an enum type.");
            }

            return member.EnumType + "." + ToEnumMemberName(GetStringValue(root, member));
        }

        static string CreateNullableIntStringDictionaryValue(JsonElement root, EngineContractMember member)
        {
            if (EngineContractUtilities.TryGetElement(root, member.SourcePath, out _))
            {
                return CreateNullableIntStringFrozenDictionaryExpression(root, EngineContractUtilities.GetLeafPropertyName(member.SourcePath));
            }

            return member.MissingValue switch
            {
                "empty" => "FrozenDictionary<int, string>.Empty",
                _ => "null"
            };
        }

        static string CreateNullableIntSetValue(JsonElement root, EngineContractMember member)
        {
            if (EngineContractUtilities.TryGetElement(root, member.SourcePath, out _))
            {
                return CreateNullableIntFrozenSetExpression(root, EngineContractUtilities.GetLeafPropertyName(member.SourcePath));
            }

            return member.MissingValue switch
            {
                "empty" => "FrozenSet<int>.Empty",
                _ => "null"
            };
        }

        static string CreateBuilderValue(JsonElement root, EngineContractMember member)
        {
            if (member.Builder is null)
            {
                throw new InvalidOperationException("Builder members require a builder name.");
            }

            if (member.Builder == "billion-strategy-cardinal-scale-array" &&
                !EngineContractUtilities.TryGetElement(root, member.SourcePath, out _))
            {
                return CreateLegacyBillionStrategyCardinalScaleArrayExpression(root);
            }

            var builderRoot = string.IsNullOrWhiteSpace(member.SourcePath)
                ? root
                : EngineContractUtilities.GetRequiredElement(root, member.SourcePath);

            // Builders exist for repeated structured rows such as scale tables, suffix rules, and
            // gender-ending records. Keeping these explicit makes the generated output easier to
            // audit than trying to encode a second generic mini-language for nested collections.
            return member.Builder switch
            {
                "harmony-ordinal-scale-array" => CreateHarmonyOrdinalScaleArrayExpression(builderRoot),
                "billion-strategy-cardinal-scale-array" => CreateBillionStrategyCardinalScaleArrayExpression(builderRoot),
                "contracted-one-scale-array" => CreateContractedOneScaleArrayExpression(builderRoot),
                "linking-scale-array" => CreateLinkingScaleArrayExpression(builderRoot),
                "linking-suffix-rule-array" => CreateLinkingSuffixRuleArrayExpression(builderRoot),
                "agglutinative-ordinal-scale-array" => CreateAgglutinativeOrdinalScaleArrayExpression(builderRoot),
                "contextual-decimal-scale-array" => CreateContextualDecimalScaleArrayExpression(builderRoot),
                "conjunctional-scale-array" => CreateConjunctionalScaleArrayExpression(builderRoot),
                "joined-scale-array" => CreateJoinedScaleArrayExpression(builderRoot),
                "west-slavic-scale-forms" => CreateWestSlavicScaleFormsExpression(builderRoot),
                "east-asian-scale-array" => throw new InvalidOperationException("Unexpected east-asian scale builder."),
                "east-slavic-scale-array" => CreateEastSlavicScaleArrayExpression(builderRoot),
                "east-slavic-gender-ending" => CreateEastSlavicGenderEndingExpression(builderRoot),
                "pluralized-scale-array" => CreatePluralizedScaleArrayExpression(builderRoot),
                "ordinal-prefix-scale-array" => CreateOrdinalPrefixScaleArrayExpression(builderRoot),
                "inverted-tens-scale-array" => CreateInvertedTensScaleArrayExpression(builderRoot),
                "indian-scale-form-array" => CreateIndianScaleFormArrayExpression(builderRoot),
                "south-slavic-scale-array" => CreateSouthSlavicScaleArrayExpression(builderRoot),
                "scale-leading-compound-scale-array" => CreateScaleLeadingCompoundScaleArrayExpression(builderRoot),
                "scandinavian-scale-array" => CreateScaleStrategyScaleArrayExpression(builderRoot),
                "unit-leading-compound-scale-array" => CreateUnitLeadingCompoundScaleArrayExpression(builderRoot),
                "conjoined-gendered-scale-array" => CreateConjoinedGenderedScaleArrayExpression(builderRoot),
                "segmented-scale-array" => CreateSegmentedScaleArrayExpression(builderRoot),
                "stemmed-scale-array" => CreateStemmedScaleArrayExpression(builderRoot),
                "linked-vigesimal-scale-array" => CreateLinkedVigesimalScaleArrayExpression(builderRoot),
                "terminal-ordinal-scale-array" => CreateTerminalOrdinalScaleArrayExpression(builderRoot),
                "construct-state-scale-array" => CreateConstructStateScaleArrayExpression(builderRoot),
                "hyphenated-scale" => CreateHyphenatedScaleExpression(builderRoot),
                "hyphenated-scale-array" => CreateHyphenatedScaleArrayExpression(builderRoot),
                "hyphenated-ordinal-scale-array" => CreateHyphenatedOrdinalScaleArrayExpression(builderRoot),
                "dual-form-scale" => CreateDualFormScaleExpression(builderRoot),
                "macedonian-scale-array" => CreateMacedonianScaleArrayExpression(builderRoot),
                "triad-scale-array" => CreateTriadScaleArrayExpression(builderRoot),
                "gendered-scale-ordinal-array" => CreateGenderedScaleOrdinalArrayExpression(builderRoot),
                "long-scale-word-array" => CreateLongScaleWordArrayExpression(builderRoot),
                "variant-decade-scale-array" => CreateVariantDecadeScaleArrayExpression(builderRoot),
                _ => throw new InvalidOperationException($"Unsupported number-to-words builder '{member.Builder}'.")
            };
        }

        static string GetStringValue(JsonElement root, EngineContractMember member)
        {
            if (EngineContractUtilities.TryGetOptionalString(root, member.SourcePath, out var value))
            {
                return value!;
            }

            if (member.FallbackSourcePath is not null && EngineContractUtilities.TryGetOptionalString(root, member.FallbackSourcePath, out var fallback))
            {
                return fallback!;
            }

            if (member.DefaultValue is not null)
            {
                return member.DefaultValue;
            }

            throw new InvalidOperationException($"Missing required string property '{member.SourcePath}'.");
        }

        static string? GetOptionalStringValue(JsonElement root, EngineContractMember member)
        {
            if (EngineContractUtilities.TryGetOptionalString(root, member.SourcePath, out var value))
            {
                return value;
            }

            if (member.FallbackSourcePath is not null && EngineContractUtilities.TryGetOptionalString(root, member.FallbackSourcePath, out var fallback))
            {
                return fallback;
            }

            return member.DefaultValue;
        }

        static bool GetBooleanValue(JsonElement root, EngineContractMember member)
        {
            if (EngineContractUtilities.TryGetElement(root, member.SourcePath, out var value) &&
                value.ValueKind is JsonValueKind.True or JsonValueKind.False)
            {
                return value.GetBoolean();
            }

            return bool.TryParse(member.DefaultValue, out var defaultValue) && defaultValue;
        }

        static long GetInt64Value(JsonElement root, EngineContractMember member)
        {
            if (EngineContractUtilities.TryGetElement(root, member.SourcePath, out var value) &&
                value.ValueKind == JsonValueKind.Number &&
                value.TryGetInt64(out var number))
            {
                return number;
            }

            if (member.DefaultValue is not null && long.TryParse(member.DefaultValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out number))
            {
                return number;
            }

            throw new InvalidOperationException($"Missing required integer property '{member.SourcePath}'.");
        }

        static long? GetOptionalInt64Value(JsonElement root, EngineContractMember member)
        {
            if (EngineContractUtilities.TryGetElement(root, member.SourcePath, out var value) &&
                value.ValueKind == JsonValueKind.Number &&
                value.TryGetInt64(out var number))
            {
                return number;
            }

            return member.DefaultValue is not null &&
                   long.TryParse(member.DefaultValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out number)
                ? number
                : null;
        }

    }
}
using System.Collections.Immutable;
using System.Text.Json;

namespace Humanizer.SourceGenerators;

public sealed partial class HumanizerSourceGenerator
{
    sealed class OrdinalizerProfileDefinition(string profileName, string engine, JsonElement root)
    {
        public string ProfileName { get; } = profileName;
        public string Engine { get; } = engine;
        public JsonElement Root { get; } = root;
    }

    sealed class OrdinalDateProfileDefinition(string profileName, string pattern, string dayMode)
    {
        public string ProfileName { get; } = profileName;
        public string Pattern { get; } = pattern;
        public string DayMode { get; } = dayMode;
    }

    sealed class TimeOnlyToClockNotationProfileDefinition(string profileName, string engine, JsonElement root)
    {
        public string ProfileName { get; } = profileName;
        public string Engine { get; } = engine;
        public JsonElement Root { get; } = root;
    }

    sealed class FormatterProfileDefinition(string profileName, string engine, JsonElement root)
    {
        public string ProfileName { get; } = profileName;
        public string Engine { get; } = engine;
        public JsonElement Root { get; } = root;
    }

    sealed class NumberToWordsProfileDefinition(string profileName, string engine, JsonElement root)
    {
        public string ProfileName { get; } = profileName;
        public string Engine { get; } = engine;
        public JsonElement Root { get; } = root;
    }

    sealed class WordsToNumberProfileDefinition(string profileName, string engine, JsonElement root)
    {
        public string ProfileName { get; } = profileName;
        public string Engine { get; } = engine;
        public JsonElement Root { get; } = root;
    }

    sealed class TokenMapLocaleDefinition(
        string localeCode,
        string normalizationProfile,
        string? ordinalNumberToWordsKind,
        string? ordinalGenderVariant,
        ImmutableArray<TokenMapEntry> exactOrdinalEntries,
        ImmutableArray<TokenMapEntry> ordinalScaleEntries,
        ImmutableArray<TokenMapEntry> gluedOrdinalScaleSuffixEntries,
        ImmutableArray<TokenMapEntry> compositeScaleEntries,
        ImmutableArray<string> negativePrefixes,
        ImmutableArray<string> negativeSuffixes,
        ImmutableArray<string> ordinalPrefixes,
        ImmutableArray<string> ignoredTokens,
        ImmutableArray<string> leadingTokenPrefixesToTrim,
        ImmutableArray<string> multiplierTokens,
        ImmutableArray<string> tokenSuffixesToStrip,
        ImmutableArray<string> ordinalAbbreviationSuffixes,
        ImmutableArray<string> teenSuffixTokens,
        ImmutableArray<string> hundredSuffixTokens,
        ImmutableArray<string> aliases,
        bool allowTerminalOrdinalToken,
        bool useHundredMultiplier,
        bool allowInvariantIntegerInput,
        long? teenBaseValue,
        long? hundredSuffixValue,
        long? unitTokenMinValue,
        long? unitTokenMaxValue,
        long? hundredSuffixMinValue,
        long? hundredSuffixMaxValue,
        long? scaleThreshold,
        ImmutableArray<TokenMapEntry> cardinalEntries)
    {
        public string LocaleCode { get; } = localeCode;
        public string NormalizationProfile { get; } = normalizationProfile;
        public string? OrdinalNumberToWordsKind { get; } = ordinalNumberToWordsKind;
        public string? OrdinalGenderVariant { get; } = ordinalGenderVariant;
        public ImmutableArray<TokenMapEntry> ExactOrdinalEntries { get; } = exactOrdinalEntries;
        public ImmutableArray<TokenMapEntry> OrdinalScaleEntries { get; } = ordinalScaleEntries;
        public ImmutableArray<TokenMapEntry> GluedOrdinalScaleSuffixEntries { get; } = gluedOrdinalScaleSuffixEntries;
        public ImmutableArray<TokenMapEntry> CompositeScaleEntries { get; } = compositeScaleEntries;
        public ImmutableArray<string> NegativePrefixes { get; } = negativePrefixes;
        public ImmutableArray<string> NegativeSuffixes { get; } = negativeSuffixes;
        public ImmutableArray<string> OrdinalPrefixes { get; } = ordinalPrefixes;
        public ImmutableArray<string> IgnoredTokens { get; } = ignoredTokens;
        public ImmutableArray<string> LeadingTokenPrefixesToTrim { get; } = leadingTokenPrefixesToTrim;
        public ImmutableArray<string> MultiplierTokens { get; } = multiplierTokens;
        public ImmutableArray<string> TokenSuffixesToStrip { get; } = tokenSuffixesToStrip;
        public ImmutableArray<string> OrdinalAbbreviationSuffixes { get; } = ordinalAbbreviationSuffixes;
        public ImmutableArray<string> TeenSuffixTokens { get; } = teenSuffixTokens;
        public ImmutableArray<string> HundredSuffixTokens { get; } = hundredSuffixTokens;
        public ImmutableArray<string> Aliases { get; } = aliases;
        public bool AllowTerminalOrdinalToken { get; } = allowTerminalOrdinalToken;
        public bool UseHundredMultiplier { get; } = useHundredMultiplier;
        public bool AllowInvariantIntegerInput { get; } = allowInvariantIntegerInput;
        public long? TeenBaseValue { get; } = teenBaseValue;
        public long? HundredSuffixValue { get; } = hundredSuffixValue;
        public long? UnitTokenMinValue { get; } = unitTokenMinValue;
        public long? UnitTokenMaxValue { get; } = unitTokenMaxValue;
        public long? HundredSuffixMinValue { get; } = hundredSuffixMinValue;
        public long? HundredSuffixMaxValue { get; } = hundredSuffixMaxValue;
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
}

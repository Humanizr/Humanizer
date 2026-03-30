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
        ImmutableArray<TokenMapEntry> ordinalEntries,
        ImmutableArray<TokenMapEntry> compositeScaleEntries,
        ImmutableArray<string> negativePrefixes,
        ImmutableArray<string> negativeSuffixes,
        ImmutableArray<string> ordinalPrefixes,
        ImmutableArray<string> ignoredTokens,
        ImmutableArray<string> multiplierTokens,
        ImmutableArray<string> tokenSuffixesToStrip,
        ImmutableArray<string> ordinalAbbreviationSuffixes,
        ImmutableArray<string> aliases,
        bool allowTerminalOrdinalToken,
        bool useHundredMultiplier,
        bool allowInvariantIntegerInput,
        long? scaleThreshold,
        ImmutableArray<TokenMapEntry> cardinalEntries)
    {
        public string LocaleCode { get; } = localeCode;
        public string NormalizationProfile { get; } = normalizationProfile;
        public string? OrdinalNumberToWordsKind { get; } = ordinalNumberToWordsKind;
        public string? OrdinalGenderVariant { get; } = ordinalGenderVariant;
        public ImmutableArray<TokenMapEntry> OrdinalEntries { get; } = ordinalEntries;
        public ImmutableArray<TokenMapEntry> CompositeScaleEntries { get; } = compositeScaleEntries;
        public ImmutableArray<string> NegativePrefixes { get; } = negativePrefixes;
        public ImmutableArray<string> NegativeSuffixes { get; } = negativeSuffixes;
        public ImmutableArray<string> OrdinalPrefixes { get; } = ordinalPrefixes;
        public ImmutableArray<string> IgnoredTokens { get; } = ignoredTokens;
        public ImmutableArray<string> MultiplierTokens { get; } = multiplierTokens;
        public ImmutableArray<string> TokenSuffixesToStrip { get; } = tokenSuffixesToStrip;
        public ImmutableArray<string> OrdinalAbbreviationSuffixes { get; } = ordinalAbbreviationSuffixes;
        public ImmutableArray<string> Aliases { get; } = aliases;
        public bool AllowTerminalOrdinalToken { get; } = allowTerminalOrdinalToken;
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
}

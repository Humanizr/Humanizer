using System.Collections.Immutable;
using System.Text;

using Microsoft.CodeAnalysis;

namespace Humanizer.SourceGenerators;

public sealed partial class HumanizerSourceGenerator
{
    internal static int CompareInflectionKeys(string left, string right) =>
        global::Humanizer.InflectionUnicodeData.CompareSimpleCase(left, right);

    internal static int FoldInflectionScalar(int scalar) =>
        global::Humanizer.InflectionUnicodeData.FoldSimpleCase(scalar);

    // Owners use ushort indexes through 65,535 entries, then fall back to int indexes.
    internal static bool RequiresWideInflectionIndexes(int entryCount) =>
        entryCount > ushort.MaxValue;

    sealed partial class InflectionCatalogInput(
        ImmutableArray<InflectionProfileInput> profiles,
        ImmutableArray<InflectionOwnerInput> owners,
        ImmutableArray<Diagnostic> diagnostics,
        bool hasErrors)
    {
        internal const int MaximumOwnerSourceBytes = 256 * 1024;
        const string ExactNumericSingletonSelector = "exact-numeric-singleton";

        static readonly ImmutableHashSet<string> CardinalRules =
            new[]
            {
                "Other", "AmharicLike", "Armenian", "EnglishLike", "Sinhala", "Punjabi",
                "One", "Danish", "Icelandic", "Macedonian", "Filipino", "Latvian",
                "Hebrew", "Romanian", "SouthSlavic", "French", "Portuguese",
                "CatalanItalian", "Spanish", "Slovenian", "CzechSlovak", "Polish",
                "Belarusian", "Lithuanian", "RussianUkrainian", "Maltese", "Irish",
                "Arabic", "Welsh"
            }.ToImmutableHashSet(StringComparer.Ordinal);

        static readonly ImmutableHashSet<string> Countabilities =
            new[] { "count", "mass", "collective", "plural-only" }
                .ToImmutableHashSet(StringComparer.Ordinal);

        static readonly ImmutableHashSet<string> PluralCategories =
            new[] { "zero", "one", "two", "few", "many", "other" }
                .ToImmutableHashSet(StringComparer.Ordinal);

        static readonly ImmutableHashSet<string> Scripts =
            new[]
            {
                "Arab", "Armn", "Beng", "Cyrl", "Deva", "Geor", "Grek", "Gujr",
                "Guru", "Hani", "Hebr", "Jpan", "Khmr", "Knda", "Kore", "Laoo",
                "Latn", "Mlym", "Mymr", "Orya", "Taml", "Telu", "Thai", "Ethi",
                "Mong", "Sinh"
            }.ToImmutableHashSet(StringComparer.Ordinal);

        readonly ImmutableArray<InflectionProfileInput> profiles = profiles;
        readonly ImmutableArray<InflectionOwnerInput> owners = owners;
        readonly ImmutableArray<Diagnostic> diagnostics = diagnostics;
        readonly bool hasErrors = hasErrors;

        public bool HasErrors => hasErrors;

        public static InflectionCatalogInput Create(
            LocaleCatalogInput localeCatalog,
            InflectionOwnerSourceCatalog ownerSourceCatalog)
        {
            var profiles = ImmutableArray.CreateBuilder<InflectionProfileInput>();
            var owners = ImmutableArray.CreateBuilder<InflectionOwnerInput>();
            var diagnostics = ImmutableArray.CreateBuilder<Diagnostic>();
            if (!localeCatalog.Diagnostics.IsDefaultOrEmpty)
            {
                return new(
                    profiles.ToImmutable(),
                    owners.ToImmutable(),
                    diagnostics.ToImmutable(),
                    hasErrors: true);
            }

            if (!localeCatalog.Locales.Any(static locale =>
                    locale.AuthoredFeatureNames.Contains("inflection")))
            {
                return new(
                    profiles.ToImmutable(),
                    owners.ToImmutable(),
                    diagnostics.ToImmutable(),
                    hasErrors: false);
            }

            foreach (var locale in localeCatalog.Locales)
            {
                _ = localeCatalog.InflectionOwners.TryGetTerminalOwner(
                    locale.LocaleCode,
                    out var terminalOwner);
                var requiresAuthoredProfile = locale.VariantOf is null ||
                    !SharesLanguageSubtag(locale.LocaleCode, locale.VariantOf);
                if (requiresAuthoredProfile &&
                    !locale.AuthoredFeatureNames.Contains("inflection"))
                {
                    diagnostics.Add(CreateDiagnostic(
                        locale.LocaleCode,
                        "Every root or cross-language locale profile must author surfaces.inflection explicitly."));
                    continue;
                }

                if (locale.Inflection is null)
                {
                    diagnostics.Add(CreateDiagnostic(
                        locale.LocaleCode,
                        "No resolved inflection profile is available."));
                    continue;
                }

                try
                {
                    if (!locale.AuthoredFeatureNames.Contains("inflection"))
                    {
                        profiles.Add(new(
                            locale.LocaleCode,
                            ParseCardinalRule(locale.Inflection),
                            terminalOwner));
                        continue;
                    }

                    var profile = ParseProfile(locale.LocaleCode, locale.Inflection);
                    profiles.Add(new(profile.LocaleCode, profile.CardinalRule, terminalOwner));
                    profiles.AddRange(ParseRegionalProfiles(
                        locale.LocaleCode,
                        locale.Inflection,
                        terminalOwner));

                    if (profile.Owner is not null &&
                        localeCatalog.InflectionOwners.AtomicOwners.Contains(profile.Owner.Owner))
                    {
                        owners.Add(profile.Owner);
                    }
                }
                catch (InvalidOperationException exception)
                {
                    diagnostics.Add(CreateDiagnostic(locale.LocaleCode, exception.Message));
                }
            }

            var duplicateOwner = owners
                .GroupBy(static owner => owner.Owner, StringComparer.Ordinal)
                .FirstOrDefault(static group => group.Count() > 1);
            if (duplicateOwner is not null)
            {
                diagnostics.Add(CreateDiagnostic(
                    duplicateOwner.Key,
                    $"Atomic inflection owner '{duplicateOwner.Key}' is defined more than once."));
            }

            var duplicateProfile = profiles
                .GroupBy(static profile => profile.LocaleCode, StringComparer.Ordinal)
                .FirstOrDefault(static group => group.Count() > 1);
            if (duplicateProfile is not null)
            {
                diagnostics.Add(CreateDiagnostic(
                    duplicateProfile.Key,
                    $"Inflection profile key '{duplicateProfile.Key}' is defined more than once."));
            }

            ValidateReachableCategories(profiles, owners, diagnostics);

            var sanitizedCollision = owners
                .GroupBy(static owner => SanitizeIdentifier(owner.Owner), StringComparer.Ordinal)
                .FirstOrDefault(static group =>
                    group.Select(static owner => owner.Owner)
                        .Distinct(StringComparer.Ordinal)
                        .Skip(1)
                        .Any());
            if (sanitizedCollision is not null)
            {
                diagnostics.Add(CreateDiagnostic(
                    sanitizedCollision.Key,
                    $"Atomic inflection owners '{string.Join(
                        "', '",
                        sanitizedCollision.Select(static owner => owner.Owner).OrderBy(
                            static owner => owner,
                            StringComparer.Ordinal))}' produce the same generated identifier '{sanitizedCollision.Key}'."));
            }

            diagnostics.AddRange(ownerSourceCatalog.Diagnostics);

            return new(
                profiles.ToImmutable(),
                owners.ToImmutable(),
                diagnostics.ToImmutable(),
                hasErrors: diagnostics.Count > 0);
        }

        static string ParseCardinalRule(SimpleYamlMapping mapping)
        {
            var cardinalRule = mapping.GetScalar("cardinalRule")
                ?? throw new InvalidOperationException(
                    "surfaces.inflection must define cardinalRule.");
            return CardinalRules.Contains(cardinalRule)
                ? cardinalRule
                : throw new InvalidOperationException(
                    $"surfaces.inflection cardinalRule '{cardinalRule}' is not a CLDR 48.2 rule used by Humanizer.");
        }

        static bool SharesLanguageSubtag(string localeCode, string inheritedLocaleCode) =>
            string.Equals(
                GetLanguageSubtag(localeCode),
                GetLanguageSubtag(inheritedLocaleCode),
                StringComparison.OrdinalIgnoreCase);

        static string GetLanguageSubtag(string localeCode)
        {
            var separatorIndex = localeCode.IndexOf('-');
            return separatorIndex >= 0
                ? localeCode.Substring(0, separatorIndex)
                : localeCode;
        }

        public void EmitDiagnostics(SourceProductionContext context)
        {
            foreach (var diagnostic in diagnostics)
            {
                context.ReportDiagnostic(diagnostic);
            }
        }

        public InflectionOwnerEmissionState GetOwnerEmissionState() =>
            new(
                hasErrors,
                owners
                    .Select(static owner => owner.Owner)
                    .OrderBy(static owner => owner, StringComparer.Ordinal)
                    .ToImmutableArray());

        public InflectionRegistryInput GetRegistryInput(LocaleCatalogInput localeCatalog) =>
            InflectionRegistryInput.Create(
                profiles,
                owners,
                localeCatalog.AcceptedCultures,
                hasErrors);

    }

    sealed class InflectionOwnerEmissionState(
        bool hasErrors,
        ImmutableArray<string> owners) : IEquatable<InflectionOwnerEmissionState>
    {
        readonly bool hasErrors = hasErrors;
        readonly ImmutableArray<string> owners = owners;

        public bool CanEmitOwner(string owner) =>
            !hasErrors && owners.Contains(owner, StringComparer.Ordinal);

        public bool Equals(InflectionOwnerEmissionState? other) =>
            other is not null &&
            hasErrors == other.hasErrors &&
            owners.SequenceEqual(other.owners, StringComparer.Ordinal);

        public override bool Equals(object? obj) =>
            Equals(obj as InflectionOwnerEmissionState);

        public override int GetHashCode()
        {
            var hash = hasErrors ? 1 : 0;
            foreach (var owner in owners)
            {
                hash = unchecked((hash * 397) ^ StringComparer.Ordinal.GetHashCode(owner));
            }

            return hash;
        }
    }

    sealed class PerLocaleInflectionSourceInput(
        string? owner,
        string? source,
        Diagnostic? diagnostic) : IEquatable<PerLocaleInflectionSourceInput>
    {
        public string? Owner { get; } = owner;
        public string? Source { get; } = source;
        public Diagnostic? Diagnostic { get; } = diagnostic;

        public static PerLocaleInflectionSourceInput Create(LocaleDefinitionFile? file)
        {
            var owner = InflectionCatalogInput.ParseOwner(file);
            if (owner is null)
            {
                return new(null, null, null);
            }

            var source = InflectionCatalogInput.BuildOwnerSource(owner);
            return Encoding.UTF8.GetByteCount(source) <=
                InflectionCatalogInput.MaximumOwnerSourceBytes
                ? new(owner.Owner, source, null)
                : new(
                    owner.Owner,
                    null,
                    InflectionCatalogInput.CreateDiagnostic(
                        owner.Owner,
                        $"Generated atomic inflection owner '{owner.Owner}' exceeds the 256 KiB syntax-tree ceiling."));
        }

        public bool Equals(PerLocaleInflectionSourceInput? other) =>
            other is not null &&
            string.Equals(Owner, other.Owner, StringComparison.Ordinal) &&
            string.Equals(Source, other.Source, StringComparison.Ordinal) &&
            string.Equals(
                Diagnostic?.GetMessage(),
                other.Diagnostic?.GetMessage(),
                StringComparison.Ordinal);

        public override bool Equals(object? obj) =>
            Equals(obj as PerLocaleInflectionSourceInput);

        public override int GetHashCode()
        {
            var hash = Owner is null ? 0 : StringComparer.Ordinal.GetHashCode(Owner);
            hash = unchecked((hash * 397) ^
                (Source is null ? 0 : StringComparer.Ordinal.GetHashCode(Source)));
            return unchecked((hash * 397) ^
                (Diagnostic?.GetMessage() is { } message
                    ? StringComparer.Ordinal.GetHashCode(message)
                    : 0));
        }
    }

    sealed class InflectionOwnerSourceCatalog(ImmutableArray<Diagnostic> diagnostics)
    {
        public ImmutableArray<Diagnostic> Diagnostics { get; } = diagnostics;

        public static InflectionOwnerSourceCatalog Create(
            ImmutableArray<PerLocaleInflectionSourceInput> sources) =>
            new(
                sources
                    .Select(static source => source.Diagnostic)
                    .Where(static diagnostic => diagnostic is not null)
                    .Cast<Diagnostic>()
                    .ToImmutableArray());
    }

    sealed class PerLocaleInflectionInput(PerLocaleInflectionSourceInput? source)
    {
        readonly PerLocaleInflectionSourceInput? source = source;

        public static PerLocaleInflectionInput Create(
            PerLocaleInflectionSourceInput source,
            InflectionOwnerEmissionState state)
        {
            return new(source.Owner is not null &&
                source.Source is not null &&
                state.CanEmitOwner(source.Owner)
                ? source
                : null);
        }

        public void Emit(SourceProductionContext context)
        {
            if (source?.Owner is { } owner &&
                source.Source is { } generatedSource)
            {
                InflectionCatalogInput.EmitOwner(context, owner, generatedSource);
            }
        }
    }

    sealed class ParsedProfile(
        string localeCode,
        string cardinalRule,
        InflectionOwnerInput? owner)
    {
        public string LocaleCode { get; } = localeCode;
        public string CardinalRule { get; } = cardinalRule;
        public InflectionOwnerInput? Owner { get; } = owner;
    }

    sealed class InflectionProfileInput(
        string localeCode,
        string cardinalRule,
        string? owner)
    {
        public string LocaleCode { get; } = localeCode;
        public string CardinalRule { get; } = cardinalRule;
        public string? Owner { get; } = owner;
    }

    sealed class InflectionOwnerInput(
        string owner,
        string cardinalRule,
        string capability,
        string quantitySelector,
        ImmutableArray<string> scripts,
        string casing,
        string phraseMode,
        ImmutableArray<string> skipSimpleWords,
        ImmutableArray<InflectionLexemeInput> lexemes,
        ImmutableArray<InflectionRuleInput> rules)
    {
        public string Owner { get; } = owner;
        public string CardinalRule { get; } = cardinalRule;
        public string Capability { get; } = capability;
        public string QuantitySelector { get; } = quantitySelector;
        public ImmutableArray<string> Scripts { get; } = scripts;
        public string Casing { get; } = casing;
        public string PhraseMode { get; } = phraseMode;
        public ImmutableArray<string> SkipSimpleWords { get; } = skipSimpleWords;
        public ImmutableArray<InflectionLexemeInput> Lexemes { get; } = lexemes;
        public ImmutableArray<InflectionRuleInput> Rules { get; } = rules;
    }

    sealed class InflectionLexemeInput(
        string id,
        string countability,
        string? sense,
        InflectionFormInput singular,
        InflectionFormInput dictionaryPlural,
        ImmutableDictionary<string, InflectionFormInput> display)
    {
        public string Id { get; } = id;
        public string Countability { get; } = countability;
        public string? Sense { get; } = sense;
        public InflectionFormInput Singular { get; } = singular;
        public InflectionFormInput DictionaryPlural { get; } = dictionaryPlural;
        public ImmutableDictionary<string, InflectionFormInput> Display { get; } = display;
    }

    sealed class InflectionFormInput(
        string preferred,
        ImmutableArray<string> accepted)
    {
        public string Preferred { get; } = preferred;
        public ImmutableArray<string> Accepted { get; } = accepted;
    }

    sealed class InflectionRuleInput(
        string id,
        string direction,
        int priority,
        ImmutableArray<string> countabilities,
        string prefix,
        string suffix,
        ImmutableArray<string> precedingNot,
        string dictionaryPlural,
        ImmutableDictionary<string, string> display,
        ImmutableArray<string> excludedSurfaces,
        ImmutableArray<string> excludedLexemes,
        bool reverseEnabled,
        bool requiresExistingLexeme,
        ImmutableArray<string> scripts)
    {
        public string Id { get; } = id;
        public string Direction { get; } = direction;
        public int Priority { get; } = priority;
        public ImmutableArray<string> Countabilities { get; } = countabilities;
        public string Prefix { get; } = prefix;
        public string Suffix { get; } = suffix;
        public ImmutableArray<string> PrecedingNot { get; } = precedingNot;
        public string DictionaryPlural { get; } = dictionaryPlural;
        public ImmutableDictionary<string, string> Display { get; } = display;
        public ImmutableArray<string> ExcludedSurfaces { get; } = excludedSurfaces;
        public ImmutableArray<string> ExcludedLexemes { get; } = excludedLexemes;
        public bool ReverseEnabled { get; } = reverseEnabled;
        public bool RequiresExistingLexeme { get; } = requiresExistingLexeme;
        public ImmutableArray<string> Scripts { get; } = scripts;
    }

    sealed class InflectionLexemeTableInput(
        ImmutableArray<InflectionLexemeRecordInput> lexemes,
        ImmutableArray<InflectionLexemeEntryInput> entries,
        ImmutableArray<InflectionLexemeCandidateInput> candidates,
        ImmutableArray<int> forwardEntries,
        ImmutableArray<int> reverseEntries)
    {
        public ImmutableArray<InflectionLexemeRecordInput> Lexemes { get; } = lexemes;
        public ImmutableArray<InflectionLexemeEntryInput> Entries { get; } = entries;
        public ImmutableArray<InflectionLexemeCandidateInput> Candidates { get; } = candidates;
        public ImmutableArray<int> ForwardEntries { get; } = forwardEntries;
        public ImmutableArray<int> ReverseEntries { get; } = reverseEntries;
    }

    sealed class InflectionLexemeRecordInput(
        int singularEntryIndex,
        int dictionaryPluralEntryIndex,
        ImmutableArray<InflectionLexemeDisplayInput> display)
    {
        public int SingularEntryIndex { get; } = singularEntryIndex;
        public int DictionaryPluralEntryIndex { get; } = dictionaryPluralEntryIndex;
        public ImmutableArray<InflectionLexemeDisplayInput> Display { get; } = display;
    }

    sealed class InflectionLexemeDisplayInput(
        string category,
        int preferredEntryIndex)
    {
        public string Category { get; } = category;
        public int PreferredEntryIndex { get; } = preferredEntryIndex;
    }

    sealed class InflectionLexemeEntryInput(
        string value,
        int candidateOffset,
        int candidateCount)
    {
        public string Value { get; } = value;
        public int CandidateOffset { get; } = candidateOffset;
        public int CandidateCount { get; } = candidateCount;
    }

    sealed class InflectionLexemeCandidateInput(
        int lexemeIndex,
        int roles)
    {
        public int LexemeIndex { get; } = lexemeIndex;
        public int Roles { get; } = roles;
    }
}
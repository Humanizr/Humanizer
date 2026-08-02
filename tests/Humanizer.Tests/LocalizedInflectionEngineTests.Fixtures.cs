using System.Diagnostics.CodeAnalysis;

public partial class LocalizedInflectionEngineTests
{
    [Fact]
    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2026",
        Justification = "This test intentionally probes for removed internal fixture types by their historical assembly-qualified names.")]
    public void RuntimeAssemblyDoesNotShipFixtureOnlyLexemeBuilders()
    {
        var assembly = typeof(InflectionBundle).Assembly;
        var bundleMethods = typeof(InflectionBundle).GetMethods(
            System.Reflection.BindingFlags.Static |
            System.Reflection.BindingFlags.NonPublic);

        Assert.Null(assembly.GetType("Humanizer.InflectionDisplayForm"));
        Assert.Null(assembly.GetType("Humanizer.InflectionLexeme"));
        Assert.DoesNotContain(bundleMethods, method => method.Name == "BuildData");
    }

    readonly struct InflectionDisplayForm(
        CardinalPluralCategory category,
        string preferred,
        string[] accepted)
    {
        public CardinalPluralCategory Category { get; } = category;
        public string Preferred { get; } = preferred;
        public string[] Accepted { get; } = accepted;
    }

    sealed class InflectionLexeme(
        string id,
        string singular,
        string dictionaryPlural,
        string[] acceptedSingular,
        string[] acceptedDictionaryPlural,
        InflectionDisplayForm[] display,
        InflectionCountability countability = InflectionCountability.Count)
    {
        public string Id { get; } = id;
        public string Singular { get; } = singular;
        public string DictionaryPlural { get; } = dictionaryPlural;
        public string[] AcceptedSingular { get; } = acceptedSingular;
        public string[] AcceptedDictionaryPlural { get; } = acceptedDictionaryPlural;
        public InflectionDisplayForm[] Display { get; } = display;
        public InflectionCountability Countability { get; } = countability;
    }

    static InflectionBundle CreateBundle(
        string owner,
        CardinalPluralRuleKind cardinalRule,
        InflectionCasing casing,
        string[] scripts,
        string[] skipSimpleWords,
        InflectionLexeme[] lexemes,
        InflectionRule[] rules) =>
        CreateBundle(
            owner,
            cardinalRule,
            InflectionCapability.DisplayByCategory,
            InflectionQuantitySelector.None,
            casing,
            scripts,
            skipSimpleWords,
            lexemes,
            rules);

    static InflectionBundle CreateBundle(
        string owner,
        CardinalPluralRuleKind cardinalRule,
        InflectionCapability capability,
        InflectionCasing casing,
        string[] scripts,
        string[] skipSimpleWords,
        InflectionLexeme[] lexemes,
        InflectionRule[] rules) =>
        CreateBundle(
            owner,
            cardinalRule,
            capability,
            InflectionQuantitySelector.None,
            casing,
            scripts,
            skipSimpleWords,
            lexemes,
            rules);

    static InflectionBundle CreateBundle(
        string owner,
        CardinalPluralRuleKind cardinalRule,
        InflectionCapability capability,
        InflectionQuantitySelector quantitySelector,
        InflectionCasing casing,
        string[] scripts,
        string[] skipSimpleWords,
        InflectionLexeme[] lexemes,
        InflectionRule[] rules)
    {
        var (compactLexemes, entries, candidates) = BuildFixtureData(lexemes, casing);
        if (entries.Length > ushort.MaxValue)
        {
            var indexes = Enumerable.Range(0, entries.Length).ToArray();
            return new(
                owner,
                cardinalRule,
                capability,
                quantitySelector,
                casing,
                scripts,
                skipSimpleWords,
                compactLexemes,
                entries,
                candidates,
                indexes,
                indexes,
                rules);
        }

        var compactIndexes = Enumerable.Range(0, entries.Length)
            .Select(static index => (ushort)index)
            .ToArray();
        return new(
            owner,
            cardinalRule,
            capability,
            quantitySelector,
            casing,
            scripts,
            skipSimpleWords,
            compactLexemes,
            entries,
            candidates,
            compactIndexes,
            compactIndexes,
            rules);
    }

    static (
        InflectionLexemeRecord[] Lexemes,
        InflectionLexemeEntry[] Entries,
        InflectionLexemeCandidate[] Candidates) BuildFixtureData(
        InflectionLexeme[] lexemes,
        InflectionCasing casing)
    {
        var comparer = casing == InflectionCasing.LowerTitleUpper
            ? InflectionUnicodeData.SimpleCaseComparer.Instance
            : StringComparer.Ordinal;
        var candidatesBySurface =
            new Dictionary<string, Dictionary<int, InflectionExactRole>>(comparer);
        for (var index = 0; index < lexemes.Length; index++)
        {
            var lexeme = lexemes[index];
            AddFixtureCandidates(
                candidatesBySurface,
                lexeme.AcceptedSingular,
                index,
                InflectionExactRole.Singular);
            AddFixtureCandidates(
                candidatesBySurface,
                lexeme.AcceptedDictionaryPlural,
                index,
                InflectionExactRole.DictionaryPlural);
            foreach (var display in lexeme.Display)
            {
                AddFixtureCandidates(
                    candidatesBySurface,
                    display.Accepted,
                    index,
                    GetFixtureDisplayRole(display.Category));
            }
        }

        var entryIndexBySurface = new Dictionary<string, int>(comparer);
        var entries = new List<InflectionLexemeEntry>(candidatesBySurface.Count);
        var candidates = new List<InflectionLexemeCandidate>();
        foreach (var pair in candidatesBySurface.OrderBy(
                     static pair => pair.Key,
                     comparer))
        {
            entryIndexBySurface.Add(pair.Key, entries.Count);
            var candidateOffset = candidates.Count;
            foreach (var candidate in pair.Value.OrderBy(
                         static candidate => candidate.Key))
            {
                candidates.Add(new(candidate.Key, candidate.Value));
            }

            entries.Add(new(
                pair.Key,
                candidateOffset,
                candidates.Count - candidateOffset));
        }

        var compactLexemes = new InflectionLexemeRecord[lexemes.Length];
        for (var index = 0; index < lexemes.Length; index++)
        {
            var lexeme = lexemes[index];
            compactLexemes[index] = new(
                lexeme.Id,
                entryIndexBySurface[lexeme.Singular],
                entryIndexBySurface[lexeme.DictionaryPlural],
                lexeme.Display
                    .Select(display => new InflectionLexemeDisplay(
                        display.Category,
                        entryIndexBySurface[display.Preferred]))
                    .ToArray(),
                lexeme.Countability);
        }

        return (compactLexemes, entries.ToArray(), candidates.ToArray());
    }

    static void AddFixtureCandidates(
        Dictionary<string, Dictionary<int, InflectionExactRole>> candidatesBySurface,
        string[] forms,
        int lexemeIndex,
        InflectionExactRole role)
    {
        foreach (var form in forms)
        {
            if (!candidatesBySurface.TryGetValue(form, out var candidates))
            {
                candidates = [];
                candidatesBySurface[form] = candidates;
            }

            candidates.TryGetValue(lexemeIndex, out var roles);
            candidates[lexemeIndex] = roles | role;
        }
    }

    static InflectionExactRole GetFixtureDisplayRole(CardinalPluralCategory category) =>
        category switch
        {
            CardinalPluralCategory.Zero => InflectionExactRole.Zero,
            CardinalPluralCategory.One => InflectionExactRole.One,
            CardinalPluralCategory.Two => InflectionExactRole.Two,
            CardinalPluralCategory.Few => InflectionExactRole.Few,
            CardinalPluralCategory.Many => InflectionExactRole.Many,
            CardinalPluralCategory.Other => InflectionExactRole.Other,
            _ => InflectionExactRole.None
        };
}
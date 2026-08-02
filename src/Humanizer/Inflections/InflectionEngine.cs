namespace Humanizer;

enum InflectionStatus
{
    Exact,
    Productive,
    Invariant,
    Ambiguous,
    Unsupported,
    Unknown
}

enum InflectionDirection
{
    Forward,
    Reverse
}

enum InflectionCasing
{
    Exact,
    LowerTitleUpper,
    None
}

enum InflectionCapability
{
    DisplayByCategory,
    Invariant
}

enum InflectionQuantitySelector
{
    None,
    ExactNumericSingleton
}

[Flags]
enum InflectionCountability : byte
{
    None = 0,
    Count = 1 << 0,
    Mass = 1 << 1,
    Collective = 1 << 2,
    PluralOnly = 1 << 3,
    All = Count | Mass | Collective | PluralOnly
}

readonly struct InflectionQuantity
{
    readonly CardinalPluralOperands operands;
    readonly decimal decimalValue;
    readonly double doubleValue;
    readonly InflectionQuantityKind kind;

    public InflectionQuantity(CardinalPluralOperands operands)
    {
        this.operands = operands;
        decimalValue = default;
        doubleValue = default;
        kind = InflectionQuantityKind.Operands;
    }

    public InflectionQuantity(decimal value)
    {
        operands = default;
        decimalValue = value;
        doubleValue = default;
        kind = InflectionQuantityKind.Decimal;
    }

    public InflectionQuantity(double value)
    {
        operands = default;
        decimalValue = default;
        doubleValue = value;
        kind = InflectionQuantityKind.Double;
    }

    public bool HasValue => kind != InflectionQuantityKind.None;

    public bool IsFinite =>
        kind switch
        {
            InflectionQuantityKind.Operands => operands.IsSupported,
            InflectionQuantityKind.Double => double.IsFinite(doubleValue),
            _ => true
        };

    public bool IsExactNumericSingleton =>
        kind switch
        {
            InflectionQuantityKind.Operands => operands.IntegerDigits.IsOne && operands.FractionDigits.IsZero,
            InflectionQuantityKind.Decimal => decimalValue is 1m or -1m,
            InflectionQuantityKind.Double => doubleValue is 1d or -1d,
            _ => false
        };
}

enum InflectionQuantityKind : byte
{
    None,
    Operands,
    Decimal,
    Double
}

readonly struct InflectionResult(InflectionStatus status, string value)
{
    public InflectionStatus Status { get; } = status;
    public string Value { get; } = value;
}

readonly struct InflectionLexemeDisplay(
    CardinalPluralCategory category,
    int preferredEntryIndex)
{
    public CardinalPluralCategory Category { get; } = category;
    public int PreferredEntryIndex { get; } = preferredEntryIndex;
}

readonly struct InflectionLexemeRecord(
    string id,
    int singularEntryIndex,
    int dictionaryPluralEntryIndex,
    InflectionLexemeDisplay[] display,
    InflectionCountability countability)
{
    public string Id { get; } = id;
    public int SingularEntryIndex { get; } = singularEntryIndex;
    public int DictionaryPluralEntryIndex { get; } = dictionaryPluralEntryIndex;
    public InflectionLexemeDisplay[] Display { get; } = display;
    public InflectionCountability Countability { get; } = countability;

    public bool TryGetDisplay(
        CardinalPluralCategory category,
        out int entryIndex)
    {
        foreach (var form in Display)
        {
            if (form.Category == category)
            {
                entryIndex = form.PreferredEntryIndex;
                return true;
            }
        }

        entryIndex = -1;
        return false;
    }
}

readonly struct InflectionLexemeEntry(
    string value,
    int candidateOffset,
    int candidateCount)
{
    public string Value { get; } = value;
    public int CandidateOffset { get; } = candidateOffset;
    public int CandidateCount { get; } = candidateCount;
}

readonly struct InflectionLexemeCandidate(
    int lexemeIndex,
    InflectionExactRole roles)
{
    public int LexemeIndex { get; } = lexemeIndex;
    public InflectionExactRole Roles { get; } = roles;
}

readonly struct InflectionEntryIndexes
{
    readonly ushort[]? compact;
    readonly int[]? wide;

    public InflectionEntryIndexes(ushort[] compact)
    {
        this.compact = compact;
        wide = null;
    }

    public InflectionEntryIndexes(int[] wide)
    {
        compact = null;
        this.wide = wide;
    }

    public int Length => compact?.Length ?? wide!.Length;

    public int this[int index] =>
        compact is null
            ? wide![index]
            : compact[index];

    public bool Contains(int value)
    {
        for (var index = 0; index < Length; index++)
        {
            if (this[index] == value)
            {
                return true;
            }
        }

        return false;
    }
}

readonly struct InflectionRuleDisplay(
    CardinalPluralCategory category,
    string template)
{
    public CardinalPluralCategory Category { get; } = category;
    public string Template { get; } = template;
}


readonly struct InflectionRule
{
    public InflectionRule(
        string id,
        InflectionDirection direction,
        int priority,
        string suffix,
        string dictionaryPlural,
        bool reverseEnabled,
        bool requiresExistingLexeme,
        InflectionUnicodeScripts scripts = InflectionUnicodeScripts.All,
        InflectionCountability countabilities = InflectionCountability.All)
        : this(
            id,
            direction,
            priority,
            prefix: string.Empty,
            suffix,
            precedingNot: [],
            dictionaryPlural,
            display: [],
            excludedSurfaces: [],
            excludedLexemes: Array.Empty<ushort>(),
            reverseEnabled,
            requiresExistingLexeme,
            scripts,
            countabilities)
    {
    }

    public InflectionRule(
        string id,
        InflectionDirection direction,
        int priority,
        string prefix,
        string suffix,
        string[] precedingNot,
        string dictionaryPlural,
        InflectionRuleDisplay[] display,
        string[] excludedSurfaces,
        bool reverseEnabled,
        bool requiresExistingLexeme,
        InflectionUnicodeScripts scripts = InflectionUnicodeScripts.All,
        InflectionCountability countabilities = InflectionCountability.All)
        : this(
            id,
            direction,
            priority,
            prefix,
            suffix,
            precedingNot,
            dictionaryPlural,
            display,
            excludedSurfaces,
            excludedLexemes: Array.Empty<ushort>(),
            reverseEnabled,
            requiresExistingLexeme,
            scripts,
            countabilities)
    {
    }

    public InflectionRule(
        string id,
        InflectionDirection direction,
        int priority,
        string prefix,
        string suffix,
        string[] precedingNot,
        string dictionaryPlural,
        InflectionRuleDisplay[] display,
        string[] excludedSurfaces,
        ushort[] excludedLexemes,
        bool reverseEnabled,
        bool requiresExistingLexeme,
        InflectionUnicodeScripts scripts = InflectionUnicodeScripts.All,
        InflectionCountability countabilities = InflectionCountability.All)
        : this(
            id,
            direction,
            priority,
            prefix,
            suffix,
            precedingNot,
            dictionaryPlural,
            display,
            excludedSurfaces,
            new InflectionEntryIndexes(excludedLexemes),
            reverseEnabled,
            requiresExistingLexeme,
            scripts,
            countabilities)
    {
    }

    public InflectionRule(
        string id,
        InflectionDirection direction,
        int priority,
        string prefix,
        string suffix,
        string[] precedingNot,
        string dictionaryPlural,
        InflectionRuleDisplay[] display,
        string[] excludedSurfaces,
        int[] excludedLexemes,
        bool reverseEnabled,
        bool requiresExistingLexeme,
        InflectionUnicodeScripts scripts = InflectionUnicodeScripts.All,
        InflectionCountability countabilities = InflectionCountability.All)
        : this(
            id,
            direction,
            priority,
            prefix,
            suffix,
            precedingNot,
            dictionaryPlural,
            display,
            excludedSurfaces,
            new InflectionEntryIndexes(excludedLexemes),
            reverseEnabled,
            requiresExistingLexeme,
            scripts,
            countabilities)
    {
    }

    InflectionRule(
        string id,
        InflectionDirection direction,
        int priority,
        string prefix,
        string suffix,
        string[] precedingNot,
        string dictionaryPlural,
        InflectionRuleDisplay[] display,
        string[] excludedSurfaces,
        InflectionEntryIndexes excludedLexemes,
        bool reverseEnabled,
        bool requiresExistingLexeme,
        InflectionUnicodeScripts scripts,
        InflectionCountability countabilities)
    {
        Id = id;
        Direction = direction;
        Priority = priority;
        Prefix = prefix;
        Suffix = suffix;
        PrecedingNot = precedingNot;
        DictionaryPlural = dictionaryPlural;
        Display = display;
        ExcludedSurfaces = excludedSurfaces;
        ExcludedLexemes = excludedLexemes;
        ReverseEnabled = reverseEnabled;
        RequiresExistingLexeme = requiresExistingLexeme;
        Scripts = scripts;
        Countabilities = countabilities;
    }

    public string Id { get; }
    public InflectionDirection Direction { get; }
    public int Priority { get; }
    public string Prefix { get; }
    public string Suffix { get; }
    public string[] PrecedingNot { get; }
    public string DictionaryPlural { get; }
    public InflectionRuleDisplay[] Display { get; }
    public string[] ExcludedSurfaces { get; }
    public InflectionEntryIndexes ExcludedLexemes { get; }
    public bool ReverseEnabled { get; }
    public bool RequiresExistingLexeme { get; }
    public InflectionUnicodeScripts Scripts { get; }
    public InflectionCountability Countabilities { get; }

    public bool SupportsScripts(InflectionUnicodeScripts detectedScripts) =>
        (Scripts & detectedScripts) != InflectionUnicodeScripts.None;

    public bool SupportsCountability(InflectionCountability countability) =>
        countability == InflectionCountability.None
            ? Countabilities == InflectionCountability.All
            : (Countabilities & countability) != InflectionCountability.None;

    public bool TryGetTemplate(
        CardinalPluralCategory? category,
        [NotNullWhen(true)] out string? template)
    {
        if (category is null)
        {
            template = DictionaryPlural;
            return true;
        }

        foreach (var form in Display)
        {
            if (form.Category == category)
            {
                template = form.Template;
                return true;
            }
        }

        template = null;
        return false;
    }
}

[Flags]
enum InflectionExactRole : byte
{
    None = 0,
    Singular = 1,
    DictionaryPlural = 2,
    Zero = 4,
    One = 8,
    Two = 16,
    Few = 32,
    Many = 64,
    Other = 128
}

sealed class InflectionBundle
{
    readonly InflectionCasing casing;
    readonly InflectionQuantitySelector quantitySelector;
    readonly string[] scripts;
    readonly string[] skipSimpleWords;
    readonly InflectionLexemeRecord[] lexemes;
    readonly InflectionLexemeEntry[] entries;
    readonly InflectionLexemeCandidate[] candidates;
    readonly InflectionEntryIndexes forwardEntries;
    readonly InflectionEntryIndexes reverseEntries;
    readonly InflectionRule[] rules;

    public InflectionBundle(
        string owner,
        CardinalPluralRuleKind cardinalRule,
        InflectionCapability capability,
        InflectionQuantitySelector quantitySelector,
        InflectionCasing casing,
        string[] scripts,
        string[] skipSimpleWords,
        InflectionLexemeRecord[] lexemes,
        InflectionLexemeEntry[] entries,
        InflectionLexemeCandidate[] candidates,
        ushort[] forwardEntries,
        ushort[] reverseEntries,
        InflectionRule[] rules)
        : this(
            owner,
            cardinalRule,
            capability,
            quantitySelector,
            casing,
            scripts,
            skipSimpleWords,
            lexemes,
            entries,
            candidates,
            new InflectionEntryIndexes(forwardEntries),
            new InflectionEntryIndexes(reverseEntries),
            rules)
    {
    }

    public InflectionBundle(
        string owner,
        CardinalPluralRuleKind cardinalRule,
        InflectionCapability capability,
        InflectionQuantitySelector quantitySelector,
        InflectionCasing casing,
        string[] scripts,
        string[] skipSimpleWords,
        InflectionLexemeRecord[] lexemes,
        InflectionLexemeEntry[] entries,
        InflectionLexemeCandidate[] candidates,
        int[] forwardEntries,
        int[] reverseEntries,
        InflectionRule[] rules)
        : this(
            owner,
            cardinalRule,
            capability,
            quantitySelector,
            casing,
            scripts,
            skipSimpleWords,
            lexemes,
            entries,
            candidates,
            new InflectionEntryIndexes(forwardEntries),
            new InflectionEntryIndexes(reverseEntries),
            rules)
    {
    }

    InflectionBundle(
        string owner,
        CardinalPluralRuleKind cardinalRule,
        InflectionCapability capability,
        InflectionQuantitySelector quantitySelector,
        InflectionCasing casing,
        string[] scripts,
        string[] skipSimpleWords,
        InflectionLexemeRecord[] lexemes,
        InflectionLexemeEntry[] entries,
        InflectionLexemeCandidate[] candidates,
        InflectionEntryIndexes forwardEntries,
        InflectionEntryIndexes reverseEntries,
        InflectionRule[] rules)
    {
        Owner = owner;
        CardinalRule = cardinalRule;
        Capability = capability;
        this.quantitySelector = quantitySelector;
        this.casing = casing;
        this.scripts = scripts;
        this.skipSimpleWords = skipSimpleWords;
        this.lexemes = lexemes;
        this.entries = entries;
        this.candidates = candidates;
        this.forwardEntries = forwardEntries;
        this.reverseEntries = reverseEntries;
        this.rules = rules;
    }

    public string Owner { get; }
    public CardinalPluralRuleKind CardinalRule { get; }
    public InflectionCapability Capability { get; }

    public InflectionResult Inflect(
        string input,
        InflectionDirection direction,
        bool allowProductive,
        CardinalPluralCategory? category) =>
        Inflect(
            input,
            direction,
            allowProductive,
            category,
            quantity: default);

    public InflectionResult Inflect(
        string input,
        InflectionDirection direction,
        bool allowProductive,
        int quantity) =>
        Inflect(input, direction, allowProductive, (decimal)quantity);

    public InflectionResult Inflect(
        string input,
        InflectionDirection direction,
        bool allowProductive,
        long quantity) =>
        Inflect(input, direction, allowProductive, (decimal)quantity);

    public InflectionResult Inflect(
        string input,
        InflectionDirection direction,
        bool allowProductive,
        decimal quantity)
    {
        var selectedCategory = CardinalRule == CardinalPluralRuleKind.Other
            ? CardinalPluralCategory.Other
            : CardinalPluralRules.Select(CardinalRule, quantity);
        var inflectionQuantity = new InflectionQuantity(quantity);
        return Inflect(
            input,
            direction,
            allowProductive,
            selectedCategory,
            in inflectionQuantity);
    }

    public InflectionResult Inflect(
        string input,
        InflectionDirection direction,
        bool allowProductive,
        CardinalPluralCategory category,
        CardinalPluralOperands operands)
    {
        var quantity = new InflectionQuantity(operands);
        return Inflect(
            input,
            direction,
            allowProductive,
            category,
            in quantity);
    }

    public InflectionResult Inflect(
        string input,
        InflectionDirection direction,
        bool allowProductive,
        CardinalPluralCategory category,
        double quantity)
    {
        var inflectionQuantity = new InflectionQuantity(quantity);
        return Inflect(
            input,
            direction,
            allowProductive,
            category,
            in inflectionQuantity);
    }

    public InflectionResult Inflect(
        string input,
        InflectionDirection direction,
        bool allowProductive,
        double quantity)
    {
        var inflectionQuantity = new InflectionQuantity(quantity);
        if (CardinalRule == CardinalPluralRuleKind.Other)
        {
            return Inflect(
                input,
                direction,
                allowProductive,
                CardinalPluralCategory.Other,
                in inflectionQuantity);
        }

        if (!CardinalPluralRules.TrySelect(
                CardinalRule,
                quantity,
                out var selectedCategory))
        {
            return Inflect(
                input,
                direction,
                allowProductive,
                category: null,
                in inflectionQuantity);
        }

        return Inflect(
            input,
            direction,
            allowProductive,
            selectedCategory,
            in inflectionQuantity);
    }

    InflectionResult Inflect(
        string input,
        InflectionDirection direction,
        bool allowProductive,
        CardinalPluralCategory? category,
        in InflectionQuantity quantity)
    {
        if (quantity.HasValue && !quantity.IsFinite)
        {
            return new(InflectionStatus.Unsupported, input);
        }

        if (Capability == InflectionCapability.Invariant)
        {
            return new(InflectionStatus.Invariant, input);
        }

        if (!IsWellFormedUtf16(input))
        {
            return new(InflectionStatus.Unsupported, input);
        }

        var normalized = IsDefinitelyNormalized(input)
            ? input
            : input.Normalize(NormalizationForm.FormC);
        var projection = GetProjection(normalized);
        if (!IsScriptEligible(normalized, allowNonLetters: true))
        {
            return new(InflectionStatus.Unsupported, input);
        }

        var exact = TryExact(
            input,
            normalized,
            direction,
            allowProductive,
            category,
            quantity,
            projection);
        if (exact.Status != InflectionStatus.Unknown)
        {
            return exact.Value == input
                ? new(exact.Status, input)
                : exact;
        }

        if (quantity.HasValue &&
            (!quantity.IsFinite || category is null))
        {
            return new(InflectionStatus.Unsupported, input);
        }

        if (projection == CaseProjection.Mixed)
        {
            return new(InflectionStatus.Unsupported, input);
        }

        if (!IsEligibleToken(normalized, out var detectedScripts))
        {
            return new(InflectionStatus.Unsupported, input);
        }

        if (!allowProductive)
        {
            return new(InflectionStatus.Unknown, input);
        }

        var comparison = casing == InflectionCasing.LowerTitleUpper
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;
        if (direction == InflectionDirection.Reverse)
        {
            return InflectReverseProductive(
                input,
                normalized,
                category,
                projection,
                comparison,
                detectedScripts);
        }

        var countability = TryFindAcceptedSingular(
                normalized,
                comparison,
                out var lexemeIndex)
            ? lexemes[lexemeIndex].Countability
            : InflectionCountability.None;
        InflectionRule? selectedRule = null;
        string? selectedStem = null;
        foreach (var rule in rules)
        {
            if (rule.Direction == InflectionDirection.Forward &&
                rule.SupportsScripts(detectedScripts) &&
                rule.SupportsCountability(countability) &&
                !Contains(rule.ExcludedSurfaces, normalized, comparison) &&
                TryGetStem(rule, normalized, comparison, out var stem))
            {
                if (selectedRule is null ||
                    CompareRuleRank(rule, selectedRule.Value) > 0)
                {
                    selectedRule = rule;
                    selectedStem = stem;
                    continue;
                }

                if (CompareRuleRank(rule, selectedRule.Value) == 0)
                {
                    if (!rule.TryGetTemplate(category, out var candidateTemplate) ||
                        !selectedRule.Value.TryGetTemplate(category, out var selectedTemplate))
                    {
                        return new(InflectionStatus.Unsupported, input);
                    }

                    var candidateOutput = candidateTemplate.Replace("{stem}", stem);
                    var selectedOutput = selectedTemplate.Replace("{stem}", selectedStem);
                    if (!string.Equals(candidateOutput, selectedOutput, comparison))
                    {
                        return new(InflectionStatus.Ambiguous, input);
                    }
                }
            }
        }

        if (selectedRule is null)
        {
            return new(InflectionStatus.Unknown, input);
        }

        if (!selectedRule.Value.TryGetTemplate(category, out var template))
        {
            return new(InflectionStatus.Unsupported, input);
        }

        var output = template.Replace("{stem}", selectedStem);
        return Project(input, output, projection, InflectionStatus.Productive);
    }

    InflectionResult InflectReverseProductive(
        string input,
        string normalized,
        CardinalPluralCategory? category,
        CaseProjection projection,
        StringComparison comparison,
        InflectionUnicodeScripts detectedScripts)
    {
        InflectionRule? selectedDirectRule = null;
        var selectedDirectCandidate = default(ReverseCandidate);
        var reverseCandidate = default(ReverseCandidate);
        var hasReverseCandidate = false;
        foreach (var rule in rules)
        {
            if (rule.Direction == InflectionDirection.Reverse &&
                rule.SupportsScripts(detectedScripts) &&
                !Contains(rule.ExcludedSurfaces, normalized, comparison) &&
                TryGetStem(rule, normalized, comparison, out var stem))
            {
                if (!rule.TryGetTemplate(category, out var template))
                {
                    return new(InflectionStatus.Unsupported, input);
                }

                var directResult = template.Replace("{stem}", stem);
                var directCandidate = new ReverseCandidate(
                    directResult,
                    directResult.Length,
                    prefix: string.Empty,
                    suffix: string.Empty);
                if (IsReverseCandidateAllowed(rule, directCandidate, comparison) &&
                    !TrySelectDirectReverseCandidate(
                        rule,
                        directCandidate,
                        comparison,
                        ref selectedDirectRule,
                        ref selectedDirectCandidate))
                {
                    return new(InflectionStatus.Ambiguous, input);
                }
            }

            if (rule.Direction == InflectionDirection.Forward &&
                rule.ReverseEnabled &&
                rule.SupportsScripts(detectedScripts) &&
                TryCreateReverseCandidate(
                    rule,
                    normalized,
                    comparison,
                    out var candidate))
            {
                if (hasReverseCandidate &&
                    !reverseCandidate.Equals(candidate, comparison))
                {
                    return new(InflectionStatus.Ambiguous, input);
                }

                reverseCandidate = candidate;
                hasReverseCandidate = true;
            }
        }

        if (selectedDirectRule is not null)
        {
            if (hasReverseCandidate &&
                !selectedDirectCandidate.Equals(reverseCandidate, comparison))
            {
                return new(InflectionStatus.Ambiguous, input);
            }

            return Project(
                input,
                selectedDirectCandidate.Materialize(),
                projection,
                InflectionStatus.Productive);
        }

        return hasReverseCandidate
            ? Project(
                input,
                reverseCandidate.Materialize(),
                projection,
                InflectionStatus.Productive)
            : new(InflectionStatus.Unknown, input);
    }

    static bool TrySelectDirectReverseCandidate(
        InflectionRule rule,
        ReverseCandidate candidate,
        StringComparison comparison,
        ref InflectionRule? selectedRule,
        ref ReverseCandidate selectedCandidate)
    {
        if (selectedRule is null)
        {
            selectedRule = rule;
            selectedCandidate = candidate;
            return true;
        }

        var rank = CompareRuleRank(rule, selectedRule.Value);
        if (rank > 0)
        {
            selectedRule = rule;
            selectedCandidate = candidate;
            return true;
        }

        return rank != 0 ||
            selectedCandidate.Equals(candidate, comparison);
    }

    InflectionResult TryExact(
        string originalInput,
        string input,
        InflectionDirection direction,
        bool allowProductive,
        CardinalPluralCategory? category,
        in InflectionQuantity quantity,
        CaseProjection projection)
    {
        var entries = direction == InflectionDirection.Forward
            ? forwardEntries
            : reverseEntries;
        var low = 0;
        var high = entries.Length - 1;
        while (low <= high)
        {
            var middle = low + ((high - low) / 2);
            var entry = this.entries[entries[middle]];
            var comparison = casing == InflectionCasing.LowerTitleUpper
                ? CompareStrings(
                    entry.Value,
                    input,
                    StringComparison.OrdinalIgnoreCase)
                : string.CompareOrdinal(entry.Value, input);
            if (comparison < 0)
            {
                low = middle + 1;
                continue;
            }

            if (comparison > 0)
            {
                high = middle - 1;
                continue;
            }

            if (projection == CaseProjection.Mixed &&
                !string.Equals(entry.Value, input, StringComparison.Ordinal))
            {
                return new(InflectionStatus.Unknown, originalInput);
            }

            if (entry.CandidateCount != 1)
            {
                return new(InflectionStatus.Ambiguous, originalInput);
            }

            var candidate = candidates[entry.CandidateOffset];
            var lexeme = lexemes[candidate.LexemeIndex];
            if (quantity.HasValue && !quantity.IsFinite)
            {
                return new(InflectionStatus.Unsupported, originalInput);
            }

            if (quantity.HasValue && category is null)
            {
                return new(InflectionStatus.Unsupported, originalInput);
            }

            var selectedCategory = category;
            if (direction == InflectionDirection.Forward &&
                quantity.HasValue &&
                CardinalRule == CardinalPluralRuleKind.Other &&
                quantitySelector == InflectionQuantitySelector.ExactNumericSingleton &&
                lexeme.TryGetDisplay(CardinalPluralCategory.One, out _))
            {
                selectedCategory = quantity.IsExactNumericSingleton
                    ? CardinalPluralCategory.One
                    : CardinalPluralCategory.Other;
            }

            if ((direction == InflectionDirection.Forward &&
                 !allowProductive &&
                 (candidate.Roles & InflectionExactRole.Singular) == 0) ||
                (candidate.Roles & GetTargetRole(direction, selectedCategory)) != 0)
            {
                return new(InflectionStatus.Invariant, originalInput);
            }

            string output;
            if (direction == InflectionDirection.Reverse)
            {
                output = this.entries[lexeme.SingularEntryIndex].Value;
            }
            else if (selectedCategory is { } displayCategory)
            {
                if (!lexeme.TryGetDisplay(displayCategory, out var displayIndex))
                {
                    return new(InflectionStatus.Unsupported, originalInput);
                }

                output = this.entries[displayIndex].Value;
            }
            else
            {
                output = this.entries[lexeme.DictionaryPluralEntryIndex].Value;
            }

            return Project(originalInput, output, projection, InflectionStatus.Exact);
        }

        return new(InflectionStatus.Unknown, originalInput);
    }

    InflectionResult Project(
        string input,
        string output,
        CaseProjection projection,
        InflectionStatus status)
    {
        if (output.Length == 0 || !IsWellFormedUtf16(output))
        {
            return new(InflectionStatus.Unsupported, input);
        }

        string projected;
        if (casing == InflectionCasing.LowerTitleUpper &&
            projection == CaseProjection.Title)
        {
            if (!TryProjectUpper(output, firstOnly: true, out projected))
            {
                return new(InflectionStatus.Unsupported, input);
            }
        }
        else if (casing == InflectionCasing.LowerTitleUpper &&
                 projection == CaseProjection.Upper)
        {
            if (!TryProjectUpper(output, firstOnly: false, out projected))
            {
                return new(InflectionStatus.Unsupported, input);
            }
        }
        else
        {
            projected = output;
        }

        if (!IsScriptEligible(projected, allowNonLetters: true))
        {
            return new(InflectionStatus.Unsupported, input);
        }

        return string.Equals(projected, input, StringComparison.Ordinal)
            ? new(InflectionStatus.Invariant, input)
            : new(status, projected);
    }

    static bool TryProjectUpper(
        string value,
        bool firstOnly,
        out string projected)
    {
        char[]? characters = null;
        for (var index = 0; index < value.Length;)
        {
            var scalarOffset = index;
            var scalar = ReadScalar(value, ref index, value.Length);
            var upper = InflectionUnicodeData.ToUpperSimple(scalar);
            if (upper != scalar)
            {
                var scalarLength = index - scalarOffset;
                var upperLength = upper <= char.MaxValue ? 1 : 2;
                if (upperLength != scalarLength)
                {
                    projected = value;
                    return false;
                }

                characters ??= value.ToCharArray();
                if (upperLength == 1)
                {
                    characters[scalarOffset] = (char)upper;
                }
                else
                {
                    var supplementary = upper - 0x10000;
                    characters[scalarOffset] =
                        (char)((supplementary / 0x400) + 0xD800);
                    characters[scalarOffset + 1] =
                        (char)((supplementary % 0x400) + 0xDC00);
                }
            }

            if (firstOnly)
            {
                break;
            }
        }

        projected = characters is null ? value : new(characters);
        return true;
    }

    static CaseProjection GetProjection(string value)
    {
        if (value.Length == 0)
        {
            return CaseProjection.Exact;
        }

        if (IsAscii(value))
        {
            var hasLower = false;
            var hasUpper = false;
            var hasUpperAfterFirst = false;
            for (var index = 0; index < value.Length; index++)
            {
                var character = value[index];
                hasLower |= character is >= 'a' and <= 'z';
                if (character is >= 'A' and <= 'Z')
                {
                    hasUpper = true;
                    hasUpperAfterFirst |= index > 0;
                }
            }

            if (!hasUpper)
            {
                return CaseProjection.Lower;
            }

            if (!hasLower)
            {
                return CaseProjection.Upper;
            }

            return value[0] is >= 'A' and <= 'Z' && !hasUpperAfterFirst
                ? CaseProjection.Title
                : CaseProjection.Mixed;
        }

        var hasUnicodeLower = false;
        var hasUnicodeUpper = false;
        var hasUnicodeUpperAfterFirst = false;
        var firstScalarIsUpper = false;
        for (var index = 0; index < value.Length;)
        {
            var isFirstScalar = index == 0;
            var scalar = ReadScalar(value, ref index, value.Length);
            var unicodeCase = InflectionUnicodeData.GetCase(scalar);
            if (unicodeCase == InflectionUnicodeCase.Title)
            {
                return CaseProjection.Mixed;
            }

            if (unicodeCase == InflectionUnicodeCase.Lower)
            {
                hasUnicodeLower = true;
            }
            else if (unicodeCase == InflectionUnicodeCase.Upper)
            {
                hasUnicodeUpper = true;
                firstScalarIsUpper |= isFirstScalar;
                hasUnicodeUpperAfterFirst |= !isFirstScalar;
            }
        }

        if (!hasUnicodeUpper)
        {
            return CaseProjection.Lower;
        }

        if (!hasUnicodeLower)
        {
            return CaseProjection.Upper;
        }

        return firstScalarIsUpper && !hasUnicodeUpperAfterFirst
            ? CaseProjection.Title
            : CaseProjection.Mixed;
    }

    bool IsEligibleToken(
        string value,
        out InflectionUnicodeScripts detectedScripts)
    {
        var comparison = casing == InflectionCasing.LowerTitleUpper
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;
        if (Contains(skipSimpleWords, value, comparison))
        {
            detectedScripts = InflectionUnicodeScripts.None;
            return false;
        }

        return TryGetDetectedScripts(
            value,
            allowNonLetters: false,
            out detectedScripts);
    }

    bool IsScriptEligible(string value, bool allowNonLetters)
        => TryGetDetectedScripts(value, allowNonLetters, out _);

    bool TryGetDetectedScripts(
        string value,
        bool allowNonLetters,
        out InflectionUnicodeScripts detectedScripts)
    {
        var allowedScripts = InflectionUnicodeScripts.None;
        foreach (var script in scripts)
        {
            if (!InflectionUnicodeData.TryGetScript(script, out var scriptValue))
            {
                detectedScripts = InflectionUnicodeScripts.None;
                return false;
            }

            allowedScripts |= scriptValue;
        }

        detectedScripts = allowedScripts;
        var hasLetter = false;
        for (var index = 0; index < value.Length; index++)
        {
            var scalar = char.ConvertToUtf32(value, index);
            var category = CharUnicodeInfo.GetUnicodeCategory(value, index);
            if (char.IsHighSurrogate(value[index]))
            {
                index++;
            }

            var pinnedLetter = false;
            var pinnedMark = false;
            if (category == UnicodeCategory.OtherNotAssigned)
            {
                _ = InflectionUnicodeData.TryGetPinnedLetterOrMark(
                    scalar,
                    out pinnedLetter,
                    out pinnedMark);
            }

            var isLetter = pinnedLetter ||
                category is UnicodeCategory.UppercaseLetter or
                UnicodeCategory.LowercaseLetter or
                UnicodeCategory.TitlecaseLetter or
                UnicodeCategory.ModifierLetter or
                UnicodeCategory.OtherLetter;
            var isMark = pinnedMark ||
                category is UnicodeCategory.NonSpacingMark or
                UnicodeCategory.SpacingCombiningMark or
                UnicodeCategory.EnclosingMark;
            if (!isLetter && !isMark)
            {
                if (allowNonLetters)
                {
                    continue;
                }

                detectedScripts = InflectionUnicodeScripts.None;
                return false;
            }

            var scalarScripts = InflectionUnicodeData.GetScripts(scalar);
            if ((scalarScripts & allowedScripts) == InflectionUnicodeScripts.None)
            {
                detectedScripts = InflectionUnicodeScripts.None;
                return false;
            }

            detectedScripts &= scalarScripts;
            if (detectedScripts == InflectionUnicodeScripts.None)
            {
                return false;
            }

            hasLetter |= isLetter;
        }

        return hasLetter;
    }

    bool TryCreateReverseCandidate(
        InflectionRule rule,
        string value,
        StringComparison comparison,
        out ReverseCandidate candidate)
    {
        var marker = rule.DictionaryPlural.IndexOf("{stem}", StringComparison.Ordinal);
        if (marker != 0)
        {
            candidate = default;
            return false;
        }

        var replacementLength = rule.DictionaryPlural.Length - "{stem}".Length;
        if (value.Length <= replacementLength ||
            !RegionEquals(
                value,
                value.Length - replacementLength,
                rule.DictionaryPlural,
                "{stem}".Length,
                replacementLength,
                comparison))
        {
            candidate = default;
            return false;
        }

        var stemLength = value.Length - replacementLength;
        candidate = new(value, stemLength, rule.Prefix, rule.Suffix);
        foreach (var excludedSurface in rule.ExcludedSurfaces)
        {
            if (candidate.Equals(excludedSurface, comparison))
            {
                candidate = default;
                return false;
            }
        }

        if (rule.Prefix.Length == 0)
        {
            foreach (var prefix in rule.PrecedingNot)
            {
                if (stemLength >= prefix.Length &&
                    RegionEquals(
                        value,
                        stemLength - prefix.Length,
                        prefix,
                        0,
                        prefix.Length,
                        comparison))
                {
                    candidate = default;
                    return false;
                }
            }
        }

        return IsReverseCandidateAllowed(rule, candidate, comparison);
    }

    bool IsReverseCandidateAllowed(
        InflectionRule rule,
        ReverseCandidate candidate,
        StringComparison comparison)
    {
        if (!rule.RequiresExistingLexeme &&
            rule.ExcludedLexemes.Length == 0 &&
            rule.Countabilities == InflectionCountability.All)
        {
            return true;
        }

        if (TryFindAcceptedSingular(
                candidate,
                comparison,
                out var lexemeIndex))
        {
            return !rule.ExcludedLexemes.Contains(lexemeIndex) &&
                rule.SupportsCountability(lexemes[lexemeIndex].Countability);
        }

        return !rule.RequiresExistingLexeme &&
            rule.ExcludedLexemes.Length == 0 &&
            rule.SupportsCountability(InflectionCountability.None);
    }

    bool TryFindAcceptedSingular(
        string candidate,
        StringComparison comparisonType,
        out int lexemeIndex) =>
        TryFindAcceptedSingular(
            new ReverseCandidate(
                candidate,
                candidate.Length,
                prefix: string.Empty,
                suffix: string.Empty),
            comparisonType,
            out lexemeIndex);

    bool TryFindAcceptedSingular(
        ReverseCandidate candidate,
        StringComparison comparisonType,
        out int lexemeIndex)
    {
        var low = 0;
        var high = forwardEntries.Length - 1;
        while (low <= high)
        {
            var middle = low + ((high - low) / 2);
            var entry = entries[forwardEntries[middle]];
            var comparison = candidate.CompareTo(
                entry.Value,
                comparisonType);
            if (comparison < 0)
            {
                high = middle - 1;
            }
            else if (comparison > 0)
            {
                low = middle + 1;
            }
            else
            {
                lexemeIndex = -1;
                for (var index = 0; index < entry.CandidateCount; index++)
                {
                    var candidateEntry = candidates[entry.CandidateOffset + index];
                    if ((candidateEntry.Roles & InflectionExactRole.Singular) == 0)
                    {
                        continue;
                    }

                    if (lexemeIndex >= 0)
                    {
                        return false;
                    }

                    lexemeIndex = candidateEntry.LexemeIndex;
                }

                return lexemeIndex >= 0;
            }
        }

        lexemeIndex = -1;
        return false;
    }

    static bool TryGetStem(
        InflectionRule rule,
        string value,
        StringComparison comparison,
        [NotNullWhen(true)] out string? stem)
    {
        if (rule.Prefix.Length > 0)
        {
            if (value.Length < rule.Prefix.Length ||
                !RegionEquals(
                    value,
                    0,
                    rule.Prefix,
                    0,
                    rule.Prefix.Length,
                    comparison))
            {
                stem = null;
                return false;
            }

            stem = value.Substring(rule.Prefix.Length);
            return stem.Length > 0;
        }

        if (value.Length <= rule.Suffix.Length ||
            !RegionEquals(
                value,
                value.Length - rule.Suffix.Length,
                rule.Suffix,
                0,
                rule.Suffix.Length,
                comparison))
        {
            stem = null;
            return false;
        }

        var stemLength = value.Length - rule.Suffix.Length;
        foreach (var prefix in rule.PrecedingNot)
        {
            if (stemLength >= prefix.Length &&
                RegionEquals(
                    value,
                    stemLength - prefix.Length,
                    prefix,
                    0,
                    prefix.Length,
                    comparison))
            {
                stem = null;
                return false;
            }
        }

        stem = value.Substring(0, stemLength);
        return true;
    }

    static bool IsWellFormedUtf16(string value)
    {
        for (var index = 0; index < value.Length; index++)
        {
            if (char.IsHighSurrogate(value[index]))
            {
                if (++index >= value.Length || !char.IsLowSurrogate(value[index]))
                {
                    return false;
                }
            }
            else if (char.IsLowSurrogate(value[index]))
            {
                return false;
            }
        }

        return true;
    }

    static bool IsDefinitelyNormalized(string value)
    {
        for (var index = 0; index < value.Length; index++)
        {
            var scalar = char.ConvertToUtf32(value, index);
            var category = CharUnicodeInfo.GetUnicodeCategory(value, index);
            if (category is UnicodeCategory.NonSpacingMark or
                UnicodeCategory.SpacingCombiningMark or
                UnicodeCategory.EnclosingMark ||
                HasUncertainNfcQuickCheck(scalar))
            {
                return false;
            }

            if (char.IsHighSurrogate(value[index]))
            {
                index++;
            }
        }

        return true;
    }

    static bool HasUncertainNfcQuickCheck(int scalar)
    {
        var low = 0;
        var high = UncertainNfcQuickCheckRanges.Length - 1;
        while (low <= high)
        {
            var middle = low + ((high - low) / 2);
            var range = UncertainNfcQuickCheckRanges[middle];
            if (scalar < range.First)
            {
                high = middle - 1;
            }
            else if (scalar > range.Last)
            {
                low = middle + 1;
            }
            else
            {
                return true;
            }
        }

        return false;
    }

    readonly struct ReverseCandidate(
        string source,
        int stemLength,
        string prefix,
        string suffix)
    {
        public int Length => prefix.Length + stemLength + suffix.Length;

        public bool Equals(
            ReverseCandidate other,
            StringComparison comparison)
        {
            if (Length != other.Length)
            {
                return false;
            }

            if (comparison == StringComparison.Ordinal)
            {
                for (var index = 0; index < Length; index++)
                {
                    if (this[index] != other[index])
                    {
                        return false;
                    }
                }

                return true;
            }

            var leftIndex = 0;
            var rightIndex = 0;
            while (leftIndex < Length && rightIndex < other.Length)
            {
                if (FoldSimpleCase(ReadScalar(ref leftIndex)) !=
                    FoldSimpleCase(other.ReadScalar(ref rightIndex)))
                {
                    return false;
                }
            }

            return leftIndex == Length && rightIndex == other.Length;
        }

        public bool Equals(
            string other,
            StringComparison comparison)
        {
            if (Length != other.Length)
            {
                return false;
            }

            if (comparison == StringComparison.Ordinal)
            {
                for (var index = 0; index < Length; index++)
                {
                    if (this[index] != other[index])
                    {
                        return false;
                    }
                }

                return true;
            }

            var leftIndex = 0;
            var rightIndex = 0;
            while (leftIndex < Length && rightIndex < other.Length)
            {
                if (FoldSimpleCase(ReadScalar(ref leftIndex)) !=
                    FoldSimpleCase(
                        InflectionBundle.ReadScalar(
                            other,
                            ref rightIndex,
                            other.Length)))
                {
                    return false;
                }
            }

            return leftIndex == Length && rightIndex == other.Length;
        }

        public int CompareTo(
            string other,
            StringComparison comparisonType)
        {
            if (comparisonType == StringComparison.Ordinal)
            {
                var commonLength = Math.Min(Length, other.Length);
                for (var index = 0; index < commonLength; index++)
                {
                    var comparison = this[index].CompareTo(other[index]);
                    if (comparison != 0)
                    {
                        return comparison;
                    }
                }

                return Length.CompareTo(other.Length);
            }

            var leftIndex = 0;
            var rightIndex = 0;
            while (leftIndex < Length && rightIndex < other.Length)
            {
                var leftScalar = FoldSimpleCase(ReadScalar(ref leftIndex));
                var rightScalar = FoldSimpleCase(
                    InflectionBundle.ReadScalar(
                        other,
                        ref rightIndex,
                        other.Length));
                if (leftScalar != rightScalar)
                {
                    return leftScalar.CompareTo(rightScalar);
                }
            }

            return leftIndex < Length
                ? 1
                : rightIndex < other.Length
                    ? -1
                    : 0;
        }

        public string Materialize()
        {
            var stem = source.Substring(0, stemLength);
            return prefix.Length > 0
                ? string.Concat(prefix, stem)
                : string.Concat(stem, suffix);
        }

        char this[int index] =>
            index < prefix.Length
                ? prefix[index]
                : index < prefix.Length + stemLength
                    ? source[index - prefix.Length]
                    : suffix[index - prefix.Length - stemLength];

        int ReadScalar(ref int index)
        {
            var first = this[index++];
            if (char.IsHighSurrogate(first) &&
                index < Length &&
                char.IsLowSurrogate(this[index]))
            {
                return char.ConvertToUtf32(first, this[index++]);
            }

            return first;
        }
    }

    static bool Contains(
        string[] values,
        string value,
        StringComparison comparison)
    {
        foreach (var candidate in values)
        {
            if (CompareStrings(candidate, value, comparison) == 0)
            {
                return true;
            }
        }

        return false;
    }

    static bool RegionEquals(
        string left,
        int leftOffset,
        string right,
        int rightOffset,
        int length,
        StringComparison comparison)
    {
        if (comparison == StringComparison.Ordinal)
        {
            for (var index = 0; index < length; index++)
            {
                if (left[leftOffset + index] != right[rightOffset + index])
                {
                    return false;
                }
            }

            return true;
        }

        var leftEnd = leftOffset + length;
        var rightEnd = rightOffset + length;
        while (leftOffset < leftEnd && rightOffset < rightEnd)
        {
            if (FoldSimpleCase(ReadScalar(left, ref leftOffset, leftEnd)) !=
                FoldSimpleCase(ReadScalar(right, ref rightOffset, rightEnd)))
            {
                return false;
            }
        }

        return leftOffset == leftEnd && rightOffset == rightEnd;
    }

    static int CompareStrings(
        string left,
        string right,
        StringComparison comparison) =>
        comparison == StringComparison.Ordinal
            ? string.CompareOrdinal(left, right)
            : InflectionUnicodeData.CompareSimpleCase(left, right);

    internal static int CompareInflectionKeys(string left, string right) =>
        InflectionUnicodeData.CompareSimpleCase(left, right);

    internal static int FoldInflectionScalar(int scalar) =>
        FoldSimpleCase(scalar);

    static int ReadScalar(
        string value,
        ref int index,
        int end)
    {
        var first = value[index++];
        if (char.IsHighSurrogate(first) &&
            index < end &&
            char.IsLowSurrogate(value[index]))
        {
            return char.ConvertToUtf32(first, value[index++]);
        }

        return first;
    }

    static int CompareRuleRank(InflectionRule candidate, InflectionRule current)
    {
        if (candidate.Priority != current.Priority)
        {
            return candidate.Priority.CompareTo(current.Priority);
        }

        return GetAffixLength(candidate).CompareTo(GetAffixLength(current));
    }

    static int GetAffixLength(InflectionRule rule) =>
        rule.Prefix.Length > 0
            ? rule.Prefix.Length
            : rule.Suffix.Length;

    static bool IsAscii(string value)
    {
        foreach (var character in value)
        {
            if (character > 0x7F)
            {
                return false;
            }
        }

        return true;
    }

    // <generated-unicode-nfc-quick-check>
    // Unicode 16.0.0 NFC_QC=No/Maybe ranges from DerivedNormalizationProps.txt
    // (SHA-256 4d4c03892dea9146d674b686e495df2d55a28d071ac474041d73518f887abddc).
    static readonly InflectionUnicodeRange[] UncertainNfcQuickCheckRanges =
    [
        new(0x0300, 0x0304),
        new(0x0306, 0x030C),
        new(0x030F, 0x030F),
        new(0x0311, 0x0311),
        new(0x0313, 0x0314),
        new(0x031B, 0x031B),
        new(0x0323, 0x0328),
        new(0x032D, 0x032E),
        new(0x0330, 0x0331),
        new(0x0338, 0x0338),
        new(0x0340, 0x0345),
        new(0x0374, 0x0374),
        new(0x037E, 0x037E),
        new(0x0387, 0x0387),
        new(0x0653, 0x0655),
        new(0x093C, 0x093C),
        new(0x0958, 0x095F),
        new(0x09BE, 0x09BE),
        new(0x09D7, 0x09D7),
        new(0x09DC, 0x09DD),
        new(0x09DF, 0x09DF),
        new(0x0A33, 0x0A33),
        new(0x0A36, 0x0A36),
        new(0x0A59, 0x0A5B),
        new(0x0A5E, 0x0A5E),
        new(0x0B3E, 0x0B3E),
        new(0x0B56, 0x0B57),
        new(0x0B5C, 0x0B5D),
        new(0x0BBE, 0x0BBE),
        new(0x0BD7, 0x0BD7),
        new(0x0C56, 0x0C56),
        new(0x0CC2, 0x0CC2),
        new(0x0CD5, 0x0CD6),
        new(0x0D3E, 0x0D3E),
        new(0x0D57, 0x0D57),
        new(0x0DCA, 0x0DCA),
        new(0x0DCF, 0x0DCF),
        new(0x0DDF, 0x0DDF),
        new(0x0F43, 0x0F43),
        new(0x0F4D, 0x0F4D),
        new(0x0F52, 0x0F52),
        new(0x0F57, 0x0F57),
        new(0x0F5C, 0x0F5C),
        new(0x0F69, 0x0F69),
        new(0x0F73, 0x0F73),
        new(0x0F75, 0x0F76),
        new(0x0F78, 0x0F78),
        new(0x0F81, 0x0F81),
        new(0x0F93, 0x0F93),
        new(0x0F9D, 0x0F9D),
        new(0x0FA2, 0x0FA2),
        new(0x0FA7, 0x0FA7),
        new(0x0FAC, 0x0FAC),
        new(0x0FB9, 0x0FB9),
        new(0x102E, 0x102E),
        new(0x1161, 0x1175),
        new(0x11A8, 0x11C2),
        new(0x1B35, 0x1B35),
        new(0x1F71, 0x1F71),
        new(0x1F73, 0x1F73),
        new(0x1F75, 0x1F75),
        new(0x1F77, 0x1F77),
        new(0x1F79, 0x1F79),
        new(0x1F7B, 0x1F7B),
        new(0x1F7D, 0x1F7D),
        new(0x1FBB, 0x1FBB),
        new(0x1FBE, 0x1FBE),
        new(0x1FC9, 0x1FC9),
        new(0x1FCB, 0x1FCB),
        new(0x1FD3, 0x1FD3),
        new(0x1FDB, 0x1FDB),
        new(0x1FE3, 0x1FE3),
        new(0x1FEB, 0x1FEB),
        new(0x1FEE, 0x1FEF),
        new(0x1FF9, 0x1FF9),
        new(0x1FFB, 0x1FFB),
        new(0x1FFD, 0x1FFD),
        new(0x2000, 0x2001),
        new(0x2126, 0x2126),
        new(0x212A, 0x212B),
        new(0x2329, 0x232A),
        new(0x2ADC, 0x2ADC),
        new(0x3099, 0x309A),
        new(0xF900, 0xFA0D),
        new(0xFA10, 0xFA10),
        new(0xFA12, 0xFA12),
        new(0xFA15, 0xFA1E),
        new(0xFA20, 0xFA20),
        new(0xFA22, 0xFA22),
        new(0xFA25, 0xFA26),
        new(0xFA2A, 0xFA6D),
        new(0xFA70, 0xFAD9),
        new(0xFB1D, 0xFB1D),
        new(0xFB1F, 0xFB1F),
        new(0xFB2A, 0xFB36),
        new(0xFB38, 0xFB3C),
        new(0xFB3E, 0xFB3E),
        new(0xFB40, 0xFB41),
        new(0xFB43, 0xFB44),
        new(0xFB46, 0xFB4E),
        new(0x110BA, 0x110BA),
        new(0x11127, 0x11127),
        new(0x1133E, 0x1133E),
        new(0x11357, 0x11357),
        new(0x113B8, 0x113B8),
        new(0x113BB, 0x113BB),
        new(0x113C2, 0x113C2),
        new(0x113C5, 0x113C5),
        new(0x113C7, 0x113C9),
        new(0x114B0, 0x114B0),
        new(0x114BA, 0x114BA),
        new(0x114BD, 0x114BD),
        new(0x115AF, 0x115AF),
        new(0x11930, 0x11930),
        new(0x1611E, 0x16129),
        new(0x16D67, 0x16D68),
        new(0x1D15E, 0x1D164),
        new(0x1D1BB, 0x1D1C0),
        new(0x2F800, 0x2FA1D),
    ];
    // </generated-unicode-nfc-quick-check>

    static int FoldSimpleCase(int scalar) =>
        InflectionUnicodeData.FoldSimpleCase(scalar);

    static InflectionExactRole GetTargetRole(
        InflectionDirection direction,
        CardinalPluralCategory? category) =>
        direction == InflectionDirection.Reverse
            ? InflectionExactRole.Singular
            : category is null
                ? InflectionExactRole.DictionaryPlural
                : GetDisplayRole(category.Value);

    static InflectionExactRole GetDisplayRole(CardinalPluralCategory category) =>
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

    readonly struct InflectionUnicodeRange(int first, int last)
    {
        public int First { get; } = first;
        public int Last { get; } = last;
    }

    enum CaseProjection
    {
        Exact,
        Lower,
        Title,
        Upper,
        Mixed
    }
}
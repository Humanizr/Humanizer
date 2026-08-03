namespace Humanizer;

sealed partial class InflectionBundle
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
        var selectedRuleCoversDetectedScripts = false;
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
                    selectedRuleCoversDetectedScripts = CoversDetectedScripts(
                        rule.Scripts,
                        detectedScripts);
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

                    selectedRuleCoversDetectedScripts |= CoversDetectedScripts(
                        rule.Scripts,
                        detectedScripts);
                }
            }
        }

        if (selectedRule is null)
        {
            return new(InflectionStatus.Unknown, input);
        }

        if (!selectedRuleCoversDetectedScripts)
        {
            return new(InflectionStatus.Ambiguous, input);
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
        var selectedDirectRuleCoversDetectedScripts = false;
        var reverseCandidate = default(ReverseCandidate);
        var reverseCandidateCoversDetectedScripts = false;
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
                        ref selectedDirectCandidate,
                        ref selectedDirectRuleCoversDetectedScripts,
                        detectedScripts))
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
                reverseCandidateCoversDetectedScripts |= CoversDetectedScripts(
                    rule.Scripts,
                    detectedScripts);
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

            if (hasReverseCandidate)
            {
                selectedDirectRuleCoversDetectedScripts |=
                    reverseCandidateCoversDetectedScripts;
            }

            if (!selectedDirectRuleCoversDetectedScripts)
            {
                return new(InflectionStatus.Ambiguous, input);
            }

            return Project(
                input,
                selectedDirectCandidate.Materialize(),
                projection,
                InflectionStatus.Productive);
        }

        if (!hasReverseCandidate)
        {
            return new(InflectionStatus.Unknown, input);
        }

        return reverseCandidateCoversDetectedScripts
            ? Project(
                input,
                reverseCandidate.Materialize(),
                projection,
                InflectionStatus.Productive)
            : new(InflectionStatus.Ambiguous, input);
    }

    static bool TrySelectDirectReverseCandidate(
        InflectionRule rule,
        ReverseCandidate candidate,
        StringComparison comparison,
        ref InflectionRule? selectedRule,
        ref ReverseCandidate selectedCandidate,
        ref bool selectedRuleCoversDetectedScripts,
        InflectionUnicodeScripts detectedScripts)
    {
        if (selectedRule is null)
        {
            selectedRule = rule;
            selectedCandidate = candidate;
            selectedRuleCoversDetectedScripts = CoversDetectedScripts(
                rule.Scripts,
                detectedScripts);
            return true;
        }

        var rank = CompareRuleRank(rule, selectedRule.Value);
        if (rank > 0)
        {
            selectedRule = rule;
            selectedCandidate = candidate;
            selectedRuleCoversDetectedScripts = CoversDetectedScripts(
                rule.Scripts,
                detectedScripts);
            return true;
        }

        if (rank != 0)
        {
            return true;
        }

        if (!selectedCandidate.Equals(candidate, comparison))
        {
            return false;
        }

        selectedRuleCoversDetectedScripts |= CoversDetectedScripts(
            rule.Scripts,
            detectedScripts);
        return true;
    }

    static bool CoversDetectedScripts(
        InflectionUnicodeScripts candidateScripts,
        InflectionUnicodeScripts detectedScripts) =>
        (candidateScripts & detectedScripts) == detectedScripts;

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
}
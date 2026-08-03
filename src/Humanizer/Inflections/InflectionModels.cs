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
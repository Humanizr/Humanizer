namespace Humanizer;

/// <summary>
/// Shared renderer for English-derived scale systems where the algorithm is mostly:
/// decompose by descending large scales, render each sub-thousand chunk, then decide whether
/// conjunctions and ordinals apply at the terminal segment.
///
/// The important family rule is that locale variability is lexical and strategic rather than
/// structural. The generated profile supplies:
/// - the unit and tens vocabularies
/// - the scale words and scale ordinals
/// - whether <c>and</c> appears inside hundreds, after scale boundaries, or both
/// - whether ordinals use dedicated lexical tables or intentionally reuse cardinals
///
/// The expected output is a natural-language cardinal or ordinal phrase such as
/// "one hundred and twenty-three", "one thousand first", or "hundredth", depending on the
/// generated strategy values.
/// </summary>
class ConjunctionalScaleNumberToWordsConverter(ConjunctionalScaleNumberToWordsProfile profile) : GenderlessNumberToWordsConverter
{
    readonly ConjunctionalScaleNumberToWordsProfile profile = profile;

    /// <inheritdoc />
    public override string Convert(long number) =>
        Convert(number, profile.DefaultAddAnd);

    /// <inheritdoc />
    public override string Convert(long number, bool addAnd) =>
        ConvertCore(
            number,
            isOrdinal: false,
            profile.AddAndMode == ConjunctionalScaleAddAndMode.UseCallerFlag ? addAnd : profile.DefaultAddAnd);

    /// <inheritdoc />
    public override string ConvertToOrdinal(int number) =>
        profile.OrdinalMode == ConjunctionalScaleOrdinalMode.Cardinal
            ? Convert(number, profile.DefaultAddAnd)
            : ConvertCore(number, isOrdinal: true, profile.DefaultAddAnd);

    /// <inheritdoc />
    public override string ConvertToTuple(int number) =>
        profile.NamedTuples is not null && profile.NamedTuples.TryGetValue(number, out var namedTuple)
            ? namedTuple
            : profile.TupleSuffix.Length != 0
                ? $"{number}{profile.TupleSuffix}"
                : base.ConvertToTuple(number);

    // Large-scale traversal is intentionally recursive so every count above one thousand is still
    // rendered by the same family rules. That keeps the algorithm stable while YAML controls only
    // the words and strategy switches.
    string ConvertCore(long number, bool isOrdinal, bool addAnd)
    {
        if (number == 0)
        {
            return GetUnitValue(0, isOrdinal);
        }

        if (number < 0)
        {
            return $"{profile.MinusWord} {Convert(-number, addAnd)}";
        }

        var parts = new List<string>(20);
        var remainder = number;

        foreach (var scale in profile.Scales)
        {
            var count = remainder / scale.Value;
            if (count == 0)
            {
                continue;
            }

            // Most conjunctional-scale locales peel groups into sub-thousand chunks, so avoid
            // recursing through another full scale walk when the group count is already small.
            if (count < 1000)
            {
                AppendUnderThousand(parts, count, isOrdinal: false, addAnd, isTerminalScaleRemainder: false);
            }
            else
            {
                parts.Add(ConvertCore(count, isOrdinal: false, addAnd));
            }
            parts.Add(remainder % scale.Value == 0 && isOrdinal ? scale.OrdinalName : scale.Name);
            remainder %= scale.Value;
        }

        AppendUnderThousand(parts, remainder, isOrdinal, addAnd, isTerminalScaleRemainder: true);

        ApplyOrdinalLeadingOneStrategy(parts, isOrdinal);

        return string.Join(" ", parts);
    }

    // Under one thousand is the real "shape" of this family:
    // 1. emit the hundreds phrase if present
    // 2. decide whether a conjunction belongs before the remainder
    // 3. render tens and units, optionally switching to an ordinal form only for the terminal part
    //
    // The result is a language phrase that still reads naturally even when the larger number came
    // through the recursive scale walk above.
    void AppendUnderThousand(List<string> parts, long number, bool isOrdinal, bool addAnd, bool isTerminalScaleRemainder)
    {
        var hasHundreds = false;
        if (number >= 100)
        {
            hasHundreds = true;
            parts.Add(profile.UnitsMap[number / 100]);
            number %= 100;
            parts.Add(number == 0 && isOrdinal ? profile.HundredOrdinalWord : profile.HundredWord);
        }

        if (number == 0)
        {
            return;
        }

        if (ShouldAddAnd(addAnd, hasHundreds, parts.Count > 0, isTerminalScaleRemainder))
        {
            parts.Add(profile.AndWord);
        }

        if (number >= 20)
        {
            var tens = profile.TensMap[number / 10];
            var units = number % 10;

            if (units == 0)
            {
                parts.Add(isOrdinal ? profile.OrdinalTensMap[number / 10] : tens);
                return;
            }

            parts.Add(string.Concat(
                tens,
                profile.TensUnitsSeparator,
                GetUnitValue(units, isOrdinal)));
            return;
        }

        parts.Add(GetUnitValue(number, isOrdinal));
    }

    string GetUnitValue(long number, bool isOrdinal) =>
        isOrdinal ? profile.OrdinalUnitsMap[number] : profile.UnitsMap[number];

    // "and" placement is modeled as a structural rule family rather than as locale branches.
    // The generated profile says which insertion points are legal, and this method translates that
    // choice into the current local context: inside a hundred group, after a higher scale, both, or neither.
    bool ShouldAddAnd(bool addAnd, bool hasHundreds, bool hasScalePrefix, bool isTerminalScaleRemainder) =>
        addAnd && profile.AndStrategy switch
        {
            ConjunctionalScaleAndStrategy.WithinGroupAndAfterScaleSubHundredRemainder => hasHundreds || hasScalePrefix,
            ConjunctionalScaleAndStrategy.WithinGroupOnly => hasHundreds,
            ConjunctionalScaleAndStrategy.AfterScaleSubHundredRemainderOnly => !hasHundreds && hasScalePrefix,
            ConjunctionalScaleAndStrategy.WithinGroupAndTerminalScaleSubHundredRemainder => hasHundreds || (!hasHundreds && hasScalePrefix && isTerminalScaleRemainder),
            ConjunctionalScaleAndStrategy.Never => false,
            _ => throw new InvalidOperationException("Unsupported conjunctional-scale strategy.")
        };

    void ApplyOrdinalLeadingOneStrategy(List<string> parts, bool isOrdinal)
    {
        if (!isOrdinal || profile.OrdinalLeadingOneStrategy != ConjunctionalScaleOrdinalLeadingOneStrategy.OmitLeadingOne || parts.Count == 0)
        {
            return;
        }

        if (parts[0] == profile.UnitsMap[1])
        {
            parts.RemoveAt(0);
            return;
        }

        if (parts[0].StartsWith(profile.UnitsMap[1] + " ", StringComparison.Ordinal))
        {
            parts[0] = parts[0][profile.UnitsMap[1].Length..].TrimStart();
        }
    }
}

/// <summary>
/// Immutable generated profile for <see cref="ConjunctionalScaleNumberToWordsConverter"/>.
/// It captures the locale-owned lexicon plus the conjunction and ordinal strategy switches that
/// shape the final phrase.
/// </summary>
/// <param name="minusWord">The word used to prefix negative values.</param>
/// <param name="andWord">The conjunction token inserted between hundreds and sub-hundred remainders.</param>
/// <param name="hundredWord">The cardinal hundred word.</param>
/// <param name="hundredOrdinalWord">The ordinal hundred word used for exact hundreds.</param>
/// <param name="tensUnitsSeparator">The separator inserted between tens and units.</param>
/// <param name="defaultAddAnd">A value indicating whether callers default to conjunction insertion.</param>
/// <param name="addAndMode">The policy that decides whether callers may override <paramref name="defaultAddAnd"/>.</param>
/// <param name="andStrategy">The structural rule used to place conjunctions within groups and after scales.</param>
/// <param name="tupleSuffix">The suffix used when tuples fall back to a numeric affix form.</param>
/// <param name="ordinalLeadingOneStrategy">The policy that decides whether leading one survives in ordinal scale phrases.</param>
/// <param name="ordinalMode">The ordinal rendering mode used by the shared engine.</param>
/// <param name="unitsMap">The cardinal unit lexicon keyed by value.</param>
/// <param name="ordinalUnitsMap">The ordinal unit lexicon keyed by value.</param>
/// <param name="tensMap">The cardinal tens lexicon keyed by decade value.</param>
/// <param name="ordinalTensMap">The ordinal tens lexicon keyed by decade value.</param>
/// <param name="scales">The descending scale rows used during recursive decomposition.</param>
/// <param name="namedTuples">The optional exact tuple names that bypass the numeric suffix fallback.</param>
sealed class ConjunctionalScaleNumberToWordsProfile(
    string minusWord,
    string andWord,
    string hundredWord,
    string hundredOrdinalWord,
    string tensUnitsSeparator,
    bool defaultAddAnd,
    ConjunctionalScaleAddAndMode addAndMode,
    ConjunctionalScaleAndStrategy andStrategy,
    string tupleSuffix,
    ConjunctionalScaleOrdinalLeadingOneStrategy ordinalLeadingOneStrategy,
    ConjunctionalScaleOrdinalMode ordinalMode,
    string[] unitsMap,
    string[] ordinalUnitsMap,
    string[] tensMap,
    string[] ordinalTensMap,
    ConjunctionalScale[] scales,
    FrozenDictionary<int, string>? namedTuples = null)
{
    /// <summary>
    /// Gets the word used to prefix negative values.
    /// </summary>
    public string MinusWord { get; } = minusWord;

    /// <summary>
    /// Gets the conjunction token inserted according to <see cref="AndStrategy"/>.
    /// </summary>
    public string AndWord { get; } = andWord;

    /// <summary>
    /// Gets the cardinal hundred word used for exact and composite hundreds.
    /// </summary>
    public string HundredWord { get; } = hundredWord;

    /// <summary>
    /// Gets the ordinal hundred word used when an exact hundred terminates the phrase.
    /// </summary>
    public string HundredOrdinalWord { get; } = hundredOrdinalWord;

    /// <summary>
    /// Gets the separator used when joining tens and units, for example a hyphen or a space.
    /// </summary>
    public string TensUnitsSeparator { get; } = tensUnitsSeparator;

    /// <summary>
    /// Gets the default add-and behavior for callers that do not override it.
    /// </summary>
    public bool DefaultAddAnd { get; } = defaultAddAnd;

    /// <summary>
    /// Gets the policy that decides whether callers may override the default add-and behavior.
    /// </summary>
    public ConjunctionalScaleAddAndMode AddAndMode { get; } = addAndMode;

    /// <summary>
    /// Gets the conjunction placement strategy for hundreds and scale remainders.
    /// </summary>
    public ConjunctionalScaleAndStrategy AndStrategy { get; } = andStrategy;

    /// <summary>
    /// Gets the suffix used when the locale expresses tuples as a numeric affix.
    /// </summary>
    public string TupleSuffix { get; } = tupleSuffix;

    /// <summary>
    /// Gets the policy that decides whether leading "one" survives in ordinal scale phrases.
    /// </summary>
    public ConjunctionalScaleOrdinalLeadingOneStrategy OrdinalLeadingOneStrategy { get; } = ordinalLeadingOneStrategy;

    /// <summary>
    /// Gets the ordinal rendering mode used by the shared engine.
    /// </summary>
    public ConjunctionalScaleOrdinalMode OrdinalMode { get; } = ordinalMode;

    /// <summary>
    /// Gets the cardinal unit lexicon.
    /// </summary>
    public string[] UnitsMap { get; } = unitsMap;

    /// <summary>
    /// Gets the ordinal unit lexicon.
    /// </summary>
    public string[] OrdinalUnitsMap { get; } = ordinalUnitsMap;

    /// <summary>
    /// Gets the cardinal tens lexicon.
    /// </summary>
    public string[] TensMap { get; } = tensMap;

    /// <summary>
    /// Gets the ordinal tens lexicon.
    /// </summary>
    public string[] OrdinalTensMap { get; } = ordinalTensMap;

    /// <summary>
    /// Gets the descending scale records used during recursive decomposition.
    /// </summary>
    public ConjunctionalScale[] Scales { get; } = scales;

    /// <summary>
    /// Gets any exact tuple names that bypass the normal numeric-suffix fallback.
    /// </summary>
    public FrozenDictionary<int, string>? NamedTuples { get; } = namedTuples;
}

/// <summary>
/// Controls whether callers may override the family default "and" behavior.
/// </summary>
enum ConjunctionalScaleAddAndMode
{
    UseCallerFlag,
    AlwaysDefault
}

/// <summary>
/// Determines where the family injects conjunctions for sub-hundred remainders.
/// </summary>
enum ConjunctionalScaleAndStrategy
{
    WithinGroupAndAfterScaleSubHundredRemainder,
    WithinGroupOnly,
    AfterScaleSubHundredRemainderOnly,
    WithinGroupAndTerminalScaleSubHundredRemainder,
    Never
}

/// <summary>
/// Describes whether leading "one" survives in ordinal scale phrases such as "one thousandth".
/// </summary>
enum ConjunctionalScaleOrdinalLeadingOneStrategy
{
    KeepLeadingOne,
    OmitLeadingOne
}

/// <summary>
/// Describes whether the engine emits true ordinals or intentionally reuses cardinal wording.
/// </summary>
enum ConjunctionalScaleOrdinalMode
{
    English,
    Cardinal
}

/// <summary>
/// One descending scale row for <see cref="ConjunctionalScaleNumberToWordsConverter"/>.
/// </summary>
/// <param name="Value">The numeric value represented by the scale.</param>
/// <param name="Name">The cardinal scale word.</param>
/// <param name="OrdinalName">The ordinal scale word used when the scale terminates the phrase.</param>
readonly record struct ConjunctionalScale(long Value, string Name, string OrdinalName);

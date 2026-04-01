namespace Humanizer;

/// <summary>
/// Renders locales that split numbers into semantically distinct segments with per-segment
/// grammatical variants.
/// </summary>
class SegmentedScaleNumberToWordsConverter(SegmentedScaleNumberToWordsProfile profile) : GenderlessNumberToWordsConverter
{
    readonly SegmentedScaleNumberToWordsProfile profile = profile;

    /// <inheritdoc/>
    public override string Convert(long number)
    {
        if ((ulong)profile.MaximumValue < GetAbsoluteValue(number))
        {
            return string.Empty;
        }

        if (number == 0)
        {
            return profile.ZeroWord;
        }

        var parts = new List<string>(2);
        var remaining = GetAbsoluteValue(number);
        if (number < 0)
        {
            parts.Add(profile.MinusWord);
        }

        parts.Add(ConvertCore(remaining, SegmentedScaleVariant.Default));
        return string.Join(" ", parts);
    }

    /// <inheritdoc/>
    public override string ConvertToOrdinal(int number)
    {
        if (number <= 0 || number > profile.MaximumOrdinal)
        {
            return string.Empty;
        }

        if (profile.OrdinalMap.TryGetValue(number, out var exactOrdinal))
        {
            return exactOrdinal;
        }

        var parts = new List<string>(4);
        var remaining = number;
        foreach (var place in OrdinalDecompositionPlaces)
        {
            if (profile.OrdinalMap.TryGetValue(remaining, out var exactRemainderOrdinal))
            {
                parts.Add(exactRemainderOrdinal);
                return string.Join(" ", parts);
            }

            var component = remaining / place * place;
            if (component == 0)
            {
                continue;
            }

            if (!profile.OrdinalMap.TryGetValue(component, out var ordinalSegment))
            {
                return string.Empty;
            }

            parts.Add(ordinalSegment);
            remaining %= place;
        }

        if (remaining > 0)
        {
            if (!profile.OrdinalMap.TryGetValue(remaining, out var terminalOrdinal))
            {
                return string.Empty;
            }

            parts.Add(terminalOrdinal);
        }

        return string.Join(" ", parts);
    }

    /// <summary>
    /// Decomposes the ordinal phrase from the largest explicit place to the smallest.
    /// </summary>
    static ReadOnlySpan<int> OrdinalDecompositionPlaces => [1000, 100, 10];

    /// <summary>
    /// Converts the current magnitude using the requested segmented variant.
    /// </summary>
    string ConvertCore(ulong number, SegmentedScaleVariant variant)
    {
        if (number < 13)
        {
            return (variant == SegmentedScaleVariant.Pluralized
                ? profile.UnitsPluralized
                : profile.UnitsDefault)[(int)number];
        }

        if (number < 100)
        {
            return ConvertUnderOneHundred((int)number, variant);
        }

        if (number < 1000)
        {
            return ConvertUnderOneThousand((int)number, variant);
        }

        foreach (var scale in profile.Scales)
        {
            var scaleValue = (ulong)scale.Value;
            if (number < scaleValue)
            {
                continue;
            }

            var count = number / scaleValue;
            var remainder = number % scaleValue;
            var scaleText = count == 1
                ? scale.Singular
                : ConvertCore(count, scale.CountVariant) + " " + scale.Plural;

            if (remainder == 0)
            {
                return scaleText;
            }

            var remainderVariant = count == 1
                ? scale.SingularRemainderVariant
                : scale.PluralRemainderVariant;
            return scaleText + " " + ConvertCore(remainder, remainderVariant);
        }

        return string.Empty;
    }

    /// <summary>
    /// Converts the segment below one hundred.
    /// </summary>
    string ConvertUnderOneHundred(int number, SegmentedScaleVariant variant)
    {
        var tens = number / 10;
        var remainder = number % 10;
        var tensWord = tens == 1 ? profile.TeenPrefix : profile.TensMap[tens];
        if (remainder == 0)
        {
            return tensWord;
        }

        var remainderText = ConvertCore((ulong)remainder, variant);
        return tens == 1
            ? tensWord + remainderText
            : tensWord + " " + remainderText;
    }

    /// <summary>
    /// Converts the segment below one thousand.
    /// </summary>
    string ConvertUnderOneThousand(int number, SegmentedScaleVariant variant)
    {
        var hundreds = number / 100;
        var remainder = number % 100;
        if (hundreds == 1 && remainder == 0)
        {
            return profile.ExactOneHundredWord;
        }

        var hundredsMap = variant == SegmentedScaleVariant.Pluralized
            ? profile.HundredsPluralized
            : profile.HundredsDefault;
        var hundredsWord = hundredsMap[hundreds];
        if (remainder == 0)
        {
            return hundredsWord;
        }

        return hundredsWord + " " + ConvertCore((ulong)remainder, variant);
    }

    /// <summary>
    /// Returns the absolute value while safely handling <see cref="long.MinValue"/>.
    /// </summary>
    static ulong GetAbsoluteValue(long value) =>
        value >= 0 ? (ulong)value : unchecked((ulong)(-(value + 1)) + 1);
}

/// <summary>
/// Selects the grammatical variant used for a segmented fragment.
/// </summary>
enum SegmentedScaleVariant
{
    /// <summary>Uses the default segment lexicon.</summary>
    Default,
    /// <summary>Uses the pluralized segment lexicon.</summary>
    Pluralized
}

/// <summary>
/// Immutable generated profile for <see cref="SegmentedScaleNumberToWordsConverter"/>.
/// </summary>
/// <param name="maximumValue">The maximum supported absolute value.</param>
/// <param name="zeroWord">The word used for zero.</param>
/// <param name="minusWord">The word used to prefix negative values.</param>
/// <param name="teenPrefix">The prefix used for teen values in this family.</param>
/// <param name="exactOneHundredWord">The exact word for one hundred.</param>
/// <param name="unitsDefault">The default unit lexicon.</param>
/// <param name="unitsPluralized">The pluralized unit lexicon used by some segments.</param>
/// <param name="tensMap">The tens lexicon.</param>
/// <param name="hundredsDefault">The default hundred lexicon.</param>
/// <param name="hundredsPluralized">The pluralized hundred lexicon.</param>
/// <param name="scales">The descending scale rows used during decomposition.</param>
/// <param name="maximumOrdinal">The largest ordinal value supported by the profile.</param>
/// <param name="ordinalMap">Exact ordinal overrides keyed by value.</param>
sealed class SegmentedScaleNumberToWordsProfile(
    long maximumValue,
    string zeroWord,
    string minusWord,
    string teenPrefix,
    string exactOneHundredWord,
    string[] unitsDefault,
    string[] unitsPluralized,
    string[] tensMap,
    string[] hundredsDefault,
    string[] hundredsPluralized,
    SegmentedScale[] scales,
    int maximumOrdinal,
    FrozenDictionary<int, string> ordinalMap)
{
    /// <summary>Gets the maximum supported absolute value.</summary>
    public long MaximumValue { get; } = maximumValue;
    /// <summary>Gets the word used for zero.</summary>
    public string ZeroWord { get; } = zeroWord;
    /// <summary>Gets the word used to prefix negative values.</summary>
    public string MinusWord { get; } = minusWord;
    /// <summary>Gets the prefix used to build teen values in this family.</summary>
    public string TeenPrefix { get; } = teenPrefix;
    /// <summary>Gets the exact word for one hundred.</summary>
    public string ExactOneHundredWord { get; } = exactOneHundredWord;
    /// <summary>Gets the default unit lexicon.</summary>
    public string[] UnitsDefault { get; } = unitsDefault;
    /// <summary>Gets the pluralized unit lexicon used by certain segments.</summary>
    public string[] UnitsPluralized { get; } = unitsPluralized;
    /// <summary>Gets the tens lexicon.</summary>
    public string[] TensMap { get; } = tensMap;
    /// <summary>Gets the default hundred lexicon.</summary>
    public string[] HundredsDefault { get; } = hundredsDefault;
    /// <summary>Gets the pluralized hundred lexicon.</summary>
    public string[] HundredsPluralized { get; } = hundredsPluralized;
    /// <summary>Gets the descending scale rows used during decomposition.</summary>
    public SegmentedScale[] Scales { get; } = scales;
    /// <summary>Gets the largest ordinal value supported by the profile.</summary>
    public int MaximumOrdinal { get; } = maximumOrdinal;
    /// <summary>Gets exact ordinal overrides keyed by value.</summary>
    public FrozenDictionary<int, string> OrdinalMap { get; } = ordinalMap;
}

/// <summary>
/// One descending scale row for <see cref="SegmentedScaleNumberToWordsConverter"/>.
/// </summary>
/// <param name="Value">The divisor for the scale row.</param>
/// <param name="Singular">The singular scale name.</param>
/// <param name="Plural">The plural scale name.</param>
/// <param name="CountVariant">The variant used when rendering the count.</param>
/// <param name="SingularRemainderVariant">The variant used when a singular scale has a remainder.</param>
/// <param name="PluralRemainderVariant">The variant used when a plural scale has a remainder.</param>
readonly record struct SegmentedScale(
    long Value,
    string Singular,
    string Plural,
    SegmentedScaleVariant CountVariant,
    SegmentedScaleVariant SingularRemainderVariant,
    SegmentedScaleVariant PluralRemainderVariant);

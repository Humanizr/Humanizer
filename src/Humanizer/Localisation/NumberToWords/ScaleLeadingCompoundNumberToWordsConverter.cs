namespace Humanizer;

/// <summary>
/// Renders decimal numbering systems where scale nouns lead their counts, for example
/// "hundred two" or "thousand one", while sub-hundred compounds keep a tens-plus-unit
/// conjunction.
/// </summary>
class ScaleLeadingCompoundNumberToWordsConverter(ScaleLeadingCompoundNumberToWordsProfile profile) : GenderlessNumberToWordsConverter
{
    readonly ScaleLeadingCompoundNumberToWordsProfile profile = profile;

    /// <inheritdoc />
    public override string Convert(long number)
    {
        if (number == 0)
        {
            return profile.ZeroWord;
        }

        if (number < 0)
        {
            var magnitude = number == long.MinValue
                ? (ulong)long.MaxValue + 1UL
                : (ulong)-number;
            return profile.MinusWord + " " + ConvertPositive(magnitude);
        }

        return ConvertPositive((ulong)number);
    }

    /// <inheritdoc />
    public override string ConvertToOrdinal(int number)
    {
        if (profile.OrdinalMap.TryGetValue(number, out var ordinal))
        {
            return ordinal;
        }

        return profile.OrdinalPrefix + Convert(number) + profile.OrdinalSuffix;
    }

    string ConvertPositive(ulong number)
    {
        if (number < (ulong)profile.UnitsMap.Length)
        {
            return profile.UnitsMap[(int)number];
        }

        if (number < 100)
        {
            return ConvertUnderOneHundred(number);
        }

        var parts = new List<string>();
        var remainder = number;
        foreach (var scale in profile.Scales)
        {
            var scaleValue = (ulong)scale.Value;
            var count = remainder / scaleValue;
            if (count == 0)
            {
                continue;
            }

            parts.Add(scale.Name + " " + ConvertPositive(count));
            remainder %= scaleValue;
        }

        if (remainder > 0)
        {
            if (remainder < 100)
            {
                parts.Add(profile.ConjunctionWord);
            }

            parts.Add(ConvertPositive(remainder));
        }

        return string.Join(" ", parts);
    }

    string ConvertUnderOneHundred(ulong number)
    {
        var tens = number / 10;
        var units = number % 10;
        var tensWord = profile.TensMap[(int)tens];
        return units == 0
            ? tensWord
            : tensWord + " " + profile.ConjunctionWord + " " + profile.UnitsMap[(int)units];
    }
}

/// <summary>
/// Immutable generated profile for <see cref="ScaleLeadingCompoundNumberToWordsConverter"/>.
/// </summary>
sealed class ScaleLeadingCompoundNumberToWordsProfile(
    string zeroWord,
    string minusWord,
    string conjunctionWord,
    string ordinalPrefix,
    string ordinalSuffix,
    string[] unitsMap,
    string[] tensMap,
    ScaleLeadingCompoundScale[] scales,
    FrozenDictionary<int, string>? ordinalMap = null)
{
    /// <summary>Gets the word used for zero.</summary>
    public string ZeroWord { get; } = zeroWord;
    /// <summary>Gets the word used to prefix negative numbers.</summary>
    public string MinusWord { get; } = minusWord;
    /// <summary>Gets the conjunction inserted before sub-hundred remainders and between tens and units.</summary>
    public string ConjunctionWord { get; } = conjunctionWord;
    /// <summary>Gets the ordinal prefix used when no exact ordinal exists.</summary>
    public string OrdinalPrefix { get; } = ordinalPrefix;
    /// <summary>Gets the ordinal suffix used when no exact ordinal exists.</summary>
    public string OrdinalSuffix { get; } = ordinalSuffix;
    /// <summary>Gets cardinal words for zero through nineteen.</summary>
    public string[] UnitsMap { get; } = unitsMap;
    /// <summary>Gets decade words keyed by their tens digit.</summary>
    public string[] TensMap { get; } = tensMap;
    /// <summary>Gets descending scale rows.</summary>
    public ScaleLeadingCompoundScale[] Scales { get; } = scales;
    /// <summary>Gets exact ordinal words keyed by numeric value.</summary>
    public FrozenDictionary<int, string> OrdinalMap { get; } = ordinalMap ?? FrozenDictionary<int, string>.Empty;
}

/// <summary>
/// One descending scale row for scale-leading compound renderers.
/// </summary>
/// <param name="Value">The numeric scale value.</param>
/// <param name="Name">The localized scale noun.</param>
readonly record struct ScaleLeadingCompoundScale(long Value, string Name);
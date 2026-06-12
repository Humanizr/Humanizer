namespace Humanizer;

/// <summary>
/// Renders decimal numbering systems where scale nouns lead their count words, for example
/// "hundred one and two" or "thousand two", while sub-hundred compounds keep a
/// tens-plus-unit conjunction.
///
/// The conjunction supplied by the profile is inserted between tens and units and before
/// terminal sub-hundred remainders only. Higher-scale remainders are space-joined.
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
        if (number < 0)
        {
            var magnitude = number == int.MinValue
                ? (ulong)int.MaxValue + 1UL
                : (ulong)-number;
            return profile.MinusWord + " " + ConvertPositiveToOrdinal(magnitude);
        }

        return ConvertPositiveToOrdinal((ulong)number);
    }

    string ConvertPositiveToOrdinal(ulong number)
    {
        if (number <= int.MaxValue && profile.OrdinalMap.TryGetValue((int)number, out var ordinal))
        {
            return ordinal;
        }

        return profile.OrdinalPrefix + ConvertPositive(number) + profile.OrdinalSuffix;
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
        var consumedScale = false;
        ulong lastScaleCount = 0;
        long lastScaleValue = 0;
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
            consumedScale = true;
            lastScaleCount = count;
            lastScaleValue = scale.Value;
        }

        if (!consumedScale)
        {
            throw new InvalidOperationException("Scale-leading compound number profiles require a scale value no greater than 100.");
        }

        if (remainder > 0)
        {
            if (remainder < 100)
            {
                parts.Add(GetTerminalRemainderConjunction(lastScaleValue, lastScaleCount));
            }

            parts.Add(ConvertPositive(remainder));
        }

        return string.Join(" ", parts);
    }

    string GetTerminalRemainderConjunction(long scaleValue, ulong scaleCount) =>
        scaleValue >= 1000 && scaleCount >= 10
            ? profile.TerminalRemainderConjunctionWord
            : profile.ConjunctionWord;

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
    string? terminalRemainderConjunctionWord,
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
    /// <summary>
    /// Gets the conjunction inserted between tens and units and before terminal sub-hundred
    /// remainders. The scale-leading engine does not insert this conjunction before
    /// remainders of one hundred or greater.
    /// </summary>
    public string ConjunctionWord { get; } = conjunctionWord;
    /// <summary>Gets the conjunction inserted before potentially ambiguous terminal remainders.</summary>
    public string TerminalRemainderConjunctionWord { get; } = string.IsNullOrWhiteSpace(terminalRemainderConjunctionWord) ? conjunctionWord : terminalRemainderConjunctionWord!;
    /// <summary>Gets the ordinal prefix used when no exact ordinal exists.</summary>
    public string OrdinalPrefix { get; } = ordinalPrefix;
    /// <summary>Gets the ordinal suffix used when no exact ordinal exists.</summary>
    public string OrdinalSuffix { get; } = ordinalSuffix;
    /// <summary>Gets cardinal words for zero through nineteen.</summary>
    public string[] UnitsMap { get; } = ValidateUnitsMap(unitsMap);
    /// <summary>Gets decade words keyed by their tens digit.</summary>
    public string[] TensMap { get; } = ValidateTensMap(tensMap);
    /// <summary>Gets descending scale rows.</summary>
    public ScaleLeadingCompoundScale[] Scales { get; } = ValidateScales(scales);
    /// <summary>Gets exact ordinal words keyed by numeric value.</summary>
    public FrozenDictionary<int, string> OrdinalMap { get; } = ordinalMap ?? FrozenDictionary<int, string>.Empty;

    static string[] ValidateUnitsMap(string[] value)
    {
        if (value.Length < 20)
        {
            throw new InvalidOperationException("Scale-leading compound number profiles require unit words for 0 through 19.");
        }

        return value;
    }

    static string[] ValidateTensMap(string[] value)
    {
        if (value.Length < 10)
        {
            throw new InvalidOperationException("Scale-leading compound number profiles require decade words keyed through index 9.");
        }

        return value;
    }

    static ScaleLeadingCompoundScale[] ValidateScales(ScaleLeadingCompoundScale[] value)
    {
        if (value.Length == 0 || value[^1].Value > 100)
        {
            throw new InvalidOperationException("Scale-leading compound number profiles require a scale value no greater than 100.");
        }

        for (var i = 0; i < value.Length; i++)
        {
            if (value[i].Value <= 0)
            {
                throw new InvalidOperationException("Scale-leading compound number profiles require positive scale values.");
            }

            if (i > 0 && value[i - 1].Value <= value[i].Value)
            {
                throw new InvalidOperationException("Scale-leading compound number profiles require scales in descending order.");
            }
        }

        return value;
    }
}

/// <summary>
/// One descending scale row for scale-leading compound renderers.
/// </summary>
/// <param name="Value">The numeric scale value.</param>
/// <param name="Name">The localized scale noun.</param>
readonly record struct ScaleLeadingCompoundScale(long Value, string Name);
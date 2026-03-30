namespace Humanizer;

class SegmentedScaleNumberToWordsConverter(SegmentedScaleNumberToWordsProfile profile) : GenderlessNumberToWordsConverter
{
    readonly SegmentedScaleNumberToWordsProfile profile = profile;

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

    static ReadOnlySpan<int> OrdinalDecompositionPlaces => [1000, 100, 10];

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

    static ulong GetAbsoluteValue(long value) =>
        value >= 0 ? (ulong)value : unchecked((ulong)(-(value + 1)) + 1);
}

enum SegmentedScaleVariant
{
    Default,
    Pluralized
}

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
    public long MaximumValue { get; } = maximumValue;
    public string ZeroWord { get; } = zeroWord;
    public string MinusWord { get; } = minusWord;
    public string TeenPrefix { get; } = teenPrefix;
    public string ExactOneHundredWord { get; } = exactOneHundredWord;
    public string[] UnitsDefault { get; } = unitsDefault;
    public string[] UnitsPluralized { get; } = unitsPluralized;
    public string[] TensMap { get; } = tensMap;
    public string[] HundredsDefault { get; } = hundredsDefault;
    public string[] HundredsPluralized { get; } = hundredsPluralized;
    public SegmentedScale[] Scales { get; } = scales;
    public int MaximumOrdinal { get; } = maximumOrdinal;
    public FrozenDictionary<int, string> OrdinalMap { get; } = ordinalMap;
}

readonly record struct SegmentedScale(
    long Value,
    string Singular,
    string Plural,
    SegmentedScaleVariant CountVariant,
    SegmentedScaleVariant SingularRemainderVariant,
    SegmentedScaleVariant PluralRemainderVariant);

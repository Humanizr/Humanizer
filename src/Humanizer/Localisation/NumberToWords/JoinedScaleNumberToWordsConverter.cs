namespace Humanizer;

class JoinedScaleNumberToWordsConverter(JoinedScaleNumberToWordsProfile profile) : GenderlessNumberToWordsConverter
{
    readonly JoinedScaleNumberToWordsProfile profile = profile;

    public override string Convert(long number)
    {
        var magnitude = number == long.MinValue
            ? (ulong)long.MaxValue + 1
            : (ulong)Math.Abs(number);
        var maximumMagnitude = (ulong)profile.MaximumValue + (profile.AllowLongMinValue ? 1UL : 0UL);
        if (magnitude > maximumMagnitude)
        {
            throw new NotImplementedException();
        }

        if (number == 0)
        {
            return profile.ZeroWord;
        }

        if (number < 0)
        {
            return $"{profile.MinusWord}{profile.NegativeJoinWord}{ConvertNonNegative(magnitude)}";
        }

        return ConvertNonNegative((ulong)number);
    }

    string ConvertNonNegative(ulong number)
    {
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

            parts.Add(scale.OmitOneWhenSingular && count == 1 && (profile.OmitOneWhenSingularAlways || parts.Count == 0)
                ? scale.Name
                : $"{Convert((long)count)}{profile.ScaleCountJoinWord}{scale.Name}");
            remainder %= scaleValue;
        }

        if (remainder >= 100)
        {
            parts.Add(profile.HundredsMap[remainder / 100]);
            remainder %= 100;
        }

        if (remainder > 0)
        {
            parts.Add(ConvertUnderHundred((int)remainder));
        }

        return string.Join(profile.JoinWord, parts);
    }

    public override string ConvertToOrdinal(int number)
    {
        if (profile.OrdinalExceptions is not null &&
            profile.OrdinalExceptions.TryGetValue(number, out var exactOrdinal))
        {
            return exactOrdinal;
        }

        if (profile.CompoundOrdinalRemainder is not null &&
            profile.CompoundOrdinalWord is not null &&
            number >= 20 &&
            number % 10 == profile.CompoundOrdinalRemainder.Value &&
            !profile.CompoundOrdinalExcludedValues.Contains(number))
        {
            return $"{Convert(number / 10 * 10)}{profile.JoinWord}{profile.CompoundOrdinalWord}";
        }

        var words = Convert(number);
        if (words.Length == 0)
        {
            return words;
        }

        if (!string.IsNullOrEmpty(profile.OrdinalSuffixMatchCharacters) &&
            profile.MatchingOrdinalSuffix is not null &&
            profile.OrdinalSuffixMatchCharacters.Contains(words[^1]))
        {
            return words + profile.MatchingOrdinalSuffix;
        }

        return words + profile.DefaultOrdinalSuffix;
    }

    string ConvertUnderHundred(int number)
    {
        if (profile.SubHundredMap.Length != 0)
        {
            return profile.SubHundredMap[number];
        }

        if (number >= 20)
        {
            var parts = new List<string>(2)
            {
                profile.TensMap[number / 10]
            };

            if (number % 10 > 0)
            {
                parts.Add(profile.UnitsMap[number % 10]);
            }

            return string.Join(profile.UnderHundredJoinWord, parts);
        }

        return profile.UnitsMap[number];
    }
}

sealed class JoinedScaleNumberToWordsProfile(
    long maximumValue,
    string zeroWord,
    string minusWord,
    string negativeJoinWord,
    string joinWord,
    string scaleCountJoinWord,
    string underHundredJoinWord,
    bool omitOneWhenSingularAlways,
    string defaultOrdinalSuffix,
    string? matchingOrdinalSuffix,
    string? ordinalSuffixMatchCharacters,
    string[] unitsMap,
    string[] tensMap,
    string[] hundredsMap,
    string[] subHundredMap,
    JoinedScale[] scales,
    FrozenDictionary<int, string>? ordinalExceptions = null,
    int? compoundOrdinalRemainder = null,
    string? compoundOrdinalWord = null,
    FrozenSet<int>? compoundOrdinalExcludedValues = null)
{
    public long MaximumValue { get; } = maximumValue;
    public string ZeroWord { get; } = zeroWord;
    public string MinusWord { get; } = minusWord;
    public string NegativeJoinWord { get; } = negativeJoinWord;
    public string JoinWord { get; } = joinWord;
    public string ScaleCountJoinWord { get; } = scaleCountJoinWord;
    public string UnderHundredJoinWord { get; } = underHundredJoinWord;
    public bool OmitOneWhenSingularAlways { get; } = omitOneWhenSingularAlways;
    public string DefaultOrdinalSuffix { get; } = defaultOrdinalSuffix;
    public string? MatchingOrdinalSuffix { get; } = matchingOrdinalSuffix;
    public string? OrdinalSuffixMatchCharacters { get; } = ordinalSuffixMatchCharacters;
    public string[] UnitsMap { get; } = unitsMap;
    public string[] TensMap { get; } = tensMap;
    public string[] HundredsMap { get; } = hundredsMap;
    public string[] SubHundredMap { get; } = subHundredMap;
    public JoinedScale[] Scales { get; } = scales;
    public FrozenDictionary<int, string>? OrdinalExceptions { get; } = ordinalExceptions;
    public int? CompoundOrdinalRemainder { get; } = compoundOrdinalRemainder;
    public string? CompoundOrdinalWord { get; } = compoundOrdinalWord;
    public FrozenSet<int> CompoundOrdinalExcludedValues { get; } = compoundOrdinalExcludedValues ?? FrozenSet<int>.Empty;
    public bool AllowLongMinValue { get; } = maximumValue == long.MaxValue;
}

readonly record struct JoinedScale(long Value, string Name, bool OmitOneWhenSingular = false);

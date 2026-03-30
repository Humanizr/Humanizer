namespace Humanizer;

class AgglutinativeOrdinalScaleNumberToWordsConverter(AgglutinativeOrdinalScaleNumberToWordsProfile profile) : GenderlessNumberToWordsConverter
{
    readonly AgglutinativeOrdinalScaleNumberToWordsProfile profile = profile;

    public override string Convert(long input)
    {
        if (input is > int.MaxValue or < int.MinValue)
        {
            throw new NotImplementedException();
        }

        var number = (int)input;
        if (number < 0)
        {
            return profile.MinusWord + Convert(-number);
        }

        return ConvertCardinal(number);
    }

    public override string ConvertToOrdinal(int number) =>
        ConvertOrdinal(number, useExceptions: false);

    string ConvertCardinal(int number)
    {
        if (number == 0)
        {
            return profile.UnitsMap[0];
        }

        var parts = new List<string>();
        var remainder = number;

        foreach (var scale in profile.Scales)
        {
            var divisor = checked((int)scale.Value);
            if (remainder / divisor <= 0)
            {
                continue;
            }

            var count = remainder / divisor;
            var scaleRemainder = remainder % divisor;
            parts.Add((count == 1
                ? scale.SingularCardinal
                : ConvertCardinal(count) + scale.PluralCardinal) +
                (scale.Value >= 1000 && scaleRemainder > 0 ? " " : string.Empty));
            remainder = scaleRemainder;
        }

        if (remainder >= 20)
        {
            parts.Add(ConvertCardinal(remainder / 10) + profile.TensSuffix);
            remainder %= 10;
        }
        else if (remainder is > 10 and < 20)
        {
            parts.Add(profile.UnitsMap[remainder % 10] + profile.TeenSuffix);
            remainder = 0;
        }

        if (remainder is > 0 and <= 10)
        {
            parts.Add(profile.UnitsMap[remainder]);
        }

        return string.Concat(parts);
    }

    string ConvertOrdinal(int number, bool useExceptions)
    {
        if (number == 0)
        {
            return profile.OrdinalUnitsMap[0];
        }

        if (number < 0)
        {
            return profile.MinusWord + ConvertOrdinal(-number, useExceptions: false);
        }

        var parts = new List<string>();
        var remainder = number;

        foreach (var scale in profile.Scales)
        {
            var divisor = checked((int)scale.Value);
            if (remainder / divisor <= 0)
            {
                continue;
            }

            var count = remainder / divisor;
            var scaleRemainder = remainder % divisor;
            parts.Add((count == 1 ? string.Empty : ConvertOrdinal(count, useExceptions: true)) + scale.OrdinalWord);
            remainder = scaleRemainder;
        }

        if (remainder >= 20)
        {
            parts.Add(ConvertOrdinal(remainder / 10, useExceptions: true) + profile.OrdinalTensSuffix);
            remainder %= 10;
        }
        else if (remainder is > 10 and < 20)
        {
            parts.Add(GetOrdinalUnit(remainder % 10, useExceptions: true) + profile.TeenSuffix);
            remainder = 0;
        }

        if (remainder is > 0 and <= 10)
        {
            parts.Add(GetOrdinalUnit(remainder, useExceptions));
        }

        return string.Concat(parts);
    }

    string GetOrdinalUnit(int number, bool useExceptions) =>
        useExceptions && profile.OrdinalExceptions.TryGetValue(number, out var unit)
            ? unit
            : profile.OrdinalUnitsMap[number];
}

sealed class AgglutinativeOrdinalScaleNumberToWordsProfile(
    string minusWord,
    string tensSuffix,
    string teenSuffix,
    string ordinalTensSuffix,
    string[] unitsMap,
    string[] ordinalUnitsMap,
    AgglutinativeOrdinalScale[] scales,
    FrozenDictionary<int, string> ordinalExceptions)
{
    public string MinusWord { get; } = minusWord;
    public string TensSuffix { get; } = tensSuffix;
    public string TeenSuffix { get; } = teenSuffix;
    public string OrdinalTensSuffix { get; } = ordinalTensSuffix;
    public string[] UnitsMap { get; } = unitsMap;
    public string[] OrdinalUnitsMap { get; } = ordinalUnitsMap;
    public AgglutinativeOrdinalScale[] Scales { get; } = scales;
    public FrozenDictionary<int, string> OrdinalExceptions { get; } = ordinalExceptions;
}

readonly record struct AgglutinativeOrdinalScale(long Value, string SingularCardinal, string PluralCardinal, string OrdinalWord);

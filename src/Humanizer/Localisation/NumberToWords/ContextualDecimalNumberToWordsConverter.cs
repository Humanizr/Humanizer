namespace Humanizer;

class ContextualDecimalNumberToWordsConverter(ContextualDecimalNumberToWordsProfile profile) : GenderlessNumberToWordsConverter
{
    readonly ContextualDecimalNumberToWordsProfile profile = profile;

    public override string Convert(long number) =>
        number == 0
            ? profile.ZeroWord
            : ConvertCardinal(number);

    public override string ConvertToOrdinal(int number) =>
        profile.OrdinalPrefix + ConvertOrdinalBody(number);

    string ConvertCardinal(long number, bool hasTens = false, bool hasHigherGroup = false, bool usePostTensUnitOverrides = false)
    {
        if (number < 0)
        {
            return profile.MinusWord + ConvertCardinal(-number, hasTens, hasHigherGroup, usePostTensUnitOverrides);
        }

        foreach (var scale in profile.Scales)
        {
            if (number < scale.Value)
            {
                continue;
            }

            return $"{ConvertCardinal(number / scale.Value)} {scale.Name} {ConvertCardinal(number % scale.Value, hasHigherGroup: true)}".TrimEnd();
        }

        if (number >= 20)
        {
            return $"{profile.DigitWords[(int)(number / 10)]} {profile.TensWord} {ConvertCardinal(number % 10, hasTens: true, usePostTensUnitOverrides: true)}".TrimEnd();
        }

        if (number >= 10)
        {
            if (profile.TeenUnitOverrides.TryGetValue((int)(number % 10), out var teenOverride))
            {
                return $"{profile.TenWord} {teenOverride}";
            }

            return number == 10
                ? profile.TenWord
                : $"{profile.TenWord} {ConvertCardinal(number % 10, hasTens: true, usePostTensUnitOverrides: false)}".TrimEnd();
        }

        if (hasTens && usePostTensUnitOverrides && profile.PostTensUnitOverrides.TryGetValue((int)number, out var postTensUnit))
        {
            return postTensUnit;
        }

        if (number > 0 && hasHigherGroup && !hasTens)
        {
            return $"{profile.ZeroTensWord} {profile.DigitWords[(int)number]}";
        }

        return profile.DigitWords[(int)number];
    }

    string ConvertOrdinalBody(int number)
    {
        if (profile.OrdinalUnitOverrides.TryGetValue(number, out var ordinalUnit))
        {
            return ordinalUnit;
        }

        return Convert(number);
    }
}

sealed class ContextualDecimalNumberToWordsProfile(
    string zeroWord,
    string minusWord,
    string ordinalPrefix,
    string tenWord,
    string tensWord,
    string zeroTensWord,
    string[] digitWords,
    ContextualDecimalScale[] scales,
    FrozenDictionary<int, string> teenUnitOverrides,
    FrozenDictionary<int, string> postTensUnitOverrides,
    FrozenDictionary<int, string> ordinalUnitOverrides)
{
    public string ZeroWord { get; } = zeroWord;
    public string MinusWord { get; } = minusWord;
    public string OrdinalPrefix { get; } = ordinalPrefix;
    public string TenWord { get; } = tenWord;
    public string TensWord { get; } = tensWord;
    public string ZeroTensWord { get; } = zeroTensWord;
    public string[] DigitWords { get; } = digitWords;
    public ContextualDecimalScale[] Scales { get; } = scales;
    public FrozenDictionary<int, string> TeenUnitOverrides { get; } = teenUnitOverrides;
    public FrozenDictionary<int, string> PostTensUnitOverrides { get; } = postTensUnitOverrides;
    public FrozenDictionary<int, string> OrdinalUnitOverrides { get; } = ordinalUnitOverrides;
}

readonly record struct ContextualDecimalScale(long Value, string Name);

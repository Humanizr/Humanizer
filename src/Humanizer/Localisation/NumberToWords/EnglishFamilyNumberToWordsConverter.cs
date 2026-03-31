namespace Humanizer;

// Covers English-derived cardinal/ordinal systems whose main differences are conjunction and scale rules.
class EnglishFamilyNumberToWordsConverter(EnglishFamilyNumberToWordsProfile profile) : GenderlessNumberToWordsConverter
{
    readonly EnglishFamilyNumberToWordsProfile profile = profile;

    public override string Convert(long number) =>
        Convert(number, profile.DefaultAddAnd);

    public override string Convert(long number, bool addAnd) =>
        ConvertCore(
            number,
            isOrdinal: false,
            profile.AddAndMode == EnglishFamilyAddAndMode.UseCallerFlag ? addAnd : profile.DefaultAddAnd);

    public override string ConvertToOrdinal(int number) =>
        profile.OrdinalMode == EnglishFamilyOrdinalMode.Cardinal
            ? Convert(number, profile.DefaultAddAnd)
            : ConvertCore(number, isOrdinal: true, profile.DefaultAddAnd);

    public override string ConvertToTuple(int number) =>
        profile.NamedTuples is not null && profile.NamedTuples.TryGetValue(number, out var namedTuple)
            ? namedTuple
            : profile.TupleSuffix.Length != 0
                ? $"{number}{profile.TupleSuffix}"
                : base.ConvertToTuple(number);

    // Large scales recurse through the same engine so cultures only vary by generated lexicon and strategies.
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

            parts.Add(ConvertCore(count, isOrdinal: false, addAnd));
            parts.Add(remainder % scale.Value == 0 && isOrdinal ? scale.OrdinalName : scale.Name);
            remainder %= scale.Value;
        }

        AppendUnderThousand(parts, remainder, isOrdinal, addAnd);

        ApplyOrdinalLeadingOneStrategy(parts, isOrdinal);

        return string.Join(" ", parts);
    }

    // The under-thousand path is the real family algorithm: hundreds are fixed, while conjunction and tens joining are profile-driven.
    void AppendUnderThousand(List<string> parts, long number, bool isOrdinal, bool addAnd)
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

        if (ShouldAddAnd(addAnd, hasHundreds, parts.Count > 0))
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

    // "and" placement is a culture rule, not a locale-specific branch, so the profile chooses the insertion points.
    bool ShouldAddAnd(bool addAnd, bool hasHundreds, bool hasScalePrefix) =>
        addAnd && profile.AndStrategy switch
        {
            EnglishFamilyAndStrategy.WithinGroupAndAfterScaleSubHundredRemainder => hasHundreds || hasScalePrefix,
            EnglishFamilyAndStrategy.WithinGroupOnly => hasHundreds,
            EnglishFamilyAndStrategy.AfterScaleSubHundredRemainderOnly => !hasHundreds && hasScalePrefix,
            EnglishFamilyAndStrategy.Never => false,
            _ => throw new InvalidOperationException("Unsupported English-family conjunction strategy.")
        };

    void ApplyOrdinalLeadingOneStrategy(List<string> parts, bool isOrdinal)
    {
        if (!isOrdinal || profile.OrdinalLeadingOneStrategy != EnglishFamilyOrdinalLeadingOneStrategy.OmitLeadingOne || parts.Count == 0)
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

// Holds the immutable lexicon and strategy switches emitted from JSON profiles.
sealed class EnglishFamilyNumberToWordsProfile(
    string minusWord,
    string andWord,
    string hundredWord,
    string hundredOrdinalWord,
    string tensUnitsSeparator,
    bool defaultAddAnd,
    EnglishFamilyAddAndMode addAndMode,
    EnglishFamilyAndStrategy andStrategy,
    string tupleSuffix,
    EnglishFamilyOrdinalLeadingOneStrategy ordinalLeadingOneStrategy,
    EnglishFamilyOrdinalMode ordinalMode,
    string[] unitsMap,
    string[] ordinalUnitsMap,
    string[] tensMap,
    string[] ordinalTensMap,
    EnglishFamilyScale[] scales,
    FrozenDictionary<int, string>? namedTuples = null)
{
    public string MinusWord { get; } = minusWord;
    public string AndWord { get; } = andWord;
    public string HundredWord { get; } = hundredWord;
    public string HundredOrdinalWord { get; } = hundredOrdinalWord;
    public string TensUnitsSeparator { get; } = tensUnitsSeparator;
    public bool DefaultAddAnd { get; } = defaultAddAnd;
    public EnglishFamilyAddAndMode AddAndMode { get; } = addAndMode;
    public EnglishFamilyAndStrategy AndStrategy { get; } = andStrategy;
    public string TupleSuffix { get; } = tupleSuffix;
    public EnglishFamilyOrdinalLeadingOneStrategy OrdinalLeadingOneStrategy { get; } = ordinalLeadingOneStrategy;
    public EnglishFamilyOrdinalMode OrdinalMode { get; } = ordinalMode;
    public string[] UnitsMap { get; } = unitsMap;
    public string[] OrdinalUnitsMap { get; } = ordinalUnitsMap;
    public string[] TensMap { get; } = tensMap;
    public string[] OrdinalTensMap { get; } = ordinalTensMap;
    public EnglishFamilyScale[] Scales { get; } = scales;
    public FrozenDictionary<int, string>? NamedTuples { get; } = namedTuples;
}

// Controls whether callers may override the family default "and" behavior.
enum EnglishFamilyAddAndMode
{
    UseCallerFlag,
    AlwaysDefault
}

// Determines where the family injects conjunctions for sub-hundred remainders.
enum EnglishFamilyAndStrategy
{
    WithinGroupAndAfterScaleSubHundredRemainder,
    WithinGroupOnly,
    AfterScaleSubHundredRemainderOnly,
    Never
}

// Some locales drop the leading "one" in scale ordinals ("thousandth"), while others keep the cardinal phrase.
enum EnglishFamilyOrdinalLeadingOneStrategy
{
    KeepLeadingOne,
    OmitLeadingOne
}

// Some English-derived locales expose true ordinals; others intentionally reuse cardinal wording.
enum EnglishFamilyOrdinalMode
{
    English,
    Cardinal
}

// Large-scale words stay pure data so new English-derived locales mostly add profile rows, not converter code.
readonly record struct EnglishFamilyScale(long Value, string Name, string OrdinalName);

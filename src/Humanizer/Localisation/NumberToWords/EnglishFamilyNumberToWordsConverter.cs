namespace Humanizer;

class EnglishFamilyNumberToWordsConverter(EnglishFamilyNumberToWordsProfile profile) : GenderlessNumberToWordsConverter
{
    readonly EnglishFamilyNumberToWordsProfile profile = profile;

    public override string Convert(long number) =>
        Convert(number, profile.DefaultAddAnd);

    public override string Convert(long number, bool addAnd) =>
        ConvertCore(number, isOrdinal: false, profile.RespectAddAndFlag ? addAnd : profile.DefaultAddAnd);

    public override string ConvertToOrdinal(int number) =>
        profile.OrdinalMode == EnglishFamilyOrdinalMode.Cardinal
            ? Convert(number, profile.DefaultAddAnd)
            : ConvertCore(number, isOrdinal: true, profile.DefaultAddAnd);

    public override string ConvertToTuple(int number) =>
        profile.NamedTuples is not null && profile.NamedTuples.TryGetValue(number, out var namedTuple)
            ? namedTuple
            : profile.NamedTuples is not null
                ? $"{number}-tuple"
                : base.ConvertToTuple(number);

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

        if (isOrdinal &&
            profile.OmitLeadingOneInOrdinal &&
            parts.Count > 0 &&
            (parts[0] == profile.UnitsMap[1] ||
             parts[0].StartsWith(profile.UnitsMap[1] + " ", StringComparison.Ordinal)))
        {
            if (parts[0] == profile.UnitsMap[1])
            {
                parts.RemoveAt(0);
            }
            else
            {
                parts[0] = parts[0][profile.UnitsMap[1].Length..].TrimStart();
            }
        }

        return string.Join(" ", parts);
    }

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

        if (addAnd &&
            ((hasHundreds && profile.UseAndWithinGroup) ||
             (!hasHundreds && parts.Count > 0 && profile.UseAndAfterScaleForSubHundredRemainder)))
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
}

sealed class EnglishFamilyNumberToWordsProfile(
    string minusWord,
    string andWord,
    string hundredWord,
    string hundredOrdinalWord,
    string tensUnitsSeparator,
    bool defaultAddAnd,
    bool respectAddAndFlag,
    bool useAndWithinGroup,
    bool useAndAfterScaleForSubHundredRemainder,
    bool omitLeadingOneInOrdinal,
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
    public bool RespectAddAndFlag { get; } = respectAddAndFlag;
    public bool UseAndWithinGroup { get; } = useAndWithinGroup;
    public bool UseAndAfterScaleForSubHundredRemainder { get; } = useAndAfterScaleForSubHundredRemainder;
    public bool OmitLeadingOneInOrdinal { get; } = omitLeadingOneInOrdinal;
    public EnglishFamilyOrdinalMode OrdinalMode { get; } = ordinalMode;
    public string[] UnitsMap { get; } = unitsMap;
    public string[] OrdinalUnitsMap { get; } = ordinalUnitsMap;
    public string[] TensMap { get; } = tensMap;
    public string[] OrdinalTensMap { get; } = ordinalTensMap;
    public EnglishFamilyScale[] Scales { get; } = scales;
    public FrozenDictionary<int, string>? NamedTuples { get; } = namedTuples;
}

enum EnglishFamilyOrdinalMode
{
    English,
    Cardinal
}

readonly record struct EnglishFamilyScale(long Value, string Name, string OrdinalName);

namespace Humanizer;

class TurkicFamilyNumberToWordsConverter(TurkicNumberToWordsProfile profile) : GenderlessNumberToWordsConverter
{
    readonly TurkicNumberToWordsProfile profile = profile;

    public override string Convert(long input)
    {
        if (input > profile.MaximumValue || input < profile.MinimumValue)
        {
            throw new NotImplementedException();
        }

        var number = input;
        if (number == 0)
        {
            return profile.UnitsMap[0];
        }

        if (number < 0)
        {
            return $"{profile.MinusWord} {Convert(-number)}";
        }

        var parts = new List<string>();

        foreach (var scale in profile.Scales)
        {
            var count = number / scale.Value;
            if (count <= 0)
            {
                continue;
            }

            parts.Add(scale.OmitOneWhenSingular && count == 1
                ? scale.Name
                : $"{Convert(count)} {scale.Name}");
            number %= scale.Value;
        }

        var hundred = number / 100;
        if (hundred > 0)
        {
            parts.Add(hundred > 1
                ? $"{Convert(hundred)} {profile.HundredWord}"
                : profile.HundredWord);
            number %= 100;
        }

        if (number / 10 > 0)
        {
            parts.Add(profile.TensMap[number / 10]);
            number %= 10;
        }

        if (number > 0)
        {
            parts.Add(profile.UnitsMap[number]);
        }

        return string.Join(" ", parts);
    }

    public override string ConvertToOrdinal(int number)
    {
        var word = Convert(number);
        return AppendHarmonySuffix(word, profile.OrdinalSuffixes);
    }

    public override string ConvertToTuple(int number)
    {
        if (profile.NamedTuples is not null && profile.NamedTuples.TryGetValue(number, out var namedTuple))
        {
            return namedTuple;
        }

        if (profile.TupleSuffixes is null)
        {
            return base.ConvertToTuple(number);
        }

        return AppendHarmonySuffix(Convert(number), profile.TupleSuffixes);
    }

    static string AppendHarmonySuffix(string word, FrozenDictionary<char, string> suffixes)
    {
        var suffix = string.Empty;
        var suffixFoundOnLastVowel = false;

        for (var i = word.Length - 1; i >= 0; i--)
        {
            if (suffixes.TryGetValue(word[i], out suffix))
            {
                suffixFoundOnLastVowel = i == word.Length - 1;
                break;
            }
        }

        if (word[^1] == 't')
        {
            word = StringHumanizeExtensions.Concat(word.AsSpan(0, word.Length - 1), 'd');
        }

        if (suffixFoundOnLastVowel)
        {
            word = word[..^1];
        }

        return word + suffix;
    }
}

sealed class TurkicNumberToWordsProfile(
    long minimumValue,
    long maximumValue,
    string minusWord,
    string hundredWord,
    string[] unitsMap,
    string[] tensMap,
    TurkicScale[] scales,
    FrozenDictionary<char, string> ordinalSuffixes,
    FrozenDictionary<char, string>? tupleSuffixes = null,
    FrozenDictionary<int, string>? namedTuples = null)
{
    public long MinimumValue { get; } = minimumValue;
    public long MaximumValue { get; } = maximumValue;
    public string MinusWord { get; } = minusWord;
    public string HundredWord { get; } = hundredWord;
    public string[] UnitsMap { get; } = unitsMap;
    public string[] TensMap { get; } = tensMap;
    public TurkicScale[] Scales { get; } = scales;
    public FrozenDictionary<char, string> OrdinalSuffixes { get; } = ordinalSuffixes;
    public FrozenDictionary<char, string>? TupleSuffixes { get; } = tupleSuffixes;
    public FrozenDictionary<int, string>? NamedTuples { get; } = namedTuples;
}

readonly record struct TurkicScale(long Value, string Name, bool OmitOneWhenSingular = false);

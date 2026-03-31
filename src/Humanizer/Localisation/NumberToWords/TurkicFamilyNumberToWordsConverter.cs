namespace Humanizer;

// Shared engine for decimal-scale Turkic languages whose main variation is ordinal suffix selection.
class TurkicFamilyNumberToWordsConverter(TurkicNumberToWordsProfile profile) : GenderlessNumberToWordsConverter
{
    readonly TurkicNumberToWordsProfile profile = profile;

    public override string Convert(long input)
    {
        if (input > profile.MaximumValue || input < profile.MinimumValue)
        {
            throw new NotImplementedException();
        }

        return ConvertCore(input, allowExactHundredWord: true);
    }

    // Composite counts recurse through the same decimal-scale engine; only the generated hundred/ordinal strategies vary per locale.
    string ConvertCore(long input, bool allowExactHundredWord)
    {
        var number = input;
        if (number == 0)
        {
            return profile.UnitsMap[0];
        }

        if (number < 0)
        {
            return $"{profile.MinusWord} {ConvertCore(-number, allowExactHundredWord)}";
        }

        if (allowExactHundredWord &&
            profile.HundredStrategy == TurkicHundredStrategy.AllowExplicitOneInComposite &&
            number == 100)
        {
            return profile.HundredWord;
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
                : $"{ConvertCore(count, scale.AllowBareHundredInCount)} {scale.Name}");
            number %= scale.Value;
        }

        var hundred = number / 100;
        if (hundred > 0)
        {
            parts.Add(FormatHundreds(hundred));
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

    // Uzbek keeps the explicit "one" in composite hundreds; Turkish/Azerbaijani collapse it away.
    string FormatHundreds(long hundred)
    {
        if (hundred > 1)
        {
            return $"{ConvertCore(hundred, allowExactHundredWord: false)} {profile.HundredWord}";
        }

        return profile.HundredStrategy == TurkicHundredStrategy.AllowExplicitOneInComposite
            ? $"{profile.UnitsMap[1]} {profile.HundredWord}"
            : profile.HundredWord;
    }

    public override string ConvertToOrdinal(int number)
    {
        var word = Convert(number);
        return profile.OrdinalSuffixStrategy switch
        {
            TurkicOrdinalSuffixStrategy.LastVowelMap => AppendHarmonySuffix(word, profile.OrdinalSuffixes),
            TurkicOrdinalSuffixStrategy.FinalCharacterMembership => AppendMembershipOrdinalSuffix(
                word,
                profile.SecondOrdinalSuffixCharacters,
                profile.OrdinalSuffixPair),
            _ => throw new InvalidOperationException("Unsupported Turkic ordinal suffix strategy.")
        };
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

    // Vowel-harmony ordinals are the common Turkic rule and stay fully data-driven through the generated suffix map.
    string AppendHarmonySuffix(string word, FrozenDictionary<char, string>? suffixes)
    {
        if (suffixes is null)
        {
            throw new InvalidOperationException("Turkic harmony suffix data is missing.");
        }

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

        word = ApplyHarmonyStemAdjustments(word, suffixFoundOnLastVowel);
        return word + suffix;
    }

    // Uzbek-style ordinals split between two suffixes by final character membership instead of last-vowel lookup.
    static string AppendMembershipOrdinalSuffix(string word, string? secondOrdinalSuffixCharacters, string[] ordinalSuffixPair)
    {
        if (string.IsNullOrEmpty(secondOrdinalSuffixCharacters) || ordinalSuffixPair.Length != 2)
        {
            throw new InvalidOperationException("Turkic membership ordinal suffix data is incomplete.");
        }

        var suffixIndex = secondOrdinalSuffixCharacters.Contains(word[^1]) ? 1 : 0;
        return word + ordinalSuffixPair[suffixIndex];
    }

    string ApplyHarmonyStemAdjustments(string word, bool suffixFoundOnLastVowel)
    {
        if (profile.SoftenTerminalTBeforeSuffix && word[^1] == 't')
        {
            word = StringHumanizeExtensions.Concat(word.AsSpan(0, word.Length - 1), 'd');
        }

        if (suffixFoundOnLastVowel && profile.DropTerminalVowelBeforeHarmonySuffix)
        {
            word = word[..^1];
        }

        return word;
    }
}

// Carries the generated lexicon plus the ordinal strategy-specific data for each locale.
sealed class TurkicNumberToWordsProfile(
    long minimumValue,
    long maximumValue,
    string minusWord,
    string hundredWord,
    TurkicHundredStrategy hundredStrategy,
    string[] unitsMap,
    string[] tensMap,
    TurkicScale[] scales,
    TurkicOrdinalSuffixStrategy ordinalSuffixStrategy,
    bool softenTerminalTBeforeSuffix,
    bool dropTerminalVowelBeforeHarmonySuffix,
    FrozenDictionary<char, string>? ordinalSuffixes = null,
    string? secondOrdinalSuffixCharacters = null,
    string[]? ordinalSuffixPair = null,
    FrozenDictionary<char, string>? tupleSuffixes = null,
    FrozenDictionary<int, string>? namedTuples = null)
{
    public long MinimumValue { get; } = minimumValue;
    public long MaximumValue { get; } = maximumValue;
    public string MinusWord { get; } = minusWord;
    public string HundredWord { get; } = hundredWord;
    public TurkicHundredStrategy HundredStrategy { get; } = hundredStrategy;
    public string[] UnitsMap { get; } = unitsMap;
    public string[] TensMap { get; } = tensMap;
    public TurkicScale[] Scales { get; } = scales;
    public TurkicOrdinalSuffixStrategy OrdinalSuffixStrategy { get; } = ordinalSuffixStrategy;
    public bool SoftenTerminalTBeforeSuffix { get; } = softenTerminalTBeforeSuffix;
    public bool DropTerminalVowelBeforeHarmonySuffix { get; } = dropTerminalVowelBeforeHarmonySuffix;
    public FrozenDictionary<char, string>? OrdinalSuffixes { get; } = ordinalSuffixes;
    public string? SecondOrdinalSuffixCharacters { get; } = secondOrdinalSuffixCharacters;
    public string[] OrdinalSuffixPair { get; } = ordinalSuffixPair ?? Array.Empty<string>();
    public FrozenDictionary<char, string>? TupleSuffixes { get; } = tupleSuffixes;
    public FrozenDictionary<int, string>? NamedTuples { get; } = namedTuples;
}

// Scale metadata stays simple for this family: value, printed word, singular omission, and whether scale counts may use bare "hundred".
readonly record struct TurkicScale(
    long Value,
    string Name,
    bool OmitOneWhenSingular = false,
    bool AllowBareHundredInCount = false);

// Uzbek keeps "bir yuz" inside composite numbers while Turkish/Azerbaijani omit the leading one for hundreds.
enum TurkicHundredStrategy
{
    OmitOneWhenSingular,
    AllowExplicitOneInComposite
}

// Some Turkic locales pick suffixes by last vowel; Uzbek variants split between two suffixes by final character.
enum TurkicOrdinalSuffixStrategy
{
    LastVowelMap,
    FinalCharacterMembership
}

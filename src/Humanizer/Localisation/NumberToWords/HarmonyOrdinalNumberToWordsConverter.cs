namespace Humanizer;

/// <summary>
/// Shared decimal-scale renderer for Turkic-style systems where the cardinal decomposition is
/// mostly regular but ordinal and tuple forms depend on harmony-driven suffix selection.
///
/// The algorithm is:
/// - decompose the number by descending scales, then hundreds, tens, and units
/// - let the generated profile decide whether exact hundreds keep an explicit leading one
/// - append ordinal or tuple suffixes through either last-vowel harmony or final-character membership
///
/// The expected result is a natural Turkic-family cardinal, ordinal, or tuple string whose suffix
/// behavior comes from generated data rather than locale-specific code paths.
/// </summary>
class HarmonyOrdinalNumberToWordsConverter(HarmonyOrdinalNumberToWordsProfile profile) : GenderlessNumberToWordsConverter
{
    /// <summary>
    /// Immutable generated profile that owns the decimal-scale lexicon and harmony rules.
    /// </summary>
    readonly HarmonyOrdinalNumberToWordsProfile profile = profile;

    public override string Convert(long input)
    {
        if (input > profile.MaximumValue || input < profile.MinimumValue)
        {
            throw new NotImplementedException();
        }

        return ConvertCore(input, allowExactHundredWord: true);
    }

    // Composite counts recurse through the same decimal-scale engine; only the generated
    // hundred/ordinal strategies vary per locale.
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
            profile.HundredStrategy == HarmonyOrdinalHundredStrategy.AllowExplicitOneInComposite &&
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

    // Exact hundred behavior is one of the main family pivots: some locales keep the explicit
    // leading one in composite hundreds, others collapse it away.
    string FormatHundreds(long hundred)
    {
        if (hundred > 1)
        {
            return $"{ConvertCore(hundred, allowExactHundredWord: false)} {profile.HundredWord}";
        }

        return profile.HundredStrategy == HarmonyOrdinalHundredStrategy.AllowExplicitOneInComposite
            ? $"{profile.UnitsMap[1]} {profile.HundredWord}"
            : profile.HundredWord;
    }

    public override string ConvertToOrdinal(int number)
    {
        var word = Convert(number);
        return profile.OrdinalSuffixStrategy switch
        {
            HarmonyOrdinalSuffixStrategy.LastVowelMap => AppendHarmonySuffix(word, profile.OrdinalSuffixes),
            HarmonyOrdinalSuffixStrategy.FinalCharacterMembership => AppendMembershipOrdinalSuffix(
                word,
                profile.SecondOrdinalSuffixCharacters,
                profile.OrdinalSuffixPair),
            _ => throw new InvalidOperationException("Unsupported harmony-ordinal suffix strategy.")
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

    // Vowel-harmony ordinals are the common family rule. The runtime searches backward for the
    // last vowel with a configured suffix, then applies any configured stem adjustments first.
    string AppendHarmonySuffix(string word, FrozenDictionary<char, string>? suffixes)
    {
        if (suffixes is null)
        {
            throw new InvalidOperationException("Harmony-ordinal suffix data is missing.");
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

    // Some locales split between two ordinal suffixes by final-character membership rather than by
    // the last vowel. That alternative stays structural because the deciding character set and the
    // suffix pair still come from generated data.
    static string AppendMembershipOrdinalSuffix(string word, string? secondOrdinalSuffixCharacters, string[] ordinalSuffixPair)
    {
        if (string.IsNullOrEmpty(secondOrdinalSuffixCharacters) || ordinalSuffixPair.Length != 2)
        {
            throw new InvalidOperationException("Harmony-ordinal membership suffix data is incomplete.");
        }

        var suffixIndex = secondOrdinalSuffixCharacters.Contains(word[^1]) ? 1 : 0;
        return word + ordinalSuffixPair[suffixIndex];
    }

    // Harmony adjustments operate on the already-rendered cardinal word so the family can model
    // softening and terminal-vowel dropping without duplicating the full cardinal renderer.
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

/// <summary>
/// Immutable generated profile for <see cref="HarmonyOrdinalNumberToWordsConverter"/>.
/// </summary>
sealed class HarmonyOrdinalNumberToWordsProfile(
    long minimumValue,
    long maximumValue,
    string minusWord,
    string hundredWord,
    HarmonyOrdinalHundredStrategy hundredStrategy,
    string[] unitsMap,
    string[] tensMap,
    HarmonyOrdinalScale[] scales,
    HarmonyOrdinalSuffixStrategy ordinalSuffixStrategy,
    bool softenTerminalTBeforeSuffix,
    bool dropTerminalVowelBeforeHarmonySuffix,
    FrozenDictionary<char, string>? ordinalSuffixes = null,
    string? secondOrdinalSuffixCharacters = null,
    string[]? ordinalSuffixPair = null,
    FrozenDictionary<char, string>? tupleSuffixes = null,
    FrozenDictionary<int, string>? namedTuples = null)
{
    /// <summary>Gets the inclusive lower bound supported by the generated profile.</summary>
    public long MinimumValue { get; } = minimumValue;
    /// <summary>Gets the inclusive upper bound supported by the generated profile.</summary>
    public long MaximumValue { get; } = maximumValue;
    /// <summary>Gets the word used to prefix negative values.</summary>
    public string MinusWord { get; } = minusWord;
    /// <summary>Gets the hundred word.</summary>
    public string HundredWord { get; } = hundredWord;
    /// <summary>Gets the hundred-rendering strategy for the locale.</summary>
    public HarmonyOrdinalHundredStrategy HundredStrategy { get; } = hundredStrategy;
    /// <summary>Gets the units lexicon.</summary>
    public string[] UnitsMap { get; } = unitsMap;
    /// <summary>Gets the tens lexicon.</summary>
    public string[] TensMap { get; } = tensMap;
    /// <summary>Gets the descending scale rows used during decomposition.</summary>
    public HarmonyOrdinalScale[] Scales { get; } = scales;
    /// <summary>Gets the ordinal suffix-selection strategy for the locale.</summary>
    public HarmonyOrdinalSuffixStrategy OrdinalSuffixStrategy { get; } = ordinalSuffixStrategy;
    /// <summary>Gets a value indicating whether terminal <c>t</c> should soften before suffixing.</summary>
    public bool SoftenTerminalTBeforeSuffix { get; } = softenTerminalTBeforeSuffix;
    /// <summary>Gets a value indicating whether a terminal vowel should drop before the harmony suffix.</summary>
    public bool DropTerminalVowelBeforeHarmonySuffix { get; } = dropTerminalVowelBeforeHarmonySuffix;
    /// <summary>Gets the last-vowel suffix map used by harmony-driven ordinals.</summary>
    public FrozenDictionary<char, string>? OrdinalSuffixes { get; } = ordinalSuffixes;
    /// <summary>Gets the character set that selects the second ordinal suffix in membership mode.</summary>
    public string? SecondOrdinalSuffixCharacters { get; } = secondOrdinalSuffixCharacters;
    /// <summary>Gets the pair of ordinal suffixes used by membership-based suffix selection.</summary>
    public string[] OrdinalSuffixPair { get; } = ordinalSuffixPair ?? Array.Empty<string>();
    /// <summary>Gets the optional tuple suffix map used for harmony-driven tuple forms.</summary>
    public FrozenDictionary<char, string>? TupleSuffixes { get; } = tupleSuffixes;
    /// <summary>Gets any exact tuple names that bypass the harmony suffix path.</summary>
    public FrozenDictionary<int, string>? NamedTuples { get; } = namedTuples;
}

/// <summary>
/// One descending scale row for <see cref="HarmonyOrdinalNumberToWordsConverter"/>.
/// </summary>
readonly record struct HarmonyOrdinalScale(
    long Value,
    string Name,
    bool OmitOneWhenSingular = false,
    bool AllowBareHundredInCount = false);

/// <summary>
/// Describes whether exact and composite hundreds keep an explicit leading one.
/// </summary>
enum HarmonyOrdinalHundredStrategy
{
    OmitOneWhenSingular,
    AllowExplicitOneInComposite
}

/// <summary>
/// Describes how the ordinal suffix is selected for the locale.
/// </summary>
enum HarmonyOrdinalSuffixStrategy
{
    LastVowelMap,
    FinalCharacterMembership
}

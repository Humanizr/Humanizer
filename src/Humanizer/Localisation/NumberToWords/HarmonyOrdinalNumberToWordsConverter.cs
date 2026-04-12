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

    /// <summary>
    /// Converts the given value using the locale's harmony-based cardinal rules.
    /// </summary>
    /// <param name="input">The number to convert.</param>
    /// <returns>The localized cardinal words for <paramref name="input"/>.</returns>
    public override string Convert(long input)
    {
        if (input > profile.MaximumValue || input < profile.MinimumValue)
        {
            throw new NotImplementedException();
        }

        // The cardinal path shares one recursive engine for all supported magnitudes; the
        // profile only constrains the legal range and the exact hundred behavior.
        return ConvertCore(input, allowExactHundredWord: true);
    }

    // Composite counts recurse through the same decimal-scale engine; only the generated
    // hundred/ordinal strategies vary per locale.
    /// <summary>
    /// Converts a number using the shared decimal decomposition while honoring the requested
    /// exact-hundred behavior.
    /// </summary>
    string ConvertCore(long input, bool allowExactHundredWord)
    {
        var number = input;
        if (number == 0)
        {
            return profile.UnitsMap[0];
        }

        if (number < 0)
        {
            // Keep the sign outside the recursive decomposition so harmony rules apply to the
            // magnitude alone.
            return $"{profile.MinusWord} {ConvertCore(-number, allowExactHundredWord)}";
        }

        if (allowExactHundredWord &&
            profile.HundredStrategy == HarmonyOrdinalHundredStrategy.AllowExplicitOneInComposite &&
            number == 100)
        {
            // Some locales keep an explicit one in exact hundreds only when the hundred stands in a
            // larger composite.
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

            // Scale counts recurse back into the same engine so each row can decide whether it
            // suppresses an explicit one or needs the bare hundred form.
            parts.Add(scale.OmitOneWhenSingular && count == 1
                ? scale.Name
                : $"{ConvertCore(count, scale.AllowBareHundredInCount)} {scale.Name}");
            number %= scale.Value;
        }

        var hundred = number / 100;
        if (hundred > 0)
        {
            // Hundreds are the last scale boundary before tens and units; the locale decides whether
            // the leading one is spoken or omitted.
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
    /// <summary>
    /// Renders the hundreds portion according to the locale's exact-hundred strategy.
    /// </summary>
    string FormatHundreds(long hundred)
    {
        if (hundred > 1)
        {
            // Larger hundreds recurse so the same exact-hundred rule can be applied consistently.
            return $"{ConvertCore(hundred, allowExactHundredWord: false)} {profile.HundredWord}";
        }

        // The one-hundred case is the only place where the profile's exact-hundred strategy can
        // preserve or suppress the leading one.
        return profile.HundredStrategy == HarmonyOrdinalHundredStrategy.AllowExplicitOneInComposite
            ? $"{profile.UnitsMap[1]} {profile.HundredWord}"
            : profile.HundredWord;
    }

    /// <summary>
    /// Converts the given value using the locale's harmony-based ordinal rules.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <returns>The localized ordinal words for <paramref name="number"/>.</returns>
    public override string ConvertToOrdinal(int number)
    {
        // Ordinals are the cardinal form plus a suffix strategy chosen by the generated profile.
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

    /// <summary>
    /// Converts the given value to a tuple form, using either a named tuple or harmony suffixes.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <returns>The localized tuple words for <paramref name="number"/>.</returns>
    public override string ConvertToTuple(int number)
    {
        // Exact tuple names win first; only unknown tuples fall back to harmony-suffix rendering.
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
    /// <summary>
    /// Appends the harmony suffix selected by the last vowel in the word.
    /// </summary>
    string AppendHarmonySuffix(string word, FrozenDictionary<char, string>? suffixes)
    {
        if (suffixes is null)
        {
            throw new InvalidOperationException("Harmony-ordinal suffix data is missing.");
        }

        // Walk backward so the suffix is chosen from the last vowel rather than from the first
        // vowel or from a locale-specific hardcoded branch.
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
    /// <summary>
    /// Appends the ordinal suffix selected by final-character membership.
    /// </summary>
    static string AppendMembershipOrdinalSuffix(string word, string? secondOrdinalSuffixCharacters, string[] ordinalSuffixPair)
    {
        if (string.IsNullOrEmpty(secondOrdinalSuffixCharacters) || ordinalSuffixPair.Length != 2)
        {
            throw new InvalidOperationException("Harmony-ordinal membership suffix data is incomplete.");
        }

        // Membership-based locales still keep the suffix choice data-driven; the final character
        // set selects which of the two generated suffixes should be used.
        var suffixIndex = secondOrdinalSuffixCharacters.Contains(word[^1]) ? 1 : 0;
        return word + ordinalSuffixPair[suffixIndex];
    }

    // Harmony adjustments operate on the already-rendered cardinal word so the family can model
    // softening and terminal-vowel dropping without duplicating the full cardinal renderer.
    /// <summary>
    /// Applies the locale's stem adjustments before a harmony suffix is appended.
    /// </summary>
    string ApplyHarmonyStemAdjustments(string word, bool suffixFoundOnLastVowel)
    {
        // The stem edits happen before the suffix is appended so the same base word can support
        // softening and terminal-vowel loss without duplicating the cardinal renderer.
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
    /// <summary>
    /// Omits the leading one for exact hundreds and composite hundreds when possible.
    /// </summary>
    OmitOneWhenSingular,
    /// <summary>
    /// Keeps an explicit leading one in composite hundreds when the locale requires it.
    /// </summary>
    AllowExplicitOneInComposite
}

/// <summary>
/// Describes how the ordinal suffix is selected for the locale.
/// </summary>
enum HarmonyOrdinalSuffixStrategy
{
    /// <summary>
    /// Chooses the ordinal suffix from the last vowel in the rendered word.
    /// </summary>
    LastVowelMap,
    /// <summary>
    /// Chooses the ordinal suffix from the final character's membership in a configured set.
    /// </summary>
    FinalCharacterMembership
}
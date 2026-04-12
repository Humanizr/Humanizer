namespace Humanizer;

/// <summary>
/// Shared decimal renderer for locales that form words from contextual decimal pieces rather than
/// from a single digit table.
///
/// The generated profile controls scale names, digit substitutions, and ordinal overrides while the
/// runtime keeps the recursive decimal decomposition stable.
/// </summary>
class ContextualDecimalNumberToWordsConverter(ContextualDecimalNumberToWordsProfile profile) : GenderlessNumberToWordsConverter
{
    readonly ContextualDecimalNumberToWordsProfile profile = profile;

    /// <summary>
    /// Converts the given value into its localized cardinal form.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <returns>The localized cardinal words for <paramref name="number"/>.</returns>
    public override string Convert(long number) =>
        number == 0
            ? profile.ZeroWord
            : ConvertCardinal(number);

    /// <summary>
    /// Converts the given value into its localized ordinal form.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <returns>The localized ordinal words for <paramref name="number"/>.</returns>
    public override string ConvertToOrdinal(int number) =>
        profile.OrdinalPrefix + ConvertOrdinalBody(number);

    string ConvertCardinal(long number, bool hasTens = false, bool hasHigherGroup = false, bool usePostTensUnitExceptions = false)
    {
        if (number < 0)
        {
            // Preserve the sign separately so the positive path can stay focused on the locale's
            // recursive decimal syntax.
            return profile.MinusWord + ConvertCardinal(-number, hasTens, hasHigherGroup, usePostTensUnitExceptions);
        }

        foreach (var scale in profile.Scales)
        {
            if (number < scale.Value)
            {
                continue;
            }

            // Scale rows recurse first so the remainder can decide whether it needs an explicit
            // zero-tens form or a post-tens unit override.
            return $"{ConvertCardinal(number / scale.Value)} {scale.Name} {ConvertCardinal(number % scale.Value, hasHigherGroup: true)}".TrimEnd();
        }

        if (number >= 20)
        {
            // Tens are rendered before the unit; some locales need a different unit form after the
            // tens word, so the recursive call carries that state forward.
            return $"{profile.DigitWords[(int)(number / 10)]} {profile.TensWord} {ConvertCardinal(number % 10, hasTens: true, usePostTensUnitExceptions: true)}".TrimEnd();
        }

        if (number >= 10)
        {
            // Teen rendering is its own branch because some locales replace the unit with a teen
            // override while others keep the ten word plus a regular unit.
            if (profile.TeenUnitExceptions.TryGetValue((int)(number % 10), out var teenExceptionWord))
            {
                return $"{profile.TenWord} {teenExceptionWord}";
            }

            return number == 10
                ? profile.TenWord
                : $"{profile.TenWord} {ConvertCardinal(number % 10, hasTens: true, usePostTensUnitExceptions: false)}".TrimEnd();
        }

        if (hasTens && usePostTensUnitExceptions && profile.PostTensUnitExceptions.TryGetValue((int)number, out var postTensUnit))
        {
            // Post-tens overrides only apply after we already emitted a tens word; that keeps the
            // ordinary unit table available for the base recursive call.
            return postTensUnit;
        }

        if (number > 0 && hasHigherGroup && !hasTens)
        {
            // Some higher-group remainders require an explicit "zero tens" bridge before the digit.
            return $"{profile.ZeroTensWord} {profile.DigitWords[(int)number]}";
        }

        return profile.DigitWords[(int)number];
    }

    string ConvertOrdinalBody(int number)
    {
        // Ordinals are mostly the cardinal form plus a thin exact-value override table.
        if (profile.ExactOrdinals.TryGetValue(number, out var ordinalUnit))
        {
            return ordinalUnit;
        }

        return Convert(number);
    }
}

/// <summary>
/// Immutable generated profile for <see cref="ContextualDecimalNumberToWordsConverter"/>.
/// </summary>
sealed class ContextualDecimalNumberToWordsProfile(
    string zeroWord,
    string minusWord,
    string ordinalPrefix,
    string tenWord,
    string tensWord,
    string zeroTensWord,
    string[] digitWords,
    ContextualDecimalScale[] scales,
    FrozenDictionary<int, string> teenUnitExceptions,
    FrozenDictionary<int, string> postTensUnitExceptions,
    FrozenDictionary<int, string> exactOrdinals)
{
    /// <summary>
    /// Gets the cardinal zero word.
    /// </summary>
    public string ZeroWord { get; } = zeroWord;
    /// <summary>
    /// Gets the word used to prefix negative values.
    /// </summary>
    public string MinusWord { get; } = minusWord;
    /// <summary>
    /// Gets the ordinal prefix that is prepended to all ordinals.
    /// </summary>
    public string OrdinalPrefix { get; } = ordinalPrefix;
    /// <summary>
    /// Gets the word used for the digit ten.
    /// </summary>
    public string TenWord { get; } = tenWord;
    /// <summary>
    /// Gets the word used when combining multiple tens.
    /// </summary>
    public string TensWord { get; } = tensWord;
    /// <summary>
    /// Gets the word used when a higher group forces an explicit zero-tens form.
    /// </summary>
    public string ZeroTensWord { get; } = zeroTensWord;
    /// <summary>
    /// Gets the digit lexicon.
    /// </summary>
    public string[] DigitWords { get; } = digitWords;
    /// <summary>
    /// Gets the descending scale rows used during decomposition.
    /// </summary>
    public ContextualDecimalScale[] Scales { get; } = scales;
    /// <summary>
    /// Gets teen-specific unit exceptions.
    /// </summary>
    public FrozenDictionary<int, string> TeenUnitExceptions { get; } = teenUnitExceptions;
    /// <summary>
    /// Gets unit exceptions that apply immediately after a tens word.
    /// </summary>
    public FrozenDictionary<int, string> PostTensUnitExceptions { get; } = postTensUnitExceptions;
    /// <summary>
    /// Gets exact ordinal words for irregular unit values.
    /// </summary>
    public FrozenDictionary<int, string> ExactOrdinals { get; } = exactOrdinals;
}

/// <summary>
/// One descending decimal scale row for <see cref="ContextualDecimalNumberToWordsConverter"/>.
/// </summary>
readonly record struct ContextualDecimalScale(long Value, string Name);
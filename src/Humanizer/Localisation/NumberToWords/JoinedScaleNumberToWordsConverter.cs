namespace Humanizer;

/// <summary>
/// Renders locales that join scale words with explicit separators instead of mutating the scale
/// stems themselves.
/// </summary>
/// <remarks>
/// The converter validates the supported magnitude up front, then walks the scale table from the
/// largest value to the smallest and joins the resulting fragments with the generated separator
/// rules.
/// </remarks>
class JoinedScaleNumberToWordsConverter(JoinedScaleNumberToWordsProfile profile) : GenderlessNumberToWordsConverter
{
    readonly JoinedScaleNumberToWordsProfile profile = profile;

    /// <inheritdoc/>
    public override string Convert(long number)
    {
        // `long.MinValue` is represented as one extra magnitude slot when the profile explicitly
        // allows it; everything else is bounded by the generated profile's declared ceiling.
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

    /// <summary>
    /// Converts the non-negative portion of a value after magnitude validation.
    /// </summary>
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

            // Singular scale names may omit the leading one, but only when the locale says that
            // is grammatical. The "first visible scale" check preserves languages where the
            // omission changes once the value is already inside a larger compound.
            parts.Add(scale.OmitOneWhenSingular && count == 1 && (profile.OmitOneWhenSingularAlways || parts.Count == 0)
                ? scale.Name
                : $"{Convert((long)count)}{profile.ScaleCountJoinWord}{scale.Name}");
            remainder %= scaleValue;
        }

        // The profile can supply a dedicated hundreds lexicon or a direct sub-hundred lexicon.
        // Keeping both paths avoids forcing all locales into the same tens/units stitching rule.
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

    /// <inheritdoc/>
    public override string ConvertToOrdinal(int number)
    {
        // Exact overrides must win before any fallback logic; otherwise compound ordinal shortcut
        // rules would rewrite locale-specific exceptions into a different surface form.
        if (profile.OrdinalExceptions is not null &&
            profile.OrdinalExceptions.TryGetValue(number, out var exactOrdinal))
        {
            return exactOrdinal;
        }

        // Some locales only use a compound ordinal for a specific trailing digit. The exclusion
        // set protects hand-authored exceptions from being dragged into that shortcut.
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

    /// <summary>
    /// Converts the segment below one hundred, falling back to a generated tens/units joiner when needed.
    /// </summary>
    string ConvertUnderHundred(int number)
    {
        if (profile.SubHundredMap.Length != 0)
        {
            return profile.SubHundredMap[number];
        }

        if (number >= 20)
        {
            // Locale authors can choose whether tens and units are already fused or must be
            // stitched at runtime; the fallback joiner only applies when no fused form exists.
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

/// <summary>
/// Immutable generated profile for <see cref="JoinedScaleNumberToWordsConverter"/>.
/// </summary>
/// <param name="maximumValue">The maximum supported absolute value.</param>
/// <param name="zeroWord">The word used for zero.</param>
/// <param name="minusWord">The word used to prefix negative values.</param>
/// <param name="negativeJoinWord">The joiner used between the minus word and the positive phrase.</param>
/// <param name="joinWord">The join word used between a scale count and its scale name.</param>
/// <param name="scaleCountJoinWord">The joiner used when rendering a scale count with its scale name.</param>
/// <param name="underHundredJoinWord">The joiner used inside under-hundred fragments.</param>
/// <param name="omitOneWhenSingularAlways">Whether singular scales omit the leading one in every context.</param>
/// <param name="defaultOrdinalSuffix">The default ordinal suffix.</param>
/// <param name="matchingOrdinalSuffix">An ordinal suffix that should be preserved when already present.</param>
/// <param name="ordinalSuffixMatchCharacters">The characters that identify a matching ordinal suffix.</param>
/// <param name="unitsMap">The unit lexicon.</param>
/// <param name="tensMap">The tens lexicon.</param>
/// <param name="hundredsMap">The hundreds lexicon.</param>
/// <param name="subHundredMap">The shared under-hundred lexicon.</param>
/// <param name="scales">The descending scale rows used during decomposition.</param>
/// <param name="ordinalExceptions">Exact ordinal overrides keyed by value.</param>
/// <param name="compoundOrdinalRemainder">The trailing digit used by compound ordinal shortcut rules.</param>
/// <param name="compoundOrdinalWord">The word used by compound ordinal shortcut rules.</param>
/// <param name="compoundOrdinalExcludedValues">Values that must not use the compound ordinal shortcut.</param>
internal sealed class JoinedScaleNumberToWordsProfile(
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
    /// <summary>Gets the maximum supported absolute value.</summary>
    public long MaximumValue { get; } = maximumValue;
    /// <summary>Gets the word used for zero.</summary>
    public string ZeroWord { get; } = zeroWord;
    /// <summary>Gets the word used to prefix negative values.</summary>
    public string MinusWord { get; } = minusWord;
    /// <summary>Gets the joiner used between the minus word and the positive phrase.</summary>
    public string NegativeJoinWord { get; } = negativeJoinWord;
    /// <summary>Gets the joiner used between rendered phrase fragments.</summary>
    public string JoinWord { get; } = joinWord;
    /// <summary>Gets the joiner inserted between a rendered count and its scale name.</summary>
    public string ScaleCountJoinWord { get; } = scaleCountJoinWord;
    /// <summary>Gets the joiner used when a fallback under-hundred phrase must stitch tens and units.</summary>
    public string UnderHundredJoinWord { get; } = underHundredJoinWord;
    /// <summary>Gets a value indicating whether singular scales always omit an explicit leading one.</summary>
    public bool OmitOneWhenSingularAlways { get; } = omitOneWhenSingularAlways;
    /// <summary>Gets the default ordinal suffix.</summary>
    public string DefaultOrdinalSuffix { get; } = defaultOrdinalSuffix;
    /// <summary>Gets the alternate ordinal suffix used when the rendered word already matches the special pattern.</summary>
    public string? MatchingOrdinalSuffix { get; } = matchingOrdinalSuffix;
    /// <summary>Gets the trailing characters that select <see cref="MatchingOrdinalSuffix"/>.</summary>
    public string? OrdinalSuffixMatchCharacters { get; } = ordinalSuffixMatchCharacters;
    /// <summary>Gets the unit lexicon.</summary>
    public string[] UnitsMap { get; } = unitsMap;
    /// <summary>Gets the tens lexicon.</summary>
    public string[] TensMap { get; } = tensMap;
    /// <summary>Gets the hundreds lexicon.</summary>
    public string[] HundredsMap { get; } = hundredsMap;
    /// <summary>Gets the direct under-hundred lexicon used when the locale does not stitch tens and units at runtime.</summary>
    public string[] SubHundredMap { get; } = subHundredMap;
    /// <summary>Gets the descending scale rows used during decomposition.</summary>
    public JoinedScale[] Scales { get; } = scales;
    /// <summary>Gets exact ordinal overrides keyed by value.</summary>
    public FrozenDictionary<int, string>? OrdinalExceptions { get; } = ordinalExceptions;
    /// <summary>Gets the trailing digit that triggers the compound ordinal shortcut.</summary>
    public int? CompoundOrdinalRemainder { get; } = compoundOrdinalRemainder;
    /// <summary>Gets the compound ordinal word appended when the shortcut applies.</summary>
    public string? CompoundOrdinalWord { get; } = compoundOrdinalWord;
    /// <summary>Gets the values that must not use the compound ordinal shortcut.</summary>
    public FrozenSet<int> CompoundOrdinalExcludedValues { get; } = compoundOrdinalExcludedValues ?? FrozenSet<int>.Empty;
    /// <summary>Gets a value indicating whether the profile can represent <see cref="long.MinValue"/>.</summary>
    public bool AllowLongMinValue { get; } = maximumValue == long.MaxValue;
}

/// <summary>
/// One descending scale row for <see cref="JoinedScaleNumberToWordsConverter"/>.
/// </summary>
/// <param name="Value">The divisor for the scale row.</param>
/// <param name="Name">The localized scale name.</param>
/// <param name="OmitOneWhenSingular">Whether singular counts may omit the explicit one.</param>
internal readonly record struct JoinedScale(long Value, string Name, bool OmitOneWhenSingular = false);
namespace Humanizer;

/// <summary>
/// Renders locales that spell tens with the unit before the decade stem and rely on generated
/// joiner, prefix, and ordinal-suffix rules.
/// </summary>
class InvertedTensNumberToWordsConverter(InvertedTensNumberToWordsProfile profile) : GenderlessNumberToWordsConverter
{
    readonly InvertedTensNumberToWordsProfile profile = profile;

    /// <inheritdoc/>
    public override string Convert(long number)
    {
        if (number == 0)
        {
            return profile.UnitsMap[0];
        }

        if (number < 0)
        {
            return $"{profile.MinusWord} {Convert(-number)}";
        }

        var builder = new StringBuilder();
        AppendNumber(builder, number);
        return builder.ToString();
    }

    /// <inheritdoc/>
    public override string ConvertToOrdinal(int number) =>
        number == 1 && profile.UnitsMap[1] == "en" && profile.TensMap[2] == "tyve"
            ? "første"
            :
        profile.OrdinalMode switch
        {
            InvertedTensOrdinalMode.NumericString => number.ToString(CultureInfo.InvariantCulture),
            _ => ConvertWithSuffixOrdinal(number)
        };

    /// <summary>
    /// Applies the locale's suffix-based ordinal rules to the rendered cardinal form.
    /// </summary>
    string ConvertWithSuffixOrdinal(int number)
    {
        var word = Convert(number);

        if (profile.RemoveLeadingOneInOrdinal &&
            word.StartsWith(profile.LeadingOneWord, StringComparison.Ordinal) &&
            (profile.LeadingOnePreservePrefix.Length == 0 ||
             !word.StartsWith(profile.LeadingOnePreservePrefix, StringComparison.Ordinal)))
        {
            word = word[profile.LeadingOneWord.Length..];
        }

        foreach (var exception in profile.OrdinalExceptions)
        {
            if (!word.EndsWith(exception.Key, StringComparison.Ordinal))
            {
                continue;
            }

            return StringHumanizeExtensions.Concat(
                word.AsSpan(0, word.Length - exception.Key.Length),
                exception.Value.AsSpan());
        }

        return profile.OrdinalSteSuffixEndingChars.Contains(word[^1])
            ? word + profile.OrdinalSteSuffix
            : word + profile.OrdinalDefaultSuffix;
    }

    /// <summary>
    /// Appends the cardinal rendering for the given number, including higher scale rows and
    /// remainders.
    /// </summary>
    void AppendNumber(StringBuilder builder, long number)
    {
        var hasScalePrefix = false;

        foreach (var scale in profile.Scales)
        {
            var count = number / scale.Value;
            if (count == 0)
            {
                continue;
            }

            if (count == 1)
            {
                builder.Append(scale.OneForm);
            }
            else
            {
                AppendNumber(builder, count);
                builder.Append(scale.CountJoiner);
                builder.Append(scale.ManyForm);
            }

            number %= scale.Value;
            hasScalePrefix = true;

            if (number > 0)
            {
                builder.Append(scale.RemainderSeparator);
            }
        }

        if (number > 0)
        {
            AppendLessThanThousand(builder, (int)number, hasScalePrefix);
        }
    }

    /// <summary>
    /// Appends the segment below one thousand, applying any locale-specific tail prefix rules.
    /// </summary>
    void AppendLessThanThousand(StringBuilder builder, int number, bool hasScalePrefix)
    {
        if (hasScalePrefix &&
            ShouldPrefixTail(number, profile.ScaleTailPrefixMode) &&
            profile.ScaleTailPrefix.Length > 0)
        {
            builder.Append(profile.ScaleTailPrefix);
        }

        if (number >= 100)
        {
            var hundreds = number / 100;
            var remainder = number % 100;

            if (hundreds == 1)
            {
                builder.Append(profile.OneHundredWord);
            }
            else
            {
                builder.Append(profile.HundredsPrefixMap[hundreds]);
                builder.Append(profile.HundredJoiner);
                builder.Append(profile.HundredWord);
            }

            if (remainder > 0)
            {
                builder.Append(profile.HundredRemainderSeparator);

                if (ShouldPrefixTail(remainder, profile.HundredTailPrefixMode) &&
                    profile.HundredTailPrefix.Length > 0)
                {
                    builder.Append(profile.HundredTailPrefix);
                }

                AppendLessThanHundred(builder, remainder);
            }

            return;
        }

        AppendLessThanHundred(builder, number);
    }

    /// <summary>
    /// Appends the segment below one hundred, using the unit-before-tens joiner rules.
    /// </summary>
    void AppendLessThanHundred(StringBuilder builder, int number)
    {
        if (number < 20)
        {
            builder.Append(profile.UnitsMap[number]);
            return;
        }

        var tens = number / 10;
        var unit = number % 10;
        if (unit == 0)
        {
            builder.Append(profile.TensMap[tens]);
            return;
        }

        var unitWord = profile.UnitsMap[unit];
        builder.Append(unitWord);
        builder.Append(GetUnitTensJoiner(unitWord));
        builder.Append(profile.TensMap[tens]);
    }

    /// <summary>
    /// Returns the joiner that should appear between a unit word and a tens stem.
    /// </summary>
    string GetUnitTensJoiner(string unitWord) =>
        profile.UnitTensAlternateJoinerUnitEnding.Length > 0 &&
        unitWord.EndsWith(profile.UnitTensAlternateJoinerUnitEnding, StringComparison.Ordinal)
            ? profile.UnitTensAlternateJoiner
            : profile.UnitTensJoiner;

    /// <summary>
    /// Determines whether a tail segment should receive the locale's prefix.
    /// </summary>
    static bool ShouldPrefixTail(int number, InvertedTensTailPrefixMode mode) =>
        mode switch
        {
            InvertedTensTailPrefixMode.None => false,
            InvertedTensTailPrefixMode.SubHundred => number < 100,
            InvertedTensTailPrefixMode.LowOrExactTens => number < 20 || number < 100 && number % 10 == 0,
            _ => throw new ArgumentOutOfRangeException(nameof(mode))
        };
}

/// <summary>
/// Immutable generated profile for <see cref="InvertedTensNumberToWordsConverter"/>.
/// </summary>
/// <param name="minusWord">The word used to prefix negative values.</param>
/// <param name="unitsMap">The unit lexicon.</param>
/// <param name="tensMap">The tens lexicon.</param>
/// <param name="hundredsPrefixMap">The prefixes used before the hundred stem.</param>
/// <param name="oneHundredWord">The standalone word for one hundred.</param>
/// <param name="hundredWord">The hundred stem used after a prefix.</param>
/// <param name="hundredJoiner">The joiner between a hundreds prefix and the hundred stem.</param>
/// <param name="hundredRemainderSeparator">The separator before the remainder after hundreds.</param>
/// <param name="scaleTailPrefix">The prefix inserted before a tail segment after a scale.</param>
/// <param name="scaleTailPrefixMode">The rule that decides when the scale tail prefix applies.</param>
/// <param name="hundredTailPrefix">The prefix inserted before a tail segment after hundreds.</param>
/// <param name="hundredTailPrefixMode">The rule that decides when the hundred tail prefix applies.</param>
/// <param name="unitTensJoiner">The joiner inserted between a unit word and a tens stem.</param>
/// <param name="unitTensAlternateJoinerUnitEnding">The unit ending that selects the alternate joiner.</param>
/// <param name="unitTensAlternateJoiner">The alternate joiner used for specific unit endings.</param>
/// <param name="scales">The descending scale rows used during decomposition.</param>
/// <param name="ordinalMode">The ordinal rendering mode for the locale.</param>
/// <param name="ordinalExceptions">Exact ordinal overrides keyed by surface form suffix.</param>
/// <param name="ordinalSteSuffixEndingChars">The trailing characters that select the stem suffix.</param>
/// <param name="ordinalSteSuffix">The ordinal stem suffix.</param>
/// <param name="ordinalDefaultSuffix">The default ordinal suffix.</param>
/// <param name="removeLeadingOneInOrdinal">Whether ordinal rendering may drop the leading one.</param>
/// <param name="leadingOneWord">The leading-one word to remove when the rule applies.</param>
/// <param name="leadingOnePreservePrefix">A prefix that must be preserved when removing the leading one.</param>
internal sealed class InvertedTensNumberToWordsProfile(
    string minusWord,
    string[] unitsMap,
    string[] tensMap,
    string[] hundredsPrefixMap,
    string oneHundredWord,
    string hundredWord,
    string hundredJoiner,
    string hundredRemainderSeparator,
    string scaleTailPrefix,
    InvertedTensTailPrefixMode scaleTailPrefixMode,
    string hundredTailPrefix,
    InvertedTensTailPrefixMode hundredTailPrefixMode,
    string unitTensJoiner,
    string unitTensAlternateJoinerUnitEnding,
    string unitTensAlternateJoiner,
    InvertedTensScale[] scales,
    InvertedTensOrdinalMode ordinalMode,
    FrozenDictionary<string, string> ordinalExceptions,
    string ordinalSteSuffixEndingChars,
    string ordinalSteSuffix,
    string ordinalDefaultSuffix,
    bool removeLeadingOneInOrdinal,
    string leadingOneWord,
    string leadingOnePreservePrefix)
{
    /// <summary>Gets the word used to prefix negative values.</summary>
    public string MinusWord { get; } = minusWord;
    /// <summary>Gets the unit lexicon used for exact values and compound tails.</summary>
    public string[] UnitsMap { get; } = unitsMap;
    /// <summary>Gets the decade stems used for exact tens and unit-plus-tens compounds.</summary>
    public string[] TensMap { get; } = tensMap;
    /// <summary>Gets the prefixes that appear before the hundred stem for 200-900.</summary>
    public string[] HundredsPrefixMap { get; } = hundredsPrefixMap;
    /// <summary>Gets the standalone word for one hundred.</summary>
    public string OneHundredWord { get; } = oneHundredWord;
    /// <summary>Gets the hundred stem used after a hundreds prefix.</summary>
    public string HundredWord { get; } = hundredWord;
    /// <summary>Gets the joiner inserted between a hundreds prefix and the hundred stem.</summary>
    public string HundredJoiner { get; } = hundredJoiner;
    /// <summary>Gets the separator inserted before the remainder after a hundreds segment.</summary>
    public string HundredRemainderSeparator { get; } = hundredRemainderSeparator;
    /// <summary>Gets the prefix inserted before a tail segment that follows a large scale.</summary>
    public string ScaleTailPrefix { get; } = scaleTailPrefix;
    /// <summary>Gets the rule that decides when <see cref="ScaleTailPrefix"/> applies.</summary>
    public InvertedTensTailPrefixMode ScaleTailPrefixMode { get; } = scaleTailPrefixMode;
    /// <summary>Gets the prefix inserted before a tail segment that follows a hundreds segment.</summary>
    public string HundredTailPrefix { get; } = hundredTailPrefix;
    /// <summary>Gets the rule that decides when <see cref="HundredTailPrefix"/> applies.</summary>
    public InvertedTensTailPrefixMode HundredTailPrefixMode { get; } = hundredTailPrefixMode;
    /// <summary>Gets the default joiner inserted between a unit word and a tens stem.</summary>
    public string UnitTensJoiner { get; } = unitTensJoiner;
    /// <summary>Gets the unit ending that selects the alternate unit-to-tens joiner.</summary>
    public string UnitTensAlternateJoinerUnitEnding { get; } = unitTensAlternateJoinerUnitEnding;
    /// <summary>Gets the alternate joiner used when the unit ends with <see cref="UnitTensAlternateJoinerUnitEnding"/>.</summary>
    public string UnitTensAlternateJoiner { get; } = unitTensAlternateJoiner;
    /// <summary>Gets the descending scale rows used during decomposition.</summary>
    public InvertedTensScale[] Scales { get; } = scales;
    /// <summary>Gets the ordinal rendering strategy used by the locale.</summary>
    public InvertedTensOrdinalMode OrdinalMode { get; } = ordinalMode;
    /// <summary>Gets suffix-based ordinal overrides keyed by the rendered cardinal ending.</summary>
    public FrozenDictionary<string, string> OrdinalExceptions { get; } = ordinalExceptions;
    /// <summary>Gets the trailing characters that select the special ordinal stem suffix.</summary>
    public string OrdinalSteSuffixEndingChars { get; } = ordinalSteSuffixEndingChars;
    /// <summary>Gets the ordinal suffix used when the rendered word ends with a matching character.</summary>
    public string OrdinalSteSuffix { get; } = ordinalSteSuffix;
    /// <summary>Gets the fallback ordinal suffix for words that do not match the special stem rule.</summary>
    public string OrdinalDefaultSuffix { get; } = ordinalDefaultSuffix;
    /// <summary>Gets a value indicating whether ordinal rendering may drop an explicit leading one.</summary>
    public bool RemoveLeadingOneInOrdinal { get; } = removeLeadingOneInOrdinal;
    /// <summary>Gets the leading-one word that may be removed during ordinal rendering.</summary>
    public string LeadingOneWord { get; } = leadingOneWord;
    /// <summary>Gets a prefix that prevents the leading-one removal rule from applying.</summary>
    public string LeadingOnePreservePrefix { get; } = leadingOnePreservePrefix;
}

/// <summary>
/// One descending scale row for <see cref="InvertedTensNumberToWordsConverter"/>.
/// </summary>
/// <param name="Value">The divisor for the scale row.</param>
/// <param name="OneForm">The singular form used when the count is one.</param>
/// <param name="ManyForm">The plural form used when the count is greater than one.</param>
/// <param name="CountJoiner">The joiner inserted between the count and the scale name.</param>
/// <param name="RemainderSeparator">The separator inserted before the remainder below the scale.</param>
internal readonly record struct InvertedTensScale(long Value, string OneForm, string ManyForm, string CountJoiner, string RemainderSeparator);

/// <summary>
/// Describes how ordinals are produced for the inverted-tens family.
/// </summary>
internal enum InvertedTensOrdinalMode
{
    /// <summary>Returns ordinals as invariant numeric strings.</summary>
    NumericString,
    /// <summary>Builds ordinals from the rendered word form and suffix rules.</summary>
    Suffix
}

/// <summary>
/// Describes when a tail segment should receive a locale-specific prefix.
/// </summary>
internal enum InvertedTensTailPrefixMode
{
    /// <summary>Never prefixes the tail segment.</summary>
    None,
    /// <summary>Prefixes only tail segments below one hundred.</summary>
    SubHundred,
    /// <summary>Prefixes tail segments below twenty or exact tens below one hundred.</summary>
    LowOrExactTens
}
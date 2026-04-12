namespace Humanizer;

/// <summary>
/// Renders locales that keep scale words separate but may link the count and the scale name with
/// generated bridging rules.
/// </summary>
class LinkingScaleNumberToWordsConverter(LinkingScaleNumberToWordsProfile profile) : GenderlessNumberToWordsConverter
{
    readonly LinkingScaleNumberToWordsProfile profile = profile;

    /// <inheritdoc/>
    public override string Convert(long number)
    {
        if (number == 0)
        {
            return profile.ZeroWord;
        }

        if (number < 0)
        {
            return $"{profile.MinusWord} {Convert(-number)}";
        }

        var parts = new List<string>();
        var remainder = number;

        foreach (var scale in profile.Scales)
        {
            var part = remainder / scale.Value;
            if (part <= 0)
            {
                continue;
            }

            remainder %= scale.Value;
            parts.Add(BuildScalePart(part, scale.Name));
        }

        if (remainder > 0)
        {
            parts.Add(ConvertUnderThousand(remainder));
        }

        return string.Join(" ", parts);
    }

    /// <inheritdoc/>
    public override string ConvertToOrdinal(int number) => Convert(number);

    /// <summary>
    /// Builds the rendered count and scale name for a scale row.
    /// </summary>
    string BuildScalePart(long part, string scale)
    {
        if (part < profile.ScalePrefixes.Length && !string.IsNullOrEmpty(profile.ScalePrefixes[part]))
        {
            return $"{profile.ScalePrefixes[(int)part]} {scale}";
        }

        return $"{ApplyScaleLinking(ConvertUnderThousand(part))} {scale}";
    }

    /// <summary>
    /// Converts the fragment below one thousand.
    /// </summary>
    string ConvertUnderThousand(long number)
    {
        if (number < 100)
        {
            return ConvertUnderHundred(number);
        }

        var hundreds = number / 100;
        var remainder = number % 100;
        var parts = new List<string> { profile.HundredWords[(int)hundreds] };

        if (remainder > 0)
        {
            parts.Add($"{profile.ConjunctionWord} {ConvertUnderHundred(remainder)}");
        }

        return string.Join(" ", parts);
    }

    /// <summary>
    /// Converts the fragment below one hundred.
    /// </summary>
    string ConvertUnderHundred(long number)
    {
        if (number < profile.UnitsMap.Length)
        {
            return profile.UnitsMap[(int)number];
        }

        var tens = (int)(number / 10);
        var remainder = number % 10;
        var tensWord = profile.TensMap[tens];

        return remainder == 0
            ? tensWord
            : tensWord + profile.TensRemainderJoiner + profile.UnitsMap[(int)remainder];
    }

    /// <summary>
    /// Applies any generated scale-linking suffix replacement to the rendered count.
    /// </summary>
    string ApplyScaleLinking(string words)
    {
        foreach (var rule in profile.ScaleLinkRules)
        {
            if (words.EndsWith(rule.Suffix, StringComparison.Ordinal))
            {
                return words[..^rule.Suffix.Length] + rule.Replacement;
            }
        }

        return words;
    }
}

/// <summary>
/// Immutable generated profile for <see cref="LinkingScaleNumberToWordsConverter"/>.
/// </summary>
/// <param name="zeroWord">The word used for zero.</param>
/// <param name="minusWord">The word used to prefix negative values.</param>
/// <param name="conjunctionWord">The conjunction inserted between hundreds and their remainder.</param>
/// <param name="tensRemainderJoiner">The joiner inserted between tens and units.</param>
/// <param name="unitsMap">The cardinal unit lexicon.</param>
/// <param name="tensMap">The cardinal tens lexicon.</param>
/// <param name="hundredWords">The cardinal hundred lexicon.</param>
/// <param name="scalePrefixes">Optional prefixes for rendered scale counts.</param>
/// <param name="scales">The descending scale rows used during decomposition.</param>
/// <param name="scaleLinkRules">Suffix rewrite rules applied after rendering a scale count.</param>
sealed class LinkingScaleNumberToWordsProfile(
    string zeroWord,
    string minusWord,
    string conjunctionWord,
    string tensRemainderJoiner,
    string[] unitsMap,
    string[] tensMap,
    string[] hundredWords,
    string[] scalePrefixes,
    LinkingScale[] scales,
    LinkingSuffixRule[] scaleLinkRules)
{
    /// <summary>Gets the word used for zero.</summary>
    public string ZeroWord { get; } = zeroWord;
    /// <summary>Gets the word used to prefix negative values.</summary>
    public string MinusWord { get; } = minusWord;
    /// <summary>Gets the conjunction inserted between hundreds and their remainder.</summary>
    public string ConjunctionWord { get; } = conjunctionWord;
    /// <summary>Gets the joiner inserted between tens and units.</summary>
    public string TensRemainderJoiner { get; } = tensRemainderJoiner;
    /// <summary>Gets the cardinal unit lexicon.</summary>
    public string[] UnitsMap { get; } = unitsMap;
    /// <summary>Gets the cardinal tens lexicon.</summary>
    public string[] TensMap { get; } = tensMap;
    /// <summary>Gets the cardinal hundred lexicon.</summary>
    public string[] HundredWords { get; } = hundredWords;
    /// <summary>Gets optional prefixes that replace a rendered count for specific scale values.</summary>
    public string[] ScalePrefixes { get; } = scalePrefixes;
    /// <summary>Gets the descending scale rows used during decomposition.</summary>
    public LinkingScale[] Scales { get; } = scales;
    /// <summary>Gets the suffix rewrite rules applied after rendering a scale count.</summary>
    public LinkingSuffixRule[] ScaleLinkRules { get; } = scaleLinkRules;
}

/// <summary>
/// One descending scale row for <see cref="LinkingScaleNumberToWordsConverter"/>.
/// </summary>
/// <param name="Value">The divisor for the scale row.</param>
/// <param name="Name">The localized scale name.</param>
readonly record struct LinkingScale(long Value, string Name);

/// <summary>
/// Describes a suffix replacement rule used when a scale count needs linking changes.
/// </summary>
/// <param name="Suffix">The suffix to replace.</param>
/// <param name="Replacement">The replacement text.</param>
readonly record struct LinkingSuffixRule(string Suffix, string Replacement);
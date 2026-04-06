namespace Humanizer;

/// <summary>
/// Renders locales whose ordinal form is built by mutating only the terminal scale segment.
/// </summary>
class TerminalOrdinalScaleNumberToWordsConverter(TerminalOrdinalScaleNumberToWordsProfile profile) : GenderedNumberToWordsConverter
{
    readonly TerminalOrdinalScaleNumberToWordsProfile profile = profile;

    /// <inheritdoc/>
    public override string Convert(long input, GrammaticalGender gender, bool addAnd = true)
    {
        if (input == 0)
        {
            return profile.ZeroWord;
        }

        if (input < 0)
        {
            return profile.MinusWord + " " + Convert(-input, gender);
        }

        var parts = new List<string>(6);
        var remaining = input;

        // Cardinal rendering walks the scale table from large to small and leaves the terminal
        // under-thousand segment for the final helper so the same scale metadata can serve ordinals.
        foreach (var scale in profile.Scales)
        {
            var count = remaining / scale.Value;
            if (count == 0)
            {
                continue;
            }

            var hasRemainder = remaining % scale.Value != 0;
            parts.Add(BuildCardinalScalePart(count, hasRemainder, scale));
            remaining %= scale.Value;
        }

        AppendCardinalUnderOneThousand(parts, (int)remaining, gender, parts.Count > 0);
        return string.Join(" ", parts);
    }

    /// <inheritdoc/>
    public override string ConvertToOrdinal(int input, GrammaticalGender gender)
    {
        if (input == 0)
        {
            return profile.ZeroWord;
        }

        var parts = new List<string>(6);
        if (input < 0)
        {
            parts.Add(profile.MinusWord);
            input = -input;
        }

        var remaining = input;
        foreach (var scale in profile.Scales)
        {
            var count = remaining / scale.Value;
            if (count == 0)
            {
                continue;
            }

            var hasRemainder = remaining % (int)scale.Value != 0;
            remaining %= (int)scale.Value;
            if (remaining == 0)
            {
                // Exact scale ordinals terminate here because the current scale already became the
                // final ordinal stem; re-entering the lower helpers would double-inflect it.
                parts.Add(BuildOrdinalScalePart(count, scale, gender));
                return string.Join(" ", parts);
            }

            // As soon as a remainder survives, the current scale stays cardinal and only the tail
            // segment can switch into ordinal shape.
            parts.Add(BuildCardinalScalePart(count, hasRemainder, scale));
        }

        AppendOrdinalUnderOneThousand(parts, remaining, gender);
        return string.Join(" ", parts);
    }

    /// <summary>
    /// Builds a cardinal scale fragment.
    /// </summary>
    string BuildCardinalScalePart(long count, bool hasRemainder, TerminalOrdinalScale scale)
    {
        if (count == 1)
        {
            return hasRemainder ? scale.SingularWithRemainderCardinal : scale.ExactSingularCardinal;
        }

        return Convert(count, GrammaticalGender.Masculine) + " " + scale.PluralCardinal;
    }

    /// <summary>
    /// Builds an ordinal scale fragment.
    /// </summary>
    string BuildOrdinalScalePart(long count, TerminalOrdinalScale scale, GrammaticalGender gender) =>
        count == 1
            ? scale.OrdinalStem + GetOrdinalSuffix(gender)
            : Convert(count, GrammaticalGender.Masculine) + " " + scale.OrdinalStem + GetOrdinalSuffix(gender);

    /// <summary>
    /// Appends the cardinal fragment below one thousand.
    /// </summary>
    void AppendCardinalUnderOneThousand(List<string> parts, int number, GrammaticalGender gender, bool hasHigherScaleParts)
    {
        if (number >= 100)
        {
            var hundreds = number / 100;
            number %= 100;
            // Exact one-hundred forms differ depending on whether a larger scale already exists in
            // the phrase; the higher-scale sentinel switches between the standalone exact form and
            // the after-scale form that only makes sense inside a larger ordinal phrase.
            parts.Add(number == 0
                ? hundreds == 1
                    ? hasHigherScaleParts ? profile.ExactOneHundredAfterHigherScale : profile.ExactOneHundredCardinal
                    : Convert(hundreds, GrammaticalGender.Masculine) + " " + profile.HundredsPluralWord
                : hundreds == 1
                    ? profile.OneHundredWithRemainder
                    : Convert(hundreds, GrammaticalGender.Masculine) + " " + profile.HundredsPluralWord);
        }

        if (number >= 20)
        {
            parts.Add(profile.TensMap[number / 10]);
            number %= 10;
        }

        if (number > 0)
        {
            parts.Add(profile.UnitStems[number] + GetCardinalUnitEnding(gender, number));
        }
    }

    /// <summary>
    /// Appends the ordinal fragment below one thousand.
    /// </summary>
    void AppendOrdinalUnderOneThousand(List<string> parts, int number, GrammaticalGender gender)
    {
        if (number >= 100)
        {
            var hundreds = number / 100;
            number %= 100;
            if (number == 0)
            {
                // Exact hundreds use a dedicated ordinal stem instead of the cardinal hundred word
                // plus a suffix; otherwise the stem would be wrong for the family-specific spelling.
                parts.Add(profile.HundredsExactStems[hundreds] + GetOrdinalSuffix(gender));
                return;
            }

            parts.Add(hundreds == 1
                ? profile.OneHundredWithRemainder
                : Convert(hundreds, GrammaticalGender.Masculine) + " " + profile.HundredsPluralWord);
        }

        if (number >= 20)
        {
            var tens = number / 10;
            number %= 10;
            if (number == 0)
            {
                // Exact tens also have their own stems; the family does not build them by appending
                // a suffix to the cardinal tens form.
                parts.Add(profile.TensMap[tens] + GetOrdinalSuffix(gender));
                return;
            }

            parts.Add(profile.TensMap[tens]);
        }

        parts.Add(profile.OrdinalUnitStems[number] + GetOrdinalSuffix(gender));
    }

    /// <summary>
    /// Returns the cardinal ending for a unit stem.
    /// </summary>
    static string GetCardinalUnitEnding(GrammaticalGender gender, int number)
    {
        return NormalizeGender(gender) switch
        {
            GrammaticalGender.Masculine => number switch
            {
                1 => "s",
                < 10 when number != 3 => "i",
                _ => string.Empty
            },
            GrammaticalGender.Feminine => number switch
            {
                1 => "a",
                < 10 when number != 3 => "as",
                _ => string.Empty
            },
            _ => throw new ArgumentOutOfRangeException(nameof(gender))
        };
    }

    /// <summary>
    /// Returns the ordinal suffix for the requested gender.
    /// </summary>
    string GetOrdinalSuffix(GrammaticalGender gender) =>
        NormalizeGender(gender) switch
        {
            GrammaticalGender.Masculine => profile.MasculineOrdinalSuffix,
            GrammaticalGender.Feminine => profile.FeminineOrdinalSuffix,
            _ => throw new ArgumentOutOfRangeException(nameof(gender))
        };

    static GrammaticalGender NormalizeGender(GrammaticalGender gender) =>
        gender == GrammaticalGender.Neuter ? GrammaticalGender.Masculine : gender;
}

/// <summary>
/// Immutable generated profile that owns the terminal-ordinal lexicon and scale rows.
/// </summary>
/// <param name="zeroWord">The word used for zero.</param>
/// <param name="minusWord">The word used to prefix negative values.</param>
/// <param name="unitStems">The cardinal unit stems.</param>
/// <param name="ordinalUnitStems">The ordinal unit stems.</param>
/// <param name="tensMap">The tens lexicon.</param>
/// <param name="hundredsExactStems">The exact ordinal hundreds stems.</param>
/// <param name="exactOneHundredCardinal">The cardinal form for an exact one hundred with no higher scale.</param>
/// <param name="exactOneHundredAfterHigherScale">The cardinal form for an exact one hundred after a higher scale.</param>
/// <param name="oneHundredWithRemainder">The cardinal form for one hundred with a remainder.</param>
/// <param name="hundredsPluralWord">The plural hundred word.</param>
/// <param name="masculineOrdinalSuffix">The masculine ordinal suffix.</param>
/// <param name="feminineOrdinalSuffix">The feminine ordinal suffix.</param>
/// <param name="scales">The descending scale rows used during decomposition.</param>
internal sealed class TerminalOrdinalScaleNumberToWordsProfile(
    string zeroWord,
    string minusWord,
    string[] unitStems,
    string[] ordinalUnitStems,
    string[] tensMap,
    string[] hundredsExactStems,
    string exactOneHundredCardinal,
    string exactOneHundredAfterHigherScale,
    string oneHundredWithRemainder,
    string hundredsPluralWord,
    string masculineOrdinalSuffix,
    string feminineOrdinalSuffix,
    TerminalOrdinalScale[] scales)
{
    /// <summary>Gets the word used for zero.</summary>
    public string ZeroWord { get; } = zeroWord;
    /// <summary>Gets the word used to prefix negative values.</summary>
    public string MinusWord { get; } = minusWord;
    /// <summary>Gets the cardinal unit stems.</summary>
    public string[] UnitStems { get; } = unitStems;
    /// <summary>Gets the ordinal unit stems.</summary>
    public string[] OrdinalUnitStems { get; } = ordinalUnitStems;
    /// <summary>Gets the tens lexicon.</summary>
    public string[] TensMap { get; } = tensMap;
    /// <summary>Gets the exact ordinal hundred stems.</summary>
    public string[] HundredsExactStems { get; } = hundredsExactStems;
    /// <summary>Gets the standalone cardinal form for an exact one hundred.</summary>
    public string ExactOneHundredCardinal { get; } = exactOneHundredCardinal;
    /// <summary>Gets the cardinal form for an exact one hundred that follows a higher scale.</summary>
    public string ExactOneHundredAfterHigherScale { get; } = exactOneHundredAfterHigherScale;
    /// <summary>Gets the cardinal form for one hundred when a remainder follows.</summary>
    public string OneHundredWithRemainder { get; } = oneHundredWithRemainder;
    /// <summary>Gets the plural hundred word.</summary>
    public string HundredsPluralWord { get; } = hundredsPluralWord;
    /// <summary>Gets the masculine ordinal suffix.</summary>
    public string MasculineOrdinalSuffix { get; } = masculineOrdinalSuffix;
    /// <summary>Gets the feminine ordinal suffix.</summary>
    public string FeminineOrdinalSuffix { get; } = feminineOrdinalSuffix;
    /// <summary>Gets the descending scale rows used during decomposition.</summary>
    public TerminalOrdinalScale[] Scales { get; } = scales;
}

/// <summary>
/// One descending scale row for <see cref="TerminalOrdinalScaleNumberToWordsConverter"/>.
/// </summary>
/// <param name="Value">The divisor for the scale row.</param>
/// <param name="ExactSingularCardinal">The exact cardinal form when the scale count is one and the phrase ends here.</param>
/// <param name="SingularWithRemainderCardinal">The singular cardinal form when the scale count is one and a remainder follows.</param>
/// <param name="PluralCardinal">The plural cardinal scale name.</param>
/// <param name="OrdinalStem">The ordinal stem used for the scale row.</param>
internal readonly record struct TerminalOrdinalScale(
    long Value,
    string ExactSingularCardinal,
    string SingularWithRemainderCardinal,
    string PluralCardinal,
    string OrdinalStem);
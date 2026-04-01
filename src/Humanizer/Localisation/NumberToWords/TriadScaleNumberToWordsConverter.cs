namespace Humanizer;

/// <summary>
/// Renders locales that compose numbers in triads and use stem rewrites for ordinal forms.
/// </summary>
class TriadScaleNumberToWordsConverter(TriadScaleNumberToWordsConverter.Profile profile) : GenderedNumberToWordsConverter
{
    /// <summary>
    /// Immutable generated profile that owns the triad lexicon and ordinal rewrite rules.
    /// </summary>
    /// <param name="ZeroWord">The cardinal zero word.</param>
    /// <param name="MinusWord">The word used to prefix negative values.</param>
    /// <param name="FeminineOneWord">The feminine form of one used when the full value is exactly one.</param>
    /// <param name="LeadingOneWord">The leading one-word that may be removed for exact scale ordinals.</param>
    /// <param name="TenWord">The exact cardinal word for ten.</param>
    /// <param name="TenOrdinalStem">The dedicated ordinal stem used when the terminal value is ten.</param>
    /// <param name="CommonOrdinalStem">The shared ordinal stem appended after terminal rewrites.</param>
    /// <param name="MasculineOrdinalSuffix">The masculine ordinal suffix.</param>
    /// <param name="FeminineOrdinalSuffix">The feminine ordinal suffix.</param>
    /// <param name="OrdinalUnit3RestoredVowel">The vowel restored before the common ordinal stem when the terminal unit is three.</param>
    /// <param name="OrdinalUnit6RestoredVowel">The vowel restored before the common ordinal stem when the terminal unit is six.</param>
    /// <param name="UnitsMap">The cardinal unit words keyed by digit value.</param>
    /// <param name="UnitsFinalAccent">The alternate unit words used when the terminal triad needs the locale's final accent.</param>
    /// <param name="TensMap">The tens words keyed by decade value.</param>
    /// <param name="TeensMap">The teen words keyed by teen offset.</param>
    /// <param name="HundredsMap">The hundreds words keyed by digit value.</param>
    /// <param name="OrdinalUnderTen">The exact ordinal stems for one through nine.</param>
    /// <param name="Scales">The descending scale rows used during triad decomposition.</param>
    public sealed record Profile(
        string ZeroWord,
        string MinusWord,
        string FeminineOneWord,
        string LeadingOneWord,
        string TenWord,
        string TenOrdinalStem,
        string CommonOrdinalStem,
        string MasculineOrdinalSuffix,
        string FeminineOrdinalSuffix,
        string OrdinalUnit3RestoredVowel,
        string OrdinalUnit6RestoredVowel,
        string[] UnitsMap,
        string[] UnitsFinalAccent,
        string[] TensMap,
        string[] TeensMap,
        string[] HundredsMap,
        string[] OrdinalUnderTen,
        TriadScale[] Scales);

    /// <summary>
    /// One descending triad scale row for <see cref="TriadScaleNumberToWordsConverter"/>.
    /// </summary>
    /// <param name="Value">The numeric value represented by this scale row.</param>
    /// <param name="Singular">The singular scale word used when the count is exactly one.</param>
    /// <param name="Plural">The plural scale word used for counts above one.</param>
    /// <param name="CountToScaleJoiner">The joiner inserted between the rendered count and the scale word.</param>
    /// <param name="CountUsesFinalAccent">A value indicating whether the count uses the alternate final-accent unit table.</param>
    /// <param name="AppendTrailingSpace">A value indicating whether the rendered scale phrase must keep a trailing space.</param>
    /// <param name="OrdinalCompactionMatch">The substring replaced when an exact scale value becomes ordinal.</param>
    /// <param name="OrdinalCompactionReplacement">The replacement text used for exact scale ordinals.</param>
    /// <param name="RemoveLeadingOneOnExactOrdinal">A value indicating whether an exact scale ordinal drops the leading one-word.</param>
    /// <param name="ExactOrdinalSuffix">The suffix appended when a larger exact multiple of this scale becomes ordinal.</param>
    public sealed record TriadScale(
        int Value,
        string Singular,
        string Plural,
        string CountToScaleJoiner,
        bool CountUsesFinalAccent,
        bool AppendTrailingSpace,
        string OrdinalCompactionMatch,
        string OrdinalCompactionReplacement,
        bool RemoveLeadingOneOnExactOrdinal,
        string ExactOrdinalSuffix);

    readonly Profile profile = profile;

    /// <summary>
    /// Converts the number using the locale's triad-based cardinal rules.
    /// </summary>
    /// <inheritdoc />
    public override string Convert(long input, GrammaticalGender gender, bool addAnd = true)
    {
        if (input is > int.MaxValue or < int.MinValue)
        {
            throw new NotImplementedException();
        }

        var number = (int)input;
        if (number < 0)
        {
            return profile.MinusWord + " " + Convert(Math.Abs(number), gender);
        }

        if (number == 0)
        {
            return profile.ZeroWord;
        }

        if (gender == GrammaticalGender.Feminine && number == 1)
        {
            return profile.FeminineOneWord;
        }

        // The triad walk is least-significant-first, so the working span needs one extra slot and
        // the reconstruction loop can prepend each rendered block without allocating temporary strings.
        Span<int> parts = stackalloc int[profile.Scales.Length + 1];
        var count = SplitEveryThreeDigits(number, parts);
        var words = string.Empty;

        for (var index = 0; index < count; index++)
        {
            var part = parts[index];
            if (part == 0)
            {
                continue;
            }

            words = (index == 0 ? ConvertTriad(part, true) : ConvertScalePart(index, part)) + words;
        }

        return words.TrimEnd();
    }

    /// <summary>
    /// Converts the number to an ordinal using the locale's triad-based ordinal rules.
    /// </summary>
    /// <inheritdoc />
    public override string ConvertToOrdinal(int number, GrammaticalGender gender)
    {
        if (number == 0)
        {
            return profile.ZeroWord;
        }

        if (number <= 9)
        {
            return profile.OrdinalUnderTen[number] + GetOrdinalGenderSuffix(gender);
        }

        var words = Convert(number, gender);
        if (number % 100 == 10)
        {
            // This family does not build the "ten" ordinal by suffixing the cardinal word; the
            // stem rewrite is a dedicated rule because the trailing consonant changes.
            return words[..^profile.TenWord.Length] + profile.TenOrdinalStem + GetOrdinalGenderSuffix(gender);
        }

        words = words[..^1];
        words = ApplyOrdinalVowelRestoration(words, number % 10);
        words = ApplyExactScaleOrdinalTransforms(words, number);

        return words + profile.CommonOrdinalStem + GetOrdinalGenderSuffix(gender);
    }

    /// <summary>
    /// Splits a number into three-digit triads, least significant first.
    /// </summary>
    static int SplitEveryThreeDigits(int number, Span<int> parts)
    {
        var count = 0;
        var remaining = number;

        while (remaining > 0)
        {
            parts[count++] = remaining % 1000;
            remaining /= 1000;
        }

        return count;
    }

    /// <summary>
    /// Converts a scaled triad and appends the locale-specific scale name.
    /// </summary>
    string ConvertScalePart(int scaleIndex, int number)
    {
        var scale = profile.Scales[scaleIndex - 1];
        if (number == 1)
        {
            return scale.Singular;
        }

        var countText = ConvertTriad(number, scale.CountUsesFinalAccent);
        var joined = string.IsNullOrEmpty(scale.CountToScaleJoiner)
            ? countText + scale.Plural
            : countText + scale.CountToScaleJoiner + scale.Plural;

        return scale.AppendTrailingSpace ? joined + " " : joined;
    }

    /// <summary>
    /// Returns the ordinal suffix for the requested gender.
    /// </summary>
    string GetOrdinalGenderSuffix(GrammaticalGender gender) =>
        gender == GrammaticalGender.Feminine ? profile.FeminineOrdinalSuffix : profile.MasculineOrdinalSuffix;

    /// <summary>
    /// Restores the final vowel for specific ordinal units.
    /// </summary>
    string ApplyOrdinalVowelRestoration(string words, int lastUnit) =>
        lastUnit switch
        {
            3 => words + profile.OrdinalUnit3RestoredVowel,
            6 => words + profile.OrdinalUnit6RestoredVowel,
            _ => words
        };

    /// <summary>
    /// Applies exact scale ordinal compaction and suffix rules.
    /// </summary>
    string ApplyExactScaleOrdinalTransforms(string words, int number)
    {
        // Walk from the largest scale downward so the first exact scale match wins. A smaller
        // match would be wrong here because the rewrite needs the outermost terminating scale.
        for (var index = profile.Scales.Length - 1; index >= 0; index--)
        {
            var scale = profile.Scales[index];
            if (number < scale.Value || number % scale.Value != 0)
            {
                continue;
            }

            if (!string.IsNullOrEmpty(scale.OrdinalCompactionMatch))
            {
                words = words.Replace(scale.OrdinalCompactionMatch, scale.OrdinalCompactionReplacement);
            }

            if (scale.RemoveLeadingOneOnExactOrdinal && number == scale.Value)
            {
                // Some exact ordinals drop the leading one only when the scale itself is the whole
                // value; nested scales must keep it or the phrase becomes ungrammatical.
                words = words.Replace(profile.LeadingOneWord, string.Empty);
            }

            if (!string.IsNullOrEmpty(scale.ExactOrdinalSuffix) && number > scale.Value)
            {
                words += scale.ExactOrdinalSuffix;
            }

            break;
        }

        return words;
    }

    /// <summary>
    /// Converts a single triad.
    /// </summary>
    string ConvertTriad(int number, bool thisIsLastSet)
    {
        if (number == 0)
        {
            return string.Empty;
        }

        var tensAndUnits = number % 100;
        var hundreds = number / 100;
        var units = tensAndUnits % 10;
        var tens = tensAndUnits / 10;

        var words = string.Empty;
        words += profile.HundredsMap[hundreds];
        words += profile.TensMap[tens];

        if (tensAndUnits <= 9)
        {
            words += profile.UnitsMap[tensAndUnits];
        }
        else if (tensAndUnits <= 19)
        {
            words += profile.TeensMap[tensAndUnits - 10];
        }
        else
        {
            // 1 and 8 trigger a local stem contraction before the unit word is appended; the
            // "remove the final character" step is intentional and family-specific, not a bug.
            if (units is 1 or 8)
            {
                words = words[..^1];
            }

            var unitWord = thisIsLastSet && units == 3
                ? profile.UnitsFinalAccent[units]
                : profile.UnitsMap[units];

            words += unitWord;
        }

        return words;
    }
}

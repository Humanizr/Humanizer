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
        ulong Value,
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
        var magnitude = GetAbsoluteValue(input);
        if (magnitude == 0)
        {
            return profile.ZeroWord;
        }

        if (gender == GrammaticalGender.Feminine && magnitude == 1)
        {
            return input < 0 ? profile.MinusWord + " " + profile.FeminineOneWord : profile.FeminineOneWord;
        }

        var words = new StringBuilder();
        foreach (var scale in profile.Scales)
        {
            var count = (int)(magnitude / scale.Value);
            if (count == 0)
            {
                continue;
            }

            words.Append(ConvertScalePart(scale, count));
            magnitude %= scale.Value;
        }

        if (magnitude > 0)
        {
            words.Append(ConvertTriad((int)magnitude, true));
        }

        var result = words.ToString().TrimEnd();
        return input < 0 ? profile.MinusWord + " " + result : result;
    }

    /// <summary>
    /// Converts the number to an ordinal using the locale's triad-based ordinal rules.
    /// </summary>
    /// <inheritdoc />
    public override string ConvertToOrdinal(int number, GrammaticalGender gender)
    {
        var magnitude = number == int.MinValue ? (long)int.MaxValue + 1 : Math.Abs(number);
        if (magnitude == 0)
        {
            return profile.ZeroWord;
        }

        if (magnitude <= 9)
        {
            var ordinal = profile.OrdinalUnderTen[(int)magnitude] + GetOrdinalGenderSuffix(gender);
            return number < 0 ? profile.MinusWord + " " + ordinal : ordinal;
        }

        var words = Convert(magnitude, gender);
        if (magnitude % 100 == 10)
        {
            words = words[..^profile.TenWord.Length] + profile.TenOrdinalStem + GetOrdinalGenderSuffix(gender);
            return number < 0 ? profile.MinusWord + " " + words : words;
        }

        words = words[..^1];
        words = ApplyOrdinalVowelRestoration(words, (int)(magnitude % 10));
        words = ApplyExactScaleOrdinalTransforms(words, magnitude);
        words += profile.CommonOrdinalStem + GetOrdinalGenderSuffix(gender);
        return number < 0 ? profile.MinusWord + " " + words : words;
    }

    /// <summary>
    /// Converts a scaled triad and appends the locale-specific scale name.
    /// </summary>
    string ConvertScalePart(TriadScale scale, int number)
    {
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

    static ulong GetAbsoluteValue(long value) =>
        value >= 0 ? (ulong)value : unchecked((ulong)(-(value + 1)) + 1);

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
    string ApplyExactScaleOrdinalTransforms(string words, long number)
    {
        // Walk from the largest scale downward so the first exact scale match wins. A smaller
        // match would be wrong here because the rewrite needs the outermost terminating scale.
        foreach (var scale in profile.Scales)
        {
            if ((ulong)number < scale.Value || (ulong)number % scale.Value != 0)
            {
                continue;
            }

            if (!string.IsNullOrEmpty(scale.OrdinalCompactionMatch))
            {
                words = words.Replace(scale.OrdinalCompactionMatch, scale.OrdinalCompactionReplacement);
            }

            if (scale.RemoveLeadingOneOnExactOrdinal && (ulong)number == scale.Value)
            {
                // Some exact ordinals drop the leading one only when the scale itself is the whole
                // value; nested scales must keep it or the phrase becomes ungrammatical.
                words = words.Replace(profile.LeadingOneWord, string.Empty);
            }

            if (!string.IsNullOrEmpty(scale.ExactOrdinalSuffix) && (ulong)number > scale.Value)
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
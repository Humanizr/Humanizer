namespace Humanizer;

/// <summary>
/// Shared renderer for locales that append group words after each triad and may need special
/// handling for feminine ones, duals, or pluralized group labels.
///
/// The generated profile carries the group lexicon while the runtime keeps the decomposition logic
/// fixed, so locale differences are expressed as data rather than separate converter branches.
/// </summary>
class AppendedGroupNumberToWordsConverter(AppendedGroupNumberToWordsConverter.Profile profile) : GenderedNumberToWordsConverter
{
    /// <summary>
    /// Converts the given value using the locale's grouped cardinal rules.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <param name="gender">The grammatical gender to use when the locale distinguishes it.</param>
    /// <param name="addAnd">Reserved for compatibility with other converters; this implementation derives conjunction placement from its generated profile.</param>
    /// <returns>The localized cardinal words for <paramref name="number"/>.</returns>
    public override string Convert(long number, GrammaticalGender gender, bool addAnd = true)
    {
        if (number == 0)
        {
            return profile.ZeroWord;
        }

        if (number < 0)
        {
            return profile.NegativeWord + " " + Convert(-number, gender);
        }

        var result = string.Empty;
        var groupLevel = 0;

        // Walk from least significant triad to most significant triad so the group word can be
        // decided after we know whether the current triad is units, thousands, millions, etc.
        while (number >= 1)
        {
            var groupNumber = number % 1000;
            number /= 1000;

            var tens = groupNumber % 100;
            var hundreds = groupNumber / 100;
            var process = string.Empty;

            if (hundreds > 0)
            {
                // The hundreds phrase is the leading fragment for this triad unless the locale has a
                // dedicated "two hundred" contraction for the current group.
                process = tens == 0 && hundreds == 2
                    ? profile.AppendedTwos[0]
                    : profile.HundredsGroup[hundreds];
            }

            if (tens > 0)
            {
                if (tens < 20)
                {
                    // Higher groups have a special "two" form in some locales, but only when the
                    // current triad is itself the full scale word.
                    if (tens == 2 && hundreds == 0 && groupLevel > 0)
                    {
                        process = number switch
                        {
                            2000 or 2000000 or 2000000000 => profile.AppendedTwos[groupLevel],
                            _ => profile.Twos[groupLevel]
                        };
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(process))
                        {
                            // Conjunctions stay inside the triad phrase; they should not split the
                            // group word away from the counted value.
                            process += " " + profile.ConjunctionWord + " ";
                        }

                        if (tens == 1 && groupLevel > 0 && hundreds == 0)
                        {
                            // Some locales suppress the leading unit in higher groups such as "ten
                            // thousand"; preserve that shape instead of forcing an explicit "one".
                            process += " ";
                        }
                        else
                        {
                            process += gender == GrammaticalGender.Feminine && groupLevel == 0
                                ? profile.FeminineOnesGroup[tens]
                                : profile.OnesGroup[tens];
                        }
                    }
                }
                else
                {
                    var ones = tens % 10;
                    tens /= 10;

                    if (ones > 0)
                    {
                        if (!string.IsNullOrEmpty(process))
                        {
                            process += " " + profile.ConjunctionWord + " ";
                        }

                        process += gender == GrammaticalGender.Feminine
                            ? profile.FeminineOnesGroup[ones]
                            : profile.OnesGroup[ones];
                    }

                    if (!string.IsNullOrEmpty(process))
                    {
                        // Compound tens are rendered as "ones + conjunction + tens" in this family.
                        process += " " + profile.ConjunctionWord + " ";
                    }

                    process += profile.TensGroup[tens];
                }
            }

            if (!string.IsNullOrEmpty(process))
            {
                if (groupLevel > 0)
                {
                    if (!string.IsNullOrEmpty(result))
                    {
                        // Once a higher group already exists, the conjunction belongs between the
                        // group words rather than inside the current triad fragment.
                        result = profile.ConjunctionWord + " " + result;
                    }

                    if (groupNumber != 2)
                    {
                        if (groupNumber % 100 != 1)
                        {
                            // Singular, plural, and appended group words are data-driven because
                            // different locales use different inflection rules for the same group.
                            result = groupNumber is >= 3 and <= 10
                                ? profile.PluralGroups[groupLevel] + " " + result
                                : (string.IsNullOrEmpty(result) ? profile.Groups[groupLevel] : profile.AppendedGroups[groupLevel]) + " " + result;
                        }
                        else
                        {
                            result = profile.Groups[groupLevel] + " " + result;
                        }
                    }
                }

                result = process + " " + result;
            }

            groupLevel++;
        }

        return result.Trim();
    }

    /// <summary>
    /// Converts the given value using the locale's grouped ordinal rules.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <param name="gender">The grammatical gender to use.</param>
    /// <remarks>
    /// The ordinal form is assembled from the same grouped decomposition as the cardinal form and
    /// then adjusted with locale-specific ordinal substitutions.
    /// </remarks>
    /// <returns>The localized ordinal words for <paramref name="number"/>.</returns>
    public override string ConvertToOrdinal(int number, GrammaticalGender gender)
    {
        if (number == 0)
        {
            return profile.OrdinalZeroWord;
        }

        var beforeOneHundredNumber = number % 100;
        var overTensPart = number / 100 * 100;
        var beforeOneHundredWord = string.Empty;
        var overTensWord = string.Empty;

        if (beforeOneHundredNumber > 0)
        {
            // Render the lower part first so any ordinal exceptions apply to the actual terminal
            // value rather than the higher-scale prefix.
            beforeOneHundredWord = ParseNumber(Convert(beforeOneHundredNumber, gender), beforeOneHundredNumber, gender);
        }

        if (overTensPart > 0)
        {
            // The higher part stays cardinal; only the terminal fragment becomes ordinal.
            overTensWord = ParseNumber(Convert(overTensPart), overTensPart, gender);
        }

        return (beforeOneHundredWord +
                (overTensPart > 0
                    ? (string.IsNullOrWhiteSpace(beforeOneHundredWord) ? string.Empty : " " + profile.OrdinalSeparatorWord + " ") + overTensWord
                    : string.Empty))
            .Trim();
    }

    /// <summary>
    /// Converts a rendered cardinal fragment into its ordinal equivalent for the locale.
    /// </summary>
    /// <param name="word">The already rendered cardinal fragment.</param>
    /// <param name="number">The numeric value represented by <paramref name="word"/>.</param>
    /// <param name="gender">The grammatical gender to apply when choosing ordinal forms.</param>
    /// <returns>The ordinal rendering for the supplied fragment.</returns>
    string ParseNumber(string word, int number, GrammaticalGender gender)
    {
        if (number == 1)
        {
            return gender == GrammaticalGender.Feminine ? profile.FirstOrdinalFeminine : profile.FirstOrdinalMasculine;
        }

        var ordinals = gender == GrammaticalGender.Feminine
            ? profile.FeminineOrdinalExceptions
            : profile.OrdinalExceptions;

        if (number <= 10)
        {
            // Single-word ordinals are mostly suffix substitutions on the generated cardinal form.
            foreach (var kv in ordinals.Where(kv => word.EndsWith(kv.Key, StringComparison.Ordinal)))
            {
                return StringHumanizeExtensions.Concat(
                    word.AsSpan(0, word.Length - kv.Key.Length),
                    kv.Value.AsSpan());
            }

            return word;
        }

        if (number is > 10 and < 100)
        {
            var parts = word.Split(' ');
            var newParts = new string[parts.Length];

            for (var index = 0; index < parts.Length; index++)
            {
                var oldPart = parts[index];
                var newPart = oldPart;

                // Ordinal exceptions are applied part-by-part so the language can keep the same
                // compound structure while changing only the terminal piece that needs it.
                foreach (var kv in ordinals.Where(kv => oldPart.EndsWith(kv.Key, StringComparison.Ordinal)))
                {
                    newPart = StringHumanizeExtensions.Concat(
                        oldPart.AsSpan(0, oldPart.Length - kv.Key.Length),
                        kv.Value.AsSpan());
                }

                if (number > 19 && newPart == oldPart && oldPart.Length > 1)
                {
                    newPart = profile.OrdinalPrefix + oldPart;
                }

                newParts[index] = newPart;
            }

            return string.Join(" ", newParts);
        }

        return profile.OrdinalPrefix + word;
    }

    /// <summary>
    /// Immutable generated profile for <see cref="AppendedGroupNumberToWordsConverter"/>.
    /// </summary>
    /// <param name="ZeroWord">The cardinal zero word.</param>
    /// <param name="NegativeWord">The word used to prefix negative values.</param>
    /// <param name="ConjunctionWord">The conjunction inserted between triad fragments and grouped phrases.</param>
    /// <param name="OrdinalZeroWord">The dedicated ordinal zero word.</param>
    /// <param name="OrdinalSeparatorWord">The separator inserted between the ordinalized terminal fragment and any higher-order prefix.</param>
    /// <param name="OrdinalPrefix">The shared ordinal prefix applied when no exact exception matches.</param>
    /// <param name="FirstOrdinalMasculine">The exact masculine ordinal word for one.</param>
    /// <param name="FirstOrdinalFeminine">The exact feminine ordinal word for one.</param>
    /// <param name="Groups">The singular group words keyed by triad index.</param>
    /// <param name="AppendedGroups">The appended group forms used when a remainder follows the scale word.</param>
    /// <param name="PluralGroups">The pluralized group forms used for counts that trigger plural inflection.</param>
    /// <param name="OnesGroup">The masculine or default unit words keyed by digit value.</param>
    /// <param name="TensGroup">The tens words keyed by decade value.</param>
    /// <param name="HundredsGroup">The hundreds words keyed by digit value.</param>
    /// <param name="AppendedTwos">The special "two + group" forms used when the scale itself is contracted.</param>
    /// <param name="Twos">The plain "two + group" forms keyed by triad index.</param>
    /// <param name="FeminineOnesGroup">The feminine unit words used when the locale distinguishes terminal gender.</param>
    /// <param name="OrdinalExceptions">The masculine suffix substitutions used when converting cardinal fragments to ordinals.</param>
    /// <param name="FeminineOrdinalExceptions">The feminine suffix substitutions used when converting cardinal fragments to ordinals.</param>
    public sealed record Profile(
        string ZeroWord,
        string NegativeWord,
        string ConjunctionWord,
        string OrdinalZeroWord,
        string OrdinalSeparatorWord,
        string OrdinalPrefix,
        string FirstOrdinalMasculine,
        string FirstOrdinalFeminine,
        string[] Groups,
        string[] AppendedGroups,
        string[] PluralGroups,
        string[] OnesGroup,
        string[] TensGroup,
        string[] HundredsGroup,
        string[] AppendedTwos,
        string[] Twos,
        string[] FeminineOnesGroup,
        FrozenDictionary<string, string> OrdinalExceptions,
        FrozenDictionary<string, string> FeminineOrdinalExceptions);
}
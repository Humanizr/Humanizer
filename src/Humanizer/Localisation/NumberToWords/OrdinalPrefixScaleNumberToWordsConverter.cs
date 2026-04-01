namespace Humanizer;

/// <summary>
/// Renders locales that build ordinals by prefixing scale-specific ordinal stems onto cardinal
/// fragments.
/// </summary>
class OrdinalPrefixScaleNumberToWordsConverter(OrdinalPrefixScaleNumberToWordsProfile profile)
    : GenderedNumberToWordsConverter(profile.DefaultGender)
{
    readonly OrdinalPrefixScaleNumberToWordsProfile profile = profile;

    /// <inheritdoc/>
    public override string Convert(long number, GrammaticalGender gender, bool addAnd = true)
    {
        if (number == 0)
        {
            return profile.UnitsMap[0];
        }

        var parts = new List<string?>();
        if (number < 0)
        {
            parts.Add(profile.MinusWord);
            number = -number;
        }

        var needsAnd = false;
        foreach (var scale in profile.Scales)
        {
            CollectScaleParts(parts, ref number, ref needsAnd, scale);
        }

        if (number > 0)
        {
            if (needsAnd && IsAndSplitNeeded((int)number))
            {
                parts.Add(profile.AndWord);
            }

            CollectUnderOneThousand(parts, number, gender);
        }

        return string.Join(" ", parts);
    }

    /// <inheritdoc/>
    public override string ConvertToOrdinal(int number, GrammaticalGender gender)
    {
        if (number == 0)
        {
            return profile.UnitsOrdinalPrefixes[0] + GetOrdinalEnding(gender);
        }

        var parts = new List<string?>();
        var needsAnd = false;

        foreach (var scale in profile.Scales.Where(static scale => scale.Value <= int.MaxValue))
        {
            CollectOrdinalScaleParts(parts, ref number, ref needsAnd, scale, gender);
        }

        if (number > 0)
        {
            if (needsAnd && IsAndSplitNeeded(number))
            {
                parts.Add(profile.AndWord);
            }

            CollectOrdinalUnderOneThousand(parts, number, gender, gender, useScaleOrdinalPrefix: false, string.Empty);
        }

        return string.Join(" ", parts);
    }

    /// <summary>
    /// Determines whether an "and" bridge is required between the higher scales and the remainder.
    /// </summary>
    static bool IsAndSplitNeeded(int number) =>
        number <= 20 || number % 10 == 0 && number < 100 || number % 100 == 0;

    /// <summary>
    /// Returns the gender-specific ordinal ending.
    /// </summary>
    string GetOrdinalEnding(GrammaticalGender gender) =>
        gender == GrammaticalGender.Masculine
            ? profile.MasculineOrdinalEnding
            : profile.NonMasculineOrdinalEnding;

    /// <summary>
    /// Appends the cardinal rendering for one scale row.
    /// </summary>
    void CollectScaleParts(List<string?> parts, ref long number, ref bool needsAnd, OrdinalPrefixScale scale)
    {
        var quotient = number / scale.Value;
        if (quotient == 0)
        {
            return;
        }

        number %= scale.Value;
        var startIndex = parts.Count;
        if (quotient == 1)
        {
            parts.Add(scale.Singular);
        }
        else
        {
            CollectUnderOneThousand(parts, quotient, scale.Gender);
            parts.Add(scale.Plural);
        }

        if (number == 0 && needsAnd && !ContainsAndWord(parts, startIndex))
        {
            parts.Insert(startIndex, profile.AndWord);
        }

        needsAnd = true;
    }

    /// <summary>
    /// Appends the ordinal rendering for one scale row.
    /// </summary>
    void CollectOrdinalScaleParts(List<string?> parts, ref int number, ref bool needsAnd, OrdinalPrefixScale scale, GrammaticalGender ordinalGender)
    {
        var quotient = number / (int)scale.Value;
        if (quotient == 0)
        {
            return;
        }

        number %= (int)scale.Value;
        if (number > 0 && (number > 19 || (number % 100 > 10 && number % 10 == 0)))
        {
            if (quotient == 1)
            {
                parts.Add(scale.Singular);
            }
            else
            {
                CollectUnderOneThousand(parts, quotient, scale.Gender);
                parts.Add(scale.Plural);
            }
        }
        else
        {
            var startIndex = parts.Count;
            CollectOrdinalUnderOneThousand(parts, quotient, scale.Gender, ordinalGender, useScaleOrdinalPrefix: true, scale.OrdinalPrefix);
            if (number == 0 && needsAnd && !ContainsAndWord(parts, startIndex))
            {
                parts.Insert(startIndex, profile.AndWord);
            }
        }

        needsAnd = true;
    }

    /// <summary>
    /// Returns whether the collected parts already include the conjunction word.
    /// </summary>
    bool ContainsAndWord(List<string?> parts, int startIndex)
    {
        for (var index = startIndex; index < parts.Count; index++)
        {
            if (parts[index] == profile.AndWord)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Renders the under-one-thousand cardinal fragment.
    /// </summary>
    void CollectUnderOneThousand(List<string?> builder, long number, GrammaticalGender gender)
    {
        var hundreds = number / 100;
        var hundredRemainder = number % 100;
        var units = hundredRemainder % 10;
        var tens = hundredRemainder / 10;

        if (hundreds != 0)
        {
            AddUnit(builder, hundreds, GrammaticalGender.Neuter);
            builder.Add(hundreds == 1 ? profile.HundredSingular : profile.HundredPlural);
        }

        if (tens >= 2)
        {
            if (units != 0)
            {
                builder.Add(profile.TensMap[tens]);
                builder.Add(profile.AndWord);
                AddUnit(builder, units, gender);
            }
            else
            {
                if (hundreds != 0)
                {
                    builder.Add(profile.AndWord);
                }

                builder.Add(profile.TensMap[tens]);
            }
        }
        else if (hundredRemainder != 0)
        {
            if (hundreds != 0)
            {
                builder.Add(profile.AndWord);
            }

            AddUnit(builder, hundredRemainder, gender);
        }
    }

    /// <summary>
    /// Renders the under-one-thousand ordinal fragment.
    /// </summary>
    void CollectOrdinalUnderOneThousand(
        List<string?> builder,
        int number,
        GrammaticalGender partGender,
        GrammaticalGender ordinalGender,
        bool useScaleOrdinalPrefix,
        string scaleOrdinalPrefix)
    {
        var hundreds = number / 100;
        var hundredRemainder = number % 100;
        var units = hundredRemainder % 10;
        var decade = hundredRemainder / 10 * 10;

        if (hundreds != 0)
        {
            AddUnit(builder, hundreds, GrammaticalGender.Neuter);
            var hundredPrefix = hundreds == 1 ? profile.HundredSingular : profile.HundredPlural;
            if (hundredRemainder < 20 && !useScaleOrdinalPrefix)
            {
                builder.Add(partGender == GrammaticalGender.Masculine
                    ? hundredPrefix + "asti"
                    : hundredPrefix + "asta");
            }
            else
            {
                builder.Add(hundredPrefix);
            }
        }

        if (decade >= 20)
        {
            if (units != 0)
            {
                builder.Add(GetOrdinalUnderHundred(decade, partGender));
                builder.Add(profile.AndWord);
                builder.Add(GetOrdinalUnderHundred(units, partGender));
            }
            else
            {
                if (hundreds != 0)
                {
                    builder.Add(profile.AndWord);
                }

                builder.Add(GetOrdinalUnderHundred(decade, partGender));
            }
        }
        else if (hundredRemainder != 0)
        {
            if (hundreds != 0)
            {
                builder.Add(profile.AndWord);
            }

            if (useScaleOrdinalPrefix)
            {
                AddUnit(builder, hundredRemainder, partGender);
            }
            else
            {
                builder.Add(GetOrdinalUnderHundred(hundredRemainder, partGender));
            }
        }

        if (useScaleOrdinalPrefix)
        {
            builder.Add(scaleOrdinalPrefix + GetOrdinalEnding(ordinalGender));
        }
    }

    /// <summary>
    /// Returns the under-hundred ordinal stem for the requested gender.
    /// </summary>
    string GetOrdinalUnderHundred(int number, GrammaticalGender gender)
    {
        if (number is >= 0 and < 20)
        {
            if (number == 2)
            {
                return gender switch
                {
                    GrammaticalGender.Masculine => profile.OrdinalTwoMasculine,
                    GrammaticalGender.Feminine => profile.OrdinalTwoFeminine,
                    GrammaticalGender.Neuter => profile.OrdinalTwoNeuter,
                    _ => throw new ArgumentOutOfRangeException(nameof(gender))
                };
            }

            return profile.UnitsOrdinalPrefixes[number] + GetOrdinalEnding(gender);
        }

        return profile.TensOrdinalPrefixes[number / 10] + GetOrdinalEnding(gender);
    }

    /// <summary>
    /// Adds a gendered unit word when the locale provides one.
    /// </summary>
    void AddUnit(List<string?> builder, long number, GrammaticalGender gender)
    {
        if (number is > 0 and < 5)
        {
            var genderedForm = gender switch
            {
                GrammaticalGender.Masculine => profile.MasculineUnitsMap[number],
                GrammaticalGender.Feminine => profile.FeminineUnitsMap[number],
                GrammaticalGender.Neuter => profile.NeuterUnitsMap[number],
                _ => throw new ArgumentOutOfRangeException(nameof(gender))
            };
            builder.Add(genderedForm);
            return;
        }

        builder.Add(profile.UnitsMap[number]);
    }
}

/// <summary>
/// Immutable generated profile for <see cref="OrdinalPrefixScaleNumberToWordsConverter"/>.
/// </summary>
/// <param name="defaultGender">The default gender used when callers do not specify one.</param>
/// <param name="minusWord">The word used to prefix negative values.</param>
/// <param name="andWord">The conjunction inserted where the locale requires one.</param>
/// <param name="masculineOrdinalEnding">The masculine ordinal ending.</param>
/// <param name="nonMasculineOrdinalEnding">The ordinal ending used for non-masculine genders.</param>
/// <param name="unitsMap">The base unit lexicon.</param>
/// <param name="masculineUnitsMap">The masculine overrides for units.</param>
/// <param name="feminineUnitsMap">The feminine overrides for units.</param>
/// <param name="neuterUnitsMap">The neuter overrides for units.</param>
/// <param name="tensMap">The tens lexicon.</param>
/// <param name="unitsOrdinalPrefixes">The ordinal prefixes used for units.</param>
/// <param name="tensOrdinalPrefixes">The ordinal prefixes used for tens.</param>
/// <param name="hundredSingular">The singular hundred word.</param>
/// <param name="hundredPlural">The plural hundred word.</param>
/// <param name="ordinalTwoMasculine">The masculine ordinal form for two.</param>
/// <param name="ordinalTwoFeminine">The feminine ordinal form for two.</param>
/// <param name="ordinalTwoNeuter">The neuter ordinal form for two.</param>
/// <param name="scales">The descending scale rows used during decomposition.</param>
sealed class OrdinalPrefixScaleNumberToWordsProfile(
    GrammaticalGender defaultGender,
    string minusWord,
    string andWord,
    string masculineOrdinalEnding,
    string nonMasculineOrdinalEnding,
    string[] unitsMap,
    string[] masculineUnitsMap,
    string[] feminineUnitsMap,
    string[] neuterUnitsMap,
    string[] tensMap,
    string[] unitsOrdinalPrefixes,
    string[] tensOrdinalPrefixes,
    string hundredSingular,
    string hundredPlural,
    string ordinalTwoMasculine,
    string ordinalTwoFeminine,
    string ordinalTwoNeuter,
    OrdinalPrefixScale[] scales)
{
    /// <summary>Gets the default gender used when callers do not specify one.</summary>
    public GrammaticalGender DefaultGender { get; } = defaultGender;
    /// <summary>Gets the word used to prefix negative values.</summary>
    public string MinusWord { get; } = minusWord;
    /// <summary>Gets the conjunction inserted where the locale requires one.</summary>
    public string AndWord { get; } = andWord;
    /// <summary>Gets the masculine ordinal ending.</summary>
    public string MasculineOrdinalEnding { get; } = masculineOrdinalEnding;
    /// <summary>Gets the ordinal ending used for non-masculine genders.</summary>
    public string NonMasculineOrdinalEnding { get; } = nonMasculineOrdinalEnding;
    /// <summary>Gets the base unit lexicon.</summary>
    public string[] UnitsMap { get; } = unitsMap;
    /// <summary>Gets the masculine overrides for units.</summary>
    public string[] MasculineUnitsMap { get; } = masculineUnitsMap;
    /// <summary>Gets the feminine overrides for units.</summary>
    public string[] FeminineUnitsMap { get; } = feminineUnitsMap;
    /// <summary>Gets the neuter overrides for units.</summary>
    public string[] NeuterUnitsMap { get; } = neuterUnitsMap;
    /// <summary>Gets the tens lexicon.</summary>
    public string[] TensMap { get; } = tensMap;
    /// <summary>Gets the ordinal prefixes used for unit values.</summary>
    public string[] UnitsOrdinalPrefixes { get; } = unitsOrdinalPrefixes;
    /// <summary>Gets the ordinal prefixes used for decade values.</summary>
    public string[] TensOrdinalPrefixes { get; } = tensOrdinalPrefixes;
    /// <summary>Gets the singular hundred word.</summary>
    public string HundredSingular { get; } = hundredSingular;
    /// <summary>Gets the plural hundred word.</summary>
    public string HundredPlural { get; } = hundredPlural;
    /// <summary>Gets the masculine ordinal form for two.</summary>
    public string OrdinalTwoMasculine { get; } = ordinalTwoMasculine;
    /// <summary>Gets the feminine ordinal form for two.</summary>
    public string OrdinalTwoFeminine { get; } = ordinalTwoFeminine;
    /// <summary>Gets the neuter ordinal form for two.</summary>
    public string OrdinalTwoNeuter { get; } = ordinalTwoNeuter;
    /// <summary>Gets the descending scale rows used during decomposition.</summary>
    public OrdinalPrefixScale[] Scales { get; } = scales;
}

/// <summary>
/// One descending scale row for <see cref="OrdinalPrefixScaleNumberToWordsConverter"/>.
/// </summary>
/// <param name="Value">The divisor for the scale row.</param>
/// <param name="Singular">The singular form.</param>
/// <param name="Plural">The plural form.</param>
/// <param name="OrdinalPrefix">The ordinal prefix used when the scale becomes the terminal stem.</param>
/// <param name="Gender">The grammatical gender used for the scale row.</param>
readonly record struct OrdinalPrefixScale(
    long Value,
    string Singular,
    string Plural,
    string OrdinalPrefix,
    GrammaticalGender Gender);

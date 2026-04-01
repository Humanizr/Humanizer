namespace Humanizer;

/// <summary>
/// Shared East Slavic renderer for languages whose cardinals are mostly regular once the scale
/// gender and paucal/plural form are known, but whose ordinals are built by combining an ordinal
/// stem with a gender-specific terminal ending.
///
/// The intended result is a phrase where:
/// - large scales use generated singular/paucal/plural forms
/// - the final cardinal units respect feminine/neuter overrides for one and two
/// - ordinals mutate only the terminal segment, while earlier scale segments stay cardinal
///
/// The generated profile supplies the scale forms, ordinal stems, and terminal-ending overrides so
/// this converter can stay structural instead of language-branded.
/// </summary>
class EastSlavicNumberToWordsConverter(EastSlavicNumberToWordsProfile profile) : GenderedNumberToWordsConverter
{
    /// <summary>
    /// Immutable generated profile that owns the East Slavic scale forms, ordinal stems, and endings.
    /// </summary>
    readonly EastSlavicNumberToWordsProfile profile = profile;

    /// <summary>
    /// Converts the given value using the locale's East Slavic cardinal rules.
    /// </summary>
    /// <param name="input">The number to convert.</param>
    /// <param name="gender">The grammatical gender to use for the terminal group.</param>
    /// <param name="addAnd">Reserved for compatibility with other converters; this implementation derives conjunction placement from the generated profile.</param>
    /// <returns>The localized cardinal words for <paramref name="input"/>.</returns>
    public override string Convert(long input, GrammaticalGender gender, bool addAnd = true)
    {
        if (input == 0)
        {
            return profile.ZeroWord;
        }

        var parts = new List<string>(8);
        var remaining = GetAbsoluteValue(input);

        if (input < 0)
        {
            parts.Add(profile.MinusWord);
        }

        // Cardinal rendering is a straight scale walk once the generated profile provides the
        // gender and singular/paucal/plural words for each scale row.
        foreach (var scale in profile.Scales)
        {
            var count = remaining / scale.Value;
            if (count == 0)
            {
                continue;
            }

            AppendCardinalUnderOneThousand(parts, (int)count, scale.Gender);
            parts.Add(ChooseScaleForm((long)count, scale));
            remaining %= scale.Value;
        }

        if (remaining != 0)
        {
            AppendCardinalUnderOneThousand(parts, (int)remaining, gender);
        }

        return string.Join(" ", parts);
    }

    /// <summary>
    /// Converts the given value into a locale-specific ordinal phrase.
    /// </summary>
    /// <param name="input">The number to convert.</param>
    /// <param name="gender">The grammatical gender to use for the terminal ending.</param>
    /// <returns>The localized ordinal words for <paramref name="input"/>.</returns>
    public override string ConvertToOrdinal(int input, GrammaticalGender gender)
    {
        if (input == 0)
        {
            return profile.ZeroOrdinalStem + GetEnding(gender, 0);
        }

        var parts = new List<string>(6);
        var remaining = GetAbsoluteValue(input);

        if (input < 0)
        {
            parts.Add(profile.MinusWord);
        }

        // Ordinals are different: only the terminal segment becomes ordinal, while earlier scale
        // segments stay cardinal. The converter therefore keeps cardinal traversal until it can
        // prove which segment terminates the phrase and which ending family applies there.
        foreach (var scale in profile.Scales)
        {
            if (scale.OrdinalStem is null || remaining < scale.Value)
            {
                continue;
            }

            var terminalValue = (int)remaining;
            var count = remaining / scale.Value;
            remaining %= scale.Value;
            if (remaining == 0)
            {
                parts.Add(count == 1
                    ? scale.OrdinalStem + GetEnding(gender, terminalValue)
                    : GetOrdinalPrefix((int)count) + scale.OrdinalStem + GetEnding(gender, terminalValue));
            }
            else
            {
                AppendCardinalUnderOneThousand(parts, (int)count, scale.Gender);
                parts.Add(ChooseScaleForm((long)count, scale));
            }
        }

        if (remaining >= 100)
        {
            var terminalValue = (int)remaining;
            var hundreds = (int)(remaining / 100);
            remaining %= 100;
            if (remaining == 0)
            {
                parts.Add(profile.UnitsOrdinalPrefixes[hundreds] + "сот" + GetEnding(gender, terminalValue));
            }
            else
            {
                parts.Add(profile.HundredsMap[hundreds]);
            }
        }

        if (remaining >= 20)
        {
            var terminalValue = (int)remaining;
            var tens = (int)(remaining / 10);
            remaining %= 10;
            if (remaining == 0)
            {
                parts.Add(profile.TensOrdinal[tens] + GetEnding(gender, terminalValue));
            }
            else
            {
                parts.Add(profile.TensMap[tens]);
            }
        }

        if (remaining > 0)
        {
            parts.Add(profile.UnitsOrdinal[(int)remaining] + GetEnding(gender, (int)remaining));
        }

        return string.Join(" ", parts);
    }

    // Under one thousand, the family is regular except for the gendered one/two variants.
    // The expected result is the same phrase a speaker would use inside a larger scale count,
    // such as "two thousand" versus "two millions", with the correct gendered low digit.
    void AppendCardinalUnderOneThousand(List<string> parts, int number, GrammaticalGender gender)
    {
        if (number >= 100)
        {
            parts.Add(profile.HundredsMap[number / 100]);
            number %= 100;
        }

        if (number >= 20)
        {
            parts.Add(profile.TensMap[number / 10]);
            number %= 10;
        }

        if (number <= 0)
        {
            return;
        }

        parts.Add((number, gender) switch
        {
            (1, GrammaticalGender.Feminine) => profile.FeminineOne,
            (1, GrammaticalGender.Neuter) => profile.NeuterOne,
            (2, GrammaticalGender.Feminine) => profile.FeminineTwo,
            _ => profile.UnitsMap[number]
        });
    }

    // Ordinal prefixes are concatenative in this family: hundreds, tens, and units each contribute
    // a stem which is later combined with the gender-specific terminal ending for the final phrase.
    string GetOrdinalPrefix(int number)
    {
        var parts = new List<string>(3);

        if (number >= 100)
        {
            var hundreds = number / 100;
            number %= 100;
            parts.Add(hundreds == 1
                ? profile.HundredsMap[hundreds]
                : profile.UnitsOrdinalPrefixes[hundreds] + "сот");
        }

        if (number >= 20)
        {
            parts.Add(profile.TensOrdinalPrefixes[number / 10]);
            number %= 10;
        }

        if (number > 0)
        {
            parts.Add(number == 1
                ? profile.OneOrdinalPrefix
                : profile.UnitsOrdinalPrefixes[number]);
        }

        return string.Concat(parts);
    }

    static string ChooseScaleForm(long number, EastSlavicScale scale) =>
        RussianGrammaticalNumberDetector.Detect(number) switch
        {
            RussianGrammaticalNumber.Singular => scale.Singular,
            RussianGrammaticalNumber.Paucal => scale.Paucal,
            _ => scale.Plural
        };

    string GetEnding(GrammaticalGender gender, int terminalValue) =>
        gender switch
        {
            GrammaticalGender.Masculine => profile.MasculineEnding.Resolve(terminalValue),
            GrammaticalGender.Feminine => profile.FeminineEnding.Resolve(terminalValue),
            GrammaticalGender.Neuter => profile.NeuterEnding.Resolve(terminalValue),
            _ => throw new ArgumentOutOfRangeException(nameof(gender))
        };

    // Math.Abs(long.MinValue) overflows, so the converter uses the standard two's-complement safe
    // transformation instead of a direct absolute-value call.
    static ulong GetAbsoluteValue(long value) =>
        value >= 0 ? (ulong)value : unchecked((ulong)(-(value + 1)) + 1);

    // The int overload uses the same overflow-safe pattern for consistency with the long path.
    static ulong GetAbsoluteValue(int value) =>
        value >= 0 ? (ulong)value : unchecked((ulong)(-(value + 1)) + 1);
}

/// <summary>
/// Immutable generated profile for <see cref="EastSlavicNumberToWordsConverter"/>.
/// </summary>
sealed class EastSlavicNumberToWordsProfile(
    string zeroWord,
    string minusWord,
    string zeroOrdinalStem,
    string[] hundredsMap,
    string[] tensMap,
    string[] unitsMap,
    string[] unitsOrdinalPrefixes,
    string[] tensOrdinalPrefixes,
    string[] tensOrdinal,
    string[] unitsOrdinal,
    string feminineOne,
    string neuterOne,
    string feminineTwo,
    string oneOrdinalPrefix,
    EastSlavicScale[] scales,
    EastSlavicGenderEnding masculineEnding,
    EastSlavicGenderEnding feminineEnding,
    EastSlavicGenderEnding neuterEnding)
{
    /// <summary>Gets the cardinal zero word.</summary>
    public string ZeroWord { get; } = zeroWord;
    /// <summary>Gets the word used to prefix negative values.</summary>
    public string MinusWord { get; } = minusWord;
    /// <summary>Gets the stem used to build the zero ordinal.</summary>
    public string ZeroOrdinalStem { get; } = zeroOrdinalStem;
    /// <summary>Gets the hundreds lexicon.</summary>
    public string[] HundredsMap { get; } = hundredsMap;
    /// <summary>Gets the tens lexicon.</summary>
    public string[] TensMap { get; } = tensMap;
    /// <summary>Gets the base units lexicon.</summary>
    public string[] UnitsMap { get; } = unitsMap;
    /// <summary>Gets the unit ordinal prefixes used in concatenative ordinal stems.</summary>
    public string[] UnitsOrdinalPrefixes { get; } = unitsOrdinalPrefixes;
    /// <summary>Gets the tens ordinal prefixes used in concatenative ordinal stems.</summary>
    public string[] TensOrdinalPrefixes { get; } = tensOrdinalPrefixes;
    /// <summary>Gets the exact tens ordinal lexicon.</summary>
    public string[] TensOrdinal { get; } = tensOrdinal;
    /// <summary>Gets the exact unit ordinal lexicon.</summary>
    public string[] UnitsOrdinal { get; } = unitsOrdinal;
    /// <summary>Gets the feminine word for one.</summary>
    public string FeminineOne { get; } = feminineOne;
    /// <summary>Gets the neuter word for one.</summary>
    public string NeuterOne { get; } = neuterOne;
    /// <summary>Gets the feminine word for two.</summary>
    public string FeminineTwo { get; } = feminineTwo;
    /// <summary>Gets the special ordinal prefix for one.</summary>
    public string OneOrdinalPrefix { get; } = oneOrdinalPrefix;
    /// <summary>Gets the descending scale rows used during decomposition.</summary>
    public EastSlavicScale[] Scales { get; } = scales;
    /// <summary>Gets the masculine terminal-ending resolver.</summary>
    public EastSlavicGenderEnding MasculineEnding { get; } = masculineEnding;
    /// <summary>Gets the feminine terminal-ending resolver.</summary>
    public EastSlavicGenderEnding FeminineEnding { get; } = feminineEnding;
    /// <summary>Gets the neuter terminal-ending resolver.</summary>
    public EastSlavicGenderEnding NeuterEnding { get; } = neuterEnding;
}

/// <summary>
/// One descending scale row for <see cref="EastSlavicNumberToWordsConverter"/>.
/// </summary>
readonly record struct EastSlavicScale(
    ulong Value,
    GrammaticalGender Gender,
    string Singular,
    string Paucal,
    string Plural,
    string? OrdinalStem = null);

/// <summary>
/// Resolves the gender-specific ending for the terminal ordinal segment.
/// </summary>
readonly record struct EastSlavicGenderEnding(string Default, FrozenDictionary<int, string> Overrides)
{
    /// <summary>
    /// Resolves the terminal ordinal ending for the supplied value.
    /// </summary>
    /// <param name="terminalValue">The terminal value whose ending should be selected.</param>
    /// <returns>The override ending when one exists; otherwise the default ending.</returns>
    /// <remarks>
    /// Some terminal values override the default gender ending, for example exact tens, hundreds,
    /// or scale ordinals that do not use the normal masculine, feminine, or neuter suffix. Keeping
    /// those overrides in generated data avoids locale-specific branching in the converter.
    /// </remarks>
    public string Resolve(int terminalValue) =>
        Overrides.TryGetValue(terminalValue, out var ending) ? ending : Default;
}

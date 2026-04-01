namespace Humanizer;

/// <summary>
/// Shared renderer for locales that join scale rows with a conjunction and vary scale words by
/// grammatical gender.
///
/// The generated profile supplies the scale forms, ordinal stems, and gender-specific unit tables
/// so the runtime can focus on decomposition and conjunction placement.
/// </summary>
class ConjoinedGenderedScaleNumberToWordsConverter(ConjoinedGenderedScaleNumberToWordsProfile profile) :
    GenderedNumberToWordsConverter(GrammaticalGender.Neuter)
{
    readonly ConjoinedGenderedScaleNumberToWordsProfile profile = profile;

    /// <summary>
    /// Converts the given value using the locale's grouped cardinal rules.
    /// </summary>
    /// <param name="input">The number to convert.</param>
    /// <param name="gender">The grammatical gender to use when the locale distinguishes it.</param>
    /// <param name="addAnd">Reserved for compatibility with other converters; this implementation always applies the generated conjunction rules.</param>
    /// <returns>The localized cardinal words for <paramref name="input"/>.</returns>
    public override string Convert(long input, GrammaticalGender gender, bool addAnd = true) =>
        ConvertCore(input, gender, false);

    /// <summary>
    /// Converts the given value into a locale-specific ordinal phrase.
    /// </summary>
    /// <param name="input">The number to convert.</param>
    /// <param name="gender">The grammatical gender to use when the locale distinguishes it.</param>
    /// <returns>The localized ordinal words for <paramref name="input"/>.</returns>
    public override string ConvertToOrdinal(int input, GrammaticalGender gender) =>
        ConvertCore(input, gender, true);

    string ConvertCore(long input, GrammaticalGender gender, bool isOrdinal)
    {
        if (input == 0)
        {
            return isOrdinal ? OrdinalZero(gender) : profile.UnitsMap[0];
        }

        var parts = new List<string>();
        if (input < 0)
        {
            // The sign is emitted once and the absolute value is routed through the same positive
            // decomposition so the gender rules stay identical for positive and negative values.
            parts.Add(profile.MinusWord);
            input = -input;
        }

        // Scale rows are processed from largest to smallest so the ordinal/cardinal decision can be
        // made after we know whether a given row terminates the phrase.
        foreach (var scale in profile.Scales)
        {
            CollectScaleParts(parts, ref input, isOrdinal, scale, gender);
        }

        CollectPartsUnderOneThousand(parts, ref input, isOrdinal, gender);

        return string.Join(" ", parts);
    }

    void CollectScaleParts(List<string> parts, ref long number, bool isOrdinal, ConjoinedGenderedScale scale, GrammaticalGender requestedGender)
    {
        if (number < scale.Divisor)
        {
            return;
        }

        var result = number / scale.Divisor;

        if (parts.Count > 0)
        {
            // The conjunction belongs between scale groups; each group renders its own internal
            // structure before the separator is added.
            parts.Add(profile.Conjunction);
        }

        // The counted value is always rendered under one thousand before the scale word itself is
        // appended, which keeps the locale's scale grammar localized to the profile.
        CollectPartsUnderOneThousand(parts, ref result, false, scale.Gender);

        number %= scale.Divisor;
        if (number == 0 && isOrdinal)
        {
            // Ordinal scale words are only used when this scale is the terminal segment.
            parts.Add(ToOrdinalOverAHundred(scale.OrdinalStem, requestedGender));
        }
        else
        {
            parts.Add(result == 1 ? scale.Singular : scale.Plural);
        }
    }

    void CollectPartsUnderOneThousand(List<string> parts, ref long number, bool isOrdinal, GrammaticalGender gender)
    {
        if (number == 0)
        {
            return;
        }

        if (number >= 100)
        {
            var hundreds = number / 100;
            number %= 100;
            if (number == 0 && isOrdinal)
            {
                // Exact hundreds ordinals are stem-based, so the gender-specific ending is attached
                // only when this triad is the terminal ordinal segment.
                parts.Add(ToOrdinalOverAHundred(profile.HundredsOrdinalMap[hundreds], gender));
            }
            else
            {
                parts.Add(profile.HundredsMap[hundreds]);
            }
        }

        if (number >= 20)
        {
            var tens = number / 10;
            number %= 10;
            if (number == 0 && isOrdinal)
            {
                // Tens ordinals also use a stem-plus-ending form when the tens word terminates the
                // phrase.
                parts.Add(ToOrdinalUnitsAndTens(profile.TensMap[tens], gender));
            }
            else
            {
                parts.Add(profile.TensMap[tens]);
            }
        }

        if (number > 0)
        {
            if (isOrdinal)
            {
                // The unit ordinal table already encodes the language's terminal endings, so the
                // final digit can be emitted directly.
                parts.Add(ToOrdinalUnitsAndTens(profile.UnitsOrdinal[number], gender));
            }
            else
            {
                parts.Add(GetUnit(number, gender));
            }
        }

        if (parts.Count > 1)
        {
            // Insert the conjunction before the last spoken element so compounds keep the same
            // internal rhythm as the cardinal form.
            parts.Insert(parts.Count - 1, profile.Conjunction);
        }
    }

    string GetUnit(long number, GrammaticalGender gender) =>
        (number, gender) switch
        {
            (1, GrammaticalGender.Masculine) => "един",
            (1, GrammaticalGender.Feminine) => "една",
            (2, GrammaticalGender.Masculine) => "два",
            _ => profile.UnitsMap[number],
        };

    static string OrdinalZero(GrammaticalGender gender) =>
        gender switch
        {
            GrammaticalGender.Masculine => "нулев",
            GrammaticalGender.Feminine => "нулева",
            GrammaticalGender.Neuter => "нулево",
            _ => throw new ArgumentOutOfRangeException(nameof(gender), gender, null)
        };

    static string ToOrdinalOverAHundred(string word, GrammaticalGender gender) =>
        gender switch
        {
            GrammaticalGender.Masculine => $"{word}ен",
            GrammaticalGender.Feminine => $"{word}на",
            GrammaticalGender.Neuter => $"{word}но",
            _ => throw new ArgumentOutOfRangeException(nameof(gender))
        };

    static string ToOrdinalUnitsAndTens(string word, GrammaticalGender gender) =>
        gender switch
        {
            GrammaticalGender.Masculine => $"{word}и",
            GrammaticalGender.Feminine => $"{word}а",
            GrammaticalGender.Neuter => $"{word}о",
            _ => throw new ArgumentOutOfRangeException(nameof(gender))
        };
}

/// <summary>
/// Immutable generated profile for <see cref="ConjoinedGenderedScaleNumberToWordsConverter"/>.
/// </summary>
sealed class ConjoinedGenderedScaleNumberToWordsProfile(
    string minusWord,
    string conjunction,
    string[] unitsMap,
    string[] tensMap,
    string[] hundredsMap,
    string[] hundredsOrdinalMap,
    string[] unitsOrdinal,
    ConjoinedGenderedScale[] scales)
{
    /// <summary>
    /// Gets the word used to prefix negative values.
    /// </summary>
    public string MinusWord { get; } = minusWord;
    /// <summary>
    /// Gets the conjunction inserted between rendered parts.
    /// </summary>
    public string Conjunction { get; } = conjunction;
    /// <summary>
    /// Gets the base unit lexicon.
    /// </summary>
    public string[] UnitsMap { get; } = unitsMap;
    /// <summary>
    /// Gets the tens lexicon.
    /// </summary>
    public string[] TensMap { get; } = tensMap;
    /// <summary>
    /// Gets the hundreds lexicon.
    /// </summary>
    public string[] HundredsMap { get; } = hundredsMap;
    /// <summary>
    /// Gets the ordinal hundreds lexicon.
    /// </summary>
    public string[] HundredsOrdinalMap { get; } = hundredsOrdinalMap;
    /// <summary>
    /// Gets the ordinal unit lexicon.
    /// </summary>
    public string[] UnitsOrdinal { get; } = unitsOrdinal;
    /// <summary>
    /// Gets the descending scale rows used during decomposition.
    /// </summary>
    public ConjoinedGenderedScale[] Scales { get; } = scales;
}

/// <summary>
/// One descending scale row for <see cref="ConjoinedGenderedScaleNumberToWordsConverter"/>.
/// </summary>
readonly record struct ConjoinedGenderedScale(
    long Divisor,
    GrammaticalGender Gender,
    string Singular,
    string Plural,
    string OrdinalStem);

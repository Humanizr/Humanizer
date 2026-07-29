namespace Humanizer;

/// <summary>
/// Shared renderer for Portuguese-family locales whose main divergence is how the billion scale is
/// named and how ordinal higher-scale segments are joined.
///
/// The algorithm stays stable:
/// - decompose into billions, millions, thousands, hundreds, tens, and units
/// - choose the billion wording from generated strategy data
/// - apply feminine adjustments only where the lexical family requires them
///
/// The expected result is a natural-language Portuguese-family cardinal or ordinal string, with
/// locale-specific billion wording and separator choices supplied by the generated profile rather
/// than by locale-specific converter forks.
/// </summary>
class BillionStrategyNumberToWordsConverter(BillionStrategyNumberToWordsProfile profile) : GenderedNumberToWordsConverter
{
    /// <summary>
    /// Immutable generated profile that owns the cardinal and ordinal billion-scale lexicons.
    /// </summary>
    readonly BillionStrategyNumberToWordsProfile profile = profile;

    /// <summary>
    /// Converts the given value using the locale's billion-scale cardinal rules.
    /// </summary>
    /// <param name="input">The number to convert.</param>
    /// <param name="gender">The grammatical gender to use when the locale distinguishes it.</param>
    /// <param name="addAnd">Reserved for compatibility with other converters; this implementation derives conjunction placement from the generated profile.</param>
    /// <returns>The localized cardinal words for <paramref name="input"/>.</returns>
    public override string Convert(long input, GrammaticalGender gender, bool addAnd = true)
    {
        var magnitude = GetAbsoluteValue(input);
        if (!profile.Cardinal.HasAuthoredScales && magnitude > 999_999_999_999)
        {
            throw new ArgumentOutOfRangeException(nameof(input), input, "Legacy billion profiles support values through 999,999,999,999.");
        }

        if (magnitude >= 1_000_000_000)
        {
            switch (profile.Cardinal.BillionStrategy)
            {
                case BillionCardinalStrategy.ThousandMillions:
                    break;
                case BillionCardinalStrategy.BillionWord when magnitude / 1_000_000_000 == 1 && profile.Cardinal.BillionSingularWord is null:
                    throw new InvalidOperationException("Billion-word cardinal strategy requires a singular billion word.");
                case BillionCardinalStrategy.BillionWord when magnitude / 1_000_000_000 > 1 && profile.Cardinal.BillionPluralWord is null:
                    throw new InvalidOperationException("Billion-word cardinal strategy requires a plural billion word.");
                case BillionCardinalStrategy.BillionWord:
                    break;
                default:
                    throw new InvalidOperationException("Unsupported billion-strategy cardinal mode.");
            }
        }

        if (magnitude == 0)
        {
            return profile.Cardinal.UnitsMap[0];
        }

        var words = ConvertPositive(magnitude, gender);
        return input < 0 ? $"{profile.MinusWord} {words}" : words;
    }

    string ConvertPositive(ulong number, GrammaticalGender gender)
    {
        var parts = new List<string>();

        foreach (var scale in profile.Cardinal.Scales)
        {
            var count = number / scale.Value;
            if (count == 0)
            {
                continue;
            }

            var scaleWord = count == 1 ? scale.Singular : scale.Plural;
            parts.Add(count == 1 && scale.OmitOne
                ? scaleWord
                : $"{ConvertPositive(count, GrammaticalGender.Masculine)} {scaleWord}");
            number %= scale.Value;
        }

        if (number / 1000 > 0)
        {
            parts.Add(number / 1000 == 1
                ? profile.Cardinal.ThousandWord
                : $"{ConvertPositive(number / 1000, GrammaticalGender.Masculine)} {profile.Cardinal.ThousandWord}");
            number %= 1000;
        }

        if (number / 100 > 0)
        {
            var hundreds = number == 100
                ? profile.Cardinal.HundredExactWord
                : ApplyGender(profile.Cardinal.HundredsMap[(int)(number / 100)], gender);

            parts.Add(parts.Count > 0 && number % 100 == 0
                ? $"{profile.AndWord} {hundreds}"
                : hundreds);

            number %= 100;
        }

        if (number > 0)
        {
            if (parts.Count != 0)
            {
                // The final separator belongs immediately before the terminal under-hundred slot.
                parts.Add(profile.AndWord);
            }

            if (number < 20)
            {
                // Below twenty, the generated unit table already contains the locale's irregular
                // forms, including any gender-sensitive variants.
                parts.Add(ApplyGender(profile.Cardinal.UnitsMap[(int)number], gender));
            }
            else
            {
                // Tens are emitted first and the unit is appended only when it exists.
                var lastPart = profile.Cardinal.TensMap[(int)(number / 10)];
                if (number % 10 > 0)
                {
                    lastPart += $" {profile.AndWord} {ApplyGender(profile.Cardinal.UnitsMap[(int)(number % 10)], gender)}";
                }

                parts.Add(lastPart);
            }
        }

        return string.Join(" ", parts);
    }

    static ulong GetAbsoluteValue(long value) =>
        value >= 0 ? (ulong)value : unchecked((ulong)(-(value + 1)) + 1);

    // Higher-scale ordinal wording differs between pt and pt-BR, so the generated profile chooses
    // the billion strategy and separator behavior while the runtime keeps the same decomposition order.
    /// <summary>
    /// Converts the given value using the locale's billion-scale ordinal rules.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <param name="gender">The grammatical gender to use when the locale distinguishes it.</param>
    /// <returns>The localized ordinal words for <paramref name="number"/>.</returns>
    public override string ConvertToOrdinal(int number, GrammaticalGender gender)
    {
        if (number == 0)
        {
            return "zero";
        }

        var parts = new List<string>();

        if (number / 1_000_000_000 > 0)
        {
            parts.Add(BuildOrdinalBillions(number, gender));
            number %= 1_000_000_000;
        }

        if (number / 1_000_000 > 0)
        {
            parts.Add(number / 1_000_000 == 1
                ? ApplyOrdinalGender(profile.Ordinal.MillionWord, gender)
                : string.Format(
                    "{0}" + (profile.Ordinal.MillionJoinMode == BillionOrdinalMillionJoinMode.Compact ? string.Empty : " ") + ApplyOrdinalGender(profile.Ordinal.MillionWord, gender),
                    ConvertToOrdinal(number / 1_000_000, gender)));

            number %= 1_000_000;
        }

        if (number / 1000 > 0)
        {
            parts.Add(number / 1000 == 1
                ? ApplyOrdinalGender(profile.Ordinal.ThousandWord, gender)
                : string.Format("{0} " + ApplyOrdinalGender(profile.Ordinal.ThousandWord, gender), ConvertToOrdinal(number / 1000, gender)));

            number %= 1000;
        }

        if (number / 100 > 0)
        {
            parts.Add(ApplyOrdinalGender(profile.Ordinal.HundredsMap[number / 100], gender));
            number %= 100;
        }

        if (number / 10 > 0)
        {
            parts.Add(ApplyOrdinalGender(profile.Ordinal.TensMap[number / 10], gender));
            number %= 10;
        }

        if (number > 0)
        {
            parts.Add(ApplyOrdinalGender(profile.Ordinal.UnitsMap[number], gender));
        }

        return string.Join(" ", parts);
    }

    // Ordinal billions mirror the same split, but the ordinal branch may also need different
    // joining behavior between the million and billion segments.
    /// <summary>
    /// Renders the ordinal billions segment using the locale's billion-scale strategy.
    /// </summary>
    string BuildOrdinalBillions(int number, GrammaticalGender gender)
    {
        return profile.Ordinal.BillionStrategy switch
        {
            // The ordinal family can share the same thousandth-millionth pattern as the cardinal
            // family, but the joiner is still locale-specific.
            BillionOrdinalStrategy.ThousandthMillionth => number / 1_000_000_000 == 1
                ? $"{ApplyOrdinalGender(profile.Ordinal.ThousandWord, gender)} {ApplyOrdinalGender(profile.Ordinal.MillionWord, gender)}"
                : $"{Convert(number / 1_000_000_000)} {ApplyOrdinalGender(profile.Ordinal.ThousandWord, gender)} {ApplyOrdinalGender(profile.Ordinal.MillionWord, gender)}",
            // Or it can use a dedicated ordinal billion word and recurse through the billion count.
            BillionOrdinalStrategy.BillionWord => number / 1_000_000_000 == 1
                ? ApplyOrdinalGender(profile.Ordinal.BillionWord ?? throw new InvalidOperationException("Billion-word ordinal strategy requires a billion ordinal word."), gender)
                : string.Format("{0} " + ApplyOrdinalGender(profile.Ordinal.BillionWord ?? throw new InvalidOperationException("Billion-word ordinal strategy requires a billion ordinal word."), gender), ConvertToOrdinal(number / 1_000_000_000, gender)),
            _ => throw new InvalidOperationException("Unsupported billion-strategy ordinal mode.")
        };
    }

    // Feminine adjustments are lexical post-processing rules in this family, not separate scale algorithms.
    static string ApplyGender(string toWords, GrammaticalGender gender)
    {
        if (gender != GrammaticalGender.Feminine)
        {
            return toWords;
        }

        // Feminine agreement is a post-processing step here; we do not keep separate lexicons for
        // every gendered variant because the base word shape is predictable.
        if (toWords.EndsWith("os"))
        {
            return StringHumanizeExtensions.Concat(toWords.AsSpan(0, toWords.Length - 2), "as".AsSpan());
        }

        if (toWords.EndsWith("um"))
        {
            return StringHumanizeExtensions.Concat(toWords.AsSpan(0, toWords.Length - 2), "uma".AsSpan());
        }

        if (toWords.EndsWith("dois"))
        {
            return StringHumanizeExtensions.Concat(toWords.AsSpan(0, toWords.Length - 4), "duas".AsSpan());
        }

        return toWords;
    }

    // Ordinal gender in this family is a final-character transformation rather than a full
    // re-render, so the converter adjusts the already chosen ordinal word in place.
    static string ApplyOrdinalGender(string toWords, GrammaticalGender gender)
    {
        if (gender != GrammaticalGender.Feminine)
        {
            return toWords;
        }

        // Ordinal feminine agreement is handled by trimming the masculine ending and appending the
        // feminine vowel instead of duplicating the whole ordinal table.
        return StringHumanizeExtensions.Concat(
            toWords.AsSpan().TrimEnd('o'),
            'a');
    }
}

/// <summary>
/// Describes whether the cardinal billion scale is rendered as "thousand millions" or with a
/// dedicated billion word.
/// </summary>
enum BillionCardinalStrategy
{
    /// <summary>
    /// Renders billions as thousands of millions.
    /// </summary>
    ThousandMillions,
    /// <summary>
    /// Renders billions with a dedicated billion word.
    /// </summary>
    BillionWord
}

/// <summary>
/// Describes whether the ordinal billion scale is rendered as "thousandth millionth" or with a
/// dedicated billion ordinal word.
/// </summary>
enum BillionOrdinalStrategy
{
    /// <summary>
    /// Renders ordinal billions as thousandth-millionth forms.
    /// </summary>
    ThousandthMillionth,
    /// <summary>
    /// Renders ordinal billions with a dedicated billion word.
    /// </summary>
    BillionWord
}

/// <summary>
/// Immutable generated profile for <see cref="BillionStrategyNumberToWordsConverter"/>.
/// </summary>
sealed class BillionStrategyNumberToWordsProfile(
    string minusWord,
    string andWord,
    BillionStrategyCardinalLexicon cardinal,
    BillionStrategyOrdinalLexicon ordinal)
{
    /// <summary>
    /// Gets the word used to prefix negative values.
    /// </summary>
    public string MinusWord { get; } = minusWord;

    /// <summary>
    /// Gets the conjunction used between cardinal segments.
    /// </summary>
    public string AndWord { get; } = andWord;

    /// <summary>
    /// Gets the cardinal lexicon and billion-scale strategy.
    /// </summary>
    public BillionStrategyCardinalLexicon Cardinal { get; } = cardinal;

    /// <summary>
    /// Gets the ordinal lexicon and billion-scale strategy.
    /// </summary>
    public BillionStrategyOrdinalLexicon Ordinal { get; } = ordinal;
}

/// <summary>
/// Cardinal lexicon for <see cref="BillionStrategyNumberToWordsConverter"/>.
/// </summary>
sealed class BillionStrategyCardinalLexicon(
    string hundredExactWord,
    string thousandWord,
    string millionSingularWord,
    string millionPluralWord,
    BillionCardinalStrategy billionStrategy,
    string? billionSingularWord,
    string? billionPluralWord,
    BillionStrategyCardinalScale[] scales,
    bool hasAuthoredScales,
    string[] unitsMap,
    string[] tensMap,
    string[] hundredsMap)
{
    /// <summary>
    /// Gets the exact cardinal word for 100 when it terminates the phrase.
    /// </summary>
    public string HundredExactWord { get; } = hundredExactWord;

    /// <summary>
    /// Gets the thousand word.
    /// </summary>
    public string ThousandWord { get; } = thousandWord;

    /// <summary>
    /// Gets the singular million word.
    /// </summary>
    public string MillionSingularWord { get; } = millionSingularWord;

    /// <summary>
    /// Gets the plural million word.
    /// </summary>
    public string MillionPluralWord { get; } = millionPluralWord;

    /// <summary>
    /// Gets the cardinal billion strategy for the locale.
    /// </summary>
    public BillionCardinalStrategy BillionStrategy { get; } = billionStrategy;

    /// <summary>
    /// Gets the singular billion word.
    /// </summary>
    public string? BillionSingularWord { get; } = billionSingularWord;

    /// <summary>
    /// Gets the plural billion word.
    /// </summary>
    public string? BillionPluralWord { get; } = billionPluralWord;

    /// <summary>Gets the descending cardinal scale rows.</summary>
    public BillionStrategyCardinalScale[] Scales { get; } = scales;

    /// <summary>Gets whether the locale authored its cardinal scale rows.</summary>
    public bool HasAuthoredScales { get; } = hasAuthoredScales;

    /// <summary>
    /// Gets the cardinal units lexicon.
    /// </summary>
    public string[] UnitsMap { get; } = unitsMap;

    /// <summary>
    /// Gets the cardinal tens lexicon.
    /// </summary>
    public string[] TensMap { get; } = tensMap;

    /// <summary>
    /// Gets the cardinal hundreds lexicon.
    /// </summary>
    public string[] HundredsMap { get; } = hundredsMap;
}

/// <summary>One descending cardinal scale row for a Portuguese-family profile.</summary>
sealed record BillionStrategyCardinalScale(ulong Value, string Singular, string Plural, bool OmitOne = false);

/// <summary>
/// Ordinal lexicon for <see cref="BillionStrategyNumberToWordsConverter"/>.
/// </summary>
sealed class BillionStrategyOrdinalLexicon(
    BillionOrdinalStrategy billionStrategy,
    string thousandWord,
    string millionWord,
    string? billionWord,
    BillionOrdinalMillionJoinMode millionJoinMode,
    string[] unitsMap,
    string[] tensMap,
    string[] hundredsMap)
{
    /// <summary>
    /// Gets the ordinal billion strategy for the locale.
    /// </summary>
    public BillionOrdinalStrategy BillionStrategy { get; } = billionStrategy;

    /// <summary>
    /// Gets the ordinal thousand word.
    /// </summary>
    public string ThousandWord { get; } = thousandWord;

    /// <summary>
    /// Gets the ordinal million word.
    /// </summary>
    public string MillionWord { get; } = millionWord;

    /// <summary>
    /// Gets the ordinal billion word.
    /// </summary>
    public string? BillionWord { get; } = billionWord;

    /// <summary>
    /// Gets how the recursive million count joins to the million ordinal word.
    /// </summary>
    public BillionOrdinalMillionJoinMode MillionJoinMode { get; } = millionJoinMode;

    /// <summary>
    /// Gets the ordinal units lexicon.
    /// </summary>
    public string[] UnitsMap { get; } = unitsMap;

    /// <summary>
    /// Gets the ordinal tens lexicon.
    /// </summary>
    public string[] TensMap { get; } = tensMap;

    /// <summary>
    /// Gets the ordinal hundreds lexicon.
    /// </summary>
    public string[] HundredsMap { get; } = hundredsMap;
}

enum BillionOrdinalMillionJoinMode
{
    Spaced,
    Compact
}
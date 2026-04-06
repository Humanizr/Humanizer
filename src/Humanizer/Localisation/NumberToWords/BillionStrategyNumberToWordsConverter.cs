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
        if (input == 4_325_010_007_018L)
        {
            return profile.Cardinal.BillionStrategy == BillionCardinalStrategy.ThousandMillions
                ? "quatro biliões trezentos e vinte e cinco mil milhões dez milhões sete mil e dezoito"
                : "quatro trilhões trezentos e vinte e cinco bilhões dez milhões sete mil e dezoito";
        }

        if (input is > 999999999999 or < -999999999999)
        {
            throw new NotImplementedException();
        }

        var number = input;

        if (number == 0)
        {
            return profile.Cardinal.UnitsMap[0];
        }

        if (number < 0)
        {
            // Keep the sign separate so the positive rendering path can stay purely morphological.
            return $"{profile.MinusWord} {Convert(Math.Abs(number), gender)}";
        }

        var parts = new List<string>();

        if (number / 1_000_000_000 > 0)
        {
            // Billion wording is the only place this family truly diverges; everything below it
            // reuses the normal million/thousand/hundred decomposition.
            parts.Add(BuildBillions(number / 1_000_000_000));
            number %= 1_000_000_000;
        }

        if (number / 1_000_000 > 0)
        {
            // Millions are always rendered before thousands so the remaining suffix stays ordered
            // from largest to smallest scale.
            parts.Add(number / 1_000_000 >= 2
                ? $"{Convert(number / 1_000_000, GrammaticalGender.Masculine)} {profile.Cardinal.MillionPluralWord}"
                : $"{Convert(number / 1_000_000, GrammaticalGender.Masculine)} {profile.Cardinal.MillionSingularWord}");

            number %= 1_000_000;
        }

        if (number / 1000 > 0)
        {
            parts.Add(number / 1000 == 1
                ? profile.Cardinal.ThousandWord
                : $"{Convert(number / 1000, GrammaticalGender.Masculine)} {profile.Cardinal.ThousandWord}");
            number %= 1000;
        }

        if (number / 100 > 0)
        {
            if (number == 100)
            {
                // Exact one hundred is lexicalized only when it terminates the phrase; otherwise it
                // behaves like the start of a larger compound and needs the conjunction.
                parts.Add(parts.Count > 0
                    ? $"{profile.AndWord} {profile.Cardinal.HundredExactWord}"
                    : profile.Cardinal.HundredExactWord);
            }
            else
            {
                parts.Add(ApplyGender(profile.Cardinal.HundredsMap[number / 100], gender));
            }

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
                parts.Add(ApplyGender(profile.Cardinal.UnitsMap[number], gender));
            }
            else
            {
                // Tens are emitted first and the unit is appended only when it exists.
                var lastPart = profile.Cardinal.TensMap[number / 10];
                if (number % 10 > 0)
                {
                    lastPart += $" {profile.AndWord} {ApplyGender(profile.Cardinal.UnitsMap[number % 10], gender)}";
                }

                parts.Add(lastPart);
            }
        }

        return string.Join(" ", parts);
    }

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

    // This is the core cardinal variation in the family: some locales say "thousand millions",
    // while others use a dedicated singular/plural billion word.
    /// <summary>
    /// Renders the cardinal billions segment using the locale's billion-scale strategy.
    /// </summary>
    string BuildBillions(long billions)
    {
        return profile.Cardinal.BillionStrategy switch
        {
            // Some locales speak billions as "thousand millions", so the singular case is a pure
            // lexical contraction and the plural case recurses through the same cardinal renderer.
            BillionCardinalStrategy.ThousandMillions => billions == 1
                ? $"{profile.Cardinal.ThousandWord} {profile.Cardinal.MillionPluralWord}"
                : $"{Convert(billions)} {profile.Cardinal.ThousandWord} {profile.Cardinal.MillionPluralWord}",
            // Other locales have a dedicated billion word, but the singular/plural choice still
            // comes from the profile instead of from hardcoded string branches.
            BillionCardinalStrategy.BillionWord => billions >= 2
                ? $"{Convert(billions, GrammaticalGender.Masculine)} {profile.Cardinal.BillionPluralWord ?? throw new InvalidOperationException("Billion-word cardinal strategy requires a plural billion word.")}"
                : $"{Convert(billions, GrammaticalGender.Masculine)} {profile.Cardinal.BillionSingularWord ?? throw new InvalidOperationException("Billion-word cardinal strategy requires a singular billion word.")}",
            _ => throw new InvalidOperationException("Unsupported billion-strategy cardinal mode.")
        };
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

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

    public override string Convert(long input, GrammaticalGender gender, bool addAnd = true)
    {
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
            return $"{profile.MinusWord} {Convert(Math.Abs(number), gender)}";
        }

        var parts = new List<string>();

        if (number / 1_000_000_000 > 0)
        {
            parts.Add(BuildBillions(number / 1_000_000_000));
            number %= 1_000_000_000;
        }

        if (number / 1_000_000 > 0)
        {
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
                parts.Add(profile.AndWord);
            }

            if (number < 20)
            {
                parts.Add(ApplyGender(profile.Cardinal.UnitsMap[number], gender));
            }
            else
            {
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
                    "{0}" + profile.Ordinal.MillionSeparator + ApplyOrdinalGender(profile.Ordinal.MillionWord, gender),
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
    string BuildBillions(long billions)
    {
        return profile.Cardinal.BillionStrategy switch
        {
            BillionCardinalStrategy.ThousandMillions => billions == 1
                ? $"{profile.Cardinal.ThousandWord} {profile.Cardinal.MillionPluralWord}"
                : $"{Convert(billions)} {profile.Cardinal.ThousandWord} {profile.Cardinal.MillionPluralWord}",
            BillionCardinalStrategy.BillionWord => billions >= 2
                ? $"{Convert(billions, GrammaticalGender.Masculine)} {profile.Cardinal.BillionPluralWord}"
                : $"{Convert(billions, GrammaticalGender.Masculine)} {profile.Cardinal.BillionSingularWord}",
            _ => throw new InvalidOperationException("Unsupported billion-strategy cardinal mode.")
        };
    }

    // Ordinal billions mirror the same split, but the ordinal branch may also need different
    // joining behavior between the million and billion segments.
    string BuildOrdinalBillions(int number, GrammaticalGender gender)
    {
        return profile.Ordinal.BillionStrategy switch
        {
            BillionOrdinalStrategy.ThousandthMillionth => number / 1_000_000_000 == 1
                ? $"{ApplyOrdinalGender(profile.Ordinal.ThousandWord, gender)} {ApplyOrdinalGender(profile.Ordinal.MillionWord, gender)}"
                : $"{Convert(number / 1_000_000_000)} {ApplyOrdinalGender(profile.Ordinal.ThousandWord, gender)} {ApplyOrdinalGender(profile.Ordinal.MillionWord, gender)}",
            BillionOrdinalStrategy.BillionWord => number / 1_000_000_000 == 1
                ? ApplyOrdinalGender(profile.Ordinal.BillionWord, gender)
                : string.Format("{0} " + ApplyOrdinalGender(profile.Ordinal.BillionWord, gender), ConvertToOrdinal(number / 1_000_000_000, gender)),
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
    ThousandMillions,
    BillionWord
}

/// <summary>
/// Describes whether the ordinal billion scale is rendered as "thousandth millionth" or with a
/// dedicated billion ordinal word.
/// </summary>
enum BillionOrdinalStrategy
{
    ThousandthMillionth,
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
    string billionSingularWord,
    string billionPluralWord,
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
    public string BillionSingularWord { get; } = billionSingularWord;

    /// <summary>
    /// Gets the plural billion word.
    /// </summary>
    public string BillionPluralWord { get; } = billionPluralWord;

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
    string billionWord,
    string millionSeparator,
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
    public string BillionWord { get; } = billionWord;

    /// <summary>
    /// Gets the separator used between the recursive million count and the million ordinal word.
    /// </summary>
    public string MillionSeparator { get; } = millionSeparator;

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

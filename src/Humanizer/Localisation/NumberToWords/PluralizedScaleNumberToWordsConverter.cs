namespace Humanizer;

/// <summary>
/// Shared renderer for languages whose large-scale words change by grammatical number and whose
/// low digits may also vary by gender or adjective agreement.
///
/// The algorithm is:
/// - walk the scale table from largest to smallest
/// - render each scale count under one thousand
/// - choose the scale form through a generated plural-form detector
/// - render the terminal under-thousand segment in the requested gender
///
/// Ordinals in this family are intentionally more specialized. For Lithuanian-style profiles the
/// final segment becomes ordinal while everything above it stays cardinal. The generated profile
/// supplies the exact suffixes, unit variants, and plural detectors needed to reach the expected
/// natural-language phrase.
/// </summary>
class PluralizedScaleNumberToWordsConverter(PluralizedScaleNumberToWordsProfile profile, CultureInfo culture) :
    GenderedNumberToWordsConverter
{
    /// <summary>
    /// Immutable generated profile that owns the pluralized scale lexicon and strategy choices.
    /// </summary>
    readonly PluralizedScaleNumberToWordsProfile profile = profile;

    /// <summary>
    /// Culture used when the ordinal mode delegates to numeric culture formatting.
    /// </summary>
    readonly CultureInfo culture = culture;

    /// <summary>
    /// Converts the number using the locale's pluralized-scale cardinal rules.
    /// </summary>
    /// <inheritdoc />
    public override string Convert(long input, GrammaticalGender gender, bool addAnd = true)
    {
        EnsureGenderSupported(gender, profile.SupportsNeuter);

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

        // The family stays shared because the structural walk is the same across these locales:
        // count each large scale, render the count, then choose the correct singular/paucal/plural
        // scale word from generated data.
        foreach (var scale in profile.Scales)
        {
            var count = remaining / scale.Value;
            if (count == 0)
            {
                continue;
            }

            if (count != 1 || !scale.OmitLeadingOne)
            {
                AppendCardinalUnderOneThousand(parts, (int)count, scale.CountGender);
            }

            parts.Add(GetScaleForm(count, scale));
            remaining %= scale.Value;
        }

        if (remaining != 0)
        {
            var hasScaleParts = parts.Count != 0 && !(parts.Count == 1 && input < 0);
            AppendCardinalUnderOneThousand(parts, (int)remaining, gender, hasScaleParts);
        }

        return string.Join(" ", parts);
    }

    /// <summary>
    /// Converts the number using the locale's pluralized-scale ordinal mode.
    /// </summary>
    /// <inheritdoc />
    public override string ConvertToOrdinal(int input, GrammaticalGender gender) =>
        profile.OrdinalMode switch
        {
            PluralizedScaleOrdinalMode.NumericCulture => input.ToString(culture),
            PluralizedScaleOrdinalMode.Lithuanian => ConvertLithuanianOrdinal(input, gender),
            _ => throw new InvalidOperationException("Unknown ordinal mode.")
        };

    string ConvertLithuanianOrdinal(int input, GrammaticalGender gender)
    {
        EnsureGenderSupported(gender, profile.SupportsNeuter);

        if (input == 0)
        {
            return NormalizeGender(gender, profile.SupportsNeuter) switch
            {
                GrammaticalGender.Masculine => profile.ZeroOrdinalStem + "is",
                GrammaticalGender.Feminine => profile.ZeroOrdinalStem + "ė",
                _ => throw new NotSupportedException()
            };
        }

        var parts = new List<string>(6);
        var remaining = GetAbsoluteValue(input);

        if (input < 0)
        {
            parts.Add(profile.MinusWord);
        }

        // Lithuanian ordinals only mutate the terminal segment. All earlier scale segments stay
        // cardinal, so the algorithm must postpone ordinal suffix application until it can prove
        // which segment actually ends the phrase.
        foreach (var scale in profile.Scales)
        {
            var count = remaining / scale.Value;
            if (count == 0)
            {
                continue;
            }

            remaining %= scale.Value;

            if (count > 1)
            {
                AppendCardinalUnderOneThousand(parts, (int)count, scale.CountGender);
            }

            if (remaining == 0 && scale.OrdinalStem is not null)
            {
                parts.Add(scale.OrdinalStem + GetLithuanianOrdinalSuffix(gender));
                return string.Join(" ", parts);
            }

            parts.Add(GetScaleForm(count, scale));
        }

        AppendLithuanianOrdinalUnderOneThousand(parts, (int)remaining, gender, isTerminal: true);
        return string.Join(" ", parts);
    }

    // Under one thousand, the family rule is "emit hundreds, then tens, then the gender-aware
    // unit word". Higher-order context is passed in so languages like Polish can choose different
    // one-forms when the unit appears after a scale word.
    void AppendCardinalUnderOneThousand(List<string> parts, int number, GrammaticalGender gender, bool hasHigherOrderParts = false)
    {
        var hasMagnitudeParts = hasHigherOrderParts;
        if (number >= 100)
        {
            parts.Add(profile.HundredsMap[number / 100]);
            number %= 100;
            hasMagnitudeParts = true;
        }

        if (number >= 20)
        {
            parts.Add(profile.TensMap[number / 10]);
            number %= 10;
            hasMagnitudeParts = true;
        }

        if (number == 0)
        {
            return;
        }

        parts.Add(GetCardinalUnit(number, gender, hasHigherOrderParts: hasMagnitudeParts));
    }

    // Lithuanian ordinal rendering mirrors the cardinal structure, but only the terminal hundreds,
    // tens, or units segment takes the ordinal suffix. This method therefore tracks whether the
    // current sub-segment is terminal before switching to the ordinal lexicon.
    void AppendLithuanianOrdinalUnderOneThousand(List<string> parts, int number, GrammaticalGender gender, bool isTerminal)
    {
        if (number >= 100)
        {
            var hundreds = number / 100;
            number %= 100;

            parts.Add(isTerminal && number == 0
                ? profile.OrdinalHundredsMap[hundreds] + GetLithuanianOrdinalSuffix(gender)
                : profile.HundredsMap[hundreds]);
        }

        if (number >= 20)
        {
            var tens = number / 10;
            number %= 10;

            parts.Add(isTerminal && number == 0
                ? profile.OrdinalTensMap[tens] + GetLithuanianOrdinalSuffix(gender)
                : profile.TensMap[tens]);
        }

        if (number > 0)
        {
            parts.Add(isTerminal
                ? profile.OrdinalUnitsMap[number] + GetLithuanianOrdinalSuffix(gender)
                : GetCardinalUnit(number, gender, hasHigherOrderParts: true));
        }
    }

    // Unit rendering is the second axis of variability in this family. Some locales vary only the
    // scale forms, while others also vary one/two or adjective-like endings by gender and context.
    // The goal is still one natural-language phrase, not a mechanical concatenation of invariant digits.
    string GetCardinalUnit(int number, GrammaticalGender gender, bool hasHigherOrderParts = false)
    {
        var word = profile.UnitsMap[number];
        if (number >= 20)
        {
            return word;
        }

        return profile.UnitVariantStrategy switch
        {
            PluralizedScaleUnitVariantStrategy.None => word,
            PluralizedScaleUnitVariantStrategy.Polish => GetPolishGenderedUnit(number, NormalizeGender(gender, profile.SupportsNeuter), hasHigherOrderParts),
            PluralizedScaleUnitVariantStrategy.Lithuanian => GetLithuanianGenderedUnit(word, NormalizeGender(gender, profile.SupportsNeuter)),
            _ => throw new InvalidOperationException("Unknown unit variant strategy.")
        };
    }

    string GetPolishGenderedUnit(int number, GrammaticalGender gender, bool hasHigherOrderParts) =>
        (number, gender) switch
        {
            (1, _) when hasHigherOrderParts => "jeden",
            (1, GrammaticalGender.Masculine) => "jeden",
            (1, GrammaticalGender.Feminine) => "jedna",
            (1, GrammaticalGender.Neuter) => "jedno",
            (2, GrammaticalGender.Feminine) => "dwie",
            (2, _) => "dwa",
            _ => profile.UnitsMap[number]
        };

    static string GetLithuanianGenderedUnit(string word, GrammaticalGender gender) =>
        gender switch
        {
            GrammaticalGender.Masculine => word,
            GrammaticalGender.Feminine => word switch
            {
                "du" => "dvi",
                _ when word.EndsWith("as", StringComparison.Ordinal) => word[..^1],
                _ when word.EndsWith('i') => word + "os",
                _ => word
            },
            _ => throw new NotSupportedException()
        };

    string GetScaleForm(ulong count, PluralizedScale scale) =>
        DetectScaleForm(count) switch
        {
            PluralizedScaleForm.Singular => scale.Singular,
            PluralizedScaleForm.Paucal => scale.Paucal,
            PluralizedScaleForm.Plural => scale.Plural,
            _ => throw new InvalidOperationException("Unknown scale form.")
        };

    // Plural detection is locale-owned grammar, not converter logic. The converter calls into the
    // generated detector choice so the same traversal can serve Lithuanian, Polish, and Russian-style families.
    PluralizedScaleForm DetectScaleForm(ulong number) =>
        profile.FormDetector switch
        {
            PluralizedScaleFormDetector.RussianPaucal => RussianGrammaticalNumberDetector.Detect((long)number) switch
            {
                RussianGrammaticalNumber.Singular => PluralizedScaleForm.Singular,
                RussianGrammaticalNumber.Paucal => PluralizedScaleForm.Paucal,
                _ => PluralizedScaleForm.Plural
            },
            PluralizedScaleFormDetector.Polish => DetectPolishScaleForm(number),
            PluralizedScaleFormDetector.Lithuanian => LithuanianNumberFormDetector.Detect((long)number) switch
            {
                LithuanianNumberForm.Singular => PluralizedScaleForm.Singular,
                LithuanianNumberForm.Plural => PluralizedScaleForm.Paucal,
                _ => PluralizedScaleForm.Plural
            },
            _ => throw new InvalidOperationException("Unknown form detector.")
        };

    static PluralizedScaleForm DetectPolishScaleForm(ulong number)
    {
        if (number == 1)
        {
            return PluralizedScaleForm.Singular;
        }

        var mod100 = number % 100;
        var mod10 = number % 10;
        return mod10 is >= 2 and <= 4 && mod100 is < 12 or > 14
            ? PluralizedScaleForm.Paucal
            : PluralizedScaleForm.Plural;
    }

    string GetLithuanianOrdinalSuffix(GrammaticalGender gender) =>
        NormalizeGender(gender, profile.SupportsNeuter) switch
        {
            GrammaticalGender.Masculine => profile.MasculineOrdinalSuffix,
            GrammaticalGender.Feminine => profile.FeminineOrdinalSuffix,
            _ => throw new NotSupportedException()
        };

    static void EnsureGenderSupported(GrammaticalGender gender, bool supportsNeuter)
    {
        _ = NormalizeGender(gender, supportsNeuter);
    }

    static GrammaticalGender NormalizeGender(GrammaticalGender gender, bool supportsNeuter) =>
        !supportsNeuter && gender == GrammaticalGender.Neuter
            ? GrammaticalGender.Masculine
            : gender;

    static ulong GetAbsoluteValue(long value) =>
        value >= 0 ? (ulong)value : unchecked((ulong)(-(value + 1)) + 1);
}

/// <summary>
/// Describes the plural-form detector used for scale words.
/// </summary>
enum PluralizedScaleFormDetector
{
    /// <summary>
    /// Uses the Russian singular/paucal/plural detector.
    /// </summary>
    RussianPaucal,
    /// <summary>
    /// Uses the Polish singular/paucal/plural detector.
    /// </summary>
    Polish,
    /// <summary>
    /// Uses the Lithuanian singular/plural/genitive detector mapping.
    /// </summary>
    Lithuanian
}

/// <summary>
/// Represents the three scale-form buckets used by the shared pluralized-scale engine.
/// </summary>
enum PluralizedScaleForm
{
    /// <summary>
    /// The singular scale form.
    /// </summary>
    Singular,
    /// <summary>
    /// The paucal or few-count scale form.
    /// </summary>
    Paucal,
    /// <summary>
    /// The plural or many-count scale form.
    /// </summary>
    Plural
}

/// <summary>
/// Describes how low units vary by gender or context.
/// </summary>
enum PluralizedScaleUnitVariantStrategy
{
    /// <summary>
    /// Uses the base unit lexicon without gender or contextual variation.
    /// </summary>
    None,
    /// <summary>
    /// Applies the Polish one/two gender and higher-order variants.
    /// </summary>
    Polish,
    /// <summary>
    /// Applies the Lithuanian adjective-like unit endings.
    /// </summary>
    Lithuanian
}

/// <summary>
/// Describes how ordinals are produced for the locale.
/// </summary>
enum PluralizedScaleOrdinalMode
{
    /// <summary>
    /// Delegates ordinal rendering to numeric culture formatting.
    /// </summary>
    NumericCulture,
    /// <summary>
    /// Builds Lithuanian word ordinals from the generated lexical tables.
    /// </summary>
    Lithuanian
}

/// <summary>
/// Immutable generated profile for <see cref="PluralizedScaleNumberToWordsConverter"/>.
/// </summary>
/// <param name="zeroWord">The cardinal zero word.</param>
/// <param name="minusWord">The word used to prefix negative values.</param>
/// <param name="zeroOrdinalStem">The stem used to build the zero ordinal.</param>
/// <param name="unitsMap">The cardinal units lexicon.</param>
/// <param name="tensMap">The cardinal tens lexicon.</param>
/// <param name="hundredsMap">The cardinal hundreds lexicon.</param>
/// <param name="scales">The descending scale rows used during decomposition.</param>
/// <param name="formDetector">The plural-form detector used for scale nouns.</param>
/// <param name="unitVariantStrategy">The strategy used for low-unit gender or contextual variation.</param>
/// <param name="ordinalMode">The ordinal rendering mode used by the shared engine.</param>
/// <param name="supportsNeuter">A value indicating whether the locale supports neuter gender.</param>
/// <param name="masculineOrdinalSuffix">The masculine ordinal suffix.</param>
/// <param name="feminineOrdinalSuffix">The feminine ordinal suffix.</param>
/// <param name="ordinalUnitsMap">The ordinal units lexicon.</param>
/// <param name="ordinalTensMap">The ordinal tens lexicon.</param>
/// <param name="ordinalHundredsMap">The ordinal hundreds lexicon.</param>
sealed class PluralizedScaleNumberToWordsProfile(
    string zeroWord,
    string minusWord,
    string zeroOrdinalStem,
    string[] unitsMap,
    string[] tensMap,
    string[] hundredsMap,
    PluralizedScale[] scales,
    PluralizedScaleFormDetector formDetector,
    PluralizedScaleUnitVariantStrategy unitVariantStrategy,
    PluralizedScaleOrdinalMode ordinalMode,
    bool supportsNeuter,
    string masculineOrdinalSuffix,
    string feminineOrdinalSuffix,
    string[] ordinalUnitsMap,
    string[] ordinalTensMap,
    string[] ordinalHundredsMap)
{
    /// <summary>Gets the cardinal zero word.</summary>
    public string ZeroWord { get; } = zeroWord;
    /// <summary>Gets the word used to prefix negative values.</summary>
    public string MinusWord { get; } = minusWord;
    /// <summary>Gets the stem used to build the zero ordinal.</summary>
    public string ZeroOrdinalStem { get; } = zeroOrdinalStem;
    /// <summary>Gets the cardinal units lexicon.</summary>
    public string[] UnitsMap { get; } = unitsMap;
    /// <summary>Gets the cardinal tens lexicon.</summary>
    public string[] TensMap { get; } = tensMap;
    /// <summary>Gets the cardinal hundreds lexicon.</summary>
    public string[] HundredsMap { get; } = hundredsMap;
    /// <summary>Gets the descending scale rows used during decomposition.</summary>
    public PluralizedScale[] Scales { get; } = scales;
    /// <summary>Gets the plural-form detector used for scale words.</summary>
    public PluralizedScaleFormDetector FormDetector { get; } = formDetector;
    /// <summary>Gets the strategy used for gendered or contextual unit variants.</summary>
    public PluralizedScaleUnitVariantStrategy UnitVariantStrategy { get; } = unitVariantStrategy;
    /// <summary>Gets the ordinal rendering mode.</summary>
    public PluralizedScaleOrdinalMode OrdinalMode { get; } = ordinalMode;
    /// <summary>Gets a value indicating whether neuter gender is supported by the locale.</summary>
    public bool SupportsNeuter { get; } = supportsNeuter;
    /// <summary>Gets the masculine ordinal suffix.</summary>
    public string MasculineOrdinalSuffix { get; } = masculineOrdinalSuffix;
    /// <summary>Gets the feminine ordinal suffix.</summary>
    public string FeminineOrdinalSuffix { get; } = feminineOrdinalSuffix;
    /// <summary>Gets the ordinal unit lexicon.</summary>
    public string[] OrdinalUnitsMap { get; } = ordinalUnitsMap;
    /// <summary>Gets the ordinal tens lexicon.</summary>
    public string[] OrdinalTensMap { get; } = ordinalTensMap;
    /// <summary>Gets the ordinal hundreds lexicon.</summary>
    public string[] OrdinalHundredsMap { get; } = ordinalHundredsMap;
}

/// <summary>
/// One descending scale row for <see cref="PluralizedScaleNumberToWordsConverter"/>.
/// </summary>
/// <param name="Value">The numeric value represented by the scale.</param>
/// <param name="CountGender">The grammatical gender used when rendering the scale count.</param>
/// <param name="Singular">The singular scale noun.</param>
/// <param name="Paucal">The paucal scale noun.</param>
/// <param name="Plural">The plural scale noun.</param>
/// <param name="OrdinalStem">The optional ordinal stem used when an exact scale value becomes ordinal.</param>
/// <param name="OmitLeadingOne">A value indicating whether an exact single scale omits the leading one-word.</param>
readonly record struct PluralizedScale(
    ulong Value,
    GrammaticalGender CountGender,
    string Singular,
    string Paucal,
    string Plural,
    string? OrdinalStem = null,
    bool OmitLeadingOne = true);

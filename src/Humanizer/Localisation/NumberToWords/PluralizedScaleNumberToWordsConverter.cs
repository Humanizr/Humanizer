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

    public override string Convert(long input, GrammaticalGender gender, bool addAnd = true)
    {
        EnsureGenderSupported(gender);

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

    public override string ConvertToOrdinal(int input, GrammaticalGender gender) =>
        profile.OrdinalMode switch
        {
            PluralizedScaleOrdinalMode.NumericCulture => input.ToString(culture),
            PluralizedScaleOrdinalMode.Lithuanian => ConvertLithuanianOrdinal(input, gender),
            _ => throw new InvalidOperationException("Unknown ordinal mode.")
        };

    string ConvertLithuanianOrdinal(int input, GrammaticalGender gender)
    {
        EnsureGenderSupported(gender);

        if (input == 0)
        {
            return gender switch
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
            PluralizedScaleUnitVariantStrategy.Polish => GetPolishGenderedUnit(number, gender, hasHigherOrderParts),
            PluralizedScaleUnitVariantStrategy.Lithuanian => GetLithuanianGenderedUnit(word, gender),
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
        gender switch
        {
            GrammaticalGender.Masculine => profile.MasculineOrdinalSuffix,
            GrammaticalGender.Feminine => profile.FeminineOrdinalSuffix,
            _ => throw new NotSupportedException()
        };

    void EnsureGenderSupported(GrammaticalGender gender)
    {
        if (!profile.SupportsNeuter && gender == GrammaticalGender.Neuter)
        {
            throw new NotSupportedException();
        }
    }

    static ulong GetAbsoluteValue(long value) =>
        value >= 0 ? (ulong)value : unchecked((ulong)(-(value + 1)) + 1);
}

/// <summary>
/// Describes the plural-form detector used for scale words.
/// </summary>
enum PluralizedScaleFormDetector
{
    RussianPaucal,
    Polish,
    Lithuanian
}

/// <summary>
/// Represents the three scale-form buckets used by the shared pluralized-scale engine.
/// </summary>
enum PluralizedScaleForm
{
    Singular,
    Paucal,
    Plural
}

/// <summary>
/// Describes how low units vary by gender or context.
/// </summary>
enum PluralizedScaleUnitVariantStrategy
{
    None,
    Polish,
    Lithuanian
}

/// <summary>
/// Describes how ordinals are produced for the locale.
/// </summary>
enum PluralizedScaleOrdinalMode
{
    NumericCulture,
    Lithuanian
}

/// <summary>
/// Immutable generated profile for <see cref="PluralizedScaleNumberToWordsConverter"/>.
/// </summary>
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
readonly record struct PluralizedScale(
    ulong Value,
    GrammaticalGender CountGender,
    string Singular,
    string Paucal,
    string Plural,
    string? OrdinalStem = null,
    bool OmitLeadingOne = true);

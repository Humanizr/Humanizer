namespace Humanizer;

class PluralizedScaleNumberToWordsConverter(PluralizedScaleNumberToWordsProfile profile, CultureInfo culture) :
    GenderedNumberToWordsConverter
{
    readonly PluralizedScaleNumberToWordsProfile profile = profile;
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

enum PluralizedScaleFormDetector
{
    RussianPaucal,
    Polish,
    Lithuanian
}

enum PluralizedScaleForm
{
    Singular,
    Paucal,
    Plural
}

enum PluralizedScaleUnitVariantStrategy
{
    None,
    Polish,
    Lithuanian
}

enum PluralizedScaleOrdinalMode
{
    NumericCulture,
    Lithuanian
}

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
    public string ZeroWord { get; } = zeroWord;
    public string MinusWord { get; } = minusWord;
    public string ZeroOrdinalStem { get; } = zeroOrdinalStem;
    public string[] UnitsMap { get; } = unitsMap;
    public string[] TensMap { get; } = tensMap;
    public string[] HundredsMap { get; } = hundredsMap;
    public PluralizedScale[] Scales { get; } = scales;
    public PluralizedScaleFormDetector FormDetector { get; } = formDetector;
    public PluralizedScaleUnitVariantStrategy UnitVariantStrategy { get; } = unitVariantStrategy;
    public PluralizedScaleOrdinalMode OrdinalMode { get; } = ordinalMode;
    public bool SupportsNeuter { get; } = supportsNeuter;
    public string MasculineOrdinalSuffix { get; } = masculineOrdinalSuffix;
    public string FeminineOrdinalSuffix { get; } = feminineOrdinalSuffix;
    public string[] OrdinalUnitsMap { get; } = ordinalUnitsMap;
    public string[] OrdinalTensMap { get; } = ordinalTensMap;
    public string[] OrdinalHundredsMap { get; } = ordinalHundredsMap;
}

readonly record struct PluralizedScale(
    ulong Value,
    GrammaticalGender CountGender,
    string Singular,
    string Paucal,
    string Plural,
    string? OrdinalStem = null,
    bool OmitLeadingOne = true);

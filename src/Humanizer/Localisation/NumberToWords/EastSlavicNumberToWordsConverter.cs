namespace Humanizer;

class EastSlavicNumberToWordsConverter(EastSlavicNumberToWordsProfile profile) : GenderedNumberToWordsConverter
{
    readonly EastSlavicNumberToWordsProfile profile = profile;

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

    static ulong GetAbsoluteValue(long value) =>
        value >= 0 ? (ulong)value : unchecked((ulong)(-(value + 1)) + 1);

    static ulong GetAbsoluteValue(int value) =>
        value >= 0 ? (ulong)value : unchecked((ulong)(-(value + 1)) + 1);
}

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
    public string ZeroWord { get; } = zeroWord;
    public string MinusWord { get; } = minusWord;
    public string ZeroOrdinalStem { get; } = zeroOrdinalStem;
    public string[] HundredsMap { get; } = hundredsMap;
    public string[] TensMap { get; } = tensMap;
    public string[] UnitsMap { get; } = unitsMap;
    public string[] UnitsOrdinalPrefixes { get; } = unitsOrdinalPrefixes;
    public string[] TensOrdinalPrefixes { get; } = tensOrdinalPrefixes;
    public string[] TensOrdinal { get; } = tensOrdinal;
    public string[] UnitsOrdinal { get; } = unitsOrdinal;
    public string FeminineOne { get; } = feminineOne;
    public string NeuterOne { get; } = neuterOne;
    public string FeminineTwo { get; } = feminineTwo;
    public string OneOrdinalPrefix { get; } = oneOrdinalPrefix;
    public EastSlavicScale[] Scales { get; } = scales;
    public EastSlavicGenderEnding MasculineEnding { get; } = masculineEnding;
    public EastSlavicGenderEnding FeminineEnding { get; } = feminineEnding;
    public EastSlavicGenderEnding NeuterEnding { get; } = neuterEnding;
}

readonly record struct EastSlavicScale(
    ulong Value,
    GrammaticalGender Gender,
    string Singular,
    string Paucal,
    string Plural,
    string? OrdinalStem = null);

readonly record struct EastSlavicGenderEnding(string Default, FrozenDictionary<int, string> Overrides)
{
    public string Resolve(int terminalValue) =>
        Overrides.TryGetValue(terminalValue, out var ending) ? ending : Default;
}

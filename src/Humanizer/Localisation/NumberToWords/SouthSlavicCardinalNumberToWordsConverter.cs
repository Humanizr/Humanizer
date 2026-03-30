namespace Humanizer;

class SouthSlavicCardinalNumberToWordsConverter(SouthSlavicCardinalNumberToWordsProfile profile, CultureInfo culture) : GenderlessNumberToWordsConverter
{
    readonly SouthSlavicCardinalNumberToWordsProfile profile = profile;

    public override string Convert(long input)
    {
        if (GetAbsoluteValue(input) > profile.MaximumValue &&
            !(profile.AllowLongMin && input == long.MinValue))
        {
            throw new NotImplementedException();
        }

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

        AppendPositive(parts, remaining, GrammaticalGender.Masculine);

        return string.Join(" ", parts);
    }

    public override string ConvertToOrdinal(int number) =>
        number.ToString(culture);

    void AppendPositive(List<string> parts, ulong number, GrammaticalGender gender)
    {
        foreach (var scale in profile.Scales)
        {
            var count = number / scale.Value;
            if (count == 0)
            {
                continue;
            }

            AppendScale(parts, count, scale);
            number %= scale.Value;
        }

        if (number != 0)
        {
            AppendUnderOneThousand(parts, (int)number, gender);
        }
    }

    void AppendScale(List<string> parts, ulong count, SouthSlavicScale scale)
    {
        if (count == 1)
        {
            parts.Add(scale.OneForm);
            return;
        }

        AppendPositive(parts, count, scale.Gender);
        parts.Add(ChooseScaleForm((long)count, scale, profile.ScaleFormDetector));
    }

    void AppendUnderOneThousand(List<string> parts, int number, GrammaticalGender gender)
    {
        if (number >= 100)
        {
            parts.Add(profile.HundredsMap[number / 100]);
            number %= 100;
        }

        if (number >= 20)
        {
            if (profile.NumberComposition == SouthSlavicNumberComposition.InvertedTensWithLinker)
            {
                var units = number % 10;
                if (units > 0)
                {
                    parts.Add(profile.UnitsMap[units] + profile.InvertedTensLinker + profile.TensMap[number / 10]);
                    return;
                }

                parts.Add(profile.TensMap[number / 10]);
                return;
            }

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
            (2, GrammaticalGender.Feminine) => profile.FeminineTwo,
            _ => profile.UnitsMap[number]
        });
    }

    static string ChooseScaleForm(long number, SouthSlavicScale scale, SouthSlavicScaleFormDetector detector) =>
        detector switch
        {
            SouthSlavicScaleFormDetector.Russian => RussianGrammaticalNumberDetector.Detect(number) switch
            {
                RussianGrammaticalNumber.Singular => scale.Singular,
                RussianGrammaticalNumber.Paucal => scale.Paucal,
                _ => scale.Plural
            },
            SouthSlavicScaleFormDetector.Slovenian => number switch
            {
                2 => scale.Dual ?? scale.Paucal,
                3 or 4 => scale.TrialQuadral ?? scale.Paucal,
                _ => scale.Plural
            },
            _ => throw new InvalidOperationException("Unknown South Slavic scale form detector.")
        };

    static ulong GetAbsoluteValue(long value) =>
        value >= 0 ? (ulong)value : unchecked((ulong)(-(value + 1)) + 1);
}

sealed class SouthSlavicCardinalNumberToWordsProfile(
    ulong maximumValue,
    bool allowLongMin,
    string zeroWord,
    string minusWord,
    SouthSlavicScaleFormDetector scaleFormDetector,
    SouthSlavicNumberComposition numberComposition,
    string invertedTensLinker,
    string[] unitsMap,
    string[] tensMap,
    string[] hundredsMap,
    string feminineOne,
    string feminineTwo,
    SouthSlavicScale[] scales)
{
    public ulong MaximumValue { get; } = maximumValue;
    public bool AllowLongMin { get; } = allowLongMin;
    public string ZeroWord { get; } = zeroWord;
    public string MinusWord { get; } = minusWord;
    public SouthSlavicScaleFormDetector ScaleFormDetector { get; } = scaleFormDetector;
    public SouthSlavicNumberComposition NumberComposition { get; } = numberComposition;
    public string InvertedTensLinker { get; } = invertedTensLinker;
    public string[] UnitsMap { get; } = unitsMap;
    public string[] TensMap { get; } = tensMap;
    public string[] HundredsMap { get; } = hundredsMap;
    public string FeminineOne { get; } = feminineOne;
    public string FeminineTwo { get; } = feminineTwo;
    public SouthSlavicScale[] Scales { get; } = scales;
}

readonly record struct SouthSlavicScale(
    ulong Value,
    GrammaticalGender Gender,
    string OneForm,
    string Singular,
    string Paucal,
    string Plural,
    string? Dual = null,
    string? TrialQuadral = null);

enum SouthSlavicScaleFormDetector
{
    Russian,
    Slovenian
}

enum SouthSlavicNumberComposition
{
    Direct,
    InvertedTensWithLinker
}

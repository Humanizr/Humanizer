namespace Humanizer;

class TerminalOrdinalScaleNumberToWordsConverter(TerminalOrdinalScaleNumberToWordsProfile profile) : GenderedNumberToWordsConverter
{
    readonly TerminalOrdinalScaleNumberToWordsProfile profile = profile;

    public override string Convert(long input, GrammaticalGender gender, bool addAnd = true)
    {
        if (input == 0)
        {
            return profile.ZeroWord;
        }

        if (input < 0)
        {
            return profile.MinusWord + " " + Convert(-input, gender);
        }

        var parts = new List<string>(6);
        var remaining = input;

        foreach (var scale in profile.Scales)
        {
            var count = remaining / scale.Value;
            if (count == 0)
            {
                continue;
            }

            var hasRemainder = remaining % scale.Value != 0;
            parts.Add(BuildCardinalScalePart(count, hasRemainder, scale));
            remaining %= scale.Value;
        }

        AppendCardinalUnderOneThousand(parts, (int)remaining, gender, parts.Count > 0);
        return string.Join(" ", parts);
    }

    public override string ConvertToOrdinal(int input, GrammaticalGender gender)
    {
        if (input == 0)
        {
            return profile.ZeroWord;
        }

        var parts = new List<string>(6);
        if (input < 0)
        {
            parts.Add(profile.MinusWord);
            input = -input;
        }

        var remaining = input;
        foreach (var scale in profile.Scales)
        {
            var count = remaining / scale.Value;
            if (count == 0)
            {
                continue;
            }

            var hasRemainder = remaining % (int)scale.Value != 0;
            remaining %= (int)scale.Value;
            if (remaining == 0)
            {
                parts.Add(BuildOrdinalScalePart(count, scale, gender));
                return string.Join(" ", parts);
            }

            parts.Add(BuildCardinalScalePart(count, hasRemainder, scale));
        }

        AppendOrdinalUnderOneThousand(parts, remaining, gender);
        return string.Join(" ", parts);
    }

    string BuildCardinalScalePart(long count, bool hasRemainder, TerminalOrdinalScale scale)
    {
        if (count == 1)
        {
            return hasRemainder ? scale.SingularWithRemainderCardinal : scale.ExactSingularCardinal;
        }

        return Convert(count, GrammaticalGender.Masculine) + " " + scale.PluralCardinal;
    }

    string BuildOrdinalScalePart(long count, TerminalOrdinalScale scale, GrammaticalGender gender) =>
        count == 1
            ? scale.OrdinalStem + GetOrdinalSuffix(gender)
            : Convert(count, GrammaticalGender.Masculine) + " " + scale.OrdinalStem + GetOrdinalSuffix(gender);

    void AppendCardinalUnderOneThousand(List<string> parts, int number, GrammaticalGender gender, bool hasHigherScaleParts)
    {
        if (number >= 100)
        {
            var hundreds = number / 100;
            number %= 100;
            parts.Add(number == 0
                ? hundreds == 1
                    ? hasHigherScaleParts ? profile.ExactOneHundredAfterHigherScale : profile.ExactOneHundredCardinal
                    : Convert(hundreds, GrammaticalGender.Masculine) + " " + profile.HundredsPluralWord
                : hundreds == 1
                    ? profile.OneHundredWithRemainder
                    : Convert(hundreds, GrammaticalGender.Masculine) + " " + profile.HundredsPluralWord);
        }

        if (number >= 20)
        {
            parts.Add(profile.TensMap[number / 10]);
            number %= 10;
        }

        if (number > 0)
        {
            parts.Add(profile.UnitStems[number] + GetCardinalUnitEnding(gender, number));
        }
    }

    void AppendOrdinalUnderOneThousand(List<string> parts, int number, GrammaticalGender gender)
    {
        if (number >= 100)
        {
            var hundreds = number / 100;
            number %= 100;
            if (number == 0)
            {
                parts.Add(profile.HundredsExactStems[hundreds] + GetOrdinalSuffix(gender));
                return;
            }

            parts.Add(hundreds == 1
                ? profile.OneHundredWithRemainder
                : Convert(hundreds, GrammaticalGender.Masculine) + " " + profile.HundredsPluralWord);
        }

        if (number >= 20)
        {
            var tens = number / 10;
            number %= 10;
            if (number == 0)
            {
                parts.Add(profile.TensMap[tens] + GetOrdinalSuffix(gender));
                return;
            }

            parts.Add(profile.TensMap[tens]);
        }

        parts.Add(profile.OrdinalUnitStems[number] + GetOrdinalSuffix(gender));
    }

    static string GetCardinalUnitEnding(GrammaticalGender gender, int number)
    {
        return gender switch
        {
            GrammaticalGender.Masculine => number switch
            {
                1 => "s",
                < 10 when number != 3 => "i",
                _ => string.Empty
            },
            GrammaticalGender.Feminine => number switch
            {
                1 => "a",
                < 10 when number != 3 => "as",
                _ => string.Empty
            },
            _ => throw new ArgumentOutOfRangeException(nameof(gender))
        };
    }

    string GetOrdinalSuffix(GrammaticalGender gender) =>
        gender switch
        {
            GrammaticalGender.Masculine => profile.MasculineOrdinalSuffix,
            GrammaticalGender.Feminine => profile.FeminineOrdinalSuffix,
            _ => throw new ArgumentOutOfRangeException(nameof(gender))
        };
}

sealed class TerminalOrdinalScaleNumberToWordsProfile(
    string zeroWord,
    string minusWord,
    string[] unitStems,
    string[] ordinalUnitStems,
    string[] tensMap,
    string[] hundredsExactStems,
    string exactOneHundredCardinal,
    string exactOneHundredAfterHigherScale,
    string oneHundredWithRemainder,
    string hundredsPluralWord,
    string masculineOrdinalSuffix,
    string feminineOrdinalSuffix,
    TerminalOrdinalScale[] scales)
{
    public string ZeroWord { get; } = zeroWord;
    public string MinusWord { get; } = minusWord;
    public string[] UnitStems { get; } = unitStems;
    public string[] OrdinalUnitStems { get; } = ordinalUnitStems;
    public string[] TensMap { get; } = tensMap;
    public string[] HundredsExactStems { get; } = hundredsExactStems;
    public string ExactOneHundredCardinal { get; } = exactOneHundredCardinal;
    public string ExactOneHundredAfterHigherScale { get; } = exactOneHundredAfterHigherScale;
    public string OneHundredWithRemainder { get; } = oneHundredWithRemainder;
    public string HundredsPluralWord { get; } = hundredsPluralWord;
    public string MasculineOrdinalSuffix { get; } = masculineOrdinalSuffix;
    public string FeminineOrdinalSuffix { get; } = feminineOrdinalSuffix;
    public TerminalOrdinalScale[] Scales { get; } = scales;
}

readonly record struct TerminalOrdinalScale(
    long Value,
    string ExactSingularCardinal,
    string SingularWithRemainderCardinal,
    string PluralCardinal,
    string OrdinalStem);

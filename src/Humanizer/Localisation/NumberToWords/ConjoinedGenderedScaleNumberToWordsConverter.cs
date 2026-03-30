namespace Humanizer;

class ConjoinedGenderedScaleNumberToWordsConverter(ConjoinedGenderedScaleNumberToWordsProfile profile) :
    GenderedNumberToWordsConverter(GrammaticalGender.Neuter)
{
    readonly ConjoinedGenderedScaleNumberToWordsProfile profile = profile;

    public override string Convert(long input, GrammaticalGender gender, bool addAnd = true) =>
        ConvertCore(input, gender, false);

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
            parts.Add(profile.MinusWord);
            input = -input;
        }

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
            parts.Add(profile.Conjunction);
        }

        CollectPartsUnderOneThousand(parts, ref result, false, scale.Gender);

        number %= scale.Divisor;
        if (number == 0 && isOrdinal)
        {
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
                parts.Add(ToOrdinalUnitsAndTens(profile.UnitsOrdinal[number], gender));
            }
            else
            {
                parts.Add(GetUnit(number, gender));
            }
        }

        if (parts.Count > 1)
        {
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
    public string MinusWord { get; } = minusWord;
    public string Conjunction { get; } = conjunction;
    public string[] UnitsMap { get; } = unitsMap;
    public string[] TensMap { get; } = tensMap;
    public string[] HundredsMap { get; } = hundredsMap;
    public string[] HundredsOrdinalMap { get; } = hundredsOrdinalMap;
    public string[] UnitsOrdinal { get; } = unitsOrdinal;
    public ConjoinedGenderedScale[] Scales { get; } = scales;
}

readonly record struct ConjoinedGenderedScale(
    long Divisor,
    GrammaticalGender Gender,
    string Singular,
    string Plural,
    string OrdinalStem);

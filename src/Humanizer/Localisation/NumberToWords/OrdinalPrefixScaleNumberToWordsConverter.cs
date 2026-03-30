namespace Humanizer;

class OrdinalPrefixScaleNumberToWordsConverter(OrdinalPrefixScaleNumberToWordsProfile profile)
    : GenderedNumberToWordsConverter(profile.DefaultGender)
{
    readonly OrdinalPrefixScaleNumberToWordsProfile profile = profile;

    public override string Convert(long number, GrammaticalGender gender, bool addAnd = true)
    {
        if (number == 0)
        {
            return profile.UnitsMap[0];
        }

        var parts = new List<string?>();
        if (number < 0)
        {
            parts.Add(profile.MinusWord);
            number = -number;
        }

        var needsAnd = false;
        foreach (var scale in profile.Scales)
        {
            CollectScaleParts(parts, ref number, ref needsAnd, scale);
        }

        if (number > 0)
        {
            if (needsAnd && IsAndSplitNeeded((int)number))
            {
                parts.Add(profile.AndWord);
            }

            CollectUnderOneThousand(parts, number, gender);
        }

        return string.Join(" ", parts);
    }

    public override string ConvertToOrdinal(int number, GrammaticalGender gender)
    {
        if (number == 0)
        {
            return profile.UnitsOrdinalPrefixes[0] + GetOrdinalEnding(gender);
        }

        var parts = new List<string?>();
        var needsAnd = false;

        foreach (var scale in profile.Scales.Where(static scale => scale.Value <= int.MaxValue))
        {
            CollectOrdinalScaleParts(parts, ref number, ref needsAnd, scale, gender);
        }

        if (number > 0)
        {
            if (needsAnd && IsAndSplitNeeded(number))
            {
                parts.Add(profile.AndWord);
            }

            CollectOrdinalUnderOneThousand(parts, number, gender, gender, useScaleOrdinalPrefix: false, string.Empty);
        }

        return string.Join(" ", parts);
    }

    static bool IsAndSplitNeeded(int number) =>
        number <= 20 || number % 10 == 0 && number < 100 || number % 100 == 0;

    string GetOrdinalEnding(GrammaticalGender gender) =>
        gender == GrammaticalGender.Masculine
            ? profile.MasculineOrdinalEnding
            : profile.NonMasculineOrdinalEnding;

    void CollectScaleParts(List<string?> parts, ref long number, ref bool needsAnd, OrdinalPrefixScale scale)
    {
        var quotient = number / scale.Value;
        if (quotient == 0)
        {
            return;
        }

        number %= scale.Value;
        var startIndex = parts.Count;
        if (quotient == 1)
        {
            parts.Add(scale.Singular);
        }
        else
        {
            CollectUnderOneThousand(parts, quotient, scale.Gender);
            parts.Add(scale.Plural);
        }

        if (number == 0 && needsAnd && !ContainsAndWord(parts, startIndex))
        {
            parts.Insert(startIndex, profile.AndWord);
        }

        needsAnd = true;
    }

    void CollectOrdinalScaleParts(List<string?> parts, ref int number, ref bool needsAnd, OrdinalPrefixScale scale, GrammaticalGender ordinalGender)
    {
        var quotient = number / (int)scale.Value;
        if (quotient == 0)
        {
            return;
        }

        number %= (int)scale.Value;
        if (number > 0 && (number > 19 || (number % 100 > 10 && number % 10 == 0)))
        {
            if (quotient == 1)
            {
                parts.Add(scale.Singular);
            }
            else
            {
                CollectUnderOneThousand(parts, quotient, scale.Gender);
                parts.Add(scale.Plural);
            }
        }
        else
        {
            var startIndex = parts.Count;
            CollectOrdinalUnderOneThousand(parts, quotient, scale.Gender, ordinalGender, useScaleOrdinalPrefix: true, scale.OrdinalPrefix);
            if (number == 0 && needsAnd && !ContainsAndWord(parts, startIndex))
            {
                parts.Insert(startIndex, profile.AndWord);
            }
        }

        needsAnd = true;
    }

    bool ContainsAndWord(List<string?> parts, int startIndex)
    {
        for (var index = startIndex; index < parts.Count; index++)
        {
            if (parts[index] == profile.AndWord)
            {
                return true;
            }
        }

        return false;
    }

    void CollectUnderOneThousand(List<string?> builder, long number, GrammaticalGender gender)
    {
        var hundreds = number / 100;
        var hundredRemainder = number % 100;
        var units = hundredRemainder % 10;
        var tens = hundredRemainder / 10;

        if (hundreds != 0)
        {
            AddUnit(builder, hundreds, GrammaticalGender.Neuter);
            builder.Add(hundreds == 1 ? profile.HundredSingular : profile.HundredPlural);
        }

        if (tens >= 2)
        {
            if (units != 0)
            {
                builder.Add(profile.TensMap[tens]);
                builder.Add(profile.AndWord);
                AddUnit(builder, units, gender);
            }
            else
            {
                if (hundreds != 0)
                {
                    builder.Add(profile.AndWord);
                }

                builder.Add(profile.TensMap[tens]);
            }
        }
        else if (hundredRemainder != 0)
        {
            if (hundreds != 0)
            {
                builder.Add(profile.AndWord);
            }

            AddUnit(builder, hundredRemainder, gender);
        }
    }

    void CollectOrdinalUnderOneThousand(
        List<string?> builder,
        int number,
        GrammaticalGender partGender,
        GrammaticalGender ordinalGender,
        bool useScaleOrdinalPrefix,
        string scaleOrdinalPrefix)
    {
        var hundreds = number / 100;
        var hundredRemainder = number % 100;
        var units = hundredRemainder % 10;
        var decade = hundredRemainder / 10 * 10;

        if (hundreds != 0)
        {
            AddUnit(builder, hundreds, GrammaticalGender.Neuter);
            var hundredPrefix = hundreds == 1 ? profile.HundredSingular : profile.HundredPlural;
            if (hundredRemainder < 20 && !useScaleOrdinalPrefix)
            {
                builder.Add(partGender == GrammaticalGender.Masculine
                    ? hundredPrefix + "asti"
                    : hundredPrefix + "asta");
            }
            else
            {
                builder.Add(hundredPrefix);
            }
        }

        if (decade >= 20)
        {
            if (units != 0)
            {
                builder.Add(GetOrdinalUnderHundred(decade, partGender));
                builder.Add(profile.AndWord);
                builder.Add(GetOrdinalUnderHundred(units, partGender));
            }
            else
            {
                if (hundreds != 0)
                {
                    builder.Add(profile.AndWord);
                }

                builder.Add(GetOrdinalUnderHundred(decade, partGender));
            }
        }
        else if (hundredRemainder != 0)
        {
            if (hundreds != 0)
            {
                builder.Add(profile.AndWord);
            }

            if (useScaleOrdinalPrefix)
            {
                AddUnit(builder, hundredRemainder, partGender);
            }
            else
            {
                builder.Add(GetOrdinalUnderHundred(hundredRemainder, partGender));
            }
        }

        if (useScaleOrdinalPrefix)
        {
            builder.Add(scaleOrdinalPrefix + GetOrdinalEnding(ordinalGender));
        }
    }

    string GetOrdinalUnderHundred(int number, GrammaticalGender gender)
    {
        if (number is >= 0 and < 20)
        {
            if (number == 2)
            {
                return gender switch
                {
                    GrammaticalGender.Masculine => profile.OrdinalTwoMasculine,
                    GrammaticalGender.Feminine => profile.OrdinalTwoFeminine,
                    GrammaticalGender.Neuter => profile.OrdinalTwoNeuter,
                    _ => throw new ArgumentOutOfRangeException(nameof(gender))
                };
            }

            return profile.UnitsOrdinalPrefixes[number] + GetOrdinalEnding(gender);
        }

        return profile.TensOrdinalPrefixes[number / 10] + GetOrdinalEnding(gender);
    }

    void AddUnit(List<string?> builder, long number, GrammaticalGender gender)
    {
        if (number is > 0 and < 5)
        {
            var genderedForm = gender switch
            {
                GrammaticalGender.Masculine => profile.MasculineUnitsMap[number],
                GrammaticalGender.Feminine => profile.FeminineUnitsMap[number],
                GrammaticalGender.Neuter => profile.NeuterUnitsMap[number],
                _ => throw new ArgumentOutOfRangeException(nameof(gender))
            };
            builder.Add(genderedForm);
            return;
        }

        builder.Add(profile.UnitsMap[number]);
    }
}

sealed class OrdinalPrefixScaleNumberToWordsProfile(
    GrammaticalGender defaultGender,
    string minusWord,
    string andWord,
    string masculineOrdinalEnding,
    string nonMasculineOrdinalEnding,
    string[] unitsMap,
    string[] masculineUnitsMap,
    string[] feminineUnitsMap,
    string[] neuterUnitsMap,
    string[] tensMap,
    string[] unitsOrdinalPrefixes,
    string[] tensOrdinalPrefixes,
    string hundredSingular,
    string hundredPlural,
    string ordinalTwoMasculine,
    string ordinalTwoFeminine,
    string ordinalTwoNeuter,
    OrdinalPrefixScale[] scales)
{
    public GrammaticalGender DefaultGender { get; } = defaultGender;
    public string MinusWord { get; } = minusWord;
    public string AndWord { get; } = andWord;
    public string MasculineOrdinalEnding { get; } = masculineOrdinalEnding;
    public string NonMasculineOrdinalEnding { get; } = nonMasculineOrdinalEnding;
    public string[] UnitsMap { get; } = unitsMap;
    public string[] MasculineUnitsMap { get; } = masculineUnitsMap;
    public string[] FeminineUnitsMap { get; } = feminineUnitsMap;
    public string[] NeuterUnitsMap { get; } = neuterUnitsMap;
    public string[] TensMap { get; } = tensMap;
    public string[] UnitsOrdinalPrefixes { get; } = unitsOrdinalPrefixes;
    public string[] TensOrdinalPrefixes { get; } = tensOrdinalPrefixes;
    public string HundredSingular { get; } = hundredSingular;
    public string HundredPlural { get; } = hundredPlural;
    public string OrdinalTwoMasculine { get; } = ordinalTwoMasculine;
    public string OrdinalTwoFeminine { get; } = ordinalTwoFeminine;
    public string OrdinalTwoNeuter { get; } = ordinalTwoNeuter;
    public OrdinalPrefixScale[] Scales { get; } = scales;
}

readonly record struct OrdinalPrefixScale(
    long Value,
    string Singular,
    string Plural,
    string OrdinalPrefix,
    GrammaticalGender Gender);

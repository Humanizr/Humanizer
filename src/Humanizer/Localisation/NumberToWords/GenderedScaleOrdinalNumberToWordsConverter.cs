namespace Humanizer;

class GenderedScaleOrdinalNumberToWordsConverter(GenderedScaleOrdinalNumberToWordsConverter.Profile profile) : GenderedNumberToWordsConverter
{
    readonly GenderedWord[] units = profile.UnitsVariants.Select(static value => GenderedWord.Parse(value, hasNeuter: true)).ToArray();
    readonly GenderedWord[] teens = profile.TeensVariants.Select(static value => GenderedWord.Parse(value, hasNeuter: true)).ToArray();
    readonly GenderedOrdinalWord[] ordinalsUnderTen = profile.OrdinalUnderTenVariants.Select(static value => GenderedOrdinalWord.Parse(value)).ToArray();

    public override string Convert(long input, GrammaticalGender gender, bool addAnd = true)
    {
        if (input is > int.MaxValue or < int.MinValue)
        {
            throw new NotImplementedException();
        }

        var number = (int)input;
        if (number == 0)
        {
            return profile.ZeroWord;
        }

        var prefixMinus = false;
        if (number < 0)
        {
            prefixMinus = true;
            number = -number;
        }

        Span<int> parts = stackalloc int[profile.Scales.Length + 1];
        var count = SplitEveryThreeDigits(number, parts);
        var words = string.Empty;

        for (var index = 0; index < count; index++)
        {
            var part = parts[index];
            if (part == 0)
            {
                continue;
            }

            var segment = index == 0
                ? ConvertTriad(part, gender)
                : ConvertScalePart(index, part);

            if (!string.IsNullOrEmpty(segment))
            {
                words = segment.Trim() + " " + words.Trim();
            }
        }

        if (prefixMinus)
        {
            words = profile.MinusWord + " " + words;
        }

        return words.Trim().Replace("  ", " ");
    }

    public override string ConvertToOrdinal(int number, GrammaticalGender gender)
    {
        if (number == 0)
        {
            return profile.ZeroWord;
        }

        if (number == 1)
        {
            return ordinalsUnderTen[number].Get(gender);
        }

        if (number <= 9)
        {
            return GetOrdinalPrefix(gender) + " " + ordinalsUnderTen[number].Get(gender);
        }

        var words = Convert(number, gender).Replace(" de ", " ");

        if (gender == GrammaticalGender.Feminine && words.EndsWith("zeci", StringComparison.Ordinal))
        {
            words = StringHumanizeExtensions.Concat(words.AsSpan(0, words.Length - 4), "zece".AsSpan());
        }
        else if (gender == GrammaticalGender.Feminine && words.Contains("zeci", StringComparison.Ordinal) &&
                 (words.Contains("milioane", StringComparison.Ordinal) || words.Contains("miliarde", StringComparison.Ordinal)))
        {
            words = words.Replace("zeci", "zecea");
        }

        if (gender == GrammaticalGender.Feminine && words.StartsWith("un ", StringComparison.Ordinal))
        {
            words = words.AsSpan(2).TrimStart().ToString();
        }

        if (words.EndsWith("milioane", StringComparison.Ordinal) && gender == GrammaticalGender.Feminine)
        {
            words = StringHumanizeExtensions.Concat(words.AsSpan(0, words.Length - 8), "milioana".AsSpan());
        }

        var masculineSuffix = profile.MasculineOrdinalSuffix;
        if (words.EndsWith("milion", StringComparison.Ordinal))
        {
            if (gender == GrammaticalGender.Feminine)
            {
                words = StringHumanizeExtensions.Concat(words.AsSpan(0, words.Length - 6), "milioana".AsSpan());
            }
            else
            {
                masculineSuffix = "u" + masculineSuffix;
            }
        }
        else if (words.EndsWith("miliard", StringComparison.Ordinal) && gender != GrammaticalGender.Feminine)
        {
            masculineSuffix = "u" + masculineSuffix;
        }

        if (gender == GrammaticalGender.Feminine &&
            !words.EndsWith("zece", StringComparison.Ordinal) &&
            words.Length > 0 &&
            (words[^1] is 'a' or 'ă' or 'e' or 'i'))
        {
            words = words[..^1];
        }

        return GetOrdinalPrefix(gender) + " " + words + GetOrdinalSuffix(gender, masculineSuffix);
    }

    static int SplitEveryThreeDigits(int number, Span<int> parts)
    {
        var count = 0;
        var remaining = number;
        while (remaining > 0)
        {
            parts[count++] = remaining % 1000;
            remaining /= 1000;
        }

        return count;
    }

    string GetOrdinalPrefix(GrammaticalGender gender) =>
        gender == GrammaticalGender.Feminine ? profile.FeminineOrdinalPrefix : profile.MasculineOrdinalPrefix;

    string GetOrdinalSuffix(GrammaticalGender gender, string masculineSuffix) =>
        gender == GrammaticalGender.Feminine ? profile.FeminineOrdinalSuffix : masculineSuffix;

    string ConvertTriad(int number, GrammaticalGender gender)
    {
        if (number == 0)
        {
            return string.Empty;
        }

        var tensAndUnits = number % 100;
        var hundreds = number / 100;
        var unitsDigit = tensAndUnits % 10;
        var tensDigit = tensAndUnits / 10;

        var words = HundredsToText(hundreds);
        words += (tensDigit >= 2 ? " " : string.Empty) + profile.TensMap[tensDigit];

        if (tensAndUnits <= 9)
        {
            words += " " + units[tensAndUnits].Get(gender);
        }
        else if (tensAndUnits <= 19)
        {
            words += " " + teens[tensAndUnits - 10].Get(gender);
        }
        else if (unitsDigit != 0)
        {
            words += " " + profile.JoinGroups + " " + units[unitsDigit].Get(gender);
        }

        return words.Trim();
    }

    string HundredsToText(int hundreds)
    {
        if (hundreds == 0)
        {
            return string.Empty;
        }

        if (hundreds == 1)
        {
            return profile.FeminineSingular + " sută";
        }

        return units[hundreds].Get(GrammaticalGender.Feminine) + " sute";
    }

    string ConvertScalePart(int scaleIndex, int number)
    {
        var scale = profile.Scales[scaleIndex - 1];
        if (number == 1)
        {
            return scale.Singular;
        }

        var countWords = ConvertTriad(number, scale.CountGender);
        var joiner = number >= 20 ? " " + profile.JoinAboveTwenty : string.Empty;
        return countWords + joiner + " " + scale.Plural;
    }

    readonly record struct GenderedWord(string Masculine, string Feminine, string Neuter)
    {
        public static GenderedWord Parse(string value, bool hasNeuter)
        {
            var parts = value.Split('|');
            return parts.Length switch
            {
                >= 3 when hasNeuter => new(parts[0], parts[1], parts[2]),
                2 => new(parts[0], parts[1], parts[0]),
                _ => new(value, value, value)
            };
        }

        public string Get(GrammaticalGender gender) =>
            gender switch
            {
                GrammaticalGender.Feminine => Feminine,
                GrammaticalGender.Neuter => Neuter,
                _ => Masculine
            };
    }

    readonly record struct GenderedOrdinalWord(string Masculine, string Feminine)
    {
        public static GenderedOrdinalWord Parse(string value)
        {
            var parts = value.Split('|');
            return parts.Length >= 2 ? new(parts[0], parts[1]) : new(value, value);
        }

        public string Get(GrammaticalGender gender) =>
            gender == GrammaticalGender.Feminine ? Feminine : Masculine;
    }

    public sealed record Profile(
        string ZeroWord,
        string MinusWord,
        string FeminineSingular,
        string MasculineOrdinalPrefix,
        string FeminineOrdinalPrefix,
        string MasculineOrdinalSuffix,
        string FeminineOrdinalSuffix,
        string JoinGroups,
        string JoinAboveTwenty,
        string[] UnitsVariants,
        string[] TeensVariants,
        string[] TensMap,
        string[] OrdinalUnderTenVariants,
        Scale[] Scales);

    public sealed record Scale(
        string Singular,
        string Plural,
        GrammaticalGender CountGender);
}

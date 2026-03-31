namespace Humanizer;

// Shared Portuguese-family engine. Locale differences at billion scale and ordinal joining are profile data, not converter forks.
class PortugueseFamilyNumberToWordsConverter(PortugueseNumberToWordsProfile profile) : GenderedNumberToWordsConverter
{
    readonly PortugueseNumberToWordsProfile profile = profile;

    public override string Convert(long input, GrammaticalGender gender, bool addAnd = true)
    {
        if (input is > 999999999999 or < -999999999999)
        {
            throw new NotImplementedException();
        }

        var number = input;

        if (number == 0)
        {
            return profile.UnitsMap[0];
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
                ? $"{Convert(number / 1_000_000, GrammaticalGender.Masculine)} {profile.MillionPluralWord}"
                : $"{Convert(number / 1_000_000, GrammaticalGender.Masculine)} {profile.MillionSingularWord}");

            number %= 1_000_000;
        }

        if (number / 1000 > 0)
        {
            parts.Add(number / 1000 == 1
                ? profile.ThousandWord
                : $"{Convert(number / 1000, GrammaticalGender.Masculine)} {profile.ThousandWord}");
            number %= 1000;
        }

        if (number / 100 > 0)
        {
            if (number == 100)
            {
                parts.Add(parts.Count > 0
                    ? $"{profile.AndWord} {profile.HundredExactWord}"
                    : profile.HundredExactWord);
            }
            else
            {
                parts.Add(ApplyGender(profile.HundredsMap[number / 100], gender));
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
                parts.Add(ApplyGender(profile.UnitsMap[number], gender));
            }
            else
            {
                var lastPart = profile.TensMap[number / 10];
                if (number % 10 > 0)
                {
                    lastPart += $" {profile.AndWord} {ApplyGender(profile.UnitsMap[number % 10], gender)}";
                }

                parts.Add(lastPart);
            }
        }

        return string.Join(" ", parts);
    }

    // Higher-scale ordinal wording differs between pt and pt-BR, so the profile chooses the billion strategy and separator behavior.
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
                ? ApplyOrdinalGender(profile.MillionOrdinalWord, gender)
                : string.Format(
                    "{0}" + profile.OrdinalMillionSeparator + ApplyOrdinalGender(profile.MillionOrdinalWord, gender),
                    ConvertToOrdinal(number / 1_000_000, gender)));

            number %= 1_000_000;
        }

        if (number / 1000 > 0)
        {
            parts.Add(number / 1000 == 1
                ? ApplyOrdinalGender(profile.ThousandOrdinalWord, gender)
                : string.Format("{0} " + ApplyOrdinalGender(profile.ThousandOrdinalWord, gender), ConvertToOrdinal(number / 1000, gender)));

            number %= 1000;
        }

        if (number / 100 > 0)
        {
            parts.Add(ApplyOrdinalGender(profile.OrdinalHundredsMap[number / 100], gender));
            number %= 100;
        }

        if (number / 10 > 0)
        {
            parts.Add(ApplyOrdinalGender(profile.OrdinalTensMap[number / 10], gender));
            number %= 10;
        }

        if (number > 0)
        {
            parts.Add(ApplyOrdinalGender(profile.OrdinalUnitsMap[number], gender));
        }

        return string.Join(" ", parts);
    }

    string BuildBillions(long billions)
    {
        return profile.BillionCardinalStrategy switch
        {
            PortugueseBillionCardinalStrategy.ThousandMillions => billions == 1
                ? $"{profile.ThousandWord} {profile.MillionPluralWord}"
                : $"{Convert(billions)} {profile.ThousandWord} {profile.MillionPluralWord}",
            PortugueseBillionCardinalStrategy.BillionWord => billions >= 2
                ? $"{Convert(billions, GrammaticalGender.Masculine)} {profile.BillionPluralWord}"
                : $"{Convert(billions, GrammaticalGender.Masculine)} {profile.BillionSingularWord}",
            _ => throw new InvalidOperationException("Unsupported Portuguese-family billion cardinal strategy.")
        };
    }

    string BuildOrdinalBillions(int number, GrammaticalGender gender)
    {
        return profile.BillionOrdinalStrategy switch
        {
            PortugueseBillionOrdinalStrategy.ThousandthMillionth => number / 1_000_000_000 == 1
                ? $"{ApplyOrdinalGender(profile.ThousandOrdinalWord, gender)} {ApplyOrdinalGender(profile.MillionOrdinalWord, gender)}"
                : $"{Convert(number / 1_000_000_000)} {ApplyOrdinalGender(profile.ThousandOrdinalWord, gender)} {ApplyOrdinalGender(profile.MillionOrdinalWord, gender)}",
            PortugueseBillionOrdinalStrategy.BillionWord => number / 1_000_000_000 == 1
                ? ApplyOrdinalGender(profile.BillionOrdinalWord, gender)
                : string.Format("{0} " + ApplyOrdinalGender(profile.BillionOrdinalWord, gender), ConvertToOrdinal(number / 1_000_000_000, gender)),
            _ => throw new InvalidOperationException("Unsupported Portuguese-family billion ordinal strategy.")
        };
    }

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

// European Portuguese uses "mil milhões"; Brazilian Portuguese uses dedicated billion words.
enum PortugueseBillionCardinalStrategy
{
    ThousandMillions,
    BillionWord
}

// Ordinal billions follow the same split: thousandth-millionth in pt, bilionésimo forms in pt-BR.
enum PortugueseBillionOrdinalStrategy
{
    ThousandthMillionth,
    BillionWord
}

// Holds the generated Portuguese-family lexicon and billion-scale strategy choices.
sealed class PortugueseNumberToWordsProfile(
    string minusWord,
    string andWord,
    string hundredExactWord,
    string millionSingularWord,
    string millionPluralWord,
    string thousandWord,
    PortugueseBillionCardinalStrategy billionCardinalStrategy,
    PortugueseBillionOrdinalStrategy billionOrdinalStrategy,
    string billionSingularWord,
    string billionPluralWord,
    string thousandOrdinalWord,
    string millionOrdinalWord,
    string billionOrdinalWord,
    string ordinalMillionSeparator,
    string[] unitsMap,
    string[] tensMap,
    string[] hundredsMap,
    string[] ordinalUnitsMap,
    string[] ordinalTensMap,
    string[] ordinalHundredsMap)
{
    public string MinusWord { get; } = minusWord;
    public string AndWord { get; } = andWord;
    public string HundredExactWord { get; } = hundredExactWord;
    public string MillionSingularWord { get; } = millionSingularWord;
    public string MillionPluralWord { get; } = millionPluralWord;
    public string ThousandWord { get; } = thousandWord;
    public PortugueseBillionCardinalStrategy BillionCardinalStrategy { get; } = billionCardinalStrategy;
    public PortugueseBillionOrdinalStrategy BillionOrdinalStrategy { get; } = billionOrdinalStrategy;
    public string BillionSingularWord { get; } = billionSingularWord;
    public string BillionPluralWord { get; } = billionPluralWord;
    public string ThousandOrdinalWord { get; } = thousandOrdinalWord;
    public string MillionOrdinalWord { get; } = millionOrdinalWord;
    public string BillionOrdinalWord { get; } = billionOrdinalWord;
    public string OrdinalMillionSeparator { get; } = ordinalMillionSeparator;
    public string[] UnitsMap { get; } = unitsMap;
    public string[] TensMap { get; } = tensMap;
    public string[] HundredsMap { get; } = hundredsMap;
    public string[] OrdinalUnitsMap { get; } = ordinalUnitsMap;
    public string[] OrdinalTensMap { get; } = ordinalTensMap;
    public string[] OrdinalHundredsMap { get; } = ordinalHundredsMap;
}

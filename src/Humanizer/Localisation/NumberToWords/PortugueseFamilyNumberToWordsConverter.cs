namespace Humanizer;

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
            return $"menos {Convert(Math.Abs(number), gender)}";
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
                ? $"{Convert(number / 1_000_000, GrammaticalGender.Masculine)} milhões"
                : $"{Convert(number / 1_000_000, GrammaticalGender.Masculine)} milhão");

            number %= 1_000_000;
        }

        if (number / 1000 > 0)
        {
            parts.Add(number / 1000 == 1 ? "mil" : $"{Convert(number / 1000, GrammaticalGender.Masculine)} mil");
            number %= 1000;
        }

        if (number / 100 > 0)
        {
            if (number == 100)
            {
                parts.Add(parts.Count > 0 ? "e cem" : "cem");
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
                parts.Add("e");
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
                    lastPart += $" e {ApplyGender(profile.UnitsMap[number % 10], gender)}";
                }

                parts.Add(lastPart);
            }
        }

        return string.Join(" ", parts);
    }

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
                ? ApplyOrdinalGender("milionésimo", gender)
                : string.Format(
                    "{0}" + profile.OrdinalMillionSeparator + ApplyOrdinalGender("milionésimo", gender),
                    ConvertToOrdinal(number / 1_000_000_000, gender)));

            number %= 1_000_000;
        }

        if (number / 1000 > 0)
        {
            parts.Add(number / 1000 == 1
                ? ApplyOrdinalGender("milésimo", gender)
                : string.Format("{0} " + ApplyOrdinalGender("milésimo", gender), ConvertToOrdinal(number / 1000, gender)));

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
        if (profile.Style == PortugueseNumberingStyle.European)
        {
            return billions == 1
                ? "mil milhões"
                : $"{Convert(billions)} mil milhões";
        }

        return billions >= 2
            ? $"{Convert(billions, GrammaticalGender.Masculine)} bilhões"
            : $"{Convert(billions, GrammaticalGender.Masculine)} bilhão";
    }

    string BuildOrdinalBillions(int number, GrammaticalGender gender)
    {
        if (profile.Style == PortugueseNumberingStyle.European)
        {
            return number / 1_000_000_000 == 1
                ? $"{ApplyOrdinalGender("milésimo", gender)} {ApplyOrdinalGender("milionésimo", gender)}"
                : $"{Convert(number / 1_000_000_000)} {ApplyOrdinalGender("milésimo", gender)} {ApplyOrdinalGender("milionésimo", gender)}";
        }

        return number / 1_000_000_000 == 1
            ? ApplyOrdinalGender("bilionésimo", gender)
            : string.Format("{0} " + ApplyOrdinalGender("bilionésimo", gender), ConvertToOrdinal(number / 1_000_000_000, gender));
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

enum PortugueseNumberingStyle
{
    Brazilian,
    European
}

sealed class PortugueseNumberToWordsProfile(
    PortugueseNumberingStyle style,
    string ordinalMillionSeparator,
    string[] unitsMap,
    string[] tensMap,
    string[] hundredsMap,
    string[] ordinalUnitsMap,
    string[] ordinalTensMap,
    string[] ordinalHundredsMap)
{
    public PortugueseNumberingStyle Style { get; } = style;
    public string OrdinalMillionSeparator { get; } = ordinalMillionSeparator;
    public string[] UnitsMap { get; } = unitsMap;
    public string[] TensMap { get; } = tensMap;
    public string[] HundredsMap { get; } = hundredsMap;
    public string[] OrdinalUnitsMap { get; } = ordinalUnitsMap;
    public string[] OrdinalTensMap { get; } = ordinalTensMap;
    public string[] OrdinalHundredsMap { get; } = ordinalHundredsMap;
}

namespace Humanizer;

class DualFormScaleNumberToWordsConverter(DualFormScaleNumberToWordsProfile profile) : GenderedNumberToWordsConverter
{
    readonly DualFormScaleNumberToWordsProfile profile = profile;

    public override string Convert(long input, GrammaticalGender gender, bool addAnd = true)
    {
        var negativeNumber = false;

        if (input < 0)
        {
            negativeNumber = true;
            input *= -1;
        }

        if (input < 1000000000)
        {
            return GetMillions(input, gender) + (negativeNumber ? $" {profile.MinusSuffix}" : string.Empty);
        }

        var billions = input / 1000000000;
        var tensInBillions = billions % 100;
        var millions = input % 1000000000;

        var billionsText = GetScaleText(billions, tensInBillions, profile.BillionScale, gender);
        var millionsText = GetMillions(millions, gender);

        if (millions == 0)
        {
            return billionsText;
        }

        return $"{billionsText} {profile.Conjunction} {millionsText}" + (negativeNumber ? $" {profile.MinusSuffix}" : string.Empty);
    }

    public override string ConvertToOrdinal(int number, GrammaticalGender gender)
    {
        if (number <= 20)
        {
            return profile.OrdinalOverrideMap[number];
        }

        var ordinal = Convert(number, gender);

        if (ordinal.StartsWith('d'))
        {
            return $"id-{Convert(number, gender)}";
        }
        if (ordinal.StartsWith('s'))
        {
            return $"is-{Convert(number, gender)}";
        }
        if (ordinal.StartsWith('t'))
        {
            return $"it-{Convert(number, gender)}";
        }
        if (ordinal.StartsWith('e'))
        {
            return $"l-{Convert(number, gender)}";
        }
        return $"il-{Convert(number, gender)}";
    }

    string GetTens(long value, bool usePrefixMap, bool usePrefixMapForLowerDigits, GrammaticalGender gender)
    {
        if (value == 1 && gender == GrammaticalGender.Feminine)
        {
            return profile.FeminineOneWord;
        }

        if (value < 11 && usePrefixMap && usePrefixMapForLowerDigits)
        {
            return profile.PrefixMap[value];
        }

        if (value < 11 && usePrefixMap && !usePrefixMapForLowerDigits)
        {
            return profile.HundredsMap[value];
        }

        if (value is > 10 and < 20 && usePrefixMap)
        {
            return profile.PrefixMap[value];
        }

        if (value < 20)
        {
            return profile.UnitsMap[value];
        }

        var single = value % 10;
        var numberOfTens = value / 10;
        if (single == 0)
        {
            return profile.TensMap[numberOfTens];
        }

        return $"{profile.UnitsMap[single]} {profile.Conjunction} {profile.TensMap[numberOfTens]}";
    }

    string GetHundreds(long value, bool usePrefixMap, bool usePrefixMapForLowerValueDigits, GrammaticalGender gender)
    {
        if (value < 100)
        {
            return GetTens(value, usePrefixMap, usePrefixMapForLowerValueDigits, gender);
        }

        var tens = value % 100;
        var numberOfHundreds = value / 100;

        string hundredsText;
        if (numberOfHundreds == 1)
        {
            hundredsText = profile.HundredWord;
        }
        else if (numberOfHundreds == 2)
        {
            hundredsText = profile.DualHundredsWord;
        }
        else
        {
            hundredsText = profile.HundredsMap[numberOfHundreds] + $" {profile.HundredWord}";
        }

        if (tens == 0)
        {
            return hundredsText;
        }

        return $"{hundredsText} {profile.Conjunction} {GetTens(tens, usePrefixMap, usePrefixMapForLowerValueDigits, gender)}";
    }

    string GetThousands(long value, GrammaticalGender gender)
    {
        if (value < 1000)
        {
            return GetHundreds(value, false, false, gender);
        }

        var thousands = value / 1000;
        var tensInThousands = thousands % 100;
        var hundreds = value % 1000;

        var thousandsInText = GetScaleText(thousands, tensInThousands, profile.ThousandScale, gender);

        var hundredsInText = GetHundreds(hundreds, false, false, gender);

        if (hundreds == 0)
        {
            return thousandsInText;
        }

        return $"{thousandsInText} {profile.Conjunction} {hundredsInText}";
    }

    string GetMillions(long value, GrammaticalGender gender)
    {
        if (value < 1000000)
        {
            return GetThousands(value, gender);
        }

        var millions = value / 1000000;
        var tensInMillions = millions % 100;
        var thousands = value % 1000000;

        var millionsText = GetScaleText(millions, tensInMillions, profile.MillionScale, gender);
        var thousandsText = GetThousands(thousands, gender);

        if (thousands == 0)
        {
            return millionsText;
        }

        return $"{millionsText} {profile.Conjunction} {thousandsText}";
    }

    string GetScaleText(long count, long lastTwoDigits, DualFormScale scale, GrammaticalGender gender)
    {
        if (count == 1)
        {
            return scale.Singular;
        }

        if (count == 2)
        {
            return scale.Dual;
        }

        if (lastTwoDigits > 10)
        {
            return $"{GetHundreds(count, true, scale.UsePrefixMapForLowerDigits, gender)} {scale.Singular}";
        }

        if (count == 100)
        {
            return $"{profile.HundredPrefixWord} {scale.Singular}";
        }

        if (count == 101)
        {
            return $"{profile.HundredWord} {profile.Conjunction} {scale.Singular}";
        }

        return $"{GetHundreds(count, true, scale.UsePrefixMapForLowerDigits, gender)} {scale.Plural}";
    }
}

sealed class DualFormScaleNumberToWordsProfile(
    string conjunction,
    string minusSuffix,
    string feminineOneWord,
    string hundredWord,
    string dualHundredsWord,
    string hundredPrefixWord,
    string[] ordinalOverrideMap,
    string[] unitsMap,
    string[] tensMap,
    string[] hundredsMap,
    string[] prefixMap,
    DualFormScale thousandScale,
    DualFormScale millionScale,
    DualFormScale billionScale)
{
    public string Conjunction { get; } = conjunction;
    public string MinusSuffix { get; } = minusSuffix;
    public string FeminineOneWord { get; } = feminineOneWord;
    public string HundredWord { get; } = hundredWord;
    public string DualHundredsWord { get; } = dualHundredsWord;
    public string HundredPrefixWord { get; } = hundredPrefixWord;
    public string[] OrdinalOverrideMap { get; } = ordinalOverrideMap;
    public string[] UnitsMap { get; } = unitsMap;
    public string[] TensMap { get; } = tensMap;
    public string[] HundredsMap { get; } = hundredsMap;
    public string[] PrefixMap { get; } = prefixMap;
    public DualFormScale ThousandScale { get; } = thousandScale;
    public DualFormScale MillionScale { get; } = millionScale;
    public DualFormScale BillionScale { get; } = billionScale;
}

readonly record struct DualFormScale(
    string Singular,
    string Dual,
    string Plural,
    bool UsePrefixMapForLowerDigits);

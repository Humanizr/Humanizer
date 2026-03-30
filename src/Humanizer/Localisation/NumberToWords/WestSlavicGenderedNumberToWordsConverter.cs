namespace Humanizer;

class WestSlavicGenderedNumberToWordsConverter(WestSlavicNumberToWordsProfile profile, CultureInfo culture) :
    GenderedNumberToWordsConverter
{
    readonly WestSlavicNumberToWordsProfile profile = profile;

    public override string Convert(long number, GrammaticalGender gender, bool addAnd = true)
    {
        if (number == 0)
        {
            return profile.UnitsMap[0];
        }

        if (number == long.MinValue)
        {
            throw new NotImplementedException();
        }

        var parts = new List<string>();
        if (number < 0)
        {
            parts.Add(profile.MinusWord);
            number = -number;
        }

        CollectScale(parts, ref number, 1_000_000_000, GrammaticalGender.Feminine, profile.Billions);
        CollectScale(parts, ref number, 1_000_000, GrammaticalGender.Masculine, profile.Millions);
        CollectScale(parts, ref number, 1_000, GrammaticalGender.Masculine, profile.Thousands);
        CollectLessThanThousand(parts, number, gender);

        return string.Join(" ", parts);
    }

    public override string ConvertToOrdinal(int number, GrammaticalGender gender) =>
        number.ToString(culture);

    void CollectScale(List<string> parts, ref long number, long divisor, GrammaticalGender gender, WestSlavicScaleForms forms)
    {
        var scaleNumber = number / divisor;
        if (scaleNumber <= 0)
        {
            return;
        }

        if (forms.OmitLeadingOne && scaleNumber == 1)
        {
            parts.Add(forms.Singular);
            number %= divisor;
            return;
        }

        CollectLessThanThousand(parts, scaleNumber, scaleNumber < 19 ? gender : null);

        var units = scaleNumber % 1000;
        parts.Add(units switch
        {
            1 => forms.Singular,
            > 1 and < 5 => forms.Paucal,
            _ => forms.Plural
        });

        number %= divisor;
    }

    void CollectLessThanThousand(List<string> parts, long number, GrammaticalGender? gender)
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

        if (number > 0)
        {
            parts.Add(UnitByGender(number, gender));
        }
    }

    string UnitByGender(long number, GrammaticalGender? gender)
    {
        if (number != 1 && number != 2)
        {
            return profile.UnitsMap[number];
        }

        return gender switch
        {
            GrammaticalGender.Masculine => profile.UnitsMasculineOverrides[number - 1],
            GrammaticalGender.Feminine => profile.UnitsFeminineOverrides[number - 1],
            GrammaticalGender.Neuter => profile.UnitsNeuterOverrides[number - 1],
            null => profile.UnitsInvariantOverrides[number - 1],
            _ => throw new ArgumentOutOfRangeException(nameof(gender))
        };
    }
}

sealed class WestSlavicNumberToWordsProfile(
    string minusWord,
    string[] unitsMap,
    string[] tensMap,
    string[] hundredsMap,
    string[] unitsMasculineOverrides,
    string[] unitsFeminineOverrides,
    string[] unitsNeuterOverrides,
    string[] unitsInvariantOverrides,
    WestSlavicScaleForms thousands,
    WestSlavicScaleForms millions,
    WestSlavicScaleForms billions)
{
    public string MinusWord { get; } = minusWord;
    public string[] UnitsMap { get; } = unitsMap;
    public string[] TensMap { get; } = tensMap;
    public string[] HundredsMap { get; } = hundredsMap;
    public string[] UnitsMasculineOverrides { get; } = unitsMasculineOverrides;
    public string[] UnitsFeminineOverrides { get; } = unitsFeminineOverrides;
    public string[] UnitsNeuterOverrides { get; } = unitsNeuterOverrides;
    public string[] UnitsInvariantOverrides { get; } = unitsInvariantOverrides;
    public WestSlavicScaleForms Thousands { get; } = thousands;
    public WestSlavicScaleForms Millions { get; } = millions;
    public WestSlavicScaleForms Billions { get; } = billions;
}

readonly record struct WestSlavicScaleForms(string Singular, string Paucal, string Plural, bool OmitLeadingOne = false);

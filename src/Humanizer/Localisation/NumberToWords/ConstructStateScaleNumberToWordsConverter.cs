namespace Humanizer;

class ConstructStateScaleNumberToWordsConverter(ConstructStateScaleNumberToWordsProfile profile, CultureInfo culture) :
    GenderedNumberToWordsConverter(profile.DefaultGender)
{
    readonly ConstructStateScaleNumberToWordsProfile profile = profile;
    readonly CultureInfo culture = culture;

    public override string Convert(long input, GrammaticalGender gender, bool addAnd = true)
    {
        if (input is > int.MaxValue or < int.MinValue)
        {
            throw new NotImplementedException();
        }

        if (input == 0)
        {
            return profile.ZeroWord;
        }

        if (input < 0)
        {
            return profile.MinusWord + " " + Convert(-input, gender);
        }

        var number = (int)input;
        var parts = new List<string>(6);

        foreach (var scale in profile.Scales)
        {
            if (number < scale.Value)
            {
                continue;
            }

            var count = number / scale.Value;
            parts.Add(BuildLargeScalePart(count, scale));
            number %= scale.Value;
        }

        if (number >= 1000)
        {
            var thousands = number / 1000;
            parts.Add(BuildThousandsPart(thousands));
            number %= 1000;
        }

        if (number >= 100)
        {
            var hundreds = number / 100;
            parts.Add(BuildHundredsPart(hundreds));
            number %= 100;
        }

        if (number > 0)
        {
            parts.Add(BuildUnderOneHundredPart(number, gender, parts.Count > 0));
        }

        return string.Join(" ", parts);
    }

    public override string ConvertToOrdinal(int number, GrammaticalGender gender) =>
        number.ToString(culture);

    string BuildLargeScalePart(int count, ConstructStateScale scale) =>
        count switch
        {
            1 => scale.Singular,
            2 => scale.DualPrefix + " " + scale.Singular,
            _ => Convert(count, scale.CountGender) + " " + scale.Singular
        };

    string BuildThousandsPart(int thousands)
    {
        if (profile.ThousandsSpecialCases.TryGetValue(thousands, out var special))
        {
            return special;
        }

        if (thousands <= 10)
        {
            return profile.UnitsFeminine[thousands] + profile.ThousandsPluralSuffix;
        }

        return Convert(thousands) + profile.ThousandsSingularSuffix;
    }

    string BuildHundredsPart(int hundreds) =>
        hundreds switch
        {
            1 => profile.OneHundredWord,
            2 => profile.TwoHundredsWord,
            _ => profile.UnitsFeminine[hundreds] + profile.HundredsPluralSuffix
        };

    string BuildUnderOneHundredPart(int number, GrammaticalGender gender, bool appendAnd)
    {
        if (number <= 10)
        {
            var unit = GetUnit(number, gender);
            return appendAnd ? profile.AndPrefix + unit : unit;
        }

        if (number < 20)
        {
            var unit = GetUnit(number % 10, gender).Replace(profile.TeenNormalizationOld, profile.TeenNormalizationNew);
            var teenWord = unit + " " + (gender == GrammaticalGender.Masculine ? profile.TeenMasculineWord : profile.TeenFeminineWord);
            return appendAnd ? profile.AndPrefix + teenWord : teenWord;
        }

        var tensWord = profile.TensMap[number / 10 - 2];
        if (number % 10 == 0)
        {
            return tensWord;
        }

        return tensWord + " " + profile.AndPrefix + GetUnit(number % 10, gender);
    }

    string GetUnit(int number, GrammaticalGender gender) =>
        gender == GrammaticalGender.Masculine
            ? profile.UnitsMasculine[number]
            : profile.UnitsFeminine[number];
}

sealed class ConstructStateScaleNumberToWordsProfile(
    GrammaticalGender defaultGender,
    string zeroWord,
    string minusWord,
    string andPrefix,
    string teenMasculineWord,
    string teenFeminineWord,
    string teenNormalizationOld,
    string teenNormalizationNew,
    string[] unitsFeminine,
    string[] unitsMasculine,
    string[] tensMap,
    string oneHundredWord,
    string twoHundredsWord,
    string hundredsPluralSuffix,
    string thousandsPluralSuffix,
    string thousandsSingularSuffix,
    FrozenDictionary<int, string> thousandsSpecialCases,
    ConstructStateScale[] scales)
{
    public GrammaticalGender DefaultGender { get; } = defaultGender;
    public string ZeroWord { get; } = zeroWord;
    public string MinusWord { get; } = minusWord;
    public string AndPrefix { get; } = andPrefix;
    public string TeenMasculineWord { get; } = teenMasculineWord;
    public string TeenFeminineWord { get; } = teenFeminineWord;
    public string TeenNormalizationOld { get; } = teenNormalizationOld;
    public string TeenNormalizationNew { get; } = teenNormalizationNew;
    public string[] UnitsFeminine { get; } = unitsFeminine;
    public string[] UnitsMasculine { get; } = unitsMasculine;
    public string[] TensMap { get; } = tensMap;
    public string OneHundredWord { get; } = oneHundredWord;
    public string TwoHundredsWord { get; } = twoHundredsWord;
    public string HundredsPluralSuffix { get; } = hundredsPluralSuffix;
    public string ThousandsPluralSuffix { get; } = thousandsPluralSuffix;
    public string ThousandsSingularSuffix { get; } = thousandsSingularSuffix;
    public FrozenDictionary<int, string> ThousandsSpecialCases { get; } = thousandsSpecialCases;
    public ConstructStateScale[] Scales { get; } = scales;
}

readonly record struct ConstructStateScale(
    int Value,
    string Singular,
    string DualPrefix,
    GrammaticalGender CountGender);

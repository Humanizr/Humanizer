using System.Text;

namespace Humanizer;

class ScandinavianFamilyNumberToWordsConverter(ScandinavianNumberToWordsProfile profile)
    : GenderedNumberToWordsConverter(profile.DefaultGender)
{
    readonly ScandinavianNumberToWordsProfile profile = profile;

    public override string Convert(long number, GrammaticalGender gender, bool addAnd = true) =>
        profile.Style switch
        {
            ScandinavianFamilyStyle.Danish => ConvertDanish(number),
            ScandinavianFamilyStyle.NorwegianBokmal => ConvertNorwegian(number, gender),
            ScandinavianFamilyStyle.Swedish => ConvertSwedish(number, profile.DefaultGender),
            _ => throw new InvalidOperationException("Unsupported Scandinavian family style.")
        };

    public override string ConvertToOrdinal(int number, GrammaticalGender gender) =>
        profile.Style switch
        {
            ScandinavianFamilyStyle.Danish => number.ToString(),
            ScandinavianFamilyStyle.NorwegianBokmal => ConvertNorwegianOrdinal(number, gender),
            ScandinavianFamilyStyle.Swedish => ConvertSwedishOrdinal(number, profile.DefaultGender),
            _ => throw new InvalidOperationException("Unsupported Scandinavian family style.")
        };

    string ConvertDanish(long number)
    {
        if (number > profile.MaximumValue || number < -profile.MaximumValue)
        {
            throw new NotImplementedException();
        }

        if (number == 0)
        {
            return profile.ZeroWord;
        }

        if (number < 0)
        {
            return $"{profile.MinusWord} {ConvertDanish(-number)}";
        }

        var parts = new List<string>();
        var remainder = number;

        foreach (var scale in profile.Scales)
        {
            if (scale.Value < 1_000_000)
            {
                continue;
            }

            var count = remainder / scale.Value;
            if (count == 0)
            {
                continue;
            }

            parts.Add(count == 1
                ? scale.Name
                : $"{ConvertDanish(count)} {scale.Plural}");
            remainder %= scale.Value;
        }

        AppendDanishThousands(parts, ref remainder);

        if (remainder > 0)
        {
            var tail = remainder < 100 && parts.Count > 0
                ? $"og {ConvertDanishLessThanHundred((int)remainder)}"
                : ConvertDanishLessThanThousand((int)remainder);

            parts.Add(tail);
        }

        return string.Join(" ", parts);
    }

    void AppendDanishThousands(List<string> parts, ref long remainder)
    {
        var thousands = remainder / 1000;
        if (thousands == 0)
        {
            return;
        }

        parts.Add(thousands == 1
            ? profile.ThousandSingularWord
            : $"{ConvertDanish(thousands)} {profile.ThousandWord}");
        remainder %= 1000;
    }

    string ConvertDanishLessThanThousand(int number)
    {
        if (number >= 100)
        {
            var prefix = profile.HundredUnitMap[number / 100];
            var words = $"{prefix} {profile.HundredWord}";
            var remainder = number % 100;
            if (remainder > 0)
            {
                words = $"{words} og {ConvertDanishLessThanHundred(remainder)}";
            }

            return words;
        }

        return ConvertDanishLessThanHundred(number);
    }

    string ConvertDanishLessThanHundred(int number)
    {
        if (number < 20)
        {
            return profile.UnitsMap[number];
        }

        var tensWord = profile.TensMap[number / 10];
        var unit = number % 10;
        return unit == 0
            ? tensWord
            : $"{profile.UnitsMap[unit]}{profile.TensLinker}{tensWord}";
    }

    string ConvertNorwegian(long number, GrammaticalGender gender)
    {
        if (number is > int.MaxValue or < int.MinValue)
        {
            throw new NotImplementedException();
        }

        return ConvertNorwegian((int)number, false, gender);
    }

    string ConvertNorwegianOrdinal(int number, GrammaticalGender gender) =>
        ConvertNorwegian(number, true, gender);

    string ConvertNorwegian(int number, bool isOrdinal, GrammaticalGender gender)
    {
        if (number == 0)
        {
            return GetNorwegianUnitValue(0, isOrdinal);
        }

        if (number < 0)
        {
            return $"{profile.MinusWord} {ConvertNorwegian(-number, isOrdinal, gender)}";
        }

        if (!isOrdinal && number == 1)
        {
            return gender switch
            {
                GrammaticalGender.Feminine when !string.IsNullOrEmpty(profile.OneFeminine) => profile.OneFeminine,
                GrammaticalGender.Neuter when !string.IsNullOrEmpty(profile.OneNeuter) => profile.OneNeuter,
                _ => profile.OneDefault
            };
        }

        var parts = new List<string>();
        var hasLargeScale = false;

        foreach (var scale in profile.Scales)
        {
            if (scale.Value is not (>= 1_000_000 and <= int.MaxValue))
            {
                continue;
            }

            var count = number / (int)scale.Value;
            if (count == 0)
            {
                continue;
            }

            hasLargeScale = true;
            var exactOrdinal = isOrdinal && number % scale.Value == 0;
            parts.Add(FormatNorwegianLargeScale(scale, count, exactOrdinal));
            number %= (int)scale.Value;
        }

        var hasThousand = false;
        var thousands = number / 1000;
        if (thousands > 0)
        {
            hasThousand = true;
            parts.Add(FormatNorwegianThousand(thousands, number % 1000 < 100));
            number %= 1000;
        }

        var hasHundred = false;
        var hundreds = number / 100;
        if (hundreds > 0)
        {
            hasHundred = true;
            parts.Add(FormatNorwegianHundred(hundreds, hasLargeScale || hasThousand));
            number %= 100;
        }

        if (number > 0)
        {
            if (parts.Count != 0)
            {
                parts.Add(hasLargeScale && !hasHundred && !hasThousand
                    ? "og "
                    : profile.TensLinker);
            }

            parts.Add(number < 20
                ? GetNorwegianUnitValue(number, isOrdinal)
                : ConvertNorwegianTens(number, isOrdinal));
        }
        else if (isOrdinal)
        {
            parts[^1] += hasLargeScale ? "te" : "de";
        }

        return string.Concat(parts).Trim();
    }

    string FormatNorwegianLargeScale(ScandinavianScale scale, int count, bool exactOrdinal)
    {
        if (count == 1)
        {
            return exactOrdinal
                ? scale.Name
                : $"{profile.OneDefault} {scale.Name} ";
        }

        var scaleName = exactOrdinal ? scale.Name : scale.Plural;
        var suffix = exactOrdinal ? string.Empty : " ";
        return $"{ConvertNorwegian(count, false, scale.Gender)} {scaleName}{suffix}";
    }

    string FormatNorwegianThousand(int count, bool useStandaloneSingular) =>
        count == 1
            ? useStandaloneSingular
                ? profile.ThousandSingularWord
                : profile.ThousandCompositeSingularWord
            : $"{ConvertNorwegian(count, false, GrammaticalGender.Masculine)}{profile.ThousandWord}";

    string FormatNorwegianHundred(int count, bool useCompositeSingular) =>
        count == 1
            ? useCompositeSingular
                ? profile.HundredCompositeSingularWord
                : profile.HundredWord
            : $"{ConvertNorwegian(count, false, GrammaticalGender.Masculine)}{profile.HundredWord}";

    string ConvertNorwegianTens(int number, bool isOrdinal)
    {
        var lastPart = profile.TensMap[number / 10];
        var unit = number % 10;

        if (unit > 0)
        {
            return lastPart + GetNorwegianUnitValue(unit, isOrdinal);
        }

        return isOrdinal
            ? lastPart.TrimEnd('e') + "ende"
            : lastPart;
    }

    string GetNorwegianUnitValue(int number, bool isOrdinal)
    {
        if (!isOrdinal)
        {
            return profile.UnitsMap[number];
        }

        if (profile.OrdinalExceptions.TryGetValue(number, out var exactValue))
        {
            return exactValue;
        }

        return number < 13
            ? profile.UnitsMap[number].TrimEnd('e') + "ende"
            : profile.UnitsMap[number] + "de";
    }

    string ConvertSwedish(long number, GrammaticalGender gender)
    {
        if (number is > int.MaxValue or < int.MinValue)
        {
            throw new NotImplementedException();
        }

        return ConvertSwedish((int)number, false, gender);
    }

    string ConvertSwedishOrdinal(int number, GrammaticalGender gender) =>
        ConvertSwedish(number, true, gender);

    string ConvertSwedish(int number, bool isOrdinal, GrammaticalGender gender)
    {
        if (number == 0)
        {
            return isOrdinal
                ? profile.OrdinalExceptions[0]
                : profile.ZeroWord;
        }

        if (number < 0)
        {
            return $"{profile.MinusWord} {ConvertSwedish(-number, isOrdinal, gender)}";
        }

        var word = new StringBuilder();

        foreach (var scale in profile.Scales)
        {
            var divided = number / (int)scale.Value;
            if (divided <= 0)
            {
                continue;
            }

            if (divided == 1 && !scale.DisplayOneUnit)
            {
                word.Append(scale.Name);
            }
            else
            {
                word.Append(ConvertSwedish(divided, false, scale.Gender));
                word.Append(scale.Prefix);
                word.Append(scale.Name);
            }

            if (divided > 1 && scale.PluralSuffix.Length != 0)
            {
                word.Append(scale.PluralSuffix);
            }

            number %= (int)scale.Value;

            if (number == 0 && isOrdinal)
            {
                word.Append(scale.OrdinalSuffix);
                return word.ToString();
            }

            if (number > 0)
            {
                word.Append(scale.Postfix);
            }
        }

        if (number == 0)
        {
            return word.ToString();
        }

        if (number <= 20 && isOrdinal && profile.OrdinalExceptions.TryGetValue(number, out var exactOrdinal))
        {
            word.Append(exactOrdinal);
            return word.ToString();
        }

        if (number < 20)
        {
            word.Append(GetSwedishUnit(number, gender));
            return word.ToString();
        }

        var tens = profile.TensMap[number / 10];
        var unit = number % 10;
        if (unit == 0)
        {
            if (isOrdinal)
            {
                if (profile.OrdinalExceptions.TryGetValue(number, out var exactTensOrdinal))
                {
                    word.Append(exactTensOrdinal);
                }
                else
                {
                    word.Append(tens);
                    word.Append("nde");
                }

                return word.ToString();
            }

            word.Append(tens);
            return word.ToString();
        }

        word.Append(tens);
        word.Append(isOrdinal
            ? ConvertSwedish(unit, true, gender)
            : GetSwedishUnit(unit, gender));

        return word.ToString();
    }

    string GetSwedishUnit(int number, GrammaticalGender gender) =>
        number == 1 && gender == GrammaticalGender.Masculine
            ? profile.OneMasculine
            : profile.UnitsMap[number];
}

enum ScandinavianFamilyStyle
{
    Danish,
    NorwegianBokmal,
    Swedish
}

sealed class ScandinavianNumberToWordsProfile(
    ScandinavianFamilyStyle style,
    long maximumValue,
    GrammaticalGender defaultGender,
    string zeroWord,
    string minusWord,
    string oneDefault,
    string oneMasculine,
    string oneFeminine,
    string oneNeuter,
    string tensLinker,
    string hundredWord,
    string hundredCompositeSingularWord,
    string thousandWord,
    string thousandSingularWord,
    string thousandCompositeSingularWord,
    string[] unitsMap,
    string[] tensMap,
    string[] hundredUnitMap,
    ScandinavianScale[] scales,
    FrozenDictionary<int, string> ordinalExceptions)
{
    public ScandinavianFamilyStyle Style { get; } = style;
    public long MaximumValue { get; } = maximumValue;
    public GrammaticalGender DefaultGender { get; } = defaultGender;
    public string ZeroWord { get; } = zeroWord;
    public string MinusWord { get; } = minusWord;
    public string OneDefault { get; } = oneDefault;
    public string OneMasculine { get; } = oneMasculine;
    public string OneFeminine { get; } = oneFeminine;
    public string OneNeuter { get; } = oneNeuter;
    public string TensLinker { get; } = tensLinker;
    public string HundredWord { get; } = hundredWord;
    public string HundredCompositeSingularWord { get; } = hundredCompositeSingularWord;
    public string ThousandWord { get; } = thousandWord;
    public string ThousandSingularWord { get; } = thousandSingularWord;
    public string ThousandCompositeSingularWord { get; } = thousandCompositeSingularWord;
    public string[] UnitsMap { get; } = unitsMap;
    public string[] TensMap { get; } = tensMap;
    public string[] HundredUnitMap { get; } = hundredUnitMap;
    public ScandinavianScale[] Scales { get; } = scales;
    public FrozenDictionary<int, string> OrdinalExceptions { get; } = ordinalExceptions;
}

readonly record struct ScandinavianScale(
    long Value,
    string Name,
    string Plural,
    string Prefix,
    string Postfix,
    string PluralSuffix,
    string OrdinalSuffix,
    bool DisplayOneUnit,
    GrammaticalGender Gender);

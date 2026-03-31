using System.Text;

namespace Humanizer;

// Shared engine for the Scandinavian profiles that still share most lexical data but differ in cardinal and ordinal composition rules.
// The goal is to keep Norwegian and Swedish on one decomposition engine while pushing their stem/suffix differences into generated data.
class ScandinavianFamilyNumberToWordsConverter(ScandinavianNumberToWordsProfile profile)
    : GenderedNumberToWordsConverter(profile.DefaultGender)
{
    readonly ScandinavianNumberToWordsProfile profile = profile;

    public override string Convert(long number, GrammaticalGender gender, bool addAnd = true) =>
        profile.CardinalStrategy switch
        {
            ScandinavianCardinalStrategy.NorwegianBokmal => ConvertNorwegian(number, gender),
            ScandinavianCardinalStrategy.Swedish => ConvertSwedish(number, profile.DefaultGender),
            _ => throw new InvalidOperationException("Unsupported Scandinavian cardinal strategy.")
        };

    public override string ConvertToOrdinal(int number, GrammaticalGender gender) =>
        profile.OrdinalStrategy switch
        {
            ScandinavianOrdinalStrategy.NorwegianBokmal => ConvertNorwegianOrdinal(number, gender),
            ScandinavianOrdinalStrategy.Swedish => ConvertSwedishOrdinal(number, profile.DefaultGender),
            _ => throw new InvalidOperationException("Unsupported Scandinavian ordinal strategy.")
        };

    // Norwegian compounds are mostly concatenative, but large-scale spacing and exact ordinals still vary by generated suffix settings.
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

    // The generated profile decides where explicit spaces remain; the algorithm only decomposes the number into large scales, thousands, hundreds, and tens.
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
                    ? profile.LargeScaleRemainderJoiner
                    : profile.TensLinker);
            }

            parts.Add(number < 20
                ? GetNorwegianUnitValue(number, isOrdinal)
                : ConvertNorwegianTens(number, isOrdinal));
        }
        else if (isOrdinal)
        {
            parts[^1] += hasLargeScale ? profile.ExactLargeScaleOrdinalSuffix : profile.ExactDefaultOrdinalSuffix;
        }

        return string.Concat(parts).Trim();
    }

    // Large-scale exact ordinals reuse the generated suffix metadata instead of hardcoded locale strings.
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

    // Tens stay shared; the profile now decides both the stem trim and appended ordinal suffix.
    string ConvertNorwegianTens(int number, bool isOrdinal)
    {
        var lastPart = profile.TensMap[number / 10];
        var unit = number % 10;

        if (unit > 0)
        {
            return lastPart + GetNorwegianUnitValue(unit, isOrdinal);
        }

        return isOrdinal
            ? lastPart.TrimEnd(profile.TensOrdinalTrimEndCharacters.ToCharArray()) + profile.TensOrdinalSuffix
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

        if (number < profile.ShortOrdinalUpperBoundExclusive)
        {
            return profile.UnitsMap[number].TrimEnd(profile.ShortOrdinalTrimEndCharacters.ToCharArray()) + profile.ShortOrdinalTrimmedSuffix;
        }

        return profile.UnitsMap[number] + profile.ShortOrdinalSuffix;
    }

    // Swedish keeps more of the scale composition in metadata, so the converter mostly stitches together generated scale records.
    // Exact ordinals are intentionally looked up directly from the generated exception map instead of depending on a hardcoded threshold.
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

    // Exact scale ordinals, plural suffixes, and spacing all come from the generated scale array.
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

        if (isOrdinal && profile.OrdinalExceptions.TryGetValue(number, out var exactOrdinal))
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
                    word.Append(profile.TensOrdinalSuffix);
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

// Separates cardinal composition from ordinal composition so future Scandinavian variants can mix strategies explicitly.
enum ScandinavianCardinalStrategy
{
    NorwegianBokmal,
    Swedish
}

// The ordinal rules diverge more sharply than the cardinal rules, so they are modeled separately.
enum ScandinavianOrdinalStrategy
{
    NorwegianBokmal,
    Swedish
}

// Holds the generated lexicon and strategy selections for the Scandinavian shared engine.
sealed class ScandinavianNumberToWordsProfile(
    ScandinavianCardinalStrategy cardinalStrategy,
    ScandinavianOrdinalStrategy ordinalStrategy,
    long maximumValue,
    GrammaticalGender defaultGender,
    string zeroWord,
    string minusWord,
    string oneDefault,
    string oneMasculine,
    string oneFeminine,
    string oneNeuter,
    string tensLinker,
    string largeScaleRemainderJoiner,
    string exactLargeScaleOrdinalSuffix,
    string exactDefaultOrdinalSuffix,
    string tensOrdinalTrimEndCharacters,
    string tensOrdinalSuffix,
    int shortOrdinalUpperBoundExclusive,
    string shortOrdinalTrimEndCharacters,
    string shortOrdinalTrimmedSuffix,
    string shortOrdinalSuffix,
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
    public ScandinavianCardinalStrategy CardinalStrategy { get; } = cardinalStrategy;
    public ScandinavianOrdinalStrategy OrdinalStrategy { get; } = ordinalStrategy;
    public long MaximumValue { get; } = maximumValue;
    public GrammaticalGender DefaultGender { get; } = defaultGender;
    public string ZeroWord { get; } = zeroWord;
    public string MinusWord { get; } = minusWord;
    public string OneDefault { get; } = oneDefault;
    public string OneMasculine { get; } = oneMasculine;
    public string OneFeminine { get; } = oneFeminine;
    public string OneNeuter { get; } = oneNeuter;
    public string TensLinker { get; } = tensLinker;
    public string LargeScaleRemainderJoiner { get; } = largeScaleRemainderJoiner;
    public string ExactLargeScaleOrdinalSuffix { get; } = exactLargeScaleOrdinalSuffix;
    public string ExactDefaultOrdinalSuffix { get; } = exactDefaultOrdinalSuffix;
    public string TensOrdinalTrimEndCharacters { get; } = tensOrdinalTrimEndCharacters;
    public string TensOrdinalSuffix { get; } = tensOrdinalSuffix;
    public int ShortOrdinalUpperBoundExclusive { get; } = shortOrdinalUpperBoundExclusive;
    public string ShortOrdinalTrimEndCharacters { get; } = shortOrdinalTrimEndCharacters;
    public string ShortOrdinalTrimmedSuffix { get; } = shortOrdinalTrimmedSuffix;
    public string ShortOrdinalSuffix { get; } = shortOrdinalSuffix;
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

// Scale metadata is data-driven; the strategy enums decide how those pieces are composed.
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

namespace Humanizer;

class InvertedTensNumberToWordsConverter(InvertedTensNumberToWordsProfile profile) : GenderlessNumberToWordsConverter
{
    readonly InvertedTensNumberToWordsProfile profile = profile;

    public override string Convert(long number)
    {
        if (number == 0)
        {
            return profile.UnitsMap[0];
        }

        if (number < 0)
        {
            return $"{profile.MinusWord} {Convert(-number)}";
        }

        var builder = new StringBuilder();
        AppendNumber(builder, number);
        return builder.ToString();
    }

    public override string ConvertToOrdinal(int number) =>
        profile.OrdinalMode switch
        {
            InvertedTensOrdinalMode.NumericString => number.ToString(CultureInfo.InvariantCulture),
            _ => ConvertWithSuffixOrdinal(number)
        };

    string ConvertWithSuffixOrdinal(int number)
    {
        var word = Convert(number);

        if (profile.RemoveLeadingOneInOrdinal &&
            word.StartsWith(profile.LeadingOneWord, StringComparison.Ordinal) &&
            (profile.LeadingOnePreservePrefix.Length == 0 ||
             !word.StartsWith(profile.LeadingOnePreservePrefix, StringComparison.Ordinal)))
        {
            word = word[profile.LeadingOneWord.Length..];
        }

        foreach (var exception in profile.OrdinalExceptions)
        {
            if (!word.EndsWith(exception.Key, StringComparison.Ordinal))
            {
                continue;
            }

            return StringHumanizeExtensions.Concat(
                word.AsSpan(0, word.Length - exception.Key.Length),
                exception.Value.AsSpan());
        }

        return profile.OrdinalSteSuffixEndingChars.Contains(word[^1])
            ? word + profile.OrdinalSteSuffix
            : word + profile.OrdinalDefaultSuffix;
    }

    void AppendNumber(StringBuilder builder, long number)
    {
        var hasScalePrefix = false;

        foreach (var scale in profile.Scales)
        {
            var count = number / scale.Value;
            if (count == 0)
            {
                continue;
            }

            if (count == 1)
            {
                builder.Append(scale.OneForm);
            }
            else
            {
                AppendNumber(builder, count);
                builder.Append(scale.CountJoiner);
                builder.Append(scale.ManyForm);
            }

            number %= scale.Value;
            hasScalePrefix = true;

            if (number > 0)
            {
                builder.Append(scale.RemainderSeparator);
            }
        }

        if (number > 0)
        {
            AppendLessThanThousand(builder, (int)number, hasScalePrefix);
        }
    }

    void AppendLessThanThousand(StringBuilder builder, int number, bool hasScalePrefix)
    {
        if (hasScalePrefix &&
            ShouldPrefixTail(number, profile.ScaleTailPrefixMode) &&
            profile.ScaleTailPrefix.Length > 0)
        {
            builder.Append(profile.ScaleTailPrefix);
        }

        if (number >= 100)
        {
            var hundreds = number / 100;
            var remainder = number % 100;

            if (hundreds == 1)
            {
                builder.Append(profile.OneHundredWord);
            }
            else
            {
                builder.Append(profile.HundredsPrefixMap[hundreds]);
                builder.Append(profile.HundredJoiner);
                builder.Append(profile.HundredWord);
            }

            if (remainder > 0)
            {
                builder.Append(profile.HundredRemainderSeparator);

                if (ShouldPrefixTail(remainder, profile.HundredTailPrefixMode) &&
                    profile.HundredTailPrefix.Length > 0)
                {
                    builder.Append(profile.HundredTailPrefix);
                }

                AppendLessThanHundred(builder, remainder);
            }

            return;
        }

        AppendLessThanHundred(builder, number);
    }

    void AppendLessThanHundred(StringBuilder builder, int number)
    {
        if (number < 20)
        {
            builder.Append(profile.UnitsMap[number]);
            return;
        }

        var tens = number / 10;
        var unit = number % 10;
        if (unit == 0)
        {
            builder.Append(profile.TensMap[tens]);
            return;
        }

        var unitWord = profile.UnitsMap[unit];
        builder.Append(unitWord);
        builder.Append(GetUnitTensJoiner(unitWord));
        builder.Append(profile.TensMap[tens]);
    }

    string GetUnitTensJoiner(string unitWord) =>
        profile.UnitTensAlternateJoinerUnitEnding.Length > 0 &&
        unitWord.EndsWith(profile.UnitTensAlternateJoinerUnitEnding, StringComparison.Ordinal)
            ? profile.UnitTensAlternateJoiner
            : profile.UnitTensJoiner;

    static bool ShouldPrefixTail(int number, InvertedTensTailPrefixMode mode) =>
        mode switch
        {
            InvertedTensTailPrefixMode.None => false,
            InvertedTensTailPrefixMode.SubHundred => number < 100,
            InvertedTensTailPrefixMode.LowOrExactTens => number < 20 || number < 100 && number % 10 == 0,
            _ => throw new ArgumentOutOfRangeException(nameof(mode))
        };
}

internal sealed class InvertedTensNumberToWordsProfile(
    string minusWord,
    string[] unitsMap,
    string[] tensMap,
    string[] hundredsPrefixMap,
    string oneHundredWord,
    string hundredWord,
    string hundredJoiner,
    string hundredRemainderSeparator,
    string scaleTailPrefix,
    InvertedTensTailPrefixMode scaleTailPrefixMode,
    string hundredTailPrefix,
    InvertedTensTailPrefixMode hundredTailPrefixMode,
    string unitTensJoiner,
    string unitTensAlternateJoinerUnitEnding,
    string unitTensAlternateJoiner,
    InvertedTensScale[] scales,
    InvertedTensOrdinalMode ordinalMode,
    FrozenDictionary<string, string> ordinalExceptions,
    string ordinalSteSuffixEndingChars,
    string ordinalSteSuffix,
    string ordinalDefaultSuffix,
    bool removeLeadingOneInOrdinal,
    string leadingOneWord,
    string leadingOnePreservePrefix)
{
    public string MinusWord { get; } = minusWord;
    public string[] UnitsMap { get; } = unitsMap;
    public string[] TensMap { get; } = tensMap;
    public string[] HundredsPrefixMap { get; } = hundredsPrefixMap;
    public string OneHundredWord { get; } = oneHundredWord;
    public string HundredWord { get; } = hundredWord;
    public string HundredJoiner { get; } = hundredJoiner;
    public string HundredRemainderSeparator { get; } = hundredRemainderSeparator;
    public string ScaleTailPrefix { get; } = scaleTailPrefix;
    public InvertedTensTailPrefixMode ScaleTailPrefixMode { get; } = scaleTailPrefixMode;
    public string HundredTailPrefix { get; } = hundredTailPrefix;
    public InvertedTensTailPrefixMode HundredTailPrefixMode { get; } = hundredTailPrefixMode;
    public string UnitTensJoiner { get; } = unitTensJoiner;
    public string UnitTensAlternateJoinerUnitEnding { get; } = unitTensAlternateJoinerUnitEnding;
    public string UnitTensAlternateJoiner { get; } = unitTensAlternateJoiner;
    public InvertedTensScale[] Scales { get; } = scales;
    public InvertedTensOrdinalMode OrdinalMode { get; } = ordinalMode;
    public FrozenDictionary<string, string> OrdinalExceptions { get; } = ordinalExceptions;
    public string OrdinalSteSuffixEndingChars { get; } = ordinalSteSuffixEndingChars;
    public string OrdinalSteSuffix { get; } = ordinalSteSuffix;
    public string OrdinalDefaultSuffix { get; } = ordinalDefaultSuffix;
    public bool RemoveLeadingOneInOrdinal { get; } = removeLeadingOneInOrdinal;
    public string LeadingOneWord { get; } = leadingOneWord;
    public string LeadingOnePreservePrefix { get; } = leadingOnePreservePrefix;
}

internal readonly record struct InvertedTensScale(long Value, string OneForm, string ManyForm, string CountJoiner, string RemainderSeparator);

internal enum InvertedTensOrdinalMode
{
    NumericString,
    Suffix
}

internal enum InvertedTensTailPrefixMode
{
    None,
    SubHundred,
    LowOrExactTens
}

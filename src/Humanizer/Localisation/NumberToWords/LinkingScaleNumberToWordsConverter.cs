namespace Humanizer;

class LinkingScaleNumberToWordsConverter(LinkingScaleNumberToWordsProfile profile) : GenderlessNumberToWordsConverter
{
    readonly LinkingScaleNumberToWordsProfile profile = profile;

    public override string Convert(long number)
    {
        if (number == 0)
        {
            return profile.ZeroWord;
        }

        if (number < 0)
        {
            return $"{profile.MinusWord} {Convert(-number)}";
        }

        var parts = new List<string>();
        var remainder = number;

        foreach (var scale in profile.Scales)
        {
            var part = remainder / scale.Value;
            if (part <= 0)
            {
                continue;
            }

            remainder %= scale.Value;
            parts.Add(BuildScalePart(part, scale.Name));
        }

        if (remainder > 0)
        {
            parts.Add(ConvertUnderThousand(remainder));
        }

        return string.Join(" ", parts);
    }

    public override string ConvertToOrdinal(int number) => Convert(number);

    string BuildScalePart(long part, string scale)
    {
        if (part < profile.ScalePrefixes.Length && !string.IsNullOrEmpty(profile.ScalePrefixes[part]))
        {
            return $"{profile.ScalePrefixes[(int)part]} {scale}";
        }

        return $"{ApplyScaleLinking(ConvertUnderThousand(part))} {scale}";
    }

    string ConvertUnderThousand(long number)
    {
        if (number < 100)
        {
            return ConvertUnderHundred(number);
        }

        var hundreds = number / 100;
        var remainder = number % 100;
        var parts = new List<string> { profile.HundredWords[(int)hundreds] };

        if (remainder > 0)
        {
            parts.Add($"{profile.ConjunctionWord} {ConvertUnderHundred(remainder)}");
        }

        return string.Join(" ", parts);
    }

    string ConvertUnderHundred(long number)
    {
        if (number < profile.UnitsMap.Length)
        {
            return profile.UnitsMap[(int)number];
        }

        var tens = (int)(number / 10);
        var remainder = number % 10;
        var tensWord = profile.TensMap[tens];

        return remainder == 0
            ? tensWord
            : tensWord + profile.TensRemainderJoiner + profile.UnitsMap[(int)remainder];
    }

    string ApplyScaleLinking(string words)
    {
        foreach (var rule in profile.ScaleLinkRules)
        {
            if (words.EndsWith(rule.Suffix, StringComparison.Ordinal))
            {
                return words[..^rule.Suffix.Length] + rule.Replacement;
            }
        }

        return words;
    }
}

sealed class LinkingScaleNumberToWordsProfile(
    string zeroWord,
    string minusWord,
    string conjunctionWord,
    string tensRemainderJoiner,
    string[] unitsMap,
    string[] tensMap,
    string[] hundredWords,
    string[] scalePrefixes,
    LinkingScale[] scales,
    LinkingSuffixRule[] scaleLinkRules)
{
    public string ZeroWord { get; } = zeroWord;
    public string MinusWord { get; } = minusWord;
    public string ConjunctionWord { get; } = conjunctionWord;
    public string TensRemainderJoiner { get; } = tensRemainderJoiner;
    public string[] UnitsMap { get; } = unitsMap;
    public string[] TensMap { get; } = tensMap;
    public string[] HundredWords { get; } = hundredWords;
    public string[] ScalePrefixes { get; } = scalePrefixes;
    public LinkingScale[] Scales { get; } = scales;
    public LinkingSuffixRule[] ScaleLinkRules { get; } = scaleLinkRules;
}

readonly record struct LinkingScale(long Value, string Name);

readonly record struct LinkingSuffixRule(string Suffix, string Replacement);

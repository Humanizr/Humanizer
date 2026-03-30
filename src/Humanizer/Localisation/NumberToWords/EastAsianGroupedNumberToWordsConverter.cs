namespace Humanizer;

class EastAsianGroupedNumberToWordsConverter(EastAsianGroupedNumberToWordsProfile profile) : GenderlessNumberToWordsConverter
{
    readonly EastAsianGroupedNumberToWordsProfile profile = profile;

    public override string Convert(long number) =>
        number < 0
            ? profile.NegativePrefix + ConvertPositive(-number)
            : ConvertPositive(number);

    public override string ConvertToOrdinal(int number)
    {
        if (profile.OrdinalMap?.TryGetValue(number, out var ordinal) == true)
        {
            return ordinal;
        }

        return profile.OrdinalPrefix + ConvertPositive(number) + profile.OrdinalSuffix;
    }

    string ConvertPositive(long number)
    {
        if (number == 0)
        {
            return profile.ZeroWord;
        }

        Span<int> groups = stackalloc int[8];
        var groupCount = 0;
        while (number > 0)
        {
            groups[groupCount++] = (int)(number % 10_000);
            number /= 10_000;
        }

        var builder = new StringBuilder();
        var emittedAnyGroup = false;
        var pendingGroupZero = false;

        for (var groupIndex = groupCount - 1; groupIndex >= 0; groupIndex--)
        {
            var groupValue = groups[groupIndex];
            if (groupValue == 0)
            {
                if (emittedAnyGroup)
                {
                    pendingGroupZero = true;
                }

                continue;
            }

            if (pendingGroupZero ||
                emittedAnyGroup && profile.InsertZeroBetweenGroups && groupValue < 1000)
            {
                builder.Append(profile.ZeroWord);
            }

            builder.Append(RenderGroup(groupValue));
            builder.Append(profile.LargeUnits[groupIndex]);
            emittedAnyGroup = true;
            pendingGroupZero = false;
        }

        return builder.ToString();
    }

    string RenderGroup(int groupValue)
    {
        Span<int> digits = stackalloc int[4];
        digits[0] = groupValue / 1000;
        digits[1] = groupValue / 100 % 10;
        digits[2] = groupValue / 10 % 10;
        digits[3] = groupValue % 10;

        var builder = new StringBuilder();
        var pendingZero = false;

        for (var index = 0; index < digits.Length; index++)
        {
            var digit = digits[index];
            if (digit == 0)
            {
                if (profile.InsertZeroInGroup &&
                    builder.Length > 0 &&
                    HasNonZeroDigitAfter(digits[(index + 1)..]))
                {
                    pendingZero = true;
                }

                continue;
            }

            if (pendingZero)
            {
                builder.Append(profile.ZeroWord);
                pendingZero = false;
            }

            var smallUnitIndex = digits.Length - index - 1;
            if (smallUnitIndex == 0)
            {
                builder.Append(profile.DigitWords[digit]);
                continue;
            }

            if (!ShouldOmitOne(digit, smallUnitIndex, builder.Length == 0 && !pendingZero))
            {
                builder.Append(profile.DigitWords[digit]);
            }

            builder.Append(profile.SmallUnitWords[smallUnitIndex]);
        }

        return builder.ToString();
    }

    bool ShouldOmitOne(int digit, int smallUnitIndex, bool isLeadingDigit) =>
        digit == 1 && smallUnitIndex switch
        {
            1 => profile.OmitOneBeforeTen &&
                 (!profile.OmitOneBeforeTenOnlyWhenLeading || isLeadingDigit),
            2 => profile.OmitOneBeforeHundred,
            3 => profile.OmitOneBeforeThousand,
            _ => false
        };

    static bool HasNonZeroDigitAfter(ReadOnlySpan<int> digits)
    {
        foreach (var digit in digits)
        {
            if (digit != 0)
            {
                return true;
            }
        }

        return false;
    }
}

sealed class EastAsianGroupedNumberToWordsProfile(
    string zeroWord,
    string negativePrefix,
    string ordinalPrefix,
    string ordinalSuffix,
    string[] digitWords,
    string[] smallUnitWords,
    string[] largeUnits,
    bool omitOneBeforeTen,
    bool omitOneBeforeTenOnlyWhenLeading,
    bool omitOneBeforeHundred,
    bool omitOneBeforeThousand,
    bool insertZeroInGroup,
    bool insertZeroBetweenGroups,
    FrozenDictionary<int, string>? ordinalMap = null)
{
    public string ZeroWord { get; } = zeroWord;
    public string NegativePrefix { get; } = negativePrefix;
    public string OrdinalPrefix { get; } = ordinalPrefix;
    public string OrdinalSuffix { get; } = ordinalSuffix;
    public string[] DigitWords { get; } = digitWords;
    public string[] SmallUnitWords { get; } = smallUnitWords;
    public string[] LargeUnits { get; } = largeUnits;
    public bool OmitOneBeforeTen { get; } = omitOneBeforeTen;
    public bool OmitOneBeforeTenOnlyWhenLeading { get; } = omitOneBeforeTenOnlyWhenLeading;
    public bool OmitOneBeforeHundred { get; } = omitOneBeforeHundred;
    public bool OmitOneBeforeThousand { get; } = omitOneBeforeThousand;
    public bool InsertZeroInGroup { get; } = insertZeroInGroup;
    public bool InsertZeroBetweenGroups { get; } = insertZeroBetweenGroups;
    public FrozenDictionary<int, string>? OrdinalMap { get; } = ordinalMap;
}

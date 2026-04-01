namespace Humanizer;

/// <summary>
/// Shared renderer for East Asian grouped numbering systems that count in four-digit groups.
///
/// The generated profile provides the digit lexicon, small-unit lexicon, and zero-insertion rules
/// so the runtime can consistently walk the 10,000-based group structure.
/// </summary>
class EastAsianGroupedNumberToWordsConverter(EastAsianGroupedNumberToWordsProfile profile) : GenderlessNumberToWordsConverter
{
    readonly EastAsianGroupedNumberToWordsProfile profile = profile;

    /// <summary>
    /// Converts the given value using the locale's East Asian grouping rules.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <returns>The localized cardinal words for <paramref name="number"/>.</returns>
    public override string Convert(long number) =>
        number < 0
            ? profile.NegativePrefix + ConvertPositive(-number)
            : ConvertPositive(number);

    /// <summary>
    /// Converts the given value to the locale's ordinal form.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <returns>The localized ordinal words for <paramref name="number"/>.</returns>
    public override string ConvertToOrdinal(int number)
    {
        if (profile.OrdinalMap?.TryGetValue(number, out var ordinal) == true)
        {
            return ordinal;
        }

        return profile.OrdinalPrefix + ConvertPositive(number) + profile.OrdinalSuffix;
    }

    /// <summary>
    /// Converts a four-digit East Asian group into text.
    /// </summary>
    string ConvertPositive(long number)
    {
        if (number == 0)
        {
            return profile.ZeroWord;
        }

        // Walk 10,000-based groups from most significant to least significant so zero insertion is
        // decided with full context instead of guessing from the terminal group alone.
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
                    // A zero group only matters after we've already spoken something; otherwise it
                    // would create a leading zero token.
                    pendingGroupZero = true;
                }

                continue;
            }

            if (pendingGroupZero ||
                emittedAnyGroup && profile.InsertZeroBetweenGroups && groupValue < 1000)
            {
                // Some locales require an explicit zero between spoken groups when a lower group is
                // too small to stand alone after a skipped higher group.
                builder.Append(profile.ZeroWord);
            }

            builder.Append(RenderGroup(groupValue));
            builder.Append(profile.LargeUnits[groupIndex]);
            emittedAnyGroup = true;
            pendingGroupZero = false;
        }

        return builder.ToString();
    }

    /// <summary>
    /// Renders one four-digit group with the locale's zero and omission rules.
    /// </summary>
    string RenderGroup(int groupValue)
    {
        // Each four-digit group is rendered independently so the zero and omission rules stay
        // local to that group.
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
                    // Interior zeros only appear when there is still a spoken digit later in the
                    // same group.
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

            // The "one" omission rule is position-sensitive, so the locale decides per small-unit
            // slot whether the leading digit should be spoken explicitly.
            if (!ShouldOmitOne(digit, smallUnitIndex, builder.Length == 0 && !pendingZero))
            {
                builder.Append(profile.DigitWords[digit]);
            }

            builder.Append(profile.SmallUnitWords[smallUnitIndex]);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Determines whether the leading one may be omitted for the given small-unit position.
    /// </summary>
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

/// <summary>
/// Immutable generated profile for <see cref="EastAsianGroupedNumberToWordsConverter"/>.
/// </summary>
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
    /// <summary>
    /// Gets the cardinal zero word.
    /// </summary>
    public string ZeroWord { get; } = zeroWord;
    /// <summary>
    /// Gets the prefix used for negative values.
    /// </summary>
    public string NegativePrefix { get; } = negativePrefix;
    /// <summary>
    /// Gets the ordinal prefix.
    /// </summary>
    public string OrdinalPrefix { get; } = ordinalPrefix;
    /// <summary>
    /// Gets the ordinal suffix.
    /// </summary>
    public string OrdinalSuffix { get; } = ordinalSuffix;
    /// <summary>
    /// Gets the digit lexicon.
    /// </summary>
    public string[] DigitWords { get; } = digitWords;
    /// <summary>
    /// Gets the small-unit lexicon used within a group.
    /// </summary>
    public string[] SmallUnitWords { get; } = smallUnitWords;
    /// <summary>
    /// Gets the large-unit lexicon used between groups.
    /// </summary>
    public string[] LargeUnits { get; } = largeUnits;
    /// <summary>
    /// Gets a value indicating whether one should be omitted before ten.
    /// </summary>
    public bool OmitOneBeforeTen { get; } = omitOneBeforeTen;
    /// <summary>
    /// Gets a value indicating whether omission before ten applies only when the digit is leading.
    /// </summary>
    public bool OmitOneBeforeTenOnlyWhenLeading { get; } = omitOneBeforeTenOnlyWhenLeading;
    /// <summary>
    /// Gets a value indicating whether one should be omitted before hundred.
    /// </summary>
    public bool OmitOneBeforeHundred { get; } = omitOneBeforeHundred;
    /// <summary>
    /// Gets a value indicating whether one should be omitted before thousand.
    /// </summary>
    public bool OmitOneBeforeThousand { get; } = omitOneBeforeThousand;
    /// <summary>
    /// Gets a value indicating whether zero tokens are inserted within groups.
    /// </summary>
    public bool InsertZeroInGroup { get; } = insertZeroInGroup;
    /// <summary>
    /// Gets a value indicating whether zero tokens are inserted between groups.
    /// </summary>
    public bool InsertZeroBetweenGroups { get; } = insertZeroBetweenGroups;
    /// <summary>
    /// Gets exact ordinal overrides keyed by value.
    /// </summary>
    public FrozenDictionary<int, string>? OrdinalMap { get; } = ordinalMap;
}

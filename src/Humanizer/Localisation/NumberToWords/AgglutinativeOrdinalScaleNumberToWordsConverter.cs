namespace Humanizer;

/// <summary>
/// Shared renderer for agglutinative locales where ordinals are built from scale words and
/// locale-specific suffixes instead of from a recursive cardinal/ordinal rewrite.
///
/// The generated profile supplies the unit tables, scale rows, and ordinal exceptions so the
/// converter can keep the same decomposition order across locales while still honoring
/// language-specific affixes.
/// </summary>
class AgglutinativeOrdinalScaleNumberToWordsConverter(AgglutinativeOrdinalScaleNumberToWordsProfile profile) : GenderlessNumberToWordsConverter
{
    readonly AgglutinativeOrdinalScaleNumberToWordsProfile profile = profile;

    /// <summary>
    /// Converts the given value into a cardinal phrase for the locale.
    /// </summary>
    /// <param name="input">The value to convert.</param>
    /// <returns>The localized cardinal words for <paramref name="input"/>.</returns>
    public override string Convert(long input)
    {
        if (input < 0)
        {
            return profile.MinusWord + Convert(-input);
        }

        return ConvertCardinal(input);
    }

    /// <summary>
    /// Converts the given value into a locale-specific ordinal phrase.
    /// </summary>
    /// <param name="number">The value to convert.</param>
    /// <returns>The localized ordinal words for <paramref name="number"/>.</returns>
    public override string ConvertToOrdinal(int number) =>
        ConvertOrdinal(number, useExceptions: false);

    string ConvertCardinal(long number)
    {
        if (number == 0)
        {
            return profile.UnitsMap[0];
        }

        var parts = new List<string>();
        var remainder = number;

        // Walk the generated scale table from largest to smallest so each row can consume its
        // share of the remainder before tens and units are handled.
        foreach (var scale in profile.Scales)
        {
            var divisor = scale.Value;
            if (remainder / divisor <= 0)
            {
                continue;
            }

            var count = remainder / divisor;
            var scaleRemainder = remainder % divisor;
            // The scale word is data-driven because some locales keep a singular/plural pair while
            // others only need one scale stem.
            parts.Add((count == 1
                ? scale.SingularCardinal
                : ConvertCardinal(count) + scale.PluralCardinal) +
                (scale.Value >= 1000 && scaleRemainder > 0 ? " " : string.Empty));
            remainder = scaleRemainder;
        }

        if (remainder >= 20)
        {
            // Tens are built from the recursive unit renderer plus a locale-specific suffix.
            parts.Add(ConvertCardinal(remainder / 10) + profile.TensSuffix);
            remainder %= 10;
        }
        else if (remainder is > 10 and < 20)
        {
            // Teen forms use a separate suffix so the unit stem can stay intact.
            parts.Add(profile.UnitsMap[(int)(remainder % 10)] + profile.TeenSuffix);
            remainder = 0;
        }

        if (remainder is > 0 and <= 10)
        {
            parts.Add(profile.UnitsMap[(int)remainder]);
        }

        return string.Concat(parts);
    }

    string ConvertOrdinal(int number, bool useExceptions)
    {
        if (number == 0)
        {
            return profile.OrdinalUnitsMap[0];
        }

        if (number < 0)
        {
            return profile.MinusWord + ConvertOrdinal(-number, useExceptions: false);
        }

        var parts = new List<string>();
        var remainder = number;

        // Ordinals share the same scale walk, but the terminal piece uses ordinal words and only
        // the recursive count is allowed to opt into ordinal exceptions.
        foreach (var scale in profile.Scales)
        {
            var divisor = scale.Value;
            if (remainder / divisor <= 0)
            {
                continue;
            }

            var count = (int)(remainder / divisor);
            var scaleRemainder = (int)(remainder % divisor);
            parts.Add((count == 1 ? string.Empty : ConvertOrdinal(count, useExceptions: true)) + scale.OrdinalWord);
            remainder = scaleRemainder;
        }

        if (remainder >= 20)
        {
            // Tens ordinals are suffix-driven, so only the count portion recurses.
            parts.Add(ConvertOrdinal(remainder / 10, useExceptions: true) + profile.OrdinalTensSuffix);
            remainder %= 10;
        }
        else if (remainder is > 10 and < 20)
        {
            // Teen ordinals use the unit ordinal map, not the cardinal teen suffix.
            parts.Add(GetOrdinalUnit(remainder % 10, useExceptions: true) + profile.TeenSuffix);
            remainder = 0;
        }

        if (remainder is > 0 and <= 10)
        {
            parts.Add(GetOrdinalUnit(remainder, useExceptions));
        }

        return string.Concat(parts);
    }

    string GetOrdinalUnit(int number, bool useExceptions) =>
        // The irregular ordinal unit forms are only used when the recursion explicitly permits
        // them; higher scale counts should keep the regular unit table.
        useExceptions && profile.OrdinalExceptions.TryGetValue(number, out var unit)
            ? unit
            : profile.OrdinalUnitsMap[number];
}

/// <summary>
/// Immutable generated profile for <see cref="AgglutinativeOrdinalScaleNumberToWordsConverter"/>.
/// </summary>
sealed class AgglutinativeOrdinalScaleNumberToWordsProfile(
    string minusWord,
    string tensSuffix,
    string teenSuffix,
    string ordinalTensSuffix,
    string[] unitsMap,
    string[] ordinalUnitsMap,
    AgglutinativeOrdinalScale[] scales,
    FrozenDictionary<int, string> ordinalExceptions)
{
    /// <summary>
    /// Gets the word used to prefix negative values.
    /// </summary>
    public string MinusWord { get; } = minusWord;
    /// <summary>
    /// Gets the suffix appended to cardinal tens.
    /// </summary>
    public string TensSuffix { get; } = tensSuffix;
    /// <summary>
    /// Gets the suffix appended to teen cardinals.
    /// </summary>
    public string TeenSuffix { get; } = teenSuffix;
    /// <summary>
    /// Gets the suffix appended to ordinal tens.
    /// </summary>
    public string OrdinalTensSuffix { get; } = ordinalTensSuffix;
    /// <summary>
    /// Gets the cardinal unit lexicon.
    /// </summary>
    public string[] UnitsMap { get; } = unitsMap;
    /// <summary>
    /// Gets the ordinal unit lexicon.
    /// </summary>
    public string[] OrdinalUnitsMap { get; } = ordinalUnitsMap;
    /// <summary>
    /// Gets the descending scale rows used during decomposition.
    /// </summary>
    public AgglutinativeOrdinalScale[] Scales { get; } = scales;
    /// <summary>
    /// Gets exact ordinal exceptions keyed by unit value.
    /// </summary>
    public FrozenDictionary<int, string> OrdinalExceptions { get; } = ordinalExceptions;
}

/// <summary>
/// One descending scale row for <see cref="AgglutinativeOrdinalScaleNumberToWordsConverter"/>.
/// </summary>
readonly record struct AgglutinativeOrdinalScale(long Value, string SingularCardinal, string PluralCardinal, string OrdinalWord);

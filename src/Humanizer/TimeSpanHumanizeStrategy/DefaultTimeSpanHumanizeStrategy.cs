namespace Humanizer;

/// <summary>
/// The default strategy for converting <see cref="TimeSpan"/> values into human-readable text.
/// </summary>
public class DefaultTimeSpanHumanizeStrategy : IGrammaticalCaseTimeSpanHumanizeStrategy
{
    /// <inheritdoc />
    public string Humanize(
        TimeSpan timeSpan,
        int precision,
        bool countEmptyUnits,
        CultureInfo? culture,
        TimeUnit maxUnit,
        TimeUnit minUnit,
        string? collectionSeparator,
        bool toWords,
        bool toSymbols) =>
        TimeSpanHumanizeExtensions.DefaultHumanize(
            timeSpan,
            precision,
            countEmptyUnits,
            culture,
            maxUnit,
            minUnit,
            collectionSeparator,
            toWords,
            toSymbols);

    /// <summary>
    /// Converts a <see cref="TimeSpan"/> into human-readable text with seconds as the minimum unit.
    /// </summary>
    /// <param name="timeSpan">The time span to humanize.</param>
    /// <param name="precision">The maximum number of time units to return.</param>
    /// <param name="countEmptyUnits">Whether empty time units count toward <paramref name="precision"/>.</param>
    /// <param name="culture">The culture to use. If null, the current culture is used.</param>
    /// <param name="maxUnit">The maximum unit of time to output.</param>
    /// <param name="collectionSeparator">The separator used to combine time parts. If null, the culture's default collection formatter is used.</param>
    /// <param name="maxFractionalDigits">The maximum number of fractional-second digits, from 0 through 7.</param>
    /// <param name="roundingMode">The midpoint rounding mode.</param>
    /// <param name="toSymbols">Whether time units are rendered as symbols.</param>
    /// <returns>The human-readable time span.</returns>
    public virtual string HumanizeWithFractionalSeconds(
        TimeSpan timeSpan,
        int precision,
        bool countEmptyUnits,
        CultureInfo? culture,
        TimeUnit maxUnit,
        string? collectionSeparator,
        int maxFractionalDigits,
        MidpointRounding roundingMode,
        bool toSymbols) =>
        TimeSpanHumanizeExtensions.DefaultHumanizeWithFractionalSeconds(
            timeSpan,
            precision,
            countEmptyUnits,
            culture,
            maxUnit,
            collectionSeparator,
            maxFractionalDigits,
            roundingMode,
            toSymbols);

    string IGrammaticalCaseTimeSpanHumanizeStrategy.Humanize(
        TimeSpan timeSpan,
        int precision,
        bool countEmptyUnits,
        CultureInfo? culture,
        TimeUnit maxUnit,
        TimeUnit minUnit,
        string? collectionSeparator,
        bool toSymbols,
        GrammaticalCase grammaticalCase)
    {
        if (GetType().Assembly != typeof(DefaultTimeSpanHumanizeStrategy).Assembly)
        {
            throw new NotSupportedException(
                $"Custom strategy type '{GetType().FullName}' must explicitly implement {nameof(IGrammaticalCaseTimeSpanHumanizeStrategy)} to support grammatical-case-aware durations.");
        }

        return TimeSpanHumanizeExtensions.DefaultHumanizeWithCase(
            timeSpan,
            precision,
            countEmptyUnits,
            culture,
            maxUnit,
            minUnit,
            collectionSeparator,
            toSymbols,
            grammaticalCase);
    }
}
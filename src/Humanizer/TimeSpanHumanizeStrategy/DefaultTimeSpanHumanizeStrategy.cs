namespace Humanizer;

/// <summary>
/// The default strategy for converting <see cref="TimeSpan"/> values into human-readable text.
/// </summary>
public class DefaultTimeSpanHumanizeStrategy : ITimeSpanHumanizeStrategy, IFractionalTimeSpanHumanizeStrategy
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

    /// <inheritdoc />
    public string HumanizeWithFractionalSeconds(
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
}
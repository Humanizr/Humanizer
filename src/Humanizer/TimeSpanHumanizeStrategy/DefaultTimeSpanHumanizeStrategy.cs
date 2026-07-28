namespace Humanizer;

/// <summary>
/// The default strategy for converting <see cref="TimeSpan"/> values into human-readable text.
/// </summary>
public class DefaultTimeSpanHumanizeStrategy : ITimeSpanHumanizeStrategy
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
}
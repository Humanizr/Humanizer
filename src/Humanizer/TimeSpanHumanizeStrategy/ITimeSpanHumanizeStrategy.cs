namespace Humanizer;

/// <summary>
/// Defines a strategy for converting <see cref="TimeSpan"/> values into human-readable text.
/// </summary>
public interface ITimeSpanHumanizeStrategy
{
    /// <summary>
    /// Converts a <see cref="TimeSpan"/> into human-readable text.
    /// </summary>
    /// <param name="timeSpan">The time span to humanize.</param>
    /// <param name="precision">The maximum number of time units to return.</param>
    /// <param name="countEmptyUnits">Whether empty time units count toward <paramref name="precision"/>.</param>
    /// <param name="culture">The culture to use. If null, the current culture is used.</param>
    /// <param name="maxUnit">The maximum unit of time to output.</param>
    /// <param name="minUnit">The minimum unit of time to output.</param>
    /// <param name="collectionSeparator">The separator used to combine time parts. If null, the culture's default collection formatter is used.</param>
    /// <param name="toWords">Whether numbers are rendered as words.</param>
    /// <param name="toSymbols">Whether time units are rendered as symbols.</param>
    /// <returns>The human-readable time span.</returns>
    string Humanize(
        TimeSpan timeSpan,
        int precision,
        bool countEmptyUnits,
        CultureInfo? culture,
        TimeUnit maxUnit,
        TimeUnit minUnit,
        string? collectionSeparator,
        bool toWords,
        bool toSymbols);
}
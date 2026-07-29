namespace Humanizer;

/// <summary>
/// Optionally extends a time-span humanization strategy with grammatical-case support.
/// Existing <see cref="ITimeSpanHumanizeStrategy"/> implementations remain valid for existing
/// duration APIs but cannot service <c>HumanizeWithCase</c>.
/// </summary>
public interface IGrammaticalCaseTimeSpanHumanizeStrategy : ITimeSpanHumanizeStrategy
{
    /// <summary>
    /// Converts a <see cref="TimeSpan"/> using locale-authored unit-case phrases.
    /// </summary>
    /// <param name="timeSpan">The time span to humanize.</param>
    /// <param name="precision">The maximum number of time units to return.</param>
    /// <param name="countEmptyUnits">Whether empty time units count toward <paramref name="precision"/>.</param>
    /// <param name="culture">The culture to use. If null, the current culture is used.</param>
    /// <param name="maxUnit">The maximum unit of time to output.</param>
    /// <param name="minUnit">The minimum unit of time to output.</param>
    /// <param name="collectionSeparator">The separator used to combine time parts. If null, the culture's default collection formatter is used.</param>
    /// <param name="toSymbols">Whether time units are rendered as symbols.</param>
    /// <param name="grammaticalCase">The grammatical case used to select each unit phrase.</param>
    /// <returns>A bare locale-authored duration phrase whose count may be explicit or encoded by the unit form.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="grammaticalCase"/> is outside its defined enum range.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// The selected locale, formatter, or duration unit does not have verified support for
    /// <paramref name="grammaticalCase"/>.
    /// </exception>
    string Humanize(
        TimeSpan timeSpan,
        int precision,
        bool countEmptyUnits,
        CultureInfo? culture,
        TimeUnit maxUnit,
        TimeUnit minUnit,
        string? collectionSeparator,
        bool toSymbols,
        GrammaticalCase grammaticalCase);
}
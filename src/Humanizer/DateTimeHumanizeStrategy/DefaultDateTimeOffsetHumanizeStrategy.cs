namespace Humanizer;

/// <summary>
/// The default 'distance of time' -> words calculator.
/// </summary>
public class DefaultDateTimeOffsetHumanizeStrategy : IDateTimeOffsetHumanizeStrategy
{
    /// <summary>
    /// Calculates the distance of time in words between two provided dates
    /// </summary>
    public string Humanize(DateTimeOffset input, DateTimeOffset comparisonBase, CultureInfo? culture) =>
        DateTimeHumanizeAlgorithms.DefaultHumanize(input.UtcDateTime, comparisonBase.UtcDateTime, culture);
}
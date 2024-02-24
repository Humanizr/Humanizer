namespace Humanizer;

/// <summary>
/// The default 'distance of time' -> words calculator.
/// </summary>
public class DefaultDateTimeHumanizeStrategy : IDateTimeHumanizeStrategy
{
    /// <summary>
    /// Calculates the distance of time in words between two provided dates
    /// </summary>
    public string Humanize(DateTime input, DateTime comparisonBase, CultureInfo? culture) =>
        DateTimeHumanizeAlgorithms.DefaultHumanize(input, comparisonBase, culture);
}
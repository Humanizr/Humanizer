#if NET6_0_OR_GREATER

namespace Humanizer;

/// <summary>
/// The default 'distance of time' -> words calculator.
/// </summary>
public class DefaultDateOnlyHumanizeStrategy : IDateOnlyHumanizeStrategy
{
    /// <summary>
    /// Calculates the distance of time in words between two provided dates
    /// </summary>
    public string Humanize(DateOnly input, DateOnly comparisonBase, CultureInfo? culture) =>
        DateTimeHumanizeAlgorithms.DefaultHumanize(input, comparisonBase, culture);
}

#endif

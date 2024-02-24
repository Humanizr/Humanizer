#if NET6_0_OR_GREATER

namespace Humanizer;

/// <summary>
/// The default 'distance of time' -> words calculator.
/// </summary>
public class DefaultTimeOnlyHumanizeStrategy : ITimeOnlyHumanizeStrategy
{
    /// <summary>
    /// Calculates the distance of time in words between two provided times
    /// </summary>
    public string Humanize(TimeOnly input, TimeOnly comparisonBase, CultureInfo? culture) =>
        DateTimeHumanizeAlgorithms.DefaultHumanize(input, comparisonBase, culture);
}

#endif

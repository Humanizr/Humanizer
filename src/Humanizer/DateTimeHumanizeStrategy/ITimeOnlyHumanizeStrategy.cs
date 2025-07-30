#if NET6_0_OR_GREATER

namespace Humanizer;

/// <summary>
/// Implement this interface to create a new strategy for TimeOnly.Humanize and hook it in the Configurator.TimeOnlyHumanizeStrategy
/// </summary>
public interface ITimeOnlyHumanizeStrategy
{
    /// <summary>
    /// Calculates the distance of time in words between two provided dates used for TimeOnly.Humanize
    /// </summary>
    string Humanize(TimeOnly input, TimeOnly comparisonBase, CultureInfo? culture);
}

#endif

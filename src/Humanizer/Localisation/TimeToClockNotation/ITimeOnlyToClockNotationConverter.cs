#if NET6_0_OR_GREATER

namespace Humanizer;

/// <summary>
/// Converts times into the localized text used by <c>ToClockNotation</c>.
/// </summary>
public interface ITimeOnlyToClockNotationConverter
{
    /// <summary>
    /// Converts the given <paramref name="time"/> to clock notation.
    /// </summary>
    /// <param name="time">The time to format.</param>
    /// <param name="roundToNearestFive">
    /// The rounding mode to apply before formatting the time.
    /// </param>
    /// <returns>The localized clock-notation string.</returns>
    string Convert(TimeOnly time, ClockNotationRounding roundToNearestFive);
}

#endif

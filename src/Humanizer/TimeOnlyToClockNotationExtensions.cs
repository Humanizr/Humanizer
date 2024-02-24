#if NET6_0_OR_GREATER

namespace Humanizer;

/// <summary>
/// Humanizes TimeOnly into human readable sentence
/// </summary>
public static class TimeOnlyToClockNotationExtensions
{
    /// <summary>
    /// Turns the provided time into clock notation
    /// </summary>
    /// <param name="input">The time to be made into clock notation</param>
    /// <param name="roundToNearestFive">Whether to round the minutes to the nearest five or not</param>
    /// <returns>The time in clock notation</returns>
    public static string ToClockNotation(this TimeOnly input, ClockNotationRounding roundToNearestFive = ClockNotationRounding.None) =>
        Configurator.TimeOnlyToClockNotationConverter.Convert(input, roundToNearestFive);
}

#endif

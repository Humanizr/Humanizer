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
    /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
    /// <returns>The time in clock notation</returns>
    public static string ToClockNotation(this TimeOnly input, ClockNotationRounding roundToNearestFive = ClockNotationRounding.None, CultureInfo? culture = null) =>
        Configurator.TimeOnlyToClockNotationConverter(culture).Convert(input, roundToNearestFive, culture);
}

#endif

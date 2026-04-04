#if NET6_0_OR_GREATER

using System.Globalization;

namespace Humanizer;

/// <summary>
/// Provides Japanese clock notation using direct numeric hour and minute labels.
/// </summary>
class JapaneseTimeOnlyToClockNotationConverter : ITimeOnlyToClockNotationConverter
{
    /// <summary>
    /// Converts the given time to Japanese clock notation.
    /// </summary>
    /// <returns>The localized clock-notation string.</returns>
    public string Convert(TimeOnly time, ClockNotationRounding roundToNearestFive)
    {
        var normalizedTime = roundToNearestFive == ClockNotationRounding.NearestFiveMinutes
            ? time.AddMinutes(5 * Math.Round(time.Minute / 5.0) - time.Minute)
            : time;

        return normalizedTime.Hour.ToString(CultureInfo.InvariantCulture) +
            "時" +
            normalizedTime.Minute.ToString(CultureInfo.InvariantCulture) +
            "分";
    }
}

#endif

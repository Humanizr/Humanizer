#if NET6_0_OR_GREATER

namespace Humanizer;

/// <summary>
/// Provides French clock notation with direct hour-and-minute phrasing.
/// </summary>
class FrenchTimeOnlyToClockNotationConverter : ITimeOnlyToClockNotationConverter
{
    /// <summary>
    /// Converts the given time to French clock notation.
    /// </summary>
    /// <returns>The localized clock-notation string.</returns>
    public string Convert(TimeOnly time, ClockNotationRounding roundToNearestFive)
    {
        // French clock notation in Humanizer currently follows the direct "hour + spoken minutes"
        // pattern rather than the relative half-hour family used by Catalan/Spanish or the bucketed
        // German/Luxembourgish family. This remains a dedicated converter until a second locale
        // proves out a coherent structural engine for the same spoken-time rules.
        // Keep the rounded minute as a separate value so the 60-minute rollover can still be
        // represented explicitly instead of being flattened away too early.
        var normalizedMinutes = (int)(roundToNearestFive == ClockNotationRounding.NearestFiveMinutes
            ? 5 * Math.Round(time.Minute / 5.0)
            : time.Minute);

        return normalizedMinutes switch
        {
            00 => GetHourExpression(time.Hour),
            60 => GetHourExpression(time.Hour + 1),
            _ => $"{GetHourExpression(time.Hour)} {normalizedMinutes.ToWords(GrammaticalGender.Feminine)}"
        };

        static string GetHourExpression(int hour) =>
            hour switch
            {
                0 => "minuit",
                12 => "midi",
                // Spoken French uses singular only for one o'clock; higher values carry the plural
                // noun directly in the hour phrase.
                _ => hour.ToWords(GrammaticalGender.Feminine) + (hour > 1 ? " heures" : " heure")
            };
    }
}

#endif
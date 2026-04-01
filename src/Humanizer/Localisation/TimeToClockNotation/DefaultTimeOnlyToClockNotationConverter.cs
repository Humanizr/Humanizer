#if NET6_0_OR_GREATER

namespace Humanizer;

/// <summary>
/// Provides the default conversion from <see cref="TimeOnly"/> values to spoken clock notation.
/// </summary>
/// <remarks>
/// Pass a culture to override <see cref="CultureInfo.CurrentCulture"/>; otherwise the current
/// culture is read when <see cref="Convert(TimeOnly, ClockNotationRounding)"/> runs.
/// </remarks>
class DefaultTimeOnlyToClockNotationConverter(CultureInfo? culture = null) : ITimeOnlyToClockNotationConverter
{
    /// <summary>
    /// Converts the given time to the default spoken clock notation for the selected culture.
    /// </summary>
    /// <returns>The localized clock-notation string.</returns>
    public string Convert(TimeOnly time, ClockNotationRounding roundToNearestFive)
    {
        // Round once up front so the English bucket phrases and the non-English fallback both
        // see the same normalized time value.
        var cultureInfo = culture ?? CultureInfo.CurrentCulture;
        var normalizedTime = roundToNearestFive == ClockNotationRounding.NearestFiveMinutes
            ? time.AddMinutes(5 * Math.Round(time.Minute / 5.0) - time.Minute)
            : time;

        if (cultureInfo.TwoLetterISOLanguageName != "en")
        {
            // The default non-English path is intentionally boring: it preserves the culture's own
            // short-time rendering rather than inventing English-style spoken phrases.
            return normalizedTime.ToString("t", cultureInfo);
        }

        // Keep the special labels explicit because "midnight" and "noon" are not derivable from
        // the generic hour/minute buckets below.
        switch (normalizedTime)
        {
            case { Hour: 0, Minute: 0 }:
                return "midnight";
            case { Hour: 12, Minute: 0 }:
                return "noon";
        }

        var normalizedHour = normalizedTime.Hour % 12;
        var normalizedMinutes = normalizedTime.Minute;

        // The rounded-minute sentinel can reach 60; keep that branch so rounding to the next hour
        // stays in the same control flow instead of requiring a second normalization pass.
        return normalizedMinutes switch
        {
            00 => $"{normalizedHour.ToWords()} o'clock",
            05 => $"five past {normalizedHour.ToWords()}",
            10 => $"ten past {normalizedHour.ToWords()}",
            15 => $"a quarter past {normalizedHour.ToWords()}",
            20 => $"twenty past {normalizedHour.ToWords()}",
            25 => $"twenty-five past {normalizedHour.ToWords()}",
            30 => $"half past {normalizedHour.ToWords()}",
            40 => $"twenty to {(normalizedHour + 1).ToWords()}",
            45 => $"a quarter to {(normalizedHour + 1).ToWords()}",
            50 => $"ten to {(normalizedHour + 1).ToWords()}",
            55 => $"five to {(normalizedHour + 1).ToWords()}",
            60 => $"{(normalizedHour + 1).ToWords()} o'clock",
            _ => $"{normalizedHour.ToWords()} {normalizedMinutes.ToWords()}"
        };
    }
}

#endif

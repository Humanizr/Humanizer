#if NET6_0_OR_GREATER

namespace Humanizer;

class DefaultTimeOnlyToClockNotationConverter : ITimeOnlyToClockNotationConverter
{
    public string Convert(TimeOnly time, ClockNotationRounding roundToNearestFive, CultureInfo? culture)
    {
        switch (time)
        {
            case { Hour: 0, Minute: 0 }:
                return "midnight";
            case { Hour: 12, Minute: 0 }:
                return "noon";
        }

        var normalizedHour = time.Hour % 12;
        var normalizedMinutes = (int)(roundToNearestFive == ClockNotationRounding.NearestFiveMinutes
            ? 5 * Math.Round(time.Minute / 5.0)
            : time.Minute);

        return normalizedMinutes switch
        {
            00 => $"{normalizedHour.ToWords(culture)} o'clock",
            05 => $"five past {normalizedHour.ToWords(culture)}",
            10 => $"ten past {normalizedHour.ToWords(culture)}",
            15 => $"a quarter past {normalizedHour.ToWords(culture)}",
            20 => $"twenty past {normalizedHour.ToWords(culture)}",
            25 => $"twenty-five past {normalizedHour.ToWords(culture)}",
            30 => $"half past {normalizedHour.ToWords(culture)}",
            40 => $"twenty to {(normalizedHour + 1).ToWords(culture)}",
            45 => $"a quarter to {(normalizedHour + 1).ToWords(culture)}",
            50 => $"ten to {(normalizedHour + 1).ToWords(culture)}",
            55 => $"five to {(normalizedHour + 1).ToWords(culture)}",
            60 => $"{(normalizedHour + 1).ToWords(culture)} o'clock",
            _ => $"{normalizedHour.ToWords(culture)} {normalizedMinutes.ToWords(culture)}"
        };
    }
}

#endif

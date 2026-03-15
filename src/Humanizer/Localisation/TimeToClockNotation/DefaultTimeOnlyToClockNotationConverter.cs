#if NET6_0_OR_GREATER

namespace Humanizer;

class DefaultTimeOnlyToClockNotationConverter(CultureInfo? culture = null) : ITimeOnlyToClockNotationConverter
{
    public string Convert(TimeOnly time, ClockNotationRounding roundToNearestFive)
    {
        var cultureInfo = culture ?? CultureInfo.CurrentCulture;
        var normalizedTime = roundToNearestFive == ClockNotationRounding.NearestFiveMinutes
            ? time.AddMinutes(5 * Math.Round(time.Minute / 5.0) - time.Minute)
            : time;

        if (cultureInfo.TwoLetterISOLanguageName != "en")
        {
            return normalizedTime.ToString("t", cultureInfo);
        }

        switch (normalizedTime)
        {
            case { Hour: 0, Minute: 0 }:
                return "midnight";
            case { Hour: 12, Minute: 0 }:
                return "noon";
        }

        var normalizedHour = normalizedTime.Hour % 12;
        var normalizedMinutes = normalizedTime.Minute;

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

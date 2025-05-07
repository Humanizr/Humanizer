#if NET6_0_OR_GREATER

namespace Humanizer;

class GermanTimeOnlyToClockNotationConverter : ITimeOnlyToClockNotationConverter
{
    public string Convert(TimeOnly time, ClockNotationRounding roundToNearestFive)
    {
        switch (time)
        {
            case { Hour: 0, Minute: 0 }:
                return "zwölf Uhr nachts";
            case { Hour: 12, Minute: 0 }:
                return "zwölf Uhr mittags";
        }

        var normalizedHour = time.Hour % 12;
        var normalizedMinutes = (int)(roundToNearestFive == ClockNotationRounding.NearestFiveMinutes
            ? 5 * Math.Round(time.Minute / 5.0)
            : time.Minute);

        return normalizedMinutes switch
        {
            00 => $"{normalizedHour.ToWords()} Uhr",
            05 => $"fünf nach {normalizedHour.ToWords()}",
            10 => $"zehn nach {normalizedHour.ToWords()}",
            15 => $"viertel nach {normalizedHour.ToWords()}",
            20 => $"zwanzig nach {normalizedHour.ToWords()}",
            25 => $"fünf vor halb {(normalizedHour + 1).ToWords()}",
            30 => $"halb {(normalizedHour + 1).ToWords()}",
            35 => $"fünf nach halb {(normalizedHour + 1).ToWords()}",
            40 => $"zwanzig vor {(normalizedHour + 1).ToWords()}",
            45 => $"viertel vor {(normalizedHour + 1).ToWords()}",
            50 => $"zehn vor {(normalizedHour + 1).ToWords()}",
            55 => $"fünf vor {(normalizedHour + 1).ToWords()}",
            60 => $"{(normalizedHour + 1).ToWords()} Uhr",
            _ => $"{normalizedHour.ToWords()} Uhr {normalizedMinutes.ToWords()}"
        };
    }
}

#endif

#if NET6_0_OR_GREATER

namespace Humanizer;

class PortugueseTimeOnlyToClockNotationConverter : ITimeOnlyToClockNotationConverter
{
    public string Convert(TimeOnly time, ClockNotationRounding roundToNearestFive, CultureInfo? culture)
    {
        switch (time)
        {
            case { Hour: 0, Minute: 0 }:
                return "meia-noite";
            case { Hour: 12, Minute: 0 }:
                return "meio-dia";
        }

        var normalizedHour = time.Hour % 12;
        var normalizedMinutes = (int)(roundToNearestFive == ClockNotationRounding.NearestFiveMinutes
            ? 5 * Math.Round(time.Minute / 5.0)
            : time.Minute);

        return normalizedMinutes switch
        {
            00 => $"{normalizedHour.ToWords(GrammaticalGender.Feminine, culture)} horas",
            15 => $"{normalizedHour.ToWords(GrammaticalGender.Feminine, culture)} e um quarto",
            30 => $"{normalizedHour.ToWords(GrammaticalGender.Feminine, culture)} e meia",
            40 => $"{(normalizedHour + 1).ToWords(GrammaticalGender.Feminine, culture)} menos vinte",
            45 => $"{(normalizedHour + 1).ToWords(GrammaticalGender.Feminine, culture)} menos um quarto",
            50 => $"{(normalizedHour + 1).ToWords(GrammaticalGender.Feminine, culture)} menos dez",
            55 => $"{(normalizedHour + 1).ToWords(GrammaticalGender.Feminine, culture)} menos cinco",
            60 => $"{(normalizedHour + 1).ToWords(GrammaticalGender.Feminine, culture)} horas",
            _ => $"{normalizedHour.ToWords(GrammaticalGender.Feminine, culture)} e {normalizedMinutes.ToWords(culture)}"
        };
    }
}

#endif

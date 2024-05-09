#if NET6_0_OR_GREATER

namespace Humanizer;

class PortugueseTimeOnlyToClockNotationConverter : ITimeOnlyToClockNotationConverter
{
    public string Convert(TimeOnly time, ClockNotationRounding roundToNearestFive)
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
            00 => $"{normalizedHour.ToWords(GrammaticalGender.Feminine)} horas",
            15 => $"{normalizedHour.ToWords(GrammaticalGender.Feminine)} e um quarto",
            30 => $"{normalizedHour.ToWords(GrammaticalGender.Feminine)} e meia",
            40 => $"{(normalizedHour + 1).ToWords(GrammaticalGender.Feminine)} menos vinte",
            45 => $"{(normalizedHour + 1).ToWords(GrammaticalGender.Feminine)} menos um quarto",
            50 => $"{(normalizedHour + 1).ToWords(GrammaticalGender.Feminine)} menos dez",
            55 => $"{(normalizedHour + 1).ToWords(GrammaticalGender.Feminine)} menos cinco",
            60 => $"{(normalizedHour + 1).ToWords(GrammaticalGender.Feminine)} horas",
            _ => $"{normalizedHour.ToWords(GrammaticalGender.Feminine)} e {normalizedMinutes.ToWords()}"
        };
    }
}

#endif

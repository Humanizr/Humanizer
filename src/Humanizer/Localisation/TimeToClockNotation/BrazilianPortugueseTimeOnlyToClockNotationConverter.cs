#if NET6_0_OR_GREATER

namespace Humanizer;

class BrazilianPortugueseTimeOnlyToClockNotationConverter : ITimeOnlyToClockNotationConverter
{
    static CultureInfo culture = new("pt-BR");
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
        var normalizedMinutes = NormalizedMinutes(time, roundToNearestFive);

        return normalizedMinutes switch
        {
            00 => $"{normalizedHour.ToWords(GrammaticalGender.Feminine, culture)} em ponto",
            30 => $"{normalizedHour.ToWords(GrammaticalGender.Feminine, culture)} e meia",
            40 => $"vinte para as {(normalizedHour + 1).ToWords(GrammaticalGender.Feminine, culture)}",
            45 => $"quinze para as {(normalizedHour + 1).ToWords(GrammaticalGender.Feminine, culture)}",
            50 => $"dez para as {(normalizedHour + 1).ToWords(GrammaticalGender.Feminine, culture)}",
            55 => $"cinco para as {(normalizedHour + 1).ToWords(GrammaticalGender.Feminine, culture)}",
            60 => $"{(normalizedHour + 1).ToWords(GrammaticalGender.Feminine, culture)} em ponto",
            _ => $"{normalizedHour.ToWords(GrammaticalGender.Feminine, culture)} e {normalizedMinutes.ToWords(culture)}"
        };
    }

    static int NormalizedMinutes(TimeOnly time, ClockNotationRounding roundToNearestFive)
    {
        if (roundToNearestFive == ClockNotationRounding.NearestFiveMinutes)
        {
            return (int) (5 * Math.Round(time.Minute / 5.0));
        }

        return time.Minute;
    }
}

#endif

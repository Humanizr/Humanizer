#if NET6_0_OR_GREATER

namespace Humanizer;

class BrazilianPortugueseTimeOnlyToClockNotationConverter : ITimeOnlyToClockNotationConverter
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
        var displayHour = normalizedHour == 0 ? 12 : normalizedHour;
        var normalizedMinutes = (int)(roundToNearestFive == ClockNotationRounding.NearestFiveMinutes
            ? 5 * Math.Round(time.Minute / 5.0)
            : time.Minute);
        var nextHour = (displayHour % 12) + 1;
        var nextHourText = nextHour.ToWords(GrammaticalGender.Feminine);
        var nextHourArticle = nextHour == 1 ? "a" : "as";

        return normalizedMinutes switch
        {
            00 => $"{displayHour.ToWords(GrammaticalGender.Feminine)} em ponto",
            30 => $"{displayHour.ToWords(GrammaticalGender.Feminine)} e meia",
            40 => $"vinte para {nextHourArticle} {nextHourText}",
            45 => $"quinze para {nextHourArticle} {nextHourText}",
            50 => $"dez para {nextHourArticle} {nextHourText}",
            55 => $"cinco para {nextHourArticle} {nextHourText}",
            60 => $"{nextHourText} em ponto",
            _ => $"{displayHour.ToWords(GrammaticalGender.Feminine)} e {normalizedMinutes.ToWords()}"
        };
    }
}

#endif

#if NET6_0_OR_GREATER

namespace Humanizer;

class FrTimeOnlyToClockNotationConverter : ITimeOnlyToClockNotationConverter
{
    public string Convert(TimeOnly time, ClockNotationRounding roundToNearestFive, CultureInfo? culture)
    {
        var normalizedMinutes = (int)(roundToNearestFive == ClockNotationRounding.NearestFiveMinutes
            ? 5 * Math.Round(time.Minute / 5.0)
            : time.Minute);

        return normalizedMinutes switch
        {
            00 => GetHourExpression(time.Hour, culture),
            60 => GetHourExpression(time.Hour + 1, culture),
            _ => $"{GetHourExpression(time.Hour, culture)} {normalizedMinutes.ToWords(GrammaticalGender.Feminine, culture)}"
        };

        static string GetHourExpression(int hour, CultureInfo? culture) =>
            hour switch
            {
                0 => "minuit",
                12 => "midi",
                _ => hour.ToWords(GrammaticalGender.Feminine, culture) + (hour > 1 ? " heures" : " heure")
            };
    }
}

#endif

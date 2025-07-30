#if NET6_0_OR_GREATER

namespace Humanizer;

class FrTimeOnlyToClockNotationConverter : ITimeOnlyToClockNotationConverter
{
    public string Convert(TimeOnly time, ClockNotationRounding roundToNearestFive)
    {
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
                _ => hour.ToWords(GrammaticalGender.Feminine) + (hour > 1 ? " heures" : " heure")
            };
    }
}

#endif

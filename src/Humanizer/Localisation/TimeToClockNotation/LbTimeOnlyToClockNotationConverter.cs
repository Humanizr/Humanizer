#if NET6_0_OR_GREATER

namespace Humanizer;

class LbTimeOnlyToClockNotationConverter : ITimeOnlyToClockNotationConverter
{
    public string Convert(TimeOnly time, ClockNotationRounding roundToNearestFive, CultureInfo? culture)
    {
        var roundedTime = roundToNearestFive is ClockNotationRounding.NearestFiveMinutes
            ? GetRoundedTime(time)
            : time;

        return roundedTime switch
        {
            { Hour: 0, Minute: 0 } => "Mëtternuecht",
            { Hour: 12, Minute: 0 } => "Mëtteg",
            _ => roundedTime.Minute switch
            {
                00 => GetHourExpression(culture, roundedTime.Hour, "Auer"),
                15 => $"Véirel op {GetHourExpression(culture, roundedTime.Hour)}",
                25 => $"{GetMinuteExpression(culture, 30 - roundedTime.Minute, "vir")} hallwer {GetHourExpression(culture, roundedTime.Hour + 1)}",
                30 => $"hallwer {GetHourExpression(culture, roundedTime.Hour + 1)}",
                35 => $"{GetMinuteExpression(culture, roundedTime.Minute - 30, "op")} hallwer {GetHourExpression(culture, roundedTime.Hour + 1)}",
                45 => $"Véirel vir {GetHourExpression(culture, roundedTime.Hour + 1)}",
                60 => GetHourExpression(culture, roundedTime.Hour + 1, "Auer"),
                01 => $"{GetMinuteExpression(culture, roundedTime.Minute, "Minutt")} op {GetHourExpression(culture, roundedTime.Hour)}",
                59 => $"{GetMinuteExpression(culture, 60 - roundedTime.Minute, "Minutt")} vir {GetHourExpression(culture, roundedTime.Hour + 1)}",
                05 or 10 or 20 => $"{GetMinuteExpression(culture, roundedTime.Minute, "op")} {GetHourExpression(culture, roundedTime.Hour)}",
                40 or 50 or 55 => $"{GetMinuteExpression(culture, 60 - roundedTime.Minute, "vir")} {GetHourExpression(culture, roundedTime.Hour + 1)}",
                > 00 and < 25 => $"{GetMinuteExpression(culture, roundedTime.Minute, "Minutten")} op {GetHourExpression(culture, roundedTime.Hour)}",
                > 25 and < 30 => $"{GetMinuteExpression(culture, 30 - roundedTime.Minute, "Minutten")} vir hallwer {GetHourExpression(culture, roundedTime.Hour + 1)}",
                > 30 and < 35 => $"{GetMinuteExpression(culture, roundedTime.Minute - 30, "Minutten")} op hallwer {GetHourExpression(culture, roundedTime.Hour + 1)}",
                > 35 and < 60 => $"{GetMinuteExpression(culture, 60 - roundedTime.Minute, "Minutten")} vir {GetHourExpression(culture, roundedTime.Hour + 1)}",
                _ => $"{GetHourExpression(culture, time.Hour, "Auer")} {GetMinuteExpression(culture, time.Minute)}"
            }
        };
    }

    private static TimeOnly GetRoundedTime(TimeOnly time)
    {
        var tempRoundedMinutes = (int)(5 * Math.Round(time.Minute / 5.0));
        var roundedHours = tempRoundedMinutes == 60 ? time.Hour + 1 : time.Hour;
        var roundedMinutes = tempRoundedMinutes == 60 ? 0 : tempRoundedMinutes;
        return new(roundedHours, roundedMinutes);
    }

    private static string GetMinuteExpression(CultureInfo? culture, int minute, string nextWord = "")
        => GetFormattedExpression(culture, minute, nextWord);

    private static string GetHourExpression(CultureInfo? culture, int hour, string nextWord = "")
    {
        var normalizedHour = hour % 12;
        var hourExpression = normalizedHour == 0 ? 12 : normalizedHour;

        return GetFormattedExpression(culture, hourExpression, nextWord);
    }

    private static string GetFormattedExpression(CultureInfo? culture, int number, string nextWord) =>
        (number switch
        {
            1 or 2 => $"{number.ToWords(GrammaticalGender.Feminine, culture)} {nextWord}",
            7 => $"{number.ToWords(LuxembourgishFormatter.DoesEifelerRuleApply(nextWord) ? WordForm.Eifeler : WordForm.Normal, culture)} {nextWord}",
            _ => $"{number.ToWords(culture)} {nextWord}"
        }).TrimEnd();
}

#endif

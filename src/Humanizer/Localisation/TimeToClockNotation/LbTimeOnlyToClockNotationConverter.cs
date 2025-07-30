#if NET6_0_OR_GREATER

namespace Humanizer;

class LbTimeOnlyToClockNotationConverter : ITimeOnlyToClockNotationConverter
{
    public string Convert(TimeOnly time, ClockNotationRounding roundToNearestFive)
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
                00 => GetHourExpression(roundedTime.Hour, "Auer"),
                15 => $"Véirel op {GetHourExpression(roundedTime.Hour)}",
                25 => $"{GetMinuteExpression(30 - roundedTime.Minute, "vir")} hallwer {GetHourExpression(roundedTime.Hour + 1)}",
                30 => $"hallwer {GetHourExpression(roundedTime.Hour + 1)}",
                35 => $"{GetMinuteExpression(roundedTime.Minute - 30, "op")} hallwer {GetHourExpression(roundedTime.Hour + 1)}",
                45 => $"Véirel vir {GetHourExpression(roundedTime.Hour + 1)}",
                60 => GetHourExpression(roundedTime.Hour + 1, "Auer"),
                01 => $"{GetMinuteExpression(roundedTime.Minute, "Minutt")} op {GetHourExpression(roundedTime.Hour)}",
                59 => $"{GetMinuteExpression(60 - roundedTime.Minute, "Minutt")} vir {GetHourExpression(roundedTime.Hour + 1)}",
                05 or 10 or 20 => $"{GetMinuteExpression(roundedTime.Minute, "op")} {GetHourExpression(roundedTime.Hour)}",
                40 or 50 or 55 => $"{GetMinuteExpression(60 - roundedTime.Minute, "vir")} {GetHourExpression(roundedTime.Hour + 1)}",
                > 00 and < 25 => $"{GetMinuteExpression(roundedTime.Minute, "Minutten")} op {GetHourExpression(roundedTime.Hour)}",
                > 25 and < 30 => $"{GetMinuteExpression(30 - roundedTime.Minute, "Minutten")} vir hallwer {GetHourExpression(roundedTime.Hour + 1)}",
                > 30 and < 35 => $"{GetMinuteExpression(roundedTime.Minute - 30, "Minutten")} op hallwer {GetHourExpression(roundedTime.Hour + 1)}",
                > 35 and < 60 => $"{GetMinuteExpression(60 - roundedTime.Minute, "Minutten")} vir {GetHourExpression(roundedTime.Hour + 1)}",
                _ => $"{GetHourExpression(time.Hour, "Auer")} {GetMinuteExpression(time.Minute)}"
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

    private static string GetMinuteExpression(int minute, string nextWord = "")
        => GetFormattedExpression(minute, nextWord);

    private static string GetHourExpression(int hour, string nextWord = "")
    {
        var normalizedHour = hour % 12;
        var hourExpression = normalizedHour == 0 ? 12 : normalizedHour;

        return GetFormattedExpression(hourExpression, nextWord);
    }

    private static string GetFormattedExpression(int number, string nextWord) =>
        (number switch
        {
            1 or 2 => $"{number.ToWords(GrammaticalGender.Feminine)} {nextWord}",
            7 => $"{number.ToWords(LuxembourgishFormatter.DoesEifelerRuleApply(nextWord) ? WordForm.Eifeler : WordForm.Normal)} {nextWord}",
            _ => $"{number.ToWords()} {nextWord}"
        }).TrimEnd();
}

#endif

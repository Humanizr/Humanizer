#if NET6_0_OR_GREATER

namespace Humanizer;

class LbTimeOnlyToClockNotationConverter : ITimeOnlyToClockNotationConverter
{
    static string tenMinutes = 10.ToWords();
    static        string oneMinute = 1.ToWords(GrammaticalGender.Feminine);
    static        string fiveMinutes = 5.ToWords();

    public string Convert(TimeOnly time, ClockNotationRounding roundToNearestFive)
    {
        var rounded = roundToNearestFive is ClockNotationRounding.NearestFiveMinutes
            ? GetRoundedTime(time)
            : time;

        if (rounded is {Hour: 0, Minute: 0})
        {
            return "Mëtternuecht";
        }

        if (rounded is {Hour: 12, Minute: 0})
        {
            return "Mëtteg";
        }

        return rounded.Minute switch
        {
            00 => Hours(rounded.Hour, "Auer"),
            15 => $"Véirel op {Hours(rounded.Hour)}",
            25 => $"{Minutes(5, "vir")} hallwer {Hours(rounded.Hour + 1)}",
            30 => $"hallwer {Hours(rounded.Hour + 1)}",
            35 => $"{Minutes(5, "op")} hallwer {Hours(rounded.Hour + 1)}",
            45 => $"Véirel vir {Hours(rounded.Hour + 1)}",
            60 => Hours(rounded.Hour + 1, "Auer"),
            01 => $"{oneMinute} Minutt op {Hours(rounded.Hour)}",
            59 => $"{oneMinute} Minutt vir {Hours(rounded.Hour + 1)}",
            05 => $"{Minutes(5, "op")} {Hours(rounded.Hour)}",
            10 => $"{tenMinutes} op {Hours(rounded.Hour)}",
            20 => $"{Minutes(20, "op")} {Hours(rounded.Hour)}",
            40 => $"{Minutes(20, "vir")} {Hours(rounded.Hour + 1)}",
            50 => $"{tenMinutes} vir {Hours(rounded.Hour + 1)}",
            55 => $"{Minutes(5, "vir")} {Hours(rounded.Hour + 1)}",
            > 00 and < 25 => $"{Minutes(rounded.Minute, "Minutten")} op {Hours(rounded.Hour)}",
            > 25 and < 30 => $"{Minutes(30 - rounded.Minute, "Minutten")} vir hallwer {Hours(rounded.Hour + 1)}",
            > 30 and < 35 => $"{Minutes(rounded.Minute - 30, "Minutten")} op hallwer {Hours(rounded.Hour + 1)}",
            > 35 and < 60 => $"{Minutes(60 - rounded.Minute, "Minutten")} vir {Hours(rounded.Hour + 1)}",
            _ => $"{Hours(time.Hour, "Auer")} {Minutes(time.Minute)}"
        };
    }

    private static TimeOnly GetRoundedTime(TimeOnly time)
    {
        var tempMinutes = (int) (5 * Math.Round(time.Minute / 5.0));
        var hours = tempMinutes == 60 ? time.Hour + 1 : time.Hour;
        var minutes = tempMinutes == 60 ? 0 : tempMinutes;
        return new(hours, minutes);
    }

    static string Minutes(int minute)
        => GetFormattedExpression(minute);

    static string Minutes(int minute, string nextWord)
        => GetFormattedExpression(minute, nextWord);

    static string FiveMinutes(string nextWord)
    {
        return $"{fiveMinutes} {nextWord}";
    }

    static string Hours(int hour)
    {
        var hourExpression = HourExpression(hour);

        return GetFormattedExpression(hourExpression);
    }

    static string Hours(int hour, string nextWord)
    {
        var hourExpression = HourExpression(hour);

        return GetFormattedExpression(hourExpression, nextWord);
    }

    static int HourExpression(int hour)
    {
        var normalizedHour = hour % 12;
        return normalizedHour == 0 ? 12 : normalizedHour;
    }

    private static string GetFormattedExpression(int number)
    {
        if (number is 1 or 2)
        {
            return number.ToWords(GrammaticalGender.Feminine);
        }

        if (number == 7)
        {
            return number.ToWords(WordForm.Normal);
        }

        return number.ToWords();
    }

    private static string GetFormattedExpression(int number, string nextWord)
    {
        if (number is 1 or 2)
        {
            return $"{number.ToWords(GrammaticalGender.Feminine)} {nextWord}";
        }

        if (number == 7)
        {
            var wordForm = LuxembourgishFormatter.DoesEifelerRuleApply(nextWord) ? WordForm.Eifeler : WordForm.Normal;
            return $"{number.ToWords(wordForm)} {nextWord}";
        }

        return $"{number.ToWords()} {nextWord}";
    }
}

#endif

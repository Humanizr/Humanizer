#if NET6_0_OR_GREATER

namespace Humanizer;

class LbTimeOnlyToClockNotationConverter : ITimeOnlyToClockNotationConverter
{
    public string Convert(TimeOnly time, ClockNotationRounding roundToNearestFive)
    {
        var rounded = roundToNearestFive is ClockNotationRounding.NearestFiveMinutes
            ? GetRoundedTime(time)
            : time;

        var minute = rounded.Minute;
        var hour = rounded.Hour;
        if (hour == 0 &&  minute == 0)
        {
            return "Mëtternuecht";
        }

        if (hour == 12 &&  minute == 0)
        {
            return "Mëtteg";
        }

        return minute switch
        {
            00 => Hours(hour, "Auer"),
            15 => $"Véirel op {Hours(hour)}",
            25 => $"{Minutes(30 - minute, "vir")} hallwer {Hours(hour + 1)}",
            30 => $"hallwer {Hours(hour + 1)}",
            35 => $"{Minutes(minute - 30, "op")} hallwer {Hours(hour + 1)}",
            45 => $"Véirel vir {Hours(hour + 1)}",
            60 => Hours(hour + 1, "Auer"),
            01 => $"{Minutes(minute, "Minutt")} op {Hours(hour)}",
            59 => $"{Minutes(60 - minute, "Minutt")} vir {Hours(hour + 1)}",
            05 or 10 or 20 => $"{Minutes(minute, "op")} {Hours(hour)}",
            40 or 50 or 55 => $"{Minutes(60 - minute, "vir")} {Hours(hour + 1)}",
            > 00 and < 25 => $"{Minutes(minute, "Minutten")} op {Hours(hour)}",
            > 25 and < 30 => $"{Minutes(30 - minute, "Minutten")} vir hallwer {Hours(hour + 1)}",
            > 30 and < 35 => $"{Minutes(minute - 30, "Minutten")} op hallwer {Hours(hour + 1)}",
            > 35 and < 60 => $"{Minutes(60 - minute, "Minutten")} vir {Hours(hour + 1)}",
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

    static string Minutes(int minute, string? nextWord = null)
        => GetFormattedExpression(minute, nextWord);

    static string Hours(int hour, string? nextWord = null)
    {
        var normalizedHour = hour % 12;
        var hourExpression = normalizedHour == 0 ? 12 : normalizedHour;

        return GetFormattedExpression(hourExpression, nextWord);
    }

    private static string GetFormattedExpression(int number, string? nextWord)
    {
        if (nextWord == null)
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

        if (number is 1 or 2)
        {
            return $"{number.ToWords(GrammaticalGender.Feminine)} {nextWord}";
        }

        if (number == 7)
        {
            var eifelerApplies = LuxembourgishFormatter.DoesEifelerRuleApply(nextWord);
            return $"{number.ToWords(eifelerApplies ? WordForm.Eifeler : WordForm.Normal)} {nextWord}";
        }

        return $"{number.ToWords()} {nextWord}";
    }
}

#endif

#if NET6_0_OR_GREATER

namespace Humanizer;

class LbTimeOnlyToClockNotationConverter : ITimeOnlyToClockNotationConverter
{
    static string tenMinutes = 10.ToWords(culture);
    static string oneMinute = 1.ToWords(GrammaticalGender.Feminine, culture);
    static string fiveMinutes = 5.ToWords(culture);
    string twentyMinutes = 20.ToWords(culture);
    static CultureInfo culture = new("lb-LU");
    public string Convert(TimeOnly time, ClockNotationRounding roundToNearestFive)
    {
        if (roundToNearestFive is ClockNotationRounding.NearestFiveMinutes)
        {
            return HandlerRounded(time);
        }

        if (HandleOnHour(time, out var value))
        {
            return value;
        }

        return time.Minute switch
        {
            00 => Hours(time.Hour, "Auer"),
            01 => $"{oneMinute} Minutt op {Hours(time.Hour)}",
            05 => $"{fiveMinutes} op {Hours(time.Hour)}",
            10 => $"{tenMinutes} op {Hours(time.Hour)}",
            15 => $"Véirel op {Hours(time.Hour)}",
            20 => $"{twentyMinutes} op {Hours(time.Hour)}",
            25 => $"{fiveMinutes} vir hallwer {Hours(time.Hour + 1)}",
            30 => $"hallwer {Hours(time.Hour + 1)}",
            35 => $"{fiveMinutes} op hallwer {Hours(time.Hour + 1)}",
            40 => $"{twentyMinutes} vir {Hours(time.Hour + 1)}",
            45 => $"Véirel vir {Hours(time.Hour + 1)}",
            50 => $"{tenMinutes} vir {Hours(time.Hour + 1)}",
            55 => $"{fiveMinutes} vir {Hours(time.Hour + 1)}",
            59 => $"{oneMinute} Minutt vir {Hours(time.Hour + 1)}",
            60 => Hours(time.Hour + 1, "Auer"),
            > 00 and < 25 => $"{Minutes(time.Minute, "Minutten")} op {Hours(time.Hour)}",
            > 25 and < 30 => $"{Minutes(30 - time.Minute, "Minutten")} vir hallwer {Hours(time.Hour + 1)}",
            > 30 and < 35 => $"{Minutes(time.Minute - 30, "Minutten")} op hallwer {Hours(time.Hour + 1)}",
            _ => $"{Minutes(60 - time.Minute, "Minutten")} vir {Hours(time.Hour + 1)}",
        };
    }

    static bool HandleOnHour(TimeOnly time, [NotNullWhen(true)] out string? result)
    {
        if (time is {Hour: 0, Minute: 0})
        {
            result = "Mëtternuecht";
            return true;
        }

        if (time is {Hour: 12, Minute: 0})
        {
            result = "Mëtteg";
            return true;
        }

        result = null;
        return false;
    }

    string HandlerRounded(TimeOnly time)
    {
        var rounded = GetRoundedTime(time);

        if (HandleOnHour(time, out var value))
        {
            return value;
        }

        return rounded.Minute switch
        {
            00 => Hours(rounded.Hour, "Auer"),
            05 => $"{fiveMinutes} op {Hours(rounded.Hour)}",
            10 => $"{tenMinutes} op {Hours(rounded.Hour)}",
            15 => $"Véirel op {Hours(rounded.Hour)}",
            20 => $"{twentyMinutes} op {Hours(rounded.Hour)}",
            25 => $"{fiveMinutes} vir hallwer {Hours(rounded.Hour + 1)}",
            30 => $"hallwer {Hours(rounded.Hour + 1)}",
            35 => $"{fiveMinutes} op hallwer {Hours(rounded.Hour + 1)}",
            40 => $"{twentyMinutes} vir {Hours(rounded.Hour + 1)}",
            45 => $"Véirel vir {Hours(rounded.Hour + 1)}",
            50 => $"{tenMinutes} vir {Hours(rounded.Hour + 1)}",
            55 => $"{fiveMinutes} vir {Hours(rounded.Hour + 1)}",
            _ => Hours(rounded.Hour + 1, "Auer"),
        };
    }

    static TimeOnly GetRoundedTime(TimeOnly time)
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

    static string GetFormattedExpression(int number)
    {
        if (number is 1 or 2)
        {
            return number.ToWords(GrammaticalGender.Feminine, culture);
        }

        if (number == 7)
        {
            return number.ToWords(WordForm.Normal, culture);
        }

        return number.ToWords(culture);
    }

    static string GetFormattedExpression(int number, string nextWord)
    {
        if (number is 1 or 2)
        {
            return $"{number.ToWords(GrammaticalGender.Feminine, culture)} {nextWord}";
        }

        if (number == 7)
        {
            var wordForm = LuxembourgishFormatter.DoesEifelerRuleApply(nextWord) ? WordForm.Eifeler : WordForm.Normal;
            return $"{number.ToWords(wordForm, culture)} {nextWord}";
        }

        return $"{number.ToWords(culture)} {nextWord}";
    }
}

#endif

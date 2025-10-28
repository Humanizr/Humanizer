#if NET6_0_OR_GREATER

namespace Humanizer;

class CaTimeOnlyToClockNotationConverter : ITimeOnlyToClockNotationConverter
{
    const int MORNING = 6;
    const int NOON = 12;
    const int AFTERNOON = 21;

    public string Convert(TimeOnly time, ClockNotationRounding roundToNearestFive)
    {
        switch (time)
        {
            case { Hour: 0, Minute: 0 }:
                return "mitjanit";

            case { Hour: 12, Minute: 0 }:
                return "migdia";
        }

        var article = GetArticle(time);
        var articleNextHour = GetArticle(time.AddHours(1));
        var hour = NormalizeHour(time).ToWords(GrammaticalGender.Feminine);
        var nextHour = NormalizeHour(time.AddHours(1)).ToWords(GrammaticalGender.Feminine);
        var dayPeriod = GetDayPeriod(time);
        var dayPeriodNextHour = GetDayPeriod(time.AddHours(1));

        var normalizedMinutes = (int)(roundToNearestFive == ClockNotationRounding.NearestFiveMinutes
            ? 5 * Math.Round(time.Minute / 5.0)
            : time.Minute);

        return normalizedMinutes switch
        {
            0 => $"{article} {hour} {dayPeriod}",
            15 => $"{article} {hour} i quart {dayPeriod}",
            30 => $"{article} {hour} i mitja {dayPeriod}",
            35 => $"{articleNextHour} {nextHour} menys vint-i-cinc {dayPeriodNextHour}",
            40 => $"{articleNextHour} {nextHour} menys vint {dayPeriodNextHour}",
            45 => $"{articleNextHour} {nextHour} menys quart {dayPeriodNextHour}",
            50 => $"{articleNextHour} {nextHour} menys deu {dayPeriodNextHour}",
            55 => $"{articleNextHour} {nextHour} menys cinc {dayPeriodNextHour}",
            60 => $"{articleNextHour} {nextHour} {dayPeriodNextHour}",
            _ => $"{article} {hour} i {normalizedMinutes.ToWords()} {dayPeriod}"
        };
    }

    static int NormalizeHour(TimeOnly time) =>
        time.Hour % 12 != 0 ? time.Hour % 12 : 12;

    static string GetArticle(TimeOnly time) =>
        time.Hour is 1 or 13 ? "la" : "les";

    static string GetDayPeriod(TimeOnly time)
    {
        if (IsEarlyMorning(time))
        {
            return "de la matinada";
        }

        if (IsMorning(time))
        {
            return "del matí";
        }

        if (IsAfternoon(time))
        {
            return "de la tarda";
        }

        return "de la nit";
    }

    static bool IsEarlyMorning(TimeOnly time) =>
        time.Hour is >= 1 and < MORNING;

    static bool IsMorning(TimeOnly time) =>
        time.Hour is >= MORNING and < NOON;

    static bool IsAfternoon(TimeOnly time) =>
        time.Hour is >= NOON and < AFTERNOON;
}

#endif
#if NET6_0_OR_GREATER

namespace Humanizer;

class EsTimeOnlyToClockNotationConverter : ITimeOnlyToClockNotationConverter
{
    const int MORNING = 6;
    const int NOON = 12;
    const int AFTERNOON = 21;

    public string Convert(TimeOnly time, ClockNotationRounding roundToNearestFive)
    {
        switch (time)
        {
            case { Hour: 0, Minute: 0 }:
                return "medianoche";

            case { Hour: 12, Minute: 0 }:
                return "mediodía";
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
            15 => $"{article} {hour} y cuarto {dayPeriod}",
            30 => $"{article} {hour} y media {dayPeriod}",
            35 => $"{articleNextHour} {nextHour} menos veinticinco {dayPeriodNextHour}",
            40 => $"{articleNextHour} {nextHour} menos veinte {dayPeriodNextHour}",
            45 => $"{articleNextHour} {nextHour} menos cuarto {dayPeriodNextHour}",
            50 => $"{articleNextHour} {nextHour} menos diez {dayPeriodNextHour}",
            55 => $"{articleNextHour} {nextHour} menos cinco {dayPeriodNextHour}",
            60 => $"{articleNextHour} {nextHour} {dayPeriodNextHour}",
            _ => $"{article} {hour} y {normalizedMinutes.ToWords()} {dayPeriod}"
        };
    }

    static int NormalizeHour(TimeOnly time) =>
        time.Hour % 12 != 0 ? time.Hour % 12 : 12;

    static string GetArticle(TimeOnly time) =>
        time.Hour is 1 or 13 ? "la" : "las";

    static string GetDayPeriod(TimeOnly time)
    {
        if (IsEarlyMorning(time))
        {
            return "de la madrugada";
        }

        if (IsMorning(time))
        {
            return "de la mañana";
        }

        if (IsAfternoon(time))
        {
            return "de la tarde";
        }

        return "de la noche";
    }

    static bool IsEarlyMorning(TimeOnly time) =>
        time.Hour is >= 1 and < MORNING;

    static bool IsMorning(TimeOnly time) =>
        time.Hour is >= MORNING and < NOON;

    static bool IsAfternoon(TimeOnly time) =>
        time.Hour is >= NOON and < AFTERNOON;
}

#endif

#if NET6_0_OR_GREATER

namespace Humanizer;

class EsTimeOnlyToClockNotationConverter : ITimeOnlyToClockNotationConverter
{
    public string Convert(TimeOnly time, ClockNotationRounding roundToNearestFive)
    {
        switch (time)
        {
            case {Hour: 0, Minute: 0}:
                return "medianoche";

            case {Hour: 12, Minute: 0}:
                return "mediodía";
        }

        var article = GetArticle(time);

        var normalizedMinutes = NormalizedMinutes(time, roundToNearestFive);

        if (normalizedMinutes == 0)
        {
            return $"{article} {GetHour(time)} {GetDayPeriod(time)}";
        }

        if (normalizedMinutes == 15)
        {
            return $"{article} {GetHour(time)} y cuarto {GetDayPeriod(time)}";
        }

        if (normalizedMinutes == 30)
        {
            return $"{article} {GetHour(time)} y media {GetDayPeriod(time)}";
        }

        var oneHourForward = time.AddHours(1);
        if (normalizedMinutes == 35)
        {
            return $"{GetArticle(oneHourForward)} {GetNextHour(oneHourForward)} menos veinticinco {GetDayPeriod(oneHourForward)}";
        }

        if (normalizedMinutes == 40)
        {
            return $"{GetArticle(oneHourForward)} {GetNextHour(oneHourForward)} menos veinte {GetDayPeriod(oneHourForward)}";
        }

        if (normalizedMinutes == 45)
        {
            return $"{GetArticle(oneHourForward)} {GetNextHour(oneHourForward)} menos cuarto {GetDayPeriod(oneHourForward)}";
        }

        if (normalizedMinutes == 50)
        {
            return $"{GetArticle(oneHourForward)} {GetNextHour(oneHourForward)} menos diez {GetDayPeriod(oneHourForward)}";
        }

        if (normalizedMinutes == 55)
        {
            return $"{GetArticle(oneHourForward)} {GetNextHour(oneHourForward)} menos cinco {GetDayPeriod(oneHourForward)}";
        }

        if (normalizedMinutes == 60)
        {
            return $"{GetArticle(oneHourForward)} {GetNextHour(oneHourForward)} {GetDayPeriod(oneHourForward)}";
        }

        return $"{article} {GetHour(time)} y {normalizedMinutes.ToWords()} {GetDayPeriod(time)}";
    }

    static int NormalizedMinutes(TimeOnly time, ClockNotationRounding rounding)
    {
        if (rounding == ClockNotationRounding.NearestFiveMinutes)
        {
            return (int) (5 * Math.Round(time.Minute / 5.0));
        }

        return time.Minute;
    }

    static string GetNextHour(TimeOnly oneHourForward) =>
        NormalizeHour(oneHourForward)
            .ToWords(GrammaticalGender.Feminine);

    static string GetHour(TimeOnly time) =>
        NormalizeHour(time)
            .ToWords(GrammaticalGender.Feminine);

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

    const int MORNING = 6;
    const int NOON = 12;
    const int AFTERNOON = 21;

    static bool IsEarlyMorning(TimeOnly time) =>
        time.Hour is >= 1 and < MORNING;

    static bool IsMorning(TimeOnly time) =>
        time.Hour is >= MORNING and < NOON;

    static bool IsAfternoon(TimeOnly time) =>
        time.Hour is >= NOON and < AFTERNOON;
}

#endif

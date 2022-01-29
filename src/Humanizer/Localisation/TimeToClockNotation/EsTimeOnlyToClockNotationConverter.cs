#if NET6_0_OR_GREATER

using System;
using System.Collections.Generic;

namespace Humanizer.Localisation.TimeToClockNotation
{
    internal class EsTimeOnlyToClockNotationConverter : ITimeOnlyToClockNotationConverter
    {
        private const int MORNING = 6;
        private const int NOON = 12;
        private const int AFTERNOON = 21;

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

            var clockNotationMap = new Dictionary<int, string>()
            {
                { 0, $"{article} {hour} {dayPeriod}" },
                { 15 , $"{article} {hour} y cuarto {dayPeriod}" },
                { 30 , $"{article} {hour} y media {dayPeriod}"},
                { 35 , $"{articleNextHour} {nextHour} menos veinticinco {dayPeriodNextHour}"},
                { 40 , $"{articleNextHour} {nextHour} menos veinte {dayPeriodNextHour}"},
                { 45 , $"{articleNextHour} {nextHour} menos cuarto {dayPeriodNextHour}"},
                { 50 , $"{articleNextHour} {nextHour} menos diez {dayPeriodNextHour}"},
                { 55 , $"{articleNextHour} {nextHour} menos cinco {dayPeriodNextHour}"},
                { 60 , $"{articleNextHour} {nextHour} {dayPeriodNextHour}"},
            };

            return clockNotationMap.GetValueOrDefault(
                normalizedMinutes,
                $"{article} {hour} y {normalizedMinutes.ToWords()} {dayPeriod}");
        }

        private static int NormalizeHour(TimeOnly time)
        {
            return time.Hour % 12 != 0 ? (time.Hour % 12) : 12;
        }

        private static string GetArticle(TimeOnly time)
        {
            return (time.Hour == 1 || time.Hour == 13) ? "la" : "las";
        }

        private static string GetDayPeriod(TimeOnly time)
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

        private static bool IsEarlyMorning(TimeOnly time)
        {
            return time.Hour >= 1 && time.Hour < MORNING;
        }

        private static bool IsMorning(TimeOnly time)
        {
            return time.Hour >= MORNING && time.Hour < NOON;
        }

        private static bool IsAfternoon(TimeOnly time)
        {
            return time.Hour >= NOON && time.Hour < AFTERNOON;
        }
    }
}

#endif

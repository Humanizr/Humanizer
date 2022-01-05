#if NET6_0_OR_GREATER

using System;

namespace Humanizer.Localisation.TimeToClockNotation
{
    internal class EsTimeOnlyToClockNotationConverter : ITimeOnlyToClockNotationConverter
    {
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
                00 => $"{article} {hour} {dayPeriod}",
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
            const int MORNING = 6;
            const int NOON = 12;
            const int AFTERNOON = 21;

            return time.Hour switch
            {
                int h when h is 0 => "de la noche",
                int h when h is >= 1 and < MORNING => "de la madrugada",
                int h when h is >= MORNING and < NOON => "de la mañana",
                int h when h is >= NOON and < AFTERNOON => "de la tarde",
                int h when h is >= AFTERNOON and <= 24 => "de la noche",
                _ => ""
            };
        }
    }
}

#endif

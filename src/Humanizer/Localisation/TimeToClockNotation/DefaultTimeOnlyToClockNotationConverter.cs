#if NET6_0_OR_GREATER

using System;

using Humanizer;

namespace Humanizer.Localisation.TimeToClockNotation
{
    internal class DefaultTimeOnlyToClockNotationConverter : ITimeOnlyToClockNotationConverter
    {
        public virtual string Convert(TimeOnly time, ClockNotationRounding roundToNearestFive)
        {
            switch (time)
            {
                case { Hour: 0, Minute: 0 }:
                    return "midnight";
                case { Hour: 12, Minute: 0 }:
                    return "noon";
            }

            var normalizedHour = time.Hour % 12;
            var normalizedMinutes = (int)(roundToNearestFive == ClockNotationRounding.NearestFiveMinutes
                ? 5 * Math.Round(time.Minute / 5.0)
                : time.Minute);

            return normalizedMinutes switch
            {
                00 => $"{normalizedHour.ToWords()} o'clock",
                05 => $"five past {normalizedHour.ToWords()}",
                10 => $"ten past {normalizedHour.ToWords()}",
                15 => $"a quarter past {normalizedHour.ToWords()}",
                20 => $"twenty past {normalizedHour.ToWords()}",
                25 => $"twenty-five past {normalizedHour.ToWords()}",
                30 => $"half past {normalizedHour.ToWords()}",
                40 => $"twenty to {(normalizedHour + 1).ToWords()}",
                45 => $"a quarter to {(normalizedHour + 1).ToWords()}",
                50 => $"ten to {(normalizedHour + 1).ToWords()}",
                55 => $"five to {(normalizedHour + 1).ToWords()}",
                60 => $"{(normalizedHour + 1).ToWords()} o'clock",
                _ => $"{normalizedHour.ToWords()} {normalizedMinutes.ToWords()}"
            };
        }
    }
}

#endif

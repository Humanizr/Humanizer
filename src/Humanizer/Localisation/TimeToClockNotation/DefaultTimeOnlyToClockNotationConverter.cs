#if NET6_0_OR_GREATER

using System;

namespace Humanizer.Localisation.TimeToClockNotation
{
    public class DefaultTimeOnlyToClockNotationConverter : ITimeOnlyToClockNotationConverter
    {
        public virtual string Convert(TimeOnly time)
        {
            switch (time)
            {
                case { Hour: 0, Minute: 0 }:
                    return "midnight";
                case { Hour: 12, Minute: 0 }:
                    return "noon";
            }

            var normalizedHour = time.Hour % 12;

            return time switch
            {
                { Minute: 00 } => $"{normalizedHour.ToWords()} o'clock",
                { Minute: 05 } => $"five past {normalizedHour.ToWords()}",
                { Minute: 10 } => $"ten past {normalizedHour.ToWords()}",
                { Minute: 15 } => $"a quarter past {normalizedHour.ToWords()}",
                { Minute: 20 } => $"twenty past {normalizedHour.ToWords()}",
                { Minute: 25 } => $"twenty-five past {normalizedHour.ToWords()}",
                { Minute: 30 } => $"half past {normalizedHour.ToWords()}",
                { Minute: 40 } => $"twenty to {(normalizedHour + 1).ToWords()}",
                { Minute: 45 } => $"a quarter to {(normalizedHour + 1).ToWords()}",
                { Minute: 50 } => $"ten to {(normalizedHour + 1).ToWords()}",
                { Minute: 55 } => $"five to {(normalizedHour + 1).ToWords()}",
                _ => $"{normalizedHour.ToWords()} {time.Minute.ToWords()}"
            };
        }
    }
}

#endif

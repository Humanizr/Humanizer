#if NET6_0_OR_GREATER

using System;

namespace Humanizer.Localisation.TimeToClockNotation
{
    public class BrazilianPortugueseTimeOnlyToClockNotationConverter : ITimeOnlyToClockNotationConverter
    {
        public virtual string Convert(TimeOnly time)
        {
            switch (time)
            {
                case { Hour: 0, Minute: 0 }:
                    return "meia-noite";
                case { Hour: 12, Minute: 0 }:
                    return "meio-dia";
            }

            var normalizedHour = time.Hour % 12;

            return time switch
            {
                { Minute: 00 } => $"{normalizedHour.ToWords()} em ponto",
                { Minute: 30 } => $"{normalizedHour.ToWords()} e meia",
                { Minute: 40 } => $"vinte para as {(normalizedHour + 1).ToWords()}",
                { Minute: 45 } => $"quinze para as {(normalizedHour + 1).ToWords()}",
                { Minute: 50 } => $"dez para as {(normalizedHour + 1).ToWords()}",
                { Minute: 55 } => $"cinco para as {(normalizedHour + 1).ToWords()}",
                _ => $"{normalizedHour.ToWords()} e {time.Minute.ToWords()}"
            };
        }
    }
}

#endif

#if NET6_0_OR_GREATER

using System;
using Humanizer;

namespace Humanizer.Localisation.TimeToClockNotation
{
    internal class FrTimeOnlyToClockNotationConverter : ITimeOnlyToClockNotationConverter
    {
        public virtual string Convert(TimeOnly time, ClockNotationRounding roundToNearestFive)
        {
            switch (time)
            {
                case { Hour: 0, Minute: 0 }:
                    return "minuit";
                case { Hour: 12, Minute: 0 }:
                    return "midi";
            }

            var normalizedMinutes = (int)(roundToNearestFive == ClockNotationRounding.NearestFiveMinutes
                ? 5 * Math.Round(time.Minute / 5.0)
                : time.Minute);

            return normalizedMinutes switch
            {
                00 => GetHourExpression(time.Hour),
                15 => $"{GetHourExpression(time.Hour)} et quart",
                30 => $"{GetHourExpression(time.Hour)} et demie",
                40 => $"{GetHourExpression(time.Hour + 1)} moins vingt",
                45 => $"{GetHourExpression(time.Hour + 1)} moins le quart",
                60 => $"{GetHourExpression(time.Hour + 1)}",
                _ => $"{GetHourExpression(time.Hour)} {normalizedMinutes.ToWords(GrammaticalGender.Feminine)}"
            };

            static string GetHourExpression(int hour)
            {
                return hour.ToWords(GrammaticalGender.Feminine) + (hour > 1 ? " heures" : " heure");
            }
        }
    }
}

#endif

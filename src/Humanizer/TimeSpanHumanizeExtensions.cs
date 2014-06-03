using System;
using System.Globalization;
using System.Text;
using Humanizer.Configuration;
using Humanizer.Localisation;

namespace Humanizer
{
    /// <summary>
    /// Humanizes TimeSpan into human readable form
    /// </summary>
    public static class TimeSpanHumanizeExtensions
    {
        /// <summary>
        /// Turns a TimeSpan into a human readable form. E.g. 1 day.
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <param name="precision">The maximum number of time units to return. Defaulted is 1 which means the largest unit is returned</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <returns></returns>
        public static string Humanize(this TimeSpan timeSpan, int precision = 1, CultureInfo culture = null)
        {
            var result = new StringBuilder();
            for (int i = 0; i < precision; i++)
            {
                var timePart = GetTimePart(timeSpan, culture);
            
                if (result.Length > 0)
                    result.Append(", ");
                
                result.Append(timePart);

                timeSpan = TakeOutTheLargestUnit(timeSpan);
                if (timeSpan == TimeSpan.Zero)
                    break;
            }

            return result.ToString();
        }

        private static string GetTimePart(TimeSpan timespan, CultureInfo culture)
        {
            var formatter = Configurator.GetFormatter(culture);
            if (timespan.Days >= 7)
                return formatter.TimeSpanHumanize(TimeUnit.Week, timespan.Days/7);

            if (timespan.Days >= 1)
                return formatter.TimeSpanHumanize(TimeUnit.Day, timespan.Days);

            if (timespan.Hours >= 1)
                return formatter.TimeSpanHumanize(TimeUnit.Hour, timespan.Hours);

            if (timespan.Minutes >= 1)
                return formatter.TimeSpanHumanize(TimeUnit.Minute, timespan.Minutes);

            if (timespan.Seconds >= 1)
                return formatter.TimeSpanHumanize(TimeUnit.Second, timespan.Seconds);

            if (timespan.Milliseconds >= 1)
                return formatter.TimeSpanHumanize(TimeUnit.Millisecond, timespan.Milliseconds);

            return formatter.TimeSpanHumanize_Zero();
        }

        static TimeSpan TakeOutTheLargestUnit(TimeSpan timeSpan)
        {
            return timeSpan - LargestUnit(timeSpan);
        }

        static TimeSpan LargestUnit(TimeSpan timeSpan)
        {
            var days = timeSpan.Days;
            if (days >= 7)
                return TimeSpan.FromDays((days/7) * 7);
            if (days >= 1)
                return TimeSpan.FromDays(days);

            var hours = timeSpan.Hours;
            if (hours >= 1)
                return TimeSpan.FromHours(hours);

            var minutes = timeSpan.Minutes;
            if (minutes >= 1)
                return TimeSpan.FromMinutes(minutes);

            var seconds = timeSpan.Seconds;
            if (seconds >= 1)
                return TimeSpan.FromSeconds(seconds);

            var milliseconds = timeSpan.Milliseconds;
            if (milliseconds >= 1)
                return TimeSpan.FromMilliseconds(milliseconds);

            return TimeSpan.Zero;
        }
    }
}
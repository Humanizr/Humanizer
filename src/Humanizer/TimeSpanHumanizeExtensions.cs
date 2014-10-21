using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Humanizer.Configuration;
using Humanizer.Localisation;
using Humanizer.Localisation.Formatters;

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
            return string.Join(", ", GetTimeParts(timeSpan, culture).Take(precision).Where(x => x != null));
        }

        private static IEnumerable<string> GetTimeParts(TimeSpan timespan, CultureInfo culture)
        {
            var weeks = timespan.Days / 7;
            var daysInWeek = timespan.Days % 7;
            var hours = timespan.Hours;
            var minutes = timespan.Minutes;
            var seconds = timespan.Seconds;
            var milliseconds = timespan.Milliseconds;

            var outputWeeks = weeks > 0;
            var outputDays = outputWeeks || daysInWeek > 0;
            var outputHours = outputDays || hours > 0;
            var outputMinutes = outputHours || minutes > 0;
            var outputSeconds = outputMinutes || seconds > 0;
            var outputMilliseconds = outputSeconds || milliseconds > 0;

            var formatter = Configurator.GetFormatter(culture);
            if (outputWeeks)
                yield return GetTimePart(formatter, TimeUnit.Week, weeks);
            if (outputDays)
                yield return GetTimePart(formatter, TimeUnit.Day, daysInWeek);
            if (outputHours)
                yield return GetTimePart(formatter, TimeUnit.Hour, hours);
            if (outputMinutes)
                yield return GetTimePart(formatter, TimeUnit.Minute, minutes);
            if (outputSeconds)
                yield return GetTimePart(formatter, TimeUnit.Second, seconds);
            if (outputMilliseconds)
                yield return GetTimePart(formatter, TimeUnit.Millisecond, milliseconds);
            else
                yield return formatter.TimeSpanHumanize_Zero();
        }

        private static string GetTimePart(IFormatter formatter, TimeUnit timeUnit, int unit)
        {
            return unit != 0
                ? formatter.TimeSpanHumanize(timeUnit, unit)
                : null;
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
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
            return Humanize(timeSpan, precision, false, culture);
        }

        /// <summary>
        /// Turns a TimeSpan into a human readable form. E.g. 1 day.
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <param name="precision">The maximum number of time units to return.</param>
        /// <param name="countEmptyUnits">Controls whether empty time units should be counted towards maximum number of time units. Leading empty time units never count.</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <returns></returns>
        public static string Humanize(this TimeSpan timeSpan, int precision, bool countEmptyUnits, CultureInfo culture = null)
        {
            var timeParts = GetTimeParts(timeSpan, culture);
            if (!countEmptyUnits)
                timeParts = timeParts.Where(x => x != null);
            timeParts = timeParts.Take(precision);
            if (countEmptyUnits)
                timeParts = timeParts.Where(x => x != null);
            return string.Join(", ", timeParts);
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
    }
}
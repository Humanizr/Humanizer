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
        /// <param name="maxUnit">The maximum unit of time to output.</param>
        /// <returns></returns>
        public static string Humanize(this TimeSpan timeSpan, int precision = 1, CultureInfo culture = null, TimeUnit maxUnit = TimeUnit.Week)
        {
            return Humanize(timeSpan, precision, false, culture, maxUnit);
        }

        /// <summary>
        /// Turns a TimeSpan into a human readable form. E.g. 1 day.
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <param name="precision">The maximum number of time units to return.</param>
        /// <param name="countEmptyUnits">Controls whether empty time units should be counted towards maximum number of time units. Leading empty time units never count.</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <param name="maxUnit">The maximum unit of time to output.</param>
        /// <returns></returns>
        public static string Humanize(this TimeSpan timeSpan, int precision, bool countEmptyUnits, CultureInfo culture = null, TimeUnit maxUnit = TimeUnit.Week)
        {
            var timeParts = GetTimeParts(timeSpan, culture, maxUnit);
            if (!countEmptyUnits)
                timeParts = timeParts.Where(x => x != null);
            timeParts = timeParts.Take(precision);
            if (countEmptyUnits)
                timeParts = timeParts.Where(x => x != null);
            return string.Join(", ", timeParts);
        }

        private static IEnumerable<string> GetTimeParts(TimeSpan timespan, CultureInfo culture, TimeUnit maxUnit)
        {
            var weeks =  maxUnit > TimeUnit.Day ? timespan.Days / 7 : 0;
            var days = maxUnit > TimeUnit.Day ? timespan.Days % 7 : (int)timespan.TotalDays;
            var hours = maxUnit > TimeUnit.Hour ? timespan.Hours : (int)timespan.TotalHours;
            var minutes = maxUnit > TimeUnit.Minute ? timespan.Minutes : (int)timespan.TotalMinutes;
            var seconds = maxUnit > TimeUnit.Second ? timespan.Seconds : (int)timespan.TotalSeconds;
            var milliseconds = maxUnit > TimeUnit.Millisecond ? timespan.Milliseconds : (int)timespan.TotalMilliseconds;

            var outputWeeks = weeks > 0 && maxUnit == TimeUnit.Week;
            var outputDays = (outputWeeks || days > 0) && maxUnit >= TimeUnit.Day;
            var outputHours = (outputDays || hours > 0) && maxUnit >= TimeUnit.Hour;
            var outputMinutes = (outputHours || minutes > 0) && maxUnit >= TimeUnit.Minute;
            var outputSeconds = (outputMinutes || seconds > 0) && maxUnit >= TimeUnit.Second;
            var outputMilliseconds = (outputSeconds || milliseconds > 0) && maxUnit >= TimeUnit.Millisecond;

            var formatter = Configurator.GetFormatter(culture);
            if (outputWeeks)
                yield return GetTimePart(formatter, TimeUnit.Week, weeks);
            if (outputDays)
                yield return GetTimePart(formatter, TimeUnit.Day, days);
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
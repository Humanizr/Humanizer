using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <returns></returns>
        /// <remarks>
        /// This method chooses the largest part of the TimeSpan (Day, Hour, Minute, Second,
        /// Millisecond) and returns only that part.
        /// </remarks>
        public static string Humanize(this TimeSpan timeSpan)
        {
            return FormatParameters
                .Select(format => TryFormat(format, timeSpan))
                .FirstOrDefault(result => result != null);
        }

        /// <summary>
        /// Gets the elements of the TimeSpan associated with their correct formatter methods
        /// in zero, single and multiple forms.
        /// </summary>
        static IEnumerable<TimeSpanPropertyFormat> FormatParameters
        {
            get
            {
                var formatter = Configurator.Formatter;
                return new[]
                {
                    new TimeSpanPropertyFormat(
                        timespan => timespan.Days / 7,
                        formatter.TimeSpanHumanize_SingleWeek,
                        formatter.TimeSpanHumanize_MultipleWeeks), 
                    new TimeSpanPropertyFormat(
                        timespan => timespan.Days,
                        formatter.TimeSpanHumanize_SingleDay,
                        formatter.TimeSpanHumanize_MultipleDays),
                    new TimeSpanPropertyFormat(
                        timespan => timespan.Hours,
                        formatter.TimeSpanHumanize_SingleHour,
                        formatter.TimeSpanHumanize_MultipleHours),
                    new TimeSpanPropertyFormat(
                        timespan => timespan.Minutes,
                        formatter.TimeSpanHumanize_SingleMinute,
                        formatter.TimeSpanHumanize_MultipleMinutes),
                    new TimeSpanPropertyFormat(
                        timespan => timespan.Seconds,
                        formatter.TimeSpanHumanize_SingleSecond,
                        formatter.TimeSpanHumanize_MultipleSeconds),
                    new TimeSpanPropertyFormat(
                        timespan => timespan.Milliseconds,
                        formatter.TimeSpanHumanize_SingleMillisecond,
                        formatter.TimeSpanHumanize_MultipleMilliseconds),
                    new TimeSpanPropertyFormat(
                        timespan => 0,
                        formatter.TimeSpanHumanize_Zero) 
                };
            }
        }
        
        /// <summary>
        /// Maps a single property (Day, Hour etc.) of TimeSpan to a formatted string "1 day" etc.
        /// </summary>
        /// <param name="propertyFormat"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        private static string TryFormat(
            TimeSpanPropertyFormat propertyFormat,
            TimeSpan timeSpan)
        {
            var value = propertyFormat.PropertySelector(timeSpan);
            switch (value)
            {
                case 0:
                    return propertyFormat.Zero();
                case 1:
                    return propertyFormat.Single();
                default:
                    return propertyFormat.Multiple(value);
            }
        }
    }
}
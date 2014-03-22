using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Humanizer.Configuration;

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
        /// <returns></returns>
        public static string Humanize(this TimeSpan timeSpan, int precision = 1)
        {
            var result = new StringBuilder();
            for (int i = 0; i < precision; i++)
            {
                var timePart = FormatParameters
                    .Select(format => TryFormat(format, timeSpan))
                    .FirstOrDefault(part => part != null);
            
                if (result.Length > 0)
                    result.Append(", ");
                
                result.Append(timePart);

                timeSpan = TakeOutTheLargestUnit(timeSpan);
                if (timeSpan == TimeSpan.Zero)
                    break;
            }

            return result.ToString();
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
                        formatter.TimeSpanHumanize_Weeks), 
                    new TimeSpanPropertyFormat(
                        timespan => timespan.Days,
                        formatter.TimeSpanHumanize_Days),
                    new TimeSpanPropertyFormat(
                        timespan => timespan.Hours,
                        formatter.TimeSpanHumanize_Hours),
                    new TimeSpanPropertyFormat(
                        timespan => timespan.Minutes,
                        formatter.TimeSpanHumanize_Minutes),
                    new TimeSpanPropertyFormat(
                        timespan => timespan.Seconds,
                        formatter.TimeSpanHumanize_Seconds),
                    new TimeSpanPropertyFormat(
                        timespan => timespan.Milliseconds,
                        formatter.TimeSpanHumanize_Milliseconds),
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
                default:
                    return propertyFormat.Format(value);
            }
        }

        /// <summary>
        /// Stores a single mapping of a part of the time span (Day, Hour etc.) to its associated
        /// formatter method for Zero, Single, Multiple.
        /// </summary>
        class TimeSpanPropertyFormat
        {
            public TimeSpanPropertyFormat(
                Func<TimeSpan, int> propertySelector,
                Func<int, string> format)
            {
                PropertySelector = propertySelector;
                Format = format;
                Zero = () => null;
            }

            public TimeSpanPropertyFormat(Func<TimeSpan, int> propertySelector, Func<string> zeroFunc)
            {
                PropertySelector = propertySelector;
                Zero = zeroFunc;
            }

            public Func<TimeSpan, int> PropertySelector { get; private set; }
            public Func<int, string> Format { get; private set; }
            public Func<string> Zero { get; private set; }
        }
    }
}
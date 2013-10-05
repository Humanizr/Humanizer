using System;
using System.Linq;
using Humanizer.TimeSpanLocalisation;

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
            var formatMap = new DefaultTimeSpanFormatMap();
            return formatMap
                .FormatParameters
                .Select(format => TryFormat(format, timeSpan))
                .FirstOrDefault(result => result != null);
        }

        /// <summary>
        /// Maps a single property (Day, Hour etc.) of TimeSpan to a formatted string "1 day" etc.
        /// </summary>
        /// <param name="propertyFormat"></param>
        /// <param name="timeSpan"></param>
        /// <param name="result"></param>
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
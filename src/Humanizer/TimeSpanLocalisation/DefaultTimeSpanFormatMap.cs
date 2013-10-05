using System.Collections.Generic;
using Humanizer.Configuration;

namespace Humanizer.TimeSpanLocalisation
{
    /// <summary>
    /// A default version of the TimeSpan format map.
    /// </summary>
    public class DefaultTimeSpanFormatMap
    {
        /// <summary>
        /// Gets the elements of the TimeSpan associated with their correct formatter methods
        /// in zero, single and multiple forms.
        /// </summary>
        public virtual IEnumerable<TimeSpanPropertyFormat> FormatParameters
        {
            get
            {
                var formatter = Configurator.TimeSpanFormatter;
                return new[]
                {
                    new TimeSpanPropertyFormat(
                        timespan => timespan.Days / 7,
                        formatter.SingleWeek,
                        formatter.MultipleWeeks), 
                    new TimeSpanPropertyFormat(
                        timespan => timespan.Days,
                        formatter.SingleDay,
                        formatter.MultipleDays),
                    new TimeSpanPropertyFormat(
                        timespan => timespan.Hours,
                        formatter.SingleHour,
                        formatter.MultipleHours),
                    new TimeSpanPropertyFormat(
                        timespan => timespan.Minutes,
                        formatter.SingleMinute,
                        formatter.MultipleMinutes),
                    new TimeSpanPropertyFormat(
                        timespan => timespan.Seconds,
                        formatter.SingleSecond,
                        formatter.MultipleSeconds),
                    new TimeSpanPropertyFormat(
                        timespan => timespan.Milliseconds,
                        formatter.SingleMillisecond,
                        formatter.MultipleMilliseconds),
                    new TimeSpanPropertyFormat(
                        timespan => 0,
                        formatter.Zero), 
                };
            }
        }
    }
}
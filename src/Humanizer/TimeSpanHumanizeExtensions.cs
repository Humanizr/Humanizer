using System;
using System.Diagnostics;

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
        public static string Humanize(this TimeSpan timeSpan)
        {
            var days = timeSpan.Days;
            if (days > 1) return string.Format("{0} days", days);
            if (days == 1) return string.Format("1 day");

            var hours = timeSpan.Hours;
            if (hours > 1) return string.Format("{0} hours", hours);
            if (hours == 1) return string.Format("1 hour");

            var minutes = timeSpan.Minutes;
            if (minutes > 1) return string.Format("{0} minutes", minutes);
            if (minutes == 1) return string.Format("1 minute");

            var seconds = timeSpan.Seconds;
            if (seconds > 1) return string.Format("{0} seconds", seconds);
            if (seconds == 1) return string.Format("1 second");

            var milliseconds = timeSpan.Milliseconds;
            if (milliseconds > 1) return string.Format("{0} milliseconds", milliseconds);
            if (milliseconds == 1) return string.Format("1 millisecond");

            Debug.Assert(timeSpan == TimeSpan.Zero, "Should be zero");
            return "No time";
        }
    }
}
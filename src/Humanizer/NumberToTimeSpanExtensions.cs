using System;
namespace Humanizer
{
    /// <summary>
    /// Number to TimeSpan extensions
    /// </summary>
    public static class NumberToTimeSpanExtensions
    {
        /// <summary>
        /// 2.Weeks() == new TimeSpan(14, 0, 0, 0)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static TimeSpan Weeks(this int input)
        {
            return Days(7*input);
        }

        /// <summary>
        /// 2.Days() == TimeSpan.FromDays(2)
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        public static TimeSpan Days(this int days)
        {
            return TimeSpan.FromDays(days);
        }

        /// <summary>
        /// 3.Hours() == TimeSpan.FromHours(3)
        /// </summary>
        /// <param name="hours"></param>
        /// <returns></returns>
        public static TimeSpan Hours(this int hours)
        {
            return TimeSpan.FromHours(hours);
        }

        /// <summary>
        /// 4.Minutes() == TimeSpan.FromMinutes(4) 
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static TimeSpan Minutes(this int minutes)
        {
            return TimeSpan.FromMinutes(minutes);
        }

        /// <summary>
        /// 5.Seconds() == TimeSpan.FromSeconds(5) 
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static TimeSpan Seconds(this int seconds)
        {
            return TimeSpan.FromSeconds(seconds);
        }

        /// <summary>
        /// 5.Milliseconds() == TimeSpan.FromMilliseconds(5) 
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static TimeSpan Milliseconds(this int ms)
        {
            return TimeSpan.FromMilliseconds(ms);
        }
    }
}

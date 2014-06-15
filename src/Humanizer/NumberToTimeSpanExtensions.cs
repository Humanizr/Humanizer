using System;

namespace Humanizer
{
    /// <summary>
    /// Number to TimeSpan extensions
    /// </summary>
    public static class NumberToTimeSpanExtensions
    {
        /// <summary>
        /// 5.Milliseconds() == TimeSpan.FromMilliseconds(5) 
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static TimeSpan Milliseconds(this byte ms)
        {
            return Milliseconds((double)ms);
        }

        /// <summary>
        /// 5.Milliseconds() == TimeSpan.FromMilliseconds(5) 
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static TimeSpan Milliseconds(this sbyte ms)
        {
            return Milliseconds((double)ms);
        }

        /// <summary>
        /// 5.Milliseconds() == TimeSpan.FromMilliseconds(5) 
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static TimeSpan Milliseconds(this short ms)
        {
            return Milliseconds((double)ms);
        }

        /// <summary>
        /// 5.Milliseconds() == TimeSpan.FromMilliseconds(5) 
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static TimeSpan Milliseconds(this ushort ms)
        {
            return Milliseconds((double)ms);
        }

        /// <summary>
        /// 5.Milliseconds() == TimeSpan.FromMilliseconds(5) 
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static TimeSpan Milliseconds(this int ms)
        {
            return Milliseconds((double) ms);
        }

        /// <summary>
        /// 5.Milliseconds() == TimeSpan.FromMilliseconds(5) 
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static TimeSpan Milliseconds(this uint ms)
        {
            return Milliseconds((double) ms);
        }

        /// <summary>
        /// 5.Milliseconds() == TimeSpan.FromMilliseconds(5) 
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static TimeSpan Milliseconds(this long ms)
        {
            return Milliseconds((double) ms);
        }

        /// <summary>
        /// 5.Milliseconds() == TimeSpan.FromMilliseconds(5) 
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static TimeSpan Milliseconds(this ulong ms)
        {
            return Milliseconds((double)ms);
        }

        /// <summary>
        /// 5.Milliseconds() == TimeSpan.FromMilliseconds(5) 
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static TimeSpan Milliseconds(this double ms)
        {
            return TimeSpan.FromMilliseconds(ms);
        }

        /// <summary>
        /// 5.Seconds() == TimeSpan.FromSeconds(5) 
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static TimeSpan Seconds(this byte seconds)
        {
            return Seconds((double)seconds);
        }

        /// <summary>
        /// 5.Seconds() == TimeSpan.FromSeconds(5) 
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static TimeSpan Seconds(this sbyte seconds)
        {
            return Seconds((double)seconds);
        }

        /// <summary>
        /// 5.Seconds() == TimeSpan.FromSeconds(5) 
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static TimeSpan Seconds(this short seconds)
        {
            return Seconds((double)seconds);
        }

        /// <summary>
        /// 5.Seconds() == TimeSpan.FromSeconds(5) 
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static TimeSpan Seconds(this ushort seconds)
        {
            return Seconds((double)seconds);
        }

        /// <summary>
        /// 5.Seconds() == TimeSpan.FromSeconds(5) 
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static TimeSpan Seconds(this int seconds)
        {
            return Seconds((double) seconds);
        }

        /// <summary>
        /// 5.Seconds() == TimeSpan.FromSeconds(5) 
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static TimeSpan Seconds(this uint seconds)
        {
            return Seconds((double) seconds);
        }

        /// <summary>
        /// 5.Seconds() == TimeSpan.FromSeconds(5) 
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static TimeSpan Seconds(this long seconds)
        {
            return Seconds((double) seconds);
        }

        /// <summary>
        /// 5.Seconds() == TimeSpan.FromSeconds(5) 
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static TimeSpan Seconds(this ulong seconds)
        {
            return Seconds((double)seconds);
        }

        /// <summary>
        /// 5.Seconds() == TimeSpan.FromSeconds(5) 
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static TimeSpan Seconds(this double seconds)
        {
            return TimeSpan.FromSeconds(seconds);
        }

        /// <summary>
        /// 4.Minutes() == TimeSpan.FromMinutes(4) 
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static TimeSpan Minutes(this byte minutes)
        {
            return Minutes((double)minutes);
        }

        /// <summary>
        /// 4.Minutes() == TimeSpan.FromMinutes(4) 
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static TimeSpan Minutes(this sbyte minutes)
        {
            return Minutes((double)minutes);
        }

        /// <summary>
        /// 4.Minutes() == TimeSpan.FromMinutes(4) 
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static TimeSpan Minutes(this short minutes)
        {
            return Minutes((double)minutes);
        }

        /// <summary>
        /// 4.Minutes() == TimeSpan.FromMinutes(4) 
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static TimeSpan Minutes(this ushort minutes)
        {
            return Minutes((double)minutes);
        }

        /// <summary>
        /// 4.Minutes() == TimeSpan.FromMinutes(4) 
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static TimeSpan Minutes(this int minutes)
        {
            return Minutes((double) minutes);
        }

        /// <summary>
        /// 4.Minutes() == TimeSpan.FromMinutes(4) 
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static TimeSpan Minutes(this uint minutes)
        {
            return Minutes((double) minutes);
        }

        /// <summary>
        /// 4.Minutes() == TimeSpan.FromMinutes(4) 
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static TimeSpan Minutes(this long minutes)
        {
            return Minutes((double) minutes);
        }

        /// <summary>
        /// 4.Minutes() == TimeSpan.FromMinutes(4) 
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static TimeSpan Minutes(this ulong minutes)
        {
            return Minutes((double)minutes);
        }

        /// <summary>
        /// 4.Minutes() == TimeSpan.FromMinutes(4) 
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static TimeSpan Minutes(this double minutes)
        {
            return TimeSpan.FromMinutes(minutes);
        }

        /// <summary>
        /// 3.Hours() == TimeSpan.FromHours(3)
        /// </summary>
        /// <param name="hours"></param>
        /// <returns></returns>
        public static TimeSpan Hours(this byte hours)
        {
            return Hours((double)hours);
        }

        /// <summary>
        /// 3.Hours() == TimeSpan.FromHours(3)
        /// </summary>
        /// <param name="hours"></param>
        /// <returns></returns>
        public static TimeSpan Hours(this sbyte hours)
        {
            return Hours((double)hours);
        }

        /// <summary>
        /// 3.Hours() == TimeSpan.FromHours(3)
        /// </summary>
        /// <param name="hours"></param>
        /// <returns></returns>
        public static TimeSpan Hours(this short hours)
        {
            return Hours((double)hours);
        }

        /// <summary>
        /// 3.Hours() == TimeSpan.FromHours(3)
        /// </summary>
        /// <param name="hours"></param>
        /// <returns></returns>
        public static TimeSpan Hours(this ushort hours)
        {
            return Hours((double)hours);
        }

        /// <summary>
        /// 3.Hours() == TimeSpan.FromHours(3)
        /// </summary>
        /// <param name="hours"></param>
        /// <returns></returns>
        public static TimeSpan Hours(this int hours)
        {
            return Hours((double) hours);
        }

        /// <summary>
        /// 3.Hours() == TimeSpan.FromHours(3)
        /// </summary>
        /// <param name="hours"></param>
        /// <returns></returns>
        public static TimeSpan Hours(this uint hours)
        {
            return Hours((double) hours);
        }

        /// <summary>
        /// 3.Hours() == TimeSpan.FromHours(3)
        /// </summary>
        /// <param name="hours"></param>
        /// <returns></returns>
        public static TimeSpan Hours(this long hours)
        {
            return Hours((double) hours);
        }

        /// <summary>
        /// 3.Hours() == TimeSpan.FromHours(3)
        /// </summary>
        /// <param name="hours"></param>
        /// <returns></returns>
        public static TimeSpan Hours(this ulong hours)
        {
            return Hours((double)hours);
        }

        /// <summary>
        /// 3.Hours() == TimeSpan.FromHours(3)
        /// </summary>
        /// <param name="hours"></param>
        /// <returns></returns>
        public static TimeSpan Hours(this double hours)
        {
            return TimeSpan.FromHours(hours);
        }

        /// <summary>
        /// 2.Days() == TimeSpan.FromDays(2)
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        public static TimeSpan Days(this byte days)
        {
            return Days((double)days);
        }

        /// <summary>
        /// 2.Days() == TimeSpan.FromDays(2)
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        public static TimeSpan Days(this sbyte days)
        {
            return Days((double)days);
        }

        /// <summary>
        /// 2.Days() == TimeSpan.FromDays(2)
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        public static TimeSpan Days(this short days)
        {
            return Days((double)days);
        }

        /// <summary>
        /// 2.Days() == TimeSpan.FromDays(2)
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        public static TimeSpan Days(this ushort days)
        {
            return Days((double)days);
        }

        /// <summary>
        /// 2.Days() == TimeSpan.FromDays(2)
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        public static TimeSpan Days(this int days)
        {
            return Days((double) days);
        }

        /// <summary>
        /// 2.Days() == TimeSpan.FromDays(2)
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        public static TimeSpan Days(this uint days)
        {
            return Days((double) days);
        }

        /// <summary>
        /// 2.Days() == TimeSpan.FromDays(2)
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        public static TimeSpan Days(this long days)
        {
            return Days((double) days);
        }

        /// <summary>
        /// 2.Days() == TimeSpan.FromDays(2)
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        public static TimeSpan Days(this ulong days)
        {
            return Days((double)days);
        }

        /// <summary>
        /// 2.Days() == TimeSpan.FromDays(2)
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        public static TimeSpan Days(this double days)
        {
            return TimeSpan.FromDays(days);
        }

        /// <summary>
        /// 2.Weeks() == new TimeSpan(14, 0, 0, 0)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static TimeSpan Weeks(this byte input)
        {
            return Weeks((double)input);
        }

        /// <summary>
        /// 2.Weeks() == new TimeSpan(14, 0, 0, 0)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static TimeSpan Weeks(this sbyte input)
        {
            return Weeks((double)input);
        }

        /// <summary>
        /// 2.Weeks() == new TimeSpan(14, 0, 0, 0)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static TimeSpan Weeks(this short input)
        {
            return Weeks((double)input);
        }

        /// <summary>
        /// 2.Weeks() == new TimeSpan(14, 0, 0, 0)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static TimeSpan Weeks(this ushort input)
        {
            return Weeks((double)input);
        }

        /// <summary>
        /// 2.Weeks() == new TimeSpan(14, 0, 0, 0)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static TimeSpan Weeks(this int input)
        {
            return Weeks((double) input);
        }

        /// <summary>
        /// 2.Weeks() == new TimeSpan(14, 0, 0, 0)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static TimeSpan Weeks(this uint input)
        {
            return Weeks((double) input);
        }

        /// <summary>
        /// 2.Weeks() == new TimeSpan(14, 0, 0, 0)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static TimeSpan Weeks(this long input)
        {
            return Weeks((double) input);
        }
        
        /// <summary>
        /// 2.Weeks() == new TimeSpan(14, 0, 0, 0)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static TimeSpan Weeks(this ulong input)
        {
            return Weeks((double)input);
        }

        /// <summary>
        /// 2.Weeks() == new TimeSpan(14, 0, 0, 0)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static TimeSpan Weeks(this double input)
        {
            return Days(7*input);
        }
    }
}
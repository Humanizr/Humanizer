using System;
namespace Humanizer
{
    public static class NumberToTimeSpanExtensions
    {
        /// <summary>
        /// 2.Weeks() == new TimeSpan(14, 0, 0, 0)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static TimeSpan Weeks(this int input)
        {
            return new TimeSpan(input*7, 0, 0, 0);
        }

        /// <summary>
        /// 2.Days() == new TimeSpan(2, 0, 0, 0)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static TimeSpan Days(this int input)
        {
            return new TimeSpan(input, 0, 0, 0);
        }

        /// <summary>
        /// 3.Hours() == new TimeSpan(0, 3, 0, 0)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static TimeSpan Hours(this int input)
        {
            return new TimeSpan(0, input, 0, 0);
        }

        /// <summary>
        /// 4.Minutes() == new TimeSpan(0, 0, 4, 0) 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static TimeSpan Minutes(this int input)
        {
            return new TimeSpan(0, 0, input, 0);
        }

        /// <summary>
        /// 5.Seconds() == new TimeSpan(0, 0, 0, 5) 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static TimeSpan Seconds(this int input)
        {
            return new TimeSpan(0, 0, 0, input);
        }

        /// <summary>
        /// 5.Milliseconds() == new TimeSpan(0, 0, 0, 0, 5) 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static TimeSpan Milliseconds(this int input)
        {
            return new TimeSpan(0, 0, 0, 0, input);
        }
    }
}

using System;
namespace Humanizer
{
    public static class NumberToTimeSpanExtensions
    {
        public static TimeSpan Days(this int input)
        {
            return new TimeSpan(input, 0, 0, 0);
        }

        public static TimeSpan Hours(this int input)
        {
            return new TimeSpan(0, input, 0, 0);
        }

        public static TimeSpan Minutes(this int input)
        {
            return new TimeSpan(0, 0, input, 0);
        }

        public static TimeSpan Seconds(this int input)
        {
            return new TimeSpan(0, 0, 0, input);
        }
    }
}

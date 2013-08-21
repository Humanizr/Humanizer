using System;
namespace Humanizer
{
    public static class PrepositionsExtensions
    {
        public static DateTime At(this DateTime date, int hour, int min = 0, int second = 0, int millisecond = 0)
        {
            return new DateTime(date.Year, date.Month, date.Day, hour, min, second, millisecond);
        }

        public static DateTime AtMidnight(this DateTime date)
        {
            return date.At(0);
        }

        public static DateTime AtNoon(this DateTime date)
        {
            return date.At(12);
        }

        public static DateTime In(this DateTime date, int year)
        {
            return new DateTime(year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Millisecond);
        }
    }
}

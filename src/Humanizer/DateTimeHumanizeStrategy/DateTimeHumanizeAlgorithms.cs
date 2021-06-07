using System;
using System.Globalization;
using Humanizer.Configuration;
using Humanizer.Localisation;

namespace Humanizer.DateTimeHumanizeStrategy
{
    /// <summary>
    /// Algorithms used to convert distance between two dates into words.
    /// </summary>
    internal static class DateTimeHumanizeAlgorithms
    {
        /// <summary>
        /// Returns localized &amp; humanized distance of time between two dates; given a specific precision.
        /// </summary>
        public static string PrecisionHumanize(DateTime input, DateTime comparisonBase, double precision, CultureInfo culture)
        {
            var ts = new TimeSpan(Math.Abs(comparisonBase.Ticks - input.Ticks));
            var tense = input > comparisonBase ? Tense.Future : Tense.Past;

            return PrecisionHumanize(ts, tense, precision, culture);
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// Returns localized &amp; humanized distance of time between two dates; given a specific precision.
        /// </summary>
        public static string PrecisionHumanize(DateOnly input, DateOnly comparisonBase, double precision, CultureInfo culture)
        {
            var diffDays = Math.Abs(comparisonBase.DayOfYear - input.DayOfYear);
            var ts = new TimeSpan(diffDays, 0, 0, 0);
            var tense = input > comparisonBase ? Tense.Future : Tense.Past;

            return PrecisionHumanize(ts, tense, precision, culture);
        }

        /// <summary>
        /// Returns localized &amp; humanized distance of time between two times; given a specific precision.
        /// </summary>
        public static string PrecisionHumanize(TimeOnly input, TimeOnly comparisonBase, double precision, CultureInfo culture)
        {
            var ts = new TimeSpan(Math.Abs(comparisonBase.Ticks - input.Ticks));
            var tense = input > comparisonBase ? Tense.Future : Tense.Past;

            return PrecisionHumanize(ts, tense, precision, culture);
        }
#endif
        private static string PrecisionHumanize(TimeSpan ts, Tense tense, double precision, CultureInfo culture)
        {
            int seconds = ts.Seconds, minutes = ts.Minutes, hours = ts.Hours, days = ts.Days;
            int years = 0, months = 0;

            // start approximate from smaller units towards bigger ones
            if (ts.Milliseconds >= 999 * precision)
            {
                seconds += 1;
            }

            if (seconds >= 59 * precision)
            {
                minutes += 1;
            }

            if (minutes >= 59 * precision)
            {
                hours += 1;
            }

            if (hours >= 23 * precision)
            {
                days += 1;
            }

            // month calculation 
            if (days >= 30 * precision & days <= 31)
            {
                months = 1;
            }

            if (days > 31 && days < 365 * precision)
            {
                var factor = Convert.ToInt32(Math.Floor((double)days / 30));
                var maxMonths = Convert.ToInt32(Math.Ceiling((double)days / 30));
                months = (days >= 30 * (factor + precision)) ? maxMonths : maxMonths - 1;
            }

            // year calculation
            if (days >= 365 * precision && days <= 366)
            {
                years = 1;
            }

            if (days > 365)
            {
                var factor = Convert.ToInt32(Math.Floor((double)days / 365));
                var maxMonths = Convert.ToInt32(Math.Ceiling((double)days / 365));
                years = (days >= 365 * (factor + precision)) ? maxMonths : maxMonths - 1;
            }

            // start computing result from larger units to smaller ones
            var formatter = Configurator.GetFormatter(culture);
            if (years > 0)
            {
                return formatter.DateHumanize(TimeUnit.Year, tense, years);
            }

            if (months > 0)
            {
                return formatter.DateHumanize(TimeUnit.Month, tense, months);
            }

            if (days > 0)
            {
                return formatter.DateHumanize(TimeUnit.Day, tense, days);
            }

            if (hours > 0)
            {
                return formatter.DateHumanize(TimeUnit.Hour, tense, hours);
            }

            if (minutes > 0)
            {
                return formatter.DateHumanize(TimeUnit.Minute, tense, minutes);
            }

            if (seconds > 0)
            {
                return formatter.DateHumanize(TimeUnit.Second, tense, seconds);
            }

            return formatter.DateHumanize(TimeUnit.Millisecond, tense, 0);
        }

        // http://stackoverflow.com/questions/11/how-do-i-calculate-relative-time
        /// <summary>
        /// Calculates the distance of time in words between two provided dates
        /// </summary>
        public static string DefaultHumanize(DateTime input, DateTime comparisonBase, CultureInfo culture)
        {
            var tense = input > comparisonBase ? Tense.Future : Tense.Past;
            var ts = new TimeSpan(Math.Abs(comparisonBase.Ticks - input.Ticks));

            var sameMonth = comparisonBase.Date.AddMonths(tense == Tense.Future ? 1 : -1) == input.Date;

            var days = Math.Abs((input.Date - comparisonBase.Date).Days);

            return DefaultHumanize(ts, sameMonth, days, tense, culture);
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// Calculates the distance of time in words between two provided dates
        /// </summary>
        public static string DefaultHumanize(DateOnly input, DateOnly comparisonBase, CultureInfo culture)
        {
            var tense = input > comparisonBase ? Tense.Future : Tense.Past;
            var diffDays = Math.Abs(comparisonBase.DayOfYear - input.DayOfYear);
            var ts = new TimeSpan(diffDays, 0, 0, 0);

            var sameMonth = comparisonBase.AddMonths(tense == Tense.Future ? 1 : -1) == input;

            var days = Math.Abs(input.DayOfYear - comparisonBase.DayOfYear);

            return DefaultHumanize(ts, sameMonth, days, tense, culture);
        }

        /// <summary>
        /// Calculates the distance of time in words between two provided times
        /// </summary>
        public static string DefaultHumanize(TimeOnly input, TimeOnly comparisonBase, CultureInfo culture)
        {
            var tense = input > comparisonBase ? Tense.Future : Tense.Past;
            var ts = new TimeSpan(Math.Abs(comparisonBase.Ticks - input.Ticks));

            return DefaultHumanize(ts, true, 0, tense, culture);
        }
#endif

        private static string DefaultHumanize(TimeSpan ts, bool sameMonth, int days, Tense tense, CultureInfo culture)
        {
            var formatter = Configurator.GetFormatter(culture);

            if (ts.TotalMilliseconds < 500)
            {
                return formatter.DateHumanize(TimeUnit.Millisecond, tense, 0);
            }

            if (ts.TotalSeconds < 60)
            {
                return formatter.DateHumanize(TimeUnit.Second, tense, ts.Seconds);
            }

            if (ts.TotalSeconds < 120)
            {
                return formatter.DateHumanize(TimeUnit.Minute, tense, 1);
            }

            if (ts.TotalMinutes < 60)
            {
                return formatter.DateHumanize(TimeUnit.Minute, tense, ts.Minutes);
            }

            if (ts.TotalMinutes < 90)
            {
                return formatter.DateHumanize(TimeUnit.Hour, tense, 1);
            }

            if (ts.TotalHours < 24)
            {
                return formatter.DateHumanize(TimeUnit.Hour, tense, ts.Hours);
            }

            if (ts.TotalHours < 48)
            {   
                return formatter.DateHumanize(TimeUnit.Day, tense, days);
            }

            if (ts.TotalDays < 28)
            {
                return formatter.DateHumanize(TimeUnit.Day, tense, ts.Days);
            }

            if (ts.TotalDays >= 28 && ts.TotalDays < 30)
            {
                if (sameMonth)
                {
                    return formatter.DateHumanize(TimeUnit.Month, tense, 1);
                }

                return formatter.DateHumanize(TimeUnit.Day, tense, ts.Days);
            }

            if (ts.TotalDays < 345)
            {
                var months = Convert.ToInt32(Math.Floor(ts.TotalDays / 29.5));
                return formatter.DateHumanize(TimeUnit.Month, tense, months);
            }

            var years = Convert.ToInt32(Math.Floor(ts.TotalDays / 365));
            if (years == 0)
            {
                years = 1;
            }

            return formatter.DateHumanize(TimeUnit.Year, tense, years);
        }
    }
}

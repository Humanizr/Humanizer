using System;
using System.Globalization;
using Humanizer.Configuration;
using Humanizer.Localisation;

namespace Humanizer.DateTimeHumanizeStrategy
{
    /// <summary>
    /// The default 'distance of time' -> words calculator.
    /// </summary>
    public class DefaultDateTimeHumanizeStrategy : IDateTimeHumanizeStrategy
    {
        // http://stackoverflow.com/questions/11/how-do-i-calculate-relative-time
        /// <summary>
        ///     Calculates the distance of time in words between two provided dates
        /// </summary>
        /// <param name="input"></param>
        /// <param name="comparisonBase"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public string Humanize(DateTime input, DateTime comparisonBase, CultureInfo culture)
        {
            var tense = input > comparisonBase ? Tense.Future : Tense.Past;
            var ts = new TimeSpan(Math.Abs(comparisonBase.Ticks - input.Ticks));

            var formatter = Configurator.GetFormatter(culture);

            if (ts.TotalMilliseconds < 500)
                return formatter.DateHumanize(TimeUnit.Millisecond, tense, 0);

            if (ts.TotalSeconds < 60)
                return formatter.DateHumanize(TimeUnit.Second, tense, ts.Seconds);

            if (ts.TotalSeconds < 120)
                return formatter.DateHumanize(TimeUnit.Minute, tense, 1);

            if (ts.TotalMinutes < 60)
                return formatter.DateHumanize(TimeUnit.Minute, tense, ts.Minutes);

            if (ts.TotalMinutes < 90)
                return formatter.DateHumanize(TimeUnit.Hour, tense, 1);

            if (ts.TotalHours < 24)
                return formatter.DateHumanize(TimeUnit.Hour, tense, ts.Hours);

            if (ts.TotalHours < 48)
            {
                var days = Math.Abs((input.Date - comparisonBase.Date).Days);
                return formatter.DateHumanize(TimeUnit.Day, tense, days);
            }

            if (ts.TotalDays < 28)
                return formatter.DateHumanize(TimeUnit.Day, tense, ts.Days);

            if (ts.TotalDays >= 28 && ts.TotalDays < 30)
            {
                if (comparisonBase.Date.AddMonths(tense == Tense.Future ? 1 : -1) == input.Date)
                    return formatter.DateHumanize(TimeUnit.Month, tense, 1);
                return formatter.DateHumanize(TimeUnit.Day, tense, ts.Days);
            }

            if (ts.TotalDays < 345)
            {
                int months = Convert.ToInt32(Math.Floor(ts.TotalDays / 29.5));
                return formatter.DateHumanize(TimeUnit.Month, tense, months);
            }

            int years = Convert.ToInt32(Math.Floor(ts.TotalDays / 365));
            if (years == 0) years = 1;

            return formatter.DateHumanize(TimeUnit.Year, tense, years);
        }
    }
}
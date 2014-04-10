using System;
using Humanizer.Configuration;
using Humanizer.Localisation;

namespace Humanizer.DateTimeHumanizeStrategy
{
    /// <summary>
    ///     The default distance of time in works calculator
    /// </summary>
    public class DefaultDateTimeHumanizeStrategy : IDateTimeHumanizeStrategy
    {
        // http://stackoverflow.com/questions/11/how-do-i-calculate-relative-time
        /// <summary>
        ///     Calculates the distance of time in words between two provided dates
        /// </summary>
        /// <param name="input"></param>
        /// <param name="comparisonBase"></param>
        /// <returns></returns>
        public string Humanize(DateTime input, DateTime comparisonBase)
        {
            var tense = input > comparisonBase ? Tense.Future : Tense.Past;
            var ts = new TimeSpan(Math.Abs(comparisonBase.Ticks - input.Ticks));

            if (ts.TotalMilliseconds < 500)
                return Configurator.Formatter.DateHumanize(TimeUnit.Millisecond, tense, 0);

            if (ts.TotalSeconds < 60)
                return Configurator.Formatter.DateHumanize(TimeUnit.Second, tense, ts.Seconds);

            if (ts.TotalSeconds < 120)
                return Configurator.Formatter.DateHumanize(TimeUnit.Minute, tense, 1);

            if (ts.TotalMinutes < 45)
                return Configurator.Formatter.DateHumanize(TimeUnit.Minute, tense, ts.Minutes);

            if (ts.TotalMinutes < 90)
                return Configurator.Formatter.DateHumanize(TimeUnit.Hour, tense, 1);

            if (ts.TotalHours < 24)
                return Configurator.Formatter.DateHumanize(TimeUnit.Hour, tense, ts.Hours);

            if (ts.TotalHours < 48)
                return Configurator.Formatter.DateHumanize(TimeUnit.Day, tense, 1);

            if (ts.TotalDays < 28)
                return Configurator.Formatter.DateHumanize(TimeUnit.Day, tense, ts.Days);

            if (ts.TotalDays >= 28 && ts.TotalDays < 30)
            {
                if (comparisonBase.Date.AddMonths(tense == Tense.Future ? 1 : -1) == input.Date)
                    return Configurator.Formatter.DateHumanize(TimeUnit.Month, tense, 1);
                return Configurator.Formatter.DateHumanize(TimeUnit.Day, tense, ts.Days);
            }

            if (ts.TotalDays < 345)
            {
                int months = Convert.ToInt32(Math.Floor(ts.TotalDays / 29.5));
                return Configurator.Formatter.DateHumanize(TimeUnit.Month, tense, months);
            }

            int years = Convert.ToInt32(Math.Floor(ts.TotalDays / 365));
            if (years == 0) years = 1;

            return Configurator.Formatter.DateHumanize(TimeUnit.Year, tense, years);
        }
    }
}
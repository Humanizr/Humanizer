using System;
using Humanizer.Configuration;
using Humanizer.Localisation;

namespace Humanizer
{
    /// <summary>
    /// Humanizes DateTime into human readable sentence
    /// </summary>
    public static class DateHumanizeExtensions
    {
        // http://stackoverflow.com/questions/11/how-do-i-calculate-relative-time
        /// <summary>
        /// Turns the current or provided date into a human readable sentence
        /// </summary>
        /// <param name="input">The date to be humanized</param>
        /// <param name="utcDate">Boolean value indicating whether the date is in UTC or local</param>
        /// <param name="dateToCompareAgainst">Date to compare the input against. If null, current date is used as base</param>
        /// <returns></returns>
        public static string Humanize(this DateTime input, bool utcDate = true, DateTime? dateToCompareAgainst = null)
        {
            if (dateToCompareAgainst == null)
                dateToCompareAgainst = DateTime.UtcNow;

            var formatter = Configurator.Formatter;
            var comparisonBase = dateToCompareAgainst.Value;

            if (!utcDate)
                comparisonBase = comparisonBase.ToLocalTime();

            if (input <= comparisonBase && comparisonBase.Subtract(input) < TimeSpan.FromMilliseconds(500))
                return formatter.DateHumanize_Now();

            var timeUnitTense = input > comparisonBase ? Tense.Future : Tense.Past;
            var ts = new TimeSpan(Math.Abs(comparisonBase.Ticks - input.Ticks));

            if (ts.TotalSeconds < 60)
                return formatter.DateHumanize(TimeUnit.Second, timeUnitTense, ts.Seconds);

            if (ts.TotalSeconds < 120)
                return formatter.DateHumanize(TimeUnit.Minute, timeUnitTense, 1);

            if (ts.TotalMinutes < 45)
                return formatter.DateHumanize(TimeUnit.Minute, timeUnitTense, ts.Minutes);

            if (ts.TotalMinutes < 90)
                return formatter.DateHumanize(TimeUnit.Hour, timeUnitTense, 1);

            if (ts.TotalHours < 24)
                return formatter.DateHumanize(TimeUnit.Hour, timeUnitTense, ts.Hours);

            if (ts.TotalHours < 48)
                return formatter.DateHumanize(TimeUnit.Day, timeUnitTense, 1);

            if (ts.TotalDays < 28)
                return formatter.DateHumanize(TimeUnit.Day, timeUnitTense, ts.Days);

            if (ts.TotalDays >= 28 && ts.TotalDays < 30)
            {
                if (comparisonBase.Date.AddMonths(timeUnitTense == Tense.Future ? 1 : -1) == input.Date)
                    return formatter.DateHumanize(TimeUnit.Month, timeUnitTense, 1);

                return formatter.DateHumanize(TimeUnit.Day, timeUnitTense, ts.Days);
            }

            if (ts.TotalDays < 345)
            {
                int months = Convert.ToInt32(Math.Floor(ts.TotalDays / 29.5));
                return formatter.DateHumanize(TimeUnit.Month, timeUnitTense, months);
            }

            int years = Convert.ToInt32(Math.Floor(ts.TotalDays / 365));
            if (years == 0) years = 1;
            return formatter.DateHumanize(TimeUnit.Year, timeUnitTense, years);
        }
    }
}
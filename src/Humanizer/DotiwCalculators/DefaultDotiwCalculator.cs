using System;
using Humanizer.Configuration;
using Humanizer.Localisation;

namespace Humanizer.DotiwCalculators
{
    /// <summary>
    /// The default distance of time in works calculator
    /// </summary>
    public class DefaultDotiwCalculator : IDistanceOfTimeInWords
    {
        // http://stackoverflow.com/questions/11/how-do-i-calculate-relative-time
        /// <summary>
        /// Calculates the distance of time in words between two provided dates
        /// </summary>
        /// <param name="date1">date 1</param>
        /// <param name="date2">date 2</param>
        /// <returns></returns>
        public string Calculate(DateTime date1, DateTime date2)
        {
            var formatter = Configurator.Formatter;

            if (date1 <= date2 && date2.Subtract(date1) < TimeSpan.FromMilliseconds(500))
                return formatter.DateHumanize_Now();

            var timeUnitTense = date1 > date2 ? TimeUnitTense.Future : TimeUnitTense.Past;
            var ts = new TimeSpan(Math.Abs(date2.Ticks - date1.Ticks));

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
                if (date2.Date.AddMonths(timeUnitTense == TimeUnitTense.Future ? 1 : -1) == date1.Date)
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
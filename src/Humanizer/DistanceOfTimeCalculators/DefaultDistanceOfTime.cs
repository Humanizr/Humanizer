using System;
using Humanizer.Configuration;
using Humanizer.Localisation;

namespace Humanizer.DistanceOfTimeCalculators
{
    /// <summary>
    ///     The default distance of time in works calculator
    /// </summary>
    public class DefaultDistanceOfTime : IDistanceOfTimeInWords
    {
        // http://stackoverflow.com/questions/11/how-do-i-calculate-relative-time
        /// <summary>
        ///     Calculates the distance of time in words between two provided dates
        /// </summary>
        /// <param name="date1">date 1</param>
        /// <param name="date2">date 2</param>
        /// <returns></returns>
        public string Calculate(DateTime date1, DateTime date2)
        {
            return Configurator.Formatter.Humanize(Compute(date1, date2));
        }

        private static DistanceOfTime Compute(DateTime date1, DateTime date2)
        {
            var timeUnitTense = date1 > date2 ? TimeUnitTense.Future : TimeUnitTense.Past;
            var ts = new TimeSpan(Math.Abs(date2.Ticks - date1.Ticks));

            if (ts.TotalMilliseconds < 500)
                return DistanceOfTime.Create(TimeUnit.Millisecond, timeUnitTense, count: 0);

            if (ts.TotalSeconds < 60)
                return DistanceOfTime.Create(TimeUnit.Second, timeUnitTense, count: ts.Seconds);

            if (ts.TotalSeconds < 120)
                return DistanceOfTime.Create(TimeUnit.Minute, timeUnitTense);

            if (ts.TotalMinutes < 45)
                return DistanceOfTime.Create(TimeUnit.Minute, timeUnitTense, count: ts.Minutes);

            if (ts.TotalMinutes < 90)
                return DistanceOfTime.Create(TimeUnit.Hour, timeUnitTense);

            if (ts.TotalHours < 24)
                return DistanceOfTime.Create(TimeUnit.Hour, timeUnitTense, count: ts.Hours);

            if (ts.TotalHours < 48)
                return DistanceOfTime.Create(TimeUnit.Day, timeUnitTense);

            if (ts.TotalDays < 28)
                return DistanceOfTime.Create(TimeUnit.Day, timeUnitTense, count: ts.Days);

            if (ts.TotalDays >= 28 && ts.TotalDays < 30)
            {
                if (date2.Date.AddMonths(timeUnitTense == TimeUnitTense.Future ? 1 : -1) == date1.Date)
                    return DistanceOfTime.Create(TimeUnit.Month, timeUnitTense);
                return DistanceOfTime.Create(TimeUnit.Day, timeUnitTense, count: ts.Days);
            }

            if (ts.TotalDays < 345)
            {
                int months = Convert.ToInt32(Math.Floor(ts.TotalDays / 29.5));
                return DistanceOfTime.Create(TimeUnit.Month, timeUnitTense, count: months);
            }

            int years = Convert.ToInt32(Math.Floor(ts.TotalDays / 365));
            if (years == 0) years = 1;

            return DistanceOfTime.Create(TimeUnit.Year, timeUnitTense, count: years);
        }
    }
}
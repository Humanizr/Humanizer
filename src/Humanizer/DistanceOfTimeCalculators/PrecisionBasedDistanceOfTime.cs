using System;
using System.Diagnostics;
using Humanizer.Configuration;
using Humanizer.Localisation;

namespace Humanizer.DistanceOfTimeCalculators
{
    /// <summary>
    /// Precision-based distance of time in works calculator
    /// </summary>
    public class PrecisionBasedDistanceOfTime : IDistanceOfTimeInWords
    {
        /// <summary>
        /// Calculates the DOTIW between two dates.
        /// </summary>
        /// <param name="date1">date 1</param>
        /// <param name="date2">date 2</param>
        /// <returns>Distance of time in words (DOTIW)</returns>
        public string Calculate(DateTime date1, DateTime date2)
        {
            Debug.WriteLine("Using PrecisionBasedDistanceOfTime");

            var ts = new TimeSpan(Math.Abs(date2.Ticks - date1.Ticks));
            var timeUnitTense = date1 > date2 ? TimeUnitTense.Future : TimeUnitTense.Past;
            var distanceOfTime = Compute(ts, timeUnitTense, TimeStructure.DateTime);

            return Configurator.Formatter.Humanize(distanceOfTime);
        }

        private static DistanceOfTime Compute(TimeSpan ts, TimeUnitTense tense, TimeStructure structure)
        {
            //Note: Calculation has magic numbers but it'll be much difficult with variables. I did try :)

            double precision = Configurator.DefaultPrecision;
            int seconds = ts.Seconds, minutes = ts.Minutes, hours = ts.Hours, days = ts.Days;
            int years = 0, months = 0;

            // start approximate from smaller units towards bigger ones
            if (ts.Milliseconds >= 999 * precision) seconds += 1;
            if (seconds >= 59 * precision) minutes += 1;
            if (minutes >= 59 * precision) hours += 1;
            if (hours >= 23 * precision) days += 1;

            // month calculation 
            if (days >= 30 * precision & days <= 31) months += 1;
            if (days > 31)
            {
                int factor = Convert.ToInt32(Math.Floor((double)days / 30));
                int maxMonths = Convert.ToInt32(Math.Ceiling((double)days / 30));
                months = (days >= 30 * (factor + precision)) ? maxMonths : maxMonths - 1;
            }

            // year calculation
            if (days >= 365 * precision && days <= 366) years += 1;
            if (days > 365)
            {
                int factor = Convert.ToInt32(Math.Floor((double)days / 365));
                int maxMonths = Convert.ToInt32(Math.Ceiling((double)days / 365));
                years = (days >= 365 * (factor + precision)) ? maxMonths : maxMonths - 1;
            }

            // start computing result from larger units to smaller ones
            if (years > 0) return DistanceOfTime.Create(TimeUnit.Year, tense, structure, years);
            if (months > 0) return DistanceOfTime.Create(TimeUnit.Month, tense, structure, months);
            if (days > 0) return DistanceOfTime.Create(TimeUnit.Day, tense, structure, days);
            if (hours > 0) return DistanceOfTime.Create(TimeUnit.Hour, tense, structure, hours);
            if (minutes > 0) return DistanceOfTime.Create(TimeUnit.Minute, tense, structure, minutes);
            if (seconds > 0) return DistanceOfTime.Create(TimeUnit.Second, tense, structure, seconds);
            return DistanceOfTime.Create(TimeUnit.Millisecond, tense, structure, 0);
        }
    }
}
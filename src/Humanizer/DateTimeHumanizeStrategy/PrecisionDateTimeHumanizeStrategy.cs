using System;
using System.Globalization;
using Humanizer.Configuration;
using Humanizer.Localisation;

namespace Humanizer.DateTimeHumanizeStrategy
{
    /// <summary>
    /// 
    /// </summary>
    public class PrecisionDateTimeHumanizeStrategy : IDateTimeHumanizeStrategy
    {
        private readonly double _precision;

        /// <summary>
        /// Constructs a precision-based calculator for distance of time with default precision 0.75.
        /// </summary>
        /// <param name="precision">precision of approximation, if not provided  0.75 will be used as a default precision.</param>
        public PrecisionDateTimeHumanizeStrategy(double precision = .75)
        {
            _precision = precision;
        }

        /// <summary>
        /// Returns localized &amp; humanized distance of time between two dates; given a specific precision.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="comparisonBase"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public string Humanize(DateTime input, DateTime comparisonBase, CultureInfo culture)
        {
            var ts = new TimeSpan(Math.Abs(comparisonBase.Ticks - input.Ticks));
            var tense = input > comparisonBase ? Tense.Future : Tense.Past;

            int seconds = ts.Seconds, minutes = ts.Minutes, hours = ts.Hours, days = ts.Days;
            int years = 0, months = 0;

            // start approximate from smaller units towards bigger ones
            if (ts.Milliseconds >= 999 * _precision) seconds += 1;
            if (seconds >= 59 * _precision) minutes += 1;
            if (minutes >= 59 * _precision) hours += 1;
            if (hours >= 23 * _precision) days += 1;

            // month calculation 
            if (days >= 30 * _precision & days <= 31) months = 1;
            if (days > 31 && days < 365 * _precision)
            {
                int factor = Convert.ToInt32(Math.Floor((double)days / 30));
                int maxMonths = Convert.ToInt32(Math.Ceiling((double)days / 30));
                months = (days >= 30 * (factor + _precision)) ? maxMonths : maxMonths - 1;
            }

            // year calculation
            if (days >= 365 * _precision && days <= 366) years = 1;
            if (days > 365)
            {
                int factor = Convert.ToInt32(Math.Floor((double)days / 365));
                int maxMonths = Convert.ToInt32(Math.Ceiling((double)days / 365));
                years = (days >= 365 * (factor + _precision)) ? maxMonths : maxMonths - 1;
            }

            // start computing result from larger units to smaller ones
            var formatter = Configurator.GetFormatter(culture);
            if (years > 0) return formatter.DateHumanize(TimeUnit.Year, tense, years);
            if (months > 0) return formatter.DateHumanize(TimeUnit.Month, tense, months);
            if (days > 0) return formatter.DateHumanize(TimeUnit.Day, tense, days);
            if (hours > 0) return formatter.DateHumanize(TimeUnit.Hour, tense, hours);
            if (minutes > 0) return formatter.DateHumanize(TimeUnit.Minute, tense, minutes);
            if (seconds > 0) return formatter.DateHumanize(TimeUnit.Second, tense, seconds);
            return formatter.DateHumanize(TimeUnit.Millisecond, tense, 0);
        }
    }
}
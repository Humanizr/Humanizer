using System;
using Humanizer.Configuration;

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
            
            const int second = 1;
            const int minute = 60 * second;
            const int hour = 60 * minute;
            const int day = 24 * hour;
            const int month = 30 * day;

            var comparisonBase = dateToCompareAgainst.Value;
            if (!utcDate)
                comparisonBase = comparisonBase.ToLocalTime();

            var formatter = Configurator.Formatter;

            var isFuture = input > comparisonBase;

            var ts = new TimeSpan(comparisonBase.Ticks - input.Ticks);
            if(isFuture)
                ts = new TimeSpan(-ts.Ticks);

            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1*minute)
            {
                if(isFuture)
                    return ts.Seconds == 1 ? formatter.DateHumanize_SingleSecondFromNow() : formatter.DateHumanize_MultipleSecondsFromNow(ts.Seconds);

                return ts.Seconds == 1 ? formatter.DateHumanize_SingleSecondAgo() : formatter.DateHumanize_MultipleSecondsAgo(ts.Seconds);
            }

            if (delta < 2 * minute)
                return isFuture ? formatter.DateHumanize_SingleMinuteFromNow() : formatter.DateHumanize_SingleMinuteAgo();

            if (delta < 45 * minute)
                return isFuture ? formatter.DateHumanize_MultipleMinutesFromNow(ts.Minutes) : formatter.DateHumanize_MultipleMinutesAgo(ts.Minutes);

            if (delta < 90 * minute)
                return isFuture ? formatter.DateHumanize_SingleHourFromNow() : formatter.DateHumanize_SingleHourAgo();

            if (delta < 24 * hour)
                return isFuture ? formatter.DateHumanize_MultipleHoursFromNow(ts.Hours) : formatter.DateHumanize_MultipleHoursAgo(ts.Hours);

            if (delta < 48 * hour)
                return isFuture ? formatter.DateHumanize_SingleDayFromNow() : formatter.DateHumanize_SingleDayAgo();

            if (delta < 30 * day)
                return isFuture ? formatter.DateHumanize_MultipleDaysFromNow(ts.Days) : formatter.DateHumanize_MultipleDaysAgo(ts.Days);

            if (delta < 12 * month)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                
                if(isFuture)
                    return months <= 1 ? formatter.DateHumanize_SingleMonthFromNow() : formatter.DateHumanize_MultipleMonthsFromNow(months);

                return months <= 1 ? formatter.DateHumanize_SingleMonthAgo() : formatter.DateHumanize_MultipleMonthsAgo(months);
            }

            int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            
            if(isFuture)
                return years <= 1 ? formatter.DateHumanize_SingleYearFromNow() : formatter.DateHumanize_MultipleYearsFromNow(years);

            return years <= 1 ? formatter.DateHumanize_SingleYearAgo() : formatter.DateHumanize_MultipleYearsAgo(years);
        }
    }
}
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

            var formatter = Configurator.Formatter;
            var comparisonBase = dateToCompareAgainst.Value;

            if (!utcDate)
                comparisonBase = comparisonBase.ToLocalTime();

            if (input <= comparisonBase && comparisonBase.Subtract(input) < TimeSpan.FromMilliseconds(500))
                return formatter.DateHumanize_Now();

            var isFuture = input > comparisonBase;
            var ts = new TimeSpan(Math.Abs(comparisonBase.Ticks - input.Ticks));

            if (ts.TotalSeconds < 60)
            {
                if (isFuture)
                    return ts.TotalSeconds <= 1 ? formatter.DateHumanize_SingleSecondFromNow() : formatter.DateHumanize_MultipleSecondsFromNow(ts.Seconds);

                return ts.TotalSeconds <= 1 ? formatter.DateHumanize_SingleSecondAgo() : formatter.DateHumanize_MultipleSecondsAgo(ts.Seconds);
            }

            if (ts.TotalSeconds < 120)
                return isFuture ? formatter.DateHumanize_SingleMinuteFromNow() : formatter.DateHumanize_SingleMinuteAgo();

            if (ts.TotalMinutes < 45)
                return isFuture ? formatter.DateHumanize_MultipleMinutesFromNow(ts.Minutes) : formatter.DateHumanize_MultipleMinutesAgo(ts.Minutes);

            if (ts.TotalMinutes < 90)
                return isFuture ? formatter.DateHumanize_SingleHourFromNow() : formatter.DateHumanize_SingleHourAgo();

            if (ts.TotalHours < 24)
                return isFuture ? formatter.DateHumanize_MultipleHoursFromNow(ts.Hours) : formatter.DateHumanize_MultipleHoursAgo(ts.Hours);

            if (ts.TotalHours < 48)
                return isFuture ? formatter.DateHumanize_SingleDayFromNow() : formatter.DateHumanize_SingleDayAgo();

            if (ts.TotalDays < 28)
                return isFuture ? formatter.DateHumanize_MultipleDaysFromNow(ts.Days) : formatter.DateHumanize_MultipleDaysAgo(ts.Days);

            if (ts.TotalDays >= 28 && ts.TotalDays < 30)
            {
                if (comparisonBase.Date.AddMonths(isFuture ? 1 : -1) == input.Date)
                    return isFuture ? formatter.DateHumanize_SingleMonthFromNow() : formatter.DateHumanize_SingleMonthAgo();

                return isFuture ? formatter.DateHumanize_MultipleDaysFromNow(ts.Days) : formatter.DateHumanize_MultipleDaysAgo(ts.Days);
            }

            if (ts.TotalDays < 345)
            {
                int months = Convert.ToInt32(Math.Floor(ts.TotalDays / 29.5));

                if (isFuture)
                    return months <= 1 ? formatter.DateHumanize_SingleMonthFromNow() : formatter.DateHumanize_MultipleMonthsFromNow(months);

                return months <= 1 ? formatter.DateHumanize_SingleMonthAgo() : formatter.DateHumanize_MultipleMonthsAgo(months);
            }

            int years = Convert.ToInt32(Math.Floor(ts.TotalDays / 365));

            if (isFuture)
                return years <= 1 ? formatter.DateHumanize_SingleYearFromNow() : formatter.DateHumanize_MultipleYearsFromNow(years);

            return years <= 1 ? formatter.DateHumanize_SingleYearAgo() : formatter.DateHumanize_MultipleYearsAgo(years);
        }
    }
}
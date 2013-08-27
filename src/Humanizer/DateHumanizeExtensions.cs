using System;
using System.ComponentModel;
using Humanizer.Configuration;
using Humanizer.Properties;

namespace Humanizer
{
    /// <summary>
    /// Humanizes DateTime into human readable sentence
    /// </summary>
    [Localizable(true)]
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

            if (input > comparisonBase)
                return Resources.DateHumanize_not_yet;

            var ts = new TimeSpan(comparisonBase.Ticks - input.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * minute)
                return ts.Seconds == 1 ? Resources.DateHumanize_one_second_ago : Configurator.Formatter.FormatNumberInString(Resources.DateHumanize__seconds_ago, ts.Seconds);

            if (delta < 2 * minute)
                return Resources.DateHumanize_a_minute_ago;

            if (delta < 45 * minute)
                return Configurator.Formatter.FormatNumberInString(Resources.DateHumanize__minutes_ago, ts.Minutes);

            if (delta < 90 * minute)
                return Resources.DateHumanize_an_hour_ago;

            if (delta < 24 * hour)
                return Configurator.Formatter.FormatNumberInString(Resources.DateHumanize__hours_ago, ts.Hours);

            if (delta < 48 * hour)
                return Resources.DateHumanize_yesterday;

            if (delta < 30 * day)
                return Configurator.Formatter.FormatNumberInString(Resources.DateHumanize__days_ago, ts.Days);

            if (delta < 12 * month)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? Resources.DateHumanize_one_month_ago : Configurator.Formatter.FormatNumberInString(Resources.DateHumanize__months_ago, months);
            }

            int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            return years <= 1 ? Resources.DateHumanize_one_year_ago : Configurator.Formatter.FormatNumberInString(Resources.DateHumanize__years_ago, years);
        }
    }
}
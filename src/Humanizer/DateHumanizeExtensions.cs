using System;
using Humanizer.Configuration;
using Humanizer.DistanceOfTimeCalculators;

namespace Humanizer
{
    /// <summary>
    /// Humanizes DateTime into human readable sentence
    /// </summary>
    public static class DateHumanizeExtensions
    {
        /// <summary>
        /// Distance of time in works calculator
        /// </summary>
        private static readonly IDistanceOfTimeInWords DistanceOfTimeInWords = Configurator.DistanceOfTimeInWords;

        /// <summary>
        /// Turns the current or provided date into a human readable sentence
        /// </summary>
        /// <param name="input">The date to be humanized</param>
        /// <param name="utcDate">Boolean value indicating whether the date is in UTC or local</param>
        /// <param name="dateToCompareAgainst">Date to compare the input against. If null, current date is used as base</param>
        /// <returns>distance of time in words</returns>
        public static string Humanize(this DateTime input, bool utcDate = true, DateTime? dateToCompareAgainst = null)
        {
            var comparisonBase = dateToCompareAgainst ?? DateTime.UtcNow;

            if (!utcDate)
                comparisonBase = comparisonBase.ToLocalTime();

            return DistanceOfTimeInWords.Calculate(input, comparisonBase);
        }
    }
}
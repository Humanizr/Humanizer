﻿using System;
using System.Globalization;
using Humanizer.Configuration;

namespace Humanizer
{
    /// <summary>
    /// Humanizes DateTime into human readable sentence
    /// </summary>
    public static class DateHumanizeExtensions
    {
        /// <summary>
        /// Turns the current or provided date into a human readable sentence
        /// </summary>
        /// <param name="input">The date to be humanized</param>
        /// <param name="utcDate">Boolean value indicating whether the date is in UTC or local</param>
        /// <param name="dateToCompareAgainst">Date to compare the input against. If null, current date is used as base</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <param name="showQuantityAs">How to show the quantity. Numeric by default</param>
        /// <returns>distance of time in words</returns>
        public static string Humanize(this DateTime input, bool utcDate = true, DateTime? dateToCompareAgainst = null, CultureInfo culture = null ,  ShowQuantityAs showQuantityAs = ShowQuantityAs.Numeric)
        {
            var comparisonBase = dateToCompareAgainst ?? DateTime.UtcNow;

            if (!utcDate)
                comparisonBase = comparisonBase.ToLocalTime();

            return Configurator.DateTimeHumanizeStrategy.Humanize(input, comparisonBase, culture,   showQuantityAs );
        }

        /// <summary>
        /// Turns the current or provided date into a human readable sentence
        /// </summary>
        /// <param name="input">The date to be humanized</param>
        /// <param name="dateToCompareAgainst">Date to compare the input against. If null, current date is used as base</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <param name="showQuantityAs">How to show the quantity. Numeric by default</param>
        /// <returns>distance of time in words</returns>
        public static string Humanize(this DateTimeOffset input, DateTimeOffset? dateToCompareAgainst = null, CultureInfo culture = null, ShowQuantityAs showQuantityAs =  ShowQuantityAs.Numeric)
        {
            var comparisonBase = dateToCompareAgainst ?? DateTimeOffset.UtcNow;

            return Configurator.DateTimeOffsetHumanizeStrategy.Humanize(input, comparisonBase, culture, showQuantityAs);
        }
    }
}
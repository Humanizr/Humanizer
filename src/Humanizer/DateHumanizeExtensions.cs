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
        /// <param name="utcDate">Nullable boolean value indicating whether the date is in UTC or local. If null, current date is used with the same DateTimeKind of input</param>
        /// <param name="dateToCompareAgainst">Date to compare the input against. If null, current date is used as base</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <returns>distance of time in words</returns>
        public static string Humanize(this DateTime input, bool? utcDate = null, DateTime? dateToCompareAgainst = null, CultureInfo culture = null)
        {
            var comparisonBase = dateToCompareAgainst.HasValue ? dateToCompareAgainst.Value : DateTime.UtcNow;
            utcDate ??= input.Kind != DateTimeKind.Local;
            comparisonBase = utcDate.Value ? comparisonBase.ToUniversalTime() : comparisonBase.ToLocalTime();

            return Configurator.DateTimeHumanizeStrategy.Humanize(input, comparisonBase, culture);
        }

        /// <summary>
        /// Turns the current or provided date into a human readable sentence, overload for the nullable DateTime, returning 'never' in case null
        /// </summary>
        /// <param name="input">The date to be humanized</param>
        /// <param name="utcDate">Nullable boolean value indicating whether the date is in UTC or local. If null, current date is used with the same DateTimeKind of input</param>
        /// <param name="dateToCompareAgainst">Date to compare the input against. If null, current date is used as base</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <returns>distance of time in words</returns>
        public static string Humanize(this DateTime? input, bool? utcDate = null, DateTime? dateToCompareAgainst = null, CultureInfo culture = null)
        {
            if (input.HasValue)
            {
                return Humanize(input.Value, utcDate, dateToCompareAgainst, culture);
            }
            else
            {
                return Configurator.GetFormatter(culture).DateHumanize_Never();
            }
        }

        /// <summary>
        /// Turns the current or provided date into a human readable sentence
        /// </summary>
        /// <param name="input">The date to be humanized</param>
        /// <param name="dateToCompareAgainst">Date to compare the input against. If null, current date is used as base</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <returns>distance of time in words</returns>
        public static string Humanize(this DateTimeOffset input, DateTimeOffset? dateToCompareAgainst = null, CultureInfo culture = null)
        {
            var comparisonBase = dateToCompareAgainst ?? DateTimeOffset.UtcNow;

            return Configurator.DateTimeOffsetHumanizeStrategy.Humanize(input, comparisonBase, culture);
        }

        /// <summary>
        /// Turns the current or provided date into a human readable sentence, overload for the nullable DateTimeOffset, returning 'never' in case null
        /// </summary>
        /// <param name="input">The date to be humanized</param>
        /// <param name="dateToCompareAgainst">Date to compare the input against. If null, current date is used as base</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <returns>distance of time in words</returns>
        public static string Humanize(this DateTimeOffset? input, DateTimeOffset? dateToCompareAgainst = null, CultureInfo culture = null)
        {
            if (input.HasValue)
            {
                return Humanize(input.Value, dateToCompareAgainst, culture);
            }
            else
            {
                return Configurator.GetFormatter(culture).DateHumanize_Never();
            }
        }
    }
}

using System;
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
        /// <returns>distance of time in words</returns>
        public static string Humanize(this DateTime input, bool utcDate = true, DateTime? dateToCompareAgainst = null, CultureInfo culture = null)
        {
            var comparisonBase = dateToCompareAgainst ?? DateTime.UtcNow;

            if (!utcDate)
                comparisonBase = comparisonBase.ToLocalTime();

            return Configurator.DateTimeHumanizeStrategy.Humanize(input, comparisonBase, culture);
        }

        /// <summary>
        /// Extracts the month out of a date
        /// </summary>
        /// <param name="input">The date of which you want the month</param>
        /// <returns></returns>
        public static string ToMonth(this DateTime input)
        {
            string output = string.Empty;
            switch (input.Month)
            {
                case 1:
                    output = "january";
                    break;
                case 2:
                    output = "february";
                    break;
                case 3:
                    output = "march";
                    break;
                case 4:
                    output = "april";
                    break;
                case 5:
                    output = "may";
                    break;
                case 6:
                    output = "june";
                    break;
                case 7:
                    output = "july";
                    break;
                case 8:
                    output = "august";
                    break;
                case 9:
                    output = "september";
                    break;
                case 10:
                    output = "october";
                    break;
                case 11:
                    output = "november";
                    break;
                case 12:
                    output = "december";
                    break;

                default:
                    break;
            }
            return output;
        }

        /// <summary>
        /// Extracts the day out of a date
        /// </summary>
        /// <param name="input">The date of which you want the day</param>
        /// <returns></returns>
        public static string ToDay(this DateTime input)
        {
            string result = input.Day.ToString();
            char endingNumber = result[result.Length - 1];
            if (endingNumber == '1')
            {
                result += "st";
            }
            else if (endingNumber == '2')
            {
                result += "nd";
            }
            else if (endingNumber == '3')
            {
                result += "rd";
            }
            else
            {
                result += "th";
            }
            return result;
        }
    }
}
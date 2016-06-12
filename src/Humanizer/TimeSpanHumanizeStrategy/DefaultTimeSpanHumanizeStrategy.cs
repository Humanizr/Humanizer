using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Humanizer.Configuration;
using Humanizer.Localisation;
using Humanizer.Localisation.Formatters;

namespace Humanizer.TimeSpanHumanizeStrategy
{
    /// <summary>
    /// Converts a period of time into a more human friendly form based assuming a fixed number of
    /// days in a month, and in a year.
    /// </summary>
    public class DefaultTimeSpanHumanizeStrategy : ITimeSpanHumanizeStrategy
    {
        /// <summary>
        /// The approximate number of days in a year.
        /// </summary>
        int daysInYear = 365;

        /// <summary>
        /// The approximate number of days in a month.
        /// </summary>
        int daysInMonth = 30;

        /// <summary>
        /// The exact number of days in a day.
        /// </summary>
        int daysInWeek = 7;

        /// <summary>
        /// Calculates the distance of time in words between two provided dates used for TimeSpan.Humanize
        /// </summary>
        public string Humanize(TimeSpanAnalysisParameters tsAnalysisParameters)
        {
            var timeParts = CreateTheTimePartsWithUpperAndLowerLimits(tsAnalysisParameters);
            timeParts = SetPrecisionOfTimeSpan(timeParts, tsAnalysisParameters.Precision, tsAnalysisParameters.CountEmptyUnits);

            return ConcatenateTimeSpanParts(timeParts, tsAnalysisParameters.CollectionSeparator);
        }

        private IEnumerable<string> CreateTheTimePartsWithUpperAndLowerLimits(TimeSpanAnalysisParameters timeSpanAnalysisParameters)
        {
            var cultureFormatter = Configurator.GetFormatter(timeSpanAnalysisParameters.CultureInfo);
            var firstValueFound = false;
            var timeUnitsEnumTypes = GetEnumTypesForTimeUnit();
            var timeParts = new List<string>();

            foreach (var timeUnitType in timeUnitsEnumTypes)
            {
                var timepart = GetTimeUnitPart(timeUnitType, timeSpanAnalysisParameters, cultureFormatter);

                if (timepart != null || firstValueFound)
                {
                    firstValueFound = true;
                    timeParts.Add(timepart);
                }
            }
            if (IsContainingOnlyNullValue(timeParts))
            {
                var noTimeValueCultureFarmated = cultureFormatter.TimeSpanHumanize_Zero();
                timeParts = CreateTimePartsWithNoTimeValue(noTimeValueCultureFarmated);
            }
            return timeParts;
        }

        private IEnumerable<TimeUnit> GetEnumTypesForTimeUnit()
        {
            var enumTypeEnumerator = (IEnumerable<TimeUnit>)Enum.GetValues(typeof(TimeUnit));

            return enumTypeEnumerator.Reverse();
        }

        private string GetTimeUnitPart(TimeUnit timeUnitToGet, TimeSpanAnalysisParameters timeSpanAnalysisParameters, IFormatter cultureFormatter)
        {
            if (timeUnitToGet <= timeSpanAnalysisParameters.MaximumTimeUnit && timeUnitToGet >= timeSpanAnalysisParameters.MinimumTimeUnit)
            {
                var isTimeUnitToGetTheMaximumTimeUnit = (timeUnitToGet == timeSpanAnalysisParameters.MaximumTimeUnit);
                var numberOfTimeUnits = GetTimeUnitNumericalValue(timeUnitToGet, timeSpanAnalysisParameters.TimeSpan, isTimeUnitToGetTheMaximumTimeUnit);
                return BuildFormatTimePart(cultureFormatter, timeUnitToGet, numberOfTimeUnits);
            }
            return null;
        }

        private int GetTimeUnitNumericalValue(TimeUnit timeUnitToGet, TimeSpan timeSpan, bool isTimeUnitToGetTheMaximumTimeUnit)
        {
            switch (timeUnitToGet)
            {
                case TimeUnit.Millisecond:
                    return GetNormalCaseTimeAsInteger(timeSpan.Milliseconds, timeSpan.TotalMilliseconds, isTimeUnitToGetTheMaximumTimeUnit);
                case TimeUnit.Second:
                    return GetNormalCaseTimeAsInteger(timeSpan.Seconds, timeSpan.TotalSeconds, isTimeUnitToGetTheMaximumTimeUnit);
                case TimeUnit.Minute:
                    return GetNormalCaseTimeAsInteger(timeSpan.Minutes, timeSpan.TotalMinutes, isTimeUnitToGetTheMaximumTimeUnit);
                case TimeUnit.Hour:
                    return GetNormalCaseTimeAsInteger(timeSpan.Hours, timeSpan.TotalHours, isTimeUnitToGetTheMaximumTimeUnit);
                case TimeUnit.Day:
                    return GetSpecialCaseDaysAsInteger(timeSpan, isTimeUnitToGetTheMaximumTimeUnit);
                case TimeUnit.Week:
                    return GetSpecialCaseWeeksAsInteger(timeSpan, isTimeUnitToGetTheMaximumTimeUnit);
                case TimeUnit.Month:
                    return GetSpecialCaseMonthsAsInteger(timeSpan, isTimeUnitToGetTheMaximumTimeUnit);
                case TimeUnit.Year:
                    return GetSpecialCaseYearsAsInteger(timeSpan, isTimeUnitToGetTheMaximumTimeUnit);
                default:
                    return 0;
            }
        }
        private int GetSpecialCaseYearsAsInteger(TimeSpan timespan, bool isTimeUnitToGetTheMaximumTimeUnit)
        {
            if (isTimeUnitToGetTheMaximumTimeUnit)
            {
                return timespan.Days / daysInYear;
            }
            // To be implemented with the implementation of Month and Year
            return 0;
        }

        private int GetSpecialCaseMonthsAsInteger(TimeSpan timespan, bool isTimeUnitToGetTheMaximumTimeUnit)
        {
            if (isTimeUnitToGetTheMaximumTimeUnit)
            {
                return timespan.Days / daysInMonth;
            }
            // To be implemented with the implementation of Month and Year
            return 0;
        }

        private int GetSpecialCaseWeeksAsInteger(TimeSpan timespan, bool isTimeUnitToGetTheMaximumTimeUnit)
        {
            if (isTimeUnitToGetTheMaximumTimeUnit)
            {
                return timespan.Days / daysInWeek;
            }
            // To be implemented with the implementation of Month and Year
            return 0;
        }

        private int GetSpecialCaseDaysAsInteger(TimeSpan timespan, bool isTimeUnitToGetTheMaximumTimeUnit)
        {
            if (isTimeUnitToGetTheMaximumTimeUnit)
            {
                return timespan.Days;
            }
            return timespan.Days % daysInWeek;
        }

        private int GetNormalCaseTimeAsInteger(int timeNumberOfUnits, double totalTimeNumberOfUnits, bool isTimeUnitToGetTheMaximumTimeUnit)
        {
            if (isTimeUnitToGetTheMaximumTimeUnit)
            {
                try
                {
                    return (int)totalTimeNumberOfUnits;
                }
                catch
                {
                    //To be implemented so that TimeSpanHumanize method accepts double type as unit
                    return 0;
                }
            }
            return timeNumberOfUnits;
        }

        private string BuildFormatTimePart(IFormatter cultureFormatter, TimeUnit timeUnitType, int amountOfTimeUnits)
        {
            // Always use positive units to account for negative timespans
            return amountOfTimeUnits != 0
                ? cultureFormatter.TimeSpanHumanize(timeUnitType, Math.Abs(amountOfTimeUnits))
                : null;
        }

        private List<string> CreateTimePartsWithNoTimeValue(string noTimeValue)
        {
            return new List<string> { noTimeValue };
        }

        private static bool IsContainingOnlyNullValue(IEnumerable<string> timeParts)
        {
            return (timeParts.Count(x => x != null) == 0);
        }

        private IEnumerable<string> SetPrecisionOfTimeSpan(IEnumerable<string> timeParts, int precision, bool countEmptyUnits)
        {
            if (!countEmptyUnits)
                timeParts = timeParts.Where(x => x != null);
            timeParts = timeParts.Take(precision);
            if (countEmptyUnits)
                timeParts = timeParts.Where(x => x != null);

            return timeParts;
        }

        private string ConcatenateTimeSpanParts(IEnumerable<string> timeSpanParts, string collectionSeparator)
        {
            if (collectionSeparator == null)
            {
                return Configurator.CollectionFormatter.Humanize(timeSpanParts);
            }

            return string.Join(collectionSeparator, timeSpanParts);
        }
    }
}


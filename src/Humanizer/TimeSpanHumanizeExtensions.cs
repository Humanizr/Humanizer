using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Humanizer.Configuration;
using Humanizer.Localisation;
using Humanizer.Localisation.Formatters;

namespace Humanizer
{
    /// <summary>
    /// Humanizes TimeSpan into human readable form
    /// </summary>
    public static class TimeSpanHumanizeExtensions
    {
        private const int _lastTimeUnitTypeIndexImplemented = (int)TimeUnit.Week;
        private const int _daysInAWeek = 7;

        /// <summary>
        /// Turns a TimeSpan into a human readable form. E.g. 1 day.
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <param name="precision">The maximum number of time units to return. Defaulted is 1 which means the largest unit is returned</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <param name="maxUnit">The maximum unit of time to output.</param>
        /// <param name="minUnit">The minimum unit of time to output.</param>
        /// <returns></returns>
        public static string Humanize(this TimeSpan timeSpan, int precision = 1, CultureInfo culture = null, TimeUnit maxUnit = TimeUnit.Week, TimeUnit minUnit = TimeUnit.Millisecond)
        {
            return Humanize(timeSpan, precision, false, culture, maxUnit, minUnit);
        }

        /// <summary>
        /// Turns a TimeSpan into a human readable form. E.g. 1 day.
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <param name="precision">The maximum number of time units to return.</param>
        /// <param name="countEmptyUnits">Controls whether empty time units should be counted towards maximum number of time units. Leading empty time units never count.</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <param name="maxUnit">The maximum unit of time to output.</param>
        /// <param name="minUnit">The minimum unit of time to output.</param>
        /// <returns></returns>
        public static string Humanize(this TimeSpan timeSpan, int precision, bool countEmptyUnits, CultureInfo culture = null, TimeUnit maxUnit = TimeUnit.Week, TimeUnit minUnit = TimeUnit.Millisecond)
        {
            IEnumerable<string> timeParts = CreateTheTimePartsWithUperAndLowerLimits(timeSpan, culture, maxUnit, minUnit);
            timeParts = SetPrecisionOfTimeSpan(timeParts, precision, countEmptyUnits);

            return ConcatenateTimeSpanParts(timeParts);
        }

        private static IEnumerable<string> CreateTheTimePartsWithUperAndLowerLimits(TimeSpan timespan, CultureInfo culture, TimeUnit maxUnit, TimeUnit minUnit)
        {
            var cultureFormatter = Configurator.GetFormatter(culture);
            bool firstValueFound = false;
            IEnumerable<TimeUnit> timeUnitsEnumTypes = GetEnumTypesForTimeUnit();
            List<string> timeParts = new List<string>();

            foreach (var timeUnitType in timeUnitsEnumTypes)
            {
                var timepart = GetTimeUnitPart(timeUnitType, timespan, culture, maxUnit, minUnit, cultureFormatter);

                if (timepart != null || firstValueFound)
                {
                    firstValueFound = true;
                    timeParts.Add(timepart);
                }
            }
            if (IsContainingOnlyNullValue(timeParts))
            {
                string noTimeValueCultureFarmated = cultureFormatter.TimeSpanHumanize_Zero();
                timeParts = CreateTimePartsWithNoTimeValue(noTimeValueCultureFarmated);
            }
            return timeParts;
        }

        private static IEnumerable<TimeUnit> GetEnumTypesForTimeUnit()
        {
            IEnumerable<TimeUnit> enumTypeEnumerator = (IEnumerable<TimeUnit>)Enum.GetValues(typeof(TimeUnit));
            enumTypeEnumerator = enumTypeEnumerator.Take(_lastTimeUnitTypeIndexImplemented + 1);

            return enumTypeEnumerator.Reverse();
        }

        private static string GetTimeUnitPart(TimeUnit timeUnitToGet, TimeSpan timespan, CultureInfo culture, TimeUnit maximumTimeUnit, TimeUnit minimumTimeUnit, IFormatter cultureFormatter)
        {
            if(timeUnitToGet <= maximumTimeUnit && timeUnitToGet >= minimumTimeUnit)
            {
                var isTimeUnitToGetTheMaximumTimeUnit = (timeUnitToGet == maximumTimeUnit);
                var numberOfTimeUnits = GetTimeUnitNumericalValue(timeUnitToGet, timespan, isTimeUnitToGetTheMaximumTimeUnit);
                return BuildFormatTimePart(cultureFormatter, timeUnitToGet, numberOfTimeUnits);
            }
            return null;
        }

        private static int GetTimeUnitNumericalValue(TimeUnit timeUnitToGet, TimeSpan timespan, bool isTimeUnitToGetTheMaximumTimeUnit)
        {
            switch (timeUnitToGet)
            {
                case TimeUnit.Millisecond:
                    return GetNormalCaseTimeAsInteger(timespan.Milliseconds, timespan.TotalMilliseconds, isTimeUnitToGetTheMaximumTimeUnit);
                case TimeUnit.Second:
                    return GetNormalCaseTimeAsInteger(timespan.Seconds, timespan.TotalSeconds, isTimeUnitToGetTheMaximumTimeUnit);
                case TimeUnit.Minute:
                    return GetNormalCaseTimeAsInteger(timespan.Minutes, timespan.TotalMinutes, isTimeUnitToGetTheMaximumTimeUnit);
                case TimeUnit.Hour:
                    return GetNormalCaseTimeAsInteger(timespan.Hours, timespan.TotalHours, isTimeUnitToGetTheMaximumTimeUnit);
                case TimeUnit.Day:
                    return GetSpecialCaseDaysAsInteger(timespan, isTimeUnitToGetTheMaximumTimeUnit);
                case TimeUnit.Week:
                    return GetSpecialCaseWeeksAsInteger(timespan, isTimeUnitToGetTheMaximumTimeUnit);
                case TimeUnit.Month:
                    // To be implemented
                case TimeUnit.Year:
                    // To be implemented
                default:
                    return 0;
            }
        }

        private static int GetSpecialCaseWeeksAsInteger(TimeSpan timespan, bool isTimeUnitToGetTheMaximumTimeUnit)
        {
            if (isTimeUnitToGetTheMaximumTimeUnit)
            {
                return timespan.Days / _daysInAWeek;
            }
            // To be implemented with the implementation of Month and Year
            return 0;
        }

        private static int GetSpecialCaseDaysAsInteger(TimeSpan timespan, bool isTimeUnitToGetTheMaximumTimeUnit)
        {
            if (isTimeUnitToGetTheMaximumTimeUnit)
            {
                return timespan.Days;
            }
            return timespan.Days % _daysInAWeek;
        }

        private static int GetNormalCaseTimeAsInteger(int timeNumberOfUnits, double totalTimeNumberOfUnits, bool isTimeUnitToGetTheMaximumTimeUnit)
        {
            if (isTimeUnitToGetTheMaximumTimeUnit)
            {
                try
                {
                    return Convert.ToInt32(totalTimeNumberOfUnits);
                }
                catch
                {
                    //To be implemented so that TimeSpanHumanize method accepts double type as unit
                    return 0;
                }
            }
            return timeNumberOfUnits;
        }

        private static string BuildFormatTimePart(IFormatter cultureFormatter, TimeUnit timeUnitType, int amountOfTimeUnits)
        {
            return amountOfTimeUnits != 0
                ? cultureFormatter.TimeSpanHumanize(timeUnitType, amountOfTimeUnits)
                : null;
        }

        private static List<string> CreateTimePartsWithNoTimeValue(string noTimeValue)
        {
            return new List<string>() { noTimeValue };
        }

        private static bool IsContainingOnlyNullValue(IEnumerable<string> timeParts)
        {
            return (timeParts.Count(x => x != null) == 0);
        }

        private static IEnumerable<string> SetPrecisionOfTimeSpan(IEnumerable<string> timeParts, int precision, bool countEmptyUnits)
        {
            if (!countEmptyUnits)
                timeParts = timeParts.Where(x => x != null);
            timeParts = timeParts.Take(precision);
            if (countEmptyUnits)
                timeParts = timeParts.Where(x => x != null);

            return timeParts;
        }

        private static string ConcatenateTimeSpanParts(IEnumerable<string> timeSpanParts)
        {
            return string.Join(", ", timeSpanParts);
        }
    }
}
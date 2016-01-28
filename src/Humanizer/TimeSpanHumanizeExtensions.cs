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
        /// <summary>
        /// The exact number of days in a week.
        /// </summary>
        private const int DaysInAWeek = 7;

        /// <summary>
        /// Turns a TimeSpan into a human readable form. E.g. 1 day.
        /// </summary>
        /// <param name="timeSpan">The time amount to be interpreted.</param>
        /// <param name="precision">The maximum number of time units to return. Defaulted is 1 which means the largest unit is returned</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <param name="maxUnit">The maximum unit of time to output.</param>
        /// <param name="minUnit">The minimum unit of time to output.</param>
        /// <param name="collectionSeparator">The separator to use when combining humanized time parts. If null, the default collection formatter for the current culture is used.</param>
        /// <returns>The given <paramref name="timeSpan"/> interpreted in a more friendly form.</returns>
        public static string Humanize(this TimeSpan timeSpan, int precision = 1, CultureInfo culture = null, TimeUnit maxUnit = TimeUnit.Week, TimeUnit minUnit = TimeUnit.Millisecond, string collectionSeparator = ", ")
        {
            TimeSpanData timeSpanData = new TimeSpanData(timeSpan, precision, culture, maxUnit, minUnit, collectionSeparator: collectionSeparator);

            return Humanize(timeSpanData);
        }

        /// <summary>
        /// Turns a TimeSpan into a human readable form. E.g. 1 day.
        /// </summary>
        /// <param name="timeSpan">The time amount to be interpreted.</param>
        /// <param name="referenceTime">A reference in time from which the given <paramref name="timeSpan"/> is calculated.</param>
        /// <param name="periodBeforeReference">Denotes whether the <paramref name="timeSpan"/> happened before or after the given <paramref name="referenceTime"/>.</param>
        /// <param name="precision">The maximum number of time units to return.</param>
        /// <param name="countEmptyUnits">Controls whether empty time units should be counted towards maximum number of time units. Leading empty time units never count.</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <param name="maxUnit">The maximum unit of time to output.</param>
        /// <param name="minUnit">The minimum unit of time to output.</param>
        /// <param name="collectionSeparator">The separator to use when combining humanized time parts. If null, the default collection formatter for the current culture is used.</param>
        /// <returns>The given <paramref name="timeSpan"/> interpreted in a more friendly form.</returns>
        public static string Humanize(this TimeSpan timeSpan, DateTime referenceTime, bool periodBeforeReference, int precision = 1, bool countEmptyUnits = false, CultureInfo culture = null, TimeUnit maxUnit = TimeUnit.Week, TimeUnit minUnit = TimeUnit.Millisecond, string collectionSeparator = ", ")
        {
            TimeSpanData timeSpanData = new TimeSpanData(timeSpan, precision, culture, maxUnit, minUnit, referenceTime, periodBeforeReference, countEmptyUnits, collectionSeparator);

            return Humanize(timeSpanData);
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
        /// <param name="collectionSeparator">The separator to use when combining humanized time parts. If null, the default collection formatter for the current culture is used.</param>
        /// <returns>The given <paramref name="timeSpan"/> interpreted in a more friendly form.</returns>
        public static string Humanize(this TimeSpan timeSpan, int precision, bool countEmptyUnits, CultureInfo culture = null, TimeUnit maxUnit = TimeUnit.Week, TimeUnit minUnit = TimeUnit.Millisecond, string collectionSeparator = ", ")
        {
            TimeSpanData timeSpanData = new TimeSpanData(timeSpan, precision, culture, maxUnit, minUnit, countEmptyUnits: countEmptyUnits, collectionSeparator: collectionSeparator);

            return Humanize(timeSpanData);
        }
        
        private static string Humanize(TimeSpanData tsData)
        {
            var timeParts = CreateTheTimePartsWithUpperAndLowerLimits(tsData);
            timeParts = SetPrecisionOfTimeSpan(timeParts, tsData.Precision, tsData.CountEmptyUnits);

            return ConcatenateTimeSpanParts(timeParts, tsData.CollectionSeparator);
        }

        private static IEnumerable<string> CreateTheTimePartsWithUpperAndLowerLimits(TimeSpanData timeSpanData)
        {
            IFormatter cultureFormatter = Configurator.GetFormatter(timeSpanData.CultureInfo);
            bool firstValueFound = false, higherUnitsInterpreted = false;
            IEnumerable<TimeUnit> timeUnitsEnumTypes = GetEnumTypesForTimeUnit();
            List<string> timeParts = new List<string>();

            foreach (var timeUnitType in timeUnitsEnumTypes)
            {
                if (timeUnitType <= timeSpanData.MaximumTimeUnit && timeUnitType >= timeSpanData.MinimumTimeUnit)
                {
                    if (timeUnitType >= TimeUnit.Day && !higherUnitsInterpreted)
                    {
                        List<string> higherUnitsOfTime = GetDayDependentTimeUnits(timeSpanData, cultureFormatter);

                        if (higherUnitsOfTime.Count >= 1)
                        {
                            timeParts.AddRange(higherUnitsOfTime);
                            higherUnitsInterpreted = true;
                        }
                    }
                    else
                    {
                        string timePart = GetTimeUnitPart(timeUnitType, timeSpanData, cultureFormatter);

                        if (timePart != null || firstValueFound)
                        {
                            firstValueFound = true;
                            timeParts.Add(timePart);
                        }
                    }
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

            return enumTypeEnumerator.Reverse();
        }

        private static List<string> GetDayDependentTimeUnits(TimeSpanData timeSpanData, IFormatter cultureFormatter)
        {
            List<string> timeUnitsFound = new List<string>();
            int currentYear = timeSpanData.TimeReference.Year, currentMonth = timeSpanData.TimeReference.Month, daysRemaining = Math.Abs(timeSpanData.TimeSpan.Days);
            bool yearsCalculated = false, monthsCalculated = false, weeksCalculated = false, daysCalculated = false;            
            
            if (timeSpanData.MinimumTimeUnit == TimeUnit.Year)
            {
                yearsCalculated = InterpretYears(timeSpanData, cultureFormatter, timeUnitsFound, ref currentYear, ref daysRemaining);
            }

            if (timeSpanData.MaximumTimeUnit == TimeUnit.Month)
            {
                yearsCalculated = InterpretYears(timeSpanData, cultureFormatter, timeUnitsFound, ref currentYear, ref daysRemaining);
                monthsCalculated = InterpretMonths(timeSpanData, cultureFormatter, currentMonth, currentYear, timeUnitsFound, ref daysRemaining);
            }

            if (timeSpanData.MinimumTimeUnit == TimeUnit.Week)
            {
                yearsCalculated = InterpretYears(timeSpanData, cultureFormatter, timeUnitsFound, ref currentYear, ref daysRemaining);
                monthsCalculated = InterpretMonths(timeSpanData, cultureFormatter, currentMonth, currentYear, timeUnitsFound, ref daysRemaining);
                weeksCalculated = InterpretWeeks(cultureFormatter, timeUnitsFound, ref daysRemaining);
            }

            if (timeSpanData.MinimumTimeUnit == TimeUnit.Day)
            {
                yearsCalculated = InterpretYears(timeSpanData, cultureFormatter, timeUnitsFound, ref currentYear, ref daysRemaining);
                monthsCalculated = InterpretMonths(timeSpanData, cultureFormatter, currentMonth, currentYear, timeUnitsFound, ref daysRemaining);
                weeksCalculated = InterpretWeeks(cultureFormatter, timeUnitsFound, ref daysRemaining);
                daysCalculated = InterpretDays(cultureFormatter, timeUnitsFound, daysRemaining);
            }

            if (timeSpanData.MaximumTimeUnit == TimeUnit.Day && !daysCalculated)
            {
                InterpretDays(cultureFormatter, timeUnitsFound, daysRemaining);
            }

            if (timeSpanData.MaximumTimeUnit == TimeUnit.Week && !weeksCalculated)
            {
                InterpretWeeks(cultureFormatter, timeUnitsFound, ref daysRemaining);
                InterpretDays(cultureFormatter, timeUnitsFound, daysRemaining);
            }

            if (timeSpanData.MinimumTimeUnit == TimeUnit.Month && !monthsCalculated)
            {
                InterpretMonths(timeSpanData, cultureFormatter, currentMonth, currentYear, timeUnitsFound, ref daysRemaining);
                InterpretWeeks(cultureFormatter, timeUnitsFound, ref daysRemaining);
                InterpretDays(cultureFormatter, timeUnitsFound, daysRemaining);
            }

            if (timeSpanData.MaximumTimeUnit == TimeUnit.Year && !yearsCalculated)
            {
                InterpretYears(timeSpanData, cultureFormatter, timeUnitsFound, ref currentYear, ref daysRemaining);
                InterpretMonths(timeSpanData, cultureFormatter, currentMonth, currentYear, timeUnitsFound, ref daysRemaining);
                InterpretWeeks(cultureFormatter, timeUnitsFound, ref daysRemaining);
                InterpretDays(cultureFormatter, timeUnitsFound, daysRemaining);
            }

            return timeUnitsFound.Where(x => x != null).ToList();
        }

        private static bool InterpretYears(TimeSpanData timeSpanData, IFormatter cultureFormatter, List<string> timeUnitsFound, ref int currentYear, ref int daysRemaining)
        {
            int numberOfYears = GetNumberOfYears(timeSpanData, ref daysRemaining, ref currentYear);
            timeUnitsFound.Add(BuildFormatTimePart(cultureFormatter, TimeUnit.Year, numberOfYears));
            return true;
        }

        private static bool InterpretWeeks(IFormatter cultureFormatter, List<string> timeUnitsFound, ref int daysRemaining)
        {
            int numberOfWeeks = GetNumberOfWeeks(ref daysRemaining);
            timeUnitsFound.Add(BuildFormatTimePart(cultureFormatter, TimeUnit.Week, numberOfWeeks));
            return true;
        }

        private static bool InterpretMonths(TimeSpanData timeSpanData, IFormatter cultureFormatter, int currentMonth, int currentYear, List<string> timeUnitsFound, ref int daysRemaining)
        {
            int numberOfMonths = GetNumberOfMonths(timeSpanData, currentMonth, currentYear, ref daysRemaining);
            timeUnitsFound.Add(BuildFormatTimePart(cultureFormatter, TimeUnit.Month, numberOfMonths));
            return true;
        }

        private static bool InterpretDays(IFormatter cultureFormatter, List<string> timeUnitsFound, int daysRemaining)
        {
            timeUnitsFound.Add(BuildFormatTimePart(cultureFormatter, TimeUnit.Day, daysRemaining));
            return true;
        }

        private static int GetNumberOfYears(TimeSpanData timeSpanData, ref int daysRemaining, ref int currentYear)
        {
            int daysInAYear = GetDaysInAYear(currentYear);
            int numberOfYears = 0;

            while (daysRemaining >= daysInAYear)
            {
                numberOfYears++;
                daysRemaining = daysRemaining - daysInAYear;

                if (timeSpanData.IsBeforeReference)
                    currentYear--;
                else
                    currentYear++;

                daysInAYear = GetDaysInAYear(currentYear);
            }

            return numberOfYears;
        }

        private static int GetDaysInAYear(int year)
        {
            return IsLeapYear(year) ? 366 : 365;
        }

        private static bool IsLeapYear(int year)
        {
            bool isLeapYear = false;

            if (year % 4 == 0)
            {
                if (year % 100 == 0)
                {
                    if (year % 400 == 0)
                        isLeapYear = true;
                }
                else
                    isLeapYear = true;
            }

            return isLeapYear;
        }

        private static int GetNumberOfMonths(TimeSpanData timeSpanData, int currentMonth, int currentYear, ref int daysRemaining)
        {
            int daysInMonth = GetDaysInMonth(currentMonth, currentYear);
            int numberOfMonths = 0;

            while (daysRemaining >= daysInMonth)
            {
                numberOfMonths++;
                daysRemaining = daysRemaining - daysInMonth;
                currentMonth = AdvanceCurrentMonth(timeSpanData.IsBeforeReference, currentMonth);
                daysInMonth = GetDaysInMonth(currentMonth, currentYear);
            }
            return numberOfMonths;
        }

        private static int GetDaysInMonth(int month, int year)
        {
            switch (month)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    return 31;
                case 4:
                case 6:
                case 9:
                case 11:
                    return 30;
                case 2:
                    return IsLeapYear(year) ? 29 : 28;
                default:
                    throw new ArgumentOutOfRangeException(string.Format("{0} is not a valid month number.", month));
            }
        }

        private static int AdvanceCurrentMonth(bool referenceTimeIsBefore, int currentMonth)
        {
            const int numberOfMonthsPerYear = 12;
            if (referenceTimeIsBefore)
                currentMonth++;
            else
                currentMonth--;

            if (currentMonth > numberOfMonthsPerYear)
                currentMonth = 1;
            else if (currentMonth <= 0)
                currentMonth = 12;

            return currentMonth;
        }

        private static int GetNumberOfWeeks(ref int daysRemaining)
        {
            int numberOfWeeks = 0;
            if (daysRemaining >= DaysInAWeek)
                numberOfWeeks = daysRemaining/DaysInAWeek;

            daysRemaining = daysRemaining - numberOfWeeks*DaysInAWeek;
            return numberOfWeeks;
        }

        private static string GetTimeUnitPart(TimeUnit timeUnitToGet, TimeSpanData timeSpanData, IFormatter cultureFormatter)
        {
            if(timeUnitToGet <= timeSpanData.MaximumTimeUnit && timeUnitToGet >= timeSpanData.MinimumTimeUnit)
            {
                bool isTimeUnitToGetTheMaximumTimeUnit = (timeUnitToGet == timeSpanData.MaximumTimeUnit);
                int numberOfTimeUnits = GetTimeUnitNumericalValue(timeUnitToGet, timeSpanData.TimeSpan, isTimeUnitToGetTheMaximumTimeUnit);
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
                default:
                    return 0;
            }
        }
        
        private static int GetNormalCaseTimeAsInteger(int timeNumberOfUnits, double totalTimeNumberOfUnits, bool isTimeUnitToGetTheMaximumTimeUnit)
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

        private static string BuildFormatTimePart(IFormatter cultureFormatter, TimeUnit timeUnitType, int amountOfTimeUnits)
        {
            // Always use positive units to account for negative timespans
            return amountOfTimeUnits != 0
                ? cultureFormatter.TimeSpanHumanize(timeUnitType, Math.Abs(amountOfTimeUnits))
                : null;
        }
        
        private static bool IsContainingOnlyNullValue(IEnumerable<string> timeParts)
        {
            return (timeParts.Count(x => x != null) == 0);
        }

        private static List<string> CreateTimePartsWithNoTimeValue(string noTimeValue)
        {
            return new List<string>() { noTimeValue };
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

        private static string ConcatenateTimeSpanParts(IEnumerable<string> timeSpanParts, string collectionSeparator)
        {
            if (collectionSeparator == null)
            {
                return Configurator.CollectionFormatter.Humanize(timeSpanParts);
            }

            return string.Join(collectionSeparator, timeSpanParts);
        }
    }
}
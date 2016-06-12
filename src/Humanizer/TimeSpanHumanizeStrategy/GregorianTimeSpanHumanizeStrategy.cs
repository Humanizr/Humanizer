using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Humanizer.Configuration;
using Humanizer.Localisation;
using Humanizer.Localisation.Formatters;

namespace Humanizer.TimeSpanHumanizeStrategy
{
    /// <summary>
    /// Converts a period of time into a more human friendly form based on the Gregorian calendar.
    /// </summary>
    public class GregorianTimeSpanHumanizeStrategy : ITimeSpanHumanizeStrategy
    {
        /// <summary>
        /// Total number of days in a week, according the the Gregorian calendar.
        /// </summary>
        private const int DaysInAWeek = 7;

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
            var higherUnitsInterpreted = false;
            var timeUnitsEnumTypes = GetEnumTypesForTimeUnit();
            var timeParts = new List<string>();

            foreach (var timeUnitType in timeUnitsEnumTypes)
            {
                if (timeUnitType <= timeSpanAnalysisParameters.MaximumTimeUnit && timeUnitType >= timeSpanAnalysisParameters.MinimumTimeUnit)
                {
                    if (timeUnitType >= TimeUnit.Day && !higherUnitsInterpreted)
                    {
                        var higherUnitsOfTime = GetDayDependentTimeUnits(timeSpanAnalysisParameters, cultureFormatter);

                        if (higherUnitsOfTime.Count >= 1)
                        {
                            timeParts.AddRange(higherUnitsOfTime);
                            higherUnitsInterpreted = true;
                        }
                    }
                    else
                    {
                        var timePart = GetTimeUnitPart(timeUnitType, timeSpanAnalysisParameters, cultureFormatter);

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

        private List<string> GetDayDependentTimeUnits(TimeSpanAnalysisParameters timeSpanAnalysisParameters, IFormatter cultureFormatter)
        {
            var timeUnitsFound = new List<string>();
            var currentYear = timeSpanAnalysisParameters.TimeReference.Year;
            var currentMonth = timeSpanAnalysisParameters.TimeReference.Month;
            var daysRemaining = Math.Abs(timeSpanAnalysisParameters.TimeSpan.Days);
            var yearsCalculated = false;
            var monthsCalculated = false;
            var weeksCalculated = false;
            var daysCalculated = false;

            switch (timeSpanAnalysisParameters.MinimumTimeUnit)
            {
                case TimeUnit.Year:
                    yearsCalculated = InterpretYears(timeSpanAnalysisParameters, cultureFormatter, timeUnitsFound, ref currentYear, ref daysRemaining);
                    break;
                case TimeUnit.Month:
                    yearsCalculated = InterpretYears(timeSpanAnalysisParameters, cultureFormatter, timeUnitsFound, ref currentYear, ref daysRemaining);
                    monthsCalculated = InterpretMonths(timeSpanAnalysisParameters, cultureFormatter, currentMonth, currentYear, timeUnitsFound, ref daysRemaining);
                    break;
                case TimeUnit.Week:
                    yearsCalculated = InterpretYears(timeSpanAnalysisParameters, cultureFormatter, timeUnitsFound, ref currentYear, ref daysRemaining);
                    monthsCalculated = InterpretMonths(timeSpanAnalysisParameters, cultureFormatter, currentMonth, currentYear, timeUnitsFound, ref daysRemaining);
                    weeksCalculated = InterpretWeeks(cultureFormatter, timeUnitsFound, ref daysRemaining);
                    break;
                case TimeUnit.Day:
                    yearsCalculated = InterpretYears(timeSpanAnalysisParameters, cultureFormatter, timeUnitsFound, ref currentYear, ref daysRemaining);
                    monthsCalculated = InterpretMonths(timeSpanAnalysisParameters, cultureFormatter, currentMonth, currentYear, timeUnitsFound, ref daysRemaining);
                    weeksCalculated = InterpretWeeks(cultureFormatter, timeUnitsFound, ref daysRemaining);
                    daysCalculated = InterpretDays(cultureFormatter, timeUnitsFound, daysRemaining);
                    break;
            }

            if (timeSpanAnalysisParameters.MaximumTimeUnit == TimeUnit.Day && !daysCalculated)
            {
                InterpretDays(cultureFormatter, timeUnitsFound, daysRemaining);
            }

            if (timeSpanAnalysisParameters.MaximumTimeUnit == TimeUnit.Week && !weeksCalculated)
            {
                InterpretWeeks(cultureFormatter, timeUnitsFound, ref daysRemaining);
                InterpretDays(cultureFormatter, timeUnitsFound, daysRemaining);
            }

            if (timeSpanAnalysisParameters.MaximumTimeUnit == TimeUnit.Month && !monthsCalculated)
            {
                InterpretMonths(timeSpanAnalysisParameters, cultureFormatter, currentMonth, currentYear, timeUnitsFound, ref daysRemaining);
                InterpretWeeks(cultureFormatter, timeUnitsFound, ref daysRemaining);
                InterpretDays(cultureFormatter, timeUnitsFound, daysRemaining);
            }

            if (timeSpanAnalysisParameters.MaximumTimeUnit == TimeUnit.Year && !yearsCalculated)
            {
                InterpretYears(timeSpanAnalysisParameters, cultureFormatter, timeUnitsFound, ref currentYear, ref daysRemaining);
                InterpretMonths(timeSpanAnalysisParameters, cultureFormatter, currentMonth, currentYear, timeUnitsFound, ref daysRemaining);
                InterpretWeeks(cultureFormatter, timeUnitsFound, ref daysRemaining);
                InterpretDays(cultureFormatter, timeUnitsFound, daysRemaining);
            }

            return timeUnitsFound.Where(x => x != null)
                                 .ToList();
        }

        private bool InterpretYears(TimeSpanAnalysisParameters timeSpanAnalysisParameters, IFormatter cultureFormatter, List<string> timeUnitsFound, ref int currentYear, ref int daysRemaining)
        {
            var numberOfYears = GetNumberOfYears(timeSpanAnalysisParameters, ref daysRemaining, ref currentYear);
            timeUnitsFound.Add(BuildFormatTimePart(cultureFormatter, TimeUnit.Year, numberOfYears));
            return true;
        }

        private bool InterpretWeeks(IFormatter cultureFormatter, List<string> timeUnitsFound, ref int daysRemaining)
        {
            var numberOfWeeks = GetNumberOfWeeks(ref daysRemaining);
            timeUnitsFound.Add(BuildFormatTimePart(cultureFormatter, TimeUnit.Week, numberOfWeeks));
            return true;
        }

        private bool InterpretMonths(TimeSpanAnalysisParameters timeSpanAnalysisParameters, IFormatter cultureFormatter, int currentMonth, int currentYear, List<string> timeUnitsFound, ref int daysRemaining)
        {
            var numberOfMonths = GetNumberOfMonths(timeSpanAnalysisParameters, currentMonth, currentYear, ref daysRemaining);
            timeUnitsFound.Add(BuildFormatTimePart(cultureFormatter, TimeUnit.Month, numberOfMonths));
            return true;
        }

        private bool InterpretDays(IFormatter cultureFormatter, List<string> timeUnitsFound, int daysRemaining)
        {
            timeUnitsFound.Add(BuildFormatTimePart(cultureFormatter, TimeUnit.Day, daysRemaining));
            return true;
        }

        private int GetNumberOfYears(TimeSpanAnalysisParameters timeSpanAnalysisParameters, ref int daysRemaining, ref int currentYear)
        {
            var daysInAYear = GetDaysInAYear(currentYear);
            var numberOfYears = 0;

            while (daysRemaining >= daysInAYear)
            {
                numberOfYears++;
                daysRemaining = daysRemaining - daysInAYear;

                if (timeSpanAnalysisParameters.IsBeforeReference)
                    currentYear--;
                else
                    currentYear++;

                daysInAYear = GetDaysInAYear(currentYear);
            }

            return numberOfYears;
        }

        private int GetDaysInAYear(int year)
        {
            return IsLeapYear(year) ? 366 : 365;
        }

        private bool IsLeapYear(int year)
        {
            var isLeapYear = false;

            if (year%4 == 0)
            {
                if (year%100 == 0)
                {
                    if (year%400 == 0)
                        isLeapYear = true;
                }
                else
                    isLeapYear = true;
            }

            return isLeapYear;
        }

        private int GetNumberOfMonths(TimeSpanAnalysisParameters timeSpanAnalysisParameters, int currentMonth, int currentYear, ref int daysRemaining)
        {
            var daysInMonth = GetDaysInMonth(currentMonth, currentYear);
            var numberOfMonths = 0;

            while (daysRemaining >= daysInMonth)
            {
                numberOfMonths++;
                daysRemaining = daysRemaining - daysInMonth;
                currentMonth = AdvanceCurrentMonth(timeSpanAnalysisParameters.IsBeforeReference, currentMonth);
                daysInMonth = GetDaysInMonth(currentMonth, currentYear);
            }
            return numberOfMonths;
        }

        private int GetDaysInMonth(int month, int year)
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

        private int AdvanceCurrentMonth(bool referenceTimeIsBefore, int currentMonth)
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

        private int GetNumberOfWeeks(ref int daysRemaining)
        {
            var numberOfWeeks = 0;
            if (daysRemaining >= DaysInAWeek)
                numberOfWeeks = daysRemaining/DaysInAWeek;

            daysRemaining = daysRemaining - numberOfWeeks*DaysInAWeek;
            return numberOfWeeks;
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

        private int GetTimeUnitNumericalValue(TimeUnit timeUnitToGet, TimeSpan timespan, bool isTimeUnitToGetTheMaximumTimeUnit)
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
            return new List<string>()
            {
                noTimeValue
            };
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
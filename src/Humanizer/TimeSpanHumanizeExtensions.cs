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
    private const int _monthsInAYear = 12;

    // 365 days in a year, except for every fourth year which has 366 (leap years)
    // thus one year is equivalent to a quarter of 1461 days (365 + 365 + 365 + 366), or 365.25
    private const double _daysInAYear = 365.25d;
    private static readonly int[] _daysInMonths = new int[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

    /// <summary>
    /// Turns a TimeSpan into a human readable form. E.g. 1 day.
    /// </summary>
    /// <param name="timeSpan"></param>
    /// <param name="precision">The maximum number of time units to return. Defaulted is 1 which means the largest unit is returned</param>
    /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
    /// <param name="maxUnit">The maximum unit of time to output.</param>
    /// <param name="minUnit">The minimum unit of time to output.</param>
    /// <param name="collectionSeparator">The separator to use when combining humanized time parts. If null, the default collection formatter for the current culture is used.</param>
    /// <returns></returns>
    public static string Humanize(this TimeSpan timeSpan, int precision = 1, CultureInfo culture = null, TimeUnit maxUnit = TimeUnit.Week, TimeUnit minUnit = TimeUnit.Millisecond, string collectionSeparator = ", ")
    {
      return Humanize(timeSpan, precision, false, culture, maxUnit, minUnit, collectionSeparator);
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
    /// <returns></returns>
    public static string Humanize(this TimeSpan timeSpan, int precision, bool countEmptyUnits, CultureInfo culture = null, TimeUnit maxUnit = TimeUnit.Week, TimeUnit minUnit = TimeUnit.Millisecond, string collectionSeparator = ", ")
    {
      var timeParts = CreateTheTimePartsWithUpperAndLowerLimits(timeSpan, culture, maxUnit, minUnit);
      timeParts = SetPrecisionOfTimeSpan(timeParts, precision, countEmptyUnits);

      return ConcatenateTimeSpanParts(timeParts, collectionSeparator);
    }

    private static IEnumerable<string> CreateTheTimePartsWithUpperAndLowerLimits(TimeSpan timespan, CultureInfo culture, TimeUnit maxUnit, TimeUnit minUnit)
    {
      var cultureFormatter = Configurator.GetFormatter(culture);
      var firstValueFound = false;
      var timeUnitsEnumTypes = GetEnumTypesForTimeUnit(maxUnit);
      var timeParts = new List<string>();

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
        var noTimeValueCultureFarmated = cultureFormatter.TimeSpanHumanize_Zero();
        timeParts = CreateTimePartsWithNoTimeValue(noTimeValueCultureFarmated);
      }
      return timeParts;
    }

    private static IEnumerable<TimeUnit> GetEnumTypesForTimeUnit(TimeUnit maxUnit = (TimeUnit)_lastTimeUnitTypeIndexImplemented)
    {
      var enumTypeEnumerator = (IEnumerable<TimeUnit>)Enum.GetValues(typeof(TimeUnit));
      enumTypeEnumerator = enumTypeEnumerator.Take(((int)maxUnit) + 1);

      return enumTypeEnumerator.Reverse();
    }

    private static string GetTimeUnitPart(TimeUnit timeUnitToGet, TimeSpan timespan, CultureInfo culture, TimeUnit maximumTimeUnit, TimeUnit minimumTimeUnit, IFormatter cultureFormatter)
    {
      if (timeUnitToGet <= maximumTimeUnit && timeUnitToGet >= minimumTimeUnit)
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
          return GetSpecialCaseMonthsAsInteger(timespan, isTimeUnitToGetTheMaximumTimeUnit);
        case TimeUnit.Year:
          return GetSpecialCaseYearsAsInteger(timespan, isTimeUnitToGetTheMaximumTimeUnit);
        default:
          return 0;
      }
    }

    private static int GetSpecialCaseYearsAsInteger(TimeSpan timespan, bool isTimeUnitToGetTheMaximumTimeUnit)
    {
      if (isTimeUnitToGetTheMaximumTimeUnit)
      {
        return (int)Math.Floor(timespan.TotalDays/_daysInAYear);
      }

      return (int)(timespan.TotalDays%_daysInAYear);
    }

    private static int GetSpecialCaseMonthsAsInteger(TimeSpan timespan, bool isTimeUnitToGetTheMaximumTimeUnit)
    {
      var months = (GetSpecialCaseYearsAsInteger(timespan, true)*_monthsInAYear);
      var daysRemaining = GetSpecialCaseYearsAsInteger(timespan, false);
      var monthCount = 0;
      while (true)
      {
        if (daysRemaining > _daysInMonths[monthCount])
        {
          daysRemaining -= _daysInMonths[monthCount];
          monthCount++;
        }
        else
        {
          if (isTimeUnitToGetTheMaximumTimeUnit)
            break;

          return daysRemaining;
        }
      }

      months += monthCount;

      return months;
    }

    private static int GetSpecialCaseWeeksAsInteger(TimeSpan timespan, bool isTimeUnitToGetTheMaximumTimeUnit)
    {
      if (isTimeUnitToGetTheMaximumTimeUnit)
      {
        return timespan.Days/_daysInAWeek;
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
      return timespan.Days%_daysInAWeek;
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

    private static List<string> CreateTimePartsWithNoTimeValue(string noTimeValue)
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
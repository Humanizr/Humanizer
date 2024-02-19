namespace Humanizer
{
    /// <summary>
    /// Humanizes TimeSpan into human readable form
    /// </summary>
    public static class TimeSpanHumanizeExtensions
    {
        const int _daysInAWeek = 7;
        const double _daysInAYear = 365.2425; // see https://en.wikipedia.org/wiki/Gregorian_calendar
        const double _daysInAMonth = _daysInAYear / 12;

        static TimeUnit[] _timeUnits = Enum.GetValues(typeof(TimeUnit))
            .Cast<TimeUnit>()
            .Reverse()
            .ToArray();

        /// <summary>
        /// Turns a TimeSpan into a human readable form. E.g. 1 day.
        /// </summary>
        /// <param name="precision">The maximum number of time units to return. Defaulted is 1 which means the largest unit is returned</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <param name="maxUnit">The maximum unit of time to output. The default value is <see cref="TimeUnit.Week"/>. The time units <see cref="TimeUnit.Month"/> and <see cref="TimeUnit.Year"/> will give approximations for time spans bigger 30 days by calculating with 365.2425 days a year and 30.4369 days a month.</param>
        /// <param name="minUnit">The minimum unit of time to output.</param>
        /// <param name="collectionSeparator">The separator to use when combining humanized time parts. If null, the default collection formatter for the current culture is used.</param>
        /// <param name="toWords">Uses words instead of numbers if true. E.g. one day.</param>
        public static string Humanize(this TimeSpan timeSpan, int precision = 1, CultureInfo culture = null, TimeUnit maxUnit = TimeUnit.Week, TimeUnit minUnit = TimeUnit.Millisecond, string collectionSeparator = ", ", bool toWords = false) =>
            Humanize(timeSpan, precision, false, culture, maxUnit, minUnit, collectionSeparator, toWords);

        /// <summary>
        /// Turns a TimeSpan into a human readable form. E.g. 1 day.
        /// </summary>
        /// <param name="precision">The maximum number of time units to return.</param>
        /// <param name="countEmptyUnits">Controls whether empty time units should be counted towards maximum number of time units. Leading empty time units never count.</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <param name="maxUnit">The maximum unit of time to output. The default value is <see cref="TimeUnit.Week"/>. The time units <see cref="TimeUnit.Month"/> and <see cref="TimeUnit.Year"/> will give approximations for time spans bigger than 30 days by calculating with 365.2425 days a year and 30.4369 days a month.</param>
        /// <param name="minUnit">The minimum unit of time to output.</param>
        /// <param name="collectionSeparator">The separator to use when combining humanized time parts. If null, the default collection formatter for the current culture is used.</param>
        /// <param name="toWords">Uses words instead of numbers if true. E.g. one day.</param>
        public static string Humanize(this TimeSpan timeSpan, int precision, bool countEmptyUnits, CultureInfo culture = null, TimeUnit maxUnit = TimeUnit.Week, TimeUnit minUnit = TimeUnit.Millisecond, string collectionSeparator = ", ", bool toWords = false)
        {
            var timeParts = CreateTheTimePartsWithUpperAndLowerLimits(timeSpan, culture, maxUnit, minUnit, toWords);
            timeParts = SetPrecisionOfTimeSpan(timeParts, precision, countEmptyUnits);

            return ConcatenateTimeSpanParts(timeParts, culture, collectionSeparator);
        }

        static IEnumerable<string> CreateTheTimePartsWithUpperAndLowerLimits(TimeSpan timespan, CultureInfo culture, TimeUnit maxUnit, TimeUnit minUnit, bool toWords = false)
        {
            var cultureFormatter = Configurator.GetFormatter(culture);
            var firstValueFound = false;
            var timeParts = new List<string>();

            foreach (var timeUnit in _timeUnits)
            {
                var timePart = GetTimeUnitPart(timeUnit, timespan, maxUnit, minUnit, cultureFormatter, toWords);

                if (timePart != null || firstValueFound)
                {
                    firstValueFound = true;
                    timeParts.Add(timePart);
                }
            }
            if (IsContainingOnlyNullValue(timeParts))
            {
                var noTimeValueCultureFormatted = toWords ? cultureFormatter.TimeSpanHumanize_Zero()
                    : cultureFormatter.TimeSpanHumanize(minUnit, 0);
                timeParts = CreateTimePartsWithNoTimeValue(noTimeValueCultureFormatted);
            }
            return timeParts;
        }

        static string GetTimeUnitPart(TimeUnit timeUnitToGet, TimeSpan timespan, TimeUnit maximumTimeUnit, TimeUnit minimumTimeUnit, IFormatter cultureFormatter, bool toWords = false)
        {
            if (timeUnitToGet <= maximumTimeUnit && timeUnitToGet >= minimumTimeUnit)
            {
                var numberOfTimeUnits = GetTimeUnitNumericalValue(timeUnitToGet, timespan, maximumTimeUnit);
                return BuildFormatTimePart(cultureFormatter, timeUnitToGet, numberOfTimeUnits, toWords);
            }
            return null;
        }

        static int GetTimeUnitNumericalValue(TimeUnit timeUnitToGet, TimeSpan timespan, TimeUnit maximumTimeUnit)
        {
            var isTimeUnitToGetTheMaximumTimeUnit = timeUnitToGet == maximumTimeUnit;
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
                    return GetSpecialCaseDaysAsInteger(timespan, maximumTimeUnit);
                case TimeUnit.Week:
                    return GetSpecialCaseWeeksAsInteger(timespan, isTimeUnitToGetTheMaximumTimeUnit);
                case TimeUnit.Month:
                    return GetSpecialCaseMonthAsInteger(timespan, isTimeUnitToGetTheMaximumTimeUnit);
                case TimeUnit.Year:
                    return GetSpecialCaseYearAsInteger(timespan);
                default:
                    return 0;
            }
        }

        static int GetSpecialCaseMonthAsInteger(TimeSpan timespan, bool isTimeUnitToGetTheMaximumTimeUnit)
        {
            if (isTimeUnitToGetTheMaximumTimeUnit)
            {
                return (int)(timespan.Days / _daysInAMonth);
            }

            var remainingDays = timespan.Days % _daysInAYear;
            return (int)(remainingDays / _daysInAMonth);
        }

        static int GetSpecialCaseYearAsInteger(TimeSpan timespan) =>
            (int)(timespan.Days / _daysInAYear);

        static int GetSpecialCaseWeeksAsInteger(TimeSpan timespan, bool isTimeUnitToGetTheMaximumTimeUnit)
        {
            if (isTimeUnitToGetTheMaximumTimeUnit || timespan.Days < _daysInAMonth)
            {
                return timespan.Days / _daysInAWeek;
            }
            return 0;
        }

        static int GetSpecialCaseDaysAsInteger(TimeSpan timespan, TimeUnit maximumTimeUnit)
        {
            if (maximumTimeUnit == TimeUnit.Day)
            {
                return timespan.Days;
            }
            if (timespan.Days < _daysInAMonth || maximumTimeUnit == TimeUnit.Week)
            {
                var remainingDays = timespan.Days % _daysInAWeek;
                return remainingDays;
            }
            return (int)(timespan.Days % _daysInAMonth);
        }

        static int GetNormalCaseTimeAsInteger(int timeNumberOfUnits, double totalTimeNumberOfUnits, bool isTimeUnitToGetTheMaximumTimeUnit)
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

        static string BuildFormatTimePart(IFormatter cultureFormatter, TimeUnit timeUnitType, int amountOfTimeUnits, bool toWords = false) =>
            // Always use positive units to account for negative timespans
            amountOfTimeUnits != 0
                ? cultureFormatter.TimeSpanHumanize(timeUnitType, Math.Abs(amountOfTimeUnits), toWords)
                : null;

        static List<string> CreateTimePartsWithNoTimeValue(string noTimeValue) =>
            [noTimeValue];

        static bool IsContainingOnlyNullValue(IEnumerable<string> timeParts) =>
            timeParts.Count(x => x != null) == 0;

        static IEnumerable<string> SetPrecisionOfTimeSpan(IEnumerable<string> timeParts, int precision, bool countEmptyUnits)
        {
            if (!countEmptyUnits)
            {
                timeParts = timeParts.Where(x => x != null);
            }

            timeParts = timeParts.Take(precision);
            if (countEmptyUnits)
            {
                timeParts = timeParts.Where(x => x != null);
            }

            return timeParts;
        }

        static string ConcatenateTimeSpanParts(IEnumerable<string> timeSpanParts, CultureInfo culture, string collectionSeparator)
        {
            if (collectionSeparator == null)
            {
                return Configurator.CollectionFormatters.ResolveForCulture(culture).Humanize(timeSpanParts);
            }

            return string.Join(collectionSeparator, timeSpanParts);
        }
    }
}

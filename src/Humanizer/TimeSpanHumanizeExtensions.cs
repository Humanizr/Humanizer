namespace Humanizer;

/// <summary>
/// Humanizes TimeSpan into human readable form
/// </summary>
public static class TimeSpanHumanizeExtensions
{
    const int DaysInAWeek = 7;
    const double DaysInAYear = 365.2425; // see https://en.wikipedia.org/wiki/Gregorian_calendar
    const double DaysInAMonth = DaysInAYear / 12;

    static readonly TimeUnit[] TimeUnits = [.. Enumerable.Reverse(Enum.GetValues<TimeUnit>())];

    /// <summary>
    /// Turns a TimeSpan into a human readable form. E.g. 1 day.
    /// </summary>
    /// <param name="precision">The maximum number of time units to return. Defaulted is 1 which means the largest unit is returned</param>
    /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
    /// <param name="maxUnit">The maximum unit of time to output. The default value is <see cref="TimeUnit.Week"/>. The time units <see cref="TimeUnit.Month"/> and <see cref="TimeUnit.Year"/> will give approximations for time spans bigger 30 days by calculating with 365.2425 days a year and 30.4369 days a month.</param>
    /// <param name="minUnit">The minimum unit of time to output.</param>
    /// <param name="collectionSeparator">The separator to use when combining humanized time parts. If null, the default collection formatter for the current culture is used.</param>
    /// <param name="toWords">Uses words instead of numbers if true. E.g. one day.</param>
    public static string Humanize(this TimeSpan timeSpan, int precision = 1, CultureInfo? culture = null, TimeUnit maxUnit = TimeUnit.Week, TimeUnit minUnit = TimeUnit.Millisecond, string? collectionSeparator = ", ", bool toWords = false) =>
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
    public static string Humanize(this TimeSpan timeSpan, int precision, bool countEmptyUnits, CultureInfo? culture = null, TimeUnit maxUnit = TimeUnit.Week, TimeUnit minUnit = TimeUnit.Millisecond, string? collectionSeparator = ", ", bool toWords = false)
    {
        var timeParts = CreateTheTimePartsWithUpperAndLowerLimits(timeSpan, culture, maxUnit, minUnit, toWords);
        timeParts = SetPrecisionOfTimeSpan(timeParts, precision, countEmptyUnits);

        return ConcatenateTimeSpanParts(timeParts, culture, collectionSeparator);
    }

    /// <summary>
    /// Turns a TimeSpan into an age expression, e.g. "40 years old"
    /// </summary>
    /// <param name="timeSpan">Elapsed time</param>
    /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
    /// <param name="maxUnit">The maximum unit of time to output. The default value is <see cref="TimeUnit.Year"/>.</param>
    /// <param name="toWords">Uses words instead of numbers if true. E.g. "forty years old".</param>
    /// <returns>Age expression in the given culture/language</returns>
    public static string ToAge(this TimeSpan timeSpan, CultureInfo? culture = null, TimeUnit maxUnit = TimeUnit.Year, bool toWords = false)
    {
        var timeSpanExpression = timeSpan.Humanize(culture: culture, maxUnit: maxUnit, toWords: toWords);

        var cultureFormatter = Configurator.GetFormatter(culture);
        var ageFormat = cultureFormatter.TimeSpanHumanize_Age();
        
        // Fast path: avoid string.Format for common "{0} old" pattern
        if (ageFormat == "{0} old")
        {
            return string.Concat(timeSpanExpression, " old");
        }
        
        return string.Format(ageFormat, timeSpanExpression);
    }

    static List<string?> CreateTheTimePartsWithUpperAndLowerLimits(TimeSpan timespan, CultureInfo? culture, TimeUnit maxUnit, TimeUnit minUnit, bool toWords = false)
    {
        var cultureFormatter = Configurator.GetFormatter(culture);
        var firstValueFound = false;
        var timeParts = new List<string?>();

        foreach (var timeUnit in TimeUnits)
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
            var noTimeValueCultureFormatted = toWords
                ? cultureFormatter.TimeSpanHumanize_Zero()
                : cultureFormatter.TimeSpanHumanize(minUnit, 0);
            timeParts = CreateTimePartsWithNoTimeValue(noTimeValueCultureFormatted);
        }

        return timeParts;
    }

    static string? GetTimeUnitPart(TimeUnit timeUnitToGet, TimeSpan timespan, TimeUnit maximumTimeUnit, TimeUnit minimumTimeUnit, IFormatter cultureFormatter, bool toWords = false)
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
        return timeUnitToGet switch
        {
            TimeUnit.Millisecond => GetNormalCaseTimeAsInteger(timespan.Milliseconds, timespan.TotalMilliseconds, isTimeUnitToGetTheMaximumTimeUnit),
            TimeUnit.Second => GetNormalCaseTimeAsInteger(timespan.Seconds, timespan.TotalSeconds, isTimeUnitToGetTheMaximumTimeUnit),
            TimeUnit.Minute => GetNormalCaseTimeAsInteger(timespan.Minutes, timespan.TotalMinutes, isTimeUnitToGetTheMaximumTimeUnit),
            TimeUnit.Hour => GetNormalCaseTimeAsInteger(timespan.Hours, timespan.TotalHours, isTimeUnitToGetTheMaximumTimeUnit),
            TimeUnit.Day => GetSpecialCaseDaysAsInteger(timespan, maximumTimeUnit),
            TimeUnit.Week => GetSpecialCaseWeeksAsInteger(timespan, isTimeUnitToGetTheMaximumTimeUnit),
            TimeUnit.Month => GetSpecialCaseMonthAsInteger(timespan, isTimeUnitToGetTheMaximumTimeUnit),
            TimeUnit.Year => GetSpecialCaseYearAsInteger(timespan),
            _ => 0
        };
    }

    static int GetSpecialCaseMonthAsInteger(TimeSpan timespan, bool isTimeUnitToGetTheMaximumTimeUnit)
    {
        if (isTimeUnitToGetTheMaximumTimeUnit)
        {
            return (int)(timespan.Days / DaysInAMonth);
        }

        var remainingDays = timespan.Days % DaysInAYear;
        return (int)(remainingDays / DaysInAMonth);
    }

    static int GetSpecialCaseYearAsInteger(TimeSpan timespan) =>
        (int)(timespan.Days / DaysInAYear);

    static int GetSpecialCaseWeeksAsInteger(TimeSpan timespan, bool isTimeUnitToGetTheMaximumTimeUnit)
    {
        if (isTimeUnitToGetTheMaximumTimeUnit || timespan.Days < DaysInAMonth)
        {
            return timespan.Days / DaysInAWeek;
        }

        return 0;
    }

    static int GetSpecialCaseDaysAsInteger(TimeSpan timespan, TimeUnit maximumTimeUnit)
    {
        if (maximumTimeUnit == TimeUnit.Day)
        {
            return timespan.Days;
        }

        if (timespan.Days < DaysInAMonth || maximumTimeUnit == TimeUnit.Week)
        {
            var remainingDays = timespan.Days % DaysInAWeek;
            return remainingDays;
        }

        return (int)(timespan.Days % DaysInAMonth);
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

    static string? BuildFormatTimePart(IFormatter cultureFormatter, TimeUnit timeUnitType, int amountOfTimeUnits, bool toWords = false) =>
        // Always use positive units to account for negative timespans
        amountOfTimeUnits != 0
            ? cultureFormatter.TimeSpanHumanize(timeUnitType, Math.Abs(amountOfTimeUnits), toWords)
            : null;

    static List<string?> CreateTimePartsWithNoTimeValue(string noTimeValue) =>
        [noTimeValue];

    static bool IsContainingOnlyNullValue(IEnumerable<string?> timeParts) =>
        !timeParts.Any(x => x != null);

    static List<string?> SetPrecisionOfTimeSpan(IEnumerable<string?> timeParts, int precision, bool countEmptyUnits)
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

        return [.. timeParts];
    }

    static string ConcatenateTimeSpanParts(IEnumerable<string?> timeSpanParts, CultureInfo? culture, string? collectionSeparator)
    {
        if (collectionSeparator == null)
        {
            return Configurator
                .CollectionFormatters.ResolveForCulture(culture)
                .Humanize(timeSpanParts);
        }

        return string.Join(collectionSeparator, timeSpanParts);
    }
}

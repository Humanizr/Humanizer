namespace Humanizer;

/// <summary>
/// Describes how the day component should be rendered within an ordinal date pattern.
/// </summary>
enum OrdinalDateDayMode
{
    /// <summary>Renders the day as a culture-aware numeric value.</summary>
    Numeric,
    /// <summary>Renders the day as an ordinal word.</summary>
    Ordinal,
    /// <summary>Renders the day as a numeric value except for the first day of the month.</summary>
    OrdinalWhenDayIsOne,
    /// <summary>Renders the first day of the month using the masculine ordinal form.</summary>
    MasculineOrdinalWhenDayIsOne,
    /// <summary>Renders the day as a numeric value followed by a dot suffix.</summary>
    DotSuffix
}

/// <summary>
/// Formats a date by combining a day-rendering mode with a culture-specific pattern template.
/// </summary>
/// <remarks>
/// The template can contain <c>{day}</c>. The day token is replaced after the formatted date string
/// is produced so the rest of the culture-specific pattern stays intact.
/// </remarks>
sealed class OrdinalDatePattern(string template, OrdinalDateDayMode dayMode, OrdinalDateCalendarMode calendarMode = OrdinalDateCalendarMode.Gregorian)
{
    const string DayPlaceholder = "{day}";
    const string DayMarker = "<<DAY>>";

    // Include a real 'd' day specifier adjacent to the marker so culture formatting sees
    // a combined day+month pattern and emits genitive month names in Slavic locales.
    // The numeric day output is stripped during replacement.
    const string DayMarkerFormat = "d'<<DAY>>'";

    readonly string template = template;
    readonly OrdinalDateDayMode dayMode = dayMode;
    readonly OrdinalDateCalendarMode calendarMode = calendarMode;

    /// <summary>
    /// Formats the pattern for the specified date.
    /// </summary>
    /// <returns>The formatted date string.</returns>
    public string Format(DateTime date)
    {
        var culture = GetPatternCulture();
        var calendarDay = culture.DateTimeFormat.Calendar.GetDayOfMonth(date);
        return ReplaceDayMarker(date.ToString(GetFormatString(), culture), FormatDay(calendarDay), calendarDay);
    }

#if NET6_0_OR_GREATER
    /// <summary>
    /// Formats the pattern for the specified date.
    /// </summary>
    /// <returns>The formatted date string.</returns>
    public string Format(DateOnly date)
    {
        var culture = GetPatternCulture();
        var calendarDay = culture.DateTimeFormat.Calendar.GetDayOfMonth(date.ToDateTime(default));
        return ReplaceDayMarker(date.ToString(GetFormatString(), culture), FormatDay(calendarDay), calendarDay);
    }
#endif

    string GetFormatString() => template.Replace(DayPlaceholder, DayMarkerFormat);

    // The format string includes a real 'd' specifier that emits the numeric day before the marker.
    // Strip both the numeric day and the marker, then insert the rendered day.
    static string ReplaceDayMarker(string formattedDate, string renderedDay, int day)
    {
        var markerIndex = formattedDate.IndexOf(DayMarker, StringComparison.Ordinal);
        if (markerIndex < 0)
        {
            return formattedDate;
        }

        // The numeric day text appears immediately before the marker (e.g., "25<<DAY>>").
        var dayText = day.ToString(CultureInfo.CurrentCulture);
        var dayStart = markerIndex - dayText.Length;
#if NETSTANDARD2_0 || NET48
        if (dayStart >= 0 && formattedDate.Substring(dayStart, dayText.Length) == dayText)
        {
            return formattedDate.Substring(0, dayStart) + renderedDay + formattedDate.Substring(markerIndex + DayMarker.Length);
        }
#else
        if (dayStart >= 0 && formattedDate.AsSpan(dayStart, dayText.Length).SequenceEqual(dayText.AsSpan()))
        {
            return string.Concat(formattedDate.AsSpan(0, dayStart), renderedDay.AsSpan(), formattedDate.AsSpan(markerIndex + DayMarker.Length));
        }
#endif

        // Fallback: just replace the marker.
        return formattedDate.Replace(DayMarker, renderedDay);
    }

    CultureInfo GetPatternCulture()
    {
        var culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();

        // When the locale requests the native calendar (e.g., Thai Buddhist), return the
        // culture as-is so DateTime.ToString() produces the native year.
        if (calendarMode == OrdinalDateCalendarMode.Native)
        {
            return culture;
        }

        try
        {
            culture.DateTimeFormat.Calendar = new GregorianCalendar(GregorianCalendarTypes.Localized);
            return culture;
        }
        catch (ArgumentOutOfRangeException)
        {
        }

        foreach (var calendar in culture.OptionalCalendars)
        {
            if (calendar is not GregorianCalendar gregorianCalendar)
            {
                continue;
            }

            culture.DateTimeFormat.Calendar = gregorianCalendar;
            return culture;
        }

        return culture;
    }

    string FormatDay(int day) =>
        dayMode switch
        {
            // Keep the day rendering logic separate from the date pattern so locales can combine
            // the same template with several day forms without duplicating the surrounding format.
            OrdinalDateDayMode.Numeric => day.ToString(CultureInfo.CurrentCulture),
            OrdinalDateDayMode.Ordinal => day.Ordinalize(),
            OrdinalDateDayMode.OrdinalWhenDayIsOne => day > 1 ? day.ToString(CultureInfo.CurrentCulture) : day.Ordinalize(),
            OrdinalDateDayMode.MasculineOrdinalWhenDayIsOne => day > 1 ? day.ToString(CultureInfo.CurrentCulture) : day.Ordinalize(GrammaticalGender.Masculine),
            OrdinalDateDayMode.DotSuffix => day.ToString(CultureInfo.CurrentCulture) + ".",
            _ => throw new InvalidOperationException("Unsupported ordinal date day mode.")
        };
}
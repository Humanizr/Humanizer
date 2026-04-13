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
sealed class OrdinalDatePattern(
    string template,
    OrdinalDateDayMode dayMode,
    OrdinalDateCalendarMode calendarMode = OrdinalDateCalendarMode.Gregorian,
    string[]? months = null,
    string[]? monthsGenitive = null,
    string[]? hijriMonths = null)
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
    readonly string[]? months = months;
    readonly string[]? monthsGenitive = monthsGenitive;
    readonly string[]? hijriMonths = hijriMonths;

    /// <summary>
    /// Formats the pattern for the specified date.
    /// </summary>
    /// <returns>The formatted date string.</returns>
    public string Format(DateTime date)
    {
        var culture = GetPatternCulture();
        var calendar = culture.DateTimeFormat.Calendar;
        var calendarDay = calendar.GetDayOfMonth(date);
        var month = calendar.GetMonth(date);
        return StripDirectionalityControls(
            ReplaceDayMarker(date.ToString(GetFormatString(month, calendarDay, calendar), culture), FormatDay(calendarDay), calendarDay));
    }

#if NET6_0_OR_GREATER
    /// <summary>
    /// Formats the pattern for the specified date.
    /// </summary>
    /// <returns>The formatted date string.</returns>
    public string Format(DateOnly date)
    {
        var culture = GetPatternCulture();
        var calendar = culture.DateTimeFormat.Calendar;
        var dateTime = date.ToDateTime(default);
        var calendarDay = calendar.GetDayOfMonth(dateTime);
        var month = calendar.GetMonth(dateTime);
        return StripDirectionalityControls(
            ReplaceDayMarker(date.ToString(GetFormatString(month, calendarDay, calendar), culture), FormatDay(calendarDay), calendarDay));
    }
#endif

    string GetFormatString(int month, int day, Calendar calendar)
    {
        var formatString = template.Replace(DayPlaceholder, DayMarkerFormat);
        var activeMonths = ResolveMonthArray(calendar);
        if (activeMonths is null)
        {
            return formatString;
        }

        return SubstituteMonth(formatString, month, day, activeMonths);
    }

    string[]? ResolveMonthArray(Calendar calendar)
    {
        if (hijriMonths is not null && calendar is HijriCalendar)
        {
            return hijriMonths;
        }

        return months;
    }

    /// <summary>
    /// Replaces the first unescaped MMMM specifier in the format string with a single-quoted
    /// literal month name from the override array. Uses genitive forms when the day is adjacent
    /// to the month specifier (d MMMM or MMMM d patterns).
    /// </summary>
    string SubstituteMonth(string formatString, int month, int day, string[] activeMonths)
    {
        // Find the first unescaped MMMM.
        var inQuote = false;
        var mmmmStart = -1;
        var mmmmLength = 0;
        for (var i = 0; i < formatString.Length; i++)
        {
            if (formatString[i] == '\'')
            {
                inQuote = !inQuote;
                continue;
            }

            if (inQuote)
            {
                continue;
            }

            if (formatString[i] == 'M')
            {
                var start = i;
                while (i < formatString.Length && formatString[i] == 'M')
                {
                    i++;
                }

                if (i - start >= 4)
                {
                    mmmmStart = start;
                    mmmmLength = i - start;
                    break; // Use the first unescaped MMMM.
                }

                i--; // Loop will increment.
            }
        }

        if (mmmmStart < 0)
        {
            return formatString;
        }

        // Determine genitive context: day adjacent to month (e.g., "d'<<DAY>>' MMMM" or "MMMM d").
        // Genitive forms only apply to the Gregorian months array, not Hijri.
        var useGenitive = monthsGenitive is not null && activeMonths == months && IsDayAdjacentToMonth(formatString, mmmmStart, mmmmLength);
        var monthNames = useGenitive ? monthsGenitive! : activeMonths;
        var monthName = monthNames[month - 1];

        // Escape embedded apostrophes per DateTimeFormatInfo literal quoting rules.
        var escapedMonth = monthName.Replace("'", "''");
        var literal = "'" + escapedMonth + "'";

#if NETSTANDARD2_0 || NET48
        return formatString.Substring(0, mmmmStart) + literal + formatString.Substring(mmmmStart + mmmmLength);
#else
        return string.Concat(formatString.AsSpan(0, mmmmStart), literal.AsSpan(), formatString.AsSpan(mmmmStart + mmmmLength));
#endif
    }

    /// <summary>
    /// Returns true when a day-of-month specifier (d or dd, NOT ddd/dddd day-of-week)
    /// is adjacent to the MMMM specifier, indicating genitive month form should be used.
    /// </summary>
    static bool IsDayAdjacentToMonth(string formatString, int mmmmStart, int mmmmLength)
    {
        // Check before MMMM: look for 'd'/'dd' (day-of-month) before the specifier,
        // skipping spaces and quoted literals.
        if (FindAdjacentDayOfMonth(formatString, mmmmStart - 1, backward: true))
        {
            return true;
        }

        // Check after MMMM.
        return FindAdjacentDayOfMonth(formatString, mmmmStart + mmmmLength, backward: false);
    }

    /// <summary>
    /// Scans from a starting position looking for a day-of-month specifier (d or dd).
    /// Returns false for ddd/dddd (day-of-week). Skips spaces and quoted literals.
    /// </summary>
    static bool FindAdjacentDayOfMonth(string formatString, int start, bool backward)
    {
        var i = start;
        while (backward ? i >= 0 : i < formatString.Length)
        {
            var c = formatString[i];

            // Skip whitespace and common date-pattern punctuation (e.g., "25. MMMM", "25, MMMM").
            if (c is ' ' or '.' or ',' or '-' or '/')
            {
                i += backward ? -1 : 1;
                continue;
            }

            if (c == '\'')
            {
                // Skip through quoted literal.
                i += backward ? -1 : 1;
                while (backward ? i >= 0 : i < formatString.Length)
                {
                    if (formatString[i] == '\'')
                    {
                        i += backward ? -1 : 1;
                        break;
                    }

                    i += backward ? -1 : 1;
                }

                continue;
            }

            if (c == 'd')
            {
                // Count consecutive 'd' characters to distinguish d/dd (day-of-month)
                // from ddd/dddd (day-of-week).
                var dCount = 1;
                var probe = i + (backward ? -1 : 1);
                while ((backward ? probe >= 0 : probe < formatString.Length) && formatString[probe] == 'd')
                {
                    dCount++;
                    probe += backward ? -1 : 1;
                }

                // d or dd = day-of-month (genitive context), ddd+ = day-of-week (not genitive).
                return dCount <= 2;
            }

            break;
        }

        return false;
    }

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

    static string StripDirectionalityControls(string value)
    {
        if (value.IndexOfAny(['\u200E', '\u200F', '\u061C']) < 0)
        {
            return value;
        }

#if NETSTANDARD2_0 || NET48
        return value.Replace("\u200E", "").Replace("\u200F", "").Replace("\u061C", "");
#else
        return value.Replace("\u200E", "", StringComparison.Ordinal)
                    .Replace("\u200F", "", StringComparison.Ordinal)
                    .Replace("\u061C", "", StringComparison.Ordinal);
#endif
    }
}
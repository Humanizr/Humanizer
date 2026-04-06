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
sealed class OrdinalDatePattern(string template, OrdinalDateDayMode dayMode)
{
    const string DayPlaceholder = "{day}";
    const string DayMarker = "<<DAY>>";
    const string DayMarkerFormat = "'<<DAY>>'";

    readonly string template = template;
    readonly OrdinalDateDayMode dayMode = dayMode;

    /// <summary>
    /// Formats the pattern for the specified date.
    /// </summary>
    /// <returns>The formatted date string.</returns>
    public string Format(DateTime date) =>
        ReplaceDayMarker(date.ToString(GetFormatString(), GetPatternCulture()), FormatDay(date.Day));

#if NET6_0_OR_GREATER
    /// <summary>
    /// Formats the pattern for the specified date.
    /// </summary>
    /// <returns>The formatted date string.</returns>
    public string Format(DateOnly date) =>
        ReplaceDayMarker(date.ToString(GetFormatString(), GetPatternCulture()), FormatDay(date.Day));
#endif

    string GetFormatString() => template.Replace(DayPlaceholder, DayMarkerFormat);

    // The day is emitted through a temporary marker so the rest of the culture-specific format
    // string can be preserved exactly as defined by the pattern.
    static string ReplaceDayMarker(string formattedDate, string renderedDay) =>
        formattedDate.Replace(DayMarker, renderedDay);

    static CultureInfo GetPatternCulture()
    {
        var culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();

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
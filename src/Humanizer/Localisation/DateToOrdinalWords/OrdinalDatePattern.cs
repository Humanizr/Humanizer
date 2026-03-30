namespace Humanizer;

enum OrdinalDateDayMode
{
    Numeric,
    Ordinal,
    OrdinalWhenDayIsOne,
    MasculineOrdinalWhenDayIsOne,
    DotSuffix
}

sealed class OrdinalDatePattern(string template, OrdinalDateDayMode dayMode)
{
    const string DayPlaceholder = "{day}";
    const string DayMarkerFormat = "'<<'d'>>'";

    readonly string template = template;
    readonly OrdinalDateDayMode dayMode = dayMode;

    public string Format(DateTime date) =>
        ReplaceDayMarker(date.ToString(GetFormatString()), FormatDay(date.Day), date.Day);

#if NET6_0_OR_GREATER
    public string Format(DateOnly date) =>
        ReplaceDayMarker(date.ToString(GetFormatString()), FormatDay(date.Day), date.Day);
#endif

    string GetFormatString() => template.Replace(DayPlaceholder, DayMarkerFormat);

    static string ReplaceDayMarker(string formattedDate, string renderedDay, int day) =>
        formattedDate.Replace("<<" + day.ToString(CultureInfo.CurrentCulture) + ">>", renderedDay);

    string FormatDay(int day) =>
        dayMode switch
        {
            OrdinalDateDayMode.Numeric => day.ToString(CultureInfo.CurrentCulture),
            OrdinalDateDayMode.Ordinal => day.Ordinalize(),
            OrdinalDateDayMode.OrdinalWhenDayIsOne => day > 1 ? day.ToString(CultureInfo.CurrentCulture) : day.Ordinalize(),
            OrdinalDateDayMode.MasculineOrdinalWhenDayIsOne => day > 1 ? day.ToString(CultureInfo.CurrentCulture) : day.Ordinalize(GrammaticalGender.Masculine),
            OrdinalDateDayMode.DotSuffix => day.ToString(CultureInfo.CurrentCulture) + ".",
            _ => throw new InvalidOperationException("Unsupported ordinal date day mode.")
        };
}

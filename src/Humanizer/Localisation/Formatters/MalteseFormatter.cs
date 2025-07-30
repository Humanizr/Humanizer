namespace Humanizer;

class MalteseFormatter(CultureInfo culture) :
    DefaultFormatter(culture)
{
    protected override string GetResourceKey(string resourceKey, int number)
    {
        if (number != 2)
        {
            return resourceKey;
        }

        return resourceKey switch
        {
            "DateHumanize_MultipleDaysAgo" => "DateHumanize_MultipleDaysAgo_Dual",
            "DateHumanize_MultipleDaysFromNow" => "DateHumanize_MultipleDaysFromNow_Dual",
            "DateHumanize_MultipleHoursAgo" => "DateHumanize_MultipleHoursAgo_Dual",
            "DateHumanize_MultipleHoursFromNow" => "DateHumanize_MultipleHoursFromNow_Dual",
            "DateHumanize_MultipleMonthsAgo" => "DateHumanize_MultipleMonthsAgo_Dual",
            "DateHumanize_MultipleMonthsFromNow" => "DateHumanize_MultipleMonthsFromNow_Dual",
            "DateHumanize_MultipleYearsAgo" => "DateHumanize_MultipleYearsAgo_Dual",
            "DateHumanize_MultipleYearsFromNow" => "DateHumanize_MultipleYearsFromNow_Dual",
            "TimeSpanHumanize_MultipleDays" => "TimeSpanHumanize_MultipleDays_Dual",
            "TimeSpanHumanize_MultipleYears" => "TimeSpanHumanize_MultipleYears_Dual",
            "TimeSpanHumanize_MultipleMonths" => "TimeSpanHumanize_MultipleMonths_Dual",
            "TimeSpanHumanize_MultipleHours" => "TimeSpanHumanize_MultipleHours_Dual",
            "TimeSpanHumanize_MultipleWeeks" => "TimeSpanHumanize_MultipleWeeks_Dual",
            _ => resourceKey,
        };
    }
}
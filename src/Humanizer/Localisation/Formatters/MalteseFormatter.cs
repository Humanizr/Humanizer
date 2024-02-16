namespace Humanizer
{
    internal class MalteseFormatter(string localeCode) :
        DefaultFormatter(localeCode)
    {
        private const string DualPostfix = "_Dual";

        private static readonly string[] DualResourceKeys =
        [
            "DateHumanize_MultipleDaysAgo", "DateHumanize_MultipleDaysFromNow", "DateHumanize_MultipleHoursAgo", "DateHumanize_MultipleHoursFromNow" ,
            "DateHumanize_MultipleMonthsAgo", "DateHumanize_MultipleMonthsFromNow", "DateHumanize_MultipleYearsAgo", "DateHumanize_MultipleYearsFromNow",
            "TimeSpanHumanize_MultipleDays", "TimeSpanHumanize_MultipleYears", "TimeSpanHumanize_MultipleMonths", "TimeSpanHumanize_MultipleHours",
            "TimeSpanHumanize_MultipleWeeks"
        ];

        protected override string GetResourceKey(string resourceKey, int number)
        {
            if (number == 2 && DualResourceKeys.Contains(resourceKey))
            {
                return resourceKey + DualPostfix;
            }

            return resourceKey;
        }
    }
}

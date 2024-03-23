namespace Humanizer;

public partial class ResourceKeys
{
    /// <summary>
    /// Encapsulates the logic required to get the resource keys for DateTime.Humanize
    /// </summary>
    public static class DateHumanize
    {
        /// <summary>
        /// Resource key for Now.
        /// </summary>
        public const string Now = "DateHumanize_Now";

        /// <summary>
        /// Resource key for Never.
        /// </summary>
        public const string Never = "DateHumanize_Never";

        /// <summary>
        /// Generates Resource Keys according to convention.
        /// </summary>
        /// <param name="timeUnit">Time unit</param>
        /// <param name="timeUnitTense">Is time unit in future or past</param>
        /// <param name="count">Number of units, default is One.</param>
        /// <returns>Resource key, like DateHumanize_SingleMinuteAgo</returns>
        public static string GetResourceKey(TimeUnit timeUnit, Tense timeUnitTense, int count = 1)
        {
            ValidateRange(count);

            if (count == 0)
            {
                return Now;
            }

            if (count == 1)
            {
                if (timeUnitTense == Tense.Future)
                {
                    return timeUnit switch
                    {
                        TimeUnit.Millisecond => "DateHumanize_SingleMillisecondFromNow",
                        TimeUnit.Second => "DateHumanize_SingleSecondFromNow",
                        TimeUnit.Minute => "DateHumanize_SingleMinuteFromNow",
                        TimeUnit.Hour => "DateHumanize_SingleHourFromNow",
                        TimeUnit.Day => "DateHumanize_SingleDayFromNow",
                        TimeUnit.Week => "DateHumanize_SingleWeekFromNow",
                        TimeUnit.Month => "DateHumanize_SingleMonthFromNow",
                        TimeUnit.Year => "DateHumanize_SingleYearFromNow",
                        _ => throw new ArgumentOutOfRangeException(nameof(timeUnit), timeUnit, null)
                    };
                }

                return timeUnit switch
                {
                    TimeUnit.Millisecond => "DateHumanize_SingleMillisecondAgo",
                    TimeUnit.Second => "DateHumanize_SingleSecondAgo",
                    TimeUnit.Minute => "DateHumanize_SingleMinuteAgo",
                    TimeUnit.Hour => "DateHumanize_SingleHourAgo",
                    TimeUnit.Day => "DateHumanize_SingleDayAgo",
                    TimeUnit.Week => "DateHumanize_SingleWeekAgo",
                    TimeUnit.Month => "DateHumanize_SingleMonthAgo",
                    TimeUnit.Year => "DateHumanize_SingleYearAgo",
                    _ => throw new ArgumentOutOfRangeException(nameof(timeUnit), timeUnit, null)
                };
            }

            if (timeUnitTense == Tense.Future)
            {
                return timeUnit switch
                {
                    TimeUnit.Millisecond => "DateHumanize_MultipleMillisecondsFromNow",
                    TimeUnit.Second => "DateHumanize_MultipleSecondsFromNow",
                    TimeUnit.Minute => "DateHumanize_MultipleMinutesFromNow",
                    TimeUnit.Hour => "DateHumanize_MultipleHoursFromNow",
                    TimeUnit.Day => "DateHumanize_MultipleDaysFromNow",
                    TimeUnit.Week => "DateHumanize_MultipleWeeksFromNow",
                    TimeUnit.Month => "DateHumanize_MultipleMonthsFromNow",
                    TimeUnit.Year => "DateHumanize_MultipleYearsFromNow",
                    _ => throw new ArgumentOutOfRangeException(nameof(timeUnit), timeUnit, null)
                };
            }

            return timeUnit switch
            {
                TimeUnit.Millisecond => "DateHumanize_MultipleMillisecondsAgo",
                TimeUnit.Second => "DateHumanize_MultipleSecondsAgo",
                TimeUnit.Minute => "DateHumanize_MultipleMinutesAgo",
                TimeUnit.Hour => "DateHumanize_MultipleHoursAgo",
                TimeUnit.Day => "DateHumanize_MultipleDaysAgo",
                TimeUnit.Week => "DateHumanize_MultipleWeeksAgo",
                TimeUnit.Month => "DateHumanize_MultipleMonthsAgo",
                TimeUnit.Year => "DateHumanize_MultipleYearsAgo",
                _ => throw new ArgumentOutOfRangeException(nameof(timeUnit), timeUnit, null)
            };
        }
    }
}
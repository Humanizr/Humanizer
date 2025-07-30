namespace Humanizer;

public partial class ResourceKeys
{
    /// <summary>
    /// Encapsulates the logic required to get the resource keys for TimeSpan.Humanize
    /// Examples: TimeSpanHumanize_SingleMinute, TimeSpanHumanize_MultipleHours.
    /// </summary>
    public static class TimeSpanHumanize
    {
        /// <summary>
        /// Generates Resource Keys according to convention.
        /// </summary>
        /// <param name="unit">Time unit, <see cref="TimeUnit"/>.</param>
        /// <param name="count">Number of units, default is One.</param>
        /// <param name="toWords">Result to words, default is false.</param>
        /// <returns>Resource key, like TimeSpanHumanize_SingleMinute</returns>
        public static string GetResourceKey(TimeUnit unit, int count = 1, bool toWords = false)
        {
            ValidateRange(count);

            if (count == 0 && toWords)
            {
                return "TimeSpanHumanize_Zero";
            }

            if (count == 1)
            {
                return unit switch
                {
                    TimeUnit.Millisecond => "TimeSpanHumanize_SingleMillisecond",
                    TimeUnit.Second => "TimeSpanHumanize_SingleSecond",
                    TimeUnit.Minute => "TimeSpanHumanize_SingleMinute",
                    TimeUnit.Hour => "TimeSpanHumanize_SingleHour",
                    TimeUnit.Day => "TimeSpanHumanize_SingleDay",
                    TimeUnit.Week => "TimeSpanHumanize_SingleWeek",
                    TimeUnit.Month => "TimeSpanHumanize_SingleMonth",
                    TimeUnit.Year => "TimeSpanHumanize_SingleYear",
                    _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
                };
            }

            return unit switch
            {
                TimeUnit.Millisecond => "TimeSpanHumanize_MultipleMilliseconds",
                TimeUnit.Second => "TimeSpanHumanize_MultipleSeconds",
                TimeUnit.Minute => "TimeSpanHumanize_MultipleMinutes",
                TimeUnit.Hour => "TimeSpanHumanize_MultipleHours",
                TimeUnit.Day => "TimeSpanHumanize_MultipleDays",
                TimeUnit.Week => "TimeSpanHumanize_MultipleWeeks",
                TimeUnit.Month => "TimeSpanHumanize_MultipleMonths",
                TimeUnit.Year => "TimeSpanHumanize_MultipleYears",
                _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
            };
        }
    }
}
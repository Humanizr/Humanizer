namespace Humanizer;

/// <inheritdoc cref="ResourceKeys"/>
public partial class ResourceKeys
{
    /// <summary>
    /// Builds resource keys for <see cref="TimeSpan"/> humanization.
    /// </summary>
    public static class TimeSpanHumanize
    {
        /// <summary>
        /// Generates the resource key for a <see cref="TimeSpan"/> phrase.
        /// </summary>
        /// <param name="unit">The unit being described.</param>
        /// <param name="count">The number of units. A value of <c>0</c> maps to the zero key when <paramref name="toWords"/> is <c>true</c>.</param>
        /// <param name="toWords">Whether the number should be rendered as words.</param>
        /// <returns>The resource key for the requested <see cref="TimeSpan"/> phrase.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="count"/> is negative or <paramref name="unit"/> is unsupported.
        /// </exception>
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

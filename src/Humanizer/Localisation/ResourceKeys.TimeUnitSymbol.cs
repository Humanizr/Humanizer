namespace Humanizer;

public partial class ResourceKeys
{
    /// <summary>
    /// Encapsulates the logic required to get the resource keys for TimeUnit.ToSymbol
    /// </summary>
    public static class TimeUnitSymbol
    {
        /// <summary>
        /// Generates Resource Keys according to convention.
        /// Examples: TimeUnit_Minute, TimeUnit_Hour.
        /// </summary>
        /// <param name="unit">Time unit, <see cref="TimeUnit"/>.</param>
        /// <returns>Resource key, like TimeSpanHumanize_SingleMinute</returns>
        public static string GetResourceKey(TimeUnit unit) =>
            unit switch
            {
                TimeUnit.Millisecond => "TimeUnit_Millisecond",
                TimeUnit.Second => "TimeUnit_Second",
                TimeUnit.Minute => "TimeUnit_Minute",
                TimeUnit.Hour => "TimeUnit_Hour",
                TimeUnit.Day => "TimeUnit_Day",
                TimeUnit.Week => "TimeUnit_Week",
                TimeUnit.Month => "TimeUnit_Month",
                TimeUnit.Year => "TimeUnit_Year",
                _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
            };
    }
}
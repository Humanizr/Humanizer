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
            $"TimeUnit_{unit}";
    }
}
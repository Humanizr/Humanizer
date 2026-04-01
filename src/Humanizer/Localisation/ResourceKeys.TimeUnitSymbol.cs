namespace Humanizer;

/// <inheritdoc cref="ResourceKeys"/>
public partial class ResourceKeys
{
    /// <summary>
    /// Builds resource keys for localized <see cref="TimeUnit"/> symbols.
    /// </summary>
    public static class TimeUnitSymbol
    {
        /// <summary>
        /// Generates the resource key for a localized <see cref="TimeUnit"/> symbol.
        /// </summary>
        /// <param name="unit">The unit whose symbol key should be returned.</param>
        /// <returns>The resource key for the requested <see cref="TimeUnit"/> symbol.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="unit"/> is unsupported.</exception>
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

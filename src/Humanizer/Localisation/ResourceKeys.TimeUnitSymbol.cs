namespace Humanizer.Localisation
{
    public partial class ResourceKeys
    {
        /// <summary>
        /// Encapsulates the logic required to get the resource keys for TimeUnit.ToSymbol
        /// </summary>
        public static class TimeUnitSymbol
        {
            /// <summary>
            /// Examples: TimeUnit_Minute, TimeUnit_Hour.
            /// </summary>
            private const string TimeUnitFormat = "TimeUnit_{0}";

            /// <summary>
            /// Generates Resource Keys according to convention.
            /// </summary>
            /// <param name="unit">Time unit, <see cref="TimeUnit"/>.</param>
            /// <returns>Resource key, like TimeSpanHumanize_SingleMinute</returns>
            public static string GetResourceKey(TimeUnit unit)
            {
                return TimeUnitFormat.FormatWith(unit);
            }
        }
    }
}

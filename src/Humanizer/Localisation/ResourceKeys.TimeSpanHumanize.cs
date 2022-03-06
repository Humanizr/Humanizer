namespace Humanizer.Localisation
{
    public partial class ResourceKeys
    {
        /// <summary>
        /// Encapsulates the logic required to get the resource keys for TimeSpan.Humanize
        /// </summary>
        public static class TimeSpanHumanize
        {
            /// <summary>
            /// Examples: TimeSpanHumanize_SingleMinute, TimeSpanHumanize_MultipleHours.
            /// Note: "s" for plural served separately by third part.
            /// </summary>
            private const string TimeSpanFormat = "TimeSpanHumanize_{0}{1}{2}{3}";
            private const string Zero = "TimeSpanHumanize_Zero";

            /// <summary>
            /// Generates Resource Keys according to convention.
            /// </summary>
            /// <param name="unit">Time unit, <see cref="TimeUnit"/>.</param>
            /// <param name="count">Number of units, default is One.</param>
            /// <param name="timeSpanStyle"></param>
            /// <returns>Resource key, like TimeSpanHumanize_SingleMinute</returns>
            public static string GetResourceKey(TimeUnit unit, int count = 1, TimeSpanStyle timeSpanStyle = TimeSpanStyle.Full)
            {
                ValidateRange(count);

                if (count == 0 && timeSpanStyle == TimeSpanStyle.Words)
                {
                    return Zero;
                }

                return TimeSpanFormat.FormatWith(
                    count == 1 ? Single : Multiple,
                    unit,
                    count == 1 ? "" : "s",
                    timeSpanStyle switch
                    {
                        TimeSpanStyle.Short => "_Short",
                        TimeSpanStyle.Abbreviated => "_Abbr",
                        TimeSpanStyle.Words => count == 1 ? "_Words" : "", // only Single has _Words resource
                        _ => ""
                    });
            }
        }
    }
}

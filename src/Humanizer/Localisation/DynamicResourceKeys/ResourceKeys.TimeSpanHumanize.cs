using System;

namespace Humanizer.Localisation.DynamicResourceKeys
{
    public partial class ResourceKeys
    {
        public static class TimeSpanHumanize
        {
            /// <summary>
            /// Examples: TimeSpanHumanize_SingleMinute, TimeSpanHumanize_MultipleHours.
            /// Note: "s" for plural served separately by third part.
            /// </summary>
            private const string TimeSpanFormat = "TimeSpanHumanize_{0}{1}{2}";
            private const string Zero = "TimeSpanHumanize_Zero";
            
            public static string GetResourceKey(TimeUnit unit, int count)
            {
                if (count == 0) return Zero;

                ValidateRange(count);
                return TimeSpanFormat.FormatWith(count == 1 ? Single : Multiple, unit, count == 1 ? "" : "s");
            }
        }
    }
}

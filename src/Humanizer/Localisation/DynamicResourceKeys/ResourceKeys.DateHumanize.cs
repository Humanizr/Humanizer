using System;

namespace Humanizer.Localisation.DynamicResourceKeys
{
    public partial class ResourceKeys
    {
        public static class DateHumanize
        {
            /// <summary>
            /// Examples: DateHumanize_SingleMinuteAgo, DateHumanize_MultipleHoursAgo
            /// Note: "s" for plural served separately by third part.
            /// </summary>
            private const string DateTimeFormat = "DateHumanize_{0}{1}{2}{3}";
            private const string Now = "DateHumanize_Now";
            private const string Ago = "Ago";
            private const string FromNow = "FromNow";

            public static string GetResourceKey(TimeUnit unit, int count, bool isFuture = false)
            {
                if (count == 0) return Now;

                ValidateRange(count);
                return DateTimeFormat.FormatWith(count == 1 ? Single : Multiple, unit, count == 1 ? "" : "s", isFuture ? FromNow : Ago);
            }
        }
    }
}

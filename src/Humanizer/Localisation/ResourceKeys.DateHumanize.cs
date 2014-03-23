namespace Humanizer.Localisation
{
    public partial class ResourceKeys
    {
        public static class DateHumanize
        {
            /// <summary>
            /// Resource key for Now.
            /// </summary>
            public const string Now = "DateHumanize_Now";

            /// <summary>
            /// Examples: DateHumanize_SingleMinuteAgo, DateHumanize_MultipleHoursAgo
            /// Note: "s" for plural served separately by third part.
            /// </summary>
            private const string DateTimeFormat = "DateHumanize_{0}{1}{2}{3}";

            private const string Ago = "Ago";
            private const string FromNow = "FromNow";

            /// <summary>
            /// Generates Resource Keys accordning to convention.
            /// </summary>
            /// <param name="unit">Time unit, <see cref="TimeUnit"/>.</param>
            /// <param name="count">Number of units, default is One.</param>
            /// <param name="isFuture">Boolean flag, is it in future? Default is false</param>
            /// <returns>Resource key, like DateHumanize_SingleMinuteAgo</returns>
            public static string GetResourceKey(TimeUnit unit, int count = 1, bool isFuture = false)
            {
                ValidateRange(count);

                if (count == 0) 
                    return Now;

                return DateTimeFormat.FormatWith(count == 1 ? Single : Multiple, unit, count == 1 ? "" : "s", isFuture ? FromNow : Ago);
            }
        }
    }
}

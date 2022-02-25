namespace Humanizer.Localisation
{
    public partial class ResourceKeys
    {
        /// <summary>
        /// Encapsulates the logic required to get the resource keys for DateTime.Humanize
        /// </summary>
        public static class DateHumanize
        {
            /// <summary>
            /// Resource key for Now.
            /// </summary>
            public const string Now = "DateHumanize_Now";

            /// <summary>
            /// Resource key for SingleDay.
            /// </summary>
            public const string SingleDay = "DateHumanize_SingleDay";

            /// <summary>
            /// Resource key for SingleDayAgo.
            /// </summary>
            public const string SingleDayAgo = "DateHumanize_SingleDayAgo";

            /// <summary>
            /// Resource key for SingleDayFromNow.
            /// </summary>
            public const string SingleDayFromNow = "DateHumanize_SingleDayFromNow";

            /// <summary>
            /// Resource key for Never.
            /// </summary>
            public const string Never = "DateHumanize_Never";

            /// <summary>
            /// Examples: DateHumanize_SingleMinute, DateHumanize_MultipleHours
            /// Note: "s" for plural served separately by third part.
            /// </summary>
            private const string DateTimeFormat = "DateHumanize_{0}{1}";

            /// <summary>
            /// Generates Resource Keys according to convention.
            /// </summary>
            /// <param name="timeUnit">Time unit</param>
            /// <param name="timeUnitTense">Is time unit in future or past</param>
            /// <param name="count">Number of units, default is One.</param>
            /// <returns>Resource key, like DateHumanize_SingleMinuteAgo</returns>
            public static string GetResourceKey(TimeUnit timeUnit, Tense timeUnitTense, int count = 1)
            {
                // TODO remove timeUnitTense parameter

                ValidateRange(count);

                if (count == 0)
                {
                    return Now;
                }

                var singularity = count == 1 ? Single : Multiple;
                var unit = timeUnit.ToString().ToQuantity(count, ShowQuantityAs.None);
                return DateTimeFormat.FormatWith(singularity, unit);
            }
        }
    }
}

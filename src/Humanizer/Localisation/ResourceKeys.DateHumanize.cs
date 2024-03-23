namespace Humanizer;

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
        /// Generates Resource Keys according to convention.
        /// </summary>
        /// <param name="timeUnit">Time unit</param>
        /// <param name="timeUnitTense">Is time unit in future or past</param>
        /// <param name="count">Number of units, default is One.</param>
        /// <returns>Resource key, like DateHumanize_SingleMinuteAgo</returns>
        public static string GetResourceKey(TimeUnit timeUnit, Tense timeUnitTense, int count = 1)
        {
            ValidateRange(count);

            if (count == 0)
            {
                return Now;
            }

            if (count == 1)
            {
                if (timeUnitTense == Tense.Future)
                {
                    return $"DateHumanize_Single{timeUnit}FromNow";
                }

                return $"DateHumanize_Single{timeUnit}Ago";
            }

            if (timeUnitTense == Tense.Future)
            {
                return $"DateHumanize_Multiple{timeUnit}sFromNow";
            }

            return $"DateHumanize_Multiple{timeUnit}sAgo";
        }
    }
}
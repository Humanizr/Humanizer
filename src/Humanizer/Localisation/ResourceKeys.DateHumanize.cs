using Humanizer.Localisation.Quantifier;
using System.Globalization;
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
            private const string DateTimeFormat = "DateHumanize_{0}{1}{2}";

            private const string Ago = "Ago";
            private const string FromNow = "FromNow";

            /// <summary>
            /// Generates Resource Keys accordning to convention.
            /// </summary>
            /// <param name="timeUnit">Time unit</param>
            /// <param name="timeUnitTense">Is time unit in future or past</param>
            /// <param name="count">Number of units, default is One.</param>
            /// <returns>Resource key, like DateHumanize_SingleMinuteAgo</returns>
            public static string GetResourceKey(TimeUnit timeUnit, TimeUnitTense timeUnitTense, int count = 1)
            {
                ValidateRange(count);

                if (count == 0) 
                    return Now;

                var singularity = count == 1 ? Single : Multiple;
                var tense = timeUnitTense == TimeUnitTense.Future ? FromNow : Ago;
                var unit = QuantifierFactory.GetQuantifier(CultureInfo.InvariantCulture).ToQuantity(timeUnit.ToString(), count, ShowQuantityAs.None);
                return DateTimeFormat.FormatWith(singularity, unit, tense);
            }
        }
    }
}

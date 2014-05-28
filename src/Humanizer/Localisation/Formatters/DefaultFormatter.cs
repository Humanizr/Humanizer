using System.Globalization;

namespace Humanizer.Localisation.Formatters
{
    /// <summary>
    /// Default implementation of IFormatter interface.
    /// </summary>
    public class DefaultFormatter : IFormatter
    {
        /// <summary>
        /// Now
        /// </summary>
        /// <param name="culture"></param>
        /// <returns>Returns Now</returns>
        public virtual string DateHumanize_Now(CultureInfo culture)
        {
            return GetResourceForDate(TimeUnit.Millisecond, Tense.Past, 0, culture);
        }

        /// <summary>
        /// Returns the string representation of the provided DateTime
        /// </summary>
        /// <param name="timeUnit"></param>
        /// <param name="timeUnitTense"></param>
        /// <param name="unit"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public virtual string DateHumanize(TimeUnit timeUnit, Tense timeUnitTense, int unit, CultureInfo culture)
        {
            return GetResourceForDate(timeUnit, timeUnitTense, unit, culture);
        }

        /// <summary>
        /// 0 seconds
        /// </summary>
        /// <returns>Returns 0 seconds as the string representation of Zero TimeSpan</returns>
        public virtual string TimeSpanHumanize_Zero()
        {
            return GetResourceForTimeSpan(TimeUnit.Millisecond, 0);
        }

        /// <summary>
        /// Returns the string representation of the provided TimeSpan
        /// </summary>
        /// <param name="timeUnit"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public virtual string TimeSpanHumanize(TimeUnit timeUnit, int unit)
        {
            return GetResourceForTimeSpan(timeUnit, unit);
        }

        private string GetResourceForDate(TimeUnit unit, Tense timeUnitTense, int count, CultureInfo culture)
        {
            string resourceKey = ResourceKeys.DateHumanize.GetResourceKey(unit, timeUnitTense: timeUnitTense, count: count);
            return count == 1 ? Format(resourceKey, culture) : Format(resourceKey, count, culture);
        }

        private string GetResourceForTimeSpan(TimeUnit unit, int count)
        {
            string resourceKey = ResourceKeys.TimeSpanHumanize.GetResourceKey(unit, count);
            return count == 1 ? Format(resourceKey, null) : Format(resourceKey, count, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceKey"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        protected virtual string Format(string resourceKey, CultureInfo culture)
        {
            return Resources.GetResource(GetResourceKey(resourceKey), culture);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceKey"></param>
        /// <param name="number"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        protected virtual string Format(string resourceKey, int number, CultureInfo culture)
        {
            return Resources.GetResource(GetResourceKey(resourceKey, number), culture).FormatWith(number);
        }

        /// <summary>
        /// Override this method if your locale has complex rules around multiple units; e.g. Arabic, Russian
        /// </summary>
        /// <param name="resourceKey">The resource key that's being in formatting</param>
        /// <param name="number">The number of the units being used in formatting</param>
        /// <returns></returns>
        protected virtual string GetResourceKey(string resourceKey, int number)
        {
            return resourceKey;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceKey"></param>
        /// <returns></returns>
        protected virtual string GetResourceKey(string resourceKey)
        {
            return resourceKey;
        }
    }
}
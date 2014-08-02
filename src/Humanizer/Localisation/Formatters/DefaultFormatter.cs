using System;
using System.Globalization;

namespace Humanizer.Localisation.Formatters
{
    /// <summary>
    /// Default implementation of IFormatter interface.
    /// </summary>
    public class DefaultFormatter : IFormatter
    {
        private readonly CultureInfo _culture;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="localeCode">Name of the culture to use.</param>
        public DefaultFormatter(string localeCode)
        {
            _culture = new CultureInfo(localeCode);
        }

        /// <summary>
        /// Now
        /// </summary>
        /// <returns>Returns Now</returns>
        public virtual string DateHumanize_Now()
        {
            return GetResourceForDate(TimeUnit.Millisecond, Tense.Past, 0);
        }

        /// <summary>
        /// Returns the string representation of the provided DateTime
        /// </summary>
        /// <param name="timeUnit"></param>
        /// <param name="timeUnitTense"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public virtual string DateHumanize(TimeUnit timeUnit, Tense timeUnitTense, int unit)
        {
            return GetResourceForDate(timeUnit, timeUnitTense, unit);
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
        /// <param name="timeUnit">Must be less than or equal to TimeUnit.Week</param>
        /// <param name="unit"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Is thrown when timeUnit is larger than TimeUnit.Week</exception>
        public virtual string TimeSpanHumanize(TimeUnit timeUnit, int unit)
        {
            if (timeUnit > TimeUnit.Week)
                throw new ArgumentOutOfRangeException("timeUnit", "There's no meaningful way to humanize passed timeUnit.");

            return GetResourceForTimeSpan(timeUnit, unit);
        }

        private string GetResourceForDate(TimeUnit unit, Tense timeUnitTense, int count)
        {
            string resourceKey = ResourceKeys.DateHumanize.GetResourceKey(unit, timeUnitTense: timeUnitTense, count: count);
            return count == 1 ? Format(resourceKey) : Format(resourceKey, count);
        }

        private string GetResourceForTimeSpan(TimeUnit unit, int count)
        {
            string resourceKey = ResourceKeys.TimeSpanHumanize.GetResourceKey(unit, count);
            return count == 1 ? Format(resourceKey) : Format(resourceKey, count);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceKey"></param>
        /// <returns></returns>
        protected virtual string Format(string resourceKey)
        {
            return Resources.GetResource(GetResourceKey(resourceKey), _culture);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceKey"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        protected virtual string Format(string resourceKey, int number)
        {
            return Resources.GetResource(GetResourceKey(resourceKey, number), _culture).FormatWith(number);
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
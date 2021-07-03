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
        /// Never
        /// </summary>
        /// <returns>Returns Never</returns>
        public virtual string DateHumanize_Never()
        {
            return Format(ResourceKeys.DateHumanize.Never);
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
            return GetResourceForTimeSpan(TimeUnit.Millisecond, 0, true);
        }

        /// <summary>
        /// Returns the string representation of the provided TimeSpan
        /// </summary>
        /// <param name="timeUnit">A time unit to represent.</param>
        /// <param name="unit"></param>
        /// <param name="toWords"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Is thrown when timeUnit is larger than TimeUnit.Week</exception>
        public virtual string TimeSpanHumanize(TimeUnit timeUnit, int unit, bool toWords = false)
        {
            return GetResourceForTimeSpan(timeUnit, unit, toWords);
        }

        /// <inheritdoc cref="IFormatter.DataUnitHumanize(DataUnit, double, bool)"/>
        public virtual string DataUnitHumanize(DataUnit dataUnit, double count, bool toSymbol = true)
        {
            var resourceKey = toSymbol ? $"DataUnit_{dataUnit}Symbol" : $"DataUnit_{dataUnit}";
            var resourceValue = Format(resourceKey);

            if (!toSymbol && count > 1)
                resourceValue += 's';

            return resourceValue;
        }

        /// <inheritdoc />
        public virtual string TimeUnitHumanize(TimeUnit timeUnit)
        {
            var resourceKey = ResourceKeys.TimeUnitSymbol.GetResourceKey(timeUnit);
            return Format(resourceKey);
        }

        private string GetResourceForDate(TimeUnit unit, Tense timeUnitTense, int count)
        {
            var resourceKey = ResourceKeys.DateHumanize.GetResourceKey(unit, timeUnitTense: timeUnitTense, count: count);
            return count == 1 ? Format(resourceKey) : Format(resourceKey, count);
        }

        private string GetResourceForTimeSpan(TimeUnit unit, int count, bool toWords = false)
        {
            var resourceKey = ResourceKeys.TimeSpanHumanize.GetResourceKey(unit, count, toWords);
            return count == 1 ? Format(resourceKey + (toWords ? "_Words" : "")) : Format(resourceKey, count, toWords);
        }

        /// <summary>
        /// Formats the specified resource key.
        /// </summary>
        /// <param name="resourceKey">The resource key.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">If the resource not exists on the specified culture.</exception>
        protected virtual string Format(string resourceKey)
        {
            var resourceString = Resources.GetResource(GetResourceKey(resourceKey), _culture);

            if (string.IsNullOrEmpty(resourceString))
            {
                throw new ArgumentException($"The resource object with key '{resourceKey}' was not found", nameof(resourceKey));
            }

            return resourceString;
        }

        /// <summary>
        /// Formats the specified resource key.
        /// </summary>
        /// <param name="resourceKey">The resource key.</param>
        /// <param name="number">The number.</param>
        /// <param name="toWords"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">If the resource not exists on the specified culture.</exception>
        protected virtual string Format(string resourceKey, int number, bool toWords = false)
        {
            var resourceString = Resources.GetResource(GetResourceKey(resourceKey, number), _culture);

            if (string.IsNullOrEmpty(resourceString))
            {
                throw new ArgumentException($"The resource object with key '{resourceKey}' was not found", nameof(resourceKey));
            }

            return toWords
                ? resourceString.FormatWith(number.ToWords(_culture))
                : resourceString.FormatWith(number);
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

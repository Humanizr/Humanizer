namespace Humanizer.Localisation.Formatters
{
    /// <summary>
    /// Default implementation of IFormatter interface.
    /// </summary>
    public class DefaultFormatter : IFormatter
    {
        /// <summary>
        /// Now!
        /// </summary>
        /// <returns>Time expressed in words</returns>
        public virtual string DateHumanize_Now()
        {
            return GetResourceForDate(TimeUnit.Millisecond, Tense.Past, 0);
        }

        public virtual string DateHumanize(TimeUnit timeUnit, Tense timeUnitTense, int unit)
        {
            return GetResourceForDate(timeUnit, timeUnitTense, unit);
        }

        /// <summary>
        /// In NO time!
        /// </summary>
        /// <returns>Time expressed in words</returns>
        public virtual string TimeSpanHumanize_Zero()
        {
            return GetResourceForTimeSpan(TimeUnit.Millisecond, 0);
        }

        public virtual string TimeSpanHumanize(TimeUnit timeUnit, int unit)
        {
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

        protected virtual string Format(string resourceKey)
        {
            return Resources.GetResource(GetResourceKey(resourceKey));
        }

        protected virtual string Format(string resourceKey, int number)
        {
            return Resources.GetResource(GetResourceKey(resourceKey, number)).FormatWith(number);
        }

        protected virtual string GetResourceKey(string resourceKey, int number)
        {
            return resourceKey;
        }

        protected virtual string GetResourceKey(string resourceKey)
        {
            return resourceKey;
        }
    }
}
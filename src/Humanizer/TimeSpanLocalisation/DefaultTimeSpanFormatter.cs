using Humanizer.Localisation;

namespace Humanizer.TimeSpanLocalisation
{
    /// <summary>
    /// A default time span formatter. Override this to provide custom logic for formatting time
    /// spans.
    /// </summary>
    public class DefaultTimeSpanFormatter : ITimeSpanFormatter
    {
        public string MultipleWeeks(int weeks)
        {
            return Format(ResourceKeys.MultipleWeeks, weeks);
        }

        public string SingleWeek()
        {
            return Format(ResourceKeys.SingleWeek);
        }

        public virtual string MultipleDays(int days)
        {
            return Format(ResourceKeys.MultipleDays, days);
        }

        public virtual string SingleDay()
        {
            return Format(ResourceKeys.SingleDay);
        }

        public virtual string MultipleHours(int hours)
        {
            return Format(ResourceKeys.MultipleHours, hours);
        }

        public virtual string SingleHour()
        {
            return Format(ResourceKeys.SingleHour);
        }

        public virtual string MultipleMinutes(int minutes)
        {
            return Format(ResourceKeys.MultipleMinutes, minutes);
        }

        public virtual string SingleMinute()
        {
            return Format(ResourceKeys.SingleMinute);
        }

        public virtual string MultipleSeconds(int seconds)
        {
            return Format(ResourceKeys.MultipleSeconds, seconds);
        }

        public virtual string SingleSecond()
        {
            return Format(ResourceKeys.SingleSecond);
        }

        public virtual string MultipleMilliseconds(int milliSeconds)
        {
            return Format(ResourceKeys.MultipleMilliseconds, milliSeconds);
        }

        public virtual string SingleMillisecond()
        {
            return Format(ResourceKeys.SingleMillisecond);
        }

        public string Zero()
        {
            return Format(ResourceKeys.Zero);
        }

        protected virtual string Format(string resourceKey, params object[] args)
        {
            var actualResourceKey = GetResourceKey(resourceKey, args);
            var resource = Resources.GetResource(actualResourceKey);
            return string.Format(resource, args);
        }

        protected virtual string GetResourceKey(string resourceKey, params object[] args)
        {
            return resourceKey;
        }
    }
}
namespace Humanizer.Localisation
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
            return GetResourceForDate(TimeUnit.Millisecond, 0);
        }

        /// <summary>
        /// To express time in secounds.
        /// </summary>
        /// <param name="seconds">number of seconds</param>
        /// <param name="timeUnitTense">Is time unit in future or past</param>
        /// <returns>Time expressed in words</returns>
        public virtual string DateHumanize_Seconds(int seconds = 1, TimeUnitTense timeUnitTense = TimeUnitTense.Past)
        {
            return GetResourceForDate(TimeUnit.Second, seconds, timeUnitTense);
        }

        /// <summary>
        /// To express time in minutes.
        /// </summary>
        /// <param name="minutes">number of minutes</param>
        /// <param name="timeUnitTense">Is time unit in future or past</param>
        /// <returns>Time expressed in words</returns>
        public virtual string DateHumanize_Minutes(int minutes = 1, TimeUnitTense timeUnitTense = TimeUnitTense.Past)
        {
            return GetResourceForDate(TimeUnit.Minute, minutes, timeUnitTense);
        }

        /// <summary>
        /// To express time in hours.
        /// </summary>
        /// <param name="hours">number of hours</param>
        /// <param name="timeUnitTense">Is time unit in future or past</param>
        /// <returns>Time expressed in words</returns>
        public virtual string DateHumanize_Hours(int hours = 1, TimeUnitTense timeUnitTense = TimeUnitTense.Past)
        {
            return GetResourceForDate(TimeUnit.Hour, hours, timeUnitTense);
        }

        /// <summary>
        /// To express time in days.
        /// </summary>
        /// <param name="days">number of days</param>
        /// <param name="timeUnitTense">Is time unit in future or past</param>
        /// <returns>Time expressed in words</returns>
        public virtual string DateHumanize_Days(int days = 1, TimeUnitTense timeUnitTense = TimeUnitTense.Past)
        {
            return GetResourceForDate(TimeUnit.Day, days, timeUnitTense);
        }

        /// <summary>
        /// To express time in months
        /// </summary>
        /// <param name="months">number of months</param>
        /// <param name="timeUnitTense">Is time unit in future or past</param>
        /// <returns>Time expressed in words</returns>
        public virtual string DateHumanize_Months(int months = 1, TimeUnitTense timeUnitTense = TimeUnitTense.Past)
        {
            return GetResourceForDate(TimeUnit.Month, months, timeUnitTense);
        }

        /// <summary>
        /// To express time in years
        /// </summary>
        /// <param name="years">number of years</param>
        /// <param name="timeUnitTense">Is time unit in future or past</param>
        /// <returns>Time expressed in words</returns>
        public virtual string DateHumanize_Years(int years = 1, TimeUnitTense timeUnitTense = TimeUnitTense.Past)
        {
            return GetResourceForDate(TimeUnit.Year, years, timeUnitTense);
        }

        /// <summary>
        /// In NO time!
        /// </summary>
        /// <returns>Time expressed in words</returns>
        public virtual string TimeSpanHumanize_Zero()
        {
            return GetResourceForTimeSpan(TimeUnit.Millisecond, 0);
        }

        /// <summary>
        /// To express time in milliseconds.
        /// </summary>
        /// <param name="milliSeconds">number of milliseconds</param>
        /// <returns>Time expressed in words</returns>
        public virtual string TimeSpanHumanize_Milliseconds(int milliSeconds = 1)
        {
            return GetResourceForTimeSpan(TimeUnit.Millisecond, milliSeconds);
        }

        /// <summary>
        /// To express time in secounds.
        /// </summary>
        /// <param name="seconds">number of seconds</param>
        /// <returns>Time expressed in words</returns>
        public virtual string TimeSpanHumanize_Seconds(int seconds = 1)
        {
            return GetResourceForTimeSpan(TimeUnit.Second, seconds);
        }

        /// <summary>
        /// To express time in minutes.
        /// </summary>
        /// <param name="minutes">number of minutes</param>
        /// <returns>Time expressed in words</returns>
        public virtual string TimeSpanHumanize_Minutes(int minutes = 1)
        {
            return GetResourceForTimeSpan(TimeUnit.Minute, minutes);
        }

        /// <summary>
        /// To express time in hours
        /// </summary>
        /// <param name="hours">number of hours</param>
        /// <returns>Time expressed in words</returns>
        public virtual string TimeSpanHumanize_Hours(int hours = 1)
        {
            return GetResourceForTimeSpan(TimeUnit.Hour, hours);
        }

        /// <summary>
        /// To express time in days.
        /// </summary>
        /// <param name="days">number of days</param>
        /// <returns>Time expressed in words</returns>
        public virtual string TimeSpanHumanize_Days(int days = 1)
        {
            return GetResourceForTimeSpan(TimeUnit.Day, days);
        }

        /// <summary>
        /// To express time in weeks.
        /// </summary>
        /// <param name="weeks">number of weeks</param>
        /// <returns>Time expressed in words</returns>
        public virtual string TimeSpanHumanize_Weeks(int weeks = 1)
        {
            return GetResourceForTimeSpan(TimeUnit.Week, weeks);
        }

        private string GetResourceForDate(TimeUnit unit, int count, TimeUnitTense timeUnitTense = TimeUnitTense.Past)
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